using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Element;

public class Parameter
{
    //Random
    public static int randomAccuracy = 100000;
    //Multibuy
    public static long[] multibuyNums = new long[] { 1, 5, 10, 25, 50, 100, 250, 1000, 2500, 10000, 100000, 1000000 };
    public static readonly int maxSkillNum = 20;
    public static readonly long maxHeroLevel = 1000;
    //combatRange
    public static float[] combatRanges = new float[] { 80, 100, 120, 150, 200, 300 };
    //DailyQuest
    public static double[] dailyQuestRarityChance0 = new double[] { 0.60d, 0.20d, 0.15d, 0.04d, 0.01d };
    public static double[] dailyQuestRarityChance1 = new double[] { 0.00d, 0.45d, 0.35d, 0.15d, 0.05d };
    public static double[] dailyQuestRarityChance2 = new double[] { 0.00d, 0.00d, 0.60d, 0.30d, 0.10d };
    public static long[] dailyQuestRewardEC = new long[] { 100, 150, 200, 300, 500 };
    public static double[] dailyQuestRewardPortalOrb = new double[] { 3, 5, 7, 10, 15 };
    public static double[] dailyQuestDefeatNum = new double[] { 500, 750, 1200, 2000, 5000 };
    public static double[] dailyQuestCaptureNum = new double[] { 10, 20, 35, 50, 125 };
    public static double[] dailyQuestAreaClearNum = new double[] { 50, 100, 200, 400, 800 };
    //Battle Field
    public static readonly float moveRangeX = 800f;
    public static readonly float moveRangeY = 800f;
    public static readonly int battleFieldSquareNum = 9;
    public static readonly Vector2 heroInitPosition = Vector2.down * 400f;
    public static readonly Vector2 hidePosition = Vector2.down * 900f;
    public static Vector2[] heroAllyPositions = new Vector2[]
    {
        new Vector2(-300f,-300f),
        new Vector2(300f,-300f),
        new Vector2(300f, 300f),
        new Vector2(-300f, 300f),
        new Vector2(0, 400f),
    };
    public static Vector2[] petPositions = new Vector2[]
    {
        new Vector2(   0f,-300f),
        new Vector2(-250f,-400f),
        new Vector2( 250f,-400f),
        new Vector2(-100f,-300f),
        new Vector2( 100f,-300f),
        new Vector2(-200f,-300f),
        new Vector2( 200f,-300f),
        new Vector2(-300f,-300f),
        new Vector2( 300f,-300f),
        new Vector2(   0f,-200f),
    };
    public static readonly int maxMonsterSpawnNum = 30;//最大30体まで出現可能
    public static readonly int maxPetSpawnNum = 10;
    //Log
    public static readonly float defaultLogShowTimesec = 3.0f;

