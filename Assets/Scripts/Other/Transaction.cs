using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static UsefulMethod;

public partial class SaveR
{
    public int multibuyId;
}

public class SpecifiedTransaction
{
    public SpecifiedTransaction(INTEGER level, long maxLevel = 0)
    {
        this.level = level;
        for (int i = 0; i < Math.Max(maxLevel, level.maxValue()); i++)
        {
            costsList.Add(new List<SpecifiedTransactionCost>());
            additionalBuyConditionsList.Add(new List<Func<bool>>());
        }
    }
    public void AddCost(long level, NUMBER resource, double value)
    {
        for (int i = 0; i < level + 1; i++)
        {
            if (costsList.Count >= level + 1) break;
            costsList.Add(new List<SpecifiedTransactionCost>());
        }
        costsList[(int)level].Add(new SpecifiedTransactionCost(resource, value));
    }
    public INTEGER level;
    public List<List<SpecifiedTransactionCost>> costsList = new List<List<SpecifiedTransactionCost>>();
    private List<List<Func<bool>>> additionalBuyConditionsList = new List<List<Func<bool>>>();
    public void SetAdditionalBuyCondition(long level, Func<bool> condition)
    {
        for (int i = 0; i < level + 1; i++)
        {
            if (additionalBuyConditionsList.Count >= level + 1) break;
            additionalBuyConditionsList.Add(new List<Func<bool>>());
        }
        additionalBuyConditionsList[(int)level].Add(condition);
    }

    public bool CanBuy()
    {
        if (level.IsMaxed()) return false;
        for (int i = 0; i < additionalBuyConditionsList[(int)level.value].Count; i++)
        {
            if (!additionalBuyConditionsList[(int)level.value][i]()) return false;
        }
        for (int i = 0; i < costsList[(int)level.value].Count; i++)
        {
            if (!costsList[(int)level.value][i].CanBuy()) return false;
        }
        return true;
    }
    public void Buy()
    {
        if (!CanBuy()) return;
        for (int i = 0; i < costsList[(int)level.value].Count; i++)
        {
            costsList[(int)level.value][i].Buy();
        }
        level.Increase(1);
    }

    public string DescriptionString()
    {
        string tempStr = optStr;
        for (int i = 0; i < costsList[(int)level.value].Count; i++)
        {
            tempStr += "- " + costsList[(int)level.value][i].resource.Name();
            tempStr += " : ";
            if (costsList[(int)level.value][i].CanBuy()) tempStr += "<color=green>";
            else tempStr += "<color=red>";
            tempStr += tDigit(costsList[(int)level.value][i].resource.value) + "</color> / ";
            tempStr += tDigit(costsList[(int)level.value][i].value);
            tempStr += "\n";
        }
        return tempStr;
    }
}
public class SpecifiedTransactionCost
{
    public SpecifiedTransactionCost(NUMBER resource, double value)
    {
        this.resource = resource;
        this.value = value;
    }
    public NUMBER resource;
    public double value;
    public bool CanBuy()
    {
        if (resource.value < value) return false;
        return true;
    }
    public void Buy()
    {
        resource.Decrease(value);
    }
}

public class Transaction
{
    public INTEGER level;
    private Func<bool> additionalBuyCondition;
    private Func<long, bool> additionalBuyConditionWithLevelIncrement;
    public List<TransactionCost> cost = new List<TransactionCost>();
    public Action<long> additionalBuyActionWithLevelIncrement;
    public Action additionalBuyAction;
    public bool isBuyOne = false;
    public Func<bool> isOnBuyOneToggle = () => false;

    public Transaction(INTEGER level)
    {
        this.level = level;
    }
    public Transaction(INTEGER level, NUMBER resource, Func<double> initCost, Func<double> baseCost, bool isLinear = false)
    {
        this.level = level;
        cost.Add(new TransactionCost(level, resource, initCost, baseCost, isLinear));
    }
    public Transaction(INTEGER level, NUMBER resource, Func<long, double> cost)
    {
        this.level = level;
        this.cost.Add(new TransactionCost(level, resource, cost));
    }
    public void SetAnotherResource(NUMBER resource, Func<double> initCost, Func<double> baseCost, bool isLinear = false)
    {
        cost.Add(new TransactionCost(level, resource, initCost, baseCost, isLinear));
    }
    public void SetAnotherResource(NUMBER resource, Func<long, double> cost)
    {
        this.cost.Add(new TransactionCost(level, resource, cost));
    }
    public void SetAdditionalBuyCondition(Func<bool> condition)
    {
        additionalBuyCondition = condition;
    }
    public void SetAdditionalBuyCondition(Func<long, bool> condition)
    {
        additionalBuyConditionWithLevelIncrement = condition;
    }

