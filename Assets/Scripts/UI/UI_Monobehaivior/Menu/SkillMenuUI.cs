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

public class SkillMenuUI : MENU_UI
{
    [SerializeField] GameObject[] skillTables;//[hero]
    SkillTableUI[] skillTablesUI;
    [SerializeField] Button[] heroSelectButtons;
    SwitchTabUI heroSwitchTabUI;
    public HeroKind heroKind { get => (HeroKind)heroSwitchTabUI.currentId; }
    [SerializeField] Button[] stanceSelectButtons;
    StanceSwitchTabUI stanceSwitchTabUI;
    TextMeshProUGUI[] stanceButtonTexts;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;
    //Popup
    public static SkillPopupUI skillPopupUI;
    public SkillRankupPopupUI skillRankupPopupUI;
    public StancePopupUI stancePopupUI;
    public PopupUI popupUI;

    void SetUI()
    {
        for (int i = 0; i < heroSelectButtons.Length; i++)
        {
            if (i == heroSwitchTabUI.currentId)
                heroSelectButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            else
                heroSelectButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = halftrans;
        }

        for (int i = 0; i < stanceSelectButtons.Length; i++)
        {
            int count = i;
            if (count < game.skillCtrl.classSkills[(int)heroKind].stances.Length)
            {
                SetActive(stanceSelectButtons[count].gameObject, true);
                stanceButtonTexts[count].text = localized.Stance(game.skillCtrl.classSkills[(int)heroKind].stances[count]).name;
            }
            else
            {
                SetActive(stanceSelectButtons[count].gameObject, false);
            }
        }
    }

    private void Awake()
    {
        heroSwitchTabUI = new SwitchTabUI(heroSelectButtons);
        stanceSwitchTabUI = new StanceSwitchTabUI(this, stanceSelectButtons, true);
        stanceButtonTexts = new TextMeshProUGUI[stanceSelectButtons.Length];
        for (int i = 0; i < stanceButtonTexts.Length; i++)
        {
            stanceButtonTexts[i] = stanceSelectButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
        skillPopupUI = new SkillPopupUI(gameUI.popupCtrlUI.skill);

        skillAbuseToggle = gameUI.menuUI.MenuUI(MenuKind.Setting).GetComponent<SettingMenuUI>().toggles[(int)ToggleKind.SkillLessMPAvailable].GetComponent<Toggle>();
    }
    // Start is called before the first frame update
    void Start()
    {
        heroSwitchTabUI.openAction = SetUI;
        stanceSwitchTabUI.openActionWithId = game.skillCtrl.classSkills[(int)heroKind].SwitchStance;

        skillTablesUI = new SkillTableUI[skillTables.Length];
        for (int i = 0; i < skillTablesUI.Length; i++)
        {
            skillTablesUI[i] = new SkillTableUI(skillTables[i], (HeroKind)i);
            //OpenClose
            for (int j = 0; j < heroSelectButtons.Length; j++)
            {
                if (j == i) skillTablesUI[i].thisOpenClose.SetOpenButton(heroSelectButtons[j]);
                else skillTablesUI[i].thisOpenClose.SetCloseButton(heroSelectButtons[j]);
            }
        }

        //Popup
        skillRankupPopupUI = new SkillRankupPopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < skillTablesUI.Length; i++)
        {
            for (int j = 0; j < skillTablesUI[i].skillsUI.Length; j++)
            {
                skillTablesUI[i].skillsUI[j].SetPopup(skillPopupUI);
                skillTablesUI[i].skillsUI[j].SetRankupPopup(skillRankupPopupUI);
            }
        }
        for (int i = 0; i < gameUI.battleStatusUI.skillSlotObjects.Length; i++)
        {
            int count = i;
            GameObject slot = gameUI.battleStatusUI.skillSlotObjects[i];
            skillPopupUI.SetTargetObject(slot, () => skillPopupUI.SetUI(game.battleCtrl.skillSet.currentSkillSet[count]));
        }
        for (int i = 0; i < gameUI.battleStatusUI.globalSkillSlotObjects.Length; i++)
        {
            int count = i;
            GameObject slot = gameUI.battleStatusUI.globalSkillSlotObjects[i];
            skillPopupUI.SetTargetObject(slot, () => skillPopupUI.SetUI(game.battleCtrl.skillSet.currentGlobalSkillSet[count]));
        }
        stancePopupUI = new StancePopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < stanceSelectButtons.Length; i++)
        {
            int count = i;
            stancePopupUI.SetTargetObject(stanceSelectButtons[count].gameObject, () => stancePopupUI.SetUI(game.skillCtrl.classSkills[(int)heroKind].stances[count]));
        }
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(skillAbuseToggle.gameObject, () => "<sprite=\"locks\" index=0> Epic Store", () => !game.epicStoreCtrl.Item(EpicStoreKind.SkillLessMpAvailable).IsPurchased());

        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Fox, MonsterColor.Red).pet, petQoLIcon, popupUI, "Auto-Rank");

        stanceSelectButtons[game.skillCtrl.classSkills[(int)heroKind].currentStanceId].onClick.Invoke();
    }

    public override void Initialize()
    {
        //現在の職業のSkillTableを開いておく
        heroSelectButtons[(int)game.currentHero].onClick.Invoke();
    }

    Toggle skillAbuseToggle;

    public override void UpdateUI()
    {
        base.UpdateUI();
        for (int i = 0; i < skillTablesUI.Length; i++)
        {
            skillTablesUI[i].UpdateUI();
        }
        //Popup
        skillRankupPopupUI.UpdateUI();
        stancePopupUI.UpdateUI();
        popupUI.UpdateUI();
        petQoLUI.UpdateUI();
        skillAbuseToggle.interactable = game.epicStoreCtrl.Item(EpicStoreKind.SkillLessMpAvailable).IsPurchased();

        for (int i = 0; i < heroSelectButtons.Length; i++)
        {
            int count = i;
            heroSelectButtons[i].interactable = game.guildCtrl.Member((HeroKind)count).IsUnlocked();
        }
    }
    //public void FixedUpdate()
    //{
    //    skillPopupUI.UpdateUI();//SkillSlotでは常に表示したいため、ここにかいた。何か他の方法があれば...
    //}
}

