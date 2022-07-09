using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using System;

public partial class Save
{
    public double portalOrbNum;
}
//public partial class SaveR
//{
//    public double[] areaClearedNums
//}
public class AreaController
{
    public AREA[][] areas;//[kind][id]
    public AREA Area(AreaKind kind, int id) { return areas[(int)kind][id]; }
    public List<AREA> areaList = new List<AREA>();
    public NullArea nullArea;
    public DUNGEON[][] dungeons;//[kind][id]
    public DUNGEON Dungeon(AreaKind kind, int id) { return dungeons[(int)kind][id]; }
    public List<DUNGEON> dungeonList = new List<DUNGEON>();
    //public AREA Challenge(ChallengeMonsterKind kind)
    //{
    //    return areas[10][(int)kind];
    //}
    //public CHALLENGE_AREA[] challenges;//[challengeMonsterKind]
    //public CHALLENGE_AREA Challenge(ChallengeMonsterKind kind)
    //{
    //    for (int i = 0; i < challenges.Length; i++)
    //    {
    //        if (challenges[i].challengeMonsterKind == kind) return challenges[i];
    //    }
    //    return challenges[0];
    //}
    public PortalOrb portalOrb;

    public Multiplier[] expBonuses = new Multiplier[Enum.GetNames(typeof(AreaKind)).Length];
    public Multiplier[] moveSpeedBonuses = new Multiplier[Enum.GetNames(typeof(AreaKind)).Length];

    public Multiplier areaDebuffReduction;

    //Rebirth用。heroごとの今回のrebirthでのareaClear#
    public double[] areaClearedNums = new double[Enum.GetNames(typeof(HeroKind)).Length];

    public void Initialize()
    {
        for (int i = 0; i < areaList.Count; i++)
        {
            areaList[i].InitializeSimulation();
        }
        for (int i = 0; i < dungeonList.Count; i++)
        {
            dungeonList[i].InitializeSimulation();
        }
    }

