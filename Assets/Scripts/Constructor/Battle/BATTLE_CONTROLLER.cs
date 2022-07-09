using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static SpriteSourceUI;
using static MonsterParameter;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

//Battleのシミュレーション用Class
public class BATTLE_CONTROLLER 
{
    public virtual bool isSimulated { get => true; }
    public bool isActiveBattle { get => main.SR.isActiveBattle[(int)heroKind]; set => main.SR.isActiveBattle[(int)heroKind] = value; }
    public bool isTryingRaid;
    public bool isPaused;

    public HeroKind heroKind;
    public HERO_BATTLE hero;
    public List<HeroAlly> heroAllys = new List<HeroAlly>();
    public PET_BATTLE[] pets = new PET_BATTLE[Parameter.maxPetSpawnNum];
    public MONSTER_BATTLE[] monsters = new MONSTER_BATTLE[Parameter.maxMonsterSpawnNum];//petに攻撃されたらpetをターゲットにする
    public CHALLENGE_BATTLE[] challengeMonsters = new CHALLENGE_BATTLE[Enum.GetNames(typeof(ChallengeMonsterKind)).Length];
    public List<BATTLE> heroesList = new List<BATTLE>();
    public List<BATTLE> monstersList = new List<BATTLE>();
    public AREA_BATTLE areaBattle;
    public BlessingController blessingCtrl;
    public SkillSet skillSet;
    public NUMBER totalDamage;
    public float timecount;
    public float timecountForSec;

    //Mission, Challenge用の制限
    public bool isAttemptNoEQ, isAttemptOnlyBase;
    public bool[] isHandicapped = new bool[Enum.GetNames(typeof(ChallengeHandicapKind)).Length];
    public void AttemptMissionNoEQ(AREA area)
    {
        areaBattle.Start(area);
        isAttemptNoEQ = true;
        isEquippedAnyEQ = false;
    }
    public void AttemptMissionOnlyBase(AREA area)
    {
        areaBattle.Start(area);
        isAttemptOnlyBase = true;
        isEquippedAnySkill = false;
        if (game.IsUI(heroKind) && skillSet.equipAction != null) skillSet.equipAction();
    }
    public void StartHandicappedChallenge(AREA challengeArea, List<ChallengeHandicapKind> handicapKindList)
    {
        areaBattle.StartChallenge(challengeArea);
        for (int i = 0; i < handicapKindList.Count; i++)
        {
            isHandicapped[(int)handicapKindList[i]] = true;
        }
        if (game.IsUI(heroKind) && skillSet.equipAction != null) skillSet.equipAction();
    }

