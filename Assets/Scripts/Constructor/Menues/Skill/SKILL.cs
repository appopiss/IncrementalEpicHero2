using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static UsefulMethod;
using static SkillParameter;
using static GameController;
using static SpriteSourceUI;
using System;

public partial class Save
{
    //統計量
    //SkillTrigger
    public double[] warriorSkillTriggeredNum;//[id]
    public double[] wizardSkillTriggeredNum;//[id]
    public double[] angelSkillTriggeredNum;
    public double[] thiefSkillTriggeredNum;
    public double[] archerSkillTriggeredNum;
    public double[] tamerSkillTriggeredNum;
}

public partial class SaveR
{
    public long[] warriorSkillLevel;
    public long[] wizardSkillLevel;
    public long[] angelSkillLevel;
    public long[] thiefSkillLevel;
    public long[] archerSkillLevel;
    public long[] tamerSkillLevel;
    public long[] warriorMaxReachedSkillLevel;
    public long[] wizardMaxReachedSkillLevel;
    public long[] angelMaxReachedSkillLevel;
    public long[] thiefMaxReachedSkillLevel;
    public long[] archerMaxReachedSkillLevel;
    public long[] tamerMaxReachedSkillLevel;
    public long[] warriorSkillRank;
    public long[] wizardSkillRank;
    public long[] angelSkillRank;
    public long[] thiefSkillRank;
    public long[] archerSkillRank;
    public long[] tamerSkillRank;
    public double[] warriorSkillProficiency;
    public double[] wizardSkillProficiency;
    public double[] angelSkillProficiency;
    public double[] thiefSkillProficiency;
    public double[] archerSkillProficiency;
    public double[] tamerSkillProficiency;
}

public class SKILL
{
    public SKILL(HeroKind heroKind, int id)
    {
        this.heroKind = heroKind;
        this.id = id;
        if (id >= 0)
        {
            rank = new SkillRank(heroKind, id, () => maxSkillRank);
            level = new SkillLevel(heroKind, id, MaxLevel);
            if (level.value < 0) level.ChangeValue(0);
            proficiency = new SkillProficiency(heroKind, id, RequiredProficiency, level);
            rankupTransaction = new Transaction(rank, game.resourceCtrl.Resource(resourceKind), () => initCost, () => baseCost);
            rankupTransaction.SetAdditionalBuyCondition(IsUnlocked);
            triggeredNum = new SkillTriggeredNum(heroKind, id);
        }
        for (int i = 0; i < currentCooltime.Length; i++)
        {
            currentCooltime[i] = new NUMBER(Interval);
        }
        for (int i = 0; i < currentCooltimeSimulated.Length; i++)
        {
            currentCooltimeSimulated[i] = new NUMBER(Interval);
        }
        for (int i = 0; i < attackLists.Length; i++)
        {
            attackLists[i] = new List<Attack>();
        }
    }

