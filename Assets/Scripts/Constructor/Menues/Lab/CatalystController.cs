using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;

public partial class SaveR
{
    public long[] catalystLevels;//[CatalystKind]
    public bool[] isEquippedCatarysts;//[CatalystKind]
    public long[] essenceWaterPerSecs;//[EssenceKind]
    public double[] essenceProgresses;//[EssenceKind]
}

public class CatalystController
{
    public CatalystController()
    {
        catalystLevelCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => PotionParameter.catalystMaxLevel));
        catalystCostReduction = new Multiplier(() => 0.50d, () => 0);
        catalysts.Add(new SlimeCatalyst(this));
        catalysts.Add(new ManaCatalyst(this));
        catalysts.Add(new FrostCatalyst(this));
        catalysts.Add(new FlameCatalyst(this));
        catalysts.Add(new StormCatalyst(this));
        catalysts.Add(new SoulCatalyst(this));
        catalysts.Add(new SunCatalyst(this));
        catalysts.Add(new VoidCatalyst(this));
        //Add Here

        for (int i = 0; i < catalysts.Count; i++)
        {
            int count = i;
            essenceProductions.AddRange(catalysts[count].essenceProductionList);
        }

        availableNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        essenceProductionMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        criticalChanceMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
    }

    public Multiplier criticalChanceMultiplier;
    public Multiplier catalystLevelCap;
    public Multiplier catalystCostReduction;

    //Catalyst
    public List<Catalyst> catalysts = new List<Catalyst>();
    public Catalyst Catalyst(CatalystKind kind)
    {
        return catalysts[(int)kind];
    }
    //EssenceProduction
    public List<EssenceProduction> essenceProductions = new List<EssenceProduction>();
    public Multiplier essenceProductionMultiplier;

    public void Update()
    {
        for (int i = 0; i < catalysts.Count; i++)
        {
            catalysts[i].Update();
        }
    }
    public double TotalAllocatedWaterPerSec()
    {
        double tempValue = 0;
        for (int i = 0; i < catalysts.Count; i++)
        {
            tempValue += catalysts[i].TotalAllocatedWaterPerSec();
        }
        return tempValue;
    }
    public bool CanEquip()
    {
        return EquippedNum() < AvailableNum();
    }
    public Multiplier availableNum; 
    public int AvailableNum() { return (int)availableNum.Value(); }
    public int EquippedNum()
    {
        int tempValue = 0;
        for (int i = 0; i < catalysts.Count; i++)
        {
            if (catalysts[i].isEquipped) tempValue++;
        }
        return tempValue;
    }
}

public class SlimeCatalyst : Catalyst
{
    public SlimeCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Slime;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfSlime, 1));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfLife, 5));
        criticalMaterial = MaterialKind.SlimeBall;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 3 * costFactor, () => 3 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.OilOfSlime), () => 5 * costFactor, () => 5 * costFactor, true);
    }
}
public class ManaCatalyst : Catalyst
{
    public ManaCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Mana;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfMagic, 3));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfCreation, 7));
        criticalMaterial = MaterialKind.ManaSeed;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 30 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.EnchantedCloth), () => 5 * costFactor, () => 5 * costFactor, true);
    }
}
public class FrostCatalyst : Catalyst
{
    public FrostCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Frost;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfIce, 5));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfWinter, 10));
        criticalMaterial = MaterialKind.UnmeltingIce;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 120 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.FrostShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}
public class FlameCatalyst : Catalyst
{
    public FlameCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Flame;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfFire, 5));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfSummer, 10));
        criticalMaterial = MaterialKind.EternalFlame;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 120 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.FlameShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}
public class StormCatalyst : Catalyst
{
    public StormCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Storm;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfThunder, 5));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfWind, 10));
        criticalMaterial = MaterialKind.AncientBattery;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 120 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.LightningShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}
public class SoulCatalyst : Catalyst
{
    public SoulCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Soul;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfSpirit, 10));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfDeath, 20));
        criticalMaterial = MaterialKind.Ectoplasm;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 200 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.NatureShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}
