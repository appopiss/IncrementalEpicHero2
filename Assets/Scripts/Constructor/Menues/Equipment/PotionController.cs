using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static PotionParameter;
using static UsefulMethod;

public class PotionController
{
    public List<PotionGlobalInformation> globalInformations = new List<PotionGlobalInformation>();
    public List<PotionGlobalInformation> potions = new List<PotionGlobalInformation>();
    public List<PotionGlobalInformation> traps = new List<PotionGlobalInformation>();
    public List<Talisman> talismans = new List<Talisman>();
    public Multiplier maxStackNum;
    public Multiplier preventConsumeChance;
    public Multiplier effectMultiplier;
    //public Multiplier[] effectMultipliers = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier trapEffectMultiplier;
    public Multiplier trapCooltimeReduction;
    public Multiplier disassembleGoldGainMultiplier;
    public Multiplier potionMaxLevel;
    public AlchemyController alchemyCtrl { get => game.alchemyCtrl; }
    public PotionGlobalInformation GlobalInfo(PotionKind potionKind)
    {
        if (globalInformations.Count < 1) { Debug.Log("Error"); return null; }
        for (int i = 0; i < globalInformations.Count; i++)
        {
            if (globalInformations[i].kind == potionKind)
                return globalInformations[i];
        }
        return globalInformations[0];
    }

