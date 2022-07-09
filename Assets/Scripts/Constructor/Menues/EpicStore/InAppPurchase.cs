using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static InAppPurchaseKind;

public partial class Save
{
    public long[] inAppPurchasedNum;//[kind]
}

public class InAppPurchaseController
{
    public List<InAppPurchase> inAppPuchaseList = new List<InAppPurchase>();
    public InAppPurchaseController()
    {
        inAppPuchaseList.Add(new InAppPurchase_500EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_1050EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_2650EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_5500EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_12000EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_18500EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_31000EpicCoin());
        inAppPuchaseList.Add(new InAppPurchase_70000EpicCoin());
    }
    public void Restore(long[] inAppPurchaseNum)
    {
        for (int i = 0; i < inAppPuchaseList.Count; i++)
        {
            long tempLack = inAppPurchaseNum[i] - inAppPuchaseList[i].purchasedNum;
            if (tempLack > 0)
            {
                Debug.Log(inAppPuchaseList[i].kind.ToString() + " : " + inAppPuchaseList[i].purchasedNum + " / " + inAppPurchaseNum[i]);
                inAppPuchaseList[i].GetEpicCoin(tempLack);
            }
        }
    }
    public bool CanRestore(long[] inAppPurchaseNum)
    {
        for (int i = 0; i < inAppPuchaseList.Count; i++)
        {
            long tempLack = inAppPurchaseNum[i] - inAppPuchaseList[i].purchasedNum;
            if (tempLack > 0)
            {
                return true;
            }
        }
        return false;
    }
}

public class InAppPurchase
{
    public int id => 100 + (int)kind;
    public virtual int priceDollars => 1;
    public virtual string priceCent => (priceDollars * 100).ToString();//cent単位の価格
    //public string description => "aiueod";
    public virtual string description => "Get " + epicCoinGain.ToString("F0") + " Epic Coin right now!";

    public virtual Action successAction => SuccessAction;
    public virtual Action failAction => null;

    public virtual InAppPurchaseKind kind => EpicCoin500;
    public virtual double epicCoinGain => 500;
    public virtual bool isDoubleFirstPurchase => false;//初回2倍ボーナス

    void SuccessAction()
    {
        //この順番は変えない。
        GetEpicCoin();
    }
    public void GetEpicCoin(long times = 1)
    {
        if (isDoubleFirstPurchase && purchasedNum == 0)
        {
            times++;
        }
        game.epicStoreCtrl.epicCoin.Increase(epicCoinGain * times);
        purchasedNum++;
    }
    public long purchasedNum { get => main.S.inAppPurchasedNum[(int)kind]; set => main.S.inAppPurchasedNum[(int)kind] = value; }

    public string NameString()
    {
        if (isDoubleFirstPurchase && purchasedNum == 0)
        {
            return "<sprite=\"EpicCoin\" index=0> " + (epicCoinGain).ToString("F0") + " Epic Coin<size=24><color=orange><b> [ x2 ]</b></color></size>";
        }
        return "<sprite=\"EpicCoin\" index=0> " + epicCoinGain.ToString("F0") + " Epic Coin";
    }
    public string Description()
    {
        if (isDoubleFirstPurchase && purchasedNum == 0)
        {
            return "Get <s><i><size=18> " + epicCoinGain.ToString("F0") + "</s></i> <size=24><b><color=green><sprite=\"EpicCoin\" index=0> " + (2 * epicCoinGain).ToString("F0") + "<size=20></b></color> Epic Coin right now!\n<color=yellow>Only the first purchase doubles Epic Coin!</color>";
        }
        return "Get <sprite=\"EpicCoin\" index=0> <color=green>" + epicCoinGain.ToString("F0") + "</color> Epic Coin right now!";
    }
}

public class InAppPurchase_500EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin500;
    public override int priceDollars => 1;
    public override double epicCoinGain => 500;
}
public class InAppPurchase_1050EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin1050;
    public override int priceDollars => 2;
    public override double epicCoinGain => 1050;
}
public class InAppPurchase_2650EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin2650;
    public override int priceDollars => 5;
    public override double epicCoinGain => 2650;
    public override bool isDoubleFirstPurchase => true;
}
public class InAppPurchase_5500EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin5500;
    public override int priceDollars => 10;
    public override double epicCoinGain => 5500;
    public override bool isDoubleFirstPurchase => true;
}
public class InAppPurchase_12000EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin12000;
    public override int priceDollars => 20;
    public override double epicCoinGain => 12000;
    public override bool isDoubleFirstPurchase => true;
}
public class InAppPurchase_18500EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin12000;
    public override int priceDollars => 30;
    public override double epicCoinGain => 18500;
    public override bool isDoubleFirstPurchase => true;
}
public class InAppPurchase_31000EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin12000;
    public override int priceDollars => 50;
    public override double epicCoinGain => 31000;
}
public class InAppPurchase_70000EpicCoin : InAppPurchase
{
    public override InAppPurchaseKind kind => EpicCoin12000;
    public override int priceDollars => 100;
    public override double epicCoinGain => 70000;
}
public enum InAppPurchaseKind
{
    EpicCoin500,
    EpicCoin1050,
    EpicCoin2650,
    EpicCoin5500,
    EpicCoin12000,
    EpicCoin18500,
    EpicCoin31000,
    EpicCoin70000,
}


