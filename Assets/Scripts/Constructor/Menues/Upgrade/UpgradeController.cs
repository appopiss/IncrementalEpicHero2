using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static UsefulMethod;
using static SkillParameter;
using static GameController;
using System;

public partial class Save
{
    public long[] upgradeQueuesResource;
    public long[] upgradeQueuesBasicStats;
    public long upgradeQueuesGoldGain, upgradeQueueExpGain;
    public long[] upgradeQueuesGoldCap;
    public long[] upgradeQueuesSlimebank;
    public bool[] upgradeIsSuperQueuesResource;
    public bool[] upgradeIsSuperQueuesBasicStats;
    public bool upgradeIsSuperQueuesGoldGain, upgradeIsSuperQueueExpGain;
    public bool[] upgradeIsSuperQueuesGoldCap;
    public bool[] upgradeIsSuperQueuesSlimebank;

}
public partial class SaveR
{
    public long[] upgradeLevelsResource;
    public long[] upgradeLevelsBasicStats;
    public long upgradeLevelsGoldGain, upgradeLevelsExpGain;
    public long[] upgradeLevelsGoldCap;
    public long[] upgradeLevelsSlimebank;
}
public class UpgradeController
{
    UPGRADE[][] upgrades;
    public UPGRADE Upgrade(UpgradeKind kind, int id)
    {
        return upgrades[(int)kind][id];
    }
    public SLIMEBANK_UPGRADE SlimeBankUpgrade(SlimeBankUpgradeKind kind)
    {
        for (int i = 0; i < slimebankUpgrades.Length; i++)
        {
            if (slimebankUpgrades[i].slimebankKind == kind) return slimebankUpgrades[i];
        }
        return slimebankUpgrades[0];
    }
    ResourceUpgrade[] resourceUpgrades = new ResourceUpgrade[Parameter.resourceUpgradeTier];
    BasicStatsUpgrade[] basicStatsUpgrades = new BasicStatsUpgrade[Enum.GetNames(typeof(BasicStatsKind)).Length];
    UPGRADE goldGainUpgrade, expGainUpgrade;
    GoldCapUpgrade[] goldCapUpgrades = new GoldCapUpgrade[Enum.GetNames(typeof(ResourceKind)).Length];
    SLIMEBANK_UPGRADE[] slimebankUpgrades = new SLIMEBANK_UPGRADE[Enum.GetNames(typeof(SlimeBankUpgradeKind)).Length];
    public List<UPGRADE> upgradeList = new List<UPGRADE>();
    public Multiplier costReduction;
    public Multiplier costReductionFromWA;
    public Multiplier resourceMultiplier;
    public Multiplier statsMultiplier;
    public Multiplier goldcapMultiplier;
    public Multiplier[] goldcapMultipliers = new Multiplier[Enum.GetNames(typeof(ResourceKind)).Length];
    public UpgradeController()
    {
        costReduction = new Multiplier(() => 0.90d, () => 0);
        costReductionFromWA = new Multiplier(() => 0.90d, () => 0);
        resourceMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        statsMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        goldcapMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        for (int i = 0; i < goldcapMultipliers.Length; i++)
        {
            goldcapMultipliers[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        }
        for (int i = 0; i < resourceUpgrades.Length; i++)
        {
            resourceUpgrades[i] = new ResourceUpgrade(i);
        }
        for (int i = 0; i < basicStatsUpgrades.Length; i++)
        {
            int count = i;
            basicStatsUpgrades[i] = new BasicStatsUpgrade((BasicStatsKind)count);
        }
        goldGainUpgrade = new GoldGainUpgrade();
        expGainUpgrade = new ExpGainUpgrade();
        for (int i = 0; i < goldCapUpgrades.Length; i++)
        {
            int count = i;
            goldCapUpgrades[i] = new GoldCapUpgrade((ResourceKind)count);
        }
        //Slimebank
        slimebankUpgrades[0] = new SB_SlimeCoinCap();
        slimebankUpgrades[1] = new SB_SlimeCoinEfficiency();
        slimebankUpgrades[2] = new SB_UpgradeCostReduction();
        slimebankUpgrades[3] = new SB_ResourceBooster();
        slimebankUpgrades[4] = new SB_StatsBooster();
        slimebankUpgrades[5] = new SB_GoldCapBooster();
        //
        slimebankUpgrades[6] = new SB_MysteriousWaterGain();
        slimebankUpgrades[7] = new SB_MaterialFinder();
        slimebankUpgrades[8] = new SB_ShopTimer();
        slimebankUpgrades[9] = new SB_ResearchPower();
        //
        slimebankUpgrades[10] = new SB_SPD();
        slimebankUpgrades[11] = new SB_FireRes();
        slimebankUpgrades[12] = new SB_IceRes();
        slimebankUpgrades[13] = new SB_ThunderRes();
        slimebankUpgrades[14] = new SB_LightRes();
        slimebankUpgrades[15] = new SB_DarkRes();
        slimebankUpgrades[16] = new SB_DebuffRes();
        slimebankUpgrades[17] = new SB_SkillProf();
        slimebankUpgrades[18] = new SB_EquipmentProf();
        slimebankUpgrades[19] = new SB_CritDamage();


        //Register
        for (int i = 0; i < Enum.GetNames(typeof(ResourceKind)).Length; i++)
        {
            game.statsCtrl.ResourceGain((ResourceKind)i).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Upgrade, MultiplierType.Add, () => ResourceGain()));
        }

