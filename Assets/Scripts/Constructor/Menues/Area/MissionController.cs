using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using static MultiplierKind;
using static MultiplierType;

public class MissionController
{
    public MissionController()
    {
        unlockedNum = new Multiplier(new MultiplierInfo(Base, Add, () => 100));
    }
    public void Start()
    {
        milestoneEffectList.Add(new MissionMilestone(0, clearNum, 5, (x) => { game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.10d, x)); }, "EXP Gain + 10%"));
        milestoneEffectList.Add(new MissionMilestone(1, clearNum, 10, (x) => { game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.10d, x)); }, "Equipment Proficiency Gain + 10%"));
        milestoneEffectList.Add(new MissionMilestone(2, clearNum, 15, (x) => { game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.50d, x)); }, "Town Material Gain + 50%"));
        milestoneEffectList.Add(new MissionMilestone(3, clearNum, 20, (x) => { game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 10, x)); }, "Equipment Inventory Slot + 10"));
        milestoneEffectList.Add(new MissionMilestone(4, clearNum, 25, (x) => { game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.15d, x)); }, "EXP Gain + 15%"));
        milestoneEffectList.Add(new MissionMilestone(5, clearNum, 30, (x) => { game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Mul, () => 0.50d, x)); }, "Gold Cap + 50%"));
        milestoneEffectList.Add(new MissionMilestone(6, clearNum, 35, (x) => { game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.20d, x)); }, "Equipment Proficiency Gain + 20%"));
        milestoneEffectList.Add(new MissionMilestone(7, clearNum, 40, (x) => { game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Mul, () => 0.50d, x)); }, "Mysterious Water Gain + 50%"));
        milestoneEffectList.Add(new MissionMilestone(8, clearNum, 45, (x) => { game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.25d, x)); }, "EXP Gain + 25%"));
        milestoneEffectList.Add(new MissionMilestone(9, clearNum, 50, (x) => { game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.50d, x)); }, "Town Material Gain + 50%"));
        milestoneEffectList.Add(new MissionMilestone(10, clearNum, 60, (x) => { game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(MultiplierKind.MissionMilestone, Mul, () => 1.00d, x));}, "Resource Gain + 100%"));
        milestoneEffectList.Add(new MissionMilestone(11, clearNum, 70, (x) => { game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Mul, () => 0.50d, x)); }, "Gold Gain + 50%"));
        milestoneEffectList.Add(new MissionMilestone(12, clearNum, 80, (x) => { game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 0.50d, x)); }, "EXP Gain + 50%"));
        milestoneEffectList.Add(new MissionMilestone(13, clearNum, 90, (x) => { game.monsterCtrl.colorMaterialDropChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Mul, () => 0.50d, x)); }, "Multiply Rare Material Drop Chance by 150%"));
        milestoneEffectList.Add(new MissionMilestone(14, clearNum, 100, (x) => { game.statsCtrl.globalSkillSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.MissionMilestone, Add, () => 1, x)); }, "Global Skill Slot + 1"));
    }

    public Func<long> clearNum { get => () => game.areaCtrl.TotalClearedMissionNum(false); }
    public Multiplier unlockedNum;//作ってはみたが、実質廃止
    public List<MissionMilestone> milestoneEffectList = new List<MissionMilestone>();
}

public class MissionMilestone : MILESTONE
{
    public MissionMilestone(int id, Func<long> number, long requiredNumber, Action<Func<bool>> registerInfoAction, string descriptionString) : base(number, requiredNumber, registerInfoAction, descriptionString)
    {
        this.id = id;
    }
    public int id;

    public override bool isUnlocked { get => id < game.missionCtrl.unlockedNum.Value(); }
}

public class MILESTONE
{
    public MILESTONE(Func<long> number, long requiredNumber, Action<Func<bool>> registerInfoAction, string descriptionString)
    {
        this.number = number;
        this.requiredNumber = requiredNumber;
        if (registerInfoAction != null) registerInfoAction(IsActive);
        this.descriptionString = descriptionString;
    }
    public Func<long> number;
    public bool IsActive() { return number() >= requiredNumber && isUnlocked; }
    public virtual bool isUnlocked { get => true; }
    public long requiredNumber;
    public string descriptionString;
    public virtual string DescriptionString()
    {
        string tempStr = optStr;
        if (IsActive()) tempStr += "<color=green>";
        else if (isUnlocked) tempStr += "<color=white>";
        tempStr += tDigit(requiredNumber) + " : ";
        if (!isUnlocked) tempStr += "???";
        else tempStr += descriptionString + "</color>";
        return tempStr;
    }
}
