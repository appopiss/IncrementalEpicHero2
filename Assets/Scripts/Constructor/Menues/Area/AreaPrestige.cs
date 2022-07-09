using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;

public partial class SaveR
{
    public double[] areaPrestigePointsSlime;
    public double[] areaPrestigePointsMagicSlime;
    public double[] areaPrestigePointsSpider;
    public double[] areaPrestigePointsBat;
    public double[] areaPrestigePointsFairy;
    public double[] areaPrestigePointsFox;
    public double[] areaPrestigePointsDevilFish;
    public double[] areaPrestigePointsTreant;
    public double[] areaPrestigePointsFlameTiger;
    public double[] areaPrestigePointsUnicorn;

    public long[] areaPrestigeUpgradeLevelsSlime;
    public long[] areaPrestigeUpgradeLevelsMagicSlime;
    public long[] areaPrestigeUpgradeLevelsSpider;
    public long[] areaPrestigeUpgradeLevelsBat;
    public long[] areaPrestigeUpgradeLevelsFairy;
    public long[] areaPrestigeUpgradeLevelsFox;
    public long[] areaPrestigeUpgradeLevelsDevilFish;
    public long[] areaPrestigeUpgradeLevelsTreant;
    public long[] areaPrestigeUpgradeLevelsFlameTiger;
    public long[] areaPrestigeUpgradeLevelsUnicorn;
}

public class AreaPrestige
{
    public AreaPrestige(AREA area)
    {
        this.area = area;
        point = new AreaPrestigePoint(areaKind, id, isDungeon);

        if (isDungeon)
        {
            upgrades.Add(new APU_MaxAreaLevelUp(this));
            upgrades.Add(new APU_TreasureChest(this));
            upgrades.Add(new APU_LimitTime(this));
            upgrades.Add(new APU_MetalSlime(this));
            upgrades.Add(new APU_PortalOrb(this));
        }
        else
        {
            upgrades.Add(new APU_MaxAreaLevelUp(this));
            upgrades.Add(new APU_UnlockMission(this));
            upgrades.Add(new APU_ClearCount(this));
            upgrades.Add(new APU_DecreaseMaxWave(this));
            upgrades.Add(new APU_ExpBonus(this));
            upgrades.Add(new APU_MoveSpeedBonus(this));
        }

        for (int i = 0; i < upgrades.Count; i++)
        {
            upgrades[i].transaction.isOnBuyOneToggle = () => main.S.isToggleOn[(int)ToggleKind.BuyOneAreaPrestige1] || main.S.isToggleOn[(int)ToggleKind.BuyOneAreaPrestige2];
        }
    }
    public AREA area;
    public AreaKind areaKind { get => area.kind; }
    public int id { get => area.id; }
    public bool isDungeon { get => area.isDungeon; }
    public AreaPrestigePoint point;
    public List<AreaPrestigeUpgrade> upgrades = new List<AreaPrestigeUpgrade>();

    public void ResetUpgrade()
    {
        double tempPoint = point.value;
        for (int i = 0; i < upgrades.Count; i++)
        {
            tempPoint += upgrades[i].transaction.TotalCostConsumed();
            upgrades[i].level.ChangeValue(0);
        }
        point.ChangeValue(tempPoint);
        ResetAreaLevel();
    }
    public void ResetAreaLevel()
    {
        bool[] tempIsActive = new bool[game.battleCtrls.Length];
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            tempIsActive[i] = game.battleCtrls[i].isActiveBattle;
            if (tempIsActive[i] && game.battleCtrls[i].areaBattle.CurrentArea() == area)
                game.battleCtrls[i].isActiveBattle = false;
        }
        area.level.ChangeValue(0);
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            if (tempIsActive[i] && game.battleCtrls[i].areaBattle.CurrentArea() == area)
            {
                game.battleCtrls[i].areaBattle.Start();
                game.battleCtrls[i].isActiveBattle = true;
            }
        }
    }
}

