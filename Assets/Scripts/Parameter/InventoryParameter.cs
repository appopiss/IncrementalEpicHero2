using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryParameter
{
    public static readonly int equipMaxDropNum = 30;//フィールド上にドロップする最大数
    public static readonly int resourceMaxDropNum = 30;//フィールド上にドロップする最大数
    public static readonly int materialMaxDropNum = 30;//フィールド上にドロップする最大数
    public static readonly int chestMaxDropNum = 10;
    //0~259をInventory, 260~331をWarriorEQ,332~403をWizardEQ,...とする。
    public static readonly int equipPartSlotId = 24;//各部位最大24まで
    public static readonly int equipInventorySlotId = 260;//EquipmentInventorySlotを260個までとする
    public static readonly int allEquipmentSlotId = equipInventorySlotId + equipPartSlotId * Enum.GetNames(typeof(EquipmentPart)).Length * Enum.GetNames(typeof(HeroKind)).Length;
    public static readonly int enchantSlotId = 26 * 5;
    public static readonly int potionSlotId = 26 * 10;//PotionInventorySlot
    public static readonly int equipPotionSlotId = 6;
    //0~129をPotionInventory, 130~135をWarriorEQPotion, 136~をWiz...とする。
    public static readonly int allPotionSlotId = potionSlotId + equipPotionSlotId * Enum.GetNames(typeof(HeroKind)).Length;
}

public class DictionaryParameter
{
    public static long MaxLevel(DictionaryUpgradeKind kind)
    {
        switch (kind)
        {
            case DictionaryUpgradeKind.EquipmentProficiencyGainWarrior:
                return 20;
            case DictionaryUpgradeKind.EquipmentProficiencyGainWizard:
                return 20;
            case DictionaryUpgradeKind.EquipmentProficiencyGainAngel:
                return 20;
            case DictionaryUpgradeKind.EquipmentProficiencyGainThief:
                return 20;
            case DictionaryUpgradeKind.EquipmentProficiencyGainArcher:
                return 20;
            case DictionaryUpgradeKind.EquipmentProficiencyGainTamer:
                return 20;
            case DictionaryUpgradeKind.EquipmentDropChance:
                return 20;
            case DictionaryUpgradeKind.EnchantedEffectChance1:
                return 20;
            case DictionaryUpgradeKind.EnchantedEffectChance2:
                return 20;
            case DictionaryUpgradeKind.EnchantedEffectChance3:
                return 20;
        }
        return 0;
    }

    public static (double initCost, double baseCost, bool isLinier) UpgradeCost(DictionaryUpgradeKind kind)
    {
        switch (kind)
        {
            case DictionaryUpgradeKind.EquipmentProficiencyGainWarrior:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentProficiencyGainWizard:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentProficiencyGainAngel:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentProficiencyGainThief:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentProficiencyGainArcher:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentProficiencyGainTamer:
                return (2, 1.5, false);
            case DictionaryUpgradeKind.EquipmentDropChance:
                return (5, 5, true);
            case DictionaryUpgradeKind.EnchantedEffectChance1:
                return (10, 10, true);
            case DictionaryUpgradeKind.EnchantedEffectChance2:
                return (20, 20, true);
            case DictionaryUpgradeKind.EnchantedEffectChance3:
                return (30, 30, true);
        }
        return (1, 1, false);
    }

    public static double EffectValue(DictionaryUpgradeKind kind, long level)
    {
        switch (kind)
        {
            case DictionaryUpgradeKind.EquipmentProficiencyGainWarrior:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentProficiencyGainWizard:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentProficiencyGainAngel:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentProficiencyGainThief:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentProficiencyGainArcher:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentProficiencyGainTamer:
                return 0.05 * level;
            case DictionaryUpgradeKind.EquipmentDropChance:
                return 0.0025 * level / 100d;
            case DictionaryUpgradeKind.EnchantedEffectChance1:
                return 0.5 * level / 100d;//0.5%
            case DictionaryUpgradeKind.EnchantedEffectChance2:
                return 0.05 * level / 100d;
            case DictionaryUpgradeKind.EnchantedEffectChance3:
                return 0.005 * level / 100d;
        }
        return 0;
    }
}

