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

public class ShopMenuUI : MENU_UI
{
    [SerializeField] Button[] selectButtons;
    TextMeshProUGUI[] selectButtonTexts;
    [SerializeField] GameObject[] tables;
    [SerializeField] TextMeshProUGUI restockTimeText;

    [SerializeField] GameObject[] materials;
    public ShopMaterialTableUI materialTableUI;
    [SerializeField] GameObject[] traps;
    public ShopTrapTableUI trapTableUI;
    [SerializeField] GameObject[] blessings;
    public ShopBlessingTableUI blessingTableUI;
    [SerializeField] GameObject[] townMaterials;
    public ShopTownMaterialTableUI townMaterialTableUI;

    public SwitchCanvasUI switchCanvasUI;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;
    PopupUI popupUIPet;

    public static PopupUI popupUI;
    private void Awake()
    {
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUIPet = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
    }
    // Start is called before the first frame update
    void Start()
    {
        selectButtonTexts = new TextMeshProUGUI[selectButtons.Length];
        for (int i = 0; i < selectButtonTexts.Length; i++)
        {
            selectButtonTexts[i] = selectButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
        materialTableUI = new ShopMaterialTableUI(materials);
        trapTableUI = new ShopTrapTableUI(traps);
        blessingTableUI = new ShopBlessingTableUI(blessings);
        townMaterialTableUI = new ShopTownMaterialTableUI(townMaterials);
        switchCanvasUI = new SwitchCanvasUI(tables, selectButtons, false, false, true);
        selectButtons[0].onClick.Invoke();

        popupUI.SetTargetObject(selectButtons[3].gameObject, () => "<sprite=\"locks\" index=0> Town Building [ Arcane Researcher ]", () => !townMaterialTableUI.shopTownMaterialsUI[0].shopTownMaterial.unlock.IsUnlocked());
        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.DevifFish, MonsterColor.Green).pet, petQoLIcon, popupUIPet, "Auto-buy Blessings");
    }
    public override void UpdateUI()
    {
        base.UpdateUI();

        selectButtons[3].interactable = townMaterialTableUI.shopTownMaterialsUI[0].shopTownMaterial.unlock.IsUnlocked();
        selectButtonTexts[3].text = townMaterialTableUI.shopTownMaterialsUI[0].shopTownMaterial.unlock.IsUnlocked() ? "Town Material" : "???";

        restockTimeText.text = "Next Restock Time Left : " + DoubleTimeToDate(game.shopCtrl.restockTimeleft);
        if (switchCanvasUI.openCloseUIs[0].isOpen)
            materialTableUI.UpdateUI();
        if (switchCanvasUI.openCloseUIs[1].isOpen)
            trapTableUI.UpdateUI();
        if (switchCanvasUI.openCloseUIs[2].isOpen)
        {
            blessingTableUI.UpdateUI();
            restockTimeText.text += "\nAvailable : " + tDigit(game.shopCtrl.Blessing(BlessingKind.Hp).stockNum) + " / " + tDigit(game.shopCtrl.blessingStockNum.Value());
        }
        if (switchCanvasUI.openCloseUIs[3].isOpen)
            townMaterialTableUI.UpdateUI();

        popupUI.UpdateUI();
        popupUIPet.UpdateUI();
        petQoLUI.UpdateUI();
    }
}

