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

public class LabMenuUI : MENU_UI
{
    //Alchemy
    [SerializeField] TextMeshProUGUI alchemyPointText;
    [SerializeField] TextMeshProUGUI mysteriousWaterProgressText;
    [SerializeField] TextMeshProUGUI mysteriousWaterNumText;
    [SerializeField] Slider mysteriousWaterSlider;
    [SerializeField] Button mysteriousWaterExpandButton;
    [SerializeField] GameObject petQoLIcon, petQoLIcon2, expandIconI;
    PetQoLUI petQoLUI, petQoLUI2;

    //Catalyst
    [SerializeField] TextMeshProUGUI availableCatalystText;
    [SerializeField] GameObject[] catalysts;
    CatalystUI[] catalystsUI;
    //Essence
    [SerializeField] GameObject[] essences;
    EssenceProductionUI[] essencesUI;

    //Potion
    [SerializeField] GameObject potionCanvas;
    [SerializeField] GameObject[] alchemyPotions;
    AlchemyPotionUI[] alchemyPotionsUI;
    OpenCloseUI potionOpenCloseUI;
    [SerializeField] GameObject scrollTrans;

    //Upgrade
    [SerializeField] GameObject upgradeCanvas;
    //[SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] GameObject[] upgrades;
    AlchemyUpgradeUI[] upgradesUI;
    OpenCloseUI upgradeOpenCloseUI;
    [SerializeField] Button potionButton, upgradeButton;

    //Popup
    MysteriousWaterPopupUI mysteriousWaterPopupUI;
    CatalystPopupUI catalystPopupUI;
    AlchemyPotionPopupUI alchemyPotionPopupUI;
    AlchemyUpgradePopupUI alchemyUpgradePopupUI;
    PopupUI popupUI;

    //Inventory
    [SerializeField] GameObject materialInventory;
    MaterialTableUI materialTableUI;
    [SerializeField] GameObject essenceInventory;
    EssenceTableUI essenceTableUI;
    [SerializeField] Button materialInventoryButton, essenceInventoryButton;

