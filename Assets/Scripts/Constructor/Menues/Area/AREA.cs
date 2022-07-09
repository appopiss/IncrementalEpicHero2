using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using System.Threading.Tasks;

public partial class SaveR
{
    public long[] currentAreaLevelsSlime;
    public long[] currentAreaLevelsMagicSlime;
    public long[] currentAreaLevelsSpider;
    public long[] currentAreaLevelsBat;
    public long[] currentAreaLevelsFairy;
    public long[] currentAreaLevelsFox;
    public long[] currentAreaLevelsDevilFish;
    public long[] currentAreaLevelsTreant;
    public long[] currentAreaLevelsFlameTiger;
    public long[] currentAreaLevelsUnicorn;

    public bool[] areaIsReceivedFirstRewardSlime;
    public bool[] areaIsReceivedFirstRewardMagicSlime;
    public bool[] areaIsReceivedFirstRewardSpider;
    public bool[] areaIsReceivedFirstRewardBat;
    public bool[] areaIsReceivedFirstRewardFairy;
    public bool[] areaIsReceivedFirstRewardFox;
    public bool[] areaIsReceivedFirstRewardDevilFish;
    public bool[] areaIsReceivedFirstRewardTreant;
    public bool[] areaIsReceivedFirstRewardFlameTiger;
    public bool[] areaIsReceivedFirstRewardUnicorn;

    public double[] areaCompleteNumsSlime;
    public double[] areaCompleteNumsMagicSlime;
    public double[] areaCompleteNumsSpider;
    public double[] areaCompleteNumsBat;
    public double[] areaCompleteNumsFairy;
    public double[] areaCompleteNumsFox;
    public double[] areaCompleteNumsDevilFish;
    public double[] areaCompleteNumsTreant;
    public double[] areaCompleteNumsFlameTiger;
    public double[] areaCompleteNumsUnicorn;

    public float[] areaBestTimesSlime;
    public float[] areaBestTimesMagicSlime;
    public float[] areaBestTimesSpider;
    public float[] areaBestTimesBat;
    public float[] areaBestTimesFairy;
    public float[] areaBestTimesFox;
    public float[] areaBestTimesDevilFish;
    public float[] areaBestTimesTreant;
    public float[] areaBestTimesFlameTiger;
    public float[] areaBestTimesUnicorn;

    public double[] areaBestGoldsSlime;
    public double[] areaBestGoldsMagicSlime;
    public double[] areaBestGoldsSpider;
    public double[] areaBestGoldsBat;
    public double[] areaBestGoldsFairy;
    public double[] areaBestGoldsFox;
    public double[] areaBestGoldsDevilFish;
    public double[] areaBestGoldsTreant;
    public double[] areaBestGoldsFlameTiger;
    public double[] areaBestGoldsUnicorn;