    public HeroKind heroKind;//This Skill's heroKind
    public int id;
    public ClassSkill classSkill { get => game.skillCtrl.classSkills[(int)heroKind]; }
    public ResourceKind resourceKind
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return ResourceKind.Stone;
                case HeroKind.Wizard:
                    return ResourceKind.Crystal;
                case HeroKind.Angel:
                    return ResourceKind.Leaf;
                case HeroKind.Thief:
                    return ResourceKind.Stone;
                case HeroKind.Archer:
                    return ResourceKind.Crystal;
                case HeroKind.Tamer:
                    return ResourceKind.Leaf;
            }
            return ResourceKind.Stone;
        }
    }

    public virtual SkillType type { get => SkillType.Attack; }
    public virtual Element element { get => Element.Physical; }
    public virtual SkillEffectCenter effectCenter { get => SkillEffectCenter.Target; }
    public virtual Debuff debuff { get => Debuff.Nothing; }
    public virtual Buff buff { get => Buff.Nothing; }

    public double initCost { get => skillCosts[(int)heroKind][id][0] * game.skillCtrl.skillRankCostFactors[(int)heroKind].Value(); }
    public double baseCost { get => skillCosts[(int)heroKind][id][1]; }

    public List<SkillPassiveEffect> passiveEffectLists = new List<SkillPassiveEffect>();
    public double initDamage { get => skillFactors[(int)heroKind][id][0]; }
    public double incrementDamage { get => skillFactors[(int)heroKind][id][1]; }
    public double initMpGain { get => skillFactors[(int)heroKind][id][2]; }
    public double incrementMpGain { get => skillFactors[(int)heroKind][id][3]; }
    public double initMpConsume { get => skillFactors[(int)heroKind][id][4]; }
    public double incrementMpConsume { get => skillFactors[(int)heroKind][id][5]; }
    public double initInterval { get => skillFactors[(int)heroKind][id][6]; }
    public double profDifficulty { get => skillFactors[(int)heroKind][id][7]; }
    public double range { get => skillFactors[(int)heroKind][id][8]; }
    public double initEffectRange { get => skillFactors[(int)heroKind][id][9]; }
    public double incrementEffectRange { get => skillFactors[(int)heroKind][id][10]; }
    public double maxEffectRange { get => skillFactors[(int)heroKind][id][11]; }
    public double initDebuffChance { get => skillFactors[(int)heroKind][id][12]; }
    public double incrementDebuffChance { get => skillFactors[(int)heroKind][id][13]; }
    public double maxDebuffChance { get => skillFactors[(int)heroKind][id][14]; }
    public double initHitCount { get => skillFactors[(int)heroKind][id][15]; }
    public double incrementHitCount { get => skillFactors[(int)heroKind][id][16]; }
    public double maxHitCount { get => skillFactors[(int)heroKind][id][17]; }


    public float throwSpeed = 2000f;

    public List<Attack>[] attackLists = new List<Attack>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<Attack> simulatedAttackList = new List<Attack>();
    public SkillRank rank;
    public SkillLevel level;
    public SkillProficiency proficiency;
    public Transaction rankupTransaction;
    public SkillTriggeredNum triggeredNum;

    public NUMBER[] currentCooltime = new NUMBER[Enum.GetNames(typeof(HeroKind)).Length];
    public NUMBER[] currentCooltimeSimulated = new NUMBER[Enum.GetNames(typeof(HeroKind)).Length];

    //必要スキル <id, level>
    public Dictionary<int, long> requiredSkills = new Dictionary<int, long>();

    public bool IsUnlocked()
    {
        if (Rank() > 0) return true;//Rebirth後はTrue
        foreach (var item in requiredSkills)
        {
            if (game.skillCtrl.Skill(heroKind, item.Key).level.value < item.Value) return false;
        }
        return true;
    }
    public bool CanEquip()
    {
        return Rank() > 0 && IsUnlocked();
    }
    public bool IsEquipped(HeroKind heroKind)
    {
        SkillSet skillSet = game.battleCtrls[(int)heroKind].skillSet;
        if (!skillSet.IsEquipped(this)) return false;
        if (this.heroKind == heroKind)//classSkillの場合
            return Array.IndexOf(skillSet.currentSkillSet, this) < skillSet.currentEquippingNum;
        return Array.IndexOf(skillSet.currentGlobalSkillSet, this) < skillSet.currentGlobalEquippingNum;
    }

    public virtual string Description() { return "";}

    public virtual long levelBonus => (long)game.skillCtrl.skillLevelBonus[(int)heroKind].Value() + (long)game.skillCtrl.skillLevelBonusFromHolyArch[(int)heroKind].Value();
    public virtual long Level()
    {
        if (Rank() <= 0) return 0;
        return level.value + levelBonus;
    }
    public long Rank()
    {
        return rank.value;
    }

    public long MaxLevel()
    {
        return Rank() * 5;
    }

    public double IncrementDamagePerLevel()
    {
        return incrementDamage * Rank();
    }
    public virtual double Damage()
    {
        double tempValue = initDamage + IncrementDamagePerLevel() * Level();
        return tempValue;
    }
    public virtual double Interval()
    {
        return initInterval * Math.Max(0.5, (1 - 0.0005 * Level()));
    }
    public double IncrementMpGainPerLevel()
    {
        return incrementMpGain * Rank();
    }
    public virtual double GainMp()
    {
        double tempValue = initMpGain + IncrementMpGainPerLevel() * Level();
        return tempValue;
    }
    public virtual double ConsumeMp()
    {
        if (type == SkillType.Buff) return 0;
        double tempValue = initMpConsume + incrementMpConsume * Level();
        return tempValue;
    }
    public virtual double ChanneledMp()
    {
        if (type != SkillType.Buff) return 0;
        double tempValue = initMpConsume + incrementMpConsume * Level();
        return tempValue;
    }
    public int HitCount()//SkillLevelのBuffはカウントしない
    {
        int tempValue = Math.Min((int)maxHitCount, (int)(initHitCount + incrementHitCount * level.value));
        return tempValue;
    }
    public virtual float Range()
    {
        return (float)range;
    }
    public virtual float EffectRange()
    {
        float tempValue = (float)Math.Min(maxEffectRange, initEffectRange + incrementEffectRange * Level());
        return tempValue;
    }
    public virtual double DebuffChance()
    {
        double tempValue = Math.Min(maxDebuffChance, initDebuffChance + incrementDebuffChance * Level());
        return tempValue;
    }
    public double RequiredProficiency(long level)
    {
        return Math.Floor((5 * (1 + profDifficulty * 0.75) * (1 + level) * Math.Pow(3d, level / 100d)) / Math.Max(0.1, initInterval));
    }
    public double RequiredProficiency()
    {
        return RequiredProficiency(level.value);
    }
    public double Damage(BATTLE myself, bool isDisplay = false)//isDisplay=trueなら、MPの補正を無視する
    {
        double tempDamage = Damage();
        if (element == Element.Physical) tempDamage *= myself.atk;
        else tempDamage *= myself.matk;
        //tempDamage *= game.statsCtrl.ElementDamage(myself.heroKind, element).Value();
        if (isDisplay) return tempDamage;
        tempDamage *= skillAbuseMpPercents[(int)myself.heroKind];
        return tempDamage;
    }
    public double[] skillAbuseMpPercents = new double[Enum.GetNames(typeof(HeroKind)).Length];//Skill Abuse (EpicStore)

    public double IncrementDamagePerLevel(BATTLE myself)
    {
        double tempDamage = IncrementDamagePerLevel();
        if (element == Element.Physical) tempDamage *= myself.atk;
        else tempDamage *= myself.matk;
        tempDamage *= game.statsCtrl.ElementDamage(myself.heroKind, element).Value();
        return tempDamage;
    }
    public double HealPoint()
    {
        return Damage();
    }
    public double IncrementHealPointPerLevel()
    {
        return incrementDamage * Rank();
    }
    public virtual double BuffPercent()
    {
        double tempValue = initDamage + IncrementBuffPercentPerLevel() * Level();
        return tempValue;
    }
    public virtual double IncrementBuffPercentPerLevel()
    {
        return incrementDamage * Rank();
    }
    public bool IsCrit(BATTLE myself)
    {
        bool tempIsCrit = false;
        if (element == Element.Physical)
            tempIsCrit = WithinRandom(myself.phyCrit);
        else
            tempIsCrit = WithinRandom(myself.magCrit);
        return tempIsCrit;
    }
    public double Interval(BATTLE myself)
    {
        //SPD100で94.5%,200で89.6%,500で78%,1000で65%,2000で49.5%,5000で31.5%,10000で22%,100000で8.5%
        //ver0.1.2.12 さらにspd/5したので、SPD500で94.5%, 1000で89.6%...
        return Math.Max(0.05d,
            Interval()
            * (1d / Math.Log(1.40d + Math.Max(0, myself.spd / 5) / 5000, 1.40d))//ver00020100以前は1.30dでした
            * Math.Max(0.5, 1 - game.skillCtrl.skillCooltimeReduction[(int)heroKind].Value())
            );
    }
    public double Dps(BATTLE myself, bool isDisplay = false)
    {
        return Damage(myself, isDisplay) / Interval(myself) * game.statsCtrl.ElementDamage(myself.heroKind, element).Value();
    }
    public virtual double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        //elementの計算をここに入れる
        return Dps(myself, isDisplay) * HitCount();// * game.statsCtrl.ElementDamage(myself.heroKind, element).Value();
    }
    public Vector2 EffectInitPosition(SkillEffectCenter effectCenter, BATTLE myself, Func<BATTLE> target)
    {
        switch (effectCenter)
        {
            case SkillEffectCenter.Target:
                return target().move.position;
            case SkillEffectCenter.Myself:
                return myself.move.position;
            case SkillEffectCenter.Field:
                return Vector2.zero;
        }
        return Vector2.zero;
    }
    public Debuff LotteryDebuff()
    {
        if (DebuffChance() <= 0) return Debuff.Nothing;
        else
        {
            if (UnityEngine.Random.Range(0, 10000) < DebuffChance() * 10000)
                return debuff;
            return Debuff.Nothing;
        }
        
    }


    //Battle用
    public void SetTrigger(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        if (type == SkillType.Attack) SetAttack(battleCtrl, myself, target, targetList);
        if (battleCtrl.isSimulated)
        {
            simulatedAttackArray = simulatedAttackList.ToArray();
            return;
        }
        SetBuff(battleCtrl.heroKind);
        //BuffのChannel
        if (type == SkillType.Buff) game.statsCtrl.heroes[(int)battleCtrl.heroKind].channeledMp.RegisterMultiplier(new MultiplierInfo(MultiplierKind.ChanneledSkill, MultiplierType.Add, () => ChanneledMp(), () => IsEquipped(battleCtrl.heroKind)));

        //Attackの登録が終わったのち、Arrayに変換する
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            attackArray[i] = attackLists[i].ToArray();
        }
    }
    Attack[][] attackArray = new Attack[Enum.GetNames(typeof(HeroKind)).Length][];
    Attack[] simulatedAttackArray;
    public List<Attack> AttackList(BATTLE_CONTROLLER battleCtrl)
    {
        if (battleCtrl.isSimulated) return simulatedAttackList;
        return attackLists[(int)battleCtrl.heroKind];
    }
    public Attack[] AttackArray(BATTLE_CONTROLLER battleCtrl)
    {
        if (battleCtrl.isSimulated) return simulatedAttackArray;
        return attackArray[(int)battleCtrl.heroKind];
    }
    public virtual void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff()));
    }
    public virtual void SetBuff(HeroKind heroKind)
    {

    }
    bool isSimulated, isUI;
    Element tempElement;
    //Battle用
    public void Trigger(BATTLE battle, NUMBER mp, bool isPetUse = false, double profGainPercent = 1)
    {
        skillAbuseMpPercents[(int)battle.heroKind] = !isPetUse && ConsumeMp() > 0 ? Math.Min(1.0d, Math.Max(0.1d, battle.currentMp.value / ConsumeMp())) : 1d;//main.S.isToggleOn[(int)ToggleKind.SkillLessMPAvailable] && 
        isSimulated = battle.battleCtrl.isSimulated;
        switch (type)
        {
            case SkillType.Attack:
                Attack(battle);
                isUI = !isSimulated && game.IsUI(battle.battleCtrl.heroKind);
                if (isUI) ShowEffectUI(battle.battleCtrl.heroKind);
                break;
            case SkillType.Buff:
                DoBuff(battle);
                break;
            case SkillType.Heal:
                DoHeal(battle);
                break;
            case SkillType.Order:
                DoOrder(battle, isUI);
                break;
        }
        AttackWithTime(battle, isUI);


        //Cooltimeのリセット
        if (!isPetUse) ResetCooltime(battle, isSimulated);
        //MP消費
        if (!isPetUse) mp.Decrease(ConsumeMp());
        if (isSimulated) return;
        //統計量
        triggeredNum.Increase(skillAbuseMpPercents[(int)battle.heroKind]);
        //SlayerOilによってelementが変更された場合は統計データも変更されたelementで登録する
        tempElement = element;
        if (battle.battleCtrl.CurrentSlayerElement() != Element.Physical) tempElement = battle.battleCtrl.CurrentSlayerElement();
        game.statsCtrl.ElementSkillTriggeredNum(battle.heroKind, tempElement).Increase(1);
        //Proficiency
        GetProficiency(skillAbuseMpPercents[(int)battle.heroKind] * profGainPercent, battle.heroKind);
    }
    public virtual void Attack(BATTLE battle)//このbattleはこのSkillを発動する人
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle);
    }
    public virtual async void AttackWithTime(BATTLE battle, bool isUI) { }//isUI=trueの時はUIもふくむ
    public virtual void DoBuff(BATTLE battle)
    {

    }
    public virtual void DoHeal(BATTLE battle)
    {
        battle.Heal(HealPoint());
    }
    public virtual void DoOrder(BATTLE battle, bool isUI) { }

    public Action<Attack, Sprite> effectUIAction;
    public Action<Attack, Sprite, Func<Vector2>> effectUIActionWithDirection;
    public Action<Attack, AnimationEffectKind> animationEffectUIAction;
    public Action<Attack, ParticleEffectKind> particleEffectUIAction;
    public void SetSkillEffectUIAction(Action<Attack, Sprite> effectUIAction)
    {
        this.effectUIAction = effectUIAction;
    }
    public void SetSkillAnimationEffectUIAction(Action<Attack, AnimationEffectKind> animationEffectUIAction)
    {
        this.animationEffectUIAction = animationEffectUIAction;
    }
    public void SetSkillParticleEffectUIAction(Action<Attack, ParticleEffectKind> particleEffectUIAction)
    {
        this.particleEffectUIAction = particleEffectUIAction;
    }

    public virtual void ShowEffectUI(HeroKind heroKind)
    {
        //UI
        if (effectUIAction != null) effectUIAction(attackLists[(int)heroKind][0], sprite.skillEffects[(int)this.heroKind][id]);
    }

    public void GetProficiency(double gain, HeroKind heroKind)
    {
        proficiency.Increase(gain, heroKind);
    }
    public float ProficiencyPercent()
    {
        return (float)(proficiency.value / RequiredProficiency());
    }
    public void CountCooltime(BATTLE battle, float deltaTime, bool isSimulated)
    {
        if (isSimulated) currentCooltimeSimulated[(int)battle.heroKind].Increase(deltaTime);
        else currentCooltime[(int)battle.heroKind].Increase(deltaTime);
    }
    public bool IsFilledCoolttime(BATTLE battle, bool isSimulated)
    {
        if (isSimulated) return currentCooltimeSimulated[(int)battle.heroKind].value >= Interval();
        return currentCooltime[(int)battle.heroKind].value >= Interval();
    }
    public float CooltimePercent()
    {
        return (float)(currentCooltime[(int)game.currentHero].value / Interval());
    }
    public void ResetCooltime(BATTLE battle, bool isSimulated)
    {
        if (isSimulated) currentCooltimeSimulated[(int)battle.heroKind].ChangeValue(0);
        else  currentCooltime[(int)battle.heroKind].ChangeValue(0);
    }
}

