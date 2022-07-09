using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GuildParameter
{
    public static readonly long maxGuildLevel = 300;
    public static double RequiredExp(long level)
    {
        return Math.Floor(
            500d * (level + 1)
            + 50d * Math.Pow(level, 2)
            + 500d * Math.Pow(level / 5d, 3)
            + 2000d * Math.Pow(level / 10d, 6)
            + 25000d * Math.Pow(level / 20d, 9)
            + 300000d * Math.Pow(level / 30d, 12)
            );
    }
    public static double ExpGainPerHeroLevel(long level)
    {
        return 200d + level * 5d;
    }
    public static long MemberUnlockLevel(HeroKind heroKind)
    {
        switch (heroKind)
        {
            case HeroKind.Warrior: return 0;
            case HeroKind.Wizard: return 5;
            case HeroKind.Angel: return 10;
            case HeroKind.Thief: return 30;
            case HeroKind.Archer: return 45;// 50;
            case HeroKind.Tamer: return 60;// 70;
        }
        return 100;
    }
    public static (long unlockLevel, long maxLevel, double initCost, double baseCost, bool isLinear) Ability(GuildAbilityKind kind)//init, base
    {
        switch (kind)
        {
            //case GuildAbilityKind.Member:
            //    return (0, 5, 5, 0, true);//cost:5,5,5,5,5
            case GuildAbilityKind.StoneGain:
                return (0, 100, 1, 0, true);
            case GuildAbilityKind.CrystalGain:
                return (0, 100, 1, 0, true);
            case GuildAbilityKind.LeafGain:
                return (0, 100, 1, 0, true);
            case GuildAbilityKind.GuildExpGain:
                return (0, 10, 1, 0, true);
            case GuildAbilityKind.EquipmentInventory:
                return (0, 10, 1, 0, true);
            case GuildAbilityKind.EnchantInventory:
                return (0, 10, 1, 0, true);
            case GuildAbilityKind.PotionInventory:
                return (0, 10, 1, 0, true);
            case GuildAbilityKind.MysteriousWater:
                return (0, 10, 1, 0, true);
            case GuildAbilityKind.GlobalSkillSlot:
                return (0, 3, 5, 4, false);//5,20,80
            case GuildAbilityKind.SkillProficiency:
                return (0, 10, 1, 1, true);

            //WA以降
            case GuildAbilityKind.GoldCap://WA1
                return (0, 10, 1, 1, true);
            case GuildAbilityKind.GoldGain://WA1
                return (0, 10, 1, 1, true);
            case GuildAbilityKind.Trapping:
                return (15, 10, 1, 1, true);
            case GuildAbilityKind.UpgradeCost:
                return (15, 10, 1, 1, true);

            case GuildAbilityKind.EquipmentProficiency:
                return (15, 10, 5, 0, true);
            case GuildAbilityKind.PhysicalAbsorption:
                return (15, 10, 1, 0, true);
            case GuildAbilityKind.MagicalAbsoption:
                return (15, 10, 1, 0, true);
            //case GuildAbilityKind.BonusAbilityPointRebirth:
            //    return (15, 20, 5, 0, true);
            case GuildAbilityKind.MaterialDrop:
                return (15, 10, 5, 0, true);
            case GuildAbilityKind.NitroCap:
                return (15, 10, 2, 0, true);
            case GuildAbilityKind.ExpGain:
                return (15, 100, 1, 0, true);
            //case GuildAbilityKind.ActiveNum:
            //    return (15, 5, 20, 2, false);//cost:20,40,80,160,320
            //case GuildAbilityKind.HP:
            //    return (20, 20, 1, 0, true);
            //case GuildAbilityKind.MP:
            //    return (20, 20, 1, 0, true);
            //case GuildAbilityKind.ATK:
            //    return (25, 20, 1, 0, true);
            //case GuildAbilityKind.MATK:
            //    return (25, 20, 1, 0, true);
            //case GuildAbilityKind.DEFMDEF:
            //    return (30, 20, 1, 0, true);
            //case GuildAbilityKind.SPD:
            //    return (40, 20, 1, 0, true);
            //case GuildAbilityKind.ElementResistance:
            //    return (50, 20, 1, 0, true);
            //case GuildAbilityKind.MoveSpeed:
            //    return (60, 20, 2, 0, true);
            //case GuildAbilityKind.EquipDropChance:
            //    return (70, 20, 2, 0, true);
            default:
                return (500, 1, 100, 100, true);
        }
    }

    public static double AbilityEffectValue(GuildAbilityKind kind, long level)
    {
        switch (kind)
        {
            case GuildAbilityKind.GlobalSkillSlot:
                return level;
            case GuildAbilityKind.EquipmentInventory:
                return 3 * level;
            case GuildAbilityKind.EnchantInventory:
                return level;
            case GuildAbilityKind.PotionInventory:
                return 2 * level;
            case GuildAbilityKind.GuildExpGain:
                return level * 0.1d;
            case GuildAbilityKind.MysteriousWater:
                return 0.1d * level;
            case GuildAbilityKind.StoneGain:
                return level;
            case GuildAbilityKind.CrystalGain:
                return level;
            case GuildAbilityKind.LeafGain:
                return level;
            case GuildAbilityKind.SkillProficiency:
                return 0.1d * level;


            case GuildAbilityKind.GoldCap:
                return 0.10d * level;
            case GuildAbilityKind.GoldGain:
                return 0.10d * level;
            case GuildAbilityKind.Trapping:
                return 0.05d * level;
            case GuildAbilityKind.UpgradeCost:
                return 0.05d * level;

            case GuildAbilityKind.EquipmentProficiency:
                return 0.2d * level;
            case GuildAbilityKind.PhysicalAbsorption:
                return 0.01d * level;
            case GuildAbilityKind.MagicalAbsoption:
                return 0.01d * level;
            //case GuildAbilityKind.BonusAbilityPointRebirth:
            //    return 5 * level;
            case GuildAbilityKind.MaterialDrop:
                return level;
            case GuildAbilityKind.NitroCap:
                return 1000 * level;
            case GuildAbilityKind.ExpGain:
                return 0.10d * level;
            default:
                break;
        }
        return 0;
    }
}