        upgrades = new UPGRADE[][]
        {
            resourceUpgrades,
            basicStatsUpgrades,
            new UPGRADE[] {goldGainUpgrade },
            new UPGRADE[] {expGainUpgrade },
            goldCapUpgrades,
            slimebankUpgrades,
        };

        upgradeList.AddRange(resourceUpgrades);
        upgradeList.AddRange(basicStatsUpgrades);
        upgradeList.Add(goldGainUpgrade);
        upgradeList.Add(expGainUpgrade);
        upgradeList.AddRange(goldCapUpgrades);
        upgradeList.AddRange(slimebankUpgrades);

        //Queue
        availableQueue = new Multiplier();
    }
    public void Start()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeList[i].Start();
        }
    }

    public long TotalLevel(UpgradeKind kind)
    {
        long tempLevel = 0;
        for (int i = 0; i < upgrades[(int)kind].Length; i++)
        {
            tempLevel += upgrades[(int)kind][i].level.value;
        }
        return tempLevel;
    }

    public double ResourceGain(bool isNextValue = false, int id = 0)
    {
        long[] levels = new long[resourceUpgrades.Length];
        double tempValue = 1;//Debugで1にしてる
        for (int i = 0; i < levels.Length; i++)
        {
            if (isNextValue && i == id) levels[i] = resourceUpgrades[i].transaction.ToLevel();
            else levels[i] = resourceUpgrades[i].level.value;
        }
        //ここから計算//([0]+[1]+...+[9])*(1+[1]+...+[9])*(1+[2]+...+[9])*...*(1+[9])
        for (int i = 0; i < levels.Length; i++)
        {
            tempValue += levels[i] / (1 + i);
        }
        for (int i = 1; i < levels.Length; i++)
        {
            double tempTempSumValue = 0;
            for (int j = i; j < levels.Length; j++)
            {
                tempTempSumValue += levels[j] / Math.Pow(1d + j, 1d / (1d + j)) / 2d;//2^1/2, 3^1/3
            }
            tempValue *= 1 + tempTempSumValue;
        }
        tempValue *= resourceMultiplier.Value();
        return tempValue;
    }
    //Queue
    public Multiplier availableQueue;
    public long CurrentAvailableQueue()
    {
        long tempAssignedQueue = 0;
        for (int i = 0; i < upgradeList.Count; i++)
        {
            tempAssignedQueue += upgradeList[i].queue + 10 * Convert.ToInt64(upgradeList[i].isSuperQueued);
        }
        return (long)availableQueue.Value() - tempAssignedQueue;
    }
    public void AssignQueue(UPGRADE upgrade, bool isSuperQueue = false)
    {
        if (!CanAssignQueue(upgrade, isSuperQueue)) return;
        upgrade.AssignQueue(Math.Min(TransactionCost.MultibuyNum(), CurrentAvailableQueue()), isSuperQueue);
    }
    public void RemoveQueue(UPGRADE upgrade)
    {
        if (!CanRemoveQueue(upgrade)) return;
        upgrade.RemoveQueue(Math.Min(TransactionCost.MultibuyNum(), upgrade.queue));
    }
    public void AdjustAssignedQueue()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            if (CurrentAvailableQueue() >= 0) return;
            if (CanRemoveQueue(upgradeList[i])) upgradeList[i].ResetQueue();
        }
    }
    public bool CanAssignQueue(UPGRADE upgrade, bool isSuperQueue)
    {
        if (isSuperQueue)
        {
            if (!game.epicStoreCtrl.Item(EpicStoreKind.SuperQueueUpgrade).IsPurchased()) return false;
            if (CurrentAvailableQueue() < 10) return false;
        }
        if (CurrentAvailableQueue() < 1) return false;
        return true;
    }
    public bool CanRemoveQueue(UPGRADE upgrade)
    {
        return upgrade.queue > 0 || upgrade.isSuperQueued;
    }
    public void BuyByQueue()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeList[i].BuyByQueue();
        }
    }
}