public class StanceSwitchTabUI : SwitchTabUI
{
    public StanceSwitchTabUI(SkillMenuUI skillMenuUI, Button[] buttons, bool isTextColorChange = false, int firstOpenId = -1, Action openAction = null, Action<int> openActionWithId = null) : base(buttons, isTextColorChange, firstOpenId, openAction, openActionWithId)
    {
        this.skillMenuUI = skillMenuUI;
    }
    SkillMenuUI skillMenuUI;
    public override int currentId { get => game.skillCtrl.classSkills[(int)skillMenuUI.heroKind].currentStanceId; set => game.skillCtrl.classSkills[(int)skillMenuUI.heroKind].currentStanceId = value; }
}

public class SkillTableUI
{
    HeroKind heroKind;
    GameObject thisObject;
    public OpenCloseUI thisOpenClose;
    public SkillUI[] skillsUI;

    public SkillTableUI(GameObject thisObject, HeroKind heroKind)
    {
        this.thisObject = thisObject;
        this.heroKind = heroKind;
        thisOpenClose = new OpenCloseUI(thisObject, false, false, true);

        skillsUI = new SkillUI[thisObject.transform.childCount];
        for (int i = 0; i < skillsUI.Length; i++)
        {
            skillsUI[i] = new SkillUI(thisObject.transform.GetChild(i).gameObject, heroKind, i);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < skillsUI.Length; i++)
        {
            skillsUI[i].UpdateUI();
        }
    }
}

public class SkillUI
{
    HeroKind heroKind;
    int id;//skillId

