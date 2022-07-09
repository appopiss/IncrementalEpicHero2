using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UsefulMethod;

public class Unlock
{
    public bool IsUnlocked()
    {
        for (int i = 0; i < conditionList.Count; i++)
        {
            if (!conditionList[i]()) return false;
        }
        return true;
    }
    public void RegisterCondition(Func<bool> condition, string conditionString = "")
    {
        conditionList.Add(condition);
        conditionStringList.Add(conditionString);
    }
    public List<Func<bool>> conditionList = new List<Func<bool>>();
    public List<string> conditionStringList = new List<string>();
    public string LockString()
    {
        string tempStr = optStr + "<sprite=\"locks\" index=0> ";
        for (int i = 0; i < conditionStringList.Count; i++)
        {
            if (conditionStringList[i] != "") tempStr += conditionStringList[i] + "  ";
        }
        return tempStr;
    }
}
