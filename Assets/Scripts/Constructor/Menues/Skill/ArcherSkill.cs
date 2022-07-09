using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class ArcherSkill : ClassSkill
{
    public ArcherSkill()
    {
        skills[0] = new ArrowAttack(HeroKind.Archer, 0);
        skills[1] = new PiercingArrow(HeroKind.Archer, 1);
        skills[2] = new BurstArrow(HeroKind.Archer, 2);
        skills[3] = new Multishot(HeroKind.Archer, 3);
        skills[4] = new ShockArrow(HeroKind.Archer, 4);
        skills[5] = new FrozenArrow(HeroKind.Archer, 5);
        skills[6] = new ExplodingArrow(HeroKind.Archer, 6);
        skills[7] = new ShiningArrow(HeroKind.Archer, 7);
        skills[8] = new GravityArrow(HeroKind.Archer, 8);
        skills[9] = new Kiting(HeroKind.Archer, 9);
    }
}
public class ArrowAttack : SKILL
{
    public ArrowAttack(HeroKind heroKind, int id) : base(heroKind, id)
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
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(SkillEffectCenter.Myself, x, target), (x) => Damage(x) * game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value(), () => IsCrit(myself), () => 35f, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
        //SlimeBall
        if (!game.IsUI(battle.battleCtrl.heroKind) || game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value() <= 0) return;
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (effectUIActionWithDirection != null)
        {
            effectUIActionWithDirection(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][0].throwVec);
            effectUIActionWithDirection(attackLists[(int)heroKind][1], SpriteSourceUI.sprite.challengeAttackEffects[0], () => attackLists[(int)heroKind][1].throwVec);
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
}
public class PiercingArrow : SKILL
{
    public PiercingArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.PhysCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.PhysCritChance, MultiplierType.Add, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.PhysCritChance, MultiplierType.Add, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.ATK, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 0.300d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (effectUIActionWithDirection != null)
            effectUIActionWithDirection(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][0].throwVec);
    }
}

public class BurstArrow : SKILL
{
    public BurstArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.ArrowAttak, 120);
        requiredSkills.Add((int)SkillKindArcher.PiercingArrow, 120);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Physical Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.ATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, BasicStatsKind.ATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Add, 25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.ATK, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Physical Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Physical Damage + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 0.150d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Physical).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 10) tempValue += 0.20d;
        if (level.value >= 75) tempValue += 0.30d;
        if (level.value >= 175) tempValue += 0.50d;
        return tempValue;
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        for (int i = 0; i < 10; i++)
        {
            AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
        }
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        for (int i = 0; i < HitCount(); i++)
        {
            int count = i;
            AttackArray(battle.battleCtrl)[i].ThrowAttack(battle, null, false);
            if (isUI && effectUIActionWithDirection != null)
                effectUIActionWithDirection(AttackList(battle.battleCtrl)[count], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[count].throwVec);
            if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame((int)Math.Ceiling(30d / HitCount()));
            //await UniTask.DelayFrame((int)Math.Ceiling(30d / HitCount()));
        }
    }
    public override void Attack(BATTLE battle)
    {
        //あえてoverrideして中身を書かない
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
    }
}
public class Multishot : SKILL
{
    public Multishot(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.PiercingArrow, 240);
        requiredSkills.Add((int)SkillKindArcher.BurstArrow, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.ATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.ATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.ATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Mul, 0.75d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.ATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.ATK, MultiplierType.Mul, 1.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Enables a penetrating attack for this skill"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.ATK, MultiplierType.Mul, 3.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override string Description()
    {
        return "Shoot multiple arrows that target all the monsters in the field";
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        for (int i = 0; i < myself.TargetArray().Length; i++)
        {
            int count = i;
            AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => myself.TargetArray()[count].move.position));
        }
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        bool isPenetrate = level.value >= 100;
        for (int i = 0; i < AttackArray(battle.battleCtrl).Length; i++)
        {
            int count = i;
            if (battle.TargetArray()[count].isAlive)
            {
                AttackArray(battle.battleCtrl)[count].ThrowAttack(battle, null, isPenetrate);
                if (isUI && effectUIActionWithDirection != null)
                    effectUIActionWithDirection(AttackList(battle.battleCtrl)[count], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[count].throwVec);
            }
        }
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //何も書かない
    }
}


