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

public class EquipMenuUI : MENU_UI
{
    public int moveFromId = -1;//eq用-1の時は何もつかんでいない状態
    public int moveFromIdEnchant = -1;//enchant用
    public int moveFromIdPotion = -1;//potion
    [SerializeField] GameObject[] petQoLIcon;
    PetQoLUI[] petQoLUI;
    [SerializeField] GameObject moveIcon;
    RectTransform moveIconRect;
    Image moveIconImage;
    [SerializeField] GameObject[] weaponSlots;
    [SerializeField] GameObject[] armorSlots;
    [SerializeField] GameObject[] jewelrySlots;
    [SerializeField] GameObject[] equipInventorySlots;
    EquipmentSlotUI[] weaponSlotsUI;
    EquipmentSlotUI[] armorSlotsUI;
    EquipmentSlotUI[] jewelrySlotsUI;
    EquipmentSlotUI[] equipInventorySlotsUI;
    EquipmentSlotUI[] equipmentSlotsUI;//上４つ合わせたもの
    [SerializeField] Button[] equipSortButtons;
    [SerializeField] Button[] equipPageButtons;
    SwitchTabUI equipSwitchTabUI;
    [SerializeField] Button[] heroButtons;
    SwitchTabUI heroSwitchTabUI;
    [SerializeField] Button[] equipInventoryPageButtons;
    SwitchTabUI equipInventorySiwtchTabUI;
    [SerializeField] Button[] enchantInventoryPageButtons;
    SwitchTabUI enchantInventorySwitchTabUI;
    [SerializeField] Button[] utilityInventoryPageButtons;
    [SerializeField] Button utilitySortButton;
    SwitchTabUI utilityInventorySwitchTabUI;

    [SerializeField] GameObject[] enchantSlots;
    EnchantSlotUI[] enchantSlotsUI;
    [SerializeField] GameObject[] potionEquipSlots;
    [SerializeField] GameObject[] potionInventorySlots;
    PotionSlotUI[] potionEquipSlotsUI;
    PotionSlotUI[] potionInventorySlotsUI;
    PotionSlotUI[] potionSlotsUI;//上２つを合わせたもの
    //Disassemble
    [SerializeField] GameObject disassembleObject;
    DisassembleUI disassembleUI;
    //Debug
    //public GameObject trashBox;
    //TrashBoxUI trashBoxUI;
    //Popup
    public EquipPopupUI equipPopupUI;
    public EnchantPopupUI enchantPopupUI;
    public PotionPopupUI potionPopupUI;
    public PopupUI popupUI;
    //Confirm
    public EnchantConfirmUI enchantConfirmUI;

    public EquipmentDictionaryUI dictionaryUI;
    public EquipmentTalismanUI talismanUI;
    [SerializeField] GameObject equipmentInventoryCanvasObject;
    public OpenCloseUI dictionaryOpenCloseUI;

    public static HeroKind heroKind;// { get => game.currentHero; }//変更できるようにする

    public void SetMoveIcon(Sprite iconSprite)//掴むとき
    {
        setActive(moveIcon);
        moveIconImage.sprite = iconSprite;
        gameUI.soundUI.Play(SoundEffect.Remove);
    }
    public void InitializeUI()//放す時
    {
        moveFromId = -1;
        moveFromIdEnchant = -1;
        moveFromIdPotion = -1;
        moveIconRect.anchoredPosition = Parameter.hidePosition;
        setFalse(moveIcon);
        SetUI();
    }
    public void SetUI()
    {
        isPurchasedEquipheroAccess = game.epicStoreCtrl.Item(EpicStoreKind.EquipHeroAccess).IsPurchased();
        for (int i = 0; i < equipmentSlotsUI.Length; i++)
        {
            equipmentSlotsUI[i].SetUI();
        }
        for (int i = 0; i < enchantSlotsUI.Length; i++)
        {
            enchantSlotsUI[i].SetUI();
        }
        for (int i = 0; i < potionSlotsUI.Length; i++)
        {
            potionSlotsUI[i].SetUI();
        }
    }

    private void Awake()
    {
        moveIconRect = moveIcon.GetComponent<RectTransform>();
        moveIconImage = moveIcon.GetComponent<Image>();

        dictionaryOpenCloseUI = new OpenCloseUI(equipmentInventoryCanvasObject, false, false, false, true);
        dictionaryOpenCloseUI.SetOpenButton(dictionaryUI.closeButton);
        dictionaryOpenCloseUI.SetCloseButton(dictionaryUI.openButton);
        dictionaryOpenCloseUI.SetOpenButton(talismanUI.closeButton);
        dictionaryOpenCloseUI.SetCloseButton(talismanUI.openButton);
    }