    long tempToLevel;
    public long ToLevel()
    {
        if (isBuyOne || isOnBuyOneToggle()) return level.value + 1;
        tempToLevel = 10000000000;
        for (int i = 0; i < cost.Count; i++)
        {
            tempToLevel = (long)Mathf.Min(cost[i].ToLevel(), tempToLevel);
        }
        if (additionalBuyConditionWithLevelIncrement == null) return tempToLevel;
        if (additionalBuyConditionWithLevelIncrement(tempToLevel - level.value)) return tempToLevel;
        for (int i = 1; i <= tempToLevel - level.value; i++)
        {
            if (!additionalBuyConditionWithLevelIncrement(i)) return level.value + Math.Max(1, i - 1);
        }
        return tempToLevel;
    }
    public long LevelIncrement()
    {
        return ToLevel() - level.value;
    }
    long tempLevelIncrement;
    long tempToLevelForBuy;
    public void Buy(bool isBuyOne = false, bool isPreventAdditionalAction = false)//AdditionalActionをしたくない場合はtrue
    {
        if (this.isBuyOne || isOnBuyOneToggle()) isBuyOne = true;
        if (!CanBuy(isBuyOne)) return;
        tempToLevelForBuy = ToLevel();
        if (isBuyOne)
        {
            for (int i = 0; i < cost.Count; i++)
            {
                cost[i].resource.Decrease(cost[i].Cost(level.value));
            }
            level.Increase(1);
            tempLevelIncrement = 1;
        }
        else
        {
            for (int i = 0; i < cost.Count; i++)
            {
                cost[i].resource.Decrease(cost[i].TotalCost(tempToLevelForBuy));
            }
            tempLevelIncrement = tempToLevelForBuy - level.value;
            level.ChangeValue(tempToLevelForBuy);
        }
        //買った瞬間にコスト計算する
        CalculateCost();

        if (isPreventAdditionalAction) return;
        if (additionalBuyAction != null) additionalBuyAction();
        if (additionalBuyActionWithLevelIncrement != null) additionalBuyActionWithLevelIncrement(tempLevelIncrement);
    }

    public void CalculateCost()
    {
        for (int i = 0; i < cost.Count; i++)
        {
            cost[i].CalculateCost();
        }
    }

    public bool CanBuy(bool isBuyOne = false)
    {
        if (additionalBuyCondition != null && !additionalBuyCondition()) return false;
        if (additionalBuyConditionWithLevelIncrement != null && !additionalBuyConditionWithLevelIncrement(1)) return false;
        for (int i = 0; i < cost.Count; i++)
        {
            if (!cost[i].CanBuy(isBuyOne)) return false;
        }
        return true;
    }
    public bool CanBuy(int id)
    {
        if (id >= cost.Count) id = 0;
        return cost[id].CanBuy(true);
    }
    public double Cost(bool isBuyOne = false)
    {
        return Cost(0, isBuyOne);
    }
    public double Cost(int id, bool isBuyOne = false)
    {
        if (id >= cost.Count) id = 0;
        if (isBuyOne) return cost[id].Cost();
        return cost[id].TotalCost(ToLevel());
    }
    public double TotalCostConsumed()
    {
        return TotalCostConsumed(0);
    }
    public double TotalCostConsumed(int id)
    {
        if (id >= cost.Count) id = 0;
        return cost[id].TotalCostConsumed();
    }
    public double ResourceValue()
    {
        return ResourceValue(0);
    }
    public double ResourceValue(int id)
    {
        if (id >= cost.Count) id = 0;
        return cost[id].resource.value;
    }