    //Stats:HP,MP,ATK,MATK,DEF,MDEF,SPD,PhyCrit,MagCrit,ResourceD->CritDamage,EQD,MoveS
    //初期値
    public static readonly double[][] baseStats = new double[][]
    {
        new double[] {20,  5.0, 2.0, 0.5, 0.0, 0.0, 0, 0.0100d, 0.0000d, 2.0000d, 0.00100d, 200},//Warrior
        new double[] {10, 10.0, 0.5, 2.0, 0.0, 0.0, 0, 0.0000d, 0.0100d, 2.0000d, 0.00100d, 150},//Wizard
        new double[] {15,  7.5, 1.5, 1.5, 0.0, 0.0, 0, 0.0050d, 0.0050d, 2.0000d, 0.00100d, 250},//Angel
        new double[] {10,  5.0, 1.0, 1.0, 0.0, 0.0, 0, 0.0500d, 0.0500d, 2.0000d, 0.00150d, 250},//Thief
        new double[] {10, 10.0, 1.5, 1.5, 0.0, 0.0, 0, 0.0050d, 0.0050d, 2.0000d, 0.00100d, 200},//Archer
        new double[] {20, 10.0, 1.0, 1.0, 0.0, 0.0, 0, 0.0050d, 0.0050d, 2.0000d, 0.00100d, 150},//Tamer
    };
    //増分
    public static readonly double[][] stats = new double[6][]
    {
        new double[] { 20, 05.0, 1.00, 0.25, 0.50, 0.50, 1.0, 0.0001d, 0.0000d, 0.0000d, 0.00001d, 1.000 },//0.200},//Warrior
        new double[] { 10, 10.0, 0.25, 1.00, 0.15, 0.15, 0.5, 0.0000d, 0.0001d, 0.0000d, 0.00001d, 0.750 },//0.150},//Wizard
        new double[] { 15, 07.5, 0.50, 0.50, 0.35, 0.35, 1.5, 0.00005, 0.00005, 0.0000d, 0.00001d, 1.250 },//0.250},//Angel
        new double[] { 10, 05.0, 0.35, 0.35, 0.05, 0.05, 2.5, 0.00020, 0.00020, 0.0000d, 0.00001d, 1.250 },//0.250},//Thief
        new double[] { 10, 10.0, 0.50, 0.50, 0.35, 0.35, 1.0, 0.00010, 0.00010, 0.0000d, 0.00001d, 1.000 },//0.200},//Archer
        new double[] { 20, 10.0, 0.25, 0.25, 0.25, 0.25, 1.5, 0.00005, 0.00005, 0.0000d, 0.00001d, 0.750 },//0.150},//Tamer
        //new double[] { 20, 05, 1.00, 0.25, 0.75, 0.75, 1.0, 0.0001d, 0.0000d, 0.0000d, 0.00001d, 0.200},//Warrior
        //new double[] { 10, 10, 0.25, 1.00, 0.25, 0.25, 0.5, 0.0000d, 0.0001d, 0.0000d, 0.00001d, 0.150},//Wizard
        //new double[] { 15, 05, 0.50, 0.50, 0.50, 0.50, 1.5, 0.00005, 0.00005, 0.0000d, 0.00001d, 0.250},//Angel
        //new double[] { 10, 05, 0.35, 0.35, 0.10, 0.10, 2.5, 0.00020, 0.00020, 0.0000d, 0.00001d, 0.250},//Thief
        //new double[] { 10, 10, 0.50, 0.50, 0.50, 0.50, 1.0, 0.00010, 0.00010, 0.0000d, 0.00001d, 0.200},//Archer
        //new double[] { 20, 05, 0.50, 0.50, 1.00, 1.00, 0.5, 0.00005, 0.00005, 0.0000d, 0.00001d, 0.150},//Tamer
    };
    public static double RequiredExp(long level)
    {
        level++;//最初0スタートにしたため
        return Math.Floor(
        100d * level
        + 50d * Math.Pow(level / 2d, 2)
        + 100d * Math.Pow(level / 5d, 3)
        + 150d * Math.Pow(level / 10d, 4)
        + 200d * Math.Pow(level / 15d, 5)
        + 1000d * Math.Pow(level / 30d, 6)
        + 2000d * Math.Pow(level / 50d, 8)
        + 100000d * Math.Pow(level / 100d, 12)
        + 10000000d * Math.Pow(level / 200d, 20)
        + 1000000000d * Math.Pow(level / 300d, 35)
        );
    }

    //Upgrade
    public static readonly int resourceUpgradeTier = 10;

    //Drop
    public static Vector2[] randomVec = new Vector2[]
    {
        Vector2.zero,
        Vector2.right, Vector2.down, Vector2.left, Vector2.up,
        Vector2.one, Vector2.right+Vector2.down, Vector2.left+Vector2.down, Vector2.left+Vector2.up
    };
    public static Vector2 RandomVec()
    {
        return randomVec[UnityEngine.Random.Range(0, randomVec.Length - 1)];
    }