public class EquipmentParameter
{
    public static int maxOptionEffectNum = 7;//6;//Drop時の3 + ExpandScrollによる1 + ThiefMasteryによる3
    public static int maxLevelForEachHero = 10;
    public static EquipmentRarity Rarity(EquipmentKind kind)
    {
        switch (kind)
        {
            case EquipmentKind.Nothing:
                return EquipmentRarity.Common;
            case EquipmentKind.DullSword:
                return EquipmentRarity.Common;
            case EquipmentKind.BrittleStaff:
                return EquipmentRarity.Common;
            case EquipmentKind.FlimsyWing:
                return EquipmentRarity.Common;
            case EquipmentKind.WornDart:
                return EquipmentRarity.Common;
            case EquipmentKind.SmallBow:
                return EquipmentRarity.Common;
            case EquipmentKind.WoodenRecorder:
                return EquipmentRarity.Common;
            case EquipmentKind.OldCloak:
                return EquipmentRarity.Common;
            case EquipmentKind.BlueHat:
                return EquipmentRarity.Common;
            case EquipmentKind.OrangeHat:
                return EquipmentRarity.Common;
            case EquipmentKind.ClothShoes:
                return EquipmentRarity.Common;
            case EquipmentKind.IronRing:
                return EquipmentRarity.Common;
            case EquipmentKind.TShirt:
                return EquipmentRarity.Common;
            case EquipmentKind.ClothGlove:
                return EquipmentRarity.Common;
            case EquipmentKind.BlueTShirt:
                return EquipmentRarity.Common;
            case EquipmentKind.OrangeTShirt:
                return EquipmentRarity.Common;
            case EquipmentKind.ClothBelt:
                return EquipmentRarity.Common;
            case EquipmentKind.PearlEarring:
                return EquipmentRarity.Common;
            case EquipmentKind.FireBrooch:
                return EquipmentRarity.Common;
            case EquipmentKind.IceBrooch:
                return EquipmentRarity.Common;
            case EquipmentKind.ThunderBrooch:
                return EquipmentRarity.Common;
            case EquipmentKind.LightBrooch:
                return EquipmentRarity.Common;
            case EquipmentKind.DarkBrooch:
                return EquipmentRarity.Common;
            case EquipmentKind.WoodenShield:
                return EquipmentRarity.Common;
            case EquipmentKind.LongSword:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LongStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.WarmWing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.DualDagger:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.ReinforcedBow:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.Flute:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.FireStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.IceStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.ThunderStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LeatherBelt:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LeatherShoes:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.WarmCloak:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LeatherArmor:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LeatherGlove:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LeatherShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.FireRing:
                return EquipmentRarity.Common;
            case EquipmentKind.IceRing:
                return EquipmentRarity.Common;
            case EquipmentKind.ThunderRing:
                return EquipmentRarity.Common;
            case EquipmentKind.LightRing:
                return EquipmentRarity.Common;
            case EquipmentKind.DarkRing:
                return EquipmentRarity.Common;
            case EquipmentKind.EnhancedFireBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.EnhancedIceBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.EnhancedThunderBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.EnhancedLightBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.EnhancedDarkBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleSword:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleWing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleDagger:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleBow:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleRecorder:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SlimeSword:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimeGlove:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimeRing:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimeBelt:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimePincenez:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimeWing:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimePoncho:
                return EquipmentRarity.Rare;
            case EquipmentKind.SlimeDart:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeStick:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeHat:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeBow:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeShoes:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeRecorder:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeEarring:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeBalloon:
                return EquipmentRarity.Rare;
            case EquipmentKind.MagicSlimeSkirt:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderHat:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderSkirt:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderSuit:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderDagger:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderWing:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderCatchingNet:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderStick:
                return EquipmentRarity.Rare;
            case EquipmentKind.SpiderFoldingFan:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatRing:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatShoes:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatSword:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatHat:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatRecorder:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatBow:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatMascaradeMask:
                return EquipmentRarity.Rare;
            case EquipmentKind.BatCloak:
                return EquipmentRarity.Rare;
            case EquipmentKind.BronzeShoulder:
                return EquipmentRarity.Common;
            case EquipmentKind.BattleRing:
                return EquipmentRarity.Common;
            case EquipmentKind.Halo:
                return EquipmentRarity.Common;
            case EquipmentKind.IronShoulder:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.StrengthRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenFireRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenIceRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenThunderRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenLightRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.GoldenDarkRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.IronBelt:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.IronShoes:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.CopperArmor:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.IronGlove:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.TowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.FireTowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.IceTowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.ThunderTowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LightTowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.DarkTowerShield:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SavageRing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SpellboundFireBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SpellboundIceBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SpellboundThunderBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SpellboundLightBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.SpellboundDarkBrooch:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.CopperHelm:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.BattleHelm:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.WizardHelm:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeSword:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeStaff:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeWing:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeDagger:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeBow:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.LargeOcarina:
                return EquipmentRarity.Uncommon;
            case EquipmentKind.FairyClothes:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyStaff:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyBoots:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyGlove:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyBrooch:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyLamp:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyWing:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FairyShuriken:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxKanzashi:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxLoincloth:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxMask:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxHamayayumi:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxHat:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxCoat:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxBoot:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.FoxEma:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishSword:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishWing:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishRecorder:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishArmor:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishScarf:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishGill:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishPendant:
                return EquipmentRarity.SuperRare;
            case EquipmentKind.DevilfishRing:
                return EquipmentRarity.SuperRare;
        }
        return EquipmentRarity.Common;
    }
    public static EquipmentPart Part(EquipmentKind kind)
    {
        switch (kind)
        {
            case EquipmentKind.Nothing:
                return EquipmentPart.Weapon;
            case EquipmentKind.DullSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.BrittleStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.FlimsyWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.WornDart:
                return EquipmentPart.Weapon;
            case EquipmentKind.SmallBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.WoodenRecorder:
                return EquipmentPart.Weapon;
            case EquipmentKind.OldCloak:
                return EquipmentPart.Armor;
            case EquipmentKind.BlueHat:
                return EquipmentPart.Armor;
            case EquipmentKind.OrangeHat:
                return EquipmentPart.Armor;
            case EquipmentKind.ClothShoes:
                return EquipmentPart.Armor;
            case EquipmentKind.IronRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.TShirt:
                return EquipmentPart.Armor;
            case EquipmentKind.ClothGlove:
                return EquipmentPart.Armor;
            case EquipmentKind.BlueTShirt:
                return EquipmentPart.Armor;
            case EquipmentKind.OrangeTShirt:
                return EquipmentPart.Armor;
            case EquipmentKind.ClothBelt:
                return EquipmentPart.Armor;
            case EquipmentKind.PearlEarring:
                return EquipmentPart.Jewelry;
            case EquipmentKind.FireBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.IceBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.ThunderBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.LightBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.DarkBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.WoodenShield:
                return EquipmentPart.Armor;
            case EquipmentKind.LongSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.LongStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.WarmWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.DualDagger:
                return EquipmentPart.Weapon;
            case EquipmentKind.ReinforcedBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.Flute:
                return EquipmentPart.Weapon;
            case EquipmentKind.FireStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.IceStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.ThunderStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.LeatherBelt:
                return EquipmentPart.Armor;
            case EquipmentKind.LeatherShoes:
                return EquipmentPart.Armor;
            case EquipmentKind.WarmCloak:
                return EquipmentPart.Armor;
            case EquipmentKind.LeatherArmor:
                return EquipmentPart.Armor;
            case EquipmentKind.LeatherGlove:
                return EquipmentPart.Armor;
            case EquipmentKind.LeatherShield:
                return EquipmentPart.Armor;
            case EquipmentKind.FireRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.IceRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.ThunderRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.LightRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.DarkRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.EnhancedFireBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.EnhancedIceBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.EnhancedThunderBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.EnhancedLightBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.EnhancedDarkBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.BattleSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.BattleStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.BattleWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.BattleDagger:
                return EquipmentPart.Weapon;
            case EquipmentKind.BattleBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.BattleRecorder:
                return EquipmentPart.Weapon;
            case EquipmentKind.SlimeSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.SlimeGlove:
                return EquipmentPart.Armor;
            case EquipmentKind.SlimeRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SlimeBelt:
                return EquipmentPart.Armor;
            case EquipmentKind.SlimePincenez:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SlimeWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.SlimePoncho:
                return EquipmentPart.Armor;
            case EquipmentKind.SlimeDart:
                return EquipmentPart.Weapon;
            case EquipmentKind.MagicSlimeStick:
                return EquipmentPart.Weapon;
            case EquipmentKind.MagicSlimeHat:
                return EquipmentPart.Armor;
            case EquipmentKind.MagicSlimeBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.MagicSlimeShoes:
                return EquipmentPart.Armor;
            case EquipmentKind.MagicSlimeRecorder:
                return EquipmentPart.Weapon;
            case EquipmentKind.MagicSlimeEarring:
                return EquipmentPart.Jewelry;
            case EquipmentKind.MagicSlimeBalloon:
                return EquipmentPart.Jewelry;
            case EquipmentKind.MagicSlimeSkirt:
                return EquipmentPart.Armor;
            case EquipmentKind.SpiderHat:
                return EquipmentPart.Armor;
            case EquipmentKind.SpiderSkirt:
                return EquipmentPart.Armor;
            case EquipmentKind.SpiderSuit:
                return EquipmentPart.Armor;
            case EquipmentKind.SpiderDagger:
                return EquipmentPart.Weapon;
            case EquipmentKind.SpiderWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.SpiderCatchingNet:
                return EquipmentPart.Weapon;
            case EquipmentKind.SpiderStick:
                return EquipmentPart.Weapon;
            case EquipmentKind.SpiderFoldingFan:
                return EquipmentPart.Jewelry;
            case EquipmentKind.BatRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.BatShoes:
                return EquipmentPart.Armor;
            case EquipmentKind.BatSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.BatHat:
                return EquipmentPart.Armor;
            case EquipmentKind.BatRecorder:
                return EquipmentPart.Weapon;
            case EquipmentKind.BatBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.BatMascaradeMask:
                return EquipmentPart.Jewelry;
            case EquipmentKind.BatCloak:
                return EquipmentPart.Armor;
            case EquipmentKind.BronzeShoulder:
                return EquipmentPart.Armor;
            case EquipmentKind.BattleRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.Halo:
                return EquipmentPart.Jewelry;
            case EquipmentKind.IronShoulder:
                return EquipmentPart.Armor;
            case EquipmentKind.StrengthRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenFireRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenIceRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenThunderRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenLightRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.GoldenDarkRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.IronBelt:
                return EquipmentPart.Armor;
            case EquipmentKind.IronShoes:
                return EquipmentPart.Armor;
            case EquipmentKind.CopperArmor:
                return EquipmentPart.Armor;
            case EquipmentKind.IronGlove:
                return EquipmentPart.Armor;
            case EquipmentKind.TowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.FireTowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.IceTowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.ThunderTowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.LightTowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.DarkTowerShield:
                return EquipmentPart.Armor;
            case EquipmentKind.SavageRing:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SpellboundFireBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SpellboundIceBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SpellboundThunderBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SpellboundLightBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.SpellboundDarkBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.CopperHelm:
                return EquipmentPart.Armor;
            case EquipmentKind.BattleHelm:
                return EquipmentPart.Armor;
            case EquipmentKind.WizardHelm:
                return EquipmentPart.Armor;
            case EquipmentKind.LargeSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.LargeStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.LargeWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.LargeDagger:
                return EquipmentPart.Weapon;
            case EquipmentKind.LargeBow:
                return EquipmentPart.Weapon;
            case EquipmentKind.LargeOcarina:
                return EquipmentPart.Weapon;
            case EquipmentKind.FairyClothes:
                return EquipmentPart.Armor;
            case EquipmentKind.FairyStaff:
                return EquipmentPart.Weapon;
            case EquipmentKind.FairyBoots:
                return EquipmentPart.Armor;
            case EquipmentKind.FairyGlove:
                return EquipmentPart.Armor;
            case EquipmentKind.FairyBrooch:
                return EquipmentPart.Jewelry;
            case EquipmentKind.FairyLamp:
                return EquipmentPart.Jewelry;
            case EquipmentKind.FairyWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.FairyShuriken:
                return EquipmentPart.Weapon;
            case EquipmentKind.FoxKanzashi:
                return EquipmentPart.Jewelry;
            case EquipmentKind.FoxLoincloth:
                return EquipmentPart.Armor;
            case EquipmentKind.FoxMask:
                return EquipmentPart.Jewelry;
            case EquipmentKind.FoxHamayayumi:
                return EquipmentPart.Weapon;
            case EquipmentKind.FoxHat:
                return EquipmentPart.Armor;
            case EquipmentKind.FoxCoat:
                return EquipmentPart.Armor;
            case EquipmentKind.FoxBoot:
                return EquipmentPart.Armor;
            case EquipmentKind.FoxEma:
                return EquipmentPart.Jewelry;
            case EquipmentKind.DevilfishSword:
                return EquipmentPart.Weapon;
            case EquipmentKind.DevilfishWing:
                return EquipmentPart.Weapon;
            case EquipmentKind.DevilfishRecorder:
                return EquipmentPart.Weapon;
            case EquipmentKind.DevilfishArmor:
                return EquipmentPart.Armor;
            case EquipmentKind.DevilfishScarf:
                return EquipmentPart.Armor;
            case EquipmentKind.DevilfishGill:
                return EquipmentPart.Jewelry;
            case EquipmentKind.DevilfishPendant:
                return EquipmentPart.Jewelry;
            case EquipmentKind.DevilfishRing:
                return EquipmentPart.Jewelry;
        }
        return EquipmentPart.Weapon;
    }
    public static int RarityFactor(EquipmentEffectKind kind)
    {
        switch (kind)
        {
            case EquipmentEffectKind.Nothing:
                return 1;
            case EquipmentEffectKind.HPAdder:
                return 1;
            case EquipmentEffectKind.MPAdder:
                return 1;
            case EquipmentEffectKind.ATKAdder:
                return 1;
            case EquipmentEffectKind.MATKAdder:
                return 1;
            case EquipmentEffectKind.DEFAdder:
                return 1;
            case EquipmentEffectKind.MDEFAdder:
                return 1;
            case EquipmentEffectKind.SPDAdder:
                return 5;
            case EquipmentEffectKind.HPMultiplier:
                return 5;
            case EquipmentEffectKind.MPMultiplier:
                return 5;
            case EquipmentEffectKind.ATKMultiplier:
                return 5;
            case EquipmentEffectKind.MATKMultiplier:
                return 5;
            case EquipmentEffectKind.DEFMultiplier:
                return 5;
            case EquipmentEffectKind.MDEFMultiplier:
                return 5;
            case EquipmentEffectKind.ATKPropotion:
                return 3;
            case EquipmentEffectKind.MATKPropotion:
                return 3;
            case EquipmentEffectKind.DEFPropotion:
                return 3;
            case EquipmentEffectKind.MDEFPropotion:
                return 3;
            case EquipmentEffectKind.FireResistance:
                return 2;
            case EquipmentEffectKind.IceResistance:
                return 2;
            case EquipmentEffectKind.ThunderResistance:
                return 2;
            case EquipmentEffectKind.LightResistance:
                return 2;
            case EquipmentEffectKind.DarkResistance:
                return 2;
            case EquipmentEffectKind.PhysicalAbsorption:
                return 4;
            case EquipmentEffectKind.FireAbsorption:
                return 4;
            case EquipmentEffectKind.IceAbsorption:
                return 4;
            case EquipmentEffectKind.ThunderAbsorption:
                return 4;
            case EquipmentEffectKind.LightAbsorption:
                return 4;
            case EquipmentEffectKind.DarkAbsorption:
                return 4;
            case EquipmentEffectKind.DebuffResistance:
                return 3;
            case EquipmentEffectKind.PhysicalCritical:
                return 2;
            case EquipmentEffectKind.MagicalCritical:
                return 2;
            case EquipmentEffectKind.EXPGain:
                return 10;
            case EquipmentEffectKind.SkillProficiency:
                return 10;
            case EquipmentEffectKind.EquipmentProficiency:
                return 10;
            case EquipmentEffectKind.MoveSpeedAdder:
                return 3;
            case EquipmentEffectKind.MoveSpeedMultiplier:
                return 10;
            case EquipmentEffectKind.GoldGain:
                return 5;
            case EquipmentEffectKind.StoneGain:
                return 2;
            case EquipmentEffectKind.CrystalGain:
                return 2;
            case EquipmentEffectKind.LeafGain:
                return 2;
            case EquipmentEffectKind.WarriorSkillLevel:
                return 4;
            case EquipmentEffectKind.WizardSkillLevel:
                return 4;
            case EquipmentEffectKind.AngelSkillLevel:
                return 4;
            case EquipmentEffectKind.ThiefSkillLevel:
                return 4;
            case EquipmentEffectKind.ArcherSkillLevel:
                return 4;
            case EquipmentEffectKind.TamerSkillLevel:
                return 4;
            case EquipmentEffectKind.AllSkillLevel:
                return 5;
            case EquipmentEffectKind.SlimeKnowledge:
                return 10;
            case EquipmentEffectKind.MagicSlimeKnowledge:
                return 20;
            case EquipmentEffectKind.SpiderKnowledge:
                return 30;
            case EquipmentEffectKind.BatKnowledge:
                return 40;
            case EquipmentEffectKind.FairyKnowledge:
                return 50;
            case EquipmentEffectKind.FoxKnowledge:
                return 60;
            case EquipmentEffectKind.DevilFishKnowledge:
                return 70;
            case EquipmentEffectKind.TreantKnowledge:
                return 80;
            case EquipmentEffectKind.FlameTigerKnowledge:
                return 90;
            case EquipmentEffectKind.UnicornKnowledge:
                return 100;
            case EquipmentEffectKind.PhysicalDamage:
                return 5;
            case EquipmentEffectKind.FireDamage:
                return 5;
            case EquipmentEffectKind.IceDamage:
                return 5;
            case EquipmentEffectKind.ThunderDamage:
                return 5;
            case EquipmentEffectKind.LightDamage:
                return 5;
            case EquipmentEffectKind.DarkDamage:
                return 5;
            case EquipmentEffectKind.EquipmentDropChance:
                return 10;
            case EquipmentEffectKind.SlimeDropChance:
                return 10;
            case EquipmentEffectKind.MagicSlimeDropChance:
                return 20;
            case EquipmentEffectKind.SpiderDropChance:
                return 30;
            case EquipmentEffectKind.BatDropChance:
                return 40;
            case EquipmentEffectKind.FairyDropChance:
                return 50;
            case EquipmentEffectKind.FoxDropChance:
                return 60;
            case EquipmentEffectKind.DevilFishDropChance:
                return 70;
            case EquipmentEffectKind.TreantDropChance:
                return 80;
            case EquipmentEffectKind.FlameTigerDropChance:
                return 90;
            case EquipmentEffectKind.UnicornDropChance:
                return 100;
            case EquipmentEffectKind.ColorMaterialDropChance:
                return 10;
            case EquipmentEffectKind.HpRegen:
                return 10;
            case EquipmentEffectKind.MpRegen:
                return 10;
            case EquipmentEffectKind.TamingPoint:
                return 10;
            case EquipmentEffectKind.WarriorSkillRange:
                return 100;
            case EquipmentEffectKind.WizardSkillRange:
                return 100;
            case EquipmentEffectKind.AngelSkillRange:
                return 100;
            case EquipmentEffectKind.ThiefSkillRange:
                return 100;
            case EquipmentEffectKind.ArcherSkillRange:
                return 100;
            case EquipmentEffectKind.TamerSkillRange:
                return 100;
            case EquipmentEffectKind.TownMatGain:
                return 100;
            case EquipmentEffectKind.TownMatAreaClearGain:
                return 100;
            //case EquipmentEffectKind.TownMatDungeonRewardGain:
            //    return 100;
            case EquipmentEffectKind.RebirthPointGain1:
                return 100;
            case EquipmentEffectKind.RebirthPointGain2:
                return 100;
            case EquipmentEffectKind.RebirthPointGain3:
                return 100;
            case EquipmentEffectKind.CriticalDamage:
                return 10;
            case EquipmentEffectKind.BlessingEffect:
                return 100;
            default:
                return 10;
        }
    }
    public static long MaxLevel(EquipmentEffectKind kind)
    {
        switch (kind)
        {
            case EquipmentEffectKind.HPAdder:
                return 10;
            case EquipmentEffectKind.MPAdder:
                return 10;
            case EquipmentEffectKind.ATKAdder:
                return 10;
            case EquipmentEffectKind.MATKAdder:
                return 10;
            case EquipmentEffectKind.DEFAdder:
                return 10;
            case EquipmentEffectKind.MDEFAdder:
                return 10;
            case EquipmentEffectKind.SPDAdder:
                return 10;
            case EquipmentEffectKind.HPMultiplier:
                return 5;
            case EquipmentEffectKind.MPMultiplier:
                return 5;
            case EquipmentEffectKind.ATKMultiplier:
                return 5;
            case EquipmentEffectKind.MATKMultiplier:
                return 5;
            case EquipmentEffectKind.DEFMultiplier:
                return 5;
            case EquipmentEffectKind.MDEFMultiplier:
                return 5;
            case EquipmentEffectKind.ATKPropotion:
                return 5;
            case EquipmentEffectKind.MATKPropotion:
                return 5;
            case EquipmentEffectKind.DEFPropotion:
                return 5;
            case EquipmentEffectKind.MDEFPropotion:
                return 5;
            case EquipmentEffectKind.FireResistance:
                return 10;
            case EquipmentEffectKind.IceResistance:
                return 10;
            case EquipmentEffectKind.ThunderResistance:
                return 10;
            case EquipmentEffectKind.LightResistance:
                return 10;
            case EquipmentEffectKind.DarkResistance:
                return 10;
            case EquipmentEffectKind.PhysicalAbsorption:
                return 2;
            case EquipmentEffectKind.FireAbsorption:
                return 2;
            case EquipmentEffectKind.IceAbsorption:
                return 2;
            case EquipmentEffectKind.ThunderAbsorption:
                return 2;
            case EquipmentEffectKind.LightAbsorption:
                return 2;
            case EquipmentEffectKind.DarkAbsorption:
                return 2;
            case EquipmentEffectKind.DebuffResistance:
                return 5;
            case EquipmentEffectKind.PhysicalCritical:
                return 2;
            case EquipmentEffectKind.MagicalCritical:
                return 2;
            case EquipmentEffectKind.EXPGain:
                return 10;//もともと5でした
            case EquipmentEffectKind.SkillProficiency:
                return 5;
            case EquipmentEffectKind.EquipmentProficiency:
                return 5;
            case EquipmentEffectKind.MoveSpeedAdder:
                return 10;
            case EquipmentEffectKind.MoveSpeedMultiplier:
                return 10;
            case EquipmentEffectKind.GoldGain:
                return 5;
            case EquipmentEffectKind.StoneGain:
                return 10;
            case EquipmentEffectKind.CrystalGain:
                return 10;
            case EquipmentEffectKind.LeafGain:
                return 10;
            case EquipmentEffectKind.WarriorSkillLevel:
                return 10;
            case EquipmentEffectKind.WizardSkillLevel:
                return 10;
            case EquipmentEffectKind.AngelSkillLevel:
                return 10;
            case EquipmentEffectKind.ThiefSkillLevel:
                return 10;
            case EquipmentEffectKind.ArcherSkillLevel:
                return 10;
            case EquipmentEffectKind.TamerSkillLevel:
                return 10;
            case EquipmentEffectKind.AllSkillLevel:
                return 10;
            case EquipmentEffectKind.SlimeKnowledge:
                return 10;
            case EquipmentEffectKind.MagicSlimeKnowledge:
                return 10;
            case EquipmentEffectKind.SpiderKnowledge:
                return 10;
            case EquipmentEffectKind.BatKnowledge:
                return 10;
            case EquipmentEffectKind.FairyKnowledge:
                return 10;
            case EquipmentEffectKind.FoxKnowledge:
                return 10;
            case EquipmentEffectKind.DevilFishKnowledge:
                return 10;
            case EquipmentEffectKind.TreantKnowledge:
                return 10;
            case EquipmentEffectKind.FlameTigerKnowledge:
                return 10;
            case EquipmentEffectKind.UnicornKnowledge:
                return 10;
            case EquipmentEffectKind.PhysicalDamage:
                return 10;
            case EquipmentEffectKind.FireDamage:
                return 10;
            case EquipmentEffectKind.IceDamage:
                return 10;
            case EquipmentEffectKind.ThunderDamage:
                return 10;
            case EquipmentEffectKind.LightDamage:
                return 10;
            case EquipmentEffectKind.DarkDamage:
                return 10;
            case EquipmentEffectKind.EquipmentDropChance:
                return 10;
            case EquipmentEffectKind.SlimeDropChance:
                return 10;
            case EquipmentEffectKind.MagicSlimeDropChance:
                return 10;
            case EquipmentEffectKind.SpiderDropChance:
                return 10;
            case EquipmentEffectKind.BatDropChance:
                return 10;
            case EquipmentEffectKind.FairyDropChance:
                return 10;
            case EquipmentEffectKind.FoxDropChance:
                return 10;
            case EquipmentEffectKind.DevilFishDropChance:
                return 10;
            case EquipmentEffectKind.TreantDropChance:
                return 10;
            case EquipmentEffectKind.FlameTigerDropChance:
                return 10;
            case EquipmentEffectKind.UnicornDropChance:
                return 10;
            case EquipmentEffectKind.ColorMaterialDropChance:
                return 10;
            case EquipmentEffectKind.HpRegen:
                return 5;
            case EquipmentEffectKind.MpRegen:
                return 5;
            case EquipmentEffectKind.TamingPoint:
                return 10;
            case EquipmentEffectKind.WarriorSkillRange:
                return 1;
            case EquipmentEffectKind.WizardSkillRange:
                return 1;
            case EquipmentEffectKind.AngelSkillRange:
                return 1;
            case EquipmentEffectKind.ThiefSkillRange:
                return 1;
            case EquipmentEffectKind.ArcherSkillRange:
                return 1;
            case EquipmentEffectKind.TamerSkillRange:
                return 1;
            case EquipmentEffectKind.TownMatGain:
                return 10;
            case EquipmentEffectKind.TownMatAreaClearGain:
                return 10;
            //case EquipmentEffectKind.TownMatDungeonRewardGain:
            //    return 10;
            case EquipmentEffectKind.RebirthPointGain1:
                return 10;
            case EquipmentEffectKind.RebirthPointGain2:
                return 10;
            case EquipmentEffectKind.RebirthPointGain3:
                return 10;
            case EquipmentEffectKind.CriticalDamage:
                return 2;
            case EquipmentEffectKind.BlessingEffect:
                return 10;
            default:
                return 1;
        }
    }