//Material
public class ShopMaterialTableUI
{
    public ShopMaterialTableUI(GameObject[] shopMaterialObjects)
    {
        this.shopMaterialObjects = shopMaterialObjects;
        shopMaterialsUI = new ShopMaterialUI[shopMaterialObjects.Length];
        for (int i = 0; i < shopMaterialsUI.Length; i++)
        {
            int count = i;
            shopMaterialsUI[i] = new ShopMaterialUI(shopMaterialObjects[count], game.shopCtrl.shop_MaterialList[count]);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < shopMaterialsUI.Length; i++)
        {
            shopMaterialsUI[i].UpdateUI();
        }
    }
    public GameObject[] shopMaterialObjects;
    public ShopMaterialUI[] shopMaterialsUI;
}
public class ShopMaterialUI
{
    public ShopMaterialUI(GameObject gameObject, Shop_Material shopMaterial)
    {
        this.gameObject = gameObject;
        this.shopMaterial = shopMaterial;
        iconImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        infoText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        buyButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        buyText = buyButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        sellButton = gameObject.transform.GetChild(4).gameObject.GetComponent<Button>();
        sellText = sellButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        petIconFrame = gameObject.transform.GetChild(5).gameObject.GetComponent<Image>();
        petQoLUI = new PetQoLUI(pet, petIconFrame.gameObject, ShopMenuUI.popupUI, "Auto-Buy");
        SetUI();
    }
    public void SetUI()
    {
        iconImage.sprite = sprite.materials[(int)shopMaterial.materialKind];
        buyButton.onClick.AddListener(() => shopMaterial.Buy());
        sellButton.onClick.AddListener(() => shopMaterial.Sell());
    }
    public void UpdateUI()
    {
        if (!shopMaterial.unlock.IsUnlocked())
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);
        buyButton.interactable = shopMaterial.buyTransaction.CanBuy();
        sellButton.interactable = shopMaterial.sellTransaction.CanBuy();
        infoText.text = InfoString();
        buyText.text = "Buy ( " + tDigit(shopMaterial.buyTransaction.LevelIncrement()) + " )\n<sprite=\"resource\" index=0> " + tDigit(shopMaterial.buyTransaction.Cost());
        sellText.text = "Sell ( " + tDigit(shopMaterial.sellTransaction.LevelIncrement()) + " )\n<sprite=\"resource\" index=0> " + tDigit(shopMaterial.SellPrice() * shopMaterial.sellTransaction.LevelIncrement());
        petQoLUI.UpdateUI();
    }
    string InfoString()
    {
        string tempStr = optStr + "<size=20>";
        tempStr += shopMaterial.item.Name();
        tempStr += "<size=18>";
        tempStr += "\nStock " + tDigit(shopMaterial.stockNum) + " / " + tDigit(shopMaterial.LimitNum());
        tempStr += "\nYou have " + tDigit(shopMaterial.item.value);
        return tempStr;
    }
    public Shop_Material shopMaterial;
    public GameObject gameObject;
    public Image iconImage, petIconFrame;
    public TextMeshProUGUI infoText, buyText, sellText;
    public Button buyButton, sellButton;
    public PetQoLUI petQoLUI;
    public MonsterPet pet//AutoBuyQoLを持つPet
    {
        get
        {
            switch (shopMaterial.materialKind)
            {
                case MaterialKind.OilOfSlime: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Yellow).pet;
                case MaterialKind.EnchantedCloth: return game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Yellow).pet;
                case MaterialKind.SpiderSilk: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Yellow).pet;
                case MaterialKind.BatWing: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Yellow).pet;
                case MaterialKind.FairyDust: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Yellow).pet;
                case MaterialKind.FoxTail: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Fox, MonsterColor.Yellow).pet;
                case MaterialKind.FishScales: return game.monsterCtrl.GlobalInformation(MonsterSpecies.DevifFish, MonsterColor.Yellow).pet;
                case MaterialKind.CarvedBranch: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Treant, MonsterColor.Yellow).pet;
                case MaterialKind.ThickFur: return game.monsterCtrl.GlobalInformation(MonsterSpecies.FlameTiger, MonsterColor.Yellow).pet;
                case MaterialKind.UnicornHorn: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Unicorn, MonsterColor.Yellow).pet;
            }
            return null;
        }
    }
}

//Trap
public class ShopTrapTableUI
{
    public ShopTrapTableUI(GameObject[] shopTrapObjects)
    {
        this.shopTrapObjects = shopTrapObjects;
        shopTrapsUI = new ShopTrapUI[shopTrapObjects.Length];
        for (int i = 0; i < shopTrapsUI.Length; i++)
        {
            int count = i;
            shopTrapsUI[i] = new ShopTrapUI(shopTrapObjects[count], game.shopCtrl.shop_TrapList[count]);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < shopTrapsUI.Length; i++)
        {
            shopTrapsUI[i].UpdateUI();
        }
    }
    public GameObject[] shopTrapObjects;
    public ShopTrapUI[] shopTrapsUI;
}
public class ShopTrapUI
{
    public ShopTrapUI(GameObject gameObject, Shop_Trap shopTrap)
    {
        this.gameObject = gameObject;
        this.shopTrap = shopTrap;
        iconImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        infoText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        buyButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        buyText = buyButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        petIconFrame = gameObject.transform.GetChild(4).gameObject.GetComponent<Image>();
        petQoLUI = new PetQoLUI(pet, petIconFrame.gameObject, ShopMenuUI.popupUI, "Auto-Buy");
        SetUI();
    }
    public void SetUI()
    {
        iconImage.sprite = sprite.potions[(int)shopTrap.potionKind];
        buyButton.onClick.AddListener(() => shopTrap.Buy());
    }

