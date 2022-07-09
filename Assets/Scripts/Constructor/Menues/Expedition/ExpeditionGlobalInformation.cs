using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static MonsterParameter;
using System;
using Cysharp.Threading.Tasks;

public partial class Save
{
    public ExpeditionKind[] expeditionKinds;//[id]
    public long[] expeditionLevels;//[ExpeditionKind]
    public double[] expeditionExps;//[ExpeditionKind]

    public double[] expeditionCompletedNums;//[ExpeditionKind]
    public double[] expeditionTimes;//[ExpeditionKind]
}

public class Expedition_Brick : ExpeditionGlobalInformation
{
    public Expedition_Brick(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.Brick;
    public override string name => "Manufacturing Bricks";
    public override string effectString => "Bricks Gain : <color=green>" + percent(Math.Pow(1.1d, level.value)) + "</color>  ( x1.1 per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.01d;
    public override string passiveEffectString => optStr + "Town Brick Buildings' level effect <color=green>+ " + percent(EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        game.townCtrl.townLevelEffectMultipliers[0].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Add, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return tDigit(RewardAmount(expedition, pet, timeHour)) + " " + game.townCtrl.TownMaterial(townMatBricks[(int)pet.species]).Name();
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        game.townCtrl.TownMaterial(townMatBricks[(int)pet.species]).Increase(RewardAmount(expedition, pet, timeHour));
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 50 * Math.Pow(1.1d, level.value);
        tempAmount *= game.townCtrl.GlobalTownMaterialGainMultiplier();
        tempAmount *= 1.0d * pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        return tempAmount;
    }
}
public class Expedition_Log : ExpeditionGlobalInformation
{
    public Expedition_Log(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.Log;
    public override string name => "Logging Trees";
    public override string effectString => "Logs Gain : <color=green>" + percent(Math.Pow(1.1d, level.value)) + "</color>  ( x1.1 per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.01d;
    public override string passiveEffectString => optStr + "Town Log Buildings' level effect <color=green>+ " + percent(EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        game.townCtrl.townLevelEffectMultipliers[1].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Add, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return tDigit(RewardAmount(expedition, pet, timeHour)) + " " + game.townCtrl.TownMaterial(townMatLogs[(int)pet.species]).Name();
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        game.townCtrl.TownMaterial(townMatLogs[(int)pet.species]).Increase(RewardAmount(expedition, pet, timeHour));
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 50 * Math.Pow(1.1d, level.value);
        tempAmount *= game.townCtrl.GlobalTownMaterialGainMultiplier();
        tempAmount *= 1.0d * pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        return tempAmount;
    }
}

public class Expedition_Shard : ExpeditionGlobalInformation
{
    public Expedition_Shard(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.Shard;
    public override string name => "Gathering Shards";
    public override string effectString => "Shards Gain : <color=green>" + percent(Math.Pow(1.1d, level.value)) + "</color>  ( x1.1 per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.01d;
    public override string passiveEffectString => optStr + "Town Shard Buildings' level effect <color=green>+ " + percent(EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        game.townCtrl.townLevelEffectMultipliers[2].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Add, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return tDigit(RewardAmount(expedition, pet, timeHour)) + " " + game.townCtrl.TownMaterial(townMatShards[(int)pet.species]).Name();
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        game.townCtrl.TownMaterial(townMatShards[(int)pet.species]).Increase(RewardAmount(expedition, pet, timeHour));
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 50 * Math.Pow(1.1d, level.value);
        tempAmount *= game.townCtrl.GlobalTownMaterialGainMultiplier();
        tempAmount *= 1.0d * pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        return tempAmount;
    }
}

public class Expedition_Equipment : ExpeditionGlobalInformation
{
    public Expedition_Equipment(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.Equipment;
    public override string name => "Training Equipment";
    public override string effectString => "Proficiency Scroll's Time : <color=green>" + percent(1 + 0.10d * level.value) + "</color>  ( + " + percent(0.10d) + " per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.05d;
    public override string passiveEffectString => optStr + "Multiply Equipment Proficiency Gain by <color=green>" + percent(1 + EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Mul, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return Localized.localized.EnchantName(EnchantKind.InstantProf) + " [ " + DoubleTimeToDate(RewardAmount(expedition, pet, timeHour)) + " ]";
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        game.inventoryCtrl.CreateEnchant(EnchantKind.InstantProf, EquipmentEffectKind.Nothing, 0, RewardAmount(expedition, pet, timeHour));
    }
    public override bool ClaimCondition(Expedition expedition)
    {
        return game.inventoryCtrl.CanCreateEnchant(expedition.PetNum());
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 1800 * (1 + 0.10d * level.value);
        tempAmount *= 1.0d + 0.1d * pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        return tempAmount;
    }
}
public class Expedition_PetRank : ExpeditionGlobalInformation
{
    public Expedition_PetRank(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.PetRank;
    public override string name => "Capturing Monsters";
    public override string effectString => "Taming Point : <color=green>" + percent(Math.Pow(1.1d, level.value)) + "</color>  ( x1.1 per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.05d;
    public override string passiveEffectString => optStr + "Multiply Taming Point Gain by <color=green>" + percent(1 + EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Mul, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return tDigit(RewardAmount(expedition, pet, timeHour), 1) + " " + pet.globalInfo.Name() + "'s Taming Point";
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        pet.tamingPoint.Increase(RewardAmount(expedition, pet, timeHour));
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 30 * Math.Pow(1.1d, level.value);
        tempAmount *= 1.0d + 0.1d * expedition.TotalRank();//Pet全員が影響する//pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        tempAmount *= game.statsCtrl.HeroStats(game.currentHero, Stats.TamingPointGain).Value();
        return tempAmount;
    }
}
public class Expedition_PetExp : ExpeditionGlobalInformation
{
    public Expedition_PetExp(ExpeditionController expeditionCtrl) : base(expeditionCtrl)
    {
    }

