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
    public double nitro;
    public double nitroConsumed;//合計使用量

    public double nitroSpeed;//1.5~3.0 0.1単位
}

public class NitroController
{
    public Multiplier nitroCap;
    public Nitro nitro;
    public Multiplier maxNitroSpeed;
    public NitroController()
    {
        nitroCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 5000));
        nitroCap.Calculate();
        nitro = new Nitro(() => nitroCap.Value());
        maxNitroSpeed = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 2));
        speed = new NitroSpeed(() => maxNitroSpeed.Value(), () => 1.5d);
        //DebugSpeed
        maxNitroSpeed.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => nitroTimescale - 2));
        ////Debug:Playtest
        //if (main.S.isPlaytestBeforeJune10) nitroTimescale = 3.0f;
    }
    public NitroSpeed speed;
    public bool isActive;
    public float nitroTimescale = 2.0f;
    double lastTime;
    public double nitroConsumption => speed.value - 1;
    public void SwitchActive()
    {
        isActive = !isActive;
        if (nitro.value <= 0) isActive = false;
        if (isActive)
        {
            nitro.Decrease(nitroConsumption);
            lastTime = main.allTimeRealtime;
            Time.timeScale = (float)speed.value;
        }
        else Time.timeScale = 1.0f;
        if (switchUIAction != null) switchUIAction();
    }
    public Action switchUIAction;
    float tempNitroSpeed;
    public void Update()
    {
        if (!isActive) return;
        if (main.allTimeRealtime >= lastTime + 1)
        {
            tempNitroSpeed = (float)speed.value;
            lastTime = main.allTimeRealtime;
            Time.timeScale = tempNitroSpeed;
            nitro.Decrease(nitroConsumption);
            main.S.nitroConsumed += nitroConsumption;
            if (nitro.value <= 0) SwitchActive();
        }
    }
}

public class NitroSpeed : NUMBER
{
    public override double value { get => main.S.nitroSpeed; set => main.S.nitroSpeed = value; }
    public NitroSpeed(Func<double> maxValue = null, Func<double> minValue = null) : base(maxValue, minValue)
    {
    }
}

public class Nitro : NUMBER
{
    public override double value { get => main.S.nitro; set => main.S.nitro = value; }
    public Nitro(Func<double> maxValue)
    {
        this.maxValue = maxValue;
    }
    //EpicStoreのNitroやDLCのニトロはCapを越して獲得できるようにした。一度に最大で獲得できる量がmaxValue（Cap)
    public void IncreaseWithoutLimit(double increment)
    {
        increment = Math.Min(increment, maxValue());
        value += increment;
        RegisterGainPerSec(Math.Min(increment, maxValue()));
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
    }
    public override void Increase(double increment)
    {
        if (value >= maxValue()) return;//既に超過している場合は何もしない
        value += increment;
        RegisterGainPerSec(Math.Min(increment, maxValue()));
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());
    }

    public override void Decrease(double decrement)
    {
        value -= decrement;
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
    }
    //EpicStoreのNitroはこれを呼ぶ
    public void ToMaxWithoutLimit()
    {
        IncreaseWithoutLimit(maxValue());
    }
}
