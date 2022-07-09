using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using static GameController;
using static Main;
using static MultiplierKind;
using static MultiplierType;
using static UsefulMethod;

public partial class Save
{
    public long[] ascensionMilestoneLevelReached;//[kind]
}

public class WAM_BuildingLevel : WorldAscensionMilestone
{
    public WAM_BuildingLevel(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.TownBuldingLevel;
    public override string Name() { return "Raise the Town"; }
    public override string Description() { return "Total levels of Town Buildings"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Town Building's level effect + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.townCtrl.TotalBuildingLevel();
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 150;
            case 2: return 300;
            case 3: return 450;
            case 4: return 600;
            case 5: return 750;
            case 6: return 900;
            case 7: return 1000;
            case 8: return 1100;
            case 9: return 1150;
            case 10: return 1200;
        }
        return 1500;
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.10d * level * Math.Pow(2, (level - 1d) / 9d);
    }
    public override void SetEffect()
    {
        game.townCtrl.townLevelEffectMultiplier.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => currentPassiveEffectValue));
    }
}
public class WAM_DictionaryPoint : WorldAscensionMilestone
{
    public WAM_DictionaryPoint(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.DictionaryPoint;
    public override string Name() { return "Polished Equipper"; }
    public override string Description() { return "Total Dictionary Points gained"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Dictionary Upgrade effect + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.equipmentCtrl.DictionaryTotalPoint();
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 150;
            case 2: return 500;
            case 3: return 1000;
            case 4: return 2000;
            case 5: return 4000;
            case 6: return 7500;
            case 7: return 15000;
            case 8: return 30000;
            case 9: return 60000;
            case 10: return 100000;
        }
        return 100000;
        //return 75 * Math.Pow(2, level);//150,300,600,1200,2400,4800,
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.10d * level * Math.Pow(2, (level - 1d) / 9d);
    }
    public override void SetEffect()
    {
        game.equipmentCtrl.dictionaryUpgradeEffectMultiplier.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => currentPassiveEffectValue));
    }
}
public class WAM_MissionClearNum : WorldAscensionMilestone
{
    public WAM_MissionClearNum(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.MissionClearNum;
    public override string Name() { return "Map Milestoner"; }
    public override string Description() { return "Total Area Mission completed #"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Town Material reward per Area Difficulty + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.areaCtrl.TotalClearedMissionNum(true);
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 50;
            case 2: return 100;
            case 3: return 250;
            case 4: return 450;
            case 5: return 700;
            case 6: return 1200;
            case 7: return 2000;
            case 8: return 3000;
            case 9: return 3500;
            case 10: return 4000;
        }
        return 4000;
        //return 25 * Math.Pow(2, level);//50,100,200,400,800,1600
    }
    public override double PassiveEffectValue(long level)
    {
        return level * 0.50d;
    }
    public override void SetEffect()
    {
        game.areaCtrl.townMaterialGainPerDifficultyMultiplier.RegisterMultiplier(new MultiplierInfo(Ascension, Add, () => currentPassiveEffectValue));
    }
}
public class WAM_UpgradeLevel : WorldAscensionMilestone
{
    public WAM_UpgradeLevel(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.UpgradeLevel;
    public override string Name() { return "Very Resourceful"; }
    public override string Description() { return "Total levels of Resource Upgrades"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Resource Gain + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.upgradeCtrl.TotalLevel(UpgradeKind.Resource);
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 300;
            case 2: return 500;// 450;
            case 3: return 700;// 600;
            case 4: return 900;// 750;
            case 5: return 1100;// 900;
            case 6: return 1300;//1100;
            case 7: return 1500;// 1350;
            case 8: return 1750;// 1500;
            case 9: return 2000;// 1750;
            case 10: return 2500;
        }
        return 10000;
        //return 150 + (150 + 10 * level) * level;//300,450,600,750,...1650
    }
    public override double PassiveEffectValue(long level)
    {
        return Math.Max(0, Math.Pow(2d, level) - 1d);//level * Math.Pow(10, (level - 1d) / 9d);
    }
    public override void SetEffect()
    {
        game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(Ascension, Mul, () => currentPassiveEffectValue));
    }
}
public class WAM_RebirthPointGainTier1 : WorldAscensionMilestone
{
    public WAM_RebirthPointGainTier1(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.RebirthPointGainTier1;
    public override string Name() { return "Rebirther 1"; }
    public override string Description() { return "Total Tier 1 Rebirth Points gained"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Tier 1 Rebirth Point Gain + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.rebirthCtrl.TotalRebirthPoint(0);
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 2000000;
            case 2: return 5000000;
            case 3: return 12500000;
            case 4: return 35000000;
            case 5: return 100000000;
            case 6: return 300000000;
            case 7: return 1000000000;
            case 8: return 5000000000;
            case 9: return 15000000000;
            case 10: return 50000000000;
                //case 1: return 2000000;
                //case 2: return 5000000;
                //case 3: return 12000000;
                //case 4: return 28000000;
                //case 5: return 80000000;
                //case 6: return 200000000;
                //case 7: return 500000000;
                //case 8: return 1200000000;
                //case 9: return 3000000000;
                //case 10: return 10000000000;
        }
        return 10000000000000;
        //return 1000000 * Math.Pow(2, level);//200万
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.10d * level * Math.Pow(2, (level - 1d) / 9d);
    }
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.rebirthCtrl.Rebirth(heroKind, 0).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(Ascension, Mul, () => currentPassiveEffectValue));
        }
    }
}
public class WAM_RebirthPointGainTier2 : WorldAscensionMilestone
{
    public WAM_RebirthPointGainTier2(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.RebirthPointGainTier2;
    public override string Name() { return "Rebirther 2"; }
    public override string Description() { return "Total Tier 2 Rebirth Points gained"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Tier 2 Rebirth Point Gain + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.rebirthCtrl.TotalRebirthPoint(1);
    public override double GoalValue(long level)
    {
        switch (level)
        {
            case 1: return 200000;
            case 2: return 500000;
            case 3: return 1250000;
            case 4: return 3500000;
            case 5: return 10000000;
            case 6: return 30000000;
            case 7: return 100000000;
            case 8: return 500000000;
            case 9: return 1500000000;
            case 10: return 5000000000;
                //case 1: return 200000;
                //case 2: return 500000;
                //case 3: return 1200000;
                //case 4: return 2800000;
                //case 5: return 8000000;
                //case 6: return 20000000;
                //case 7: return 50000000;
                //case 8: return 120000000;
                //case 9: return 300000000;
                //case 10: return 1000000000;
        }
        return 100000000000000;
        //return 100000 * Math.Pow(2, level);//20万
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.10d * level * Math.Pow(2, (level - 1d) / 9d);
    }
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.rebirthCtrl.Rebirth(heroKind, 1).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(Ascension, Mul, () => currentPassiveEffectValue));
        }
    }
}
public class WAM_MoveDistance : WorldAscensionMilestone
{
    public WAM_MoveDistance(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.MoveDistance;
    public override string Name() { return "Walk the World"; }
    public override string Description() { return "Total Walked Distance (meters)"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Move Speed + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => game.statsCtrl.TotalMovedDistance(true) / 100d;//meter単位にするため100で割る
    public override double GoalValue(long level)
    {
        return 500000000 * Math.Pow(3, level - 1) / 100d;//meter単位にするため100で割る
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.01d * Math.Pow(level, 2);
    }
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.MoveSpeed, new MultiplierInfo(Ascension, Mul, () => currentPassiveEffectValue));
    }
}
public class WAM_DisassembleEquipment : WorldAscensionMilestone
{
    public WAM_DisassembleEquipment(WorldAscension wa) : base(wa)
    {
    }
    public override WorldAscensionMiletoneKind kind => WorldAscensionMiletoneKind.DisassembleEquipment;
    public override string Name() { return "Pro Disassembler"; }
    public override string Description() { return "Total Town Material Gained from disassembling Equipment"; }
    public override string PassiveEffectString(double currentValue, double nextValue)
    {
        return "Equipment Proficiency Gain + " + percent(currentValue) + " -> <color=green>+ " + percent(nextValue) + "</color>";
    }
    public override double currentValue => TotalTownMatGainFromDissasemble();//meter単位にするため100で割る
    double TotalTownMatGainFromDissasemble()
    {
        double tempValue = 0;
        for (int i = 0; i < main.SR.townMatGainFromdisassemble.Length; i++)
        {
            tempValue += main.SR.townMatGainFromdisassemble[i];
        }
        return tempValue;
    }
    //double TotalDisassembledEquipmentNum()
    //{
    //    double tempValue = 0;
    //    for (int i = 0; i < main.SR.disassembledEquipmentNums.Length; i++)
    //    {
    //        tempValue += main.SR.disassembledEquipmentNums[i];
    //    }
    //    return tempValue;
    //}
    public override double GoalValue(long level)
    {
        return 50000 * Math.Pow(3, level - 1);
    }
    public override double PassiveEffectValue(long level)
    {
        return 0.20d * level * Math.Pow(2, (level - 1d) / 9d); ;
    }
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(Ascension, Mul, () => currentPassiveEffectValue));
    }
}


