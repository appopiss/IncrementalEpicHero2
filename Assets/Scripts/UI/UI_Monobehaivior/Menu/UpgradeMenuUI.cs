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

public class UpgradeMenuUI : MENU_UI
{
    [SerializeField] GameObject[] upgradeTables;//[upgradeKind]
    UpgradeTableUI[] upgradeTablesUI;
    [SerializeField] Button[] kindSelectButton;
    TextMeshProUGUI[] kindSelectText;
    [SerializeField] TextMeshProUGUI titleText, queueText;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;
    PopupUI popupUI;

    // Start is called before the first frame update
    void Start()
    {
        kindSelectText = new TextMeshProUGUI[kindSelectButton.Length];
        for (int i = 0; i < kindSelectText.Length; i++)
        {
            kindSelectText[i] = kindSelectButton[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
        upgradeTablesUI = new UpgradeTableUI[upgradeTables.Length];
        for (int i = 0; i < upgradeTablesUI.Length; i++)
        {
            upgradeTablesUI[i] = new UpgradeTableUI(upgradeTables[i], i);

            for (int j = 0; j < kindSelectButton.Length; j++)
            {
                if (j == i)
                    upgradeTablesUI[i].thisOpenClose.SetOpenButton(kindSelectButton[j]);
                else
                    upgradeTablesUI[i].thisOpenClose.SetCloseButton(kindSelectButton[j]);
            }
        }
        kindSelectButton[0].onClick.Invoke();

        //あとでローカライズ　未
        upgradeTablesUI[0].thisOpenClose.openActions.Add(() => { currentTableId = 0; titleText.text = "Resource Upgrade"; });
        upgradeTablesUI[1].thisOpenClose.openActions.Add(() => { currentTableId = 1; titleText.text = "Stats Upgrade"; });
        upgradeTablesUI[2].thisOpenClose.openActions.Add(() => { currentTableId = 2; titleText.text = "Gold Cap Upgrade"; });
        upgradeTablesUI[3].thisOpenClose.openActions.Add(() => { currentTableId = 3; titleText.text = "Slime Bank Upgrade 1"; });
        upgradeTablesUI[4].thisOpenClose.openActions.Add(() => { currentTableId = 4; titleText.text = "Slime Bank Upgrade 2"; });
        upgradeTablesUI[5].thisOpenClose.openActions.Add(() => { currentTableId = 5; titleText.text = "Slime Bank Upgrade 3"; });

        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(queueText.gameObject, () => QueueTextPopupString());

        for (int i = 3; i < 6; i++)
        {
            popupUI.SetTargetObject(kindSelectButton[i].gameObject, () => "<sprite=\"locks\" index=0> Town Building [ Slime Bank ]", () => game.townCtrl.Building(BuildingKind.SlimeBank).Rank() < 1);
        }

        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Purple).pet, petQoLIcon, popupUI, "Keep SC until Maxed Cap");
    }
    int currentTableId = 0;
    string QueueTextPopupString()
    {
        string tempStr = optStr + "Upgrade Queue";
        tempStr += "<size=18>\n- Upgrade Queue will automatically purchase an upgrade when you have the required Gold.";
        tempStr += "\n- Right-Click on an upgrade to assign queue while Shift + Right-Click to remove queue.";
        tempStr += "\n- You can get Upgrade Queue through Global Quests, Pet effects and Epic Store purchases.";
        return tempStr;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        popupUI.UpdateUI();
        for (int i = 0; i < upgradeTablesUI.Length; i++)
        {
            upgradeTablesUI[i].UpdateUI();
        }
        kindSelectButton[3].interactable = game.townCtrl.Building(BuildingKind.SlimeBank).Rank() > 0;
        if (kindSelectButton[3].interactable) kindSelectText[3].text = "Slime Bank 1";
        else kindSelectText[3].text = "???";
        kindSelectButton[4].interactable = game.townCtrl.Building(BuildingKind.SlimeBank).Rank() > 0;
        if (kindSelectButton[4].interactable) kindSelectText[4].text = "Slime Bank 2";
        else kindSelectText[4].text = "???";
        kindSelectButton[5].interactable = game.townCtrl.Building(BuildingKind.SlimeBank).Rank() > 0;
        if (kindSelectButton[5].interactable) kindSelectText[5].text = "Slime Bank 3";
        else kindSelectText[5].text = "???";

        queueText.text = optStr + "Available Queue : " + tDigit(game.upgradeCtrl.CurrentAvailableQueue()) + " / " + tDigit(game.upgradeCtrl.availableQueue.Value());

        SetActive(petQoLIcon, currentTableId >= 3);
        if (currentTableId >= 3) petQoLUI.UpdateUI();
    }
    private void Update()
    {
        for (int i = 0; i < upgradeTablesUI.Length; i++)
        {
            upgradeTablesUI[i].Update();
        }
    }
}


