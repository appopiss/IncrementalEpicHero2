using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using static EquipmentParameter;
using static MaterialKind;
using static Localized;

public partial class SaveR
{
    public long[] craftEnchantScrollLevels;//[EquipmentEffectKind]
}

public class CRAFT
{
    public CRAFT(CraftController craftCtrl)
    {
        this.craftCtrl = craftCtrl;
        craftedNum = new INTEGER();
        transaction = new Transaction(craftedNum);
        unlock = new Unlock();
        transaction.SetAdditionalBuyCondition(CanCraft);
        transaction.additionalBuyActionWithLevelIncrement = CraftAction;
        transaction.isBuyOne = true;
        //SetCostは派生先で呼ぶ
    }
    public virtual void SetCost() { }
    public INTEGER craftedNum;
    public Transaction transaction;
    public Unlock unlock;
    public CraftController craftCtrl;
    public virtual bool CanCraft(long num) { return true; }
    public virtual void CraftAction(long num) { }
    public virtual string NameString() { return ""; }
    public virtual string CostString() { return transaction.DescriptionString(); }
}

//LevelUpScroll,DeleteScrollなど
public class CRAFT_SCROLL : CRAFT
{
    public CRAFT_SCROLL(CraftController craftCtrl) : base(craftCtrl)
    {
    }
    public virtual EnchantKind enchantKind { get => EnchantKind.Nothing; }
    public EquipmentEffectKind kind;
    public CraftEnchantScrollLevel level;
    public override bool CanCraft(long num)
    {
        if (!unlock.IsUnlocked()) return false;
        if (!game.inventoryCtrl.CanCreateEnchant(num)) return false;
        return true;
    }
    public override void CraftAction(long num)
    {
        game.inventoryCtrl.CreateEnchant(enchantKind);
    }
    public override string NameString()
    {
        return localized.EnchantName(enchantKind);
    }
}

