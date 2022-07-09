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
using static InventoryParameter;
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public class EquipmentDictionaryUI : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    EquipmentItemUI[] itemsUI;
    [SerializeField] Button[] pageButtons;
    [SerializeField] Button removeAllButton;
    [SerializeField] GameObject[] pageCanvases;
    SwitchCanvasUI pageSwitchCanvasUI;
    [SerializeField] TextMeshProUGUI disassembleText;
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] Button resetButton;
    [SerializeField] GameObject[] upgrades;
    DictionaryUpgradeUI[] upgradesUI;
    public EquipPopupUI popupUI;
    public EquipDictionaryPopupUI equipDictionaryPopupUI;
    DictionaryUpgradePopupUI dictionaryPopupUI;
    public Button openButton, closeButton;
    [NonSerialized] public TextMeshProUGUI openButtonText;
    public OpenCloseUI openCloseUI;
    PopupUI disassembleExcludeEnchantPopupUI, resetButtonPopupUI;
    static Toggle disassembleExcludeEnchantToggle => SettingMenuUI.Toggle(ToggleKind.AutoDisassembleExcludeEnchanted).thisToggle;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;
    PopupUI popupUIPet;

    private void Awake()
    {
        //Popup
        popupUI = new EquipPopupUI(gameUI.popupCtrlUI.equipment);
        popupUIPet = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        equipDictionaryPopupUI = new EquipDictionaryPopupUI(gameUI.popupCtrlUI.equipment);
        disassembleExcludeEnchantPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup, () => !disassembleExcludeEnchantToggle.interactable);
        //↑２個目の引数はEpicStore購入条件でも可

        openButtonText = openButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        pageSwitchCanvasUI = new SwitchCanvasUI(pageCanvases, pageButtons, false, false, true);
        itemsUI = new EquipmentItemUI[Math.Min(items.Length, game.equipmentCtrl.equipments.Length - 1)];
        for (int i = 0; i < itemsUI.Length; i++)
        {
            int count = i;
            itemsUI[count] = new EquipmentItemUI(items[count], () => count + 1);
        }
        pageButtons[0].onClick.Invoke();

        upgradesUI = new DictionaryUpgradeUI[upgrades.Length];
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i] = new DictionaryUpgradeUI(upgrades[i], (DictionaryUpgradeKind)i);
        }

        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(closeButton);

        //Lv順に並び替え
        Sort();

        //DictionaryUpgradeのPointReset
        resetButton.onClick.AddListener(game.equipmentCtrl.ResetDictionaryUpgrade);

        //Popup
        for (int i = 0; i < itemsUI.Length; i++)
        {
            int count = i;
            equipDictionaryPopupUI.SetTargetObject(itemsUI[count].thisObject, () => equipDictionaryPopupUI.SetUI(() => itemsUI[count].thisEquipment, ()=> itemsUI[count].thisEquipment.globalInfo.isGotOnce));
        }
        for (int i = 0; i < gameUI.worldMapUI.areaTableUI.areasUI.Length; i++)
        {
            AreaUI tempAreaUI = gameUI.worldMapUI.areaTableUI.areasUI[i];
            popupUI.SetTargetObject(tempAreaUI.uniqueEQobject, () => popupUI.SetUI(() => tempAreaUI.uniqueEQ, () => tempAreaUI.uniqueEQ.globalInfo.isGotOnce));
        }
        popupUI.SetTargetObject(gameUI.worldMapUI.areaInfoUI.uniqueEQobject, () => popupUI.SetUI(() => gameUI.worldMapUI.areaInfoUI.uniqueEQ, () => gameUI.worldMapUI.areaInfoUI.uniqueEQ.globalInfo.isGotOnce));
        popupUI.SetTargetObject(gameUI.battleCtrlUI.areaInfoUI.uniqueEQobject, () => popupUI.SetUI(() => gameUI.battleCtrlUI.areaInfoUI.uniqueEQ, () => gameUI.battleCtrlUI.areaInfoUI.uniqueEQ.globalInfo.isGotOnce));

        dictionaryPopupUI = new DictionaryUpgradePopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            dictionaryPopupUI.SetTargetObject(upgrades[count], () => dictionaryPopupUI.ShowAction(upgradesUI[count].thisUpgrade));
        }
        disassembleExcludeEnchantPopupUI.SetTargetObject(disassembleExcludeEnchantToggle.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.DisassembleEQExclude).NameString() + " in Epic Store");
        disassembleExcludeEnchantPopupUI.additionalShowCondition = () => !game.epicStoreCtrl.Item(EpicStoreKind.DisassembleEQExclude).IsPurchased();
        resetButtonPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        resetButtonPopupUI.SetTargetObject(resetButton.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.DictionaryReset).NameString() + " in Epic Store");
        resetButtonPopupUI.additionalShowCondition = () => !game.epicStoreCtrl.Item(EpicStoreKind.DictionaryReset).IsPurchased();

        removeAllButton.onClick.AddListener(() => game.equipmentCtrl.RemoveAllAutoDisassemble());

        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.DevifFish, MonsterColor.Red).pet, petQoLIcon, popupUIPet, "Auto-Disassemble on crafting");
    }


    //Lv順に並び替え
    void Sort()
    {
        List<EquipmentItemUI> itemsUIList = new List<EquipmentItemUI>();
        itemsUIList.AddRange(itemsUI);
        itemsUIList.Sort(CompareRequiredLevel);
        for (int i = 0; i < itemsUI.Length; i++)
        {
            int count = i;
            itemsUI[count] = new EquipmentItemUI(items[count], itemsUIList[count].id);
            itemsUI[count].SetInfo();
        }
    }

    int CompareRequiredLevel(EquipmentItemUI x, EquipmentItemUI y)
    {
        if (x.thisEquipment.RequiredLevel() > y.thisEquipment.RequiredLevel()) return 1;
        if (x.thisEquipment.RequiredLevel() < y.thisEquipment.RequiredLevel()) return -1;
        if ((int)x.thisEquipment.globalInfo.rarity > (int)y.thisEquipment.globalInfo.rarity) return 1;
        if ((int)x.thisEquipment.globalInfo.rarity < (int)y.thisEquipment.globalInfo.rarity) return -1;
        if ((int)x.thisEquipment.globalInfo.setKind > (int)y.thisEquipment.globalInfo.setKind) return 1;
        if ((int)x.thisEquipment.globalInfo.setKind < (int)y.thisEquipment.globalInfo.setKind) return -1;
        if ((int)x.thisEquipment.globalInfo.part > (int)y.thisEquipment.globalInfo.part) return 1;
        if ((int)x.thisEquipment.globalInfo.part < (int)y.thisEquipment.globalInfo.part) return -1;
        if ((int)x.thisEquipment.globalInfo.kind > (int)y.thisEquipment.globalInfo.kind) return 1;
        if ((int)x.thisEquipment.globalInfo.kind < (int)y.thisEquipment.globalInfo.kind) return -1;
        return 0;
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        disassembleText.text = optStr + "<size=20>Auto-Disassemble : " + tDigit(game.equipmentCtrl.AutoDisassembleActiveNum()) + " / " + tDigit(game.equipmentCtrl.autoDisassembleAvailableNum.Value());
        upgradeText.text = optStr + "<size=24>Dictionary Upgrade <size=20>( point left : " + tDigit(game.equipmentCtrl.dictionaryPointLeft.value) + " / " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " )";
        popupUI.UpdateUI();
        popupUIPet.UpdateUI();
        petQoLUI.UpdateUI();
        equipDictionaryPopupUI.UpdateUI();
        dictionaryPopupUI.UpdateUI();
        for (int i = 0; i < itemsUI.Length; i++)
        {
            itemsUI[i].UpdateUI();
        }
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i].UpdateUI();
        }

        disassembleExcludeEnchantToggle.interactable = game.epicStoreCtrl.Item(EpicStoreKind.DisassembleEQExclude).IsPurchased();
        disassembleExcludeEnchantPopupUI.UpdateUI();

        resetButton.interactable = game.epicStoreCtrl.Item(EpicStoreKind.DictionaryReset).IsPurchased();
        resetButtonPopupUI.UpdateUI();

        removeAllButton.interactable = game.equipmentCtrl.AutoDisassembleActiveNum() > 0;
    }
    private void Update()
    {
        equipDictionaryPopupUI.Update();
    }
}

