using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;

public class BattleStatusUI : MonoBehaviour
{
    private Func<BattleController> battleCtrl;
    private HeroKind heroKind => battleCtrl().heroKind;

    [SerializeField] Button autoMoveButton;
    [SerializeField] Button combatRangeButton;
    TextMeshProUGUI combatRangeText;

    [SerializeField] Image hpBar;
    [SerializeField] Image mpBar;
    [SerializeField] Image expBar;
    [SerializeField] Image goldBar;
    [SerializeField] public GameObject[] skillSlotObjects;
    [SerializeField] public GameObject[] globalSkillSlotObjects;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI detailStatsText, totalDPStext;
    [SerializeField] GameObject blackImage;

    SkillSlotUI[] skillSlots;
    SkillSlotUI[] globalSkillSlots;

    PopupUI statusPopupUI;
    PopupUI autoMovePopupUI;
    public PopupUI combatRangePopupUI;

    //Blessing
    public GameObject[] blessings;
    public BlessingUI[] blessingsUI;
    public BlessingPopupUI blessingPopupUI;
    //Debuff
    public GameObject[] debuffs;
    public DebuffingUI[] debuffsUI;
    public DebuffingPopupUI debuffingPopupUI;

    private void Awake()
    {
        battleCtrl = () => game.battleCtrl;

        combatRangeText = combatRangeButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        //Blessing
        blessingsUI = new BlessingUI[blessings.Length];
        for (int i = 0; i < blessingsUI.Length; i++)
        {
            int count = i;
            blessingsUI[i] = new BlessingUI(blessings[count], () => battleCtrl().blessingCtrl.blessings[count]);
        }
        blessingPopupUI = new BlessingPopupUI(gameUI.popupCtrlUI.defaultPopup);

        debuffsUI = new DebuffingUI[debuffs.Length];
        for (int i = 0; i < debuffsUI.Length; i++)
        {
            int count = i;
            debuffsUI[i] = new DebuffingUI(debuffs[count], () => battleCtrl().hero.debuffings[count + 1]);//Nothing除外のため+1
        }
        debuffingPopupUI = new DebuffingPopupUI(gameUI.popupCtrlUI.defaultPopup);

        autoMovePopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        combatRangePopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
    }

    public void SetSkillSlot()
    {
        SKILL tempSkill = game.skillCtrl.NullSkill();
        bool isAvailable = false;
        for (int i = 0; i < skillSlots.Length; i++)
        {
            int count = i;
            tempSkill = battleCtrl().skillSet.currentSkillSet[count];
            isAvailable = i < battleCtrl().limitedSkillNum;
            if (i >= battleCtrl().skillSet.maxSkillSlotNum.Value()) skillSlots[count].SetUI(sprite.lockedSlot, tempSkill, isAvailable);
            else if (tempSkill == game.skillCtrl.NullSkill()) skillSlots[count].SetUI(sprite.skillSlot, tempSkill, isAvailable);
            else
            {
                skillSlots[count].SetUI(sprite.skillIcons[(int)tempSkill.heroKind][tempSkill.id], tempSkill, isAvailable);
            }
        }
        for (int i = 0; i < globalSkillSlots.Length; i++)
        {
            int count = i;
            tempSkill = battleCtrl().skillSet.currentGlobalSkillSet[count];
            isAvailable = i < battleCtrl().limitedGlobalSkillNum;
            if (i >= battleCtrl().skillSet.maxGlobalSkillSlotNum.Value()) globalSkillSlots[count].SetUI(sprite.lockedSlot, tempSkill, isAvailable);
            else if (tempSkill == game.skillCtrl.NullSkill()) globalSkillSlots[count].SetUI(sprite.skillSlot, tempSkill, isAvailable);
            else globalSkillSlots[count].SetUI(sprite.skillIcons[(int)tempSkill.heroKind][tempSkill.id], tempSkill, isAvailable);
        }
    }

