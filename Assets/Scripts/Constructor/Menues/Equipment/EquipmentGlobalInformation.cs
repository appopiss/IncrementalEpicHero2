using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static EquipmentParameter;
using static UsefulMethod;

public partial class SaveR
{
    public long[] equipmentLevelsWarrior;//[EquipmentKind]
    public long[] equipmentLevelsWizard;
    public long[] equipmentLevelsAngel;
    public long[] equipmentLevelsThief;
    public long[] equipmentLevelsArcher;
    public long[] equipmentLevelsTamer;

    public double[] equipmentProficiencyWarrior;
    public double[] equipmentProficiencyWizard;
    public double[] equipmentProficiencyAngel;
    public double[] equipmentProficiencyThief;
    public double[] equipmentProficiencyArcher;
    public double[] equipmentProficiencyTamer;

    public bool[] equipmentIsMaxedWarrior;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedWizard;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedAngel;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedThief;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedArcher;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedTamer;//[EquipmentKind]Lv10に到達したかどうか

    public bool[] equipmentIsAutoDisassemble;

    //統計
    public double[] disassembledEquipmentNums;//[EquipmentKind]
    public double[] townMatGainFromdisassemble;//[EquipmentKind]
}
public partial class Save
{
    //統計
    public double[] disassembledEquipmentNums;//[EquipmentKind]
    public double[] townMatGainFromdisassemble;//[EquipmentKind]

    public bool[] equipmentIsGotOnce;

    public bool[] equipmentIsMaxedWarrior;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedWizard;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedAngel;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedThief;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedArcher;//[EquipmentKind]Lv10に到達したかどうか
    public bool[] equipmentIsMaxedTamer;//[EquipmentKind]Lv10に到達したかどうか
}

public class EquipmentGlobalInformation
{
    public EquipmentGlobalInformation(EquipmentKind kind)
    {
        this.kind = kind;
        for (int i = 0; i < levels.Length; i++)
        {
            int count = i;
            levels[i] = new EquipmentLevel(kind, (HeroKind)count);
            proficiencies[i] = new EquipmentProficiency(kind, (HeroKind)count, (level) => RequiredProficiency((HeroKind)count, level), levels[count]);
        }
        SetEffectAndRequiredAbility();
        SetLevelMaxEffect();
    }