    void SwitchHero()
    {
        heroKind = (HeroKind)heroSwitchTabUI.currentId;
        for (int i = 0; i < heroButtons.Length; i++)
        {
            if (i == heroSwitchTabUI.currentId)
                heroButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            else
                heroButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = halftrans;
        }
        SetUI();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Equipment
        weaponSlotsUI = new EquipmentSlotUI[weaponSlots.Length];
        armorSlotsUI = new EquipmentSlotUI[armorSlots.Length];
        jewelrySlotsUI = new EquipmentSlotUI[jewelrySlots.Length];
        equipInventorySlotsUI = new EquipmentSlotUI[equipInventorySlots.Length];
        equipInventorySiwtchTabUI = new SwitchTabUI(equipInventoryPageButtons, true);
        equipSwitchTabUI = new SwitchTabUI(equipPageButtons, true);
        heroSwitchTabUI = new SwitchTabUI(heroButtons);
        heroSwitchTabUI.openAction = SwitchHero;

        utilityInventorySwitchTabUI = new SwitchTabUI(utilityInventoryPageButtons, true);
        enchantInventorySwitchTabUI = new SwitchTabUI(enchantInventoryPageButtons, true);
        for (int i = 0; i < weaponSlotsUI.Length; i++)
        {
            int count = i;
            weaponSlotsUI[i] = new EquipmentSlotUI(this, weaponSlots[count], () => equipInventorySlotId + Enum.GetNames(typeof(EquipmentPart)).Length * equipPartSlotId * (int)heroKind + count + equipSwitchTabUI.currentId * weaponSlots.Length);
        }
        for (int i = 0; i < armorSlotsUI.Length; i++)
        {
            int count = i;
            armorSlotsUI[i] = new EquipmentSlotUI(this, armorSlots[count], () => equipInventorySlotId + Enum.GetNames(typeof(EquipmentPart)).Length * equipPartSlotId * (int)heroKind + equipPartSlotId + count + equipSwitchTabUI.currentId * armorSlots.Length);
        }
        for (int i = 0; i < jewelrySlotsUI.Length; i++)
        {
            int count = i;
            jewelrySlotsUI[i] = new EquipmentSlotUI(this, jewelrySlots[count], () => equipInventorySlotId + Enum.GetNames(typeof(EquipmentPart)).Length * equipPartSlotId * (int)heroKind + equipPartSlotId * 2 + count + equipSwitchTabUI.currentId * jewelrySlots.Length);
        }
        for (int i = 0; i < equipInventorySlotsUI.Length; i++)
        {
            int count = i;
            equipInventorySlotsUI[i] = new EquipmentSlotUI(this, equipInventorySlots[count], () => equipInventorySiwtchTabUI.currentId * equipInventorySlots.Length + count);
        }
        List<EquipmentSlotUI> equipmentSlotsUIList = new List<EquipmentSlotUI>();
        equipmentSlotsUIList.AddRange(weaponSlotsUI);
        equipmentSlotsUIList.AddRange(armorSlotsUI);
        equipmentSlotsUIList.AddRange(jewelrySlotsUI);
        equipmentSlotsUIList.AddRange(equipInventorySlotsUI);
        equipmentSlotsUI = equipmentSlotsUIList.ToArray();

        //Enchant
        enchantSlotsUI = new EnchantSlotUI[enchantSlots.Length];
        for (int i = 0; i < enchantSlotsUI.Length; i++)
        {
            int count = i;
            enchantSlotsUI[i] = new EnchantSlotUI(this, enchantSlots[count], () => count + enchantInventorySwitchTabUI.currentId * enchantSlots.Length);
        }
        //Potion
        potionEquipSlotsUI = new PotionSlotUI[potionEquipSlots.Length];
        for (int i = 0; i < potionEquipSlotsUI.Length; i++)
        {
            int count = i;
            potionEquipSlotsUI[i] = new PotionSlotUI(this, potionEquipSlots[count], () => potionSlotId + equipPotionSlotId * (int)heroKind + count + equipSwitchTabUI.currentId * potionEquipSlots.Length);
        }
        potionInventorySlotsUI = new PotionSlotUI[potionInventorySlots.Length];
        for (int i = 0; i < potionInventorySlotsUI.Length; i++)
        {
            int count = i;
            potionInventorySlotsUI[i] = new PotionSlotUI(this, potionInventorySlots[count], () => count + utilityInventorySwitchTabUI.currentId * potionInventorySlots.Length);
        }
        List<PotionSlotUI> potionSlotsUIList = new List<PotionSlotUI>();
        potionSlotsUIList.AddRange(potionEquipSlotsUI);
        potionSlotsUIList.AddRange(potionInventorySlotsUI);
        potionSlotsUI = potionSlotsUIList.ToArray();

        InitializeUI();
        //SetUI
        game.inventoryCtrl.slotUIAction = SetUI;
        game.guildCtrl.Ability(GuildAbilityKind.EquipmentInventory).level.increaseUIAction = SetUI;
        game.guildCtrl.Ability(GuildAbilityKind.EnchantInventory).level.increaseUIAction = SetUI;
        game.guildCtrl.Ability(GuildAbilityKind.PotionInventory).level.increaseUIAction = SetUI;
        equipInventorySiwtchTabUI.openAction = SetUI;
        equipSwitchTabUI.openAction = SetUI;
        utilityInventorySwitchTabUI.openAction = SetUI;
        enchantInventorySwitchTabUI.openAction = SetUI;

        //Popup
        equipPopupUI = new EquipPopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < equipmentSlotsUI.Length; i++)
        {
            int count = i;
            equipPopupUI.SetTargetObject(equipmentSlotsUI[count].thisObject, () => equipPopupUI.SetUI(() => equipmentSlotsUI[count].thisEquipmentSlot().equipment, null, equipmentSlotsUI[count].thisEquipmentSlot().isUnlocked));
        }
        enchantPopupUI = new EnchantPopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < enchantSlotsUI.Length; i++)
        {
            int count = i;
            enchantPopupUI.SetTargetObject(enchantSlotsUI[count].thisObject, () => enchantPopupUI.SetUI(() => enchantSlotsUI[count].thisEnchantSlot().enchant));
        }
        potionPopupUI = new PotionPopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < potionSlotsUI.Length; i++)
        {
            int count = i;
            potionPopupUI.SetTargetObject(potionSlotsUI[count].thisObject, () => potionPopupUI.SetUI(() => potionSlotsUI[count].thisPotionSlot().potion));
        }
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(disassembleObject, DisassemblePopupString);
        for (int i = 0; i < heroButtons.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(heroButtons[count].gameObject, () => localized.Hero((HeroKind)count));
        }
        //Confirm
        enchantConfirmUI = new EnchantConfirmUI(gameUI.popupCtrlUI.enchantConfirm);

        //Disassemble
        disassembleUI = new DisassembleUI(this, disassembleObject);

        //InventoryTabButton
        isHovers = new bool[equipInventoryPageButtons.Length];
        for (int i = 0; i < equipInventoryPageButtons.Length; i++)
        {
            int count = i;
            var dragItem = new EventTrigger.Entry();
            var exit = new EventTrigger.Entry();
            dragItem.eventID = EventTriggerType.PointerEnter;
            exit.eventID = EventTriggerType.PointerExit;
            dragItem.callback.AddListener((data) => SwitchEquipInventoryTab(count));
            exit.callback.AddListener((data) => isHovers[count] = false) ;
            equipInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(dragItem);
            equipInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(exit);
        }
        isHoversEnchant = new bool[enchantInventoryPageButtons.Length];
        for (int i = 0; i < enchantInventoryPageButtons.Length; i++)
        {
            int count = i;
            var dragItem = new EventTrigger.Entry();
            var exit = new EventTrigger.Entry();
            dragItem.eventID = EventTriggerType.PointerEnter;
            exit.eventID = EventTriggerType.PointerExit;
            dragItem.callback.AddListener((data) => SwitchEnchantInventoryTab(count));
            exit.callback.AddListener((data) => isHoversEnchant[count] = false);
            enchantInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(dragItem);
            enchantInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(exit);
        }

        isHoversUtility = new bool[utilityInventoryPageButtons.Length];
        for (int i = 0; i < utilityInventoryPageButtons.Length; i++)
        {
            int count = i;
            var dragItem = new EventTrigger.Entry();
            var exit = new EventTrigger.Entry();
            dragItem.eventID = EventTriggerType.PointerEnter;
            exit.eventID = EventTriggerType.PointerExit;
            dragItem.callback.AddListener((data) => SwitchUtilityInventoryTab(count));
            exit.callback.AddListener((data) => isHoversUtility[count] = false);
            utilityInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(dragItem);
            utilityInventoryPageButtons[count].GetComponent<EventTrigger>().triggers.Add(exit);
        }

        isHoversEquip = new bool[equipPageButtons.Length];
        for (int i = 0; i < equipPageButtons.Length; i++)
        {
            int count = i;
            var dragItem = new EventTrigger.Entry();
            var exit = new EventTrigger.Entry();
            dragItem.eventID = EventTriggerType.PointerEnter;
            exit.eventID = EventTriggerType.PointerExit;
            dragItem.callback.AddListener((data) => SwitchEquipSlotTab(count));
            exit.callback.AddListener((data) => isHoversEquip[count] = false);
            equipPageButtons[count].GetComponent<EventTrigger>().triggers.Add(dragItem);
            equipPageButtons[count].GetComponent<EventTrigger>().triggers.Add(exit);
        }

        for (int i = 0; i < equipSortButtons.Length; i++)
        {
            int count = i;
            equipSortButtons[i].onClick.AddListener(() => SortEquipment((SortEquipmentType)count));
        }
        utilitySortButton.onClick.AddListener(SortUtility);

        petQoLUI = new PetQoLUI[petQoLIcon.Length];
        petQoLUI[0] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Normal).pet, petQoLIcon[0], popupUI, "Auto-Equip Utility");
        petQoLUI[1] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Fox, MonsterColor.Purple).pet, petQoLIcon[1], popupUI, "Auto-Equip");
        //Start()終わり
    }

    public override void Initialize()
    {
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon1, game.currentHero).rewardUIAction = SetUI;
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon2, game.currentHero).rewardUIAction = SetUI;
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor1, game.currentHero).rewardUIAction = SetUI;
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor2, game.currentHero).rewardUIAction = SetUI;
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry1, game.currentHero).rewardUIAction = SetUI;
        game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry2, game.currentHero).rewardUIAction = SetUI;

        equipInventoryPageButtons[0].onClick.Invoke();
        equipPageButtons[0].onClick.Invoke();
        utilityInventoryPageButtons[0].onClick.Invoke();
        heroButtons[(int)game.currentHero].onClick.Invoke();
    }

    bool[] isHovers;
    async void SwitchEquipInventoryTab(int tabId)
    {
        if (moveFromId < 0) return;
        isHovers[tabId] = true;
        int tempId = moveFromId;
        for (int i = 0; i < 3; i++)
        {
            await UniTask.DelayFrame(5, PlayerLoopTiming.FixedUpdate);
            if (moveFromId < 0 || moveFromId != tempId) return;
            if (!isHovers[tabId]) return;
        }
        equipInventoryPageButtons[tabId].onClick.Invoke();
    }
    bool[] isHoversEnchant;
    async void SwitchEnchantInventoryTab(int tabId)
    {
        if (moveFromIdEnchant < 0) return;
        isHoversEnchant[tabId] = true;
        int tempId = moveFromIdEnchant;
        for (int i = 0; i < 3; i++)
        {
            await UniTask.DelayFrame(5, PlayerLoopTiming.FixedUpdate);
            if (moveFromIdEnchant < 0 || moveFromIdEnchant != tempId) return;
            if (!isHoversEnchant[tabId]) return;
        }
        enchantInventoryPageButtons[tabId].onClick.Invoke();
    }
    bool[] isHoversUtility;
    async void SwitchUtilityInventoryTab(int tabId)
    {
        if (moveFromIdPotion < 0) return;
        isHoversUtility[tabId] = true;
        int tempId = moveFromIdPotion;
        for (int i = 0; i < 3; i++)
        {
            await UniTask.DelayFrame(5, PlayerLoopTiming.FixedUpdate);
            if (moveFromIdPotion < 0 || moveFromIdPotion != tempId) return;
            if (!isHoversUtility[tabId]) return;
        }
        utilityInventoryPageButtons[tabId].onClick.Invoke();
    }
    bool[] isHoversEquip;//装備スロット
    async void SwitchEquipSlotTab(int tabId)
    {
        if (moveFromId < 0 && moveFromIdPotion < 0) return;
        isHoversEquip[tabId] = true;
        bool isEquipment = moveFromId >= 0;
        int tempId;
        if (isEquipment) tempId = moveFromId;
        else tempId = moveFromIdPotion;
        for (int i = 0; i < 3; i++)
        {
            await UniTask.DelayFrame(5, PlayerLoopTiming.FixedUpdate);
            if (isEquipment)
            {
                if (moveFromId < 0 || moveFromId != tempId) return;
                if (!isHoversEquip[tabId]) return;
            }
            else
            {
                if (moveFromIdPotion < 0 || moveFromIdPotion != tempId) return;
                if (!isHoversEquip[tabId]) return;
            }
        }
        equipPageButtons[tabId].onClick.Invoke();
    }

    //Sort機能
    int unlockedEquipmentCount => (int)game.inventoryCtrl.equipInventoryUnlockedNum.Value();
    void SortEquipment(SortEquipmentType type)
    {
        bool isReverse = false;
        if (GameControllerUI.isShiftPressed) isReverse = true;
        int tempI = 0;
        int tempJ = 0;
        switch (type)
        {
            case SortEquipmentType.Default:
                for (int i = 0; i < unlockedEquipmentCount; i++)
                {
                    for (int j = i + 1; j < unlockedEquipmentCount; j++)
                    {
                        //Kind
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Part
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentPart)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.part;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentPart)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.part;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //SetKind
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentSetKind)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.setKind;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentSetKind)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.setKind;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Rarity
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentRarity)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.rarity;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentRarity)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.rarity;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Level
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? 10000 : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.requiredAbilities[0].requiredValue;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? 10000 : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.requiredAbilities[0].requiredValue;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                    }
                }
                break;
            case SortEquipmentType.Part:
                for (int i = 0; i < unlockedEquipmentCount; i++)
                {
                    for (int j = i + 1; j < unlockedEquipmentCount; j++)
                    {
                        //Kind
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Part
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentPart)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.part;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentPart)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.part;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                    }
                }
                break;
            case SortEquipmentType.Rarity:
                for (int i = 0; i < unlockedEquipmentCount; i++)
                {
                    for (int j = i + 1; j < unlockedEquipmentCount; j++)
                    {
                        //Kind
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Rarity
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentRarity)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.rarity;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentRarity)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.rarity;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                    }
                }
                break;
            case SortEquipmentType.Level:
                for (int i = 0; i < unlockedEquipmentCount; i++)
                {
                    for (int j = i + 1; j < unlockedEquipmentCount; j++)
                    {
                        //Kind
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind;
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? Enum.GetNames(typeof(EquipmentKind)).Length : (int)game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind;
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                        //Level
                        tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? 10000 : (int)game.inventoryCtrl.equipmentSlots[i].equipment.RequiredLevel();
                        tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? 10000 : (int)game.inventoryCtrl.equipmentSlots[j].equipment.RequiredLevel();
                        if (tempI > tempJ)
                            game.inventoryCtrl.MoveEquipment(i, j);
                    }
                }
                break;
        }
        if (isReverse)
        {
            for (int i = 0; i < unlockedEquipmentCount; i++)
            {
                for (int j = i + 1; j < unlockedEquipmentCount; j++)
                {
                    tempI = game.inventoryCtrl.equipmentSlots[i].equipment.globalInfo.kind == EquipmentKind.Nothing ? 0 : (int)game.inventoryCtrl.equipmentSlots[i].slotId;
                    tempJ = game.inventoryCtrl.equipmentSlots[j].equipment.globalInfo.kind == EquipmentKind.Nothing ? 0 : (int)game.inventoryCtrl.equipmentSlots[j].slotId;
                    if (tempJ > tempI)
                        game.inventoryCtrl.MoveEquipment(i, j);
                }
            }
        }
        SetUI();
    }
    int unlockedUtilityCount => (int)game.inventoryCtrl.potionUnlockedNum.Value();
    public void SortUtility()
    {
        bool isReverse = false;
        if (GameControllerUI.isShiftPressed) isReverse = true;
        int tempI = 0;
        int tempJ = 0;
        //まずStack#を調整するため同じKindのもの同士でMoveさせる
        for (int i = 0; i < unlockedUtilityCount; i++)
        {
            for (int j = 0; j < unlockedUtilityCount; j++)
            {
                if (i != j)//同じスロットは考えない
                {
                    //Kind
                    tempI = game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind;
                    tempJ = game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind;
                    if (tempI == tempJ)
                        game.inventoryCtrl.MovePotion(i, j);
                }
            }
        }
        //種類ごとに並べる
        for (int i = 0; i < unlockedUtilityCount; i++)
        {
            for (int j = i + 1; j < unlockedUtilityCount; j++)
            {
                //Kind
                tempI = game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind;
                tempJ = game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind;
                if (tempI > tempJ)
                    game.inventoryCtrl.MovePotion(i, j);
            }
        }
        //Stack#の小さいものを手前へ
        for (int i = 0; i < unlockedUtilityCount; i++)
        {
            for (int j = i + 1; j < unlockedUtilityCount; j++)
            {
                //Kind
                tempI = game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind;
                tempJ = game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind == PotionKind.Nothing ? Enum.GetNames(typeof(PotionKind)).Length : (int)game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind;
                if (tempI == tempJ && game.inventoryCtrl.potionSlots[i].potion.stackNum.value > game.inventoryCtrl.potionSlots[j].potion.stackNum.value)
                    game.inventoryCtrl.MovePotion(i, j);
            }
        }

        if (isReverse)
        {
            for (int i = 0; i < unlockedUtilityCount; i++)
            {
                for (int j = i + 1; j < unlockedUtilityCount; j++)
                {
                    tempI = game.inventoryCtrl.potionSlots[i].potion.globalInfo.kind == PotionKind.Nothing ? 0 : (int)game.inventoryCtrl.potionSlots[i].slotId;
                    tempJ = game.inventoryCtrl.potionSlots[j].potion.globalInfo.kind == PotionKind.Nothing ? 0 : (int)game.inventoryCtrl.potionSlots[j].slotId;
                    if (tempJ > tempI)
                        game.inventoryCtrl.MovePotion(i, j);
                }
            }
        }

        SetUI();
    }
    enum SortEquipmentType
    {
        Default,
        Part,
        Rarity,
        Level,//この装備の要求レベル
    }

    string DisassemblePopupString()
    {
        string tempStr = "<size=20>Disassembly Box<size=18>";
        tempStr += "\n- Drag and drop an item here to dissassemble.";
        tempStr += "\n\nHotkey : ";
        tempStr += "\n- <color=orange>Shift+D</color> and <color=orange>Double Click</color> on <color=orange>ITEM</color> : Disassembles a piece of equipment/utility item.";
        tempStr += "\n- <color=orange>Shift+D</color> and <color=orange>Double Click</color> on <color=orange>HERE</color> : Disassembles all Equipment in inventory except for locked items.";
        tempStr += "\n- <color=orange>L</color> on an item : Locks / Unlocks to disassemble the item.";
        tempStr += "\n- <color=orange>Shift</color> and <color=orange>Click</color> on <color=orange>Sort Button</color> : Sorts in reverse order.";
        tempStr += "\n- Drag and drop a utility item while holding <color=orange>Shift</color> : Splits its stack # by the amount of multiplier at top left.";
        return tempStr;
    }

    public override void SetOpenClose()
    {
        base.SetOpenClose();
        openClose.openActions.Add(SetUI);
        openClose.openActions.Add(game.inventoryCtrl.UpdateCanCreatePotion);
    }
    double tempPoint;
    public override void UpdateUI()
    {
        base.UpdateUI();
        tempPoint = game.equipmentCtrl.dictionaryPointLeft.value;
        dictionaryUI.openButtonText.text = tempPoint > 0 ? optStr + "Dictionary ( <color=green>" + tDigit(tempPoint) + "</color> )" : "Dictionary";
        if (dictionaryUI.openCloseUI.isOpen)
        {
            dictionaryUI.UpdateUI();
            return;
        }
        if (talismanUI.openCloseUI.isOpen)
        {
            talismanUI.UpdateUI();
            return;
        }

        if (moveFromId >= 0 || moveFromIdEnchant >= 0 || moveFromIdPotion >= 0)
        {
            float correction = gameUI.autoCanvasScaler.mouseCorrection;
            Vector3 position = Input.mousePosition * correction - gameUI.autoCanvasScaler.defaultScreenSize * 0.5f;
            moveIconRect.anchoredPosition = position;
        }

        for (int i = 0; i < equipmentSlotsUI.Length; i++)
        {
            equipmentSlotsUI[i].UpdateUI();
        }
        for (int i = 0; i < potionSlotsUI.Length; i++)
        {
            potionSlotsUI[i].UpdateUI();
        }

        disassembleUI.UpdateUI();
        equipPopupUI.UpdateUI();
        enchantPopupUI.UpdateUI();
        potionPopupUI.UpdateUI();
        popupUI.UpdateUI();
        for (int i = 0; i < petQoLUI.Length; i++)
        {
            petQoLUI[i].UpdateUI();
        }
        for (int i = 0; i < heroButtons.Length; i++)
        {
            int count = i;
            SetActive(heroButtons[i].gameObject, isPurchasedEquipheroAccess && game.guildCtrl.Member((HeroKind)count).IsUnlocked());
            //if (isPurchasedEquipheroAccess)
            //    heroButtons[i].interactable = ;
        }
    }
    bool isPurchasedEquipheroAccess;
    public void Update()
    {
        equipPopupUI.Update();
    }
}

