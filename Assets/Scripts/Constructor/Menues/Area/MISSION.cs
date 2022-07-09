using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;

public partial class Save
{
    public bool[] isClearedMission;
}
public partial class SaveR
{
    public bool[] isClearedMission;
}

public class MISSION 
{
    public MISSION(AREA area, int prestigeLevel, int id)
    {
        this.id = id;
        this.prestigeLevel = prestigeLevel;
        this.area = area;
    }
    public virtual bool IsClearCondition(HERO_BATTLE heroBattle) { return false; }
    public void CheckMissionClear(HERO_BATTLE heroBattle)
    {
        if (isClearedThisAscension) return;
        if (!IsClearCondition(heroBattle)) return;
        ClearAction();
    }
    void ClearAction()
    {
        if (!isCleared)
        {
            isCleared = true;
            //EpicCoinの獲得
            game.epicStoreCtrl.epicCoin.Increase(EpicCoinGain());
        }
        if (!isClearedThisAscension)
        {
            isClearedThisAscension = true;
            //WorldAscensionのポイント獲得
        }
    }
    public double EpicCoinGain()
    {
        return 10 * (1 + id) + 5 * prestigeLevel;
    }

    public int id;
    public AREA area;
    public int prestigeLevel;
    public virtual MissionKind kind { get; }
    int saveId { get => id + AreaParameter.firstMissionIdForSave * prestigeLevel + AreaParameter.firstMissionIdForSave * AreaParameter.firstLevelIdForSave * area.id + AreaParameter.firstMissionIdForSave * AreaParameter.firstAreaIdForSave * AreaParameter.firstLevelIdForSave * (int)area.kind; }
    public bool isCleared { get => main.S.isClearedMission[saveId]; set => main.S.isClearedMission[saveId] = value; }
    public bool isClearedThisAscension { get => main.SR.isClearedMission[saveId]; set => main.SR.isClearedMission[saveId] = value; }

    public string Description()
    {
        string temp = optStr;
        if (isCleared)
        {
            if (isClearedThisAscension) temp += "<color=green>";
            else temp += "<color=orange>";
        }
        temp += DescriptionString();
        temp += "</color>";
        return temp;
    }
    public string RewardDescription()
    {
        string temp = optStr;
        if (isCleared)
        {
            temp += "<color=green>";//2回目以降はEpicCoinはもらえないため
            //if (isClearedThisAscension) temp += "<color=green>";
            //else temp += "<color=orange>";
        }
        temp += "\n<sprite=\"EpicCoin\" index=0> " + tDigit(EpicCoinGain()) + " Epic Coin";
        temp += "</color>";
        return temp;
    }
    public virtual string DescriptionString() { return ""; }
}
public enum MissionKind
{
    Clear,
    SaveHp,
    WithinTime,
    Gold,
    Exp,
    NoEQ,
    OnlyBase,
    //Capture,
    //OnlyBase,
    //NoEQ,
}

public class Mission_Clear : MISSION
{    
    public Mission_Clear(AREA area, int prestigeLevel, int id) : base(area, prestigeLevel, id)
    {
    }
    public override MissionKind kind => MissionKind.Clear;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return area.completedNum.value > 0;
    }
    public override string DescriptionString()
    {
        return "Clear this area";
    }
}
public class Mission_SaveHp : MISSION
{
    public Mission_SaveHp(AREA area, int prestigeLevel, int id, double hpPercent) : base(area, prestigeLevel, id)
    {
        this.hpPercent = hpPercent;
    }
    public override MissionKind kind => MissionKind.SaveHp;
    double hpPercent;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return heroBattle.HpPercent() >= hpPercent;
    }
    public override string DescriptionString()
    {
        return "Clear this area when your HP is more than " + percent(hpPercent, 0);
    }
}
public class Mission_WithinTime : MISSION
{
    public Mission_WithinTime(AREA area, int prestigeLevel, int id, float time) : base(area, prestigeLevel, id)
    {
        this.time = time;
    }
    public override MissionKind kind => MissionKind.WithinTime;
    float time;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return area.bestTime <= time;
    }
    public override string DescriptionString()
    {
        return "Clear this area within " + tDigit(time) + " sec";
    }
}
public class Mission_Gold : MISSION
{
    public Mission_Gold(AREA area, int prestigeLevel, int id, Func<double> gold) : base(area, prestigeLevel, id)
    {
        this.gold = gold;
    }
    public override MissionKind kind => MissionKind.Gold;
    Func<double> gold;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return area.bestGold >= gold();
    }
    public override string DescriptionString()
    {
        return "Gain " + tDigit(gold()) + " Gold in one clear";
    }
}
public class Mission_Exp : MISSION
{
    public Mission_Exp(AREA area, int prestigeLevel, int id, Func<double> exp) : base(area, prestigeLevel, id)
    {
        this.exp = exp;
    }
    public override MissionKind kind => MissionKind.Exp;
    Func<double> exp;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return area.bestExp >= exp();
    }
    public override string DescriptionString()
    {
        return "Gain " + tDigit(exp()) + " EXP in one clear";
    }
}
public class Mission_NoEQ : MISSION
{
    public Mission_NoEQ(AREA area, int prestigeLevel, int id) : base(area, prestigeLevel, id)
    {
    }
    public override MissionKind kind => MissionKind.NoEQ;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return !heroBattle.battleCtrl.isEquippedAnyEQ;
    }
    public override string DescriptionString()
    {
        return "Clear this area without any Equipment";
    }
}
public class Mission_OnlyBase : MISSION
{
    public Mission_OnlyBase(AREA area, int prestigeLevel, int id) : base(area, prestigeLevel, id)
    {
    }
    public override MissionKind kind => MissionKind.NoEQ;
    public override bool IsClearCondition(HERO_BATTLE heroBattle)
    {
        return !heroBattle.battleCtrl.isEquippedAnySkill;
    }
    public override string DescriptionString()
    {
        return "Clear this area with only Base Attack skill equipped";
    }
}
