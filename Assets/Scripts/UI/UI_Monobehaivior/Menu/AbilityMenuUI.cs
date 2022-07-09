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

public class AbilityMenuUI : MENU_UI
{
    [SerializeField] GameObject heroInfo;
    [SerializeField] TextMeshProUGUI heroInfoText;
    [SerializeField] Image heroImage;
    [SerializeField] GameObject[] abilities;
    AbilityUI[] abilitiesUI;
    [SerializeField] GameObject[] basicStats, heroStats, globalStats;
    BasicStatsUI[] basicStatsUI;
    StatsUI[] heroStatsUI;
    GlobalStatsUI[] globalStatsUI;
    [SerializeField] GameObject[] titles;
    TitleUI[] titlesUI;
    [SerializeField] GameObject statsCanvasObject, titleCanvasObject;
    OpenCloseUI statsOpenCloseUI, titleOpenCloseUI;
    [SerializeField] Button statsOpenButton;
    public Button titleOpenButton;
    [SerializeField] TextMeshProUGUI pointText;
    StatsPopupUI statsPopupUI;
    AbilityPopupUI abilityPopupUI;
    TitlePopupUI titlePopupUI;
    PopupUI popupUI;

    HeroKind heroKind => game.currentHero;

    //AutoAbility
    [SerializeField] GameObject autoAbilityCanvas;
    [SerializeField] Button openAutoAbilityButton, closeAutoAbilityButton;
    [SerializeField] Toggle autoAbilityPointToggle;
    [SerializeField] TMP_InputField[] abilityInputFields;
    [SerializeField] TMP_InputField[] maxAbilityInputFields;
    PopupUI autoAbilityPopupUI;
    void SetUIAutoAbilityPoint()
    {
        bool tempIsOn = main.S.isAutoAbilityPointPresets[(int)heroKind];
        abilityInputFields[0].SetTextWithoutNotify(main.S.autoAbilityPointPresetsVIT[(int)heroKind].ToString());
        abilityInputFields[1].SetTextWithoutNotify(main.S.autoAbilityPointPresetsSTR[(int)heroKind].ToString());
        abilityInputFields[2].SetTextWithoutNotify(main.S.autoAbilityPointPresetsINT[(int)heroKind].ToString());
        abilityInputFields[3].SetTextWithoutNotify(main.S.autoAbilityPointPresetsAGI[(int)heroKind].ToString());
        abilityInputFields[4].SetTextWithoutNotify(main.S.autoAbilityPointPresetsLUK[(int)heroKind].ToString());
        maxAbilityInputFields[0].SetTextWithoutNotify(main.S.autoAbilityPointMaxVIT[(int)heroKind].ToString());
        maxAbilityInputFields[1].SetTextWithoutNotify(main.S.autoAbilityPointMaxSTR[(int)heroKind].ToString());
        maxAbilityInputFields[2].SetTextWithoutNotify(main.S.autoAbilityPointMaxINT[(int)heroKind].ToString());
        maxAbilityInputFields[3].SetTextWithoutNotify(main.S.autoAbilityPointMaxAGI[(int)heroKind].ToString());
        maxAbilityInputFields[4].SetTextWithoutNotify(main.S.autoAbilityPointMaxLUK[(int)heroKind].ToString());
        autoAbilityPointToggle.SetIsOnWithoutNotify(tempIsOn);
    }
    OpenCloseUI autoAbilityOpenCloseUI;