    //以下、Equipment追加作業で追加する項目
    //1.Enumを追加
    public EquipmentKind kind;
    //2.Rarityを設定
    public EquipmentRarity rarity { get => Rarity(kind); }
    //3.Partを設定
    public EquipmentPart part { get => Part(kind); }
    //4.EffectとRequiredAbilityを設定（ここまで）
    public void SetEffectAndRequiredAbility()
    {
        switch (kind)
        {
            case EquipmentKind.Nothing:
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                break;
            case EquipmentKind.DullSword:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 4, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 3));
                break;
            case EquipmentKind.BrittleStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 4, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 3));
                break;
            case EquipmentKind.FlimsyWing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 2, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 2, () => 0.5));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 2));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 2));
                break;
            case EquipmentKind.WornDart:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 2.5, () => 0.50));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 4, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 0.1d, () => 0.025d));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 3));
                break;
            case EquipmentKind.SmallBow:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 2.5, () => 0.50));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 4, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 0.1d, () => 0.025d));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 3));
                break;
            case EquipmentKind.WoodenRecorder:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 2.5, () => 0.50));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 2, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 2, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 0.1d, () => 0.025d));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 2));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 2));
                break;
            case EquipmentKind.OldCloak:
                //effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 20, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 10, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(1));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 3));
                break;
            case EquipmentKind.BlueHat:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 1, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(5));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 3));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 2));
                break;
            case EquipmentKind.OrangeHat:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 1, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(5));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 3));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 2));
                break;
            case EquipmentKind.TShirt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 20, () => 5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 0.5, () => 0.5));
                requiredAbilities.Add(new EquipmentRequiredAbility(5));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                break;
            case EquipmentKind.ClothGlove:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 2.5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalCritical, () => 0.005, () => 0.00025));
                requiredAbilities.Add(new EquipmentRequiredAbility(10));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 5));
                break;
            case EquipmentKind.BlueTShirt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 30, () => 3));
                requiredAbilities.Add(new EquipmentRequiredAbility(15));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                break;
            case EquipmentKind.OrangeTShirt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 20, () => 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(15));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 5));
                break;
            case EquipmentKind.ClothBelt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.01, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(5));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 3));
                break;
            case EquipmentKind.ClothShoes:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedAdder, () => 5, () => 0.25));//() => 0.020, () => 0.0010));
                requiredAbilities.Add(new EquipmentRequiredAbility(5));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 5));
                break;
            case EquipmentKind.IronRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.01, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(10));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                break;
            case EquipmentKind.FireRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireResistance, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.IceRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceResistance, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.ThunderRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderResistance, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.LightRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightResistance, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.DarkRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkResistance, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.PearlEarring:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 20, () => 4));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.01, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(10));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 5));
                break;
            case EquipmentKind.FireBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.IceBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.ThunderBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.LightBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.DarkBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.BattleRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(20));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                break;
            case EquipmentKind.WoodenShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 20, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(15));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 5));
                break;
            case EquipmentKind.BronzeShoulder:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 20, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(15));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 5));
                break;
            case EquipmentKind.Halo:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HpRegen, () => 5, () => 0.25));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 5));
                break;

            //Uncommon
            case EquipmentKind.LongSword:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WarriorSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                break;
            case EquipmentKind.LongStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WizardSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.WarmWing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.AngelSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.DualDagger:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 8, () => 0.8d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 2, () => 0.2d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThiefSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                break;
            case EquipmentKind.ReinforcedBow:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 2, () => 0.2d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 8, () => 0.8d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ArcherSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.Flute:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamerSkillLevel, () => 2, () => 1 / 10d));
                requiredAbilities.Add(new EquipmentRequiredAbility(30));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.FireStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.1, () => 0.005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                break;
            case EquipmentKind.IceStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.1, () => 0.005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                break;
            case EquipmentKind.ThunderStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.1, () => 0.005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                break;
            case EquipmentKind.LeatherBelt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 5));
                break;
            case EquipmentKind.LeatherShoes:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 10, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedAdder, () => 10, () => 0.5));// () => 0.050, () => 0.0010));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 20));
                break;
            case EquipmentKind.WarmCloak:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 100, () => 25));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 25, () => 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                break;
            case EquipmentKind.LeatherArmor:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 20, () => 2));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.LeatherGlove:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicalCritical, () => 0.005, () => 0.0005));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 5));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 15));
                break;
            case EquipmentKind.LeatherShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 20, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalAbsorption, () => 0.01, () => 0.0005));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 10));
                break;
            case EquipmentKind.EnhancedFireBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.EnhancedIceBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.EnhancedThunderBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.EnhancedLightBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.EnhancedDarkBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.StrengthRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.15, () => 0.0050));
                requiredAbilities.Add(new EquipmentRequiredAbility(60));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 50));
                break;
            case EquipmentKind.BattleSword:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 20, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.01, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.10, () => 0.005));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 60));
                break;
            case EquipmentKind.BattleStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 20, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.01, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.10, () => 0.005));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 60));
                break;
            case EquipmentKind.BattleWing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 10, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.005, () => 0.00025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.005, () => 0.00025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.05, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                break;
            case EquipmentKind.BattleDagger:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 15, () => 1.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalCritical, () => 0.05, () => 0.0005));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.BattleBow:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 5, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 15, () => 1.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicalCritical, () => 0.05, () => 0.0005));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.BattleRecorder:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalCritical, () => 0.025, () => 0.00025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicalCritical, () => 0.025, () => 0.00025));
                requiredAbilities.Add(new EquipmentRequiredAbility(80));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.IronShoulder:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 25, () => 5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 25, () => 1));
                requiredAbilities.Add(new EquipmentRequiredAbility(35));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 15));
                break;
            case EquipmentKind.GoldenRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.GoldenFireRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireResistance, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.GoldenIceRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceResistance, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.GoldenThunderRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderResistance, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.GoldenLightRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightResistance, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.GoldenDarkRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.020, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkResistance, () => 0.10, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.IronBelt:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFPropotion, () => 0.1, () => 0.01));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 50));
                break;
            case EquipmentKind.IronShoes:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedAdder, () => 20, () => 0.5));//() => 0.10, () => 0.0020));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 100));
                break;
            case EquipmentKind.CopperArmor:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 40, () => 4));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 4));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 60));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.IronGlove:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFPropotion, () => 0.1, () => 0.01));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                break;
            case EquipmentKind.TowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.FireTowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.IceTowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.ThunderTowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.LightTowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.DarkTowerShield:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 40, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkAbsorption, () => 0.02, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(120));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.SavageRing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 150));
                break;
            case EquipmentKind.SpellboundFireBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.SpellboundIceBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.SpellboundThunderBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.SpellboundLightBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.SpellboundDarkBrooch:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.20, () => 0.0075));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.CopperHelm:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 250, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.01, () => 0.001));
                requiredAbilities.Add(new EquipmentRequiredAbility(70));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                break;
            case EquipmentKind.BattleHelm:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.025, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(70));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 30));
                break;
            case EquipmentKind.WizardHelm:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.025, () => 0.0025));
                requiredAbilities.Add(new EquipmentRequiredAbility(70));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                break;
            case EquipmentKind.LargeSword:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 30, () => 3));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.25, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WarriorSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.LargeStaff:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 30, () => 3));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.25, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WizardSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.LargeWing:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.AngelSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.LargeDagger:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThiefSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.LargeBow:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.20, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ArcherSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.LargeOcarina:
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.10, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamerSkillLevel, () => 5, () => 1 / 10d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 1, () => 0.2));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;

            //SetItem
            case EquipmentKind.SlimeSword:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 15, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 20));
                break;
            case EquipmentKind.SlimeGlove:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 10));
                break;
            case EquipmentKind.SlimeRing:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 0.5, () => 0.1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 0.5, () => 0.1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 0.5, () => 0.1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.SlimeBelt:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 5, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DebuffResistance, () => 0.1, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 10));
                break;
            case EquipmentKind.SlimePincenez:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.SlimeWing:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.005, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.005, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.20, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.AngelSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                break;
            case EquipmentKind.SlimePoncho:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 100, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.050, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                break;
            case EquipmentKind.SlimeDart:
                setKind = EquipmentSetKind.Slime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.20, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.20, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 1, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(25));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 10));
                break;
            case EquipmentKind.MagicSlimeStick:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 15, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.01, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.MagicSlimeHat:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 10, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                break;
            case EquipmentKind.MagicSlimeBow:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireDamage, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 1.00, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.MagicSlimeShoes:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireResistance, () => 0.1, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceResistance, () => 0.1, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderResistance, () => 0.1, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 15));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 5));
                break;
            case EquipmentKind.MagicSlimeRecorder:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 1, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamerSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 10));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.MagicSlimeEarring:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.10, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.MagicSlimeBalloon:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ColorMaterialDropChance, () => 0.000150, () => 0.000001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.MagicSlimeSkirt:
                setKind = EquipmentSetKind.Magicslime;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicSlimeDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(50));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 20));
                break;
            case EquipmentKind.SpiderHat:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 500, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFPropotion, () => 0.1, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 40));
                break;
            case EquipmentKind.SpiderSkirt:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkResistance, () => 0.1, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 10));
                break;
            case EquipmentKind.SpiderSuit:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 500, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFPropotion, () => 0.1, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                break;
            case EquipmentKind.SpiderDagger:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 30, () => 3));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.20, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThiefSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 80));
                break;
            case EquipmentKind.SpiderWing:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalCritical, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicalCritical, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalDamage, () => 0.20, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.20, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 30));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 20));
                break;
            case EquipmentKind.SpiderCatchingNet:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.50, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.SpiderStick:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.25, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ColorMaterialDropChance, () => 0.000100, () => 0.000001));//() => 0.000100, () => 0.000001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WizardSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.SpiderFoldingFan:
                setKind = EquipmentSetKind.Spider;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.1, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SpiderDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(100));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.BatRing:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DebuffResistance, () => 0.1, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 1, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 1, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 1, () => 0.2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 60));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.BatShoes:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedAdder, () => 20, () => 1));//() => 0.10, () => 0.0020));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 100));
                break;
            case EquipmentKind.BatSword:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 50, () => 5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WarriorSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 60));
                break;
            case EquipmentKind.BatHat:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 40));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 60));
                break;
            case EquipmentKind.BatRecorder:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamerSkillLevel, () => 10, () => 1 / 4d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 100));
                break;
            case EquipmentKind.BatBow:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.20, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ArcherSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 100));
                break;
            case EquipmentKind.BatMascaradeMask:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.20, () => 0.01d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 60));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 40));
                break;
            case EquipmentKind.BatCloak:
                setKind = EquipmentSetKind.Bat;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 2));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalAbsorption, () => 0.025, () => 0.0005d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BatDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(150));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 20));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 60));
                break;
            case EquipmentKind.FairyClothes:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TownMatGain, () => 0.05, () => 0.0025));
                //effects.Add(new EquipmentEffect(EquipmentEffectKind.TownMatAreaClearGain, () => 5, () => 0.1d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
            case EquipmentKind.FairyStaff:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 2, () => 0.2d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.5, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 100));
                break;
            case EquipmentKind.FairyBoots:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedAdder, () => 20, () => 1));//() => 0.10, () => 0.0020));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightResistance, () => 0.1, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 100));
                break;
            case EquipmentKind.FairyGlove:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.15, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 100));
                break;
            case EquipmentKind.FairyBrooch:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.30, () => 0.015));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 250));
                break;
            case EquipmentKind.FairyLamp:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TownMatAreaClearGain, () => 2, () => 0.1d));
                //effects.Add(new EquipmentEffect(EquipmentEffectKind.TownMatGain, () => 0.05, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
            case EquipmentKind.FairyWing:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 2, () => 0.2d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.25, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.25, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.FairyShuriken:
                setKind = EquipmentSetKind.Fairy;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 2, () => 0.2d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.10, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.5, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FairyDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(200));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 100));
                break;
            case EquipmentKind.FoxKanzashi:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BlessingEffect, () => 0.10, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
            case EquipmentKind.FoxLoincloth:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.50, () => 0.025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => -0.200, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => -0.200, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
            case EquipmentKind.FoxMask:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MpRegen, () => 100, () => 10));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalAbsorption, () => 0.02, () => 0.0001d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.FoxHamayayumi:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CriticalDamage, () => 0.20, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ArcherSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DebuffResistance, () => 0.05, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Agility, 50));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.FoxHat:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 500, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.050, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HpRegen, () => 10, () => 1));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 350));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 50));
                break;
            case EquipmentKind.FoxCoat:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 100));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 100));
                break;
            case EquipmentKind.FoxBoot:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MoveSpeedMultiplier, () => -0.20, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.50, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ThunderDamage, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LightDamage, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 200));
                break;
            case EquipmentKind.FoxEma:
                setKind = EquipmentSetKind.Fox;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.BlessingEffect, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.EquipmentDropChance, () => 0.000100, () => 0.000005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FoxDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(250));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 400));
                break;
            case EquipmentKind.DevilfishSword:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => -0.200, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => -0.200, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.PhysicalCritical, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MagicalCritical, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CriticalDamage, () => 0.10, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.WarriorSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
            case EquipmentKind.DevilfishWing:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.025, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.05, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.AngelSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 200));
                break;
            case EquipmentKind.DevilfishRecorder:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.50, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkDamage, () => 0.25, () => 0.0050));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => -0.20, () => 0.002));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.TamerSkillLevel, () => 5, () => 1 / 5d));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 200));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 400));
                break;
            case EquipmentKind.DevilfishArmor:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 50, () => 5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 50, () => 5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.025, () => 0.001));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkResistance, () => 0.15, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 600));
                break;
            case EquipmentKind.DevilfishScarf:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DEFPropotion, () => 0.1, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.025, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.MDEFPropotion, () => 0.1, () => 0.01));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 300));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Strength, 150));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 150));
                break;
            case EquipmentKind.DevilfishGill:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceDamage, () => 0.05, () => 0.0025));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.FireAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.IceAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DarkAbsorption, () => 0.025, () => 0.0005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 350));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Intelligence, 250));
                break;
            case EquipmentKind.DevilfishPendant:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.20, () => 0.02));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 600));
                break;
            case EquipmentKind.DevilfishRing:
                setKind = EquipmentSetKind.Devilfish;
                effects.Add(new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 200, () => 20));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DebuffResistance, () => 0.15, () => 0.0015));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 3, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 3, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 3, () => 0.5));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishKnowledge, () => 0.2, () => 0.005));
                effects.Add(new EquipmentEffect(EquipmentEffectKind.DevilFishDropChance, () => 0.0005, () => 0.00005));
                requiredAbilities.Add(new EquipmentRequiredAbility(300));//Level
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Vitality, 400));
                requiredAbilities.Add(new EquipmentRequiredAbility(AbilityKind.Luck, 200));
                break;
        }
    }
    //mastery
    public void SetLevelMaxEffect()
    {
        //あとでpartごとにも追加する
        switch (rarity)
        {
            case EquipmentRarity.Common:
                switch (part)
                {
                    case EquipmentPart.Weapon:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.ATKAdder, () => 15, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MATKAdder, () => 15, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.AllSkillLevel, () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 20, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.05, () => 0, true);
                        break;
                    case EquipmentPart.Armor:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 100, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 50, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.HpRegen, () => 5, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.MpRegen, () => 20, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.05, () => 0, true);
                        break;
                    case EquipmentPart.Jewelry:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.StoneGain, () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.CrystalGain, () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.LeafGain, () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.TownMatAreaClearGain , () => 1, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.05, () => 0, true);
                        break;
                }
                break;
            case EquipmentRarity.Uncommon:
                switch (part)
                {
                    case EquipmentPart.Weapon:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.025d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 50, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.10, () => 0, true);
                        break;
                    case EquipmentPart.Armor:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.DEFAdder, () => 25, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MDEFAdder, () => 25, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 50, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.EquipmentProficiency, () => 0.025d, () => 0, true);
                        break;
                    case EquipmentPart.Jewelry:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.10d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.TownMatGain, () => 0.025, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.10, () => 0, true);
                        break;
                }
                break;
            case EquipmentRarity.Rare:
                switch (part)
                {
                    case EquipmentPart.Weapon:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.ATKPropotion, () => 0.25d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MATKPropotion, () => 0.25d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.05d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 100, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.25, () => 0, true);
                        break;
                    case EquipmentPart.Armor:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.DEFMultiplier, () => 0.10d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MDEFMultiplier, () => 0.10d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.HpRegen, () => 10, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 100, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.EquipmentProficiency, () => 0.05, () => 0, true);
                        break;
                    case EquipmentPart.Jewelry:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.HPMultiplier, () => 0.10, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MPMultiplier, () => 0.10, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.RebirthPointGain1, () => 0.025d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.TownMatAreaClearGain, () => 2, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.25, () => 0, true);
                        break;
                }
                break;
            case EquipmentRarity.SuperRare:
                switch (part)
                {
                    case EquipmentPart.Weapon:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.ATKMultiplier, () => 0.20d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MATKMultiplier, () => 0.20d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.CriticalDamage, () => 0.10d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 200, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.50, () => 0, true);
                        break;
                    case EquipmentPart.Armor:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.DEFPropotion, () => 0.25, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MDEFPropotion, () => 0.25, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.10, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);//Debug
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.SPDAdder, () => 200, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.EquipmentProficiency, () => 0.10, () => 0, true);
                        break;
                    case EquipmentPart.Jewelry:
                        levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.EXPGain, () => 0.25, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.SkillProficiency, () => 0.10, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.RebirthPointGain2, () => 0.025d, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 2, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.TownMatGain, () => 0.05, () => 0, true);
                        levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.TamingPoint, () => 0.50, () => 0, true);
                        break;
                }
                break;

            default:
                levelMaxEffects[(int)HeroKind.Warrior] = new EquipmentEffect(EquipmentEffectKind.HPAdder, () => 100, () => 0, true);
                levelMaxEffects[(int)HeroKind.Wizard] = new EquipmentEffect(EquipmentEffectKind.MPAdder, () => 50, () => 0, true);
                levelMaxEffects[(int)HeroKind.Angel] = new EquipmentEffect(EquipmentEffectKind.GoldGain, () => 0.10d, () => 0, true);
                levelMaxEffects[(int)HeroKind.Thief] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);//Debug
                levelMaxEffects[(int)HeroKind.Archer] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);
                levelMaxEffects[(int)HeroKind.Tamer] = new EquipmentEffect(EquipmentEffectKind.Nothing, () => 1, () => 0, true);
                break;
        }
    }

    public TownMaterial DisassembleMaterialKind()
    {
        TownMaterialKind tempKind = TownMaterialKind.MudBrick;
        switch (part)
        {
            case EquipmentPart.Weapon:
                switch (rarity)
                {
                    case EquipmentRarity.Common:
                        tempKind = TownMaterialKind.MudBrick;
                        break;
                    case EquipmentRarity.Uncommon:
                        tempKind = TownMaterialKind.LimestoneBrick;
                        break;
                    case EquipmentRarity.Rare:
                        tempKind = TownMaterialKind.MarbleBrick;
                        break;
                    case EquipmentRarity.SuperRare:
                        tempKind = TownMaterialKind.GraniteBrick;
                        break;
                    case EquipmentRarity.Epic:
                        tempKind = TownMaterialKind.BasaltBrick;
                        break;
                }
                break;
            case EquipmentPart.Armor:
                switch (rarity)
                {
                    case EquipmentRarity.Common:
                        tempKind = TownMaterialKind.PineLog;
                        break;
                    case EquipmentRarity.Uncommon:
                        tempKind = TownMaterialKind.MapleLog;
                        break;
                    case EquipmentRarity.Rare:
                        tempKind = TownMaterialKind.AshLog;
                        break;
                    case EquipmentRarity.SuperRare:
                        tempKind = TownMaterialKind.MahoganyLog;
                        break;
                    case EquipmentRarity.Epic:
                        tempKind = TownMaterialKind.RosewoodLog;
                        break;
                }
                break;
            case EquipmentPart.Jewelry:
                switch (rarity)
                {
                    case EquipmentRarity.Common:
                        tempKind = TownMaterialKind.JasperShard;
                        break;
                    case EquipmentRarity.Uncommon:
                        tempKind = TownMaterialKind.OpalShard;
                        break;
                    case EquipmentRarity.Rare:
                        tempKind = TownMaterialKind.OnyxShard;
                        break;
                    case EquipmentRarity.SuperRare:
                        tempKind = TownMaterialKind.JadeShard;
                        break;
                    case EquipmentRarity.Epic:
                        tempKind = TownMaterialKind.SapphireShard;
                        break;
                }
                break;
        }
        return game.townCtrl.TownMaterial(tempKind);
    }
    public double DisassembleMaterialNum()
    {
        return Math.Max(1, Math.Floor((TotalLevel() + requiredAbilities[0].requiredValue / 2d) / 4d));
    }
    public double CraftCostMaterialNum()
    {
        double tempValue = DisassembleMaterialNum() * 200d * (1 - game.craftCtrl.costReduction.Value());
        tempValue *= 1 + (int)rarity * 2;

        //Craftのコストが分解で得られるMatを下回らないようにする必要がある
        if (tempValue < DisassembleMaterialNum() * game.equipmentCtrl.disassembleMultiplier.Value() * 4)//オプションが最大3ついてる場合はx4なので。
            tempValue = DisassembleMaterialNum() * game.equipmentCtrl.disassembleMultiplier.Value() * 4;
        return tempValue;
    }

    public long TotalLevel()
    {
        long tempValue = 0;
        for (int i = 0; i < levels.Length; i++)
        {
            tempValue += levels[i].value;
        }
        return tempValue;
    }

    public double RequiredProficiency(HeroKind heroKind, long level)
    {
        return Math.Pow(3, (int)rarity) * (1d + 1.5d * (int)rarity) * 300 * (level * (1 + (int)rarity) + 1);
    }
    public double RequiredProficiency(HeroKind heroKind)
    {
        return RequiredProficiency(heroKind, levels[(int)heroKind].value);
    }
    public double ProficiencyPercent(HeroKind heroKind)
    {
        return proficiencies[(int)heroKind].value / RequiredProficiency(heroKind);
    }
    public double ProficiencyLeft(HeroKind heroKind)
    {
        return Math.Max(0, RequiredProficiency(heroKind) - proficiencies[(int)heroKind].value);
    }
    public string ProficiencyTimeLeftString(HeroKind heroKind)
    {
        return DoubleTimeToDate(ProficiencyLeft(heroKind) / game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).Value());
    }
    public EquipmentLevel[] levels = new EquipmentLevel[Enum.GetNames(typeof(HeroKind)).Length];
    public EquipmentProficiency[] proficiencies = new EquipmentProficiency[Enum.GetNames(typeof(HeroKind)).Length];
    public List<EquipmentEffect> effects = new List<EquipmentEffect>();
    public List<EquipmentRequiredAbility> requiredAbilities = new List<EquipmentRequiredAbility>();
    public EquipmentEffect[] levelMaxEffects = new EquipmentEffect[Enum.GetNames(typeof(HeroKind)).Length];
    public bool isGotOnce { get => main.S.equipmentIsGotOnce[(int)kind]; set => main.S.equipmentIsGotOnce[(int)kind] = value; }
    public EquipmentSetKind setKind;
    public bool isAutoDisassemble { get => main.SR.equipmentIsAutoDisassemble[(int)kind]; set => main.SR.equipmentIsAutoDisassemble[(int)kind] = value; }

    public void SwitchAutoDisassemble()
    {
        if (!isGotOnce) return;
        if (isAutoDisassemble)
        {
            isAutoDisassemble = false; return;
        }
        if (!game.equipmentCtrl.CanAssignAutoDisassemble()) return;
        isAutoDisassemble = true;
    }
}

