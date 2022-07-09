using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;

public class ThiefSkill : ClassSkill
{
    public ThiefSkill()
    {
        skills[0] = new DaggerAttack(HeroKind.Thief, 0);
        skills[1] = new Stab(HeroKind.Thief, 1);
        skills[2] = new KnifeToss(HeroKind.Thief, 2);
        skills[3] = new LuckyBlow(HeroKind.Thief, 3);
        skills[4] = new SpreadToss(HeroKind.Thief, 4);
        skills[5] = new ShadowStrike(HeroKind.Thief, 5);
        skills[6] = new SneakyStrike(HeroKind.Thief, 6);
        skills[7] = new Pilfer(HeroKind.Thief, 7);
        skills[8] = new DarkWield(HeroKind.Thief, 8);
        skills[9] = new Assassination(HeroKind.Thief, 9);
    }
}

public class DaggerAttack : SKILL
{
    public DaggerAttack(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, GlobalStats.StoneGain, MultiplierType.Mul, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, GlobalStats.StoneGain, MultiplierType.Mul, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, GlobalStats.StoneGain, MultiplierType.Mul, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, GlobalStats.StoneGain, MultiplierType.Mul, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.SkillProficiencyGain, MultiplierType.Add, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, Stats.SkillProficiencyGain, MultiplierType.Add, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.SkillProficiencyGain, MultiplierType.Add, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.SkillProficiencyGain, MultiplierType.Add, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
        if (rank.value <= 0) rank.ChangeValue(1);
    }
    public override Debuff debuff
    {
        get
        {
            if (game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value() > 0) return Debuff.Poison;
            return Debuff.Nothing;
        }
    }
    public override double DebuffChance()
    {
        return game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value();
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        base.SetAttack(battleCtrl, myself, target, targetList);
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(SkillEffectCenter.Myself, x, target), (x) => Damage(x) * game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value(), () => IsCrit(myself), () => 35f, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        base.Attack(battle);
        //SlimeBall
        if (!game.IsUI(battle.battleCtrl.heroKind) || game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value() <= 0) return;
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //UI
        if (effectUIAction != null)
        {
            effectUIAction(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id]);
            effectUIActionWithDirection(attackLists[(int)heroKind][1], SpriteSourceUI.sprite.challengeAttackEffects[0], () => attackLists[(int)heroKind][1].throwVec);
        }
    }
}
public class Stab : SKILL
{
    public Stab(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 3));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.HP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.PhysCritChance, MultiplierType.Add, 0.005d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.PhysCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.PhysCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.ATK, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.ATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Debuff debuff => Debuff.DefDown;
}
public class KnifeToss : SKILL
{
    public KnifeToss(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.Stab, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.ATK, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 0.0750d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        //bool tempIsPenetrate = false;
        //if (level.value >= 50) tempIsPenetrate = true;
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (effectUIActionWithDirection != null)
            effectUIActionWithDirection(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][0].throwVec);
    }
}

public class LuckyBlow : SKILL
{
    public LuckyBlow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.DaggerAttack, 120);
        requiredSkills.Add((int)SkillKindThief.Stab, 120);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.PhysCritChance, MultiplierType.Add, 0.025));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "Physical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Physical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Physical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Physical Critical Chance + 10% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.PhysCritChance, MultiplierType.Add, 0.050));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.SPD, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override string Description()
    {
        return "This skill's critical chance : " + percent(ThisSkillCriticalChance());
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, CriticalValue, () => IsEquipped(heroKind));
        game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).RegisterMultiplier(info);
    }
    public override float EffectRange()
    {
        if (level.value >= 100) return base.EffectRange() + 100f;
        return base.EffectRange();
    }
    double ThisSkillCriticalChance()
    {
        return Math.Min(1.0d, 0.25d + 0.0025d * Level());
    }
    double CriticalValue()
    {
        double tempValue = 0;
        if (level.value >= 25) tempValue += 0.05d;
        if (level.value >= 50) tempValue += 0.05d;
        if (level.value >= 75) tempValue += 0.05d;
        if (level.value >= 150) tempValue += 0.10d;
        return tempValue;
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCritical(myself), EffectRange, HitCount, () => element, () => LotteryDebuff()));
    }
    bool IsCritical(BATTLE myself)
    {
        return IsCrit(myself) || WithinRandom(ThisSkillCriticalChance());
    }
}
public class SpreadToss : SKILL
{
    public SpreadToss(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.KnifeToss, 60);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.PhysCritChance, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "Enables a penetrating attack for this skill"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.SPD, MultiplierType.Add, 250));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.HP, MultiplierType.Add, 1000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.SPD, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.PhysCritChance, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.SPD, MultiplierType.Add, 1000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 1.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        bool tempIsPenetrate = false;
        if (level.value >= 25) tempIsPenetrate = true;
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, () => Vector2.up, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, () => Vector2.right, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[2].ThrowAttack(battle, () => Vector2.down, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[3].ThrowAttack(battle, () => Vector2.left, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[4].ThrowAttack(battle, () => Vector2.up + Vector2.right, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[5].ThrowAttack(battle, () => Vector2.down + Vector2.right, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[6].ThrowAttack(battle, () => Vector2.up + Vector2.left, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[7].ThrowAttack(battle, () => Vector2.down + Vector2.left, tempIsPenetrate);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (effectUIActionWithDirection != null)
        {
            effectUIActionWithDirection(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][0].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][1], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][1].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][2], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][2].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][3], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][3].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][4], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][4].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][5], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][5].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][6], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][6].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][7], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][7].throwVec);
        }
    }

}


