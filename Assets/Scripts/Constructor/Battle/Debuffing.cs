using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static UsefulMethod;

public class Debuffing
{
    public Debuffing(BATTLE battle, Debuff debuff)
    {
        this.battle = battle;
        this.debuff = debuff;
    }
    BATTLE battle;
    public Debuff debuff;
    public bool isDebuff => !IsTimeOver();
    public void StartDebuff()
    {
        if (debuff == Debuff.Knockback) return;
        if (debuff == Debuff.Death) return;
        startTime = battle.battleCtrl.timecount;
    }
    public void Cure()
    {
        startTime = -60;
    }
    public float startTime = -60;
    public double duration
    {
        get
        {
            if (debuff == Debuff.Stop) return 1;
            return 10;
        }
    }
    public float ElapsedTime(float timecount) { return timecount - startTime; }
    public float TimeLeft() { return Mathf.Max(0, (float)duration - ElapsedTime(battle.battleCtrl.timecount)); }
    public bool IsTimeOver() { return ElapsedTime(battle.battleCtrl.timecount) > duration; }

    //これはHeroBattleからのみ呼ぶ
    public void SetEffect()
    {
        switch (debuff)
        {
            case Debuff.AtkDown:
                game.statsCtrl.BasicStats(battle.heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                break;
            case Debuff.MatkDown:
                game.statsCtrl.BasicStats(battle.heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                break;
            case Debuff.DefDown:
                game.statsCtrl.BasicStats(battle.heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                break;
            case Debuff.MdefDown:
                game.statsCtrl.BasicStats(battle.heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                break;
            case Debuff.SpdDown:
                game.statsCtrl.BasicStats(battle.heroKind, BasicStatsKind.SPD).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                game.statsCtrl.HeroStats(battle.heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Mul, () => -0.50d, () => isDebuff));
                break;
            case Debuff.FireResDown:
                game.statsCtrl.HeroStats(battle.heroKind, Stats.FireRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Add, () => -1.00d, () => isDebuff));
                break;
            case Debuff.IceResDown:
                game.statsCtrl.HeroStats(battle.heroKind, Stats.IceRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Add, () => -1.00d, () => isDebuff));
                break;
            case Debuff.ThunderResDown:
                game.statsCtrl.HeroStats(battle.heroKind, Stats.ThunderRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Add, () => -1.00d, () => isDebuff));
                break;
            case Debuff.LightResDown:
                game.statsCtrl.HeroStats(battle.heroKind, Stats.LightRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Add, () => -1.00d, () => isDebuff));
                break;
            case Debuff.DarkResDown:
                game.statsCtrl.HeroStats(battle.heroKind, Stats.DarkRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Debuff, MultiplierType.Add, () => -1.00d, () => isDebuff));
                break;
        }
    }

    public string NameString()
    {
        return Localized.localized.DebuffName(debuff);
    }
    public string EffectString()
    {
        switch (debuff)
        {
            case Debuff.AtkDown:
                return Localized.localized.BasicStats(BasicStatsKind.ATK) + " -50.00%";
            case Debuff.MatkDown:
                return Localized.localized.BasicStats(BasicStatsKind.MATK) + " -50.00%";
            case Debuff.DefDown:
                return Localized.localized.BasicStats(BasicStatsKind.DEF) + " -50.00%";
            case Debuff.MdefDown:
                return Localized.localized.BasicStats(BasicStatsKind.MDEF) + " -50.00%";
            case Debuff.SpdDown:
                return Localized.localized.BasicStats(BasicStatsKind.SPD) + " and Move Speed -50.00%";
            case Debuff.Electric:
                return "Receive additional 10% damage";
            case Debuff.Poison:
                return "Receive " + tDigit(battle.poisonDamagePerSec, 1) + " damage every sec";
            case Debuff.FireResDown:
                return Localized.localized.Stat(Stats.FireRes) + " -100.00%";
            case Debuff.IceResDown:
                return Localized.localized.Stat(Stats.IceRes) + " -100.00%";
            case Debuff.ThunderResDown:
                return Localized.localized.Stat(Stats.ThunderRes) + " -100.00%";
            case Debuff.LightResDown:
                return Localized.localized.Stat(Stats.LightRes) + " -100.00%";
            case Debuff.DarkResDown:
                return Localized.localized.Stat(Stats.DarkRes) + " -100.00%";
            case Debuff.Stop:
                return "Attacks and Movement stops for 1 second";
            case Debuff.Death:
                return "Instantly killed";
            case Debuff.Knockback:
                return "Knocked back and stunned for 0.5 second";
            case Debuff.Gravity:
                return "Pulled toward the center of the field";
        }
        return "";
    }
}