//EnchantScroll(OptionAdd)
public class CraftEnchantScroll : CRAFT_SCROLL
{
    public CraftEnchantScroll(CraftController craftCtrl, EquipmentEffectKind kind) : base(craftCtrl)
    {
        this.kind = kind;
        SetCost();
    }
    public override EnchantKind enchantKind => EnchantKind.OptionAdd;
    public override void SetCost()
    {
        level = new CraftEnchantScrollLevel(this);
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => EnchantedShardCost());
        switch (kind)
        {
            case EquipmentEffectKind.HPAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(OilOfSlime), (x) => Cost());
                break;
            case EquipmentEffectKind.MPAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedCloth), (x) => Cost());
                break;
            case EquipmentEffectKind.ATKAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(BatWing), (x) => Cost());
                break;
            case EquipmentEffectKind.MATKAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(FairyDust), (x) => Cost());
                break;
            case EquipmentEffectKind.DEFAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(OilOfSlime), (x) => Cost()/2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost()/2);
                break;
            case EquipmentEffectKind.MDEFAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedCloth), (x) => Cost()/2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost()/2);
                break;
            case EquipmentEffectKind.SPDAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(UnicornHorn), (x) => Cost());
                break;
            case EquipmentEffectKind.HPMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(OilOfSlime), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.MPMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedCloth), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.ATKMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(BatWing), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.MATKMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(FairyDust), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.DEFMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(OilOfSlime), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.MDEFMultiplier:
                transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedCloth), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(CarvedBranch), (x) => Cost(true));
                break;
            case EquipmentEffectKind.ATKPropotion:
                transaction.SetAnotherResource(game.materialCtrl.Material(BatWing), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(FoxTail), (x) => Cost(true));
                break;
            case EquipmentEffectKind.MATKPropotion:
                transaction.SetAnotherResource(game.materialCtrl.Material(FairyDust), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(FoxTail), (x) => Cost(true));
                break;
            case EquipmentEffectKind.DEFPropotion:
                transaction.SetAnotherResource(game.materialCtrl.Material(OilOfSlime), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(FoxTail), (x) => Cost(true));
                break;
            case EquipmentEffectKind.MDEFPropotion:
                transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedCloth), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost() / 2);
                transaction.SetAnotherResource(game.materialCtrl.Material(FoxTail), (x) => Cost(true));
                break;
            case EquipmentEffectKind.FireResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(FlameShard), (x) => Cost(true));
                break;
            case EquipmentEffectKind.IceResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(FrostShard), (x) => Cost(true));
                break;
            case EquipmentEffectKind.ThunderResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(LightningShard), (x) => Cost(true));
                break;
            case EquipmentEffectKind.LightResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(NatureShard), (x) => Cost(true));
                break;
            case EquipmentEffectKind.DarkResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(PoisonShard), (x) => Cost(true));
                break;
            case EquipmentEffectKind.PhysicalAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.FireAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(FlameShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.IceAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(FrostShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.ThunderAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(LightningShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.LightAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(NatureShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.DarkAbsorption:
                transaction.SetAnotherResource(game.materialCtrl.Material(PoisonShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.DebuffResistance:
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost());
                break;
            //case EquipmentEffectKind.EquipmentDropChance:
            //    break;
            //case EquipmentEffectKind.SlimeDropChance:
            //    break;
            //case EquipmentEffectKind.MagicSlimeDropChance:
            //    break;
            //case EquipmentEffectKind.SpiderDropChance:
            //    break;
            //case EquipmentEffectKind.BatDropChance:
            //    break;
            //case EquipmentEffectKind.FairyDropChance:
            //    break;
            //case EquipmentEffectKind.FoxDropChance:
            //    break;
            //case EquipmentEffectKind.DevilFishDropChance:
            //    break;
            //case EquipmentEffectKind.TreantDropChance:
            //    break;
            //case EquipmentEffectKind.FlameTigerDropChance:
            //    break;
            //case EquipmentEffectKind.UnicornDropChance:
            //    break;
            //case EquipmentEffectKind.ColorMaterialDropChance:
            //    break;
            case EquipmentEffectKind.PhysicalCritical:
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BatWing), (x) => Cost());
                break;
            case EquipmentEffectKind.MagicalCritical:
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(FairyDust), (x) => Cost());
                break;
            case EquipmentEffectKind.EXPGain:
                transaction.SetAnotherResource(game.materialCtrl.Material(FoxTail), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.SkillProficiency:
                transaction.SetAnotherResource(game.materialCtrl.Material(UnicornHorn), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                break;
            case EquipmentEffectKind.EquipmentProficiency:
                transaction.SetAnotherResource(game.materialCtrl.Material(UnicornHorn), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.MoveSpeedAdder:
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                break;
            case EquipmentEffectKind.GoldGain:
                transaction.SetAnotherResource(game.materialCtrl.Material(FishScales), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.StoneGain:
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost());
                break;
            case EquipmentEffectKind.CrystalGain:
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost());
                break;
            case EquipmentEffectKind.LeafGain:
                transaction.SetAnotherResource(game.materialCtrl.Material(SpiderSilk), (x) => Cost());
                break;
            case EquipmentEffectKind.WarriorSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.WizardSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.AngelSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.ThiefSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.ArcherSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.TamerSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                break;
            case EquipmentEffectKind.AllSkillLevel:
                transaction.SetAnotherResource(game.materialCtrl.Material(ThickFur), (x) => Cost());
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true));// / 5);
                break;
            //case EquipmentEffectKind.SlimeKnowledge:
            //    break;
            //case EquipmentEffectKind.MagicSlimeKnowledge:
            //    break;
            //case EquipmentEffectKind.SpiderKnowledge:
            //    break;
            //case EquipmentEffectKind.BatKnowledge:
            //    break;
            //case EquipmentEffectKind.FairyKnowledge:
            //    break;
            //case EquipmentEffectKind.FoxKnowledge:
            //    break;
            //case EquipmentEffectKind.DevilFishKnowledge:
            //    break;
            //case EquipmentEffectKind.TreantKnowledge:
            //    break;
            //case EquipmentEffectKind.FlameTigerKnowledge:
            //    break;
            //case EquipmentEffectKind.UnicornKnowledge:
            //    break;
            case EquipmentEffectKind.PhysicalDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(MonsterFluid), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.FireDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(FlameShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.IceDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(FrostShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.ThunderDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(LightningShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.LightDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(NatureShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
            case EquipmentEffectKind.DarkDamage:
                transaction.SetAnotherResource(game.materialCtrl.Material(PoisonShard), (x) => Cost(true));
                transaction.SetAnotherResource(game.materialCtrl.Material(BlackPearl), (x) => Cost(true) / 5);
                break;
        }
    }
    double EnchantedShardCost() { return Math.Max(1, Math.Ceiling(RarityFactor(kind) * level.value * CostIncrementFactorPerLevel() * (1 - craftCtrl.costReduction.Value()))); }
    double CostIncrementFactorPerLevel() { return  10d / MaxLevel(kind); }//1 or 2 or 5
    double CostExponential(double baseValue = 10) { return Math.Pow(baseValue, level.value * CostIncrementFactorPerLevel()); }
    double Cost(bool isRareMaterial = false) { return Math.Ceiling(RarityFactor(kind) * 5 * Math.Pow(5, Convert.ToInt16(!isRareMaterial)) * CostExponential(1.5d + 0.5d * RarityFactor(kind)) * (1 - craftCtrl.costReduction.Value())); }
    public override void CraftAction(long num)
    {
        game.inventoryCtrl.CreateEnchant(enchantKind, kind, level.value);
    }
    public override string NameString()
    {
        return localized.EquipmentEffectName(kind);
    }
}

public class CraftEnchantScrollLevel : INTEGER
{
    CraftEnchantScroll scroll;
    EquipmentEffectKind kind => scroll.kind;
    public CraftEnchantScrollLevel(CraftEnchantScroll scroll)
    {
        this.scroll = scroll;
        minValue = () => 1;
        maxValue = () => MaxLevel(kind);
        value = (long)Mathf.Clamp(value, minValue(), maxValue());
    }
    public override long value { get => main.SR.craftEnchantScrollLevels[(int)kind]; set => main.SR.craftEnchantScrollLevels[(int)kind] = value; }
}