//DictionaryPopup
public class EquipDictionaryPopupUI : EquipPopupUI
{
    public override bool isDicitonary => true;
    public EquipDictionaryPopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public override string DescriptionString()
    {
        string tempString = base.DescriptionString();
        if (thisEquipment().CanCraft())
        {
            tempString += "\n\n";
            tempString += optStr + "<size=20><u>Cost to Craft</u><size=18>";
            if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.AutoCraftDisassembleEQ))
            {
                tempString += "<color=orange> Disassemble";
                if (game.epicStoreCtrl.Item(EpicStoreKind.CraftUnlimitedDisassemble).IsPurchased())
                {
                    tempString += " x" + tDigit(thisEquipment().craftTransaction.LevelIncrement());
                }
                tempString += "</color>";
            }
            tempString += "\n" + thisEquipment().craftTransaction.DescriptionString();
            tempString += "<color=yellow>Shift + C to craft</color>";
        }
        return tempString;
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
    }
    long tempCraftNum = 0;
    public override void Update()
    {
        if (!isShow) return;
        if (isShiftPressed && Input.GetKeyDown(KeyCode.C))
        {
            tempCraftNum = thisEquipment().craftTransaction.LevelIncrement();
            if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.AutoCraftDisassembleEQ))
            {
                thisEquipment().globalInfo.DisassembleMaterialKind().Increase((-thisEquipment().globalInfo.CraftCostMaterialNum() + thisEquipment().DisassembleMaterialNum()) * tempCraftNum);
                main.SR.disassembledEquipmentNums[(int)thisEquipment().kind] += tempCraftNum;
                main.S.disassembledEquipmentNums[(int)thisEquipment().kind] += tempCraftNum;
                main.SR.townMatGainFromdisassemble[(int)thisEquipment().kind] += thisEquipment().DisassembleMaterialNum() * tempCraftNum;
                main.S.townMatGainFromdisassemble[(int)thisEquipment().kind] += thisEquipment().DisassembleMaterialNum() * tempCraftNum;
                return;
            }
            thisEquipment().craftTransaction.Buy();
        }
    }
}