    public string DescriptionString()
    {
        if (level.IsMaxed()) return "Level Maxed";
        string tempStr = optStr;
        for (int i = 0; i < cost.Count; i++)
        {
            if (cost[i].TotalCost() > 0)
            {
                tempStr += "- " + cost[i].resource.Name();
                tempStr += " : ";
                if (cost[i].CanBuy()) tempStr += "<color=green>";
                else tempStr += "<color=red>";
                tempStr += tDigit(cost[i].resource.value) + "</color> / ";
                tempStr += tDigit(cost[i].TotalCost(ToLevel()));
                tempStr += "\n";
            }
        }
        return tempStr;
    }
}

public class TransactionCost
{
    public INTEGER level;
    public NUMBER resource;
    public Func<double> initCost;
    public Func<double> baseCost;
    public Func<long, double> cost;
    public bool isLinear;

    public static long MultibuyNum()
    {
        return Parameter.multibuyNums[main.SR.multibuyId];
        //return Parameter.multibuyNums[Mathf.Clamp(main.SR.multibuyId, 0, Parameter.multibuyNums.Length - 1)];
    }
    public static void SwitchMultibuyNum(bool isMinus = false)
    {
        if (isMinus)
        {
            if (main.SR.multibuyId <= 0)
                main.SR.multibuyId = Parameter.multibuyNums.Length - 1;
            else
                main.SR.multibuyId--;
            return;
        }
        if (main.SR.multibuyId >= Parameter.multibuyNums.Length - 1)
            main.SR.multibuyId = 0;
        else
            main.SR.multibuyId++;
    }

    public TransactionCost(INTEGER level, NUMBER resource, Func<double> initCost, Func<double> baseCost, bool isLinear = false)
    { 
        this.level = level;
        this.resource = resource;
        this.initCost = initCost;
        this.baseCost = baseCost;
        this.isLinear = isLinear;
    }
    public TransactionCost(INTEGER level, NUMBER resource, Func<long, double> cost)
    {
        this.level = level;
        this.resource = resource;
        this.cost = cost;
    }
    public long ToLevel()
    {
        TotalCost();
        return toLevel;
    }
    long toLevel;
    double calculatedTotalCost = 1e300d;
    double allTime => 1 + main.allTime;
    static double randomTimesec = UnityEngine.Random.Range(0f, 0.25f);

    public double calculateSpanTimesec => Main.main.calculateSpanTimeSec + randomTimesec;
    double lastCalculatedTime = 0;

    long levelIncrement;
    long tempLevel;
    double tempCost;
    double tempResourceAmount;
    double tempInitCost, tempBaseCost;
    public double TotalCost()//現在のレベルから購入可能なレベルまでのコスト
    {
        levelIncrement = LevelIncrement();
        if (levelIncrement <= 0)
        {
            toLevel = level.value;
            calculatedTotalCost = 0;
            return calculatedTotalCost;
        }
        //x50以下ならその場で計算。それ以上の場合は処理の間隔を開ける
        if (levelIncrement > 50 && lastCalculatedTime + calculateSpanTimesec > allTime)
            return calculatedTotalCost;

        CalculateCost();
        return calculatedTotalCost;
    }
    public void CalculateCost()
    {
        tempLevel = level.value;
        tempResourceAmount = resource.value;

        //線形の場合は計算が楽なので単純計算する
        if (isLinear)
        {
            tempInitCost = initCost();
            tempBaseCost = baseCost();
            if (tempBaseCost == 0)//baseCost()が0の場合は最も単純
            {
                toLevel = tempLevel + Math.Max(1, Math.Min(levelIncrement, (long)(tempResourceAmount / tempInitCost)));
                calculatedTotalCost = Math.Floor(tempInitCost * (toLevel - tempLevel));
            }
            else
            {
                //まずはlevelIncrement分のコストを計算し、resourceが足りるかどうか
                //等差数列の和の公式より iI + I(I-1)/2*b + l*I*b [i:initCost, I:levelIncrement, b:baseCost, l:現在level]
                tempCost = Math.Floor(tempInitCost * levelIncrement + 0.5d * levelIncrement * (levelIncrement - 1) * tempBaseCost + tempLevel * levelIncrement * tempBaseCost);
                if (tempResourceAmount >= tempCost)
                {
                    toLevel = tempLevel + levelIncrement;
                    calculatedTotalCost = tempCost;
                }
                else//levelIncrementまで届かない場合...通常の計算をする
                {
                    tempCost = Cost(tempLevel);
                    for (int i = 1; i <= levelIncrement; i++)
                    {
                        tempLevel = level.value + i;
                        tempCost += Cost(tempLevel);
                        if (tempResourceAmount < tempCost) break;
                    }
                    toLevel = tempLevel;
                    calculatedTotalCost = tempCost - Cost(tempLevel);
                }
            }
        }
        else
        {
            tempCost = Cost(tempLevel);
            for (int i = 1; i <= levelIncrement; i++)
            {
                tempLevel = level.value + i;
                tempCost += Cost(tempLevel);
                if (tempResourceAmount < tempCost) break;
            }
            toLevel = tempLevel;
            calculatedTotalCost = tempCost - Cost(tempLevel);
        }
        lastCalculatedTime = allTime;
    }