    public int limitedEQWeaponNum
    {
        get
        {
            if (isAttemptNoEQ) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.NoEQ]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyArmor]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyJewelry]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Armor]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Jewelry]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Weapon]) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1EQforAllPart]) return 1;
            return 30;
        }
    }
    public int limitedEQArmorNum
    {
        get
        {
            if (isAttemptNoEQ) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.NoEQ]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyWeapon]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyJewelry]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Weapon]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Jewelry]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Armor]) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1EQforAllPart]) return 1;
            return 30;
        }
    }
    public int limitedEQJewelryNum
    {
        get
        {
            if (isAttemptNoEQ) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.NoEQ]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyWeapon]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyArmor]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Weapon]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Armor]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1Jewelry]) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.Only1EQforAllPart]) return 1;
            return 30;
        }
    }
    public int limitedSkillNum
    {
        get
        {
            if (isAttemptOnlyBase) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyBaseAndGlobal]) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyBaseSkill]) return 1;
            if (isHandicapped[(int)ChallengeHandicapKind.Only2ClassSkillAnd1Global]) return 2;
            if (isHandicapped[(int)ChallengeHandicapKind.Only2ClassSkill]) return 2;
            return 10;
        }
    }
    public int limitedGlobalSkillNum
    {
        get
        {
            if (isAttemptOnlyBase) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyClassSkill]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.OnlyBaseSkill]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only2ClassSkill]) return 0;
            if (isHandicapped[(int)ChallengeHandicapKind.Only2ClassSkillAnd1Global]) return 1;
            return 10;
        }
    }


    public BATTLE_CONTROLLER(HeroKind heroKind)
    {
        this.heroKind = heroKind;
        Awake();
        totalDamage = new NUMBER();
    }
    public virtual void Awake()
    {
        hero = new HERO_BATTLE(heroKind, this);
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (heroKind != (HeroKind)i) heroAllys.Add(new HeroAlly((HeroKind)count, this));
        }
        for (int i = 0; i < pets.Length; i++)
        {
            pets[i] = new PET_BATTLE(this);
        }
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = new MONSTER_BATTLE(this);
        }
        InstantiateChallengeMonster();
        AddList();
        //Area
        areaBattle = new AREA_BATTLE(this);
    }
    public void InstantiateChallengeMonster()
    {
        challengeMonsters[(int)ChallengeMonsterKind.SlimeKing] = new Battle_SlimeKing(this);
        challengeMonsters[(int)ChallengeMonsterKind.WindowQueen] = new Battle_WindowQueen(this);
        challengeMonsters[(int)ChallengeMonsterKind.Golem] = new Battle_Golem(this);
    }
    public void AddList()
    {
        heroesList.Add(hero);
        heroesList.AddRange(heroAllys);
        heroesList.AddRange(pets);
        monstersList.AddRange(monsters);
        monstersList.AddRange(challengeMonsters);
    }
    public virtual void Start()//Simulation用
    {
        skillSet = game.battleCtrl.skillSet;
        game.skillCtrl.ResetSimulatedTrigger();
        game.skillCtrl.SetTrigger(this, hero, hero.Target, monstersList);
        hero.Start();
    }

    public float deltaTime = 1 / 60f;
    public void Update()
    {
        deltaTime = Time.deltaTime;
        //deltaTime = 1 / 60f * Time.timeScale;
        if (isPaused) return;
        if (isTryingRaid) return;
        if (!isActiveBattle) return;

        for (int i = 0; i < heroesList.Count; i++)
        {
            heroesList[i].Update(deltaTime);
        }
        for (int i = 0; i < monstersList.Count; i++)
        {
            monstersList[i].Update(deltaTime);
        }
        timecount += deltaTime;
        //タイムリミット
        if (hero.isAlive && areaBattle.CurrentArea().IsTimeOver(this))
            areaBattle.QuitCurrentArea();
        //Potion
        if (hero.isAlive && hero.HpPercent() < 0.5d) game.inventoryCtrl.ConsumePotion(PotionConsumeConditionKind.HpHalf, hero, isSimulated);
        //PerSec
        timecountForSec += deltaTime;
        if (timecountForSec >= 1)
        {
            UpdatePerSec();
            timecountForSec = 0;
            totalDamage.ChangeCountGainperSec();
        }

        UpdateAttack();
    }

    public void UpdateAttack()
    {
        for (int i = 0; i < game.skillCtrl.skillsOneDimensionArray.Length; i++)
        {
            for (int j = 0; j < game.skillCtrl.skillsOneDimensionArray[i].AttackList(this).Count; j++)
            {
                game.skillCtrl.skillsOneDimensionArray[i].AttackList(this)[j].Update(deltaTime);
            }
        }
        //heroAttack
        for (int i = 0; i < heroesList.Count; i++)
        {
            for (int j = 0; j < heroesList[i].attack.Count; j++)
            {
                heroesList[i].attack[j].Update(deltaTime);
            }
        }
        //monsterAttack
        for (int i = 0; i < monstersList.Count; i++)
        {
            for (int j = 0; j < monstersList[i].attack.Count; j++)
            {
                monstersList[i].attack[j].Update(deltaTime);
            }
        }
    }
    public void ResetAttack()
    {
        for (int i = 0; i < game.skillCtrl.skillsOneDimensionArray.Length; i++)
        {
            for (int j = 0; j < game.skillCtrl.skillsOneDimensionArray[i].AttackList(this).Count; j++)
            {
                game.skillCtrl.skillsOneDimensionArray[i].AttackList(this)[j].move.Initialize();
            }
        }
        //heroAttack
        for (int i = 0; i < heroesList.Count; i++)
        {
            for (int j = 0; j < heroesList[i].attack.Count; j++)
            {
                heroesList[i].attack[j].move.Initialize();
            }
        }
        //monsterAttack
        for (int i = 0; i < monstersList.Count; i++)
        {
            for (int j = 0; j < monstersList[i].attack.Count; j++)
            {
                monstersList[i].attack[j].move.Initialize();
            }
        }
    }

    //PerSec
    public void UpdatePerSec()
    {
        for (int i = 0; i < heroesList.Count; i++)
        {
            heroesList[i].UpdatePerSec();
        }
        for (int i = 0; i < monstersList.Count; i++)
        {
            monstersList[i].UpdatePerSec();
        }
        //Debuff from Aura Potion
        for (int i = 0; i < Enum.GetNames(typeof(Debuff)).Length; i++)
        {
            int count = i;
            if (WithinRandom(game.statsCtrl.DebuffChance(heroKind, (Debuff)count).Value()))
            {
                for (int j = 0; j < monstersList.Count; j++)
                {
                    monstersList[j].ReceiveDebuff((Debuff)count, 1);//1の部分は毒ダメージ/sec
                }
            }
        }
    }

    public HeroAlly HeroAlly(HeroKind heroKind)
    {
        for (int i = 0; i < heroAllys.Count; i++)
        {
            if (heroAllys[i].heroKind == heroKind) return heroAllys[i];
        }
        return heroAllys[0];
    }

    //SlayerOil Potion
    public Element CurrentSlayerElement()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Element)).Length; i++)
        {
            int count = i;
            if (game.statsCtrl.ElementSlayerDamage(heroKind, (Element)count).Value() > 0) return (Element)count;
        }
        return Element.Physical;
    }
    //Drop
    public virtual void ResetDrop() { }
    public virtual void ResetChest() { }
    public virtual void GetDropResource() { }
    public virtual void GetDropMaterial() { }
    public virtual void GetDropEquipment() { }
    public virtual void DropResource(ResourceKind kind, double num, Vector2 position) { }
    public virtual void DropMaterial(MaterialKind kind, double num, Vector2 position, bool isColorMaterial = false) { }
    public virtual void DropEquipment(long monsterLevel, HeroKind heroKind, Vector2 position) { }
    public virtual void DropEquipment(EquipmentKind kind, long monsterLevel, HeroKind heroKind, Vector2 position) { }
    public virtual void DropUniqueEquipment(long monsterLevel, HeroKind heroKind, Vector2 position) { }
    public virtual long EquipmentDroppingNum() { return 0; }
    public virtual void DropChest(long monsterLevel, HeroKind heroKind, Vector2 position) { }

    public int MonsterAliveNum()
    {
        int tempNum = 0;
        for (int i = 0; i < monstersList.Count; i++)
        {
            if (monstersList[i].isAlive) tempNum++;
        }
        return tempNum;
    }

    public void InitializeBattle()
    {
        //Attackを全て消す
        for (int i = 0; i < game.skillCtrl.skillsOneDimensionArray.Length; i++)
        {
            for (int j = 0; j < game.skillCtrl.skillsOneDimensionArray[i].AttackList(this).Count; j++)
            {
                game.skillCtrl.skillsOneDimensionArray[i].AttackList(this)[j].move.Initialize();
            }
        }
        //heroAttack
        for (int i = 0; i < heroesList.Count; i++)
        {
            for (int j = 0; j < heroesList[i].attack.Count; j++)
            {
                heroesList[i].attack[j].move.Initialize();
            }
        }
        //monsterAttack
        for (int i = 0; i < monstersList.Count; i++)
        {
            for (int j = 0; j < monstersList[i].attack.Count; j++)
            {
                monstersList[i].attack[j].move.Initialize();
            }
        }
    }

    //Mission用
    public bool isEquippedAnyEQ;
    public bool isEquippedAnySkill;//Baseは除く
    public void InitializeMission()
    {
        isEquippedAnyEQ = false;
        isEquippedAnySkill = false;
        isAttemptNoEQ = false;
        isAttemptOnlyBase = false;
        for (int i = 0; i < isHandicapped.Length; i++)
        {
            isHandicapped[i] = false;
        }
        if (game.IsUI(heroKind) && skillSet.equipAction != null) skillSet.equipAction();
    }
    public void CheckMission()//エリアトライ中にチェックするもの
    {
        if (!isEquippedAnyEQ)
            isEquippedAnyEQ = game.inventoryCtrl.IsEquippedEquipment(heroKind);
        if (!isEquippedAnySkill)
            isEquippedAnySkill = skillSet.IsEquippedAnySkill();
    }
}
public class AREA_BATTLE
{
    public BATTLE_CONTROLLER battleCtrl;
    public AREA currentArea { get => game.areaCtrl.Area(currentAreaKind, currentAreaId); }
    public virtual AreaKind currentAreaKind { get; set; }
    public virtual int currentAreaId { get; set; }
    public AREA currentDungeon;
    public AREA currentChallenge;
    public int currentWave;
    public bool isTringDungeon { get => currentDungeon != game.areaCtrl.nullArea; }
    public bool isTringChallenge { get => currentChallenge != game.areaCtrl.nullArea; }
    public AREA CurrentArea()
    {
        if (isTringChallenge) return currentChallenge;
        if (isTringDungeon) return currentDungeon;
        return currentArea;
    }
    public int CurrentWave()
    {
        return 1 + currentWave;
    }
    public Action<AREA> fieldUIAction;
    public ArrayId spawnId;
    public AREA_BATTLE(BATTLE_CONTROLLER battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        spawnId = new ArrayId(0, () => battleCtrl.monsters.Length);
        currentDungeon = game.areaCtrl.nullArea;
        currentChallenge = game.areaCtrl.nullArea;
    }
    public double areaDebuffFactor => 1 - game.areaCtrl.areaDebuffReduction.Value();