    GameObject thisObject, backgroundObject;
    Button iconButton;
    GameObject equipFrame;
    TextMeshProUGUI description;
    TextMeshProUGUI rankText;
    Button rankupButton;
    Slider proficiencySlider;
    SKILL thisSkill;
    GameObject lockObject;
    TextMeshProUGUI lockText;
    public SkillUI(GameObject thisObject, HeroKind heroKind, int id)
    {
        this.thisObject = thisObject;
        this.heroKind = heroKind;
        this.id = id;
        backgroundObject = thisObject.transform.GetChild(0).gameObject;
        iconButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        equipFrame = iconButton.gameObject.transform.GetChild(1).gameObject;
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        rankText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        rankupButton = thisObject.transform.GetChild(5).gameObject.GetComponent<Button>();
        proficiencySlider = thisObject.transform.GetChild(6).gameObject.GetComponent<Slider>();
        lockObject = thisObject.transform.GetChild(7).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        thisSkill = game.skillCtrl.Skill(heroKind, id);
        SetUI();
    }
    public void UpdateUI()
    {
        SetActive(lockObject, !thisSkill.IsUnlocked());
        if (!thisSkill.IsUnlocked())
        {
            lockText.text = LockString();
            return;
        }
        proficiencySlider.value = thisSkill.ProficiencyPercent();
        description.text = DescriptionString();
        rankText.text = RankString();
        rankupButton.interactable = thisSkill.rankupTransaction.CanBuy();
        iconButton.interactable = thisSkill.CanEquip();
        SetActive(equipFrame, game.battleCtrl.skillSet.IsEquipped(thisSkill));
    }
    string LockString()
    {
        string tempString = optStr + "<size=18><sprite=\"locks\" index=0> Required Skill<size=18>";
        foreach (var item in thisSkill.requiredSkills)
        {
            tempString += optStr + "\n- " + localized.SkillName(thisSkill.heroKind, item.Key) + " Lv " + tDigit(item.Value);
        }
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStr + "<size=20>" + localized.SkillName(heroKind, id);
        switch (thisSkill.type)
        {
            case SkillType.Attack:
                tempString += optStr + "\n<size=16>DPS : " + tDigit(thisSkill.Dps(game.battleCtrl.hero, true), 2);
                if (thisSkill.HitCount() > 1) tempString += optStr + " * " + tDigit(thisSkill.HitCount());
                break;
            case SkillType.Buff:
                tempString += optStr + "\n<size=16>";
                break;
            case SkillType.Heal:
                tempString += optStr + "\n<size=16>";
                break;
            case SkillType.Order:
                tempString += optStr + "\n<size=16>";
                break;
        }
        tempString += "\n<color=green>Lv " + tDigit(thisSkill.level.value);
        if (thisSkill.levelBonus > 0) tempString += optStr + " +" + tDigit(thisSkill.levelBonus);

        tempString += "</color> / " + tDigit(thisSkill.MaxLevel());
        return tempString;
    }
    string RankString()
    {
        return optStr + "<size=18><color=orange>Rank " + tDigit(thisSkill.Rank());
    }
    void SetUI()
    {
        SetSprite();
        SetButton();
        SetBackgroundColor();
    }
    void SetSprite()
    {
        rankupButton.gameObject.GetComponent<Image>().sprite = sprite.resourcesBackgroundBlack[(int)thisSkill.resourceKind];
        iconButton.gameObject.GetComponent<Image>().sprite = sprite.skillIcons[(int)heroKind][id];
    }
    void SetButton()
    {
        iconButton.onClick.AddListener(() => game.battleCtrl.skillSet.EquipOrRemove(thisSkill));
        iconButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Equip));
        rankupButton.onClick.AddListener(() => thisSkill.rankupTransaction.Buy());
        rankupButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
    }
    void SetBackgroundColor()
    {
        Color tempColor = blue;
        switch (thisSkill.resourceKind)
        {
            case ResourceKind.Stone:
                tempColor = blue;
                break;
            case ResourceKind.Crystal:
                tempColor = yellow;
                break;
            case ResourceKind.Leaf:
                tempColor = green;
                break;
        }
        thisObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = tempColor;
    }
    public void SetPopup(SkillPopupUI popupUI)
    {
        popupUI.SetTargetObject(backgroundObject, () => popupUI.SetUI(thisSkill));
        popupUI.SetTargetObject(iconButton.gameObject, () => popupUI.SetUI(thisSkill));
        popupUI.SetTargetObject(rankupButton.gameObject, () => popupUI.SetUI(thisSkill));
    }
    public void SetRankupPopup(SkillRankupPopupUI popupUI)
    {
        popupUI.SetTargetObject(rankupButton.gameObject, () => popupUI.SetUI(thisSkill));
    }
    Color blue = new Color(0f / 255, 50f / 255, 150f / 255, 200f / 255);
    Color yellow = new Color(200f / 255, 100f / 255, 0f / 255, 200f / 255);
    Color green = new Color(0f / 255, 100f / 255, 0f / 255, 200f / 255);

}