    public static double EffectCalculation(EquipmentEffectKind kind, long level)
    {
        switch (kind)
        {
            case EquipmentEffectKind.HPAdder:
                return 10 * Math.Pow(level, 2);
            case EquipmentEffectKind.MPAdder:
                return 5 * Math.Pow(level, 2);
            case EquipmentEffectKind.ATKAdder:
                return Math.Pow(level, 2);
            case EquipmentEffectKind.MATKAdder:
                return Math.Pow(level, 2);
            case EquipmentEffectKind.DEFAdder:
                return Math.Pow(level, 2);
            case EquipmentEffectKind.MDEFAdder:
                return Math.Pow(level, 2);
            case EquipmentEffectKind.SPDAdder:
                return Math.Pow(level, 2);
            case EquipmentEffectKind.HPMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MPMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.ATKMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MATKMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.DEFMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MDEFMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.ATKPropotion:
                return 10 * level / 100d;
            case EquipmentEffectKind.MATKPropotion:
                return 10 * level / 100d;
            case EquipmentEffectKind.DEFPropotion:
                return 10 * level / 100d;
            case EquipmentEffectKind.MDEFPropotion:
                return 10 * level / 100d;
            case EquipmentEffectKind.FireResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.IceResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.ThunderResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.LightResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.DarkResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.PhysicalAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.FireAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.IceAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.ThunderAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.LightAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.DarkAbsorption:
                return Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.DebuffResistance:
                return 5 * level / 100d;
            case EquipmentEffectKind.PhysicalCritical:
                return 3 * level / 100d;
            case EquipmentEffectKind.MagicalCritical:
                return 3 * level / 100d;
            case EquipmentEffectKind.CriticalDamage:
                return 5 * level * (level + 1) / 100d;//10%~30%
            case EquipmentEffectKind.EXPGain:
                return 1d * level * (level + 1) / 100d;//2%~
            case EquipmentEffectKind.SkillProficiency:
                return 1d * level * (level + 1) / 100d;//2%~
            case EquipmentEffectKind.EquipmentProficiency:
                return 1d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MoveSpeedAdder:
                return level * (level + 1);
            case EquipmentEffectKind.MoveSpeedMultiplier:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.GoldGain:
                return 1d * level * (level + 1) / 100d;
            case EquipmentEffectKind.StoneGain:
                return 5 * Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.CrystalGain:
                return 5 * Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.LeafGain:
                return 5 * Math.Pow(level, 2) / 100d;
            case EquipmentEffectKind.WarriorSkillLevel:
                return level;
            case EquipmentEffectKind.WizardSkillLevel:
                return level;
            case EquipmentEffectKind.AngelSkillLevel:
                return level;
            case EquipmentEffectKind.ThiefSkillLevel:
                return level;
            case EquipmentEffectKind.ArcherSkillLevel:
                return level;
            case EquipmentEffectKind.TamerSkillLevel:
                return level;
            case EquipmentEffectKind.AllSkillLevel:
                return level;
            case EquipmentEffectKind.SlimeKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MagicSlimeKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.SpiderKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.BatKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FairyKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FoxKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.DevilFishKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.TreantKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FlameTigerKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.UnicornKnowledge:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.PhysicalDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FireDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.IceDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.ThunderDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.LightDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.DarkDamage:
                return 0.5d * level * (level + 1) / 100d;
            case EquipmentEffectKind.EquipmentDropChance:
                return 0.001d * level * (level + 1) / 100d;
            case EquipmentEffectKind.SlimeDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.MagicSlimeDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.SpiderDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.BatDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FairyDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FoxDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.DevilFishDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.TreantDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.FlameTigerDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.UnicornDropChance:
                return 0.01d * level * (level + 1) / 100d;
            case EquipmentEffectKind.ColorMaterialDropChance:
                return 0.001d * level * (level + 1) / 100d;
            case EquipmentEffectKind.HpRegen:
                return 5 * level;
            case EquipmentEffectKind.MpRegen:
                return 10 * level * (level + 1);
            case EquipmentEffectKind.TamingPoint:
                return 0.05d * level;
            case EquipmentEffectKind.WarriorSkillRange:
                return 10;
            case EquipmentEffectKind.WizardSkillRange:
                return 10;
            case EquipmentEffectKind.AngelSkillRange:
                return 10;
            case EquipmentEffectKind.ThiefSkillRange:
                return 10;
            case EquipmentEffectKind.ArcherSkillRange:
                return 10;
            case EquipmentEffectKind.TamerSkillRange:
                return 10;
            case EquipmentEffectKind.TownMatGain:
                return 0.01d * level;
            case EquipmentEffectKind.TownMatAreaClearGain:
                return level;
            //case EquipmentEffectKind.TownMatDungeonRewardGain:
            //    return 0.01d * level;
            case EquipmentEffectKind.RebirthPointGain1:
                return 0.01d * level;
            case EquipmentEffectKind.RebirthPointGain2:
                return 0.01d * level;
            case EquipmentEffectKind.RebirthPointGain3:
                return 0.01d * level;
            case EquipmentEffectKind.BlessingEffect:
                return 0.01d * level;
            default:
                return 0;
        }
    }
    public static long RequiredLevelIncrement(EquipmentEffectKind kind, long level)
    {
        if (kind == EquipmentEffectKind.Nothing) return 0;
        return (100 / MaxLevel(kind)) * level * RarityFactor(kind);
    }

    public static double areaUniqueDropChanceBase = 0.00001;//0.001%
}

public class PotionParameter
{
    public static readonly long maxLevel = 50;
    public static readonly long upgradeMaxLevel = 999;
    public static readonly long catalystMaxLevel = 50;
}