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

public class GuildMenuUI : MENU_UI
{
    [SerializeField] TextMeshProUGUI guildInfoText;
    [SerializeField] TextMeshProUGUI activeText;

    [SerializeField] GameObject[] members;
    GuildMemberUI[] membersUI;
    [SerializeField] TextMeshProUGUI abilityText;
    [SerializeField] GameObject[] abilities;
    GuildAbilityUI[] abilitiesUI;
    GuildAbilityPopupUI popupUI;
    PopupUI activatePopupUI;

    // Start is called before the first frame update
    void Start()
    {
        abilitiesUI = new GuildAbilityUI[Math.Min(abilities.Length, Enum.GetNames(typeof(GuildAbilityKind)).Length)];
        for (int i = 0; i < abilities.Length; i++)
        {
            if (i < Enum.GetNames(typeof(GuildAbilityKind)).Length)
                abilitiesUI[i] = new GuildAbilityUI(abilities[i].gameObject, (GuildAbilityKind)i);
            else
                SetActive(abilities[i], false);
        }
        membersUI = new GuildMemberUI[members.Length];
        for (int i = 0; i < membersUI.Length; i++)
        {
            int count = i;
            membersUI[i] = new GuildMemberUI(members[count], (HeroKind)count);
        }

        //Popup
        popupUI = new GuildAbilityPopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < abilities.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(abilities[count], () => popupUI.ShowAction(abilitiesUI[count].thisAbility));
        }
        activatePopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < membersUI.Length; i++)
        {
            int count = i;
            activatePopupUI.SetTargetObject(membersUI[count].popupRaycastObject, () => HeroInfoPopupString((HeroKind)count));
            activatePopupUI.SetTargetObject(membersUI[count].activeButton.gameObject, membersUI[count].ActivatePopupString);
            activatePopupUI.SetTargetObject(membersUI[count].playButton.gameObject, membersUI[count].PlayPopupString);
        }
        activatePopupUI.SetTargetObject(activeText.gameObject, ActivableTextPopupString);
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        abilityText.text = AbilityString();
        activeText.text = ActiveString();
        guildInfoText.text = GuildInfoString();
        for (int i = 0; i < abilitiesUI.Length; i++)
        {
            abilitiesUI[i].UpdateUI();
        }
        for (int i = 0; i < membersUI.Length; i++)
        {
            membersUI[i].UpdateUI();
        }
        popupUI.UpdateUI();
        activatePopupUI.UpdateUI();
        //if (game.guildCtrl.isTryingSwitchPlay)
        //{
        //    popupUI.DelayHide();
        //    activatePopupUI.DelayHide();
        //}
    }
    public string GuildInfoString()
    {
        return optStr + "<size=24>" + localized.Basic(BasicWord.GuildLevel) + " : <color=green>Lv " + tDigit(game.guildCtrl.Level())
            + "</color>    <size=20>EXP  :  " + tDigit(game.guildCtrl.exp.value) + " / " + tDigit(game.guildCtrl.RequiredExp())
            + "  ( " + percent(game.guildCtrl.ExpPercent(), 3) + " )";
    }
    string ActiveString()
    {
        return optStr + "<size=20>Active : " + tDigit(game.guildCtrl.CurrentActiveNum()) + " / " + tDigit(game.guildCtrl.activableNum.Value());
    }
    string AbilityString()
    {
        return optStr + "<size=24>Guild Ability <size=20>( point left : " + tDigit(game.guildCtrl.abilityPointLeft.value) + " )";
    }

    string HeroInfoPopupString(HeroKind heroKind)
    {
        string tempStr = optStr;
        tempStr += localized.Hero(heroKind) + "  < <color=green>Lv " + tDigit(game.statsCtrl.Level(heroKind)) + "</color> ><size=18>";
        tempStr += "\n- ";
        if (membersUI[(int)heroKind].guildMember.isActive) tempStr += "In battle at " + game.battleCtrls[(int)heroKind].areaBattle.CurrentArea().Name();
        else tempStr += "Waiting in " + game.battleCtrls[(int)heroKind].areaBattle.CurrentArea().Name();
        tempStr += "\n- EXP : " + tDigit(game.statsCtrl.Exp(heroKind).value) + " / " + tDigit(game.statsCtrl.RequiredExp(heroKind))
            + "  ( " + percent(game.statsCtrl.ExpPercent(heroKind), 3) + " )";
        tempStr += "\n- EXP Gained in the last minute : <color=green>" + tDigit(game.statsCtrl.Exp(heroKind).TotalGainInLastMinute())
            + " ( " + percent(game.statsCtrl.Exp(heroKind).TotalGainInLastMinute() / game.statsCtrl.RequiredExp(heroKind), 3) + " )</color>";
        tempStr += "\n- Unspent Ability Point : " + tDigit(membersUI[(int)heroKind].unspentAbilityPoint);
        return tempStr;
    }

    string ActivableTextPopupString()
    {
        string tempStr = optStr + "<size=18>" + tDigit(game.guildCtrl.CurrentActiveNum());
        if (game.guildCtrl.CurrentActiveNum() > 1) tempStr += " Heroes are currently active";
        else tempStr += " Hero is currently active";
        tempStr += optStrL
            + "\n- In addition to the current playing hero, you can activate the other heroes in background " +
            "\nafter you get Title Quest [Proof of Rebirth]."
            + "\n- Activable heroes at the same time are currently limited to "
            + tDigit(game.guildCtrl.activableNum.Value()) + "." + 
            "\n- You can increase the limit through a World Ascension upgrade.";
        return tempStr;
    }
}