public class ResourceUpgrade : UPGRADE
{
    public override NUMBER resource { get => game.resourceCtrl.gold; }
    public override UpgradeKind kind => UpgradeKind.Resource;
    public override long queue { get => main.S.upgradeQueuesResource[id]; set => main.S.upgradeQueuesResource[id] = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueuesResource[id]; set => main.S.upgradeIsSuperQueuesResource[id] = value; }
    public override double Cost(long level)
    {
        return 20 * Math.Pow(5, id) * Math.Pow(1.10d + 0.125d * id, level / 2d);
    }
    public ResourceUpgrade(int id)
    {
        this.id = id;
    }
    public override void SetUnlockCondition()
    {
        switch (id)
        {
            case 0:
                break;
            case 1:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.UpgradeResource).isCleared);
                break;
            case 2:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade1).isCleared);
                break;
            case 3:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade2).isCleared);
                break;
            case 4:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade3).isCleared);
                break;
            case 5:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade4).isCleared);
                break;
            case 6:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade5).isCleared);
                break;
            case 7:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade6).isCleared);
                break;
            case 8:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade7).isCleared);
                break;
            case 9:
                unlockConditions.Add(() => game.questCtrl.Quest(QuestKindGlobal.Upgrade8).isCleared);
                break;

        }
    }
}

public class BasicStatsUpgrade : UPGRADE
{
    BasicStatsKind statsKind;
    public override long queue { get => main.S.upgradeQueuesBasicStats[(int)statsKind]; set => main.S.upgradeQueuesBasicStats[(int)statsKind] = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueuesBasicStats[(int)statsKind]; set => main.S.upgradeIsSuperQueuesBasicStats[(int)statsKind] = value; }
    public override NUMBER resource { get => game.resourceCtrl.gold; }
    NUMBER subResrouce { get => game.resourceCtrl.Resource(ResourceKind()); }
    public override UpgradeKind kind => UpgradeKind.BasicStats;
    public override double Cost(long level)
    {
        return 1000;// + 50 * (Math.Pow(level, 2) - 1);ここから検討
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        switch (statsKind)
        {
            case BasicStatsKind.HP:
                return tempLevel * 25 * game.upgradeCtrl.statsMultiplier.Value();
            case BasicStatsKind.MP:
                return tempLevel * 10 * game.upgradeCtrl.statsMultiplier.Value();
            case BasicStatsKind.ATK:
                return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
            case BasicStatsKind.MATK:
                return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
            case BasicStatsKind.DEF:
                return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
            case BasicStatsKind.MDEF:
                return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
        }
        return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
    }
    public BasicStatsUpgrade(BasicStatsKind statsKind)
    {
        this.statsKind = statsKind;
        id = (int)statsKind;
    }
    public override void Start()
    {
        base.Start();
        game.statsCtrl.SetEffect(statsKind, new MultiplierInfo(MultiplierKind.Upgrade, MultiplierType.Add, () => EffectValue()));
        //for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        //{
        //    game.statsCtrl.BasicStats((HeroKind)i, statsKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Upgrade, MultiplierType.Add, () => EffectValue()));
        //}

        //ver00020200以前はx/5d
        transaction.SetAnotherResource(subResrouce, (x) => 100 * Math.Pow(1000, x / 4d) * (1d - Convert.ToInt16(kind != UpgradeKind.SlimeBank) * game.upgradeCtrl.costReduction.Value()) * (1d - game.upgradeCtrl.costReductionFromWA.Value()));
    }
    public override ResourceKind ResourceKind()
    {
        switch (statsKind)
        {
            case BasicStatsKind.HP:
                return global::ResourceKind.Stone;
            case BasicStatsKind.MP:
                return global::ResourceKind.Crystal;
            case BasicStatsKind.ATK:
                return global::ResourceKind.Stone;
            case BasicStatsKind.MATK:
                return global::ResourceKind.Crystal;
            case BasicStatsKind.DEF:
                return global::ResourceKind.Leaf;
            case BasicStatsKind.MDEF:
                return global::ResourceKind.Leaf;
        }
        return global::ResourceKind.Stone;
    }
}