    public static List<Vector2> spawnPosition = new List<Vector2>();
    public AreaController()
    {
        portalOrb = new PortalOrb();
        for (int i = 0; i < Parameter.battleFieldSquareNum; i++)
        {
            for (int j = 0; j < Parameter.battleFieldSquareNum; j++)
            {
                spawnPosition.Add(Vector2.right * (i - 4) + Vector2.up * (j - 4));
            }
        }
        for (int i = 0; i < expBonuses.Length; i++)
        {
            expBonuses[i] = new Multiplier();
            moveSpeedBonuses[i] = new Multiplier();
        }
        areaDebuffReduction = new Multiplier(() => 0.90d, () => 0);

        nullArea = new NullArea(this, AreaKind.SlimeVillage, 9);
        areas = new AREA[Enum.GetNames(typeof(AreaKind)).Length][];
        areas[0] = new AREA[8];//追加するときはこの値も変更する
        areas[0][0] = new Area0_0(this, AreaKind.SlimeVillage, 0);
        areas[0][1] = new Area0_1(this, AreaKind.SlimeVillage, 1);
        areas[0][2] = new Area0_2(this, AreaKind.SlimeVillage, 2);
        areas[0][3] = new Area0_3(this, AreaKind.SlimeVillage, 3);
        areas[0][4] = new Area0_4(this, AreaKind.SlimeVillage, 4);
        areas[0][5] = new Area0_5(this, AreaKind.SlimeVillage, 5);
        areas[0][6] = new Area0_6(this, AreaKind.SlimeVillage, 6);
        areas[0][7] = new Area0_7(this, AreaKind.SlimeVillage, 7);
        areas[1] = new AREA[8];//追加するときはこの値も変更する
        areas[1][0] = new Area1_0(this, AreaKind.MagicSlimeCity, 0);
        areas[1][1] = new Area1_1(this, AreaKind.MagicSlimeCity, 1);
        areas[1][2] = new Area1_2(this, AreaKind.MagicSlimeCity, 2);
        areas[1][3] = new Area1_3(this, AreaKind.MagicSlimeCity, 3);
        areas[1][4] = new Area1_4(this, AreaKind.MagicSlimeCity, 4);
        areas[1][5] = new Area1_5(this, AreaKind.MagicSlimeCity, 5);
        areas[1][6] = new Area1_6(this, AreaKind.MagicSlimeCity, 6);
        areas[1][7] = new Area1_7(this, AreaKind.MagicSlimeCity, 7);
        areas[2] = new AREA[8];//追加するときはこの値も変更する
        areas[2][0] = new Area2_0(this, AreaKind.SpiderMaze, 0);
        areas[2][1] = new Area2_1(this, AreaKind.SpiderMaze, 1);
        areas[2][2] = new Area2_2(this, AreaKind.SpiderMaze, 2);
        areas[2][3] = new Area2_3(this, AreaKind.SpiderMaze, 3);
        areas[2][4] = new Area2_4(this, AreaKind.SpiderMaze, 4);
        areas[2][5] = new Area2_5(this, AreaKind.SpiderMaze, 5);
        areas[2][6] = new Area2_6(this, AreaKind.SpiderMaze, 6);
        areas[2][7] = new Area2_7(this, AreaKind.SpiderMaze, 7);
        areas[3] = new AREA[8];//追加するときはこの値も変更する
        areas[3][0] = new Area3_0(this, AreaKind.BatCave, 0);
        areas[3][1] = new Area3_1(this, AreaKind.BatCave, 1);
        areas[3][2] = new Area3_2(this, AreaKind.BatCave, 2);
        areas[3][3] = new Area3_3(this, AreaKind.BatCave, 3);
        areas[3][4] = new Area3_4(this, AreaKind.BatCave, 4);
        areas[3][5] = new Area3_5(this, AreaKind.BatCave, 5);
        areas[3][6] = new Area3_6(this, AreaKind.BatCave, 6);
        areas[3][7] = new Area3_7(this, AreaKind.BatCave, 7);
        areas[4] = new AREA[8];//追加するときはこの値も変更する
        areas[4][0] = new Area4_0(this, AreaKind.FairyGarden, 0);
        areas[4][1] = new Area4_1(this, AreaKind.FairyGarden, 1);
        areas[4][2] = new Area4_2(this, AreaKind.FairyGarden, 2);
        areas[4][3] = new Area4_3(this, AreaKind.FairyGarden, 3);
        areas[4][4] = new Area4_4(this, AreaKind.FairyGarden, 4);
        areas[4][5] = new Area4_5(this, AreaKind.FairyGarden, 5);
        areas[4][6] = new Area4_6(this, AreaKind.FairyGarden, 6);
        areas[4][7] = new Area4_7(this, AreaKind.FairyGarden, 7);
        areas[5] = new AREA[8];//追加するときはこの値も変更する
        areas[5][0] = new Area5_0(this, AreaKind.FoxShrine, 0);
        areas[5][1] = new Area5_1(this, AreaKind.FoxShrine, 1);
        areas[5][2] = new Area5_2(this, AreaKind.FoxShrine, 2);
        areas[5][3] = new Area5_3(this, AreaKind.FoxShrine, 3);
        areas[5][4] = new Area5_4(this, AreaKind.FoxShrine, 4);
        areas[5][5] = new Area5_5(this, AreaKind.FoxShrine, 5);
        areas[5][6] = new Area5_6(this, AreaKind.FoxShrine, 6);
        areas[5][7] = new Area5_7(this, AreaKind.FoxShrine, 7);
        areas[6] = new AREA[8];//追加するときはこの値も変更する
        areas[6][0] = new Area6_0(this, AreaKind.DevilFishLake, 0);
        areas[6][1] = new Area6_1(this, AreaKind.DevilFishLake, 1);
        areas[6][2] = new Area6_2(this, AreaKind.DevilFishLake, 2);
        areas[6][3] = new Area6_3(this, AreaKind.DevilFishLake, 3);
        areas[6][4] = new Area6_4(this, AreaKind.DevilFishLake, 4);
        areas[6][5] = new Area6_5(this, AreaKind.DevilFishLake, 5);
        areas[6][6] = new Area6_6(this, AreaKind.DevilFishLake, 6);
        areas[6][7] = new Area6_7(this, AreaKind.DevilFishLake, 7);
        areas[7] = new AREA[8];//追加するときはこの値も変更する
        areas[7][0] = new Area7_0(this, AreaKind.TreantDarkForest, 0);
        areas[7][1] = new Area7_1(this, AreaKind.TreantDarkForest, 1);
        areas[7][2] = new Area7_2(this, AreaKind.TreantDarkForest, 2);
        areas[7][3] = new Area7_3(this, AreaKind.TreantDarkForest, 3);
        areas[7][4] = new Area7_4(this, AreaKind.TreantDarkForest, 4);
        areas[7][5] = new Area7_5(this, AreaKind.TreantDarkForest, 5);
        areas[7][6] = new Area7_6(this, AreaKind.TreantDarkForest, 6);
        areas[7][7] = new Area7_7(this, AreaKind.TreantDarkForest, 7);
        areas[8] = new AREA[8];//追加するときはこの値も変更する
        areas[8][0] = new Area8_0(this, AreaKind.FlameTigerVolcano, 0);
        areas[8][1] = new Area8_1(this, AreaKind.FlameTigerVolcano, 1);
        areas[8][2] = new Area8_2(this, AreaKind.FlameTigerVolcano, 2);
        areas[8][3] = new Area8_3(this, AreaKind.FlameTigerVolcano, 3);
        areas[8][4] = new Area8_4(this, AreaKind.FlameTigerVolcano, 4);
        areas[8][5] = new Area8_5(this, AreaKind.FlameTigerVolcano, 5);
        areas[8][6] = new Area8_6(this, AreaKind.FlameTigerVolcano, 6);
        areas[8][7] = new Area8_7(this, AreaKind.FlameTigerVolcano, 7);
        areas[9] = new AREA[8];//追加するときはこの値も変更する
        areas[9][0] = new Area9_0(this, AreaKind.UnicornIsland, 0);
        areas[9][1] = new Area9_1(this, AreaKind.UnicornIsland, 1);
        areas[9][2] = new Area9_2(this, AreaKind.UnicornIsland, 2);
        areas[9][3] = new Area9_3(this, AreaKind.UnicornIsland, 3);
        areas[9][4] = new Area9_4(this, AreaKind.UnicornIsland, 4);
        areas[9][5] = new Area9_5(this, AreaKind.UnicornIsland, 5);
        areas[9][6] = new Area9_6(this, AreaKind.UnicornIsland, 6);
        areas[9][7] = new Area9_7(this, AreaKind.UnicornIsland, 7);

        for (int i = 0; i < areas.Length; i++)
        {
            areaList.AddRange(areas[i]);
        }

        dungeons = new DUNGEON[Enum.GetNames(typeof(AreaKind)).Length][];
        dungeons[0] = new DUNGEON[4];
        dungeons[0][0] = new Dungeon0_0(this, AreaKind.SlimeVillage, 0);
        dungeons[0][1] = new Dungeon0_1(this, AreaKind.SlimeVillage, 1);
        dungeons[0][2] = new Dungeon0_2(this, AreaKind.SlimeVillage, 2);
        dungeons[0][3] = new Dungeon0_3(this, AreaKind.SlimeVillage, 3);
        dungeons[1] = new DUNGEON[8];
        dungeons[1][0] = new Dungeon1_0(this, AreaKind.MagicSlimeCity, 0);
        dungeons[1][1] = new Dungeon1_1(this, AreaKind.MagicSlimeCity, 1);
        dungeons[1][2] = new Dungeon1_2(this, AreaKind.MagicSlimeCity, 2);
        dungeons[1][3] = new Dungeon1_3(this, AreaKind.MagicSlimeCity, 3);
        dungeons[1][4] = new Dungeon1_4(this, AreaKind.MagicSlimeCity, 4);
        dungeons[1][5] = new Dungeon1_5(this, AreaKind.MagicSlimeCity, 5);
        dungeons[1][6] = new Dungeon1_6(this, AreaKind.MagicSlimeCity, 6);
        dungeons[1][7] = new Dungeon1_7(this, AreaKind.MagicSlimeCity, 7);
        dungeons[2] = new DUNGEON[4];
        dungeons[2][0] = new Dungeon2_0(this, AreaKind.SpiderMaze, 0);
        dungeons[2][1] = new Dungeon2_1(this, AreaKind.SpiderMaze, 1);
        dungeons[2][2] = new Dungeon2_2(this, AreaKind.SpiderMaze, 2);
        dungeons[2][3] = new Dungeon2_3(this, AreaKind.SpiderMaze, 3);
        dungeons[3] = new DUNGEON[4];
        dungeons[3][0] = new Dungeon3_0(this, AreaKind.BatCave, 0);
        dungeons[3][1] = new Dungeon3_1(this, AreaKind.BatCave, 1);
        dungeons[3][2] = new Dungeon3_2(this, AreaKind.BatCave, 2);
        dungeons[3][3] = new Dungeon3_3(this, AreaKind.BatCave, 3);
        dungeons[4] = new DUNGEON[4];
        dungeons[4][0] = new Dungeon4_0(this, AreaKind.FairyGarden, 0);
        dungeons[4][1] = new Dungeon4_1(this, AreaKind.FairyGarden, 1);
        dungeons[4][2] = new Dungeon4_2(this, AreaKind.FairyGarden, 2);
        dungeons[4][3] = new Dungeon4_3(this, AreaKind.FairyGarden, 3);
        dungeons[5] = new DUNGEON[4];
        dungeons[5][0] = new Dungeon5_0(this, AreaKind.FoxShrine, 0);
        dungeons[5][1] = new Dungeon5_1(this, AreaKind.FoxShrine, 1);
        dungeons[5][2] = new Dungeon5_2(this, AreaKind.FoxShrine, 2);
        dungeons[5][3] = new Dungeon5_3(this, AreaKind.FoxShrine, 3);
        dungeons[6] = new DUNGEON[4];
        dungeons[6][0] = new Dungeon6_0(this, AreaKind.DevilFishLake, 0);
        dungeons[6][1] = new Dungeon6_1(this, AreaKind.DevilFishLake, 1);
        dungeons[6][2] = new Dungeon6_2(this, AreaKind.DevilFishLake, 2);
        dungeons[6][3] = new Dungeon6_3(this, AreaKind.DevilFishLake, 3);
        dungeons[7] = new DUNGEON[0];
        dungeons[8] = new DUNGEON[0];
        dungeons[9] = new DUNGEON[0];

        for (int i = 0; i < dungeons.Length; i++)
        {
            dungeonList.AddRange(dungeons[i]);
        }


        for (int i = 0; i < unlocks.Length; i++)
        {
            unlocks[i] = new Unlock();
        }
        maxAreaPrestigeLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 9));
        maxAreaExpLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 100));
        maxAreaMoveSpeedLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 100));
        townMaterialGainBonus = new Multiplier();
        for (int i = 0; i < townMaterialGainBonusClass.Length; i++)
        {
            townMaterialGainBonusClass[i] = new Multiplier();
        }
        townMaterialDungeonRewardMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        townMaterialGainPerDifficultyMultiplier = new Multiplier();
        chestPortalOrbChance = new Multiplier();
        for (int i = 0; i < clearCountBonusClass.Length; i++)
        {
            clearCountBonusClass[i] = new Multiplier();
        }

        currentSwarmArea = nullArea;
    }

    public Unlock[] unlocks = new Unlock[Enum.GetNames(typeof(AreaKind)).Length];
    public bool IsUnlocked(AreaKind areaKind)
    {
        return unlocks[(int)areaKind].IsUnlocked();
    }
    public Multiplier maxAreaPrestigeLevel;
    public Multiplier maxAreaExpLevel;
    public Multiplier maxAreaMoveSpeedLevel;
    public Multiplier townMaterialGainBonus;
    public Multiplier[] townMaterialGainBonusClass = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier townMaterialDungeonRewardMultiplier;
    public Multiplier townMaterialGainPerDifficultyMultiplier;
    public Multiplier chestPortalOrbChance;
    public Multiplier[] clearCountBonusClass = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public double TotalCompletedNum()
    {
        double tempNum = 0;
        for (int j = 0; j < Enum.GetNames(typeof(AreaKind)).Length; j++)
        {
            for (int i = 0; i < areas[j].Length; i++)
            {
                tempNum += areas[j][i].completedNum.TotalCompletedNum();
            }
        }
        return tempNum;
    }
    public long TotalClearedMissionNum(AreaKind areaKind, bool thisAscension)
    {
        long tempNum = 0;
        for (int i = 0; i < areas[(int)areaKind].Length; i++)
        {
            tempNum += areas[(int)areaKind][i].TotalClearedMissionNum(thisAscension);
        }
        return tempNum;
    }
    public long TotalClearedMissionNum(bool thisAscension = false)
    {
        long tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(AreaKind)).Length; i++)
        {
            int count = i;
            tempNum += TotalClearedMissionNum((AreaKind)count, thisAscension);
        }
        return tempNum;
    }

    //Swarm
    public void StartSwarm()//35分ごとに発生する可能性がある
    {
        if (currentSwarmArea != nullArea) FinishSwarm();
        if (game.guildCtrl.Level() < 10 || WithinRandom(0.50d))//50%の確率で発生する
            return;
        currentSwarmArea = RandomArea();
        currentSwarmArea.swarm.Start();
    }
    public void FinishSwarm()
    {
        currentSwarmArea.swarm.Initialize();
        currentSwarmArea = nullArea;
    }
    public AREA currentSwarmArea;
    public bool isSwarm => currentSwarmArea != nullArea;

    //Area１つをランダムで選ぶ関数
    public AREA RandomArea()
    {
        AreaKind areaKind = RandomAreaKind();
        int id = RandomAreaId(areaKind);
        return Area(areaKind, id);
    }
    public AreaKind RandomAreaKind()
    {
        int tempAreaKindId = 10;//Randomで使うので最大数+1になっている
        for (int i = 0; i < game.areaCtrl.unlocks.Length; i++)
        {
            if (!game.areaCtrl.unlocks[i].IsUnlocked())
            {
                tempAreaKindId = i;
                break;
            }
        }
        return (AreaKind)UnityEngine.Random.Range(0, tempAreaKindId);
    }
    public int RandomAreaId(AreaKind kind)
    {
        int tempAreaId = 8;
        for (int i = 0; i < 8; i++)
        {
            if (!game.areaCtrl.areas[(int)kind][i].IsUnlocked())
            {
                tempAreaId = i;
                break;
            }
        }
        return UnityEngine.Random.Range(0, tempAreaId);
    }
}
public class PortalOrb : NUMBER
{
    public override double value { get => main.S.portalOrbNum; set => main.S.portalOrbNum = value; }
}

