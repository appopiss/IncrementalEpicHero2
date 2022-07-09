using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;

//MenuUIという名前だが、MenuUIの派生クラスではない
public class EpicStoreMenuUI : MonoBehaviour
{
    [SerializeField] Button openButton, quitButton;
    [SerializeField] TextMeshProUGUI ecText;
    [SerializeField] GameObject[] items;
    EpicStoreItemUI[] itemsUI;
    public OpenCloseUI openCloseUI;
    [SerializeField] Button[] tabButtons;
    [SerializeField] Button inAppTabButton;
    SwitchTabUI switchTabUI;
    [SerializeField] GameObject inAppCanvas;
    [SerializeField] GameObject[] inAppItems;
    InAppItemUI[] inAppItemsUI;
    [SerializeField] GameObject hideImageObject;
    public TextMeshProUGUI inAppFailedText;
    OpenCloseUI inAppPurchaseOpenCloseUI;
    [SerializeField] Button restorePurchaseButton;
    public ConfirmUI confirmUI;

    public void Start()
    {
        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(quitButton);
        inAppPurchaseOpenCloseUI = new OpenCloseUI(inAppCanvas);
        inAppPurchaseOpenCloseUI.SetOpenButton(inAppTabButton);
        inAppPurchaseOpenCloseUI.SetCloseButton(tabButtons);

        openButton.onClick.AddListener(() => game.epicStoreCtrl.CheckHack());
        openButton.onClick.AddListener(() => main.inAppRestore.GetInAppPurchaseReport());
        restorePurchaseButton.onClick.AddListener(() => { restorePurchaseButton.interactable = false; main.inAppRestore.RestoreAction(); });

        switchTabUI = new SwitchTabUI(tabButtons, true);

        itemsUI = new EpicStoreItemUI[items.Length];
        for (int i = 0; i < itemsUI.Length; i++)
        {
            int count = i;
            itemsUI[i] = new EpicStoreItemUI(this, items[count], () =>
            {
                if (count < game.epicStoreCtrl.itemArray[switchTabUI.currentId].Length)
                    return game.epicStoreCtrl.itemArray[switchTabUI.currentId][count];
                return new EPIC_STORE(game.epicStoreCtrl);
            });
        }
        inAppItemsUI = new InAppItemUI[inAppItems.Length];
        for (int i = 0; i < inAppItemsUI.Length; i++)
        {
            int count = i;
            inAppItemsUI[i] = new InAppItemUI(this, inAppItems[count], game.inAppPurchaseCtrl.inAppPuchaseList[count]);
        }

        switchTabUI.openAction = () =>
        {
            for (int i = 0; i < itemsUI.Length; i++)
            {
                itemsUI[i].SetUI();
            }
        };
        tabButtons[0].onClick.Invoke();

        confirmUI = new ConfirmUI(gameUI.popupCtrlUI.defaultConfirm);
    }
    public void UpdateUI()
    {
        SetActive(hideImageObject, SteamIAP.isTxn);

        if (!openCloseUI.isOpen) return;
        ecText.text = "<sprite=\"EpicCoin\" index=0> Epic Coin : " + game.epicStoreCtrl.epicCoin.value.ToString("F0");
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int count = i;
            tabButtons[i].interactable = game.epicStoreCtrl.itemArray[count].Length > 0;
        }
        for (int i = 0; i < itemsUI.Length; i++)
        {
            int count = i;
            bool isActiveself = count < game.epicStoreCtrl.itemArray[switchTabUI.currentId].Length;
            SetActive(items[count], isActiveself);
            if (isActiveself) itemsUI[i].UpdateUI();
        }
        for (int i = 0; i < inAppItemsUI.Length; i++)
        {
            inAppItemsUI[i].UpdateUI();
        }
        restorePurchaseButton.interactable = main.inAppRestore.CanRestore();
    }
}