    public double[] areaBestExpsSlime;
    public double[] areaBestExpsMagicSlime;
    public double[] areaBestExpsSpider;
    public double[] areaBestExpsBat;
    public double[] areaBestExpsFairy;
    public double[] areaBestExpsFox;
    public double[] areaBestExpsDevilFish;
    public double[] areaBestExpsTreant;
    public double[] areaBestExpsFlameTiger;
    public double[] areaBestExpsUnicorn;
}
public class AREA
{
    public AREA() { }
    public AREA(AreaController areaCtrl, AreaKind kind, int id)
    {
        this.areaCtrl = areaCtrl;
        this.kind = kind;
        this.id = id;
        swarm = new Swarm(this);
        maxAreaLevel = new Multiplier();
        missionUnlockedNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 2));
        clearCountBonus = new Multiplier();
        decreaseMaxWaveNum = new Multiplier();
        chestDropChance = new Multiplier();
        addLimitTime = new Multiplier();
        metalSlimeChance = new Multiplier();
        portalOrbReduceFactor = new Multiplier();
        level = new AreaLevel(kind, id, isDungeon, () => (long)maxAreaLevel.Value());
        SetAreaMastery();
        completedNum = new AreaCompleteNum(this, kind, id, level, isDungeon);
        prestige = new AreaPrestige(this);
        SetAllWave();
        SetMonsterSpawns();
        //Mission
        for (int i = 0; i < missionListArray.Length; i++)
        {
            missionListArray[i] = new List<MISSION>();
        }
        SetMission();
    }

    //Swarm
    public Swarm swarm;
    //Challenge
    public virtual bool isChallenge => false;
    //For Dungeon
    public virtual bool isDungeon => false;
    public float[] enterTime = new float[Enum.GetNames(typeof(HeroKind)).Length];//スタートした時刻[heroKind]
    public float[] additionalTime = new float[Enum.GetNames(typeof(HeroKind)).Length];//DungeonのChestによるLimitTime+
    public float simulatedEnterTime;
    public float simulatedAdditionalTime;
    public virtual float limitTime => 60 * 60;//デフォルト:60分
    public float ElapsedTime(HeroKind heroKind, float timecount, bool isSimulated)
    {
        if (isSimulated) return RealElapsedTime(heroKind, timecount, isSimulated) - simulatedAdditionalTime;
        return RealElapsedTime(heroKind, timecount, isSimulated) - additionalTime[(int)heroKind];
    }
    public float RealElapsedTime(HeroKind heroKind, float timecount, bool isSimulated)
    {
        if (isSimulated) return timecount - simulatedEnterTime;
        return timecount - enterTime[(int)heroKind];
    }
    public float TimeLeft(BATTLE_CONTROLLER battleCtrl) { return Math.Max(0, limitTime - ElapsedTime(battleCtrl.heroKind, battleCtrl.timecount, battleCtrl.isSimulated)); }
    public bool IsTimeOver(BATTLE_CONTROLLER battleCtrl) { return ElapsedTime(battleCtrl.heroKind, battleCtrl.timecount, battleCtrl.isSimulated) > limitTime; }

    //Reward
    public Action rewardUIAction;
    public Func<double> rewardExp => RewardExp;
    public Func<double> rewardGold => RewardGold;
    public virtual double RewardExp() { return 0; }
    public virtual double RewardGold() { return 0; }
    //public Func<long> rewardEC = () => 0;
    //Repeatable Reward
    public Func<EnchantKind> rewardEnchantKind = () => EnchantKind.Nothing;
    public Func<EquipmentEffectKind> rewardEnchantEffectKind = () => EquipmentEffectKind.Nothing;
    public Func<long> rewardEnchantLevel = () => 1;
    public Dictionary<Func<PotionKind>, Func<long>> rewardPotion = new Dictionary<Func<PotionKind>, Func<long>>();
    public Dictionary<NUMBER, Func<HeroKind, double>> rewardMaterial = new Dictionary<NUMBER, Func<HeroKind, double>>();
    public virtual double TownMaterialRewardNum(HeroKind heroKind)
    {
        return Math.Floor((1 + level.value * areaCtrl.townMaterialGainPerDifficultyMultiplier.Value()) * (1 + areaCtrl.townMaterialGainBonus.Value() + areaCtrl.townMaterialGainBonusClass[(int)heroKind].Value()) * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value());
    }
    //↓はver0.1.1.2まで
    //public virtual double townMaterialRewardNum => (1 + 2 * level.value + areaCtrl.townMaterialGainBonus.Value()) * game.townCtrl.townMaterialGainMultiplier.Value();
    public double materialRewardNum => 1 + level.value;//MaxLevel();
    public virtual double enchantedShardRewardNum => 1 + level.value;
    //First Reward
    public Func<EnchantKind> rewardEnchantKindFirst = () => EnchantKind.Nothing;
    public Func<EquipmentEffectKind> rewardEnchantEffectKindFirst = () => EquipmentEffectKind.Nothing;
    public Func<long> rewardEnchantLevelFirst = () => 1;
    public Dictionary<Func<PotionKind>, Func<long>> rewardPotionFirst = new Dictionary<Func<PotionKind>, Func<long>>();
    public Dictionary<NUMBER, Func<HeroKind, double>> rewardMaterialFirst = new Dictionary<NUMBER, Func<HeroKind, double>>();
    public virtual void RewardAction(HeroKind heroKind) { }//その他のReward
    void GetReward(HeroKind heroKind, double clearNum)
    {
        if (rewardExp() > 0) game.statsCtrl.Exp(heroKind).Increase(rewardExp() * clearNum, true);
        if (rewardGold() > 0) game.resourceCtrl.gold.Increase(rewardGold() * clearNum);
        //EC if(rewardEC()>0)


        for (int i = 0; i < clearNum; i++)
        {
            game.inventoryCtrl.CreateEnchant(rewardEnchantKind(), rewardEnchantEffectKind(), rewardEnchantLevel());
            foreach (var item in rewardPotion)
            {
                game.inventoryCtrl.CreatePotion(item.Key(), item.Value());
            }
        }
        foreach (var item in rewardMaterial)
        {
            item.Key.Increase(item.Value(heroKind) * clearNum);
        }

        //First Reward:complete#を++した後に呼ぶ<-これではclearNumが２以上の時に成り立たない。
        //よって、complete#を++する前に別途呼ぶことにした
        //if (completedNum.value <= 1)
        //{
        //    game.inventoryCtrl.CreateEnchant(rewardEnchantKindFirst(), rewardEnchantEffectKindFirst(), rewardEnchantLevelFirst());
        //    foreach (var item in rewardPotionFirst)
        //    {
        //        game.inventoryCtrl.CreatePotion(item.Key(), item.Value());
        //    }
        //    foreach (var item in rewardMaterialFirst)
        //    {
        //        item.Key.Increase(item.Value() * clearNum);
        //    }
        //}
        RewardAction(heroKind);
        if (rewardUIAction != null) rewardUIAction();
    }

    public double RequiredPortalOrb()
    {
        if (!isDungeon) return 0;
        return 1 + Math.Max(0, level.value - portalOrbReduceFactor.Value());
    }
    public virtual bool CanStart()
    {
        if (!areaCtrl.IsUnlocked(kind)) return false;
        if (!IsUnlocked()) return false;
        if (areaCtrl.portalOrb.value < RequiredPortalOrb()) return false;
        return true;
    }
    public virtual void StartAction(BATTLE_CONTROLLER battleCtrl, float startTime, bool isSimulated)
    {
        if (isSimulated)
        {
            InitializeSimulation();
            simulatedAdditionalTime = 0;
            simulatedEnterTime = startTime;
            return;
        }
        enterTime[(int)battleCtrl.heroKind] = startTime;
        additionalTime[(int)battleCtrl.heroKind] = 0;
        areaCtrl.portalOrb.Decrease(RequiredPortalOrb());
    }
    double tempClearNum;
    public virtual void ClearAction(HERO_BATTLE heroBattle, bool isCleared, double gold, double exp)
    {
        if (RealElapsedTime(heroBattle.heroKind, heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated) < 1f) return;//1秒以下はバグと判断する
        bestGold = Math.Max(bestGold, gold);
        bestExp = Math.Max(bestExp, exp);

        if (!isCleared) return;
        tempClearNum = 1 + ClearCountBonus(heroBattle.heroKind);
        //background
        tempClearNum *= game.guildCtrl.Member(heroBattle.heroKind).gainRate;
        GetFirstReward(heroBattle.heroKind);
        completedNum.Increase(tempClearNum);
        areaCtrl.areaClearedNums[(int)heroBattle.heroKind] += tempClearNum;
        if (swarm.isSwarm) swarm.IncreaseScoreOnAreaClear(tempClearNum);
        if (bestTime <= 1f) bestTime = RealElapsedTime(heroBattle.heroKind, heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated);
        else bestTime = Mathf.Min(bestTime, RealElapsedTime(heroBattle.heroKind, heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated));
        GetReward(heroBattle.heroKind, tempClearNum);
        CheckMission(heroBattle);
    }

    public double ClearCountBonus(HeroKind heroKind)
    {
        if (isDungeon) return 0;//Dungeonの場合はなしにした
        return clearCountBonus.Value() + areaCtrl.clearCountBonusClass[(int)heroKind].Value();
    }

    void GetFirstReward(HeroKind heroKind)
    {
        if (completedNum.isReceivedFirstReward) return;
        game.inventoryCtrl.CreateEnchant(rewardEnchantKindFirst(), rewardEnchantEffectKindFirst(), rewardEnchantLevelFirst());
        foreach (var item in rewardPotionFirst)
        {
            game.inventoryCtrl.CreatePotion(item.Key(), item.Value());
        }
        foreach (var item in rewardMaterialFirst)
        {
            item.Key.Increase(item.Value(heroKind));
        }
        completedNum.isReceivedFirstReward = true;
    }
    void CheckMission(HERO_BATTLE heroBattle)
    {
        for (int i = 0; i < Math.Min(missionListArray[level.value].Count, missionUnlockedNum.Value()); i++)
        {
            missionListArray[level.value][i].CheckMissionClear(heroBattle);
        }
    }
    //全てのPrestigeを含む
    public long TotalClearedMissionNum(bool thisAscension = false)
    {
        long tempNum = 0;
        for (int i = 0; i < missionListArray.Length; i++)
        {
            int count = i;
            tempNum += ClearedMissionNum(count, thisAscension);
        }
        return tempNum;
    }
    //現在のPrestigeレベルでの数
    public long ClearedMissionNum(long prestigeLevel, bool thisAscension = false)
    {
        long tempNum = 0;
        for (int i = 0; i < missionListArray[prestigeLevel].Count; i++)
        {
            if (thisAscension)
            {
                if (missionListArray[prestigeLevel][i].isClearedThisAscension) tempNum++;
            }
            else
            {
                if (missionListArray[prestigeLevel][i].isCleared) tempNum++;
            }
        }
        return tempNum;
    }

    //実際に次のWaveへ行く処理
    public async void GenerateWave(int waveNum, BATTLE_CONTROLLER battleCtrl)
    {
        //Debug.Log("challenge" + isChallenge);
        double tempDifficultyIncrement = 0;
        if (isDungeon && waveNum >= 100)
        {
            tempDifficultyIncrement = (waveNum - 100) / 10d;
            if (waveNum % 100 == 0) await SelectBlessing(battleCtrl);
        }
        if (waveNum >= wave.Count)
        {
            if (waveNum % 100 == 99) wave[99].Generate(battleCtrl);//Dungeonのボス
            else wave[UnityEngine.Random.Range(0, Math.Min(wave.Count, 99))].Generate(battleCtrl, tempDifficultyIncrement);
        }
        else wave[waveNum].Generate(battleCtrl, tempDifficultyIncrement);
    }
    public async Task SelectBlessing(BATTLE_CONTROLLER battleCtrl)
    {
        if (battleCtrl.isSimulated) return;
        battleCtrl.isPaused = true;
        if (game.IsUI(battleCtrl.heroKind) && battleCtrl.blessingCtrl.selectBlessingTask != null) await battleCtrl.blessingCtrl.selectBlessingTask();
        battleCtrl.isPaused = false;
    }

    public List<WaveGenerater> wave = new List<WaveGenerater>();


    public AreaController areaCtrl;
    //AreaKind Area Id-Level (例: Slime Village Area 1 - 0)
    public AreaKind kind;
    public int id;
    public AreaLevel level;//AreaPretigeによるもの
    int saveId { get => id + AreaParameter.firstLevelIdForSave * (int)level.value + Convert.ToInt32(isDungeon) * AreaParameter.firstDungeonIdForSave; }
    public virtual long minLevel => 1;
    public virtual long MinLevel() { return minLevel + 5 * level.value * (level.value + 4); }
    public virtual long maxLevel => 1;
    public virtual long MaxLevel() { return maxLevel + 5 * level.value * (level.value + 4); }//+25,+60,+105,+160,+225,...,+585
    public double difficulty = 0d;
    public static int defaultMaxWaveNum = 10;
    public static int defaultMaxWaveNumDungeon = 100;
    public Dictionary<int, long> requiredCompleteNum = new Dictionary<int, long>();
    public AreaCompleteNum completedNum;
    public double nextMilestoneCompletedNum { get => completedNum.Milestone(completedNum.currentMilestoneId + 1); }
    public float bestTime
    {
        get
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    return main.SR.areaBestTimesSlime[saveId];
                case AreaKind.MagicSlimeCity:
                    return main.SR.areaBestTimesMagicSlime[saveId];
                case AreaKind.SpiderMaze:
                    return main.SR.areaBestTimesSpider[saveId];
                case AreaKind.BatCave:
                    return main.SR.areaBestTimesBat[saveId];
                case AreaKind.FairyGarden:
                    return main.SR.areaBestTimesFairy[saveId];
                case AreaKind.FoxShrine:
                    return main.SR.areaBestTimesFox[saveId];
                case AreaKind.DevilFishLake:
                    return main.SR.areaBestTimesDevilFish[saveId];
                case AreaKind.TreantDarkForest:
                    return main.SR.areaBestTimesTreant[saveId];
                case AreaKind.FlameTigerVolcano:
                    return main.SR.areaBestTimesFlameTiger[saveId];
                case AreaKind.UnicornIsland:
                    return main.SR.areaBestTimesUnicorn[saveId];
                default:
                    return 0;
            }
        }
        set
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    main.SR.areaBestTimesSlime[saveId] = value; break;
                case AreaKind.MagicSlimeCity:
                    main.SR.areaBestTimesMagicSlime[saveId] = value; break;
                case AreaKind.SpiderMaze:
                    main.SR.areaBestTimesSpider[saveId] = value; break;
                case AreaKind.BatCave:
                    main.SR.areaBestTimesBat[saveId] = value; break;
                case AreaKind.FairyGarden:
                    main.SR.areaBestTimesFairy[saveId] = value; break;
                case AreaKind.FoxShrine:
                    main.SR.areaBestTimesFox[saveId] = value; break;
                case AreaKind.DevilFishLake:
                    main.SR.areaBestTimesDevilFish[saveId] = value; break;
                case AreaKind.TreantDarkForest:
                    main.SR.areaBestTimesTreant[saveId] = value; break;
                case AreaKind.FlameTigerVolcano:
                    main.SR.areaBestTimesFlameTiger[saveId] = value; break;
                case AreaKind.UnicornIsland:
                    main.SR.areaBestTimesUnicorn[saveId] = value; break;
            }
        }
    }
    public double bestGold
    {
        get
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    return main.SR.areaBestGoldsSlime[saveId];
                case AreaKind.MagicSlimeCity:
                    return main.SR.areaBestGoldsMagicSlime[saveId];
                case AreaKind.SpiderMaze:
                    return main.SR.areaBestGoldsSpider[saveId];
                case AreaKind.BatCave:
                    return main.SR.areaBestGoldsBat[saveId];
                case AreaKind.FairyGarden:
                    return main.SR.areaBestGoldsFairy[saveId];
                case AreaKind.FoxShrine:
                    return main.SR.areaBestGoldsFox[saveId];
                case AreaKind.DevilFishLake:
                    return main.SR.areaBestGoldsDevilFish[saveId];
                case AreaKind.TreantDarkForest:
                    return main.SR.areaBestGoldsTreant[saveId];
                case AreaKind.FlameTigerVolcano:
                    return main.SR.areaBestGoldsFlameTiger[saveId];
                case AreaKind.UnicornIsland:
                    return main.SR.areaBestGoldsUnicorn[saveId];
                default:
                    return 0;
            }
        }
        set
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    main.SR.areaBestGoldsSlime[saveId] = value; break;
                case AreaKind.MagicSlimeCity:
                    main.SR.areaBestGoldsMagicSlime[saveId] = value; break;
                case AreaKind.SpiderMaze:
                    main.SR.areaBestGoldsSpider[saveId] = value; break;
                case AreaKind.BatCave:
                    main.SR.areaBestGoldsBat[saveId] = value; break;
                case AreaKind.FairyGarden:
                    main.SR.areaBestGoldsFairy[saveId] = value; break;
                case AreaKind.FoxShrine:
                    main.SR.areaBestGoldsFox[saveId] = value; break;
                case AreaKind.DevilFishLake:
                    main.SR.areaBestGoldsDevilFish[saveId] = value; break;
                case AreaKind.TreantDarkForest:
                    main.SR.areaBestGoldsTreant[saveId] = value; break;
                case AreaKind.FlameTigerVolcano:
                    main.SR.areaBestGoldsFlameTiger[saveId] = value; break;
                case AreaKind.UnicornIsland:
                    main.SR.areaBestGoldsUnicorn[saveId] = value; break;
            }
        }
    }
    public double bestExp
    {
        get
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    return main.SR.areaBestExpsSlime[saveId];
                case AreaKind.MagicSlimeCity:
                    return main.SR.areaBestExpsMagicSlime[saveId];
                case AreaKind.SpiderMaze:
                    return main.SR.areaBestExpsSpider[saveId];
                case AreaKind.BatCave:
                    return main.SR.areaBestExpsBat[saveId];
                case AreaKind.FairyGarden:
                    return main.SR.areaBestExpsFairy[saveId];
                case AreaKind.FoxShrine:
                    return main.SR.areaBestExpsFox[saveId];
                case AreaKind.DevilFishLake:
                    return main.SR.areaBestExpsDevilFish[saveId];
                case AreaKind.TreantDarkForest:
                    return main.SR.areaBestExpsTreant[saveId];
                case AreaKind.FlameTigerVolcano:
                    return main.SR.areaBestExpsFlameTiger[saveId];
                case AreaKind.UnicornIsland:
                    return main.SR.areaBestExpsUnicorn[saveId];
                default:
                    return 0;
            }
        }
        set
        {
            switch (kind)
            {
                case AreaKind.SlimeVillage:
                    main.SR.areaBestExpsSlime[saveId] = value; break;
                case AreaKind.MagicSlimeCity:
                    main.SR.areaBestExpsMagicSlime[saveId] = value; break;
                case AreaKind.SpiderMaze:
                    main.SR.areaBestExpsSpider[saveId] = value; break;
                case AreaKind.BatCave:
                    main.SR.areaBestExpsBat[saveId] = value; break;
                case AreaKind.FairyGarden:
                    main.SR.areaBestExpsFairy[saveId] = value; break;
                case AreaKind.FoxShrine:
                    main.SR.areaBestExpsFox[saveId] = value; break;
                case AreaKind.DevilFishLake:
                    main.SR.areaBestExpsDevilFish[saveId] = value; break;
                case AreaKind.TreantDarkForest:
                    main.SR.areaBestExpsTreant[saveId] = value; break;
                case AreaKind.FlameTigerVolcano:
                    main.SR.areaBestExpsFlameTiger[saveId] = value; break;
                case AreaKind.UnicornIsland:
                    main.SR.areaBestExpsUnicorn[saveId] = value; break;
            }
        }
    }
    public bool[][] isSpawnMonsters = new bool[Enum.GetNames(typeof(MonsterSpecies)).Length][];//[MonsterSpecies][MonsterColor]

    //AreaPrestige
    public AreaPrestige prestige;
    public Multiplier maxAreaLevel;
    public Multiplier missionUnlockedNum;
    public Multiplier clearCountBonus;
    public Multiplier decreaseMaxWaveNum;
    public Multiplier chestDropChance;
    public Multiplier addLimitTime;
    public Multiplier metalSlimeChance;
    public Multiplier portalOrbReduceFactor;

    public double ChestDropChance()
    {
        if (isDungeon) return chestDropChance.Value();
        return 0;
    }
    public double MetalSlimeChance()
    {
        if (isDungeon) return metalSlimeChance.Value();
        return 0;
    }

    //AreaDebuff
    public double[] debuffElement = new double[Enum.GetNames(typeof(Element)).Length];
    public double debuffPhyCrit, debuffMagCrit;

    //UniqueEQDrop
    public EquipmentKind uniqueEquipmentKind;
    public Multiplier uniqueEquipmentDropChance;

    //Mission
    public List<MISSION>[] missionListArray = new List<MISSION>[10];//PrestigeLevel0~9まで

    void SetAreaMastery()
    {
        uniqueEquipmentDropChance = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => EquipmentParameter.areaUniqueDropChanceBase * (1 + level.value)));
    }
    public void SetMonsterSpawns()
    {
        for (int i = 0; i < Enum.GetNames(typeof(MonsterSpecies)).Length; i++)
        {
            isSpawnMonsters[i] = new bool[Enum.GetNames(typeof(MonsterColor)).Length];
        }
        for (int i = 0; i < wave.Count; i++)
        {
            int count = i;
            if (wave[count].isFixedColor) isSpawnMonsters[(int)wave[count].species][(int)wave[count].color] = true;
            else
            {
                switch (wave[count].rarity)
                {
                    case MonsterRarity.Normal:
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Normal] = true;
                        break;
                    case MonsterRarity.Common:
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Normal] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Blue] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Yellow] = true;
                        break;
                    case MonsterRarity.Uncommon:
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Blue] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Yellow] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Red] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Green] = true;
                        break;
                    case MonsterRarity.Rare:
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Red] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Green] = true;
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Purple] = true;
                        break;
                    case MonsterRarity.Boss:
                        isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Boss] = true;
                        break;
                }
            }
            if (isDungeon) isSpawnMonsters[(int)wave[count].species][(int)MonsterColor.Metal] = true;
        }
    }

    public bool IsUnlocked()
    {
        //if (!areaCtrl.IsUnlocked(kind)) return false;
        foreach (var item in requiredCompleteNum)
        {
            if (areaCtrl.Area(kind, item.Key).completedNum.TotalCompletedNum() < item.Value) return false;
        }
        return true;
    }
    public int MaxWaveNum()
    {
        if (isChallenge)
        {
            return wave.Count;
        }
        if (isDungeon) return defaultMaxWaveNumDungeon + Math.Max(0, ((int)level.value + 1) * (int)level.value * 50);
        return defaultMaxWaveNum + Math.Max(0, (int)level.value * 20 - (int)decreaseMaxWaveNum.Value());
    }

    public virtual void SetAllWave()
    {
        //重い場合はランダム要素をなくす
        SetWave();
        //if (isDungeon) return;
        //while (true)
        //{
        //    if (wave.Count >= defaultMaxWaveNum + AreaParameter.maxPrestigeLevel * 20) break;
        //    wave.Add(wave[UnityEngine.Random.Range(0, wave.Count)]);
        //}
    }
    public virtual void SetWave()
    {
        for (int i = 0; i < defaultMaxWaveNum; i++)
        {
            wave.Add(DefaultWave(1 + i));
        }
    }
    public virtual void SetMission()
    {
        for (int i = 0; i < missionListArray.Length; i++)
        {
            int prestigeLevel = i;
            missionListArray[i].Add(new Mission_SaveHp(this, prestigeLevel, 0, 0.90d));
            missionListArray[i].Add(new Mission_WithinTime(this, prestigeLevel, 1, 10));
            missionListArray[i].Add(new Mission_NoEQ(this, prestigeLevel, 2));
            missionListArray[i].Add(new Mission_OnlyBase(this, prestigeLevel, 3));
            missionListArray[i].Add(new Mission_WithinTime(this, prestigeLevel, 4, 3));//これはあとで一番難しいやつに変更
            //missionListArray[i].Add(new Mission_Gold(this, prestigeLevel, 3, () => MaxWaveNum() * 100));
            //missionListArray[i].Add(new Mission_Exp(this, prestigeLevel, 4, () => MaxWaveNum() * 100));
        }
    }

    public long RandomLevel()
    {
        return UnityEngine.Random.Range((int)MinLevel(), (int)MaxLevel() + 1);
    }
    public WaveGenerater DefaultWave(int spawnNum)
    {
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, defaultRarity, RandomLevel, () => difficulty);
        tempWave.SetPositionDefault(spawnNum);
        return tempWave;
    }
    public WaveGenerater DefaultWave(int spawnNum, MonsterRarity rarity, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, rarity, level, difficulty);
        tempWave.SetPositionDefault(spawnNum);
        return tempWave;
    }
    public WaveGenerater DefaultWave(int spawnNum, MonsterColor color, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, color, level, difficulty);
        tempWave.SetPositionDefault(spawnNum);
        return tempWave;
    }
    public WaveGenerater DefaultWave(int spawnNum, MonsterSpecies species, MonsterRarity rarity, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, species, rarity, level, difficulty);
        tempWave.SetPositionDefault(spawnNum);
        return tempWave;
    }
    public WaveGenerater DefaultWave(int spawnNum, MonsterSpecies species, MonsterColor color, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, species, color, level, difficulty);
        tempWave.SetPositionDefault(spawnNum);
        return tempWave;
    }


    public WaveGenerater RandomWave(int spawnNum)
    {
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, defaultRarity, RandomLevel, () => difficulty);
        tempWave.SetPositionRandom(spawnNum);
        return tempWave;
    }
    public WaveGenerater RandomWave(int spawnNum, MonsterRarity rarity, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, rarity, level, difficulty);
        tempWave.SetPositionRandom(spawnNum);
        return tempWave;
    }
    public WaveGenerater RandomWave(int spawnNum, MonsterColor color, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, (MonsterSpecies)kind, color, level, difficulty);
        tempWave.SetPositionRandom(spawnNum);
        return tempWave;
    }
    public WaveGenerater RandomWave(int spawnNum, MonsterSpecies species, MonsterRarity rarity, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, species, rarity, level, difficulty);
        tempWave.SetPositionRandom(spawnNum);
        return tempWave;
    }
    public WaveGenerater RandomWave(int spawnNum, MonsterSpecies species, MonsterColor color, Func<long> level = null, Func<double> difficulty = null)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, species, color, level, difficulty);
        tempWave.SetPositionRandom(spawnNum);
        return tempWave;
    }

    public WaveGenerater ChallengeWave(ChallengeMonsterKind kind, Func<long> level = null, Func<double> difficulty = null, float positionX = 0, float positionY = 0.5f)
    {
        if (level == null) level = RandomLevel;
        if (difficulty == null) difficulty = () => 0;
        var tempWave = new WaveGenerater(this, kind, level, difficulty);
        tempWave.SetPositionFixed(positionX, positionY);
        return tempWave;
    }

    public virtual MonsterRarity defaultRarity { get => AreaParameter.DefaultRarity(id); }

    //UI
    public virtual string Name(bool isRegion = true, bool isLevel = true, bool isShort = false)
    {
        string tempStr = optStr;
        if (isRegion) tempStr = optStr + Localized.localized.AreaName(kind) + " : ";
        if (isDungeon)
        {
            if (isShort)
                tempStr += "Dung" + (1 + id).ToString();
            else
                tempStr += Localized.localized.Basic(BasicWord.Dungeon) + " " + (1 + id).ToString();
        }
        else tempStr += optStr + Localized.localized.Basic(BasicWord.Area) + " " + (1 + id).ToString();
        if (main.S.ascensionNum[0] > 0 && isLevel) tempStr += " - " + (1 + level.value).ToString();
        return tempStr;
    }

    //Simulation
    public bool isSimulated;//Simulationが終了したかどうか
    public bool simulatedIsClear;
    public float simulatedTime;
    public double simulatedExp;
    public double simulatedGold;
    public double simulatedExpPerSec
    {
        get
        {
            //if (isDungeon) return (simulatedExp + RewardExp()) / simulatedTime;
            return simulatedExp / simulatedTime;
        }
    }
    public double simulatedGoldPerSec => simulatedGold / simulatedTime;
    public int simulatedWave;
    public void InitializeSimulation()
    {
        isSimulated = false;
        simulatedIsClear = false;
        simulatedTime = 0;
        simulatedExp = 0;
        simulatedGold = 0;
    }
    public void RegisterSimulation(AREA_BATTLE areaBattle)
    {
        simulatedIsClear = areaBattle.simulatedIsClear;
        simulatedTime = areaBattle.simulatedTime;
        simulatedExp = areaBattle.simulatedExp + RewardExp() * game.statsCtrl.HeroStats(areaBattle.battleCtrl.heroKind, Stats.ExpGain).Value() + game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue();
        simulatedGold = areaBattle.simulatedGold + RewardGold() * game.statsCtrl.GoldGain().Value() + game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();
        simulatedWave = areaBattle.currentWave;
        isSimulated = true;
    }
}