public enum SkillPassiveEffectKind
{
    BasicStats,
    HeroStats,
    GlobalStats,
    Others,
}
public class SkillPassiveEffect
{
    public SKILL skill;
    public SkillLevel level;
    public long requiredLevel;
    public SkillPassiveEffectKind effectKind;
    public BasicStatsKind basicStatsKind;
    public Stats statsKind;
    public GlobalStats globalStatsKind;
    public MultiplierType multiplierType;
    public string description;

    public double value;
    public SkillPassiveEffect(SKILL skill, long requiredLevel, BasicStatsKind basicStatsKind, MultiplierType multiplierType, double value)
    {
        effectKind = SkillPassiveEffectKind.BasicStats;
        this.skill = skill;
        this.level = skill.level;
        this.requiredLevel = requiredLevel;
        this.basicStatsKind = basicStatsKind;
        this.multiplierType = multiplierType;
        this.value = value;
        SetEffect();
    }
    public SkillPassiveEffect(SKILL skill, long requiredLevel, Stats statsKind, MultiplierType multiplierType, double value)
    {
        effectKind = SkillPassiveEffectKind.HeroStats;
        this.skill = skill;
        this.level = skill.level;
        this.requiredLevel = requiredLevel;
        this.statsKind = statsKind;
        this.multiplierType = multiplierType;
        this.value = value;
        SetEffect();
    }
    public SkillPassiveEffect(SKILL skill, long requiredLevel, GlobalStats globalStatsKind, MultiplierType multiplierType, double value)
    {
        effectKind = SkillPassiveEffectKind.GlobalStats;
        this.skill = skill;
        this.level = skill.level;
        this.requiredLevel = requiredLevel;
        this.globalStatsKind = globalStatsKind;
        this.multiplierType = multiplierType;
        this.value = value;
        SetEffect();
    }
    public SkillPassiveEffect(SKILL skill, long requiredLevel, string description, Action<Func<bool>> registerInfo = null)
    {
        effectKind = SkillPassiveEffectKind.Others;
        this.skill = skill;
        this.level = skill.level;
        this.requiredLevel = requiredLevel;
        this.description = description;
        if (registerInfo != null) registerInfo(IsActivated);
    }
    public bool IsActivated()//最初はそのClassでのみ有効。（後から追加で別Classも）
    {
        return IsEnoughLevel();
        //return skill.heroKind == game.currentHero && IsEnoughLevel();
    }
    public bool IsEnoughLevel()
    {
        //RB4以降のアップグレード
        //if() return level.maxReachedLevel >= requiredLevel;
        return level.value >= requiredLevel;
    }
    public void SetEffect()
    {
        MultiplierInfo multiInfo;
        switch (effectKind)
        {
            case SkillPassiveEffectKind.BasicStats:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    multiInfo = new MultiplierInfo(MultiplierKind.SkillPassive, multiplierType, () => EffectValue((HeroKind)count), IsActivated);
                    game.statsCtrl.BasicStats((HeroKind)count, basicStatsKind).RegisterMultiplier(multiInfo);
                }
                //全部のClassに有効になると↓のようにする
                //game.statsCtrl.SetEffect(basicStatsKind, multiInfo);
                break;
            case SkillPassiveEffectKind.HeroStats:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    multiInfo = new MultiplierInfo(MultiplierKind.SkillPassive, multiplierType, () => EffectValue((HeroKind)count), IsActivated);
                    game.statsCtrl.HeroStats((HeroKind)count, statsKind).RegisterMultiplier(multiInfo);
                }
                //game.statsCtrl.SetEffect(statsKind, multiInfo);
                break;
            case SkillPassiveEffectKind.GlobalStats:
                Func<double> effectValue = () =>
                {
                    if (skill.heroKind == game.currentHero) return value;
                    return value * game.skillCtrl.skillPassiveShareFactors[(int)skill.heroKind].Value();
                };
                multiInfo = new MultiplierInfo(MultiplierKind.SkillPassive, multiplierType, effectValue, () => IsActivated());
                game.statsCtrl.globalStats[(int)globalStatsKind].RegisterMultiplier(multiInfo);
                //if (skill.heroKind == game.currentHero)
                //{
                //    multiInfo = new MultiplierInfo(MultiplierKind.SkillPassive, multiplierType, () => value, () => IsActivated());
                //    game.statsCtrl.globalStats[(int)globalStatsKind].RegisterMultiplier(multiInfo);
                //}
                //else
                //{
                //    multiInfo = new MultiplierInfo(MultiplierKind.SkillPassive, multiplierType, () => value * game.skillCtrl.skillPassiveShareFactors[(int)skill.heroKind].Value(), () => IsActivated());
                //    game.statsCtrl.globalStats[(int)globalStatsKind].RegisterMultiplier(multiInfo);
                //}