public class GuildMemberUI
{
    public GuildMemberUI(GameObject gameObject, HeroKind heroKind)
    {
        thisObject = gameObject;
        this.heroKind = heroKind;
        guildMember = game.guildCtrl.Member(heroKind);
        nameText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        levelFillImage = thisObject.transform.GetChild(3).gameObject.GetComponent<Image>();
        levelText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        areaFillImage = thisObject.transform.GetChild(6).gameObject.GetComponent<Image>();
        areaText = thisObject.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        abilityPointText = thisObject.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        popupRaycastObject = thisObject.transform.GetChild(9).gameObject;
        activeButton = thisObject.transform.GetChild(10).gameObject.GetComponent<Button>();
        activeButtonText = activeButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        playButton = thisObject.transform.GetChild(11).gameObject.GetComponent<Button>();
        playButtonText = playButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = thisObject.transform.GetChild(12).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        activeButton.onClick.AddListener(SwicthActiveBattle);
        playButton.onClick.AddListener(SwitchPlay);
    }
    void SwicthActiveBattle()
    {
        guildMember.SwitchActive();
    }
    void SwitchPlay()
    {
        guildMember.SwitchPlay();
    }
    public void UpdateUI()
    {
        SetActive(lockObject, !guildMember.IsUnlocked());
        if (!guildMember.IsUnlocked())
        {
            lockText.text = LockString();
            return;
        }
        nameText.text = NameString();
        levelFillImage.fillAmount = game.statsCtrl.ExpPercent(heroKind);
        levelText.text = LevelString();
        areaFillImage.fillAmount = (float)((game.battleCtrls[(int)heroKind].areaBattle.currentWave + 1) / (float)game.battleCtrls[(int)heroKind].areaBattle.CurrentArea().MaxWaveNum());
        areaText.text = AreaString();
        abilityPointText.text = "<color=red>AP " + tDigit(unspentAbilityPoint);
        activeButton.interactable = guildMember.ActiveInteractable();
        if (guildMember.isActive)
        {
            if(guildMember.isPlaying)
                activeButtonText.text = "<color=orange>Active";
            else activeButtonText.text = "<color=orange>Passive";
        }
        else activeButtonText.text = "<color=white>Inactive";
        playButton.interactable = !guildMember.isPlaying;
        if (guildMember.isPlaying) playButtonText.text = "<color=orange>Playing";
        else playButtonText.text = "<color=white>Play";
    }
    public double unspentAbilityPoint => game.statsCtrl.AbilityPointLeft(guildMember.heroKind).value;
    string LockString()
    {
        return optStr + "<sprite=\"Locks\" index=0> Guild Lv " + tDigit(GuildParameter.MemberUnlockLevel(heroKind));
    }
    string NameString()
    {
        return localized.Hero(heroKind); 
    }
    string LevelString()
    {
        return optStr + "Lv " + tDigit(game.statsCtrl.Level(heroKind));
    }
    AREA tempArea;
    string AreaString()
    {
        tempArea = game.battleCtrls[(int)heroKind].areaBattle.CurrentArea();
        return optStr + "<sprite=\"Monsters\" index=" + ((int)tempArea.kind).ToString() + ">" + " " + tempArea.Name(false, false, true);//+ localized.Basic(BasicWord.Area) + " " + tDigit(tempArea.id + 1);
    }
    public string ActivatePopupString()
    {
        if (guildMember.isActive)
        {
            if (guildMember.isPlaying) return "This hero is currently active.";
            return "This hero is currently active in background.\nBackground Gain Efficiency : " + percent(guildMember.gainRate);
        }
        if (guildMember.canBackgroundActive.IsUnlocked()) return "You can activate this hero in background.\nBackground Gain Efficiency : " + percent(guildMember.backgroundGainRate.Value());
        return "Get Title Quest [Proof of Rebirth] to enable Background Activation.";
    }
    public string PlayPopupString()
    {
        if (guildMember.isPlaying) return "You are currently playing " + localized.Hero(heroKind);
        return "Switch to play " + localized.Hero(heroKind);
    }
    public GuildMember guildMember;
    public GameObject thisObject, popupRaycastObject;
    HeroKind heroKind;
    TextMeshProUGUI nameText, levelText, areaText, lockText, activeButtonText, playButtonText, abilityPointText;
    public Button activeButton, playButton;
    GameObject lockObject;
    Image levelFillImage, areaFillImage;
}