public class AreaLevel : INTEGER
{
    public AreaLevel(AreaKind areaKind, int id, bool isDungeon, Func<long> maxValue)
    {
        this.areaKind = areaKind;
        this.id = id;
        this.isDungeon = isDungeon;
        this.maxValue = maxValue;
    }
    AreaKind areaKind;
    int id;
    int saveId { get => id + Convert.ToInt32(isDungeon) * 10; }
    bool isDungeon;
    public override long value
    {
        get
        {
            switch (areaKind)
            {
                case AreaKind.SlimeVillage:
                    return main.SR.currentAreaLevelsSlime[saveId];
                case AreaKind.MagicSlimeCity:
                    return main.SR.currentAreaLevelsMagicSlime[saveId];
                case AreaKind.SpiderMaze:
                    return main.SR.currentAreaLevelsSpider[saveId];
                case AreaKind.BatCave:
                    return main.SR.currentAreaLevelsBat[saveId];
                case AreaKind.FairyGarden:
                    return main.SR.currentAreaLevelsFairy[saveId];
                case AreaKind.FoxShrine:
                    return main.SR.currentAreaLevelsFox[saveId];
                case AreaKind.DevilFishLake:
                    return main.SR.currentAreaLevelsDevilFish[saveId];
                case AreaKind.TreantDarkForest:
                    return main.SR.currentAreaLevelsTreant[saveId];
                case AreaKind.FlameTigerVolcano:
                    return main.SR.currentAreaLevelsFlameTiger[saveId];
                case AreaKind.UnicornIsland:
                    return main.SR.currentAreaLevelsUnicorn[saveId];
                default:
                    return main.SR.currentAreaLevelsSlime[saveId];
            }
        }
        set
        {
            switch (areaKind)
            {
                case AreaKind.SlimeVillage:
                    main.SR.currentAreaLevelsSlime[saveId] = value;
                    break;
                case AreaKind.MagicSlimeCity:
                    main.SR.currentAreaLevelsMagicSlime[saveId] = value;
                    break;
                case AreaKind.SpiderMaze:
                    main.SR.currentAreaLevelsSpider[saveId] = value;
                    break;
                case AreaKind.BatCave:
                    main.SR.currentAreaLevelsBat[saveId] = value;
                    break;
                case AreaKind.FairyGarden:
                    main.SR.currentAreaLevelsFairy[saveId] = value;
                    break;
                case AreaKind.FoxShrine:
                    main.SR.currentAreaLevelsFox[saveId] = value;
                    break;
                case AreaKind.DevilFishLake:
                    main.SR.currentAreaLevelsDevilFish[saveId] = value;
                    break;
                case AreaKind.TreantDarkForest:
                    main.SR.currentAreaLevelsTreant[saveId] = value;
                    break;
                case AreaKind.FlameTigerVolcano:
                    main.SR.currentAreaLevelsFlameTiger[saveId] = value;
                    break;
                case AreaKind.UnicornIsland:
                    main.SR.currentAreaLevelsUnicorn[saveId] = value;
                    break;
            }
        }
    }
}