    //Craft
    [SerializeField] GameObject alchemyCanvas, craftCanvas;
    OpenCloseUI alchemyOpenCloseUI, craftOpenCloseUI;
    [SerializeField] Button alchemyOpenButton, craftOpenButton;
    [SerializeField] Button[] craftSelectButtons;
    public SwitchTabUI craftSwitchTabUI;
    [SerializeField] GameObject[] craftScrolls;
    CraftScrollUI[] craftScrollsUI;
    CraftScrollPopupUI craftScrollPopupUI;
    public bool isAutoDisassemble => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.DisassemblePotion) && GameControllerUI.isShiftPressed && Input.GetKey(KeyCode.D);

    void Start()
    {
        //Inventory
        materialTableUI = new MaterialTableUI(materialInventory);
        essenceTableUI = new EssenceTableUI(essenceInventory);
        materialTableUI.openCloseUI.SetOpenButton(materialInventoryButton);
        materialTableUI.openCloseUI.SetCloseButton(essenceInventoryButton);
        essenceTableUI.openCloseUI.SetOpenButton(essenceInventoryButton);
        essenceTableUI.openCloseUI.SetCloseButton(materialInventoryButton);
        materialInventoryButton.onClick.Invoke();

        //Alchemy
        mysteriousWaterExpandButton.onClick.AddListener(()=> { mysteriousWaterExpandButton.interactable = false; game.alchemyCtrl.ExpandMysteriousWaterCap(); });
        mysteriousWaterExpandButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));

        //Catalyst
        catalystsUI = new CatalystUI[Math.Min(catalysts.Length, game.catalystCtrl.catalysts.Count)];
        for (int i = 0; i < catalysts.Length; i++)
        {
            int count = i;
            if (count < game.catalystCtrl.catalysts.Count)
                catalystsUI[count] = new CatalystUI(catalysts[count], game.catalystCtrl.catalysts[count]);
            else
                SetActive(catalysts[count], false);
        }
        //Essence
        essencesUI = new EssenceProductionUI[Math.Min(essences.Length, game.catalystCtrl.essenceProductions.Count)];
        for (int i = 0; i < essences.Length; i++)
        {
            int count = i;
            if (count < game.catalystCtrl.essenceProductions.Count)
                essencesUI[count] = new EssenceProductionUI(essences[count], game.catalystCtrl.essenceProductions[count]);
            else
                SetActive(essences[count], false);
        }

        //Potion
        alchemyPotionsUI = new AlchemyPotionUI[Math.Min(alchemyPotions.Length, (int)PotionKind.ThrowingNet) - 1];// game.potionCtrl.globalInformations.Count - 1)];
        for (int i = 0; i < alchemyPotions.Length; i++)
        {
            int count = i;
            if (count < (int)PotionKind.ThrowingNet - 1)//game.potionCtrl.globalInformations.Count - 1)
            {
                alchemyPotionsUI[i] = new AlchemyPotionUI(this, alchemyPotions[count], count + 1);
            }
            else
                SetActive(alchemyPotions[count], false);
        }
        potionOpenCloseUI = new OpenCloseUI(potionCanvas, false, false, true);
        potionOpenCloseUI.SetOpenButton(potionButton);
        potionOpenCloseUI.SetCloseButton(upgradeButton);

        //Upgrade
        upgradesUI = new AlchemyUpgradeUI[upgrades.Length];
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i] = new AlchemyUpgradeUI(upgrades[i], i);
        }
        upgradeOpenCloseUI = new OpenCloseUI(upgradeCanvas, false, false, true);
        upgradeOpenCloseUI.SetOpenButton(upgradeButton);
        upgradeOpenCloseUI.SetCloseButton(potionButton);
        potionButton.onClick.Invoke();

        //Craft
        alchemyOpenCloseUI = new OpenCloseUI(alchemyCanvas, false, false, true);
        craftOpenCloseUI = new OpenCloseUI(craftCanvas, false, false, true);
        alchemyOpenCloseUI.SetOpenButton(alchemyOpenButton);
        craftOpenCloseUI.SetOpenButton(craftOpenButton);
        alchemyOpenCloseUI.SetCloseButton(craftOpenButton);
        craftOpenCloseUI.SetCloseButton(alchemyOpenButton);
        alchemyOpenButton.onClick.Invoke();
        craftSwitchTabUI = new SwitchTabUI(craftSelectButtons, true);
        craftScrollsUI = new CraftScrollUI[craftScrolls.Length];
        for (int i = 0; i < craftScrollsUI.Length; i++)
        {
            int count = i;
            craftScrollsUI[i] = new CraftScrollUI(this, craftScrolls[count], count);
        }
        craftSwitchTabUI.openAction = () =>
        {
            for (int i = 0; i < craftScrollsUI.Length; i++)
            {
                craftScrollsUI[i].SetUI();
            }
        };
        craftSelectButtons[0].onClick.Invoke();
        

        //Popup
        mysteriousWaterPopupUI = new MysteriousWaterPopupUI(gameUI.popupCtrlUI.stats);
        mysteriousWaterPopupUI.SetTargetObject(mysteriousWaterSlider.gameObject, ()=> mysteriousWaterPopupUI.SetUI());
        catalystPopupUI = new CatalystPopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < catalystsUI.Length; i++)
        {
            int count = i;
            catalystPopupUI.SetTargetObject(catalystsUI[count].thisObject, () => catalystPopupUI.SetUI(() => catalystsUI[count].catalyst));
        }
        alchemyPotionPopupUI = new AlchemyPotionPopupUI(this, gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < alchemyPotionsUI.Length; i++)
        {
            int count = i;
            alchemyPotionPopupUI.SetTargetObject(alchemyPotionsUI[count].thisObject, () => alchemyPotionPopupUI.SetUI(() => alchemyPotionsUI[count].globalInfo));
        }
        alchemyUpgradePopupUI = new AlchemyUpgradePopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            alchemyUpgradePopupUI.SetTargetObject(upgrades[count], () => alchemyUpgradePopupUI.ShowAction(upgradesUI[count].thisUpgrade));
        }
        craftScrollPopupUI = new CraftScrollPopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < craftScrollsUI.Length; i++)
        {
            int count = i;
            craftScrollPopupUI.SetTargetObject(craftScrolls[count], () => craftScrollPopupUI.SetUI(() => craftScrollsUI[count].CraftScroll()));
            craftScrollPopupUI.SetTargetObject(craftScrollsUI[count].backgroundImage.gameObject, () => craftScrollPopupUI.SetUI(() => craftScrollsUI[count].CraftScroll()));
            craftScrollPopupUI.SetTargetObject(craftScrollsUI[count].iconImage.gameObject, () => craftScrollPopupUI.SetUI(() => craftScrollsUI[count].CraftScroll()));
        }
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Red).pet, petQoLIcon, popupUI, "Auto-Expand");
        petQoLUI2 = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Blue).pet, petQoLIcon2, popupUI, "Auto-Disassemble");
    }
    public override void SetOpenClose()
    {
        base.SetOpenClose();
        openClose.openActions.Add(game.inventoryCtrl.UpdateCanCreatePotion);
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        materialTableUI.UpdateUI();
        essenceTableUI.UpdateUI();

        //Alchemy
        if (alchemyOpenCloseUI.isOpen)
        {
            mysteriousWaterExpandButton.interactable = game.alchemyCtrl.CanExpandMysteriousWater();
            SetActive(expandIconI, !SettingMenuUI.Toggle(ToggleKind.DisableNotificationLab).isOn && game.alchemyCtrl.CanExpandMysteriousWater());
            alchemyPointText.text = optStr + "Points left : " + tDigit(game.alchemyCtrl.alchemyPoint.value);
            mysteriousWaterNumText.text = optStr + tDigit(game.alchemyCtrl.mysteriousWater.value) + " / " + tDigit(game.alchemyCtrl.mysteriousWaterCap.Value());
            if (game.alchemyCtrl.mysteriousWater.IsMaxed())
            {
                mysteriousWaterProgressText.text = optStr + "Mysterious Water : 1.000 ( + " + tDigit(game.alchemyCtrl.AvailableWaterPerSec(), 3) + " / " + localized.Basic(BasicWord.Sec) + " )";
                mysteriousWaterSlider.value = 1;
            }
            else
            {
                mysteriousWaterProgressText.text = optStr + "Mysterious Water : " + tDigit(game.alchemyCtrl.mysteriousWaterProgress.value, 3) + " ( + " + tDigit(game.alchemyCtrl.AvailableWaterPerSec(), 3) + " / " + localized.Basic(BasicWord.Sec) + " )";
                mysteriousWaterSlider.value = game.alchemyCtrl.MysteriousWaterProgressPercent();
            }
            //Catalyst
            availableCatalystText.text = "Available " + tDigit(game.catalystCtrl.EquippedNum()) + " / " + tDigit(game.catalystCtrl.AvailableNum());
            for (int i = 0; i < catalystsUI.Length; i++)
            {
                catalystsUI[i].UpdateUI();
            }
            //Essence
            for (int i = 0; i < essencesUI.Length; i++)
            {
                essencesUI[i].UpdateUI();
            }
            //Potion
            if (alchemyOpenCloseUI.isOpen)
            {
                for (int i = 0; i < alchemyPotionsUI.Length; i++)
                {
                    alchemyPotionsUI[i].UpdateUI();
                }
            }
            //Scroll
            SetActive(scrollTrans, Input.mouseScrollDelta.y != 0);

            //Upgrade
            if (upgradeOpenCloseUI.isOpen)
            {
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i].UpdateUI();
                }
            }
            //Popup
            mysteriousWaterPopupUI.UpdateUI();
            catalystPopupUI.UpdateUI();
            alchemyPotionPopupUI.UpdateUI();
            alchemyUpgradePopupUI.UpdateUI();
            popupUI.UpdateUI();
            petQoLUI.UpdateUI();
            petQoLUI2.UpdateUI();
        }

        craftOpenButton.interactable = game.townCtrl.Building(BuildingKind.Blacksmith).Rank() > 0;
        if (craftOpenButton.interactable) craftOpenButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Craft";
        else craftOpenButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "???";
        //Craft
        if (craftOpenCloseUI.isOpen)
        {
            craftSelectButtons[2].interactable = game.craftCtrl.enchantScroll3List[0].unlock.IsUnlocked();
            craftSelectButtons[3].interactable = game.craftCtrl.enchantScroll5List[0].unlock.IsUnlocked();
            for (int i = 0; i < craftScrollsUI.Length; i++)
            {
                craftScrollsUI[i].UpdateUI();
            }
            //Popup
            craftScrollPopupUI.UpdateUI();
        }
    }
    private void Update()
    {
        //Potion
        if (alchemyOpenCloseUI.isOpen)
        {
            for (int i = 0; i < alchemyPotionsUI.Length; i++)
            {
                alchemyPotionsUI[i].Update();
            }
        }
    }
}