public class EquipmentEffect
{
    public EquipmentEffect(EquipmentEffectKind kind, Func<double> initValue, Func<double> baseValue, bool isMaxEffect = false)
    {
        this.kind = kind;
        this.initValue = initValue;
        this.baseValue = baseValue;
        this.isMaxEffect = isMaxEffect;
    }
    public double EffectValue(long level)
    {
        if (kind == EquipmentEffectKind.Nothing) return Math.Floor(initValue());// * Math.Min(2, game.equipmentCtrl.effectMultiplier.Value()));
        if(isMaxEffect) return initValue() * Math.Max(1d, (double)level / 10d) * game.equipmentCtrl.effectMultiplier.Value();//Lv10以降はその分倍率上がる
        return (initValue() + baseValue() * level) * game.equipmentCtrl.effectMultiplier.Value();
    }
    public bool isMaxEffect;
    public EquipmentEffectKind kind;
    public Func<double> initValue;
    public Func<double> baseValue;
}

public class EquipmentRequiredAbility
{
    public EquipmentRequiredAbility(AbilityKind kind, long requiredValue)
    {
        this.kind = kind;
        this.requiredValue = requiredValue;
    }
    public EquipmentRequiredAbility(long requiredLevel)
    {
        isLevel = true;
        requiredValue = requiredLevel;
    }
    public bool IsEnough(HeroKind heroKind)
    {
        if (isLevel) return game.statsCtrl.LevelForEquipment(heroKind).Value() >= requiredValue;
        return game.statsCtrl.Ability(heroKind, kind).Point() >= requiredValue;
    }
    public AbilityKind kind;
    public long requiredValue;
    public bool isLevel;
}

