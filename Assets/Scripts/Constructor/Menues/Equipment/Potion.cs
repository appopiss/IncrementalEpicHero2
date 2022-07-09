using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;
using static InventoryParameter;
using static PotionParameter;

public partial class SaveR
{
    public PotionKind[] potionKinds;//[potionId]
    public long[] potionStackNums;//[potionId]
}

public class Potion
{
    //AddHere (What effect triggers when it consumed)
    void ConsumeEffectAction(BATTLE battle)
    {
        globalInfo.ConsumeEffectAction(battle);
        //switch (kind)
        //{
        //    case PotionKind.MinorHealthPotion:
        //        battle.Heal(globalInfo.effectValue);
        //        break;
        //    case PotionKind.ChilledHealthPotion:
        //        battle.Heal(globalInfo.effectValue);
        //        break;
        //}
    }

    public int id;//potionId
    public PotionKind kind { get => main.SR.potionKinds[id]; set => main.SR.potionKinds[id] = value;}
    public PotionStackNum stackNum;
    public PotionGlobalInformation globalInfo { get
        {
            if (tempGlobalInfo == null)
            {
                tempGlobalInfo = game.potionCtrl.GlobalInfo(kind);
            }
            return tempGlobalInfo;
        } }
    PotionGlobalInformation tempGlobalInfo;
    public long maxStackNum
    {
        get
        {
            //if (globalInfo.type == PotionType.Talisman && slotId < potionSlotId) return 9999;
            return (long)game.potionCtrl.maxStackNum.Value();
        }
    }
    public PotionConsumeConditionKind consumeKind { get => globalInfo.consumeKind; }
    public Potion(int id)
    {
        this.id = id;
        stackNum = new PotionStackNum(id, () => maxStackNum);
    }
    int slotId;
    public void UpdateSlotId(int slotId)
    {
        this.slotId = slotId;
    }
    public void Create(PotionKind kind, long num)
    {
        if (num <= 0) return;
        if (this.kind != kind)
        {
            //createId++;
            this.kind = kind;
            tempGlobalInfo = game.potionCtrl.GlobalInfo(kind);
            //Talismanの場合はlevelチェック
            if (globalInfo.type == PotionType.Talisman && globalInfo.level.value < 1)
                globalInfo.level.ChangeValue(1);
            //ここでEffectを適用
            //SetEffect();
        }
        stackNum.Increase(num);
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }

    public double TalismanEffectValue()
    {
        return globalInfo.effectValue * stackNum.value;//globalInfo.EffectValue(stackNum.value);
    }
    public double TalismanPassiveEffectValue()
    {
        return globalInfo.PassiveEffectValue(stackNum.value);
    }

    public void CreateMax(PotionKind kind)
    {
        this.kind = kind;
        tempGlobalInfo = game.potionCtrl.GlobalInfo(kind);
        stackNum.ToMax();
    }
    public (bool isMax, long leftNum) IsFull(long num)
    {
        if (!stackNum.IsMaxed(num - 1)) return (false, 0);
        return (true, num - leftEmptyNum);
    }
    public long leftEmptyNum => stackNum.maxValue() - stackNum.value;
    public void Delete()
    {
        kind = PotionKind.Nothing;
        tempGlobalInfo = game.potionCtrl.GlobalInfo(kind);
        stackNum.ChangeValue(0);
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }

    public bool isAutoRestore { get => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.EquipPotion); }
    public void Consume(long num)
    {
        stackNum.Increase(-num);
        //PetによるAuto補充
        if (isAutoRestore)
            game.inventoryCtrl.AutoRestorePotion(slotId);

        //EpicStore[UtilityKeeper]を購入している場合はDeleteせず、Moveした時にDeleteする
        if (game.epicStoreCtrl.Item(EpicStoreKind.UtilityKeeper).IsPurchased()) return;
        StackNumCheckToDelete();
    }
    public void StackNumCheckToDelete()
    {
        if (stackNum.value <= 0 && kind != PotionKind.Nothing) Delete();
        if (kind == PotionKind.Nothing && stackNum.value > 0) Delete();
    }

    public void LotteryConsume(PotionConsumeConditionKind consumeKind, BATTLE battle, bool isSimulated = false)
    {
        if (!IsEquipped(battle.heroKind)) return;
        if (consumeKind != this.consumeKind) return;
        if (!globalInfo.IsAvailableCooltime(battle.heroKind, isSimulated)) return;
        ConsumeEffectAction(battle);
        if (WithinRandom(globalInfo.consumeChance))
        {
            //これはポーション自体の％を低くすることで解決しました
            ////さらに抽選。AlchemyUpgradeのPrevent-Consume
            //if (WithinRandom(game.potionCtrl.preventConsumeChance.Value())) return;
            if (isSimulated)
                globalInfo.lastUsedTimeSimulated = battle.battleCtrl.timecount;
            else
            {
                Consume(1);
                globalInfo.lastUsedTime[(int)battle.heroKind] = battle.battleCtrl.timecount;
            }
        }
        if (isSimulated) return;
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }
    public void LotteryConsume(PotionKind potionKind, BATTLE battle, bool isSimulated = false)
    {
        if (!IsEquipped(battle.heroKind)) return;
        if (potionKind != kind) return;
        if (!globalInfo.IsAvailableCooltime(battle.heroKind, isSimulated)) return;
        ConsumeEffectAction(battle);
        if (WithinRandom(globalInfo.consumeChance))
        {
            if (isSimulated)
                globalInfo.lastUsedTimeSimulated = battle.battleCtrl.timecount;
            else
            {
                Consume(1);
                globalInfo.lastUsedTime[(int)battle.heroKind] = battle.battleCtrl.timecount;
            }
        }
        if (isSimulated) return;
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }
    public bool IsEquipped(HeroKind heroKind)
    {
        if (kind == PotionKind.Nothing) return false;
        if (stackNum.value <= 0) return false; //{ Delete(); return false; }
        if (IsInventory()) return false;
        return (int)heroKind == (slotId - potionSlotId) / equipPotionSlotId;
    }
    public bool IsInventory()
    {
        return slotId < potionSlotId;
    }

    //int createId;//DeleteしたらEffectの登録も消す（無効にする）必要があるため、Createするたびに更新する


    public double DisassembleGoldNum()
    {
        return globalInfo.DisassembleGoldNum() * stackNum.value;
    }
}

public class PotionStackNum : INTEGER
{
    public int id;
    public override long value { get => main.SR.potionStackNums[id]; set => main.SR.potionStackNums[id] = value; }
    public PotionStackNum(int id, Func<long> maxValue)
    {
        this.id = id;
        this.maxValue = maxValue;
    }
}
public enum PotionKind
{
    Nothing,
    MinorHealthPotion,
    MinorRegenerationPoultice,
    MinorResourcePoultice,
    SlickShoeSolution,
    MinorManaRegenerationPoultice,
    MaterialMultiplierMist,
    BasicElixirOfBrawn,
    BasicElixirOfBrains,
    BasicElixirOfFortitude,
    BasicElixirOfConcentration,
    BasicElixirOfUnderstanding,
    ChilledHealthPotion,
    ChilledRegenerationPoultice,
    FrostyDefensePotion,
    IcyAuraDraught,
    SlightlyStickySalve,
    SlickerShoeSolution,
    CoolHeadOintment,
    FrostySlayersOil,
    BurningDefensePotion,
    BlazingAuraDraught,
    FierySlayersOil,
    ElectricDefensePotion,
    WhirlingAuraDraught,
    ShockingSlayersOil,

    //Trap
    ThrowingNet,
    IceRope,
    ThunderRope,
    FireRope,
    LightRope,
    DarkRope,

    //Talisman
    GuildMembersEmblem,
    CertificateOfCompetence,
    MasonsTrowel,
    EnchantedAlembic,
    TrackersMap,
    BerserkersStone,
    TrappersTag,
    HitanDoll,
    RingoldDoll,
    NuttyDoll,
    MorkylDoll,
    FlorzporbDoll,
    ArachnettaDoll,
    GuardianKorDoll,
    SlimeBadge,
    MagicslimeBadge,
    SpiderBadge,
    BatBadge,
    FairyBadge,
    FoxBadge,
    DevilfishBadge,
    TreantBadge,
    FlametigerBadge,
    UnicornBadge,
    AscendedFromIEH1,
    WarriorsBadge,
    WizardsBadge,
    AngelsBadge,
    ThiefsBadge,
    ArchersBadge,
    TamersBadge,

    //Enumの順番を変えたり消したりしてはいけない！

