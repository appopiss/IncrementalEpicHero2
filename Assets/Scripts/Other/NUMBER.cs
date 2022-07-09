using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UsefulMethod;
using static Localized;

public class NUMBER
{
    public virtual double value { get; set; }
    public Func<double> maxValue = () => 1e301d;
    public Func<double> minValue = () => 0;
    public virtual string Name() { return ""; }//表示名
    public double[] gainPerSec = new double[61];
    public ArrayId gainId;

    public NUMBER(Func<double> maxValue = null, Func<double> minValue = null)
    {
        if (maxValue != null) this.maxValue = maxValue;
        if (minValue != null) this.minValue = minValue;
        gainId = new ArrayId(0, () => gainPerSec.Length);
    }

    public virtual double Percent()
    {
        if (maxValue() <= 0 || value <= 0) return 0;
        return value / maxValue();
    }
    public virtual string Description(bool isPercent = false)
    {
        if(isPercent) return tDigit(value) + " / " + maxValue() + " ( " + Percent() + " )";
        return tDigit(value) + " / " + maxValue();
    }

    public double TotalGainInLastMinute()
    {
        double tempValue = 0;
        for (int i = 0; i < gainPerSec.Length; i++)
        {
            if (i != gainId.value)
                tempValue += gainPerSec[i];
        }
        return tempValue;
    }

    public void ChangeCountGainperSec()
    {
        gainId.Increase();
        gainPerSec[gainId.value] = 0;
    }
    public void RegisterGainPerSec(double increment)
    {
        gainPerSec[gainId.value] += increment;
    }

    public virtual void Increase(double increment)
    {
        value += increment;
        RegisterGainPerSec(Math.Min(increment, maxValue()));
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());
    }

    public virtual void Decrease(double decrement)
    {
        value -= decrement;
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());
    }

    public void ChangeValue(double toValue)
    {
        value = toValue;
        if (Double.IsInfinity(value)) value = maxValue();
        if (Double.IsNaN(value)) value = minValue();
        value = Math.Max(value, minValue());
        value = Math.Min(value, maxValue());
    }

    public virtual void ToMax()
    {
        value = maxValue();
    }

    public bool IsMaxed()
    {
        return IsMaxed(0);
    }

    public bool IsMaxed(double increment)
    {
        return value + increment >= maxValue();
    }

}

////1e307の壁を突破できるNUMBER
//public class LARGENUMBER : NUMBER
//{
//    public virtual double 
//}


public class INTEGER
{
    public virtual long value { get; set; }
    public Func<long> maxValue = () => 9223372036854775807;
    public Func<long> minValue = () => 0;

    public virtual void Increase(long increment)
    {
        value += increment;
        if (value == Mathf.Infinity) value = maxValue();
        value = (long)Mathf.Clamp(value, minValue(), maxValue());
    }

    public virtual void ChangeValue(long toValue)
    {
        value = toValue;
        if (value == Mathf.Infinity) value = maxValue();
        value = (long)Mathf.Clamp(value, minValue(), maxValue());
    }

    public void ToMax()
    {
        value = maxValue();
    }

    public bool IsMaxed()
    {
        return IsMaxed(0);
    }

    public bool IsMaxed(long increment)
    {
        return value + increment >= maxValue();
    }

    public bool IsMined()
    {
        return IsMined(0);
    }

    public bool IsMined(long decrement)
    {
        return value - decrement <= minValue();
    }
}

public class EXP : NUMBER
{
    public INTEGER level;
    public Func<long, double> requiredValue;
    public double RequiredExp() { return requiredValue(level.value); }
    public override void Increase(double increment)
    {
        if (level.IsMaxed()) return;// && value.Equals(requiredValue(level.value))) return;

        base.Increase(increment);
        if (value < requiredValue(level.value)) return;

        long tempLevelIncrement = 0;
        long tempLevel = level.value;
        while (true)
        {
            if (value < requiredValue(tempLevel)) break;

            if (tempLevel >= level.maxValue())
            {
                ChangeValue(requiredValue(tempLevel));
                break;
            }

            base.Decrease(requiredValue(tempLevel));
            tempLevel++;
            tempLevelIncrement++;
        }
        level.Increase(tempLevelIncrement);
    }
    public override double Percent()
    {
        if (maxValue() <= 0 || value <= 0) return 0;
        return value / RequiredExp();
    }
    public override string Description(bool isPercent = false)
    {
        if (isPercent) return tDigit(value) + " / " + tDigit(RequiredExp()) + " ( " + percent(Percent()) + " )";
        return tDigit(value) + " / " + tDigit(RequiredExp());
    }

}

public class PROGRESS : NUMBER
{
    public NUMBER point;
    public Func<double> requiredValue;
    public Action<long> pointIncreaseAction;
    public override void Increase(double increment)
    {
        if (point.IsMaxed()) return;

        base.Increase(increment);
        if (value < requiredValue()) return;

        long tempPointIncrement = 0;
        while (true)
        {
            if (value < requiredValue()) break;

            if (point.IsMaxed())
            {
                ChangeValue(requiredValue());
                break;
            }

            base.Decrease(requiredValue());
            tempPointIncrement++;
        }
        point.Increase(tempPointIncrement);
        if (pointIncreaseAction != null) pointIncreaseAction(tempPointIncrement);
    }
}