    //MonsterのSpawn
    public void SpawnMonster(MonsterSpecies species, MonsterColor color, long level, double difficulty, Vector2 position, bool isMutant)
    {
        battleCtrl.monsters[spawnId.value].Spawn(species, color, level, difficulty, position, isMutant);
        spawnId.Increase();
    }
    public void SpawnMonster(ChallengeMonsterKind challengeMonsterKind, long level, double difficulty, Vector2 position)
    {
        battleCtrl.challengeMonsters[(int)challengeMonsterKind].Spawn(level, difficulty, position);
    }

    public bool IsAllMonsterDied()
    {
        for (int i = 0; i < battleCtrl.monstersList.Count; i++)
        {
            if (battleCtrl.monstersList[i].isAlive) return false;
        }
        return true;
    }
    public void GenerateWave()
    {
        CurrentArea().GenerateWave(currentWave, battleCtrl);
    }

    //AreaのStart/Quit
    public void Start(AreaKind areaKind, int id)//新規スタート
    {
        if (!game.areaCtrl.Area(areaKind, id).CanStart())
        {
            if (battleCtrl.isSimulated) 
            {
                InitializeSimulatedValue();
                isFinishedSimulation = true;
                game.areaCtrl.Area(areaKind, id).RegisterSimulation(this);
            }
            return;
        }
        Initialize();
        currentAreaKind = areaKind;
        currentAreaId = id;
        Activate();
    }
    public void Start(AREA dungeon)//ダンジョン新規スタート
    {
        if (!dungeon.isDungeon) //通常のエリアを入れた場合
        {
            Start(dungeon.kind, dungeon.id);
            return;
        }
        if (!dungeon.CanStart())
        {
            if (battleCtrl.isSimulated)
            {
                InitializeSimulatedValue();
                isFinishedSimulation = true;
                dungeon.RegisterSimulation(this);
            }
            return;
        }
        Initialize();
        battleCtrl.ResetDrop();
        currentDungeon = dungeon;
        Activate();
    }
    public void StartChallenge(AREA challenge)//チャレンジ新規スタート
    {
        if (!challenge.CanStart())
        {
            if (battleCtrl.isSimulated)
            {
                InitializeSimulatedValue();
                isFinishedSimulation = true;
                challenge.RegisterSimulation(this);
            }
            return;
        }
        Initialize();
        battleCtrl.ResetDrop();
        currentChallenge = challenge;
        Activate();
    }