public enum WorldAscensionMiletoneKind
{
    TownBuldingLevel,//TownEffect
    //AreaPrestigePoint,これの代わりにMissionだな
    MissionClearNum,//TownMatRewardPerDifficulty EpicCoinは獲得できない使用のため、最初からGreenにしておく
    UpgradeLevel,//ResourceGain
    MoveDistance,//MoveSpeed
    DictionaryPoint,//DictionaryEffect
    DisassembleEquipment,//EquipProfGain
    //GeneralQuestClear,これはRebirthPointとかぶる
    RebirthPointGainTier1,//RebirthPointGain
    RebirthPointGainTier2,//RebirthPointGain
    //GeneralQuest,やっぱり色々かぶるしマクロ組まれるのがアウト

    //BuildingResearch,リセットしない
    //SkillTriggeredNum,リセットしない
    //GuildLevel, HeroLevel,Rebirthなどとかぶれる
    //TotalGoldGained,Upgradeとかぶる

}

public class WorldAscensionMilestone 
{
    public WorldAscensionMilestone(WorldAscension wa)
    {
        this.wa = wa;
        maxLevelReached = new WorldAscensionMilestoneLevel(this);
    }
    public void Start() { SetEffect(); }
    public WorldAscension wa;
    public virtual WorldAscensionMiletoneKind kind { get; }
    public virtual double GoalValue(long level) { return 1e300d; }
    public virtual double currentValue { get; }
    public virtual string Name() { return ""; }//このMilestoneの名称
    public virtual string Description() { return ""; }
    public virtual string PassiveEffectString(double currentValue, double nextValue) { return ""; }
    public virtual double PassiveEffectValue(long level) { return 0; }
    public virtual void SetEffect() { }
    public double currentPassiveEffectValue => PassiveEffectValue(maxLevelReached.value);