    double tempValue;
    public double Cost(long level)//そのレベルにおけるコスト
    {
        if (cost != null) return Math.Floor(cost(level));
        if (isLinear) return Math.Floor(initCost() + baseCost() * level);
        tempValue = initCost() * Math.Pow(baseCost(), level);
        if (tempValue >= 1e300d || Double.IsInfinity(tempValue)) return 1e300d;
        return Math.Max(1, Math.Floor(tempValue));
    }
    public double Cost()//現在のコスト
    {
        return Cost(level.value);
    }
    //0レベルからtoLevelまでのトータルコスト
    double tempCostForTotalCostFromZero;
    public double TotalCostFromZero(long toLevel)
    {
        tempCostForTotalCostFromZero = 0;
        for (int i = 0; i < toLevel; i++)
        {
            tempCostForTotalCostFromZero += Cost(i);
        }
        return tempCostForTotalCostFromZero;
    }
    //現在のレベルからtoLevelまでのトータルコスト//これは非常に重い可能性があるので頻繁には呼ばない
    double tempCostForTotalCost;
    long tempLevelIncrement;
    double tempInitCostSub, tempBaseCostSub;
    long lastCalculatedToLevel;
    double lastCalculatedCost;
    public double TotalCost(long toLevel)
    {
        tempLevelIncrement = toLevel - level.value;
        if (tempLevelIncrement >= 100 && toLevel == lastCalculatedToLevel) return lastCalculatedCost;

        //線形の場合は計算が楽なので単純計算する
        if (isLinear)
        {
            tempInitCostSub = initCost();
            tempBaseCostSub = baseCost();
            if (tempBaseCostSub == 0)//baseCost()が0の場合は最も単純
                lastCalculatedCost = tempInitCostSub * tempLevelIncrement;
            else
                lastCalculatedCost = Math.Floor(tempInitCostSub * tempLevelIncrement + 0.5d * tempLevelIncrement * (tempLevelIncrement - 1) * tempBaseCostSub + level.value * tempLevelIncrement * tempBaseCostSub);
        }
        else
        {
            tempCostForTotalCost = 0;
            for (int i = (int)level.value; i < toLevel; i++)
            {
                tempCostForTotalCost += Cost(i);
            }
            lastCalculatedCost = tempCostForTotalCost;
        }
        lastCalculatedToLevel = toLevel;
        return lastCalculatedCost;
    }
    //現在のレベルにするまでに費やしたTotalCost
    public double TotalCostConsumed()
    {
        return TotalCostFromZero(level.value);
    }
    double tempResourceAmountForCanBuy;
    public bool CanBuy(bool isBuyOne = false)
    {
        tempResourceAmountForCanBuy = resource.value;
        if (level.IsMaxed()) return false;
        if (isBuyOne) return tempResourceAmountForCanBuy >= Cost(level.value);
        if (tempResourceAmountForCanBuy < TotalCost()) return false;
        return true;
    }
    public long LevelIncrement()
    {
        return (long)Mathf.Min(MultibuyNum(), level.maxValue() - level.value);
    }
}
