using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Element;
using static PetActiveEffectKind;
using static PetPassiveEffectKind;
using static TownMaterialKind;
using System;

public class MonsterParameter
{
    public static double dropChanceBase = 0.01d;
    public static double colorDropChanceBase = 0.001d;
    public static MaterialKind Material(MonsterSpecies species)
    {
        switch (species)
        {
            case MonsterSpecies.Slime:
                return MaterialKind.OilOfSlime;
            case MonsterSpecies.MagicSlime:
                return MaterialKind.EnchantedCloth;
            case MonsterSpecies.Spider:
                return MaterialKind.SpiderSilk;
            case MonsterSpecies.Bat:
                return MaterialKind.BatWing;
            case MonsterSpecies.Fairy:
                return MaterialKind.FairyDust;
            case MonsterSpecies.Fox:
                return MaterialKind.FoxTail;
            case MonsterSpecies.DevifFish:
                return MaterialKind.FishScales;
            case MonsterSpecies.Treant:
                return MaterialKind.CarvedBranch;
            case MonsterSpecies.FlameTiger:
                return MaterialKind.ThickFur;
            case MonsterSpecies.Unicorn:
                return MaterialKind.UnicornHorn;
            case MonsterSpecies.Mimic:
                return MaterialKind.BlackPearl;
            case MonsterSpecies.ChallengeBoss:
                return MaterialKind.MonsterFluid;
        }
        return MaterialKind.OilOfSlime;
    }
    public static MaterialKind Material(MonsterColor color)
    {
        switch (color)
        {
            case MonsterColor.Normal:
                return MaterialKind.MonsterFluid;
            case MonsterColor.Blue:
                return MaterialKind.FrostShard;
            case MonsterColor.Yellow:
                return MaterialKind.LightningShard;
            case MonsterColor.Red:
                return MaterialKind.FlameShard;
            case MonsterColor.Green:
                return MaterialKind.NatureShard;
            case MonsterColor.Purple:
                return MaterialKind.PoisonShard;
            case MonsterColor.Boss:
                return MaterialKind.BlackPearl;
            case MonsterColor.Metal:
                return MaterialKind.BlackPearl;
        }
        return MaterialKind.MonsterFluid;
    }
    //[species]ごとのTownMat
    public static TownMaterialKind[] townMatBricks = new TownMaterialKind[] { MudBrick, MudBrick, LimestoneBrick, LimestoneBrick, MarbleBrick, MarbleBrick, GraniteBrick, GraniteBrick, BasaltBrick, BasaltBrick };
    public static TownMaterialKind[] townMatLogs = new TownMaterialKind[] { PineLog, PineLog, MapleLog, MapleLog, AshLog, AshLog, MahoganyLog, MahoganyLog, RosewoodLog, RosewoodLog };
    public static TownMaterialKind[] townMatShards = new TownMaterialKind[] { JasperShard, JasperShard, OpalShard, OpalShard, OnyxShard, OnyxShard, JadeShard, JadeShard, SapphireShard, SapphireShard };

    public static double ColorFactor(MonsterColor color)
    {
        switch (color)
        {
            case MonsterColor.Normal:
                return 1;
            case MonsterColor.Blue:
                return 1.25d;
            case MonsterColor.Yellow:
                return 1.25d;
            case MonsterColor.Red:
                return 1.5d;
            case MonsterColor.Green:
                return 2.5d;
            case MonsterColor.Purple:
                return 3.0d;
            case MonsterColor.Boss:
                return 10d;
            case MonsterColor.Metal:
                return 500d;
        }
        return 1;
    }