    //OilSpillTrap,
    //AntimagicOilTrap,
    //SilkyStrandTrap,
    //BloodyClothTrap,
    //FairyHouseTrap,
    //CleverPuzzleTrap,
    //BaitedHookTrap,
    //FertileEarthTrap,
    //SmokedMeatTrap,
    //SirensLureTrap,
    //WitchsCauldron,

}
public enum PotionConsumeConditionKind
{
    Nothing,//消費しない
    HpHalf,
    AreaComplete,
    Defeat,
    Move,
    Capture,
}
public enum PotionType
{
    Nothing,
    Health,
    HealthRegen,
    ResourceGain,
    Move,
    Mana,
    ManaRegen,
    MaterialGain,
    PhysicalDamage,
    MagicalDamage,
    MaxHP,
    MaxMP,
    SkillProfGain,
    ElementResistance,
    Aura,
    GoldGain,
    ExpGain,
    SlayerOil,

    Trap,

    Talisman,
}
public enum TalismanRarity
{
    Common,//MonsterSpeciesBadge
    Uncommon,//ClassBadge
    Rare,//ChallengeBoss
    SuperRare,//ユニークなもの
    Epic,//IEH1Badgeなど入手が限られているもの
}

//public void SetEffect()//Talisman
//{
//    if (globalInfo.type == PotionType.Talisman)
//    {
//        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
//        {
//            HeroKind heroKind = (HeroKind)i;
//            SetEffect(heroKind, TalismanEffectValue, createId);
//        }
//        //pasiveは１回だけ登録する。->不要。potionCtrlで一括でセット？
//        //SetEffect(HeroKind.Warrior, TalismanPassiveEffectValue, createId, true);
//    }
//    else
//    {
//        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
//        {
//            HeroKind heroKind = (HeroKind)i;
//            SetEffect(heroKind, () => globalInfo.effectValue, createId);
//        }
//    }
//}


//public void SetEffect(HeroKind heroKind, Func<double> effectValue, int createId)//, bool isPassive = false)
//{
//    Func<bool> activateCondition = () =>
//    {
//        if (!game.battleCtrls[(int)heroKind].isActiveBattle) return false;
//        return this.createId == createId && IsEquipped(heroKind);
//    };
//    switch (kind)
//    {
//        case PotionKind.MinorRegenerationPoultice:
//            game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.MinorResourcePoultice:
//            game.statsCtrl.ResourceGain(ResourceKind.Stone).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.ResourceGain(ResourceKind.Crystal).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.ResourceGain(ResourceKind.Leaf).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.SlickShoeSolution:
//            game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.MinorManaRegenerationPoultice:
//            game.statsCtrl.MpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.MaterialMultiplierMist:
//            game.statsCtrl.MaterialLootGain(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BasicElixirOfBrawn:
//            game.statsCtrl.ElementDamage(heroKind, Element.Physical).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BasicElixirOfBrains:
//            game.statsCtrl.ElementDamage(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            game.statsCtrl.ElementDamage(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            game.statsCtrl.ElementDamage(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            game.statsCtrl.ElementDamage(heroKind, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            game.statsCtrl.ElementDamage(heroKind, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BasicElixirOfFortitude:
//            game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BasicElixirOfConcentration:
//            game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BasicElixirOfUnderstanding:
//            game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.ChilledRegenerationPoultice:
//            game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.FrostyDefensePotion:
//            game.statsCtrl.ElementResistance(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.IcyAuraDraught:
//            game.statsCtrl.DebuffChance(heroKind, Debuff.SpdDown).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.SlightlyStickySalve:
//            game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.SlickerShoeSolution:
//            game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.CoolHeadOintment:
//            game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.FrostySlayersOil:
//            game.statsCtrl.ElementSlayerDamage(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BurningDefensePotion:
//            game.statsCtrl.ElementResistance(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.BlazingAuraDraught:
//            game.statsCtrl.DebuffChance(heroKind, Debuff.Knockback).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.FierySlayersOil:
//            game.statsCtrl.ElementSlayerDamage(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.ElectricDefensePotion:
//            game.statsCtrl.ElementResistance(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.WhirlingAuraDraught:
//            game.statsCtrl.DebuffChance(heroKind, Debuff.Electric).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.ShockingSlayersOil:
//            game.statsCtrl.ElementSlayerDamage(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//            break;

