using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;
using System.Threading.Tasks;
using static MultiplierKind;
using static MultiplierType;

public class BlessingController
{
    public BlessingController(BattleController battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        blessings.Add(new Blessing_Hp(battleCtrl));
        blessings.Add(new Blessing_Atk(battleCtrl));
        blessings.Add(new Blessing_MAtk(battleCtrl));
        blessings.Add(new Blessing_MoveSpeed(battleCtrl));
        blessings.Add(new Blessing_SkillProficiency(battleCtrl));
        blessings.Add(new Blessing_EquipProficiency(battleCtrl));
        blessings.Add(new Blessing_GoldGain(battleCtrl));
        blessings.Add(new Blessing_ExpGain(battleCtrl));
    }
    public BattleController battleCtrl;
    public List<BLESSING> blessings = new List<BLESSING>();
    public BLESSING Blessing(BlessingKind kind) { return blessings[(int)kind]; }
    public Func<Task> selectBlessingTask;
}

public class BlessingInfoController
{
    public BlessingInfoController()
    {
        effectMultiplier = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        for (int i = 0; i < effectMultipliers.Length; i++)
        {
            effectMultipliers[i] = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        }
        duration = new Multiplier(new MultiplierInfo(Base, Add, () => 3 * 60));
    }
    public Multiplier effectMultiplier;
    public Multiplier[] effectMultipliers = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier duration;
}

public class Blessing_Hp : BLESSING
{
    public override BlessingKind kind => BlessingKind.Hp;
    public Blessing_Hp(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.BasicStats(battleCtrl.heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
        game.statsCtrl.HpRegenerate(battleCtrl.heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Add, () => subEffectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.5d * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override double subEffectValue => 20* info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "HP Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply HP by " + percent(1 + effectValue, 0);
    }
    public override string SubEffectString()
    {
        return optStr + "Regenerate HP + " + tDigit(subEffectValue, 1) + " / sec";
    }
}
public class Blessing_Atk : BLESSING
{
    public override BlessingKind kind => BlessingKind.Atk;
    public Blessing_Atk(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.BasicStats(battleCtrl.heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.5d * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "ATK Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply ATK by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_MAtk : BLESSING
{
    public override BlessingKind kind => BlessingKind.MAtk;
    public Blessing_MAtk(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.BasicStats(battleCtrl.heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.5d * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "MATK Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply MATK by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_MoveSpeed : BLESSING
{
    public override BlessingKind kind => BlessingKind.MoveSpeed;
    public Blessing_MoveSpeed(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.25d * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "Move Speed Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply Move Speed by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_SkillProficiency : BLESSING
{
    public override BlessingKind kind => BlessingKind.SkillProficiency;
    public Blessing_SkillProficiency(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.25 * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "Skill Proficiency Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply Skill Proficiency Gain by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_EquipProficiency : BLESSING
{
    public override BlessingKind kind => BlessingKind.EquipProficiency;
    public Blessing_EquipProficiency(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.25 * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "Equipment Proficiency Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply Equipment Proficiency Gain by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_GoldGain : BLESSING
{
    public override BlessingKind kind => BlessingKind.GoldGain;
    public Blessing_GoldGain(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.5 * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "Gold Gain Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply Gold Gain by " + percent(1 + effectValue, 0);
    }
}
public class Blessing_ExpGain : BLESSING
{
    public override BlessingKind kind => BlessingKind.ExpGain;
    public Blessing_ExpGain(BattleController battleCtrl) : base(battleCtrl)
    {
    }
    public override void SetEffect()
    {
        game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Blessing, MultiplierType.Mul, () => effectValue, () => !IsTimeOver()));
    }
    public override double effectValue => 0.5 * info.effectMultiplier.Value() * info.effectMultipliers[(int)battleCtrl.heroKind].Value();
    public override string NameString()
    {
        return "EXP Gain Blessing";
    }
    public override string EffectString()
    {
        return optStr + "Multiply EXP Gain by " + percent(1 + effectValue, 0);
    }
}


public class BLESSING
{
    public BLESSING(BattleController battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        SetEffect();
    }
    public virtual void SetEffect() { }
    public virtual double effectValue { get; }
    public virtual double subEffectValue { get; }
    public void StartBlessing(float fixedDuration = 0)
    {
        if (fixedDuration > 0)
        {
            if (TimeLeft() >= fixedDuration) return;
            currentDuration = fixedDuration;
            startTime = battleCtrl.timecount + fixedDuration - (float)duration;
        }
        else
        {
            if (TimeLeft() >= duration) return;
            startTime = battleCtrl.timecount;
            currentDuration = duration;
        }
    }
    public double currentDuration;
    public float startTime = -60 * 60;
    public double duration => game.blessingInfoCtrl.duration.Value();
    public float ElapsedTime(float timecount) { return timecount - startTime; }
    public float TimeLeft() { return Mathf.Max(0, (float)duration - ElapsedTime(battleCtrl.timecount)); }
    public bool IsTimeOver() { return ElapsedTime(battleCtrl.timecount) > duration; }
    public virtual BlessingKind kind { get; }
    public BattleController battleCtrl;
    public BlessingInfoController info { get => game.blessingInfoCtrl; }
    public virtual string NameString() { return ""; }
    public virtual string EffectString() { return ""; }
    public virtual string SubEffectString() { return ""; }
}

public enum BlessingKind
{
    Hp,
    Atk,
    MAtk,
    MoveSpeed,
    SkillProficiency,
    EquipProficiency,
    GoldGain,
    ExpGain,
}