    public override void Initialize()
    {
        //SetUI
        heroImage.sprite = sprite.heroesWholebody[(int)game.currentHero];
        SetUIAutoAbilityPoint();
        autoAbilityOpenCloseUI.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        abilitiesUI = new AbilityUI[abilities.Length];
        for (int i = 0; i < abilitiesUI.Length; i++)
        {
            abilitiesUI[i] = new AbilityUI(abilities[i].gameObject, (AbilityKind)i);
        }

        basicStatsUI = new BasicStatsUI[basicStats.Length];
        for (int i = 0; i < basicStats.Length; i++)
        {
            basicStatsUI[i] = new BasicStatsUI(basicStats[i], (BasicStatsKind)i);
        }
        heroStatsUI = new StatsUI[heroStats.Length];
        for (int i = 0; i < heroStats.Length; i++)
        {
            heroStatsUI[i] = new StatsUI(heroStats[i], (Stats)i);
        }
        globalStatsUI = new GlobalStatsUI[globalStats.Length];
        for (int i = 0; i < globalStats.Length; i++)
        {
            globalStatsUI[i] = new GlobalStatsUI(globalStats[i], (GlobalStats)i);
        }
        titlesUI = new TitleUI[titles.Length];
        for (int i = 0; i < titlesUI.Length; i++)
        {
            titlesUI[i] = new TitleUI(titles[i], (TitleKind)i);
        }

        //Popup
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(heroInfo, () => HeroInfoPopupString());
        statsPopupUI = new StatsPopupUI(gameUI.popupCtrlUI.stats);
        for (int i = 0; i < basicStatsUI.Length; i++)
        {
            int count = i;
            statsPopupUI.SetTargetObject(basicStats[count], () => statsPopupUI.SetUI(basicStatsUI[count]));
        }
        for (int i = 0; i < heroStatsUI.Length; i++)
        {
            int count = i;
            statsPopupUI.SetTargetObject(heroStats[count], () => statsPopupUI.SetUI(heroStatsUI[count]));
        }
        for (int i = 0; i < globalStatsUI.Length; i++)
        {
            int count = i;
            statsPopupUI.SetTargetObject(globalStats[count], () => statsPopupUI.SetUI(globalStatsUI[count]));
        }
        abilityPopupUI = new AbilityPopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < abilities.Length; i++)
        {
            int count = i;
            abilityPopupUI.SetTargetObject(abilities[count], () => abilityPopupUI.SetUI(abilitiesUI[count].DescriptionString));
        }
        titlePopupUI = new TitlePopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < titlesUI.Length; i++)
        {
            int count = i;
            titlePopupUI.SetTargetObject(titles[count], () => titlePopupUI.ShowAction(titlesUI[count].kind));
        }
        //OpenClose
        statsOpenCloseUI = new OpenCloseUI(statsCanvasObject, false, false, false, true);
        statsOpenCloseUI.SetOpenButton(statsOpenButton);
        statsOpenCloseUI.SetCloseButton(titleOpenButton);
        titleOpenCloseUI = new OpenCloseUI(titleCanvasObject);
        titleOpenCloseUI.SetOpenButton(titleOpenButton);
        titleOpenCloseUI.SetCloseButton(statsOpenButton);

        //AutoAbility
        autoAbilityOpenCloseUI = new OpenCloseUI(autoAbilityCanvas);
        autoAbilityOpenCloseUI.SetOpenButton(openAutoAbilityButton);
        autoAbilityOpenCloseUI.SetCloseButton(closeAutoAbilityButton);
        autoAbilityPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < abilityInputFields.Length; i++)
        {
            int count = i;
            autoAbilityPopupUI.SetTargetObject(abilityInputFields[count].gameObject, () => "<sprite=\"locks\" index=0> Global Quest [ Tutorial 11 : Challenge ]", () => !game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared);// + game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).NameString() + " in Epic Store");
        }
        for (int i = 0; i < maxAbilityInputFields.Length; i++)
        {
            int count = i;
            autoAbilityPopupUI.SetTargetObject(maxAbilityInputFields[count].gameObject, () => "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).NameString() + " in Epic Store", () => !game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased());
        }
        autoAbilityPopupUI.SetTargetObject(autoAbilityPointToggle.gameObject, () => "<sprite=\"locks\" index=0> Global Quest [ Tutorial 11 : Challenge ]", () => !game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared);// + game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).NameString() + " in Epic Store");
        //autoAbilityPopupUI.additionalShowCondition = () => !game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared;//!game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased();
        abilityInputFields[0].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointPresetsVIT[(int)heroKind] = 0; abilityInputFields[0].text = "0"; } else main.S.autoAbilityPointPresetsVIT[(int)heroKind] = long.Parse(valueString); });
        abilityInputFields[1].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointPresetsSTR[(int)heroKind] = 0; abilityInputFields[1].text = "0"; } else main.S.autoAbilityPointPresetsSTR[(int)heroKind] = long.Parse(valueString); });
        abilityInputFields[2].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointPresetsINT[(int)heroKind] = 0; abilityInputFields[2].text = "0"; } else main.S.autoAbilityPointPresetsINT[(int)heroKind] = long.Parse(valueString); });
        abilityInputFields[3].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointPresetsAGI[(int)heroKind] = 0; abilityInputFields[3].text = "0"; } else main.S.autoAbilityPointPresetsAGI[(int)heroKind] = long.Parse(valueString); });
        abilityInputFields[4].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointPresetsLUK[(int)heroKind] = 0; abilityInputFields[4].text = "0"; } else main.S.autoAbilityPointPresetsLUK[(int)heroKind] = long.Parse(valueString); });
        maxAbilityInputFields[0].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointMaxVIT[(int)heroKind] = 0; maxAbilityInputFields[0].text = "0"; } else main.S.autoAbilityPointMaxVIT[(int)heroKind] = long.Parse(valueString); });
        maxAbilityInputFields[1].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointMaxSTR[(int)heroKind] = 0; maxAbilityInputFields[1].text = "0"; } else main.S.autoAbilityPointMaxSTR[(int)heroKind] = long.Parse(valueString); });
        maxAbilityInputFields[2].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointMaxINT[(int)heroKind] = 0; maxAbilityInputFields[2].text = "0"; } else main.S.autoAbilityPointMaxINT[(int)heroKind] = long.Parse(valueString); });
        maxAbilityInputFields[3].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointMaxAGI[(int)heroKind] = 0; maxAbilityInputFields[3].text = "0"; } else main.S.autoAbilityPointMaxAGI[(int)heroKind] = long.Parse(valueString); });
        maxAbilityInputFields[4].onEndEdit.AddListener((valueString) => { if (valueString == "") { main.S.autoAbilityPointMaxLUK[(int)heroKind] = 0; maxAbilityInputFields[4].text = "0"; } else main.S.autoAbilityPointMaxLUK[(int)heroKind] = long.Parse(valueString); });
        autoAbilityPointToggle.onValueChanged.AddListener((isOn) => {
            for (int i = 0; i < abilityInputFields.Length; i++)
            {
                if (abilityInputFields[i].text == "")
                {
                    abilityInputFields[i].text = "0";
                }
                switch (i)
                {
                    case 0: main.S.autoAbilityPointPresetsVIT[(int)heroKind] = long.Parse(abilityInputFields[0].text); break;
                    case 1: main.S.autoAbilityPointPresetsSTR[(int)heroKind] = long.Parse(abilityInputFields[1].text); break;
                    case 2: main.S.autoAbilityPointPresetsINT[(int)heroKind] = long.Parse(abilityInputFields[2].text); break;
                    case 3: main.S.autoAbilityPointPresetsAGI[(int)heroKind] = long.Parse(abilityInputFields[3].text); break;
                    case 4: main.S.autoAbilityPointPresetsLUK[(int)heroKind] = long.Parse(abilityInputFields[4].text); break;
                }
            }
            for (int i = 0; i < maxAbilityInputFields.Length; i++)
            {
                if (maxAbilityInputFields[i].text == "")
                {
                    maxAbilityInputFields[i].text = "0";
                }
                switch (i)
                {
                    case 0: main.S.autoAbilityPointMaxVIT[(int)heroKind] = long.Parse(maxAbilityInputFields[0].text); break;
                    case 1: main.S.autoAbilityPointMaxSTR[(int)heroKind] = long.Parse(maxAbilityInputFields[1].text); break;
                    case 2: main.S.autoAbilityPointMaxINT[(int)heroKind] = long.Parse(maxAbilityInputFields[2].text); break;
                    case 3: main.S.autoAbilityPointMaxAGI[(int)heroKind] = long.Parse(maxAbilityInputFields[3].text); break;
                    case 4: main.S.autoAbilityPointMaxLUK[(int)heroKind] = long.Parse(maxAbilityInputFields[4].text); break;
                }
            }

            main.S.isAutoAbilityPointPresets[(int)heroKind] = isOn;
        });
        for (int i = 0; i < abilityInputFields.Length; i++)
        {
            abilityInputFields[i].onValueChanged.AddListener((valueString) => autoAbilityPointToggle.isOn = false);
        }
        for (int i = 0; i < maxAbilityInputFields.Length; i++)
        {
            maxAbilityInputFields[i].onValueChanged.AddListener((valueString) => autoAbilityPointToggle.isOn = false);
        }
        SetUIAutoAbilityPoint();

        openClose.closeActions.Add(() => autoAbilityOpenCloseUI.Close());

        //Debug
        gameUI.debugCtrl.abilityPointBox.GetComponent<Button>().onClick.AddListener(() => game.statsCtrl.Exp(game.currentHero).Increase(game.statsCtrl.RequiredExp(game.currentHero)));
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        heroInfoText.text = HeroInfoString();
        pointText.text = optStr + localized.AbilityWord(AbilityWord.PointLeft) + "\n" + tDigit(game.statsCtrl.AbilityPointLeft(game.currentHero).value);

        for (int i = 0; i < abilitiesUI.Length; i++)
        {
            abilitiesUI[i].UpdateUI();
        }
        if (statsOpenCloseUI.isOpen)
        {
            for (int i = 0; i < basicStatsUI.Length; i++)
            {
                basicStatsUI[i].UpdateUI();
            }
            for (int i = 0; i < heroStatsUI.Length; i++)
            {
                heroStatsUI[i].UpdateUI();
            }
            for (int i = 0; i < globalStatsUI.Length; i++)
            {
                globalStatsUI[i].UpdateUI();
            }
            statsPopupUI.UpdateUI();
        }
        if (titleOpenCloseUI.isOpen)
        {
            for (int i = 0; i < titlesUI.Length; i++)
            {
                titlesUI[i].UpdateUI();
            }
            titlePopupUI.UpdateUI();
        }
        abilityPopupUI.UpdateUI();
        popupUI.UpdateUI();

        autoAbilityPopupUI.UpdateUI();
        autoAbilityPointToggle.interactable = game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared;//game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased();
        for (int i = 0; i < abilityInputFields.Length; i++)
        {
            abilityInputFields[i].interactable = game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared;//game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased();
        }
        for (int i = 0; i < maxAbilityInputFields.Length; i++)
        {
            maxAbilityInputFields[i].interactable = game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased();
        }
    }
    string HeroInfoString()
    {
        return optStr + "<size=24>" + localized.Hero(game.currentHero) + "  < <color=green>Lv " + tDigit(game.statsCtrl.Level(game.currentHero))
            + "</color> >    <size=20>EXP  :  " + tDigit(game.statsCtrl.Exp(game.currentHero).value) + " / " + tDigit(game.statsCtrl.RequiredExp(game.currentHero))
            + "  ( " + percent(game.statsCtrl.ExpPercent(game.currentHero), 3) + " )";
    }

    string HeroInfoPopupString()
    {
        string tempStr = optStr;
        tempStr += localized.Hero(game.currentHero) + "  < <color=green>Lv " + tDigit(game.statsCtrl.Level(game.currentHero)) + "</color> >";
        tempStr += "<size=18>\n- EXP : " + tDigit(game.statsCtrl.Exp(game.currentHero).value) + " / " + tDigit(game.statsCtrl.RequiredExp(game.currentHero))
            + "  ( " + percent(game.statsCtrl.ExpPercent(game.currentHero), 3) + " )";
        tempStr += "\n- EXP Gained in the last minute : <color=green>" + tDigit(game.statsCtrl.Exp(game.currentHero).TotalGainInLastMinute())
            + " ( " + percent(game.statsCtrl.Exp(game.currentHero).TotalGainInLastMinute() / game.statsCtrl.RequiredExp(game.currentHero), 3) + " )</color>";
        return tempStr;
    }
}