                //全部のClass有効にする場合はこのIf文を消す
                //if (skill.heroKind == game.currentHero)
                //    game.statsCtrl.globalStats[(int)globalStatsKind].RegisterMultiplier(multiInfo);
                break;
        }
    }

    double EffectValue(HeroKind heroKind)
    {
        double tempValue = value;
        if (heroKind != skill.heroKind)
            tempValue *= game.skillCtrl.skillPassiveShareFactors[(int)skill.heroKind].Value();
        return tempValue;
    }
}

public class SkillProficiency : EXP
{
    public SkillProficiency(HeroKind heroKind, int id, Func<long, double> requiredProficiency, INTEGER level)
    {
        this.heroKind = heroKind;//このHeroKindは、スキルのHeroKind
        this.id = id;
        this.requiredValue = requiredProficiency;
        this.level = level;
    }

    //このheroKindは、誰が発動したか。（Global Skill Slotの時も、スキルを実際に使用したクラス）
    public void Increase(double increment, HeroKind heroKind)
    {
        if (heroKind != this.heroKind)//GlobalSlotで発動した場合
        {
            increment *= game.skillCtrl.globalSkillProfGainRate.Value();
        }
        increment *= game.guildCtrl.Member(heroKind).gainRate;
        increment *= game.statsCtrl.HeroStats(this.heroKind, Stats.SkillProficiencyGain).Value();
        base.Increase(increment);
    }