public class DisassembleUI
{
    EquipMenuUI eqMenuUI;
    GameObject thisObject;
    TextMeshProUGUI thisText;
    public DisassembleUI(EquipMenuUI eqMenuUI, GameObject thisObject)
    {
        this.eqMenuUI = eqMenuUI;
        this.thisObject = thisObject;
        thisText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        SetEventAction();
    }
    void SetEventAction()
    {
        if (thisText.GetComponent<EventTrigger>() == null) thisText.gameObject.AddComponent<EventTrigger>();
        var drop = new EventTrigger.Entry();
        var click = new EventTrigger.Entry();
        drop.eventID = EventTriggerType.Drop;
        click.eventID = EventTriggerType.PointerClick;
        drop.callback.AddListener((data) => { DropAction(); });
        click.callback.AddListener((data) => { ClickAction(); });
        thisText.gameObject.GetComponent<EventTrigger>().triggers.Add(drop);
        thisText.GetComponent<EventTrigger>().triggers.Add(click);
    }
    void DropAction()
    {
        if (eqMenuUI.moveFromIdEnchant >= 0)
        {
            EquipmentEnchant tempEnchant = game.inventoryCtrl.enchantSlots[eqMenuUI.moveFromIdEnchant].enchant;
            game.inventoryCtrl.Disassemble(tempEnchant);
            return;
        }
        if (eqMenuUI.moveFromId >= 0)
        {
            Equipment tempEquipment = game.inventoryCtrl.equipmentSlots[eqMenuUI.moveFromId].equipment;
            game.inventoryCtrl.Disassemble(tempEquipment);
            return;
        }
        if (eqMenuUI.moveFromIdPotion >= 0)
        {
            Potion tempPotion = game.inventoryCtrl.potionSlots[eqMenuUI.moveFromIdPotion].potion;
            game.inventoryCtrl.Disassemble(tempPotion);
            return;
        }
    }
    int doubleClickCount;
    void ClickAction()
    {
        doubleClickCount++;
        if (doubleClickCount < 2)
        {
            ResetClickCount(); return;
        }
        doubleClickCount = 0;
        if (GameControllerUI.isShiftPressed && Input.GetKey(KeyCode.D))
        {
            for (int i = 0; i < game.inventoryCtrl.equipInventoryUnlockedNum.Value(); i++)
            {
                Equipment eq = game.inventoryCtrl.equipmentSlots[i].equipment;
                game.inventoryCtrl.Disassemble(eq);
            }
        }
    }
    async void ResetClickCount()
    {
        await UniTask.Delay(500, true);
        doubleClickCount = 0;
    }
    public void UpdateUI()
    {
        thisText.text = ShowString();
        
    }
    string ShowString()
    {
        string tempStr = optStr;
        if (eqMenuUI.moveFromIdEnchant >= 0)
        {
            EquipmentEnchant tempEnchant = game.inventoryCtrl.enchantSlots[eqMenuUI.moveFromIdEnchant].enchant;
            return "Disassemble to gain " + tDigit(game.resourceCtrl.gold.MultipliedValue(tempEnchant.DisassembleGoldNum())) + " Gold";
        }
        if (eqMenuUI.moveFromId >= 0)
        {
            Equipment tempEquipment = game.inventoryCtrl.equipmentSlots[eqMenuUI.moveFromId].equipment;
            return "Disassemble to gain " + tDigit(tempEquipment.DisassembleMaterialNum()) + " " +  tempEquipment.globalInfo.DisassembleMaterialKind().Name();
        }
        if (eqMenuUI.moveFromIdPotion >= 0)
        {
            Potion tempPotion = game.inventoryCtrl.potionSlots[eqMenuUI.moveFromIdPotion].potion;
            if (tempPotion.globalInfo.type == PotionType.Talisman)
            {
                if (tempPotion.globalInfo.talismanRarity == TalismanRarity.Epic)
                    return "Disassemble";
                return "Disassemble to gain " + tDigit(tempPotion.DisassembleGoldNum()) + " Fragments";
            }
            return "Disassemble to gain " + tDigit(game.resourceCtrl.gold.MultipliedValue(tempPotion.DisassembleGoldNum())) + " Gold";
        }
        return "Disassemble";
    }
}

