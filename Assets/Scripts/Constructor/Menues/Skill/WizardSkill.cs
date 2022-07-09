using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UsefulMethod;
using static GameController;
using Cysharp.Threading.Tasks;


public class WizardSkill : ClassSkill
{
    public override HeroKind heroKind => HeroKind.Wizard;
    public WizardSkill()
    {
        skills[0] = new StaffAttack(HeroKind.Wizard, 0);
        skills[1] = new FireBolt(HeroKind.Wizard, 1);
        skills[2] = new FireStorm(HeroKind.Wizard, 2);
        skills[3] = new MeteorStrike(HeroKind.Wizard, 3);
        skills[4] = new IceBolt(HeroKind.Wizard, 4);
        skills[5] = new ChillingTouch(HeroKind.Wizard, 5);
        skills[6] = new Blizzard(HeroKind.Wizard, 6);
        skills[7] = new ThunderBolt(HeroKind.Wizard, 7);
        skills[8] = new DoubleThunderBolt(HeroKind.Wizard, 8);
        skills[9] = new LightningThunder(HeroKind.Wizard, 9);

        stances = new Stance[Enum.GetNames(typeof(WizardStanceKind)).Length];
        stances[0] = new NullStance(HeroKind.Wizard, 0);
        stances[1] = new FireStance(HeroKind.Wizard, 1);
        stances[2] = new IceStance(HeroKind.Wizard, 2);
        stances[3] = new ThunderStance(HeroKind.Wizard, 3);
    }
}
public enum WizardStanceKind
{
    Nothing,
    Fire,
    Ice,
    Thunder,
}
public class FireStance : Stance
{
    public FireStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
}
public class IceStance : Stance
{
    public IceStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
}
public class ThunderStance : Stance
{
    public ThunderStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
}

//Wizard
public class StaffAttack : SKILL
{
    public StaffAttack(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, GlobalStats.CrystalGain, MultiplierType.Mul, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, GlobalStats.CrystalGain, MultiplierType.Mul, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, GlobalStats.CrystalGain, MultiplierType.Mul, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, GlobalStats.CrystalGain, MultiplierType.Mul, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.SkillProficiencyGain, MultiplierType.Add, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, Stats.SkillProficiencyGain, MultiplierType.Add, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.SkillProficiencyGain, MultiplierType.Add, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.SkillProficiencyGain, MultiplierType.Add, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
        if (rank.value <= 0) rank.ChangeValue(1);
    }
    public override Element element
    {
        get
        {
            switch ((WizardStanceKind)classSkill.currentStanceId)
            {
                case WizardStanceKind.Nothing://Fireにした。
                    return Element.Fire;
                case WizardStanceKind.Fire:
                    return Element.Fire;
                case WizardStanceKind.Ice:
                    return Element.Ice;
                case WizardStanceKind.Thunder:
                    return Element.Thunder;
            }
            return Element.Fire;
        }
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
public class FireBolt : SKILL
{
    public FireBolt(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 3));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.MagCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.DEF, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MDEF, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, Stats.MagCritChance, MultiplierType.Add, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.MagCritChance, MultiplierType.Add, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MATK, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Fire;
}
public class FireStorm : SKILL
{
    public FireStorm(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.StaffAttack, 24);
        requiredSkills.Add((int)SkillKindWizard.FireBolt, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.DEF, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MDEF, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Effect Range + " + meter(30)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Effect Range + " + meter(50)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Fire;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 10);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (animationEffectUIAction != null)
            animationEffectUIAction(attackLists[(int)heroKind][0], AnimationEffectKind.FireStorm);
    }
    public override float EffectRange()
    {
        if (level.value >= 150) return base.EffectRange() + 80;
        if (level.value >= 50) return base.EffectRange() + 30;
        return base.EffectRange();
    }
}

