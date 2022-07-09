using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public class WarriorSkill : ClassSkill
{
    public override HeroKind heroKind => HeroKind.Warrior;
    public WarriorSkill()
    {
        skills[0] = new SwordAttack(HeroKind.Warrior, 0);
        skills[1] = new Slash(HeroKind.Warrior, 1);
        skills[2] = new DoubleSlash(HeroKind.Warrior, 2);
        skills[3] = new SonicSlash(HeroKind.Warrior, 3);
        skills[4] = new SwingDown(HeroKind.Warrior, 4);
        skills[5] = new SwingAround(HeroKind.Warrior, 5);
        skills[6] = new ChargeSwing(HeroKind.Warrior, 6);
        skills[7] = new FanSwing(HeroKind.Warrior, 7);
        skills[8] = new ShieldAttack(HeroKind.Warrior, 8);
        skills[9] = new KnockingShot(HeroKind.Warrior, 9);

        stances = new Stance[Enum.GetNames(typeof(WarriorStanceKind)).Length];
        stances[0] = new NullStance(HeroKind.Warrior, 0);
        stances[1] = new AttackStance(HeroKind.Warrior, 1);
        stances[2] = new ReachStance(HeroKind.Warrior, 2);
        stances[3] = new KnockStance(HeroKind.Warrior, 3);
    }
}
public enum WarriorStanceKind
{
    Nothing,
    Attack,
    Reach,
    Knock,
}
public class AttackStance : Stance
{
    public AttackStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
    public override double effectValueBuff => 0.50d;

}
public class ReachStance : Stance
{
    public ReachStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
    public override double effectValueBuff => 300;
}
public class KnockStance : Stance
{
    public KnockStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
    public override double effectValueBuff => 0.5d; 
}

