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

public partial class SaveR
{
    public AreaKind[] currentAreaKind;//[hero]
    public int[] currentAreaId;//[hero]
    public bool[] isActiveBattle;
}

public class BattleController : BATTLE_CONTROLLER
{
    public override bool isSimulated => false;

    public BattleController(HeroKind heroKind) : base(heroKind)
    {
    }
    public override void Awake()
    {
        hero = new HeroBattle(heroKind, this);
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (heroKind != (HeroKind)i) heroAllys.Add(new HeroAlly((HeroKind)count, this));
        }
        for (int i = 0; i < pets.Length; i++)
        {
            pets[i] = new PetBattle(this);
        }
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = new MonsterBattle(this);
        }
        InstantiateChallengeMonster();
        AddList();
        //Area
        areaBattle = new AreaBattle(this);
        //ResourceDrop
        for (int i = 0; i < resourceGenerators.Length; i++)
        {
            resourceGenerators[i] = new ResourceGenerator(heroKind);
        }
        //MaterialDrop
        for (int i = 0; i < materialGenerators.Length; i++)
        {
            materialGenerators[i] = new MaterialGenerator(heroKind);
        }
        //EQDrop
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            equipmentGenerators[i] = new EquipmentGenerator(heroKind);
        }
        //ChestDrop
        for (int i = 0; i < chestGenerators.Length; i++)
        {
            chestGenerators[i] = new TreasureChestGenerator(this, heroKind);
        }
        //Blessing
        blessingCtrl = new BlessingController(this);
    }
    public override void Start()
    {
        skillSet = new SkillSet(this);
        game.skillCtrl.SetTrigger(this, hero, hero.Target, monstersList);
        hero.Start();
    }
    public override void ResetDrop()
    {
        for (int i = 0; i < resourceGenerators.Length; i++)
        {
            resourceGenerators[i].Initialize();
        }
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            equipmentGenerators[i].Initialize();
        }
        for (int i = 0; i < materialGenerators.Length; i++)
        {
            materialGenerators[i].Initialize();
        }
        ResetChest();
    }
    public override void ResetChest()
    {
        for (int i = 0; i < chestGenerators.Length; i++)
        {
            chestGenerators[i].Initialize();
        }
    }
    public override void GetDropResource()
    {
        for (int i = 0; i < resourceGenerators.Length; i++)
        {
            resourceGenerators[i].Get();
        }
    }
    public override void GetDropMaterial()
    {
        for (int i = 0; i < materialGenerators.Length; i++)
        {
            materialGenerators[i].Get();
        }
    }
    public override void GetDropEquipment()
    {
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            if (!game.inventoryCtrl.CanCreateEquipment()) break;
            equipmentGenerators[i].Get();
        }
    }

    //ResourceDrop
    public ResourceGenerator[] resourceGenerators = new ResourceGenerator[InventoryParameter.resourceMaxDropNum];
    public override void DropResource(ResourceKind kind, double num, Vector2 position)
    {
        if (!game.IsUI(heroKind) && !game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetResource)) return;
        for (int i = 0; i < resourceGenerators.Length; i++)
        {
            if (resourceGenerators[i].num == 0) 
            {
                resourceGenerators[i].Drop(kind, num, position);
                return;
            }
        }
        resourceGenerators[UnityEngine.Random.Range(0, resourceGenerators.Length)].Drop(kind, num, position);
    }
    //MaterialDrop
    public MaterialGenerator[] materialGenerators = new MaterialGenerator[InventoryParameter.materialMaxDropNum];
    public override void DropMaterial(MaterialKind kind, double num, Vector2 position, bool isColorMaterial = false)
    {
        if (!game.IsUI(heroKind) && !game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetMaterial)) return;
        for (int i = 0; i < materialGenerators.Length; i++)
        {
            if (materialGenerators[i].num == 0)
            {
                materialGenerators[i].Drop(kind, num, position);
                return;
            }
        }
        for (int i = 0; i < materialGenerators.Length; i++)
        {
            if (materialGenerators[i].kind == kind)
            {
                materialGenerators[i].Drop(kind, num, position);
                return;
            }
        }
        if (isColorMaterial)
            materialGenerators[UnityEngine.Random.Range(0, materialGenerators.Length)].Drop(kind, num, position);
    }
    //EQDrop
    public EquipmentGenerator[] equipmentGenerators = new EquipmentGenerator[InventoryParameter.equipMaxDropNum];
    public override void DropEquipment(long monsterLevel, HeroKind heroKind, Vector2 position)//EQのドロップ
    {
        if (!game.IsUI(heroKind) && !game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetEquipment)) return;
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            if (equipmentGenerators[i].kind == EquipmentKind.Nothing)
            {
                equipmentGenerators[i].Drop(monsterLevel, position);
                return;
            }
        }
    }
    public override void DropEquipment(EquipmentKind kind, long monsterLevel, HeroKind heroKind, Vector2 position)
    {
        if (!game.IsUI(heroKind) && !game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetEquipment)) return;
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            if (equipmentGenerators[i].kind == EquipmentKind.Nothing)
            {
                equipmentGenerators[i].Drop(kind, monsterLevel, position);
                return;
            }
        }
    }
    public override void DropUniqueEquipment(long monsterLevel, HeroKind heroKind, Vector2 position)
    {
        if (!game.IsUI(heroKind) && !game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetEquipment)) return;
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            if (equipmentGenerators[i].kind == EquipmentKind.Nothing)
            {
                equipmentGenerators[i].Drop(areaBattle.CurrentArea().uniqueEquipmentKind, monsterLevel, position);
                return;
            }
        }
    }
    public override long EquipmentDroppingNum()
    {
        long tempNum = 0;
        for (int i = 0; i < equipmentGenerators.Length; i++)
        {
            if (equipmentGenerators[i].kind != EquipmentKind.Nothing)
                tempNum++;
        }
        return tempNum;
    }
    //ChestDrop
    public TreasureChestGenerator[] chestGenerators = new TreasureChestGenerator[InventoryParameter.chestMaxDropNum];
    public override void DropChest(long monsterLevel, HeroKind heroKind, Vector2 position)
    {
        for (int i = 0; i < chestGenerators.Length; i++)
        {
            if (!chestGenerators[i].isActive)
            {
                chestGenerators[i].Drop(monsterLevel, position);
                return;
            }
        }
    }
}

