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

public class RebirthMenuUI : MENU_UI
{
    [SerializeField] Button[] tierSwitchButtons;
    TextMeshProUGUI[] buttonTexts;
    SwitchTabUI tierSwitchTabUI;
    int tier { get => tierSwitchTabUI.currentId; }
    int displayTier => tier + 1;
    [SerializeField] TextMeshProUGUI descriptionText, infoText, upgradeText;
    [SerializeField] Button rebirthButton;
    [SerializeField] GameObject[] upgrades;
    RebirthUpgradeUI[] upgradesUI;
    [SerializeField] Button[] heroSwitchButtons;
    SwitchTabUI heroSwitchTabUI;
    RebirthUpgradePopupUI upgradePopupUI;
    HeroKind heroKind { get => (HeroKind)heroSwitchTabUI.currentId; }
    Rebirth currentRebirth { get => game.rebirthCtrl.Rebirth(game.currentHero, tier); }
    Rebirth currentRebirthForUpgrade { get => game.rebirthCtrl.Rebirth(heroKind, tier); }
    Rebirth currentAutoRebirth { get => game.rebirthCtrl.Rebirth(heroKind, main.S.autoRebirthTiers[(int)heroKind]); }
    [SerializeField] GameObject autoRebirthCustomizeCanvas;
    [SerializeField] Toggle autoRebirthToggle, bestExpSecToggle;
    [SerializeField] Button customizeOpenButton, customizeQuitButton;
    [SerializeField] TextMeshProUGUI autoRebirthTitleText;
    public TMP_Dropdown tierDropdown, regionDropdown, areaDropdown;
    [SerializeField] TMP_InputField rebirthPointInputField, levelInputField;
    OpenCloseUI autoRebirthOpenCloseUI;
    PopupUI autoRebirthPopupUI, advancedAutoRebirthPopupUI, favoriteAreaPopupUI, bestExpPerSecPopupUI;
    PopupUI popupUI;
    //ConfirmUI confirmUI;