public class ArrayId : INTEGER
{
    public ArrayId(int initId, Func<long> arrayLength)
    {
        if (initId >= 0)
            value = initId;
        this.maxValue = arrayLength;
    }
    public void Increase()
    {
        value = value >= maxValue() - 1 ? 0 : value + 1;

        if (changeAction != null) changeAction();
        if (increaseAction != null) increaseAction();
    }
    public void Decrease()
    {
        value = value <= 0 ? maxValue() - 1 : value - 1;

        if (changeAction != null) changeAction();
        if (decreaseAction != null) decreaseAction();
    }
    public Action increaseAction;
    public Action decreaseAction;
    public Action changeAction;
}

public enum MultiplierType
{
    Add,
    Mul
}
public enum MultiplierKind
{
    Base,
    Ability,
    Title,
    Quest,
    Skill,
    ChanneledSkill,
    SkillPassive,
    Stance,
    Upgrade,
    Town,
    TownResearch,
    Equipment,
    Dictionary,
    Alchemy,
    AlchemyExpand,
    Potion,
    Pet,
    Guild,
    Rebirth,
    Challenge,
    Expedition,
    MissionMilestone,
    AreaPrestige,
    Ascension,
    AreaDebuff,
    Blessing,
    Buff,
    Debuff,
    Talisman,
    TalismanPassive,
    Achievement,
    EpicStore,
    DLC,
}
public class MultiplierInfo
{
    public Func<double> multiplier { get; }
    public Func<bool> trigger { get; }
    public MultiplierType type { get; }
    public MultiplierKind kind { get; }
    public MultiplierInfo(MultiplierKind kind, MultiplierType type, Func<double> multiplier, Func<bool> trigger = null)
    {
        this.kind = kind;
        this.multiplier = multiplier;
        this.trigger = trigger == null ? () => true : trigger;
        this.type = type;
    }

}
[Serializable]
public class Multiplier
{
    public Multiplier()
    {

    }
    public Multiplier(bool isAlwaysCalculated = false)
    {
        this.isAlwaysCalculated = isAlwaysCalculated;
    }
    public Multiplier(Func<double> maxValue = null, Func<double> minValue = null)
    {
        if (maxValue != null) this.maxValue = maxValue;
        if (minValue != null) this.minValue = minValue;
    }
    public Multiplier(MultiplierInfo multiplierInfo = null, Func<double> maxValue = null, Func<double> minValue = null)
    {
        if (multiplierInfo != null) RegisterMultiplier(multiplierInfo);
        if (maxValue != null) this.maxValue = maxValue;
        if (minValue != null) this.minValue = minValue;
    }

    public void RegisterMultiplier(MultiplierInfo multiplierInfo)
    {
        if (multiplierInfo.type == MultiplierType.Add)
        {
            //if (multiplierInfo.multiplier() > 1e100) Debug.Log("Too big Add value : " + multiplierInfo.kind.ToString() + " " + multiplierInfo.multiplier());
            AddMultiplierKind.Add(multiplierInfo.kind);
            AddMultiplierValue.Add(() =>
            {
                if (!multiplierInfo.trigger())
                    return 0;

                return multiplierInfo.multiplier();
            });
        }
        if (multiplierInfo.type == MultiplierType.Mul)
        {
            //if (multiplierInfo.multiplier() > 100) Debug.Log("Too big Mul value : " + multiplierInfo.kind.ToString() + " " + multiplierInfo.multiplier());
            MulMultiplierKind.Add(multiplierInfo.kind);
            MulMultiplierValue.Add(() =>
            {
                if (!multiplierInfo.trigger())
                    return 0;

                return multiplierInfo.multiplier();
            });
        }
    }
    public bool IsMaxed() { return Add() * Mul() >= maxValue(); }
    public double Value(double initValue = 0)
    {
        return Math.Max(minValue(), Math.Min(maxValue(), (initValue + Add()) * Mul()));
    }
    public string ValueString(int decimalPoint = 0, bool isPercent = false, bool isColor = false)
    {
        string tempStr = "";
        if (isColor)
        {
            if (Mul(MultiplierKind.Debuff) < 1 || Add(MultiplierKind.AreaDebuff) < 0) tempStr += "<color=red>";
            else if (Mul(MultiplierKind.Blessing) > 1) tempStr += "<color=green>";
        }
        if (isPercent) return tempStr + percent(Value(), decimalPoint) + "</color>";
        return tempStr + tDigit(Value(), decimalPoint) + "</color>";
    }