    public void UpdateUI()
    {
        if (!shopTrap.unlock.IsUnlocked())
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);

        buyButton.interactable = shopTrap.buyTransaction.CanBuy();
        infoText.text = InfoString();
        buyText.text = "Buy ( " + tDigit(shopTrap.buyTransaction.LevelIncrement()) + " )\n<sprite=\"resource\" index=0> " + tDigit(shopTrap.buyTransaction.Cost());
        petQoLUI.UpdateUI();
    }
    string InfoString()
    {
        string tempStr = optStr + "<size=20>";
        tempStr += localized.PotionName(shopTrap.potionKind);
        tempStr += "<size=18>";
        tempStr += "\nStock " + tDigit(shopTrap.stockNum) + " / " + tDigit(shopTrap.LimitNum());
        if (!game.inventoryCtrl.CanCreatePotion(shopTrap.potionKind, 1))
            tempStr += "  <size=16><color=yellow>(You need an empty Utility Slot to buy this)</color></size>";
        tempStr += "\n" + localized.PotionEffect(shopTrap.potionKind, shopTrap.globalInfo.EffectValue(0));
        return tempStr;
    }
    public Shop_Trap shopTrap;
    public GameObject gameObject;
    public Image iconImage, petIconFrame;//, petIcon;
    public TextMeshProUGUI infoText, buyText;
    public Button buyButton;
    public PetQoLUI petQoLUI;
    public MonsterPet pet//AutoBuyQoLを持つPet
    {
        get
        {
            switch (shopTrap.potionKind)
            {
                case PotionKind.ThrowingNet: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Blue).pet;
                case PotionKind.IceRope: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Red).pet;
                case PotionKind.ThunderRope: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Green).pet;
                case PotionKind.FireRope: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Green).pet;
                case PotionKind.LightRope: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Purple).pet;
                case PotionKind.DarkRope: return game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Purple).pet;
            }
            return null;
        }
    }
}

//Blessing
public class ShopBlessingTableUI
{
    public ShopBlessingTableUI(GameObject[] shopBlessingObjects)
    {
        this.shopBlessingObjects = shopBlessingObjects;
        shopBlessingsUI = new ShopBlessingUI[shopBlessingObjects.Length];
        for (int i = 0; i < shopBlessingsUI.Length; i++)
        {
            int count = i;
            shopBlessingsUI[i] = new ShopBlessingUI(shopBlessingObjects[count], game.shopCtrl.shop_BlessingList[count]);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < shopBlessingsUI.Length; i++)
        {
            shopBlessingsUI[i].UpdateUI();
        }
    }
    public GameObject[] shopBlessingObjects;
    public ShopBlessingUI[] shopBlessingsUI;
}
public class ShopBlessingUI
{
    public ShopBlessingUI(GameObject gameObject, Shop_Blessing shopBlessing)
    {
        this.gameObject = gameObject;
        this.shopBlessing = shopBlessing;
        iconImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        infoText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        buyButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        buyText = buyButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        //availableText = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        SetUI();
    }
    public void SetUI()
    {
        iconImage.sprite = sprite.blessing[(int)shopBlessing.blessingKind];
        buyButton.onClick.AddListener(() => shopBlessing.buyTransaction.Buy(true));
    }
    public void UpdateUI()
    {
        if (!shopBlessing.unlock.IsUnlocked())
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);