    HeroKind heroKind;
    int id;
    public override double value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.warriorSkillProficiency[id];
                case HeroKind.Wizard:
                    return main.SR.wizardSkillProficiency[id];
                case HeroKind.Angel:
                    return main.SR.angelSkillProficiency[id];
                case HeroKind.Thief:
                    return main.SR.thiefSkillProficiency[id];
                case HeroKind.Archer:
                    return main.SR.archerSkillProficiency[id];
                case HeroKind.Tamer:
                    return main.SR.tamerSkillProficiency[id];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.warriorSkillProficiency[id] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.wizardSkillProficiency[id] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.angelSkillProficiency[id] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.thiefSkillProficiency[id] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.archerSkillProficiency[id] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.tamerSkillProficiency[id] = value;
                    break;
            }
        }
    }
}

public class SkillRank : INTEGER
{
    public SkillRank(HeroKind heroKind, int id, Func<long> maxRank)
    {
        this.heroKind = heroKind;
        this.id = id;
        maxValue = maxRank;
    }
    HeroKind heroKind;
    int id;
    public override long value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.warriorSkillRank[id];
                case HeroKind.Wizard:
                    return main.SR.wizardSkillRank[id];
                case HeroKind.Angel:
                    return main.SR.angelSkillRank[id];
                case HeroKind.Thief:
                    return main.SR.thiefSkillRank[id];
                case HeroKind.Archer:
                    return main.SR.archerSkillRank[id];
                case HeroKind.Tamer:
                    return main.SR.tamerSkillRank[id];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.warriorSkillRank[id] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.wizardSkillRank[id] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.angelSkillRank[id] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.thiefSkillRank[id] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.archerSkillRank[id] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.tamerSkillRank[id] = value;
                    break;
            }
        }
    }
}