public class NullArea : AREA
{
    public NullArea(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
    }
}
public class Area0_0 : AREA
{
    public override long minLevel => 1;
    public override long maxLevel => 1;
    public Area0_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SlimeSword;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
    }
    public override void SetMission()
    {
        base.SetMission();
    }
}
public class Area0_1 : AREA
{
    public override long minLevel => 1;
    public override long maxLevel => 5;
    public Area0_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 1);
        uniqueEquipmentKind = EquipmentKind.SlimeGlove;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));

    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area0_2 : AREA
{
    public override long minLevel => 5;
    public override long maxLevel => 15;
    public Area0_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 1);
        uniqueEquipmentKind = EquipmentKind.SlimeRing;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(3, MonsterRarity.Uncommon));
    }
}
public class Area0_3 : AREA//単体強め向け
{
    public override long minLevel => 15;
    public override long maxLevel => 25;
    public Area0_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 10);
        uniqueEquipmentKind = EquipmentKind.SlimeBelt;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2, MonsterColor.Red));
    }
}
public class Area0_4 : AREA//範囲向け
{
    public override long minLevel => 20;
    public override long maxLevel => 35;
    public Area0_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 100);
        uniqueEquipmentKind = EquipmentKind.SlimePincenez;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(6));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(7));
    }
}
public class Area0_5 : AREA//単体
{
    public override long minLevel => 30;
    public override long maxLevel => 45;
    public Area0_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 250);
        uniqueEquipmentKind = EquipmentKind.SlimeWing;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(DefaultWave(1, MonsterColor.Purple));
    }
}
public class Area0_6 : AREA//範囲
{
    public override long minLevel => 55;
    public override long maxLevel => 70;
    public Area0_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(4, 750);
        uniqueEquipmentKind = EquipmentKind.SlimePoncho;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(8));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(9));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(10));//5
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(11));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(12));
        wave.Add(DefaultWave(3));
    }
}
public class Area0_7 : AREA//単体
{
    public override long minLevel => 85;
    public override long maxLevel => 105;
    public Area0_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 2500);
        requiredCompleteNum.Add(6, 1250);
        uniqueEquipmentKind = EquipmentKind.SlimeDart;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.OilOfSlime), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(2));
        wave.Add(DefaultWave(3));
    }
}