public class AreaCompleteNum : NUMBER
{
    public AreaCompleteNum(AREA area, AreaKind areaKind, int id, AreaLevel level, bool isDungeon)
    {
        this.area = area;
        this.areaKind = areaKind;
        this.id = id;
        this.level = level;
        this.isDungeon = isDungeon;
        //SetMasteryEffect();
    }

    void SetMasteryEffect()
    {
        //area.uniqueEquipmentDropChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaMastery, MultiplierType.Add, () => EquipmentParameter.areaUniqueDropChanceBase * currentMilestoneId));
        //area.expBonus.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaMastery, MultiplierType.Add, () => 0.05d * currentMilestoneId));
        //area.moveSpeedBonus.RegisterMultiplier(new MultiplierInfo(MultiplierKind.AreaMastery, MultiplierType.Add, () => 0.01d * currentMilestoneId));
    }

    public override void Increase(double increment)
    {
        int tempMilestoneId = currentMilestoneId;
        ////ここにAreaCompleteNumの倍率をかく
        //increment += area.clearCountBonus.Value();
        base.Increase(increment);
        //AreaPrestigePoint
        int milestoneIdIncrement = currentMilestoneId - tempMilestoneId;
        if (milestoneIdIncrement > 0) area.prestige.point.Increase(milestoneIdIncrement);
    }
    public int currentMilestoneId { get => NextMilestoneId() - 1; }
    int NextMilestoneId()
    {
        int id = 0;
        while (true)
        {
            if (value < Milestone(id)) break;
            id++;
        }
        return id;
    }
    public double Milestone(int id)
    {
        if (area.isDungeon) return 0.5d * id * (id + 1) * 10;//10,30,60,100,150,210,
        return 0.5d * id * (id + 1) * 500;//500,1500,3000,5000,7500,15000,
    }
    AREA area;// { get { if (isDungeon) return areaCtrl.Dungeon(areaKind, id); return game.areaCtrl.Area(areaKind, id); } }

