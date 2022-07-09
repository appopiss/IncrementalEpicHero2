using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillParameter
{
    public static readonly long maxSkillRank = 50;//いずれ100まで行く想定
    public static readonly int maxSkillSlotNum = 8;
    public static readonly int maxGlobalSkillSlotNum = 8;
    public static readonly double[][][] skillCosts = new double[][][]
    {
        //Warrior
        new double[][]
        {
            //SwordAttack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //Slash
            new double[] {50, Math.Pow(1000, 1/10d * 1.5) },
            //DoubleSlash
            new double[] {5000, Math.Pow(1000, 1/10d * 1.75) },
            //SonicSlash
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //SwingDown
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //SwingAround
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //ChargeSwing
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //FanSwing
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
            //ShieldAttack
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //KnockingShot
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
        },
        //Wizard
        new double[][]
        {
            //StaffAttack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //FireBolt
            new double[] {50, Math.Pow(1000, 1/10d * 1.5d)},
            //FireStorm
            new double[] {5000, Math.Pow(1000, 1/10d * 1.75d)},
            //MeteoStrike
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //IceBolt
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //ChillingTouch
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //Blizzard
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //ThunderBolt
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //Double Thunder Bolt
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //Lightning Thunder
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
        },
        //Angel
        new double[][]
        {
            //WingAttack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //WingShoot
            new double[] {50, Math.Pow(1000, 1/10d)},
            //Heal
            new double[] {5000, Math.Pow(1000, 1/10d * 2)},
            //GodBless
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //MuscleInflation
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //MagicImpact
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //ProtectWall
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //Haste
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //WingStorm
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //HolyArch
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
        },
        //Thief
        new double[][]//Dark:近接攻撃
        {
            //Dagger Attack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //Stab（素早い・近距離・一発〜二発）
            new double[] {50, Math.Pow(1000, 1/10d * 1.5) },
            //Knife Toss（素早い・遠距離・2発〜4発）
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //Lucky Blow（近距離・装備中Critical率UP）
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //Spread Toss（遠距離・全範囲）
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //Shadow Strike (Dark, Poison, 小範囲, ImpactPoison)
            new double[] {5000, Math.Pow(1000, 1/10d * 1.75) },
            //SneekyStrike（Dark, 装備中Critical率UP）
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //Pilfer（Dark, Material獲得）
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //Dark Wield (Dark・小〜中範囲・knockback)
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //Assassination (Dark, Death, ImpactBlood, 単体ダメージ倍率大)
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
        },
        //Archer
        new double[][]
        {
            //ArrowAttack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //PiercingArrow
            new double[] {50, Math.Pow(1000, 1/10d * 1.5d)},
            //BurstArrow
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //Multishot
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
            //ShockArrow
            new double[] {5000, Math.Pow(1000, 1/10d * 1.75d)},
            //FrozenArrow
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //ExplodingArrow
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //ShiningArrow
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //GravityArrow
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //Kiting
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
        },
        //Tamer
        new double[][]
        {
            //SonnetAttack : initCost, baseCost
            new double[] {10, Math.Pow(1000, 1/10d)},
            //AttackingOrder
            new double[] {50, Math.Pow(1000, 1/10d * 1.5d)},
            //RushOrder
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //DefensiveOrder
            new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            //SoothingBallad
            new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            //OdeOfFriendship
            new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            //AnthemOfEnthusiasm
            new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            //FeedChilli
            new double[] {1e17, Math.Pow(1000, 1/10d * 7)},
            //BreedingKnowledge
            new double[] {5000, Math.Pow(1000, 1/10d * 1.75d)},
            //TuneOfTotalTaming
            new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
        },
    };
    public static readonly double[][][] skillFactors = new double[][][]
    {
        //Warrior
        new double[][]
        {
            //SwordAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,Interval,ProfDiff
            new double[] {1, 0.02, 1, 0.025, 0, 0, 1.50, 0, 80, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //Slash
            new double[] {2, 0.05, 1, 0.050, 0, 0, 1.25, 0, 80, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //DoubleSlash
            new double[] {6.5, 0.125, 0, 0, 10, 1, 1.25, 1, 80, 50, 0, 50, 0, 0, 0, 2, 0, 2},
            //SonicSlash
            new double[] {35, 0.100, 0, 0, 25, 2.5d, 1.00, 2, 80, 120, 0, 120, 0, 0, 0, 4, 1/25d, 8},
            //SwingDown
            new double[] {550, 1.50, 10, 0.05, 100, 5.0, 2.75, 2, 120, 80, 0, 80, 0.5d, 0.0025, 0.90d, 1, 0, 1},
            //SwingAround
            new double[] {500, 0.75, 0, 0, 240, 5.0, 3.00, 3, 150, 150, 0, 300, 0, 0, 0, 1, 0, 1},
            //ChargeSwing
            new double[] {2000, 4.50, 0, 0, 500, 7.5, 4.00, 4, 120, 80, 0, 80, 0.5d, 0.0025, 0.90d, 1, 0, 1},
            //FanSwing
            new double[] {600, 1.00, 0, 0, 600, 10.0, 2.00, 5, 300, 300, 2, 500, 0, 0, 0, 2, 1/50d, 4},
            //ShieldAttack
            new double[] {20, 0.05, 10, 0.05, 0, 0, 1.5, 1, 500, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //KnockingAttack
            new double[] {60, 0.20, 30, 0.25, 20, 1, 1.5, 2, 80, 80, 0, 80, 0.90d, 0.0001, 0.95d, 1, 0, 1},
        },
        //Wizard
        new double[][]
        {
            //StaffAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,Interval,ProfDiff
            new double[] {1, 0.02, 1, 0.025, 0, 0, 1.50, 0, 100, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //FireBolt
            new double[] {4, 0.10, 3, 0.05, 0, 0, 2.00, 0, 120, 80, 0, 80, 0, 0, 0, 1, 0, 1},
            //FireStorm
            new double[] {6, 0.10, 0, 0, 30, 2, 2.50, 1, 120, 120, 0, 200, 0, 0, 0, 1, 1/100d, 3},
            //MeteorStrike
            new double[] {900, 0.75, 0, 0, 360, 10.0, 3.00, 3, 200, 150, 1, 300, 0, 0, 0, 1, 1/100d, 3},
            //IceBolt
            new double[] {30, 0.125, 10, 0.10, 0, 0, 1.5, 2, 150, 80, 0, 80, 0.10d, 0.0025, 0.90d, 1, 0, 1},
            //ChillingTouch
            new double[] {70, 0.200, 10, 0.10, 20, 1, 1.5, 2, 100, 50, 0, 50, 0.10d, 0.001d, 0.50d, 1, 0, 1},
            //Blizard
            new double[] {1000, 1.00, 0, 0, 500, 12.5, 2.50, 4, 1000, 250, 0, 500, 0.25d, 0.0025d, 1.0d, 1, 0, 1},
            //ThunderBolt
            new double[] {10, 0.025, 25, 0.50, 0, 0, 1.00, 1, 200, 50, 0, 50, 0.20, 0.002, 0.50, 1, 0, 1},
            //DoubleThunderBolt
            new double[] {30, 0.050, 10, 1.00, 30, 2, 1.00, 2, 200, 50, 0, 50, 0.50, 0.002, 1.0, 2, 0, 2},
            //LightningThunder
            new double[] {2000, 1.50, 0, 0, 1250, 50.0, 3.50, 5, 200, 100, 0, 100, 0.50, 0.001, 1.0, 1, 1/50d, 4},
        },
        //Angel
        new double[][]
        {
            //WingAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,Interval,ProfDiff
            new double[] {2, 0.0050, 1, 0.025, 0, 0, 0.75, 0, 150, 50, 0, 50, 0, 0, 0, 1, 1/50d, 5},
            //WingShoot
            new double[] {2, 0.0050, 1, 0.025, 0, 0, 0.75, 0, 150, 50, 0, 50, 0, 0, 0, 1, 1/50d, 5},
            //Heal
            new double[] {15, 0.075, 0, 0, 30, 1.5, 3.5, 1, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //GodBless
            new double[] {0.25, 0.0005, 0, 0, 10, 2.5, 1.0, 1, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //MuscleInflation
            new double[] {0.25, 0.0005, 0, 0, 10, 10, 1.0, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //MagicImpact
            new double[] {0.25, 0.0005, 0, 0, 10, 10, 1.0, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //ProtectWall
            new double[] {0.25, 0.0005, 0, 0, 20, 20, 1.0, 3, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //Haste
            new double[] {0.25, 0.0001, 0, 0, 250, 25, 1.0, 4, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //WingStorm
            new double[] {250, 0.20, 0, 0, 240, 2.0, 2.5, 2, 200, 200, 0, 400, 0.80d, 0.001d, 0.95d, 1, 1/50d, 5},
            //HolyArch
            new double[] {10, 0.01, 0, 0, 500, 50, 1.0, 5, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
        },
        //Thief
        new double[][]
        {
            //DaggerAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,Interval,ProfDiff
            new double[] {1, 0.01, 1, 0.025, 0, 0, 1.00, 0, 80, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //Stab
            new double[] {2, 0.0075, 2, 0.05, 0, 0, 0.75, 0, 80, 50, 0, 50, 0.05d, 0.0025d, 0.90d, 1, 1/50d, 3},
            //KnifeToss
            new double[] {35, 0.125, 0, 0, 25, 1.5, 1.00, 2, 200, 50, 0, 50, 0, 00, 00, 2, 1/100d, 4},
            //LuckyBlow
            new double[] {100, 0.200, 0, 0, 35, 2.0d, 1.25, 2, 80, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //SpreadToss
            new double[] {150, 0.25, 0, 0, 250, 3.75, 2.00, 4, 200, 50, 0, 50, 0, 0, 0, 2, 1/50d, 4},
            //ShadowStrike
            new double[] {4, 0.05, 0, 0, 20, 2, 2.00, 1, 80, 120, 0, 120, 0.50d, 0.005d, 0.90d, 1, 0, 1},
            //SneakyStrike
            new double[] {20, 0.10, 5, 0.10, 30, 2, 1.25, 1, 500, 50, 0, 50, 0.20d, 0.0025d, 0.90d, 1, 1/50d, 4},
            //Pilfer
            new double[] {10, 0.10, 0, 0, 10, 0.5, 2.00, 2, 80, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //DarkWield
            new double[] {300, 0.25, 0, 0, 150, 3.0, 2.00, 3, 150, 150, 0, 300, 0.25d, 0.0025d, 0.90d, 1, 0, 1},
            //Assassination
            new double[] {2500, 2.50, 0, 0, 400, 5.0, 2.50, 5, 80, 50, 0, 50, 0.05d, 0.001d, 0.25d, 1, 0, 1},
        },
        //Archer
        new double[][]
        {
            //ArrowAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,Interval,ProfDiff
            new double[] {1, 0.02, 1, 0.025, 0, 0, 1.50, 0, 300, 80, 0, 80, 0, 0, 0, 1, 0, 1},
            //PiercingArrow
            new double[] {2, 0.015, 2, 0.05, 0, 0, 1.25, 0, 300, 100, 0, 100, 0, 0, 0, 1, 1/50d, 5},
            //BurstArrow
            new double[] {45, 0.050, 0, 0, 35, 2.5d, 1.50, 2, 300, 100, 0, 100, 0, 0, 0, 5, 1/50d, 10},
            //Multishot
            new double[] {1250, 2.50, 0, 0, 500, 10.0, 2.00, 5, 500, 100, 0, 100, 0, 0, 0, 1, 0, 1},
            //ShockArrow
            new double[] {6, 0.05, 0, 0, 20, 2, 1.75, 1, 300, 100, 0.5d, 250, 0.25d, 0.0025d, 0.90d, 1, 1/100d, 3},
            //FrozenArrow
            new double[] {20, 0.100, 5, 0.10, 10, 1, 1.00, 1, 500, 150, 0, 150, 0.25d, 0.0025d, 0.90d, 1, 0, 1},
            //ExplodingArrow
            new double[] {85, 0.250, 0, 0, 50, 4.0, 2.25, 2, 300, 100, 0.5d, 250, 0.25d, 0.0025d, 0.90d, 1, 1/100d, 3},
            //ShiningArrow
            new double[] {80, 0.15, 0, 0, 200, 5.0, 2.50, 3, 500, 100, 0, 100, 0.25d, 0.0025d, 0.90d, 8, 0, 8},
            //GravityArrow
            new double[] {40, 0.10, 0, 0, 400, 30.0, 5.00, 4, 500, 500, 0, 500, 0.25d, 0.0025d, 0.90d, 4, 1/50d, 8},
            //Kiting
            new double[] {-0.35d, 0.00001, 0, 0, 200, 20, 1.0, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
        },
        //Tamer
        new double[][]
        {
            //SonnetAttack Dmg,Dmg+,MpGain,MpGain+,MpCons,MpCons+,
            new double[] {1, 0.01, 1, 1, 0, 0, 1.0, 0, 300, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //AttackingOrder//ATK,MATKの%
            new double[] {2, 0.015, 2, 0.05, 0, 0, 1.0, 0, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //RushOrder
            new double[] {40, 0.20, 30, 0.25, 20, 2, 1.5, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //DefensiveOrder
            new double[] {150, 1.00, 0, 0, 200, 5.0, 2.0, 3, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //SoothingBallad
            new double[] {20, 0.10, 0, 0, 30, 2.5, 3.5, 1, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //OdeOfFriendship
            new double[] {0.25, 0.001, 0, 0, 250, 25, 5.0, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //AnthemOfEnthusiasm
            new double[] {1.00, 0.01, 0, 0, 250, 25, 1.0, 2, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //FeedChilli
            new double[] {1.50, 0.00025, 0, 0, 500, 50, 1.0, 4, 1000, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            //BreedingKnowledge
            new double[] {4, 0.05, 0, 0, 20, 2, 2.00, 1, 300, 50, 0, 50, 0, 0, 0, 1, 0, 1},
            //TuneOfTotalTaming
            new double[] {200, 2.00, 0, 0, 500, 5, 5.00, 5, 300, 50, 0, 50, 0, 0, 0, 1, 0, 1},
        },
            //        //SonnetAttack : initCost, baseCost
            //new double[] {10, Math.Pow(1000, 1/10d)},
            ////AttackingOrder
            //new double[] {50, Math.Pow(1000, 1/10d * 1.5d)},
            ////RushOrder
            //new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            ////DefensiveOrder
            //new double[] {1e10, Math.Pow(1000, 1/10d * 5)},
            ////SoothingBallad
            //new double[] {1e5, Math.Pow(1000, 1/10d * 2)},
            ////OdeOfFriendship
            //new double[] {1e7, Math.Pow(1000, 1/10d * 3)},
            ////AnthemOfEnthusiasm
            //new double[] {1e8, Math.Pow(1000, 1/10d * 3)},
            ////FeedChilli
            //new double[] {1e25, Math.Pow(1000, 1/10d * 10)},
            ////BreedingKnowledge
            //new double[] {5000, Math.Pow(1000, 1/10d * 1.75d)},
            ////TuneOfTotalTaming
            //new double[] {1e17, Math.Pow(1000, 1/10d * 7)},

    };
}

public enum SkillKindWarrior
{
    SwordAttack,
    Slash,
    DoubleSlash,
    SonicSlash,
    SwingDown,
    SwingAround,
    ChargeSwing,
    FanSwing,
    ShieldAttack,
    KnockingShot,
}
public enum SkillKindWizard
{
    StaffAttack,
    FireBolt,
    FireStorm,
    MeteorStrike,
    IceBolt,
    ChillingTouch,
    Blizzard,
    ThunderBolt,
    DoubleThunderBolt,
    LightningThunder,
}
public enum SkillKindAngel
{
    WingAttack,
    WingShoot,
    Heal,
    GodBless,
    MuscleInflation,
    MagicImpact,
    ProtectWall,
    Haste,
    WingStorm,
    HolyArch,
}
public enum SkillKindThief
{
    DaggerAttack,
    Stab,
    KnifeToss,
    LuckyBlow,
    SpreadToss,
    ShadowStrike,
    SneakyStrike,
    Pilfer,
    DarkWield,
    Assassination,
}
public enum SkillKindArcher
{
    ArrowAttak,
    PiercingArrow,
    BurstArrow,
    Multishot,
    ShockArrow,
    FrozenArrow,
    ExplodingArrow,
    ShiningArrow,
    GravityArrow,
    Kiting,
}
public enum SkillKindTamer
{
    SonnetAttack,
    AttackingOrder,
    RushOrder,
    DefensiveOrder,
    SoothingBallad,
    OdeOfFriendship,
    AnthemOfEnthusiasm,
    FeedChilli,
    BreedingKnowledge,
    TuneOfTotalTaming,
}
public enum SkillType
{
    Attack,
    Buff,
    Heal,
    Order,
}
public enum Element
{
    Physical,
    Fire,
    Ice,
    Thunder,
    Light,
    Dark,
}
public enum Buff
{
    Nothing,
    HpUp,
    AtkUp,
    MatkUp,
    DefMDefUp,
    SpdUp,
    MoveSpeedUp,
    GoldUp,
    SkillLevelUp,
}
public enum Debuff
{
    Nothing,
    AtkDown,
    MatkDown,
    DefDown,
    MdefDown,
    SpdDown,
    Stop,
    Electric,
    Poison,
    Death,
    Knockback,
    FireResDown,
    IceResDown,
    ThunderResDown,
    LightResDown,
    DarkResDown,
    Gravity,
}
public enum SkillEffectCenter
{
    Target,
    Myself,
    Field,
}