public class Area1_0 : AREA
{
    public override long minLevel => 20;
    public override long maxLevel => 30;
    public Area1_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
    }
}

public class Area1_1 : AREA
{
    public override long minLevel => 45;
    public override long maxLevel => 65;
    public Area1_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 30);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeHat;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area1_2 : AREA
{
    public override long minLevel => 70;
    public override long maxLevel => 95;
    public Area1_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 60);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeBow;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(DefaultWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
    }
}
public class Area1_3 : AREA
{
    public override long minLevel => 80;
    public override long maxLevel => 105;
    public Area1_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 120);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeShoes;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(DefaultWave(5, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area1_4 : AREA
{
    public override long minLevel => 90;
    public override long maxLevel => 115;
    public Area1_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 240);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeRecorder;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
    }
}
public class Area1_5 : AREA
{
    public override long minLevel => 115;
    public override long maxLevel => 135;
    public Area1_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 480);
        requiredCompleteNum.Add(4, 480);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeEarring;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(5));
    }
}
public class Area1_6 : AREA
{
    public override long minLevel => 135;
    public override long maxLevel => 160;
    public Area1_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 2000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeBalloon;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(DefaultWave(3, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
    }
}
public class Area1_7 : AREA
{
    public override long minLevel => 160;
    public override long maxLevel => 185;
    public Area1_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeSkirt;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedCloth), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(3, MonsterColor.Purple));
    }
}

