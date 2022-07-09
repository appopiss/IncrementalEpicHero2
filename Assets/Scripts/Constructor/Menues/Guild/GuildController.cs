using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using static Main;
using static GuildParameter;
using static GameController;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public partial class SaveR
{
    public long guildLevel;
    public double guildExp;
    public double guildAbilityPoint;
    public long[] guildAbilityLevels;

    public double[] accomplishedFirstTimesGuildLevel;//[GuildLevel]
    public double[] accomplishedTimesGuildLevel;//[GuildLevel]
    public double[] accomplishedBestTimesGuildLevel;
}
public partial class Save
{
    public long maxGuildLevel;//WAをまたいでの最大到達レベル
}
public class GuildController
{
    public GuildController()
    {
        level = new GuildLevel(() => maxGuildLevel);
        exp = new GuildExp(RequiredExp, level);
        expGain = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        activableNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 3));
        for (int i = 0; i < members.Length; i++)
        {
            members[i] = new GuildMember(this, (HeroKind)i);
        }
        abilityPointLeft = new GuildAbilityPointLeft();
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new GuildAbility(this, (GuildAbilityKind)i, abilityPointLeft);
        }

        for (int i = 0; i < accomplishGuildLevels.Length; i++)
        {
            accomplishGuildLevels[i] = new AccomplishGuildLevel(i);
        }
    }

    public void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Start();
        }
    }
    float timecountSec;
    public void Update()
    {
        timecountSec += Time.deltaTime;
        if (timecountSec >= 60)//１分ごと
        {
            exp.ChangeCountGainperSec();
            timecountSec = 0;
        }
    }

    public void Initialize()
    {
        activableNum.Calculate();
        AdjustActiveNum();
    }
    //Active#のAdjust
    public void AdjustActiveNum()
    {
        if (CurrentActiveNum() <= activableNum.Value()) return;
        for (int i = 0; i < members.Length; i++)
        {
            if (!members[i].isPlaying && members[i].isActive)
                members[i].SwitchActive();
            if (CurrentActiveNum() <= activableNum.Value()) return;
        }
    }
    //Member
    public Multiplier activableNum;//Active状態にできる数
    public int CurrentActiveNum()
    {
        int temp = 0;
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i].isActive) temp++;
        }
        return temp;
    }
    public bool CanActive() { return CurrentActiveNum() < activableNum.Value(); }

    public GuildLevel level;
    public GuildExp exp;
    public Multiplier expGain;
    GuildMember[] members = new GuildMember[Enum.GetNames(typeof(HeroKind)).Length];
    public GuildMember Member(HeroKind kind)
    {
        return members[(int)kind];
    }
    public GuildAbilityPointLeft abilityPointLeft;
    public GuildAbility[] abilities = new GuildAbility[Enum.GetNames(typeof(GuildAbilityKind)).Length];
    public GuildAbility Ability(GuildAbilityKind kind) { return abilities[(int)kind]; }
    public long Level() { return level.value; }
    public long MaxLevelReached() { return main.S.maxGuildLevel; }
    public double RequiredExp(long level) { return GuildParameter.RequiredExp(level); }
    public double RequiredExp() { return RequiredExp(Level()); }
    public float ExpPercent() { return Mathf.Clamp((float)exp.value / (float)RequiredExp(), 0, 1); }


    //Ability
    public double TotalAbilityPoint()
    {
        return Level();
    }
    public void ResetGuildAbility()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].level.ChangeValue(0);
        }
        abilityPointLeft.ChangeValue(TotalAbilityPoint());

        //GlobalSlotのリセット処理
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].skillSet.CheckMaxNum();
            game.battleCtrls[i].skillSet.UpdateCurrentSkillSet();
        }
        //InbentorySlotのリセット処理...?
        //これは再度拡張するまでアクセスできなくした

        //MysteriousWaterのリセット処理
        game.alchemyCtrl.mysteriousWaterProductionPerSec.Calculate();
        game.alchemyCtrl.CheckMysteriousWaterAssignments();
    }

    //Accomplish
    public AccomplishGuildLevel[] accomplishGuildLevels = new AccomplishGuildLevel[maxGuildLevel + 1];//0スタートのため、+1してLv300まで可能にする


    public bool isTryingSwitchPlay;
}
public class GuildMember
{
    public GuildMember(GuildController guildCtrl, HeroKind heroKind)
    {
        this.guildCtrl = guildCtrl;
        this.heroKind = heroKind;
        canBackgroundActive = new Unlock();
        backgroundGainRate = new Multiplier();
        canBackgroundActive.RegisterCondition(() => backgroundGainRate.Value() > 0);
    }
    public double gainRate
    {
        get
        {
            if (isPlaying) return 1;
            if (isActive) return Math.Min(backgroundGainRate.Value(), 1);
            return 0;
        }
    }
    public bool IsUnlocked()
    { 
        return guildCtrl.MaxLevelReached() >= MemberUnlockLevel(heroKind);
    }
    public bool ActiveInteractable()
    {
        if (isPlaying) return false;
        if (!IsUnlocked()) return false;
        if (isActive) return true;
        if (!canBackgroundActive.IsUnlocked()) return false;
        return guildCtrl.CanActive();
    }
    public void SwitchActive()
    {
        if (!ActiveInteractable()) return;
        SwitchActive(!isActive);
    }
    public void SwitchActive(bool toActive)
    {
        isActive = toActive;
    }
    public bool isPlaying { get=> game.currentHero == heroKind; }
    public void SwitchPlay()
    {
        if (guildCtrl.isTryingSwitchPlay) return;
        guildCtrl.isTryingSwitchPlay = true;
        //if (!isActive)//Playするときは必ずActiveにする
        //{
        if (!guildCtrl.CanActive() && !isActive)//これ以上枠がない場合はfalse
            guildCtrl.Member(game.currentHero).SwitchActive(false);
        if (guildCtrl.Member(game.currentHero).backgroundGainRate.Value() <= 0)//GainRateが0の時でもfalse
            guildCtrl.Member(game.currentHero).SwitchActive(false);
        SwitchActive(true);
        //}
        game.currentHero = heroKind;
        //main.saveCtrl.setSaveKey();
        //game.LoadScene();

        game.isPause = true;
        //AttackのリセットとDropのリセット
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].ResetAttack();
            game.battleCtrls[i].ResetDrop();
        }
        //ChallengeのQuit
        for (int i = 0; i < game.challengeCtrl.challengeList.Count; i++)
        {
            game.challengeCtrl.challengeList[i].Quit();
        }
        SwitchHeroUI();
    }
    async void SwitchHeroUI()
    {
        await GameControllerUI.gameUI.sceneUI.SwitchHero(heroKind);
        game.isPause = false;
        guildCtrl.isTryingSwitchPlay = false;
    }
    GuildController guildCtrl;
    public HeroKind heroKind;
    public bool isActive { get => game.battleCtrls[(int)heroKind].isActiveBattle; set => game.battleCtrls[(int)heroKind].isActiveBattle = value; }
    public Unlock canBackgroundActive;//BackgroundでActive可能かどうか
    public Multiplier backgroundGainRate;
}

