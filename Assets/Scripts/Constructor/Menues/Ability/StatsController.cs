using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;
using static MultiplierType;
using System;

public partial class Save
{
    //統計（WAでリセット・WAのMilestone用）
    public double[] totalMovedDistance;//[heroKind]
    public double totalMovedDistancePet;
    //リセットしない
    public double[] movedDistance;//[heroKind]
    public double movedDistancePet;
    public double[] maxHeroLevelReached;//[heroKind]

    //Element
    public double[] physicalTriggeredNum;//[HeroKind]
    public double[] fireTriggeredNum;//[HeroKind]
    public double[] iceTriggeredNum;//[HeroKind]
    public double[] thunderTriggeredNum;//[HeroKind]
    public double[] lightTriggeredNum;//[HeroKind]
    public double[] darkTriggeredNum;//[HeroKind]
}
public partial class SaveR
{
    //統計
    public double[] movedDistance;//[heroKind]
    public double movedDistancePet;

    public long[] heroLevel;//[hero]
    public double[] heroExp;//[hero]
    public double[] abilityPoints;//[hero]
    public long[] abilityPointsVitality;//[hero]
    public long[] abilityPointsStrength;//[hero]
    public long[] abilityPointsIntelligence;//[hero]
    public long[] abilityPointsAgility;//[hero]
    public long[] abilityPointsLuck;//[hero]

