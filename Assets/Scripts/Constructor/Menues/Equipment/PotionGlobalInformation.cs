using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static PotionParameter;
using static UsefulMethod;

public partial class Save
{
    public long[] potionQueues;//[PotionKind]
    public bool[] potionIsSuperQueues;//[PotionKind]
}
public partial class SaveR
{
    public long[] potionLevels;//[PotionKind]
    public long[] potionProductedNums;//[PotionKind]
    public double[] potionDisassembledNums;//[PotionKind]

}

public class PotionGlobalInformation
{
    public PotionKind kind;
    public PotionType type;
    public PotionConsumeConditionKind consumeKind;
    public virtual double EffectValue(long level) { return 0; }
    public double consumeChance { get => ConsumeChance(level.value); }
    public virtual double effectValue { get => EffectValue(level.value) * game.potionCtrl.effectMultiplier.Value(); }
    public virtual double nextEffectValue { get => EffectValue(levelTransaction.ToLevel()) * game.potionCtrl.effectMultiplier.Value(); }
    public virtual double PassiveEffectValue(long level) { return EffectValue(level) * 0.01d; }//Talisman用。Levelではなく実際はDisassembled#のこと
    public double passiveEffectValue => PassiveEffectValue((long)disassembledNum.value);
    public virtual bool isUnlocked { get; }
    public Dictionary<EssenceKind, Func<double>> alchemyCosts = new Dictionary<EssenceKind, Func<double>>();
    public virtual double AlchemyPointGain(long level) { return 1; }
    public double alchemyPointGain { get => AlchemyPointGain(level.value); }
    public INTEGER level;
    public PotionProductedNum productedNum;
    public PotionDisassembledNum disassembledNum;
    public Transaction levelTransaction;
    public Transaction alchemyTransaction;
    public AlchemyController alchemyCtrl { get => game.alchemyCtrl; }
    public NUMBER alchemyPoint { get => alchemyCtrl.alchemyPoint; }//alchemyPoint
    public TalismanFragment talismanFragment => alchemyCtrl.talismanFragment;
    public virtual bool isEffectedByLowerTier => false;//MinorHealth->ChilledHealthみたいに下のTierから効果を受け継ぐもの
    public virtual void SetEffect(HeroKind heroKind, Func<double> equipNum) { }
    public bool IsActiveEffect(HeroKind heroKind, Func<double> equipNum)
    {
        if (!game.battleCtrls[(int)heroKind].isActiveBattle) return false;
        return equipNum() > 0;
    }
    public virtual void ConsumeEffectAction(BATTLE battle) { }

    //Talisman用
    public virtual TalismanRarity talismanRarity => TalismanRarity.Common;
    public bool canDisassemble
    {
        get
        {
            if (talismanRarity == TalismanRarity.Epic)
                return false;
            return true;
        }
    }

    //Cooltime
    public float[] lastUsedTime = new float[Enum.GetNames(typeof(HeroKind)).Length];
    public float lastUsedTimeSimulated;
    public virtual float cooltime => 0;//デフォルト:60分
    public float ElapsedTime(HeroKind heroKind, bool isSimulated)
    {
        if (isSimulated) return game.battleCtrls[(int)heroKind].timecount - lastUsedTimeSimulated;
        return game.battleCtrls[(int)heroKind].timecount - lastUsedTime[(int)heroKind];
    }
    float TimeLeft(HeroKind heroKind) { return Math.Max(0, cooltime - ElapsedTime(heroKind, false)); }
    public bool IsAvailableCooltime(HeroKind heroKind, bool isSimulated = false) { return ElapsedTime(heroKind, isSimulated) > cooltime; }
    public float TimeLeftPercent(HeroKind heroKind) { return TimeLeft(heroKind) / Math.Max(1, cooltime); }

    public virtual void SetInfo() { }
    public double ConsumeChance(long level)
    {
        double tempValue = 1;
        switch (consumeKind)
        {
            case PotionConsumeConditionKind.AreaComplete:
                tempValue = 0.90d * (1 - 0.5d * level / maxLevel);
                break;
            case PotionConsumeConditionKind.Defeat:
                tempValue = 0.015d * (1 - 0.5d * level / maxLevel);
                break;
            case PotionConsumeConditionKind.Move:
                tempValue = 0.050d * (1 - 0.5d * level / maxLevel);
                break;
        }
        tempValue *= (1 - game.potionCtrl.preventConsumeChance.Value());
        return tempValue;
    }