    public void Start()//現在のAreaをスタート
    {
        Initialize();
        Activate();
    }
    SummonPet summonPet;
    public void Activate()
    {
        CurrentArea().StartAction(battleCtrl, battleCtrl.timecount, battleCtrl.isSimulated);
        battleCtrl.hero.Activate();
        for (int i = 0; i < game.statsCtrl.heroes[(int)battleCtrl.heroKind].summonPetSlot.Value(); i++)
        {
            summonPet = game.statsCtrl.heroes[(int)battleCtrl.heroKind].summonPets[i];
            if (summonPet.isSet)
                battleCtrl.pets[i].Spawn(summonPet.pet.species, summonPet.pet.color, summonPet.pet.Level(), 0, Parameter.petPositions[i]);
        }

        GenerateWave();
        if (game.IsUI(battleCtrl.heroKind) && fieldUIAction != null) fieldUIAction(CurrentArea());
    }
    public void Inactivate()
    {
        for (int i = 0; i < battleCtrl.heroesList.Count; i++)
        {
            battleCtrl.heroesList[i].Inactivate();
        }
        for (int i = 0; i < battleCtrl.monstersList.Count; i++)
        {
            battleCtrl.monstersList[i].Inactivate();
        }
    }
    public virtual void Initialize()
    {
        Inactivate();
        currentDungeon = game.areaCtrl.nullArea;
        currentChallenge = game.areaCtrl.nullArea;
        currentWave = 0;
        isShowingResultPanel = false;
        gold = 0;
        exp = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i] = 0;
        }
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = 0;
        }
        battleCtrl.ResetChest();//Chestはリセットする
        battleCtrl.InitializeBattle();

        if (battleCtrl.isSimulated) InitializeSimulatedValue();
    }
    public virtual void CheckToNextWave()
    {
        //PotionConsume
        game.inventoryCtrl.ConsumePotion(PotionConsumeConditionKind.Defeat, battleCtrl.hero, battleCtrl.isSimulated);
        if (!IsAllMonsterDied()) return;

        //Cleared
        if (CurrentWave() >= CurrentArea().MaxWaveNum())
        {
            Inactivate();
            ClearAction();
        }
        else
        {
            currentWave++;
            GenerateWave();
        }
    }
    //ここにSimulationの結果
    public bool isFinishedSimulation;
    public bool simulatedIsClear;
    public double simulatedExp;
    public double simulatedGold;
    public float simulatedTime;
    void InitializeSimulatedValue()
    {
        isFinishedSimulation = false;
        simulatedIsClear = false;
        simulatedExp = 0;
        simulatedGold = 0;
        simulatedTime = 0;
        currentWave = 0;
    }
    public async virtual void QuitCurrentArea()
    {
        simulatedIsClear = false;
        simulatedTime = CurrentArea().RealElapsedTime(battleCtrl.heroKind, battleCtrl.timecount, battleCtrl.isSimulated);
        CurrentArea().RegisterSimulation(this);
        isFinishedSimulation = true;
    }
    public async virtual void ClearAction()
    {
        simulatedIsClear = true;
        simulatedTime = CurrentArea().RealElapsedTime(battleCtrl.heroKind, battleCtrl.timecount, battleCtrl.isSimulated);
        CurrentArea().RegisterSimulation(this);
        isFinishedSimulation = true;
    }
    //UI
    public Func<Task> failedTask;
    public Func<Task> clearedTask;
    public bool isShowingResultPanel;
    //Score
    public double gold;
    public double exp;
    public double[] resources = new double[Enum.GetNames(typeof(ResourceKind)).Length];
    public double[] materials = new double[Enum.GetNames(typeof(MaterialKind)).Length];
}