    public int[] combatRangeId;//[hero]
}
public class StatsController
{
    public StatsController()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i] = new HeroStats((HeroKind)i);
        }
        for (int i = 0; i < globalStats.Length; i++)
        {
            globalStats[i] = new Multiplier();
        }
        globalStats[(int)GlobalStats.GoldGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        globalSkillSlotNum = new Multiplier(true);
        //critDamage = new Multiplier();
    }

    //ヒーロー共有のStats
    public Multiplier[] globalStats = new Multiplier[Enum.GetNames(typeof(GlobalStats)).Length];
    public Multiplier GoldGain()
    {
        return globalStats[(int)GlobalStats.GoldGain];
    }
    public Multiplier ResourceGain(ResourceKind kind)
    {
        switch (kind)
        {
            case ResourceKind.Stone:
                return globalStats[(int)GlobalStats.StoneGain];
            case ResourceKind.Crystal:
                return globalStats[(int)GlobalStats.CrystalGain];
            case ResourceKind.Leaf:
                return globalStats[(int)GlobalStats.LeafGain];
        }
        return globalStats[(int)GlobalStats.StoneGain];
    }
    public Multiplier globalSkillSlotNum;
    //public Multiplier critDamage;
    //以下はヒーロー個別のStats
    HeroKind currentHero { get => main.S.currentHero; set => main.S.currentHero = value; }
    public HeroStats[] heroes = new HeroStats[Enum.GetNames(typeof(HeroKind)).Length];

    //以下は外部からアクセスする用
    public Multiplier BasicStats(HeroKind heroKind, BasicStatsKind kind)
    {
        return heroes[(int)heroKind].basicStats[(int)kind];
    }
    public void SetEffect(BasicStatsKind kind, MultiplierInfo info)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            BasicStats((HeroKind)i, kind).RegisterMultiplier(info);
        }
    }
    public Multiplier BasicStatsPerLevel(HeroKind heroKind, BasicStatsKind kind)
    {
        return heroes[(int)heroKind].basicStatsPerLevel[(int)kind];
    }
    public void SetEffectBasicStatsPerLevel(BasicStatsKind kind, MultiplierInfo info)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            BasicStatsPerLevel((HeroKind)i, kind).RegisterMultiplier(info);
        }
    }

    //Stats
    public Multiplier HeroStats(HeroKind heroKind, Stats kind)
    {
        return heroes[(int)heroKind].stats[(int)kind];
    }
    public void SetEffect(Stats kind, MultiplierInfo info)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            HeroStats((HeroKind)i, kind).RegisterMultiplier(info);
        }
    }
    public void SetEffectResourceGain(MultiplierInfo info)
    {
        for (int i = 0; i < Enum.GetNames(typeof(ResourceKind)).Length; i++)
        {
            int count = i;
            ResourceGain((ResourceKind)count).RegisterMultiplier(info);
        }
    }

    //OptionEffect付加確率
    public Multiplier OptionEffectChance(HeroKind heroKind, int optionId)
    {
        return heroes[(int)heroKind].optionEffectChance[optionId];
    }

    //Element
    public Multiplier ElementResistance(HeroKind heroKind, Element kind)
    {
        switch (kind)
        {
            case Element.Physical:
                return null;
            case Element.Fire:
                return heroes[(int)heroKind].stats[(int)Stats.FireRes];
            case Element.Ice:
                return heroes[(int)heroKind].stats[(int)Stats.IceRes];
            case Element.Thunder:
                return heroes[(int)heroKind].stats[(int)Stats.ThunderRes];
            case Element.Light:
                return heroes[(int)heroKind].stats[(int)Stats.LightRes];
            case Element.Dark:
                return heroes[(int)heroKind].stats[(int)Stats.DarkRes];
        }
        return null;
    }
    
    public Multiplier HpRegenerate(HeroKind heroKind)
    {
        return heroes[(int)heroKind].hpRegenerate;
    }
    public Multiplier MpRegenerate(HeroKind heroKind)
    {
        return heroes[(int)heroKind].mpRegenerate;
    }
    public Multiplier SkillSlotNum(HeroKind heroKind)
    {
        return heroes[(int)heroKind].skillSlotNum;
    }
    public Multiplier LevelForEquipment(HeroKind heroKind)
    {
        return heroes[(int)heroKind].levelForEquipment;
    }

    public Multiplier MonsterDamage(HeroKind heroKind, MonsterSpecies species)
    {
        return heroes[(int)heroKind].monsterDamages[(int)species];
    }
    public Multiplier ElementDamage(HeroKind heroKind, Element element)
    {
        return heroes[(int)heroKind].elementDamages[(int)element];
    }
    public Multiplier ElementAbsorption(HeroKind heroKind, Element element)
    {
        return heroes[(int)heroKind].elementAbsoptions[(int)element];
    }
    public Multiplier ElementInvalid(HeroKind heroKind, Element element)
    {
        return heroes[(int)heroKind].elementInvalids[(int)element];
    }
    public Multiplier MonsterDistinguishMaxLevel(HeroKind heroKind)
    {
        return heroes[(int)heroKind].monsterDistinguishMaxLevel;
    }
    public Multiplier MonsterCaptureMaxLevelIncrement(HeroKind heroKind)
    {
        return heroes[(int)heroKind].monsterCaptureMaxLevelIncrement;
    }
    public Multiplier MaterialLootGain(HeroKind heroKind)
    {
        return heroes[(int)heroKind].materialLootGain;
    }

    public HeroMovedDistance MovedDistance(HeroKind heroKind, bool isReset = false)
    {
        if(isReset) return heroes[(int)heroKind].movedDistanceReset;
        return heroes[(int)heroKind].movedDistance;
    }
    public HeroElementSkillTriggeredNum ElementSkillTriggeredNum(HeroKind heroKind, Element element)
    {
        return heroes[(int)heroKind].elementSkillTriggeredNum[(int)element];
    }

    //Aura Potion
    public Multiplier DebuffChance(HeroKind heroKind, Debuff kind)
    {
        return heroes[(int)heroKind].debuffChances[(int)kind];
    }
    //SlayerOil Potion
    public Multiplier ElementSlayerDamage(HeroKind heroKind, Element kind)
    {
        return heroes[(int)heroKind].elementSlayerDamages[(int)kind];
    }

    //Ability
    public AbilityPointLeft AbilityPointLeft(HeroKind heroKind)
    {
        return heroes[(int)heroKind].abilityPointLeft;
    }
    //public double AdditionalAbilityPoint(HeroKind heroKind)
    //{

    //}
    public HeroAbility Ability(HeroKind heroKind, AbilityKind kind)
    {
        return heroes[(int)heroKind].abilities[(int)kind];
    }
    public void ResetAbilityPoint(HeroKind heroKind, bool isEpicStorePurchase = false)
    {
        if (isEpicStorePurchase) AbilityPointLeft(heroKind).ChangeValue(TotalAbilityPoint(heroKind));
        else AbilityPointLeft(heroKind).ChangeValue(0);
        for (int i = 0; i < Enum.GetNames(typeof(AbilityKind)).Length; i++)
        {
            int count = i;
            Ability(heroKind, (AbilityKind)count).point.ChangeValue(0);
        }
        heroes[(int)heroKind].ResetPresetCycle();
    }
    public double TotalAbilityPoint(HeroKind heroKind)
    {
        double tempPoint = AbilityPointLeft(heroKind).value;
        for (int i = 0; i < Enum.GetNames(typeof(AbilityKind)).Length; i++)
        {
            int count = i;
            tempPoint += Ability(heroKind, (AbilityKind)count).Point();
        }
        return tempPoint;
    }

    public long Level(HeroKind heroKind)
    {
        return heroes[(int)heroKind].Level();
    }
    public HeroLevel HeroLevel(HeroKind heroKind)
    {
        return heroes[(int)heroKind].level;
    }
    
    public HeroExp Exp(HeroKind heroKind)
    {
        return heroes[(int)heroKind].exp;
    }

    public double RequiredExp(HeroKind heroKind)
    {
        return heroes[(int)heroKind].RequiredExp();
    }

    public float ExpPercent(HeroKind heroKind)
    {
        return (float)(Exp(heroKind).value / RequiredExp(heroKind));
    }

    public (long level, double expPercent, long levelIncrement) EstimatedLevel(HeroKind heroKind, double expGain, bool isNoLimit = false)
    {
        long currentLevel = HeroLevel(heroKind).value;
        long tempLevel = currentLevel;
        double tempExp = Exp(heroKind).value;
        for (int i = 0; i < 30 + Convert.ToInt32(isNoLimit) * 500; i++)
        {
            if (expGain < Parameter.RequiredExp(tempLevel) - tempExp)//Levelが上がらない場合
            {
                tempExp += expGain;
                return (tempLevel, tempExp / Parameter.RequiredExp(tempLevel), tempLevel - currentLevel);
            }
            expGain -= Parameter.RequiredExp(tempLevel) - tempExp;
            tempLevel++;
            tempExp = 0;
        }
        return (tempLevel, 0, tempLevel - currentLevel);
    }

    //Auto
    public void AutoAddAbilityPoint()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i].AutoAddAbilityPoint();
        }
    }

    public double TotalMovedDistance(bool isThisAscension = false)
    {
        double tempValue = 0;
        if (isThisAscension)
        {
            for (int i = 0; i < main.S.totalMovedDistance.Length; i++)
            {
                tempValue += main.S.totalMovedDistance[i];
            }
            tempValue += main.S.totalMovedDistancePet;
        }
        else
        {
            for (int i = 0; i < main.S.movedDistance.Length; i++)
            {
                tempValue += main.S.movedDistance[i];
            }
            tempValue += main.S.movedDistancePet;
        }
        return tempValue;
    }

}

