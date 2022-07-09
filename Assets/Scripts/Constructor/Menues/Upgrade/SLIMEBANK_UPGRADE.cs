using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static UsefulMethod;
using static SkillParameter;
using static GameController;
using System;
using static SlimeBankUpgradeKind;
using static MultiplierKind;
using static MultiplierType;

public class SB_SlimeCoinCap : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.resourceCtrl.slimeCoinCap.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => SlimeCoinCap;
    public override double Cost(long level)
    {
        return 10000 + 10000 * level;
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return 10000 + 10000 * tempLevel + 10 * Math.Pow(tempLevel, 2);//1000でちょうど同じ
    }
    public override string Name()
    {
        return "Slime Coin Cap";
    }
    public override string Description()
    {
        return "Increases Slime Coin Cap";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return tDigit(EffectValue(isNextValue));
    }
}
public class SB_SlimeCoinEfficiency : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => SlimeCoinEfficiency;
    public override double Cost(long level)
    {
        return 7500 * Math.Pow(2, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return 0.00050d + tempLevel * 0.00025d;
    }
    public override string Name()
    {
        return "Slime Coin Efficiency";
    }
    public override string Description()
    {
        return "Increases Slime Coin Gain per overflowed Gold ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return percent(EffectValue(isNextValue), 3);
    }
}
public class SB_UpgradeCostReduction : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.upgradeCtrl.costReduction.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => UpgradeCostReduction;
    public override double Cost(long level)
    {
        return 100 * Math.Pow(1.35d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return "Upgrade Tab Cost Reduction";
    }
    public override string Description()
    {
        return "Reduces upgrade's cost";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return percent(EffectValue(isNextValue));
    }
}

public class SB_ResourceBooster : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.upgradeCtrl.resourceMultiplier.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => ResourceBooster;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.20d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.25d;
    }
    public override string Name()
    {
        return "Resource Booster";
    }
    public override string Description()
    {
        return "Increases Resource Upgrade's effect";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_StatsBooster : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.upgradeCtrl.statsMultiplier.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => StatsBooster;
    public override double Cost(long level)
    {
        return 1000 * Math.Pow(1.5d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.20d;
    }
    public override string Name()
    {
        return "Stats Booster";
    }
    public override string Description()
    {
        return "Increases Stats Upgrade's effect";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_GoldCapBooster : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.upgradeCtrl.goldcapMultiplier.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => GoldCapBooster;
    public override double Cost(long level)
    {
        return 2000 * Math.Pow(1.5d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.20d;
    }
    public override string Name()
    {
        return "Gold Cap Booster";
    }
    public override string Description()
    {
        return "Multiply Gold Cap Upgrade's effect";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "by " + percent(1 + EffectValue(isNextValue));
    }
}
//
public class SB_MysteriousWaterGain : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => MysteriousWaterGain;
    public override double Cost(long level)
    {
        return 2000 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.002d;
    }
    public override string Name()
    {
        return "Mysterious Water Gain";
    }
    public override string Description()
    {
        return "Increases Mysterious Water Gain";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + tDigit(EffectValue(isNextValue), 3) + " / sec";
    }
}
public class SB_MaterialFinder : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < game.monsterCtrl.speciesMaterialDropChance.Length; i++)
        {
            game.monsterCtrl.speciesMaterialDropChance[i].RegisterMultiplier(new MultiplierInfo(Upgrade, Mul, () => EffectValue()));
        }
    }
    public override SlimeBankUpgradeKind slimebankKind => MaterialFinder;
    public override double Cost(long level)
    {
        return 5000 * Math.Pow(1.5d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.05d;
    }
    public override string Name()
    {
        return "Material Finder";
    }
    public override string Description()
    {
        return "Multiplies Common Material Drop Chance";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "by " + percent(1 + EffectValue(isNextValue));
    }
}
public class SB_ShopTimer : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.shopCtrl.restockTimesec.RegisterMultiplier(new MultiplierInfo(Upgrade, Add, () => -EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => ShopTimer;
    public override double Cost(long level)
    {
        return 10000 * Math.Pow(1.20d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return Math.Min(590, tempLevel * 5);
    }
    public override string Name()
    {
        return "Shop Timer Reduction";
    }
    public override string Description()
    {
        return "Decreases Shop's restock timer";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "by " + tDigit(EffectValue(isNextValue)) + " sec";
    }
}
public class SB_ResearchPower : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < game.townCtrl.researchPower.Length; i++)
        {
            game.townCtrl.researchPower[i].RegisterMultiplier(new MultiplierInfo(Upgrade, Mul, () => EffectValue()));
        }
    }
    public override SlimeBankUpgradeKind slimebankKind => ResearchPower;
    public override double Cost(long level)
    {
        return 5000 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.10d;
    }
    public override string Name()
    {
        return "Town Research Booster";
    }
    public override string Description()
    {
        return "Multiplies Town Research Power ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "by " + percent(1 + EffectValue(isNextValue));
    }
}