    void TierOnValueChanged(int tier)
    {
        int tempTier = tier;
        if (tier > 0)
        {
            if (!IsUnlockedAutoRebirth(tier))
            {
                tierDropdown.value = 0;
                tempTier = 0;
            }
        }
        main.S.autoRebirthTiers[(int)heroKind] = tempTier;
        SetUIAutoRebirth(true);
    }
    void PointOnValueChanged(string valueString)
    {
        double tempValue = double.Parse(valueString);
        tempValue = Math.Min(100000000, Math.Max(0, tempValue));
        rebirthPointInputField.text = tempValue.ToString();
        currentAutoRebirth.autoRebirthPoint = tempValue;
    }
    void LevelOnValueChanged(string valueString)
    {
        long tempValue = long.Parse(valueString);
        tempValue = Math.Min(2000, Math.Max(currentAutoRebirth.heroLevel, tempValue));
        levelInputField.text = tempValue.ToString();
        currentAutoRebirth.autoRebirthLevel = tempValue;
    }
    void RegionOnValueChanged(int id)
    {
        int tempId = id;
        if (!game.areaCtrl.IsUnlocked((AreaKind)tempId))
        {
            regionDropdown.value = 0;
            tempId = 0;
        }
        main.S.favoriteAreaKinds[(int)heroKind] = (AreaKind)tempId;
        AreaOnValueChanged(main.S.favoriteAreaIds[(int)heroKind]);
    }
    void AreaOnValueChanged(int id)
    {
        int tempId = id;
        if (!game.areaCtrl.Area(main.S.favoriteAreaKinds[(int)heroKind], tempId).IsUnlocked())
        {
            areaDropdown.value = 0;
            tempId = 0;
        }
        main.S.favoriteAreaIds[(int)heroKind] = tempId;
    }
    bool IsUnlockedAutoRebirth(int tier)
    {
        switch (tier)
        {
            case 0: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier1)) return false; break;
            case 1: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier2)) return false; break;
            case 2: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier3)) return false; break;
            case 3: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier4)) return false; break;
            case 4: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier5)) return false; break;
            case 5: if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier6)) return false; break;
        }
        return true;
    }
    void SetUIAutoRebirth(bool isTierChanged)
    {
        //現在の値を代入
        autoRebirthToggle.isOn = main.S.isAutoRebirthOns[(int)heroKind];
        if (!isTierChanged) tierDropdown.value = main.S.autoRebirthTiers[(int)heroKind];
        rebirthPointInputField.text = (currentAutoRebirth.autoRebirthPoint).ToString();
        levelInputField.text = currentAutoRebirth.autoRebirthLevel.ToString();
        regionDropdown.value = (int)game.rebirthCtrl.FavoriteArea(heroKind).kind;
        areaDropdown.value = (int)game.rebirthCtrl.FavoriteArea(heroKind).id;
        bestExpSecToggle.isOn = main.S.isBestExpSecAreas[(int)heroKind];
    }

    string ConfirmString()
    {
        string tempStr = optStr;
        tempStr += "<size=20>" + localized.Hero(currentRebirth.heroKind) + " Tier " + tDigit(displayTier) + " Rebirth<size=18>";
        tempStr += "\n\nAre you sure to rebirth right now?";
        tempStr += "\n\nYou will gain <color=green>" + tDigit(currentRebirth.RebirthPointGain()) + " Rebirth Point</color>";
        //tempStr += "\n\n" + ResetString();
        return tempStr;
    }

    private void Start()
    {
        upgradesUI = new RebirthUpgradeUI[upgrades.Length];
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            upgradesUI[i] = new RebirthUpgradeUI(upgrades[count], count);
        }

        heroSwitchTabUI = new SwitchTabUI(heroSwitchButtons, true);
        tierSwitchTabUI = new SwitchTabUI(tierSwitchButtons, true);
        heroSwitchTabUI.openAction = SetUI;
        tierSwitchTabUI.openAction = SetUI;

        rebirthButton.onClick.AddListener(() => { rebirthButton.interactable = false; currentRebirth.DoRebirth(); });//() => confirmUI.SetUI(ConfirmString()));

        buttonTexts = new TextMeshProUGUI[tierSwitchButtons.Length];
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i] = tierSwitchButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        //Popup
        upgradePopupUI = new RebirthUpgradePopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            upgradePopupUI.SetTargetObject(upgrades[count], () => upgradePopupUI.ShowAction(upgradesUI[count].thisUpgrade()));
        }
        autoRebirthPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        autoRebirthPopupUI.SetTargetObject(autoRebirthToggle.gameObject, AutoRebirthToggleString);
        advancedAutoRebirthPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        advancedAutoRebirthPopupUI.SetTargetObject(rebirthPointInputField.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.AdvancedRebirth).NameString() + " in Epic Store");
        advancedAutoRebirthPopupUI.SetTargetObject(levelInputField.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.AdvancedRebirth).NameString() + " in Epic Store");
        advancedAutoRebirthPopupUI.additionalShowCondition = () => !game.epicStoreCtrl.Item(EpicStoreKind.AdvancedRebirth).IsPurchased();
        favoriteAreaPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        favoriteAreaPopupUI.SetTargetObject(regionDropdown.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).NameString() + " in Epic Store");
        favoriteAreaPopupUI.SetTargetObject(areaDropdown.gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).NameString() + " in Epic Store");
        favoriteAreaPopupUI.additionalShowCondition = () => !game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).IsPurchased();
        bestExpPerSecPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        bestExpPerSecPopupUI.SetTargetObject(bestExpSecToggle.gameObject, ()=> "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.BestExpPerSec).NameString() + " in Epic Store");
        bestExpPerSecPopupUI.additionalShowCondition = () => !game.epicStoreCtrl.Item(EpicStoreKind.BestExpPerSec).IsPurchased();
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < tierSwitchButtons.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(tierSwitchButtons[count].gameObject, () => RebirthPopupString(count));
        }
        popupUI.SetTargetObject(rebirthButton.gameObject, () => RebirthPopupString());
        //confirmUI = new ConfirmUI(gameUI.popupCtrlUI.defaultConfirm);
        //confirmUI.quitButton.onClick.AddListener(() => currentRebirth.DoRebirth());

        //AutoRebirthSetting
        autoRebirthOpenCloseUI = new OpenCloseUI(autoRebirthCustomizeCanvas);
        autoRebirthOpenCloseUI.SetOpenButton(customizeOpenButton);
        autoRebirthOpenCloseUI.SetCloseButton(customizeQuitButton);

        autoRebirthToggle.onValueChanged.AddListener((isOn) => main.S.isAutoRebirthOns[(int)heroKind] = isOn);
        tierDropdown.onValueChanged.AddListener(TierOnValueChanged);
        rebirthPointInputField.onEndEdit.AddListener(PointOnValueChanged);
        levelInputField.onEndEdit.AddListener(LevelOnValueChanged);
        regionDropdown.onValueChanged.AddListener(RegionOnValueChanged);
        areaDropdown.onValueChanged.AddListener(AreaOnValueChanged);
        bestExpSecToggle.onValueChanged.AddListener((isOn) => main.S.isBestExpSecAreas[(int)heroKind] = isOn);

        SetUIAutoRebirth(false);
    }

    string RebirthPopupString()
    {
        string tempStr = "<size=20>" + localized.Hero(currentRebirth.heroKind) + " Tier " + tDigit(displayTier) + " Rebirth";
        if (currentRebirth.CanRebirth())
            return tempStr;
        if (game.statsCtrl.Level(heroKind) < currentRebirth.heroLevel)
        {
            tempStr += "\n<size=18><sprite=\"locks\" index=0> Hero Level " + tDigit(currentRebirth.heroLevel);
            return tempStr;
        }
        if (game.challengeCtrl.IsTryingChallenge())
        {
            tempStr += "\n<size=18><sprite=\"locks\" index=0> You can't rebirth while you are trying Challenge.";
            return tempStr;
        }
        if (game.battleCtrls[(int)game.currentHero].areaBattle.CurrentArea().isDungeon)
        {
            tempStr += "\n<size=18><sprite=\"locks\" index=0> You can't rebirth while you are in Dungeon.";
            return tempStr;
        }
        if (game.areaCtrl.areaClearedNums[(int)game.currentHero] < 1)
        {
            tempStr += "\n<size=18><sprite=\"locks\" index=0> Clear an area at least once";
            return tempStr;
        }
        return tempStr;
    }

    public override void Initialize()
    {
        heroSwitchButtons[(int)game.currentHero].onClick.Invoke();
        tierSwitchButtons[0].onClick.Invoke();
    }

    string RebirthPopupString(int tier)
    {
        if (!tierSwitchButtons[tier].interactable) return "<sprite=\"Locks\" index=0> Building [Temple] Rank " + tDigit(tier);
        string tempStr = optStr + "<size=18>";
        switch (tier)
        {
            case 0:
                tempStr += "Rebirth is a prestige feature for each hero that resets some progresses\n" +
                           "but gain Rebirth Points that is used for Rebirth Upgrades, which allow\n" +
                           "you to reach higher Hero Level faster!";
                break;
            case 1:
                tempStr += "Rebirth Tier 2 resets all Skill Level and Skill Proficiency in addition\n" +
                           "to what Tier 1 resets. However, Tier 2 Rebirth Upgrades will boost Tier 1\n" +
                           "Upgrades and Skill Proficiency Gain!";
                break;
            case 2:
                tempStr += "Rebirth Tier 3 resets Equipment Level and Proficiency of current playing hero \n" +
                           "in addition to what Tier 2 resets, which mean you can earn Dictionary Point again.\n" +
                           "Tier 3 Rebirth Upgrades will boost lower Tier's upgrades and you can increase both\n" +
                           "Hero Level and Equipment Level faster and higher!";
                break;
        }
        return tempStr;
    }
    
    string AutoRebirthToggleString()
    {
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier1))
        {
            if (autoRebirthToggle.isOn) return localized.Hero(heroKind) + " Auto-Rebirth <color=green>ON</color>";
            return localized.Hero(heroKind) + " Auto-Rebirth OFF";
        }
        return "<sprite=\"locks\" index=0> Normal Fairy Pet Rank 1";
    }

    void SetUI()
    {
        for (int i = 0; i < heroSwitchButtons.Length; i++)
        {
            if (i == heroSwitchTabUI.currentId)
                heroSwitchButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            else
                heroSwitchButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = halftrans;
        }

        for (int i = 0; i < upgradesUI.Length; i++)
        {
            int count = i;
            if (count < currentRebirthForUpgrade.rebirthUpgrades.Length)
                upgradesUI[count].SetUI(currentRebirthForUpgrade.rebirthUpgrades[count]);
            else
                upgradesUI[count].Disable();
        }
        SetUIAutoRebirth(false);
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        for (int i = 0; i < upgradesUI.Length; i++)
        {
            upgradesUI[i].UpdateUI();
        }
        rebirthButton.interactable = currentRebirth.CanRebirth();
        descriptionText.text = DescriptionString();
        infoText.text = InfoString();
        upgradeText.text = UpgradeString();

        popupUI.UpdateUI();
        upgradePopupUI.UpdateUI();

        for (int i = 0; i < tierSwitchButtons.Length; i++)
        {
            int count = i;
            tierSwitchButtons[i].interactable = game.rebirthCtrl.unlocks[i].IsUnlocked();
            if (i < 3)//Tier4以降を実装したらここを変える
                buttonTexts[i].text = "Tier " + (1 + count).ToString() + " ( Lv " + tDigit(game.rebirthCtrl.Rebirth(game.currentHero, count).heroLevel) + " )";
            else buttonTexts[i].text = "???";
        }
        for (int i = 0; i < heroSwitchButtons.Length; i++)
        {
            int count = i;
            heroSwitchButtons[i].interactable = game.guildCtrl.Member((HeroKind)count).IsUnlocked();
        }
        
        autoRebirthPopupUI.UpdateUI();
        autoRebirthToggle.interactable = game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier1);
        advancedAutoRebirthPopupUI.UpdateUI();
        rebirthPointInputField.interactable = game.epicStoreCtrl.Item(EpicStoreKind.AdvancedRebirth).IsPurchased();
        levelInputField.interactable = game.epicStoreCtrl.Item(EpicStoreKind.AdvancedRebirth).IsPurchased();
        favoriteAreaPopupUI.UpdateUI();
        regionDropdown.interactable = game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).IsPurchased();
        areaDropdown.interactable = game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).IsPurchased();
        bestExpPerSecPopupUI.UpdateUI();
        bestExpSecToggle.interactable = game.epicStoreCtrl.Item(EpicStoreKind.BestExpPerSec).IsPurchased();

        //AutoRebirth
        if (!autoRebirthOpenCloseUI.isOpen) return;
        autoRebirthTitleText.text = "Auto Rebirth Settings [ " + localized.Hero(heroKind) + " ]";
        for (int i = 0; i < tierDropdown.options.Count; i++)
        {
            int count = i;
            tierDropdown.options[i].text = IsUnlockedAutoRebirth(count) ? "" : "<sprite=\"locks\" index=0> ";
            tierDropdown.options[i].text += "Tier " + (1 + count).ToString();
        }
        for (int i = 0; i < regionDropdown.options.Count; i++)
        {
            int count = i;
            regionDropdown.options[i].text = game.areaCtrl.IsUnlocked((AreaKind)count) ? "" : "<sprite=\"locks\" index=0> ";
            regionDropdown.options[i].text += localized.AreaName((AreaKind)count);
        }
        for (int i = 0; i < areaDropdown.options.Count; i++)
        {
            int count = i;
            areaDropdown.options[i].text = game.areaCtrl.Area(main.S.favoriteAreaKinds[(int)heroKind], count).IsUnlocked() ? "" : "<sprite=\"locks\" index=0> ";
            areaDropdown.options[i].text += "Area " + (1 + count).ToString();
        }
    }
    string UpgradeString()
    {
        return optStr + "<size=20>Tier " + tDigit(displayTier) + " Rebirth Upgrade <size=20>( point left : " + tDigit(currentRebirthForUpgrade.rebirthPoint.value) + " )";
    }
    string ResetString()
    {
        string tempStr = optStr;
        tempStr += "<size=18>Tier " + tDigit(displayTier) + " Rebirth Resets :";
        tempStr += "<size=18>\n- Hero Level and EXP\n- General Quest";
        if (tier > 0) tempStr += "\n- Skill Level and Proficiency ( Rank remains )";
        if (tier > 1) tempStr += "\n- Equipment Level and Proficiency ( Dictionary Point and Mastery Effect remains )";
        return tempStr;
    }
    string DescriptionString()
    {
        string tempStr = optStr;
        //tempStr += "You can Rebirth as the same class when you reach Hero Level " + tDigit(currentRebirth.heroLevel) + "!";
        //tempStr += " Rebirth is a prestige mechanic that provies you access to powerful upgrades that will increase your overall power and how quickly you are able to progress through the game!";
        //tempStr += "\n\n";
        //Reset
        tempStr += ResetString();
        //tempStr += "<size=18>Tier " + tDigit(displayTier) + " Rebirth Resets :";
        //tempStr += "<size=18>\n- Hero Level and EXP\n- General Quest";
        //if (tier > 0) tempStr += "\n- Skill Level and Proficiency ( Rank remains )";
        //if (tier > 1) tempStr += "\n- Equipment Level and Proficiency ( Dictionary Point and Level Maxed Effect remains )";
        //Bonus
        tempStr += "\n\n<size=18>Tier " + tDigit(displayTier) + " Rebirth Bonus :";
        tempStr += "<size=18>";
        switch (tier)
        {
            case 0: tempStr += "\n- Additional Ability Point : [Max Hero Level Reached in Tier 1 (at most " + tDigit(game.rebirthCtrl.tier1AbilityPointBonusLevelCap.Value()) + ")] - " + RebirthParameter.tierHeroLevel[0]; break;
            //case 1: tempStr += "\n- Additional Ability Point every 25th Hero Level : [Max Hero Level Reached in Tier 2 (at most " + tDigit(game.rebirthCtrl.tier2AbilityPointBonusLevelCap.Value()) + ")] - 200\n- Tier 1 Rebirth Bonus Effect : + ([Tier 2 Rebirth #] x 10) %"; break;
            case 1: tempStr += "\n- After Rebirth, gain ([Max Hero Level Reached in Tier 2 (at most " + tDigit(game.rebirthCtrl.tier2AbilityPointBonusLevelCap.Value()) + ")] - " + RebirthParameter.tierHeroLevel[1] + ") Ability Point every 25th Hero Level\n- Tier 1 Rebirth Bonus Effect : + 10 x [Tier 2 Rebirth #]<sup>2/3</sup> %"; break;
            case 2: tempStr += "\n- Initial Base Attack Skill Level : [Max Hero Level Reached in Tier 3 (at most " + RebirthParameter.tierHeroLevel[3] + ")] - " + RebirthParameter.tierHeroLevel[2] + "\n- Tier 2 Rebirth Bonus Effect : + 10 x [Tier 3 Rebirth #]<sup>2/3</sup> %"; break;
        }

        tempStr += "\n\n<size=18>Tier " + tDigit(displayTier) + " Rebirth Time Played : " + DoubleTimeToDate(currentRebirth.RebirthPlaytime());
        if (currentRebirth.bestRebirthPlayTime > 0) tempStr += " ( Best : " + DoubleTimeToDate(currentRebirth.bestRebirthPlayTime) + " )"; 
        tempStr += "<size=18>\n" + localized.RebirthPointGain(currentRebirth);
        return tempStr;
    }
    string InfoString()
    {
        string tempStr = optStr + "<size=18>";
        tempStr += localized.RebirthInfo(currentRebirthForUpgrade);
        return tempStr;
    }

}