public class HeroStats
{
    //AutoAbility
    bool isAutoAbility => main.S.isAutoAbilityPointPresets[(int)heroKind];//game.rebirthCtrl.IsAutoRebirth(heroKind) && 
    long vitPreset => main.S.autoAbilityPointPresetsVIT[(int)heroKind];
    long strPreset => main.S.autoAbilityPointPresetsSTR[(int)heroKind];
    long intPreset => main.S.autoAbilityPointPresetsINT[(int)heroKind];
    long agiPreset => main.S.autoAbilityPointPresetsAGI[(int)heroKind];
    long lukPreset => main.S.autoAbilityPointPresetsLUK[(int)heroKind];
    long[] abilityPreset => new long[]
    {
        vitPreset,
        strPreset,
        intPreset,
        agiPreset,
        lukPreset,
    };
    long[] maxAbilityPreset => new long[]
    {
        main.S.autoAbilityPointMaxVIT[(int)heroKind],
        main.S.autoAbilityPointMaxSTR[(int)heroKind],
        main.S.autoAbilityPointMaxINT[(int)heroKind],
        main.S.autoAbilityPointMaxAGI[(int)heroKind],
        main.S.autoAbilityPointMaxLUK[(int)heroKind],
    };
    long[] abilityTempAdded = new long[Enum.GetNames(typeof(AbilityKind)).Length];
    //bool[] isAbilityTempAddedFinished = new bool[Enum.GetNames(typeof(AbilityKind)).Length];
    //ArrayId autoAbilityId;
    