public class UpgradeTableUI
{
    public UpgradeTableUI(GameObject thisObject, int pageId)
    {
        this.thisObject = thisObject;
        upgradesUI = new UpgradeUI[thisObject.transform.childCount];

        switch (pageId)
        {
            case 0:
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i] = new UpgradeUI(thisObject.transform.GetChild(i).gameObject, UpgradeKind.Resource, i);
                }
                break;
            case 1:
                upgradesUI[0] = new UpgradeUI(thisObject.transform.GetChild(0).gameObject, UpgradeKind.BasicStats, 0);
                upgradesUI[1] = new UpgradeUI(thisObject.transform.GetChild(1).gameObject, UpgradeKind.BasicStats, 1);
                upgradesUI[2] = new UpgradeUI(thisObject.transform.GetChild(2).gameObject, UpgradeKind.BasicStats, 2);
                upgradesUI[3] = new UpgradeUI(thisObject.transform.GetChild(3).gameObject, UpgradeKind.BasicStats, 3);
                upgradesUI[4] = new UpgradeUI(thisObject.transform.GetChild(4).gameObject, UpgradeKind.BasicStats, 4);
                upgradesUI[5] = new UpgradeUI(thisObject.transform.GetChild(5).gameObject, UpgradeKind.BasicStats, 5);
                upgradesUI[6] = new UpgradeUI(thisObject.transform.GetChild(6).gameObject, UpgradeKind.GoldGain, 0);
                upgradesUI[7] = new UpgradeUI(thisObject.transform.GetChild(7).gameObject, UpgradeKind.ExpGain, 0);
                break;
            case 2:
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i] = new UpgradeUI(thisObject.transform.GetChild(i).gameObject, UpgradeKind.GoldCap, i);
                }
                break;
            case 3:
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i] = new UpgradeUI(thisObject.transform.GetChild(i).gameObject, UpgradeKind.SlimeBank, i);
                }
                break;
            case 4:
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i] = new UpgradeUI(thisObject.transform.GetChild(i).gameObject, UpgradeKind.SlimeBank, (int)SlimeBankUpgradeKind.MysteriousWaterGain + i);
                }
                break;
            case 5:
                for (int i = 0; i < upgradesUI.Length; i++)
                {
                    upgradesUI[i] = new UpgradeUI(thisObject.transform.GetChild(i).gameObject, UpgradeKind.SlimeBank, (int)SlimeBankUpgradeKind.SPD + i);
                }
                break;
        }

        thisOpenClose = new OpenCloseUI(thisObject, false, false, true);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i].UpdateUI();
        }
    }
    public void Update()
    {
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i].Update();
        }
    }
    GameObject thisObject;
    public OpenCloseUI thisOpenClose;
    public UpgradeUI[] upgradesUI;
}

public class UpgradeUI
{
    public UpgradeUI(GameObject thisObject, UpgradeKind kind, int id)
    {
        this.thisObject = thisObject;
        this.kind = kind;
        this.id = id;
        thisButton = thisObject.GetComponent<Button>();
        description = thisObject.GetComponentInChildren<TextMeshProUGUI>();
        thisUpgrade = game.upgradeCtrl.Upgrade(kind, id);
        thisButton.onClick.AddListener(() => thisUpgrade.transaction.Buy());
        thisButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
        SetEventAction();
    }
    UpgradeKind kind;
    int id;
    GameObject thisObject;
    Button thisButton;
    TextMeshProUGUI description;
    UPGRADE thisUpgrade;

