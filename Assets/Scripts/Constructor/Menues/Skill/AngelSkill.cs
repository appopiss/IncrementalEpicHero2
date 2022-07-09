using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public class AngelSkill : ClassSkill
{
    public override HeroKind heroKind => HeroKind.Angel;
    public AngelSkill()
    {
        skills[0] = new WingAttack(HeroKind.Angel, 0);
        skills[1] = new WingShoot(HeroKind.Angel, 1);
        skills[2] = new Heal(HeroKind.Angel, 2);
        skills[3] = new GodBless(HeroKind.Angel, 3);
        skills[4] = new MuscleInflation(HeroKind.Angel, 4);
        skills[5] = new MagicImpact(HeroKind.Angel, 5);
        skills[6] = new ProtectWall(HeroKind.Angel, 6);
        skills[7] = new Haste(HeroKind.Angel, 7);
        skills[8] = new WingStorm(HeroKind.Angel, 8);
        skills[9] = new HolyArch(HeroKind.Angel, 9);
    }
}
public class WingAttack : SKILL
{
    public WingAttack(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, GlobalStats.LeafGain, MultiplierType.Mul, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, GlobalStats.LeafGain, MultiplierType.Mul, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, GlobalStats.LeafGain, MultiplierType.Mul, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, GlobalStats.LeafGain, MultiplierType.Mul, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.SkillProficiencyGain, MultiplierType.Add, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.SkillProficiencyGain, MultiplierType.Add, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.SkillProficiencyGain, MultiplierType.Add, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.SkillProficiencyGain, MultiplierType.Add, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
        if (rank.value <= 0) rank.ChangeValue(1);
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        //一発目は一番近い敵
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff()));
        for (int i = 0; i < 4; i++)
        {
            AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => x.RandomTarget().move.position, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff()));
        }
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(SkillEffectCenter.Myself, x, target), (x) => Damage(x) * game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value(), () => IsCrit(myself), () => 35f, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        for (int i = 0; i < HitCount(); i++)
        {
            int count = i;
            AttackArray(battle.battleCtrl)[i].NormalAttack(battle);
            if (isUI && effectUIAction != null) effectUIAction(AttackList(battle.battleCtrl)[count], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id]);
            //if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame(1);
        }
        //SlimeBall
        if (!isUI || game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value() <= 0) return;
        AttackArray(battle.battleCtrl)[5].ThrowAttack(battle, null, true);
        if (isUI && effectUIAction != null)
            effectUIActionWithDirection(attackLists[(int)heroKind][5], SpriteSourceUI.sprite.challengeAttackEffects[0], () => attackLists[(int)heroKind][5].throwVec);
    }
    public override void Attack(BATTLE battle)
    {
        //AttackList(battle.battleCtrl)[0].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
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
}

public class WingShoot : SKILL
{
    public WingShoot(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.MagCritChance, MultiplierType.Add, 0.015d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, Stats.MagCritChance, MultiplierType.Add, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill fires an additional projectile"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Light;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        //一発目は一番近い敵
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
        for (int i = 0; i < 4; i++)
        {
            AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => myself.RandomTarget().move.position));
        }
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        for (int i = 0; i < HitCount(); i++)
        {
            int count = i;
            AttackArray(battle.battleCtrl)[i].ThrowAttack(battle, null, true);
            if (isUI && effectUIActionWithDirection != null)
                effectUIActionWithDirection(AttackList(battle.battleCtrl)[count], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[count].throwVec);
            if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame(1);
        }
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

public class Heal : SKILL
{
    public Heal(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.WingAttack, 24);
        requiredSkills.Add((int)SkillKindAngel.WingShoot, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.DEF, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "25% chance to double Heal Point every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.HP, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "25% chance to cure debuffs every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.HP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "5% chance to additionally restore 10% of Max HP"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "0.5% chance to Full Heal every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Heal;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        if (isDisplay) return HealPoint();
        return HealPoint() * skillAbuseMpPercents[(int)myself.heroKind];
    }
    public override void DoHeal(BATTLE battle)
    {
        if (level.value >= 50 && WithinRandom(0.25d)) battle.Heal(HealPoint() * 2);
        else battle.Heal(HealPoint() * skillAbuseMpPercents[(int)battle.heroKind]);
        if (level.value >= 100 && WithinRandom(0.25d)) battle.CureDebuff();
        if (level.value >= 150 && WithinRandom(0.05d)) battle.Heal(battle.hp * 0.10d);
        if (level.value >= 250 && WithinRandom(0.005d)) battle.FullHeal();
    }
}