public class Area2_0 : AREA
{
    public override long minLevel => 40;
    public override long maxLevel => 60;
    public Area2_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderHat;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
    }
}
public class Area2_1 : AREA
{
    public override long minLevel => 70;
    public override long maxLevel => 90;
    public Area2_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderSkirt;
        requiredCompleteNum.Add(0, 500);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5, MonsterColor.Blue));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}

public class Area2_2 : AREA
{
    public override long minLevel => 95;
    public override long maxLevel => 120;
    public Area2_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderSuit;
        requiredCompleteNum.Add(1, 500);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(3, MonsterColor.Red));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(5, MonsterColor.Red));
    }
}

public class Area2_3 : AREA
{
    public override long minLevel => 120;
    public override long maxLevel => 140;
    public Area2_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderDagger;
        requiredCompleteNum.Add(2, 1500);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(5, MonsterColor.Green));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(8));
        wave.Add(RandomWave(5, MonsterColor.Green));
    }
}

public class Area2_4 : AREA
{
    public override long minLevel => 135;
    public override long maxLevel => 165;
    public Area2_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderWing;
        requiredCompleteNum.Add(3, 2000);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(6));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(DefaultWave(5, MonsterRarity.Uncommon));
        wave.Add(RandomWave(6, MonsterRarity.Uncommon));
        wave.Add(RandomWave(7, MonsterRarity.Uncommon));
        wave.Add(RandomWave(7, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(5, MonsterColor.Purple));
    }
}
public class Area2_5 : AREA
{
    public override long minLevel => 160;
    public override long maxLevel => 180;
    public Area2_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderCatchingNet;
        requiredCompleteNum.Add(0, 5000);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(10));
        wave.Add(DefaultWave(11));
        wave.Add(DefaultWave(12));
        wave.Add(DefaultWave(13));
        wave.Add(DefaultWave(14));
        wave.Add(DefaultWave(15));
        wave.Add(DefaultWave(16));
        wave.Add(DefaultWave(17));
        wave.Add(DefaultWave(18));
        wave.Add(DefaultWave(19));
    }
}
public class Area2_6 : AREA
{
    public override long minLevel => 175;
    public override long maxLevel => 200;
    public Area2_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderStick;
        requiredCompleteNum.Add(4, 2500);
        requiredCompleteNum.Add(5, 2500);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(8));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(9));
        wave.Add(RandomWave(11));
        wave.Add(RandomWave(13));
        wave.Add(RandomWave(15));
    }
}
public class Area2_7 : AREA
{
    public override long minLevel => 210;
    public override long maxLevel => 250;
    public Area2_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.SpiderFoldingFan;
        requiredCompleteNum.Add(5, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.SpiderSilk), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(20));
        wave.Add(RandomWave(20));
        wave.Add(RandomWave(20));
        wave.Add(RandomWave(20));
        wave.Add(RandomWave(20));
    }
}
//Bat
public class Area3_0 : AREA
{
    public override long minLevel => 60;
    public override long maxLevel => 80;
    public Area3_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.BatRing;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}