public class ShadowStrike : SKILL
{
    public ShadowStrike(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.DaggerAttack, 24);
        requiredSkills.Add((int)SkillKindThief.Stab, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.SPD, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's damage + 100%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MATK, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's damage + 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's damage + 400%")); 
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.SPD, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's damage + 800%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Dark;
    public override Debuff debuff => Debuff.Poison;
    double tempMul;
    public override double Damage()
    {
        tempMul = 1;
        if (level.value >= 50) tempMul += 1;
        if (level.value >= 100) tempMul += 2;
        if (level.value >= 150) tempMul += 4;
        if (level.value >= 200) tempMul += 8;
        return base.Damage() * tempMul;
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 30);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.ShadowStrike);
    }
}
public class SneakyStrike : SKILL
{
    public SneakyStrike(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.Stab, 48);
        requiredSkills.Add((int)SkillKindThief.ShadowStrike, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.MagCritChance, MultiplierType.Add, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Magical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, "Magical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Magical Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.MoveSpeed, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, "Magical Critical Chance + 10% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override string Description()
    {
        return "Disappear briefly and reappear at the furthest monster to deal damage";
    }
    public override void Attack(BATTLE battle)
    {
        //瞬時に最も遠いTargetへ移動する
        battle.move.MoveTo(battle.FurthestTarget().move.position + Vector2.up + Parameter.randomVec[UnityEngine.Random.Range(0, Parameter.randomVec.Length)] * 50f, true); 
        base.Attack(battle);
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, CriticalValue, () => IsEquipped(heroKind));
        game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).RegisterMultiplier(info);
    }
    double CriticalValue()
    {
        double tempValue = 0;
        if (level.value >= 75) tempValue += 0.05d;
        if (level.value >= 125) tempValue += 0.05d;
        if (level.value >= 175) tempValue += 0.05d;
        if (level.value >= 225) tempValue += 0.10d;
        return tempValue;
    }
    public override Element element => Element.Dark;
    public override Debuff debuff => Debuff.MdefDown;
}
public class Pilfer : SKILL
{
    public Pilfer(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.DaggerAttack, 90);
        requiredSkills.Add((int)SkillKindThief.SneakyStrike, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MDEF, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, Stats.EquipmentDropChance, MultiplierType.Add, 0.0005d));//0.05%
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.DEF, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.DEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, "Steal Chance + 5%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.HP, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, GlobalStats.GoldGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MP, MultiplierType.Add, 250d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Steal Chance + 10%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.EquipmentDropChance, MultiplierType.Add, 0.0005d));//0.05%
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Steal Chance + 20%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.EquipmentDropChance, MultiplierType.Add, 0.001d));//0.1%
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Steal Chance + 50%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Dark;
    public override string Description()
    {
        return percent(PilferChance()) + " chance to steal a loot material every trigger";
    }
    public override void Attack(BATTLE battle)
    {
        battle.Target().Pilfer(PilferChance() * skillAbuseMpPercents[(int)battle.heroKind]);
        base.Attack(battle);
    }
    double PilferChance()
    {
        double tempValue = 0.05d;
        if (level.value >= 50) tempValue += 0.05d;
        if (level.value >= 100) tempValue += 0.10d;
        if (level.value >= 150) tempValue += 0.20d;
        if (level.value >= 250) tempValue += 0.50d;
        return tempValue;
    }
}
public class DarkWield : SKILL
{
    public DarkWield(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.ShadowStrike, 60);
        requiredSkills.Add((int)SkillKindThief.SneakyStrike, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.MagCritChance, MultiplierType.Add, 0.050));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MATK, MultiplierType.Add, 75));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MATK, MultiplierType.Add, 125));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.HP, MultiplierType.Add, 1500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.SPD, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Add, 500d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Effect Range + " + meter(150)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.75d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.DEF, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MDEF, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Dark;
    public override Debuff debuff => Debuff.Knockback;
    public override float EffectRange()
    {
        if (level.value >= 150) return base.EffectRange() + 250f;
        if (level.value >= 50) return base.EffectRange() + 100f;
        return base.EffectRange();
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 10);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.DarkWield);
    }
}
public class Assassination : SKILL
{
    public Assassination(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindThief.LuckyBlow, 90);
        requiredSkills.Add((int)SkillKindThief.Pilfer, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.MagCritChance, MultiplierType.Add, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, Stats.MagCritChance, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.MagCritChance, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Mul, 1.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Dark;
    public override Debuff debuff => Debuff.Death;
    public override string Description()
    {
        return "This skill's critical chance : " + percent(ThisSkillCriticalChance());
    }
    double ThisSkillCriticalChance()
    {
        return Math.Min(1.0d, 0.25d + 0.0025d * Level());
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCritical(myself), EffectRange, HitCount, () => element, () => LotteryDebuff()));
    }
    bool IsCritical(BATTLE myself)
    {
        return IsCrit(myself) || WithinRandom(ThisSkillCriticalChance());
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.Assassination);
    }
}