    void SwitchAutoMove()
    {
        battleCtrl().hero.isAutoMove = !battleCtrl().hero.isAutoMove;
        Sprite tempSprite;
        if (battleCtrl().hero.isAutoMove) tempSprite = sprite.autoMove;
        else tempSprite = sprite.manualMove;
        autoMoveButton.gameObject.GetComponent<Image>().sprite = tempSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        skillSlots = new SkillSlotUI[skillSlotObjects.Length];
        globalSkillSlots = new SkillSlotUI[globalSkillSlotObjects.Length];
        for (int i = 0; i < skillSlots.Length; i++)
        {
            int count = i;
            skillSlots[i] = new SkillSlotUI(skillSlotObjects[i]);
            skillSlots[i].thisButton.onClick.AddListener(() => battleCtrl().skillSet.EquipOrRemove(battleCtrl().skillSet.currentSkillSet[count]));
            skillSlots[i].thisButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Remove));
        }
        for (int i = 0; i < globalSkillSlots.Length; i++)
        {
            int count = i;
            globalSkillSlots[i] = new SkillSlotUI(globalSkillSlotObjects[i]);
            globalSkillSlots[i].thisButton.onClick.AddListener(() => battleCtrl().skillSet.EquipOrRemove(battleCtrl().skillSet.currentGlobalSkillSet[count]));
            skillSlots[i].thisButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Remove));
        }
        game.guildCtrl.Ability(GuildAbilityKind.GlobalSkillSlot).level.increaseUIAction = SetSkillSlot;

        autoMoveButton.onClick.AddListener(SwitchAutoMove);

        var clickAction = new ClickAction(combatRangeButton.gameObject, () => game.statsCtrl.heroes[(int)battleCtrl().heroKind].SwitchCombatRange(), () => game.statsCtrl.heroes[(int)battleCtrl().heroKind].SwitchCombatRange(true));
        //combatRangeButton.onClick.AddListener(() => game.statsCtrl.heroes[(int)battleCtrl().heroKind].SwitchCombatRange());

        //Popup
        statusPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        statusPopupUI.SetTargetObject(hpBar.gameObject, () => optStr + "<size=20>HP : " + tDigit(battleCtrl().hero.currentHp.value, 1) + " / " + tDigit(battleCtrl().hero.hp, 1) + " ( " + percent(battleCtrl().hero.HpPercent()) + " )");
        statusPopupUI.SetTargetObject(mpBar.gameObject, () => optStr + "<size=20>MP : " + tDigit(battleCtrl().hero.currentMp.value, 1) + " / " + tDigit(battleCtrl().hero.mp, 1) + " ( " + percent(battleCtrl().hero.MpPercent()) + " )");
        statusPopupUI.SetTargetObject(expBar.gameObject, () => optStr + "<size=20>EXP : " + tDigit(game.statsCtrl.Exp(heroKind).value) + " / " + tDigit(game.statsCtrl.RequiredExp(heroKind)) + " ( " + percent(game.statsCtrl.ExpPercent(heroKind)) + " )");
        statusPopupUI.SetTargetObject(goldBar.gameObject, () => optStr + "<size=20>Gold : " + tDigit(game.resourceCtrl.gold.value) + " / " + tDigit(game.resourceCtrl.goldCap.Value()) + " ( " + percent(game.resourceCtrl.gold.Percent()) + " )");
        autoMovePopupUI.SetTargetObject(autoMoveButton.gameObject, () => battleCtrl().hero.isAutoMove ? "Auto Move\n- WASD keys or arrow keys to move manually" : "Manual Move\n- WASD keys or arrow keys to move manually");
        combatRangePopupUI.SetTargetObject(combatRangeButton.gameObject, () => "Combat Range : " + meter(battleCtrl().hero.range));
        for (int i = 0; i < blessingsUI.Length; i++)
        {
            int count = i;
            blessingPopupUI.SetTargetObject(blessings[count], () => blessingPopupUI.BlessingString(blessingsUI[count].blessing));
        }
        for (int i = 0; i < debuffsUI.Length; i++)
        {
            int count = i;
            debuffingPopupUI.SetTargetObject(debuffs[count], () => debuffingPopupUI.DebuffingString(debuffsUI[count].debuffing));
        }
    }

    public void Initialize()
    {
        game.questCtrl.Quest(QuestKindTitle.SkillMaster1, game.currentHero).rewardUIAction = SetSkillSlot;
        game.questCtrl.Quest(QuestKindTitle.SkillMaster2, game.currentHero).rewardUIAction = SetSkillSlot;
        game.questCtrl.Quest(QuestKindTitle.SkillMaster3, game.currentHero).rewardUIAction = SetSkillSlot;
        battleCtrl().skillSet.SetUI(SetSkillSlot);
        SetSkillSlot();
    }


    public void UpdateUI()
    {
        hpBar.fillAmount = battleCtrl().hero.HpPercent();
        mpBar.fillAmount = battleCtrl().hero.MpPercent();
        expBar.fillAmount = game.statsCtrl.ExpPercent(heroKind);
        goldBar.fillAmount = game.resourceCtrl.GoldPercent();

        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].UpdateUI();
        }
        for (int i = 0; i < globalSkillSlots.Length; i++)
        {
            globalSkillSlots[i].UpdateUI();
        }
        UpdateText();
        for (int i = 0; i < blessingsUI.Length; i++)
        {
            blessingsUI[i].UpdateUI();
        }
        for (int i = 0; i < debuffsUI.Length; i++)
        {
            debuffsUI[i].UpdateUI();
        }
        //Popup
        statusPopupUI.UpdateUI();
        blessingPopupUI.UpdateUI();
        debuffingPopupUI.UpdateUI();
        autoMovePopupUI.UpdateUI();
        combatRangePopupUI.UpdateUI();


        SetActive(blackImage, main.S.isToggleOn[(int)ToggleKind.ShowDetailStats]);
    }

    void UpdateText()
    {
        statusText.text = StatusString();
        detailStatsText.text = DetailStatsString();
        combatRangeText.text = meter(battleCtrl().hero.range, 1, true);
        totalDPStext.text = optStr + "Total DPS : " + tDigit(game.battleCtrl.totalDamage.TotalGainInLastMinute() / 60d, 1);
    }

    string DetailStatsString()
    {
        if (!main.S.isToggleOn[(int)ToggleKind.ShowDetailStats]) return "";
        return optStr + "<size=18>EXP : " + tDigit(game.statsCtrl.Exp(heroKind).value) + " / " + tDigit(game.statsCtrl.RequiredExp(heroKind))
            + "\n<size=20>HP : " + tDigit(battleCtrl().hero.currentHp.value,1) + " / " + tDigit(battleCtrl().hero.hp,1)
            + "\nMP : " + tDigit(battleCtrl().hero.currentMp.value,1) + " / " + tDigit(battleCtrl().hero.mp,1);
    }
    string StatusString()
    {
        string tempString = optStr + "<size=20>";
        tempString += optStr + localized.Hero(heroKind) + " Lv " + tDigit(game.statsCtrl.Level(heroKind)) + "  ";
        tempString += optStr + localized.BasicStats(BasicStatsKind.ATK, true) + " " + game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).ValueString(1, false, true) + "  ";
        tempString += optStr + localized.BasicStats(BasicStatsKind.MATK, true) + " " + game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).ValueString(1, false, true) + "  ";
        tempString += optStr + localized.BasicStats(BasicStatsKind.DEF, true) + " " + game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).ValueString(1, false, true) + "  ";
        tempString += optStr + localized.BasicStats(BasicStatsKind.MDEF, true) + " " + game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).ValueString(1, false, true) + "  ";
        tempString += optStr + localized.BasicStats(BasicStatsKind.SPD, true) + " " + game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).ValueString(1, false, true) + "  ";
        tempString += "\n";
        tempString += optStr + localized.Stat(Stats.FireRes, true) + " " + game.statsCtrl.ElementResistance(heroKind, Element.Fire).ValueString(2, true, true) + "  ";
        tempString += optStr + localized.Stat(Stats.IceRes, true) + " " + game.statsCtrl.ElementResistance(heroKind, Element.Ice).ValueString(2, true, true) + "  ";
        tempString += optStr + localized.Stat(Stats.ThunderRes, true) + " " + game.statsCtrl.ElementResistance(heroKind, Element.Thunder).ValueString(2, true, true) + "  ";
        tempString += optStr + localized.Stat(Stats.LightRes, true) + " " + game.statsCtrl.ElementResistance(heroKind, Element.Light).ValueString(2, true, true) + "  ";
        tempString += optStr + localized.Stat(Stats.DarkRes, true) + " " + game.statsCtrl.ElementResistance(heroKind, Element.Dark).ValueString(2, true, true) + "  ";
        return tempString;
    }
}
public class SkillSlotUI
{
    Image thisImage;
    Image cooltimeImage;
    GameObject tenacityObject, redImageObject;
    TextMeshProUGUI mpText;
    public Button thisButton;
    SKILL skill;
    //GoUpTextUI levelupTextUI;
    TextMeshProUGUI dpsText;
    public SkillSlotUI(GameObject gameObject)
    {
        thisImage = gameObject.GetComponent<Image>();
        cooltimeImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        tenacityObject = gameObject.transform.GetChild(3).gameObject;
        redImageObject = gameObject.transform.GetChild(4).gameObject;
        thisButton = gameObject.GetComponent<Button>();
        mpText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        dpsText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        //levelupTextUI = new GoUpTextUI(gameObject.transform.GetChild(2).gameObject, Vector2.zero);
        //levelupTextUI.ShowText("Level Up");
    }
    public void SetUI(Sprite sprite, SKILL skill, bool isAvailable)
    {
        thisImage.sprite = sprite;
        this.skill = skill;
        SetActive(redImageObject, !isAvailable);
    }
    public void UpdateUI()
    {
        if (skill == game.skillCtrl.NullSkill())
        {
            cooltimeImage.fillAmount = 0;
            SetActive(dpsText.gameObject, false);
            SetActive(mpText.gameObject, false);
            SetActive(tenacityObject, false);
            return;
        }
        cooltimeImage.fillAmount = skill.CooltimePercent();
        bool isNotEnoughMp = skill.ConsumeMp() > game.battleCtrl.hero.mp;
        SetActive(tenacityObject, isNotEnoughMp && main.S.isToggleOn[(int)ToggleKind.SkillLessMPAvailable]);
        SetActive(mpText.gameObject, isNotEnoughMp && !main.S.isToggleOn[(int)ToggleKind.SkillLessMPAvailable]);
        SetActive(dpsText.gameObject, main.S.isToggleOn[(int)ToggleKind.ShowDPS]);
        dpsText.text = tDigit(skill.TotalDps(game.battleCtrl.hero, true), 1);
    }
}