public class EquipmentLevel : INTEGER
{
    public EquipmentLevel(EquipmentKind kind, HeroKind heroKind)
    {
        this.kind = kind;
        this.heroKind = heroKind;
        maxValue = () => (long)game.equipmentCtrl.maxLevels[(int)heroKind].Value();
    }
    public override void Increase(long increment)
    {
        if (IsMaxed()) return;
        base.Increase(increment);
        if (value < 10) return;
        isMaxed = true;
        if (!isMaxedThisRebirth && value >= 10)
        {
            game.equipmentCtrl.GetDictionaryPoint(kind);
            isMaxedThisRebirth = true;
        }
    }
    public EquipmentKind kind;
    public HeroKind heroKind;
    public bool isMaxed
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.S.equipmentIsMaxedWarrior[(int)kind];
                case HeroKind.Wizard:
                    return main.S.equipmentIsMaxedWizard[(int)kind];
                case HeroKind.Angel:
                    return main.S.equipmentIsMaxedAngel[(int)kind];
                case HeroKind.Thief:
                    return main.S.equipmentIsMaxedThief[(int)kind];
                case HeroKind.Archer:
                    return main.S.equipmentIsMaxedArcher[(int)kind];
                case HeroKind.Tamer:
                    return main.S.equipmentIsMaxedTamer[(int)kind];
            }
            return false;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.S.equipmentIsMaxedWarrior[(int)kind] = value;
                    break;
                case HeroKind.Wizard:
                    main.S.equipmentIsMaxedWizard[(int)kind] = value;
                    break;
                case HeroKind.Angel:
                    main.S.equipmentIsMaxedAngel[(int)kind] = value;
                    break;
                case HeroKind.Thief:
                    main.S.equipmentIsMaxedThief[(int)kind] = value;
                    break;
                case HeroKind.Archer:
                    main.S.equipmentIsMaxedArcher[(int)kind] = value;
                    break;
                case HeroKind.Tamer:
                    main.S.equipmentIsMaxedTamer[(int)kind] = value;
                    break;
            }
        }
    }
    public bool isMaxedThisRebirth
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.equipmentIsMaxedWarrior[(int)kind];
                case HeroKind.Wizard:
                    return main.SR.equipmentIsMaxedWizard[(int)kind];
                case HeroKind.Angel:
                    return main.SR.equipmentIsMaxedAngel[(int)kind];
                case HeroKind.Thief:
                    return main.SR.equipmentIsMaxedThief[(int)kind];
                case HeroKind.Archer:
                    return main.SR.equipmentIsMaxedArcher[(int)kind];
                case HeroKind.Tamer:
                    return main.SR.equipmentIsMaxedTamer[(int)kind];
            }
            return false;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.equipmentIsMaxedWarrior[(int)kind] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.equipmentIsMaxedWizard[(int)kind] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.equipmentIsMaxedAngel[(int)kind] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.equipmentIsMaxedThief[(int)kind] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.equipmentIsMaxedArcher[(int)kind] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.equipmentIsMaxedTamer[(int)kind] = value;
                    break;
            }
        }
    }
    public override long value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.equipmentLevelsWarrior[(int)kind];
                case HeroKind.Wizard:
                    return main.SR.equipmentLevelsWizard[(int)kind];
                case HeroKind.Angel:
                    return main.SR.equipmentLevelsAngel[(int)kind];
                case HeroKind.Thief:
                    return main.SR.equipmentLevelsThief[(int)kind];
                case HeroKind.Archer:
                    return main.SR.equipmentLevelsArcher[(int)kind];
                case HeroKind.Tamer:
                    return main.SR.equipmentLevelsTamer[(int)kind];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.equipmentLevelsWarrior[(int)kind] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.equipmentLevelsWizard[(int)kind] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.equipmentLevelsAngel[(int)kind] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.equipmentLevelsThief[(int)kind] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.equipmentLevelsArcher[(int)kind] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.equipmentLevelsTamer[(int)kind] = value;
                    break;
            }
        }
    }
}