    public PotionController()
    {
        maxStackNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 10));
        preventConsumeChance = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0));
        effectMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        //for (int i = 0; i < effectMultipliers.Length; i++)
        //{
        //    effectMultipliers[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        //}
        trapCooltimeReduction = new Multiplier(() => 10, () => 1);
        trapCooltimeReduction.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 10));
        trapEffectMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        disassembleGoldGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        potionMaxLevel = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => PotionParameter.maxLevel));
        //Add Here

        availableQueue = new Multiplier();
        //Start();
    }

    public void Start()
    {
        globalInformations.Add(new NullPotion());
        potions.Add(new MinorHealthPotion());
        potions.Add(new ChilledHealthPotion());
        potions.Add(new MinorRegenerationPoultice());
        potions.Add(new ChilledRegenerationPoultice());
        potions.Add(new MinorManaRegenerationPoultice());
        potions.Add(new MinorResourcePoultice());
        potions.Add(new SlickShoeSolution());
        potions.Add(new SlickerShoeSolution());
        potions.Add(new SlightlyStickSalve());
        potions.Add(new CoolHeadOintment());
        potions.Add(new MaterialMultiplierMist());
        potions.Add(new BasicElixirOfBrawn());
        potions.Add(new BasicElixirOfBrains());
        potions.Add(new BasicElixirOfFortitude());
        potions.Add(new BasicElixirOfConcentration());
        potions.Add(new BasicElixirOfUnderstanding());
        potions.Add(new FrostyDefensePotion());
        potions.Add(new BurningDefensePotion());
        potions.Add(new ElectricDefensePotion());
        potions.Add(new IcyAuraDraught());
        potions.Add(new BlazingAuraDraught());
        potions.Add(new WhirlingAuraDraught());
        potions.Add(new FrostySlayersOil());
        potions.Add(new FierySlayersOil());
        potions.Add(new ShockingSlayersOil());

        /*
        potions.Add(new MinorHealthPotion());
        potions.Add(new MinorRegenerationPoultice());
        potions.Add(new MinorResourcePoultice());
        potions.Add(new SlickShoeSolution());
        potions.Add(new MinorManaRegenerationPoultice());
        potions.Add(new MaterialMultiplierMist());
        potions.Add(new BasicElixirOfBrawn());
        potions.Add(new BasicElixirOfBrains());
        potions.Add(new BasicElixirOfFortitude());
        potions.Add(new BasicElixirOfConcentration());
        potions.Add(new BasicElixirOfUnderstanding());
        potions.Add(new ChilledHealthPotion());
        potions.Add(new ChilledRegenerationPoultice());
        potions.Add(new FrostyDefensePotion());
        potions.Add(new IcyAuraDraught());
        potions.Add(new SlightlyStickSalve());
        potions.Add(new SlickerShoeSolution());
        potions.Add(new CoolHeadOintment());
        potions.Add(new FrostySlayersOil());
        potions.Add(new BurningDefensePotion());
        potions.Add(new BlazingAuraDraught());
        potions.Add(new FierySlayersOil());
        potions.Add(new ElectricDefensePotion());
        potions.Add(new WhirlingAuraDraught());
        potions.Add(new ShockingSlayersOil());
        */
        //Trap
        traps.Add(new ThrowingNet());
        traps.Add(new IceRope());
        traps.Add(new ThunderRope());
        traps.Add(new FireRope());
        traps.Add(new LightRope());
        traps.Add(new DarkRope());

        //Talisman（多分、順番は好きに変えてOK）
        talismans.Add(new SlimeBadge());
        talismans.Add(new MagicslimeBadge());
        talismans.Add(new SpiderBadge());
        talismans.Add(new BatBadge());
        talismans.Add(new FairyBadge());
        talismans.Add(new FoxBadge());
        talismans.Add(new DevilfishBadge());
        talismans.Add(new TreantBadge());
        talismans.Add(new FlametigerBadge());
        talismans.Add(new UnicornBadge());
        talismans.Add(new WarriorsBadge());
        talismans.Add(new WizardsBadge());
        talismans.Add(new AngelsBadge());
        talismans.Add(new ThiefsBadge());
        talismans.Add(new ArchersBadge());
        talismans.Add(new TamersBadge());

        talismans.Add(new FlorzporbDoll());
        talismans.Add(new ArachnettaDoll());
        talismans.Add(new GuardianKorDoll());

        //talismans.Add(new GuildMembersEmblem());
        //talismans.Add(new CertificateOfCompetence());
        //talismans.Add(new MasonsTrowel());
        //talismans.Add(new EnchantedAlembic());
        //talismans.Add(new TrackersMap());
        //talismans.Add(new BerserkersStone());
        //talismans.Add(new TrappersTag());

        //talismans.Add(new HitanDoll());
        //talismans.Add(new RingoldDoll());
        //talismans.Add(new NuttyDoll());
        //talismans.Add(new MorkylDoll());
        talismans.Add(new AscendedFromIEH1());

        globalInformations.AddRange(potions);
        globalInformations.AddRange(traps);
        globalInformations.AddRange(talismans);

        SetEffect();
    }

    public void SetEffect()
    {
        for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
        {
            HeroKind heroKind = (HeroKind)j;
            for (int i = 0; i < globalInformations.Count; i++)
            {
                int count = i;
                globalInformations[i].SetEffect(heroKind, () => game.inventoryCtrl.PotionEquipNum(globalInformations[count].kind, heroKind));
            }
        }
    }

    long tempPotionLevel;
    public long TotalPotionLevel()
    {
        tempPotionLevel = 0;
        for (int i = 0; i < potions.Count; i++)
        {
            tempPotionLevel += potions[i].level.value;
        }
        return tempPotionLevel;
    }

    //Queue
    public Multiplier availableQueue;
    public long CurrentAvailableQueue()
    {
        long tempAssignedQueue = 0;
        for (int i = 0; i < globalInformations.Count; i++)
        {
            tempAssignedQueue += globalInformations[i].queue + 10 * Convert.ToInt64(globalInformations[i].isSuperQueued);
        }
        return (long)availableQueue.Value() - tempAssignedQueue;
    }
    public void AssignQueue(PotionGlobalInformation potion, bool isSuperQueue = false)
    {
        if (!CanAssignQueue(potion, isSuperQueue)) return;
        potion.AssignQueue(Math.Min(TransactionCost.MultibuyNum(), CurrentAvailableQueue()), isSuperQueue);
    }
    public void RemoveQueue(PotionGlobalInformation potion)
    {
        if (!CanRemoveQueue(potion)) return;
        potion.RemoveQueue(Math.Min(TransactionCost.MultibuyNum(), potion.queue));
    }
    public void AdjustAssignedQueue()
    {
        for (int i = 0; i < globalInformations.Count; i++)
        {
            if (CurrentAvailableQueue() >= 0) return;
            if (CanRemoveQueue(globalInformations[i])) globalInformations[i].ResetQueue();
        }
    }
    public bool CanAssignQueue(PotionGlobalInformation potion, bool isSuperQueue)
    {
        if (isSuperQueue)
        {
            if (!game.epicStoreCtrl.Item(EpicStoreKind.SuperQueueAlchemy).IsPurchased()) return false;
            if (CurrentAvailableQueue() < 10) return false;
        }
        if (CurrentAvailableQueue() < 1) return false;
        return true;
    }
    public bool CanRemoveQueue(PotionGlobalInformation potion)
    {
        return potion.queue > 0 || potion.isSuperQueued;
    }
    public void BuyByQueue()
    {
        for (int i = 0; i < globalInformations.Count; i++)
        {
            globalInformations[i].BuyByQueue();
        }
    }
}