//Debug
//public class TrashBoxUI
//{
//    EquipMenuUI eqMenuUI;
//    GameObject thisObject;
//    public TrashBoxUI(EquipMenuUI eqMenuUI, GameObject thisObject)
//    {
//        this.eqMenuUI = eqMenuUI;
//        this.thisObject = thisObject;
//        SetEventAction();
//    }
//    void SetEventAction()
//    {
//        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
//        var drop = new EventTrigger.Entry();
//        drop.eventID = EventTriggerType.Drop;
//        drop.callback.AddListener((data) => { DropAction(); });
//        thisObject.GetComponent<EventTrigger>().triggers.Add(drop);
//    }
//    void DropAction()
//    {
//        if (eqMenuUI.moveFromIdEnchant >= 0)
//        {
//            EquipmentEnchant tempEnchant = game.inventoryCtrl.enchantSlots[eqMenuUI.moveFromIdEnchant].enchant;
//            game.inventoryCtrl.Delete(tempEnchant);
//            return;
//        }
//        if (eqMenuUI.moveFromId >= 0) 
//        {
//            Equipment tempEquipment = game.inventoryCtrl.equipmentSlots[eqMenuUI.moveFromId].equipment;
//            game.inventoryCtrl.Delete(tempEquipment);
//            return;
//        }
//        if (eqMenuUI.moveFromIdPotion >= 0)
//        {
//            Potion tempPotion = game.inventoryCtrl.potionSlots[eqMenuUI.moveFromIdPotion].potion;
//            game.inventoryCtrl.Delete(tempPotion);
//            return;
//        }
//    }
//}