public class GuildAbilityPointLeft : NUMBER
{
    public override double value { get => main.SR.guildAbilityPoint; set => main.SR.guildAbilityPoint = value; }
}

public class GuildAbility
{
    public GuildAbility(GuildController guildCtrl, GuildAbilityKind kind, GuildAbilityPointLeft pointLeft)
    {
        this.guildCtrl = guildCtrl;
        this.kind = kind;
        level = new GuildAbilityLevel(kind, () => Ability(kind).maxLevel);
        transaction = new Transaction(level, pointLeft, () => Ability(kind).initCost, () => Ability(kind).baseCost, Ability(kind).isLinear);
        transaction.isOnBuyOneToggle = () => main.S.isToggleOn[(int)ToggleKind.BuyOneGuildAbility];
    }
    public void Start()
    {
        SetEffect();
    }
    public double effectValue { get => AbilityEffectValue(kind, level.value); }
    public double nextEffectValue { get => AbilityEffectValue(kind, transaction.ToLevel()); }
    void SetEffect()
    {
        switch (kind)
        {
            case GuildAbilityKind.GlobalSkillSlot:
                game.statsCtrl.globalSkillSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.EquipmentInventory:
                game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.EnchantInventory:
                game.inventoryCtrl.enchantUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.PotionInventory:
                game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.MysteriousWater:
                game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            //case GuildAbilityKind.HP:
            //    game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.MP:
            //    game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.ATK:
            //    game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.MATK:
            //    game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.DEFMDEF:
            //    game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.SPD:
            //    game.statsCtrl.SetEffect(BasicStatsKind.SPD, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.ElementResistance:
            //    game.statsCtrl.SetEffect(Stats.FireRes, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    game.statsCtrl.SetEffect(Stats.IceRes, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    game.statsCtrl.SetEffect(Stats.ThunderRes, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    game.statsCtrl.SetEffect(Stats.LightRes, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    game.statsCtrl.SetEffect(Stats.DarkRes, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.MoveSpeed:
            //    game.statsCtrl.SetEffect(Stats.MoveSpeed, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            //case GuildAbilityKind.EquipDropChance:
            //    game.statsCtrl.SetEffect(Stats.EquipmentDropChance, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
            //    break;
            case GuildAbilityKind.GuildExpGain:
                guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.StoneGain:
                game.statsCtrl.globalStats[(int)GlobalStats.StoneGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.CrystalGain:
                game.statsCtrl.globalStats[(int)GlobalStats.CrystalGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.LeafGain:
                game.statsCtrl.globalStats[(int)GlobalStats.LeafGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.SkillProficiency:
                game.statsCtrl.SetEffect(Stats.SkillProficiencyGain, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;

            //WA以降
            case GuildAbilityKind.GoldCap:
                game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.GoldGain:
                game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.Trapping:
                game.monsterCtrl.trapNotConsumedChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.UpgradeCost:
                game.upgradeCtrl.costReduction.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;

            case GuildAbilityKind.EquipmentProficiency:
                game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
            case GuildAbilityKind.PhysicalAbsorption:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Physical).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                }
                break;
            case GuildAbilityKind.MagicalAbsoption:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                    game.statsCtrl.ElementAbsorption((HeroKind)count, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                }
                break;
            //case GuildAbilityKind.BonusAbilityPointRebirth:
            //    break;
            case GuildAbilityKind.MaterialDrop:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    game.statsCtrl.MaterialLootGain((HeroKind)count).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                }
                break;
            case GuildAbilityKind.NitroCap:
                game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                break;
            case GuildAbilityKind.ExpGain:
                game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Mul, () => effectValue));
                break;
                //case GuildAbilityKind.ActiveNum:
                //    guildCtrl.activableNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Guild, MultiplierType.Add, () => effectValue));
                //    break;
        }
    }

    GuildController guildCtrl;
    public GuildAbilityKind kind;
    public GuildAbilityLevel level;
    public Transaction transaction;
    //public long unlockLevel { get => 0; }//Ability(kind).unlockLevel; }解禁はなしにした
    public bool IsUnlocked()
    {
        if ((int)kind < (int)GuildAbilityKind.GoldCap) return true;
        switch (kind)
        {
            case GuildAbilityKind.GoldCap:
                return game.ascensionCtrl.worldAscensions[0].performedNum > 0;
            case GuildAbilityKind.GoldGain:
                return game.ascensionCtrl.worldAscensions[0].performedNum > 0;
            case GuildAbilityKind.Trapping:
                return game.ascensionCtrl.worldAscensions[0].performedNum > 0;
            case GuildAbilityKind.UpgradeCost:
                return game.ascensionCtrl.worldAscensions[0].performedNum > 0;
            //case GuildAbilityKind.PhysicalAbsorption:
            //    break;
            //case GuildAbilityKind.MagicalAbsoption:
            //    break;
            //case GuildAbilityKind.ExpGain:
            //    break;
            //case GuildAbilityKind.EquipmentProficiency:
            //    break;
            //case GuildAbilityKind.MaterialDrop:
            //    break;
            //case GuildAbilityKind.NitroCap:
            //    break;
        }
        return false;//guildCtrl.Level() >= unlockLevel;
    }
}
public class GuildAbilityLevel : INTEGER
{
    GuildAbilityKind kind;
    public GuildAbilityLevel(GuildAbilityKind kind, Func<long> maxValue)
    {
        this.kind = kind;
        this.maxValue = maxValue;
    }
    public override void Increase(long increment)
    {
        base.Increase(increment);
        IncreaseAction();
        if (increaseUIAction != null) increaseUIAction();
    }
    public override void ChangeValue(long toValue)
    {
        base.ChangeValue(toValue);
        IncreaseAction();
        if (increaseUIAction != null) increaseUIAction();
    }
    void IncreaseAction()
    {
        switch (kind)
        {
            //case GuildAbilityKind.AbilityPoint:
            //    for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
            //    {
            //        game.statsCtrl.AbilityPointLeft((HeroKind)i).Increase(1);
            //    }
            //    break;
            default:
                break;
        }
    }
    public Action increaseUIAction;
    public override long value { get => main.SR.guildAbilityLevels[(int)kind]; set => main.SR.guildAbilityLevels[(int)kind] = value; }
}

