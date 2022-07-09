using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
}

public enum Direction
{
    up,
    right,
    down,
    left
}
public enum HeroKind
{
    Warrior,
    Wizard,
    Angel,
    Thief,
    Archer,
    Tamer,
}
public enum AbilityKind
{
    Vitality,
    Strength,
    Intelligence,
    Agility,
    Luck,
}
public enum BasicStatsKind
{
    HP,
    MP,
    ATK,
    MATK,
    DEF,
    MDEF,
    SPD,
}
//Hero固有のStats
public enum Stats
{
    FireRes,
    IceRes,
    ThunderRes,
    LightRes,
    DarkRes,
    DebuffRes,
    PhysCritChance,
    MagCritChance,
    CriticalDamage,//ver00020200で追加
    EquipmentDropChance,//DropChanceのBreakdownには、Option付加チャンスも追加する
    MoveSpeed,
    SkillProficiencyGain,
    EquipmentProficiencyGain,
    TamingPointGain,
    ExpGain,
}

//Heroに共通のStats
public enum GlobalStats
{
    GoldGain,
    StoneGain,
    CrystalGain,
    LeafGain,
}

public enum ResourceKind
{
    Stone,
    Crystal,
    Leaf,
}

public enum AreaKind
{
    SlimeVillage,
    MagicSlimeCity,
    SpiderMaze,
    BatCave,
    FairyGarden,
    FoxShrine,
    DevilFishLake,
    TreantDarkForest,
    FlameTigerVolcano,
    UnicornIsland,
}
public enum MonsterSpecies
{
    Slime,
    MagicSlime,
    Spider,
    Bat,
    Fairy,
    Fox,
    DevifFish,
    Treant,
    FlameTiger,
    Unicorn,
    Mimic,
    ChallengeBoss,
}
public enum MonsterColor
{
    Normal,
    Blue,
    Yellow,
    Red,
    Green,
    Purple,
    Boss,
    Metal,
}
public enum MonsterRarity
{
    Normal,
    Common,
    Uncommon,
    Rare,
    Boss,
}
public enum ChallengeMonsterKind
{
    SlimeKing,
    WindowQueen,
    Golem,
    //GoldenBat,
    //FairyQueen,
    //Ninetale,
    //Octobaddie,
}
public enum RaidMonsterKind
{
    Golem,

}

public enum EffectKind
{
    PhysicalDamage,
    FireDamage,
    IceDamage,
    ThunderDamage,
    LightDamage,
    DarkDamage,
    Heal,
    DebuffKind,
    DebuffChance,
    MPGain,
    MPConsumption,
    Cooltime,
    Range,
    EffectRange,
}

public enum GuildAbilityKind
{
    //Member,
    StoneGain,//Mining
    CrystalGain,//Synthesizing
    LeafGain,//Gathering
    GuildExpGain,//Training
    EquipmentInventory,//Collecting
    EnchantInventory,//
    PotionInventory,//Alchemising
    MysteriousWater,//Purifying
    SkillProficiency,//Studying
    GlobalSkillSlot,//Imitating
                    //ActiveNum,//->World Ascensionにこのアップグレードを作る

    //以降WAでUnlock
    GoldCap,//Banking(WA1)
    GoldGain,//Monetizing(WA1)
    Trapping,//Trapping(WA1)
    UpgradeCost,//Optimizing(WA1)

    PhysicalAbsorption,//Shielding(WA2)
    MagicalAbsoption,//Dispersing(WA2)
    ExpGain,//Learning(WA2)
    EquipmentProficiency,//Smithing//WA1Milestoneと被ってる
    //BonusAbilityPointRebirth,//Understanding
    MaterialDrop,//Finding//これはかなり強いのでWA2かWA3
    NitroCap,//Racing
    //HP,
    //MP,
    //ATK,
    //MATK,
    //DEFMDEF,
    //SPD,
    //ElementResistance,
    //MoveSpeed,
    //EquipDropChance,
    //AbilityPoint,
    //TalentWarrior,
    //TalentWizard,
    //TalentAngel,
    //TalentThief,
    //TalentArcher,
    //TalentTamer,
    //EquipProficiency,
    //UpgradeLevel,
    //Talentはスキルレベルor熟練度
}

public enum RebirthUpgradeKind
{
    //Tier1
    //Class-exclusive
    ExpGain,
    EQRequirement,
    QuestAcceptableNum,
    //Global
    BasicAtk,//warrior
    BasicMAtk,//wizard
    BasicHp,//angel
    BasicDef,//thief
    BasicMDef,//archer
    BasicMp,//tamer
    StoneGain,//warrior
    CrystalGain,//wiz
    LeafGain,//ang
    StoneGoldCap,//thief
    CrystalGoldCap,//arc
    LeafGoldCap,//tamer

    //Tier2
    //Class-exclusive
    SkillProfGain,
    SkillRankCostReduction,
    ClassSkillSlot,
    ShareSkillPassive,
    T1ExpGainBoost,//Tier1のExpGainのブースト
    T1RebirthPointGainBoost,
    //Global
    T1BasicAtkBoost,//warrior
    T1BasicMAtkBoost,//wizard
    T1BasicHpBoost,//angel
    T1BasicDefBoost,//thief
    T1BasicMDefBoost,//archer
    T1BasicMpBoost,//tamer
    T1StoneGainBoost,//warrior
    T1CrystalGainBoost,//wiz
    T1LeafGainBoost,//ang
    T1StoneGoldCapBoost,//thief
    T1CrystalGoldCapBoost,//arc
    T1LeafGoldCapBoost,//tamer

    //Tier3
    //Class-exclussive
    EQProfGain,
    EQLevelCap,
    EQWeaponSlot,
    EQArmorSlot,
    EQJewelrySlot,
    T2ExpGainBoost,//Tier2のExpGainBoostのブースト
    T2SkillProfGainBoost,
    T2RebirthPointGainBoost,
    //Global
    T2BasicAtkBoost,//warrior
    T2BasicMAtkBoost,//wizard
    T2BasicHpBoost,//angel
    T2BasicDefBoost,//thief
    T2BasicMDefBoost,//archer
    T2BasicMpBoost,//tamer
    T2StoneGainBoost,//warrior
    T2CrystalGainBoost,//wiz
    T2LeafGainBoost,//ang
    T2StoneGoldCapBoost,//thief
    T2CrystalGoldCapBoost,//arc
    T2LeafGoldCapBoost,//tamer

    //Tier4
    //PreserveSkillPassive
}

public enum SlimeBankUpgradeKind
{
    SlimeCoinCap,//0
    SlimeCoinEfficiency,//0
    UpgradeCostReduction,//0
    ResourceBooster,//0
    StatsBooster,//0
    GoldCapBooster,//0

    //
    MysteriousWaterGain,//10
    MaterialFinder,//30
    ShopTimer,//50
    ResearchPower,//70

    //Stats
    SPD,//10
    FireRes,//20
    IceRes,//20
    ThunderRes,//20
    LightRes,//20
    DarkRes,//20
    DebuffRes,//40
    SkillProf,//40
    EquipmentProf,//40
    CritDamage,//60

    //BrokenCounter
}