public class EquipmentSlotUI
{
    public EquipmentSlotUI(EquipMenuUI eqMenuUI, GameObject thisObject, Func<int> slotId)
    {
        this.eqMenuUI = eqMenuUI;
        this.thisObject = thisObject;
        this.slotId = slotId;
        thisImage = thisObject.GetComponent<Image>();
        starText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        enchantedTextObject = thisObject.transform.GetChild(1).gameObject;
        tenacityObject = thisObject.transform.GetChild(2).gameObject;
        redImageObject = thisObject.transform.GetChild(3).gameObject;
        lockObject = thisObject.transform.GetChild(4).gameObject;
        thisEquipmentSlot = () => game.inventoryCtrl.equipmentSlots[slotId()];
        SetEventAction();
    }
    EquipMenuUI eqMenuUI;
    Func<int> slotId;
    public GameObject thisObject;
    Image thisImage;
    TextMeshProUGUI starText;
    GameObject enchantedTextObject, redImageObject, tenacityObject, lockObject;
    public Func<EquipmentSlot> thisEquipmentSlot;

    string StarString()
    {        
        string tempString = optStr + "<size=18><b>";
        if (thisEquipmentSlot().equipment.globalInfo.levels[0].isMaxedThisRebirth) tempString += "<color=#00FFFF>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipmentSlot().equipment.globalInfo.levels[1].isMaxedThisRebirth) tempString += "<color=#FFFF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipmentSlot().equipment.globalInfo.levels[2].isMaxedThisRebirth) tempString += "<color=#99FF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        tempString += " \n";
        if (thisEquipmentSlot().equipment.globalInfo.levels[3].isMaxedThisRebirth) tempString += "<color=#00FFFF>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipmentSlot().equipment.globalInfo.levels[4].isMaxedThisRebirth) tempString += "<color=#FFFF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        if (thisEquipmentSlot().equipment.globalInfo.levels[5].isMaxedThisRebirth) tempString += "<color=#99FF99>*"; else tempString += "<alpha=#00>*<alpha=#99>";
        tempString += "</b>";
        //tempString += optStr + " <size=12>\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n         <color=green>Lv " + tDigit(thisEquipmentSlot().equipment.Level());
        return tempString;
    }
    void SetEventAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var down = new EventTrigger.Entry();
        var up = new EventTrigger.Entry();
        var drop = new EventTrigger.Entry();
        var click = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        drop.eventID = EventTriggerType.Drop;
        click.eventID = EventTriggerType.PointerClick;
        down.callback.AddListener((data) => { PointerDownAction(); });
        up.callback.AddListener((data) => { PointerUpAction(); });
        drop.callback.AddListener((data) => { DropAction(); });
        click.callback.AddListener((data) => { ClickAction(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(down);
        thisObject.GetComponent<EventTrigger>().triggers.Add(up);
        thisObject.GetComponent<EventTrigger>().triggers.Add(drop);
        thisObject.GetComponent<EventTrigger>().triggers.Add(click);
    }
    int doubleClickCount;
    void ClickAction()
    {
        if (thisEquipmentSlot().equipment.kind == EquipmentKind.Nothing) return;
        doubleClickCount++;
        if (doubleClickCount < 2)
        {
            ResetClickCount(); return;
        }
        doubleClickCount = 0;
        if (GameControllerUI.isShiftPressed && Input.GetKey(KeyCode.D))
        {
            game.inventoryCtrl.Disassemble(thisEquipmentSlot().equipment);
            return;
        }
        game.inventoryCtrl.MoveEquipmentShortcut(slotId(), EquipMenuUI.heroKind);
        //Popupを消す
        if (thisEquipmentSlot().equipment.kind == EquipmentKind.Nothing)
            eqMenuUI.equipPopupUI.DelayHide();
    }
    async void ResetClickCount()
    {
        await UniTask.Delay(500, true);
        doubleClickCount = 0;
    }
    void PointerDownAction()
    {
        if (thisEquipmentSlot().equipment.kind == EquipmentKind.Nothing) return;
        if (!thisEquipmentSlot().isUnlocked) return;

        eqMenuUI.moveFromId = slotId();
        eqMenuUI.SetMoveIcon(thisImage.sprite);
    }
    async void PointerUpAction()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        eqMenuUI.InitializeUI();
        gameUI.soundUI.Play(SoundEffect.Equip);
    }
    void DropAction()
    {
        if (!thisEquipmentSlot().isUnlocked) return;
        if (!thisEquipmentSlot().isEquipmentSlot && eqMenuUI.moveFromIdEnchant >= 0)//Enchant実行
        {
            EquipmentEnchant tempEnchant = game.inventoryCtrl.enchantSlots[eqMenuUI.moveFromIdEnchant].enchant;
            switch (tempEnchant.kind)
            {
                case EnchantKind.OptionAdd:
                    game.inventoryCtrl.TryEnchant(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionLevelup:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionLevelMax:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionLottery:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionDelete:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionExtract:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.OptionCopy:
                    eqMenuUI.enchantConfirmUI.SetUI(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.ExpandEnchantSlot:
                    game.inventoryCtrl.TryEnchant(tempEnchant, thisEquipmentSlot().equipment);
                    break;
                case EnchantKind.InstantProf:
                    game.inventoryCtrl.TryEnchant(tempEnchant, thisEquipmentSlot().equipment);
                    break;
            }
            return;
        }
        if (eqMenuUI.moveFromId < 0) return;
        game.inventoryCtrl.MoveEquipment(eqMenuUI.moveFromId, slotId());
        eqMenuUI.equipPopupUI.SetUI(() => thisEquipmentSlot().equipment);
    }

    public void SetUI()
    {
        if (!thisEquipmentSlot().isUnlocked)
        {
            thisImage.sprite = sprite.lockedSlot;
            return;
        }
        if (thisEquipmentSlot().equipment.kind == EquipmentKind.Nothing)
        {
            if (!thisEquipmentSlot().isEquipmentSlot)
            {
                thisImage.sprite = sprite.inventorySlot;
                return;
            }
            switch (thisEquipmentSlot().equipmentPart)
            {
                case EquipmentPart.Weapon:
                    thisImage.sprite = sprite.weaponSlot;
                    break;
                case EquipmentPart.Armor:
                    thisImage.sprite = sprite.armorSlot;
                    break;
                case EquipmentPart.Jewelry:
                    thisImage.sprite = sprite.jewelrySlot;
                    break;
            }
            return;
        }
        thisImage.sprite = sprite.equipments[(int)thisEquipmentSlot().equipment.kind];

        isUnlockedAndEquipment = thisEquipmentSlot().equipment.kind != EquipmentKind.Nothing && thisEquipmentSlot().isUnlocked;
        if (SettingMenuUI.Toggle(ToggleKind.ShowEquipmentStar).isOn && isUnlockedAndEquipment) thisImage.color = iconStaredColor;
        else thisImage.color = Color.white;
    }
    static Color iconStaredColor = new Color(200f / 255, 200f / 255, 200f / 255);
    bool isUnlockedAndEquipment;
    public void UpdateUI()
    {
        isUnlockedAndEquipment = thisEquipmentSlot().equipment.kind != EquipmentKind.Nothing && thisEquipmentSlot().isUnlocked;
        SetActive(redImageObject, !thisEquipmentSlot().isAvailableEQ || (thisEquipmentSlot().isUnlocked && isUnlockedAndEquipment && !thisEquipmentSlot().equipment.CanEquip(EquipMenuUI.heroKind)));
        SetActive(enchantedTextObject, thisEquipmentSlot().equipment.HasEnchantedEffect() && isUnlockedAndEquipment);
        SetActive(starText.gameObject, SettingMenuUI.Toggle(ToggleKind.ShowEquipmentStar).isOn && isUnlockedAndEquipment);
        SetActive(tenacityObject, thisEquipmentSlot().equipment.EQAbusePercent(EquipMenuUI.heroKind) < 1 && isUnlockedAndEquipment);
        SetActive(lockObject, thisEquipmentSlot().equipment.isLocked && isUnlockedAndEquipment);
        if (SettingMenuUI.Toggle(ToggleKind.ShowEquipmentStar).isOn && isUnlockedAndEquipment) starText.text = StarString();
    }
}
//Enchant
public class EnchantSlotUI
{
    public EnchantSlotUI(EquipMenuUI eqMenuUI, GameObject thisObject, Func<int> slotId)
    {
        this.eqMenuUI = eqMenuUI;
        this.thisObject = thisObject;
        this.slotId = slotId;
        thisImage = thisObject.GetComponent<Image>();
        thisEnchantSlot = () => game.inventoryCtrl.enchantSlots[slotId()];
        SetEventAction();
    }
    EquipMenuUI eqMenuUI;
    Func<int> slotId;
    public GameObject thisObject;
    Image thisImage;
    public Func<EnchantSlot> thisEnchantSlot;
    void SetEventAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var down = new EventTrigger.Entry();
        var up = new EventTrigger.Entry();
        var drop = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        drop.eventID = EventTriggerType.Drop;
        down.callback.AddListener((data) => { PointerDownAction(); });
        up.callback.AddListener((data) => { PointerUpAction(); });
        drop.callback.AddListener((data) => { DropAction(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(down);
        thisObject.GetComponent<EventTrigger>().triggers.Add(up);
        thisObject.GetComponent<EventTrigger>().triggers.Add(drop);
    }
    void PointerDownAction()
    {
        if (thisEnchantSlot().enchant.kind == EnchantKind.Nothing) return;
        if (!thisEnchantSlot().isUnlocked) return;

        eqMenuUI.moveFromIdEnchant = slotId();
        eqMenuUI.SetMoveIcon(thisImage.sprite);
    }
    async void PointerUpAction()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        eqMenuUI.InitializeUI();
        gameUI.soundUI.Play(SoundEffect.Equip);
    }
    void DropAction()
    {
        if (!thisEnchantSlot().isUnlocked) return;

        if (eqMenuUI.moveFromIdEnchant < 0) return;
        game.inventoryCtrl.MoveEnchant(eqMenuUI.moveFromIdEnchant, slotId());
        eqMenuUI.enchantPopupUI.SetUI(() => thisEnchantSlot().enchant);
    }
    public void SetUI()
    {
        if (!thisEnchantSlot().isUnlocked)
        {
            thisImage.sprite = sprite.lockedSlot;
            return;
        }
        if (thisEnchantSlot().enchant.kind == EnchantKind.Nothing)
        {
            thisImage.sprite = sprite.inventorySlot;
            return;
        }
        thisImage.sprite = sprite.enchants[(int)thisEnchantSlot().enchant.kind];
    }
}
//Potion
public class PotionSlotUI
{
    public PotionSlotUI(EquipMenuUI eqMenuUI, GameObject thisObject, Func<int> slotId)
    {
        this.eqMenuUI = eqMenuUI;
        this.thisObject = thisObject;
        this.slotId = slotId;
        thisImage = thisObject.GetComponent<Image>();
        cooltimeImage = thisObject.transform.GetChild(2).gameObject.GetComponent<Image>();
        stackNumText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        thisPotionSlot = () => game.inventoryCtrl.potionSlots[slotId()];
        SetEventAction();
        if (!thisPotionSlot().isEquipSlot) SetActive(cooltimeImage.gameObject, false);
    }
    EquipMenuUI eqMenuUI;
    Func<int> slotId;
    public GameObject thisObject;
    Image thisImage;
    Image cooltimeImage;
    TextMeshProUGUI stackNumText;
    public Func<PotionSlot> thisPotionSlot;
    void SetEventAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var down = new EventTrigger.Entry();
        var up = new EventTrigger.Entry();
        var drop = new EventTrigger.Entry();
        var click = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        drop.eventID = EventTriggerType.Drop;
        click.eventID = EventTriggerType.PointerClick;
        down.callback.AddListener((data) => { PointerDownAction(); });
        up.callback.AddListener((data) => { PointerUpAction(); });
        drop.callback.AddListener((data) => { DropAction(); });
        click.callback.AddListener((data) => { ClickAction(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(down);
        thisObject.GetComponent<EventTrigger>().triggers.Add(up);
        thisObject.GetComponent<EventTrigger>().triggers.Add(drop);
        thisObject.GetComponent<EventTrigger>().triggers.Add(click);
    }
    void PointerDownAction()
    {
        if (thisPotionSlot().potion.kind == PotionKind.Nothing) return;
        if (!thisPotionSlot().isUnlocked) return;

        eqMenuUI.moveFromIdPotion = slotId();
        eqMenuUI.SetMoveIcon(thisImage.sprite);
    }
    async void PointerUpAction()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        eqMenuUI.InitializeUI();
        gameUI.soundUI.Play(SoundEffect.Equip);
    }
    int doubleClickCount;
    void ClickAction()
    {
        if (thisPotionSlot().potion.kind == PotionKind.Nothing) return;
        doubleClickCount++;
        if (doubleClickCount < 2)
        {
            ResetClickCount(); return;
        }
        doubleClickCount = 0;
        if (GameControllerUI.isShiftPressed && Input.GetKey(KeyCode.D))
        {
            game.inventoryCtrl.Disassemble(thisPotionSlot().potion);
            return;
        }
        game.inventoryCtrl.MovePotionShortcut(slotId(), EquipMenuUI.heroKind);
        //Popupを消す
        if (thisPotionSlot().potion.kind == PotionKind.Nothing)
            eqMenuUI.potionPopupUI.DelayHide();
    }
    async void ResetClickCount()
    {
        await UniTask.Delay(500, true);
        doubleClickCount = 0;
    }
    void DropAction()
    {
        if (!thisPotionSlot().isUnlocked) return;
        if (eqMenuUI.moveFromIdPotion < 0) return;
        if (eqMenuUI.moveFromIdPotion == slotId()) return;
        if (isShiftPressed && !thisPotionSlot().isEquipSlot)//EquipSlotではSpilitできない
            game.inventoryCtrl.MovePotion(eqMenuUI.moveFromIdPotion, slotId(), TransactionCost.MultibuyNum());
        else
            game.inventoryCtrl.MovePotion(eqMenuUI.moveFromIdPotion, slotId());
        eqMenuUI.potionPopupUI.SetUI(() => thisPotionSlot().potion);
    }
    public void SetUI()
    {
        stackNumText.text = "";
        if (!thisPotionSlot().isUnlocked)
        {
            thisImage.sprite = sprite.lockedSlot;
            return;
        }
        if (thisPotionSlot().potion.kind == PotionKind.Nothing)
        {
            if (thisPotionSlot().isEquipSlot)
            {
                thisImage.sprite = sprite.potionSlot;
                return;
            }
            thisImage.sprite = sprite.inventorySlot;
            return;
        }
        thisImage.sprite = sprite.potions[(int)thisPotionSlot().potion.kind];
        stackNumText.text = optStr + "<color=green>" + tDigit(thisPotionSlot().potion.stackNum.value);
    }
    public void UpdateUI()
    {
        //if (thisPotionSlot().potion.globalInfo.cooltime == 0) return;
        //if (thisPotionSlot().potion.globalInfo.TimeLeftPercent(eqMenuUI.heroKind) <= 0) return;
        if (thisPotionSlot().isEquipSlot) cooltimeImage.fillAmount = thisPotionSlot().potion.globalInfo.TimeLeftPercent(EquipMenuUI.heroKind);
    }
}




//Popup
public class EquipPopupUI : PopupUI
{
    public Func<Equipment> thisEquipment;
    Image icon;
    TextMeshProUGUI nameText, description;
    public virtual bool isDicitonary => false;
    public EquipPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(Func<Equipment> thisEquipment, Func<bool> isGotOnce = null, bool isUnlockedSlot = true)
    {
        if (isGotOnce != null && !isGotOnce()) return;
        this.thisEquipment = thisEquipment;
        if (thisEquipment().kind == EquipmentKind.Nothing) return;
        if (!isUnlockedSlot) return;
        //if (!thisEquipmentSlot().isUnlocked) return;
        icon.sprite = sprite.equipments[(int)thisEquipment().kind];
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        nameText.text = NameString();
        description.text = DescriptionString();
    }
    public override void Update()
    {
        if (!isShow) return;
        if (Input.GetKeyDown(KeyCode.L))
        {
            thisEquipment().Lock();
        }
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        SetCorner();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.EquipmentName(thisEquipment().kind);
        tempString += optStr + " < <color=green>Lv " + tDigit(thisEquipment().Level()) + "</color> ><color=yellow>";
        for (int i = 0; i < thisEquipment().totalOptionNum.Value(); i++)
        {
            if (thisEquipment().optionEffects[i].kind == EquipmentEffectKind.Nothing) tempString += optStr + " [ " + localized.EquipmentEffectName(thisEquipment().optionEffects[i].kind) + " ]";
            else tempString += optStr + " [ " + localized.EquipmentEffectName(thisEquipment().optionEffects[i].kind) + " Lv " + tDigit(thisEquipment().optionEffects[i].level.value) + " ]";
        }
        return tempString;
    }
    public virtual string DescriptionString()
    {
        double effectMultiplier = thisEquipment().EffectMultiplier(EquipMenuUI.heroKind);
        //Info
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += "\n- " + localized.Basic(BasicWord.Part) + " : " + thisEquipment().globalInfo.part.ToString();
        tempString += "\n- " + localized.Basic(BasicWord.Rarity) + " : " + thisEquipment().globalInfo.rarity.ToString();
        if (thisEquipment().isSetItem)
        {
            tempString += "\n- Unique : " + thisEquipment().globalInfo.setKind.ToString() + " Set";
            tempString += " <color=green>[ Effect Bonus + " + percent(effectMultiplier - 1d, 0) + " ]</color> ";
            tempString += " Equipping : <color=green>" + tDigit(game.inventoryCtrl.SetItemEquippedNum(thisEquipment().globalInfo.setKind, EquipMenuUI.heroKind))
                + "</color> / 8";
        }
        if (!isDicitonary && thisEquipment().isLocked) tempString += "\n- <color=orange>Locked</color>";
        tempString += "\n\n";
        //Effect
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u>";
        if (thisEquipment().EQAbusePercent(EquipMenuUI.heroKind) < 1.0d) tempString += "<color=yellow>  Efficiency (Equipment Tenacity) : " + percent(thisEquipment().EQAbusePercent(EquipMenuUI.heroKind)) + "</color>";
        tempString += "<size=18>";
        for (int i = 0; i < thisEquipment().globalInfo.effects.Count; i++)
        {
            int count = i;
            tempString += "\n- " + localized.EquipmentEffect(thisEquipment().globalInfo.effects[count].kind, thisEquipment().globalInfo.effects[count].EffectValue(thisEquipment().Level()) * effectMultiplier, false, thisEquipment().globalInfo.effects[count].baseValue() * effectMultiplier * game.equipmentCtrl.effectMultiplier.Value());
        }
        for (int i = 0; i < thisEquipment().totalOptionNum.Value(); i++)
        {
            int count = i;
            tempString += "\n- " + localized.EquipmentEffect(thisEquipment().optionEffects[count].kind, thisEquipment().optionEffects[count].effectValue * effectMultiplier);
        }
        tempString += "\n\n";
        //Proficiency
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Proficiency) + "</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            EquipmentLevel tempLevel = thisEquipment().globalInfo.levels[count];
            tempString += "\n- " + localized.Hero((HeroKind)i) + " < <color=green>Lv " + tDigit(tempLevel.value) + "</color> > : ";
            if (tempLevel.IsMaxed())
                tempString += optStr + "Mastered";
            else
                tempString += percent(thisEquipment().globalInfo.ProficiencyPercent((HeroKind)count), 2) + " ( " + thisEquipment().globalInfo.ProficiencyTimeLeftString((HeroKind)count) + " left )";
            if (tempLevel.isMaxed) tempString += "  <color=green>";
            else tempString += "  <color=grey>";
            if (thisEquipment().globalInfo.levelMaxEffects[i].kind == EquipmentEffectKind.Nothing)
                tempString += "[ " + localized.EquipmentEffect(thisEquipment().globalInfo.levelMaxEffects[i].kind, thisEquipment().globalInfo.levelMaxEffects[i].EffectValue(0), true) + " ]</color>";
            else
                tempString += "[ " + localized.EquipmentEffect(thisEquipment().globalInfo.levelMaxEffects[i].kind, thisEquipment().globalInfo.levelMaxEffects[i].EffectValue(0) * effectMultiplier, true) + " ]</color>";
        }
        tempString += "\n\n";
        //RequiredAbility
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.RequiredAbility) + "</u><size=18>";
        tempString += "\n- " + localized.Basic(BasicWord.HeroLevel) + " " + thisEquipment().RequiredLevel();
        tempString += "  ( ";
        if (thisEquipment().IsEnoughLevel(EquipMenuUI.heroKind)) tempString += "<color=green>";
        else tempString += "<color=red>";
        tempString += tDigit(game.statsCtrl.LevelForEquipment(EquipMenuUI.heroKind).Value()) + "</color> )";
        for (int i = 1; i < thisEquipment().globalInfo.requiredAbilities.Count; i++)
        {
            int count = i;
            tempString += "\n- " + localized.Ability(thisEquipment().globalInfo.requiredAbilities[count].kind) + " " + tDigit(thisEquipment().globalInfo.requiredAbilities[count].requiredValue);
            tempString += "  ( ";
            if (thisEquipment().globalInfo.requiredAbilities[count].IsEnough(EquipMenuUI.heroKind)) tempString += "<color=green>";
            else tempString += "<color=red>";
            tempString += tDigit(game.statsCtrl.Ability(EquipMenuUI.heroKind, thisEquipment().globalInfo.requiredAbilities[count].kind).Point()) + "</color> )";
        }
        return tempString;
    }
}

public class EnchantPopupUI : PopupUI
{
    Func<EquipmentEnchant> thisEnchant;
    Image icon;
    TextMeshProUGUI nameText, description;
    public EnchantPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(Func<EquipmentEnchant> thisEnchant)
    {
        this.thisEnchant = thisEnchant;
        if (thisEnchant().kind == EnchantKind.Nothing) return;
        icon.sprite = sprite.enchants[(int)thisEnchant().kind];
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
        //CheckMouseOver();
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.EnchantName(thisEnchant().kind);
        if (thisEnchant().kind == EnchantKind.OptionAdd)
            tempString += optStr + "<color=yellow> [ " + localized.EquipmentEffectName(thisEnchant().effectKind) + " Lv " + tDigit(thisEnchant().effectLevel) + " ]</color>";
        if (thisEnchant().kind == EnchantKind.InstantProf)
            tempString += " [ " + DoubleTimeToDate(thisEnchant().proficiencyTimesec) + " ]";
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += "\n- " + localized.EnchantInformation(thisEnchant().kind);
        //tempString += "\n- " + localized.Basic(BasicWord.SuccessChance) + " : 100%";//Debug
        if (thisEnchant().kind == EnchantKind.OptionAdd)
        {
            tempString += "\n\n";
            tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.AdditionalEffect) + "</u><size=18>";
            tempString += optStr + "\n- " + localized.EquipmentEffect(thisEnchant().effectKind, EquipmentParameter.EffectCalculation(thisEnchant().effectKind, thisEnchant().effectLevel));
            tempString += optStr + " ~ " + localized.EquipmentEffect(thisEnchant().effectKind, EquipmentParameter.EffectCalculation(thisEnchant().effectKind, thisEnchant().effectLevel + 1));
            tempString += "\n\n";
            tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.RequiredAbilityIncrement) + "</u><size=18>";
            tempString += "\n- " + localized.Basic(BasicWord.HeroLevel) + " + " + EquipmentParameter.RequiredLevelIncrement(thisEnchant().effectKind, thisEnchant().effectLevel);
        }
        return tempString;
    }
}