public class AreaBattle : AREA_BATTLE//Areaの進行を管理する
{
    public AreaBattle(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
        //SetResultUIAction();
        //AreaMasteryの適用
        SetAreaMastery();
        //AreaDebuffの適用
        SetAreaDebuff();
    }
    public override AreaKind currentAreaKind { get => main.SR.currentAreaKind[(int)battleCtrl.heroKind]; set=> main.SR.currentAreaKind[(int)battleCtrl.heroKind] = value; }
    public override int currentAreaId { get => main.SR.currentAreaId[(int)battleCtrl.heroKind]; set => main.SR.currentAreaId[(int)battleCtrl.heroKind] = value; }
    void SetAreaMastery()
    {
        var info = new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Mul, () => game.areaCtrl.expBonuses[(int)CurrentArea().kind].Value());
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.ExpGain).RegisterMultiplier(info);
        info = new MultiplierInfo(MultiplierKind.AreaPrestige, MultiplierType.Mul, () => game.areaCtrl.moveSpeedBonuses[(int)CurrentArea().kind].Value());
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.MoveSpeed).RegisterMultiplier(info);
    }
    void SetAreaDebuff()
    {
        MultiplierInfo info;
        //Physicalは関係ないのでi=1(Fire)から
        for (int i = 1; i < Enum.GetNames(typeof(Element)).Length; i++)
        {
            int count = i;
            info = new MultiplierInfo(MultiplierKind.AreaDebuff, MultiplierType.Add, () => CurrentArea().debuffElement[count] * areaDebuffFactor);
            game.statsCtrl.HeroStats(battleCtrl.heroKind, (Stats)(count - 1)).RegisterMultiplier(info);
        }
        info = new MultiplierInfo(MultiplierKind.AreaDebuff, MultiplierType.Add, () => CurrentArea().debuffPhyCrit * areaDebuffFactor);
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.PhysCritChance).RegisterMultiplier(info);
        info = new MultiplierInfo(MultiplierKind.AreaDebuff, MultiplierType.Add, () => CurrentArea().debuffMagCrit * areaDebuffFactor);
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.MagCritChance).RegisterMultiplier(info);
    }
    //void SetResultUIAction()
    //{
    //    game.statsCtrl.Exp(battleCtrl.heroKind).resultUIAction = (value) => exp += value;
    //    game.resourceCtrl.gold.resultUIAction = (value) => gold += value;
    //    for (int i = 0; i < Enum.GetNames(typeof(ResourceKind)).Length; i++)
    //    {
    //        int count = i;
    //        game.resourceCtrl.Resource((ResourceKind)count).resultUIAction = (value) => resources[count] += value;
    //    }
    //    for (int i = 0; i < Enum.GetNames(typeof(MaterialKind)).Length; i++)
    //    {
    //        int count = i;
    //        game.materialCtrl.Material((MaterialKind)count).resultUIAction = (value) => materials[count] += value;
    //    }
    //}

    public override void Initialize()
    {
        base.Initialize();
        battleCtrl.InitializeMission();
        battleCtrl.CheckMission();
    }
    public override void CheckToNextWave()
    {
        battleCtrl.CheckMission();
        base.CheckToNextWave();
    }

    public async override void ClearAction()
    {
        //統計
        CurrentArea().ClearAction(battleCtrl.hero, true, gold, exp);
        isShowingResultPanel = true;
        if (game.IsUI(battleCtrl.heroKind) && clearedTask != null) await clearedTask();
        else await UniTask.DelayFrame(60);
        if (isShowingResultPanel)
        {
            isShowingResultPanel = false;
            //RetryDungeon
            if (CurrentArea().isDungeon && game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RetryDungeon) && CurrentArea().CanStart())
                Start(CurrentArea());
            else Start();
        }
        //PotionConsume
        game.inventoryCtrl.ConsumePotion(PotionConsumeConditionKind.AreaComplete, battleCtrl.hero, battleCtrl.isSimulated);
    }
    public async override void QuitCurrentArea()
    {
        Inactivate();
        //統計
        CurrentArea().ClearAction(battleCtrl.hero, false, gold, exp);
        //Failed
        isShowingResultPanel = true;
        //PotionConsume
        game.inventoryCtrl.ConsumePotion(PotionConsumeConditionKind.AreaComplete, battleCtrl.hero, battleCtrl.isSimulated);
        if (game.IsUI(battleCtrl.heroKind) && failedTask != null) await failedTask();
        else await UniTask.DelayFrame(60);
        if (isShowingResultPanel)
        {
            isShowingResultPanel = false;
            Start();
        }
    }
}