    AreaKind areaKind;
    int id;
    AreaLevel level;
    public double TotalCompletedNum()
    {
        double tempNum = 0;
        for (int i = 0; i <= level.maxValue(); i++)
        {
            tempNum += SaveArray()[id + AreaParameter.firstLevelIdForSave * i + Convert.ToInt32(isDungeon) * AreaParameter.firstDungeonIdForSave];
        }
        return tempNum;
    }
    int saveId { get => id + AreaParameter.firstLevelIdForSave * (int)level.value + Convert.ToInt32(isDungeon) * AreaParameter.firstDungeonIdForSave; }
    bool isDungeon;
    double[] SaveArray()
    {
        switch (areaKind)
        {
            case AreaKind.SlimeVillage:
                return main.SR.areaCompleteNumsSlime;
            case AreaKind.MagicSlimeCity:
                return main.SR.areaCompleteNumsMagicSlime;
            case AreaKind.SpiderMaze:
                return main.SR.areaCompleteNumsSpider;
            case AreaKind.BatCave:
                return main.SR.areaCompleteNumsBat;
            case AreaKind.FairyGarden:
                return main.SR.areaCompleteNumsFairy;
            case AreaKind.FoxShrine:
                return main.SR.areaCompleteNumsFox;
            case AreaKind.DevilFishLake:
                return main.SR.areaCompleteNumsDevilFish;
            case AreaKind.TreantDarkForest:
                return main.SR.areaCompleteNumsTreant;
            case AreaKind.FlameTigerVolcano:
                return main.SR.areaCompleteNumsFlameTiger;
            case AreaKind.UnicornIsland:
                return main.SR.areaCompleteNumsUnicorn;
            default:
                return main.SR.areaCompleteNumsSlime;
        }
    }
    bool[] SaveArrayForFirstReward()
    {
        switch (areaKind)
        {
            case AreaKind.SlimeVillage:
                return main.SR.areaIsReceivedFirstRewardSlime;
            case AreaKind.MagicSlimeCity:
                return main.SR.areaIsReceivedFirstRewardMagicSlime;
            case AreaKind.SpiderMaze:
                return main.SR.areaIsReceivedFirstRewardSpider;
            case AreaKind.BatCave:
                return main.SR.areaIsReceivedFirstRewardBat;
            case AreaKind.FairyGarden:
                return main.SR.areaIsReceivedFirstRewardFairy;
            case AreaKind.FoxShrine:
                return main.SR.areaIsReceivedFirstRewardFox;
            case AreaKind.DevilFishLake:
                return main.SR.areaIsReceivedFirstRewardDevilFish;
            case AreaKind.TreantDarkForest:
                return main.SR.areaIsReceivedFirstRewardTreant;
            case AreaKind.FlameTigerVolcano:
                return main.SR.areaIsReceivedFirstRewardFlameTiger;
            case AreaKind.UnicornIsland:
                return main.SR.areaIsReceivedFirstRewardUnicorn;
            default:
                return main.SR.areaIsReceivedFirstRewardSlime;
        }
    }
    public override double value { get => SaveArray()[saveId]; set => SaveArray()[saveId] = value; }
    public bool isReceivedFirstReward { get => SaveArrayForFirstReward()[saveId]; set => SaveArrayForFirstReward()[saveId] = value; }
}
public class WaveGenerater//Waveの管理
{
    AREA area;
    public MonsterSpecies species;
    public MonsterRarity rarity;
    public MonsterColor color;//カラーが設定されている時はrarityに関係なく固定カラーにする
    public ChallengeMonsterKind challengeMonsterKind;
    Func<long> level;
    Func<double> difficulty;
    List<Vector2> spawnPosition = new List<Vector2>();
    public bool isFixedColor;
    public bool isChallenge;
    Lottery monsterLottery;
    public WaveGenerater(AREA area, MonsterSpecies species, MonsterRarity rarity, Func<long> level, Func<double> difficulty)
    {
        this.area = area;
        this.species = species;
        this.rarity = rarity;
        this.level = level;
        this.difficulty = difficulty;
        monsterLottery = new Lottery(AreaParameter.monsterColorRate[(int)rarity]);
    }
    //カラー一色固定の場合はこっちを呼ぶ
    public WaveGenerater(AREA area, MonsterSpecies species, MonsterColor color, Func<long> level, Func<double> difficulty)
    {
        this.area = area;
        this.species = species;
        this.color = color;
        this.level = level;
        this.difficulty = difficulty;
        isFixedColor = true;
    }
    public WaveGenerater(AREA area, ChallengeMonsterKind challengeMonsterKind, Func<long> level, Func<double> difficulty)
    {
        this.area = area;
        this.challengeMonsterKind = challengeMonsterKind;
        this.level = level;
        this.difficulty = difficulty;
        isChallenge = true;
    }