public class SunCatalyst : Catalyst
{
    public SunCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Sun;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfLight, 15));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfRebirth, 30));
        criticalMaterial = MaterialKind.Stardust;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 350 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.NatureShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}
public class VoidCatalyst : Catalyst
{
    public VoidCatalyst(CatalystController catalystCtrl) : base(catalystCtrl)
    {
    }

    public override void SetInfo()
    {
        kind = CatalystKind.Void;
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfTime, 25));
        essenceProductionList.Add(new EssenceProduction(this, EssenceKind.EssenceOfEternity, 45));
        criticalMaterial = MaterialKind.VoidEgg;
    }
    public override void SetTransaction()
    {
        transaction = new CatalystTransaction(level, mysteriousWater, () => 500 * costFactor, () => 5 * costFactor, true);
        transaction.SetAnotherResource(materialCtrl.Material(MaterialKind.PoisonShard), () => 10 * costFactor, () => 10 * costFactor, true);
    }
}

//Add Here

public class Catalyst 
{
    public Catalyst(CatalystController catalystCtrl)
    {
        this.catalystCtrl = catalystCtrl;
        SetInfo();
        SetEssenceProgress();
        level = new CatalystLevel(kind, () => (long)catalystCtrl.catalystLevelCap.Value());
        SetTransaction();
        unlock = new Unlock();
    }
    public CatalystController catalystCtrl;
    public CatalystKind kind;
    public List<EssenceProduction> essenceProductionList = new List<EssenceProduction>();
    public MaterialKind criticalMaterial;
    public CatalystLevel level;
    public MaterialController materialCtrl { get => game.materialCtrl; }
    public EssenceController essenceCtrl { get => game.essenceCtrl; }
    public MysteriousWater mysteriousWater { get => game.alchemyCtrl.mysteriousWater; }
    public CatalystTransaction transaction;
    public virtual void SetInfo() { }
    public virtual void SetTransaction() { }
    public double petBonusRank { get => 0; }
    public double criticalChance { get => (0.0010d + 0.0001d * level.value) * game.catalystCtrl.criticalChanceMultiplier.Value();}
    public double costFactor => 1 - catalystCtrl.catalystCostReduction.Value();
    public Unlock unlock;
    public bool isEquipped { get => main.SR.isEquippedCatarysts[(int)kind]; set => main.SR.isEquippedCatarysts[(int)kind] = value; }
    public void Equip()
    {
        if (!unlock.IsUnlocked()) return;
        if (level.value <= 0) return;
        if (isEquipped)
        {
            isEquipped = false;
            RemoveWater();
        }
        else
        {
            if (!game.catalystCtrl.CanEquip()) return;
            isEquipped = true;
        }
    }
    public void RemoveWater()
    {
        for (int i = 0; i < essenceProductionList.Count; i++)
        {
            essenceProductionList[i].allocatedWaterPerSec.ChangeValue(0);
        }
    }
    void SetEssenceProgress()
    {
        for (int i = 0; i < essenceProductionList.Count; i++)
        {
            essenceProductionList[i].progress.pointIncreaseAction = LotteryCriticalMaterial;
        }
    }
    void LotteryCriticalMaterial(long lotteryTimes)
    {
        for (int i = 0; i < lotteryTimes; i++)
        {
            if (WithinRandom(criticalChance)) materialCtrl.Material(criticalMaterial).Increase(1);
        }
    }
    public void Update()
    {
        for (int i = 0; i < essenceProductionList.Count; i++)
        {
            essenceProductionList[i].ProgressEssence(Time.deltaTime);
        }
    }
    public double TotalAllocatedWaterPerSec()
    {
        long tempValue = 0;
        for (int i = 0; i < essenceProductionList.Count; i++)
        {
            tempValue += essenceProductionList[i].allocatedWaterPerSec.value;
        }
        return tempValue / 10d;
    }
}