public class AbilityUI
{
    GameObject thisObject;
    AbilityKind kind;
    TextMeshProUGUI thisText;
    Button thisButton;
    public AbilityUI(GameObject gameObject, AbilityKind kind)
    {
        thisObject = gameObject;
        this.kind = kind;
        thisText = thisObject.GetComponentInChildren<TextMeshProUGUI>();
        thisButton = thisObject.GetComponentInChildren<Button>();

        thisButton.onClick.AddListener(PlusPoint);
        thisButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
    }
    public void UpdateUI()
    {
        thisButton.interactable = Interactable();
        thisText.text = String();
    }
    string tempStr;
    double tempValue;
    string String()
    {
        tempStr = optStr + "<size=20>" + localized.Ability(kind) + "\n";
        tempValue = game.statsCtrl.Ability(game.currentHero, kind).Point();
        if (tempValue >= 1000) tempStr += "<size=18>";
        tempStr += tDigit(tempValue);
        return tempStr;
    }
    bool Interactable()
    {
        return game.statsCtrl.AbilityPointLeft(game.currentHero).value >= 1;
    }
    void PlusPoint()
    {
        game.statsCtrl.Ability(game.currentHero, kind).transaction.Buy();
    }
    public string DescriptionString()
    {
        string tempString = optStr + "<size=20>" + localized.Ability(kind);
        tempString += optStr + "\n<size=20>- " + localized.AbilityDescription(kind);
        return tempString;
    }
}
public class AbilityPopupUI : PopupUI
{
    public AbilityPopupUI(GameObject thisObject) : base(thisObject)
    {
    }
}