public class EnchantConfirmUI : ConfirmUI
{
    public Button[] effectButtons = new Button[EquipmentParameter.maxOptionEffectNum];
    public TextMeshProUGUI description;
    public TextMeshProUGUI[] buttonTexts = new TextMeshProUGUI[EquipmentParameter.maxOptionEffectNum];
    public EnchantConfirmUI(GameObject thisObject) : base(thisObject)
    {
        description = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < effectButtons.Length; i++)
        {
            int count = i;
            effectButtons[i] = thisObject.transform.GetChild(3 + count).gameObject.GetComponent<Button>();
            effectButtons[i].onClick.AddListener(() => { game.inventoryCtrl.TryEnchant(enchant(), equipment(), count); Hide(); });
            buttonTexts[i] = effectButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        quitButton.onClick.AddListener(Hide);
    }
    Func<EquipmentEnchant> enchant;
    Func<Equipment> equipment;
    public void SetUI(EquipmentEnchant enchant, Equipment equipment)
    {
        description.text = "Choose the target effect.";
        this.enchant = () => enchant;
        this.equipment = () => equipment;
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            int count = i;
            if (i < equipment.totalOptionNum.Value())
            {                
                setActive(effectButtons[i].gameObject);
                buttonTexts[i].text = optStr + "<size=18>" + localized.EquipmentEffectName(equipment.optionEffects[i].kind) + " Lv " + tDigit(equipment.optionEffects[i].level.value);
            }
            else
            {
                setFalse(effectButtons[i].gameObject);
            }
        }
        DelayShow();
    }
}