    List<MonsterSpecies> fixedSpecies = new List<MonsterSpecies>();
    List<MonsterColor> fixedColor = new List<MonsterColor>();
    List<Vector2> fixedPosition = new List<Vector2>();
    List<Func<long>> fixedLevel = new List<Func<long>>();
    List<Func<double>> fixedDifficulty = new List<Func<double>>();
    public void SetPositionFixedMonster(MonsterSpecies species, MonsterColor color, float normalizedPositionX, float normalizedPositionY, Func<long> level = null, Func<double> difficulty = null)
    {
        fixedSpecies.Add(species);
        fixedColor.Add(color);
        fixedPosition.Add(SpawnPosition(normalizedPositionX, normalizedPositionY));
        if (level == null) fixedLevel.Add(this.level);
        else fixedLevel.Add(level);
        if (difficulty == null) fixedDifficulty.Add(this.difficulty);
        else fixedDifficulty.Add(difficulty);
    }

    public void SetPositionRandom(int spawnNum)
    {
        spawnPosition.AddRange(RandomSpawnPosition(spawnNum));
    }

    public void SetPositionFixed(float normalizedPositionX, float normalizedPositionY)
    {
        spawnPosition.Add(SpawnPosition(normalizedPositionX, normalizedPositionY));
    }

    public void SetPositionFormation(float normalizedPositionX, float normalizedPositionY, SpawnFormation formation, int length = 3)
    {
        spawnPosition.AddRange(SpawnPosition(normalizedPositionX, normalizedPositionY, formation, length));
    }