//Craft
public class CraftScrollUI
{
    public CraftScrollUI(LabMenuUI labMenuUI, GameObject gameObject, int id)
    {
        this.labMenuUI = labMenuUI;
        this.gameObject = gameObject;
        this.id = id;
        backgroundImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        iconImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        nameText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        leveldownButton = gameObject.transform.GetChild(4).gameObject.GetComponent<Button>();
        leveldownButton.onClick.AddListener(() => CraftScroll().level.Increase(-1));
        levelupButton = gameObject.transform.GetChild(5).gameObject.GetComponent<Button>();
        levelupButton.onClick.AddListener(() => CraftScroll().level.Increase(1));
        craftButton = gameObject.transform.GetChild(6).gameObject.GetComponent<Button>();
        craftButton.onClick.AddListener(() => CraftScroll().transaction.Buy());
    }
    public void SetUI()
    {
        iconImage.sprite = sprite.enchants[(int)CraftScroll().enchantKind];
    }
    public void UpdateUI()
    {       
        if (CraftScroll().enchantKind == EnchantKind.Nothing)
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, CraftScroll().unlock.IsUnlocked());
        craftButton.interactable = CraftScroll().transaction.CanBuy();
        SetActive(leveldownButton.gameObject, CraftScroll().enchantKind == EnchantKind.OptionAdd);
        SetActive(levelupButton.gameObject, CraftScroll().enchantKind == EnchantKind.OptionAdd);
        SetActive(levelText.gameObject, CraftScroll().enchantKind == EnchantKind.OptionAdd);
        nameText.text = CraftScroll().NameString();
        if (CraftScroll().enchantKind != EnchantKind.OptionAdd) return;
        leveldownButton.interactable = !CraftScroll().level.IsMined();
        levelupButton.interactable = !CraftScroll().level.IsMaxed();
        levelText.text = optStr + "<color=green>Lv " + tDigit(CraftScroll().level.value);
    }

    LabMenuUI labMenuUI;
    public GameObject gameObject;
    public int id;
    public Image backgroundImage;
    public Image iconImage;
    TextMeshProUGUI nameText, levelText;
    Button leveldownButton, levelupButton, craftButton;
    public CRAFT_SCROLL CraftScroll()
    {
        switch (labMenuUI.craftSwitchTabUI.currentId)
        {
            case 0:
                if (id >= game.craftCtrl.scrollList.Count) return game.craftCtrl.CraftScroll(EnchantKind.Nothing);
                return game.craftCtrl.scrollList[id];
            case 1:
                if (id >= game.craftCtrl.enchantScroll1List.Count)
                {
                    if (id >= game.craftCtrl.enchantScroll1List.Count + game.craftCtrl.enchantScroll2List.Count)
                        return game.craftCtrl.CraftScroll(EnchantKind.Nothing);
                    return game.craftCtrl.enchantScroll2List[id - game.craftCtrl.enchantScroll1List.Count];
                }
                return game.craftCtrl.enchantScroll1List[id];
            case 2:
                if (id >= game.craftCtrl.enchantScroll3List.Count)
                {
                    if (id >= game.craftCtrl.enchantScroll3List.Count + game.craftCtrl.enchantScroll4List.Count)
                        return game.craftCtrl.CraftScroll(EnchantKind.Nothing);
                    return game.craftCtrl.enchantScroll4List[id- game.craftCtrl.enchantScroll3List.Count];
                }
                return game.craftCtrl.enchantScroll3List[id];
            case 3:
                if (id >= game.craftCtrl.enchantScroll5List.Count)
                    return game.craftCtrl.CraftScroll(EnchantKind.Nothing);
                return game.craftCtrl.enchantScroll5List[id];
            default:
                return game.craftCtrl.CraftScroll(EnchantKind.Nothing);
        }
    }
}