public class InAppItemUI
{
    EpicStoreMenuUI menuUI;
    GameObject gameObject;
    InAppPurchase inAppPurchase;
    TextMeshProUGUI nameText, descriptionText, priceText;
    Button purchaseButton;
    SteamIAP steamIAP;
    public InAppItemUI(EpicStoreMenuUI menuUI, GameObject gameObject, InAppPurchase inAppPurchase)
    {
        this.menuUI = menuUI;
        this.gameObject = gameObject;
        this.inAppPurchase = inAppPurchase;
        nameText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        descriptionText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        purchaseButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        priceText = purchaseButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        steamIAP = new SteamIAP();
        steamIAP.Initialize(purchaseButton, inAppPurchase.id.ToString(), inAppPurchase.priceCent, inAppPurchase.description, inAppPurchase.successAction);
    }
    public void UpdateUI()
    {
        nameText.text = inAppPurchase.NameString();
        descriptionText.text = inAppPurchase.Description();
        priceText.text = "$" + inAppPurchase.priceDollars.ToString("F2");
    }

}

public class EpicStoreItemUI
{
    EpicStoreMenuUI menuUI;
    GameObject gameObject;
    Image thisImage, iconImage;
    static Color white = new Color(1f, 1f, 1f, 200 / 255f);
    static Color green = new Color(0f, 1f, 0f, 200 / 255f);
    Func<EPIC_STORE> item;
    TextMeshProUGUI nameText, descriptionText, priceText;
    Button purchaseButton;

    public EpicStoreItemUI(EpicStoreMenuUI menuUI, GameObject gameObject, Func<EPIC_STORE> item)
    {
        this.menuUI = menuUI;
        this.gameObject = gameObject;
        thisImage = gameObject.GetComponent<Image>();
        this.item = item;
        iconImage = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
        nameText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        descriptionText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        purchaseButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        priceText = purchaseButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        purchaseButton.onClick.AddListener(() => { OnClickAction(); gameUI.soundUI.Play(SoundEffect.Upgrade); });
    }
    void OnClickAction()
    {
        menuUI.confirmUI.SetUI(ConfirmString());
        SetActive(menuUI.confirmUI.quitButton.gameObject, true);
        SetActive(menuUI.confirmUI.okButton.gameObject, true);
        menuUI.confirmUI.okButton.onClick.RemoveAllListeners();
        menuUI.confirmUI.okButton.onClick.AddListener(()=> { item().transaction.Buy(); menuUI.confirmUI.Hide(); });
    }
    string ConfirmString()
    {
        string tempStr = "Are you sure to purchase this item?\n\n" + item().NameString() + "\n<sprite=\"EpicCoin\" index=0> " + item().price.ToString("F0");
        if (item().isFreeTheFirst && !item().IsPurchased())
            tempStr += "\n<color=green>This item is FREE the first time!</color>";
        return tempStr;
    }
    public void SetUI()
    {
        iconImage.sprite = sprite.epicStoreIcons[(int)item().kind];
    }
    public void UpdateUI()
    {
        thisImage.color = item().IsSoldOut() ? green : white;
        nameText.text = item().NameString();
        descriptionText.text = DescriptionString();
        if (item().isFreeTheFirst && !item().IsPurchased()) descriptionText.text += "\n<color=green>This item is normally <sprite=\"EpicCoin\" index=0> " + item().price.ToString("F0") + ", but it's FREE the first time!</color>";

        purchaseButton.interactable = item().transaction.CanBuy();

        if (item().IsSoldOut())
            priceText.text = "Sold Out";
        else if (item().isFreeTheFirst && !item().IsPurchased())
            priceText.text = "<color=green>Free!</color>";
        else
            priceText.text = "<sprite=\"EpicCoin\" index=0> " + item().price.ToString("F0");
    }

    string DescriptionString()
    {
        string tempStr = item().EffectString();
        if (item().isOnetimePurchase) tempStr += " <color=yellow>( One-time Purchase )</color>";
        else if (item().purchaseLimitNum > 0 && item().purchaseLimitNum < 1000) tempStr += " <color=yellow>( Purchase # Limit : " + tDigit(item().purchasedNum.value) + " / " + tDigit(item().purchaseLimitNum) + " )</color>";
        return tempStr;
    }
}
