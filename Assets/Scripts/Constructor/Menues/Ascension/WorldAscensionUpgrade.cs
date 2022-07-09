using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using System;
using static MultiplierKind;
using static MultiplierType;

public partial class Save
{
    public long[] worldAscensionUpgradeLevels;//[kind]
}

public class WAU_GuildExpGain : WorldAscensionUpgrade
{
    public WAU_GuildExpGain(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.GuildExpGain;
    public override long maxLevel => 20;
    public override string Name()
    {
        return "Guild EXP Booster";
    }
    public override string EffectString(double value)
    {
        return "Multiplies Guild EXP Gain by " + percent(1 + value);
    }
    public override double EffectValue(long level)
    {
        return 0.25d * level;
    }
    public override double Cost(long level)
    {
        return 1 + level;
    }
    public override void SetEffect()
    {
        game.guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(Ascension, Mul, () => effectValue));
    }
}
public class WAU_AreaClearCount : WorldAscensionUpgrade
{
    public WAU_AreaClearCount(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.AreaClearCount;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Area Booster";
    }
    public override string EffectString(double value)
    {
        return "Increases Area Clear # and Clear Reward by " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return level;
    }
    public override double Cost(long level)
    {
        return 2 + 2 * level;
    }
    public override void SetEffect()
    {
        for (int i = 0; i < game.areaCtrl.clearCountBonusClass.Length; i++)
        {
            game.areaCtrl.clearCountBonusClass[i].RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
        }
    }
}
public class WAU_ActiveHero : WorldAscensionUpgrade
{
    public WAU_ActiveHero(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.ActiveHero;
    public override long maxLevel => 1;
    public override string Name()
    {
        return "Active Hero Slot Expansion";
    }
    public override string EffectString(double value)
    {
        return "Increases activable Hero slot by " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return level;
    }
    public override double Cost(long level)
    {
        return 10;
    }
    public override void SetEffect()
    {
        game.guildCtrl.activableNum.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
    }
}
public class WAU_SkillProfGain : WorldAscensionUpgrade
{
    public WAU_SkillProfGain(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.SkillProfGain;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Skill Proficiency Boost";
    }
    public override string EffectString(double value)
    {
        return "Increases Skill Proficiency Gain by " + percent(value);
    }
    public override double EffectValue(long level)
    {
        return 0.25d * level;
    }
    public override double Cost(long level)
    {
        return 2;
    }
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.SkillProficiencyGain, new MultiplierInfo(Ascension, Mul, () => effectValue));
    }
}

public class WAU_PreRebirthTier1 : WorldAscensionUpgrade
{
    public WAU_PreRebirthTier1(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.PreRebirthTier1;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Pre-Rebirth Tier 1";
    }
    public override string EffectString(double value)
    {
        return "Adds free Rebirth Tier 1 Point + " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return level * 1000;
    }
    public override double Cost(long level)
    {
        return 1;
    }
    public override void BuyAction(long levelIncrement)
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            double tempPointIncrement = EffectValue(level.value) - EffectValue(level.value - levelIncrement);
            tempPointIncrement *= effectMultiplier.Value();
            game.rebirthCtrl.Rebirth(heroKind, 0).rebirthPoint.Increase(tempPointIncrement);
        }
    }
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.rebirthCtrl.Rebirth(heroKind, 0).initRebirthPoint.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
        }
    }
}
public class WAU_PreRebirthTier2 : WorldAscensionUpgrade
{
    public WAU_PreRebirthTier2(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.PreRebirthTier2;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Pre-Rebirth Tier 2";
    }
    public override string EffectString(double value)
    {
        return "Adds free Rebirth Tier 2 Point + " + tDigit(value);
    }
    public override string EffectString()
    {
        return base.EffectString() + "\n<color=yellow>Need Temple Rank 1 to access the Tier 2 Rebirth upgrades</color>";
    }
    public override double EffectValue(long level)
    {
        return level * 1000;
    }
    public override double Cost(long level)
    {
        return 2;
    }
    public override void BuyAction(long levelIncrement)
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            double tempPointIncrement = EffectValue(level.value) - EffectValue(level.value - levelIncrement);
            tempPointIncrement *= effectMultiplier.Value();
            game.rebirthCtrl.Rebirth(heroKind, 1).rebirthPoint.Increase(tempPointIncrement);
        }
    }
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.rebirthCtrl.Rebirth(heroKind, 1).initRebirthPoint.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
        }
    }
}
public class WAU_RebirthTier1BonusCap : WorldAscensionUpgrade
{
    public WAU_RebirthTier1BonusCap(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.RebirthTier1BonusCap;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Rebirth Tier 1 Bonus Expansion";
    }
    public override string EffectString(double value)
    {
        return "Increases the level cap for Tier 1 Rebirth Bonus Ability Points by Lv " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return 25 * level;
    }
    public override double Cost(long level)
    {
        return 1 + level;
    }
    public override void SetEffect()
    {
        game.rebirthCtrl.tier1AbilityPointBonusLevelCap.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
    }
}
public class WAU_RebirthTier2BonusCap : WorldAscensionUpgrade
{
    public WAU_RebirthTier2BonusCap(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.RebirthTier2BonusCap;
    public override long maxLevel => 10;
    public override string Name()
    {
        return "Rebirth Tier 2 Bonus Expansion";
    }
    public override string EffectString(double value)
    {
        return "Increases the level cap for Tier 2 Rebirth Bonus Ability Points by Lv " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return 20 * level;
    }
    public override double Cost(long level)
    {
        return 2 + 2 * level;
    }
    public override void SetEffect()
    {
        game.rebirthCtrl.tier2AbilityPointBonusLevelCap.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
    }
}
public class WAU_PointGainBonus : WorldAscensionUpgrade
{
    public WAU_PointGainBonus(WorldAscension wa) : base(wa)
    {
    }
    public override AscensionUpgradeKind kind => AscensionUpgradeKind.PointGainBonus;
    public override long maxLevel => 3;
    public override string Name()
    {
        return "WA Milestone Point Efficiency";
    }
    public override string EffectString(double value)
    {
        return "Increases the point gain per level of WA Milestones by " + tDigit(value);
    }
    public override double EffectValue(long level)
    {
        return level;
    }
    public override double Cost(long level)
    {
        return 25 * Math.Pow(2, level);
    }
    public override void SetEffect()
    {
        wa.pointGainBonus.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => effectValue));
    }
}