//APU:AreaPrestigeUpgrade
public class APU_MaxAreaLevelUp : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.MaxAreaLevelUp;
    public APU_MaxAreaLevelUp(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => Math.Min((long)prestige.area.areaCtrl.maxAreaPrestigeLevel.Value(), AreaParameter.maxPrestigeLevel));
        transaction = new Transaction(level, point, () => 1, () => 2, false);
        prestige.area.maxAreaLevel.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level;
    }
}
public class APU_UnlockMission : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.UnlockMission;
    public APU_UnlockMission(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 3);
        transaction = new Transaction(level, point, () => 3, () => 3, true);
        prestige.area.missionUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level;
    }
}
public class APU_ClearCount : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.ClearCount;
    public APU_ClearCount(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 10);
        transaction = new Transaction(level, point, () => 5, () => 5, true);
        prestige.area.clearCountBonus.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level;
    }
}
public class APU_DecreaseMaxWave : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.DecreaseMaxWave;
    public APU_DecreaseMaxWave(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 20);
        transaction = new Transaction(level, point, () => 2, () => 2, true);
        prestige.area.decreaseMaxWaveNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level * 5;
    }
}
public class APU_ExpBonus : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.ExpBonus;
    public APU_ExpBonus(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => (long)prestige.area.areaCtrl.maxAreaExpLevel.Value());
        transaction = new Transaction(level, point, () => 1, () => 0, true);
        prestige.area.areaCtrl.expBonuses[(int)prestige.areaKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return 0.025 * level;
    }
}
public class APU_MoveSpeedBonus : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.MoveSpeedBonus;
    public APU_MoveSpeedBonus(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => (long)prestige.area.areaCtrl.maxAreaMoveSpeedLevel.Value());
        transaction = new Transaction(level, point, () => 1, () => 0, true);
        prestige.area.areaCtrl.moveSpeedBonuses[(int)prestige.areaKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return 0.001 * level;
    }
}
public class APU_TreasureChest : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.TreasureChest;
    public APU_TreasureChest(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 10);
        transaction = new Transaction(level, point, () => 1, () => 1, true);
        prestige.area.chestDropChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return 0.005 + 0.001 * level;
    }
}
public class APU_LimitTime : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.LimitTime;
    public APU_LimitTime(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 20);
        transaction = new Transaction(level, point, () => 1, () => 0, true);
        prestige.area.addLimitTime.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level * 60;
    }
}
public class APU_MetalSlime : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.MetalSlime;
    public APU_MetalSlime(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 20);
        transaction = new Transaction(level, point, () => 1, () => 1, true);
        prestige.area.metalSlimeChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return 0.0005d + level * 0.00025;
    }
}
public class APU_PortalOrb : AreaPrestigeUpgrade
{
    public override AreaPrestigeUpgradeKind kind => AreaPrestigeUpgradeKind.PortalOrb;
    public APU_PortalOrb(AreaPrestige prestige) : base(prestige)
    {
        level = new AreaPrestigeUpgradeLevel(this, () => 8);
        transaction = new Transaction(level, point, () => 3, () => 3, true);
        prestige.area.portalOrbReduceFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Add, () => effectValue));
    }
    public override double EffectValue(long level)
    {
        return level;
    }
}


public enum AreaPrestigeUpgradeKind //セーブの都合上、最大20個まで
{
    MaxAreaLevelUp,
    UnlockMission,
    ClearCount,
    DecreaseMaxWave,
    ExpBonus,
    MoveSpeedBonus,
    TreasureChest,
    LimitTime,
    MetalSlime,
    PortalOrb,
}