public class BasicStatsUI : STATS_UI
{
    public BasicStatsUI(GameObject gameObject, BasicStatsKind kind) : base(gameObject)
    {
        type = StatsType.BasicStats;
        this.basicStatsKind = kind;
        thisMulti = ()=> game.statsCtrl.BasicStats(game.currentHero, kind);
    }
    public override string NameString()
    {
        return localized.BasicStats(basicStatsKind);
    }
    public override string ValueString()
    {
        if (basicStatsKind == BasicStatsKind.HP)
            return optStr + tDigit(game.battleCtrl.hero.currentHp.value, 1) + " / " + tDigit(thisMulti().Value(), 1);
        if (basicStatsKind == BasicStatsKind.MP)
        {
            string tempStr = optStr + tDigit(game.battleCtrl.hero.currentMp.value, 1) + " / " + tDigit(game.battleCtrl.hero.mp, 1);
            if (thisMulti().Value() != game.battleCtrl.hero.mp) tempStr += " ( " + tDigit(thisMulti().Value(), 1) + " )";
            return tempStr;
        }

        return tDigit(thisMulti().Value(), 1);
    }
}
public class StatsUI : STATS_UI
{
    public StatsUI(GameObject gameObject, Stats kind) : base(gameObject)
    {
        type = StatsType.HeroStats;
        this.heroStatsKind = kind;
        thisMulti = () => game.statsCtrl.HeroStats(game.currentHero, kind);
        if (kind == Stats.MoveSpeed) numberType = NumberType.meter;
        else numberType = NumberType.percent;
    }
    public override string NameString()
    {
        return localized.Stat(heroStatsKind);
    }
    public override string ValueString()
    {
        if (numberType == NumberType.meter) return meter(thisMulti().Value(), 3) + " / " + localized.Basic(BasicWord.Sec);
        else if (heroStatsKind == Stats.EquipmentDropChance) return percent(thisMulti().Value(), 4);
        else if (numberType == NumberType.percent)
        {
            if (thisMulti().IsMaxed()) return percent(thisMulti().Value(), 3) + " (Max)";
            return percent(thisMulti().Value(), 3);
        }
        else return tDigit(thisMulti().Value(), 3);
    }
}
public class GlobalStatsUI : STATS_UI
{
    public GlobalStatsUI(GameObject gameObject, GlobalStats kind) : base(gameObject)
    {
        type = StatsType.GlobalStats;
        this.globalStatsKind = kind;
        thisMulti = () => game.statsCtrl.globalStats[(int)kind];
        if (kind == GlobalStats.GoldGain) numberType = NumberType.percent;
    }
    public override string NameString()
    {
        return localized.GlobalStat(globalStatsKind);
    }
    public override string ValueString()
    {
        if (numberType == NumberType.percent) return percent(thisMulti().Value(), 3);
        else return tDigit(thisMulti().Value()) + " / kill";
    }
}
public enum StatsType
{
    BasicStats,
    HeroStats,
    GlobalStats
}
public class STATS_UI
{
    GameObject thisObject;
    TextMeshProUGUI nameText, valueText;
    public Func<Multiplier> thisMulti;
    public NumberType numberType;
    public StatsType type;