    public void AutoAddAbilityPoint()
    {
        for (int j = 0; j < 1000; j++)
        {
            if (abilityPointLeft.value < 1) return;
            if (!isAutoAbility) return;
            if (!IsActivatedAutoAddAbility()) return;
            if (!game.guildCtrl.Member(heroKind).isActive) return;
            for (int i = 0; i < abilityPreset.Length; i++)
            {
                for (int k = 0; k < abilityPreset[i]+1; k++)
                {
                    if (abilityPointLeft.value < 1) return;
                    if (maxAbilityPreset[i] > 0 && abilities[i].point.value >= maxAbilityPreset[i])
                        break;
                    if (abilityTempAdded[i] >= abilityPreset[i])
                    {
                        //isAbilityTempAddedFinished[i] = true;
                        break;
                    }
                    abilities[i].transaction.Buy(true);
                    abilityTempAdded[i]++;
                }
                //autoAbilityId.Increase();
            }
            if (IsFinishedPresetCycle())//次のサイクルへ
            {
                ResetPresetCycle();
            }
        }
    }
    public void ResetPresetCycle()
    {
        //for (int i = 0; i < isAbilityTempAddedFinished.Length; i++)
        //{
        //    isAbilityTempAddedFinished[i] = false;
        //}
        for (int i = 0; i < abilityTempAdded.Length; i++)
        {
            abilityTempAdded[i] = 0;
        }
    }
    bool IsActivatedAutoAddAbility()
    {
        for (int i = 0; i < abilityPreset.Length; i++)
        {
            if (abilityPreset[i] > 0) return true;
        }
        return false;
    }
    bool IsFinishedPresetCycle()
    {
        for (int i = 0; i < abilityPreset.Length; i++)
        {
            if (abilityTempAdded[i] < abilityPreset[i] && !(maxAbilityPreset[i] > 0 && abilities[i].point.value >= maxAbilityPreset[i])) return false;
        }
        //for (int i = 0; i < isAbilityTempAddedFinished.Length; i++)
        //{
            
        //    if (!isAbilityTempAddedFinished[i]) return false;
        //}
        return true;
    }

    public Multiplier[] basicStats = new Multiplier[Enum.GetNames(typeof(BasicStatsKind)).Length];//HP,MP,ATK,...
    public Multiplier[] basicStatsPerLevel = new Multiplier[Enum.GetNames(typeof(BasicStatsKind)).Length];
    public Multiplier[] stats = new Multiplier[Enum.GetNames(typeof(Stats)).Length];
    public Multiplier[] optionEffectChance = new Multiplier[3];//EquipmentのOption付与確率（3OP目まで）
    public Multiplier hpRegenerate;
    public Multiplier mpRegenerate;
    public Multiplier skillSlotNum;
    public Multiplier levelForEquipment;
    public float combatRange => combatRanges[main.SR.combatRangeId[(int)heroKind]];
    public void SwitchCombatRange(bool isMinus = false)
    {
        if (isMinus)
        {
            if (main.SR.combatRangeId[(int)heroKind] <= 0) main.SR.combatRangeId[(int)heroKind] = combatRanges.Length - 1;
            else main.SR.combatRangeId[(int)heroKind]--;
        }
        else
        {
            if (main.SR.combatRangeId[(int)heroKind] >= combatRanges.Length - 1) main.SR.combatRangeId[(int)heroKind] = 0;
            else main.SR.combatRangeId[(int)heroKind]++;
        }
    }
    //種族別ダメージボーナス
    public Multiplier[] monsterDamages = new Multiplier[Enum.GetNames(typeof(MonsterSpecies)).Length];
    //属性ダメージボーナス
    public Multiplier[] elementDamages = new Multiplier[Enum.GetNames(typeof(Element)).Length];
    //属性吸収（最大99%）
    public Multiplier[] elementAbsoptions = new Multiplier[Enum.GetNames(typeof(Element)).Length];
    //属性無効
    public Multiplier[] elementInvalids = new Multiplier[Enum.GetNames(typeof(Element)).Length];
    public Multiplier golemInvalidDamageHpPercent = new Multiplier();
    //Title関係,統計量
    public Multiplier monsterDistinguishMaxLevel;
    public Multiplier monsterCaptureMaxLevelIncrement;
    ////TownMat
    //public Multiplier townMaterialGain;
    
    public HeroMovedDistance movedDistance;
    public HeroMovedDistance movedDistanceReset;//Rebirthでリセット
    public HeroElementSkillTriggeredNum[] elementSkillTriggeredNum = new HeroElementSkillTriggeredNum[Enum.GetNames(typeof(Element)).Length];
    //Material獲得量（Lootからのみ）
    public Multiplier materialLootGain;
    //Potion
    public Multiplier[] elementSlayerDamages = new Multiplier[Enum.GetNames(typeof(Element)).Length];
    public Multiplier[] debuffChances = new Multiplier[Enum.GetNames(typeof(Debuff)).Length];
    //MovePatern
    public MovePattern MovePattern()
    {
        if (game.skillCtrl.Skill(HeroKind.Archer, (int)SkillKindArcher.Kiting).IsEquipped(heroKind))
            return global::MovePattern.Kiting;
        return global::MovePattern.Shortest;
    }
    //ChannelMP
    public Multiplier channeledMp;
    //SummonPet
    public Multiplier summonPetSlot;
    public SummonPet[] summonPets = new SummonPet[Parameter.maxPetSpawnNum];
    public Multiplier loyaltyPoingGain;
    public Multiplier summonPetATKMATKMultiplier;
    public Multiplier summonPetSPDMoveSpeedMultiplier;
    public Multiplier summonPetCriticalChanceAdder;

