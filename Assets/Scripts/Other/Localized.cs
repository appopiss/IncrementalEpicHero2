using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UsefulMethod;
using static GameController;
using static Main;

public enum Language
{
    English,
    Japanese,
    Chinese,
}
public enum BasicWord
{
    Gain,
    Proficiency,
    Area,
    Wave,
    Areacleared,
    Areafailed,
    CompltedTime,
    TotalGoldGained,
    TotalExpGained,
    TotalMaterialsGained,
    Mission,
    AreaMastery,
    MasteryBonus,
    CompletedNum,
    Current,
    Next,
    LevelUp,
    RankUp,
    Sec,
    Effect,
    PassiveEffect,
    MaterialsToLevelUp,
    MaterialsToRankUp,
    Levelup,
    Rankup,
    Bonus,
    GoldCap,
    WorldMap,
    Gold,
    Trigger,
    Gained,
    FullInventory,
    RequiredAbility,
    HeroLevel,
    Information,
    Rarity,
    Part,
    SuccessChance,
    AdditionalEffect,
    RequiredAbilityIncrement,
    GuildLevel,
    Cost,
    Accept,
    Cancel,
    Claim,
    Client,
    Description,
    ClearCondition,
    Reward,
    UnlockCondition,
    EpicCoin,
    Additive,
    Multiplicative,
    Total,
    ProductionCost,
    LevelupCost,
    Regen,
    Dungeon,
    PortalOrb,
}
public enum AbilityWord
{
    PointLeft,
}

public class Localized : MonoBehaviour
{
    public Language language;
    public static Localized localized;
    private void Awake()
    {
        localized = this;
    }