public class GoldGainUpgrade : UPGRADE
{
    public GoldGainUpgrade()
    {
    }
    public override NUMBER resource { get => game.resourceCtrl.gold; }
    public override UpgradeKind kind => UpgradeKind.GoldGain;
    public override long queue { get => main.S.upgradeQueuesGoldGain; set => main.S.upgradeQueuesGoldGain = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueuesGoldGain; set => main.S.upgradeIsSuperQueuesGoldGain = value; }
    public override double Cost(long level)
    {
        return 200 * Math.Pow(1.5d, level);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * game.upgradeCtrl.statsMultiplier.Value();
    }
}
public class ExpGainUpgrade : UPGRADE
{
    public ExpGainUpgrade()
    {
    }
    public override UpgradeKind kind => UpgradeKind.ExpGain;
    public override long queue { get => main.S.upgradeQueueExpGain; set => main.S.upgradeQueueExpGain = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueueExpGain; set => main.S.upgradeIsSuperQueueExpGain = value; }
    public override NUMBER resource { get => game.resourceCtrl.gold; }
    public override double Cost(long level)
    {
        return 250 + 250 * level;
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 5 * game.upgradeCtrl.statsMultiplier.Value();
    }
}
public class GoldCapUpgrade : UPGRADE
{
    public override UpgradeKind kind => UpgradeKind.GoldCap;
    public override long queue { get => main.S.upgradeQueuesGoldCap[(int)resourceKind]; set => main.S.upgradeQueuesGoldCap[(int)resourceKind] = value; }
    public override bool isSuperQueued { get => main.S.upgradeIsSuperQueuesGoldCap[(int)resourceKind]; set => main.S.upgradeIsSuperQueuesGoldCap[(int)resourceKind] = value; }
    ResourceKind resourceKind;
    public GoldCapUpgrade(ResourceKind resourceKind)
    {
        this.resourceKind = resourceKind;
        id = (int)resourceKind;
    }
    public override void Start()
    {
        base.Start();
        game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Upgrade, MultiplierType.Add, () => EffectValue()));
    }
    public override ResourceKind ResourceKind() { return resourceKind; }
    public override NUMBER resource { get => game.resourceCtrl.Resource(resourceKind); }
    public override double Cost(long level)
    {
        return 1000 * Math.Pow(2, level / 5d);
    }
    public override double EffectValue(bool isNextValue = false)
    {
        long tempLevel = level.value;
        if (isNextValue) tempLevel = transaction.ToLevel();
        return tempLevel * 50 * game.upgradeCtrl.goldcapMultiplier.Value() * game.upgradeCtrl.goldcapMultipliers[id].Value();
    }
}