public class MeteorStrike : SKILL
{
    public MeteorStrike(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.FireBolt, 90);
        requiredSkills.Add((int)SkillKindWizard.FireStorm, 60);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.MagCritChance, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MATK, MultiplierType.Mul, 0.20d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MP, MultiplierType.Add, 1000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.75d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 1.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Fire;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 60);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.MeteorStrike);
    }
}


public class IceBolt : SKILL
{
    public IceBolt(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.FireBolt, 60);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.HP, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.DEF, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MDEF, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.HP, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 0.500d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Ice;
    public override Debuff debuff => Debuff.SpdDown;
}
public class ChillingTouch : SKILL
{
    public ChillingTouch(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.StaffAttack, 120);
        requiredSkills.Add((int)SkillKindWizard.IceBolt, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "This skill's Effect Range + " + meter(70)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Effect Range + " + meter(150)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MDEF, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.DEF, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.075d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MP, MultiplierType.Mul, 1.000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Ice;
    public override Debuff debuff => Debuff.Stop;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 30);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.ChillingTouch);
    }
    public override float EffectRange()
    {
        if (level.value >= 100) return base.EffectRange() + 220f;
        if (level.value >= 25) return base.EffectRange() + 70f;
        return base.EffectRange();
    }
}

public class Blizzard : SKILL
{
    public Blizzard(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.IceBolt, 120);
        requiredSkills.Add((int)SkillKindWizard.ChillingTouch, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, "This skill's Effect Range + " + meter(50)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.MagCritChance, MultiplierType.Add, 0.05d)); ;
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.MagCritChance, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.HP, MultiplierType.Add, 2000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.MagCritChance, MultiplierType.Add, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's MP Consumpotion -50%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Add, 3000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 3.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override double ConsumeMp()
    {
        if (level.value >= 200) return base.ConsumeMp() * 0.5d;
        return base.ConsumeMp();
    }
    public override float EffectRange()
    {
        if (level.value >= 100) return base.EffectRange() + 250;
        if (level.value >= 50) return base.EffectRange() + 150;
        if (level.value >= 20) return base.EffectRange() + 50;
        return base.EffectRange();
    }
    public override Element element => Element.Ice;
    public override Debuff debuff => Debuff.SpdDown;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Field;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 30);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.Blizzard);
    }
}


public class ThunderBolt : SKILL
{
    public ThunderBolt(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.StaffAttack, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.SPD, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.MagCritChance, MultiplierType.Add, 0.025));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's MP Gain + 100%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MATK, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Thunder;
    public override Debuff debuff => Debuff.Electric;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;

    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
    }
    public override double GainMp()
    {
        if (level.value >= 100) return base.GainMp() * 2;
        return base.GainMp();
    }
}

public class DoubleThunderBolt : SKILL
{
    public DoubleThunderBolt(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.StaffAttack, 90);
        requiredSkills.Add((int)SkillKindWizard.ThunderBolt, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.SPD, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.MoveSpeed, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, GlobalStats.GoldGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.HP, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Interval -50%"));// BasicStatsKind.MDEF, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.075d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 250));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));

    }
    public override double Interval()
    {
        if (level.value >= 150) return base.Interval() * 0.5d;
        return base.Interval();
    }
    public override Element element => Element.Thunder;
    public override Debuff debuff => Debuff.Electric;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;

    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => myself.RandomTarget().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => myself.RandomTarget().move.position));
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
        if (isUI && effectUIAction != null) effectUIAction(AttackList(battle.battleCtrl)[0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id]);
        if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame(3);
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, null, false);
        if (isUI && effectUIAction != null) effectUIAction(AttackList(battle.battleCtrl)[1], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id]);
    }
    public override void Attack(BATTLE battle)
    {
        //AttackList(battle.battleCtrl)[0].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
    }
}


public class LightningThunder : SKILL
{
    public LightningThunder(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindWizard.ThunderBolt, 150);
        requiredSkills.Add((int)SkillKindWizard.DoubleThunderBolt, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MATK, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Thunder;
    public override Debuff debuff => Debuff.Electric;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 30);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.LightningThunder);
    }
}