public class SkillLevel : INTEGER
{
    public SkillLevel(HeroKind heroKind, int id, Func<long> maxLevel)
    {
        this.heroKind = heroKind;
        this.id = id;
        maxValue = maxLevel;
    }
    public HeroKind heroKind;
    int id;
    public override void Increase(long increment)
    {
        base.Increase(increment);
        maxReachedLevel = Math.Max(maxReachedLevel, value);
    }
    public override long value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.warriorSkillLevel[id];
                case HeroKind.Wizard:
                    return main.SR.wizardSkillLevel[id];
                case HeroKind.Angel:
                    return main.SR.angelSkillLevel[id];
                case HeroKind.Thief:
                    return main.SR.thiefSkillLevel[id];
                case HeroKind.Archer:
                    return main.SR.archerSkillLevel[id];
                case HeroKind.Tamer:
                    return main.SR.tamerSkillLevel[id];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.warriorSkillLevel[id] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.wizardSkillLevel[id] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.angelSkillLevel[id] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.thiefSkillLevel[id] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.archerSkillLevel[id] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.tamerSkillLevel[id] = value;
                    break;
            }
        }
    }
    public long maxReachedLevel//SkillPassive解放に使う（RB4以降）
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.warriorMaxReachedSkillLevel[id];
                case HeroKind.Wizard:
                    return main.SR.wizardMaxReachedSkillLevel[id];
                case HeroKind.Angel:
                    return main.SR.angelMaxReachedSkillLevel[id];
                case HeroKind.Thief:
                    return main.SR.thiefMaxReachedSkillLevel[id];
                case HeroKind.Archer:
                    return main.SR.archerMaxReachedSkillLevel[id];
                case HeroKind.Tamer:
                    return main.SR.tamerMaxReachedSkillLevel[id];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.warriorMaxReachedSkillLevel[id] = value;
                    break;
                case HeroKind.Wizard:
                    main.SR.wizardMaxReachedSkillLevel[id] = value;
                    break;
                case HeroKind.Angel:
                    main.SR.angelMaxReachedSkillLevel[id] = value;
                    break;
                case HeroKind.Thief:
                    main.SR.thiefMaxReachedSkillLevel[id] = value;
                    break;
                case HeroKind.Archer:
                    main.SR.archerMaxReachedSkillLevel[id] = value;
                    break;
                case HeroKind.Tamer:
                    main.SR.tamerMaxReachedSkillLevel[id] = value;
                    break;
            }
        }
    }

}