public class NullPotion : PotionGlobalInformation
{
}
public class MinorHealthPotion : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.MinorHealthPotion;
        type = PotionType.Health;
        consumeKind = PotionConsumeConditionKind.HpHalf;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 2, () => Math.Pow(1.5d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 1);
    }
    public override void ConsumeEffectAction(BATTLE battle)
    {
        battle.Heal(effectValue);
    }
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Slime).unlock.IsUnlocked(); }
    public override float cooltime => 10;
    public override double EffectValue(long level) { return 15 + 5 * level; }
    public override double AlchemyPointGain(long level) { return 1; }
}
public class MinorRegenerationPoultice : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.MinorRegenerationPoultice;
        type = PotionType.HealthRegen;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 5, () => Math.Pow(1.5d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 5);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
    }
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Slime).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 1 + 0.2 * level; }
    public override double AlchemyPointGain(long level) { return 2 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}
public class MinorResourcePoultice : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.MinorResourcePoultice;
        type = PotionType.ResourceGain;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 5, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 1);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ResourceGain(ResourceKind.Stone).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ResourceGain(ResourceKind.Crystal).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ResourceGain(ResourceKind.Leaf).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
    }
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Slime).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.25 + 0.05 * level; }
    public override double AlchemyPointGain(long level) { return 1 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}