    public string NameString()//一覧表示用
    {
        string tempStr = optStr;
        tempStr += Name() + "  < <color=green>Lv " + tDigit(CurrentLevel()) + "</color> >";
        //+ "  [ <color=green>" + tDigit(currentValue) + "</color> / " + tDigit(GoalValue(CurrentLevel() + 1)) + " ]";
        if (maxLevelReached.value > 0) tempStr += "<size=16>  ( Max Lv " + tDigit(maxLevelReached.value) + " )";
        return tempStr;
    }
    public string DescriptionString()//Tooltip表示用
    {
        string tempStr = optStr + "<size=20>" + Name() + " < <color=green>Lv " + tDigit(CurrentLevel()) + "</color> ><size=18>";
        tempStr += "\n" + Description() + " : <color=green>" + tDigit(calculatedCurrentValue) + "</color>";
        //tempStr += "\n\n<size=20><u>Milestone</u><size=18>";
        tempStr += MilestoneString();
        tempStr += "\n\n<size=20><u>Information</u><size=18>";
        tempStr += "\n- Max Level Reached : Lv " + tDigit(maxLevelReached.value) + " -> <color=green>Lv " + tDigit(Math.Max(maxLevelReached.value, CurrentLevel())) + "</color>";
        tempStr += "\n\n<size=20><u>Passive Effect</u><size=18>";
        tempStr += "\n- " + PassiveEffectString(currentPassiveEffectValue, PassiveEffectValue(maxLevelReachedAfterWA));
        return tempStr;
    }
    string MilestoneString()
    {
        string tempStr = optStr;
        for (int i = 0; i < 10; i++)
        {
            int tempLevel = 1 + i;
            if (tempLevel <= CurrentLevel()) tempStr += "<color=green>";
            tempStr += "\n- Lv " + tDigit(tempLevel) + " : " + tDigit(GoalValue(tempLevel));
            if (tempLevel == 1) tempStr += "   ( World Ascension Point + " + tDigit(PointGainIncrement(tempLevel)) + " )</color>";
            else tempStr += "   ( + " + tDigit(PointGainIncrement(tempLevel)) + " )</color>";
        }
        return tempStr;
    }