    public BasicStatsKind basicStatsKind;
    public Stats heroStatsKind;
    public GlobalStats globalStatsKind;

    public STATS_UI(GameObject gameObject)
    {
        thisObject = gameObject;
        nameText = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        valueText = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }
    public void UpdateUI()
    {
        nameText.text = NameString();
        valueText.text = ValueString();
    }
    public virtual string NameString() { return ""; }
    public virtual string ValueString() { return ""; }
}

public class TitleUI
{
    GameObject thisObject;
    TextMeshProUGUI nameText, levelText;
    public TitleKind kind;
    public TitleUI(GameObject gameObject, TitleKind kind)
    {
        thisObject = gameObject;
        this.kind = kind;
        nameText = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        levelText = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }
    public void UpdateUI()
    {
        SetActive(thisObject, game.questCtrl.TitleLevel(game.currentHero, kind) > 0);
        nameText.text = NameString();
        levelText.text = LevelString();
    }
    public virtual string NameString() { return localized.Title(kind); }
    public virtual string LevelString() { return optStr + "<color=green>Lv " + tDigit(game.questCtrl.TitleLevel(game.currentHero, kind)); }
}

//Popup
public class StatsPopupUI : PopupUI
{
    STATS_UI statsUI;
    TextMeshProUGUI description, valueText;
    public StatsPopupUI(GameObject thisObject) : base(thisObject)
    {
        description = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        valueText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(STATS_UI statsUI)
    {
        this.statsUI = statsUI;
        UpdateText();
        DelayShow();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        base.UpdateUI();
        UpdateText();
    }
    void UpdateText()
    {
        description.text = DescriptionString(statsUI);
        valueText.text = ValueString(statsUI);
    }
    public string DescriptionString(STATS_UI statsUI)
    {
        string tempStr = optStr + "<size=20>" + statsUI.NameString();
        if (statsUI.type == StatsType.BasicStats) tempStr += "<size=18>\n- " + localized.BasicStatsDescription(statsUI.basicStatsKind);
        tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Additive) + "</u><size=18>";
        //Base
        tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (statsUI.thisMulti().Add((MultiplierKind)i) != 0)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i);
        }
        tempStr += optStr + "\n" + localized.Basic(BasicWord.Additive) + " " + localized.Basic(BasicWord.Total) + " : ";
        //Mul
        tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Multiplicative) + "</u><size=18>";
        tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (statsUI.thisMulti().Mul((MultiplierKind)i) != 1)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i);
        }
        tempStr += "\n" + localized.Basic(BasicWord.Multiplicative) + " " +  localized.Basic(BasicWord.Total) + " : ";

        //Expのボーナス
        if (statsUI.type == StatsType.HeroStats && statsUI.heroStatsKind == Stats.ExpGain)
        {
            tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Bonus) + "</u><size=18>";
            tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Upgrade);
        }
        //Goldのボーナス
        if (statsUI.type == StatsType.GlobalStats && statsUI.globalStatsKind == GlobalStats.GoldGain)
        {
            tempStr += optStr + "<size=20>\n\n<u>" + localized.Basic(BasicWord.Bonus) + "</u><size=18>";
            tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Upgrade);
        }
        tempStr += optStr + "<size=20>\n\n" + localized.Basic(BasicWord.Total) + " : ";

        //HP/MPRegen
        if (statsUI.type == StatsType.BasicStats)
        {
            if (statsUI.basicStatsKind == BasicStatsKind.HP || statsUI.basicStatsKind == BasicStatsKind.MP)
                tempStr += optStr + "<size=18>\n" + localized.Basic(BasicWord.Regen);
        }

        return tempStr;
    }
    public string ValueString(STATS_UI statsUI)
    {
        string tempStr = optStr + "<size=20> ";
        if (statsUI.type == StatsType.BasicStats) tempStr += "<size=18>\n ";
        tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
        //Base
        tempStr += optStr + "\n" + tDigit(statsUI.thisMulti().Add(MultiplierKind.Base), statsUI.numberType);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (statsUI.thisMulti().Add((MultiplierKind)i) > 0)
                tempStr += optStr + "\n+ " + tDigit(statsUI.thisMulti().Add((MultiplierKind)i), statsUI.numberType);
            else if (statsUI.thisMulti().Add((MultiplierKind)i) < 0)
                tempStr += optStr + "\n<color=yellow> " + tDigit(statsUI.thisMulti().Add((MultiplierKind)i), statsUI.numberType) + "</color>";
        }
        tempStr += "\n" + tDigit(statsUI.thisMulti().Add(), statsUI.numberType);
        //Mul
        tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
        //Base
        tempStr += optStr + "\n" + percent(statsUI.thisMulti().Mul(MultiplierKind.Base), 4);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (statsUI.thisMulti().Mul((MultiplierKind)i) > 1)
                tempStr += optStr + "\n* " + percent(statsUI.thisMulti().Mul((MultiplierKind)i), 4);
            else if (statsUI.thisMulti().Mul((MultiplierKind)i) < 1)
                tempStr += optStr + "\n<color=yellow>* " + percent(statsUI.thisMulti().Mul((MultiplierKind)i), 4) + "</color>";
        }
        tempStr += "\n" + percent(statsUI.thisMulti().Mul(), 4);

        //Expのボーナス
        if (statsUI.type == StatsType.HeroStats && statsUI.heroStatsKind == Stats.ExpGain)
        {
            tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
            tempStr += optStr + "\n+ " + tDigit(game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue());
        }
        //Goldのボーナス
        if (statsUI.type == StatsType.GlobalStats && statsUI.globalStatsKind == GlobalStats.GoldGain)
        {
            tempStr += optStr + "<size=20>\n\n <u></u><size=18>";
            tempStr += optStr + "\n+ " + tDigit(game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue());
        }

        tempStr += optStr + "<size=20> \n\n" + statsUI.ValueString();
        if (statsUI.type == StatsType.HeroStats && statsUI.heroStatsKind == Stats.ExpGain)
            tempStr += optStr + " + " + tDigit(game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue());
        if (statsUI.type == StatsType.GlobalStats && statsUI.globalStatsKind == GlobalStats.GoldGain)
            tempStr += optStr + " + " + tDigit(game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue());

        //HP/MPRegen
        if (statsUI.type == StatsType.BasicStats)
        {
            if(statsUI.basicStatsKind == BasicStatsKind.HP)
                tempStr += optStr + "<size=18>\n" + tDigit(game.statsCtrl.HpRegenerate(game.currentHero).Value(), 1) + " / " + localized.Basic(BasicWord.Sec);
            if (statsUI.basicStatsKind == BasicStatsKind.MP)
                tempStr += optStr + "<size=18>\n" + tDigit(game.statsCtrl.MpRegenerate(game.currentHero).Value(), 1) + " / " + localized.Basic(BasicWord.Sec);
        }

        return tempStr;
    }
}