public class GodBless : SKILL
{
    public GodBless(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.WingAttack, 48);
        requiredSkills.Add((int)SkillKindAngel.WingShoot, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.HP, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MDEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.DEF, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "HP Regenerate + 10.0 / sec while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.HP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "HP Regenerate + 15.0 / sec while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, GlobalStats.GoldGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "HP Regenerate + 20.0 / sec while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "HP Regenerate + 25.0 / sec while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.HpUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(info);
        //Regen
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, RegenValue, () => IsEquipped(heroKind));
        game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(info);

    }
    double RegenValue()
    {
        if (level.value < 50) return 0;
        if (level.value < 100) return 10;
        if (level.value < 150) return 25;
        if (level.value < 200) return 45;
        return 70;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class MuscleInflation : SKILL
{
    public MuscleInflation(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.WingAttack, 90);
        requiredSkills.Add((int)SkillKindAngel.GodBless, 30);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.PhysCritChance, MultiplierType.Add, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Mul, 0.025d)); ;
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.ATK, MultiplierType.Mul, 0.025d)); ;
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.HP, MultiplierType.Add, 1000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.PhysCritChance, MultiplierType.Add, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.ATK, MultiplierType.Add, 135));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 0.60d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.AtkUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;

    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(info);
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class MagicImpact : SKILL
{
    public MagicImpact(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.WingShoot, 90);
        requiredSkills.Add((int)SkillKindAngel.GodBless, 30);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.MagCritChance, MultiplierType.Add, 0.010d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MATK, MultiplierType.Mul, 0.025d)); ;
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MATK, MultiplierType.Mul, 0.025d)); ;
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.MagCritChance, MultiplierType.Add, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MATK, MultiplierType.Add, 125));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.MatkUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(info);
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class ProtectWall : SKILL
{
    public ProtectWall(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.Heal, 48);
        requiredSkills.Add((int)SkillKindAngel.GodBless, 42);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.DEF, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MDEF, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.HP, MultiplierType.Add, 2000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MP, MultiplierType.Add, 1000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.DEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MDEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Physical Nullified Chance + 2.50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.HP, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.DEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MDEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Physical Nullified Chance + 7.50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.DefMDefUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;

    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(info);
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(info);
        //PhysicalInvalid
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, InvalidValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementInvalid(heroKind, Element.Physical).RegisterMultiplier(info);

    }
    double InvalidValue()
    {
        if (level.value >= 200) return 0.100d;
        if (level.value >= 100) return 0.025d;
        return 0;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class Haste : SKILL
{
    public Haste(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.GodBless, 100);
        requiredSkills.Add((int)SkillKindAngel.ProtectWall, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.SPD, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.MoveSpeed, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.SPD, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.SPD, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.MoveSpeed, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Move Speed + 10% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.SPD, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Move Speed + 15% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.SPD, MultiplierType.Add, 350));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Move Speed + 25% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.SPD, MultiplierType.Add, 400));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.SPD, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.SpdUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).RegisterMultiplier(info);
        //MoveSpeed
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, MoveSpeedValue, () => IsEquipped(heroKind));
        game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(info);
    }
    double MoveSpeedValue()
    {
        if (level.value >= 200) return 0.50d;
        if (level.value >= 150) return 0.25d;
        if (level.value >= 100) return 0.10d;
        return 0;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class WingStorm : SKILL
{
    public WingStorm(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.WingAttack, 120);
        requiredSkills.Add((int)SkillKindAngel.WingShoot, 120);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.MP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "This skill's Effect Range + " + meter(50)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "This skill's Effect Range + " + meter(50)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MATK, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "This skill's Effect Range + " + meter(100)));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.125d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "This skill's Interval -25%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override Element element => Element.Light;
    public override Debuff debuff => Debuff.Knockback;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].NormalAttack(battle, 15);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (particleEffectUIAction != null)
            particleEffectUIAction(attackLists[(int)heroKind][0], ParticleEffectKind.WingStorm);
    }
    public override double Interval()
    {
        if (level.value >= 250) return base.Interval() * 0.75d;
        return base.Interval();
    }
    public override float EffectRange()
    {
        if (level.value >= 175) return base.EffectRange() + 200f;
        if (level.value >= 75) return base.EffectRange() + 100f;
        if (level.value >= 25) return base.EffectRange() + 50f;
        return base.EffectRange();
    }
}

public class HolyArch : SKILL
{
    public HolyArch(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindAngel.Haste, 18);
        requiredSkills.Add((int)SkillKindAngel.WingStorm, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Debuff Resistance + 10% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Debuff Resistance + 15% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Debuff Resistance + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Debuff Resistance + 25% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));

    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.SkillLevelUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, BuffPercent, () => IsEquipped(heroKind));
        game.skillCtrl.skillLevelBonusFromHolyArch[(int)heroKind].RegisterMultiplier(info);
        //DebuffRes
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, DebuffResValue, () => IsEquipped(heroKind));
        game.statsCtrl.HeroStats(heroKind, Stats.DebuffRes).RegisterMultiplier(info);
    }
    double DebuffResValue()
    {
        if (level.value >= 250) return 0.70d;
        if (level.value >= 150) return 0.45d;
        if (level.value >= 100) return 0.25d;
        if (level.value >= 50) return 0.10d;
        return 0;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent();
    }
    public override long levelBonus => (long)game.skillCtrl.skillLevelBonus[(int)heroKind].Value();
    public override long Level()
    {
        if (Rank() <= 0) return 0;
        return level.value;
    }
    public override double BuffPercent()
    {
        return 20 + IncrementBuffPercentPerLevel() * Level();
    }
    public override double IncrementBuffPercentPerLevel()
    {
        return 1 + 0.02d * Rank();
    }
}