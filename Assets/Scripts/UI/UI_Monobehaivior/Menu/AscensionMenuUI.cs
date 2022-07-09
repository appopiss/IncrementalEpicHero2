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

public class AscensionMenuUI : MENU_UI
{
    [SerializeField] GameObject infoIcon, missionMilestoneIcon;
    [SerializeField] Slider progressSlider;
    [SerializeField] TextMeshProUGUI progressText, descriptionText, infoText;
    [SerializeField] Button ascensionButton;
    [SerializeField] GameObject[] milestones;
    [SerializeField] TextMeshProUGUI missionText, missionClearText, upgradeTitleText;
    [SerializeField] GameObject[] upgrades;
    WorldAscensionMilestoneUI[] milestonesUI;
    WorldAscensionUpgradeUI[] upgradesUI;
    PopupUI popupUI;
    WorldAscension wa => game.ascensionCtrl.worldAscensions[0];

    // Start is called before the first frame update
    void Start()
    {
        milestonesUI = new WorldAscensionMilestoneUI[milestones.Length];
        for (int i = 0; i < milestonesUI.Length; i++)
        {
            int count = i;
            milestonesUI[i] = new WorldAscensionMilestoneUI(milestones[count], wa.milestoneList[count]);
        }
        upgradesUI = new WorldAscensionUpgradeUI[upgrades.Length];
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            upgradesUI[i] = new WorldAscensionUpgradeUI(upgrades[count], wa.upgradeList[count]);
        }
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(infoIcon, () => InfoPopupString());
        for (int i = 0; i < milestonesUI.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(milestones[count], () => milestonesUI[count].milestone.DescriptionString());
        }
        popupUI.SetTargetObject(missionMilestoneIcon, () => MissionMilestonePopupString());

        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(upgrades[count], () => upgradesUI[count].upgrade.DescriptionString());
        }
        ascensionButton.onClick.AddListener(() => { ascensionButton.interactable = false; ConfirmPerform(); });
        openClose.openActions.Add(SetUI);
        SetUI();//一度呼んでおく

        confirmUI = new ConfirmUI(GameControllerUI.gameUI.popupCtrlUI.defaultConfirm);
    }

    //Perform
    ConfirmUI confirmUI;
    void ConfirmPerform()
    {
        confirmUI.SetUI("Are you sure you want to perform Tier " + tDigit(wa.tier + 1) + " World Ascension right now?\n" + DescriptionString());
        SetActive(confirmUI.quitButton.gameObject, true);
        confirmUI.okButton.onClick.RemoveAllListeners();
        confirmUI.okButton.onClick.AddListener(()=> {
            ascensionButton.interactable = false;
            SetActive(gameUI.leftCanvas, false);
            wa.Perform();
            SetActive(gameUI.leftCanvas, true);
            confirmUI.Hide();
        });
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        popupUI.UpdateUI();
        countSec++;
        if (countSec >= 60)
        {
            SetUI();
            countSec = 0;
        }
    }
    string DescriptionString()
    {
        return optStr + "You will gain <color=green>" + tDigit(wa.PointGain()) + " World Ascension Point</color> of Tier " + tDigit(wa.tier + 1) + " when you ascend right now!";
    }
    void SetUI()
    {
        wa.CheckAccomplishementClear();
        wa.CalculateTotalMilestoneLevel();
        progressText.text = optStr + "Tier " + tDigit(wa.tier + 1) + " World Ascension : Total Milestone Lv " + tDigit(wa.TotalMilestoneLevel()) + " / " + tDigit(wa.NextMilestoneLevelToPerform()) + "  ( " + percent(Math.Min(1, progressPercent)) + " )";
        descriptionText.text = DescriptionString();
        infoText.text = InfoString();
        missionText.text = MissionString();
        missionClearText.text = optStr + "Clear # <color=green>" + tDigit(wa.MissionClearNum()) + "</color> / " + tDigit(wa.missionList.Count);
        ascensionButton.interactable = wa.CanPerform();
        upgradeTitleText.text = "<size=20>Tier " + tDigit(wa.tier + 1) + " World Ascension Upgrade <size=18>( point left : " + tDigit(wa.point.value) + " )";
        progressSlider.value = progressPercent;
        for (int i = 0; i < milestonesUI.Length; i++)
        {
            milestonesUI[i].UpdateUI();
        }
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i].UpdateUI();
        }
    }
    int countSec;
    float progressPercent => (float)wa.TotalMilestoneLevel() / (float)wa.NextMilestoneLevelToPerform();

    string InfoString()
    {
        string tempStr = optStr;
        tempStr += "Tier " + tDigit(wa.tier + 1) + " World Ascension # <color=green>" + tDigit(wa.performedNum) + "</color>";
        tempStr += "\nTime Played : " + DoubleTimeToDate(wa.WorldAscensionPlaytime());
        if (wa.bestPlayTime > 0) tempStr += " ( Best : " + DoubleTimeToDate(wa.bestPlayTime) + " )";
        return tempStr;
    }
    string MissionString()
    {
        string tempStr = optStr;
        for (int i = 0; i < wa.missionList.Count; i++)
        {
            tempStr += optStr + wa.missionList[i].Description() + "\n";
        }
        return tempStr;
    }
    string MissionMilestonePopupString()
    {
        string tempStr = optStr;
        tempStr += "<size=24>Accomplishment Rewards<size=20> ( Total Cleared # <color=green>" + tDigit(wa.MissionClearNum()) + "</color> )";
        for (int i = 0; i < wa.missionMilestoneList.Count; i++)
        {
            tempStr += "\n" + wa.missionMilestoneList[i].DescriptionString();
        }
        return tempStr;
    }
    string InfoPopupString()
    {
        string tempStr = optStr;
        switch (wa.tier)
        {
            case 0:
                tempStr += "<size=20>World Ascension Tier 1<size=18>";
                tempStr += "\n\n<u>What is unlocked after Tier 1 World Ascension</u>";
                tempStr += "\n- Area / Dungeon Prestige";
                tempStr += "\n- New Guild Abilities";
                //tempStr += "\n- New Regular Upgrades in Upgrade Tab";

                tempStr += "\n\n<u>What RESETS after Tier 1 World Ascension</u>";
                tempStr += "\n- Area Cleared # and Area Prestige Upgrade";
                tempStr += "\n- Gold, Slime Coin, Resources, Hero Level and EXP";
                tempStr += "\n- General Quest except for Mastery effects";
                tempStr += "\n- Skill Rank, Level and Proficiency";
                tempStr += "\n- Upgrade Level in Upgrade Tab";
                tempStr += "\n- Equipment Level, Proficiency and Dictionary Upgrade";
                tempStr += "\n- Equipment that has no enchanted slots in inventory and equip slots";
                tempStr += "\n- Expanded Mysterious Water and Catalysts";
                tempStr += "\n- Guild Level, EXP and Guild Ability";
                tempStr += "\n- Town Building Rank, Level and Town Materials";
                tempStr += "\n- Rebirth and Challenge progresses";
                tempStr += "\n- Expedition Team progresses";

                tempStr += "\n\n<u>What does NOT reset after Tier 1 World Ascension</u>";
                tempStr += "\n- Portal Orb, Dungeon Cleared # and Dungeon Prestige Upgrade";
                tempStr += "\n- Global Quests, Daily Quests, Title Quests and Titles that are already acquired, General Quest's Mastery";
                tempStr += "\n- Equipment Mastery Effect";
                tempStr += "\n- Equipment that has any enchanted effects or enchanted slots in inventory";
                tempStr += "\n- Items in Enchant Inventory, Items in Utility Inventory and its equipped slots, Talisman progresses";
                tempStr += "\n- Essence and Materials except for Town Materials";
                tempStr += "\n- Alchemy Upgrade and Potion Level";
                tempStr += "\n- Town Building's Research progresses";
                tempStr += "\n- Bestiary progresses";
                tempStr += "\n- Expedition Level and EXP";
                tempStr += "\n- Area Mission Milestone, Nitro, Epic Coin and Epic Store purchases";

                tempStr += "\n\n<u>Caution</u>";
                tempStr += "\n- Each World Ascension increases the requirement of total milestone levels to ascend next time by 1";
                tempStr += "\n- Heroes that were unlocked will be still unlocked after World Ascension";
                tempStr += "\n- Town Buildings that were unlocked will be still unlocked after World Ascesion";
                tempStr += "\n- Daily Quests and Title Quests that are currently accepted will be unaccepted after World Ascension";
                tempStr += "\n- Items in expanded inventory slots will not be available until you unlock the slots again";
                tempStr += "\n- Activated Pets that are exceeded activable # will be automatically inactivated";
                tempStr += "\n- You will be able to try completed missions but you won't get another Epic Coin from them";
                break;
        }
        return tempStr;
    }
}