        buyButton.interactable = shopBlessing.buyTransaction.CanBuy(true);
        infoText.text = InfoString();
        buyText.text = "Buy\n<sprite=\"resource\" index=0> " + tDigit(shopBlessing.buyTransaction.Cost(true));
        //availableText.text = "Available : " + tDigit(shopBlessing.stockNum) + " / " + tDigit(shopBlessing.LimitNum());
    }
    string InfoString()
    {
        string tempStr = optStr + "<size=20>";
        tempStr += shopBlessing.blessing.NameString() + "<size=18>  ( Duration : " + DoubleTimeToDate(shopBlessing.blessing.duration) + " )";
        tempStr += "<size=18>";
        tempStr += "\n- " + shopBlessing.blessing.EffectString();
        if (shopBlessing.blessing.SubEffectString() != "") tempStr += "\n- " + shopBlessing.blessing.SubEffectString();
        return tempStr;
    }
    public Shop_Blessing shopBlessing;
    public GameObject gameObject;
    public Image iconImage;
    public TextMeshProUGUI infoText, buyText;//, availableText;
    public Button buyButton;
}

//TownMaterial
public class ShopTownMaterialTableUI
{
    public ShopTownMaterialTableUI(GameObject[] shopTownMaterialObjects)
    {
        this.shopTownMaterialObjects = shopTownMaterialObjects;
        shopTownMaterialsUI = new ShopTownMaterialUI[shopTownMaterialObjects.Length];
        for (int i = 0; i < shopTownMaterialsUI.Length; i++)
        {
            int count = i;
            shopTownMaterialsUI[i] = new ShopTownMaterialUI(shopTownMaterialObjects[count], game.shopCtrl.shop_TownMaterialList[count]);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < shopTownMaterialsUI.Length; i++)
        {
            shopTownMaterialsUI[i].UpdateUI();
        }
    }
    public GameObject[] shopTownMaterialObjects;
    public ShopTownMaterialUI[] shopTownMaterialsUI;
}
public class ShopTownMaterialUI
{
    public ShopTownMaterialUI(GameObject gameObject, Shop_TownMaterial shopTownMaterial)
    {
        this.gameObject = gameObject;
        this.shopTownMaterial = shopTownMaterial;
        //iconImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        infoText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        buyButton = gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        buyText = buyButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        //sellButton = gameObject.transform.GetChild(4).gameObject.GetComponent<Button>();
        //sellText = sellButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        SetUI();
    }
    public void SetUI()
    {
        //iconImage.sprite = sprite.materials[(int)shopMaterial.materialKind];
        buyButton.onClick.AddListener(() => shopTownMaterial.Buy());
        //sellButton.onClick.AddListener(() => shopMaterial.Sell());
    }
    public void UpdateUI()
    {
        if (!shopTownMaterial.unlock.IsUnlocked())
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);
        buyButton.interactable = shopTownMaterial.buyTransaction.CanBuy();
        //sellButton.interactable = shopMaterial.sellTransaction.CanBuy();
        infoText.text = InfoString();
        buyText.text = "Convert ( " + tDigit(shopTownMaterial.buyTransaction.LevelIncrement() * game.shopCtrl.convertTownMaterialAmount.Value())
            + " )\n" + tDigit(shopTownMaterial.buyTransaction.Cost()) + " " + shopTownMaterial.buyTransaction.cost[0].resource.Name();
        //sellText.text = "Sell ( " + tDigit(shopMaterial.sellTransaction.LevelIncrement()) + " )\n<sprite=\"resource\" index=0> " + tDigit(shopMaterial.SellPrice() * shopMaterial.sellTransaction.LevelIncrement());
    }
    string InfoString()
    {
        string tempStr = optStr + "<size=20>Convert <color=green>";
        tempStr += tDigit(shopTownMaterial.Price()) + " " + shopTownMaterial.buyTransaction.cost[0].resource.Name();
        tempStr += "</color> to <color=green>" + " " + tDigit(game.shopCtrl.convertTownMaterialAmount.Value()) + " " + shopTownMaterial.item.Name() + "</color>";
        tempStr += "<size=18>";
        tempStr += "\nYou have " + tDigit(shopTownMaterial.buyTransaction.cost[0].resource.value) + " " + shopTownMaterial.buyTransaction.cost[0].resource.Name();
        tempStr += "\nYou have " + tDigit(shopTownMaterial.item.value) + " " + shopTownMaterial.item.Name();
        return tempStr;
    }
    public Shop_TownMaterial shopTownMaterial;
    public GameObject gameObject;
    public Image iconImage;
    public TextMeshProUGUI infoText, buyText, sellText;
    public Button buyButton, sellButton;
}