public class EquipmentProficiency : EXP
{
    public EquipmentProficiency(EquipmentKind kind, HeroKind heroKind, Func<long, double> requiredProficiency, EquipmentLevel level)
    {
        this.kind = kind;
        this.heroKind = heroKind;
        this.requiredValue = requiredProficiency;
        this.level = level;
    }

    public override void Increase(double increment)
    {
        if (level.IsMaxed()) return;
        increment *= game.guildCtrl.Member(heroKind).gainRate;

        increment *= game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).Value();
        base.Increase(increment);
    }

    public EquipmentKind kind;
    public HeroKind heroKind;
    public override double value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.equipmentProficiencyWarrior[(int)kind];
                case HeroKind.Wizard:
                    return main.SR.equipmentProficiencyWizard[(int)kind];
                case HeroKind.Angel:
                    return main.SR.equipmentProficiencyAngel[(int)kind];
                case HeroKind.Thief:
                    return main.SR.equipmentProficiencyThief[(int)kind];
                case HeroKind.Archer:
                    return main.SR.equipmentProficiencyArcher[(int)kind];
                case HeroKind.Tamer:
                    return main.SR.equipmentProficiencyTamer[(int)kind];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.equipmentProficiencyWarrior[(int)kind] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.equipmentProficiencyWizard[(int)kind] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.equipmentProficiencyAngel[(int)kind] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.equipmentProficiencyThief[(int)kind] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.equipmentProficiencyArcher[(int)kind] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.equipmentProficiencyTamer[(int)kind] = value;
                    break;
            }
        }
    }

}