public class CatalystUI
{
    public CatalystUI(GameObject gameObject, Catalyst catalyst)
    {
        thisObject = gameObject;
        this.catalyst = catalyst;
        iconButton = thisObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        equipObject = iconButton.gameObject.transform.GetChild(1).gameObject;
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        levelUpButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        lockObject = thisObject.transform.GetChild(3).gameObject;
        iconButton.onClick.AddListener(catalyst.Equip);
        iconButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Equip));
        levelUpButton.onClick.AddListener(() => catalyst.transaction.Buy());
        levelUpButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
        //SetUI
        iconButton.gameObject.GetComponent<Image>().sprite = sprite.catalysts[(int)catalyst.kind];
    }
    public void UpdateUI()
    {
        SetActive(lockObject, !(catalyst.unlock.IsUnlocked() && catalyst.level.value > 0));
        SetActive(equipObject, catalyst.isEquipped);
        if (catalyst.unlock.IsUnlocked())
        {
            levelText.text = optStr + "<color=green>Lv " + tDigit(catalyst.level.value);
            SetActive(levelUpButton.gameObject, true);
            levelUpButton.interactable = catalyst.transaction.CanBuy();
        }
        else
        {
            levelText.text = "";
            SetActive(levelUpButton.gameObject, false);
        }
    }
    public Catalyst catalyst;
    public GameObject thisObject;
    GameObject equipObject;
    GameObject lockObject;
    Button iconButton;
    TextMeshProUGUI levelText;
    Button levelUpButton;
}

public class EssenceProductionUI
{
    public EssenceProductionUI(GameObject gameObject, EssenceProduction production)
    {
        thisObject = gameObject;
        this.production = production;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        progressSlider = thisObject.transform.GetChild(1).gameObject.GetComponent<Slider>();
        progressText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        rateText = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        allocatedWaterText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        upButton = thisObject.transform.GetChild(5).gameObject.GetComponent<Button>();
        downButton = thisObject.transform.GetChild(6).gameObject.GetComponent<Button>();

        upButton.onClick.AddListener(() => production.AllocateWaterPerSec((long)Math.Min(TransactionCost.MultibuyNum() * 1, game.alchemyCtrl.AvailableWaterPerSec() * 10)));
        downButton.onClick.AddListener(() => production.AllocateWaterPerSec(Math.Max(TransactionCost.MultibuyNum() * -1, -production.allocatedWaterPerSec.value)));
    }
    public void UpdateUI()
    {
        if (!production.isAvailable)
        {
            SetActive(thisObject, false);
            return;
        }
        SetActive(thisObject, true);
        nameText.text = localized.EssenceName(production.kind);
        progressSlider.value = (float)production.progress.value;
        progressText.text = tDigit(production.progress.value, 3);
        rateText.text = optStr + "+ " + tDigit(production.ProductionPerSec(), 3) + " / " + localized.Basic(BasicWord.Sec);
        allocatedWaterText.text = tDigit(production.allocatedWaterPerSec.value / 10d, 1);
        upButton.interactable = production.CanAllocatedWaterPerSec(1);
        downButton.interactable = production.CanAllocatedWaterPerSec(-1);
    }
    EssenceProduction production;
    GameObject thisObject;
    TextMeshProUGUI nameText;
    Slider progressSlider;
    TextMeshProUGUI progressText, rateText, allocatedWaterText;
    Button upButton, downButton;
}

