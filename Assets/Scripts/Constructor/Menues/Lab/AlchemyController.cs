using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static PotionParameter;
using static UsefulMethod;

public partial class Save
{
    public double talismanFragement;
    public double totalAlchemyPointGained;//WAでもリセットしない。WATier2でリセットする場合はアップグレードなどから計算する
}
public partial class SaveR
{
    public double alchemyPoint;
    public double mysteriousWater;
    public double mysteriousWaterProgress;
    public double mysteriousWaterExpandedCapNum;
}
public class AlchemyController
{
    public AlchemyController()
    {
        alchemyPoint = new AlchemyPoint();
        talismanFragment = new TalismanFragment();
        maxMysteriousWaterExpandedCapNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 50));
        mysteriousWaterExpandedCapNum = new MysteriousWaterExpandedCapNum(() => maxMysteriousWaterExpandedCapNum.Value());
        mysteriousWaterCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => mysteriousWaterExpandedCapNum.value));
        mysteriousWater = new MysteriousWater(() => mysteriousWaterCap.Value());
        mysteriousWaterProgress = new MysteriousWaterProgress(mysteriousWater, () => 1);
        mysteriousWaterProductionPerSec = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0.1d));
        doubleAlchemyPointChance = new Multiplier();
        alchemyPointGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        preventUseEssenceToCraft = new Multiplier();
        nitroGainOnCraft = new Multiplier();

        //Upgrade
        maxPurificationLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 50));
        maxWaterPreservationLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 5));
    }
    public void Start()
    {
        //Add Here
        alchemyUpgrades.Add(new Purification());
        alchemyUpgrades.Add(new DeeperCapacity());
        alchemyUpgrades.Add(new CharmedLife());
        alchemyUpgrades.Add(new Catalystic());
        alchemyUpgrades.Add(new EssenceHoarder());
        alchemyUpgrades.Add(new PotentPotables());
        alchemyUpgrades.Add(new Aurumology());
        alchemyUpgrades.Add(new WaterPreservation());
        alchemyUpgrades.Add(new MaterialThrift());
        alchemyUpgrades.Add(new NitrousExtraction());
    }
    public void Update()
    {
        ProgressMysteriousWater(Time.deltaTime);
        catalystCtrl.Update();
    }
    void ProgressMysteriousWater(float timesec)
    {
        mysteriousWaterProgress.Increase(AvailableWaterPerSec() * timesec);
    }
    public bool CanExpandMysteriousWater()
    {
        return !mysteriousWaterExpandedCapNum.IsMaxed() && mysteriousWater.IsMaxed();
    }
    public void ExpandMysteriousWaterCap()
    {
        if (!CanExpandMysteriousWater()) return;
        mysteriousWater.ChangeValue(0);
        mysteriousWaterProgress.ChangeValue(0);
        mysteriousWaterExpandedCapNum.Increase(1);
    }
    public float MysteriousWaterProgressPercent()
    {
        return (float)(mysteriousWaterProgress.value / 1d);
    }

    //AllocateWaterToCatalyst
    public double AvailableWaterPerSec()
    {
        return mysteriousWaterProductionPerSec.Value() - catalystCtrl.TotalAllocatedWaterPerSec();
    }
    public bool CanAllocateWaterPerSec(long value)
    {
        return AvailableWaterPerSec() >= value / 10d;
    }

    public void CheckMysteriousWaterAssignments()
    {
        if (AvailableWaterPerSec() >= 0) return;
        for (int i = 0; i < catalystCtrl.catalysts.Count; i++)
        {
            //catalystCtrl.catalysts[i].isEquipped = false;
            catalystCtrl.catalysts[i].RemoveWater();
        }

    }

    public CatalystController catalystCtrl { get => game.catalystCtrl; }
    public AlchemyPoint alchemyPoint;
    public TalismanFragment talismanFragment;
    public MysteriousWater mysteriousWater;
    public MysteriousWaterProgress mysteriousWaterProgress;
    public Multiplier mysteriousWaterProductionPerSec;
    public Multiplier mysteriousWaterCap;
    public Multiplier maxMysteriousWaterExpandedCapNum;
    public MysteriousWaterExpandedCapNum mysteriousWaterExpandedCapNum;
    public Multiplier doubleAlchemyPointChance;
    public Multiplier alchemyPointGainMultiplier;

    //Upgrade
    public List<AlchemyUpgrade> alchemyUpgrades = new List<AlchemyUpgrade>();
    public Multiplier maxPurificationLevel;
    public Multiplier maxWaterPreservationLevel;

    public Multiplier preventUseEssenceToCraft;
    public Multiplier nitroGainOnCraft;

    public bool isAutoExpandCap { get => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.ExpandMysteriousWaterCap); }
    //AutoExpandCap
    public void AutoExpandCap()
    {
        if (!isAutoExpandCap) return;
        ExpandMysteriousWaterCap();
    }
}


public class MysteriousWaterProgress : PROGRESS
{
    public override double value { get => main.SR.mysteriousWaterProgress; set => main.SR.mysteriousWaterProgress = value; }
    public MysteriousWaterProgress(NUMBER mysteriousWater, Func<double> requiredValue)
    {
        point = mysteriousWater;
        this.requiredValue = requiredValue;
    }
}

public class AlchemyPoint : NUMBER
{
    public override double value { get => main.SR.alchemyPoint; set => main.SR.alchemyPoint = value; }
    public override string Name()
    {
        return "Alchemy Point";
    }
    public override void Increase(double increment)
    {
        base.Increase(increment);
        main.S.totalAlchemyPointGained += increment;
    }
}
public class TalismanFragment : NUMBER
{
    public override string Name()
    {
        return "Talisman Fragments";
    }
    public override double value { get => main.S.talismanFragement; set => main.S.talismanFragement = value; }
}

public class MysteriousWaterExpandedCapNum : NUMBER
{
    public MysteriousWaterExpandedCapNum(Func<double> maxValue)
    {
        this.maxValue = maxValue;
    }
    public override double value { get => main.SR.mysteriousWaterExpandedCapNum; set => main.SR.mysteriousWaterExpandedCapNum = value; }
}

public class MysteriousWater : NUMBER
{
    public MysteriousWater(Func<double> maxValue)
    {
        this.maxValue = maxValue;
    }
    public override double value { get => main.SR.mysteriousWater; set => main.SR.mysteriousWater = value; }
}