public class WorldAscensionMilestoneUI
{
    public WorldAscensionMilestoneUI(GameObject gameObject, WorldAscensionMilestone milestone)
    {
        thisObject = gameObject;
        this.milestone = milestone;
        backgroundImage = thisObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        fillImage = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateUI()
    {
        backgroundImage.color = imageColor[Math.Min(10, milestone.CurrentLevel())];
        if (milestone.CurrentLevel() >= 10) fillImage.color = Color.clear;
        else fillImage.color = imageColor[milestone.CurrentLevel() + 1];
        fillImage.fillAmount = ProgressPercent();
        nameText.text = milestone.NameString();
    }
    public float ProgressPercent()
    {
        if (milestone.CurrentLevel() > 0)
            return (float)((milestone.calculatedCurrentValue - milestone.GoalValue(milestone.CurrentLevel())) / (milestone.GoalValue(milestone.CurrentLevel() + 1) - milestone.GoalValue(milestone.CurrentLevel())));
        return (float)(milestone.calculatedCurrentValue / milestone.GoalValue(milestone.CurrentLevel() + 1));
        //return (float)(milestone.currentValue / milestone.GoalValue(milestone.maxLevelReached.value + 1));
    }
    //Color ImageColor(long level)
    //{
    //    switch (level)
    //    {
    //        case 1: return Color.green;
    //        default:
    //            break;
    //    }
    //    return Color.black;
    //}
    public WorldAscensionMilestone milestone;
    public GameObject thisObject;
    public Image backgroundImage, fillImage;
    public TextMeshProUGUI nameText;
    static Color[] imageColor = new Color[]//[level]
    {
        Color.black,//0
        new Color(80f / 255f, 130f / 255f, 0f),//1
        new Color(180f / 255f, 120f / 255f, 0f),//2
        new Color(180f / 255f, 80f / 255f, 0f),//3
        new Color(180f / 255f, 0f, 0f),//4
        new Color(200f / 255f, 0f, 200f / 255f),//5
        new Color(128f / 255f, 0f, 255f / 255f),//6
        Color.blue,//7
        new Color(0, 150f / 255f, 255f / 255f),//8
        new Color(0, 150f / 200f, 200f / 255f),//9
        new Color(0, 150f / 150f, 80f / 255f),//10
    };
}

public class WorldAscensionUpgradeUI
{
    public WorldAscensionUpgradeUI(GameObject gameObject, WorldAscensionUpgrade upgrade)
    {
        thisObject = gameObject;
        this.upgrade = upgrade;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(() =>
        {
            upgrade.transaction.Buy();
            UpdateUI();
        });
    }
    public void UpdateUI()
    {
        plusButton.interactable = upgrade.transaction.CanBuy();
        nameText.text = upgrade.Name();
        levelText.text = "<color=green>Lv " + tDigit(upgrade.level.value) + "</color>";
    }
    public WorldAscensionUpgrade upgrade;
    GameObject thisObject;
    TextMeshProUGUI nameText, levelText;
    Button plusButton;
}