public class HERO_BATTLE : BATTLE
{
    public override bool isHero => true;
    public HERO_BATTLE(HeroKind heroKind, BATTLE_CONTROLLER battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        this.heroKind = heroKind;
        move = new Move(initPosition, () => moveSpeed, true, this);
        currentHp = new NUMBER(() => hp);
        currentMp = new NUMBER(() => mp);
        for (int i = 0; i < debuffings.Length; i++)
        {
            int count = i;
            debuffings[i] = new Debuffing(this, (Debuff)count);
        }
    }
    //Simulation
    //Simulationの場合はFieldDebuffを別で考慮する必要がある
    double fieldDebuffPhyCrit => Convert.ToInt16(battleCtrl.isSimulated) * battleCtrl.areaBattle.CurrentArea().debuffPhyCrit;
    double fieldDebuffMagCrit => Convert.ToInt16(battleCtrl.isSimulated) * battleCtrl.areaBattle.CurrentArea().debuffMagCrit;
    double[] fieldDebuffElement { get
        {
            if (battleCtrl.isSimulated) return battleCtrl.areaBattle.CurrentArea().debuffElement;
            return zeroDebuffElements;
        }
    }
    static double[] zeroDebuffElements = new double[] { 0, 0, 0, 0, 0, 0 };

