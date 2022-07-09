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

public class BestiaryMenuUI : MENU_UI
{
    //[SerializeField] TextMeshProUGUI activeNumText;
    [SerializeField] Image monsterImage, monsterSlot;
    [SerializeField] TextMeshProUGUI activeNumText, nameText, defeatedNumText, dropMaterialText, dropChanceText;
    [SerializeField] TextMeshProUGUI petRankText, petLevelText, tamingProgressText, effectText, statsText;
    [SerializeField] Image tamingPointFillImage, loyaltyFillImage;
    [SerializeField] Button activeButton, summonButton;//levelupButton,
    [SerializeField] TextMeshProUGUI activeText, summonText, loyaltyText, loyaltyPointText;//levelupText, monsterMilkText, 
    [SerializeField] Image trapImage;
    [SerializeField] GameObject[] monsterButtonObjects;
    BestiaryMonsterUI[] bestiaryMonstersUI;
    MonsterGlobalInformation globalInfo;
    PopupUI popupUI;

    void Start()
    {
        bestiaryMonstersUI = new BestiaryMonsterUI[Math.Min(game.monsterCtrl.globalInfoList.Count, monsterButtonObjects.Length)];
        for (int i = 0; i < monsterButtonObjects.Length; i++)
        {
            int count = i;
            if (count < game.monsterCtrl.globalInfoList.Count)
                bestiaryMonstersUI[i] = new BestiaryMonsterUI(this, game.monsterCtrl.globalInfoList[count], monsterButtonObjects[count]);
            else
                SetActive(monsterButtonObjects[count], false);
        }

        activeButton.onClick.AddListener(() => { globalInfo.pet.SwitchActive(); SetActiveUI(); });
        openClose.openActions.Add(SetActiveUI);
        SetUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal));
        summonButton.onClick.AddListener(() => game.monsterCtrl.SetOrRemoveSummonPet(game.currentHero, globalInfo));
        //levelupButton.onClick.AddListener(() => globalInfo.pet.levelTransaction.Buy());

        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(tamingPointFillImage.gameObject, TamingPopupString);
        popupUI.SetTargetObject(trapImage.gameObject, TrapPopupString);
        //popupUI.SetTargetObject(levelupButton.gameObject, LevelupPopupString);
        popupUI.SetTargetObject(loyaltyText.gameObject, LoyaltyPopupString);
        popupUI.SetTargetObject(loyaltyPointText.gameObject, LoyaltyPopupString);
    }

    string LoyaltyPopupString()
    {
        string tempStr = optStr + "<size=20><sprite=\"peticon\" index=0> Pet Loyalty : " + tDigit(globalInfo.pet.loyalty.value) + "<size=18>";
        tempStr += optStr + "\n- Loyalty Point : " + globalInfo.pet.loyaltyExp.Description(true);
        tempStr += optStr + "\n- Loyalty Point Gain per Tamer's 'Order' skill trigger : " + tDigit(game.statsCtrl.heroes[(int)game.currentHero].loyaltyPoingGain.Value(), 1);
        tempStr += optStr + "\n\n<u>Effect</u>" +
            "\n- Multiplies Pet Passive Effect by " + percent(1d + globalInfo.pet.loyalty.value / 100d, 0) + " ( + " + percent(1 / 100d, 0) + " / Loyalty" + " )" +
            "\n- Multiplies Pet Stats (" + localized.BasicStats(BasicStatsKind.HP, true) + localized.BasicStats(BasicStatsKind.ATK, true) + localized.BasicStats(BasicStatsKind.MATK, true) + localized.BasicStats(BasicStatsKind.DEF, true) + localized.BasicStats(BasicStatsKind.MDEF, true) + localized.BasicStats(BasicStatsKind.SPD, true) + ") by " + percent(1d + 0.05d * globalInfo.pet.loyalty.value , 0) + " ( + " + percent(0.05d, 0) + " / Loyalty" + " )";
        return tempStr;
    }

    //string LevelupPopupString()
    //{
    //    string tempStr = optStrL + "<size=20><u>Cost to Level Up Cost  ( <color=green>Lv " + tDigit(globalInfo.pet.levelTransaction.ToLevel()) + "</color> )</u><size=18>";
    //    tempStr += optStr + "\n" + globalInfo.pet.levelTransaction.DescriptionString();
    //    return tempStr;
    //}

    string TrapPopupString()
    {
        string tempStr = optStr;
        if (globalInfo.species <= MonsterSpecies.Unicorn && globalInfo.color <= MonsterColor.Red)
            tempStr += "You must use <color=green>" + localized.PotionName((PotionKind)((int)PotionKind.ThrowingNet + (int)globalInfo.color)) + "</color> to capture this monster."
                + "\n<size=18>- Capturable Monster Level increases along with Hero Level based on Title [Monster Study].";
        else
            tempStr += "This pet cannot currently be captured.\nStay tuned for future updates!";
        return tempStr;
    }
    string TamingPopupString()
    {
        string tempStr = optStr;
        tempStr += "<size=20>Taming Point<size=18>";
        tempStr += "\n- To increase Rank : " + globalInfo.pet.tamingPoint.Description(true);//tDigit(globalInfo.pet.tamingPoint.value) + " / " + tDigit(globalInfo.pet.RequiredTamingPoint()) + " ( " + 
        tempStr += "\n\n<u>Gain Breakdowns</u>";
        //Multiplier multi = game.monsterCtrl.tamingPointGainMultiplier;
        //tempStr += optStr + "\n" + multi.BreakdownString();
        //tempStr += "\nBase Total : " + tDigit(globalInfo.pet.BaseTamingPointGainPerCapture(), 2);
        tempStr += "\n- Base : 1";
        tempStr += optStr + "\n- Defeated # : + " + tDigit(globalInfo.pet.BaseTamingPointGainPerCapture() - 1, 2)
            + " | Formula : Log2(1 + [Defeated#]/10000)";
        tempStr += "\n- Hero Stats : * " + percent(game.statsCtrl.HeroStats(game.currentHero, Stats.TamingPointGain).Value());
        //tempStr += "\nMultiplier Total : " + percent(game.monsterCtrl.tamingPointGainMultiplier.Value());
        //tempStr += "\n- Base : 100.0%";
        //for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        //{
        //    if (multi.Add((MultiplierKind)i) != 0)
        //        tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : + " + percent(multi.Add((MultiplierKind)i), 3);
        //}
        //for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        //{
        //    if (multi.Mul((MultiplierKind)i) != 1)
        //        tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : * " + percent(multi.Mul((MultiplierKind)i), 3);
        //}
        tempStr += "\nTotal : " + tDigit(globalInfo.pet.TamingPointGainPerCapture(game.currentHero), 1) + " / capture";
        return tempStr;
    }

    public void SetUI(MonsterGlobalInformation globalInfo)
    {
        this.globalInfo = globalInfo;
        if (globalInfo.species == MonsterSpecies.ChallengeBoss)
            monsterImage.sprite = sprite.monsters[0][(int)globalInfo.species][(int)globalInfo.challengeMonsterKind];
        else
            monsterImage.sprite = sprite.monsters[0][(int)globalInfo.species][(int)globalInfo.color];
        nameText.text = globalInfo.Name();
        if (globalInfo.species <= MonsterSpecies.Unicorn && globalInfo.color <= MonsterColor.Red)
        {
            //SetActive(trapImage.gameObject, true);
            trapImage.sprite = sprite.potions[(int)PotionKind.ThrowingNet + (int)globalInfo.color];
        }
        else
        {
            trapImage.sprite = sprite.questionmarkSlot;
            //SetActive(trapImage.gameObject, false);
        }

        monsterSlot.color = globalInfo.pet.isActive ? Color.green : Color.white;

    }
    public void SetActiveUI()
    {
        for (int i = 0; i < bestiaryMonstersUI.Length; i++)
        {
            bestiaryMonstersUI[i].SetActiveUI();
        }
        monsterSlot.color = globalInfo.pet.isActive ? Color.green : Color.white;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        defeatedNumText.text = DefeatedNumString();
        dropMaterialText.text = DropMaterialString();
        dropChanceText.text = DropChanceString();
        activeNumText.text = optStrL + "Active : " + tDigit(game.monsterCtrl.PetActiveNum()) + " / " + tDigit(game.monsterCtrl.petActiveCap.Value())
            + "\nSummon : " + tDigit(game.monsterCtrl.CurrentSummonPetNum(game.currentHero)) + " / " + tDigit(game.statsCtrl.heroes[(int)game.currentHero].summonPetSlot.Value());

        activeButton.interactable = globalInfo.pet.CanSwitchActive();
        activeText.text = globalInfo.pet.isActive ? "<color=green>ON" : "OFF";
        summonButton.interactable = globalInfo.pet.SummonButtonInteractable(game.currentHero);
        summonText.text = globalInfo.pet.IsSummon(game.currentHero) ? "<color=green>Summon</color>" : "Summon";
        tamingPointFillImage.fillAmount = globalInfo.pet.TamingPointPercent();
        tamingProgressText.text = tDigit(globalInfo.pet.tamingPoint.value, 1) + " / " + tDigit(globalInfo.pet.RequiredTamingPoint(), 1) + " ( + " + tDigit(globalInfo.pet.TamingPointGainPerCapture(game.currentHero), 1) + " / capture )";//"Taming Point : ";
        petRankText.text = optStr + "<color=orange>Pet Rank " + tDigit(globalInfo.pet.rank.value) + "</color>";
        petLevelText.text = optStr + "<color=green>Lv " + tDigit(globalInfo.pet.level.value) + "</color> / " + tDigit(globalInfo.pet.MaxLevel()) + "  |  EXP : " + tDigit(globalInfo.pet.exp.value) + " / " + tDigit(globalInfo.pet.exp.RequiredExp()) + " ( " + percent(globalInfo.pet.exp.Percent(), 1) + " )";
        //levelupButton.interactable = globalInfo.pet.LevelButtonInteractable();
        //levelupText.text = optStr + "Level Up  ( <color=green>Lv " + tDigit(globalInfo.pet.levelTransaction.ToLevel()) + "</color> )";
        //monsterMilkText.text = optStr + "Monster Milk : " + tDigit(game.monsterCtrl.monsterMilk.value);
        loyaltyText.text = optStr + "<sprite=\"peticon\" index=0> " + tDigit(globalInfo.pet.loyalty.value);
        loyaltyPointText.text = optStr + "Loyalty Point : " + globalInfo.pet.loyaltyExp.Description(true);
        loyaltyFillImage.fillAmount = (float)globalInfo.pet.loyaltyExp.Percent();
        effectText.text = EffectString();
        statsText.text = StatsString();
        for (int i = 0; i < bestiaryMonstersUI.Length; i++)
        {
            bestiaryMonstersUI[i].UpdateUI();
        }

        popupUI.UpdateUI();
    }

    string DefeatedNumString()
    {
        string tempStr = optStr + "<size=18>";
        tempStr += "- Defeated # <color=green>" + tDigit(globalInfo.DefeatedNum()) + "</color>";
        if (globalInfo.DefeatedNum(true) >= 1) tempStr += "<size=14> / Mutant: <color=green>" + tDigit(globalInfo.DefeatedNum(true)) + "</color>";
        tempStr += "\n<size=18>- Captured # <color=green>" + tDigit(globalInfo.CapturedNum()) + "</color>";
        if (globalInfo.CapturedNum(true) >= 1) tempStr += "<size=14> / Mutant: <color=green>" + tDigit(globalInfo.CapturedNum(true)) + "</color>";
        return tempStr;
    }
    string DropMaterialString()
    {
        string tempStr = optStr + "<size=20>Drop Material<size=18>";
        tempStr += "\n- Common : " + localized.Material(globalInfo.dropSpeciesMaterial);
        tempStr += "\n- Rare : " + localized.Material(globalInfo.dropColorMaterial);
        return tempStr;
    }
    string DropChanceString()
    {
        string tempStr = optStr + "<size=20>   <size=18>";
        tempStr += "\n" + percent(game.monsterCtrl.speciesMaterialDropChance[(int)globalInfo.species].Value(), 4);
        tempStr += "\n" + percent(game.monsterCtrl.colorMaterialDropChance.Value(), 4);
        return tempStr;
    }
    string EffectString()
    {
        string tempStr = optStr + "<size=18><u>Pet Active Effect</u>";
        tempStr += "\n- " + globalInfo.pet.ActiveEffectString() + "</size>";
        tempStr += "\n\n<u>Pet Passive Effect</u>";
        tempStr += "\n- " + globalInfo.pet.PassiveEffectString();
        return tempStr;
    }
    string StatsString()
    {
        string tempStr = optStr + "<size=16><u>Pet Stats</u><size=16>";
        //tempStr += optStr + "\nLv : " + tDigit(globalInfo.pet.Level()) + " / " + tDigit(globalInfo.pet.MaxLevel());
        //tempStr += optStr + "\nEXP : " + tDigit(globalInfo.pet.exp.value) + " / " + tDigit(globalInfo.pet.RequiredExp()) + " ( " + percent(globalInfo.pet.ExpPercent(), 3) + " )";
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.HP) + " : " + tDigit(globalInfo.Hp(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.ATK) + " : " + tDigit(globalInfo.Atk(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MATK) + " : " + tDigit(globalInfo.MAtk(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.DEF) + " : " + tDigit(globalInfo.Def(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MDEF) + " : " + tDigit(globalInfo.MDef(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.SPD) + " : " + tDigit(globalInfo.Spd(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.Stat(Stats.FireRes) + " : " + percent(globalInfo.Fire(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.Stat(Stats.IceRes) + " : " + percent(globalInfo.Ice(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.Stat(Stats.ThunderRes) + " : " + percent(globalInfo.Thunder(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.Stat(Stats.LightRes) + " : " + percent(globalInfo.Light(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.Stat(Stats.DarkRes) + " : " + percent(globalInfo.Dark(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.SkillEffect((EffectKind)globalInfo.AttackElement()) + " : " + tDigit(globalInfo.Damage(0, 0, true, game.currentHero), 1);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Cooltime) + " : " + tDigit(globalInfo.AttackIntervalSec(0, 0, true, game.currentHero), 1) + " " + localized.Basic(BasicWord.Sec);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Range) + " : " + meter(globalInfo.Range());
        tempStr += optStr + "\n" + localized.Stat(Stats.MoveSpeed) + " : " + meter(globalInfo.MoveSpeed(0, 0, true, game.currentHero)) + " / " + localized.Basic(BasicWord.Sec);
        return tempStr;
    }
}

public class BestiaryMonsterUI
{
    BestiaryMenuUI bestiaryMenuUI;
    MonsterGlobalInformation globalInfo;
    GameObject thisObject;
    Button button;
    public Image slotImage, monsterImage;
    GameObject lockObject, checkmark, summonIcon;
    public BestiaryMonsterUI(BestiaryMenuUI bestiaryMenuUI, MonsterGlobalInformation globalInfo, GameObject gameObject)
    {
        this.bestiaryMenuUI = bestiaryMenuUI;
        this.globalInfo = globalInfo;
        thisObject = gameObject;
        button = thisObject.GetComponent<Button>();
        slotImage = thisObject.GetComponent<Image>();
        monsterImage = thisObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        checkmark = thisObject.transform.GetChild(1).gameObject;
        summonIcon = thisObject.transform.GetChild(2).gameObject;
        lockObject = thisObject.transform.GetChild(3).gameObject;

        button.onClick.AddListener(() => bestiaryMenuUI.SetUI(globalInfo));
        SetUI();
        SetActiveUI();
    }
    void SetUI()
    {
        //Debug
        //if ((int)globalInfo.species > (int)MonsterSpecies.Spider) return;
        if (globalInfo.species == MonsterSpecies.ChallengeBoss)
            monsterImage.sprite = sprite.monsters[0][(int)globalInfo.species][(int)globalInfo.challengeMonsterKind];
        else
            monsterImage.sprite = sprite.monsters[0][(int)globalInfo.species][(int)globalInfo.color];
    }
    public void SetActiveUI()
    {
        slotImage.color = globalInfo.pet.isActive ? Color.green : Color.white;
    }
    public void UpdateUI()
    {
        SetActive(lockObject, globalInfo.DefeatedNum() <= 0);
        button.interactable = globalInfo.DefeatedNum() > 0;
        SetActive(checkmark, globalInfo.pet.isActive);
        SetActive(summonIcon, globalInfo.pet.IsSummon(game.currentHero));
    }
}