public class AlchemyPotionUI
{
    public AlchemyPotionUI(LabMenuUI labMenuUI, GameObject gameObject, int id)
    {
        this.labMenuUI = labMenuUI;
        thisObject = gameObject;
        globalInfo = game.potionCtrl.globalInformations[id];
        potionKind = globalInfo.kind;
        iconButton = thisObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        levelUpButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        queueText = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = thisObject.transform.GetChild(4).gameObject;
        iconButton.onClick.AddListener(AlchemisePotion);
        iconButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Alchemise));
        levelUpButton.onClick.AddListener(() => globalInfo.levelTransaction.Buy());
        levelUpButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));

        SetUI();

        SetEventAction();
    }

    public void AlchemisePotion()
    {
        //shift-dで即Disassemble
        if (labMenuUI.isAutoDisassemble)
        {
            long tempIncrement = globalInfo.alchemyTransaction.LevelIncrement();
            globalInfo.alchemyTransaction.Buy(false, true);
            globalInfo.Create(tempIncrement, true);
            game.resourceCtrl.gold.Increase(globalInfo.DisassembleGoldNum() * tempIncrement);
            return;
        }
        globalInfo.alchemyTransaction.Buy();
    }

    public void UpdateUI()
    {
        SetActive(lockObject, !globalInfo.isUnlocked);
        SetActive(queueText.gameObject, globalInfo.isUnlocked);
        if (globalInfo.isUnlocked)
        {
            iconButton.interactable = globalInfo.alchemyTransaction.CanBuy();
            levelText.text = optStr + "<color=green>Lv " + tDigit(globalInfo.level.value);
            SetActive(levelUpButton.gameObject, true);
            levelUpButton.interactable = globalInfo.levelTransaction.CanBuy();
            if (globalInfo.isSuperQueued) queueText.text = "<color=orange>SQ</color>";
            else if (globalInfo.queue > 0) queueText.text = "<color=orange>Q" + tDigit(globalInfo.queue) + "</color>";
            else queueText.text = "";
        }
        else
        {
            levelText.text = "";
            SetActive(levelUpButton.gameObject, false);
            return;
        }
    }
    public void Update()
    {
        if (!isHoverMouse) return;
        if (Input.GetMouseButtonDown(1))//右クリック
        {
            if (GameControllerUI.isShiftPressed) game.potionCtrl.RemoveQueue(globalInfo);
            else game.potionCtrl.AssignQueue(globalInfo);
        }
        if (game.epicStoreCtrl.Item(EpicStoreKind.SuperQueueAlchemy).IsPurchased() && Input.GetKeyDown(KeyCode.Q))
        {
            //if (GameControllerUI.isShiftPressed) game.potionCtrl.RemoveQueue(globalInfo);
            //else
            game.potionCtrl.AssignQueue(globalInfo, true);
        }
    }
    public void SetUI()
    {
        iconButton.gameObject.GetComponent<Image>().sprite = sprite.potions[(int)potionKind];
    }
    public PotionKind potionKind;
    public PotionGlobalInformation globalInfo;
    public GameObject thisObject;
    public Button iconButton;
    public TextMeshProUGUI levelText, queueText;
    public Button levelUpButton;
    public GameObject lockObject;
    LabMenuUI labMenuUI;
    bool isHoverMouse;
    void SetEventAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var enter = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        enter.callback.AddListener((data) => { isHoverMouse = true; });
        exit.callback.AddListener((data) => { isHoverMouse = false; });
        thisObject.GetComponent<EventTrigger>().triggers.Add(enter);
        thisObject.GetComponent<EventTrigger>().triggers.Add(exit);
    }
}