    public virtual void Start() { }
    public virtual Vector2 initPosition => Parameter.heroInitPosition;
    public bool isAutoMove = true;
    public override double hp { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).Value(); }
    public override double mp { get => Math.Max(0, game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).Value() - game.statsCtrl.heroes[(int)heroKind].channeledMp.Value()); }
    public override double atk { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).Value(); }
    public override double matk { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).Value(); }
    public override double def { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).Value(); }
    public override double mdef { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).Value(); }
    public override double spd { get => game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).Value(); }
    public override double fire { get => game.statsCtrl.ElementResistance(heroKind, Element.Fire).Value() + fieldDebuffElement[1] * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double ice { get => game.statsCtrl.ElementResistance(heroKind, Element.Ice).Value() + fieldDebuffElement[2] * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double thunder { get => game.statsCtrl.ElementResistance(heroKind, Element.Thunder).Value() + fieldDebuffElement[3] * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double light { get => game.statsCtrl.ElementResistance(heroKind, Element.Light).Value() + fieldDebuffElement[4] * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double dark { get => game.statsCtrl.ElementResistance(heroKind, Element.Dark).Value() + fieldDebuffElement[5] * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double phyCrit { get => game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).Value() + fieldDebuffPhyCrit * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double magCrit { get => game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).Value() + fieldDebuffMagCrit * battleCtrl.areaBattle.areaDebuffFactor; }
    public override double critDamage { get => 2; }//MonsterからうけるCritは常に2倍
    public override float range { get => game.statsCtrl.heroes[(int)heroKind].combatRange; }
    public override float moveSpeed { get => (float)game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).Value(); }
    public override double debuffResistance { get => game.statsCtrl.HeroStats(heroKind, Stats.DebuffRes).Value(); }
    public override double physicalAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Physical).Value(); }
    public override double fireAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Fire).Value(); }
    public override double iceAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Ice).Value(); }
    public override double thunderAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Thunder).Value(); }
    public override double lightAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Light).Value(); }
    public override double darkAbsorption { get => game.statsCtrl.ElementAbsorption(heroKind, Element.Dark).Value(); }
    public override double physicalInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Physical).Value();
    public override double fireInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Fire).Value();
    public override double iceInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Ice).Value();
    public override double thunderInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Thunder).Value();
    public override double lightInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Light).Value();
    public override double darkInvalidChance => game.statsCtrl.ElementInvalid(heroKind, Element.Dark).Value();
    public override double golemInvalidChanceDamageHpPercent => Math.Min(0.25d, game.statsCtrl.heroes[(int)heroKind].golemInvalidDamageHpPercent.Value());
    public Action initUIAction;

    public override Vector2 MoveDirection()
    {
        if (!isAlive) return Vector2.zero;
        return AliveMoveDirection();
    }
    public Vector2 AliveMoveDirection()
    {
        switch (game.statsCtrl.heroes[(int)heroKind].MovePattern())
        {
            case MovePattern.Kiting:
                return KitingMoveDirection();
        }
        if (IsWithinRange(this, Target(), range)) return Vector2.zero;
        if (!Target().isAlive) return Vector2.zero;
        return Target().move.position - move.position;
    }
    Vector2 KitingMoveDirection()
    {
        if (move.position.y >= 350f && move.position.x < 350f)//上の端にいる場合
            return Vector2.right * 500;
        if (move.position.x <= -350f)//左の端にいる場合
            return Vector2.up * 500;
        if (move.position.y <= -350f)//下の端にいる場合
            return Vector2.left * 500;
        if (move.position.x >= 350f)//右の端にいる場合
            return Vector2.down * 500;
        //端以外にいる場合
        if (move.position.x >= 0 && move.position.y >= 0)//画面の右上にいる場合
            return Vector2.right * 500;
        if (move.position.x >= 0 && move.position.y < 0)//画面の右下にいる場合
            return Vector2.down * 500;
        if (move.position.x < 0 && move.position.y < 0)//画面の左下にいる場合
            return Vector2.left * 500;
        if (move.position.x < 0 && move.position.y >= 0)//画面の左上にいる場合
            return Vector2.up * 500;
        //例外処理（これにはならないはず）
        return Vector2.up;
    }

    public override BATTLE Target()
    {
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }
    public override BATTLE FurthestTarget()
    {
        BATTLE tempBattle = FurthestTarget(this, battleCtrl.monstersList.ToArray());
        if (tempBattle.isAlive) return tempBattle;
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }
    public override BATTLE RandomTarget()
    {
        BATTLE tempBattle = RandomTarget(this, battleCtrl.monstersList.ToArray());
        if (tempBattle.isAlive) return tempBattle;
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }

    public override void UpdateSkillCooltime(float deltaTime)
    {
        for (int i = 0; i < battleCtrl.skillSet.currentEquippingNum; i++)
        {
            if (battleCtrl.skillSet.currentSkillSet[i] != game.skillCtrl.NullSkill() && !battleCtrl.skillSet.currentSkillSet[i].IsFilledCoolttime(this, battleCtrl.isSimulated))
                battleCtrl.skillSet.currentSkillSet[i].CountCooltime(this, deltaTime, battleCtrl.isSimulated);
        }
        for (int i = 0; i < battleCtrl.skillSet.currentGlobalEquippingNum; i++)
        {
            if (battleCtrl.skillSet.currentGlobalSkillSet[i] != game.skillCtrl.NullSkill() && !battleCtrl.skillSet.currentGlobalSkillSet[i].IsFilledCoolttime(this, battleCtrl.isSimulated))
                battleCtrl.skillSet.currentGlobalSkillSet[i].CountCooltime(this, deltaTime, battleCtrl.isSimulated);
        }
    }
    public override void UpdateTriggerSkill()
    {
        for (int i = 0; i < battleCtrl.skillSet.currentEquippingNum; i++)
        {
            if (CanTriggerSkill(battleCtrl.skillSet.currentSkillSet[i]))
            {
                battleCtrl.skillSet.currentSkillSet[i].Trigger(this, currentMp);
            }
        }
        for (int i = 0; i < battleCtrl.skillSet.currentGlobalEquippingNum; i++)
        {
            if (CanTriggerSkill(battleCtrl.skillSet.currentGlobalSkillSet[i]))
            {
                battleCtrl.skillSet.currentGlobalSkillSet[i].Trigger(this, currentMp);
            }
        }
    }
    int tempSkillId;
    SKILL tempSkill;
    public override void TriggerRandomSkill(BATTLE battle, double profGainPercent)
    {
        tempSkillId = UnityEngine.Random.Range(0, 2);//0 or 1
        if (tempSkillId == 0)//classSkillSlot
        {
            tempSkillId = UnityEngine.Random.Range(0, battleCtrl.skillSet.currentEquippingNum);
            tempSkill = battleCtrl.skillSet.currentSkillSet[tempSkillId];
        }
        else//globalSkillSlot
        {
            tempSkillId = UnityEngine.Random.Range(0, battleCtrl.skillSet.currentGlobalEquippingNum);
            tempSkill = battleCtrl.skillSet.currentGlobalSkillSet[tempSkillId];
        }
        if (tempSkill == game.skillCtrl.Skill(HeroKind.Tamer, (int)SkillKindTamer.OdeOfFriendship)) return;
        if (tempSkill == game.skillCtrl.NullSkill()) tempSkill = battleCtrl.skillSet.currentSkillSet[0];
        tempSkill.Trigger(battle, currentMp, true, profGainPercent);
    }
    bool CanTriggerSkill(SKILL skill)
    {
        if (skill == game.skillCtrl.NullSkill()) return false;
        if (!IsWithinRange(this, Target(), skill.Range())) return false;
        if (!skill.IsFilledCoolttime(this, battleCtrl.isSimulated)) return false;
        //Skill Abuse from Epic Store
        if (main.S.isToggleOn[(int)ToggleKind.SkillLessMPAvailable] && currentMp.value > 0)//>= skill.ConsumeMp() * 0.1d)// game.epicStoreCtrl.Item(EpicStoreKind.SkillLessMpAvailable).IsPurchased())
            return true;//SkillAbuseがOnの時はcooltimeに関係なく発動する
        if (currentMp.value < skill.ConsumeMp()) return false;
        return true;
    }
    public override void DeadAction()
    {
        battleCtrl.areaBattle.QuitCurrentArea();
    }
    public override void Regenerate(float deltaTime)
    {
        HpRegenerate(deltaTime);
        MpRegenerate(deltaTime);
    }
    void HpRegenerate(float deltaTime)
    {
        currentHp.Increase(game.statsCtrl.HpRegenerate(heroKind).Value() * deltaTime);
    }
    void MpRegenerate(float deltaTime)
    {
        currentMp.Increase(game.statsCtrl.MpRegenerate(heroKind).Value() * deltaTime);
    }
    public override void Activate()
    {
        if (game.IsUI(battleCtrl.heroKind) && initUIAction != null) initUIAction();
        base.Activate();
    }
    public override void ResetCooltime()
    {
        for (int i = 0; i < battleCtrl.skillSet.currentEquippingNum; i++)
        {
            battleCtrl.skillSet.currentSkillSet[i].ResetCooltime(this, battleCtrl.isSimulated);
        }
        for (int i = 0; i < battleCtrl.skillSet.currentGlobalEquippingNum; i++)
        {
            battleCtrl.skillSet.currentGlobalSkillSet[i].ResetCooltime(this, battleCtrl.isSimulated);
        }
    }
}