public enum AscensionUpgradeKind
{
    GuildExpGain,
    AreaClearCount,
    ActiveHero,
    SkillProfGain,
    PreRebirthTier1,
    PreRebirthTier2,
    RebirthTier1BonusCap,
    RebirthTier2BonusCap,
    PointGainBonus,

    //EquipProfGain,これはMilestoneにある

    //MaterialGain,//これ絶対必要->Tier1ではリセットしないことにした

    //HeroExpGain,
    //GoldGain,
    //SlimeCoinGain,
    //TownMaterialGain,
    //MysteriousWater,
    //MaxAreaPrestigeLevel,
    //AreaPrestigeExpEffect,
}

public class WorldAscensionUpgrade
{
    public WorldAscensionUpgrade(WorldAscension wa)
    {
        this.wa = wa;
        effectMultiplier = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        SetCost();
    }
    public void Start()
    {
        SetEffect();
    }
    public WorldAscension wa;
    public virtual AscensionUpgradeKind kind { get; }
    public WorldAscensionUpgradeLevel level;
    public Transaction transaction;
    public virtual long maxLevel => 1;
    public virtual double Cost(long level) { return 1 + level; }
    public virtual double EffectValue(long level) { return 0; }
    public Multiplier effectMultiplier;
    public double effectValue => EffectValue(level.value) * effectMultiplier.Value();
    public double nextEffectValue => EffectValue(transaction.ToLevel()) * effectMultiplier.Value();
    public void SetCost()
    {
        level = new WorldAscensionUpgradeLevel(this, () => maxLevel);
        transaction = new Transaction(level, wa.point, Cost);
        transaction.additionalBuyActionWithLevelIncrement = BuyAction;
        transaction.isOnBuyOneToggle = () => main.S.isToggleOn[(int)ToggleKind.BuyOneWorldAscensionUpgrade];
    }
    public virtual void SetEffect() { }
    public virtual void BuyAction(long levelIncrement) { }
    public virtual string Name() { return ""; }
    public virtual string EffectString(double value) { return ""; }
    public virtual string EffectString()
    {
        string tempStr = optStr + "<size=20><u>Effect</u><size=18>";
        tempStr += "\n- Current : " + EffectString(effectValue);
        if (level.IsMaxed()) return tempStr;
        tempStr += "\n- Next : " + EffectString(nextEffectValue) + " ( <color=green>Lv " + tDigit(transaction.ToLevel()) + "</color> )";
        return tempStr;
    }
    string CostString()
    {
        string tempStr = optStr + "<size=20><u>Cost</u><size=18>";
        tempStr += "\n- " + tDigit(transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(transaction.ToLevel()) + "</color> )";
        return tempStr;
    }
    string NameString()
    {
        return Name() + " < <color=green>Lv " + tDigit(level.value) + "</color> >";
    }
    public string DescriptionString()
    {
        string tempStr = optStr + "<size=20>" + NameString() + "<size=18>";
        tempStr += "\n\n<size=20><u>Information</u><size=18>";
        tempStr += "\n- Max Level : Lv " + tDigit(level.maxValue());
        tempStr += "\n\n" + EffectString();
        if (level.IsMaxed()) return tempStr;
        tempStr += "\n\n" + CostString();
        return tempStr;
    }
}
public class WorldAscensionUpgradeLevel : INTEGER
{
    public WorldAscensionUpgradeLevel(WorldAscensionUpgrade upgrade, Func<long> maxValue)
    {
        this.upgrade = upgrade;
        this.maxValue = maxValue;
    }
    public WorldAscensionUpgrade upgrade;
    public override long value { get => main.S.worldAscensionUpgradeLevels[(int)upgrade.kind]; set => main.S.worldAscensionUpgradeLevels[(int)upgrade.kind] = value; }
}