public class SlickShoeSolution : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.SlickShoeSolution;
        type = PotionType.Move;
        consumeKind = PotionConsumeConditionKind.Move;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 10, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 5);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
    }
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Slime).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.10 + 0.0010 * level; }
    public override double AlchemyPointGain(long level) { return 3 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class MinorManaRegenerationPoultice : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.MinorManaRegenerationPoultice;
        type = PotionType.ManaRegen;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 5, () => 5, true);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 5);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 1);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.MpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 1 + 0.2 * level; }
    public override double AlchemyPointGain(long level) { return 3 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class MaterialMultiplierMist : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.MaterialMultiplierMist;
        type = PotionType.MaterialGain;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 25, () => Math.Pow(10, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 10);
        alchemyCosts.Add(EssenceKind.EssenceOfCreation, () => 5);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.MaterialLootGain(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 1 + 0.10d * level; }
    public override double effectValue { get => Math.Floor(EffectValue(level.value) * game.potionCtrl.effectMultiplier.Value()); }
    public override double nextEffectValue { get => Math.Floor(EffectValue(levelTransaction.ToLevel()) * game.potionCtrl.effectMultiplier.Value()); }
    public override double AlchemyPointGain(long level) { return 4 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BasicElixirOfBrawn : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BasicElixirOfBrawn;
        type = PotionType.PhysicalDamage;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 10, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 3);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementDamage(heroKind, Element.Physical).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20 + 0.020 * level; }
    public override double AlchemyPointGain(long level) { return 4 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BasicElixirOfBrains : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BasicElixirOfBrains;
        type = PotionType.MagicalDamage;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 10, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 3);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementDamage(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ElementDamage(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ElementDamage(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ElementDamage(heroKind, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));
        game.statsCtrl.ElementDamage(heroKind, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20 + 0.020 * level; }
    public override double AlchemyPointGain(long level) { return 4 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BasicElixirOfFortitude : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BasicElixirOfFortitude;
        type = PotionType.MaxHP;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 10, () => Math.Pow(1.5d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 50);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 50 + 25 * level; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BasicElixirOfConcentration : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BasicElixirOfConcentration;
        type = PotionType.MaxMP;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 10, () => Math.Pow(1.5d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 50);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 20 + 10 * level; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BasicElixirOfUnderstanding : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BasicElixirOfUnderstanding;
        type = PotionType.SkillProfGain;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 15, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 10);
        alchemyCosts.Add(EssenceKind.EssenceOfCreation, () => 5);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.10d + 0.005d * level; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class ChilledHealthPotion : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ChilledHealthPotion;
        type = PotionType.Health;
        consumeKind = PotionConsumeConditionKind.HpHalf;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 30, () => Math.Pow(2d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 10);
        alchemyCosts.Add(EssenceKind.EssenceOfIce, () => 1);
    }
    public override void ConsumeEffectAction(BATTLE battle)
    {
        battle.Heal(effectValue);
    }

    public override bool isEffectedByLowerTier => true;
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override float cooltime => 10;
    public override double EffectValue(long level) { return (1.50 + 0.05d * level) * game.potionCtrl.GlobalInfo(PotionKind.MinorHealthPotion).effectValue; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}
public class ChilledRegenerationPoultice : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ChilledRegenerationPoultice;
        type = PotionType.HealthRegen;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 50, () => Math.Pow(2d, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfIce, () => 5);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isEffectedByLowerTier => true;
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return (1.50 + 0.05d * level) * game.potionCtrl.GlobalInfo(PotionKind.MinorRegenerationPoultice).effectValue; }
    public override double AlchemyPointGain(long level) { return 6 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class FrostyDefensePotion : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.FrostyDefensePotion;
        type = PotionType.ElementResistance;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 25, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfIce, () => 10);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementResistance(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 6 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class IcyAuraDraught : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.IcyAuraDraught;
        type = PotionType.Aura;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 40, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfIce, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfWinter, () => 2);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.DebuffChance(heroKind, Debuff.SpdDown).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.25d + level * 0.005d; }
    public override double AlchemyPointGain(long level) { return 7 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class SlightlyStickSalve : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.SlightlyStickySalve;
        type = PotionType.GoldGain;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 50, () => Math.Pow(7.5d, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 25);
        alchemyCosts.Add(EssenceKind.EssenceOfCreation, () => 5);
        alchemyCosts.Add(EssenceKind.EssenceOfWinter, () => 1);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.25d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 10 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class SlickerShoeSolution : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.SlickerShoeSolution;
        type = PotionType.Move;
        consumeKind = PotionConsumeConditionKind.Move;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 100, () => Math.Pow(7.5d, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfLife, () => 25);
        alchemyCosts.Add(EssenceKind.EssenceOfWinter, () => 10);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Mul, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isEffectedByLowerTier => true;
    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return (1.25 + 0.005d * level) * game.potionCtrl.GlobalInfo(PotionKind.SlickShoeSolution).effectValue; }
    public override double AlchemyPointGain(long level) { return 8 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class CoolHeadOintment : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.CoolHeadOintment;
        type = PotionType.ExpGain;
        consumeKind = PotionConsumeConditionKind.Defeat;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 75, () => Math.Pow(7.5d, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfSlime, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 10);
        alchemyCosts.Add(EssenceKind.EssenceOfWinter, () => 10);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20d + level * 0.020d; }
    public override double AlchemyPointGain(long level) { return 10 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class FrostySlayersOil : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.FrostySlayersOil;
        type = PotionType.SlayerOil;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 100, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 25);
        alchemyCosts.Add(EssenceKind.EssenceOfIce, () => 25);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementSlayerDamage(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.10d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 10 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class BurningDefensePotion : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BurningDefensePotion;
        type = PotionType.ElementResistance;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 25, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfFire, () => 10);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementResistance(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Flame).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}
public class BlazingAuraDraught : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.BlazingAuraDraught;
        type = PotionType.Aura;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 40, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfFire, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfSummer, () => 2);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.DebuffChance(heroKind, Debuff.Knockback).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Flame).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.25d + level * 0.005d; }
    public override double AlchemyPointGain(long level) { return 7 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class FierySlayersOil : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.FierySlayersOil;
        type = PotionType.SlayerOil;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 100, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 25);
        alchemyCosts.Add(EssenceKind.EssenceOfFire, () => 25);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementSlayerDamage(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Flame).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.10d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 10 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class ElectricDefensePotion : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ElectricDefensePotion;
        type = PotionType.ElementResistance;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 25, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfThunder, () => 10);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementResistance(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Storm).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.20d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 5 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}