public class PET_BATTLE : MONSTER_BATTLE
{
    public override bool isHero => true;
    public PET_BATTLE(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
        heroKind = battleCtrl.heroKind;
        move.isPet = true;
    }
    public override bool isPet { get => true; }
    public override void SetAttack()
    {
        if (attack.Count > 0) return;
        attack.Add(new Attack(battleCtrl, battleCtrl.monstersList, (x) => Target().move.position, (x) => Damage(), IsCrit, () => 30f, () => 1, () => attackElement));
    }
    public override void UpdateTriggerSkill()
    {
        if (CanAttack())
        {
            Attack(game.skillCtrl.Skill(HeroKind.Tamer, (int)SkillKindTamer.SonnetAttack).Damage());//TamerのSonnetAttackの倍率分
            currentMp.ChangeValue(0);
        }
    }
    public override void DeadAction()
    {
        Inactivate();
    }
    public override BATTLE Target()
    {
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }
    public override BATTLE FurthestTarget()
    {
        BATTLE tempBattle = FurthestTarget(this, battleCtrl.monstersList.ToArray());
        if (tempBattle.isAlive) return tempBattle;
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }
    public override BATTLE RandomTarget()
    {
        BATTLE tempBattle = RandomTarget(this, battleCtrl.monstersList.ToArray());
        if (tempBattle.isAlive) return tempBattle;
        return ShortestTarget(this, battleCtrl.monstersList.ToArray());
    }
}