//統計
public class SkillTriggeredNum : NUMBER
{
    public SkillTriggeredNum(HeroKind heroKind, int id)
    {
        this.heroKind = heroKind;
        this.id = id;
    }
    HeroKind heroKind;
    int id;
    public override double value {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.S.warriorSkillTriggeredNum[id];
                case HeroKind.Wizard:
                    return main.S.wizardSkillTriggeredNum[id];
                case HeroKind.Angel:
                    return main.S.angelSkillTriggeredNum[id];
                case HeroKind.Thief:
                    return main.S.thiefSkillTriggeredNum[id];
                case HeroKind.Archer:
                    return main.S.archerSkillTriggeredNum[id];
                case HeroKind.Tamer:
                    return main.S.tamerSkillTriggeredNum[id];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.S.warriorSkillTriggeredNum[id] = value;
                    break;
                case HeroKind.Wizard:
                    main.S.wizardSkillTriggeredNum[id] = value;
                    break;
                case HeroKind.Angel:
                    main.S.angelSkillTriggeredNum[id] = value;
                    break;
                case HeroKind.Thief:
                    main.S.thiefSkillTriggeredNum[id] = value;
                    break;
                case HeroKind.Archer:
                    main.S.archerSkillTriggeredNum[id] = value;
                    break;
                case HeroKind.Tamer:
                    main.S.tamerSkillTriggeredNum[id] = value;
                    break;
            }
        }
    }
}