    public void SetPositionDefault(int spawnNum)
    {
        spawnPosition.AddRange(SpawnPositionDefault(spawnNum));
    }


    //ここで実際にSpawnさせる
    int spawnCount = 0;
    int tempTotalSpawnCount = 0;
    Vector2 tempVec;
    bool tempIsMutant;
    public void Generate(BATTLE_CONTROLLER battleCtrl, double difficultyIncrement = 0)
    {
        if (isChallenge)
        {
            battleCtrl.areaBattle.SpawnMonster(challengeMonsterKind, level(), difficulty() + difficultyIncrement, spawnPosition[0]);
            return;
        }
        spawnCount = 0;
        tempTotalSpawnCount = 0;
        for (int i = 0; i < fixedSpecies.Count; i++)
        {
            battleCtrl.areaBattle.SpawnMonster(fixedSpecies[i], fixedColor[i], fixedLevel[i](), fixedDifficulty[i]() + difficultyIncrement, fixedPosition[i], false);
            spawnCount++;
            tempTotalSpawnCount++;
        }
        if (isSwarm)
        {
            for (int i = 0; i < spawnCount * (spawnNumFactor - 1); i++)
            {
                tempVec = 100f * AreaController.spawnPosition[UnityEngine.Random.Range(0, AreaController.spawnPosition.Count)];
                if (WithinRandom(mutantSpawnChance)) tempIsMutant = true;
                else tempIsMutant = false;
                battleCtrl.areaBattle.SpawnMonster(fixedSpecies[i], fixedColor[i], fixedLevel[i](), fixedDifficulty[i]() + difficultyIncrement, tempVec, tempIsMutant);
                tempTotalSpawnCount++;
            }
        }
        //この書き方だと、同じカラーのみが出現する（メタルも複数でる）ため、アウト。Delegateにする必要がある
        //if (isFixedColor)
        //{
        //    if (color != MonsterColor.Boss && WithinRandom(area.MetalSlimeChance()))
        //        tempColor = MonsterColor.Metal;
        //    else tempColor = color;
        //}
        //else tempColor = Color();

        spawnCount = 0;
        for (int i = 0; i < spawnPosition.Count; i++)
        {
            battleCtrl.areaBattle.SpawnMonster(species, RolledMonsterColor(), level(), difficulty() + difficultyIncrement, spawnPosition[i], false);
            spawnCount++;
            tempTotalSpawnCount++;
        }
        if (!isSwarm) return;
        for (int i = 0; i < spawnCount * (spawnNumFactor-1); i++)
        {
            tempVec = 100f * AreaController.spawnPosition[UnityEngine.Random.Range(0, AreaController.spawnPosition.Count)];
            if (WithinRandom(mutantSpawnChance)) tempIsMutant = true;
            else tempIsMutant = false;
            battleCtrl.areaBattle.SpawnMonster(species, RolledMonsterColor(), level(), difficulty() + difficultyIncrement, tempVec, tempIsMutant);
            tempTotalSpawnCount++;
        }

        //Swarm時にTotalSpawnCountがMinimumSpawn#を満たない時はその分をNormalのMonsterで補う
        for (int i = 0; i < area.swarm.minimumSpawnNum - tempTotalSpawnCount; i++)
        {
            tempVec = 100f * AreaController.spawnPosition[UnityEngine.Random.Range(0, AreaController.spawnPosition.Count)];
            if (WithinRandom(mutantSpawnChance)) tempIsMutant = true;
            else tempIsMutant = false;
            battleCtrl.areaBattle.SpawnMonster(species, MonsterColor.Normal, level(), difficulty() + difficultyIncrement, tempVec, tempIsMutant);
        }
    }
    MonsterColor RolledMonsterColor()
    {
        if (isFixedColor)
        {
            if (color != MonsterColor.Boss && WithinRandom(area.MetalSlimeChance()))
                return MonsterColor.Metal;
            else return color;
        }
        return Color();
    }
    bool isSwarm => area.swarm.isSwarm;
    double mutantSpawnChance => area.swarm.mutantSpawnChance;
    int spawnNumFactor
    {
        get
        {
            if (isSwarm) return area.swarm.spawnNumFactor;
            return 1;
        }
    }

    //以下は素材
    MonsterColor Color()
    {
        if (WithinRandom(area.MetalSlimeChance())) return MonsterColor.Metal;
        return (MonsterColor)monsterLottery.SelectedId();
    }

    Vector2 SpawnPosition(float x, float y)
    {
        float tempX = Mathf.Clamp(100f * x, -Parameter.moveRangeX / 2, Parameter.moveRangeX / 2);
        float tempY = Mathf.Clamp(100f * y, -Parameter.moveRangeX / 2, Parameter.moveRangeX / 2);
        return Vector2.right * tempX + Vector2.up * tempY;
    }

    List<Vector2> RandomSpawnPosition(int spawnNum)
    {
        spawnNum = Mathf.Min(spawnNum, Parameter.maxMonsterSpawnNum);
        List<Vector2> tempPos = new List<Vector2>();
        tempPos.AddRange(AreaController.spawnPosition);
        List<Vector2> selectedPos = new List<Vector2>();
        Vector2 tempVec;
        for (int i = 0; i < spawnNum; i++)
        {
            tempVec = tempPos[UnityEngine.Random.Range(0, tempPos.Count)];
            selectedPos.Add(tempVec * 100f);
            tempPos.Remove(tempVec);
        }
        return selectedPos;
    }