    public override ExpeditionKind kind => ExpeditionKind.PetExp;
    public override string name => "Exploring Dungeons";
    public override string effectString => "Pet EXP Gain : <color=green>" + percent(Math.Pow(1.2d, level.value)) + "</color>  ( x1.2 per Level )";
    public override double passiveEffectValueIncrementPerLevel => 0.05d;
    public override string passiveEffectString => optStr + "Multiply Pet EXP Gain from any expeditions by <color=green>" + percent(1 + EffectValue()) + "</color>  ( + " + percent(passiveEffectValueIncrementPerLevel) + " / Lv )";
    public override void SetEffect()
    {
        expeditionCtrl.petExpGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Expedition, MultiplierType.Mul, () => EffectValue()));
    }
    public override string RewardString(Expedition expedition, MonsterPet pet, double timeHour)
    {
        return tDigit(RewardAmount(expedition, pet, timeHour)) + " " + pet.globalInfo.Name() + "'s Pet EXP";
    }
    public override void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour)
    {
        pet.exp.Increase(RewardAmount(expedition, pet, timeHour));
    }
    public override double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour)
    {
        double tempAmount = 3 * 3600 * Math.Pow(1.2d, level.value);
        tempAmount *= 1.0d + 0.1d * expedition.TotalRank();//Pet全員が影響する//* pet.rank.value;
        tempAmount *= Math.Pow(timeHour, 0.85d);
        tempAmount *= expeditionCtrl.petExpGainMultiplier.Value();
        return tempAmount;
    }
}



public class ExpeditionGlobalInformation
{
    public ExpeditionGlobalInformation(ExpeditionController expeditionCtrl)
    {
        this.expeditionCtrl = expeditionCtrl;
        level = new ExpeditionLevel(kind);
        exp = new ExpeditionExp(kind, level, RequiredExp);
        unlock = new Unlock();
    }
    public void Start()
    {
        SetEffect();
    }