    public static double SpeciesFactor(MonsterSpecies species)
    {
        switch (species)
        {
            case MonsterSpecies.Slime:
                return 1d;
            case MonsterSpecies.MagicSlime:
                return 1.15d;
            case MonsterSpecies.Spider:
                return 0.80d;
            case MonsterSpecies.Bat:
                return 1.25d;
            case MonsterSpecies.Fairy:
                return 1.35d;
            case MonsterSpecies.Fox:
                return 1.50d;
            case MonsterSpecies.DevifFish:
                return 1.75d;
            case MonsterSpecies.Treant:
                return 2.0d;
            case MonsterSpecies.FlameTiger:
                return 2.25d;
            case MonsterSpecies.Unicorn:
                return 2.50d;
            case MonsterSpecies.Mimic:
                return 10d;
            case MonsterSpecies.ChallengeBoss:
                return 5d;
        }
        return 1;
    }
    //MonsterStats:HP,MP,ATK,MATK,DEF,MDEF,SPD,FIRE,ICE,TUNDER,LIGHT,DARK
    public static readonly double[][][] monsterStats = new double[][][]//[Species][Color][stats]
    {
        new double[][]//Slime
        {
            new double[] { 10,  0, 0.50, 0, 0, 0, 5, -0.50, -0.50, -0.50, -0.50, -0.50 },//Normal
            new double[] { 20,  0, 0.75, 0, 0, 0, 5, -0.50, -0.50, -0.50, -0.50, -0.50 },//Blue
            new double[] { 10,  0, 0.50, 0, 0, 0, 7.5, -0.50, -0.50, -0.50, -0.50, -0.50 },//Yellow
            new double[] { 20,  0, 1.25, 0, 0, 0, 2.5,  -0.50, -0.50, -0.50, -0.50, -0.50 },//Red
            new double[] { 30,  0, 0.75, 0, 5, 0, 10,  -0.50, -0.50, -0.50, -0.50, -0.50 },//Green
            new double[] { 50,  0, 2.50, 0, 5, 0, 2.5,  -0.50, -0.50, -0.50, -0.50, -0.50 },//Purple
            new double[] { 200, 0, 2.50, 0, 20, 0, 5,  -0.20, -0.20, -0.20, -0.20, -0.20 },//Boss
            new double[] { 1, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//MagicSlime
        {
            new double[] { 10, 0, 0, 0.5, 0, 0, 5, -0.50, -0.50, -0.50, -0.50, -0.50 },//Normal
            new double[] { 20, 0, 0, 0.75, 0, 0, 5, -0.50, 0.50, 0.00, 0.00, 0.00 },//Blue
            new double[] { 10, 0, 0, 0.5, 0, 0, 7.5, 0.00, -0.50, 0.50, 0.00, 0.00 },//Yellow
            new double[] { 20, 0, 0, 1.25, 0, 0, 2.5,  0.50, 0.00, -0.50, 0.00, 0.00 },//Red
            new double[] { 30, 0, 0, 0.75, 0, 5, 10, 0.00, 0.00, 0.00, 0.50, -0.50 },//Green
            new double[] { 50, 0, 0, 2.5, 0, 5, 2.5, 0.00, 0.00, 0.00, -0.50, 0.50 },//Purple
            new double[] { 200, 0, 0, 2.5, 0, 20, 5, 0.00, 0.00, 0.00, 0.00, 0.00 },//Boss
            new double[] { 2, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Spider : HP低いけどATK大でたくさん出てくる
        {
            new double[] { 7.5, 0, 1.0,   0, 2.5,   0,  5.0, -0.20, 0.00, 0.00, -0.50, 0.50 },//Normal
            new double[] { 15,  0, 0,   1.0, 0,   2.5,  5.0, -0.60, 0.60, 0.00, -0.50, 0.50 },//Blue
            new double[] { 10,  0, 0.75,  0, 2.5,   0,  7.5, -0.20, 0.00, 0.00, -0.50, 0.50 },//Yellow
            new double[] { 15,  0, 0,  1.50, 0,   2.5,  2.5, 0.40, -0.60, 0.00, -0.50, 0.50 },//Red
            new double[] { 20,  0, 1.0,   0, 5,     0,  10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },//Green
            new double[] { 30,  0, 0,   3.0, 0,     5,  2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },//Purple
            new double[] { 150, 0, 3.75,  0, 20,   20,  5.0,  0.50, 0.50, 0.50, -0.50, 0.50 },//Boss
            new double[] { 3,   0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Bat : ATK低いがSPD高い、全て物理
        {
            new double[] { 7.500, 0, 0.50, 0.0, 2.50, 0, 20.0, -0.20, -0.20, -0.20, -0.50, 0.0 },//Normal
            new double[] { 15.00, 0, 0.50, 0.0, 2.50, 0, 20.0, -0.20,  0.50, -0.20, -0.50, 0.0 },//Blue
            new double[] { 10.00, 0, 0.40, 0.0, 2.50, 0, 30.0, -0.20, -0.20,  0.50, -0.50, 0.0 },//Yellow
            new double[] { 15.00, 0, 1.00, 0.0, 2.50, 0, 10.00,  0.50, -0.20, -0.20, -0.50, 0.0 },//Red
            new double[] { 20.00, 0, 0.60, 0.0, 5.00, 0, 40.00, -0.20, -0.20, -0.20, -0.20, 0.0 },//Green
            new double[] { 30.00, 0, 1.50, 0.0, 5.00, 0, 10.00, -0.20, -0.20, -0.20, -0.50, 0.50 },//Purple
            new double[] { 150.0, 0, 2.00, 0.0, 20.0, 0, 40.00, 0, 0, 0, -0.50, 0 },//Boss
            new double[] { 4, 0, 0.10, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Fairy : ATK低いがSPD高い、全て魔法
        {
            new double[] { 7.500, 0, 0.0, 0.50, 0, 2.50, 20.00,  0.00,  0.00,  0.00, 0.50, -0.50 },//Normal
            new double[] { 15.00, 0, 0.0, 0.50, 0, 2.50, 20.00,  0.00,  0.50, -0.20, 0.50, -0.50 },//Blue
            new double[] { 10.00, 0, 0.0, 0.40, 0, 2.50, 30.00, -0.20,  0.00,  0.50, 0.50, -0.50 },//Yellow
            new double[] { 15.00, 0, 0.0, 1.00, 0, 2.50, 10.00,  0.50, -0.20,  0.00, 0.50, -0.50 },//Red
            new double[] { 20.00, 0, 0.0, 0.60, 0, 5.00, 40.00,  0.00,  0.00,  0.00, 0.90, -0.50 },//Green
            new double[] { 30.00, 0, 0.0, 1.50, 0, 5.00, 10.00,  0.00,  0.00,  0.00, 0.50, -0.20 },//Purple
            new double[] { 150.0, 0, 0.0, 2.00, 0, 20.0, 40.00,  0.20,  0.20,  0.20, 0.90, -0.20 },//Boss
            new double[] { 5, 0, 0.1, 0.10, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Fox : HP普通、ATK/MATK大、DEF小、Debuffいっぱい！
        {
            new double[] { 10,  0, 0, 1.00, 1.00, 1.00, 5,    -0.20, 0.10, 0.80, 0.50, -0.25 },//Normal
            new double[] { 20,  0, 1.50, 0, 1.00, 1.00, 5,    -0.20, 0.10, 0.25, 0.50, -0.25 },//Blue
            new double[] { 10,  0, 0, 1.00, 1.00, 1.00, 7.5,  -0.20, 0.10, 0.80, 0.50, -0.25 },//Yellow
            new double[] { 20,  0, 2.50, 0, 1.00, 1.00, 2.5,  -0.20, 0.10, 0.50, 0.50, -0.25 },//Red
            new double[] { 30,  0, 0, 1.50, 2.00, 2.00, 10,   -0.20, 0.10, 0.50, 0.90, -0.25 },//Green
            new double[] { 50,  0, 5.00, 0, 2.00, 2.00, 2.5,  -0.20, 0.10, 0.50, 0.50, -0.25 },//Purple
            new double[] { 200, 0, 0, 5.00, 5.00, 5.00, 5.0,   0.00, 0.50, 0.90, 0.90, -0.10 },//Boss
            new double[] { 6, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//DevilFish : HPとても高い、DEF/MDEF高い、SPD小
        {
            new double[] { 25,  0, 0.50, 0, 10.00, 10.00, 2.5, 0.50, 0.20, -0.50, -0.25, 0.80 },//Normal
            new double[] { 50,  0, 0, 0.75, 10.00, 10.00, 2.5, 0.50, 0.50, -0.50, -0.25, 0.80 },//Blue
            new double[] { 25,  0, 0.50, 0, 10.00, 10.00, 3.5, 0.50, 0.20, -0.50, -0.25, 0.80 },//Yellow
            new double[] { 50,  0, 1.25, 0, 10.00, 10.00, 1.5, 0.50, 0.20, -0.50, -0.25, 0.80 },//Red
            new double[] { 75,  0, 0.75, 0, 25.00, 25.00, 3.5, 0.50, 0.20, -0.50, -0.25, 0.80 },//Green
            new double[] { 125, 0, 0, 2.50, 25.00, 25.00, 1.5, 0.50, 0.20, -0.50, -0.25, 0.90 },//Purple
            new double[] { 500, 0, 2.50, 0, 50.00, 50.00, 2.5, 0.90, 0.50, -0.20,  0.00, 0.90 },//Boss
            new double[] { 7, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        //Fox
        //new Element[]{Thunder, Physical, Thunder, Physical, Element.Light, Physical, Thunder, Physical},
        ////Devil Fish
        //new Element[]{Physical, Ice, Physical, Physical, Physical, Dark, Physical, Physical},

        new double[][]//Treant
        {
            new double[] { 7.5, 0, 0.5, 2.5, 0, 0, 5.0, -0.20, 0.00, 0.00, -0.50, 0.50 },//Normal
            new double[] { 15, 0, 0, 1.0, 0, 2.5, 5.0, -0.60, 0.60, 0.00, -0.50, 0.50 },//Blue
            new double[] { 10, 0, 0.75, 0, 2.5, 0, 7.5, -0.20, 0.00, 0.00, -0.50, 0.50 },//Yellow
            new double[] { 15, 0, 0, 1.50, 0, 2.5, 2.5, 0.40, -0.60, 0.00, -0.50, 0.50 },//Red
            new double[] { 20, 0, 1.0, 0, 5, 0, 10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },//Green
            new double[] { 30, 0, 0, 3.0, 0, 5, 2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },//Purple
            new double[] { 150, 0, 3.75, 0, 20, 20, 5.0,  0.00, 0.00, 0.00, 0.00, 0.00 },//Boss
            new double[] { 8, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//FlameTiger
        {
            new double[] { 7.5, 0, 0.5, 2.5, 0, 0, 5.0, -0.20, 0.00, 0.00, -0.50, 0.50 },//Normal
            new double[] { 15, 0, 0, 1.0, 0, 2.5, 5.0, -0.60, 0.60, 0.00, -0.50, 0.50 },//Blue
            new double[] { 10, 0, 0.75, 0, 2.5, 0, 7.5, -0.20, 0.00, 0.00, -0.50, 0.50 },//Yellow
            new double[] { 15, 0, 0, 1.50, 0, 2.5, 2.5, 0.40, -0.60, 0.00, -0.50, 0.50 },//Red
            new double[] { 20, 0, 1.0, 0, 5, 0, 10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },//Green
            new double[] { 30, 0, 0, 3.0, 0, 5, 2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },//Purple
            new double[] { 150, 0, 3.75, 0, 20, 20, 5.0,  0.00, 0.00, 0.00, 0.00, 0.00 },//Boss
            new double[] { 0.3, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Unicorn
        {
            new double[] { 7.5, 0, 0.5, 2.5, 0, 0, 5.0, -0.20, 0.00, 0.00, -0.50, 0.50 },//Normal
            new double[] { 15, 0, 0, 1.0, 0, 2.5, 5.0, -0.60, 0.60, 0.00, -0.50, 0.50 },//Blue
            new double[] { 10, 0, 0.75, 0, 2.5, 0, 7.5, -0.20, 0.00, 0.00, -0.50, 0.50 },//Yellow
            new double[] { 15, 0, 0, 1.50, 0, 2.5, 2.5, 0.40, -0.60, 0.00, -0.50, 0.50 },//Red
            new double[] { 20, 0, 1.0, 0, 5, 0, 10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },//Green
            new double[] { 30, 0, 0, 3.0, 0, 5, 2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },//Purple
            new double[] { 150, 0, 3.75, 0, 20, 20, 5.0,  0.00, 0.00, 0.00, 0.00, 0.00 },//Boss
            new double[] { 0.3, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Mimic
        {
            new double[] { 200, 0, 1.0, 0, 10, 10, 5, 0.00, 0.00, 0.00, 0.00, 0.00 },//Normal
            //new double[] { 15, 0, 0, 1.0, 0, 2.5, 5.0, -0.60, 0.60, 0.00, -0.50, 0.50 },//Blue
            //new double[] { 10, 0, 0.75, 0, 2.5, 0, 7.5, -0.20, 0.00, 0.00, -0.50, 0.50 },//Yellow
            //new double[] { 15, 0, 0, 1.50, 0, 2.5, 2.5, 0.40, -0.60, 0.00, -0.50, 0.50 },//Red
            //new double[] { 20, 0, 1.0, 0, 5, 0, 10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },//Green
            //new double[] { 30, 0, 0, 3.0, 0, 5, 2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },//Purple
            //new double[] { 150, 0, 3.75, 0, 20, 20, 5.0,  0.00, 0.00, 0.00, 0.00, 0.00 },//Boss
            //new double[] { 0.3, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        },
        new double[][]//Challenge : HP,MP,ATK,MATK,DEF,MDEF,SPD,FIRE,ICE,TUNDER,LIGHT,DARK
        {
            //SlimeKing
            new double[] { 1000, 0, 2.5, 0, 10, 10, 10/4d, 0.00, 0.00, 0.00, 0.00, 0.00 },
            //SpiderQueen
            new double[] { 850, 0, 0, 5, 100, 10, 10/4d,  0.50, 0.50, 0.50, -0.50, 0.50 },
            //Golem
            new double[] { 2000, 0, 5, 5, 200, 200, 10/5d,  0.00, 0.00, 0.00, 0.00, 0.00 },
            //FairyQueen
            new double[] { 20, 0, 1.0, 0, 5, 0, 10.0,  -0.20, 0.00, 0.00, -0.50, 0.50 },
            //Ninetale
            new double[] { 30, 0, 0, 3.0, 0, 5, 2.5,  -0.20, 0.00, 0.00, -1.00, 0.90 },
            //Octobaddie
            new double[] { 150, 0, 3.75, 0, 20, 20, 5.0,  0.00, 0.00, 0.00, 0.00, 0.00 },
            //new double[] { 0.3, 0, 0.1, 0.1, 1e300d, 1e300d, 10, 0.90, 0.90, 0.90, 0.90, 0.90 },//Metal
        }
    };
    //MonsterAttackElement
    public static readonly Element[][] monsterAttackElements = new Element[][]//[Species][Color]
    {
        //Slime
        new Element[]{Physical, Physical, Physical, Physical, Physical, Physical, Physical, Physical},
        //Magic Slime
        new Element[]{Ice, Ice, Thunder, Fire, Element.Light, Dark, Fire, Physical},
        //Spider
        new Element[]{Physical, Ice, Physical, Fire, Physical, Dark, Physical, Physical},
        //Bat
        new Element[]{Physical, Physical, Physical, Physical, Physical, Physical, Physical, Physical},
        //Fairy
        new Element[]{Fire, Ice, Thunder, Fire, Element.Light, Dark, Dark, Physical},
        //Fox
        new Element[]{Thunder, Physical, Thunder, Physical, Element.Light, Physical, Thunder, Physical},
        //Devil Fish
        new Element[]{Physical, Ice, Physical, Fire, Physical, Dark, Physical, Physical},
        //Treant
        new Element[]{Physical, Ice, Physical, Fire, Physical, Dark, Physical, Physical},
        //Flame Tiger
        new Element[]{Fire, Fire, Fire, Fire, Fire, Fire, Fire, Physical},
        //Unicorn
        new Element[]{Physical, Ice, Physical, Fire, Physical, Dark, Physical, Physical},
        //Mimic
        new Element[]{Physical, Ice, Physical, Fire, Physical, Dark, Physical, Physical},
        //Challenge:
        new Element[]{Physical, Dark, Physical, Physical, Physical, Dark, Physical, Physical},
    };

    public static readonly PetActiveEffectKind[][] petActiveEffectKinds = new PetActiveEffectKind[][]//[Species][Color]
    {
        //Slime
        new PetActiveEffectKind[]{GetResource, GetMaterial, BuyShopMaterialSlime, GetEquipment, UpgradeQueue, SmartSlimeCoins, None, None,  },
        //MagicSlime
        new PetActiveEffectKind[]{EquipPotion, DisassemblePotion, BuyShopMaterialMagicSlime, ExpandMysteriousWaterCap, AlchemyQueue, TownLevelUp, None, None, },
        //Spider
        new PetActiveEffectKind[]{Capture, BuyShopTrapNormal, BuyShopMaterialSpider, BuyShopTrapIce, BuyShopTrapThunder, BuyShopTrapLight, None, None, },
        //Bat
        new PetActiveEffectKind[]{UpgradeQueue, DisassembleEquipment, BuyShopMaterialBat, DisassembleEquipment, DisassembleEquipment, RebirthUpgradeEXP, None, None, },
        //Fairy
        new PetActiveEffectKind[]{RebirthTier1, RetryDungeon, BuyShopMaterialFairy, OpenChest, BuyShopTrapFire, BuyShopTrapDark, None, None, },
        //Fox
        new PetActiveEffectKind[]{UpgradeQueue, DisassembleEquipment, BuyShopMaterialFox, SkillRankUp, RebirthTier2, EquipNonMaxedEQ, None, None, },
        //DevilFish
        new PetActiveEffectKind[]{AlchemyQueue, AlchemyQueue, BuyShopMaterialDevilfish, AutoCraftDisassembleEQ, BuyBlessing, DisassembleTalismanCommon ,None, None },
        //Treant
        new PetActiveEffectKind[]{None,None, BuyShopMaterialTreant, None,None,None,None, None },
        //FlameTiger
        new PetActiveEffectKind[]{None,None, BuyShopMaterialFlametiger, None,None,None,None, None },
        //Unicorn
        new PetActiveEffectKind[]{None,None, BuyShopMaterialUnicorn, None,None,None,None, None },
        //Mimic
        new PetActiveEffectKind[]{None,None,None,None,None,None,None, None },
        //Challenge
        new PetActiveEffectKind[]{None,None,None,None,None,None,None, None },
    };

    public static readonly PetPassiveEffectKind[][] petPassiveEffectKinds = new PetPassiveEffectKind[][]//[Species][Color]
    {
        //Slime
        new PetPassiveEffectKind[]{ResourceGain, DoubleMaterialChance, OilOfSlimeDropChance, EquipProfGain, TownMatGain, GoldGain, Nothing, Nothing, Nothing,  },
        //MagicSlime
        new PetPassiveEffectKind[]{PotionEffect, GoldGainOnDisassemblePotion, EnchantedClothDropChance, MysteriousWaterGain, ResearchPowerStone, ResearchPowerLeaf, Nothing, Nothing, Nothing,  },
        //Spider
        new PetPassiveEffectKind[]{TamingPointGain, TamingPointGain, SpiderSilkDropChance, TamingPointGain, TamingPointGain, TamingPointGain, Nothing, Nothing, Nothing,  },
        //Bat
        new PetPassiveEffectKind[]{GoldCap, DisassembleTownMatGain, BatWingDropChance, DisassembleTownMatGain, ResearchPowerCrystal, ResearchPowerCrystal, Nothing, Nothing, Nothing, },
        //Fairy
        new PetPassiveEffectKind[]{ExpGain, TownMatGainFromDungeonReward, FairyDustDropChance, ChestPortalOrbChance, TamingPointGain, TamingPointGain, Nothing, Nothing, Nothing, },
        //Fox
        new PetPassiveEffectKind[]{GoldGain, DisassembleTownMatGain, FoxTailDropChance, SkillProfGain, ResearchPowerLeaf, ResearchPowerStone, Nothing, Nothing, Nothing, },
        //DevilFish
        new PetPassiveEffectKind[]{CatalystCriticalChance, MysteriousWaterCap, FishScalesDropChance, PotionEffect, Nothing, Nothing, Nothing, Nothing, Nothing, },
        //Treant
        new PetPassiveEffectKind[]{Nothing, Nothing, CarvedBranchDropChance,Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,  },
        //FlameTiger
        new PetPassiveEffectKind[]{ Nothing, Nothing, ThickFurDropChance,Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,  },
        //Unicorn
        new PetPassiveEffectKind[]{ Nothing, Nothing, UnicornHornDropChance,Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,  },
        //Mimic
        new PetPassiveEffectKind[]{ Nothing, Nothing, Nothing, Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,  },
        //Challenge
        new PetPassiveEffectKind[]{ Nothing, Nothing, Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,Nothing,  },
    };

    public static double PetPassiveEffectValue(PetPassiveEffectKind kind, long rank)
    {
        switch (kind)
        {
            case ResourceGain:
                return 0.10d * rank;
            case OilOfSlimeDropChance:
                return 0.0001d * rank;
            case EnchantedClothDropChance:
                return 0.0001d * rank;
            case SpiderSilkDropChance:
                return 0.0001d * rank;
            case BatWingDropChance:
                return 0.0001d * rank;
            case FairyDustDropChance:
                return 0.0001d * rank;
            case FoxTailDropChance:
                return 0.0001d * rank;
            case FishScalesDropChance:
                return 0.0001d * rank;
            case CarvedBranchDropChance:
                return 0.0001d * rank;
            case ThickFurDropChance:
                return 0.0001d * rank;
            case UnicornHornDropChance:
                return 0.0001d * rank;
            case PotionEffect:
                return 0.005d * rank;
            case TamingPointGain:
                return 0.05d * rank;
            case GoldCap:
                return 0.025d * rank;
            case GoldGain:
                return 0.025d * rank;
            case ExpGain:
                return 0.01d * rank;
            case DoubleMaterialChance://x2Material
                return Math.Min(0.0005d * rank, 0.1d);
            case GoldGainOnDisassemblePotion:
                return 0.05d * rank;
            case DisassembleTownMatGain:
                return 0.01d * rank;
            case TownMatGainFromDungeonReward:
                return 0.01d * rank;
            case EquipProfGain:
                return 0.01d * rank;
            case MysteriousWaterGain://%ではない
                return 0.01d * rank;
            case ChestPortalOrbChance:
                return Math.Min(0.000005d * rank, 0.001d);
            case SkillProfGain:
                return 0.01d * rank;
            case TownMatGain:
                return 0.01d * rank;//Multi
            case ResearchPowerStone:
                return 0.025d * rank;
            case ResearchPowerCrystal:
                return 0.025d * rank;
            case ResearchPowerLeaf:
                return 0.025d * rank;
            case CatalystCriticalChance://Multi
                return 0.01d * rank;
            case MysteriousWaterCap:
                return 2 * rank;
        }
        return 0;
    }

    //SpeciesTalisman
    public static PotionKind SpeciesTalismanKind(AreaKind areaKind)
    {
        switch (areaKind)
        {
            case AreaKind.SlimeVillage:
                return PotionKind.SlimeBadge;
            case AreaKind.MagicSlimeCity:
                return PotionKind.MagicslimeBadge;
            case AreaKind.SpiderMaze:
                return PotionKind.SpiderBadge;
            case AreaKind.BatCave:
                return PotionKind.BatBadge;
            case AreaKind.FairyGarden:
                return PotionKind.FairyBadge;
            case AreaKind.FoxShrine:
                return PotionKind.FoxBadge;
            case AreaKind.DevilFishLake:
                return PotionKind.DevilfishBadge;
            case AreaKind.TreantDarkForest:
                return PotionKind.TreantBadge;
            case AreaKind.FlameTigerVolcano:
                return PotionKind.FlametigerBadge;
            case AreaKind.UnicornIsland:
                return PotionKind.UnicornBadge;
        }
        return PotionKind.SlimeBadge;
    }
}
public enum PetPassiveEffectKind
{
    Nothing,//TBD
    ResourceGain,
    PotionEffect,
    TamingPointGain,
    GoldCap,
    GoldGain,
    ExpGain,
    DoubleMaterialChance,
    GoldGainOnDisassemblePotion,
    DisassembleTownMatGain,
    TownMatGainFromDungeonReward,
    OilOfSlimeDropChance,
    EnchantedClothDropChance,
    SpiderSilkDropChance,
    BatWingDropChance,
    FairyDustDropChance,
    FoxTailDropChance,
    FishScalesDropChance,
    CarvedBranchDropChance,
    ThickFurDropChance,
    UnicornHornDropChance,
    EquipProfGain,
    MysteriousWaterGain,
    ChestPortalOrbChance,
    SkillProfGain,
    TownMatGain,
    ResearchPowerStone,
    ResearchPowerCrystal,
    ResearchPowerLeaf,
    CatalystCriticalChance,
    MysteriousWaterCap,
}

public enum PetActiveEffectKind
{
    None,//TBD
    GetResource,
    GetMaterial,
    GetEquipment,
    DisassembleEquipment,//複数可
    DisassemblePotion,
    UpgradeQueue,//複数可
    ExpandMysteriousWaterCap,
    //AcceptGeneralQuest,
    //ClaimGeneralQuest,
    AlchemyQueue,//複数可
    EquipPotion,
    Capture,
    RebirthTier1,
    RebirthTier2,
    RebirthTier3,
    RebirthTier4,
    RebirthTier5,
    RebirthTier6,
    BuyShopMaterialSlime,
    BuyShopMaterialMagicSlime,
    BuyShopMaterialSpider,
    BuyShopMaterialBat,
    BuyShopMaterialFairy,
    BuyShopMaterialFox,
    BuyShopMaterialDevilfish,
    BuyShopMaterialTreant,
    BuyShopMaterialFlametiger,
    BuyShopMaterialUnicorn,
    BuyShopTrapNormal,
    BuyShopTrapIce,
    BuyShopTrapThunder,
    BuyShopTrapFire,
    BuyShopTrapLight,
    BuyShopTrapDark,
    //BuyShopBlessing,//Queueにするかも
    RetryDungeon,//Toggleを有効にする？
    OpenChest,
    SkillRankUp,
    SmartSlimeCoins,//SlimeCoinMaxでQueueを購入
    TownLevelUp,
    RebirthUpgradeEXP,//各TierのEXPBoosterを購入
    EquipNonMaxedEQ,//装備しているEQがMaxに到達した時、インベントリからMaxに達していないものを探し出して代わりに装備する

    //ここから実装
    AutoCraftDisassembleEQ,//Craftと同時にDisassembleする（DictionaryにToggleを追加し、ToggleONの時はCraft時にAutoDisassemble。Dは関係ない）
    DisassembleTalismanCommon,//TalismanのページにDを追加？
    BuyBlessing,//
}