public class GuildAbilityUI
{
    public GuildAbilityUI(GameObject gameObject, GuildAbilityKind kind)
    {
        thisObject = gameObject;
        this.kind = kind;
        lockObject = thisObject.transform.GetChild(4).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(PlusPoint);
        plusButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
    }
    GameObject thisObject;
    GameObject lockObject;
    GuildAbilityKind kind;
    public GuildAbility thisAbility { get => game.guildCtrl.Ability(kind); }
    TextMeshProUGUI nameText, levelText, lockText;
    Button plusButton;
    public void UpdateUI()
    {
        SetActive(lockObject, !thisAbility.IsUnlocked());
        if (!thisAbility.IsUnlocked())
        {
            lockText.text = LockString();
            return;
        }
        plusButton.interactable = Interactable();
        nameText.text = NameString();
        levelText.text = LevelString();
    }
    string LockString()
    {
        switch (kind)
        {
            case GuildAbilityKind.GoldCap:
                return "<sprite=\"Locks\" index=0> Tier 1 World Ascension";
            case GuildAbilityKind.GoldGain:
                return "<sprite=\"Locks\" index=0> Tier 1 World Ascension";
            case GuildAbilityKind.Trapping:
                return "<sprite=\"Locks\" index=0> Tier 1 World Ascension";
            case GuildAbilityKind.UpgradeCost:
                return "<sprite=\"Locks\" index=0> Tier 1 World Ascension";
            case GuildAbilityKind.PhysicalAbsorption:
                return "<sprite=\"Locks\" index=0> Tier 2 World Ascension";
            case GuildAbilityKind.MagicalAbsoption:
                return "<sprite=\"Locks\" index=0> Tier 2 World Ascension";
            case GuildAbilityKind.ExpGain:
                return "<sprite=\"Locks\" index=0> Tier 2 World Ascension";
            case GuildAbilityKind.EquipmentProficiency:
                return "<sprite=\"Locks\" index=0> Tier 2 World Ascension";
            case GuildAbilityKind.MaterialDrop:
                return "<sprite=\"Locks\" index=0> Tier 3 World Ascension";
            case GuildAbilityKind.NitroCap:
                return "<sprite=\"Locks\" index=0> Tier 3 World Ascension";
        }
        return "<sprite=\"Locks\" index=0> World Ascension";
        //return optStr + "<sprite=\"Locks\" index=0> " + localized.Basic(BasicWord.GuildLevel) + " " + tDigit(thisAbility.unlockLevel);
    }
    string NameString()
    {
        return localized.GuildAbilityName(kind);
    }
    string LevelString()
    {
        return optStr + "<color=green>Lv " + tDigit(thisAbility.level.value);// + "</color> / " + tDigit(thisAbility.level.maxValue());
    }
    bool Interactable()
    {
        return thisAbility.transaction.CanBuy();
    }
    void PlusPoint()
    {
        thisAbility.transaction.Buy();;
    }
}

public class GuildAbilityPopupUI : PopupUI
{
    public GuildAbilityPopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public void ShowAction(GuildAbility ability)
    {
        if (!ability.IsUnlocked()) return;
        SetUI(() => DescriptionString(ability));
    }
    string DescriptionString(GuildAbility ability)
    {
        string tempString = optStr + "<size=20>" + localized.GuildAbilityName(ability.kind) + " < <color=green>Lv " + tDigit(ability.level.value) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- Max Level : Lv " + tDigit(ability.level.maxValue());
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Current) + " : " + localized.GuildAbilityEffect(ability.kind, ability.effectValue);
        if (ability.level.IsMaxed()) return tempString;
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Next) + " : " + localized.GuildAbilityEffect(ability.kind, ability.nextEffectValue) + " ( <color=green>Lv " + tDigit(ability.transaction.ToLevel()) + "</color> )";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(ability.transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(ability.transaction.ToLevel()) + "</color> )";
        return tempString;
    }
}