    public HeroStats(HeroKind kind)
    {
        this.heroKind = kind;
        level = new HeroLevel(kind, () => Parameter.maxHeroLevel);
        exp = new HeroExp(kind, RequiredExp, level);
        abilityPointLeft = new AbilityPointLeft(heroKind);
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new HeroAbility(kind, (AbilityKind)i, abilityPointLeft);
        }
        movedDistance = new HeroMovedDistance(kind, false);
        movedDistanceReset = new HeroMovedDistance(kind, true);
        for (int i = 0; i < elementSkillTriggeredNum.Length; i++)
        {
            int count = i;
            elementSkillTriggeredNum[i] = new HeroElementSkillTriggeredNum(heroKind, (Element)count);
        }

        //autoAbilityId = new ArrayId(0, () => abilityTempAdded.Length);
        //以下Statsの設定
        SetStats();
    }
    void SetStats()
    {
        for (int i = 0; i < basicStats.Length; i++)
        {
            int count = i;
            basicStats[count] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][count]));
            basicStats[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => AbilityStats((BasicStatsKind)count)));
            basicStatsPerLevel[count] = new Multiplier();
            basicStats[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, Add, () => basicStatsPerLevel[count].Value() * Level()));
        }
        for (int i = 0; i < stats.Length; i++)
        {
            int count = i;
            stats[count] = new Multiplier();
        }
        stats[(int)Stats.FireRes].maxValue = () => 0.90d;//90%が最大
        stats[(int)Stats.IceRes].maxValue = () => 0.90d;
        stats[(int)Stats.ThunderRes].maxValue = () => 0.90d;
        stats[(int)Stats.LightRes].maxValue = () => 0.90d;
        stats[(int)Stats.DarkRes].maxValue = () => 0.90d;
        stats[(int)Stats.FireRes].minValue = () => -100;//-10000%が最小
        stats[(int)Stats.IceRes].minValue = () => -100;
        stats[(int)Stats.ThunderRes].minValue = () => -100;
        stats[(int)Stats.LightRes].minValue = () => -100;
        stats[(int)Stats.DarkRes].minValue = () => -100;
        stats[(int)Stats.PhysCritChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][7]));
        stats[(int)Stats.PhysCritChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => Parameter.stats[(int)heroKind][7] * abilities[(int)AbilityKind.Luck].Point()));//0.1%
        stats[(int)Stats.MagCritChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][8]));
        stats[(int)Stats.MagCritChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => Parameter.stats[(int)heroKind][8] * abilities[(int)AbilityKind.Luck].Point()));//0.1%
        stats[(int)Stats.CriticalDamage].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][9]));
        stats[(int)Stats.EquipmentDropChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][10]));
        stats[(int)Stats.EquipmentDropChance].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => Parameter.stats[(int)heroKind][10] * Math.Pow(abilities[(int)AbilityKind.Luck].Point(), 2 / 3d)));//0.001%
        stats[(int)Stats.MoveSpeed].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => baseStats[(int)heroKind][11]));
        stats[(int)Stats.MoveSpeed].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => Parameter.stats[(int)heroKind][11] * Math.Pow(abilities[(int)AbilityKind.Agility].Point(), 2 / 3d)));//abilities[(int)AbilityKind.Agility].Point()));
        stats[(int)Stats.SkillProficiencyGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        stats[(int)Stats.EquipmentProficiencyGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        stats[(int)Stats.TamingPointGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        stats[(int)Stats.ExpGain].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        for (int i = 0; i < optionEffectChance.Length; i++)
        {
            int count = i;
            optionEffectChance[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 0.01d * Math.Pow(0.1d, count)));//1%,0.1%,0.01%
            optionEffectChance[i].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Ability, Add, () => 0.01d * 0.02d * Math.Pow(0.1d, count) * abilities[(int)AbilityKind.Luck].Point()));//0.02%,0.002%,0.0002%
        }
        hpRegenerate = new Multiplier();
        mpRegenerate = new Multiplier();
        skillSlotNum = new Multiplier(true);
        skillSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        levelForEquipment = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => Level()));
        for (int i = 0; i < monsterDamages.Length; i++)
        {
            monsterDamages[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        }
        for (int i = 0; i < elementDamages.Length; i++)
        {
            elementDamages[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        }
        for (int i = 0; i < elementAbsoptions.Length; i++)
        {
            elementAbsoptions[i] = new Multiplier(() => 0.90d, () => 0);//最大90%
        }
        for (int i = 0; i < elementInvalids.Length; i++)
        {
            elementInvalids[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 0));
        }
        golemInvalidDamageHpPercent = new Multiplier();
        monsterDistinguishMaxLevel = new Multiplier();
        monsterCaptureMaxLevelIncrement = new Multiplier();
        materialLootGain = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        //townMaterialGain = new Multiplier();
        //Potion
        for (int i = 0; i < debuffChances.Length; i++)
        {
            debuffChances[i] = new Multiplier();
        }
        for (int i = 0; i < elementSlayerDamages.Length; i++)
        {
            elementSlayerDamages[i] = new Multiplier();
        }

        channeledMp = new Multiplier();
        summonPetSlot = new Multiplier(() => maxPetSpawnNum, ()=>0);
        loyaltyPoingGain = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        summonPetATKMATKMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        summonPetSPDMoveSpeedMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 1));
        summonPetCriticalChanceAdder = new Multiplier();
        ////Debug
        //summonPetSlot.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, Add, () => 3));

        for (int i = 0; i < summonPets.Length; i++)
        {
            summonPets[i] = new SummonPet(heroKind, i);
        }
    }

    public HeroKind heroKind;
    public HeroLevel level;
    public HeroExp exp;
    public HeroAbility[] abilities = new HeroAbility[Enum.GetNames(typeof(AbilityKind)).Length];

    public long Level() { return level.value; }
    public double RequiredExp(long level) { return Parameter.RequiredExp(level); }
    public double RequiredExp() { return RequiredExp(Level()); }
    public HeroAbility Ability(AbilityKind kind) { return abilities[(int)kind]; }
    public AbilityPointLeft abilityPointLeft;

    public double AbilityStats(BasicStatsKind kind)
    {
        double temp = Parameter.stats[(int)heroKind][(int)kind];
        switch (kind)
        {
            case BasicStatsKind.HP:
                temp *= Ability(AbilityKind.Vitality).Point();
                break;
            case BasicStatsKind.MP:
                temp *= (double)(Ability(AbilityKind.Agility).Point() + Ability(AbilityKind.Intelligence).Point()) / 2d;
                break;
            case BasicStatsKind.ATK:
                temp *= Ability(AbilityKind.Strength).Point();
                break;
            case BasicStatsKind.MATK:
                temp *= Ability(AbilityKind.Intelligence).Point();
                break;
            case BasicStatsKind.DEF:
                temp *= (double)(Ability(AbilityKind.Vitality).Point() + Ability(AbilityKind.Strength).Point()) / 2d;
                break;
            case BasicStatsKind.MDEF:
                temp *= (double)(Ability(AbilityKind.Vitality).Point() + Ability(AbilityKind.Intelligence).Point()) / 2d;
                break;
            case BasicStatsKind.SPD:
                temp *= Ability(AbilityKind.Agility).Point();
                break;
        }
        return temp;
    }
}