public class Area3_1 : AREA
{
    public override long minLevel => 100;
    public override long maxLevel => 120;
    public Area3_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 500);
        uniqueEquipmentKind = EquipmentKind.BatShoes;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area3_2 : AREA
{
    public override long minLevel => 125;
    public override long maxLevel => 155;
    public Area3_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 1000);
        uniqueEquipmentKind = EquipmentKind.BatSword;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4, MonsterColor.Yellow));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area3_3 : AREA
{
    public override long minLevel => 175;
    public override long maxLevel => 210;
    public Area3_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 1500);
        uniqueEquipmentKind = EquipmentKind.BatHat;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(2, MonsterColor.Red));
    }
}
public class Area3_4 : AREA
{
    public override long minLevel => 200;
    public override long maxLevel => 240;
    public Area3_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 2000);
        uniqueEquipmentKind = EquipmentKind.BatRecorder;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(6));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(7));
        wave.Add(RandomWave(8));
        wave.Add(RandomWave(8));
        wave.Add(RandomWave(10));
    }
}
public class Area3_5 : AREA
{
    public override long minLevel => 230;
    public override long maxLevel => 260;
    public Area3_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(4, 2500);
        uniqueEquipmentKind = EquipmentKind.BatBow;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area3_6 : AREA
{
    public override long minLevel => 270;
    public override long maxLevel => 290;
    public Area3_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 3000);
        uniqueEquipmentKind = EquipmentKind.BatMascaradeMask;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area3_7 : AREA
{
    public override long minLevel => 300;
    public override long maxLevel => 330;
    public Area3_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(6, 5000);
        uniqueEquipmentKind = EquipmentKind.BatCloak;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.BatWing), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
        wave.Add(RandomWave(5));
    }
}
//Fairy
public class Area4_0 : AREA
{
    public override long minLevel => 105;
    public override long maxLevel => 125;
    public Area4_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.FairyClothes;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}

public class Area4_1 : AREA
{
    public override long minLevel => 125;
    public override long maxLevel => 150;
    public Area4_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 500);
        uniqueEquipmentKind = EquipmentKind.FairyStaff;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(4));
    }
}
public class Area4_2 : AREA
{
    public override long minLevel => 150;
    public override long maxLevel => 180;
    public Area4_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 1000);
        uniqueEquipmentKind = EquipmentKind.FairyBoots;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(DefaultWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
    }
}
public class Area4_3 : AREA
{
    public override long minLevel => 180;
    public override long maxLevel => 215;
    public Area4_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 2000);
        uniqueEquipmentKind = EquipmentKind.FairyGlove;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(DefaultWave(5, MonsterColor.Yellow));
        wave.Add(RandomWave(4, MonsterColor.Yellow));
        wave.Add(RandomWave(4, MonsterColor.Yellow));
        wave.Add(RandomWave(4, MonsterColor.Yellow));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area4_4 : AREA
{
    public override long minLevel => 215;
    public override long maxLevel => 235;
    public Area4_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 3000);
        uniqueEquipmentKind = EquipmentKind.FairyBrooch;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
        wave.Add(DefaultWave(3, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
    }
}
public class Area4_5 : AREA
{
    public override long minLevel => 235;
    public override long maxLevel => 270;
    public Area4_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 5000);
        uniqueEquipmentKind = EquipmentKind.FairyLamp;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(5));
    }
}
public class Area4_6 : AREA
{
    public override long minLevel => 265;
    public override long maxLevel => 295;
    public Area4_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 5000);
        requiredCompleteNum.Add(2, 5000);
        uniqueEquipmentKind = EquipmentKind.FairyWing;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(RandomWave(2, MonsterColor.Green));
        wave.Add(DefaultWave(2, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
        wave.Add(RandomWave(5, MonsterColor.Green));
    }
}
public class Area4_7 : AREA
{
    public override long minLevel => 295;
    public override long maxLevel => 320;
    public Area4_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 5000);
        requiredCompleteNum.Add(3, 5000);
        uniqueEquipmentKind = EquipmentKind.FairyShuriken;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.FairyDust), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(DefaultWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(3, MonsterColor.Purple));
    }
}

//Fox
public class Area5_0 : AREA
{
    public override long minLevel => 140;
    public override long maxLevel => 165;
    public Area5_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.FoxKanzashi;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}