    //Title
    public static (double main, double sub) TitleEffectValue(TitleKind kind, long level)
    {
        switch (kind)
        {
            case TitleKind.MonsterDistinguisher://20,40,80,160,320,640,1280(Max7)
                return (Math.Min(1, level) * 10 * Math.Pow(2, level), 10 * level);
            case TitleKind.EquipmentSlotWeapon:
                return (level, 0);
            case TitleKind.EquipmentSlotArmor:
                return (level, 0);
            case TitleKind.EquipmentSlotJewelry:
                return (level, 0);
            case TitleKind.PotionSlot:
                return (level, 0);
            case TitleKind.EquipmentProficiency:
                return (level * 0.20, 0);
            case TitleKind.SkillMaster:
                return (level, level * 0.1);
            case TitleKind.Survival:
                return (0.10d * Math.Min(1, level) + 0.10d * level, 0);
            case TitleKind.FireResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.IceResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.ThunderResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.LightResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.DarkResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.DebuffResistance:
                return (Math.Ceiling(Math.Pow(level, 1.5d)) * 0.025d, level * 0.01);
            case TitleKind.MoveSpeed:
                return (level * 0.05, 0);
            case TitleKind.Alchemist:
                return (level * 0.025, 0);
            case TitleKind.MetalHunter:
                return (level, 0.25 * level);
            case TitleKind.BreakingTheLimit:
                return (level, 0);
            case TitleKind.PhysicalDamage:
                return (level * 0.025, 0);
            case TitleKind.FireDamage:
                return (level * 0.025, 0);
            case TitleKind.IceDamage:
                return (level * 0.025, 0);
            case TitleKind.ThunderDamage:
                return (level * 0.025, 0);
            case TitleKind.LightDamage:
                return (level * 0.025, 0);
            case TitleKind.DarkDamage:
                return (level * 0.025, 0);
            case TitleKind.Cooperation:
                return (Math.Min(cooperationEfficiency[Math.Min(level, 6)], 1d), 0);
            case TitleKind.Quester:
                return (level, 0);
        }
        return (0, 0);
    }

    static double[] cooperationEfficiency = new double[] { 0.00d, 0.20d, 0.40d, 0.55d, 0.70d, 0.80d, 0.90d };
}

public class RebirthParameter
{
    public static long[] tierHeroLevel = new long[] { 100, 200, 300, 500, 750, 1000 };
    public static (double initCost, double baseCost, bool isLinear, long maxLevel, double effectValue) Upgrade(RebirthUpgradeKind kind, long level)
    {
        switch (kind)
        {
            case RebirthUpgradeKind.ExpGain:
                return (50, 50, true, 100, 0.25d * level);//元は100,100,0.50
            case RebirthUpgradeKind.EQRequirement:
                return (75, 75, true, 100, 5 * level);
            case RebirthUpgradeKind.QuestAcceptableNum:
                return (250, Math.Pow(10, 1 / 5d), false, 20, level);
            case RebirthUpgradeKind.BasicAtk:
                return (200, 200, true, 50, 0.05d * level);
            case RebirthUpgradeKind.BasicMAtk:
                return (200, 200, true, 50, 0.05d * level);
            case RebirthUpgradeKind.BasicHp:
                return (200, 200, true, 50, 0.25d * level);//1でした
            case RebirthUpgradeKind.BasicDef:
                return (200, 200, true, 50, 0.05d * level);
            case RebirthUpgradeKind.BasicMDef:
                return (200, 200, true, 50, 0.05d * level);
            case RebirthUpgradeKind.BasicMp:
                return (200, 200, true, 50, 0.10d * level);//0.5でした
            case RebirthUpgradeKind.StoneGain:
                return (100, 100, true, 100, level);
            case RebirthUpgradeKind.CrystalGain:
                return (100, 100, true, 100, level);
            case RebirthUpgradeKind.LeafGain:
                return (100, 100, true, 100, level);
            case RebirthUpgradeKind.StoneGoldCap:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.CrystalGoldCap:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.LeafGoldCap:
                return (100, 100, true, 100, 0.05d * level);
            //Tier2
            case RebirthUpgradeKind.SkillProfGain:
                return (100, 100, true, 100, 0.25d * level);//元は100,100,0.50
            case RebirthUpgradeKind.SkillRankCostReduction:
                return (100, 100, true, 100, Math.Pow(0.75d, level));//Math.Pow(0.75d, level));//8回購入して10%になる
            case RebirthUpgradeKind.ClassSkillSlot:
                return (10000, 100, false, 1, level);
            case RebirthUpgradeKind.ShareSkillPassive:
                return (2500, 2500, true, 10, level * 0.10d);
            case RebirthUpgradeKind.T1ExpGainBoost:
                return (250, 250, true, 100, 0.10d * level);
            case RebirthUpgradeKind.T1RebirthPointGainBoost:
                return (500, 500, true, 10, 0.10d * level);
            case RebirthUpgradeKind.T1BasicAtkBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1BasicMAtkBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1BasicHpBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1BasicDefBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1BasicMDefBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1BasicMpBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T1StoneGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T1CrystalGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T1LeafGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T1StoneGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.T1CrystalGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.T1LeafGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);
            //Tier3
            case RebirthUpgradeKind.EQProfGain:
                return (100, 100, true, 100, 0.25d * level);//元は100,100,0.50
            case RebirthUpgradeKind.EQLevelCap:
                return (5000, 5000, true, 2, 5 * level);
            case RebirthUpgradeKind.EQWeaponSlot:
                return (250, 5, false, 5, level);
            case RebirthUpgradeKind.EQArmorSlot:
                return (250, 5, false, 5, level);
            case RebirthUpgradeKind.EQJewelrySlot:
                return (250, 5, false, 5, level);
            case RebirthUpgradeKind.T2ExpGainBoost:
                return (250, 250, true, 100, 0.10d * level);
            case RebirthUpgradeKind.T2SkillProfGainBoost:
                return (250, 250, true, 100, 0.10d * level);
            case RebirthUpgradeKind.T2RebirthPointGainBoost:
                return (500, 500, true, 10, 0.10d * level);
            case RebirthUpgradeKind.T2BasicAtkBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2BasicMAtkBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2BasicHpBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2BasicDefBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2BasicMDefBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2BasicMpBoost:
                return (200, 200, true, 50, 0.10d * level);
            case RebirthUpgradeKind.T2StoneGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T2CrystalGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T2LeafGainBoost:
                return (100, 100, true, 100, 0.20d * level);
            case RebirthUpgradeKind.T2StoneGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.T2CrystalGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);
            case RebirthUpgradeKind.T2LeafGoldCapBoost:
                return (100, 100, true, 100, 0.05d * level);