public class TitlePopupUI : PopupUI
{
    public TitlePopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public void ShowAction(TitleKind titleKind)
    {
        SetUI(() => DescriptionString(titleKind));
    }
    string DescriptionString(TitleKind titleKind)
    {
        string tempString = optStr + "<size=20>" + localized.Title(titleKind) + " < <color=green>Lv " + tDigit(game.questCtrl.TitleLevel(game.currentHero, titleKind)) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.TitleEffect(titleKind, Parameter.TitleEffectValue(titleKind, game.questCtrl.TitleLevel(game.currentHero, titleKind)).main);
        if (IsSub(titleKind))
            tempString += optStr + "\n- " + localized.TitleEffect(titleKind, Parameter.TitleEffectValue(titleKind, game.questCtrl.TitleLevel(game.currentHero, titleKind)).sub, true);
        //tempString += "\n\n";
        return tempString;
    }
    bool IsSub(TitleKind kind)
    {
        if (kind == TitleKind.SkillMaster) return true;
        if (kind == TitleKind.MonsterDistinguisher) return true;
        if (kind == TitleKind.MetalHunter) return true;
        if (kind == TitleKind.FireResistance) return true;
        if (kind == TitleKind.IceResistance) return true;
        if (kind == TitleKind.ThunderResistance) return true;
        if (kind == TitleKind.LightResistance) return true;
        if (kind == TitleKind.DarkResistance) return true;
        return false;
    }
}