    public string Basic(BasicWord basicWord)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (basicWord)
                {
                    case BasicWord.Gain:
                        tempString = "獲得量";
                        break;
                    case BasicWord.Area:
                        tempString = "エリア";
                        break;
                    case BasicWord.Wave:
                        tempString = "ステージ";
                        break;
                    case BasicWord.Proficiency:
                        tempString = "熟練度";
                        break;
                    case BasicWord.Areacleared:
                        tempString = "エリアコンプリート!";
                        break;
                    case BasicWord.Areafailed:
                        tempString = "探索失敗...";
                        break;
                    case BasicWord.CompltedTime:
                        tempString = "探索時間";
                        break;
                    case BasicWord.TotalGoldGained:
                        tempString = "合計獲得ゴールド";
                        break;
                    case BasicWord.TotalExpGained:
                        tempString = "合計獲得EXP";
                        break;
                    case BasicWord.TotalMaterialsGained:
                        tempString = "獲得した素材";
                        break;
                    case BasicWord.Mission:
                        tempString = "ミッション";
                        break;
                    case BasicWord.AreaMastery:
                        tempString = "探索ボーナス";
                        break;
                    case BasicWord.MasteryBonus:
                        tempString = "探索ボーナス";
                        break;
                    case BasicWord.CompletedNum:
                        tempString = "クリア回数";
                        break;
                    case BasicWord.Current:
                        tempString = "現在";
                        break;
                    case BasicWord.Next:
                        tempString = "次回";
                        break;
                    case BasicWord.LevelUp:
                        tempString = "レベルアップ";
                        break;
                    case BasicWord.RankUp:
                        tempString = "ランクアップ";
                        break;
                    case BasicWord.Sec:
                        tempString = "秒";
                        break;
                    case BasicWord.Effect:
                        tempString = "効果";
                        break;
                    case BasicWord.PassiveEffect:
                        tempString = "パッシブ効果";
                        break;
                    case BasicWord.MaterialsToLevelUp:
                        tempString = "レベルアップに必要な素材";
                        break;
                    case BasicWord.MaterialsToRankUp:
                        tempString = "ランクアップに必要な素材";
                        break;
                    case BasicWord.Levelup:
                        tempString = "レベルアップ";
                        break;
                    case BasicWord.Rankup:
                        tempString = "ランクアップ";
                        break;
                    case BasicWord.Bonus:
                        tempString = "ボーナス";
                        break;
                    case BasicWord.WorldMap:
                        tempString = "ワールドマップ";
                        break;
                    case BasicWord.Gold:
                        tempString = "ゴールド";
                        break;
                    case BasicWord.Trigger:
                        tempString = "スキル発動";
                        break;
                    case BasicWord.Gained:
                        tempString = "";
                        break;
                    case BasicWord.FullInventory:
                        tempString = "空きスロットがありません.";
                        break;
                    case BasicWord.RequiredAbility:
                        tempString = "要求能力";
                        break;
                    case BasicWord.HeroLevel:
                        tempString = "ヒーローレベル";
                        break;
                    case BasicWord.Information:
                        tempString = "基本情報";
                        break;
                    case BasicWord.Rarity:
                        tempString = "レア度";
                        break;
                    case BasicWord.Part:
                        tempString = "部位";
                        break;
                    case BasicWord.SuccessChance:
                        tempString = "成功確率";
                        break;
                    case BasicWord.AdditionalEffect:
                        tempString = "オプション効果";
                        break;
                    case BasicWord.RequiredAbilityIncrement:
                        tempString = "要求能力上昇値";
                        break;
                    case BasicWord.GuildLevel:
                        tempString = "ギルドレベル";
                        break;
                    case BasicWord.Cost:
                        tempString = "コスト";
                        break;
                    case BasicWord.Accept:
                        tempString = "受諾";
                        break;
                    case BasicWord.Cancel:
                        tempString = "キャンセル";
                        break;
                    case BasicWord.Claim:
                        tempString = "完了する";
                        break;
                    case BasicWord.GoldCap:
                        tempString = "ゴールドキャップ";
                        break;
                    case BasicWord.Client:
                        tempString = "クライアント";
                        break;
                    case BasicWord.Description:
                        tempString = "説明";
                        break;
                    case BasicWord.ClearCondition:
                        tempString = "クリア条件";
                        break;
                    case BasicWord.Reward:
                        tempString = "報酬";
                        break;
                    case BasicWord.UnlockCondition:
                        tempString = "解禁条件";
                        break;
                    case BasicWord.EpicCoin:
                        tempString = "エピックコイン";
                        break;
                    case BasicWord.Additive:
                        tempString = "加算";
                        break;
                    case BasicWord.Multiplicative:
                        tempString = "乗算";
                        break;
                    case BasicWord.Total:
                        tempString = "合計";
                        break;
                    case BasicWord.ProductionCost:
                        tempString = "生産コスト";
                        break;
                    case BasicWord.LevelupCost:
                        tempString = "レベルアップコスト";
                        break;
                    case BasicWord.Regen:
                        tempString = "自動回復";
                        break;
                    case BasicWord.Dungeon:
                        tempString = "ダンジョン";
                        break;
                    case BasicWord.PortalOrb:
                        tempString = "ポータルオーブ";
                        break;
                }
                break;
            default://English
                switch (basicWord)
                {
                    case BasicWord.Gain:
                        tempString = "Gain";
                        break;
                    case BasicWord.Area:
                        tempString = "Area";
                        break;
                    case BasicWord.Wave:
                        tempString = "Wave";
                        break;
                    case BasicWord.Proficiency:
                        tempString = "Proficiency";
                        break;
                    case BasicWord.Areacleared:
                        tempString = "Area Cleared!";
                        break;
                    case BasicWord.Areafailed:
                        tempString = "Area Failed...";
                        break;
                    case BasicWord.CompltedTime:
                        tempString = "Time";
                        break;
                    case BasicWord.TotalGoldGained:
                        tempString = "Total Gold Gained";
                        break;
                    case BasicWord.TotalExpGained:
                        tempString = "Total EXP Gained";
                        break;
                    case BasicWord.TotalMaterialsGained:
                        tempString = "Total Materials Gained";
                        break;
                    case BasicWord.Mission:
                        tempString = "Mission";
                        break;
                    case BasicWord.AreaMastery:
                        tempString = "Area Mastery";
                        break;
                    case BasicWord.MasteryBonus:
                        tempString = "Mastery Bonus";
                        break;
                    case BasicWord.CompletedNum:
                        tempString = "Clear #";
                        break;
                    case BasicWord.Current:
                        tempString = "Current";
                        break;
                    case BasicWord.Next:
                        tempString = "Next";
                        break;
                    case BasicWord.LevelUp:
                        tempString = "Level Up";
                        break;
                    case BasicWord.RankUp:
                        tempString = "Rank Up";
                        break;
                    case BasicWord.Sec:
                        tempString = "sec";
                        break;
                    case BasicWord.Effect:
                        tempString = "Effect";
                        break;
                    case BasicWord.PassiveEffect:
                        tempString = "Passive Effect";
                        break;
                    case BasicWord.MaterialsToLevelUp:
                        tempString = "Materials to Level Up";
                        break;
                    case BasicWord.MaterialsToRankUp:
                        tempString = "Materials to Rank Up";
                        break;
                    case BasicWord.Levelup:
                        tempString = "Level Up";
                        break;
                    case BasicWord.Rankup:
                        tempString = "Rank Up";
                        break;
                    case BasicWord.Bonus:
                        tempString = "Bonus";
                        break;
                    case BasicWord.WorldMap:
                        tempString = "World Map";
                        break;
                    case BasicWord.Gold:
                        tempString = "Gold";
                        break;
                    case BasicWord.Trigger:
                        tempString = "trigger";
                        break;
                    case BasicWord.Gained:
                        tempString = "Gained";
                        break;
                    case BasicWord.FullInventory:
                        tempString = "Full Inventory!";
                        break;
                    case BasicWord.RequiredAbility:
                        tempString = "Required Ability";
                        break;
                    case BasicWord.HeroLevel:
                        tempString = "Hero Level";
                        break;
                    case BasicWord.Information:
                        tempString = "Information";
                        break;
                    case BasicWord.Rarity:
                        tempString = "Rarity";
                        break;
                    case BasicWord.Part:
                        tempString = "Type";
                        break;
                    case BasicWord.SuccessChance:
                        tempString = "Success Chance";
                        break;
                    case BasicWord.AdditionalEffect:
                        tempString = "Enchanted Effect";
                        break;
                    case BasicWord.RequiredAbilityIncrement:
                        tempString = "Required Ability Increment";
                        break;
                    case BasicWord.GuildLevel:
                        tempString = "Guild Level";
                        break;
                    case BasicWord.Cost:
                        tempString = "Cost";
                        break;
                    case BasicWord.Accept:
                        tempString = "Accept";
                        break;
                    case BasicWord.Cancel:
                        tempString = "Cancel";
                        break;
                    case BasicWord.Claim:
                        tempString = "Claim";
                        break;
                    case BasicWord.GoldCap:
                        tempString = "Gold Cap";
                        break;
                    case BasicWord.Client:
                        tempString = "Client";
                        break;
                    case BasicWord.Description:
                        tempString = "Description";
                        break;
                    case BasicWord.ClearCondition:
                        tempString = "Clear Condition";
                        break;
                    case BasicWord.Reward:
                        tempString = "Reward";
                        break;
                    case BasicWord.UnlockCondition:
                        tempString = "Unlock Condition";
                        break;
                    case BasicWord.EpicCoin:
                        tempString = "Epic Coin";
                        break;
                    case BasicWord.Additive:
                        tempString = "Additive";
                        break;
                    case BasicWord.Multiplicative:
                        tempString = "Multiplicative";
                        break;
                    case BasicWord.Total:
                        tempString = "Total";
                        break;
                    case BasicWord.ProductionCost:
                        tempString = "Cost to Produce";
                        break;
                    case BasicWord.LevelupCost:
                        tempString = "Cost to Level Up";
                        break;
                    case BasicWord.Regen:
                        tempString = "Regenerate";
                        break;
                    case BasicWord.Dungeon:
                        tempString = "Dungeon";
                        break;
                    case BasicWord.PortalOrb:
                        tempString = "Portal Orb";
                        break;
                }
                break;
        }
        return tempString;
    }

    //Menu
    public string Menu(MenuKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case MenuKind.Ability:
                        return "能力";
                    case MenuKind.Skill:
                        return "スキル";
                    case MenuKind.Upgrade:
                        return "<size=20>アップ\nグレード";
                    case MenuKind.Equip:
                        return "装備";
                    case MenuKind.Guild:
                        return "ギルド";
                    case MenuKind.Quest:
                        return "クエスト";
                    case MenuKind.Lab:
                        return "錬金術";
                    case MenuKind.Setting:
                        return "設定";
                    case MenuKind.Bestiary:
                        return "図鑑";
                    case MenuKind.Rebirth:
                        return "再誕";
                    case MenuKind.Ascension:
                        return "<size=20>アセン\nション";
                    case MenuKind.Town:
                        return "タウン";
                    case MenuKind.Shop:
                        return "ショップ";
                }
                break;
            default:
                switch (kind)
                {
                    case MenuKind.Ability:
                        return "Ability";
                    case MenuKind.Skill:
                        return "Skill";
                    case MenuKind.Upgrade:
                        return "Upgrade";
                    case MenuKind.Equip:
                        return "Equip";
                    case MenuKind.Guild:
                        return "Guild";
                    case MenuKind.Quest:
                        return "Quest";
                    case MenuKind.Lab:
                        return "Lab";
                    case MenuKind.Setting:
                        return "Settings";
                    case MenuKind.Bestiary:
                        return "Bestiary";
                    case MenuKind.Rebirth:
                        return "Rebirth";
                    case MenuKind.Challenge:
                        return "Challenge";
                    case MenuKind.Ascension:
                        return "Ascension";
                    case MenuKind.Town:
                        return "Town";
                    case MenuKind.Shop:
                        return "Shop";
                    case MenuKind.Expedition:
                        return "Expedition";
                    default:
                        break;
                }
                break;
        }
        return tempString;
    }
    //StatsBreakdown
    public string StatsBreakdown(MultiplierKind kind)
    {
        switch (language)
        {
            case Language.Japanese:
                break;
            default://English
                switch (kind)
                {
                    case MultiplierKind.Base:
                        return "Base";
                    case MultiplierKind.Ability:
                        return "Ability";
                    case MultiplierKind.Title:
                        return "Title";
                    case MultiplierKind.Skill:
                        return "Skill";
                    case MultiplierKind.Upgrade:
                        return "Upgrade";
                    case MultiplierKind.Equipment:
                        return "Equipment";
                    case MultiplierKind.Dictionary:
                        return "Dictionary";
                    case MultiplierKind.Guild:
                        return "Guild";
                    case MultiplierKind.AreaPrestige:
                        return "Area Prestige";
                    case MultiplierKind.AreaDebuff:
                        return "Field Debuff";
                    case MultiplierKind.Potion:
                        return "Potion";
                    case MultiplierKind.Alchemy:
                        return "Alchemy Upgrade";
                    case MultiplierKind.Pet:
                        return "Pet";
                    case MultiplierKind.Rebirth:
                        return "Rebirth";
                    case MultiplierKind.Stance:
                        return "Stance";
                    case MultiplierKind.AlchemyExpand:
                        return "Expand Cap";
                    case MultiplierKind.Blessing:
                        return "Blessing";
                    case MultiplierKind.Town:
                        return "Town";
                    case MultiplierKind.TownResearch:
                        return "Town Research";
                    case MultiplierKind.MissionMilestone:
                        return "Mission Milestone";
                    case MultiplierKind.Debuff:
                        return "Debuff";
                    case MultiplierKind.EpicStore:
                        return "Epic Store";
                    case MultiplierKind.Challenge:
                        return "Challenge";
                    case MultiplierKind.SkillPassive:
                        return "Skill Passive";
                    case MultiplierKind.Talisman:
                        return "Talisman";
                    case MultiplierKind.TalismanPassive:
                        return "Talisman Passive";
                    case MultiplierKind.Buff:
                        return "Skill Buff";
                    case MultiplierKind.ChanneledSkill:
                        return "Channeled Skill";
                    case MultiplierKind.Ascension:
                        return "World Ascension";
                    case MultiplierKind.Quest:
                        return "Quest";
                    case MultiplierKind.Achievement:
                        return "Achievement";
                    case MultiplierKind.DLC:
                        return "Steam DLC";
                    case MultiplierKind.Expedition:
                        return "Expedition";
                }
                break;
        }
        return kind.ToString();
    }

    //Resource
    public string ResourceName(ResourceKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {

                    case ResourceKind.Stone:
                        tempString = "ストーン";
                        break;
                    case ResourceKind.Crystal:
                        tempString = "クリスタル";
                        break;
                    case ResourceKind.Leaf:
                        tempString = "リーフ";
                        break;
                }
                break;
            default://English
                switch (kind)
                {

                    case ResourceKind.Stone:
                        tempString = "Stone";
                        break;
                    case ResourceKind.Crystal:
                        tempString = "Crystal";
                        break;
                    case ResourceKind.Leaf:
                        tempString = "Leaf";
                        break;
                }
                break;
        }
        return tempString;
    }

    //Hero
    public string Hero(HeroKind kind)
    {
        //string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case HeroKind.Warrior:
                        return "戦士";
                    case HeroKind.Wizard:
                        return "魔法使い";
                    case HeroKind.Angel:
                        return "天使";
                    case HeroKind.Thief:
                        return "シーフ";
                    case HeroKind.Archer:
                        return "アーチャー";
                    case HeroKind.Tamer:
                        return "テイマー";
                }
                break;
            default:
                switch (kind)
                {
                    case HeroKind.Warrior:
                        return "Warrior";
                    case HeroKind.Wizard:
                        return "Wizard";
                    case HeroKind.Angel:
                        return "Angel";
                    case HeroKind.Thief:
                        return "Thief";
                    case HeroKind.Archer:
                        return "Archer";
                    case HeroKind.Tamer:
                        return "Tamer";
                }
                break;
        }
        return "";
    }
    //Stats
    string tempStrBaseStats;
    public string BasicStats(BasicStatsKind kind, bool isShort = false)
    {
        switch (kind)
        {
            case BasicStatsKind.HP:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=0>";
                if (!isShort) tempStrBaseStats += " HP";
                break;
            case BasicStatsKind.MP:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=1>";
                if (!isShort) tempStrBaseStats += " MP";
                break;
            case BasicStatsKind.ATK:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=2>";
                if (!isShort) tempStrBaseStats += " ATK";
                break;
            case BasicStatsKind.MATK:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=3>";
                if (!isShort) tempStrBaseStats += " MATK";
                break;
            case BasicStatsKind.DEF:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=4>";
                if (!isShort) tempStrBaseStats += " DEF";
                break;
            case BasicStatsKind.MDEF:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=5>";
                if (!isShort) tempStrBaseStats += " MDEF";
                break;
            case BasicStatsKind.SPD:
                tempStrBaseStats = optStr + "<sprite=\"stats\" index=6>";
                if (!isShort) tempStrBaseStats += " SPD";
                break;
        }
        return tempStrBaseStats;
    }
    public string BasicStatsDescription(BasicStatsKind kind)
    {
        //string tempString = "";

        switch (kind)
        {
            case BasicStatsKind.HP:
                return "Increases Health Points";
            case BasicStatsKind.MP:
                return "Increases Mana Points";
            case BasicStatsKind.ATK:
                return "Increases Physical Attack damage";
            case BasicStatsKind.MATK:
                return "Increases Magical Type Attack damage";
            case BasicStatsKind.DEF:
                return "Increases Defense from Physical Attacks";
            case BasicStatsKind.MDEF:
                return "Increases Magical Defense from Magical Attacks";
            case BasicStatsKind.SPD:
                return "Lowers Skill Cooldowns";
        }
        return "";
        //return tempString;
    }
    string tempStrStats;
    public string Stat(Stats stats, bool isShort = false)
    {
        switch (language)
        {
            case Language.Japanese:
                switch (stats)
                {
                    case Stats.FireRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=7>";
                        if (!isShort) tempStrStats += " 火抵抗値";
                        break;
                    case Stats.IceRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=8>";
                        if (!isShort) tempStrStats += " 氷抵抗値";
                        break;
                    case Stats.ThunderRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=9>";
                        if (!isShort) tempStrStats += " 雷抵抗値";
                        break;
                    case Stats.LightRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=10>";
                        if (!isShort) tempStrStats += " 光抵抗値";
                        break;
                    case Stats.DarkRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=11>";
                        if (!isShort) tempStrStats += " 闇抵抗値";
                        break;
                    case Stats.DebuffRes:
                        tempStrStats = "状態異常抵抗値";
                        break;
                    //case Stats.DodgeChance:
                    //    tempStrStats = "回避率";
                    //    break;
                    case Stats.PhysCritChance:
                        tempStrStats = "物理クリティカル率";
                        break;
                    case Stats.MagCritChance:
                        tempStrStats = "魔法クリティカル率";
                        break;
                    case Stats.ExpGain:
                        tempStrStats = "EXP獲得量";
                        break;
                    case Stats.SkillProficiencyGain:
                        tempStrStats = "スキル熟練度獲得量";
                        break;
                    case Stats.EquipmentProficiencyGain:
                        tempStrStats = "装備熟練度獲得量";
                        break;
                    case Stats.EquipmentDropChance:
                        tempStrStats = "装備ドロップ率";
                        break;
                    //case Stats.ColorDropChance:
                    //    tempStrStats = "カラードロップ率";
                    //    break;
                    //case Stats.UniqueDropChance:
                    //    tempStrStats = "ユニークドロップ率";
                    //    break;
                    case Stats.MoveSpeed:
                        tempStrStats = "移動速度";
                        break;
                }
                break;
            default://English
                switch (stats)
                {
                    case Stats.FireRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=7>";
                        if (!isShort) tempStrStats += " Fire Resistance";
                        break;
                    case Stats.IceRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=8>";
                        if (!isShort) tempStrStats += " Ice Resistance";
                        break;
                    case Stats.ThunderRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=9>";
                        if (!isShort) tempStrStats += " Thunder Resistance";
                        break;
                    case Stats.LightRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=10>";
                        if (!isShort) tempStrStats += " Light Resistance";
                        break;
                    case Stats.DarkRes:
                        tempStrStats = optStr + "<sprite=\"stats\" index=11>";
                        if (!isShort) tempStrStats += " Dark Resistance";
                        break;
                    //case Stats.DodgeChance:
                    //    tempStrStats = "Dodge Chance";
                    //    break;
                    case Stats.DebuffRes:
                        tempStrStats = "Debuff Resistance";
                        break;
                    case Stats.PhysCritChance:
                        tempStrStats = "Physical Critical Chance";
                        break;
                    case Stats.MagCritChance:
                        tempStrStats = "Magical Critical Chance";
                        break;
                    case Stats.CriticalDamage:
                        tempStrStats = "Critical Damage";
                        break;
                    case Stats.ExpGain:
                        tempStrStats = "EXP Gain";
                        break;
                    case Stats.SkillProficiencyGain:
                        tempStrStats = "Skill Proficiency Gain";
                        break;
                    case Stats.EquipmentProficiencyGain:
                        tempStrStats = "Equipment Proficiency Gain";
                        break;
                    case Stats.EquipmentDropChance:
                        tempStrStats = "Equipment Drop Chance";
                        break;
                    //case Stats.ColorDropChance:
                    //    tempStrStats = "Color Drop Chance";
                    //    break;
                    //case Stats.UniqueDropChance:
                    //    tempStrStats = "Unique Drop Chance";
                    //    break;
                    case Stats.MoveSpeed:
                        tempStrStats = "Move Speed";
                        break;
                    case Stats.TamingPointGain:
                        return "Taming Point Gain";
                }
                break;
        }
        return tempStrStats;
    }
    public string GlobalStat(GlobalStats stats)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (stats)
                {
                    case GlobalStats.GoldGain:
                        tempString = "ゴールド獲得量";
                        break;
                    case GlobalStats.StoneGain:
                        tempString = "ストーン生産量";
                        break;
                    case GlobalStats.CrystalGain:
                        tempString = "クリスタル生産量";
                        break;
                    case GlobalStats.LeafGain:
                        tempString = "リーフ生産量";
                        break;
                }
                break;
            default://English
                switch (stats)
                {
                    case GlobalStats.GoldGain:
                        tempString = "Gold Gain";
                        break;
                    case GlobalStats.StoneGain:
                        tempString = "Stone Gain";
                        break;
                    case GlobalStats.CrystalGain:
                        tempString = "Crystal Gain";
                        break;
                    case GlobalStats.LeafGain:
                        tempString = "Leaf Gain";
                        break;
                }
                break;
        }
        return tempString;
    }

    //Ability
    public string Ability(AbilityKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case AbilityKind.Vitality:
                        tempString = "体力";
                        break;
                    case AbilityKind.Strength:
                        tempString = "力";
                        break;
                    case AbilityKind.Intelligence:
                        tempString = "知力";
                        break;
                    case AbilityKind.Agility:
                        tempString = "敏捷";
                        break;
                    case AbilityKind.Luck:
                        tempString = "運";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case AbilityKind.Vitality:
                        tempString = "VIT";
                        break;
                    case AbilityKind.Strength:
                        tempString = "STR";
                        break;
                    case AbilityKind.Intelligence:
                        tempString = "INT";
                        break;
                    case AbilityKind.Agility:
                        tempString = "AGI";
                        break;
                    case AbilityKind.Luck:
                        tempString = "LUK";
                        break;
                }
                break;
        }
        return tempString;
    }
    public string AbilityDescription(AbilityKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case AbilityKind.Vitality:
                        tempString = "HP, ATK（物理攻撃力）, DEF（物理防御力）に影響します.";
                        break;
                    case AbilityKind.Strength:
                        tempString = "ATK（物理攻撃力）に影響します.";
                        break;
                    case AbilityKind.Intelligence:
                        tempString = "MP, MATK（魔法攻撃力）に影響します.";
                        break;
                    case AbilityKind.Agility:
                        tempString = "MP, SPD（速度）に影響します.";
                        break;
                    case AbilityKind.Luck:
                        tempString = "クリティカル率, ドロップ率などに影響します.";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case AbilityKind.Vitality:
                        tempString = "Vitality has an effect on HP, DEF and MDEF.";
                        break;
                    case AbilityKind.Strength:
                        tempString = "Strength has an effect on ATK and DEF.";
                        break;
                    case AbilityKind.Intelligence:
                        tempString = "Intelligence has an effect on MP, MATK and MDEF.";
                        break;
                    case AbilityKind.Agility:
                        tempString = "Agility has an effect on MP, SPD and Move Speed.";
                        break;
                    case AbilityKind.Luck:
                        tempString = "Luck has an effect on Critical Chance and Drop Chance.";
                        break;
                }
                break;
        }
        return tempString;
    }
    public string AbilityWord(AbilityWord kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case global::AbilityWord.PointLeft:
                        tempString = "ポイント";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case global::AbilityWord.PointLeft:
                        tempString = "Points left";
                        break;
                }
                break;
        }
        return tempString;
    }

    //Title
    public string Title(TitleKind kind)
    {
        string tempString = kind.ToString();//Debug
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case TitleKind.MonsterDistinguisher:
                        tempString = "Monster Study";
                        break;
                    case TitleKind.EquipmentSlotWeapon:
                        tempString = "Apprentice of Weapon";
                        break;
                    case TitleKind.EquipmentSlotArmor:
                        tempString = "Apprentice of Armor";
                        break;
                    case TitleKind.EquipmentSlotJewelry:
                        tempString = "Apprentice of Jewelry";
                        break;
                    case TitleKind.PotionSlot:
                        tempString = "Apprentice of Potion";
                        break;
                    case TitleKind.EquipmentProficiency:
                        tempString = "Equipment Master";
                        break;
                    case TitleKind.SkillMaster:
                        tempString = "Class Master";
                        break;
                    case TitleKind.Survival:
                        tempString = "Survival";
                        break;
                    case TitleKind.FireResistance:
                        tempString = "Fire Soul";
                        break;
                    case TitleKind.IceResistance:
                        tempString = "Ice Soul";
                        break;
                    case TitleKind.ThunderResistance:
                        tempString = "Thunder Soul";
                        break;
                    case TitleKind.LightResistance:
                        tempString = "Light Soul";
                        break;
                    case TitleKind.DarkResistance:
                        tempString = "Dark Soul";
                        break;
                    case TitleKind.DebuffResistance:
                        tempString = "Holy Guard";
                        break;
                    case TitleKind.MoveSpeed:
                        tempString = "Racer";
                        break;
                    case TitleKind.Alchemist:
                        tempString = "Alchemist";
                        break;
                    case TitleKind.MetalHunter:
                        tempString = "Metal Hunter";
                        break;
                    case TitleKind.BreakingTheLimit:
                        tempString = "Breaking The Limit";
                        break;
                    case TitleKind.PhysicalDamage:
                        tempString = "Master of Physical Attacks";
                        break;
                    case TitleKind.FireDamage:
                        tempString = "Master of Fire Attacks";
                        break;
                    case TitleKind.IceDamage:
                        tempString = "Master of Ice Attacks";
                        break;
                    case TitleKind.ThunderDamage:
                        tempString = "Master of Thunder Attacks";
                        break;
                    case TitleKind.LightDamage:
                        tempString = "Master of Light Attacks";
                        break;
                    case TitleKind.DarkDamage:
                        tempString = "Master of Dark Attacks";
                        break;
                    case TitleKind.Cooperation:
                        tempString = "Proof of Rebirth";
                        break;
                    case TitleKind.Quester:
                        return "Quester";
                }
                break;
        }
        return tempString;
    }
    public string TitleEffect(TitleKind kind, double effectValue, bool isSub = false)
    {
        //Debug
        string tempString = kind.ToString() + " + " + tDigit(effectValue);
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case TitleKind.MonsterDistinguisher:
                        if (isSub) tempString = optStr + "Capturable Monsters level : [ Hero's Level " + tDigit(effectValue - 100, 0, false, null, true) + " ] or less";
                        else
                            tempString = optStr + "Visible Monster stats on level " + tDigit(effectValue) + " or less monsters";
                        break;
                    case TitleKind.EquipmentSlotWeapon:
                        tempString = optStr + "Equipment Weapon Slot + " + tDigit(effectValue);
                        break;
                    case TitleKind.EquipmentSlotArmor:
                        tempString = optStr + "Equipment Armor Slot + " + tDigit(effectValue);
                        break;
                    case TitleKind.EquipmentSlotJewelry:
                        tempString = optStr + "Equipment Jewelry Slot + " + tDigit(effectValue);
                        break;
                    case TitleKind.PotionSlot:
                        tempString = optStr + "Utility Slot + " + tDigit(effectValue);
                        break;
                    case TitleKind.EquipmentProficiency:
                        tempString = optStr + "Equipment Proficiency Gain + " + percent(effectValue);
                        break;
                    case TitleKind.SkillMaster:
                        if (isSub) tempString = optStr + "Skill Proficiency Gain + " + percent(effectValue);
                        else tempString = optStr + "Class Skill Slot + " + tDigit(effectValue);
                        break;
                    case TitleKind.Survival:
                        tempString = optStr + "Critical Chance + 50% when HP is less than " + percent(effectValue);
                        break;
                    case TitleKind.FireResistance:
                        if (isSub) tempString = optStr + percent(effectValue) + " chance to nullify fire damage";
                        else tempString = optStr + "Fire Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.IceResistance:
                        if (isSub) tempString = optStr + percent(effectValue) + " chance to nullify ice damage";
                        else tempString = optStr + "Ice Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.ThunderResistance:
                        if (isSub) tempString = optStr + percent(effectValue) + " chance to nullify thunder damage";
                        else tempString = optStr + "Thunder Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.LightResistance:
                        if (isSub) tempString = optStr + percent(effectValue) + " chance to nullify light damage";
                        else tempString = optStr + "Light Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.DarkResistance:
                        if (isSub) tempString = optStr + percent(effectValue) + " chance to nullify dark damage";
                        else tempString = optStr + "Dark Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.DebuffResistance:
                        tempString = optStr + "Debuff Resistance + " + percent(effectValue);
                        break;
                    case TitleKind.MoveSpeed:
                        tempString = optStr + "Move Speed + " + percent(effectValue);
                        break;
                    case TitleKind.Alchemist:
                        tempString = optStr + "Increase Mysterious Water Gain + " + percent(effectValue);
                        break;
                    case TitleKind.MetalHunter:
                        if (isSub) tempString = optStr + "EXP Gain from Metal monsters + " + percent(effectValue);
                        else
                            tempString = optStr + "Damage to Metal monsters + " + tDigit(effectValue);
                        break;
                    case TitleKind.BreakingTheLimit:
                        tempString = optStr + "Damage Cap + " + tDigit(effectValue);
                        break;
                    case TitleKind.PhysicalDamage:
                        tempString = optStr + "Physical Damage + " + percent(effectValue);
                        break;
                    case TitleKind.FireDamage:
                        tempString = optStr + "Fire Damage + " + percent(effectValue);
                        break;
                    case TitleKind.IceDamage:
                        tempString = optStr + "Ice Damage + " + percent(effectValue);
                        break;
                    case TitleKind.ThunderDamage:
                        tempString = optStr + "Thunder Damage + " + percent(effectValue);
                        break;
                    case TitleKind.LightDamage:
                        tempString = optStr + "Light Damage + " + percent(effectValue);
                        break;
                    case TitleKind.DarkDamage:
                        tempString = optStr + "Dark Damage + " + percent(effectValue);
                        break;
                    case TitleKind.Cooperation:
                        tempString = optStr + "Enable Background Cooperation that you gain " + percent(effectValue) + " in background";
                        break;
                    case TitleKind.Quester:
                        return optStr + "General Quest Clear # + " + tDigit(effectValue) + " per clear";
                }
                break;
        }
        return tempString;
    }

    //Monster
    public string MonsterSpeciesName(MonsterSpecies species)
    {
        string tempSpeciesStr = species.ToString();
        switch (species)
        {
            case MonsterSpecies.Slime:
                tempSpeciesStr = "Slimes"; break;
            case MonsterSpecies.MagicSlime:
                tempSpeciesStr = "Magicslime"; break;
            case MonsterSpecies.Spider:
                tempSpeciesStr = "Spiders"; break;
            case MonsterSpecies.Bat:
                tempSpeciesStr = "Bats"; break;
            case MonsterSpecies.Fairy:
                tempSpeciesStr = "Fairies"; break;
            case MonsterSpecies.Fox:
                tempSpeciesStr = "Foxes"; break;
            case MonsterSpecies.DevifFish:
                tempSpeciesStr = "Devilfishes"; break;
            case MonsterSpecies.Treant:
                tempSpeciesStr = "Treants"; break;
            case MonsterSpecies.FlameTiger:
                tempSpeciesStr = "Flametigers"; break;
            case MonsterSpecies.Unicorn:
                tempSpeciesStr = "Unicorns"; break;
            case MonsterSpecies.Mimic:
                return "Mimic";
        }
        return tempSpeciesStr;
    }
    public string MonsterName(MonsterSpecies species, MonsterColor color)
    {
        string tempColorStr = color.ToString();
        string tempSpeciesStr = species.ToString();
        switch (language)
        {
            case Language.Japanese:
                switch (color)
                {
                    case MonsterColor.Normal:
                        tempColorStr = "ノーマル";
                        break;
                    case MonsterColor.Blue:
                        tempColorStr = "ブルー";
                        break;
                    case MonsterColor.Yellow:
                        tempColorStr = "イエロー";
                        break;
                    case MonsterColor.Red:
                        tempColorStr = "レッド";
                        break;
                    case MonsterColor.Green:
                        tempColorStr = "グリーン";
                        break;
                    case MonsterColor.Purple:
                        tempColorStr = "パープル";
                        break;
                    case MonsterColor.Boss:
                        tempColorStr = "ボス";
                        break;
                    case MonsterColor.Metal:
                        tempColorStr = "メタル";
                        break;
                }
                switch (species)
                {
                    case MonsterSpecies.Slime:
                        tempSpeciesStr = "スライム"; break;
                    case MonsterSpecies.MagicSlime:
                        tempSpeciesStr = "マジックスライム"; break;
                    case MonsterSpecies.Spider:
                        tempSpeciesStr = "スパイダー"; break;
                    case MonsterSpecies.Bat:
                        tempSpeciesStr = "バット"; break;
                    case MonsterSpecies.Fairy:
                        tempSpeciesStr = "フェアリー"; break;
                    case MonsterSpecies.Fox:
                        tempSpeciesStr = "フォックス"; break;
                    case MonsterSpecies.DevifFish:
                        tempSpeciesStr = "デビルフィッシュ"; break;
                    case MonsterSpecies.Treant:
                        tempSpeciesStr = "トレント"; break;
                    case MonsterSpecies.FlameTiger:
                        tempSpeciesStr = "フレイムタイガー"; break;
                    case MonsterSpecies.Unicorn:
                        tempSpeciesStr = "ユニコーン"; break;
                    case MonsterSpecies.Mimic:
                        return "ミミック";
                }
                break;
            default:
                switch (species)
                {
                    case MonsterSpecies.Slime:
                        tempSpeciesStr = "Slime"; break;
                    case MonsterSpecies.MagicSlime:
                        tempSpeciesStr = "Magicslime"; break;
                    case MonsterSpecies.Spider:
                        tempSpeciesStr = "Spider"; break;
                    case MonsterSpecies.Bat:
                        tempSpeciesStr = "Bat"; break;
                    case MonsterSpecies.Fairy:
                        tempSpeciesStr = "Fairy"; break;
                    case MonsterSpecies.Fox:
                        tempSpeciesStr = "Fox"; break;
                    case MonsterSpecies.DevifFish:
                        tempSpeciesStr = "Devilfish"; break;
                    case MonsterSpecies.Treant:
                        tempSpeciesStr = "Treant"; break;
                    case MonsterSpecies.FlameTiger:
                        tempSpeciesStr = "Flametiger"; break;
                    case MonsterSpecies.Unicorn:
                        tempSpeciesStr = "Unicorn"; break;
                    case MonsterSpecies.Mimic:
                        return "Mimic";
                }
                break;
        }
        return tempColorStr + " " + tempSpeciesStr;
    }
    //Pet
    public string PetActiveEffect(PetActiveEffectKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case PetActiveEffectKind.None:
                        return "Stay tuned for future updates!";
                    case PetActiveEffectKind.GetResource:
                        return "Automatically pick up dropped Resources";
                    case PetActiveEffectKind.GetMaterial:
                        return "Automatically pick up dropped Materials";
                    case PetActiveEffectKind.UpgradeQueue:
                        return "Upgrade's Available Queue + 5 \n<size=16>Right-Click on an upgrade to assign Queue, while Shift + Right-Click to remove Queue</size>";
                    case PetActiveEffectKind.ExpandMysteriousWaterCap:
                        return "Automatically expand Mysterious Water Cap";
                    case PetActiveEffectKind.RebirthTier1:
                        return "Unleashes a toggle in Rebirth tab to enable Auto Rebirth Tier 1";
                    case PetActiveEffectKind.RebirthTier2:
                        return "Enable Auto Rebirth Tier 2 (Needs Normal Fairy Active)";
                    case PetActiveEffectKind.RebirthTier3:
                        return "Enable Auto Rebirth Tier 3 (Needs Normal Fairy Active)";
                    case PetActiveEffectKind.RebirthTier4:
                        return "Enable Auto Rebirth Tier 4 (Needs Normal Fairy Active)";
                    case PetActiveEffectKind.RebirthTier5:
                        return "Enable Auto Rebirth Tier 5 (Needs Normal Fairy Active)";
                    case PetActiveEffectKind.RebirthTier6:
                        return "Enable Auto Rebirth Tier 6 (Needs Normal Fairy Active)";
                    case PetActiveEffectKind.AlchemyQueue:
                        return "Alchemy's Available Queue + 10 \n<size=16>Right-Click on a potion to assign Queue, while Shift + Right-Click to remove Queue</size>";
                    case PetActiveEffectKind.EquipPotion:
                        return "Automatically equip to restore Utility items that you are equipping";
                    case PetActiveEffectKind.Capture:
                        return "Automatically use a trap to capture a monster when you defeat it, while having traps equipped";
                    case PetActiveEffectKind.GetEquipment:
                        return "Automatically pick up dropped Equipment";
                    case PetActiveEffectKind.DisassembleEquipment:
                        return "Auto-Disassemble Equipment Slot + 10 \n<size=16>Click an equipment in dictionary to assign/ remove auto-disassemble when picked up";
                    case PetActiveEffectKind.DisassemblePotion:
                        return "Enable Shift+D key to disassemble a potion while alchemising it";
                    case PetActiveEffectKind.BuyShopMaterialSlime:
                        return "Automatically buy " + Material(MaterialKind.OilOfSlime) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialMagicSlime:
                        return "Automatically buy " + Material(MaterialKind.EnchantedCloth) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialSpider:
                        return "Automatically buy " + Material(MaterialKind.SpiderSilk) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialBat:
                        return "Automatically buy " + Material(MaterialKind.BatWing) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialFairy:
                        return "Automatically buy " + Material(MaterialKind.FairyDust) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialFox:
                        return "Automatically buy " + Material(MaterialKind.FoxTail) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialDevilfish:
                        return "Automatically buy " + Material(MaterialKind.FishScales) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialTreant:
                        return "Automatically buy " + Material(MaterialKind.CarvedBranch) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialFlametiger:
                        return "Automatically buy " + Material(MaterialKind.ThickFur) + " in shop";
                    case PetActiveEffectKind.BuyShopMaterialUnicorn:
                        return "Automatically buy " + Material(MaterialKind.UnicornHorn) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapNormal:
                        return "Automatically buy " + PotionName(PotionKind.ThrowingNet) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapIce:
                        return "Automatically buy " + PotionName(PotionKind.IceRope) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapThunder:
                        return "Automatically buy " + PotionName(PotionKind.ThunderRope) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapFire:
                        return "Automatically buy " + PotionName(PotionKind.FireRope) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapLight:
                        return "Automatically buy " + PotionName(PotionKind.LightRope) + " in shop";
                    case PetActiveEffectKind.BuyShopTrapDark:
                        return "Automatically buy " + PotionName(PotionKind.DarkRope) + " in shop";
                    case PetActiveEffectKind.RetryDungeon:
                        return "Automatically retry the current dungeon when you clear it";
                    case PetActiveEffectKind.OpenChest:
                        return "Automatically open treasure chests in dungeon";
                    case PetActiveEffectKind.SkillRankUp:
                        return "Automatically increase skill's rank";
                    case PetActiveEffectKind.SmartSlimeCoins:
                        return "Keep Slime Coin until it get maxed cap before buying upgrades with Queue";
                    case PetActiveEffectKind.TownLevelUp:
                        return "Automatically increase building's level";
                    case PetActiveEffectKind.RebirthUpgradeEXP:
                        return "Automatically increase Tier 1 Rebirth Upgrade [EXP Multiplier]";
                    case PetActiveEffectKind.EquipNonMaxedEQ:
                        return "Automatically replace maxed-Lv equipment with non-maxed-Lv EQ when it reaches max Lv";
                    case PetActiveEffectKind.AutoCraftDisassembleEQ:
                        return "Automatically disassemble equipment as soon as you craft it in Dictionary";
                    case PetActiveEffectKind.DisassembleTalismanCommon:
                        return "Automatically disassemble common Talisman as soon as you get it";
                    case PetActiveEffectKind.BuyBlessing:
                        return "Automatically buy Blessings randomly";
                    default:
                        break;
                }
                break;
        }
        return tempString;
    }

    //Area
    public string AreaName(AreaKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case AreaKind.SlimeVillage:
                        tempString = "スライムの村";
                        break;
                    case AreaKind.MagicSlimeCity:
                        tempString = "マジックスライムの都市";
                        break;
                    case AreaKind.SpiderMaze:
                        tempString = "スパイダー迷宮";
                        break;
                    case AreaKind.BatCave:
                        tempString = "コウモリ洞窟";
                        break;
                    case AreaKind.FairyGarden:
                        tempString = "妖精の庭";
                        break;
                    case AreaKind.FoxShrine:
                        tempString = "狐の神社";
                        break;
                    case AreaKind.DevilFishLake:
                        tempString = "デビルフィッシュレイク";
                        break;
                    case AreaKind.TreantDarkForest:
                        tempString = "トレントの森";
                        break;
                    case AreaKind.FlameTigerVolcano:
                        tempString = "フレイムタイガー火山";
                        break;
                    case AreaKind.UnicornIsland:
                        tempString = "ユニコーンアイランド";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case AreaKind.SlimeVillage:
                        tempString = "Slime Village";
                        break;
                    case AreaKind.MagicSlimeCity:
                        tempString = "Magicslime City";
                        break;
                    case AreaKind.SpiderMaze:
                        tempString = "Spider Maze";
                        break;
                    case AreaKind.BatCave:
                        tempString = "Bat Cave";
                        break;
                    case AreaKind.FairyGarden:
                        tempString = "Fairy Garden";
                        break;
                    case AreaKind.FoxShrine:
                        tempString = "Fox Shrine";
                        break;
                    case AreaKind.DevilFishLake:
                        tempString = "Devilfish Lake";
                        break;
                    case AreaKind.TreantDarkForest:
                        tempString = "Treant Darkforest";
                        break;
                    case AreaKind.FlameTigerVolcano:
                        tempString = "Flametiger Volcano";
                        break;
                    case AreaKind.UnicornIsland:
                        tempString = "Unicorn Island";
                        break;
                }
                break;
        }
        return tempString;
    }
    //AreaPrestige
    public (string name, string currentEffect, string nextEffect) AreaPrestigeUpgrade(AreaPrestigeUpgrade upgrade)
    {
        string tempName = upgrade.kind.ToString();
        string tempEffect = "";
        string tempNextEffect = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (upgrade.kind)
                {
                    case AreaPrestigeUpgradeKind.MaxAreaLevelUp:
                        tempName = "Area Prestige";
                        tempEffect = "Increases the Area Difficulty cap + " + tDigit(upgrade.effectValue);
                        tempNextEffect = "Increases the Area Difficulty cap + " + tDigit(upgrade.nextEffectValue);
                        break;
                    case AreaPrestigeUpgradeKind.UnlockMission:
                        tempName = "Mission Challenger";
                        tempEffect = "Unlocks another " + tDigit(upgrade.effectValue) + " Mission";
                        tempNextEffect = "Unlocks another " + tDigit(upgrade.nextEffectValue) + " Mission";
                        break;
                    case AreaPrestigeUpgradeKind.ClearCount:
                        tempName = "Explorer's Boon";
                        tempEffect = "Increases Area Clear # and Clear Reward by " + tDigit(upgrade.effectValue);
                        tempNextEffect = "Increases Area Clear # and Clear Reward by " + tDigit(upgrade.nextEffectValue);
                        break;
                    case AreaPrestigeUpgradeKind.DecreaseMaxWave:
                        tempName = "A Moment to Breathe";
                        tempEffect = "Decreases the max wave count required to clear an area by " + tDigit(upgrade.effectValue);
                        tempNextEffect = "Decreases the max wave count required to clear an area by " + tDigit(upgrade.nextEffectValue)
                            + "\n( Cannot be reduced below 10 waves )";
                        break;
                    case AreaPrestigeUpgradeKind.ExpBonus:
                        tempName = "EXP Bonus";
                        tempEffect = "EXP Gain + " + percent(upgrade.effectValue) + " while you are in the entire region of " + AreaName(upgrade.prestige.areaKind);
                        tempNextEffect = "EXP Gain + " + percent(upgrade.nextEffectValue) + " while you are in the entire region of " + AreaName(upgrade.prestige.areaKind);
                        break;
                    case AreaPrestigeUpgradeKind.MoveSpeedBonus:
                        tempName = "Move Speed Bonus";
                        tempEffect = "Move Speed + " + percent(upgrade.effectValue) + " while you are in the entire region of " + AreaName(upgrade.prestige.areaKind);
                        tempNextEffect = "Move Speed + " + percent(upgrade.nextEffectValue) + " while you are in the entire region of " + AreaName(upgrade.prestige.areaKind);
                        break;
                    case AreaPrestigeUpgradeKind.TreasureChest:
                        tempName = "Treasure Hunter";
                        tempEffect = "Treasure Chest Drop Chance " + percent(upgrade.effectValue) + " when you defeat a monster";
                        tempNextEffect = "Treasure Chest Drop Chance " + percent(upgrade.nextEffectValue) + " when you defeat a monster";
                        break;
                    case AreaPrestigeUpgradeKind.LimitTime:
                        tempName = "Additional Time";
                        tempEffect = "Time Limit + " + tDigit(upgrade.effectValue) + " sec";
                        tempNextEffect = "Time Limit + " + tDigit(upgrade.nextEffectValue) + " sec";
                        break;
                    case AreaPrestigeUpgradeKind.MetalSlime:
                        tempName = "Metal Chaser";
                        tempEffect = "Metal Encount Chance " + percent(upgrade.effectValue, 3);
                        tempNextEffect = "Metal Encount Chance " + percent(upgrade.nextEffectValue, 3);
                        break;
                    case AreaPrestigeUpgradeKind.PortalOrb:
                        tempName = "Portal Key";
                        tempEffect = "Reduce Portal Orb cost for entering this dungeon by " + tDigit(upgrade.effectValue);
                        tempNextEffect = "Reduce Portal Orb cost for entering this dungeon by " + tDigit(upgrade.nextEffectValue)
                            + "\n( Cannot be reduced below 1 Portal Orb )";
                        break;
                }
                break;
        }
        return (tempName, tempEffect, tempNextEffect);
    }


    //Equipment
    public string EquipmentName(EquipmentKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case EquipmentKind.DullSword:
                        tempString = "なまくらな剣";
                        break;
                    default:
                        //Debug
                        tempString = kind.ToString();
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case EquipmentKind.Nothing:
                        break;
                    case EquipmentKind.DullSword:
                        tempString = "Dull Sword";
                        break;
                    case EquipmentKind.BrittleStaff:
                        tempString = "Brittle Staff";
                        break;
                    case EquipmentKind.FlimsyWing:
                        tempString = "Flimsy Wing";
                        break;
                    case EquipmentKind.WornDart:
                        tempString = "Worn Dart";
                        break;
                    case EquipmentKind.SmallBow:
                        tempString = "Small Bow";
                        break;
                    case EquipmentKind.WoodenRecorder:
                        tempString = "Wooden Recorder";
                        break;
                    case EquipmentKind.OldCloak:
                        tempString = "Old Cloak";
                        break;
                    case EquipmentKind.BlueHat:
                        tempString = "Blue Hat";
                        break;
                    case EquipmentKind.OrangeHat:
                        tempString = "Orange Hat";
                        break;
                    case EquipmentKind.ClothShoes:
                        tempString = "Cloth Shoes";
                        break;
                    case EquipmentKind.IronRing:
                        tempString = "Iron Ring";
                        break;
                    case EquipmentKind.SlimeRing:
                        tempString = "Slime Ring";
                        break;
                    case EquipmentKind.SlimeSword:
                        tempString = "Slime Sword";
                        break;
                    case EquipmentKind.TShirt:
                        tempString = "T-shirt";
                        break;
                    case EquipmentKind.ClothGlove:
                        tempString = "Cloth Glove";
                        break;
                    case EquipmentKind.BlueTShirt:
                        tempString = "Blue T-shirt";
                        break;
                    case EquipmentKind.OrangeTShirt:
                        tempString = "Orange T-shirt";
                        break;
                    case EquipmentKind.ClothBelt:
                        tempString = "Cloth Belt";
                        break;
                    case EquipmentKind.PearlEarring:
                        tempString = "Pearl Earring";
                        break;
                    case EquipmentKind.FireBrooch:
                        tempString = "Fire Brooch";
                        break;
                    case EquipmentKind.IceBrooch:
                        tempString = "Ice Brooch";
                        break;
                    case EquipmentKind.ThunderBrooch:
                        tempString = "Thunder Brooch";
                        break;
                    case EquipmentKind.LightBrooch:
                        tempString = "Light Brooch";
                        break;
                    case EquipmentKind.DarkBrooch:
                        tempString = "Dark Brooch";
                        break;
                    case EquipmentKind.WoodenShield:
                        tempString = "Wooden Shield";
                        break;
                    case EquipmentKind.LongSword:
                        tempString = "Long Sword";
                        break;
                    case EquipmentKind.LongStaff:
                        tempString = "Long Staff";
                        break;
                    case EquipmentKind.WarmWing:
                        tempString = "Warm Wing";
                        break;
                    case EquipmentKind.DualDagger:
                        tempString = "Dual Dagger";
                        break;
                    case EquipmentKind.ReinforcedBow:
                        tempString = "Reinforced Bow";
                        break;
                    case EquipmentKind.Flute:
                        tempString = "Flute";
                        break;
                    case EquipmentKind.FireStaff:
                        tempString = "Fire Staff";
                        break;
                    case EquipmentKind.IceStaff:
                        tempString = "Ice Staff";
                        break;
                    case EquipmentKind.ThunderStaff:
                        tempString = "Thunder Staff";
                        break;
                    case EquipmentKind.LeatherBelt:
                        tempString = "Leather Belt";
                        break;
                    case EquipmentKind.LeatherShoes:
                        tempString = "Leather Shoes";
                        break;
                    case EquipmentKind.WarmCloak:
                        tempString = "Warm Cloak";
                        break;
                    case EquipmentKind.LeatherArmor:
                        tempString = "Leather Armor";
                        break;
                    case EquipmentKind.LeatherGlove:
                        tempString = "Leather Glove";
                        break;
                    case EquipmentKind.LeatherShield:
                        tempString = "Leather Shield";
                        break;
                    case EquipmentKind.FireRing:
                        tempString = "Fire Ring";
                        break;
                    case EquipmentKind.IceRing:
                        tempString = "Ice Ring";
                        break;
                    case EquipmentKind.ThunderRing:
                        tempString = "Thunder Ring";
                        break;
                    case EquipmentKind.LightRing:
                        tempString = "Light Ring";
                        break;
                    case EquipmentKind.DarkRing:
                        tempString = "Dark Ring";
                        break;
                    case EquipmentKind.EnhancedFireBrooch:
                        tempString = "Enhanced Fire Brooch";
                        break;
                    case EquipmentKind.EnhancedIceBrooch:
                        tempString = "Enhanced Ice Brooch";
                        break;
                    case EquipmentKind.EnhancedThunderBrooch:
                        tempString = "Enhanced Thunder Brooch";
                        break;
                    case EquipmentKind.EnhancedLightBrooch:
                        tempString = "Enhanced Light Brooch";
                        break;
                    case EquipmentKind.EnhancedDarkBrooch:
                        tempString = "Enhanced Dark Brooch";
                        break;
                    case EquipmentKind.BattleSword:
                        tempString = "Battle Sword";
                        break;
                    case EquipmentKind.BattleStaff:
                        tempString = "Battle Staff";
                        break;
                    case EquipmentKind.BattleWing:
                        tempString = "Battle Wing";
                        break;
                    case EquipmentKind.BattleDagger:
                        tempString = "Battle Dagger";
                        break;
                    case EquipmentKind.BattleBow:
                        tempString = "Battle Bow";
                        break;
                    case EquipmentKind.BattleRecorder:
                        tempString = "Battle Bagpipes";
                        break;
                    case EquipmentKind.SlimeGlove:
                        tempString = "Slime Glove";
                        break;
                    case EquipmentKind.SlimeBelt:
                        tempString = "Slime Belt";
                        break;
                    case EquipmentKind.SlimePincenez:
                        tempString = "Slime Pince-nez";
                        break;
                    case EquipmentKind.SlimeWing:
                        tempString = "Slime Wing";
                        break;
                    case EquipmentKind.SlimePoncho:
                        tempString = "Slime Poncho";
                        break;
                    case EquipmentKind.SlimeDart:
                        tempString = "Slime Dart";
                        break;
                    case EquipmentKind.MagicSlimeStick:
                        tempString = "Magicslime Stick";
                        break;
                    case EquipmentKind.MagicSlimeHat:
                        tempString = "Magicslime Hat";
                        break;
                    case EquipmentKind.MagicSlimeBow:
                        tempString = "Magicslime Bow";
                        break;
                    case EquipmentKind.MagicSlimeShoes:
                        tempString = "Magicslime Shoes";
                        break;
                    case EquipmentKind.MagicSlimeRecorder:
                        tempString = "Magicslime Recorder";
                        break;
                    case EquipmentKind.MagicSlimeEarring:
                        tempString = "Magicslime Earring";
                        break;
                    case EquipmentKind.MagicSlimeBalloon:
                        tempString = "Magicslime Balloon";
                        break;
                    case EquipmentKind.MagicSlimeSkirt:
                        tempString = "Magicslime Skirt";
                        break;
                    case EquipmentKind.SpiderHat:
                        return "Spider Hat";
                    case EquipmentKind.SpiderSkirt:
                        return "Spider Skirt";
                    case EquipmentKind.SpiderSuit:
                        return "Spider Suit";
                    case EquipmentKind.SpiderDagger:
                        return "Spider Dagger";
                    case EquipmentKind.SpiderWing:
                        return "Spider Wing";
                    case EquipmentKind.SpiderCatchingNet:
                        return "Spider Catching Net";
                    case EquipmentKind.SpiderStick:
                        return "Spider Stick";
                    case EquipmentKind.SpiderFoldingFan:
                        return "Spider Folding Fan";
                    case EquipmentKind.BatRing:
                        return "Bat Ring";
                    case EquipmentKind.BatShoes:
                        return "Bat Shoes";
                    case EquipmentKind.BatSword:
                        return "Bat Sword";
                    case EquipmentKind.BatHat:
                        return "Bat Helm";
                    case EquipmentKind.BatRecorder:
                        return "Bat Recorder";
                    case EquipmentKind.BatBow:
                        return "Bat Bow";
                    case EquipmentKind.BatMascaradeMask:
                        return "Bat Mascarade Mask";
                    case EquipmentKind.BatCloak:
                        return "Bat Cloak";
                    case EquipmentKind.BronzeShoulder:
                        return "Bronze Shoulder";
                    case EquipmentKind.BattleRing:
                        return "Battle Ring";
                    case EquipmentKind.Halo:
                        return "Halo";
                    case EquipmentKind.IronShoulder:
                        return "Iron Shoulder";
                    case EquipmentKind.StrengthRing:
                        return "Strength Ring";
                    case EquipmentKind.GoldenRing:
                        return "Golden Ring";
                    case EquipmentKind.GoldenFireRing:
                        return "Golden Fire Ring";
                    case EquipmentKind.GoldenIceRing:
                        return "Golden Ice Ring";
                    case EquipmentKind.GoldenThunderRing:
                        return "Golden Thunder Ring";
                    case EquipmentKind.GoldenLightRing:
                        return "Golden Light Ring";
                    case EquipmentKind.GoldenDarkRing:
                        return "Golden Dark Ring";
                    case EquipmentKind.IronBelt:
                        return "Iron Belt";
                    case EquipmentKind.IronShoes:
                        return "Iron Shoes";
                    case EquipmentKind.CopperArmor:
                        return "Copper Armor";
                    case EquipmentKind.IronGlove:
                        return "Iron Glove";
                    case EquipmentKind.TowerShield:
                        return "Tower Shield";
                    case EquipmentKind.FireTowerShield:
                        return "Fire Tower Shield";
                    case EquipmentKind.IceTowerShield:
                        return "Ice Tower Shield";
                    case EquipmentKind.ThunderTowerShield:
                        return "Thunder Tower Shield";
                    case EquipmentKind.LightTowerShield:
                        return "Light Tower Shield";
                    case EquipmentKind.DarkTowerShield:
                        return "Dark Tower Shield";
                    case EquipmentKind.SavageRing:
                        return "Savage Ring";
                    case EquipmentKind.SpellboundFireBrooch:
                        return "Spellbound Fire Brooch";
                    case EquipmentKind.SpellboundIceBrooch:
                        return "Spellbound Ice Brooch";
                    case EquipmentKind.SpellboundThunderBrooch:
                        return "Spellbound Thunder Brooch";
                    case EquipmentKind.SpellboundLightBrooch:
                        return "Spellbound Light Brooch";
                    case EquipmentKind.SpellboundDarkBrooch:
                        return "Spellbound Dark Brooch";
                    case EquipmentKind.CopperHelm:
                        return "Copper Helm";
                    case EquipmentKind.BattleHelm:
                        return "Battle Helm";
                    case EquipmentKind.WizardHelm:
                        return "Wizard Helm";
                    case EquipmentKind.LargeSword:
                        return "Large Sword";
                    case EquipmentKind.LargeStaff:
                        return "Large Staff";
                    case EquipmentKind.LargeWing:
                        return "Large Wing";
                    case EquipmentKind.LargeDagger:
                        return "Large Dagger";
                    case EquipmentKind.LargeBow:
                        return "Large Bow";
                    case EquipmentKind.LargeOcarina:
                        return "Large Ocarina";
                    case EquipmentKind.FairyClothes:
                        return "Fairy Clothes";
                    case EquipmentKind.FairyStaff:
                        return "Fairy Staff";
                    case EquipmentKind.FairyBoots:
                        return "Fairy Boots";
                    case EquipmentKind.FairyGlove:
                        return "Fairy Glove";
                    case EquipmentKind.FairyBrooch:
                        return "Fairy Brooch";
                    case EquipmentKind.FairyLamp:
                        return "Fairy Lamp";
                    case EquipmentKind.FairyWing:
                        return "Fairy Wing";
                    case EquipmentKind.FairyShuriken:
                        return "Fairy Shuriken";
                    case EquipmentKind.FoxKanzashi:
                        return "Fox Kanzashi";
                    case EquipmentKind.FoxLoincloth:
                        return "Fox Loincloth";
                    case EquipmentKind.FoxMask:
                        return "Fox Mask";
                    case EquipmentKind.FoxHamayayumi:
                        return "Fox Hamaya-yumi";
                    case EquipmentKind.FoxHat:
                        return "Fox Hat";
                    case EquipmentKind.FoxCoat:
                        return "Fox Coat";
                    case EquipmentKind.FoxBoot:
                        return "Fox Boot";
                    case EquipmentKind.FoxEma:
                        return "Fox Ema";
                    case EquipmentKind.DevilfishSword:
                        return "Devilfish Sword";
                    case EquipmentKind.DevilfishWing:
                        return "Devilfish Wing";
                    case EquipmentKind.DevilfishRecorder:
                        return "Devilfish Recorder";
                    case EquipmentKind.DevilfishArmor:
                        return "Devilfish Armor";
                    case EquipmentKind.DevilfishScarf:
                        return "Devilfish Scarf";
                    case EquipmentKind.DevilfishGill:
                        return "Devilfish Gill";
                    case EquipmentKind.DevilfishPendant:
                        return "Devilfish Pendant";
                    case EquipmentKind.DevilfishRing:
                        return "Devilfish Ring";
                    default:
                        //Debug
                        tempString = kind.ToString();
                        break;
                }
                break;
        }
        return tempString;
    }
    public string EquipmentEffectName(EquipmentEffectKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case EquipmentEffectKind.Nothing:
                        tempString = "エンチャントスロット";
                        break;
                    case EquipmentEffectKind.HPAdder:
                        tempString = "HP加算";
                        break;
                    default:
                        //Debug
                        tempString = kind.ToString();
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case EquipmentEffectKind.Nothing:
                        tempString = "Enchant Slot";
                        break;
                    case EquipmentEffectKind.HPAdder:
                        tempString = "HP Adder";
                        break;
                    case EquipmentEffectKind.MPAdder:
                        tempString = "MP Adder";
                        break;
                    case EquipmentEffectKind.ATKAdder:
                        tempString = "ATK Adder";
                        break;
                    case EquipmentEffectKind.MATKAdder:
                        tempString = "MATK Adder";
                        break;
                    case EquipmentEffectKind.DEFAdder:
                        tempString = "DEF Adder";
                        break;
                    case EquipmentEffectKind.MDEFAdder:
                        tempString = "MDEF Adder";
                        break;
                    case EquipmentEffectKind.SPDAdder:
                        tempString = "SPD Adder";
                        break;
                    case EquipmentEffectKind.HPMultiplier:
                        tempString = "HP Multiplier";
                        break;
                    case EquipmentEffectKind.MPMultiplier:
                        tempString = "MP Multiplier";
                        break;
                    case EquipmentEffectKind.ATKMultiplier:
                        tempString = "ATK Multiplier";
                        break;
                    case EquipmentEffectKind.MATKMultiplier:
                        tempString = "MATK Multiplier";
                        break;
                    case EquipmentEffectKind.DEFMultiplier:
                        tempString = "DEF Multiplier";
                        break;
                    case EquipmentEffectKind.MDEFMultiplier:
                        tempString = "MDEF Multiplier";
                        break;
                    case EquipmentEffectKind.ATKPropotion:
                        tempString = "ATK Proportion";
                        break;
                    case EquipmentEffectKind.MATKPropotion:
                        tempString = "MATK Proportion";
                        break;
                    case EquipmentEffectKind.DEFPropotion:
                        tempString = "DEF Proportion";
                        break;
                    case EquipmentEffectKind.MDEFPropotion:
                        tempString = "MDEF Proportion";
                        break;
                    case EquipmentEffectKind.FireResistance:
                        tempString = "Fire Resistance";
                        break;
                    case EquipmentEffectKind.IceResistance:
                        tempString = "Ice Resistance";
                        break;
                    case EquipmentEffectKind.ThunderResistance:
                        tempString = "Thunder Resistance";
                        break;
                    case EquipmentEffectKind.LightResistance:
                        tempString = "Light Resistance";
                        break;
                    case EquipmentEffectKind.DarkResistance:
                        tempString = "Dark Resistance";
                        break;
                    case EquipmentEffectKind.PhysicalAbsorption:
                        tempString = "Physical Absorption";
                        break;
                    case EquipmentEffectKind.FireAbsorption:
                        tempString = "Fire Absorption";
                        break;
                    case EquipmentEffectKind.IceAbsorption:
                        tempString = "Ice Absorption";
                        break;
                    case EquipmentEffectKind.ThunderAbsorption:
                        tempString = "Thunder Absorption";
                        break;
                    case EquipmentEffectKind.LightAbsorption:
                        tempString = "Light Absorption";
                        break;
                    case EquipmentEffectKind.DarkAbsorption:
                        tempString = "Dark Absorption";
                        break;
                    case EquipmentEffectKind.DebuffResistance:
                        tempString = "Debuff Resistance";
                        break;
                    case EquipmentEffectKind.PhysicalCritical:
                        tempString = "Physical Critical";
                        break;
                    case EquipmentEffectKind.MagicalCritical:
                        tempString = "Magical Critical";
                        break;
                    case EquipmentEffectKind.EXPGain:
                        tempString = "EXP Gain";
                        break;
                    case EquipmentEffectKind.SkillProficiency:
                        tempString = "Skill Proficiency";
                        break;
                    case EquipmentEffectKind.EquipmentProficiency:
                        tempString = "Equipment Proficiency";
                        break;
                    case EquipmentEffectKind.MoveSpeedAdder:
                        tempString = "Move Speed Adder";
                        break;
                    case EquipmentEffectKind.MoveSpeedMultiplier:
                        tempString = "Move Speed Multiplier";
                        break;
                    case EquipmentEffectKind.GoldGain:
                        tempString = "Gold Gain";
                        break;
                    case EquipmentEffectKind.StoneGain:
                        tempString = "Stone Gain";
                        break;
                    case EquipmentEffectKind.CrystalGain:
                        tempString = "Crystal Gain";
                        break;
                    case EquipmentEffectKind.LeafGain:
                        tempString = "Leaf Gain";
                        break;
                    case EquipmentEffectKind.WarriorSkillLevel:
                        tempString = "Warrior Skill";
                        break;
                    case EquipmentEffectKind.WizardSkillLevel:
                        tempString = "Wizard Skill";
                        break;
                    case EquipmentEffectKind.AngelSkillLevel:
                        tempString = "Angel Skill";
                        break;
                    case EquipmentEffectKind.ThiefSkillLevel:
                        tempString = "Thief Skill";
                        break;
                    case EquipmentEffectKind.ArcherSkillLevel:
                        tempString = "Archer Skill";
                        break;
                    case EquipmentEffectKind.TamerSkillLevel:
                        tempString = "Tamer Skill";
                        break;
                    case EquipmentEffectKind.AllSkillLevel:
                        tempString = "All Skill";
                        break;
                    case EquipmentEffectKind.SlimeKnowledge:
                        tempString = "Slime Knowledge";
                        break;
                    case EquipmentEffectKind.MagicSlimeKnowledge:
                        tempString = "Magicslime Knowledge";
                        break;
                    case EquipmentEffectKind.SpiderKnowledge:
                        tempString = "Spider Knowledge";
                        break;
                    case EquipmentEffectKind.BatKnowledge:
                        tempString = "Bat Knowledge";
                        break;
                    case EquipmentEffectKind.FairyKnowledge:
                        tempString = "Fairy Knowledge";
                        break;
                    case EquipmentEffectKind.FoxKnowledge:
                        tempString = "Fox Knowledge";
                        break;
                    case EquipmentEffectKind.DevilFishKnowledge:
                        tempString = "Devil Fish Knowledge";
                        break;
                    case EquipmentEffectKind.TreantKnowledge:
                        tempString = "Treant Knowledge";
                        break;
                    case EquipmentEffectKind.FlameTigerKnowledge:
                        tempString = "Flame Tiger Knowledge";
                        break;
                    case EquipmentEffectKind.UnicornKnowledge:
                        tempString = "Unicorn Knowledge";
                        break;
                    case EquipmentEffectKind.PhysicalDamage:
                        tempString = "Physical Damage";
                        break;
                    case EquipmentEffectKind.FireDamage:
                        tempString = "Fire Damage";
                        break;
                    case EquipmentEffectKind.IceDamage:
                        tempString = "Ice Damage";
                        break;
                    case EquipmentEffectKind.ThunderDamage:
                        tempString = "Thunder Damage";
                        break;
                    case EquipmentEffectKind.LightDamage:
                        tempString = "Light Damage";
                        break;
                    case EquipmentEffectKind.DarkDamage:
                        tempString = "Dark Damage";
                        break;
                    case EquipmentEffectKind.EquipmentDropChance:
                        tempString = "Equipment Drop";
                        break;
                    case EquipmentEffectKind.SlimeDropChance:
                        tempString = "Slime Drop";
                        break;
                    case EquipmentEffectKind.MagicSlimeDropChance:
                        tempString = "Magicslime Drop";
                        break;
                    case EquipmentEffectKind.SpiderDropChance:
                        tempString = "Spider Drop";
                        break;
                    case EquipmentEffectKind.BatDropChance:
                        tempString = "Bat Drop";
                        break;
                    case EquipmentEffectKind.FairyDropChance:
                        tempString = "Fairy Drop";
                        break;
                    case EquipmentEffectKind.FoxDropChance:
                        tempString = "Fox Drop";
                        break;
                    case EquipmentEffectKind.DevilFishDropChance:
                        tempString = "Devil Fish Drop";
                        break;
                    case EquipmentEffectKind.TreantDropChance:
                        tempString = "Treant Drop";
                        break;
                    case EquipmentEffectKind.FlameTigerDropChance:
                        tempString = "Flame Tiger Drop";
                        break;
                    case EquipmentEffectKind.UnicornDropChance:
                        tempString = "Unicorn Drop";
                        break;
                    case EquipmentEffectKind.ColorMaterialDropChance:
                        tempString = "Rare Material Drop";
                        break;
                    case EquipmentEffectKind.HpRegen:
                        tempString = "HP Regeneration";
                        break;
                    case EquipmentEffectKind.MpRegen:
                        tempString = "MP Regeneration";
                        break;
                    case EquipmentEffectKind.TamingPoint:
                        tempString = "Taming Point";
                        break;
                    case EquipmentEffectKind.WarriorSkillRange:
                        tempString = "Warrior Skill Range";
                        break;
                    case EquipmentEffectKind.WizardSkillRange:
                        tempString = "Wizard Skill Range";
                        break;
                    case EquipmentEffectKind.AngelSkillRange:
                        tempString = "Angel Skill Range";
                        break;
                    case EquipmentEffectKind.ThiefSkillRange:
                        tempString = "Thief Skill Range";
                        break;
                    case EquipmentEffectKind.ArcherSkillRange:
                        tempString = "Archer Skill Range";
                        break;
                    case EquipmentEffectKind.TamerSkillRange:
                        tempString = "Tamer Skill Range";
                        break;
                    case EquipmentEffectKind.TownMatGain:
                        return "Town Material Gain";
                    case EquipmentEffectKind.TownMatAreaClearGain:
                        return "Area Town Material";
                    //case EquipmentEffectKind.TownMatDungeonRewardGain:
                    //    return "Dungeon Town Material";
                    case EquipmentEffectKind.RebirthPointGain1:
                        return "Tier 1 Rebirth Point Gain";
                    case EquipmentEffectKind.RebirthPointGain2:
                        return "Tier 2 Rebirth Point Gain";
                    case EquipmentEffectKind.RebirthPointGain3:
                        return "Tier 3 Rebirth Point Gain";
                    case EquipmentEffectKind.CriticalDamage:
                        return "Critical Damage";
                    case EquipmentEffectKind.BlessingEffect:
                        return "Blessing Effect";
                    default:
                        //Debug
                        tempString = kind.ToString();
                        break;
                }
                break;
        }
        return tempString;
    }
    public string EquipmentEffect(EquipmentEffectKind kind, double value, bool isLevelMaxEffect = false, double perLevelValue = 0)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case EquipmentEffectKind.Nothing:
                        if (isLevelMaxEffect) tempString = "Enchant Slot + " + tDigit(value);
                        else tempString = "[エンチャント可能]";
                        break;
                    case EquipmentEffectKind.HPAdder:
                        tempString = "HP + " + tDigit(value);
                        break;
                    default:
                        //Debug
                        tempString = kind.ToString() + " + " + tDigit(value);
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case EquipmentEffectKind.Nothing:
                        if (isLevelMaxEffect) tempString = "Enchant Slot + " + tDigit(value);
                        else tempString = "[Enchant Available]";
                        break;
                    case EquipmentEffectKind.HPAdder:
                        tempString = "HP + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MPAdder:
                        tempString = "MP + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ATKAdder:
                        tempString = "ATK + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MATKAdder:
                        tempString = "MATK + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DEFAdder:
                        tempString = "DEF + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MDEFAdder:
                        tempString = "MDEF + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SPDAdder:
                        tempString = "SPD + " + tDigit(value, 1);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.HPMultiplier:
                        tempString = "HP + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MPMultiplier:
                        if (value < 0)
                        {
                            tempString = "MP <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "MP + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.ATKMultiplier:
                        if (value < 0)
                        {
                            tempString = "ATK <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "ATK + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.MATKMultiplier:
                        if (value < 0)
                        {
                            tempString = "MATK <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "MATK + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.DEFMultiplier:
                        if (value < 0)
                        {
                            tempString = "DEF <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "DEF + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.MDEFMultiplier:
                        if (value < 0)
                        {
                            tempString = "MDEF <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "MDEF + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.ATKPropotion:
                        tempString = "ATK + " + percent(value) + " of Hero Level";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MATKPropotion:
                        tempString = "MATK + " + percent(value) + " of Hero Level";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DEFPropotion:
                        tempString = "DEF + " + percent(value) + " of Hero Level";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MDEFPropotion:
                        tempString = "MDEF + " + percent(value) + " of Hero Level";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FireResistance:
                        tempString = "Fire Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.IceResistance:
                        tempString = "Ice Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ThunderResistance:
                        tempString = "Thunder Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.LightResistance:
                        tempString = "Light Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DarkResistance:
                        tempString = "Dark Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.PhysicalAbsorption:
                        tempString = "Physical Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FireAbsorption:
                        tempString = "Fire Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.IceAbsorption:
                        tempString = "Ice Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ThunderAbsorption:
                        tempString = "Thunder Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.LightAbsorption:
                        tempString = "Light Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DarkAbsorption:
                        tempString = "Dark Absorption + " + percent(value) + " of received damage";
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DebuffResistance:
                        tempString = "Debuff Resistance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.PhysicalCritical:
                        tempString = "Physical Critical Chance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MagicalCritical:
                        tempString = "Magical Critical Chance + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.EXPGain:
                        tempString = "EXP Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SkillProficiency:
                        tempString = "Skill Proficiency Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.EquipmentProficiency:
                        tempString = "Equipment Proficiency Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MoveSpeedAdder:
                        if (value < 0)
                        {
                            tempString = "Move Speed <color=red>" + meter(value) + " / sec</color>";
                            if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "Move Speed + " + meter(value) + " / sec";
                            if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.MoveSpeedMultiplier:
                        if (value < 0)
                        {
                            tempString = "Move Speed <color=red>" + percent(value) + "</color>";
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        else
                        {
                            tempString = "Move Speed + " + percent(value);
                            if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        }
                        break;
                    case EquipmentEffectKind.GoldGain:
                        tempString = "Gold Gain (Global) + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.StoneGain:
                        tempString = "Stone Gain (Global) + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.CrystalGain:
                        tempString = "Crystal Gain (Global) + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.LeafGain:
                        tempString = "Leaf Gain (Global) + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.WarriorSkillLevel:
                        tempString = "Warrior Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.WizardSkillLevel:
                        tempString = "Wizard Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.AngelSkillLevel:
                        tempString = "Angel Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ThiefSkillLevel:
                        tempString = "Thief Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ArcherSkillLevel:
                        tempString = "Archer Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TamerSkillLevel:
                        tempString = "Tamer Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.AllSkillLevel:
                        tempString = "All Skill Level (Global) + " + tDigit(value, 1); //tDigit(System.Math.Floor(value));
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SlimeKnowledge:
                        tempString = "Damage to Slime + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MagicSlimeKnowledge:
                        tempString = "Damage to Magicslime + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SpiderKnowledge:
                        tempString = "Damage to Spider + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.BatKnowledge:
                        tempString = "Damage to Bat + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FairyKnowledge:
                        tempString = "Damage to Fairy + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FoxKnowledge:
                        tempString = "Damage to Fox + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DevilFishKnowledge:
                        tempString = "Damage to Devil Fish + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TreantKnowledge:
                        tempString = "Damage to Treant + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FlameTigerKnowledge:
                        tempString = "Damage to Flame Tiger + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.UnicornKnowledge:
                        tempString = "Damage to Unicorn + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.PhysicalDamage:
                        tempString = "Physical Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FireDamage:
                        tempString = "Fire Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.IceDamage:
                        tempString = "Ice Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ThunderDamage:
                        tempString = "Thunder Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.LightDamage:
                        tempString = "Light Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DarkDamage:
                        tempString = "Dark Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.EquipmentDropChance:
                        tempString = "Equipment Drop Chance + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 4) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SlimeDropChance:
                        tempString = Material(MaterialKind.OilOfSlime) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MagicSlimeDropChance:
                        tempString = Material(MaterialKind.EnchantedCloth) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.SpiderDropChance:
                        tempString = Material(MaterialKind.SpiderSilk) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.BatDropChance:
                        tempString = Material(MaterialKind.BatWing) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FairyDropChance:
                        tempString = Material(MaterialKind.FairyDust) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FoxDropChance:
                        tempString = Material(MaterialKind.FoxTail) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.DevilFishDropChance:
                        tempString = Material(MaterialKind.FishScales) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TreantDropChance:
                        tempString = Material(MaterialKind.CarvedBranch) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.FlameTigerDropChance:
                        tempString = Material(MaterialKind.ThickFur) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.UnicornDropChance:
                        tempString = Material(MaterialKind.UnicornHorn) + " Drop Chance (Global) + " + percent(value, 3);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 3) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ColorMaterialDropChance:
                        tempString = "Rare Material Drop Chance (Global) + " + percent(value, 4);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue, 4) + " / Lv )";
                        break;
                    case EquipmentEffectKind.HpRegen:
                        tempString = "HP Regeneration + " + tDigit(value, 2) + " / sec";
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.MpRegen:
                        tempString = "MP Regeneration + " + tDigit(value, 2) + " / sec";
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TamingPoint:
                        tempString = "Taming Point Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.WarriorSkillRange:
                        tempString = "Warrior Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.WizardSkillRange:
                        tempString = "Wizard Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.AngelSkillRange:
                        tempString = "Angel Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ThiefSkillRange:
                        tempString = "Thief Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.ArcherSkillRange:
                        tempString = "Archer Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TamerSkillRange:
                        tempString = "Tamer Skill's Range + " + meter(value);
                        if (perLevelValue > 0) tempString += " ( + " + meter(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TownMatGain:
                        tempString = "Town Material Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.TownMatAreaClearGain:
                        tempString = "Town Material Gain from Area Clear + " + tDigit(value);
                        if (perLevelValue > 0) tempString += " ( + " + tDigit(perLevelValue, 2) + " / Lv )";
                        break;
                    //case EquipmentEffectKind.TownMatDungeonRewardGain:
                    //    tempString = "Town Material Gain from Dungeon Reward + " + percent(value);
                    //    if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                    //    break;
                    case EquipmentEffectKind.RebirthPointGain1:
                        tempString = "Tier 1 Rebirth Point Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.RebirthPointGain2:
                        tempString = "Tier 2 Rebirth Point Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.RebirthPointGain3:
                        tempString = "Tier 3 Rebirth Point Gain + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.CriticalDamage:
                        tempString = "Critical Damage + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    case EquipmentEffectKind.BlessingEffect:
                        tempString = "Blessing Effect + " + percent(value);
                        if (perLevelValue > 0) tempString += " ( + " + percent(perLevelValue) + " / Lv )";
                        break;
                    default:
                        //Debug
                        tempString = kind.ToString() + " + " + tDigit(value);
                        break;
                }
                break;
        }
        return tempString;
    }
    //EquipmentDictionary
    public (string name, string effect) DictionaryUpgarde(DictionaryUpgradeKind kind)
    {
        string name = "";
        string effect = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case DictionaryUpgradeKind.EquipmentProficiencyGainWarrior:
                        name = "Expert of EQ [Warrior]";
                        effect = "Warrior's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentProficiencyGainWizard:
                        name = "Expert of EQ [Wizard]";
                        effect = "Wizard's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentProficiencyGainAngel:
                        name = "Expert of EQ [Angel]";
                        effect = "Angel's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentProficiencyGainThief:
                        name = "Expert of EQ [Thief]";
                        effect = "Thief's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentProficiencyGainArcher:
                        name = "Expert of EQ [Archer]";
                        effect = "Archer's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentProficiencyGainTamer:
                        name = "Expert of EQ [Tamer]";
                        effect = "Tamer's Equipment Proficiency Gain";
                        break;
                    case DictionaryUpgradeKind.EquipmentDropChance:
                        name = "Treasure Hunting";
                        effect = "Equipment Drop Chance (except for Uniques) ";
                        break;
                    case DictionaryUpgradeKind.EnchantedEffectChance1:
                        name = "Treasure of Fortune 1";
                        effect = "Additional chance to have 1st Enchanted slot effect";
                        break;
                    case DictionaryUpgradeKind.EnchantedEffectChance2:
                        name = "Treasure of Fortune 2";
                        effect = "Additional chance to have 2nd Enchanted slot effect";
                        break;
                    case DictionaryUpgradeKind.EnchantedEffectChance3:
                        name = "Treasure of Fortune 3";
                        effect = "Additional chance to have 3rd Enchanted slot effect";
                        break;
                }
                break;
        }
        return (name, effect);
    }
    //Enchant
    public string EnchantName(EnchantKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case EnchantKind.OptionAdd:
                        tempString = "エンチャントの巻物";
                        break;
                    case EnchantKind.OptionLevelup:
                        tempString = "レベルアップの巻物";
                        break;
                    case EnchantKind.OptionLevelMax:
                        tempString = "レベル最大化の巻物";
                        break;
                    case EnchantKind.OptionLottery:
                        tempString = "再抽選の巻物";
                        break;
                    case EnchantKind.OptionDelete:
                        tempString = "消去の巻物";
                        break;
                    case EnchantKind.OptionExtract:
                        tempString = "抽出の巻物";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case EnchantKind.OptionAdd:
                        tempString = "Enchant Scroll";
                        break;
                    case EnchantKind.OptionLevelup:
                        tempString = "Level Up Scroll";
                        break;
                    case EnchantKind.OptionLevelMax:
                        tempString = "Level Max Scroll";
                        break;
                    case EnchantKind.OptionLottery:
                        tempString = "Re-lottery Scroll";
                        break;
                    case EnchantKind.OptionDelete:
                        tempString = "Delete Scroll";
                        break;
                    case EnchantKind.OptionExtract:
                        tempString = "Extract Scroll";
                        break;
                    case EnchantKind.OptionCopy:
                        return "Copy Scroll";
                    case EnchantKind.ExpandEnchantSlot:
                        return "Expand Scroll";
                    case EnchantKind.InstantProf:
                        return "Proficiency Scroll";
                }
                break;
        }
        return tempString;
    }
    public string EnchantInformation(EnchantKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese://未
                switch (kind)
                {
                    case EnchantKind.OptionAdd:
                        tempString = "エンチャントの巻物";
                        break;
                    case EnchantKind.OptionLevelup:
                        tempString = "オプションレベル増幅器";
                        break;
                    case EnchantKind.OptionLevelMax:
                        tempString = "Level Max Scroll";
                        break;
                    case EnchantKind.OptionLottery:
                        tempString = "Re-lottery Scroll";
                        break;
                    case EnchantKind.OptionDelete:
                        tempString = "Delete Scroll";
                        break;
                    case EnchantKind.OptionExtract:
                        tempString = "Extract";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case EnchantKind.OptionAdd:
                        tempString = "Gives an enchanted effect to an equipment that has [Enchant Slot].";
                        break;
                    case EnchantKind.OptionLevelup:
                        tempString = "Increases an enchanted effect's level of an equipment that is not maxed yet.";
                        break;
                    case EnchantKind.OptionLevelMax:
                        tempString = "Increases an enchanted effect's level of an equipment to max.";
                        break;
                    case EnchantKind.OptionLottery:
                        tempString = "Re-lottery an enchanted effect amount of an equipment.";
                        break;
                    case EnchantKind.OptionDelete:
                        tempString = "Deletes an enchanted effect of an equipment and changes it an available enchant slot.";
                        break;
                    case EnchantKind.OptionExtract:
                        tempString = "Extracts an enchanted effect of an equipment and creates its Enchant Scroll. The extracted enchanted effect will be deleted.";
                        break;
                    case EnchantKind.OptionCopy:
                        return "Copies an enchanted effect of an equipment and create its Enchant Scroll. The original enchanted effect will remain.";
                    case EnchantKind.ExpandEnchantSlot:
                        return "Expands available enchant slot of an equipment by 1. Max 4 slots in total except for Thief's Mastery Effect.";
                    case EnchantKind.InstantProf:
                        return "Instantly gains the current playing Hero's Equipment Proficiency of an equipment.";
                }
                break;
        }
        return tempString;
    }
    //Alchemy//未
    public (string name, string description) Catalyst(Catalyst catalyst)
    {
        string tempName = "<size=20>";
        string tempDescription = optStr;
        tempDescription += "<size=20><u>Available Essence Conversions</u><size=18>";
        for (int i = 0; i < catalyst.essenceProductionList.Count; i++)
        {
            tempDescription += "\n- " + EssenceName(catalyst.essenceProductionList[i].kind);
        }
        tempDescription += "\n\n<size=20><u>Critical</u><size=18>";
        tempDescription += "\n- " + percent(catalyst.criticalChance, 3) + " chance to gain " + Material(catalyst.criticalMaterial) + " on every conversion of this Catalyst's Essences";
        tempDescription += "\n\n<size=20><u>" + Basic(BasicWord.LevelupCost) + "</u>";
        tempDescription += "  ( <color=green>+ " + tDigit(catalyst.transaction.LevelIncrement()) + "</color> ) </u><size=18>";
        tempDescription += "\n- " + "Mysterious Water" + " : ";
        if (catalyst.transaction.CanBuy(0)) tempDescription += "<color=green>";
        else tempDescription += "<color=red>";
        tempDescription += tDigit(catalyst.transaction.ResourceValue(0)) + "</color> / ";
        tempDescription += tDigit(catalyst.transaction.Cost(0));
        for (int i = 0; i < catalyst.transaction.materialKindList.Count; i++)
        {
            int count = i;
            tempDescription += "\n- " + localized.Material(catalyst.transaction.materialKindList[count]) + " : ";
            if (catalyst.transaction.CanBuy(1 + count)) tempDescription += "<color=green>";
            else tempDescription += "<color=red>";
            tempDescription += tDigit(catalyst.transaction.ResourceValue(1 + count)) + "</color> / ";
            tempDescription += tDigit(catalyst.transaction.Cost(1 + count));
        }
        switch (catalyst.kind)
        {
            case CatalystKind.Slime:
                tempName = "Slime Catalyst";
                break;
            case CatalystKind.Mana:
                tempName = "Mana Catalyst";
                break;
            case CatalystKind.Frost:
                tempName = "Frost Catalyst";
                break;
            case CatalystKind.Flame:
                tempName = "Flame Catalyst";
                break;
            case CatalystKind.Storm:
                tempName = "Storm Catalyst";
                break;
            case CatalystKind.Soul:
                tempName = "Soul Catalyst";
                break;
            case CatalystKind.Sun:
                tempName = "Sun Catalyst";
                break;
            case CatalystKind.Void:
                tempName = "Void Catalyst";
                break;
        }
        tempName += "<size=18>\n\n<color=green>Lv " + tDigit(catalyst.level.value) + "</color> / " + tDigit(catalyst.level.maxValue());
        if (catalyst.level.value > 0 && !catalyst.isEquipped) tempName += "\n<color=yellow>Click this icon to show Essence Converter!</color>";
        return (tempName, tempDescription);
    }
    public string EssenceName(EssenceKind kind)
    {
        string tempString = kind.ToString();//未
        switch (language)
        {
            case Language.Japanese:

                break;
            default:
                switch (kind)
                {
                    case EssenceKind.EssenceOfSlime:
                        return "Essence of Slime";
                    case EssenceKind.EssenceOfLife:
                        return "Essence of Life";
                    case EssenceKind.EssenceOfMagic:
                        return "Essence of Magic";
                    case EssenceKind.EssenceOfCreation:
                        return "Essence of Creation";
                    case EssenceKind.EssenceOfIce:
                        return "Essence of Ice";
                    case EssenceKind.EssenceOfWinter:
                        return "Essence of Winter";
                    case EssenceKind.EssenceOfFire:
                        return "Essence of Fire";
                    case EssenceKind.EssenceOfSummer:
                        return "Essence of Summer";
                    case EssenceKind.EssenceOfThunder:
                        return "Essence of Thunder";
                    case EssenceKind.EssenceOfWind:
                        return "Essence of Wind";
                    case EssenceKind.EssenceOfSpirit:
                        return "Essence of Spirit";
                    case EssenceKind.EssenceOfDeath:
                        return "Essence of Death";
                    case EssenceKind.EssenceOfLight:
                        return "Essence of Light";
                    case EssenceKind.EssenceOfRebirth:
                        return "Essence of Rebirth";
                    case EssenceKind.EssenceOfTime:
                        return "Essence of Time";
                    case EssenceKind.EssenceOfEternity:
                        return "Essence of Eternity";
                }
                break;
        }
        return tempString;

    }
    //AlchemyUpgrade
    public string AlchemyUpgradeName(AlchemyUpgradeKind kind)
    {
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case AlchemyUpgradeKind.Purification:
                        return "Purification";
                    case AlchemyUpgradeKind.DeeperCapacity:
                        return "Deeper Capacity";
                    case AlchemyUpgradeKind.CharmedLife:
                        return "Charmed Life";
                    case AlchemyUpgradeKind.Catalystic:
                        return "Catalystic";
                    case AlchemyUpgradeKind.EssenceHoarder:
                        return "Essence Hoarder";
                    case AlchemyUpgradeKind.PotentPotables:
                        return "Potent Potables";
                    //case AlchemyUpgradeKind.BusierHands:
                    //    return "Busier Hands";
                    //case AlchemyUpgradeKind.RecyclingPro:
                    //    return "Recycling Pro";
                    case AlchemyUpgradeKind.Aurumology:
                        return "Aurumology";
                    case AlchemyUpgradeKind.WaterPreservation:
                        return "Water Preservation";
                    case AlchemyUpgradeKind.MaterialThrift:
                        return "Material Thrift";
                    case AlchemyUpgradeKind.NitrousExtraction:
                        return "Nitrous Extraction";
                }
                break;
        }
        return "";
    }
    public string AlchemyUpgradeEffect(AlchemyUpgradeKind kind, double effectValue)
    {
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case AlchemyUpgradeKind.Purification:
                        return "Mysterious Water Gain + " + tDigit(effectValue, 3) + " / " + Basic(BasicWord.Sec);
                    case AlchemyUpgradeKind.DeeperCapacity:
                        return "Utility Item's max stack # + " + tDigit(effectValue);
                    case AlchemyUpgradeKind.CharmedLife:
                        return percent(effectValue) + " chance to prevent potions and traps from being consumed";
                    case AlchemyUpgradeKind.Catalystic:
                        return "Available Catalysts + "+ tDigit(effectValue);
                    case AlchemyUpgradeKind.EssenceHoarder:
                        return "Essence Conversion Rate + " + percent(effectValue);
                    case AlchemyUpgradeKind.PotentPotables:
                        return "Potion's effect + " + percent(effectValue);
                    //case AlchemyUpgradeKind.BusierHands:
                    //    break;
                    //case AlchemyUpgradeKind.RecyclingPro:
                    //    break;
                    case AlchemyUpgradeKind.Aurumology:
                        return "Gold Cap + " + percent(effectValue);
                    case AlchemyUpgradeKind.WaterPreservation:
                        return "Mysterious Water Gain + " + percent(effectValue) + " per expanding cap";
                    case AlchemyUpgradeKind.MaterialThrift:
                        return "Provides " + percent(effectValue) + " chance that no essence components will be consumed on alchemising";
                    case AlchemyUpgradeKind.NitrousExtraction:
                        return "Provides 1% chance to gain " + tDigit(effectValue) + " Nitro on alchemising";
                }
                break;
        }
        return "";
    }

    //Potion
    public string PotionName(PotionKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case PotionKind.MinorHealthPotion:
                        return "Minor Health Potion";
                    case PotionKind.MinorRegenerationPoultice:
                        return "Minor Regeneration Poultice";
                    case PotionKind.MinorResourcePoultice:
                        return "Minor Resource Poultice";
                    case PotionKind.SlickShoeSolution:
                        return "Slick Shoe Solution";
                    case PotionKind.MinorManaRegenerationPoultice:
                        return "Minor Mana Regeneration Poultice";
                    case PotionKind.MaterialMultiplierMist:
                        return "Material Multiplier Mist";
                    case PotionKind.BasicElixirOfBrawn:
                        return "Basic Elixir of Brawn";
                    case PotionKind.BasicElixirOfBrains:
                        return "Basic Elixir of Brains";
                    case PotionKind.BasicElixirOfFortitude:
                        return "Basic Elixir of Fortitude";
                    case PotionKind.BasicElixirOfConcentration:
                        return "Basic Elixir of Concentration";
                    case PotionKind.BasicElixirOfUnderstanding:
                        return "Basic Elixir of Understanding";
                    case PotionKind.ChilledHealthPotion:
                        return "Chilled Health Potion";
                    case PotionKind.ChilledRegenerationPoultice:
                        return "Chilled Regeneration Poultice";
                    case PotionKind.FrostyDefensePotion:
                        return "Frosty Defense Potion";
                    case PotionKind.IcyAuraDraught:
                        return "Ice Aura Draught";
                    case PotionKind.SlightlyStickySalve:
                        return "Slightly Sticky Salve";
                    case PotionKind.SlickerShoeSolution:
                        return "Slicker Shoe Solution";
                    case PotionKind.CoolHeadOintment:
                        return "Cool Head Ointment";
                    case PotionKind.FrostySlayersOil:
                        return "Frosty Slayer's Oil";
                    case PotionKind.BurningDefensePotion:
                        return "Burning Defense Potion";
                    case PotionKind.BlazingAuraDraught:
                        return "Blazing Aura Draught";
                    case PotionKind.FierySlayersOil:
                        return "Fiery Slayer's Oil";
                    case PotionKind.ElectricDefensePotion:
                        return "Electric Defense Potion";
                    case PotionKind.WhirlingAuraDraught:
                        return "Whirling Aura Draught";
                    case PotionKind.ShockingSlayersOil:
                        return "Shocking Slayer's Oil";
                    case PotionKind.ThrowingNet:
                        return "Throwing Net";
                    case PotionKind.FireRope:
                        return "Fire Net";
                    case PotionKind.IceRope:
                        return "Ice Net";
                    case PotionKind.ThunderRope:
                        return "Thunder Net";
                    case PotionKind.LightRope:
                        return "Light Net";
                    case PotionKind.DarkRope:
                        return "Dark Net";
                    //case PotionKind.GuildMembersEmblem:
                    //    return "Guild Member's Emblem";
                    //case PotionKind.CertificateOfCompetence:
                    //    return "Certificate of Competence";
                    //case PotionKind.MasonsTrowel:
                    //    return "Mason's Trowel";
                    //case PotionKind.EnchantedAlembic:
                    //    return "Enchanted Alembic";
                    //case PotionKind.TrackersMap:
                    //    return "Tracker's Map";
                    //case PotionKind.BerserkersStone:
                    //    return "Berserker's Stone";
                    ////case PotionKind.WitchsCauldron:
                    ////    return "Witch's Cauldron";
                    //case PotionKind.TrappersTag:
                    //    return "Trapper's Tag";
                    //case PotionKind.HitanDoll:
                    //    return "Hitan Doll";
                    //case PotionKind.RingoldDoll:
                    //    return "Ringold Doll";
                    //case PotionKind.NuttyDoll:
                    //    return "Nutty Doll";
                    //case PotionKind.MorkylDoll:
                    //    return "Morkyl Doll";
                    case PotionKind.FlorzporbDoll:
                        return "Florzporb Doll";
                    case PotionKind.ArachnettaDoll:
                        return "Arachnetta Doll";
                    case PotionKind.GuardianKorDoll:
                        return "Guardian Kor Doll";
                    case PotionKind.SlimeBadge:
                        return "Slime Badge";
                    case PotionKind.MagicslimeBadge:
                        return "Magicslime Badge";
                    case PotionKind.SpiderBadge:
                        return "Spider Badge";
                    case PotionKind.BatBadge:
                        return "Bat Badge";
                    case PotionKind.FairyBadge:
                        return "Fairy Badge";
                    case PotionKind.FoxBadge:
                        return "Fox Badge";
                    case PotionKind.DevilfishBadge:
                        return "Devilfish Badge";
                    case PotionKind.TreantBadge:
                        return "Treant Badge";
                    case PotionKind.FlametigerBadge:
                        return "Flametiger Badge";
                    case PotionKind.UnicornBadge:
                        return "Unicorn Badge";
                    case PotionKind.AscendedFromIEH1:
                        return "Proof of Ascension from IEH1";
                    case PotionKind.WarriorsBadge:
                        return "Warrior's Badge";
                    case PotionKind.WizardsBadge:
                        return "Wizard's Badge";
                    case PotionKind.AngelsBadge:
                        return "Angel's Badge";
                    case PotionKind.ThiefsBadge:
                        return "Thief's Badge";
                    case PotionKind.ArchersBadge:
                        return "Archer's Badge";
                    case PotionKind.TamersBadge:
                        return "Tamer's Badge";
                }
                break;
        }
        tempString = kind.ToString();
        return tempString;
    }
    public string PotionConsume(PotionConsumeConditionKind kind, double chance)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case PotionConsumeConditionKind.Nothing://Talisman
                        return "Talisman is never consumed and its effect is multiplied by Stack #";
                    case PotionConsumeConditionKind.HpHalf:
                        return "Automatically used when hero's HP is less than 50%";
                    case PotionConsumeConditionKind.AreaComplete:
                        return optStr + percent(chance,3) + " chance to be consumed every time you attempt an area";
                    case PotionConsumeConditionKind.Defeat:
                        return optStr + percent(chance,3) + " chance to be consumed every time you defeat a monster";
                    case PotionConsumeConditionKind.Move:
                        return optStr + percent(chance,3) + " chance to be consumed every time you walk 10 meters";
                    case PotionConsumeConditionKind.Capture:
                        return "Right-click to try capturing a monster";
                }
                break;
        }
        return tempString;
    }
    public string PotionEffect(PotionKind kind, double effectValue, bool isPassive = false)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case PotionKind.MinorHealthPotion:
                        return optStr + "Restores HP + " + tDigit(effectValue) + "</color>";
                    case PotionKind.MinorRegenerationPoultice:
                        return optStr + "HP Regenerate + " + tDigit(effectValue, 2) + " / " + Basic(BasicWord.Sec) + "</color>";
                    case PotionKind.MinorResourcePoultice:
                        return optStr + "Resource Gain + " + percent(effectValue) + "</color>";
                    case PotionKind.SlickShoeSolution:
                        return optStr + "Move Speed + " + percent(effectValue) + "</color>";
                    case PotionKind.MinorManaRegenerationPoultice:
                        return optStr + "MP Regenerate + " + tDigit(effectValue, 2) + " / " + Basic(BasicWord.Sec) + "</color>";
                    case PotionKind.MaterialMultiplierMist:
                        return optStr + "Increases dropped amount of materials by " + tDigit(effectValue) + "</color>";
                    case PotionKind.BasicElixirOfBrawn:
                        return optStr + "Physical Damage + " + percent(effectValue) + "</color>";
                    case PotionKind.BasicElixirOfBrains:
                        return optStr + "Magical Damage + " + percent(effectValue) + "</color>";
                    case PotionKind.BasicElixirOfFortitude:
                        return optStr + "HP + " + tDigit(effectValue) + "</color>";
                    case PotionKind.BasicElixirOfConcentration:
                        return optStr + "MP + " + tDigit(effectValue) + "</color>";
                    case PotionKind.BasicElixirOfUnderstanding:
                        return optStr + "Skill Proficiency Gain + " + percent(effectValue) + "</color>";
                    case PotionKind.ChilledHealthPotion:
                        return optStr + "Restores HP + " + tDigit(effectValue) + "</color>";
                    case PotionKind.ChilledRegenerationPoultice:
                        return optStr + "HP Regenerate + " + tDigit(effectValue, 2) + " / " + Basic(BasicWord.Sec) + "</color>";
                    case PotionKind.FrostyDefensePotion:
                        return optStr + "Ice Resistance + " + percent(effectValue, 2) + "</color>";
                    case PotionKind.IcyAuraDraught:
                        return optStr + "Generates an aura of cold around hero that has " + percent(effectValue) + "</color> chance every second to give monsters " + DebuffName(Debuff.SpdDown) + " debuff";
                    case PotionKind.SlightlyStickySalve:
                        return optStr + "Gold Gain + " + percent(effectValue) + "</color>";
                    case PotionKind.SlickerShoeSolution:
                        return optStr + "Move Speed + " + percent(effectValue) + "</color>";
                    case PotionKind.CoolHeadOintment:
                        return optStr + "EXP Gain + " + percent(effectValue) + "</color>";
                    case PotionKind.FrostySlayersOil:
                        return optStr + "Changes skills' damage type to Ice and adds extra " + percent(effectValue) + "</color> ice damage";
                    case PotionKind.BurningDefensePotion:
                        return optStr + "Fire Resistance + " + percent(effectValue, 2) + "</color>";
                    case PotionKind.BlazingAuraDraught:
                        return optStr + "Generates an aura of fire around hero that has " + percent(effectValue) + "</color> chance every second to knockback monsters";
                    case PotionKind.FierySlayersOil:
                        return optStr + "Changes skills' damage type to Fire and adds extra " + percent(effectValue) + "</color> fire damage";
                    case PotionKind.ElectricDefensePotion:
                        return optStr + "Thunder Resistance + " + percent(effectValue, 2) + "</color>";
                    case PotionKind.WhirlingAuraDraught:
                        return optStr + "Generates an aura of thunder around hero that has " + percent(effectValue) + "</color> chance every second to give monsters " + DebuffName(Debuff.Electric) + " debuff";
                    case PotionKind.ShockingSlayersOil:
                        return optStr + "Changes skills' damage type to Thunder and adds extra " + percent(effectValue) + "</color> thunder damage";
                    case PotionKind.ThrowingNet:
                        return optStr + "Right-click to capture <color=green>Normal Type</color> monsters";
                    case PotionKind.FireRope:
                        return optStr + "Right-click to capture <color=green>Red Type</color> monsters";
                    case PotionKind.IceRope:
                        return optStr + "Right-click to capture <color=green>Blue Type</color> monsters";
                    case PotionKind.ThunderRope:
                        return optStr + "Right-click to capture <color=green>Yellow Type</color> monsters";
                    case PotionKind.LightRope:
                        return optStr + "Right-click to capture <color=green>Green Type</color> monsters";
                    case PotionKind.DarkRope:
                        return optStr + "Right-click to capture <color=green>Purple Type</color> monsters";

                    //Talisman
                    //case PotionKind.GuildMembersEmblem:
                    //    return "Increases Guild EXP Gain by " + percent(effectValue);
                    //case PotionKind.CertificateOfCompetence:
                    //    if (isPassive) return "All Skill Level + " + tDigit(effectValue);
                    //    return "Decreases Skill's cooldown by " + percent(effectValue);
                    //case PotionKind.MasonsTrowel:
                    //    if (isPassive) return "Increases Town Building's level effect by " + percent(effectValue);
                    //    return "Increases Town Material Gain by " + percent(effectValue);
                    //case PotionKind.EnchantedAlembic:
                    //    //現在、この効果は全てのキャラクター共通のため、注意が必要
                    //    if (isPassive) return "On alchemise, " + percent(effectValue) + " chance to double points acquired";
                    //    return "Increases Potion's Effect by " + percent(effectValue);
                    //case PotionKind.TrackersMap:
                    //    //現在、この効果は全てのキャラクター共通のため、注意が必要
                    //    if (isPassive) return "Nothing";
                    //    return "Increases Area Clear # and Clear Reward by " + tDigit(effectValue);
                    //case PotionKind.BerserkersStone:
                    //    if (isPassive) return "Nothing";
                    //    return "When HP is " + percent(effectValue) + " or less, ATK/MATK/SPD are tripled and DEF/MDEF get reduced to 0";
                    ////case PotionKind.WitchsCauldron:
                    ////    break;
                    //case PotionKind.TrappersTag:
                    //    if (isPassive) return "Increases Taming Point Gain by " + percent(effectValue);
                    //    return "On capture, " + percent(effectValue) + " chance to triple the capture #";
                    //case PotionKind.HitanDoll:
                    //    break;
                    //case PotionKind.RingoldDoll:
                    //    break;
                    //case PotionKind.NuttyDoll:
                    //    break;
                    //case PotionKind.MorkylDoll:
                    //    break;
                    case PotionKind.FlorzporbDoll:
                        if (isPassive) return "Multiplies Gold Gain by " + percent(1 + effectValue);
                        return "Gives additional Slime Ball attack to Base Attack Skill with " + percent(effectValue) + " damage";
                    //if (isPassive) return "Increases Slime Coin Interest rate by " + percent(effectValue);
                    //return "Increases Gold Gain by " + percent(effectValue);
                    case PotionKind.ArachnettaDoll:
                        if (isPassive) return "Gold Cap + " + percent(effectValue);
                        return "Gives Poison debuff to Base Attack Skill with " + percent(System.Math.Min(1, effectValue)) + " chance";
                    case PotionKind.GuardianKorDoll:
                        if (isPassive) return "Multiplies Equipment Proficiency Gain by " + percent(1 + effectValue);
                        return "If damage taken " + percent(System.Math.Min(0.25d, effectValue)) + " or less of HP, nullifies it (Max:25%)";
                    case PotionKind.SlimeBadge:
                        if (isPassive) return "HP + " + tDigit(effectValue, 1);
                        return "HP + " + percent(effectValue);
                    case PotionKind.MagicslimeBadge:
                        if (isPassive) return "MDEF + " + tDigit(effectValue, 1);
                        return "MDEF + " + percent(effectValue);
                    case PotionKind.SpiderBadge:
                        if (isPassive) return "DEF + " + tDigit(effectValue, 1);
                        return "DEF + " + percent(effectValue);
                    case PotionKind.BatBadge:
                        if (isPassive) return "ATK + " + tDigit(effectValue, 1);
                        return "ATK + " + percent(effectValue);
                    case PotionKind.FairyBadge:
                        if (isPassive) return "MATK + " + tDigit(effectValue, 1);
                        return "MATK + " + percent(effectValue);
                    case PotionKind.FoxBadge:
                        if (isPassive) return "MP + " + tDigit(effectValue, 1);
                        return "MP + " + percent(effectValue);
                    case PotionKind.DevilfishBadge:
                        if (isPassive) return "Stone Gain + " + percent(effectValue);
                        return "Move Speed + " + percent(effectValue);
                    case PotionKind.TreantBadge:
                        if (isPassive) return "Crystal Gain + " + percent(effectValue);
                        return "EXP Gain + " + percent(effectValue);
                    case PotionKind.FlametigerBadge:
                        if (isPassive) return "Leaf Gain + " + percent(effectValue);
                        return "Equipment Proficiency Gain + " + percent(effectValue);
                    case PotionKind.UnicornBadge:
                        if (isPassive) return "SPD + " + tDigit(effectValue, 1);
                        return "SPD + " + percent(effectValue);
                    case PotionKind.AscendedFromIEH1:
                        if (isPassive) return "Nothing";
                        return "Multiplies EXP Gain by " + percent(1 + effectValue);
                    case PotionKind.WarriorsBadge:
                        //PhyCrit
                        if (isPassive) return "Reduces Warrior's Skill Rank Cost by " + percent(effectValue);
                        return "Multiplies Physical Critical Chance by " + percent(1 + effectValue);//"Warrior's Skill Proficiency Gain + " + percent(effectValue);
                    case PotionKind.WizardsBadge:
                        //MagCrit
                        if (isPassive) return "Reduces Wizard's Skill Rank Cost by " + percent(effectValue);
                        return "Multiplies Magical Critical Chance by " + percent(1 + effectValue);
                    case PotionKind.AngelsBadge:
                        //SkillProfGain(Mul)
                        if (isPassive) return "Reduces Angel's Skill Rank Cost by " + percent(effectValue);
                        return "Multiplies Skill Proficiency Gain by " + percent(1 + effectValue);
                    case PotionKind.ThiefsBadge:
                        //EQDropChance
                        if (isPassive) return "Reduces Thief's Skill Rank Cost by " + percent(effectValue);
                        return "Multiplies Equipment Drop Chance by " + percent(1 + effectValue);
                    case PotionKind.ArchersBadge:
                        //CritDamage
                        if (isPassive) return "Reduces Archer's Skill Rank Cost by " + percent(effectValue);
                        return "Critical Damage + " + percent(effectValue);
                    case PotionKind.TamersBadge:
                        //EXPGain
                        if (isPassive) return "Reduces Tamer's Skill Rank Cost by " + percent(effectValue);
                        return "EXP Gain + " + percent(effectValue);
                }
                break;
        }
        return tempString;
    }

    //SkillName
    public string SkillName(HeroKind heroKind, int id)
    {
        switch (heroKind)
        {
            case HeroKind.Warrior:
                return SkillNameWarrior((SkillKindWarrior)id);
            case HeroKind.Wizard:
                return SkillNameWizard((SkillKindWizard)id);
            case HeroKind.Angel:
                return SkillNameAngel((SkillKindAngel)id);
            case HeroKind.Thief:
                return SkillNameThief((SkillKindThief)id);
            case HeroKind.Archer:
                return SkillNameArcher((SkillKindArcher)id);
            case HeroKind.Tamer:
                return SkillNameTamer((SkillKindTamer)id);
        }
        return "";
    }
    public string SkillNameWarrior(SkillKindWarrior kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case SkillKindWarrior.SwordAttack:
                        tempString = "ソードアタック";
                        break;
                    case SkillKindWarrior.Slash:
                        tempString = "スラッシュ";
                        break;
                    case SkillKindWarrior.DoubleSlash:
                        tempString = "ダブルスラッシュ";
                        break;
                    case SkillKindWarrior.SonicSlash:
                        tempString = "ソニックスラッシュ";
                        break;
                    case SkillKindWarrior.SwingDown:
                        tempString = "振り下ろし";
                        break;
                    case SkillKindWarrior.SwingAround:
                        tempString = "振り回し";
                        break;
                    case SkillKindWarrior.ChargeSwing:
                        tempString = "チャージスイング";
                        break;
                    case SkillKindWarrior.FanSwing:
                        tempString = "ファンスイング";
                        break;
                    case SkillKindWarrior.ShieldAttack:
                        tempString = "シールドアタック";
                        break;
                    case SkillKindWarrior.KnockingShot:
                        tempString = "ノッキングショット";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case SkillKindWarrior.SwordAttack:
                        tempString = "Sword Attack";
                        break;
                    case SkillKindWarrior.Slash:
                        tempString = "Slash";
                        break;
                    case SkillKindWarrior.DoubleSlash:
                        tempString = "Double Slash";
                        break;
                    case SkillKindWarrior.SonicSlash:
                        tempString = "Sonic Slash";
                        break;
                    case SkillKindWarrior.SwingDown:
                        tempString = "Swing Down";
                        break;
                    case SkillKindWarrior.SwingAround:
                        tempString = "Swing Around";
                        break;
                    case SkillKindWarrior.ChargeSwing:
                        tempString = "Charge Swing";
                        break;
                    case SkillKindWarrior.FanSwing:
                        tempString = "Fan Swing";
                        break;
                    case SkillKindWarrior.ShieldAttack:
                        tempString = "Shield Charge";
                        break;
                    case SkillKindWarrior.KnockingShot:
                        tempString = "Knocking Shot";
                        break;
                }
                break;
        }
        return tempString;
    }
    public string SkillNameWizard(SkillKindWizard kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case SkillKindWizard.StaffAttack:
                        tempString = "スタッフアタック";
                        break;
                    case SkillKindWizard.FireBolt:
                        tempString = "ファイヤーボルト";
                        break;
                    case SkillKindWizard.FireStorm:
                        tempString = "ファイヤーストーム";
                        break;
                    case SkillKindWizard.MeteorStrike:
                        tempString = "メテオストライク";
                        break;
                    case SkillKindWizard.IceBolt:
                        tempString = "アイスボルト";
                        break;
                    case SkillKindWizard.ChillingTouch:
                        tempString = "チリングタッチ";
                        break;
                    case SkillKindWizard.Blizzard:
                        tempString = "ブリザード";
                        break;
                    case SkillKindWizard.ThunderBolt:
                        tempString = "サンダーボルト";
                        break;
                    case SkillKindWizard.DoubleThunderBolt:
                        tempString = "ダブルサンダーボルト";
                        break;
                    case SkillKindWizard.LightningThunder:
                        tempString = "ライトニングサンダー";
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case SkillKindWizard.StaffAttack:
                        tempString = "Staff Attack";
                        break;
                    case SkillKindWizard.FireBolt:
                        tempString = "Fire Bolt";
                        break;
                    case SkillKindWizard.FireStorm:
                        tempString = "Fire Storm";
                        break;
                    case SkillKindWizard.MeteorStrike:
                        tempString = "Meteor Strike";
                        break;
                    case SkillKindWizard.IceBolt:
                        tempString = "Ice Bolt";
                        break;
                    case SkillKindWizard.ChillingTouch:
                        tempString = "Chilling Touch";
                        break;
                    case SkillKindWizard.Blizzard:
                        tempString = "Blizzard";
                        break;
                    case SkillKindWizard.ThunderBolt:
                        tempString = "Thunder Bolt";
                        break;
                    case SkillKindWizard.DoubleThunderBolt:
                        tempString = "Double Thunder Bolt";
                        break;
                    case SkillKindWizard.LightningThunder:
                        tempString = "Lightning Thunder";
                        break;
                }
                break;
        }
        return tempString;
    }
    public string SkillNameAngel(SkillKindAngel kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case SkillKindAngel.WingAttack:
                        return "ウィングアタック";
                    case SkillKindAngel.WingShoot:
                        return "ウィングシュート";
                    case SkillKindAngel.Heal:
                        return "ヒール";
                    case SkillKindAngel.GodBless:
                        return "ゴッドブレス";
                    case SkillKindAngel.MuscleInflation:
                        return "マッスルインフレーション";
                    case SkillKindAngel.MagicImpact:
                        return "マジックインパクト";
                    case SkillKindAngel.ProtectWall:
                        return "プロテクトウォール";
                    case SkillKindAngel.Haste:
                        return "ヘイスト";
                    case SkillKindAngel.WingStorm:
                        return "ウィングストーム";
                    case SkillKindAngel.HolyArch:
                        return "ホーリーアーチ";
                }
                break;
            default:
                switch (kind)
                {
                    case SkillKindAngel.WingAttack:
                        return "Wing Attack";
                    case SkillKindAngel.WingShoot:
                        return "Wing Shoot";
                    case SkillKindAngel.Heal:
                        return "Heal";
                    case SkillKindAngel.GodBless:
                        return "God Bless";
                    case SkillKindAngel.MuscleInflation:
                        return "Muscle Inflation";
                    case SkillKindAngel.MagicImpact:
                        return "Magic Impact";
                    case SkillKindAngel.ProtectWall:
                        return "Protect Wall";
                    case SkillKindAngel.Haste:
                        return "Haste";
                    case SkillKindAngel.WingStorm:
                        return "Wing Storm";
                    case SkillKindAngel.HolyArch:
                        return "Holy Arch";
                }
                break;
        }
        return tempString;
    }
    public string SkillNameThief(SkillKindThief kind)
    {
        string tempString = "";
        switch (kind)
        {
            case SkillKindThief.DaggerAttack:
                return "Dagger Attack";
            case SkillKindThief.Stab:
                return "Stab";
            case SkillKindThief.KnifeToss:
                return "Knife Toss";
            case SkillKindThief.LuckyBlow:
                return "Lucky Blow";
            case SkillKindThief.SpreadToss:
                return "Spread Toss";
            case SkillKindThief.ShadowStrike:
                return "Shadow Strike";
            case SkillKindThief.SneakyStrike:
                return "Sneaky Strike";
            case SkillKindThief.Pilfer:
                return "Pilfer";
            case SkillKindThief.DarkWield:
                return "Dark Wield";
            case SkillKindThief.Assassination:
                return "Assassination";
        }
        return tempString;
    }
    public string SkillNameArcher(SkillKindArcher kind)
    {
        string tempString = "";
        switch (kind)
        {
            case SkillKindArcher.ArrowAttak:
                return "Arrow Attack";
            case SkillKindArcher.PiercingArrow:
                return "Piercing Arrow";
            case SkillKindArcher.BurstArrow:
                return "Burst Arrow";
            case SkillKindArcher.Multishot:
                return "Multishot";
            case SkillKindArcher.ShockArrow:
                return "Shock Arrow";
            case SkillKindArcher.FrozenArrow:
                return "Frozen Arrow";
            case SkillKindArcher.ExplodingArrow:
                return "Exploding Arrow";
            case SkillKindArcher.ShiningArrow:
                return "Shining Arrow";
            case SkillKindArcher.GravityArrow:
                return "Gravity Arrow";
            case SkillKindArcher.Kiting:
                return "Kiting";
        }
        return tempString;
    }
    public string SkillNameTamer(SkillKindTamer kind)
    {
        switch (kind)
        {
            case SkillKindTamer.SonnetAttack:
                return "Sonnet Attack";
            case SkillKindTamer.AttackingOrder:
                return "Attacking Order";
            case SkillKindTamer.RushOrder:
                return "Rush Order";
            case SkillKindTamer.DefensiveOrder:
                return "Defensive Order";
            case SkillKindTamer.SoothingBallad:
                return "Soothing Ballad";
            case SkillKindTamer.OdeOfFriendship:
                return "Ode of Friendship";
            case SkillKindTamer.AnthemOfEnthusiasm:
                return "Anthem of Enthusiasm";
            case SkillKindTamer.FeedChilli:
                return "Feed Chilli";
            case SkillKindTamer.BreedingKnowledge:
                return "Breeding Knowledge";
            case SkillKindTamer.TuneOfTotalTaming:
                return "Tune of Total Taming";
        }
        return "";
    }

    public string SkillEffect(EffectKind effect)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (effect)
                {
                    case EffectKind.PhysicalDamage:
                        tempString = "物理ダメージ";
                        break;
                    case EffectKind.FireDamage:
                        tempString = "炎ダメージ";
                        break;
                    case EffectKind.IceDamage:
                        tempString = "氷ダメージ";
                        break;
                    case EffectKind.ThunderDamage:
                        tempString = "雷ダメージ";
                        break;
                    case EffectKind.LightDamage:
                        tempString = "光ダメージ";
                        break;
                    case EffectKind.DarkDamage:
                        tempString = "闇ダメージ";
                        break;
                    case EffectKind.Heal:
                        tempString = "ヒール";
                        break;
                    case EffectKind.DebuffKind:
                        tempString = "デバフ";
                        break;
                    case EffectKind.DebuffChance:
                        tempString = "デバフ成功確率";
                        break;
                    case EffectKind.MPGain:
                        tempString = "獲得MP";
                        break;
                    case EffectKind.MPConsumption:
                        tempString = "消費MP";
                        break;
                    case EffectKind.Cooltime:
                        tempString = "クールタイム";
                        break;
                    case EffectKind.Range:
                        tempString = "射程距離";
                        break;
                    case EffectKind.EffectRange:
                        tempString = "有効範囲";
                        break;
                }
                break;
            default:
                switch (effect)
                {
                    case EffectKind.PhysicalDamage:
                        tempString = "Physical Damage";
                        break;
                    case EffectKind.FireDamage:
                        tempString = "Fire Damage";
                        break;
                    case EffectKind.IceDamage:
                        tempString = "Ice Damage";
                        break;
                    case EffectKind.ThunderDamage:
                        tempString = "Thunder Damage";
                        break;
                    case EffectKind.LightDamage:
                        tempString = "Light Damage";
                        break;
                    case EffectKind.DarkDamage:
                        tempString = "Dark Damage";
                        break;
                    case EffectKind.Heal:
                        tempString = "Heal";
                        break;
                    case EffectKind.DebuffKind:
                        tempString = "Debuff";
                        break;
                    case EffectKind.DebuffChance:
                        tempString = "Debuff Chance";
                        break;
                    case EffectKind.MPGain:
                        tempString = "MP Gain";
                        break;
                    case EffectKind.MPConsumption:
                        tempString = "MP Consumption";
                        break;
                    case EffectKind.Cooltime:
                        tempString = "Cooldown";
                        break;
                    case EffectKind.Range:
                        tempString = "Range";
                        break;
                    case EffectKind.EffectRange:
                        tempString = "Effect Range";
                        break;
                }
                break;
        }
        return tempString;
    }
    public string BuffName(Buff kind)
    {
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case Buff.HpUp:
                        return "Max HP";
                    case Buff.AtkUp:
                        return "ATK";
                    case Buff.MatkUp:
                        return "MATK";
                    case Buff.DefMDefUp:
                        return "DEF & MDEF";
                    case Buff.SpdUp:
                        return "SPD";
                    case Buff.MoveSpeedUp:
                        return "Move Speed";
                    case Buff.GoldUp:
                        return "Gold Gain";
                    case Buff.SkillLevelUp:
                        return "Skill Level";
                }
                break;
        }
        return kind.ToString();
    }
    public string DebuffName(Debuff kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    case Debuff.AtkDown: return "ATK Down";
                    case Debuff.MatkDown: return "MATK Down";
                    case Debuff.DefDown: return "DEF Down";
                    case Debuff.MdefDown: return "MDEF Down";
                    case Debuff.SpdDown: return "SPD Down";
                    case Debuff.Stop: return "Freeze";
                    case Debuff.Electric: return "Electric";
                    case Debuff.Poison: return "Poison";
                    case Debuff.Death: return "Death";
                    case Debuff.Knockback: return "Knockback";
                    case Debuff.FireResDown:
                        return "Fire Resistance Down";
                    case Debuff.IceResDown:
                        return "Ice Resistance Down";
                    case Debuff.ThunderResDown:
                        return "Thunder Resistance Down";
                    case Debuff.LightResDown:
                        return "Light Resistance Down";
                    case Debuff.DarkResDown:
                        return "Dark Resistance Down";
                    case Debuff.Gravity:
                        return "Gravity";
                }
                break;
        }
        return tempString;
    }
    //Stance
    public (string name, string effect) Stance(Stance stance)
    {
        string tempName = "Normal";
        string tempEffect = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (stance.heroKind)
                {
                    case HeroKind.Warrior:
                        switch ((WarriorStanceKind)stance.id)
                        {
                            case WarriorStanceKind.Attack:
                                tempName = "Attack";
                                tempEffect = "Sword Attack's Damage + " + percent(stance.effectValueBuff);
                                break;
                            case WarriorStanceKind.Reach:
                                tempName = "Reach";
                                tempEffect = "Sword Attack's Range : " + meter(stance.effectValueBuff);
                                break;
                            case WarriorStanceKind.Knock:
                                tempName = "Knock";
                                tempEffect = "Add Knockback effect to Sword Attack";
                                break;
                        }
                        break;
                    case HeroKind.Wizard:
                        switch ((WizardStanceKind)stance.id)
                        {
                            case WizardStanceKind.Fire:
                                tempName = "Fire";
                                tempEffect = "Change Staff Attack's attack type to Fire";
                                break;
                            case WizardStanceKind.Ice:
                                tempName = "Ice";
                                tempEffect = "Change Staff Attack's attack type to Ice";
                                break;
                            case WizardStanceKind.Thunder:
                                tempName = "Thunder";
                                tempEffect = "Change Staff Attack's attack type to Thunder";
                                break;
                        }
                        break;
                    case HeroKind.Angel:
                        break;
                    case HeroKind.Thief:
                        break;
                    case HeroKind.Archer:
                        break;
                    case HeroKind.Tamer:
                        break;
                }
                break;
        }
        return (tempName, tempEffect);
    }

    public string GuildAbilityName(GuildAbilityKind kind)
    {
        string tempString = kind.ToString(); //未
        switch (language)
        {
            case Language.Japanese:
                break;
            default://English
                switch (kind)
                {
                    //case GuildAbilityKind.Member:
                    //    tempString = "Advertising";
                    //    break;
                    case GuildAbilityKind.GlobalSkillSlot:
                        tempString = "Imitating";
                        break;
                    case GuildAbilityKind.EquipmentInventory:
                        tempString = "Collecting";
                        break;
                    case GuildAbilityKind.EnchantInventory:
                        tempString = "Enchanting";
                        break;
                    case GuildAbilityKind.PotionInventory:
                        tempString = "Alchemising";
                        break;
                    case GuildAbilityKind.MysteriousWater:
                        tempString = "Purifying";
                        break;
                    case GuildAbilityKind.StoneGain:
                        tempString = "Mining";
                        break;
                    case GuildAbilityKind.CrystalGain:
                        tempString = "Synthesizing";
                        break;
                    case GuildAbilityKind.LeafGain:
                        tempString = "Gathering";
                        break;
                    //case GuildAbilityKind.AbilityPoint:
                    //    tempString = "Training";
                    //    break;
                    //case GuildAbilityKind.HP:
                    //    tempString = "Body Building";
                    //    break;
                    //case GuildAbilityKind.MP:
                    //    tempString = "Mental Training";
                    //    break;
                    //case GuildAbilityKind.ATK:
                    //    tempString = "Strengthening";
                    //    break;
                    //case GuildAbilityKind.MATK:
                    //    tempString = "Casting";
                    //    break;
                    //case GuildAbilityKind.DEFMDEF:
                    //    tempString = "Protecting";
                    //    break;
                    //case GuildAbilityKind.SPD:
                    //    tempString = "Prompting";
                    //    break;
                    //case GuildAbilityKind.ElementResistance:
                    //    tempString = "Integrating";
                    //    break;
                    //case GuildAbilityKind.MoveSpeed:
                    //    tempString = "Running";
                    //    break;
                    //case GuildAbilityKind.EquipDropChance:
                    //    tempString = "Detecting";
                    //    break;
                    case GuildAbilityKind.GuildExpGain:
                        tempString = "Training";
                        break;
                    case GuildAbilityKind.SkillProficiency:
                        return "Studying";
                    case GuildAbilityKind.Trapping:
                        return "Trapping";
                    case GuildAbilityKind.EquipmentProficiency:
                        return "Smithing";
                    case GuildAbilityKind.PhysicalAbsorption:
                        return "Shielding";
                    case GuildAbilityKind.MagicalAbsoption:
                        return "Dispersing";
                    //case GuildAbilityKind.BonusAbilityPointRebirth:
                    //    return "Understanding";
                    case GuildAbilityKind.UpgradeCost:
                        return "Financing";//"Optimizing";
                    case GuildAbilityKind.MaterialDrop:
                        return "Finding";
                    case GuildAbilityKind.NitroCap:
                        return "Racing";
                    case GuildAbilityKind.GoldCap:
                        return "Banking";
                    case GuildAbilityKind.GoldGain:
                        return "Monetizing";
                    case GuildAbilityKind.ExpGain:
                        return "Learning";
                        //case GuildAbilityKind.TalentWarrior:
                        //    break;
                        //case GuildAbilityKind.TalentWizard:
                        //    break;
                        //case GuildAbilityKind.TalentAngel:
                        //    break;
                        //case GuildAbilityKind.TalentThief:
                        //    break;
                        //case GuildAbilityKind.TalentArcher:
                        //    break;
                        //case GuildAbilityKind.TalentTamer:
                        //    break;
                        //case GuildAbilityKind.EquipProficiency:
                        //    break;
                        //case GuildAbilityKind.UpgradeLevel:
                        //    break;
                        //case GuildAbilityKind.ActiveNum:
                        //    tempString = "Cooperating";
                        //    break;
                }
                break;
        }
        return tempString;
    }
    public string GuildAbilityEffect(GuildAbilityKind kind, double effectValue)
    {
        string tempString = kind.ToString() + " + " + tDigit(effectValue);
        switch (language)
        {
            case Language.Japanese://未
                break;
            default:
                switch (kind)
                {
                    //case GuildAbilityKind.Member:
                    //    tempString = "Guild Member + " + tDigit(effectValue);
                    //    break;
                    //case GuildAbilityKind.ActiveNum:
                    //    tempString = "Available activating heroes + " + tDigit(effectValue);
                    //    break;
                    case GuildAbilityKind.StoneGain:
                        tempString = "Stone Gain + " + percent(effectValue);
                        break;
                    case GuildAbilityKind.CrystalGain:
                        tempString = "Crystal Gain + " + percent(effectValue);
                        break;
                    case GuildAbilityKind.LeafGain:
                        tempString = "Leaf Gain + " + percent(effectValue);
                        break;
                    case GuildAbilityKind.EquipmentInventory:
                        tempString = "Equipment Inventory + " + tDigit(effectValue);
                        break;
                    case GuildAbilityKind.EnchantInventory:
                        tempString = "Enchant Inventory + " + tDigit(effectValue);
                        break;
                    case GuildAbilityKind.PotionInventory:
                        tempString = "Utility Inventory + " + tDigit(effectValue);
                        break;
                    case GuildAbilityKind.GlobalSkillSlot:
                        tempString = "Global Skill Slot + " + tDigit(effectValue);
                        break;
                    case GuildAbilityKind.MysteriousWater:
                        tempString = "Mysterious Water Gain + " + percent(effectValue);
                        break;
                    //case GuildAbilityKind.HP:
                    //    tempString = "HP + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.MP:
                    //    tempString = "MP + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.ATK:
                    //    tempString = "ATK + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.MATK:
                    //    tempString = "MATK + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.DEFMDEF:
                    //    tempString = "DEF and MDEF + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.SPD:
                    //    tempString = "SPD + " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.ElementResistance:
                    //    tempString = "Multiply Element Resistance by " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.MoveSpeed:
                    //    tempString = "Multiply Move Speed by " + percent(effectValue);
                    //    break;
                    //case GuildAbilityKind.EquipDropChance:
                    //    tempString = "Multiply Equipment Drop Chance by " + percent(effectValue);
                    //    break;
                    case GuildAbilityKind.GuildExpGain:
                        tempString = "Multiply Guild EXP Gain by " + percent(1 + effectValue);
                        break;
                    case GuildAbilityKind.SkillProficiency:
                        return "Multiply Skill Proficiency Gain by " + percent(1 + effectValue);

                    case GuildAbilityKind.Trapping:
                        return "Trap Not Consumed on Use Chance + " + percent(effectValue);
                    case GuildAbilityKind.EquipmentProficiency:
                        return "Multiply Equipment Proficiency Gain by " + percent(1 + effectValue);
                    case GuildAbilityKind.PhysicalAbsorption:
                        return "Physical Absorption + " + percent(effectValue);
                    case GuildAbilityKind.MagicalAbsoption:
                        return "Magical Absorption + " + percent(effectValue);
                    //case GuildAbilityKind.BonusAbilityPointRebirth:
                    //    return "Bonus Ability Point after Rebirth + " + tDigit(effectValue);
                    case GuildAbilityKind.UpgradeCost:
                        return "Decrease Upgrade Cost by " + percent(effectValue);
                    case GuildAbilityKind.MaterialDrop:
                        return "Dropped Material Gain + " + tDigit(effectValue);
                    case GuildAbilityKind.NitroCap:
                        return "Nitro Cap + " + tDigit(effectValue);
                    case GuildAbilityKind.GoldCap:
                        return "Multiply Gold Cap by " + percent(1 + effectValue);
                    case GuildAbilityKind.GoldGain:
                        return "Multiply Gold Gain by " + percent(1 + effectValue);
                    case GuildAbilityKind.ExpGain:
                        return "Multiply EXP Gain by " + percent(1 + effectValue);
                }
                break;
        }
        return tempString;
    }

    //Quest 未
    public (string name, string client, string description, string condition, string reward, string unlock) Quest(QUEST quest)
    {
        QuestKind kind = quest.kind;
        QuestKindGlobal kindGlobal = quest.kindGlobal;
        QuestKindDaily kindDaily = quest.kindDaily;
        QuestKindTitle kindTitle = quest.kindTitle;
        QuestKindGeneral kindGeneral = quest.kindGeneral;
        HeroKind heroKind = quest.heroKind;
        string name = "Placeholder";
        string client = "";
        string description = "";
        string condition = "";//特殊な場合のみ追加
        string reward = "";
        string unlock = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case QuestKind.Global:
                        switch (kindGlobal)
                        {
                            case QuestKindGlobal.AbilityVIT:
                                name = "Tutorial 1 : Basic Training";
                                client = "Hitan";
                                description = "Welcome to Incremental Epic Hero 2! First, to get tougher, assign 1 <color=orange>Ability Point (AP)</color> to <color=orange>VIT</color>. VIT is the most basic ability, which boosts your HP, DEF and MDEF. You gain 1 AP every time you level up. \n<color=yellow>- For more information about ability points, check out the Help in top left > [Ability].</color>";
                                condition = "Assign 1 AP to VIT";
                                break;
                            case QuestKindGlobal.ClearGeneralQuest:
                                name = "Tutorial 2 : General Quest";
                                client = "Hitan";
                                description = "There are four types of quests: <color=orange>Global</color>, <color=orange>Daily</color>, <color=orange>Title</color> and <color=orange>General</color>. The Global Quest is the main story of this game and progresses with all guild members. Title and General Quest are unique to each class. Try completing the first <color=orange>General Quest</color>!\n<color=yellow>- For more information about quests, check out the Help in top left > [Quest].</color>";
                                condition = "Complete the first General Quest";
                                reward = "Unleash Tab [ Skill ]";
                                break;
                            case QuestKindGlobal.ClearTitleQuest:
                                name = "Tutorial 3 : Title Quest";
                                client = "Hitan";
                                description = "Great job! Next, <color=orange>Title Quests</color> are special quests which, when completed, award Titles. These titles have unique effects. Try completing the first Title Quest! Keep in mind that you can only accept at most 2 quests between Title and General at once. You can increase the limit later on in the game. Global and Daily Quests are not affected by this limit.\n<color=yellow>- For more information about quests, check out the Help in top left > [Quest].</color>";
                                condition = "Complete the first Title Quest";
                                reward = "Unleash Tab [ Upgrade ]";
                                break;
                            case QuestKindGlobal.UpgradeResource:
                                name = "Tutorial 4 : Upgrade Resource";
                                client = "Hitan";
                                description = "In the <color=orange>Upgrade</color> tab, you can buy various upgrades with gold to improve your stats! Try buying <color=orange>Resource Gain</color> upgrades!\n<color=yellow>- For more information about upgrades, check out the Help in top left > [Upgrade].</color>";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 25";
                                reward = "Unleash Upgrade [ Resource Gain 2 ]\n- Unleash Menu [ Equip ]";
                                break;
                            case QuestKindGlobal.Equip:
                                name = "Tutorial 5 : Equipment";
                                client = "Hitan";
                                description = "While you are defeating monsters, you have a very low chance for them to drop <color=orange>Equipment</color> after they are defeated. When you get one, please bring it to me! You can equip it by dragging and dropping it to an open equipment in the <color=orange>Equip</color> tab." +
                                    " After you get one, check out the <color=orange>Dictionary</color> in the Equip tab. This will show you all of the equipment you've looted so far!  In here you'll see that there are upgrades. These will help increase the heroes <color=orange>Equipment Proficiency Gain</color>, which lowers the required time to level up equipment.\n<color=yellow>- For more information about Equips and the Dictionary, check out the Help > [Equip].</color>";
                                condition = "Get any Equipment";
                                reward = "Unleash Tab [ Lab ]";
                                break;
                            case QuestKindGlobal.Alchemy:
                                name = "Tutorial 6 : Alchemy";
                                client = "Hitan";
                                description = optStr + "While you were out a guild member recently found some <color=orange>Mysterious Water</color> and set up a <color=orange>Lab</color>. You'll meet her soon enough, but for now, let's begin experimenting. Oh, you're not familiar with alchemy? Let me explain."
                                    + " To alchemize you first need to go to the lab and make a Catalyst. To make your first catalyst all you need to do is get some <color=orange>Oil of Slime</color>, collect <color=orange>3 Mysterious Water</color> and use that to make a <color=orange>Slime Catalyst</color>. After that, you need to make an essence."
                                    + " Essences are the raw ingredients used to make potions. To start, you need to pour at least 0.1 Mysterious Water into an essence of a catalyst you've selected to make the catalyst start producing essence. Let's try it with <color=orange>Essence of Slime</color>."
                                    + " Once that's finished all you have to do is look at <color=orange>[Mix Potion]</color>, and click the icon you want to make. Simple isn't it?"
                                    + " Now go play in the lab and don't forget to keep increasing your Mysterious Water Cap, refining your Catalyst, and upgrading your lab with the alchemy points you receive.\n<color=yellow>- For more information, Help > [Lab].</color>";
                                //"While you are fighting monsters, they will sometimes drop other kinds of items. Items that can be used for alchemising potions. To alchemise, go to the [Alchemy] Tab." +
                                //" Combine the required items to alchemise potions. You need Mysterious Water other than the dropped items, that is automatically produced over time." +
                                //"Now you can go to Lab tab. Expand the Mysterious Water's cap and gain three." +
                                //    " Unlock the first Slime Catalyst with three Mysterious Water and click the icon to select the catalyst." +
                                //    " You can allocate Mysterious Water in 0.1 to any Catalyst and convert it to any Essence. Essence is used for alchemising potions." + 
                                //    " You can use alchemised potions at [ Equip ] tab by equipping them on the potion slot." +
                                //    " When you alchemise potions, you gain Alchemy Point that is used for potion's upgrade and Alchemy Upgrade." +
                                //    " Anyway let's try alchemising " + localized.PotionName(PotionKind.MinorHealthPotion) + " first!";
                                condition = "Produce " + localized.PotionName(PotionKind.MinorHealthPotion);
                                reward = "Unleash Tab [ Guild ] [ Bestiary ]";
                                break;
                            case QuestKindGlobal.Guild:
                                name = "Tutorial 7 : Guild";
                                client = "Hitan";
                                description = "To increase guild member limit, get <color=orange>Guild Lv 5</color>! Then you can select a new hero, Wizard, to play. You can also acquire various abilities in the guild tab. Guild EXP is gained through Hero leveling, which will provide guild ability points to spend." +
                                    "\n- To start with your Heroes will work independently, but as you progress in the game they will learn to cooperate more. The <color=orange>Imitating</color> guild ability will give you a <color=orange>Global Skill Slot</color>, which allows a Hero to modify their play slightly by equipping a skill learned by one of the other Heroes. You may need to adjust your Hero's default <color=orange>Combat Range</color> so they move close enough to use the borrowed skill." +
                                    "\n<color=yellow>- For more information, Help > [Guild].</color>";
                                condition = "Guild Lv 5";
                                reward = "Unleash Tab [ Town ] [ Shop ]";
                                break;
                            case QuestKindGlobal.Town:
                                name = "Tutorial 8 : Town";
                                client = "Hitan";
                                description = optStr + "Now you can go to <color=orange>Town</color> tab." +
                                    " There are various buildings in the town." +
                                    " You can improve buildings with Town Materials that you gain every time you clear any area ( you can see it in the area info )." +
                                    " Dungeons give more Town Materials when you clear it." +
                                    " Like skills, buildings have Rank and Level. The level cap increases by 20 per rank." +
                                    " To understand more, let's raise <color=orange>Cartographer</color>'s level to 5." +
                                    " Then you will be able to go to another region in the world!\n<color=yellow>- For more information, Help > [Town].</color>"
                                    ;
                                condition = "Building [ Cartographer ] Lv 5";
                                reward = "EXP Gain Blessing (Duration 30 mins)";
                                //reward = "Unleash Tab [ Shop ]";
                                break;
                            case QuestKindGlobal.Research:
                                name = "Tutorial 9 : Town Research";
                                client = "Hitan";
                                description = optStr + "While we are in the town, each building has three different researchable effects. You can speed up the research speed by gaining more resources. Let's research Cartographer's Leaf Research. This will provide an additional Town Material per clear, which can be extremely useful in gathering town materials to level up buildings in the town!\n<color=yellow>- For more information, Help > [Town].</color>"
                                    ;
                                condition = "Cartographer's Leaf Research Lv 1";
                                reward = "Gold Gain Blessing (Duration 30 mins)\n-Unleash Tab [ Rebirth ]";
                                break;
                            case QuestKindGlobal.Rebirth:
                                name = "Tutorial 10 : Rebirth";
                                client = "Hitan";
                                description = optStr + "It's now time for a <color=orange>Rebirth</color>. There are multiple tiers of rebirthing. But for now, let's get started on Tier 1. <color=orange>Get to Hero Level 100</color> and click on the Rebirth Tab, then click on 'Rebirth'." +
                                    " Don't forget to apply some upgrades after rebirthing, especially the <color=orange>EXP Multiplier</color>. \n<color=yellow>- For more information, Help > [Rebirth].</color>"; ;
                                condition = "Perform Tier 1 Rebirth of any hero";
                                reward = "EXP Gain Blessing (Duration 30 mins)\n- Unleash Tab [ Challenge ]";
                                break;
                            case QuestKindGlobal.Challenge:
                                name = "Tutorial 11 : Challenge";
                                client = "Hitan";
                                description = optStr + "Oh, looks like a horrible monster has emerged! Go to <color=orange>Challenge</color> tab to defeat <color=orange>Florzporb</color>." +
                                    " On Raid Boss Battle, all heroes that are currently active will join the battle." +
                                    " Since you have rebirthed, you can now get Proof of Rebirth title by the quest, which enables to activate the hero even in background." +
                                    " The boss monster is very powerful, so gather active guild members to fight together and prepare well before taking it on!\n<color=yellow>- For more information, Help > [Challenge].</color>";
                                condition = "Complete Raid Boss Battle [ Florzporb Lv 100 ]";
                                reward = "Unleash Tab [ Expedition ]\n- Unlock Auto Ability Point Adder (AAPA)";
                                break;
                            case QuestKindGlobal.Expedition:
                                name = "Tutorial 12 : Expedition";
                                client = "Hitan";
                                description = optStr + "Placeholder\n<color=yellow>- For more information, Help > [Expedition].</color>";
                                condition = "Complete any expedition once";
                                reward = "Unleash Tab [ World Ascension ]\n- Unlock 1 Expedition Team";
                                break;
                            case QuestKindGlobal.WorldAscension:
                                name = "Tutorial 13 : World Ascension";
                                client = "Hitan";
                                description = optStr + "Okay, the next tutorial is... huh? You are thinking that this is a too long tutorial? Come on, this is only the beginning of this game so far. Very exciting, isn't it?" +
                                    " Now, though, it will be a while before you achieve the next goal, <color=orange>World Ascension</color>. Let's try to complete the milestones in World Ascension Tab and perform the World Ascension. Various things in this world will reset and hence further dimensional ascension will occur in this game." +
                                    " It is up to you when you want to World Ascension!\n<color=yellow>- For more information, Help > [World Ascension].</color>";
                                condition = "Perform World Ascension Tier 1 once";
                                reward = "";
                                break;
                            case QuestKindGlobal.AreaPrestige:
                                name = "Tutorial 14 : Area Prestige";
                                client = "Hitan";
                                description = optStr + "Congraturations! After World Ascension, every area of the world also has a chance to prestige. You can earn <color=orange>Area Prestige Point</color> according to its area clear #. See the next clear # to earn points in area info." +
                                    " Once you increase <color=orange>[ Area Prestige ]</color> upgrade's level, you can change the difficulty of its area. The unique equipment drop chance and the reward amount increases according to the difficulty, while the monster's level and wave # to clear increases too." +
                                    " You can also try the same missions in the area of different difficulty, so you can earn another Epic Coin and Mission Milestone Count as well.\n<color=yellow>- For more information, Help > [World Map].</color>";
                                condition = "Area Prestige Upgrade of " + AreaName(AreaKind.SlimeVillage) + " Area 1 [ Area Prestige ] Lv 1";
                                reward = "";
                                break;

                            //Upgrade
                            case QuestKindGlobal.Upgrade1:
                                name = "To Further Stage 1";
                                client = "Yuni";
                                description = "You know Resources such as blue stones are useful for ranking up skills and expanding the gold cap. You can improve resource gain in the Upgrade tab. To get resources more efficiently, get Resource Gain 1 to Lv 50!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 50";
                                reward = "Unleash Upgrade [ Resource Gain 3 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade2:
                                name = "To Further Stage 2";
                                client = "Yuni";
                                description = "Good job! To further stage, get Resource Gain 1 to Lv 100!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 100";
                                reward = "Unleash Upgrade [ Resource Gain 4 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade3:
                                name = "To Further Stage 3";
                                client = "Yuni";
                                description = "Great job! To further stage, get Resource Gain 1 to Lv 150!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 150";
                                reward = "Unleash Upgrade [ Resource Gain 5 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade4:
                                name = "To Further Stage 4";
                                client = "Yuni";
                                description = "Excellent job! To further stage, get Resource Gain 1 to Lv 200!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 200";
                                reward = "Unleash Upgrade [ Resource Gain 6 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade5:
                                name = "To Further Stage 5";
                                client = "Yuni";
                                description = "Amazing job! To further stage, get Resource Gain 1 to Lv 250!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 250";
                                reward = "Unleash Upgrade [ Resource Gain 7 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade6:
                                name = "To Further Stage 6";
                                description = "Awesome job! To further stage, get Resource Gain 1 to Lv 300!";
                                client = "Yuni";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 300";
                                reward = "Unleash Upgrade [ Resource Gain 8 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade7:
                                name = "To Further Stage 7";
                                client = "Yuni";
                                description = "Wonderful job! To further stage, get Resource Gain 1 to Lv 400!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 400";
                                reward = "Unleash Upgrade [ Resource Gain 9 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Upgrade8:
                                name = "To Further Stage 8";
                                client = "Yuni";
                                description = "Crazy job! To further stage, get Resource Gain 1 to Lv 500!";
                                condition = "Upgrade [ Resource Gain 1 ] Lv 500";
                                reward = "Unleash Upgrade [ Resource Gain 10 ]" +
                                    "\n- Upgrade Queue + 5";
                                break;
                            case QuestKindGlobal.Nitro1:
                                name = "How to be a Nitro Booster Geek 1";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "What's this? Why are you interrupting me? Can't you see that I'm busy here? Oh, you're curious about Nitro, are you? Have you ever even tried it before? Go ahead and use some Nitro (The TNT bomb icon at the top of your screen) before bothering me again.";
                                //client = "Shady Merchant";
                                //description = "A man in a trench coat slyly approaches you. He grips the edges of his trench coat before pulling them open to reveal strange red vials lining the leathery backdrop. \"Can I interest you in a boost? This will surely put some speed in your step, for a price of course.\" Try using Nitro Booster by clicking the TNT bomb icon.";
                                condition = "Turn on Nitro Booster";
                                break;
                            case QuestKindGlobal.Nitro2:
                                name = "How to be a Nitro Booster Geek 2";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "Who is it?! Oh, it's you again... You tried out the Nitro, did you? Pretty great stuff. I use it when I need to focus and finish building my contraptions and doodads, but I suppose it works for adventurers like you as well. Tell you what, come see me again after you've burned through around 5000 of the stuff and I'll show you how you can carry more of it.";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / 5000";
                                reward = "Nitro Cap + 1000";
                                break;
                            case QuestKindGlobal.Nitro3:
                                name = "How to be a Nitro Booster Geek 3";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "You see a blur whizzing about the workshop, before it finally settles into the image of a curmudgeonly, old gnomish man. \"Who are you and why are you in my workshop?!\" He adjusts the seventeen lenses of his strange spectacle headpiece before examining you again. \"Oh, hello there. So you've used a bit more Nitro than before have you? As you have just seen, I'm quite fond of it myself. Oh you want to know how to carry more Nitro? Alright, I'll give you a expansion for it after you've burned through 30,000 again, okay? Now if you'll please leave, I have work to do.\"";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(30000);
                                reward = "Nitro Cap + 2000";
                                break;
                            case QuestKindGlobal.Nitro4:
                                name = "How to be a Nitro Booster Geek 4";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "A large explosion can be heard coming from a rickety looking workshop. Peering inside, you are surprised to see that the facility doesn't appear to have any indications that an explosion just occurred there. As you enter, you are greeted by a panting, eccentric, old gnome. \"Whew, experiment Z17S34.1 was a failure. Excellent, excellent. Alright...\" He lifts his head up from a journal, locking eyes with you in a sudden jolt of surprise. \"Oh sulphurous stalactites!! Don't sneak up on an elderly fellow like that. Whew... my heart... look if you're here for another Nitro expansion, just go burn through another " + tDigit(150000) + " and I'll give you another. If you have nothing else, please leave.\" He then ignores you as he carries on writing in his journal.";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(150000);
                                reward = "Nitro Cap + 3000";
                                break;
                            case QuestKindGlobal.Nitro5:
                                name = "How to be a Nitro Booster Geek 5";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "As you approach the gnomish workshop, a blur whizzes by you, trailing off into building. Then it whizzes again back out, stopping abruptly just two feet in front of you. \"You've returned I see. Sorry, but no one is allowed in my workshop at the moment. I'm on the brink of incredible discovery! If you want another Nitro expansion, I'm too busy to help you, so beat it.\" Your head droops a bit at hearing his words and you begin to walk away. \"Oh fine, fine. Don't be that way. Go burn through " + tDigit(500000) + " of Nitro and I'll give you another expansion. I can't keep doing this forever, though, so don't expect me to make many more exceptions for you.\" There is a brief gush of wind as the figure of the tiny gnome whooshes away, leaving a trail leading back into the workshop, ending with a loud slamming of the workshop door.";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(500000);
                                reward = "Nitro Cap + 4000";
                                break;
                            case QuestKindGlobal.Nitro6:
                                name = "How to be a Nitro Booster Geek 6";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "You return to the gnomish workshop only to find a giant crater in the ground where the building once stood. At the center of the crater sits a blackened, ash-covered gnome looking a bit despondent. As you approach he looks up at you a little bit embarrassed. \"I succeeded. I broke through the Nitro barrier.\" He wipes a bit of a tear from his eye, smearing the ash across his already filthy face. \"So that's it, my life's work complete. No idea what I'm going to do now. I'm sure you're here for another Nitro expansion? Fine, but this is the last time. I'll be returning to my homeland soon to report my findings and hopefully find inspiration to start a new life's work. Well, why are you still standing there? Go burn through " + tDigit(1000000) + " of Nitro and let me know when you're done. I've got some things to take care of before I go, so I'll wait till you return.\"";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(1000000);
                                reward = "Nitro Cap + 5000";
                                break;
                            case QuestKindGlobal.Nitro7:
                                name = "How to be a Nitro Booster Geek 7";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "Who is it?! Oh, it's you again... You tried out the Nitro, did you? Pretty great stuff. I use it when I need to focus and finish building my contraptions and doodads, but I suppose it works for adventurer's like you as well. Tell you what, come see me again after you've burned through around 5000 of the stuff and I'll show you how you can carry more of it.";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(5000000);
                                reward = "Nitro Cap + 6000";
                                break;
                            case QuestKindGlobal.Nitro8:
                                name = "How to be a Nitro Booster Geek 8";
                                client = "Gnomish Engineer, Asgabit Tinkerbait";
                                description = "Who is it?! Oh, it's you again... You tried out the Nitro, did you? Pretty great stuff. I use it when I need to focus and finish building my contraptions and doodads, but I suppose it works for adventurer's like you as well. Tell you what, come see me again after you've burned through around 5000 of the stuff and I'll show you how you can carry more of it.";
                                condition = "Total Nitro consumed : " + tDigit(main.S.nitroConsumed) + " / " + tDigit(10000000);
                                reward = "Nitro Cap + 7000";
                                break;

                            case QuestKindGlobal.Capture1:
                                name = "Capturing Monsters 1";
                                //client = "Dirgah Suebur, the Monster Handler";
                                //description = "\"Oy d'ere! Ye must be looking for training on how to handle monstery types, yea? Well, look no fur'ter as I'd be happy to teach ye. First ting's first, ye need to get yerself a trap. Head yerself over to the Shop, buy yerself some traps and capture some monstery types, then come back and sees me.I'll be waitin' right here.\"";
                                //description += "\n- In the Shop, there is a tab where you can find all of the traps that you have currently unlocked. The first trap that is unlocked is the Throwing Net, which is ONLY capable of capturing \"Normal\" monsters. Also keep in mind that you must at least pass the Title Quest [ Monster Study 1 ] before you will be able to properly utilize Traps. Traps can be equipped in the Utility slot and are activated automatically upon defeating a monster whose type and level are eligible to be captured based on the type of trap equipped.";
                                //description += "\n- You can hover over a trap to see the max capturable monster level. To increase this further, you can increase your Hero level, finish Monster Study title quests, and increase the level of a particular town building.";
                                //description += "\n- Captured Monsters, or Pets, provide a range of special bonuses that not only enable you to automate various aspects of the game, but also provide passive boosts to a variety of stats.";
                                client = "Ringold";
                                description = "In the <color=orange>Shop</color>, there is a tab where you can find all of the traps that you have currently unlocked. The first trap that is unlocked is the <color=orange>Throwing Net</color>, which is ONLY capable of capturing <color=orange>'Normal' Type</color> monsters. Also keep in mind that you must at least pass the <color=orange>Title Quest [ Monster Study 1 ]</color> before you will be able to properly utilize Traps. Traps can be equipped in the <color=orange>Utility Slot</color>, you can then <color=orange>Right Click</color> on a monster. You must have the right trap type equipped and have a high enough capturable monster level for the monster you are trying to trap.";//Traps can be equipped in the <color=orange>Utility</color> slot and are activated automatically upon defeating a monster whose type and level are eligible to be captured based on the type of trap equipped.";
                                description += "\n- You can hover over a trap to see the max capturable monster level. To increase this further, you can increase your Hero level, finish Monster Study title quests, and increase the level of a particular town building.";
                                description += "\n- Captured Monsters, or Pets, provide a range of special bonuses that not only enable you to automate various aspects of the game, but also provide passive boosts to a variety of stats.";
                                description += "\n<color=yellow>For more information, Help > [Capture].</color>";
                                //description = "The <color=orange>Shop</color> has <color=orange>Traps</color> that you can purchase. The first trap you have unlocked is the <color=orange>Throwing Net</color>. Which can only be used to capture the <color=orange>'Normal' Type</color> of monsters. " +
                                //    "Capturing Monsters are useful, as they provide both active and passive effects/bonuses. There are active effects that <color=orange>auto pick up materials and resources</color>. Passive effects are bonuses, such as Resource Gain or Town Material Gain. As you capture Monsters, you'll increase their rank, boosting their passive effects. " +
                                //    "Start by <color=orange>making sure that Title Quest [ Monster Study 1 ] is completed</color>. Now, let's buy some <color=orange>Throwing Nets</color> from <color=orange>Shop</color>, so that we can capture some <color=orange>Normal Slimes</color> from Slime Village: Area 1. You'll find the purchased traps in the Utility Inventory. Drag them to the Utility equip slot. <color=orange>Right Clicking</color> on a 'Normal' Slime will capture it. " +
                                //    "You can hover over the Trap to show what the Capturable Monster level is. Increasing your Hero level, Monster Study title, and increasing a town building level, will increase the level of monster you can trap. You will also be required to unlock new traps to capture other type of monsters.\n<color=yellow>For more information, Help > [Capture].</color>";
                                condition = "Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum()) + " / " + tDigit(10);
                                reward = "Taming Point Gain + 10%";
                                break;
                            case QuestKindGlobal.Capture2:
                                name = "Capturing Monsters 2";
                                client = "Dirgah Suebur, the Monster Handler";
                                description = "Oy d'ere! Ye must be looking for training on how to handle monstery types, yea? Well, look no fur'ter as I'd be happy to teach ye. First ting's first, ye need to get yerself a trap. Head yerself over to the Shop, buy yerself some traps and capture some monstery types, then come back and sees me. I'll be waitin' right here.";
                                //client = "Ringold";
                                //description = "Looks like you got the hang of capturing monsters. More on the pets, if you noticed you can only have so many active at a time. The <color=orange>Trapper</color> building has level milestones that will increase your total amount of Active Pets. " +
                                //    "Can you find the monster that allows you to automatically capture other monsters as long as you have traps equipped. It sure would make this quest a lot easier to complete.";
                                condition = "Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum()) + " / " + tDigit(1000);
                                reward = "Taming Point Gain + 20%";
                                break;
                            case QuestKindGlobal.Capture3:
                                name = "Capturing Monsters 3";
                                client = "Dirgah Suebur, the Monster Handler";
                                description = "Ey! Yer back! Looks like ye got yer hands on some traps and figured them out alright, ye did. Yer like a wizard, ye are! Since ye know how to wrangle a monstery type now, and ye've seen hows they turn into cute, wuvable pets, they do, ye may have noticed ye can only have so many afollowing ye at once. It's important to keep improving yer Trapper building! Oh and since we be friends now, I'll tell ye a little secret that will be surely helpful to ye. Seek out a monstery type that helps ye with trapping.";
                                //client = "Ringold";
                                //description = "I sure hope you found that <color=orange>Auto Capture</color> monster already! If you haven't. I'll give you a hint. It rhymes with paranormal insider. The last tidbit that I can give you about Pets. " +
                                //    "Increasing your pets level can be achieved through the use of your <color=orange>Tamer</color> hero. The higher the rank of the pet, higher the level you can have in that pet. Maybe that's why I'm giving you these quests, to boost your pets. " +
                                //    "Did you know that there's a monster that has an active effect of <color=orange>Auto Rebirthing for Tier 1</color>? I wonder if you can find it, and make it yours. I bet that it would make getting your guild level higher easier!";
                                condition = "Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum()) + " / " + tDigit(10000);
                                reward = "Taming Point Gain + 30%";
                                break;
                            case QuestKindGlobal.Capture4:
                                name = "Capturing Monsters 4";
                                client = "Dirgah Suebur, the Monster Handler";
                                description = "Well 'allo d'ere! That spidery fella was helpful, it was? I bet it was! Now if ye be wanting to do this in a more professional manner, so to speak, ye best be expanding the selection at the Shop! That requires fixing up the Trapper building real nice, it does. Come back and see me when the Shop has stocked traps for red monstery types!";
                                condition = "Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum()) + " / " + tDigit(100000);
                                reward = "Taming Point Gain + 40%";
                                break;
                            case QuestKindGlobal.Capture5:
                                name = "Capturing Monsters 5";
                                client = "Ringold";
                                condition = "Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum()) + " / " + tDigit(1000000);
                                reward = "Taming Point Gain + 50%";
                                break;
                            case QuestKindGlobal.Alchemy1:
                                name = "The Road of Alchemy 1";
                                //client = "Wakana";
                                client = "Archimedes, the Old Hermit";
                                description = "\"Hello and welcome to my hut! Fancy a potion? ... Oh you wish to learn the craft yourself? Ahh, well that is quite fortutitous, as I was just thinking of finding an apprentice. So to get started, in your lab you must... you don't have lab, you say? Fine, use mine... you will select a catalyst and then which essence you which to synthesize. It's very rudimentary stuff, really. Once you've collected enough essence, you can begin crafting potions! Improving them is essential to growth, so come back after you've improved some.\"";
                                description += "\n- When crafting potions, you will gain <color=orange>Alchemy Points</color>, which can be used to purchase <color=orange>Alchemy Upgrades</color> or to upgrade your potions, improving their effect and disassembly value. Use those points to gain a cumulative total of 100 potion levels to proceed to the next quest.";
                                description += "\n- It will benefit you to spend some time <color=orange>expanding your Mysterious Water capacity</color>, as it will be needed to upgrade your <color=orange>Catalysts</color> and increasing your Mysterious Water production amount.";
                                //description = "When you alchemise potions, you gain Alchemy Point that is used for potion's upgrade and Alchemy Upgrade." +
                                //" To understand more, please increase Alchemy Upgrade [ " + AlchemyUpgradeName(AlchemyUpgradeKind.MysteriousWaterSpeed) + " ] to Lv 5!" +
                                //" Then you will discover another Potion!";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 100";
                                reward = "Multiplies Critical Chance of Catalyst by 125%";
                                break;
                            case QuestKindGlobal.Alchemy2:
                                name = "The Road of Alchemy 2";
                                client = "Archimedes, the Old Hermit";
                                description = "\"Oh, uhh, there you are! I was just about to come looking for you to check on how well you are progressing with Alchemy. It seems that you've got the basics down. That's good, that's good. Now I will challenge you to continue doing that a little more, before we get into the less elementary level instruction.\"";
                                description += "\n- Continue using your Alchemy Points to level up your potions until they reach a cumulative total of 200 potion levels.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 200";
                                reward = "Max Mysterious Water Cap + 100";
                                break;
                            case QuestKindGlobal.Alchemy3:
                                name = "The Road of Alchemy 3";
                                client = "Archimedes, the Old Hermit";
                                description = "\"My, you are progressing much faster than any of my former pupils. Why, that's marvelous! Good, good, good. Okay, now that you've got a handle on the basics, it's time to test your skills a little. Keep improving your potions, yes, but I also want to ensure you grasp certain potions well enough for us to proceed with your training...\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 300, but you must also collect critical materials that you rarely gain when essences are producted.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 300";
                                reward = "Potion Level Cap + 25";
                                break;
                            case QuestKindGlobal.Alchemy4:
                                name = "The Road of Alchemy 4";
                                client = "Archimedes, the Old Hermit";
                                description = "\"My, you are progressing much faster than any of my former pupils. Why, that's marvelous! Good, good, good. Okay, now that you've got a handle on the basics, it's time to test your skills a little. Keep improving your potions, yes, but I also want to ensure you grasp certain potions well enough for us to proceed with your training...\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 300, but you must also collect critical materials that you rarely gain when essences are producted.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 500";
                                reward = "Catalyst Level Cap + 25";
                                break;
                            case QuestKindGlobal.Alchemy5:
                                name = "The Road of Alchemy 5";
                                client = "Archimedes, the Old Hermit";
                                description = "\"Oh these are marvelous specimans. Good work my student! I must say these are quite rare, as I'm sure you've discovered why that is, so do your best to collect as many as you can. Now, to the next endeavor. Learning Alchemy is more than simply mixing potions, you know? It's gaining an understanding of the world and the substances that make it up. While very basic, Mysterious Water, as I'm sure you've gathered, can be converted to just about anything if you apply the knowledge correctly. Gather yourself a large quantity of it, as we will be needing it for lessons to come!\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 750, but you must also expand your Mysterious Water capacity to 300.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 750\n- Mysterious Water Cap " + tDigit(game.alchemyCtrl.mysteriousWaterCap.Value()) + " / 300";
                                reward = "Lower the cost for leveling Catalysts by 25%";
                                break;
                            case QuestKindGlobal.Alchemy6:
                                name = "The Road of Alchemy 6";
                                client = "Archimedes, the Old Hermit";
                                description = "\"Always you astound me with how quickly you are completing the tasks I give you. You remind me a bit of myself, now that I think about it. Anywho, let's get back to work, shall we? Let's see, you've done that... okay, and this... excellent. Alright, now I'd like for you to explore the deeper mysteries of Alchemy. Collect 30 Ectoplasm and let me know when you've got them in hand.\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 1000, but you must also collect 30 Ectoplasm to give to Archimedes to proceed.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 1000";
                                reward = "Multiplies Critical Chance of Catalyst by 125%" +
                                    "\n- Max Mysterious Water Cap + 250";
                                break;
                            case QuestKindGlobal.Alchemy7:
                                name = "The Road of Alchemy 7";
                                client = "Archimedes, the Old Hermit";
                                description = "\"It's miraculous, really, this Ectoplasm. It honestly shouldn't exist, belonging to another plane of existence, and yet here it is. Now, we delve deeper, as this mystery is exceeded in its enigma by another. Now you must gather the very essence of the stars and bring me 100 Stardust. Only the most powerful of wizards has reached the stars themselves, but we can sometimes synthesize the substance by using a Sun Catalyst. Return to me when you have gathered it.\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 1500, but you must also collect 100 Stardust to give to Archimedes to proceed.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 1500";
                                reward = "Potion Level Cap + 25" +
                                    "\n- Catalyst Level Cap + 25";
                                break;
                            case QuestKindGlobal.Alchemy8:
                                name = "The Road of Alchemy 8";
                                client = "Archimedes, the Old Hermit";
                                description = "\"Alas, I knew this day would come. You are about to graduate from my tutelage, so I shall challenge you to discover the last of the secrets of Alchemy. Beyond our world, nay beyond the veil of reality itself lies a realm only known as the Void. No one who dared venture there has ever returned, but we have found traces of it in strange objects known as Void Eggs that are sometimes left behind when a Void portal closes. We have figured out how to synthesize these ourselves, with the use of the Void Catalyst. Again, I ask you to bring me 1000 of them and I shall bestow upon you the last bits of my wisdom.\"";
                                description += "\n- Now you must not only reach a cumulative potion level of 2000, but you must also collect 1000 Void Eggs to give to Archimedes.";
                                condition = "Total Potion Level " + tDigit(game.potionCtrl.TotalPotionLevel()) + " / 2000";
                                reward = "Alchemy Point Gain + 100%" +
                                    "\n- Lower the cost for leveling Catalysts by 25%";
                                break;
                        }
                        break;
                    case QuestKind.Daily:
                        switch (kindDaily)
                        {
                            case QuestKindDaily.EC1:
                                name = "Epic Coin 1 : " + quest.dailyQuestRarity.ToString();
                                client = "The Adventurer's Guild";
                                description = "The Adventurer's Guild has posted a " + quest.dailyQuestRarity.ToString() + " job to defeat " + tDigit(quest.defeatRequredDefeatNum()) + " " + MonsterSpeciesName(quest.dailyTargetMonsterSpecies) + " that have been terrorizing the villagers living near " + AreaName(quest.questingArea.kind) + ". Return here when you have completed the task.";
                                break;
                            case QuestKindDaily.EC2:
                                name = "Epic Coin 2 : " + quest.dailyQuestRarity.ToString();
                                client = "The Tamer's Association";
                                description = "The Tamer's Association has posted a " + quest.dailyQuestRarity.ToString() + " request to capture " + tDigit(quest.captureRequiredNum()) + " " + MonsterSpeciesName(quest.dailyTargetMonsterSpecies) + " for further study and analysis. Return here when you have completed the task.";
                                break;
                            case QuestKindDaily.EC3:
                                name = "Epic Coin 3 : " + quest.dailyQuestRarity.ToString();
                                client = "The Adventurer's Guild";
                                description = "The Adventurer's Guild has posted a " + quest.dailyQuestRarity.ToString() + " job to defeat " + tDigit(quest.defeatRequredDefeatNum()) + " " + MonsterSpeciesName(quest.dailyTargetMonsterSpecies) + " that have been terrorizing the villagers living near " + AreaName(quest.questingArea.kind) + ". Return here when you have completed the task.";
                                break;
                            case QuestKindDaily.EC4:
                                name = "Epic Coin 4 : " + quest.dailyQuestRarity.ToString();
                                client = "The Tamer's Association";
                                description = "The Tamer's Association has posted a " + quest.dailyQuestRarity.ToString() + " request to capture " + tDigit(quest.captureRequiredNum()) + " " + MonsterSpeciesName(quest.dailyTargetMonsterSpecies) + " for further study and analysis. Return here when you have completed the task.";
                                break;
                            case QuestKindDaily.Cartographer1:
                                name = "Cartographer 1 : " + quest.dailyQuestRarity.ToString();
                                client = "The Cartographer's Symposium";
                                description = "The Cartographer's Symposium wishes to send out a few cartographers to " + AreaName(quest.completeTargetArea.kind) + " to collect more data and inspect for any changes to the landscape and the creatures that dwell there. Please escort them while they complete this work. With this job being " + quest.dailyQuestRarity.ToString() + ", it is expected you will need to clear " + quest.completeTargetArea.Name(true, false) + " at least " + tDigit(quest.areaRequredCompletedNum()) + " times before the Cartographers will be finished with their task.";
                                break;
                            case QuestKindDaily.Cartographer2:
                                name = "Cartographer 2 : " + quest.dailyQuestRarity.ToString();
                                client = "The Cartographer's Symposium";
                                description = "The Cartographer's Symposium wishes to send out a few cartographers to " + AreaName(quest.completeTargetArea.kind) + " to collect more data and inspect for any changes to the landscape and the creatures that dwell there. Please escort them while they complete this work. With this job being " + quest.dailyQuestRarity.ToString() + ", it is expected you will need to clear " + quest.completeTargetArea.Name(true, false) + " at least " + tDigit(quest.areaRequredCompletedNum()) + " times before the Cartographers will be finished with their task.";
                                break;
                            case QuestKindDaily.Cartographer3:
                                name = "Cartographer 3 : " + quest.dailyQuestRarity.ToString();
                                client = "The Cartographer's Symposium";
                                description = "The Cartographer's Symposium wishes to send out a few cartographers to " + AreaName(quest.completeTargetArea.kind) + " to collect more data and inspect for any changes to the landscape and the creatures that dwell there. Please escort them while they complete this work. With this job being " + quest.dailyQuestRarity.ToString() + ", it is expected you will need to clear " + quest.completeTargetArea.Name(true, false) + " at least " + tDigit(quest.areaRequredCompletedNum()) + " times before the Cartographers will be finished with their task.";
                                break;
                            case QuestKindDaily.Cartographer4:
                                name = "Cartographer 4 : " + quest.dailyQuestRarity.ToString();
                                client = "The Cartographer's Symposium";
                                description = "The Cartographer's Symposium wishes to send out a few cartographers to " + AreaName(quest.completeTargetArea.kind) + " to collect more data and inspect for any changes to the landscape and the creatures that dwell there. Please escort them while they complete this work. With this job being " + quest.dailyQuestRarity.ToString() + ", it is expected you will need to clear " + quest.completeTargetArea.Name(true, false) + " at least " + tDigit(quest.areaRequredCompletedNum()) + " times before the Cartographers will be finished with their task.";
                                break;
                            case QuestKindDaily.Cartographer5:
                                name = "Cartographer 5 : " + quest.dailyQuestRarity.ToString();
                                client = "The Cartographer's Symposium";
                                description = "The Cartographer's Symposium wishes to send out a few cartographers to " + AreaName(quest.completeTargetArea.kind) + " to collect more data and inspect for any changes to the landscape and the creatures that dwell there. Please escort them while they complete this work. With this job being " + quest.dailyQuestRarity.ToString() + ", it is expected you will need to clear " + quest.completeTargetArea.Name(true, false) + " at least " + tDigit(quest.areaRequredCompletedNum()) + " times before the Cartographers will be finished with their task.";
                                break;

                        }
                        break;
                    case QuestKind.Title:
                        switch (kindTitle)
                        {
                            case QuestKindTitle.SkillMaster1:
                                name = Title(TitleKind.SkillMaster) + " 1";
                                client = "Condescending Asian Kid";
                                description = "\"You there! Rookie! Yes, clearly I mean you, the 'Hero' who looks like you're practicing your white belt katas but keep forgetting the next step. I can see that you have potential but that's only because I have excellent eyesight! You need to walk before you can run, so I would suggest practicing your basic moves until you get the technique right and can do it without thinking. Then come back to me and I'll show you an easy one-two combo.\"";
                                description += "\n- Skills gain <color=orange>Proficiency</color> every time they trigger, and they gain levels just like your Hero. Each Hero collects a type of Resource that can be spent to increase <color=orange>Skill Ranks</color> which increase the Skill's maximum level.";
                                description += "\n- Clearing this quest gives the <color=orange>Class Master</color> Title, which expands a <color=orange>Class Skill Slot</color>. After you gain a new skill and an empty Class Skill slot, you can click the skill icon in the Skill tab to equip/remove it. Keep in mind that the first Base Attack Skill is not removable.";
                                //description = "Hey, you wanna learn how to brush up your skills, right? Skill levels increase when their <color=orange>Proficiency</color> reaches 100%. " +
                                //    "You gain Proficiency every time you use it! You can increase a Skill's <color=orange>Rank</color> by spending three kinds of Resources: stones, crystals and leaves. " +
                                //    "Ranking a Skill up raises its max level and improves its effect per level. First, try brushing up your first skill to level 10! " +
                                //    "You'll gain the <color=orange>Class Master</color> Title, which expands your Skill slots and boosts your Skill Proficiency Gain. " +
                                //    "After you get a new skill and another slot, you can <color=orange>click the skill icon to equip/remove it</color>. Keep in mind that the first <color=orange>Base Attack Skill is not removable.</color>";
                                condition = SkillName(heroKind, 0) + " Lv 10";
                                break;
                            case QuestKindTitle.SkillMaster2:
                                name = Title(TitleKind.SkillMaster) + " 2";
                                client = "Condescending Asian Kid";
                                description = "You see the kid from before practicing Tai Chi. His eyes are closed as moves smoothly from one position to the next, his breath timed perfectly with his movements. He starts to speak without opening his eyes as he continues gracefully moving. \"Hey Rookie. You're doing better but I'm afraid that's not saying much. You can't build a house without a strong foundation and your foundation is as weak as yesterday's tea. Strengthen your core and work on your balance until you can dance a waltz without tripping over your own feet.\"";
                                //description = "Great job getting your basic attack to level 10! However, that is just the beginning. Now focus on getting it to level 50 for another class skill slot!";
                                condition = SkillName(heroKind, 0) + " Lv 50";
                                break;
                            case QuestKindTitle.SkillMaster3:
                                name = Title(TitleKind.SkillMaster) + " 3";
                                client = "Condescending Asian Kid";
                                description = "The kid is practicing Tai Chi again, but this time he is balancing a glass of water on each upturned palm. He flows quickly and fluidly through the movements but not a ripple shows in the water. He turns and locks his gaze on you while he continues to move with the grace of a cat. \"Hey Rookie. You're getting better, I saw you almost keep control of your moves back there. Almost, anyway. Keep working on your control and your technique and before you know it you'll start to look like you know what you're doing!\"";
                                //description = "You’ve done well, and you have developed great muscle memory with so much practice. Now push yourself to your limits and achieve basic attack level 250! That will prove you are worthy of being called a master hero!";
                                condition = SkillName(heroKind, 0) + " Lv 250";
                                break;
                            case QuestKindTitle.SkillMaster4:
                                name = Title(TitleKind.SkillMaster) + " 4";
                                client = "Condescending Asian Kid";
                                description = "You find the kid sparring with a dozen men and women. Or rather, a dozen men and women are trying to spar with him but he dances around them, redirecting their strikes with light touches and avoiding every advance. He sees you watching and calls out to you. \"Hey Rookie, looks like you built a strong foundation after all. Unfortunately that means I owe Gary fifty bucks. Make it up to me, will you? I'll raise him double or nothing that you can master your whole art, I'll even cut you in if you can do it.\"";
                                //description = "Incredible, such impressive display of skill. However, one must not neglect all their other skills either. Your final task to becoming a master of skills is to raise all your skills levels to 250! The road to mastery is a long one, but you can achieve this too!";
                                condition = "All " + Hero(heroKind) + "'s skills Lv 250";
                                break;
                            case QuestKindTitle.MonsterDistinguisher1:
                                name = Title(TitleKind.MonsterDistinguisher) + " 1";
                                client = "Ranger Rick";
                                //description = "There are a lot of slimes in this area. Defeat 20 Normal Slimes as soon as possible! Monster Study Title enables you to see monsters' stats and allows you to use traps once acquired!"; 
                                description = "Oh, hey there, you want to be a Ranger Scout? Doing so will teach you how to be more knowledgeable about monsters, their habits, their diets, everything... oh, you want it to learn how to trap them more efficiently? I suppose that is also a benefit. Okay, so get started by going out there and defeating 20 normal slimes in combat! Pay close attention to their habits while you do and I’ll dub you a Ranger Scout when you’re done.";
                                break;
                            case QuestKindTitle.MonsterDistinguisher2:
                                name = Title(TitleKind.MonsterDistinguisher) + " 2";
                                client = "Ranger Rick";
                                description = "Studying monsters is a lot of fun, don’t you think? As you become more knowledgeable, your ability to capture higher level monsters will improve! So now let’s study a different monster. Go defeat around 100 Blue Slimes and report back to me when you’re done. I can’t wait to hear everything you’ve learned!";
                                break;
                            case QuestKindTitle.MonsterDistinguisher3:
                                name = Title(TitleKind.MonsterDistinguisher) + " 3";
                                client = "Ranger Rick";
                                //description = "I've heard about Yellow Slimes but I've never seen them. I wonder if they are yellow bellied slimes. They must be easy if they are yellow. You should be able to defeat 250 of them right?";
                                description = "Ever felt the slight sting of electrocution before? Well, if you want to learn how to avoid it in the wild, you’ll need to tangle with some yellow slimes and understand how to avoid their electrically charged ooze. Learning how to deal with the strengths of these monsters will give you an edge when you are attempting to capture them. So go defeat around 250 Yellow Slimes and report back to me.";
                                break;
                            case QuestKindTitle.MonsterDistinguisher4:
                                name = Title(TitleKind.MonsterDistinguisher) + " 4";
                                client = "Ranger Rick";
                                description = "This time let’s change things up, because I’m not sure what else you will learn from fighting slimes. This time, I want you to study the habits of blue spiders. Some people are incredibly afraid of spiders, in which case it’s a good effort to overcome those fears by facing them head on! Just be careful and do your best! Let me know after you’ve defeated around 1000 Blue Spiders.";
                                break;
                            case QuestKindTitle.MonsterDistinguisher5:
                                name = Title(TitleKind.MonsterDistinguisher) + " 5";
                                client = "Ranger Rick";
                                description = "There are some pretty interesting monsters out there, aren’t there? Well, how about one that can literally make you move slower? How do you handle not being able to move at the speed you are used to moving? You adapt and make every movement matter! Go and study Red Fairies, defeating around 5000 of them ought to help you to understand how to overcome their strengths and improve your ability to capture monsters.";
                                break;
                            case QuestKindTitle.MonsterDistinguisher6:
                                name = Title(TitleKind.MonsterDistinguisher) + " 6";
                                client = "Ranger Rick";
                                description = "Alright, I don’t know if I have much left to teach you, but I must ask if you’ve ever gone fishing? It’s quite a sport indeed, for those with the patience to endure it. So, my last lesson will come in the form of fishing! Go and defeat 50000 Green Devilfish and report back to me. Remember, this is a patience building exercise, but it will vastly improve your ability to capture stronger monsters – I guarantee it!";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon1:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 1";
                                client = "Equipment Trainer, Tsuba";
                                description = "Greetings novice. You have come seeking knowledge for weapon mastery? Very well. First, equip any weapon and simply start using it. As you do, you will notice your proficiency with that weapon will improve until it levels up, improving the effects it bestows upon you. Return to me when you have attained Level 10 with at least one weapon, and I shall show you how to wield more weapons.";
                                //description = "Drag and drop any equipment into equipment slot. Its proficiency increases over time during equipping. " +
                                //    "Its level increases when its proficiency reaches 100% and boosts the effect. Each class can increase equipment's level up to Lv 10 and then get bonus effect which is unique to the class. " +
                                //    "Try leveling up Weapon equipment to Lv 10! " +
                                //    "You will win Apprentice of Weapon Title, which expands the Weapon slot!";
                                condition = "1 Weapon Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon)) + " / 1";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon2:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 2";
                                client = "Equipment Trainer, Tsuba";
                                description = "You have proven yourself capable, novice. Do it again now, but this time attain Level 10 with at least five weapons, and I will again teach you how to wield more weapons!";
                                condition = "5 Weapons Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon)) + " / 5";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon3:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 3";
                                client = "Equipment Trainer, Tsuba";
                                description = "It seems I cannot keep calling you novice, as you have demonstrated fine ability with using a variety of weapons. Now, adept, if you wish to continue your training with me, I must see that you have attained Level 10 with at least 15 weapons!";
                                condition = "15 Weapons Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon)) + " / 15";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon4:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 4";
                                client = "Equipment Trainer, Tsuba";
                                description = "Well done, adept. Again, 30 weapons this time. Show me that you have what it takes to be a weapons master!";
                                condition = "30 Weapons Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon)) + " / 30";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon5:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 5";
                                client = "Equipment Trainer, Tsuba";
                                description = "I am impressed, adept. Now for something more challenging. I desire to see at least 20 weapons at level 15. Push past the limits of ordinary and begin to achieve greatness!";
                                condition = "20 Weapons Lv 15 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon, 15)) + " / 20";
                                break;
                            case QuestKindTitle.EquipmentSlotWeapon6:
                                name = Title(TitleKind.EquipmentSlotWeapon) + " 6";
                                client = "Equipment Trainer, Tsuba";
                                description = "There is very little left that I can teach you, so I will issue you one last challenge. Have over 50 weapons at level 20, and I will bestow the last of my secrets to you.";
                                condition = "50 Weapons Lv 20 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon), 20) + " / 50";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor1:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 1";
                                client = "Equipment Trainer, Koutetsu";
                                description = "The path to armor mastery begins here, with you seeking such wisdom. Prove to me you desire this wisdom by leveling any piece of armor to Level 10.";
                                    //"Try leveling up Armor equipment to Lv 10! " +
                                    //"You will win Apprentice of Armor Title, which expands the Armor slot!";
                                condition = "1 Armor Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor)) + " / 1";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor2:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 2";
                                client = "Equipment Trainer, Koutetsu";
                                description = "Ahh, excellent. Your pursuit in wisdom of armor mastery is noble my young novice. Now take what you have learned and do it five times! You will begin to understand the mind of the armor the more you practice, enabling you to wear more armor without it encumbering you.";
                                condition = "5 Armor Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor)) + " / 5";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor3:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 3";
                                client = "Equipment Trainer, Koutetsu";
                                description = "Remarkable work my keen novice. Your zeal for understanding is admirable, but you must push further if you wish to gain the clarity you seek. Now bring to me 15 pieces of armor at level 10 and I will further illuminate you to efficient armor wearing.";
                                condition = "15 Armor Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor)) + " / 15";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor4:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 4";
                                client = "Equipment Trainer, Koutetsu";
                                description = "It would seem you have become adept at this, so now my young adept, can you handle the task of bringing me 30 armor at level 10 and I will train you further on moving freely in more armor.";
                                condition = "30 Armor Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor)) + " / 30";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor5:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 5";
                                client = "Equipment Trainer, Koutetsu";
                                description = "Sometimes, what we think is the highest we can go is just another beginning. This time I ask you to exceed your limits and bring me 20 pieces of armor at level 15, and we can continue training your armor wearing ability.";
                                condition = "20 Armor Lv 15 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor, 15)) + " / 20";
                                break;
                            case QuestKindTitle.EquipmentSlotArmor6:
                                name = Title(TitleKind.EquipmentSlotArmor) + " 6";
                                client = "Equipment Trainer, Koutetsu";
                                description = "You have nearly matched me, my keen adept. The last I have to teach you is to seek the wisdom found in bringing 50 pieces of armor to level 20. I will bestow to you the last secret knowledge I have gathered in my life upon your return.";
                                condition = "50 Armor Lv 20 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor, 20)) + " / 50";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry1:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 1";
                                client = "Equipment Trainer, Joudama";
                                description = "Why hello there, are you interested in jewelry? Ahh, you are like an uncut diamond, my dear, but that we can fix. Bring me any accessory piece that you’ve leveled to 10 and we can speak more about refining your appearance. I’m looking forward to it.";
                                    //"Try leveling up Jewelry equipment to Lv 10! " +
                                    //"You will win Apprentice of Jewelry Title, which expands the Jewelry slot!";
                                condition = "1 Jewelry Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry)) + " / 1";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry2:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 2";
                                client = "Equipment Trainer, Joudama";
                                description = "Oh, now that is exquisite indeed. There is nothing quite like seeing a freshly polished gem in your hands, and you, my dear, are that gem to me. Now, our work is not yet done. Go now and come back to see me after you’ve got 5 accessory pieces to level 10. I’ll be thinking about you until we meet again!";
                                condition = "5 Jewelry Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry)) + " / 5";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry3:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 3";
                                client = "Equipment Trainer, Joudama";
                                description = "Set my heart alight, you have done so well! You may very well be a diamond in the rough, my dear. Let’s continue polishing you up and expose that hidden, ravishing beauty that you are! Bring me 15 accessory items that you’ve leveled to 10 and I will show you ways to make you sparkle in the sun!";
                                condition = "15 Jewelry Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry)) + " / 15";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry4:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 4";
                                client = "Equipment Trainer, Joudama";
                                description = "Dare I say that the stars sparkle less brightly than you? Perhaps not quite yet, but you’re certainly a twinkle in my eyes right now. Though there’s only so much we do with polishing, now we must cut this gemstone to really bring out its true beauty, as I see in you, my dear. Bring me 30 accessory pieces that are level 10, and I will show you how to shine like a diamond!";
                                condition = "30 Jewelry Lv 10 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry)) + " / 30";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry5:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 5";
                                client = "Equipment Trainer, Joudama";
                                description = "You’ve not disappointed me once, my exquisite little gem. True perfection is but a goal, though to me that which I see in you is nearly that. To rise to the next level and to reach for this goal I need you to bring me 20 accessory pieces at level 15. Only through surpassing our standards can we truly begin to glow.";
                                condition = "20 Jewelry Lv 15 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry, 15)) + " / 20";
                                break;
                            case QuestKindTitle.EquipmentSlotJewelry6:
                                name = Title(TitleKind.EquipmentSlotJewelry) + " 6";
                                client = "Equipment Trainer, Joudama";
                                description = "My dear, to say you are ravishing upon the eyes is an understatement to be sure. Alas, I fear this little project of ours is near its end. I will miss seeing your perfect little face with those dazzling eyes staring longingly at me as I return that gaze. I shall not stand in your ascent to unimaginable beauty, but I will set you on the path to reach for it. Bring me back 50 accessory pieces at level 20 and I will bestow the last bits of titillating wisdom I have with you. ";
                                condition = "50 Jewelry Lv 20 for " + Hero(heroKind) + " : " + tDigit(game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry, 20)) + " / 50";
                                break;

                            case QuestKindTitle.PotionSlot1:
                                name = Title(TitleKind.PotionSlot) + " 1";
                                client = "Alchemy Guru, Flamel";
                                description = "Hmmm? Who are you and why are you in my laboratory? Oh, you come seeking advice on Alchemy, do you? Hmm, well I’m not one to share my alchemy secrets openly, but I can show you how to be more efficient with your potions. Fetch me these items and we can talk more.";
                                break;
                            case QuestKindTitle.PotionSlot2:
                                name = Title(TitleKind.PotionSlot) + " 2";
                                client = "Alchemy Guru, Flamel";
                                description = "Do I know you? Oh, that’s right, you’re seeking alchemical knowledge. Again, sorry but I won’t share that kind of knowledge with people coming in off the street. However, I do need someone to fetch me more ingredients. If you get me these, I’ll show you how to expand your potion-carrying ability.";
                                break;
                            case QuestKindTitle.PotionSlot3:
                                name = Title(TitleKind.PotionSlot) + " 3";
                                client = "Alchemy Guru, Flamel";
                                description = "I don’t remember ordering a pizza… oh sorry, you just have that look of someone always looking to run an errand for someone else. Speaking of which, I need some more ingredients. If you’d be so kind as to fetch these I’ll teach you how to unlock another potion slot, but after this I will be setting off to travel the world, and please stop asking me for alchemical recipes – I really cannot share them with you.";
                                break;

                            case QuestKindTitle.Porter1:
                                name = "The Great Race 1";
                                client = "Bebop the Warthog";
                                description = "\"Hey, you, wanna race? I bet I can run 1000 meters faster than you can. Beat me and I’ll teach you how to be even faster than me!\" You are a little concerned that racing this large, warthog man will be pointless, but you might as well since he seems interested in teaching you something that may be useful.";
                                //description = "Hey, you know Dr McNutty? He needs you to walk 1000 meters in his slime gut covered shoes. Go kill some Slimes while you're at it.";
                                break;
                            case QuestKindTitle.Porter2:
                                name = "The Great Race 2";
                                client = "Rocksteady the Rhino";
                                description = "\"That’s no fair, you raced that warthog-faced buffoon, but not me?! I can teach you how to be even faster than me, so let’s start now!\" Why are these strange animal people approaching you like this? Who cares, might as well get moving.";
                                //description = "A crazed man walks up to you. \"I saw it, a slime with a mullet! it was over there somewhere..Go forth, find me that slime!\"";
                                break;
                            case QuestKindTitle.Porter3:
                                name = "The Great Race 3";
                                client = "Tokka the Snapping Turtle";
                                description = "\"You might think I’m slow, being a turtle, but that’s why the turtle won against the hare. I can’t stand that warhog and rhino, so I’ll teach you how to be even faster than all of us if you can beat me at running 100k meters!\" Where are these anthropomorphic animals even coming from?!";
                                break;
                            case QuestKindTitle.Porter4:
                                name = "The Great Race 4";
                                client = "Rahzar the Wolf";
                                description = "It’s easy to beat a turtle, a rhino, and a warthog… I am much more cunning and much faster in a long run than any of them. Prove to me that you’re faster than a wolf by running 1 million meters and I’ll teach you what I know about being the fastest!";
                                break;
                            case QuestKindTitle.Porter5:
                                name = "The Great Race 5";
                                client = "Baxter the Fly";
                                description = "Oh you think that beating all of these simpleton land runners that you stand a chance at beating me in a race? Hardly possible, but let’s give it a go. I won’t be running, though, since I have wings, so let’s say 10 million meters as our goal? See you at the finish line!";
                                break;
                            case QuestKindTitle.Porter6:
                                name = "The Great Race 6";
                                client = "Krang the … brain… in a flesh golem?";
                                description = "Muaha, you may have defeated all of those fools, but I can make myself grow massive with this golem body I possess. Race me! 100 million meters! Win or lose, I’ll teach you how to be the fastest in the world!";
                                break;
                            case QuestKindTitle.PhysicalAttack1:
                                name = Title(TitleKind.PhysicalDamage) + " 1";
                                client = "Boxer Tyson";
                                description = "Yo, you new around here? I’ve been using slimes as punching bags. Care to join me? Let’s beat up some monsters together. It’s very therapeutic and if you can keep up with me, I’ll give you some pointers on maximizing your physical damage.";
                                //client = "Magnificent Marcus";
                                //description = "As you arrive in Magicslime City, a man in a brown trench coat whirls his cane and bows lifting his top hat as he greets you. \"Welcome, and greetings to all you're just in time for some magic lessons, but these slimes are interrupting. Can you take care of them for me so the lesson can commence..\"";
                                break;
                            case QuestKindTitle.PhysicalAttack2:
                                name = Title(TitleKind.PhysicalDamage) + " 2";
                                client = "Boxer Tyson";
                                description = "Not bad, not bad. You really didn’t disappoint me. But we’ve got a lot more monsters and if these fists aren’t smashing, these muscles aren’t growing. Let’s get back to it and I’ll help you with your posture when you strike.";
                                //client = "Master of Arms";
                                //description = "Hey Kid! I've seen you training to kill Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.PhysicalAttack3:
                                name = Title(TitleKind.PhysicalDamage) + " 3";
                                client = "Boxer Tyson";
                                description = "You’re not getting tired, are you? Come on, the monster hordes are endless and who can ask for a better workout than this? I’m not ready to plateau yet, so let’s push even harder this time and see how swole we can get.";
                                break;
                            case QuestKindTitle.PhysicalAttack4:
                                name = Title(TitleKind.PhysicalDamage) + " 4";
                                client = "Boxer Tyson";
                                description = "Oh man, I’ve had the best time punching these monsters out with you. Your muscles look great! Alright, one last stretch before we reach our limit. Come on, we got gains to make.";

                                break;
                            case QuestKindTitle.FireAttack1:
                                name = Title(TitleKind.FireDamage) + " 1";
                                client = "Pyromancer Natsu";
                                description = "Ey, you interested in fire magic? I know a thing a two I could teach you. Come on, we can train together, and I’ll see what you’re capable of with your magic.";
                                //client = "Magnificent Marcus";
                                //description = "Now for the first lesson. Blue is the color of cold magic, and as such Blue Magicslimes resist cold. : However, every monster has a weakness. Could you demonstrate by cooking a few for me?";
                                break;
                            case QuestKindTitle.FireAttack2:
                                name = Title(TitleKind.FireDamage) + " 2";
                                client = "Pyromancer Natsu";
                                description = "Wow, you might just have a fire in your belly. Fire magic is powerful, but it requires strong convictions or else it’ll backfire and burn you too. Come on, I think I’m going to enjoy blazing next to you for a while!";
                                //client = "Master of Arms";
                                //description = "Hey Kid! I've seen you training to kill Blue Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.FireAttack3:
                                name = Title(TitleKind.FireDamage) + " 3";
                                client = "Pyromancer Natsu";
                                description = "Awesome! I love fighting alongside my friends, and I feel like we’re both really improving! Let’s get out there and show them our fire is the strongest!";
                                break;
                            case QuestKindTitle.FireAttack4:
                                name = Title(TitleKind.FireDamage) + " 4";
                                client = "Pyromancer Natsu";
                                description = "Looks like I’ll be needing to head back to my guild soon, but I have a little more fire in my belly to spare, so let’s go burn em up one last time. Glad I got to fight alongside you for a while!";
                                break;
                            case QuestKindTitle.IceAttack1:
                                name = Title(TitleKind.IceDamage) + " 1";
                                client = "Cryomancer, Shimo Aisu";
                                description = "It isn’t everyday I meet someone interested in the frozen arts. You are looking for someone to practice your skills with I take it? Good, let’s get started then.";
                                //client = "Magnificent Marcus";
                                //description = "Furthermore, Yellow Magicslimes are made out of nonconductive jelly. Making lightning almost useless against them. Try freezing them solid and see what happens.";
                                break;
                            case QuestKindTitle.IceAttack2:
                                name = Title(TitleKind.IceDamage) + " 2";
                                client = "Cryomancer, Shimo Aisu";
                                description = "I’d say we’re working up a good sweat, but then we’d probably freeze ourselves. You’re pretty good but remember that ice magic is about controlling the cold outside of yourself and the heat within. You must always remember to keep your heart warm, or it will freeze along with your enemies. Now practice focusing on your heart as you cast.";
                                //client = "Master of Arms";
                                //description = "Hey Kid! I've seen you training to kill Yellow Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.IceAttack3:
                                name = Title(TitleKind.IceDamage) + " 3";
                                client = "Cryomancer, Shimo Aisu";
                                description = "Excellent, you’re far better than I expected you to be. Ice magic tends to turn most people away because it creates unfavorable conditions for training, but a true master understands that what happens within you is more important than what happens without. Reflect on that as we practice some more.";
                                break;
                            case QuestKindTitle.IceAttack4:
                                name = Title(TitleKind.IceDamage) + " 4";
                                client = "Cryomancer, Shimo Aisu";
                                description = "I am not one for puns but watching your growth with ice magic has given me chills! You may be one of those hidden masters that only arise once a century or two! I am grateful for our time together, as I have learned much from watching you as well. Let us practice some more, though now I fear I have little left to teach you.";
                                break;
                            case QuestKindTitle.ThunderAttack1:
                                name = Title(TitleKind.ThunderDamage) + " 1";
                                client = "Thunderbeast Raiju";
                                description = "You see a strange beast, electricity arcing across its fur sporadically, resting under what appears to be a blackened tree. It glances at you, sniffs at the air, and it stares at you intently. You get the feeling it wants you to follow it and the urge to use thunder magic swells within you.";
                                //client = "Magnificent Marcus";
                                //description = "Red Magicslimes. Made of the most insulated of all goops. Could you take some down with thunder attacks? Their goop will droop if you give them a nice shock.";
                                break;
                            case QuestKindTitle.ThunderAttack2:
                                name = Title(TitleKind.ThunderDamage) + " 2";
                                client = "Thunderbeast Raiju";
                                description = "The beast appears pleased as it watches you sending bolts of thunderous magic left and right. Suddenly, it sends a sharp bolt of its own directly into your chest. Surprisingly, this is painless as you see dozens of battles play forth in your mind. It walks a few meters and pauses, as if waiting for you to follow again. You can feel the magic of thunder again wishing to burst forth from your fingers.";
                                //client = "Master of Arms";
                                //description = "Hey Kid! I've seen you training to kill Red Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.ThunderAttack3:
                                name = Title(TitleKind.ThunderDamage) + " 3";
                                client = "Thunderbeast Raiju";
                                description = "Incredibly, despite no regular means of communication, you feel you have learned from this strange lightning creature. Still, it seems to beckon to you that there is more it wishes to show you. You can tell the time you are spending with this beast is aiding your growth with thunder magic. It suddenly roars, though it sounds like a huge thunderclap and darts off running toward more enemies. You’ve come this, so you decide to continue fighting alongside it.";
                                break;
                            case QuestKindTitle.ThunderAttack4:
                                name = Title(TitleKind.ThunderDamage) + " 4";
                                client = "Thunderbeast Raiju";
                                description = "The beast stares at you for a moment, its eyes no longer sharp and untrusting as they were when you first met. It seems a little sad, as though it knows it must depart soon after growing a little attached to you. Still, it doesn’t look like its quite ready to leave just yet, and there are always more enemies to blast. Practicing alongside this legendary being has certainly been helpful, so before it must go you decide to continue the fight for as long as it can remain.";
                                break;
                            case QuestKindTitle.LightAttack1:
                                name = Title(TitleKind.LightDamage) + " 1";
                                client = "The lazy farmer Aggrezi";
                                description = "You there, hey, I uhh saw you could use light magic and wondered if you could help me out? I, uhh, am so weak and fragile and the monsters here are so, uhh, dark and scary. If you wouldn’t mind, uhh, practicing that light magic and keeping me, uhh, safe, I’d appreciate it. I’ll just be over here in my, uhh, recliner waiting for you to clear the fields of those abominations.";
                                break;
                            case QuestKindTitle.LightAttack2:
                                name = Title(TitleKind.LightDamage) + " 2";
                                client = "The lazy farmer Aggrezi";
                                description = "Delightful, ermm, I mean good work there! You, uhh, really showed them how it is done. Next time, though, you should, uhh, let the light pass through your crown chakra more, err, I mean let the heavens flow through you, yeah. I’m sure that’ll help you get stronger and, uhh, take down the baddies faster. If you need me, uhh, I’ll be over here sipping some lemonade.";
                                //client = "Master of Arms";
                                //description = "Hey Kid! I've seen you training to kill Purple Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.LightAttack3:
                                name = Title(TitleKind.LightDamage) + " 3";
                                client = "The lazy farmer Aggrezi";
                                description = "Well done mortal, errm, I mean you’ve done a splendid job clearing my fields! Hmm, you think I’m acting suspicious? Uhh, I don’t know what you mean. Just a lazy farmer whose wings, errm, arms are too weak to do the job any longer, yeah. I couldn’t help but notice, uhh, you should find a source of purity within yourself, errm, find a happy place when you cast. I’m sure that’ll help, yeah. Alright, enough chit chat, get back out there.";
                                break;
                            case QuestKindTitle.LightAttack4:
                                name = Title(TitleKind.LightDamage) + " 4";
                                client = "The lazy farmer Aggrezi";
                                description = "I cannot hide my appearance any longer from you, dear mortal. You have done exceptionally well at honing your light magic and growing the purity within yourself. I was sent here to guide you, but I was meant to do it discreetly, but you have surpassed my expectations and so I felt I owed it to you to reveal myself at least. I will be leaving soon, but before I do it would be my honor to practice the sacred light magic alongside you.";
                                break;
                            case QuestKindTitle.DarkAttack1:
                                name = Title(TitleKind.DarkDamage) + " 1";
                                break;
                            case QuestKindTitle.DarkAttack2:
                                name = Title(TitleKind.DarkDamage) + " 2";
                                client = "Master of Arms";
                                description = "Hey Kid! I've seen you training to kill Green Magicslimes. How about you master your attacks by performing them over and over again.";
                                break;
                            case QuestKindTitle.DarkAttack3:
                                name = Title(TitleKind.DarkDamage) + " 3";
                                break;
                            case QuestKindTitle.DarkAttack4:
                                name = Title(TitleKind.DarkDamage) + " 4";
                                break;
                            case QuestKindTitle.Alchemist1:
                                name = Title(TitleKind.Alchemist) + " 1";
                                break;
                            case QuestKindTitle.Alchemist2:
                                name = Title(TitleKind.Alchemist) + " 2";
                                break;
                            case QuestKindTitle.Alchemist3:
                                name = Title(TitleKind.Alchemist) + " 3";
                                break;
                            case QuestKindTitle.Alchemist4:
                                name = Title(TitleKind.Alchemist) + " 4";
                                break;
                            case QuestKindTitle.Alchemist5:
                                name = Title(TitleKind.Alchemist) + " 5";
                                break;
                            case QuestKindTitle.EquipmentProf1:
                                name = Title(TitleKind.EquipmentProficiency) + " 1";
                                client = "Karosis";
                                description = "\"Pssst,\" a disheveled individual wearing glasses and carrying a small collection of scrolls calls to you from a nearby alley. \"Looks like you found something unusual and hard to find there. I'll give you something nice if you can find me some other rare items.\"";
                                condition = "Equipment on the battlefield from monster kills : " + tDigit(GameController.game.battleCtrl.EquipmentDroppingNum()) + " / 5";
                                break;
                            case QuestKindTitle.EquipmentProf2:
                                name = Title(TitleKind.EquipmentProficiency) + " 2";
                                condition = "Equipment on the battlefield from monster kills : " + tDigit(GameController.game.battleCtrl.EquipmentDroppingNum()) + " / 10";
                                break;
                            case QuestKindTitle.EquipmentProf3:
                                name = Title(TitleKind.EquipmentProficiency) + " 3";
                                condition = "Equipment on the battlefield from monster kills : " + tDigit(GameController.game.battleCtrl.EquipmentDroppingNum()) + " / 15";
                                break;
                            case QuestKindTitle.EquipmentProf4:
                                name = Title(TitleKind.EquipmentProficiency) + " 4";
                                condition = "Equipment on the battlefield from monster kills : " + tDigit(GameController.game.battleCtrl.EquipmentDroppingNum()) + " / 20";
                                break;
                            case QuestKindTitle.EquipmentProf5:
                                name = Title(TitleKind.EquipmentProficiency) + " 5";
                                condition = "Equipment on the battlefield from monster kills : " + tDigit(GameController.game.battleCtrl.EquipmentDroppingNum()) + " / 30";
                                break;

                            case QuestKindTitle.MetalHunter1:
                                name = Title(TitleKind.MetalHunter) + " 1";
                                client = "Metallic Nuts";
                                description = "Did you know that there are metal monsters in this world? You have a rare chance of spotting them. In the Slime dungeons, there are metal slimes for instance. Their bodies are so hard that you can only deal 1 damage to them. However, if you defeat them, you'll get a bunch of EXP. First of all, I want you to go kill one metal slime. Once you've done that, I'm sure you'll be a little better at killing metal slimes.";
                                break;
                            case QuestKindTitle.MetalHunter2:
                                name = Title(TitleKind.MetalHunter) + " 2";
                                client = "Metallic Nuts";
                                description = "I see you've defeated the metal slime! It looks like you've learned a bit how to destroy metal bodies. Keep it up and you'll learn how to hunt metal more efficiently."; 
                                break;
                            case QuestKindTitle.MetalHunter3:
                                name = Title(TitleKind.MetalHunter) + " 3";
                                client = "Metallic Nuts";
                                break;
                            case QuestKindTitle.MetalHunter4:
                                name = Title(TitleKind.MetalHunter) + " 4";
                                client = "Metallic Nuts";
                                break;
                            case QuestKindTitle.FireResistance1:
                                name = Title(TitleKind.FireResistance) + " 1";
                                break;
                            case QuestKindTitle.FireResistance2:
                                name = Title(TitleKind.FireResistance) + " 2";
                                break;
                            case QuestKindTitle.FireResistance3:
                                name = Title(TitleKind.FireResistance) + " 3";
                                break;
                            case QuestKindTitle.FireResistance4:
                                name = Title(TitleKind.FireResistance) + " 4";
                                break;
                            case QuestKindTitle.FireResistance5:
                                name = Title(TitleKind.FireResistance) + " 5";
                                break;
                            case QuestKindTitle.IceResistance1:
                                name = Title(TitleKind.IceResistance) + " 1";
                                break;
                            case QuestKindTitle.IceResistance2:
                                name = Title(TitleKind.IceResistance) + " 2";
                                break;
                            case QuestKindTitle.IceResistance3:
                                name = Title(TitleKind.IceResistance) + " 3";
                                break;
                            case QuestKindTitle.IceResistance4:
                                name = Title(TitleKind.IceResistance) + " 4";
                                break;
                            case QuestKindTitle.IceResistance5:
                                name = Title(TitleKind.IceResistance) + " 5";
                                break;
                            case QuestKindTitle.ThunderResistance1:
                                name = Title(TitleKind.ThunderResistance) + " 1";
                                break;
                            case QuestKindTitle.ThunderResistance2:
                                name = Title(TitleKind.ThunderResistance) + " 2";
                                break;
                            case QuestKindTitle.ThunderResistance3:
                                name = Title(TitleKind.ThunderResistance) + " 3";
                                break;
                            case QuestKindTitle.ThunderResistance4:
                                name = Title(TitleKind.ThunderResistance) + " 4";
                                break;
                            case QuestKindTitle.ThunderResistance5:
                                name = Title(TitleKind.ThunderResistance) + " 5";
                                break;
                            case QuestKindTitle.LightResistance1:
                                name = Title(TitleKind.LightResistance) + " 1";
                                break;
                            case QuestKindTitle.LightResistance2:
                                name = Title(TitleKind.LightResistance) + " 2";
                                break;
                            case QuestKindTitle.LightResistance3:
                                name = Title(TitleKind.LightResistance) + " 3";
                                break;
                            case QuestKindTitle.LightResistance4:
                                name = Title(TitleKind.LightResistance) + " 4";
                                break;
                            case QuestKindTitle.LightResistance5:
                                name = Title(TitleKind.LightResistance) + " 5";
                                break;
                            case QuestKindTitle.DarkResistance1:
                                name = Title(TitleKind.DarkResistance) + " 1";
                                break;
                            case QuestKindTitle.DarkResistance2:
                                name = Title(TitleKind.DarkResistance) + " 2";
                                break;
                            case QuestKindTitle.DarkResistance3:
                                name = Title(TitleKind.DarkResistance) + " 3";
                                break;
                            case QuestKindTitle.DarkResistance4:
                                name = Title(TitleKind.DarkResistance) + " 4";
                                break;
                            case QuestKindTitle.DarkResistance5:
                                name = Title(TitleKind.DarkResistance) + " 5";
                                break;
                            case QuestKindTitle.Survival1:
                                name = Title(TitleKind.Survival) + " 1";
                                if (!quest.isAccepted) condition = "Defeat any monsters while your HP is 20% or less : 500";
                                else condition = "Defeat any monsters while your HP is 20% or less : " + tDigit(Main.main.SR.survivalNumQuestTitle[(int)quest.heroKind]) + " / 500";
                                break;
                            case QuestKindTitle.Survival2:
                                name = Title(TitleKind.Survival) + " 2";
                                if (!quest.isAccepted) condition = "Defeat any monsters while your HP is 20% or less : 5000";
                                else condition = "Defeat any monsters while your HP is 20% or less : " + tDigit(Main.main.SR.survivalNumQuestTitle[(int)quest.heroKind]) + " / 5000";
                                break;
                            case QuestKindTitle.Survival3:
                                name = Title(TitleKind.Survival) + " 3";
                                if (!quest.isAccepted) condition = "Defeat any monsters while your HP is 20% or less : 50.00K";
                                else condition = "Defeat any monsters while your HP is 20% or less : " + tDigit(Main.main.SR.survivalNumQuestTitle[(int)quest.heroKind]) + " / 50.00K";
                                break;
                            case QuestKindTitle.Survival4:
                                name = Title(TitleKind.Survival) + " 4";
                                if (!quest.isAccepted) condition = "Defeat any monsters while your HP is 20% or less : 500.0K";
                                else condition = "Defeat any monsters while your HP is 20% or less : " + tDigit(Main.main.SR.survivalNumQuestTitle[(int)quest.heroKind]) + " / 500.0K";
                                break;
                            case QuestKindTitle.Cooperation1:
                                name = Title(TitleKind.Cooperation) + " 1";
                                condition = "Rebirth Tier 1 # : " + tDigit(GameController.game.rebirthCtrl.Rebirth(quest.heroKind, 0).rebirthNum) + " / 1";
                                break;
                            case QuestKindTitle.Cooperation2:
                                name = Title(TitleKind.Cooperation) + " 2";
                                condition = "Rebirth Tier 2 # : " + tDigit(GameController.game.rebirthCtrl.Rebirth(quest.heroKind, 1).rebirthNum) + " / 1";
                                break;
                            case QuestKindTitle.Cooperation3:
                                name = Title(TitleKind.Cooperation) + " 3";
                                condition = "Rebirth Tier 3 # : " + tDigit(GameController.game.rebirthCtrl.Rebirth(quest.heroKind, 2).rebirthNum) + " / 1";
                                break;
                            case QuestKindTitle.Quester1:
                                name = Title(TitleKind.Quester) + " 1";
                                break;
                            case QuestKindTitle.Quester2:
                                name = Title(TitleKind.Quester) + " 2";
                                break;
                            case QuestKindTitle.Quester3:
                                name = Title(TitleKind.Quester) + " 3";
                                break;
                            case QuestKindTitle.Quester4:
                                name = Title(TitleKind.Quester) + " 4";
                                break;
                            case QuestKindTitle.Quester5:
                                name = Title(TitleKind.Quester) + " 5";
                                break;
                            case QuestKindTitle.Quester6:
                                name = Title(TitleKind.Quester) + " 6";
                                break;
                            case QuestKindTitle.Quester7:
                                name = Title(TitleKind.Quester) + " 7";
                                break;
                            case QuestKindTitle.Quester8:
                                name = Title(TitleKind.Quester) + " 8";
                                break;
                            case QuestKindTitle.Quester9:
                                name = Title(TitleKind.Quester) + " 9";
                                break;
                            case QuestKindTitle.Quester10:
                                name = Title(TitleKind.Quester) + " 10";
                                break;

                        }
                        break;
                    case QuestKind.General:
                        switch (kindGeneral)
                        {
                            case QuestKindGeneral.CompleteArea0_0:
                                name = "Find My Brother 1";
                                client = "Nohn";
                                description = "Can you please help me find my brother? He's been missing for some time now. He's worrying us sick! I wonder if he's in the Slime Village. Please check Area 1.";
                                break;
                            case QuestKindGeneral.CompleteArea0_1:
                                name = "Find My Brother 2";
                                client = "Nohn";
                                description = "No? Not in Area 1, was he? We must continue on then, I suppose. When I get my hands on him... well, let's just try and find him! What about Area 2?";
                                break;
                            case QuestKindGeneral.CompleteArea0_2:
                                name = "Find My Brother 3";
                                client = "Nohn";
                                description = "Ah, there was some evidence of a struggle? It better not have been those slimes. Show them what you're made of. Clear Area 3 and get my brother back for me!";
                                break;
                            case QuestKindGeneral.CompleteArea0_3:
                                name = "Find My Brother 4";
                                client = "Nohn";
                                description = "Thank you so much for finding Hitan. I can't believe those slimes took him like that. Clear Area 4 and bring him back home. I will take care of him from there.";
                                break;
                            case QuestKindGeneral.DefeatNormalSlime1:
                                name = "Slime Infestation 1";
                                client = "Village Mayor";
                                description = "The Slimes have overrun the village. Please help our village clean up these pesky monsters.";
                                break;
                            case QuestKindGeneral.DefeatNormalSlime2:
                                name = "Slime Infestation 2";
                                client = "Village Mayor";
                                description = "You did such an amazing job before. It looks like there's still a lot more. Could you help us again and help push back the slimes?";
                                break;
                            case QuestKindGeneral.DefeatNormalSlime3:
                                name = "Slime Infestation 3";
                                client = "";
                                description = "Oh no, your attempts to help us seem to have really upset the slimes, and now there's an army of them knocking on our doors! Since you're kind of responsible for this mess, what would you say to helping us clean... that is, cleaning it up for us?";
                                break;
                            case QuestKindGeneral.BringOilOfSlime:
                                name = "The Slime Lover";
                                client = "";
                                description = "";
                                break;
                            case QuestKindGeneral.DefeatRedSlime:
                                name = "Red Slime Threat";
                                client = "Huck the Village Farmer";
                                description = "Please help! The red slimes have been attacking my farm. Scare them away for me! Help me, you are my only hope.";
                                break;
                            case QuestKindGeneral.DefeatRedMagicSlime:
                                name = "Magicslime Menace";
                                client = "Karosis";
                                description = "A hurried individual wearing glasses and a scruffy collection of robes, runs into you, scattering scrolls everywhere. After collecting their dropped possessions, they take a look at you before nodding, \"You look like you can handle yourself. These slimes, they've been stealing my magical research and it's severely hampered my progress. Would you mind helping me out some and dealing these thieves a telling blow? I... I'm willing to pay if that's what you need.\"";
                                break;
                            case QuestKindGeneral.CompleteDungeon0_0:
                                name = "Find My Husband 1";
                                client = "Wakana";
                                description = "Excuse me, do you have a moment? My husband has been missing for the past few days. My neighbors said they saw him go to the slime cave, but I'm not good with slime so I can't go look for him. Can you please go there for me?"
                                    + " After you clear <color=orange>Slime Village: Area 2</color> 100 times, you can try the Slime dungeon through <color=orange>Dungeon</color> tab in World Map. Keep in mind that you need <color=orange>Portal Orb</color> to enter Dungeons, which you can get from such as global quests and daily quests."
                                    + " Before trying a dungeon, the <color=orange>Simulation</color> button in World Map is so useful. It simulates each area and tell you whether you can clear it or not.";
                                break;
                            case QuestKindGeneral.CompleteDungeon0_1:
                                name = "Find My Husband 2";
                                client = "Wakana";
                                description = "Oh, you didn't find him, I'm so sad. Maybe he's in another cave... I don't know. Please, can you go and look for him again? I'm so worried about him that I can't do anything about it."; break;
                            case QuestKindGeneral.CompleteDungeon0_2:
                                name = "Find My Husband 3";
                                client = "Wakana";
                                description = "What! You saw him being kidnapped by slime in the depths of the cave!? Oh, my god! I'm sure there's a slime hideout nearby, that's where he'll be. Please, please and please go and find him!";
                                break;
                            case QuestKindGeneral.DefeatNormalMagicSlime:
                                name = "A Debt Left Unpaid";
                                client = "Young Witch";
                                description = "I was studying by the tree when some Magicslimes asked to borrow my quills. I should have known something was wrong when they swallowed them, but I trusted them anyway. Now I have nothing to write with, and my homework for the academy is late. If you could teach them a lesson for me I'd gladly return the favor by making a quilt out of their hats.";
                                break;
                            case QuestKindGeneral.DefeatGreenMagicSlime:
                                name = "Sending A Message";
                                client = "Claire The Witch";
                                description = "\"I was doing extra credit to make up for my missing homework when one of the Magicslimes tossed this paper at me.\" She hands you a balled - up piece of paper inviting you to a tea party in Magicslime City. \"I'm no expert, but I'd say this is definitely a trap. Especially considering my friends at school are always too nervous to invite me over. I need you to go for me and send those lying Magicslimes Back to where they came from.\"";
                                break;
                            case QuestKindGeneral.DefeatYellowBat:
                                name = "The Mine 1";
                                client = "Brick";
                                description = "As you are about to enter the cave a small giant sporting nothing but a pair of overalls, a mining hat, and a pickaxe. He scratches his head and addresses you with a worried expression. \"Excuse me. I need to get mining for the boss but the bats always give me the spooks. Can you lend me a hand and clear them out? I can't give you the ore, but you can have the stone.\"";
                                break;
                            case QuestKindGeneral.DefeatRedBat:
                                name = "The Mine 2";
                                client = "Mira";
                                description = "A small woman in thick steel plates and a miner helmet approaches you, \"Thank you for helping clear the bats. Brick has gotten a new sense of motivation recently. If you could go a bit deeper into the mine we could get some higher quality ore, and some rough stone you could keep.\"";
                                break;
                            case QuestKindGeneral.DefeatGreenBat:
                                name = "The Mine 3";
                                client = "Mira";
                                description = "With all those bats out of the way we saw some gems glittering deeper down in the mines. The boys and I understand that going down any deeper in the cave is dangerous. So we're willing to split our findings.";
                                break;
                            case QuestKindGeneral.DefeatPurpleBat:
                                name = "A Deep Dark Place";
                                client = "Brick";
                                description = "Brick approaches you as you return from the mine with a worried expression, \"It's good your back safe. Did you see the thing moving? It is like something was living in the void of the dark itself. I don't know why, but I don't have a good feeling about it. Could you go into that darkness and take care of it for us? I'm sure everyone would be happy being able to sleep a bit easier.\"";
                                break;
                            case QuestKindGeneral.DefeatBlueBat:
                                name = "A Strange Request";
                                client = "Miss Fizzle";
                                description = "As your leaving for Bat Cave a tomboy with thick goggles and a sharp grin eyes you down from atop a rock. Swirling a fiery potion in her hand as she calls to you, \"You. Yeah, you. I was wondering if you could bring me some guano from the bats I have a project I'm working on it's going to be great just you wait.\"";
                                break;
                            case QuestKindGeneral.BringToEnchantShard:
                                name = "The Big One";
                                client = "Miss Fizzle";
                                description = "Hey, thanks for the help earlier. My experiments are almost ready. All I need is a little Oil Of Slime to act as a base, A bit of Magical Cloth to wrap it in, and some Spider Silk for a long enough fuse. Then we can create a bomb so big it could blast away a million bats. For science of course.";
                                break;
                            case QuestKindGeneral.CompleteDungeon2_0:
                                name = "Haunted Mansion 1";
                                client = "Hansen";
                                description = "Did you hear the manor in the woods is haunted by an evil spirit? I need you to go check it out. People keep telling me there are noises coming from the walls, but I'm too busy to go look, myself. Very very busy.";
                                break;
                            case QuestKindGeneral.CompleteDungeon2_1:
                                name = "Haunted Mansion 2";
                                client = "Hansen";
                                description = "Eek it was spiders making all that noise. I mean oh spiders. Could you clear some of them out for me? I have a lot of work to do in the forest today.";
                                break;
                            case QuestKindGeneral.CaptureNormalSpider:
                                name = "Haunted Mansion 3";
                                client = "Hansen";
                                description = "The spiders just keep coming huh? There must be a queen. Sadly I'm sick and can't do it myself, but could you take some nets, and capture some spiders for me. I'll help you find the queen after.";
                                break;
                            case QuestKindGeneral.CompleteDungeon2_2:
                                name = "Haunted Mansion 4";
                                client = "A Strange Note";
                                description = "I've found the spider queen. At least one of the queens.  Sadly she has me captured. Please send help!";
                                break;
                            case QuestKindGeneral.CaptureYellowSlime:
                                name = "All Things Yellow!";
                                client = "Yellow Circle";
                                description = "An individual dressed from head to toe in Yellow robes stands just outside the slime village, bouncing on their heels in excitement as they crane their neck to get a better view of the town. As you pass by, they frantically wave you down, beckoning you over. \"Please, I was passing by this village and I saw a flash of yellow run between some buildings but I couldn't keep up with it. Can you try to catch it for me? Please oh please oh please?!\" Well, you can't say no to that face. Off you go then!";
                                break;
                            case QuestKindGeneral.CaptureNormalFairy:
                                name = "King Of The Fairies 1";
                                client = "A Lost Boy";
                                description = "A young boy, sniveling and covered in bruises, approaches you, \"I just wanted to play in the garden when I tripped on a fairy. Then they all bullied me out of the garden. Can you make sure it's safe to go back?\"";
                                break;
                            case QuestKindGeneral.CaptureBlueFairy:
                                name = "King Of The Fairies 2";
                                client = "Perky Peter";
                                description = "Young adventurers like you always come by and make things worse. Don't you know every time you slay a fairy two more comes back. You need to capture them if you really want them to stop terrorizing you. I'll do my best to help as well.";
                                break;
                            case QuestKindGeneral.CaptureYellowFairy:
                                name = "King Of The Fairies 3";
                                client = "Perky Peter";
                                description = "Now if there is one thing fairies respect. It's a stronger fairy. All we have to do is make the strongest fairy in the history of ever. Just get a strong fairy, and we can take it to the gardens to clear things up.";
                                break;
                            case QuestKindGeneral.CaptureRedFairy:
                                name = "Queen Of The Fairies";
                                client = "Perky Peter";
                                description = "You did a pretty good job helping that kid, I wouldn't expect anything less. Although this is where we part ways. I want you to take good care of that fairy I gave you.";
                                break;
                            case QuestKindGeneral.CaptureGreenFairy:
                                name = "Good Luck Charms";
                                client = "Panemu";
                                description = "You know some people think fairies' wings bring good luck? We could sure use some good karma right now. If you help me make some charms we can spread them around. Just in case there is any negative energy still in the town.";
                                break;
                        }
                        break;
                }
                break;
        }
        return (name, client, description, condition, reward, unlock);
    }

    public string Material(MaterialKind kind)
    {
        string tempString = kind.ToString();//未
        switch (language)
        {
            case Language.Japanese:

                break;
            default:
                switch (kind)
                {
                    case MaterialKind.MonsterFluid:
                        tempString = "Monster Fluid";
                        break;
                    case MaterialKind.FlameShard:
                        tempString = "Flame Shard";
                        break;
                    case MaterialKind.FrostShard:
                        tempString = "Frost Shard";
                        break;
                    case MaterialKind.LightningShard:
                        tempString = "Lightning Shard";
                        break;
                    case MaterialKind.NatureShard:
                        tempString = "Nature Shard";
                        break;
                    case MaterialKind.PoisonShard:
                        tempString = "Poison Shard";
                        break;
                    case MaterialKind.BlackPearl:
                        tempString = "Black Pearl";
                        break;
                    case MaterialKind.OilOfSlime:
                        tempString = "Oil of Slime";
                        break;
                    case MaterialKind.EnchantedCloth:
                        tempString = "Magical Cloth";
                        break;
                    case MaterialKind.SpiderSilk:
                        tempString = "Spider Silk";
                        break;
                    case MaterialKind.BatWing:
                        tempString = "Bat Wing";
                        break;
                    case MaterialKind.FairyDust:
                        tempString = "Fairy Dust";
                        break;
                    case MaterialKind.FoxTail:
                        tempString = "Fox Tail";
                        break;
                    case MaterialKind.FishScales:
                        tempString = "Fish Scales";
                        break;
                    case MaterialKind.CarvedBranch:
                        tempString = "Carved Branch";
                        break;
                    case MaterialKind.ThickFur:
                        tempString = "Thick Fur";
                        break;
                    case MaterialKind.UnicornHorn:
                        tempString = "Unicorn Horn";
                        break;
                    case MaterialKind.SlimeBall:
                        return "Slime Ball";
                    case MaterialKind.ManaSeed:
                        return "Mana Seed";
                    case MaterialKind.UnmeltingIce:
                        return "Unmelting Ice";
                    case MaterialKind.EternalFlame:
                        return "Eternal Flame";
                    case MaterialKind.AncientBattery:
                        return "Ancient Battery";
                    case MaterialKind.Ectoplasm:
                        return "Ectoplasm";
                    case MaterialKind.Stardust:
                        return "Stardust";
                    case MaterialKind.VoidEgg:
                        return "Void Egg";
                    case MaterialKind.EnchantedShard:
                        return "Enchanted Shard";
                }
                break;
        }
        return tempString;
    }
    //Setting
    public string Toggle(ToggleKind kind)
    {
        //string tempString = kind.ToString();//未
        switch (language)
        {
            case Language.Japanese:
                switch (kind)
                {
                    case ToggleKind.BGM:
                        return "BGM";
                    case ToggleKind.SFX:
                        return "SFX";
                    case ToggleKind.DisableDamageText:
                        return "ダメージを非表示";
                    case ToggleKind.FastResult:
                        return "戦闘リザルトの表示時間を短くする";
                    case ToggleKind.ShowDPS:
                        return "セットされたスキルのDPSを表示する";
                    case ToggleKind.ShowDetailStats:
                        return "HP, MP, EXPの情報を表示する";
                    case ToggleKind.HideCompletedQuest:
                        return "完了済のクエストを非表示にする";
                    case ToggleKind.PerformanceMode:
                        return "軽量化モード";
                    case ToggleKind.AutoDisassembleExcludeEnchanted:
                        return "エンチャント付きは除外する";
                    case ToggleKind.DisableCombatRange:
                        return "スキルの射程距離を非表示にする";
                    case ToggleKind.DarkenBattlefield:
                        return "バトルエリアを暗くする";
                    case ToggleKind.BuyOneGuildAbility:
                        break;
                    case ToggleKind.BuyOneRebirthUpgrade:
                        break;
                    case ToggleKind.SkillLessMPAvailable:
                        break;
                    case ToggleKind.BuyOneWorldAscensionUpgrade:
                        break;
                    case ToggleKind.ShowEquipmentStar:
                        break;
                }
                break;
            default:
                switch (kind)
                {
                    case ToggleKind.BGM:
                        return "BGM";
                    case ToggleKind.SFX:
                        return "SFX";
                    case ToggleKind.DisableDamageText:
                        return "Disables Damage Text";
                    case ToggleKind.DisableGoldLog:
                        return "Gold Gains";
                    case ToggleKind.FastResult:
                        return "Shortens Battle Result";
                    case ToggleKind.ShowDPS:
                        return "Show DPS of Skillset";
                    case ToggleKind.ShowDetailStats:
                        return "Show HP, MP and EXP Stats";
                    case ToggleKind.HideCompletedQuest:
                        return "Hide Completed Quests";
                    case ToggleKind.PerformanceMode:
                        return "Performance Mode";
                    case ToggleKind.AutoDisassembleExcludeEnchanted:
                        return "Exclude Enchanted EQ";
                    case ToggleKind.DisableCombatRange:
                        return "Disable the Display of Skill Range";
                    case ToggleKind.DarkenBattlefield:
                        return "Darken the Battlefield";
                    case ToggleKind.BuyOneGuildAbility:
                        return "Override Multiplier [ x 1 ]";
                    case ToggleKind.BuyOneRebirthUpgrade:
                        return "Override Multiplier [ x 1 ]";
                    case ToggleKind.SkillLessMPAvailable:
                        return "Skill Tenacity";
                    case ToggleKind.BuyOneWorldAscensionUpgrade:
                        return "Override Multiplier [ x 1 ]";
                    case ToggleKind.ShowEquipmentStar:
                        return "Show Equipment Mastery Stars in inventory";
                    case ToggleKind.DisableParticle:
                        return "Disable Particles";
                    case ToggleKind.DisableAttackEffect:
                        return "Disable Attack Effects";
                    case ToggleKind.DisableAnyLog:
                        return "Disable All Logs";
                    case ToggleKind.BuyOneAreaPrestige1:
                        return "Override [ x 1 ]";//Multiplier 
                    case ToggleKind.BuyOneAreaPrestige2:
                        return "Override [ x 1 ]";//Multiplier 
                    case ToggleKind.SwarmChaser:
                        if (game.epicStoreCtrl.Item(EpicStoreKind.SwarmChaser).IsPurchased())
                            return "Swarm Chaser";
                        else return "Swarm Chaser ( <sprite=\"locks\" index=0> Epic Store )";
                    case ToggleKind.AdvancedAutoBuyTrap:
                        if (game.epicStoreCtrl.Item(EpicStoreKind.LimitSlotAutoBuyNet).IsPurchased())
                            return "Advanced Auto-Buy Traps";
                        else return "Advanced Auto-Buy Traps ( <sprite=\"locks\" index=0> Epic Store )";
                    case ToggleKind.DisableNotificationAchievement:
                        return "Disable notifications for Achievements";
                    case ToggleKind.DisableNotificationQuest:
                        return "Disable notifications for Quest";
                    case ToggleKind.DisableNotificationLab:
                        return "Disable notifications for Lab";
                    case ToggleKind.DisableExpLog:
                        return "EXP Gains";
                    case ToggleKind.DisableResourceLog:
                        return "Resource Gains";
                    case ToggleKind.DisableMaterialLog:
                        return "Material Gains";
                    case ToggleKind.DisableGuildLog:
                        return "Guild EXP/Level";
                    case ToggleKind.DisableCaptureLog:
                        return "Capture";
                    case ToggleKind.DisableSwarmResult:
                        return "Disable Swarm Result Popup";
                    case ToggleKind.DisableNotificationExpedition:
                        return "Disable notifications for Expedition";
                    default:
                        break;
                }
                break;
        }
        return kind.ToString();
    }

    //Rebirth
    public (string name, string effect, string nextEffect) Rebirth(RebirthUpgrade upgrade)
    {
        string tempName = "";
        string tempEffect = "";
        string tempNextEffect = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (upgrade.kind)
                {
                    case RebirthUpgradeKind.ExpGain:
                        tempName = "EXP Multiplier";
                        tempEffect = "Multiply EXP Gain by " + percent(1 + upgrade.effectValue, 0);
                        tempNextEffect = "Multiply EXP Gain by " + percent(1 + upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQRequirement:
                        tempName = "Equipment Challenger";
                        tempEffect = "Enable to equip + " + tDigit(upgrade.effectValue) + " Hero Level Equipment";
                        tempNextEffect = "Enable to equip + " + tDigit(upgrade.nextEffectValue) + " Hero Level Equipment";
                        break;
                    case RebirthUpgradeKind.QuestAcceptableNum:
                        tempName = "Multitasker";
                        tempEffect = "Accepted Quest Limit + " + tDigit(upgrade.effectValue);
                        tempNextEffect = "Accepted Quest Limit + " + tDigit(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.BasicAtk:
                        tempName = "Basic ATK";
                        tempEffect = "ATK + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "ATK + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.BasicMAtk:
                        tempName = "Basic MATK";
                        tempEffect = "MATK + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "MATK + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.BasicHp:
                        tempName = "Basic HP";
                        tempEffect = "HP + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "HP + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.BasicDef:
                        tempName = "Basic DEF";
                        tempEffect = "DEF + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "DEF + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.BasicMDef:
                        tempName = "Basic MDEF";
                        tempEffect = "MDEF + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "MDEF + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.BasicMp:
                        tempName = "Basic MP";
                        tempEffect = "MP + " + tDigit(upgrade.effectValue, 2) + " per Hero Level";
                        tempNextEffect = "MP + " + tDigit(upgrade.nextEffectValue, 2) + " per Hero Level";
                        break;
                    case RebirthUpgradeKind.StoneGain:
                        tempName = "Stone Gain Multiplier";
                        tempEffect = "Stone Gain + " + percent(upgrade.effectValue);
                        tempNextEffect = "Stone Gain + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.CrystalGain:
                        tempName = "Crystal Gain Multiplier";
                        tempEffect = "Crystal Gain + " + percent(upgrade.effectValue);
                        tempNextEffect = "Crystal Gain + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.LeafGain:
                        tempName = "Leaf Gain Multiplier";
                        tempEffect = "Leaf Gain + " + percent(upgrade.effectValue);
                        tempNextEffect = "Leaf Gain + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.StoneGoldCap:
                        tempName = "Stone Gold Cap Multiplier";
                        tempEffect = "Stone Gold Cap Upgrade's effect + " + percent(upgrade.effectValue);
                        tempNextEffect = "Stone Gold Cap Upgrade's effect + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.CrystalGoldCap:
                        tempName = "Crystal Gold Cap Multiplier";
                        tempEffect = "Crystal Gold Cap Upgrade's effect + " + percent(upgrade.effectValue);
                        tempNextEffect = "Crystal Gold Cap Upgrade's effect + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.LeafGoldCap:
                        tempName = "Leaf Gold Cap Multiplier";
                        tempEffect = "Leaf Gold Cap Upgrade's effect + " + percent(upgrade.effectValue);
                        tempNextEffect = "Leaf Gold Cap Upgrade's effect + " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.SkillProfGain:
                        tempName = "Skill Proficiency Gain Multiplier";
                        tempEffect = "Multiply Skill Proficiency Gain by " + percent(1 + upgrade.effectValue, 0);
                        tempNextEffect = "Multiply Skill Proficiency Gain by " + percent(1 + upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.SkillRankCostReduction:
                        tempName = "Skill Rank Cost Reduction";
                        tempEffect = "Reduce skill's Rank cost to " + percent(upgrade.effectValue);
                        tempNextEffect = "Reduce skill's Rank cost to " + percent(upgrade.nextEffectValue);
                        break;
                    case RebirthUpgradeKind.ClassSkillSlot:
                        tempName = "Class Skill Slot";
                        tempEffect = "Class Skill Slot + " + tDigit(upgrade.effectValue, 0);
                        tempNextEffect = "Class Skill Slot + " + tDigit(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.ShareSkillPassive:
                        tempName = "Sharing Skill Passive Effect";
                        tempEffect = "Share " + percent(upgrade.effectValue) + " of Skill Passive Effect with the other heroes";
                        tempNextEffect = "Share " + percent(upgrade.nextEffectValue) + " of Skill Passive Effect with the other heroes";
                        break;
                    case RebirthUpgradeKind.T1ExpGainBoost:
                        tempName = "Tier 1 Booster [EXP]";
                        tempEffect = "Tier 1 EXP Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 EXP Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1RebirthPointGainBoost:
                        tempName = "Tier 1 Booster [Rebirth Point]";
                        tempEffect = "Tier 1 Rebirth Point Gain + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Rebirth Point Gain + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicAtkBoost:
                        tempName = "Tier 1 Booster [Basic ATK]";
                        tempEffect = "Tier 1 Basic ATK's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic ATK's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicMAtkBoost:
                        tempName = "Tier 1 Booster [Basic MATK]";
                        tempEffect = "Tier 1 Basic MATK's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic MATK's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicHpBoost:
                        tempName = "Tier 1 Booster [Basic HP]";
                        tempEffect = "Tier 1 Basic HP's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic HP's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicDefBoost:
                        tempName = "Tier 1 Booster [Basic DEF]";
                        tempEffect = "Tier 1 Basic DEF's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic DEF's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicMDefBoost:
                        tempName = "Tier 1 Booster [Basic MDEF]";
                        tempEffect = "Tier 1 Basic MDEF's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic MDEF's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1BasicMpBoost:
                        tempName = "Tier 1 Booster [Basic MP]";
                        tempEffect = "Tier 1 Basic MP's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Basic MP's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1StoneGainBoost:
                        tempName = "Tier 1 Booster [Stone Gain]";
                        tempEffect = "Tier 1 Stone Gain Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Stone Gain Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1CrystalGainBoost:
                        tempName = "Tier 1 Booster [Crystal Gain]";
                        tempEffect = "Tier 1 Crystal Gain Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Crystal Gain Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1LeafGainBoost:
                        tempName = "Tier 1 Booster [Leaf Gain]";
                        tempEffect = "Tier 1 Leaf Gain Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Leaf Gain Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1StoneGoldCapBoost:
                        tempName = "Tier 1 Booster [Stone Gold Cap]";
                        tempEffect = "Tier 1 Stone Gold Cap Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Stone Gold Cap Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1CrystalGoldCapBoost:
                        tempName = "Tier 1 Booster [Crystal Gold Cap]";
                        tempEffect = "Tier 1 Crystal Gold Cap Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Crystal Gold Cap Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T1LeafGoldCapBoost:
                        tempName = "Tier 1 Booster [Leaf Gold Cap]";
                        tempEffect = "Tier 1 Leaf Gold Cap Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 1 Leaf Gold Cap Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQLevelCap:
                        tempName = "Equipment Level Cap";
                        tempEffect = "Increase Equipment's level cap by " + tDigit(upgrade.effectValue, 0);
                        tempNextEffect = "Increase Equipment's level cap by " + tDigit(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQProfGain:
                        tempName = "Equipment Proficiency Gain Multiplier";
                        tempEffect = "Multiply Equipment Proficiency Gain by " + percent(1 + upgrade.effectValue, 0);
                        tempNextEffect = "Multiply Equipment Proficiency Gain by " + percent(1 + upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQWeaponSlot:
                        tempName = "Equipment Weapon Slot";
                        tempEffect = "Equipment Weapon Slot + " + tDigit(upgrade.effectValue, 0);
                        tempNextEffect = "Equipment Weapon Slot + " + tDigit(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQArmorSlot:
                        tempName = "Equipment Armor Slot";
                        tempEffect = "Equipment Armor Slot + " + tDigit(upgrade.effectValue, 0);
                        tempNextEffect = "Equipment Armor Slot + " + tDigit(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.EQJewelrySlot:
                        tempName = "Equipment Jewelry Slot";
                        tempEffect = "Equipment Jewelry Slot + " + tDigit(upgrade.effectValue, 0);
                        tempNextEffect = "Equipment Jewelry Slot + " + tDigit(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2ExpGainBoost:
                        tempName = "Tier 2 Booster [EXP]";
                        tempEffect = "Tier 2 EXP Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 EXP Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2RebirthPointGainBoost:
                        tempName = "Tier 2 Booster [Rebirth Point]";
                        tempEffect = "Tier 2 Rebirth Point Gain + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Rebirth Point Gain + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicAtkBoost:
                        tempName = "Tier 2 Booster [Basic ATK]";
                        tempEffect = "Tier 2 Basic ATK Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic ATK Booster's effect  + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicMAtkBoost:
                        tempName = "Tier 2 Booster [Basic MATK]";
                        tempEffect = "Tier 2 Basic MATK Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic MATK Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicHpBoost:
                        tempName = "Tier 2 Booster [Basic HP]";
                        tempEffect = "Tier 2 Basic HP Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic HP Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicDefBoost:
                        tempName = "Tier 2 Booster [Basic DEF]";
                        tempEffect = "Tier 2 Basic DEF Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic DEF Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicMDefBoost:
                        tempName = "Tier 2 Booster [Basic MDEF]";
                        tempEffect = "Tier 2 Basic MDEF Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic MDEF Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2BasicMpBoost:
                        tempName = "Tier 2 Booster [Basic MP]";
                        tempEffect = "Tier 2 Basic MP Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Basic MP Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2StoneGainBoost:
                        tempName = "Tier 2 Booster [Stone Gain]";
                        tempEffect = "Tier 2 Stone Gain Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Stone Gain Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2CrystalGainBoost:
                        tempName = "Tier 2 Booster [Crystal Gain]";
                        tempEffect = "Tier 2 Crystal Gain Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Crystal Gain Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2LeafGainBoost:
                        tempName = "Tier 2 Booster [Leaf Gain]";
                        tempEffect = "Tier 2 Leaf Gain Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Leaf Gain Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2StoneGoldCapBoost:
                        tempName = "Tier 2 Booster [Stone Gold Cap]";
                        tempEffect = "Tier 2 Stone Gold Cap Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Stone Gold Cap Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2CrystalGoldCapBoost:
                        tempName = "Tier 2 Booster [Crystal Gold Cap]";
                        tempEffect = "Tier 2 Crystal Gold Cap Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Crystal Gold Cap Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2LeafGoldCapBoost:
                        tempName = "Tier 2 Booster [Leaf Gold Cap]";
                        tempEffect = "Tier 2 Leaf Gold Cap Booster's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Leaf Gold Cap Booster's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                    case RebirthUpgradeKind.T2SkillProfGainBoost:
                        tempName = "Tier 2 Booster [Skill Proficiency Gain]";
                        tempEffect = "Tier 2 Skill Proficiency Gain Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        tempNextEffect = "Tier 2 Skill Proficiency Gain Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        break;
                        //case RebirthUpgradeKind.ExpGain2:
                        //    tempName = "Tier 1 Booster [EXP]";
                        //    tempEffect = "Tier 1 EXP Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        //    tempNextEffect = "Tier 1 EXP Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        //    break;
                        //case RebirthUpgradeKind.SkillProfGain2:
                        //    tempName = "Tier 1 Booster [Skill Proficiency]";
                        //    tempEffect = "Tier 1 Skill Proficiency Multiplier's effect + " + percent(upgrade.effectValue, 0);
                        //    tempNextEffect = "Tier 1 Skill Proficiency Multiplier's effect + " + percent(upgrade.nextEffectValue, 0);
                        //    break;
                        //case RebirthUpgradeKind.Rebirth1PointGain:
                        //    tempName = "Tier 1 Booster [Point]";
                        //    tempEffect = "Tier 1 Rebirth Point Gain + " + percent(upgrade.effectValue, 0);
                        //    tempNextEffect = "Tier 1 Rebirth Point Gain + " + percent(upgrade.nextEffectValue, 0);
                        //    break;
                        //case RebirthUpgradeKind.SkillSlot:
                        //    tempName = "Class Skill Slot";
                        //    tempEffect = "Class Skill Slot + " + tDigit(upgrade.effectValue, 0);
                        //    tempNextEffect = "Class Skill Slot + " + tDigit(upgrade.nextEffectValue, 0);
                        //    break;
                        //case RebirthUpgradeKind.PhysCritChance:
                        //    tempName = "Physical Critical Multiplier";
                        //    tempEffect = "Multiply Physical Critical Chance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Physical Critical Chance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.MagCritChance:
                        //    tempName = "Magical Critical Multiplier";
                        //    tempEffect = "Multiply Magical Critical Chance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Magical Critical Chance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.GoldGain:
                        //    tempName = "Gold Gain Multiplier";
                        //    tempEffect = "Multiply Gold Gain by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Gold Gain by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.MaterialGain:
                        //    tempName = "Material Drop Gain";
                        //    tempEffect = "Material Gain from loot + " + tDigit(upgrade.effectValue);
                        //    tempNextEffect = "Material Gain from loot + " + tDigit(upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.MysteriousWaterGain:
                        //    tempName = "Mysterious Water Multiplier";
                        //    tempEffect = "Multiply Mysterious Water Gain by " + percent(1 + upgrade.effectValue);
                        //    tempNextEffect = "Multiply Mysterious Water Gain by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.Pet:
                        //    //未
                        //    tempName = "Pet Multiplier";
                        //    //tempEffect = "Multiply Mysterious Water Gain by " + tDigit(1 + upgrade.effectValue);
                        //    //tempNextEffect = "Multiplier Mysterious Water Gain by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.FireRes:
                        //    tempName = "Fire Resistance Multiplier";
                        //    tempEffect = "Multiply Fire Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Fire Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.IceRes:
                        //    tempName = "Ice Resistance Multiplier";
                        //    tempEffect = "Multiply Ice Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Ice Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.ThunderRes:
                        //    tempName = "Thunder Resistance Multiplier";
                        //    tempEffect = "Multiply Thunder Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Thunder Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.LightRes:
                        //    tempName = "Light Resistance Multiplier";
                        //    tempEffect = "Multiply Light Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Light Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.DarkRes:
                        //    tempName = "Dark Resistance Multiplier";
                        //    tempEffect = "Multiply Dark Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Dark Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        //case RebirthUpgradeKind.DebuffRes:
                        //    tempName = "Debuff Resistance Multiplier";
                        //    tempEffect = "Multiply Debuff Resistance by " + percent(1 + upgrade.effectValue, 3);
                        //    tempNextEffect = "Multiply Debuff Resistance by " + percent(1 + upgrade.nextEffectValue, 3);
                        //    break;
                        ////Tier3
                        //case RebirthUpgradeKind.Rebirth2PointGain:
                        //    break;
                        ////case RebirthUpgradeKind.NewSkill:
                        ////    break;
                        //case RebirthUpgradeKind.SkillLevelCap:
                        //    break;
                        //case RebirthUpgradeKind.EQProf:
                        //    break;
                        //case RebirthUpgradeKind.ExpGain3:
                        //    break;
                        //case RebirthUpgradeKind.SkillProfGain3:
                        //    break;
                }
                break;
        }
        if (upgrade.level.IsMaxed()) tempNextEffect = "Level Maxed";
        else tempNextEffect += " ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        return (tempName, tempEffect, tempNextEffect);
    }
    public string RebirthPointGain(Rebirth rebirth)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                tempString = "You will gain <color=green>" + tDigit(rebirth.RebirthPointGain()) + " Rebirth Point</color> of " + " Tier " + tDigit(rebirth.tier + 1) + " when you rebirth right now!";
                for (int i = 0; i < rebirth.rebirthPointKinds.Count; i++)
                {
                    int count = i;
                    tempString += "\n- From " + RebirthPointGainKind(rebirth.rebirthPointKinds[count]) + " : " + tDigit(rebirth.RebirthPointGain(rebirth.rebirthPointKinds[count]), 1) + "</color>";
                }
                break;
        }
        return tempString;
    }
    public string RebirthPointGainKind(RebirthPointKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case RebirthPointKind.HeroLevel:
                        return "Hero Level";
                    case RebirthPointKind.Quest:
                        return "General Quest";
                    case RebirthPointKind.Move:
                        return "Walked Distance";
                    case RebirthPointKind.SkillLevel:
                        return "Skill Level";
                    case RebirthPointKind.EQLevel:
                        return "Equipment Level";
                }
                break;
        }
        return tempString;
    }
    public string RebirthInfo(Rebirth rebirth)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                tempString += Hero(rebirth.heroKind);
                tempString += "\nTier " + tDigit(1 + rebirth.tier) + " Rebirth # <color=green>" + tDigit(rebirth.rebirthNum) + "</color>";
                tempString += "\nMax Hero Level Reached : <color=green>Lv " + tDigit(rebirth.maxHeroLevel) + "</color>";
                switch (rebirth.tier)
                {
                    case 0://Tier1
                        tempString += "\nAdditional Ability Point : <color=green>" + tDigit(rebirth.additionalAbilityPoint.Value()) + "</color>";
                        break;
                    case 1://Tier2
                        tempString += "\nAdditional Ability Point : <color=green>Gain " + tDigit(rebirth.additionalAbilityPoint.Value()) + " every 25th Hero Level</color>";
                        tempString += "\nTier 1 Rebirth Bonus Effect : <color=green>+ " + percent(rebirth.bonusEffectFactorOneDownTier.Value()) + "</color>";
                        break;
                    case 2://Tier3
                        tempString += "\nInitial Skill Level : <color=green>" + tDigit(rebirth.additionalAbilityPoint.Value()) + "</color>";
                        tempString += "\nTier 2 Rebirth Bonus Effect : <color=green>+ " + percent(rebirth.bonusEffectFactorOneDownTier.Value()) + "</color>";
                        break;
                }
                break;
        }
        return tempString;
    }

    //ChallengeHandicap
    public string HandicapString(ChallengeHandicapKind kind)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                break;
            default:
                switch (kind)
                {
                    case ChallengeHandicapKind.OnlyWeapon:
                        return "Only Weapon Equipment is available";
                    case ChallengeHandicapKind.OnlyArmor:
                        return "Only Armor Equipment is available";
                    case ChallengeHandicapKind.OnlyJewelry:
                        return "Only Jewelry Equipment is available";
                    case ChallengeHandicapKind.Only1EQforAllPart:
                        return "Only one Equipment for each type is available";
                    case ChallengeHandicapKind.Only1Weapon:
                        return "Only one Weapon Equipment is available";
                    case ChallengeHandicapKind.Only1Armor:
                        return "Only one Armor Equipment is available";
                    case ChallengeHandicapKind.Only1Jewelry:
                        return "Only one Jewelry Equipment is available";
                    case ChallengeHandicapKind.NoEQ:
                        return "No Equipment is available";
                    case ChallengeHandicapKind.OnlyClassSkill:
                        return "Only Class Skill Slots are available";
                    case ChallengeHandicapKind.OnlyBaseAndGlobal:
                        return "Only Base and Global Skill Slots are available";
                    case ChallengeHandicapKind.Only2ClassSkillAnd1Global:
                        return "Only two Class Skill Slots and one Global Skill Slot are available";
                    case ChallengeHandicapKind.Only2ClassSkill:
                        return "Only two Class Skill Slots are available";
                    case ChallengeHandicapKind.OnlyBaseSkill:
                        return "Only Base Attack Skill is available";
                    case ChallengeHandicapKind.DamageLimit:
                        return "Dealt damage of all attacks is limited to 1";
                    case ChallengeHandicapKind.DisableManualMove:
                        return "Manula Move is disabled";
                }
                break;
        }
        return tempString;
    }

    //Expedition
    //public string Expedition(ExpeditionKind kind)
    //{
    //    switch (language)
    //    {
    //        case Language.Japanese:
    //            break;
    //        default:
    //            switch (kind)
    //            {
    //                case ExpeditionKind.Brick:
    //                    return "Manufacturing Bricks";
    //                case ExpeditionKind.Log:
    //                    return "Logging Trees";
    //                case ExpeditionKind.Shard:
    //                    return "Gathering Shards";
    //            }
    //            break;
    //    }
    //    return kind.ToString();
    //}


    public string Template(int id)
    {
        string tempString = "";
        switch (language)
        {
            case Language.Japanese:
                switch (id)
                {
                    default:
                        break;
                }
                break;
            default:
                switch (id)
                {
                    default:
                        break;
                }
                break;
        }
        return tempString;
    }
}