public class WhirlingAuraDraught : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.WhirlingAuraDraught;
        type = PotionType.Aura;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 40, () => Math.Pow(2, 1 / 5d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfThunder, () => 20);
        alchemyCosts.Add(EssenceKind.EssenceOfWind, () => 2);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.DebuffChance(heroKind, Debuff.Electric).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Storm).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.25d + level * 0.005d; }
    public override double AlchemyPointGain(long level) { return 7 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

public class ShockingSlayersOil : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ShockingSlayersOil;
        type = PotionType.SlayerOil;
        consumeKind = PotionConsumeConditionKind.AreaComplete;
        level = new PotionLevel(kind);
        levelTransaction = new Transaction(level, alchemyPoint, () => 100, () => Math.Pow(5, 1 / 10d), false);
        alchemyCosts.Add(EssenceKind.EssenceOfMagic, () => 25);
        alchemyCosts.Add(EssenceKind.EssenceOfThunder, () => 25);
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.ElementSlayerDamage(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Potion, MultiplierType.Add, () => effectValue, () => IsActiveEffect(heroKind, equipNum)));

    }

    public override bool isUnlocked { get => game.catalystCtrl.Catalyst(CatalystKind.Storm).unlock.IsUnlocked(); }
    public override double EffectValue(long level) { return 0.10d + level * 0.01d; }
    public override double AlchemyPointGain(long level) { return 10 * game.alchemyCtrl.alchemyPointGainMultiplier.Value(); }
}

//Trap
public class ThrowingNet : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ThrowingNet;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class IceRope : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.IceRope;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class ThunderRope : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.ThunderRope;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class FireRope : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.FireRope;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class LightRope : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.LightRope;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class DarkRope : PotionGlobalInformation
{
    public override void SetInfo()
    {
        kind = PotionKind.DarkRope;
        type = PotionType.Trap;
        consumeKind = PotionConsumeConditionKind.Capture;
        level = new TrapLevel();
    }
    public override float cooltime => (float)game.potionCtrl.trapCooltimeReduction.Value();
    public override double EffectValue(long level) { return 0.20d; }
    public override double effectValue => EffectValue(0) * game.potionCtrl.trapEffectMultiplier.Value();
}
public class TrapLevel : INTEGER
{
    public override long value { get => game.townCtrl.Building(BuildingKind.Trapper).level.value; set { } }
}

//Talisman
public class Talisman : PotionGlobalInformation
{
    public virtual PotionKind talismanKind { get; }
    public override double effectValue => EffectValue(level.value);//potionのeffectMultiplierの影響を受けないためにoverrideした
    public override double nextEffectValue => EffectValue(levelTransaction.ToLevel());
    public override bool isUnlocked => level.value >= 1;
    public override void SetInfo()
    {
        kind = talismanKind;
        type = PotionType.Talisman;
        consumeKind = PotionConsumeConditionKind.Nothing;
        level = new TalismanLevel(kind);//new PotionLevel(kind);
        levelTransaction = new Transaction(level, talismanFragment, Cost);
        SetPassiveEffect();

        //if (level.value < 1) level.ChangeValue(1);//Talismanは初期値Lv1とする->Createの時にこれを呼ぶことにする。したがってisUnlockedはLevelを基準に
    }
    public virtual void SetPassiveEffect() { }
    double Cost(long level)
    {
        switch (talismanRarity)
        {
            case TalismanRarity.Common:
                return 9 * level + Math.Pow(level, 2);//10,21,34,49,66,85,...
            case TalismanRarity.Uncommon:
                return 99 * level + Math.Pow(level, 3);
            case TalismanRarity.Rare:
                return 999 * level + Math.Pow(level, 4);
            case TalismanRarity.SuperRare:
                return 9999 * level + Math.Pow(level, 5);
            case TalismanRarity.Epic:
                return 99999 * level + Math.Pow(level, 10);
        }
        return 10000000000000;
    }
}