public class HeroBattle : HERO_BATTLE
{
    public HeroBattle(HeroKind heroKind, BattleController battleCtrl) : base(heroKind, battleCtrl)
    {
    }
    public override void Start()
    {
        for (int i = 0; i < debuffings.Length; i++)
        {
            debuffings[i].SetEffect();
        }
    }
    public override Vector2 MoveDirection()
    {
        if (!isAlive) return Vector2.zero;
        if (game.IsUI(heroKind))
        {
            Vector2 tempVector = Vector2.zero;
            if (IsInputDirection(Direction.up)) tempVector += Vector2.up;
            if (IsInputDirection(Direction.right)) tempVector += Vector2.right;
            if (IsInputDirection(Direction.down)) tempVector += Vector2.down;
            if (IsInputDirection(Direction.left)) tempVector += Vector2.left;
            if (tempVector != Vector2.zero) return tempVector * 500;
            if (!isAutoMove) return Vector2.zero;
        }
        return AliveMoveDirection();
    }
    public override void GetExp(double value)
    {
        game.statsCtrl.Exp(battleCtrl.heroKind).Increase(value, true);
    }
}

public class HeroAlly : HERO_BATTLE
{
    public override Vector2 initPosition => Parameter.hidePosition;
    public HeroAlly(HeroKind heroKind, BATTLE_CONTROLLER battleCtrl) : base(heroKind, battleCtrl)
    {
    }
    //public override void Activate()
    //{
    //    move.MoveTo(Parameter.heroInitPosition);
    //    base.Activate();
    //}
    public void Activate(Vector2 initPosition)
    {
        move.MoveTo(initPosition);
        base.Activate();
    }
    public override void DeadAction()
    {
        Inactivate();
    }
    public override void UpdateSkillCooltime(float deltaTime)
    {
        for (int i = 0; i < game.battleCtrls[(int)heroKind].skillSet.currentEquippingNum; i++)
        {
            if (game.battleCtrls[(int)heroKind].skillSet.currentSkillSet[i] != game.skillCtrl.NullSkill() && !game.battleCtrls[(int)heroKind].skillSet.currentSkillSet[i].IsFilledCoolttime(this, battleCtrl.isSimulated))
                game.battleCtrls[(int)heroKind].skillSet.currentSkillSet[i].CountCooltime(this, deltaTime, battleCtrl.isSimulated);
        }
        for (int i = 0; i < game.battleCtrls[(int)heroKind].skillSet.currentGlobalEquippingNum; i++)
        {
            if (game.battleCtrls[(int)heroKind].skillSet.currentGlobalSkillSet[i] != game.skillCtrl.NullSkill() && !game.battleCtrls[(int)heroKind].skillSet.currentGlobalSkillSet[i].IsFilledCoolttime(this, battleCtrl.isSimulated))
                game.battleCtrls[(int)heroKind].skillSet.currentGlobalSkillSet[i].CountCooltime(this, deltaTime, battleCtrl.isSimulated);
        }
    }
    public override void UpdateTriggerSkill()
    {
        for (int i = 0; i < game.battleCtrls[(int)heroKind].skillSet.currentEquippingNum; i++)
        {
            if (CanTriggerSkill(game.battleCtrls[(int)heroKind].skillSet.currentSkillSet[i]))
            {
                game.battleCtrls[(int)heroKind].skillSet.currentSkillSet[i].Trigger(this, currentMp);
            }
        }
        for (int i = 0; i < game.battleCtrls[(int)heroKind].skillSet.currentGlobalEquippingNum; i++)
        {
            if (CanTriggerSkill(game.battleCtrls[(int)heroKind].skillSet.currentGlobalSkillSet[i]))
            {
                game.battleCtrls[(int)heroKind].skillSet.currentGlobalSkillSet[i].Trigger(this, currentMp);
            }
        }
    }
    bool CanTriggerSkill(SKILL skill)
    {
        if (skill == game.skillCtrl.NullSkill()) return false;
        if (!IsWithinRange(this, Target(), skill.Range())) return false;
        if (currentMp.value < skill.ConsumeMp()) return false;
        if (!skill.IsFilledCoolttime(this, battleCtrl.isSimulated)) return false;
        return true;
    }

}