public class RebirthUpgradeUI
{
    public RebirthUpgradeUI(GameObject gameObject, int id)
    {
        thisObject = gameObject;
        this.id = id;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(PlusPoint);
    }
    public bool isEnable = false;
    public void Disable()
    {
        isEnable = false;
    }
    public void SetUI(RebirthUpgrade upgrade)
    {
        isEnable = true;
        thisUpgrade = () => upgrade;
    }
    public void UpdateUI()
    {
        SetActive(thisObject, isEnable);
        if (!isEnable) return;
        plusButton.interactable = Interactable();
        nameText.text = NameString();
        levelText.text = LevelString();
    }
    string NameString()
    {
        return localized.Rebirth(thisUpgrade()).name;
    }
    string LevelString()
    {
        return optStr + "<color=green>Lv " + tDigit(thisUpgrade().level.value);
    }
    bool Interactable()
    {
        return thisUpgrade().transaction.CanBuy();
    }
    void PlusPoint()
    {
        thisUpgrade().transaction.Buy(); ;
    }
    public string DescriptionString()
    {
        string tempString = optStr + "<size=20>" + NameString();
        return tempString;
    }


    GameObject thisObject;
    int id;
    public Func<RebirthUpgrade> thisUpgrade;
    TextMeshProUGUI nameText, levelText;
    Button plusButton;
}

public class RebirthUpgradePopupUI : PopupUI
{
    public RebirthUpgradePopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
    }
    public void ShowAction(RebirthUpgrade upgrade)
    {
        SetUI(() => DescriptionString(upgrade));
    }
    string DescriptionString(RebirthUpgrade upgrade)
    {
        string tempString = optStr + "<size=20>" + localized.Rebirth(upgrade).name + " < <color=green>Lv " + tDigit(upgrade.level.value) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- Max Level : Lv " + tDigit(upgrade.level.maxValue());
        if(upgrade.isGlobalEffect) tempString += optStr + "\n<color=yellow>- This upgrade has effects on all the heroes.</color>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Current) + " : " + localized.Rebirth(upgrade).effect;
        if (upgrade.level.IsMaxed()) return tempString;
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Next) + " : " + localized.Rebirth(upgrade).nextEffect;
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(upgrade.transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        return tempString;
    }
}