public class UPGRADE
{
    public virtual void Start() { Set(); }
    public void Set()
    {
        unlock = new Unlock();
        level = new UpgradeLevel(kind, id);
        transaction = new Transaction(level, resource, (x) => Cost(x) * (1d - Convert.ToInt16(kind != UpgradeKind.SlimeBank) * game.upgradeCtrl.costReduction.Value()) * (1d - game.upgradeCtrl.costReductionFromWA.Value()));
        transaction.SetAdditionalBuyCondition(IsUnlocked);
        SetUnlockCondition();
    }
    public virtual UpgradeKind kind { get; }
    public int id;
    public UpgradeLevel level;
    public Transaction transaction;
    public virtual NUMBER resource { get; }
    public virtual double Cost(long level) { return level; }
    public virtual double EffectValue(bool isNextValue = false) { return 1; }
    public virtual ResourceKind ResourceKind() { return global::ResourceKind.Stone; }
    public List<Func<bool>> unlockConditions = new List<Func<bool>>();
    public Unlock unlock;
    public bool IsUnlocked()
    {
        for (int i = 0; i < unlockConditions.Count; i++)
        {
            if (!unlockConditions[i]()) return false;
        }
        if (!unlock.IsUnlocked()) return false;
        return true;
    }
    public virtual string Name() { return ""; }
    public virtual string Description() { return ""; }
    public virtual string EffectString(bool isNextValue = false) { return ""; }
    public virtual void SetUnlockCondition() { }

    //Queue
    public virtual long queue { get; set; }
    public virtual bool isSuperQueued { get; set; }
    public void BuyByQueue()
    {
        if (!isSuperQueued && queue <= 0) return;
        if (!transaction.CanBuy(true)) return;
        if (kind == UpgradeKind.SlimeBank && game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.SmartSlimeCoins) && !game.resourceCtrl.slimeCoin.IsMaxed()) return;
        transaction.Buy(true);
        if (!isSuperQueued) queue--;
    }
    public void AssignQueue(long num, bool isSuperQueue)
    {
        if (isSuperQueue)
        {
            isSuperQueued = true;
            queue = 0;
        }
        else if (!isSuperQueued)
            queue += num;
    }
    public void RemoveQueue(long num)
    {
        if (isSuperQueued) ResetQueue();
        else queue -= num;
        if (queue < 0) queue = 0;
    }
    public void ResetQueue()
    {
        queue = 0;
        isSuperQueued = false;
    }
}

public class UpgradeLevel : INTEGER
{
    public UpgradeLevel(UpgradeKind kind, int id)
    {
        this.kind = kind;
        this.id = id;
    }
    UpgradeKind kind;
    int id;
    public override long value
    {
        get
        {
            switch (kind)
            {
                case UpgradeKind.Resource:
                    return main.SR.upgradeLevelsResource[id];
                case UpgradeKind.BasicStats:
                    return main.SR.upgradeLevelsBasicStats[id];
                case UpgradeKind.GoldGain:
                    return main.SR.upgradeLevelsGoldGain;
                case UpgradeKind.ExpGain:
                    return main.SR.upgradeLevelsExpGain;
                case UpgradeKind.GoldCap:
                    return main.SR.upgradeLevelsGoldCap[id];
                case UpgradeKind.SlimeBank:
                    return main.SR.upgradeLevelsSlimebank[id];
                default:
                    return main.SR.upgradeLevelsResource[id];
            }
        }
        set
        {
            switch (kind)
            {
                case UpgradeKind.Resource:
                    main.SR.upgradeLevelsResource[id] = value;
                    break;
                case UpgradeKind.BasicStats:
                    main.SR.upgradeLevelsBasicStats[id] = value;
                    break;
                case UpgradeKind.GoldGain:
                    main.SR.upgradeLevelsGoldGain = value;
                    break;
                case UpgradeKind.ExpGain:
                    main.SR.upgradeLevelsExpGain = value;
                    break;
                case UpgradeKind.GoldCap:
                    main.SR.upgradeLevelsGoldCap[id] = value;
                    break;
                case UpgradeKind.SlimeBank:
                    main.SR.upgradeLevelsSlimebank[id] = value;
                    break;
                default:
                    main.SR.upgradeLevelsResource[id] = value;
                    break;
            }
        }
    }
}

public enum UpgradeKind
{
    Resource,
    BasicStats,
    GoldGain,
    ExpGain,
    GoldCap,
    SlimeBank,
}