//Popup
public class StancePopupUI : PopupUI
{
    public StancePopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
    }
    public void SetUI(Stance stance)
    {
        if (stance.id <= 0) return;
        base.SetUI(() => String(stance));
    }
    string String(Stance stance)
    {
        string tempString = optStr + "<size=20>";
        tempString += localized.Stance(stance).name + " Stance";
        tempString += "<size=20>\n";
        tempString += "- " + localized.Stance(stance).effect;
        return tempString;
    }
}

public class SkillPopupUI : PopupUI
{
    public SKILL thisSkill;
    Image icon;
    TextMeshProUGUI description, effectText, passiveText;
    public SkillPopupUI(GameObject thisObject) : base(thisObject)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        description = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        effectText = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        passiveText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(SKILL thisSkill)
    {
        if (thisSkill == game.skillCtrl.NullSkill()) return;
        //if (!thisSkill.IsUnlocked()) return;
        this.thisSkill = thisSkill;
        icon.sprite = sprite.skillIcons[(int)thisSkill.heroKind][thisSkill.id];
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        description.text = DescriptionString(thisSkill);
        effectText.text = EffectString(thisSkill);
        passiveText.text = PassiveEffectString(thisSkill);
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        SetMouseFollow();
        UpdateText();
    }

    string DescriptionString(SKILL thisSkill)
    {
        string tempString = optStr + "<size=20>" + localized.SkillName(thisSkill.heroKind, thisSkill.id);
        tempString += optStr + "   <color=orange>< Rank " + tDigit(thisSkill.Rank()) + " ></color>";
        tempString += optStr + "\n<size=18><color=green>Lv " + tDigit(thisSkill.level.value);
        if (thisSkill.levelBonus > 0) tempString += optStr + " +" + tDigit(thisSkill.levelBonus);
        tempString += "</color> / " + tDigit(thisSkill.MaxLevel());
        tempString += "\n" + localized.Basic(BasicWord.Proficiency) + " : " + tDigit(thisSkill.proficiency.value) + " / " + tDigit(thisSkill.RequiredProficiency()) + " ( " + percent(thisSkill.ProficiencyPercent()) + " )";
        tempString += "\nTriggered # " + tDigit(thisSkill.triggeredNum.value);
        return tempString;
    }
    string EffectString(SKILL thisSkill)
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u>";
        tempString += "<size=18>";
        //追加説明
        if (thisSkill.Description() != "") tempString += "\n- " + thisSkill.Description();
        //Damage
        switch (thisSkill.type)
        {
            case SkillType.Attack:
                tempString += "\n";
                tempString += optStr + "- " + localized.SkillEffect((EffectKind)thisSkill.element) + " : " + tDigit(thisSkill.Damage(game.battleCtrl.hero, true), 2);
                if (thisSkill.HitCount() > 1) tempString += optStr + " * " + tDigit(thisSkill.HitCount());
                tempString += optStr + " ( + " + tDigit(thisSkill.IncrementDamagePerLevel(game.battleCtrl.hero), 2) + " / Lv )";
                break;
            case SkillType.Buff:
                if (thisSkill.buff == Buff.Nothing) { }
                else if (thisSkill.buff == Buff.SkillLevelUp)
                {
                    tempString += "\n";
                    tempString += optStr + "- " + localized.BuffName(thisSkill.buff) + " : + " + tDigit(thisSkill.BuffPercent());
                    tempString += optStr + " ( + " + tDigit(thisSkill.IncrementBuffPercentPerLevel(), 2) + " / Lv )";
                }
                else
                {
                    tempString += "\n";
                    tempString += optStr + "- " + localized.BuffName(thisSkill.buff) + " : ";
                    if (thisSkill.BuffPercent() < 0) tempString += " - " + percent(-thisSkill.BuffPercent(), 3);
                    else tempString += " + " + percent(thisSkill.BuffPercent(), 3);
                    tempString += optStr + " ( + " + percent(thisSkill.IncrementBuffPercentPerLevel(), 3) + " / Lv )";
                }
                break;
            case SkillType.Heal:
                tempString += "\n";
                tempString += optStr + "- Restore HP + " + tDigit(thisSkill.HealPoint(), 2);
                tempString += optStr + " ( + " + tDigit(thisSkill.IncrementHealPointPerLevel(), 2) + " / Lv )";
                break;
            case SkillType.Order:
                tempString += "\n";
                tempString += optStr + "- Loyalty Point : + " + tDigit(game.statsCtrl.heroes[(int)game.currentHero].loyaltyPoingGain.Value(), 2) + " per trigger";
                break;
        }
        //Debuff
        if (thisSkill.debuff != Debuff.Nothing)
        {
            tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.DebuffKind) + " [ " + localized.DebuffName(thisSkill.debuff) + " ] Chance : " + percent(thisSkill.DebuffChance(), 2);
            if (thisSkill.incrementDebuffChance > 0)
                tempString += optStr + "  ( + " + percent(thisSkill.incrementDebuffChance, 2) + " / Lv )";
        }
        //Mp
        if (thisSkill.GainMp() > 0)
        {
            tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.MPGain) + " : " + tDigit(thisSkill.GainMp(), 2) + " / " + localized.Basic(BasicWord.Sec);
            tempString += optStr + "  ( + " + tDigit(thisSkill.IncrementMpGainPerLevel(), 2) + " / Lv )";
        }
        if (thisSkill.ConsumeMp() > 0)
        {
            tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.MPConsumption) + " : " + tDigit(thisSkill.ConsumeMp(), 2);
            tempString += optStr + "  ( + " + tDigit(thisSkill.incrementMpConsume, 2) + " / Lv )";
            if (thisSkill.ConsumeMp() > game.battleCtrl.hero.mp) tempString += " <color=yellow>Not enough MP!</color>";
        }
        if (thisSkill.ChanneledMp() > 0)
        {
            tempString += optStr + "\n- Channeled MP : " + tDigit(thisSkill.ChanneledMp(), 2) + "  ( + " + tDigit(thisSkill.incrementMpConsume, 2) + " / Lv )";
            tempString += "\n<color=yellow>- Channeled MP decreases Max MP while equipped, instead of consuming MP on trigger</color>";
        }
        //Cooltime
        if (thisSkill.type != SkillType.Buff)
            tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.Cooltime) + " : " + tDigit(thisSkill.Interval(game.battleCtrl.hero), 3) + " " + localized.Basic(BasicWord.Sec);
        switch (thisSkill.type)
        {
            case SkillType.Attack:
                //Range
                tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.Range) + " : " + meter(thisSkill.Range());
                //EffectRange
                tempString += optStr + "\n- " + localized.SkillEffect(EffectKind.EffectRange) + " : " + meter(thisSkill.EffectRange());
                break;
        }
        return tempString;
    }
    string PassiveEffectString(SKILL thisSkill)
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.PassiveEffect) + "</u>";
        tempString += optStr + "\n<size=18>";
        for (int i = 0; i < thisSkill.passiveEffectLists.Count; i++)
        {
            SkillPassiveEffect tempEffect = thisSkill.passiveEffectLists[i];
            if (tempEffect.IsEnoughLevel()) tempString += "<color=green>";
            tempString += optStr + "- Lv " + tDigit(tempEffect.requiredLevel) + " : ";
            //MaxLevelがそれに達していれば表示、そうでなければ「???」
            if (thisSkill.MaxLevel() >= tempEffect.requiredLevel)
            {
                switch (tempEffect.effectKind)
                {
                    case SkillPassiveEffectKind.BasicStats:
                        tempString += localized.BasicStats(tempEffect.basicStatsKind);
                        tempString += " + ";
                        if (tempEffect.multiplierType == MultiplierType.Add)
                            tempString += tDigit(tempEffect.value);
                        else
                            tempString += percent(tempEffect.value, 2);
                        break;
                    case SkillPassiveEffectKind.HeroStats:
                        tempString += localized.Stat(tempEffect.statsKind);
                        tempString += " + ";
                        if (tempEffect.multiplierType == MultiplierType.Add)
                        {
                            switch (tempEffect.statsKind)
                            {
                                case Stats.FireRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.IceRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.ThunderRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.LightRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.DarkRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.DebuffRes:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.PhysCritChance:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.MagCritChance:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.EquipmentDropChance:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.MoveSpeed:
                                    tempString += meter(tempEffect.value) + " / " + localized.Basic(BasicWord.Sec);
                                    break;
                                case Stats.SkillProficiencyGain:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.EquipmentProficiencyGain:
                                    tempString += percent(tempEffect.value);
                                    break;
                                case Stats.ExpGain:
                                    tempString += percent(tempEffect.value);
                                    break;
                                default:
                                    tempString += tDigit(tempEffect.value);
                                    break;
                            }
                        }
                        else
                            tempString += percent(tempEffect.value, 2);
                        break;
                    case SkillPassiveEffectKind.GlobalStats:
                        tempString += localized.GlobalStat(tempEffect.globalStatsKind) + " + " + percent(tempEffect.value, 2);
                        break;
                    case SkillPassiveEffectKind.Others:
                        tempString += tempEffect.description;
                        break;
                }
            }
            else
            {
                tempString += "???";
                break;
            }
            tempString += "</color>\n";
        }
        return tempString;
    }

}
public class SkillRankupPopupUI : PopupUI
{
    TextMeshProUGUI text;
    public SkillRankupPopupUI(GameObject thisObject) : base(thisObject)
    {
        text = thisObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void UpdateUI()
    {
        if (!isShow) return;
        base.UpdateUI();
        text.text = RankupCostString(thisSkill);
    }
    SKILL thisSkill;
    public void SetUI(SKILL thisSkill)
    {
        if (!thisSkill.IsUnlocked()) return;
        this.thisSkill = thisSkill;
        text.text = RankupCostString(thisSkill);
        DelayShow();
    }
    string RankupCostString(SKILL thisSkill)
    {
        string tempString = "";
        //tempString += optStr + "<size=20>" + localized.SkillName(heroKind, id);
        //tempString += optStr + "   <color=orange>< Rank " + tDigit(thisSkill.Rank()) + " ></color>";
        //tempString += optStr + "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.MaterialsToRankUp) + "  ( <color=green>+ " + tDigit(thisSkill.rankupTransaction.LevelIncrement()) + "</color> ) " + "</u>";
        tempString += optStr + "\n<size=8>\n<size=18>";
        tempString += optStr + "- " + localized.ResourceName(thisSkill.resourceKind);
        tempString += optStr + " : ";
        if (thisSkill.rankupTransaction.CanBuy()) tempString += "<color=green>";
        else tempString += "<color=red>";
        tempString += tDigit(thisSkill.rankupTransaction.ResourceValue()) + "</color> / ";
        tempString += tDigit(thisSkill.rankupTransaction.Cost());

        return tempString;
    }

    public override void SetMouseFollow()
    {
        if (!isShow) return;

        thisRect.anchorMin = Vector2.one * 0.5f;
        thisRect.anchorMax = Vector2.one * 0.5f;
        thisRect.pivot = Vector2.one * 0.5f;
        float correction = gameUI.autoCanvasScaler.mouseCorrection;
        Vector3 position = Input.mousePosition * correction;
        CheckMousePosition(position);
        position -= gameUI.autoCanvasScaler.defaultScreenSize * 0.5f;
        if (position.y >= 0 && position.x >= 0)//第一象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x * 0.75f, thisRect.sizeDelta.y * 0.35f);
        else if (position.y >= 0 && position.x < 0)//第二象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x * 0.75f, thisRect.sizeDelta.y * 0.35f);
        else if (position.y < 0 && position.x >= 0)//第四象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x * 0.75f, -thisRect.sizeDelta.y * 0.35f);
        else if (position.y < 0 && position.x < 0)//第三象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x * 0.75f, -thisRect.sizeDelta.y * 0.35f);
    }

}