public class MaterialTableUI
{
    public MaterialTableUI(GameObject gameObject)
    {
        thisObject = gameObject;
        materials = new GameObject[thisObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.childCount];
        for (int i = 0; i < materials.Length; i++)
        {
            int count = i;
            materials[i] = thisObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(count).gameObject;
        }
        materialsUI = new MaterialUI[materials.Length];
        for (int i = 0; i < materialsUI.Length; i++)
        {
            int count = i;
            materialsUI[i] = new MaterialUI(materials[count], game.materialCtrl.Material((MaterialKind)count));
        }
        openCloseUI = new OpenCloseUI(thisObject, false, false, true);
    }
    public void UpdateUI()
    {
        if (!openCloseUI.isOpen) return;
        for (int i = 0; i < materialsUI.Length; i++)
        {
            materialsUI[i].UpdateUI();
        }
    }
    GameObject thisObject;
    GameObject[] materials;
    MaterialUI[] materialsUI;
    public OpenCloseUI openCloseUI;
}
public class MaterialUI
{
    public MaterialUI(GameObject gameObject, Material material)
    {
        thisObject = gameObject;
        thisMaterial = material;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        valueText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateUI()
    {
        SetActive(thisObject, thisMaterial.value > 0);
        if (thisMaterial.value <= 0) return;
        nameText.text = localized.Material(thisMaterial.kind);
        valueText.text = tDigit(thisMaterial.value);
    }
    GameObject thisObject;
    Material thisMaterial;
    TextMeshProUGUI nameText, valueText;
}

public class EssenceTableUI
{
    public EssenceTableUI(GameObject gameObject)
    {
        thisObject = gameObject;
        essences = new GameObject[thisObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.childCount];
        for (int i = 0; i < essences.Length; i++)
        {
            int count = i;
            essences[i] = thisObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(count).gameObject;
        }
        essencesUI = new EssenceUI[essences.Length];
        for (int i = 0; i < essencesUI.Length; i++)
        {
            int count = i;
            essencesUI[i] = new EssenceUI(essences[count], game.essenceCtrl.Essence((EssenceKind)count));
        }
        openCloseUI = new OpenCloseUI(thisObject, false, false, true);
    }
    public void UpdateUI()
    {
        if (!openCloseUI.isOpen) return;
        for (int i = 0; i < essencesUI.Length; i++)
        {
            essencesUI[i].UpdateUI();
        }
    }
    GameObject thisObject;
    GameObject[] essences;
    EssenceUI[] essencesUI;
    public OpenCloseUI openCloseUI;
}
public class EssenceUI
{
    public EssenceUI(GameObject gameObject, Essence essence)
    {
        thisObject = gameObject;
        this.essence = essence;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        valueText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateUI()
    {
        SetActive(thisObject, essence.value > 0);
        if (essence.value <= 0) return;
        nameText.text = localized.EssenceName(essence.kind);
        valueText.text = tDigit(essence.value);
    }
    GameObject thisObject;
    Essence essence;
    TextMeshProUGUI nameText, valueText;
}

public class AlchemyUpgradeUI
{
    public AlchemyUpgradeUI(GameObject gameObject, int id)
    {
        thisObject = gameObject;
        this.id = id;
        kind = (AlchemyUpgradeKind)id;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(PlusPoint);
        plusButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
    }
    public void UpdateUI()
    {
        plusButton.interactable = Interactable();
        nameText.text = NameString();
        levelText.text = LevelString();
    }
    string NameString()
    {
        return localized.AlchemyUpgradeName(kind);
    }
    string LevelString()
    {
        return optStr + "<color=green>Lv " + tDigit(thisUpgrade.level.value);// + "</color> / " + tDigit(thisUpgrade.level.maxValue());
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
    int id;
    AlchemyUpgradeKind kind;
    public AlchemyUpgrade thisUpgrade { get => game.alchemyCtrl.alchemyUpgrades[id]; }
    TextMeshProUGUI nameText, levelText;
    Button plusButton;

}

//Popup
public class MysteriousWaterPopupUI : PopupUI
{
    TextMeshProUGUI description, valueText;
    public MysteriousWaterPopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
        description = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        valueText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI()
    {
        UpdateText();
        DelayShow();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        base.UpdateUI();
        UpdateText();
    }
    void UpdateText()
    {
        description.text = DescriptionString();
        valueText.text = ValueString();
    }
    string DescriptionString()
    {
        string tempStr = optStr + "<size=20>Mysterious Water";
        tempStr += "\n<size=18>- Current Cap : " + tDigit(game.alchemyCtrl.mysteriousWaterCap.Value());
        tempStr += "\n- Max Cap : " + tDigit(game.alchemyCtrl.mysteriousWaterExpandedCapNum.maxValue());
        tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Additive) + "</u><size=18>";
        //Base
        tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Add((MultiplierKind)i) != 0)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i);
        }
        tempStr += optStr + "\n" + localized.Basic(BasicWord.Additive) + " " + localized.Basic(BasicWord.Total) + " : ";
        //Mul
        tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Multiplicative) + "</u><size=18>";
        tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul((MultiplierKind)i) != 1)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i);
        }
        tempStr += "\n" + localized.Basic(BasicWord.Multiplicative) + " " + localized.Basic(BasicWord.Total) + " : ";
        tempStr += optStr + "<size=20>\n\n" + localized.Basic(BasicWord.Total) + " : ";
        return tempStr;
    }
    string ValueString()
    {
        string tempStr = optStr + "<size=20> ";
        tempStr += "\n<size=18>";
        tempStr += "\n";
        tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
        //Base
        tempStr += optStr + "\n" + tDigit(game.alchemyCtrl.mysteriousWaterProductionPerSec.Add(MultiplierKind.Base), 3);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Add((MultiplierKind)i) > 0)
                tempStr += optStr + "\n+ " + tDigit(game.alchemyCtrl.mysteriousWaterProductionPerSec.Add((MultiplierKind)i), 3);
            else if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Add((MultiplierKind)i) < 0)
                tempStr += optStr + "\n<color=yellow> " + tDigit(game.alchemyCtrl.mysteriousWaterProductionPerSec.Add((MultiplierKind)i), 3) + "</color>";
        }
        tempStr += "\n" + tDigit(game.alchemyCtrl.mysteriousWaterProductionPerSec.Add(), 3);
        //Mul
        tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
        //Base
        tempStr += optStr + "\n" + percent(game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul(MultiplierKind.Base), 3);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul((MultiplierKind)i) > 1)
                tempStr += optStr + "\n* " + percent(game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul((MultiplierKind)i), 3);
            else if (game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul((MultiplierKind)i) < 1)
                tempStr += optStr + "\n<color=yellow>* " + percent(game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul((MultiplierKind)i), 3) + "</color>";
        }
        tempStr += "\n" + percent(game.alchemyCtrl.mysteriousWaterProductionPerSec.Mul(), 3);
        tempStr += optStr + "<size=20>\n\n" + tDigit(game.alchemyCtrl.mysteriousWaterProductionPerSec.Value(), 3) + " / " + localized.Basic(BasicWord.Sec);
        return tempStr;
    }
}