public class ShockArrow : SKILL
{
    public ShockArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.ArrowAttak, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 5));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, "Thunder Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.ThunderRes, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, Stats.MagCritChance, MultiplierType.Add, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MATK, MultiplierType.Add, 15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Thunder Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.ThunderRes, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Thunder Damage + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Thunder).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 20) tempValue += 0.20d;
        if (level.value >= 150) tempValue += 0.30d;
        if (level.value >= 250) tempValue += 0.50d;
        return tempValue;
    }

    public override Element element => Element.Thunder;
    public override Debuff debuff => Debuff.ThunderResDown;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        //ダメージなし、目的の位置まで飛ばす矢
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => x.move.position, (x) => 0, () => false, () => 150, HitCount, () => element, () => Debuff.Nothing, () => target().move.position));
        //実際の攻撃
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff()));
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        if (!battle.battleCtrl.isSimulated)
        {
            AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
            if (isUI && effectUIActionWithDirection != null)
                effectUIActionWithDirection(AttackList(battle.battleCtrl)[0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[0].throwVec);
            await UniTask.DelayFrame(5);
        }
        AttackArray(battle.battleCtrl)[1].NormalAttack(battle, 30);
        if (isUI && particleEffectUIAction != null)
            particleEffectUIAction(AttackArray(battle.battleCtrl)[1], ParticleEffectKind.ShockArrow);
    }
    public override void Attack(BATTLE battle)
    {
        //あえてoverrideして中身を書かない
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
    }
}

public class FrozenArrow : SKILL
{
    public FrozenArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.PiercingArrow, 48);
        requiredSkills.Add((int)SkillKindArcher.ShockArrow, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Ice Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.MagCritChance, MultiplierType.Add, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, Stats.IceRes, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.MDEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.DEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MDEF, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.DEF, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Ice Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.IceRes, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MDEF, MultiplierType.Mul, 0.150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.DEF, MultiplierType.Mul, 0.150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, "Ice Damage + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 0.150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override string Description()
    {
        return "Strike a penetrating ice arrow at the furthest monster";
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Ice).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 10) tempValue += 0.20d;
        if (level.value >= 100) tempValue += 0.30d;
        if (level.value >= 225) tempValue += 0.50d;
        return tempValue;
    }
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override Element element => Element.Ice;
    public override Debuff debuff => Debuff.IceResDown;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff(), () => myself.FurthestTarget().move.position));
    }
    public override void Attack(BATTLE battle)
    {
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        if (effectUIActionWithDirection != null)
            effectUIActionWithDirection(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => attackLists[(int)heroKind][0].throwVec);
    }
}