    List<Vector2> SpawnPosition(float centerX, float centerY, SpawnFormation formation = SpawnFormation.Mono, int length = 3)
    {
        List<Vector2> tempPos = new List<Vector2>();
        length = Mathf.Min(Parameter.battleFieldSquareNum, length);
        tempPos.Add(SpawnPosition(centerX, centerY));
        switch (formation)
        {
            case SpawnFormation.Mono:
                break;
            case SpawnFormation.Horizontal:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX + i, centerY));
                    tempPos.Add(SpawnPosition(centerX - i, centerY));
                }
                break;
            case SpawnFormation.Vertical:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX, centerY + i));
                    tempPos.Add(SpawnPosition(centerX, centerY - i));
                }
                break;
            case SpawnFormation.Plus:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX + i, centerY));
                    tempPos.Add(SpawnPosition(centerX - i, centerY));
                    tempPos.Add(SpawnPosition(centerX, centerY + i));
                    tempPos.Add(SpawnPosition(centerX, centerY - i));
                }
                break;
            case SpawnFormation.X:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX + i, centerY + i));
                    tempPos.Add(SpawnPosition(centerX - i, centerY - i));
                    tempPos.Add(SpawnPosition(centerX + i, centerY - i));
                    tempPos.Add(SpawnPosition(centerX - i, centerY + i));
                }
                break;
            case SpawnFormation.Square:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX + i, centerY));
                    tempPos.Add(SpawnPosition(centerX - i, centerY));
                    tempPos.Add(SpawnPosition(centerX, centerY + i));
                    tempPos.Add(SpawnPosition(centerX, centerY - i));
                    for (int j = 1; j < length; j++)
                    {
                        tempPos.Add(SpawnPosition(centerX + j, centerY + i));
                        tempPos.Add(SpawnPosition(centerX - j, centerY - i));
                    }
                }
                break;
            case SpawnFormation.V:
                for (int i = 1; i < length; i++)
                {
                    tempPos.Add(SpawnPosition(centerX + i, centerY + i / 2f));
                    tempPos.Add(SpawnPosition(centerX - i, centerY + i / 2f));
                }
                break;
            case SpawnFormation.Circle:
                tempPos.Add(SpawnPosition(centerX + 2, centerY));
                tempPos.Add(SpawnPosition(centerX - 2, centerY));
                tempPos.Add(SpawnPosition(centerX + 1.73f, centerY + 1f));
                tempPos.Add(SpawnPosition(centerX - 1.73f, centerY + 1f));
                tempPos.Add(SpawnPosition(centerX + 1.73f, centerY - 1f));
                tempPos.Add(SpawnPosition(centerX - 1.73f, centerY - 1f));
                tempPos.Add(SpawnPosition(centerX + 1f, centerY + 1.73f));
                tempPos.Add(SpawnPosition(centerX - 1f, centerY + 1.73f));
                tempPos.Add(SpawnPosition(centerX + 1f, centerY - 1.73f));
                tempPos.Add(SpawnPosition(centerX - 1f, centerY - 1.73f));
                tempPos.Add(SpawnPosition(centerX, centerY + 2));
                tempPos.Add(SpawnPosition(centerX, centerY - 2));
                break;
        }
        return tempPos;
    }

    List<Vector2> SpawnPositionDefault(int spawnNum)
    {
        var tempPos = new List<Vector2>();
        switch (spawnNum)
        {
            case 1:
                tempPos.Add(SpawnPosition(0, 3));
                break;
            case 2:
                tempPos.Add(SpawnPosition(2, 2));
                tempPos.Add(SpawnPosition(-2, 2));
                break;
            case 3:
                tempPos.Add(SpawnPosition(0, 3));
                tempPos.Add(SpawnPosition(3, 1));
                tempPos.Add(SpawnPosition(-3, 1));
                break;
            case 4:
                tempPos.Add(SpawnPosition(1, 3));
                tempPos.Add(SpawnPosition(-1, 3));
                tempPos.Add(SpawnPosition(-3, 2));
                tempPos.Add(SpawnPosition(3, 2));
                break;
            case 5:
                tempPos.Add(SpawnPosition(0, 2.5f));
                tempPos.Add(SpawnPosition(2, 3));
                tempPos.Add(SpawnPosition(-2, 3));
                tempPos.Add(SpawnPosition(1, 2));
                tempPos.Add(SpawnPosition(-1, 2));
                break;
            case 6:
                tempPos.Add(SpawnPosition(1.5f, 2));
                tempPos.Add(SpawnPosition(-1.5f, 2));
                tempPos.Add(SpawnPosition(3, 3));
                tempPos.Add(SpawnPosition(-3, 3));
                tempPos.Add(SpawnPosition(0, 3));
                tempPos.Add(SpawnPosition(0, 0));
                break;
            case 7:
                tempPos.AddRange(SpawnPosition(2, 1, SpawnFormation.Vertical, 2));
                tempPos.AddRange(SpawnPosition(-2, 1, SpawnFormation.Vertical, 2));
                tempPos.Add(SpawnPosition(0, 1));
                break;
            case 8:
                tempPos.Add(SpawnPosition(0, 2));
                tempPos.AddRange(SpawnPosition(0, 0, SpawnFormation.V, 4));
                break;
            case 9:
                tempPos.AddRange(SpawnPosition(0, 0, SpawnFormation.Plus, 3));
                break;
            case 10:
                tempPos.AddRange(SpawnPosition(0, 2, SpawnFormation.V, 3));
                tempPos.AddRange(SpawnPosition(0, 0, SpawnFormation.V, 3));
                break;
            case 11:
                tempPos.AddRange(SpawnPosition(0, 3, SpawnFormation.Horizontal, 3));
                tempPos.AddRange(SpawnPosition(0, 0, SpawnFormation.V, 3));
                tempPos.Add(SpawnPosition(0, -1));
                break;
            case 12:
                tempPos.AddRange(SpawnPosition(3, 0, SpawnFormation.Vertical, 3));
                tempPos.AddRange(SpawnPosition(-3, 0, SpawnFormation.Vertical, 3));
                tempPos.Add(SpawnPosition(0, 1));
                tempPos.Add(SpawnPosition(0, 3));
                break;
            case 13:
                tempPos.AddRange(SpawnPosition(0, 1, SpawnFormation.Circle, 0));
                break;
            case 14:
                tempPos.AddRange(SpawnPosition(0, 0, SpawnFormation.X, 4));
                tempPos.Add(SpawnPosition(0, 3));
                break;
            default:
                tempPos.AddRange(RandomSpawnPosition(spawnNum));
                break;
        }
        return tempPos;
    }
}
public enum SpawnFormation
{
    Mono,
    Horizontal,
    Vertical,
    Plus,
    X,
    Square,
    V,
    Circle,
}