public class Area5_1 : AREA
{
    public override long minLevel => 200;
    public override long maxLevel => 230;
    public Area5_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 5000);
        uniqueEquipmentKind = EquipmentKind.FoxLoincloth;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(1));
    }
}
public class Area5_2 : AREA
{
    public override long minLevel => 250;
    public override long maxLevel => 275;
    public Area5_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 5000);
        uniqueEquipmentKind = EquipmentKind.FoxMask;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(4));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
    }
}
public class Area5_3 : AREA
{
    public override long minLevel => 270;
    public override long maxLevel => 300;
    public Area5_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 10000);
        requiredCompleteNum.Add(1, 10000);
        uniqueEquipmentKind = EquipmentKind.FoxHamayayumi;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterRarity.Common));
        wave.Add(RandomWave(3, MonsterRarity.Common));
        wave.Add(RandomWave(3, MonsterRarity.Common));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(4, MonsterRarity.Common));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2, MonsterRarity.Rare));
    }
}
public class Area5_4 : AREA
{
    public override long minLevel => 325;
    public override long maxLevel => 345;
    public Area5_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 10000);
        uniqueEquipmentKind = EquipmentKind.FoxHat;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(1));
    }
}
public class Area5_5 : AREA
{
    public override long minLevel => 350;
    public override long maxLevel => 370;
    public Area5_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 10000);
        requiredCompleteNum.Add(4, 10000);
        uniqueEquipmentKind = EquipmentKind.FoxCoat;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(2, MonsterRarity.Rare));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(5, MonsterRarity.Rare));
    }
}
public class Area5_6 : AREA
{
    public override long minLevel => 365;
    public override long maxLevel => 385;
    public Area5_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 20000);
        uniqueEquipmentKind = EquipmentKind.FoxBoot;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
    }
}
public class Area5_7 : AREA
{
    public override long minLevel => 380;
    public override long maxLevel => 400;
    public Area5_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(6, 25000);
        uniqueEquipmentKind = EquipmentKind.FoxEma;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.FoxTail), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(5, MonsterRarity.Common));
        wave.Add(RandomWave(5, MonsterRarity.Common));
        wave.Add(RandomWave(5, MonsterRarity.Common));
        wave.Add(RandomWave(3, MonsterRarity.Uncommon));
        wave.Add(RandomWave(3, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(5, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
    }
}

//Devilfish
public class Area6_0 : AREA
{
    public override long minLevel => 180;
    public override long maxLevel => 205;
    public Area6_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.DevilfishSword;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}

public class Area6_1 : AREA
{
    public override long minLevel => 220;
    public override long maxLevel => 245;
    public Area6_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 5000);
        uniqueEquipmentKind = EquipmentKind.DevilfishWing;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area6_2 : AREA
{
    public override long minLevel => 270;
    public override long maxLevel => 295;
    public Area6_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 10000);
        uniqueEquipmentKind = EquipmentKind.DevilfishRecorder;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(3, MonsterRarity.Uncommon));
    }
}
public class Area6_3 : AREA
{
    public override long minLevel => 320;
    public override long maxLevel => 345;
    public Area6_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 25000);
        uniqueEquipmentKind = EquipmentKind.DevilfishArmor;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(7));
        wave.Add(DefaultWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(DefaultWave(4));
        wave.Add(DefaultWave(5));
        wave.Add(DefaultWave(8));
        wave.Add(DefaultWave(6));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5));
    }
}
public class Area6_4 : AREA
{
    public override long minLevel => 340;
    public override long maxLevel => 365;
    public Area6_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 25000);
        uniqueEquipmentKind = EquipmentKind.DevilfishScarf;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(2, MonsterRarity.Rare));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3, MonsterRarity.Rare));
    }
}
public class Area6_5 : AREA
{
    public override long minLevel => 365;
    public override long maxLevel => 390;
    public Area6_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 25000);
        uniqueEquipmentKind = EquipmentKind.DevilfishGill;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(5));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(1, MonsterRarity.Rare));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(4));
        wave.Add(RandomWave(5, MonsterRarity.Rare));
    }
}
public class Area6_6 : AREA
{
    public override long minLevel => 400;
    public override long maxLevel => 420;
    public Area6_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 20000);
        requiredCompleteNum.Add(4, 20000);
        uniqueEquipmentKind = EquipmentKind.DevilfishPendant;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(9, MonsterRarity.Common));
        wave.Add(RandomWave(2, MonsterRarity.Common));
        wave.Add(RandomWave(2, MonsterRarity.Common));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(10, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
    }
}
public class Area6_7 : AREA
{
    public override long minLevel => 425;
    public override long maxLevel => 450;
    public Area6_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 20000);
        requiredCompleteNum.Add(6, 20000);
        uniqueEquipmentKind = EquipmentKind.DevilfishRing;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.FishScales), (x) => materialRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => Math.Floor(TownMaterialRewardNum(x) / 3));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(9, MonsterRarity.Common));
        wave.Add(RandomWave(3, MonsterRarity.Common));
        wave.Add(RandomWave(3, MonsterRarity.Common));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        wave.Add(DefaultWave(3, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
        wave.Add(RandomWave(3, MonsterColor.Green));
        wave.Add(RandomWave(4, MonsterColor.Purple));
        wave.Add(RandomWave(4, MonsterColor.Purple));
    }
}

