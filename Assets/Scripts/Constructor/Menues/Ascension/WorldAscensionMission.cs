using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using System;

public partial class Save
{
    public bool[] isGotRewardWAAccomplishments;//[id,とりあえず100]
}

public class WorldAscensionMission
{
    public WorldAscensionMission(WorldAscension wa, int id, double point, string name, Func<bool> condition)
    {
        this.wa = wa;
        this.id = id;
        this.point = point;
        this.name = name;
        this.condition = condition;
    }
    public WorldAscension wa;
    public int id;
    public double point;
    public string name;
    public Func<bool> condition;
    public bool isGotReward { get => main.S.isGotRewardWAAccomplishments[id]; set => main.S.isGotRewardWAAccomplishments[id] = value; }
    public void CheckClear()
    {
        if (isGotReward) return;
        if (!condition()) return;
        wa.point.Increase(point);
        isGotReward = true;
    }
    public string Description()
    {
        string tempStr = "";
        if (condition()) tempStr += "<color=green>";
        tempStr += "- " + name;
        if (id == 0) tempStr += "  ( Reward WA Point + " + tDigit(point) + " )</color>";
        else tempStr += "  ( + " + tDigit(point) + " )</color>";
        return tempStr;
    }
}

public class WorldAscensionMissionMilestone
{
    public WorldAscensionMissionMilestone(Func<long> number, long requiredNumber, string description, Action<Func<bool>> registerInfoAction = null)
    {
        this.number = number;
        this.requiredNumber = requiredNumber;
        if (registerInfoAction != null) registerInfoAction(() => isActive);
        this.description = description;
    }
    Func<long> number;
    long requiredNumber;
    public bool isActive => number() >= requiredNumber;
    string description;
    public string DescriptionString()
    {
        string tempStr = optStr;
        if(isActive) tempStr += "<color=green>";
        tempStr += tDigit(requiredNumber) + " : " + description + "</color>";
        return tempStr;
    }
}