public class AreaPrestigeUpgrade
{
    public AreaPrestigeUpgrade(AreaPrestige prestige)
    {
        this.prestige = prestige;
    }
    public virtual double EffectValue(long level) { return 0; }
    public double effectValue { get => EffectValue(level.value); }
    public double nextEffectValue { get => EffectValue(transaction.ToLevel()); }
    public Transaction transaction;
    public AreaPrestige prestige;
    public AreaPrestigePoint point { get => prestige.point; }
    public virtual AreaPrestigeUpgradeKind kind { get; }
    public AreaPrestigeUpgradeLevel level;
}
public class AreaPrestigeUpgradeLevel : INTEGER
{
    public AreaPrestigeUpgradeLevel(AreaPrestigeUpgrade upgrade, Func<long> maxLevel)
    {
        this.upgrade = upgrade;
        maxValue = maxLevel;
    }
    public AreaPrestigeUpgrade upgrade;
    public AreaPrestigeUpgradeKind kind { get => upgrade.kind; }
    public AreaKind areaKind { get => upgrade.prestige.areaKind; }
    public bool isDungeon { get => upgrade.prestige.area.isDungeon; }
    public int id { get => upgrade.prestige.id; }
    public int saveId { get => id + (int)kind * AreaParameter.firstAreaIdForSave + AreaParameter.firstDungeonIdForSave * Convert.ToInt32(isDungeon); }
    public override long value { get => SaveArray()[saveId]; set => SaveArray()[saveId] = value; }
    long[] SaveArray()
    {
        switch (areaKind)
        {
            case AreaKind.SlimeVillage:
                return main.SR.areaPrestigeUpgradeLevelsSlime;
            case AreaKind.MagicSlimeCity:
                return main.SR.areaPrestigeUpgradeLevelsMagicSlime;
            case AreaKind.SpiderMaze:
                return main.SR.areaPrestigeUpgradeLevelsSpider;
            case AreaKind.BatCave:
                return main.SR.areaPrestigeUpgradeLevelsBat;
            case AreaKind.FairyGarden:
                return main.SR.areaPrestigeUpgradeLevelsFairy;
            case AreaKind.FoxShrine:
                return main.SR.areaPrestigeUpgradeLevelsFox;
            case AreaKind.DevilFishLake:
                return main.SR.areaPrestigeUpgradeLevelsDevilFish;
            case AreaKind.TreantDarkForest:
                return main.SR.areaPrestigeUpgradeLevelsTreant;
            case AreaKind.FlameTigerVolcano:
                return main.SR.areaPrestigeUpgradeLevelsFlameTiger;
            case AreaKind.UnicornIsland:
                return main.SR.areaPrestigeUpgradeLevelsUnicorn;
            default:
                return main.SR.areaPrestigeUpgradeLevelsSlime;
        }
    }
} 

public class AreaPrestigePoint : NUMBER
{
    public AreaPrestigePoint(AreaKind areaKind, int id, bool isDungeon)
    {
        this.areaKind = areaKind;
        this.id = id;
        this.isDungeon = isDungeon;
    }
    public AreaKind areaKind;
    public int id;
    public bool isDungeon;
    int saveId => id + Convert.ToInt32(isDungeon) * AreaParameter.firstAreaIdForSave;
    public override double value { get => SaveArray()[saveId]; set => SaveArray()[saveId] = value; }
    double[] SaveArray()
    {
        switch (areaKind)
        {
            case AreaKind.SlimeVillage:
                return main.SR.areaPrestigePointsSlime;
            case AreaKind.MagicSlimeCity:
                return main.SR.areaPrestigePointsMagicSlime;
            case AreaKind.SpiderMaze:
                return main.SR.areaPrestigePointsSpider;
            case AreaKind.BatCave:
                return main.SR.areaPrestigePointsBat;
            case AreaKind.FairyGarden:
                return main.SR.areaPrestigePointsFairy;
            case AreaKind.FoxShrine:
                return main.SR.areaPrestigePointsFox;
            case AreaKind.DevilFishLake:
                return main.SR.areaPrestigePointsDevilFish;
            case AreaKind.TreantDarkForest:
                return main.SR.areaPrestigePointsTreant;
            case AreaKind.FlameTigerVolcano:
                return main.SR.areaPrestigePointsFlameTiger;
            case AreaKind.UnicornIsland:
                return main.SR.areaPrestigePointsUnicorn;
            default:
                return main.SR.areaPrestigePointsSlime;
        }
    }
}