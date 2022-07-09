using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static PotionParameter;
using static UsefulMethod;

public partial class SaveR
{
    public long[] alchemyUpgradeLevels;
}

public class AlchemyUpgrade
{
    public AlchemyController alchemyCtrl { get => game.alchemyCtrl; }
    public AlchemyPoint alchemyPoint { get => game.alchemyCtrl.alchemyPoint; }
    public AlchemyUpgradeKind kind;
    public AlchemyUpgradeLevel level;
    public Transaction transaction;
    public double effectValue { get => EffectValue(level.value); }
    public double nextEffectValue { get => EffectValue(transaction.ToLevel()); }
    public virtual double EffectValue(long level) { return 0; }
    public virtual void SetInfo() { }
    public virtual void SetEffect() { }
    public AlchemyUpgrade()
    {
        SetInfo();
        SetEffect();
    }
}

public class AlchemyUpgradeLevel : INTEGER
{
    public AlchemyUpgradeLevel(AlchemyUpgradeKind kind, Func<long> maxValue)
    {
        this.kind = kind;
        this.maxValue = maxValue;
    }
    AlchemyUpgradeKind kind;
    public override long value { get => main.SR.alchemyUpgradeLevels[(int)kind]; set => main.SR.alchemyUpgradeLevels[(int)kind] = value; }
}

public enum AlchemyUpgradeKind
{
    Purification,
    DeeperCapacity,
    CharmedLife,
    Catalystic,
    EssenceHoarder,
    PotentPotables,
    //BusierHands,
    //RecyclingPro,
    Aurumology,
    WaterPreservation,
    MaterialThrift,
    NitrousExtraction,
}

//AddHere
public class Purification : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.Purification;
        level = new AlchemyUpgradeLevel(kind, () => (long)alchemyCtrl.maxPurificationLevel.Value());
        transaction = new Transaction(level, alchemyPoint, () => 1, () => 1, true);
    }
    public override double EffectValue(long level) { return 0.001d * level; }
    public override void SetEffect()
    {
        alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class DeeperCapacity : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.DeeperCapacity;
        level = new AlchemyUpgradeLevel(kind, () => 20);
        transaction = new Transaction(level, alchemyPoint, () => 100, () => Math.Pow(10, 1 / 5d), false);
        transaction.additionalBuyAction = () =>
        {
            game.potionCtrl.maxStackNum.Calculate();
            game.inventoryCtrl.SlotUIActionWithPotionSlot();
        };
    }
    public override double EffectValue(long level) { return level; }
    public override void SetEffect()
    {
        game.potionCtrl.maxStackNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class CharmedLife : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.CharmedLife;
        level = new AlchemyUpgradeLevel(kind, () => 100);
        transaction = new Transaction(level, alchemyPoint, () => 50, () => Math.Pow(10, 1 / 10d), false);
    }
    public override double EffectValue(long level) { return 0.005d * level; }
    public override void SetEffect()
    {
        game.potionCtrl.preventConsumeChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class Catalystic : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.Catalystic;
        level = new AlchemyUpgradeLevel(kind, () => 3);
        transaction = new Transaction(level, alchemyPoint, () => 500, () => 100, false);
    }
    public override double EffectValue(long level) { return level; }
    public override void SetEffect()
    {
        game.catalystCtrl.availableNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class EssenceHoarder : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.EssenceHoarder;
        level = new AlchemyUpgradeLevel(kind, () => 10);
        transaction = new Transaction(level, alchemyPoint, () => 25, () => Math.Pow(200, 1 / 4d), false);
    }
    public override double EffectValue(long level) { return 0.05 * level; }
    public override void SetEffect()
    {
        game.catalystCtrl.essenceProductionMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class PotentPotables : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.PotentPotables;
        level = new AlchemyUpgradeLevel(kind, () => 20);
        transaction = new Transaction(level, alchemyPoint, () => 50, () => Math.Pow(200, 1 / 4d), false);
    }
    public override double EffectValue(long level) { return 0.025 * level; }
    public override void SetEffect()
    {
        game.potionCtrl.effectMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class Aurumology : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.Aurumology;
        level = new AlchemyUpgradeLevel(kind, () => 20);
        transaction = new Transaction(level, alchemyPoint, () => 50, () => Math.Pow(100, 1 / 4d), false);
    }
    public override double EffectValue(long level) { return 0.1 * level; }
    public override void SetEffect()
    {
        game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Mul, () => effectValue));
    }
}
public class WaterPreservation : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.WaterPreservation;
        level = new AlchemyUpgradeLevel(kind, () => (long)alchemyCtrl.maxWaterPreservationLevel.Value());
        transaction = new Transaction(level, alchemyPoint, () => 1000, () => 10, false);
    }
    public override double EffectValue(long level) { return 0.0025 + 0.0005 * level; }
    public override void SetEffect()
    {
        game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AlchemyExpand, MultiplierType.Mul, () => effectValue * game.alchemyCtrl.mysteriousWaterExpandedCapNum.value));
    }
}
public class MaterialThrift : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.MaterialThrift;
        level = new AlchemyUpgradeLevel(kind, () => 100);
        transaction = new Transaction(level, alchemyPoint, () => 200, () => Math.Pow(100, 1 / 20d), false);
    }
    public override double EffectValue(long level) { return 0.002 * level; }
    public override void SetEffect()
    {
        game.alchemyCtrl.preventUseEssenceToCraft.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}
public class NitrousExtraction : AlchemyUpgrade
{
    public override void SetInfo()
    {
        kind = AlchemyUpgradeKind.NitrousExtraction;
        level = new AlchemyUpgradeLevel(kind, () => 10);
        transaction = new Transaction(level, alchemyPoint, () => 5000, () => 20, false);
    }
    public override double EffectValue(long level) { return level; }
    public override void SetEffect()
    {
        game.alchemyCtrl.nitroGainOnCraft.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Alchemy, MultiplierType.Add, () => effectValue));
    }
}