public class AbilityPointLeft : NUMBER
{
    public AbilityPointLeft(HeroKind heroKind)
    {
        this.heroKind = heroKind;
    }
    HeroKind heroKind;
    public override double value { get => main.SR.abilityPoints[(int)heroKind]; set => main.SR.abilityPoints[(int)heroKind] = value; }
}

public class HeroLevel : INTEGER
{
    HeroKind heroKind;
    public override long value { get => main.SR.heroLevel[(int)heroKind]; set => main.SR.heroLevel[(int)heroKind] = value; }
    public HeroLevel(HeroKind heroKind, Func<long> maxLevel)
    {
        this.heroKind = heroKind;
        this.maxValue = maxLevel;
    }
    public Action<long> levelUpUIAction;
    public override void Increase(long increment)
    {
        game.guildCtrl.exp.GainExp(value, increment);
        for (int i = 0; i < increment; i++)
        {
            base.Increase(1);
            game.statsCtrl.AbilityPointLeft(heroKind).Increase(1);
            if (value >= 25 && value % 25 == 0)//Rebirth Tier 2のボーナス
            {
                game.statsCtrl.AbilityPointLeft(heroKind).Increase(game.rebirthCtrl.Rebirth(heroKind, 1).additionalAbilityPoint.Value());
                //BestEXPArea
                if (heroKind == game.currentHero && game.rebirthCtrl.IsTravelBestArea(heroKind))
                {
                    if (!game.rebirthCtrl.Rebirth(heroKind, game.rebirthCtrl.AutoRebirthTier(heroKind)).CanAutoRebirth())
                        game.autoCtrl.tourArea.UpdateBestArea(value);
                }
            }
        }
        if (game.IsUI(heroKind) && levelUpUIAction != null) levelUpUIAction(increment);

        main.S.maxHeroLevelReached[(int)heroKind] = Math.Max(main.S.maxHeroLevelReached[(int)heroKind], value);
        //for (int i = 0; i < game.rebirthCtrl.rebirth[(int)heroKind].Length; i++)
        //{
        //    Rebirth tempRebirth = game.rebirthCtrl.rebirth[(int)heroKind][i];
        //    if (tempRebirth.isAutoRebirthOn)
        //    {
        //        if (value >= tempRebirth.autoRebirthLevel)
        //        {
        //            tempRebirth.AutoRebirth();
        //            return;
        //        }
        //    }

        //}
    }
}