public enum EquipmentPart
{
    Weapon,
    Armor,
    Jewelry,
}
public enum EquipmentRarity
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    Epic,
}
public enum EquipmentSetKind
{
    Nothing,
    Slime,
    Magicslime,
    Spider,
    Bat,
    Fairy,
    Fox,
    Devilfish,
    Treant,
    Flametiger,
    Unicorn
}
public enum EquipmentKind
{
    Nothing,
    //Common
    DullSword,
    BrittleStaff,
    FlimsyWing,
    WornDart,
    SmallBow,
    WoodenRecorder,
    OldCloak,
    BlueHat,
    OrangeHat,
    TShirt,
    ClothGlove,
    BlueTShirt,
    OrangeTShirt,
    ClothBelt,
    ClothShoes,
    IronRing,
    PearlEarring,
    FireBrooch,
    IceBrooch,
    ThunderBrooch,
    LightBrooch,
    DarkBrooch,
    WoodenShield,
    //Uncommon
    LongSword,
    LongStaff,
    WarmWing,
    DualDagger,
    ReinforcedBow,
    Flute,
    FireStaff,
    IceStaff,
    ThunderStaff,
    LeatherBelt,
    LeatherShoes,
    WarmCloak,
    LeatherArmor,
    LeatherGlove,
    LeatherShield,
    FireRing,
    IceRing,
    ThunderRing,
    LightRing,
    DarkRing,
    EnhancedFireBrooch,
    EnhancedIceBrooch,
    EnhancedThunderBrooch,
    EnhancedLightBrooch,
    EnhancedDarkBrooch,
    BattleSword,
    BattleStaff,
    BattleWing,
    BattleDagger,
    BattleBow,
    BattleRecorder,
    //Rare
    

