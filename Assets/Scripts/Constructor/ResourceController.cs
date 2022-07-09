using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;

public partial class Save
{
    public double totalGold;//通算獲得量
    public double totalStone;
    public double totalCrystal;
    public double totalLeaf;
    public double totalSlimeCoin;
}
public partial class SaveR
{
    public double gold;
    public double slimeCoin;
    public double[] resources;
}
public class ResourceController
{
    public Multiplier goldCap;
    public Multiplier slimeCoinCap;
    public Multiplier slimeCoinEfficiency;
    public Multiplier slimeCoinInterest;//perMin
    public Gold gold;
    public SlimeCoin slimeCoin;
    public Resource[] resources = new Resource[Enum.GetNames(typeof(ResourceKind)).Length];
    public ResourceController()
    {
        goldCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1000));
        slimeCoinCap = new Multiplier();// new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 10000));
        gold = new Gold(() => goldCap.Value());
        slimeCoin = new SlimeCoin(() => slimeCoinCap.Value());
        slimeCoinEfficiency = new Multiplier();// new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0.01d));
        slimeCoinInterest = new Multiplier(() => 0.50d, () => 0);
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i] = new Resource((ResourceKind)i);
        }
    }

    public ResourceKind HeroResourceKind(HeroKind heroKind)
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
    public Resource HeroResource(HeroKind heroKind)
    {
        return resources[(int)HeroResourceKind(heroKind)];
    }
    public Resource Resource(ResourceKind kind) { return resources[(int)kind]; }

    public float GoldPercent()
    {
        return (float)(gold.value / goldCap.Value());
    }
    float timecountMin, timecountSec, timecount10Sec;
    public void Update()
    {
        timecountSec += Time.deltaTime;
        if (timecountSec >= 1)
        {
            gold.ChangeCountGainperSec();
            Resource(ResourceKind.Stone).ChangeCountGainperSec();
            Resource(ResourceKind.Crystal).ChangeCountGainperSec();
            Resource(ResourceKind.Leaf).ChangeCountGainperSec();
            //HeroExp
            for (int i = 0; i < game.statsCtrl.heroes.Length; i++)
            {
                HeroKind heroKind = (HeroKind)i;
                game.statsCtrl.Exp(heroKind).ChangeCountGainperSec();
            }
            timecountMin += timecountSec;
            timecount10Sec += timecountSec;
            timecountSec += timecountSec;
            timecountSec = 0;
            if (timecount10Sec >= 10)//10秒ごと
            {
                slimeCoin.ChangeCountGainperSec();
                timecount10Sec = 0;
            }
            if (timecountMin >= 60 * 10)//１分ごと->10分ごとにした
            {
                slimeCoin.Increase(slimeCoin.value * slimeCoinInterest.Value(), true);
                timecountMin = 0;
            }
        }
    }
}