public class CatalystPopupUI : PopupUI
{
    public Func<Catalyst> catalyst;
    public Image icon;
    public TextMeshProUGUI nameText, description;
    public CatalystPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    //こっから
    public void SetUI(Func<Catalyst> catalyst)
    {
        this.catalyst = catalyst;
        if (!catalyst().unlock.IsUnlocked()) return;
        if ((catalyst().unlock.IsUnlocked() && catalyst().level.value > 0)) icon.sprite = sprite.catalysts[(int)catalyst().kind];
        else icon.sprite = sprite.questionmarkSlot;
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        nameText.text = NameString();
        description.text = DescriptionString();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        return localized.Catalyst(catalyst()).name;
    }
    string DescriptionString()
    {
        return localized.Catalyst(catalyst()).description;
    }
}

public class AlchemyPotionPopupUI : PopupUI
{
    public Func<PotionGlobalInformation> thisGlobalInfo;
    public Image icon;
    public TextMeshProUGUI nameText, description;
    LabMenuUI labMenuUI;
    public AlchemyPotionPopupUI(LabMenuUI labMenuUI, GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        this.labMenuUI = labMenuUI;
    }
    //こっから
    public void SetUI(Func<PotionGlobalInformation> thisGlobalInfo)
    {
        this.thisGlobalInfo = thisGlobalInfo;
        if (thisGlobalInfo().kind == PotionKind.Nothing) return;
        if (!thisGlobalInfo().isUnlocked) return;
        icon.sprite = sprite.potions[(int)thisGlobalInfo().kind];
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        nameText.text = NameString();
        description.text = DescriptionString();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.PotionName(thisGlobalInfo().kind);
        if (thisGlobalInfo().queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisGlobalInfo().queue) + "</color>";
        if (thisGlobalInfo().isSuperQueued) tempString += optStr + "  <color=orange>SQ</color>";
        tempString += optStr + "<size=18>\n<color=green>Lv " + tDigit(thisGlobalInfo().level.value) + "</color> / " + tDigit(thisGlobalInfo().level.maxValue());
        tempString += optStr + "\n<size=18>" + "Type : " + thisGlobalInfo().type.ToString();
        tempString += optStr + "\nProduced # " + tDigit(thisGlobalInfo().productedNum.value);
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.PotionConsume(thisGlobalInfo().consumeKind, thisGlobalInfo().consumeChance);
        if (thisGlobalInfo().type != PotionType.Health) tempString += "\n- Remains active while equipped in potion slot";
        if (thisGlobalInfo().cooltime > 0) tempString += "\n- Cool Time : " + tDigit(thisGlobalInfo().cooltime) + " " + localized.Basic(BasicWord.Sec);
        tempString += optStr + "\n- You gain " + tDigit(thisGlobalInfo().alchemyPointGain) + " Alchemy Points every crafting";
        if (thisGlobalInfo().isEffectedByLowerTier) tempString += "\n<color=green>- This effect is boosted by the lower tier potion</color>";

        tempString += "\n\n";
        //Effect
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Current) + " : ";
        tempString += optStr + localized.PotionEffect(thisGlobalInfo().kind, thisGlobalInfo().effectValue);
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Next) + " : ";
        tempString += optStr + localized.PotionEffect(thisGlobalInfo().kind,  thisGlobalInfo().nextEffectValue);
        tempString += optStr + "  ( <color=green>Lv " + tDigit(thisGlobalInfo().levelTransaction.ToLevel()) + "</color> ) " + "</u><size=18>";
        tempString += "\n\n";
        //CostToProduce
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.ProductionCost);
        tempString += optStr + "  ( <color=green>+ " + tDigit(thisGlobalInfo().alchemyTransaction.LevelIncrement()) + "</color> ) " + "</u><size=18>";
        if (labMenuUI.isAutoDisassemble)
            tempString += "<color=orange> Disassemble</color>";
        else if (!game.inventoryCtrl.CanCreatePotion(thisGlobalInfo().kind, 1))
            tempString += "<color=yellow> You need an empty Utility slot</color>";
        //tempString += optStr + "\n- " + "Mysterious Water" + " : ";
        //if (thisGlobalInfo().alchemyTransaction.CanBuy(0)) tempString += "<color=green>";
        //else tempString += "<color=red>";
        //tempString += optStr + tDigit(thisGlobalInfo().alchemyTransaction.ResourceValue(0)) + "</color> / ";
        //tempString += optStr + tDigit(thisGlobalInfo().alchemyTransaction.Cost(0));
        int count = 0;
        foreach (var item in thisGlobalInfo().alchemyCosts)
        {
            tempString += optStr + "\n- " + localized.EssenceName(item.Key) + " : ";
            if (thisGlobalInfo().alchemyTransaction.CanBuy(count)) tempString += "<color=green>";
            else tempString += "<color=red>";
            tempString += optStr + tDigit(thisGlobalInfo().alchemyTransaction.ResourceValue(count)) + "</color> / ";
            tempString += optStr + tDigit(thisGlobalInfo().alchemyTransaction.Cost(count));
            count++;
        }
        tempString += "\n\n";
        //CostToLevelUp
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.LevelupCost)+ "</u><size=18>";
        if (thisGlobalInfo().level.IsMaxed()) tempString += optStr + "\n- Level Maxed";
        else
        {
            tempString += optStr + "\n- " + tDigit(thisGlobalInfo().levelTransaction.Cost()) + " Points";
            tempString += optStr + "  ( <color=green>Lv " + tDigit(thisGlobalInfo().levelTransaction.ToLevel()) + "</color> ) ";
        }
        if (game.potionCtrl.availableQueue.Value() <= 0) return tempString;
        tempString += "\n\n";
        tempString += "<color=orange>Available Queue : " + tDigit(game.potionCtrl.CurrentAvailableQueue()) + " / " + tDigit(game.potionCtrl.availableQueue.Value()); 
        return tempString;
    }
}