public class EquipmentItemUI
{
    Button thisButton;
    public EquipmentItemUI(GameObject gameObject, Func<int> id)
    {
        thisObject = gameObject;
        this.id = id;
    }
    public void SetInfo()//GameObject gameObject, int id)
    {
        thisObject.GetComponent<Image>().sprite = sprite.equipments[id()];
        starText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        disassembleObject = thisObject.transform.GetChild(1).gameObject;
        quetionmark = thisObject.transform.GetChild(3).gameObject;
        thisButton = thisObject.GetComponent<Button>();
        thisButton.onClick.AddListener(() => thisEquipment.globalInfo.SwitchAutoDisassemble());
    }
    public void UpdateUI()
    {
        //Debug
        //SetActive(quetionmark, false);
        SetActive(quetionmark, !thisEquipment.globalInfo.isGotOnce);
        if (!thisEquipment.globalInfo.isGotOnce) return;
        starText.text = StarString();
        SetActive(disassembleObject, thisEquipment.globalInfo.isAutoDisassemble);
    }
    string StarString()
    {        
        string tempString = optStr + "<b><size=18>";
        if (thisEquipment.globalInfo.levels[0].isMaxedThisRebirth) tempString += "<color=#00FFFF>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipment.globalInfo.levels[1].isMaxedThisRebirth) tempString += "<color=#FFFF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipment.globalInfo.levels[2].isMaxedThisRebirth) tempString += "<color=#99FF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        tempString += " \n";
        if (thisEquipment.globalInfo.levels[3].isMaxedThisRebirth) tempString += "<color=#00FFFF>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipment.globalInfo.levels[4].isMaxedThisRebirth) tempString += "<color=#FFFF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipment.globalInfo.levels[5].isMaxedThisRebirth) tempString += "<color=#99FF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        tempString += "</b>";
        tempString += optStr + " <size=12>\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n         <color=green>Lv " + tDigit(thisEquipment.Level());
        return tempString;
    }
    public Func<int> id;
    public GameObject thisObject;
    public DictionaryEquipment thisEquipment => game.equipmentCtrl.equipments[id()];
    TextMeshProUGUI starText;
    GameObject disassembleObject;
    GameObject quetionmark;
    Image frameImage;
}



public class DictionaryUpgradeUI
{
    public DictionaryUpgradeUI(GameObject gameObject, DictionaryUpgradeKind kind)
    {
        thisObject = gameObject;
        this.kind = kind;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(PlusPoint);
        lockObject = thisObject.transform.GetChild(4).gameObject;
    }
    public void UpdateUI()
    {
        SetActive(lockObject, !thisUpgrade.unlock.IsUnlocked());
        if (!thisUpgrade.unlock.IsUnlocked()) return;
        plusButton.interactable = Interactable();
        nameText.text = NameString();
        levelText.text = LevelString();
    }
    string NameString()
    {
        return localized.DictionaryUpgarde(kind).name;
    }
    string LevelString()
    {
        return optStr + "<color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> / " + tDigit(thisUpgrade.level.maxValue());
    }
    bool Interactable()
    {
        return thisUpgrade.transaction.CanBuy();
    }
    void PlusPoint()
    {
        thisUpgrade.transaction.Buy(); ;
    }
    public string DescriptionString()
    {
        string tempString = optStr + "<size=20>" + NameString();
        return tempString;
    }

    GameObject thisObject;
    DictionaryUpgradeKind kind;
    public DictionaryUpgrade thisUpgrade { get => game.equipmentCtrl.DictionaryUpgrade(kind); }
    TextMeshProUGUI nameText, levelText;
    Button plusButton;
    GameObject lockObject;
}

public class DictionaryUpgradePopupUI : PopupUI
{
    public DictionaryUpgradePopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public void ShowAction(DictionaryUpgrade upgrade)
    {
        if (!upgrade.unlock.IsUnlocked()) return;
        SetUI(() => DescriptionString(upgrade));
    }
    string DescriptionString(DictionaryUpgrade upgrade)
    {
        string tempString = optStr + "<size=20>" + localized.DictionaryUpgarde(upgrade.kind).name + " < <color=green>Lv " + tDigit(upgrade.level.value) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- Max Level : Lv " + tDigit(upgrade.level.maxValue());
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- Current : " + localized.DictionaryUpgarde(upgrade.kind).effect + " + " + percent(upgrade.effectValue, 3);
        tempString += "\n- Next : " + localized.DictionaryUpgarde(upgrade.kind).effect + " + " + percent(upgrade.nextEffectValue, 3) + " ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )"; 
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(upgrade.transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        return tempString;
    }
}