public class HeroExp : EXP
{
    HeroKind heroKind;
    public override double value { get => main.SR.heroExp[(int)heroKind]; set => main.SR.heroExp[(int)heroKind] = value; }
    public HeroExp(HeroKind heroKind, Func<long, double> requiredExp, INTEGER level)
    {
        this.heroKind = heroKind;
        this.requiredValue = requiredExp;
        this.level = level;
    }
    public void Increase(double increment, bool isBattleResult)
    {
        //ここにExpGainの倍率をかける
        increment *= game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).Value();
        increment += game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue();

        //GuildMember(Passive)のGainRate
        increment *= game.guildCtrl.Member(heroKind).gainRate;

        //increment = Math.Min(increment, requiredValue(level.value) * 100);ここで制限をかけず、Increase()で30までの制限をかける
        Increase(increment);//これで下に飛ぶ
        RegisterGainPerSec(increment);
        if (logUIAction != null && game.IsUI(heroKind)) logUIAction(increment);
        if (isBattleResult) game.battleCtrl.areaBattle.exp += increment;// && resultUIAction != null) resultUIAction(increment);
    }
    public Action<double> logUIAction;
    //public Action<double> resultUIAction;

    //LevelLimit30の制限あり
    public override void Increase(double increment)
    {
        if (level.IsMaxed()) return;// && value.Equals(requiredValue(level.value))) return;

        value += increment;
        RegisterGainPerSec(Math.Min(increment, maxValue()));
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());

        if (value < requiredValue(level.value)) return;

        long tempLevelIncrement = 0;
        long tempLevel = level.value;
        while (true)
        {
            if (value < requiredValue(tempLevel)) break;

            if (tempLevel >= level.maxValue())
            {
                ChangeValue(requiredValue(tempLevel));
                break;
            }

            base.Decrease(requiredValue(tempLevel));
            tempLevel++;
            tempLevelIncrement++;

            //LevelIncrementは最大30までというLimitをつける
            if (tempLevelIncrement >= 30)
            {
                ChangeValue(0);
                break;
            }
        }
        level.Increase(tempLevelIncrement);
    }
    //LevelLimit30の制限なし
    public void IncreaseWithoutLimit(double increment)
    {
        if (level.IsMaxed()) return;// && value.Equals(requiredValue(level.value))) return;

        value += increment;
        RegisterGainPerSec(Math.Min(increment, maxValue()));
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());

        if (value < requiredValue(level.value)) return;

        long tempLevelIncrement = 0;
        long tempLevel = level.value;
        while (true)
        {
            if (value < requiredValue(tempLevel)) break;

            if (tempLevel >= level.maxValue())
            {
                ChangeValue(requiredValue(tempLevel));
                break;
            }

            base.Decrease(requiredValue(tempLevel));
            tempLevel++;
            tempLevelIncrement++;

            if (tempLevelIncrement >= 500) break;//一応500の上限を作っておく
            ////LevelIncrement30を超えたら増加率が下がる
            ////これは、OfflineBonus時に、モンスターとのレベル差によってEXP獲得量が変わることを考慮したもの
            //if (tempLevelIncrement >= 30)
            //{
            //    base.Decrease(value * 0.01d);//1レベル上昇ごとに1%減らす
            //    //ChangeValue(0);
            //    //break;
            //}
        }
        level.Increase(tempLevelIncrement);
    }
}