public class Resource : NUMBER
{
    public override double value { get => main.SR.resources[(int)kind]; set => main.SR.resources[(int)kind] = value; }
    public ResourceKind kind;
    public override string Name()
    {
        return Localized.localized.ResourceName(kind);
    }
    public Resource(ResourceKind kind)
    {
        this.kind = kind;
    }
    public void Increase(double increment, HeroKind heroKind)
    {
        increment *= game.guildCtrl.Member(heroKind).gainRate;
        base.Increase(increment);
        if (logUIAction != null && game.IsUI(heroKind)) logUIAction(increment);
        if (game.IsUI(heroKind)) game.battleCtrl.areaBattle.resources[(int)kind] += increment;
        //if (resultUIAction != null && game.IsUI(heroKind)) resultUIAction(increment);
        switch (kind)
        {
            case ResourceKind.Stone: main.S.totalStone += increment; break;
            case ResourceKind.Crystal: main.S.totalCrystal += increment; break;
            case ResourceKind.Leaf: main.S.totalLeaf += increment; break;
        }
    }
    public Action<double> logUIAction;
    //public Action<double> resultUIAction;
}
public class Gold : NUMBER
{
    public override double value { get => main.SR.gold; set => main.SR.gold = value; }
    public override string Name()
    {
        return Localized.localized.Basic(BasicWord.Gold);
    }
    public Gold(Func<double> maxValue)
    {
        this.maxValue = maxValue;
    }
    public void Increase(double increment, HeroKind heroKind)
    {
        increment *= game.statsCtrl.GoldGain().Value();
        increment += game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();

        //GuildMember(Passive)のGainRate
        increment *= game.guildCtrl.Member(heroKind).gainRate;

        value += increment;
        RegisterGainPerSec(increment);
        if (value == Mathf.Infinity) value = maxValue();
        value = Math.Max(value, minValue());
        if (value > maxValue())
        {
            game.resourceCtrl.slimeCoin.Increase(value - maxValue());
            value = Math.Min(value, maxValue());
        }
        if (logUIAction != null && game.IsUI(heroKind)) logUIAction(increment);
        if (game.IsUI(heroKind)) game.battleCtrl.areaBattle.gold += increment;
        //if (resultUIAction != null && game.IsUI(heroKind)) resultUIAction(increment);

        main.S.totalGold += increment;
    }
    public double MultipliedValue(double baseValue)
    {
        double tempValue = baseValue;
        tempValue *= game.statsCtrl.GoldGain().Value();
        tempValue += game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();
        return tempValue;
    }
    public override void Increase(double increment)
    {
        //increment *= game.statsCtrl.GoldGain().Value();
        //increment += game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();
        value += MultipliedValue(increment);
        RegisterGainPerSec(increment);
        if (value == Mathf.Infinity) value = maxValue();
        value = Math.Max(value, minValue());
        if (value > maxValue())
        {
            game.resourceCtrl.slimeCoin.Increase(value - maxValue());
            value = Math.Min(value, maxValue());
        }

        main.S.totalGold += increment;
    }
    public void IncreaseWithoutMultiplier(double increment)
    {
        value += increment;
        RegisterGainPerSec(increment);
        if (value == Mathf.Infinity) value = maxValue();
        value = Math.Max(value, minValue());
        if (value > maxValue())
        {
            game.resourceCtrl.slimeCoin.Increase(value - maxValue());
            value = Math.Min(value, maxValue());
        }

        main.S.totalGold += increment;
    }
    public Action<double> logUIAction;
    //public Action<double> resultUIAction;
}
public class SlimeCoin : NUMBER
{
    public override double value { get => main.SR.slimeCoin; set => main.SR.slimeCoin = value; }
    public override string Name()
    {
        return "Slime Coin";
    }
    public SlimeCoin(Func<double> maxValue)
    {
        this.maxValue = maxValue;
    }
    public override void Increase(double increment)
    {
        increment *= game.resourceCtrl.slimeCoinEfficiency.Value();
        base.Increase(increment);
        main.S.totalSlimeCoin += increment;
    }
    public void Increase(double increment, bool isInterest)
    {
        if (isInterest)
        {
            base.Increase(increment);
            main.S.totalSlimeCoin += increment;
        }
        else Increase(increment);
    }
}




public class ResourceGenerator : DROP_GENERATOR
{
    public ResourceGenerator(HeroKind heroKind) : base(heroKind)
    {
    }

    public override void Drop(ResourceKind kind, double num, Vector2 position)
    {
        num *= game.statsCtrl.ResourceGain(kind).Value();
        this.num += num;
        this.kind = kind;
        if (isAutoGet)
        {
            Get();
            return;
        }
        Vector2 tempPosition = position + randomVec[UnityEngine.Random.Range(0, randomVec.Length)] * 50f * UnityEngine.Random.Range(1, 4);
        if (tempPosition.x > moveRangeX / 2f || tempPosition.x < -moveRangeX / 2f || tempPosition.y > moveRangeY / 2f || tempPosition.y < -moveRangeY / 2f)
            tempPosition = Vector2.zero;
        this.position = tempPosition;
        if (game.IsUI(heroKind) && dropUIAction != null) dropUIAction();
    }

    public override void Get()
    {
        game.resourceCtrl.Resource(kind).Increase(num, heroKind);
        Initialize();
    }
    public ResourceKind kind;
    public override bool isAutoGet => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetResource);
}