public class PotionPopupUI : PopupUI
{
    public Func<Potion> thisPotion;
    public Image icon;
    public TextMeshProUGUI nameText, description;
    public PotionPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(Func<Potion> thisPotion)
    {
        this.thisPotion = thisPotion;
        if (thisPotion().kind == PotionKind.Nothing) return;
        icon.sprite = sprite.potions[(int)thisPotion().kind];
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
        //CheckMouseOver();
        if (thisPotion().globalInfo.kind == PotionKind.Nothing)
        {
            DelayHide();
            return;
        }
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.PotionName(thisPotion().kind);
        if (thisPotion().globalInfo.type != PotionType.Trap)//thisPotion().globalInfo.type != PotionType.Talisman && 
            tempString += optStr + " < <color=green>Lv " + tDigit(thisPotion().globalInfo.level.value) + "</color> >";
        tempString += optStr + "\n\n<size=18>" + "Type : " + thisPotion().globalInfo.type.ToString();
        if(thisPotion().globalInfo.type == PotionType.Talisman)
            tempString += optStr + " (" + thisPotion().globalInfo.talismanRarity.ToString() + ")"; 
        tempString += optStr + "\n<size=18>" + "Stack # : " + tDigit(thisPotion().stackNum.value) + " / " + tDigit(thisPotion().maxStackNum);
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += "\n- " + localized.PotionConsume(thisPotion().consumeKind, thisPotion().globalInfo.consumeChance);
        if (thisPotion().globalInfo.type != PotionType.Health || thisPotion().globalInfo.type != PotionType.Trap) tempString += "\n- Remains active while equipped in Utility slot";
        if (thisPotion().globalInfo.cooltime > 0) tempString += "\n- Cool Time : " + tDigit(thisPotion().globalInfo.cooltime, 2) + " " + localized.Basic(BasicWord.Sec);
        tempString += "\n\n";
        //Effect
        tempString += optStr + "<size=20><u>Equipped Effect</u><size=18>";
        if (thisPotion().globalInfo.type == PotionType.Talisman)
            tempString += "\n- " + localized.PotionEffect(thisPotion().kind, thisPotion().TalismanEffectValue());
        else
            tempString += "\n- " + localized.PotionEffect(thisPotion().kind, thisPotion().globalInfo.effectValue);
        if (thisPotion().globalInfo.type == PotionType.Trap)
        {
            tempString += "\n- Capturable Monster Level : <color=green>Lv " + tDigit(game.monsterCtrl.monsterCapturableLevel[(int)EquipMenuUI.heroKind].Value()) + " or less</color>";
            tempString += "\n<color=yellow>It increases along with Hero Level based on Title [Monster Study].</color>";
            if (thisPotion().kind == PotionKind.ThrowingNet)
                tempString += "\n<color=yellow>You cannot capture 'color' type monsters with this item.</color>";
        }
        //PassiveEffectOnDisassemble
        if (thisPotion().globalInfo.type == PotionType.Talisman)
        {
            if (thisPotion().globalInfo.talismanRarity == TalismanRarity.Epic)
                tempString += "\n\n<color=yellow>This Talisman cannot be disassembled.</color>";
            else
            {
                tempString += "\n\n";
                tempString += optStr + "<size=20><u>Passive Effect to be obtained when disassembled</u><size=18>";
                tempString += "\n- " + localized.PotionEffect(thisPotion().kind, thisPotion().TalismanPassiveEffectValue(), true);
            }
        }
        return tempString;
    }
}