public class ExplodingArrow : SKILL
{
    public ExplodingArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.ShockArrow, 30);
        requiredSkills.Add((int)SkillKindArcher.FrozenArrow, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Fire Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, Stats.FireRes, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MATK, MultiplierType.Add, 40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, Stats.MagCritChance, MultiplierType.Add, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, "Fire Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.FireRes, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, "Fire Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Fire).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 10) tempValue += 0.20d;
        if (level.value >= 75) tempValue += 0.30d;
        if (level.value >= 175) tempValue += 0.50d;
        return tempValue;
    }

    public override Element element => Element.Fire;
    public override Debuff debuff => Debuff.FireResDown;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        //ダメージなし、目的の位置まで飛ばす矢
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => x.move.position, (x) => 0, () => false, () => 150, HitCount, () => element, () => Debuff.Nothing, () => target().move.position));
        //実際の攻撃
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, HitCount, () => element, () => LotteryDebuff()));
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        if (!battle.battleCtrl.isSimulated)
        {
            AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
            if (isUI && effectUIActionWithDirection != null)
                effectUIActionWithDirection(AttackList(battle.battleCtrl)[0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[0].throwVec);
            await UniTask.DelayFrame(5);
        }
        AttackArray(battle.battleCtrl)[1].NormalAttack(battle, 30);
        if (isUI && particleEffectUIAction != null)
            particleEffectUIAction(AttackList(battle.battleCtrl)[1], ParticleEffectKind.ExplodingArrow);
    }
    public override void Attack(BATTLE battle)
    {
        //あえてoverrideして中身を書かない
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
    }
}
public class ShiningArrow : SKILL
{
    public ShiningArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.FrozenArrow, 48);
        requiredSkills.Add((int)SkillKindArcher.ExplodingArrow, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.MATK, MultiplierType.Add, 20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Light Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, Stats.LightRes, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Light Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MP, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.HP, MultiplierType.Add, 1500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.LightRes, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Add, 1000d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Light Damage + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MATK, MultiplierType.Mul, 0.75d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.DEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MDEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 1.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Light).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 10) tempValue += 0.20d;
        if (level.value >= 50) tempValue += 0.30d;
        if (level.value >= 150) tempValue += 0.50d;
        return tempValue;
    }
    public override Element element => Element.Light;
    public override Debuff debuff => Debuff.LightResDown;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Field;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => Vector2.up * 400f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => Vector2.one * 283f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => Vector2.right * 400f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => (Vector2.right + Vector2.down) * 283f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => Vector2.down * 400f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => (Vector2.down + Vector2.left) * 283f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => Vector2.left * 400f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => (Vector2.left + Vector2.up) * 283f, (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff(), () => EffectInitPosition(effectCenter, myself, target)));
    }
    public override void Attack(BATTLE battle)
    {
        bool tempIsPenetrate = false;
        AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[2].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[3].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[4].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[5].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[6].ThrowAttack(battle, null, tempIsPenetrate);
        AttackArray(battle.battleCtrl)[7].ThrowAttack(battle, null, tempIsPenetrate);
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
public class GravityArrow : SKILL
{
    public GravityArrow(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.ExplodingArrow, 48);
        requiredSkills.Add((int)SkillKindArcher.ShiningArrow, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, "Dark Damage + 20% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, Stats.DarkRes, MultiplierType.Add, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, "Dark Damage + 30% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.DarkRes, MultiplierType.Add, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, "Dark Damage + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MP, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Hit Count + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MATK, MultiplierType.Mul, 3.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override string Description()
    {
        return "Shoots an arrow at the center that pulls all monsters on screen toward it";
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, DamageValue, () => IsEquipped(heroKind));
        game.statsCtrl.ElementDamage(heroKind, Element.Dark).RegisterMultiplier(info);
    }
    double DamageValue()
    {
        double tempValue = 0;
        if (level.value >= 5) tempValue += 0.20d;
        if (level.value >= 50) tempValue += 0.30d;
        if (level.value >= 100) tempValue += 0.50d;
        return tempValue;
    }

    public override Element element => Element.Dark;
    public override Debuff debuff => Debuff.DarkResDown;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Field;
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        //ダメージなし、目的の位置まで飛ばす矢
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => x.move.position, (x) => 0, () => false, () => 150, HitCount, () => element, () => Debuff.Nothing, () => EffectInitPosition(effectCenter, myself, target)));
        //実際の攻撃
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(effectCenter, x, target), (x) => Damage(x), () => IsCrit(myself), EffectRange, () => 1, () => element, () => LotteryDebuff()));
    }
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        if (!battle.battleCtrl.isSimulated)
        {
            AttackArray(battle.battleCtrl)[0].ThrowAttack(battle, null, false);
            if (isUI && effectUIActionWithDirection != null)
                effectUIActionWithDirection(AttackList(battle.battleCtrl)[0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id], () => AttackList(battle.battleCtrl)[0].throwVec);
            await UniTask.DelayFrame(5);
        }
        AttackArray(battle.battleCtrl)[1].LoopAttack(battle, Mathf.Min((float)Interval(), 1.5f), Mathf.Min((float)Interval(), 1.5f) / HitCount(), true);
        if (isUI && particleEffectUIAction != null)
            particleEffectUIAction(AttackList(battle.battleCtrl)[1], ParticleEffectKind.GravityArrow);
    }
    public override void Attack(BATTLE battle)
    {
        //あえてoverrideして中身を書かない
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //あえてoverrideして中身を書かない
    }
}

public class Kiting : SKILL
{
    public Kiting(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindArcher.ArrowAttak, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.DEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 15, BasicStatsKind.MDEF, MultiplierType.Add, 30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 25, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.HP, MultiplierType.Mul, 0.050d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, GlobalStats.GoldGain, MultiplierType.Add, 0.500d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, BasicStatsKind.MDEF, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.DEF, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.HP, MultiplierType.Mul, 0.100d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 160, BasicStatsKind.MP, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 180, BasicStatsKind.HP, MultiplierType.Mul, 0.200d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.SPD, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 0.500d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.MoveSpeedUp;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override string Description()
    {
        string tempStr = optStr + "Circle the edge of the field to move out of melee range of monsters with a movement penalty during Auto Move mode";
        return tempStr;
    }
    public override void SetBuff(HeroKind heroKind)
    {
        //MoveSpeed
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(info);
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return 0;
    }
}