//Stats
public class SB_SPD : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(BasicStatsKind.SPD, new MultiplierInfo(MultiplierKind.Upgrade, MultiplierType.Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => SPD;
    public override double Cost(long level)
    {
        return 1000 * Math.Pow(1.1d, level);//Math.Pow(1000, level / 100d);//=1.0715^level
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return 5 * tempLevel;
    }
    public override string Name()
    {
        return Localized.localized.BasicStats(BasicStatsKind.SPD);
    }
    public override string Description()
    {
        return "Increases SPD ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + tDigit(EffectValue(isNextValue));
    }
}
public class SB_FireRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.FireRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => FireRes;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.FireRes);
    }
    public override string Description()
    {
        return "Increases Fire Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_IceRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.IceRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => IceRes;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.IceRes);
    }
    public override string Description()
    {
        return "Increases Ice Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_ThunderRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.ThunderRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => ThunderRes;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.ThunderRes);
    }
    public override string Description()
    {
        return "Increases Thunder Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_LightRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.LightRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => LightRes;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.LightRes);
    }
    public override string Description()
    {
        return "Increases Light Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_DarkRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.DarkRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => DarkRes;
    public override double Cost(long level)
    {
        return 500 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.DarkRes);
    }
    public override string Description()
    {
        return "Increases Dark Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_DebuffRes : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.DebuffRes, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => DebuffRes;
    public override double Cost(long level)
    {
        return 4000 * Math.Pow(1.25d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.0025d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.DebuffRes);
    }
    public override string Description()
    {
        return "Increases Debuff Resistance ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_SkillProf : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.SkillProficiencyGain, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => SkillProf;
    public override double Cost(long level)
    {
        return 10000 * Math.Pow(1.20d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.05d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.SkillProficiencyGain);
    }
    public override string Description()
    {
        return "Increases Skill Proficiency Gain ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_EquipmentProf : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => EquipmentProf;
    public override double Cost(long level)
    {
        return 10000 * Math.Pow(1.20d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 0.05d;
    }
    public override string Name()
    {
        return Localized.localized.Stat(Stats.EquipmentProficiencyGain);
    }
    public override string Description()
    {
        return "Increases Equipment Proficiency Gain ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return "+ " + percent(EffectValue(isNextValue));
    }
}
public class SB_CritDamage : SLIMEBANK_UPGRADE
{
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(Stats.CriticalDamage, new MultiplierInfo(Upgrade, Add, () => EffectValue()));
    }
    public override SlimeBankUpgradeKind slimebankKind => CritDamage;
    public override double Cost(long level)
    {
        return 50000 * Math.Pow(1.10d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return 0.025d * tempLevel;
    }
    public override string Name()
    {
        return "Critical Damage";
    }
    public override string Description()
    {
        return "Increases Critical Damage ";
    }
    public override string EffectString(bool isNextValue = false)
    {
        return percent(EffectValue(isNextValue)) + " of normal damage";
    }
}

public class SLIMEBANK_UPGRADE : UPGRADE
{
    public virtual SlimeBankUpgradeKind slimebankKind { get; }
    public override UpgradeKind kind => UpgradeKind.SlimeBank;
    public override long queue { get => main.S.upgradeQueuesSlimebank[(int)slimebankKind]; set => main.S.upgradeQueuesSlimebank[(int)slimebankKind] = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueuesSlimebank[(int)slimebankKind]; set => main.S.upgradeIsSuperQueuesSlimebank[(int)slimebankKind] = value; }

    public override NUMBER resource => game.resourceCtrl.slimeCoin;
    public SLIMEBANK_UPGRADE()
    {
        id = (int)slimebankKind;
    }
}