//        //Trap
//        //case PotionKind.ThrowingNet:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.IceRope:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.ThunderRope:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.FireRope:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.LightRope:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.DarkRope:
//        //    game.monsterCtrl.monsterCaptureChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, effectValue, activateCondition));
//        //    break;

//        //Talisman
//        //case PotionKind.GuildMembersEmblem:
//        //    //if (isPassive) game.guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//        //    //else
//        //        game.guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.CertificateOfCompetence:
//        //    //if (isPassive)
//        //    //{
//        //    //    for (int i = 0; i < game.skillCtrl.skillLevelBonus.Length; i++)
//        //    //    {
//        //    //        game.skillCtrl.skillLevelBonus[i].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//        //    //    }
//        //    //}
//        //    //else
//        //        game.skillCtrl.skillCooltimeReduction[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.MasonsTrowel:
//        //    //if (isPassive) game.townCtrl.townLevelEffectMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//        //    //else
//        //    game.townCtrl.townMaterialGainMultiplier[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.EnchantedAlembic:
//        //    //if (isPassive) game.alchemyCtrl.doubleAlchemyPointChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//        //    //else
//        //        game.potionCtrl.effectMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.TrackersMap:
//        //    //if (isPassive) ;
//        //    //else
//        //        game.areaCtrl.clearCountBonusClass[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.BerserkersStone:
//        //    //if (isPassive) ;
//        //    //else
//        //    //{
//        //        Func<bool> condition = () =>
//        //        {
//        //            if (!activateCondition()) return false;
//        //            if (!game.battleCtrls[(int)heroKind].isActiveBattle) return false;
//        //            if (game.battleCtrls[(int)heroKind].hero.HpPercent() > effectValue()) return false;
//        //            return true;
//        //        };
//        //        var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => 2d, condition);
//        //        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK ).RegisterMultiplier(info);
//        //        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(info);
//        //        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).RegisterMultiplier(info);
//        //        info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => -1, condition);
//        //        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(info);
//        //        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(info);
//        //    //}
//        //    break;
//        //case PotionKind.TrappersTag:
//        //    //if (isPassive) game.monsterCtrl.tamingPointGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, () => this.createId == createId));
//        //    //else
//        //        game.monsterCtrl.captureTripleChance[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//        //    break;
//        //case PotionKind.HitanDoll:
//        //    break;
//        //case PotionKind.RingoldDoll:
//        //    break;
//        //case PotionKind.NuttyDoll:
//        //    break;
//        //case PotionKind.MorkylDoll:
//        //    break;
//        case PotionKind.FlorzporbDoll:
//            //if (isPassive) game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//                game.skillCtrl.baseAttackSlimeBall[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.ArachnettaDoll:
//            //if (isPassive) game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//                game.skillCtrl.baseAttackPoisonChance[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.GuardianKorDoll:
//            //if (isPassive) game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.heroes[(int)heroKind].golemInvalidDamageHpPercent.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.SlimeBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.MagicslimeBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.SpiderBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.BatBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.FairyBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.FoxBadge:
//            //if (isPassive) game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue(), () => this.createId == createId));
//            //else
//                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.DevilfishBadge:
//            game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.TreantBadge:
//            game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.FlametigerBadge:
//            game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.UnicornBadge:
//            game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.AscendedFromIEH1:
//            //if (isPassive) ;
//            //else
//                game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), activateCondition));
//            break;
//        case PotionKind.WarriorsBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//                //game.statsCtrl.HeroStats(HeroKind.Warrior, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.WizardsBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//            //game.statsCtrl.HeroStats(HeroKind.Wizard, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.AngelsBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//            //game.statsCtrl.HeroStats(HeroKind.Angel, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.ThiefsBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//            game.statsCtrl.HeroStats(heroKind, Stats.EquipmentDropChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            break;
//        case PotionKind.ArchersBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//            //game.statsCtrl.HeroStats(HeroKind.Archer, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.HeroStats(heroKind, Stats.CriticalDamage).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//        case PotionKind.TamersBadge:
//            //if (isPassive) game.skillCtrl.skillRankCostFactors[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue(), () => this.createId == createId));
//            //else
//            //game.statsCtrl.HeroStats(HeroKind.Tamer, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, effectValue, activateCondition));
//            game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, effectValue, activateCondition));
//            break;
//    }
//}
////ここまで