//Treant
public class Area7_0 : AREA
{
    public override long minLevel => 280;
    public override long maxLevel => 310;
    public Area7_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
    }
}

public class Area7_1 : AREA
{
    public override long minLevel => 310;
    public override long maxLevel => 350;
    public Area7_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 30);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area7_2 : AREA
{
    public override long minLevel => 350;
    public override long maxLevel => 390;
    public Area7_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 60);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeBow;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(DefaultWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
    }
}
public class Area7_3 : AREA
{
    public override long minLevel => 390;
    public override long maxLevel => 450;
    public Area7_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 120);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(DefaultWave(5, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area7_4 : AREA
{
    public override long minLevel => 460;
    public override long maxLevel => 530;
    public Area7_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 240);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
    }
}
public class Area7_5 : AREA
{
    public override long minLevel => 540;
    public override long maxLevel => 600;
    public Area7_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 480);
        requiredCompleteNum.Add(4, 480);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(5));
    }
}
public class Area7_6 : AREA
{
    public override long minLevel => 610;
    public override long maxLevel => 650;
    public Area7_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 2000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
    }
}
public class Area7_7 : AREA
{
    public override long minLevel => 650;
    public override long maxLevel => 700;
    public Area7_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.CarvedBranch), (x) => materialRewardNum);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(DefaultWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(3, MonsterColor.Purple));
    }
}
public class Area8_0 : AREA
{
    public override long minLevel => 350;
    public override long maxLevel => 390;
    public Area8_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
    }
}

public class Area8_1 : AREA
{
    public override long minLevel => 390;
    public override long maxLevel => 430;
    public Area8_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 30);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area8_2 : AREA
{
    public override long minLevel => 430;
    public override long maxLevel => 480;
    public Area8_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 60);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeBow;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(DefaultWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
    }
}
public class Area8_3 : AREA
{
    public override long minLevel => 490;
    public override long maxLevel => 540;
    public Area8_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 120);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(DefaultWave(5, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area8_4 : AREA
{
    public override long minLevel => 550;
    public override long maxLevel => 600;
    public Area8_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 240);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
    }
}
public class Area8_5 : AREA
{
    public override long minLevel => 610;
    public override long maxLevel => 680;
    public Area8_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 480);
        requiredCompleteNum.Add(4, 480);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(5));
    }
}
public class Area8_6 : AREA
{
    public override long minLevel => 690;
    public override long maxLevel => 750;
    public Area8_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 2000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
    }
}
public class Area8_7 : AREA
{
    public override long minLevel => 760;
    public override long maxLevel => 810;
    public Area8_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.ThickFur), (x) => materialRewardNum);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(DefaultWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(3, MonsterColor.Purple));
    }
}
public class Area9_0 : AREA
{
    public override long minLevel => 430;
    public override long maxLevel => 470;
    public Area9_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(3));
    }
}

public class Area9_1 : AREA
{
    public override long minLevel => 480;
    public override long maxLevel => 530;
    public Area9_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 30);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(DefaultWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
        wave.Add(RandomWave(3));
    }
}
public class Area9_2 : AREA
{
    public override long minLevel => 530;
    public override long maxLevel => 580;
    public Area9_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 60);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeBow;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(DefaultWave(4, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(2, MonsterColor.Blue));
        wave.Add(RandomWave(3, MonsterColor.Blue));
        wave.Add(RandomWave(4, MonsterColor.Blue));
    }
}
public class Area9_3 : AREA
{
    public override long minLevel => 580;
    public override long maxLevel => 630;
    public Area9_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 120);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(DefaultWave(5, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(3, MonsterColor.Yellow));
        wave.Add(RandomWave(5, MonsterColor.Yellow));
    }
}
public class Area9_4 : AREA
{
    public override long minLevel => 650;
    public override long maxLevel => 705;
    public Area9_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 240);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(RandomWave(1, MonsterColor.Red));
        wave.Add(DefaultWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(2, MonsterColor.Red));
        wave.Add(RandomWave(3, MonsterColor.Red));
    }
}
public class Area9_5 : AREA
{
    public override long minLevel => 720;
    public override long maxLevel => 780;
    public Area9_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 480);
        requiredCompleteNum.Add(4, 480);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(RandomWave(1));
        wave.Add(DefaultWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(2));
        wave.Add(RandomWave(5));
    }
}
public class Area9_6 : AREA
{
    public override long minLevel => 790;
    public override long maxLevel => 850;
    public Area9_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 2000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (x) => TownMaterialRewardNum(x));
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(DefaultWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
        wave.Add(RandomWave(1, MonsterColor.Green));
    }
}
public class Area9_7 : AREA
{
    public override long minLevel => 850;
    public override long maxLevel => 900;
    public Area9_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        uniqueEquipmentKind = EquipmentKind.MagicSlimeStick;
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.UnicornHorn), (x) => materialRewardNum);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        wave.Add(DefaultWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(DefaultWave(2, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(1, MonsterColor.Purple));
        wave.Add(RandomWave(3, MonsterColor.Purple));
    }
}