public class HeroAbility
{
    public Transaction transaction;
    public HeroAbility(HeroKind heroKind, AbilityKind kind, AbilityPointLeft pointLeft)
    { 
        this.heroKind = heroKind;
        this.kind = kind;
        point = new HeroAbilityPoint(heroKind, kind);
        transaction = new Transaction(point, pointLeft, () => 1, () => 1);
    }

    public long Point()
    {
        return point.value;
    }
    HeroKind heroKind;
    AbilityKind kind;
    public HeroAbilityPoint point;
}
public class HeroAbilityPoint : INTEGER
{
    HeroKind heroKind;
    AbilityKind kind;
    public HeroAbilityPoint(HeroKind heroKind, AbilityKind kind)
    {
        this.heroKind = heroKind;
        this.kind = kind;
    }
    public override long value
    {
        get
        {
            switch (kind)
            {
                case AbilityKind.Vitality:
                    return main.SR.abilityPointsVitality[(int)heroKind];
                case AbilityKind.Strength:
                    return main.SR.abilityPointsStrength[(int)heroKind];
                case AbilityKind.Intelligence:
                    return main.SR.abilityPointsIntelligence[(int)heroKind];
                case AbilityKind.Agility:
                    return main.SR.abilityPointsAgility[(int)heroKind];
                case AbilityKind.Luck:
                    return main.SR.abilityPointsLuck[(int)heroKind];
            }
            return 0;
        }
        set
        {
            switch (kind)
            {
                case AbilityKind.Vitality:
                    main.SR.abilityPointsVitality[(int)heroKind] = value;
                    break;
                case AbilityKind.Strength:
                    main.SR.abilityPointsStrength[(int)heroKind] = value;
                    break;
                case AbilityKind.Intelligence:
                    main.SR.abilityPointsIntelligence[(int)heroKind] = value;
                    break;
                case AbilityKind.Agility:
                    main.SR.abilityPointsAgility[(int)heroKind] = value;
                    break;
                case AbilityKind.Luck:
                    main.SR.abilityPointsLuck[(int)heroKind] = value;
                    break;
                default:
                    break;
            }
        }
    }
}

public class HeroMovedDistance : NUMBER
{
    public HeroMovedDistance(HeroKind heroKind, bool isReset)
    {
        this.heroKind = heroKind;
        this.isReset = isReset;
    }
    bool isReset;//Rebirthでリセットする値
    public HeroKind heroKind;
    public override double value
    {
        get
        {
            if (isReset) return main.SR.movedDistance[(int)heroKind];
            else return main.S.movedDistance[(int)heroKind];
        }
        set
        {
            if (isReset) main.SR.movedDistance[(int)heroKind] = value;
            else main.S.movedDistance[(int)heroKind] = value;
        }
    }
    public override void Increase(double increment)
    {
        base.Increase(increment);
    }
}
public class HeroElementSkillTriggeredNum : NUMBER
{
    public HeroElementSkillTriggeredNum(HeroKind heroKind, Element element)
    {
        this.heroKind = heroKind;
        this.element = element;
    }
    public HeroKind heroKind;
    public Element element;
    public override double value
    {
        get
        {
            switch (element)
            {
                case Element.Physical:
                    return main.S.physicalTriggeredNum[(int)heroKind];
                case Element.Fire:
                    return main.S.fireTriggeredNum[(int)heroKind];
                case Element.Ice:
                    return main.S.iceTriggeredNum[(int)heroKind];
                case Element.Thunder:
                    return main.S.thunderTriggeredNum[(int)heroKind];
                case Element.Light:
                    return main.S.lightTriggeredNum[(int)heroKind];
                case Element.Dark:
                    return main.S.darkTriggeredNum[(int)heroKind];
            }
            return 0;
        }
        set
        {
            switch (element)
            {
                case Element.Physical:
                    main.S.physicalTriggeredNum[(int)heroKind] = value;
                    break;
                case Element.Fire:
                    main.S.fireTriggeredNum[(int)heroKind] = value;
                    break;
                case Element.Ice:
                    main.S.iceTriggeredNum[(int)heroKind] = value;
                    break;
                case Element.Thunder:
                    main.S.thunderTriggeredNum[(int)heroKind] = value;
                    break;
                case Element.Light:
                    main.S.lightTriggeredNum[(int)heroKind] = value;
                    break;
                case Element.Dark:
                    main.S.darkTriggeredNum[(int)heroKind] = value;
                    break;
            }
        }
    }
}

public enum MovePattern
{
    Shortest,
    Kiting,
}