public class PetBattle : PET_BATTLE
{
    public PetBattle(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override bool CanShowStats(HeroKind heroKind)
    {
        return true;
    }

    public override void GetExp(double value)
    {
        //value *= game.guildCtrl.Member(battleCtrl.heroKind).gainRate;
        //globalInformation.pet.exp.Increase(value);
    }

}

public class MonsterBattle : MONSTER_BATTLE
{
    public MonsterBattle(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    MonsterDefeatedNumber defeatedNum { get => globalInformation.defeatedNums[(int)battleCtrl.heroKind]; }
    MonsterDefeatedNumber defeatedMutantNum { get => globalInformation.defeatedMutantNums[(int)battleCtrl.heroKind]; }

    public override void DeadAction()
    {
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.Capture))
            if (TryCapture(battleCtrl.heroKind)) return;

        //Gold獲得
        game.resourceCtrl.gold.Increase(gold, battleCtrl.heroKind);// * gainFactor
                //-> gainFactorはGold内で計算することにした。UpgradeによるFlat+があるため。
        //EXP獲得
        //game.statsCtrl.Exp(battleCtrl.heroKind).Increase(CalculatedExp(), true);
        ////Petを含めたEXP獲得
        for (int i = 0; i < battleCtrl.heroesList.Count; i++)
        {
            if (battleCtrl.heroesList[i].isAlive)
            {
                battleCtrl.heroesList[i].GetExp(CalculatedExp());// * gainFactor);
                //-> gainFactorはExp内で計算することにした。UpgradeによるFlat+があるため。
            }
        }
        //Resource獲得[ResourceのみResrouceのIncreaseにgainFactorをかいた]
        battleCtrl.DropResource(game.resourceCtrl.HeroResource(battleCtrl.heroKind).kind, resource, move.position);
        //Material獲得(個数の「１」はTitleで上昇する（HeroStatsにかく））
        if (WithinRandom(game.monsterCtrl.speciesMaterialDropChance[(int)species].Value() * (1 + Convert.ToInt16(isMutant))))
            battleCtrl.DropMaterial(globalInformation.dropSpeciesMaterial, game.statsCtrl.MaterialLootGain(battleCtrl.heroKind).Value() * gainFactor, move.position);
        //ColorMaterial獲得
        if (WithinRandom(game.monsterCtrl.colorMaterialDropChance.Value() * (1 + Convert.ToInt16(isMutant))))
            battleCtrl.DropMaterial(globalInformation.dropColorMaterial, game.statsCtrl.MaterialLootGain(battleCtrl.heroKind).Value() * gainFactor, move.position, true);
        //EQ獲得
        if (WithinRandom(game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.EquipmentDropChance).Value() * gainFactor))
            battleCtrl.DropEquipment(level, battleCtrl.heroKind, move.position);
        //Tutorialクエスト中はドロップ率UP
        if (game.questCtrl.Quest(QuestKindGlobal.Equip).isAccepted && !main.S.isGotFirstEQ)
        {
            if (WithinRandom(100 * game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.EquipmentDropChance).Value()))
            {
                battleCtrl.DropEquipment(EquipmentKind.DullSword, level, battleCtrl.heroKind, move.position);
                main.S.isGotFirstEQ = true;
            }
        }
        //Dungeon中はChest
        if (WithinRandom(battleCtrl.areaBattle.CurrentArea().ChestDropChance() * gainFactor))
            battleCtrl.DropChest(level, battleCtrl.heroKind, move.position);
        //Area固有EQ獲得
        if (!battleCtrl.areaBattle.isTringDungeon && !battleCtrl.areaBattle.isTringChallenge && WithinRandom(battleCtrl.areaBattle.CurrentArea().uniqueEquipmentDropChance.Value() * gainFactor))
            battleCtrl.DropUniqueEquipment(level, battleCtrl.heroKind, move.position);
        //統計量
        defeatedNum.Increase(1 * gainFactor);
        if (isMutant)
        {
            defeatedMutantNum.Increase(1 * gainFactor);
            battleCtrl.areaBattle.CurrentArea().swarm.IncreaseScoreOnMutantDefeat(1 * gainFactor);
        }
        if (battleCtrl.hero.HpPercent() < 0.20d) main.SR.survivalNumQuestTitle[(int)battleCtrl.heroKind] += gainFactor;
        FinishAction();
    }
    public override void Pilfer(double chance)
    {
        if (isPilfered) return;
        if (WithinRandom(chance))
        {
            MaterialKind kind = globalInformation.dropSpeciesMaterial;
            double amount = game.statsCtrl.MaterialLootGain(battleCtrl.heroKind).Value() * gainFactor;
            if (game.IsUI(battleCtrl.heroKind))
            {
                string logStr = optStr + "Successful Pilfer: ";
                logStr += tDigit(amount) + " ";
                logStr += Localized.localized.Material(kind);
                GameControllerUI.gameUI.logCtrlUI.Log("<color=green>" + logStr + "</color>");
            }
            battleCtrl.DropMaterial(kind , amount, move.position);
            isPilfered = true;
        }
    }
    public override bool CanShowStats(HeroKind heroKind)
    {
        return level <= game.statsCtrl.MonsterDistinguishMaxLevel(heroKind).Value();
    }
    //Capture
    public bool CanCapture(HeroKind heroKind, bool isTamerSkill = false)
    {
        if (isCaptured) return false;
        if (species == MonsterSpecies.Mimic) return false;
        if (level > game.monsterCtrl.monsterCapturableLevel[(int)heroKind].Value()) return false;
        switch (color)
        {
            case MonsterColor.Normal:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.ThrowingNet).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.ThrowingNet, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.ThrowingNet).IsAvailableCooltime(heroKind);
            case MonsterColor.Blue:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.IceRope).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.IceRope, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.IceRope).IsAvailableCooltime(heroKind);
            case MonsterColor.Yellow:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.ThunderRope).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.ThunderRope, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.ThunderRope).IsAvailableCooltime(heroKind);
            case MonsterColor.Red:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.FireRope).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.FireRope, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.FireRope).IsAvailableCooltime(heroKind);
            case MonsterColor.Green:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.LightRope).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.LightRope, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.LightRope).IsAvailableCooltime(heroKind);
            case MonsterColor.Purple:
                if (isTamerSkill) return game.shopCtrl.Trap(PotionKind.DarkRope).unlock.IsUnlocked();
                if (!game.inventoryCtrl.IsEquippedPotion(PotionKind.DarkRope, heroKind)) return false;
                return game.potionCtrl.GlobalInfo(PotionKind.DarkRope).IsAvailableCooltime(heroKind);
            case MonsterColor.Boss:
                return false;
            case MonsterColor.Metal:
                return false;
        }
        return true;
    }
    public override bool TryCapture(HeroKind heroKind, bool isTamerSkill = false)
    {
        if (!CanCapture(heroKind, isTamerSkill)) return false;
        isCaptured = true;
        ////UI(あとで別に書く）
        //if (game.IsUI(heroKind)) 

        //ver.0.0.4.2 Capture Chanceを廃止した
        //bool isSucceeded = false;
        //if (WithinRandom(game.monsterCtrl.monsterCaptureChance.Value()))
        //{
        Capture(heroKind);
        if (game.IsUI(heroKind))
        {
            GameControllerUI.gameUI.battleCtrlUI.CaptureParticle(move.position);
            if (!SettingMenuUI.Toggle(ToggleKind.DisableCaptureLog).isOn)
                GameControllerUI.gameUI.logCtrlUI.Log("<color=orange>Succeeded in capturing!");
        }
        //    isSucceeded = true;
        //}
        //else
        //{
        //    //UI(あとで別に書く）
        //    if (game.IsUI(heroKind)) GameControllerUI.gameUI.logCtrlUI.Log("<color=red>Failed to capture.");
        //}
        //キャプチャーの成功・失敗にかかわらずTrapを消費する

        if (isTamerSkill) return true;

        //GuildAbility[Trapping]の効果により、確率で消費しない
        if (WithinRandom(game.monsterCtrl.trapNotConsumedChance.Value())) return true;
        switch (color)
        {
            case MonsterColor.Normal: tempTrapKind = PotionKind.ThrowingNet; break;
            case MonsterColor.Blue: tempTrapKind = PotionKind.IceRope; break;
            case MonsterColor.Yellow: tempTrapKind = PotionKind.ThunderRope; break;
            case MonsterColor.Red: tempTrapKind = PotionKind.FireRope; break;
            case MonsterColor.Green: tempTrapKind = PotionKind.LightRope; break;
            case MonsterColor.Purple: tempTrapKind = PotionKind.DarkRope; break;
            case MonsterColor.Boss: tempTrapKind = PotionKind.ThrowingNet; break;
            case MonsterColor.Metal: tempTrapKind = PotionKind.ThrowingNet; break;
        }
        game.inventoryCtrl.ConsumePotion(tempTrapKind, battleCtrl.hero, battleCtrl.isSimulated);
        //return isSucceeded;
        return true;
    }
    PotionKind tempTrapKind;
    public override void Capture(HeroKind heroKind)
    {
        globalInformation.CaptureAction(heroKind, 1, isMutant);
        FinishAction();
    }
}