    string DescriptionString()
    {
        string tempString = optStr + "<size=18>";
        switch (kind)
        {
            case UpgradeKind.Resource:
                tempString += optStr + "Resource Gain " + (1+thisUpgrade.id).ToString() + "  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Current Effect : + " + tDigit(game.upgradeCtrl.ResourceGain()) + " per kill";
                tempString += optStr + "\n- Next Effect : + " + tDigit(game.upgradeCtrl.ResourceGain(true, id)) + " per kill  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=0> " + tDigit(thisUpgrade.transaction.Cost());
                tempString += "\nEffect Increment per Gold Cost : " + tDigit((game.upgradeCtrl.ResourceGain(true, id) - game.upgradeCtrl.ResourceGain()) / thisUpgrade.transaction.Cost(), 3);
                break;
            case UpgradeKind.BasicStats:
                tempString += optStr + localized.BasicStats((BasicStatsKind)id) +  "  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Current Effect : + " + tDigit(thisUpgrade.EffectValue());
                tempString += optStr + "\n- Next Effect : + " + tDigit(thisUpgrade.EffectValue(true)) + "  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=0> " + tDigit(thisUpgrade.transaction.Cost()) + ",  " + "<sprite=\"resource\" index=" + (2 + (int)thisUpgrade.ResourceKind()).ToString() + "> " + tDigit(thisUpgrade.transaction.Cost(1));
                break;
            case UpgradeKind.GoldGain:
                tempString += optStr + localized.GlobalStat(GlobalStats.GoldGain) + "  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Current Effect : + " + tDigit(thisUpgrade.EffectValue());
                tempString += optStr + "\n- Next Effect : + " + tDigit(thisUpgrade.EffectValue(true)) + "  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=0> " + tDigit(thisUpgrade.transaction.Cost());
                break;
            case UpgradeKind.ExpGain:
                tempString += optStr + localized.Stat(Stats.ExpGain) + "  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Current Effect : + " + tDigit(thisUpgrade.EffectValue());
                tempString += optStr + "\n- Next Effect : + " + tDigit(thisUpgrade.EffectValue(true)) + "  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=0> " + tDigit(thisUpgrade.transaction.Cost());
                break;
            case UpgradeKind.GoldCap:
                tempString += optStr + "Gold Cap  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Current Effect : + " + tDigit(thisUpgrade.EffectValue());
                tempString += optStr + "\n- Next Effect : + " + tDigit(thisUpgrade.EffectValue(true)) + "  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=" + (2 + (int)thisUpgrade.ResourceKind()).ToString() + "> " + tDigit(thisUpgrade.transaction.Cost());
                break;
            case UpgradeKind.SlimeBank:
                tempString += optStr + thisUpgrade.Name() + "  < <color=green>Lv " + tDigit(thisUpgrade.level.value) + "</color> >";
                if (thisUpgrade.queue > 0) tempString += optStr + "  <color=orange>Q" + tDigit(thisUpgrade.queue) + "</color>";
                if (thisUpgrade.isSuperQueued) tempString += "  <color=orange>SQ</color>";
                tempString += optStr + "\n<size=16>- Effect : " + thisUpgrade.Description();
                tempString += optStr + "\n<size=16>-- Current : " + thisUpgrade.EffectString();
                tempString += optStr + "\n-- Next : " + thisUpgrade.EffectString(true) + "  ( <color=green>Lv " + tDigit(thisUpgrade.transaction.ToLevel()) + "</color> )";
                tempString += optStr + "\n- Cost to level up : " + "<sprite=\"resource\" index=1> " + tDigit(thisUpgrade.transaction.Cost());
                break;
        }
        return tempString;
    }

    public void UpdateUI()
    {
        SetActive(thisObject, thisUpgrade.IsUnlocked());        
        thisButton.interactable = thisUpgrade.transaction.CanBuy();
        description.text = DescriptionString();
    }
    public void Update()
    {
        if (!isHoverMouse) return;
        if (Input.GetMouseButtonDown(1))//右クリック
        {
            if (GameControllerUI.isShiftPressed) game.upgradeCtrl.RemoveQueue(thisUpgrade);
            else game.upgradeCtrl.AssignQueue(thisUpgrade);
        }
        if (game.epicStoreCtrl.Item(EpicStoreKind.SuperQueueUpgrade).IsPurchased() && Input.GetKeyDown(KeyCode.Q))
        {
            //if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) game.upgradeCtrl.RemoveQueue(thisUpgrade);
            //else
            if (GameControllerUI.isShiftPressed) game.upgradeCtrl.RemoveQueue(thisUpgrade);
            else game.upgradeCtrl.AssignQueue(thisUpgrade, true);
        }
    }

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