//public class GuildMembersEmblem : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.GuildMembersEmblem;
//    public override double EffectValue(long level)
//    {
//        return 0.10d * level;
//    }
//}
//public class CertificateOfCompetence : Talisman//SPDのMultiplier
//{
//    public override PotionKind talismanKind => PotionKind.CertificateOfCompetence;
//    public override double EffectValue(long level)
//    {
//        return 0.02d * level;
//    }
//    public override double PassiveEffectValue(long level)
//    {
//        return level;
//    }
//}
//public class MasonsTrowel : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.MasonsTrowel;
//    public override double EffectValue(long level)
//    {
//        return 0.10d * level;
//    }
//    public override double PassiveEffectValue(long level)
//    {
//        return 0.01d * level;
//    }
//}
//public class EnchantedAlembic : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.EnchantedAlembic;
//    public override double EffectValue(long level)
//    {
//        return 0.10d * level;
//    }
//    public override double PassiveEffectValue(long level)
//    {
//        return 0.001d * level;
//    }
//}
//public class TrackersMap : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.TrackersMap;
//    public override double EffectValue(long level)
//    {
//        return level;
//    }
//}
//public class BerserkersStone : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.BerserkersStone;
//    public override double EffectValue(long level)
//    {
//        return 0.02d * level;
//    }
//}
//public class TrappersTag : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.TrappersTag;
//    public override double EffectValue(long level)
//    {
//        return 0.01d * level;
//    }
//    public override double PassiveEffectValue(long level)
//    {
//        return 0.01d * level;
//    }
//}
//public class HitanDoll : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.HitanDoll;
//}
//public class RingoldDoll : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.RingoldDoll;
//}
//public class NuttyDoll : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.NuttyDoll;
//}
//public class MorkylDoll : Talisman
//{
//    public override PotionKind talismanKind => PotionKind.MorkylDoll;
//}
public class FlorzporbDoll : Talisman
{
    public override PotionKind talismanKind => PotionKind.FlorzporbDoll;
    public override TalismanRarity talismanRarity => TalismanRarity.Rare;
    public override double EffectValue(long level)
    {
        return 0.10d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.01d * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.skillCtrl.baseAttackSlimeBall[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));
    }
}
public class ArachnettaDoll : Talisman
{
    public override PotionKind talismanKind => PotionKind.ArachnettaDoll;
    public override TalismanRarity talismanRarity => TalismanRarity.Rare;
    public override double EffectValue(long level)
    {
        return 0.01d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.01d * level;
    }
    public override void SetPassiveEffect()
    {
        game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.skillCtrl.baseAttackPoisonChance[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));
    }
}
public class GuardianKorDoll : Talisman
{
    public override PotionKind talismanKind => PotionKind.GuardianKorDoll;
    public override TalismanRarity talismanRarity => TalismanRarity.Rare;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.01d * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.heroes[(int)heroKind].golemInvalidDamageHpPercent.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));
    }
}
public class SlimeBadge : Talisman//変更箇所
{
    public override PotionKind talismanKind => PotionKind.SlimeBadge;
    //Rarityの設定
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    //効果を調整
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    //効果を調整
    public override double PassiveEffectValue(long level)
    {
        return 1.0 * level;
    }
    //Passiveのセット
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class MagicslimeBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.MagicslimeBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.1 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class SpiderBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.SpiderBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.1 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class BatBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.BatBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.1 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class FairyBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.FairyBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.1 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class FoxBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.FoxBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.5 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class DevilfishBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.DevilfishBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.0005d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.025 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.ResourceGain(ResourceKind.Stone).RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class TreantBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.TreantBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.005d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.025 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.ResourceGain(ResourceKind.Crystal).RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class FlametigerBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.FlametigerBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.005d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.025 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.ResourceGain(ResourceKind.Leaf).RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class UnicornBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.UnicornBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Common;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.1 * level;
    }
    public override void SetPassiveEffect()
    {
        game.statsCtrl.SetEffect(BasicStatsKind.SPD, new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Add, () => passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class AscendedFromIEH1 : Talisman
{
    public override PotionKind talismanKind => PotionKind.AscendedFromIEH1;
    public override TalismanRarity talismanRarity => TalismanRarity.Epic;
    public override double EffectValue(long level)
    {
        return 0.10d * level;
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}

public class WarriorsBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.WarriorsBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.01d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => - passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class WizardsBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.WizardsBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.01d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => -passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class AngelsBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.AngelsBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.01d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => -passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class ThiefsBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.ThiefsBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.001d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => -passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.EquipmentDropChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Mul, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}

public class ArchersBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.ArchersBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.01d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => -passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.CriticalDamage).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
public class TamersBadge : Talisman
{
    public override PotionKind talismanKind => PotionKind.TamersBadge;
    public override TalismanRarity talismanRarity => TalismanRarity.Uncommon;
    public override double EffectValue(long level)
    {
        return 0.05d * level;
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Min(0.9999, 0.0001d * level);
    }
    public override void SetPassiveEffect()
    {
        game.skillCtrl.skillRankCostFactors[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.TalismanPassive, MultiplierType.Mul, () => -passiveEffectValue));
    }
    public override void SetEffect(HeroKind heroKind, Func<double> equipNum)
    {
        game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Talisman, MultiplierType.Add, () => effectValue * equipNum(), () => IsActiveEffect(heroKind, equipNum)));

    }
}
