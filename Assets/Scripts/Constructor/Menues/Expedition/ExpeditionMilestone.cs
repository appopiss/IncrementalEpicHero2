using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using static MultiplierKind;
using static MultiplierType;

public class ExpeditionMilestone : MILESTONE
{
    public ExpeditionMilestone(Func<long> number, long requiredNumber, Action<Func<bool>> registerInfoAction, string descriptionString) : base(number, requiredNumber, registerInfoAction, descriptionString)
    {
    }
    public override string DescriptionString()
    {
        string tempStr = optStr;
        if (IsActive()) tempStr += "<color=green>";
        else if (isUnlocked) tempStr += "<color=white>";
        tempStr += "Lv " + tDigit(requiredNumber) + " : ";
        if (!isUnlocked) tempStr += "???";
        else tempStr += descriptionString + "</color>";
        return tempStr;
    }
}