public class AlchemyUpgradePopupUI : PopupUI
{
    public AlchemyUpgradePopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public void ShowAction(AlchemyUpgrade upgrade)
    {
        SetUI(() => DescriptionString(upgrade));
    }
    string DescriptionString(AlchemyUpgrade upgrade)
    {
        string tempString = optStr + "<size=20>" + localized.AlchemyUpgradeName(upgrade.kind) + " < <color=green>Lv " + tDigit(upgrade.level.value) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- Max Level : Lv " + tDigit(upgrade.level.maxValue());
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Current) + " : " + localized.AlchemyUpgradeEffect(upgrade.kind, upgrade.effectValue);
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Next) + " : " + localized.AlchemyUpgradeEffect(upgrade.kind, upgrade.nextEffectValue) + " ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(upgrade.transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        return tempString;
    }
}

public class CraftScrollPopupUI : PopupUI
{
    Func<CRAFT_SCROLL> craftScroll;
    Image icon;
    TextMeshProUGUI nameText, description;
    public CraftScrollPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(Func<CRAFT_SCROLL> craftScroll)
    {
        this.craftScroll = craftScroll;
        if (craftScroll().enchantKind == EnchantKind.Nothing) return;
        icon.sprite = sprite.enchants[(int)craftScroll().enchantKind];
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        nameText.text = NameString();
        description.text = DescriptionString();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.EnchantName(craftScroll().enchantKind);
        if (craftScroll().enchantKind == EnchantKind.OptionAdd)
            tempString += optStr + "<color=yellow> [ " + localized.EquipmentEffectName(craftScroll().kind) + " Lv " + tDigit(craftScroll().level.value) + " ]</color>";
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += "\n- " + localized.EnchantInformation(craftScroll().enchantKind);
        tempString += "\n- " + localized.Basic(BasicWord.SuccessChance) + " : 100%";//Debug
        if (craftScroll().enchantKind == EnchantKind.OptionAdd)
        {
            tempString += "\n\n";
            tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.AdditionalEffect) + "</u><size=18>";
            tempString += optStr + "\n- " + localized.EquipmentEffect(craftScroll().kind, EquipmentParameter.EffectCalculation(craftScroll().kind, craftScroll().level.value));
            tempString += optStr + " ~ " + localized.EquipmentEffect(craftScroll().kind, EquipmentParameter.EffectCalculation(craftScroll().kind, craftScroll().level.value + 1));
            tempString += "\n\n";
            tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.RequiredAbilityIncrement) + "</u><size=18>";
            tempString += "\n- " + localized.Basic(BasicWord.HeroLevel) + " + " + EquipmentParameter.RequiredLevelIncrement(craftScroll().kind, craftScroll().level.value);
        }
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost);
        //tempString += optStr + "  ( <color=green>+ " em+ tDigit(craftScroll().transaction.LevelIncrement()) + "</color> ) " + "</u><size=18>";
        tempString += "</u><size=18>";
        tempString += "\n" + craftScroll().CostString();
        return tempString;
    }
}