            //case RebirthUpgradeKind.ExpGain2:
            //    return (50, 50, true, 20, 0.2d * level);
            //case RebirthUpgradeKind.SkillProfGain2:
            //    return (100, 50, true, 20, 0.2d * level);
            //case RebirthUpgradeKind.Rebirth1PointGain:
            //    return (150, 150, true, 20, 0.1d * level);
            //case RebirthUpgradeKind.SkillSlot:
            //    return (10000, 10, false, 1, level);
            //case RebirthUpgradeKind.PhysCritChance:
            //    return (150, 2, false, 5, 0.20d * level);
            //case RebirthUpgradeKind.MagCritChance:
            //    return (150, 2, false, 5, 0.20d * level);
            //case RebirthUpgradeKind.GoldGain:
            //    return (150, 2, false, 5, 0.20d * level);
            //case RebirthUpgradeKind.MaterialGain:
            //    return (150, 2, false, 5, level);
            //case RebirthUpgradeKind.MysteriousWaterGain:
            //    return (150, 2, false, 5, 0.20d * level);
            //case RebirthUpgradeKind.Pet:
            //    return (150, 2, false, 5, 0.20d * level);
            //case RebirthUpgradeKind.FireRes:
            //    return (50, 50, true, 10, 0.10d * level);
            //case RebirthUpgradeKind.IceRes:
            //    return (50, 50, true, 10, 0.10d * level);
            //case RebirthUpgradeKind.ThunderRes:
            //    return (50, 50, true, 10, 0.10d * level);
            //case RebirthUpgradeKind.LightRes:
            //    return (50, 50, true, 10, 0.10d * level);
            //case RebirthUpgradeKind.DarkRes:
            //    return (50, 50, true, 10, 0.10d * level);
            //case RebirthUpgradeKind.DebuffRes:
            //    return (50, 50, true, 10, 0.10d * level);
            ////Tier3
            //case RebirthUpgradeKind.Rebirth2PointGain:
            //    break;
            ////case RebirthUpgradeKind.BackgroundActive:
            ////    return (50, 50, true, 1, level);
            //case RebirthUpgradeKind.SkillLevelCap:
            //    break;
            //case RebirthUpgradeKind.EQProf:
            //    break;
            //case RebirthUpgradeKind.ExpGain3:
            //    break;
            //case RebirthUpgradeKind.SkillProfGain3:
            //    break;
            default:
                break;
        }
        return (10000, 100, false, 1, 0);
    }
}