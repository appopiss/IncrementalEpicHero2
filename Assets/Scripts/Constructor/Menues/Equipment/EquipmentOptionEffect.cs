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
    public EquipmentEffectKind[] equipment1stOptionEffectKinds;//[equipmentId]
    public EquipmentEffectKind[] equipment2ndOptionEffectKinds;
    public EquipmentEffectKind[] equipment3rdOptionEffectKinds;
    public EquipmentEffectKind[] equipment4thOptionEffectKinds;
    public EquipmentEffectKind[] equipment5thOptionEffectKinds;
    public EquipmentEffectKind[] equipment6thOptionEffectKinds;

    public long[] equipment1stOptionLevels;//[equipmentId]
    public long[] equipment2ndOptionLevels;
    public long[] equipment3rdOptionLevels;
    public long[] equipment4thOptionLevels;
    public long[] equipment5thOptionLevels;
    public long[] equipment6thOptionLevels;

    public double[] equipment1stOptionEffectValues;
    public double[] equipment2ndOptionEffectValues;
    public double[] equipment3rdOptionEffectValues;
    public double[] equipment4thOptionEffectValues;
    public double[] equipment5thOptionEffectValues;
    public double[] equipment6thOptionEffectValues;
}

public class EquipmentOptionEffect
{
    public EquipmentOptionEffect(int id, int optionId)
    {
        this.id = id;
        this.optionId = optionId;
        level = new EquipmentOptionLevel(id, optionId, () => EquipmentParameter.MaxLevel(kind));
    }
    public void SetInfo(int id, int optionId)
    {
        this.id = id;
        this.optionId = optionId;
        level.SetInfo(id, optionId, () => EquipmentParameter.MaxLevel(kind));
    }

    public int createId;//DeleteしたらEffectの登録も消す（無効にする）必要があるため、Deleteしたら-1にする
    public void CreateNew(long monsterLevel, int createId)
    {
        this.createId = createId;
        List<double> optionChance = new List<double>();
        List<EquipmentEffectKind> optionKind = new List<EquipmentEffectKind>();
        for (int i = 0; i < Enum.GetNames(typeof(EquipmentEffectKind)).Length; i++)
        {
            int count = i;
            if (monsterLevel >= EquipmentParameter.RequiredLevelIncrement((EquipmentEffectKind)count, 1))
            {
                optionChance.Add(10d / EquipmentParameter.RarityFactor((EquipmentEffectKind)count));
                optionKind.Add((EquipmentEffectKind)count);
            }
        }
        optionLottery = new Lottery(optionChance.ToArray());
        kind = optionKind[optionLottery.SelectedId()];
        level = new EquipmentOptionLevel(id, optionId, () => EquipmentParameter.MaxLevel(kind));
        LotteryLevel(monsterLevel);
        LotteryEffectValue();
    }
    public void Enchant(EquipmentEffectKind effectKind, long level, int createId)
    {
        this.createId = createId;
        kind = effectKind;
        this.level = new EquipmentOptionLevel(id, optionId, () => EquipmentParameter.MaxLevel(kind));
        this.level.ChangeValue(level);
        LotteryEffectValue();
    }
    public bool CanEnchant()
    {
        return kind == EquipmentEffectKind.Nothing;
    }
    public void Delete()//Enchant Slotに戻すという意味と同義
    {
        kind = EquipmentEffectKind.Nothing;
        level.ChangeValue(0);
        effectValue = 0;
        createId = -1;
    }

    public Lottery optionLottery;
    public int id;
    public int optionId;
    public EquipmentOptionLevel level;
    public EquipmentEffectKind kind
    {
        get
        {
            switch (optionId)
            {
                case 0:
                    return main.SR.equipment1stOptionEffectKinds[id];
                case 1:
                    return main.SR.equipment2ndOptionEffectKinds[id];
                case 2:
                    return main.SR.equipment3rdOptionEffectKinds[id];
                case 3:
                    return main.SR.equipment4thOptionEffectKinds[id];
                case 4:
                    return main.SR.equipment5thOptionEffectKinds[id];
                case 5:
                    return main.SR.equipment6thOptionEffectKinds[id];
            };
            return 0;
        }
        set
        {
            switch (optionId)
            {
                case 0:
                    main.SR.equipment1stOptionEffectKinds[id] = value;
                    break;
                case 1:
                    main.SR.equipment2ndOptionEffectKinds[id] = value;
                    break;
                case 2:
                    main.SR.equipment3rdOptionEffectKinds[id] = value;
                    break;
                case 3:
                    main.SR.equipment4thOptionEffectKinds[id] = value;
                    break;
                case 4:
                    main.SR.equipment5thOptionEffectKinds[id] = value;
                    break;
                case 5:
                    main.SR.equipment6thOptionEffectKinds[id] = value;
                    break;
            };
        }
    }