    public void GetExp(double deltaTime, int petNum)
    {
        exp.Increase(deltaTime * petNum * ExpGainPerSecPerPet());
    }
    public virtual double RequiredExp(long level)
    {
        return 24 * 3600 * (1 + level);//24h,48h,
    }
    public double ExpGainPerSecPerPet()//Petの数に応じて倍になる
    {
        return 1
            * expeditionCtrl.expGainMultiplier.Value();
    }
    public virtual double PetExpGainPerSec()
    {
        return (1 + 0.20d * level.value)
            * expeditionCtrl.petExpGainMultiplier.Value();
    }
    public virtual string name => "";
    public virtual string effectString => "";
    public virtual string passiveEffectString => "";
    public virtual void SetEffect() { }
    public virtual double passiveEffectValueIncrementPerLevel => 0;
    public double EffectValue() { return passiveEffectValueIncrementPerLevel * level.value; }
    public virtual string RewardString(Expedition expedition, MonsterPet pet, double timeHour) { return ""; }
    public virtual double RewardAmount(Expedition expedition, MonsterPet pet, double timeHour) { return 0; }
    public virtual void GetRewardAction(Expedition expedition, MonsterPet pet, double timeHour) { }
    public virtual bool ClaimCondition(Expedition expedition) { return true; }
    public double completedNum { get => main.S.expeditionCompletedNums[(int)kind]; set => main.S.expeditionCompletedNums[(int)kind] = value; }
    public double totalTime { get => main.S.expeditionTimes[(int)kind]; set => main.S.expeditionTimes[(int)kind] = value; }
    public ExpeditionController expeditionCtrl;
    public virtual ExpeditionKind kind => ExpeditionKind.Brick;
    public ExpeditionLevel level;
    public ExpeditionExp exp;
    public Unlock unlock;

    //String
    public string NameString(bool isGreen = false)
    {
        if (isGreen) return optStr + name + " < <color=green>Lv " + tDigit(level.value) + "</color> >";
        return optStr + name + "  Lv " + tDigit(level.value); 
    }
    public string EffectString()
    {
        string tempStr = "<u>Expedition Effect</u>";
        tempStr += "\n- Complete Reward " + effectString;
        tempStr += "\n- Pet EXP Gain : <color=green>" + tDigit(PetExpGainPerSec(), 2) + " / sec</color>  ( + " + tDigit(0.20d, 2) + " / Lv )"; 
        return tempStr;
    }
    public string PassiveEffectString()
    {
        string tempStr = "<u>Passive Effect</u>";
        tempStr += "\n- " + passiveEffectString;
        return tempStr;
    }
    public string InfoString()
    {
        string tempStr = "<u>Information</u>";
        tempStr += "\n- Total Completed # " + tDigit(completedNum);
        tempStr += "\n- Total Time : " + DoubleTimeToDate(totalTime);
        return tempStr;
    }
}

public class ExpeditionLevel : INTEGER
{
    public ExpeditionLevel(ExpeditionKind kind)
    {
        this.kind = kind;
    }
    public ExpeditionKind kind;
    public override long value { get => main.S.expeditionLevels[(int)kind]; set => main.S.expeditionLevels[(int)kind] = value; }
}
public class ExpeditionExp : EXP
{
    public ExpeditionExp(ExpeditionKind kind, INTEGER level, Func<long, double> requiredValue)
    {
        this.level = level;
        this.requiredValue = requiredValue;
        this.kind = kind;
    }
    public ExpeditionKind kind;
    public override double value { get => main.S.expeditionExps[(int)kind]; set => main.S.expeditionExps[(int)kind] = value; }
}
public enum ExpeditionKind
{
    //Distant
    Brick,//+Material,EXP
    Log,
    Shard,
    Equipment,
    PetRank,
    PetExp,
    //Resource,
    //Equipment,
    //PetExp,
    //Nitro,
    //Venture,

    //TownMaterial,//Petによって異なる？
    //Material,//Common,Rare,CatalystCritical,TownMat,
    //PetRank,
    //PortalOrb,

    ////Passive
    //Gold,

}