public class CatalystTransaction : Transaction
{
    public List<MaterialKind> materialKindList = new List<MaterialKind>();
    public CatalystTransaction(INTEGER level, NUMBER resource, Func<double> initCost, Func<double> baseCost, bool isLinear = false) : base(level, resource, initCost, baseCost, isLinear)
    {
    }
    public void SetAnotherResource(Material material, Func<double> initCost, Func<double> baseCost, bool isLinear = false)
    {
        materialKindList.Add(material.kind);
        cost.Add(new TransactionCost(level, material, initCost, baseCost, isLinear));
    }
}

public class EssenceProduction
{
    public EssenceProduction(Catalyst catalyst, EssenceKind kind, double difficulty)
    {
        this.catalyst = catalyst;
        this.kind = kind;
        this.difficulty = difficulty;
        allocatedWaterPerSec = new EssenceWaterPerSec(kind);
        progress = new EssenceProgress(kind);
    }
    public Catalyst catalyst;
    public EssenceKind kind;
    public double difficulty;
    public EssenceProgress progress;
    public EssenceWaterPerSec allocatedWaterPerSec;
    public bool isAvailable { get => catalyst.isEquipped; }
    public bool CanAllocatedWaterPerSec(long value)
    {
        if (!isAvailable) return false;
        if (catalyst.level.value <= 0) return false;
        if (allocatedWaterPerSec.value + value < 0) return false;
        if (!game.alchemyCtrl.CanAllocateWaterPerSec(value)) return false;
        if (!catalyst.unlock.IsUnlocked()) return false;
        if (!catalyst.isEquipped) return false;
        return true;
    }
    public void AllocateWaterPerSec(long value)
    {
        if (!CanAllocatedWaterPerSec(value)) return;
        allocatedWaterPerSec.Increase(value);
    }
    public double ProductionPerSec()
    {
        if (allocatedWaterPerSec.value < 1) return 0;
        double allocatedWater = allocatedWaterPerSec.value * 0.1d;
        double tempValue = 0.1d * Math.Log(1 + allocatedWater * 10, 2);
        //tempValue += Math.Log(1 + catalyst.level.value, 10d / (1 + Math.Log10(1 + catalyst.level.value))) * (1 + allocatedWaterPerSec.value) / (1 + difficulty);
        tempValue += Math.Log(1 + catalyst.level.value / difficulty, 2d);
        //tempValue += allocatedWaterPerSec.value * catalyst.petBonusRank / difficulty;//現在0
        tempValue *= 10;
        tempValue *= Math.Log(1 + allocatedWater, 2);
        tempValue /= 60d;
        tempValue *= game.catalystCtrl.essenceProductionMultiplier.Value();
        return tempValue;
    }
    public void ProgressEssence(float timesec)
    {
        progress.Increase(ProductionPerSec() * timesec);
    }
}

public class EssenceProgress : PROGRESS
{
    public EssenceProgress(EssenceKind kind)
    {
        this.kind = kind;
        point = game.essenceCtrl.Essence(kind);
        requiredValue = () => 1.0d;
    }
    public EssenceKind kind;
    public override double value { get => main.SR.essenceProgresses[(int)kind]; set => main.SR.essenceProgresses[(int)kind] = value; }
}

public class CatalystLevel : INTEGER
{
    public CatalystLevel(CatalystKind kind, Func<long> maxLevel)
    {
        this.kind = kind;
        this.maxValue = maxLevel;
    }
    public CatalystKind kind;
    public override long value { get => main.SR.catalystLevels[(int)kind]; set => main.SR.catalystLevels[(int)kind] = value; }
}
public class EssenceWaterPerSec : INTEGER
{
    public EssenceWaterPerSec(EssenceKind essenceKind)
    {
        this.essenceKind = essenceKind;
    }
    public EssenceKind essenceKind;
    public override long value { get => main.SR.essenceWaterPerSecs[(int)essenceKind]; set => main.SR.essenceWaterPerSecs[(int)essenceKind] = value; }
}

public enum CatalystKind
{
    Slime,
    Mana,
    Frost,
    Flame,
    Storm,
    Soul,
    Sun,
    Void,
}