    public double effectValue
    {
        get
        {
            switch (optionId)
            {
                case 0:
                    return main.SR.equipment1stOptionEffectValues[id];
                case 1:
                    return main.SR.equipment2ndOptionEffectValues[id];
                case 2:
                    return main.SR.equipment3rdOptionEffectValues[id];
                case 3:
                    return main.SR.equipment4thOptionEffectValues[id];
                case 4:
                    return main.SR.equipment5thOptionEffectValues[id];
                case 5:
                    return main.SR.equipment6thOptionEffectValues[id];
            };
            return 0;
        }
        set
        {
            switch (optionId)
            {
                case 0:
                    main.SR.equipment1stOptionEffectValues[id] = value;
                    break;
                case 1:
                    main.SR.equipment2ndOptionEffectValues[id] = value;
                    break;
                case 2:
                    main.SR.equipment3rdOptionEffectValues[id] = value;
                    break;
                case 3:
                    main.SR.equipment4thOptionEffectValues[id] = value;
                    break;
                case 4:
                    main.SR.equipment5thOptionEffectValues[id] = value;
                    break;
                case 5:
                    main.SR.equipment6thOptionEffectValues[id] = value;
                    break;
            };
        }
    }
    public void LotteryLevel(long monsterLevel)
    {
        int tempMaxLevel = 1;
        for (int i = 1; i < level.maxValue(); i++)
        {
            if (monsterLevel >= EquipmentParameter.RequiredLevelIncrement(kind, i))
                tempMaxLevel = i;
        }
        level.ChangeValue(UnityEngine.Random.Range(1, tempMaxLevel + 1));
    }
    public void LotteryEffectValue()
    {
        effectValue = UnityEngine.Random.Range((float)EquipmentParameter.EffectCalculation(kind, level.value), (float)EquipmentParameter.EffectCalculation(kind, level.value + 1));
    }
    public long RequiredLevelIncrement()
    {
        return EquipmentParameter.RequiredLevelIncrement(kind, level.value);
    }
}

public class EquipmentOptionLevel : INTEGER
{
    public EquipmentOptionLevel(int id, int optionId, Func<long> maxLevel)
    {
        this.maxValue = maxLevel;
        this.id = id;
        this.optionId = optionId;
    }
    public void SetInfo(int id, int optionId, Func<long> maxLevel)
    {
        this.maxValue = maxLevel;
        this.id = id;
        this.optionId = optionId;
    }

    int id;
    int optionId;
    public override long value
    {
        get
        {
            switch (optionId)
            {
                case 0:
                    return main.SR.equipment1stOptionLevels[id];
                case 1:
                    return main.SR.equipment2ndOptionLevels[id];
                case 2:
                    return main.SR.equipment3rdOptionLevels[id];
                case 3:
                    return main.SR.equipment4thOptionLevels[id];
                case 4:
                    return main.SR.equipment5thOptionLevels[id];
                case 5:
                    return main.SR.equipment6thOptionLevels[id];
            };
            return 0;
        }
        set
        {
            switch (optionId)
            {
                case 0:
                    main.SR.equipment1stOptionLevels[id] = value;
                    break;
                case 1:
                    main.SR.equipment2ndOptionLevels[id] = value;
                    break;
                case 2:
                    main.SR.equipment3rdOptionLevels[id] = value;
                    break;
                case 3:
                    main.SR.equipment4thOptionLevels[id] = value;
                    break;
                case 4:
                    main.SR.equipment5thOptionLevels[id] = value;
                    break;
                case 5:
                    main.SR.equipment6thOptionLevels[id] = value;
                    break;
            };
        }
    }
}


public enum EquipmentEffectKind
{
    Nothing,
    HPAdder,
    MPAdder,
    ATKAdder,
    MATKAdder,
    DEFAdder,
    MDEFAdder,
    SPDAdder,
    HPMultiplier,
    MPMultiplier,
    ATKMultiplier,
    MATKMultiplier,
    DEFMultiplier,
    MDEFMultiplier,
    ATKPropotion,
    MATKPropotion,
    DEFPropotion,
    MDEFPropotion,
    FireResistance,
    IceResistance,
    ThunderResistance,
    LightResistance,
    DarkResistance,
    PhysicalAbsorption,
    FireAbsorption,
    IceAbsorption,
    ThunderAbsorption,
    LightAbsorption,
    DarkAbsorption,
    DebuffResistance,
    EquipmentDropChance,
    SlimeDropChance,
    MagicSlimeDropChance,
    SpiderDropChance,
    BatDropChance,
    FairyDropChance,
    FoxDropChance,
    DevilFishDropChance,
    TreantDropChance,
    FlameTigerDropChance,
    UnicornDropChance,
    ColorMaterialDropChance,
    PhysicalCritical,
    MagicalCritical,
    EXPGain,
    SkillProficiency,
    EquipmentProficiency,
    //MoveSpeed
    MoveSpeedMultiplier,
    GoldGain,
    StoneGain,
    CrystalGain,
    LeafGain,
    WarriorSkillLevel,
    WizardSkillLevel,
    AngelSkillLevel,
    ThiefSkillLevel,
    ArcherSkillLevel,
    TamerSkillLevel,
    AllSkillLevel,
    SlimeKnowledge,
    MagicSlimeKnowledge,
    SpiderKnowledge,
    BatKnowledge,
    FairyKnowledge,
    FoxKnowledge,
    DevilFishKnowledge,
    TreantKnowledge,
    FlameTigerKnowledge,
    UnicornKnowledge,
    PhysicalDamage,
    FireDamage,
    IceDamage,
    ThunderDamage,
    LightDamage,
    DarkDamage,
    //ver0.1.2.11以降追加
    HpRegen,
    MpRegen,
    TamingPoint,
    WarriorSkillRange,
    WizardSkillRange,
    AngelSkillRange,
    ThiefSkillRange,
    ArcherSkillRange,
    TamerSkillRange,
    TownMatGain,
    TownMatAreaClearGain,
    //TownMatDungeonRewardGain,
    RebirthPointGain1,
    RebirthPointGain2,
    RebirthPointGain3,
    CriticalDamage,
    BlessingEffect,
    //ver0.3.1.0で追加
    MoveSpeedAdder,
}