public class SwordAttack : SKILL
{
    public SwordAttack(HeroKind heroKind, int id) : base(heroKind, id)
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
    public override double Damage()
    {
        double tempDamage = base.Damage();
        if (classSkill.stances[(int)WarriorStanceKind.Attack].isActive)
            tempDamage *= 1 + classSkill.stances[(int)WarriorStanceKind.Attack].effectValueBuff;
        return tempDamage;
    }
    public override Debuff debuff
    {
        get
        {
            if (game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value() > 0) return Debuff.Poison;
            if (classSkill.stances[(int)WarriorStanceKind.Knock].isActive) return Debuff.Knockback;
            return Debuff.Nothing;
        }
    }
    public override double DebuffChance()
    {
        if (game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value() > 0) return game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value();
            return classSkill.stances[(int)WarriorStanceKind.Knock].effectValueBuff;
    }
    public override float Range()
    {
        if (classSkill.stances[(int)WarriorStanceKind.Reach].isActive) return (float)classSkill.stances[(int)WarriorStanceKind.Reach].effectValueBuff;
        return base.Range();
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
public class Slash : SKILL
{
    public Slash(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 3));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.HP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.HP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, Stats.PhysCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.PhysCritChance, MultiplierType.Add, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.PhysCritChance, MultiplierType.Add, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.ATK, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.ATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
}
public class DoubleSlash : SKILL
{
    public DoubleSlash(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.SwordAttack, 24);
        requiredSkills.Add((int)SkillKindWarrior.Slash, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.SPD, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.075d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
}
public class SonicSlash : SKILL
{
    public SonicSlash(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.SwordAttack, 120);
        requiredSkills.Add((int)SkillKindWarrior.DoubleSlash, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.PhysCritChance, MultiplierType.Add, 0.025));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 10);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.SonicSlash);
    }
}
public class SwingDown : SKILL
{
    public SwingDown(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.Slash, 60);
        requiredSkills.Add((int)SkillKindWarrior.DoubleSlash, 30);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's MP Consumption -50%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Debuff debuff => Debuff.AtkDown;
    public override double ConsumeMp()
    {
        if (level.value >= 50) return base.ConsumeMp() * 0.5d;
        return base.ConsumeMp();
    }
}
public class SwingAround : SKILL
{
    public SwingAround(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.DoubleSlash, 60);
        requiredSkills.Add((int)SkillKindWarrior.SwingDown, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.MoveSpeed, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Interval -50%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Add, 750d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Effect Range + " + meter(150)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.75d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Remove this skill's MP Consumption"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Add, 125));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 1.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override float EffectRange()
    {
        if (level.value >= 150) return base.EffectRange() + 250f;
        if (level.value >= 50) return base.EffectRange() + 100f;
        return base.EffectRange();
    }
    public override double Interval()
    {
        if (level.value >= 100) return base.Interval() * 0.5d;
        return base.Interval();
    }
    public override double ConsumeMp()
    {
        if (level.value >= 200) return 0;
        return base.ConsumeMp();
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 15);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.SwingAround);
    }
}
public class ChargeSwing : SKILL
{
    public ChargeSwing(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.DoubleSlash, 120);
        requiredSkills.Add((int)SkillKindWarrior.SwingDown, 60);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.PhysCritChance, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, "This skill's Effect Range + " + meter(70)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.HP, MultiplierType.Add, 5000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.PhysCritChance, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Range + " + meter(30)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.PhysCritChance, MultiplierType.Add, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Interval -25%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 3.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Debuff debuff => Debuff.MatkDown;
    public override float Range()
    {
        if (level.value >= 100) return base.Range() + 30;
        return base.Range();
    }
    public override float EffectRange()
    {
        if (level.value >= 150) return base.EffectRange() + 170f;
        if (level.value >= 30) return base.EffectRange() + 70f;
        return base.EffectRange();
    }
    public override double Interval()
    {
        if (level.value >= 200) return base.Interval() * 0.75d;
        return base.Interval();
    }
}
public class FanSwing : SKILL
{
    public FanSwing(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.SonicSlash, 90);
        requiredSkills.Add((int)SkillKindWarrior.SwingAround, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.SPD, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 20);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.FanSwing);
    }
}
public class ShieldAttack : SKILL
{
    public override string Description()
    {
        return "Makes a running dash toward the target monster to attack.";
    }
    public ShieldAttack(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.SwordAttack, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.DEF, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MDEF, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's damage + 50% per dash meter"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.DEF, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MDEF, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's damage + 50% per dash meter"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 160, BasicStatsKind.DEF, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 180, BasicStatsKind.MDEF, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.HP, MultiplierType.Add, 1500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    double DamageBonusPermeter()
    {
        double tempValue = 0;
        if (level.value >= 50) tempValue += 0.50d;
        if (level.value >= 100) tempValue += 0.50d;
        return tempValue;
    }
    public override double Damage()
    {
        return base.Damage() * (1 + DamageBonusPermeter() * dushMeter);
    }
    double dushMeter = 0;
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        dushMeter = vectorAbs(battle.Target().move.position - battle.move.position) / 100d;
        for (int i = 0; i < 100; i++)
        {
            battle.Move(battle.Target().move.position - battle.move.position, battle.battleCtrl.deltaTime * 5);
            if (battle.IsWithinRange(battle, battle.Target(), 80)) break;
            if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame(1);
        }
        base.Attack(battle);
        if (isUI) base.ShowEffectUI(battle.heroKind);
        //dushMeter = 0;
    }
    public override void Attack(BATTLE battle)
    {
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
    }
}
public class KnockingShot : SKILL
{
    public KnockingShot(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWarrior.SwordAttack, 90);
        requiredSkills.Add((int)SkillKindWarrior.ShieldAttack, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.HP, MultiplierType.Add, 250));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, BasicStatsKind.MDEF, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.DEF, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.HP, MultiplierType.Add, 2500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, GlobalStats.GoldGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MDEF, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.DEF, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.MDEF, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.DEF, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 160, BasicStatsKind.MDEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 180, BasicStatsKind.DEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.HP, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.500d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 1.000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Debuff debuff => Debuff.Knockback;
}