public class GuildLevel : INTEGER
{
    public override long value { get => main.SR.guildLevel; set => main.SR.guildLevel = value; }
    public GuildLevel(Func<long> maxValue)
    {
        this.maxValue = maxValue;
    }
    public override void Increase(long increment)
    {
        base.Increase(increment);
        game.guildCtrl.abilityPointLeft.Increase(increment);
        for (int i = 0; i < increment; i++)
        {
            game.guildCtrl.accomplishGuildLevels[Math.Max(0, value - i)].RegisterTime();
        }
        if (logUIAction != null) logUIAction();

        main.S.maxGuildLevel = Math.Max(main.S.maxGuildLevel, value);
    }
    public Action logUIAction;
}
public class GuildExp : EXP
{
    public override double value { get => main.SR.guildExp; set => main.SR.guildExp = value; }
    public GuildExp(Func<long, double> requiredExp, INTEGER level)
    {
        this.requiredValue = requiredExp;
        this.level = level;
    }

    public double ExpGain(long initHeroLevel, long incrementHeroLevel)
    {
        long tempLevel = initHeroLevel;
        double tempValue = 0;
        for (int i = 0; i < incrementHeroLevel; i++)
        {
            tempValue += ExpGainPerHeroLevel(tempLevel);
            tempLevel++;
        }
        //ここにGuildExpGainの倍率をかける
        tempValue *= game.guildCtrl.expGain.Value();
        return tempValue;
    }
    public void GainExp(long initHeroLevel, long incrementHeroLevel)
    {
        Increase(ExpGain(initHeroLevel, incrementHeroLevel));
    }
    public override void Increase(double increment)
    {
        base.Increase(increment);
        if (logUIAction != null) logUIAction(increment);
    }
    public Action<double> logUIAction;
}
public class AccomplishGuildLevel : ACCOMPLISH
{
    public AccomplishGuildLevel(int guildLevel)
    {
        this.guildLevel = guildLevel;
    }
    int guildLevel;
    public override double accomplishedFirstTime { get => main.SR.accomplishedFirstTimesGuildLevel[guildLevel]; set => main.SR.accomplishedFirstTimesGuildLevel[guildLevel] = value; }
    public override double accomplishedTime { get => main.SR.accomplishedTimesGuildLevel[guildLevel]; set => main.SR.accomplishedTimesGuildLevel[guildLevel] = value; }
    public override double accomplishedBestTime { get => main.SR.accomplishedBestTimesGuildLevel[guildLevel]; set => main.SR.accomplishedBestTimesGuildLevel[guildLevel] = value; }

}