    long currentLevel;
    public double calculatedCurrentValue;
    public async Task CalculateCurrentLevel()
    {
        calculatedCurrentValue = currentValue;
        await UniTask.DelayFrame(1);
        for (int i = 0; i < 10; i++)
        {
            long tempLevel = i + 1;
            if (calculatedCurrentValue < GoalValue(tempLevel))
            {
                currentLevel = tempLevel - 1;
                return;
            }
        }
        currentLevel = 10;//現状はLv10が最大とする。
    }
    public long CurrentLevel()
    {
        return currentLevel;
    }

    public WorldAscensionMilestoneLevel maxLevelReached;
    double PointGainIncrement(long level)
    {
        double tempPoint = wa.pointGainBonus.Value();
        switch (level)
        {
            case 1: tempPoint += 1; break;
            case 2: tempPoint += 2; break;//total3
            case 3: tempPoint += 2; break;//total5
            case 4: tempPoint += 3; break;//total8
            case 5: tempPoint += 3; break;//total11
            case 6: tempPoint += 4; break;//total15
            case 7: tempPoint += 5; break;//total20
            case 8: tempPoint += 7; break;//total27
            case 9: tempPoint += 10; break;//total37
            case 10: tempPoint += 15; break;//total42
            default: return 0;
        }
        return tempPoint;
        //return PointGain(level) - PointGain(level - 1);
    }
    double PointGain(long level)
    {
        double tempPoint = 0;
        for (int i = 1; i <= level; i++)
        {
            tempPoint += PointGainIncrement(i);
        }
        return tempPoint;
        //return Math.Floor(level * (1 + 0.25d * level)) * Math.Pow(2d, level / 10d) + level * wa.pointGainBonus.Value();
    }
    public double pointGain => PointGain(CurrentLevel());
    long maxLevelReachedAfterWA => Math.Max(maxLevelReached.value, CurrentLevel());
    public void UpdateMaxLevelReachedMilestone() { maxLevelReached.ChangeValue(maxLevelReachedAfterWA); }
}
public class WorldAscensionMilestoneLevel : INTEGER
{
    public WorldAscensionMilestoneLevel(WorldAscensionMilestone milestone)
    {
        this.milestone = milestone;
        maxValue = () => 10;//とりあえず10とする
    }
    public WorldAscensionMilestone milestone;
    public override long value { get => main.S.ascensionMilestoneLevelReached[(int)milestone.kind]; set => main.S.ascensionMilestoneLevelReached[(int)milestone.kind] = value; }
}