    public PotionGlobalInformation()
    {
        SetInfo();
        productedNum = new PotionProductedNum(kind);
        disassembledNum = new PotionDisassembledNum(kind);
        alchemyTransaction = new Transaction(productedNum);
        foreach (var item in alchemyCosts)
        {
            alchemyTransaction.SetAnotherResource(game.essenceCtrl.Essence(item.Key), item.Value, () => 0, true);
        }
        alchemyTransaction.SetAdditionalBuyCondition(CanCreate);
        alchemyTransaction.SetAdditionalBuyCondition(CanCreateWithNum);
        alchemyTransaction.additionalBuyActionWithLevelIncrement = (x) => Create(x);

        for (int i = 0; i < lastUsedTime.Length; i++)
        {
            lastUsedTime[i] = -10000;
        }
    }
    bool CanCreate()
    {
        if (!isUnlocked) return false;
        return true;
    }
    bool CanCreateWithNum(long num)
    {
        if (game.epicStoreCtrl.Item(EpicStoreKind.AlchemyUnlimitedDisassemble).IsPurchased() && GameControllerUI.isShiftPressed && Input.GetKey(KeyCode.D))
            return true;
        return game.inventoryCtrl.CanCreatePotion(kind, num);
    }
    public void Create(long num, bool isPreventCreatPotion = false)
    {
        if (!isPreventCreatPotion) game.inventoryCtrl.CreatePotion(kind, num);
        double tempPoint = num * alchemyPointGain;
        if (WithinRandom(alchemyCtrl.doubleAlchemyPointChance.Value())) tempPoint *= 2;
        alchemyPoint.Increase(tempPoint);

        long materialThriftCount = 0;
        double materialThriftChance = alchemyCtrl.preventUseEssenceToCraft.Value();
        long nitroCount = 0;
        for (int i = 0; i < Math.Min(num, 10000); i++)
        {
            //Material Thrift
            if (WithinRandom(materialThriftChance))
            {
                materialThriftCount++;
            }
            //Nitro
            if (WithinRandom(0.01d))
            {
                nitroCount++;
            }
        }
        if (num > 10000)
        {
            materialThriftCount *= num / 10000;
            nitroCount *= num / 10000;
        }
        foreach (var item in alchemyCosts)
        {
            game.essenceCtrl.Essence(item.Key).Increase(item.Value() * materialThriftCount);
        }
        game.nitroCtrl.nitro.Increase(alchemyCtrl.nitroGainOnCraft.Value() * nitroCount);
    }

    //Queue
    public long queue { get => main.S.potionQueues[(int)kind]; set => main.S.potionQueues[(int)kind] = value; }
    public bool isSuperQueued { get => main.S.potionIsSuperQueues[(int)kind]; set => main.S.potionIsSuperQueues[(int)kind] = value; }
    public void BuyByQueue()
    {
        if (!isSuperQueued && queue <= 0) return;
        if (!alchemyTransaction.CanBuy(true)) return;
        //SuperQuqueの場合、Inventoryに1Slot分だけ購入するようにする
        if (isSuperQueued && game.inventoryCtrl.IsOnePotionStackMaxInInventory(kind)) return;
        alchemyTransaction.Buy(true);
        if (!isSuperQueued) queue--;
    }
    public void AssignQueue(long num, bool isSuperQueue)
    {
        if (isSuperQueue)
        {
            isSuperQueued = true;
            queue = 0;
        }
        else
            queue += num;
    }
    public void RemoveQueue(long num)
    {
        if (isSuperQueued) ResetQueue();
        else queue -= num;
        if (queue < 0) queue = 0;
    }
    public void ResetQueue()
    {
        queue = 0;
        isSuperQueued = false;
    }

    //Disassemble
    public double DisassembleGoldNum()
    {
        if (type == PotionType.Talisman)//Talismanの場合はGoldではなくPoint
        {
            return Math.Floor(level.value * talismanDisassembleFragmentNumPerLevel);//1,5,25,125
        }
        return Math.Floor(level.value / 10d + alchemyPointGain) * game.potionCtrl.disassembleGoldGainMultiplier.Value();
    }
    public double talismanDisassembleFragmentNumPerLevel => Math.Pow(5, (int)talismanRarity);

    //UI
    public string Name()
    {
        return Localized.localized.PotionName(kind);
    }
}

public class PotionProductedNum : INTEGER
{
    PotionKind kind;
    public PotionProductedNum(PotionKind kind)
    {
        this.kind = kind;
    }
    public override long value { get => main.SR.potionProductedNums[(int)kind]; set => main.SR.potionProductedNums[(int)kind] = value; }
}

public class PotionLevel : INTEGER
{
    PotionKind kind;
    public PotionLevel(PotionKind kind)
    {
        this.kind = kind;
        maxValue = () => (long)game.potionCtrl.potionMaxLevel.Value();
    }
    public override long value { get => main.SR.potionLevels[(int)kind]; set => main.SR.potionLevels[(int)kind] = value; }
}
public class TalismanLevel : INTEGER
{
    PotionKind kind;
    public TalismanLevel(PotionKind kind)
    {
        this.kind = kind;
        maxValue = () => 50;
    }
    public override long value { get => main.SR.potionLevels[(int)kind]; set => main.SR.potionLevels[(int)kind] = value; }
}

public class PotionDisassembledNum : NUMBER
{
    PotionKind kind;
    public PotionDisassembledNum(PotionKind kind)
    {
        this.kind = kind;
    }
    public override double value { get => main.SR.potionDisassembledNums[(int)kind]; set => main.SR.potionDisassembledNums[(int)kind] = value; }
}