    public void Calculate()
    {
        for (int i = 0; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            int count = i;
            CalculateAdd((MultiplierKind)count);
            CalculateMul((MultiplierKind)count);
        }
        CalculateAdd();
        CalculateMul();
    }
    public void CalculateAdd()
    {
        double temp = 0;
        for (int i = 0; i < AddMultiplierValue.Count; i++)
        {
            temp += AddMultiplierValue[i]();
        }
        lastCalculatedAddTime = allTime;
        calculatedAddValue = temp;
    }
    public void CalculateAdd(MultiplierKind kind)
    {
        double temp = 0;
        for (int i = 0; i < AddMultiplierValue.Count; i++)
        {
            if (AddMultiplierKind[i] == kind) temp += AddMultiplierValue[i]();
        }
        lastCalculatedAddTimes[(int)kind] = allTime;
        calculatedAddValues[(int)kind] = temp;
    }
    public void CalculateMul()
    {
        double temp = 1d;
        for (int i = 0; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            temp *= Mul((MultiplierKind)i);
        }
        lastCalculatedMulTime = allTime;
        calculatedMulValue = temp;
    }
    public void CalculateMul(MultiplierKind kind)
    {
        double temp = 1d;
        for (int i = 0; i < MulMultiplierValue.Count; i++)
        {
            //同一Kind内は足し算になる
            if (MulMultiplierKind[i] == kind) temp += MulMultiplierValue[i]();
        }
        lastCalculatedMulTimes[(int)kind] = allTime;
        calculatedMulValues[(int)kind] = temp;
    }

    bool isAlwaysCalculated;//常に計算しなくてはならないもの
    //この値は、BackGroundで稼働するHeroは60secとかにする（基礎Statsのみ）
    static double randomTimesec = UnityEngine.Random.Range(0f, 0.25f);
    public double calculateSpanModifier = 0;//特に重い値（SkillLevelなど参照が多いもの）は計算スパンを長く設定する
    public double calculateSpanTimesec => Main.main.calculateSpanTimeSec + randomTimesec + calculateSpanModifier;
    double allTime => 1 + Main.main.allTime;
    double calculatedMulValue = 0;
    double lastCalculatedMulTime = 0;
    public double Mul()
    {
        if (!isAlwaysCalculated && lastCalculatedMulTime + calculateSpanTimesec > allTime) return calculatedMulValue;
        CalculateMul();
        return calculatedMulValue;
    }
    double[] calculatedMulValues = new double[Enum.GetNames(typeof(MultiplierKind)).Length];
    double[] lastCalculatedMulTimes = new double[Enum.GetNames(typeof(MultiplierKind)).Length];
    public double Mul(MultiplierKind kind)
    {
        if (!isAlwaysCalculated && lastCalculatedMulTimes[(int)kind] + calculateSpanTimesec * 2 > allTime) return calculatedMulValues[(int)kind];
        CalculateMul(kind);
        return calculatedMulValues[(int)kind];
    }

    double calculatedAddValue = 0;
    double lastCalculatedAddTime = 0;
    public double Add()
    {
        if (!isAlwaysCalculated && lastCalculatedAddTime + calculateSpanTimesec > allTime) return calculatedAddValue;
        CalculateAdd();
        return calculatedAddValue;
    }
    double[] calculatedAddValues = new double[Enum.GetNames(typeof(MultiplierKind)).Length];
    double[] lastCalculatedAddTimes = new double[Enum.GetNames(typeof(MultiplierKind)).Length];
    public double Add(MultiplierKind kind)
    {
        if (!isAlwaysCalculated && lastCalculatedAddTimes[(int)kind] + calculateSpanTimesec * 2 > allTime) return calculatedAddValues[(int)kind];
        CalculateAdd(kind);
        return calculatedAddValues[(int)kind];
    }

    private readonly List<MultiplierKind> AddMultiplierKind = new List<MultiplierKind>();
    private readonly List<Func<double>> AddMultiplierValue = new List<Func<double>>();
    private readonly List<MultiplierKind> MulMultiplierKind = new List<MultiplierKind>();
    private readonly List<Func<double>> MulMultiplierValue = new List<Func<double>>();
    public Func<double> maxValue = () => 1e300d;
    public Func<double> minValue = () => 0;

    public string BreakdownString(bool isPercent = false)
    {
        NumberType numberType = isPercent ? NumberType.percent : NumberType.normal;
        string tempStr = optStr;
        tempStr += optStr + "Additive (Total : " + tDigit(Add(), numberType) + ")";
        if (Add(MultiplierKind.Base) != 0)
            tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base) + " : " + tDigit(Add(MultiplierKind.Base), numberType);
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (Add((MultiplierKind)i) != 0)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : + " + tDigit(Add((MultiplierKind)i), numberType);
        }
        tempStr += optStr + "\nMultiplicative (Total : " + percent(Mul()) + ")";
        for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        {
            if (Mul((MultiplierKind)i) != 1)
                tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : * " + percent(Mul((MultiplierKind)i), 3);
        }
        tempStr += optStr + "\nTotal : " + tDigit(Value(), numberType);
        return tempStr;
    }
}