public class BlessingUI
{
    public BlessingUI(GameObject gameObject, Func<BLESSING> blessing)
    {
        this.gameObject = gameObject;
        this.blessing = blessing;
        iconImage = gameObject.GetComponent<Image>();
        durationImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        iconImage.sprite = sprite.blessing[(int)blessing().kind];
    }
    public void UpdateUI()
    {
        if (blessing().IsTimeOver())
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);
        durationImage.fillAmount = 1f - blessing().TimeLeft() / (float)blessing().currentDuration;
    }
    public GameObject gameObject;
    Image iconImage;
    Image durationImage;
    public Func<BLESSING> blessing;

}
public class DebuffingUI
{
    public DebuffingUI(GameObject gameObject, Func<Debuffing> debuffing)
    {
        this.gameObject = gameObject;
        this.debuffing = debuffing;
        iconImage = gameObject.GetComponent<Image>();
        durationImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        iconImage.sprite = sprite.debuff[(int)debuffing().debuff];
    }
    public void UpdateUI()
    {
        if (!debuffing().isDebuff)
        {
            SetActive(gameObject, false);
            return;
        }
        SetActive(gameObject, true);
        durationImage.fillAmount = 1f - debuffing().TimeLeft() / (float)debuffing().duration;
    }
    public GameObject gameObject;
    Image iconImage;
    Image durationImage;
    public Func<Debuffing> debuffing;
}

public class BlessingPopupUI : PopupUI
{
    public BlessingPopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
    }
    public string BlessingString(Func<BLESSING> blessing)
    {
        string tempStr = optStr + "<size=20>";
        tempStr += blessing().NameString();
        tempStr += "\n<u>Effect</u>";
        tempStr += "\n- " + blessing().EffectString();
        if (blessing().SubEffectString() != "") tempStr += "\n- " + blessing().SubEffectString();
        tempStr += "\n<u>Duration Left</u>";
        tempStr += "\n- " + DoubleTimeToDate(blessing().TimeLeft());
        return tempStr;
    }
}
public class DebuffingPopupUI : PopupUI
{
    public DebuffingPopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
    }
    public string DebuffingString(Func<Debuffing> debuffing)
    {
        string tempStr = optStr + "<size=20>";
        tempStr += debuffing().NameString();
        tempStr += "\n<u>Effect</u>";
        tempStr += "\n- " + debuffing().EffectString();
        tempStr += "\n<u>Duration Left</u>";
        tempStr += "\n- " + DoubleTimeToDate(debuffing().TimeLeft());
        return tempStr;
    }
}