    //SetItem
    //Slime(Rare)
    SlimeSword,
    SlimeGlove,
    SlimeRing,
    SlimeBelt,
    SlimePincenez,
    SlimeWing,
    SlimePoncho,
    SlimeDart,
    //SlimeKneeGuard
    //MagicSlime(Rare)
    MagicSlimeStick,
    MagicSlimeHat,
    MagicSlimeBow,
    MagicSlimeShoes,
    MagicSlimeRecorder,
    MagicSlimeEarring,
    MagicSlimeBalloon,
    MagicSlimeSkirt,
    //MagicSlimeLamp
    //Spider(Rare)
    SpiderHat,
    SpiderSkirt,
    SpiderSuit,
    SpiderDagger,
    SpiderWing,
    SpiderCatchingNet,
    SpiderStick,
    SpiderFoldingFan,
    //Bat(Rare)
    BatRing,
    BatShoes,
    BatSword,
    BatHat,
    BatRecorder,
    BatBow,
    BatMascaradeMask,
    BatCloak,
    //ver0.1.2.11以降追加
    BronzeShoulder,
    BattleRing,
    Halo,

    IronShoulder,
    StrengthRing,
    GoldenRing,
    GoldenFireRing,
    GoldenIceRing,
    GoldenThunderRing,
    GoldenLightRing,
    GoldenDarkRing,
    IronBelt,
    IronShoes,
    CopperArmor,
    IronGlove,
    TowerShield,
    FireTowerShield,
    IceTowerShield,
    ThunderTowerShield,
    LightTowerShield,
    DarkTowerShield,
    SavageRing,
    SpellboundFireBrooch,
    SpellboundIceBrooch,
    SpellboundThunderBrooch,
    SpellboundLightBrooch,
    SpellboundDarkBrooch,
    CopperHelm,
    BattleHelm,
    WizardHelm,
    LargeSword,
    LargeStaff,
    LargeWing,
    LargeDagger,
    LargeBow,
    LargeOcarina,
    //Fairy(Super Rare)
    FairyClothes,
    FairyStaff,
    FairyBoots,
    FairyGlove,
    FairyBrooch,
    FairyLamp,
    FairyWing,
    FairyShuriken,
    //Fox(Super Rare)
    FoxKanzashi,
    FoxLoincloth,
    FoxMask,
    FoxHamayayumi,
    FoxHat,
    FoxCoat,
    FoxBoot,
    FoxEma,
    //DevilFish(Super Rare)
    DevilfishSword,
    DevilfishWing,
    DevilfishRecorder,
    DevilfishArmor,
    DevilfishScarf,
    DevilfishGill,
    DevilfishPendant,
    DevilfishRing,
    //Treant(Super Rare)
    //FlameTiger(Epic)
    //Unicorn(Epic)

    //リリース後追加するときは必ず下に書く

}
