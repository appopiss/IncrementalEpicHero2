using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using static MultiplierKind;
using static MultiplierType;

public partial class SaveR
{
    public long[] buildingLevels;
    public long[] buildingRanks;
    public long[] buildingResearchLevelsStone;
    public long[] buildingResearchLevelsCrystal;
    public long[] buildingResearchLevelsLeaf;
    public double[] buildingResearchExpsStone;
    public double[] buildingResearchExpsCrystal;
    public double[] buildingResearchExpsLeaf;
    public bool[] IsBuildingResearchStone;
    public bool[] IsBuildingResearchCrystal;
    public bool[] IsBuildingResearchLeaf;
    public double[] accomplishedFirstTimesBuilding;
    public double[] accomplishedTimesBuilding;
    public double[] accomplishedBestTimesBuilding;
}

public class BUILDING
{
    public BUILDING()
    {
        rank = new BuildingRank(this, () => maxRank);
        level = new BuildingLevel(kind, MaxLevel);
        rankTransaction = new SpecifiedTransaction(rank, maxBuildingRank);
        levelTransaction = new Transaction(level);
        for (int i = 0; i < rankUpConditions.Length; i++)
        {
            int count = i;
            rankUpConditions[i] = new RankUpCondition();
        }
        for (int i = 0; i < researchLevels.Length; i++)
        {
            researchLevels[i] = new BuildingResearchLevel(this, (ResourceKind)i);
            researchExps[i] = new BuildingResearchExp(researchLevels[i]);
        }
        unlock = new Unlock();
        unlock.RegisterCondition(() => game.guildCtrl.MaxLevelReached() >= unlockGuildLevel);

        for (int i = 0; i < accomplish.Length; i++)
        {
            accomplish[i] = new AccomplishBuilding(this, i);
        }
    }
    public void Start()
    {
        for (int i = 0; i < rankUpConditions.Length; i++)
        {
            int count = i;
            if (kind != BuildingKind.StatueOfHeroes) rankUpConditions[i].RegisterCondition(() => townCtrl.Building(BuildingKind.StatueOfHeroes).Rank() > count, () => "Statue of Heroes Rank " + (count + 1).ToString());
            if (count > 0) rankUpConditions[i].RegisterCondition(level.IsMaxed, () => "This building's Lv " + MaxLevel(count));
        }
        SetCost();
        for (int i = 0; i < rankUpConditions.Length; i++)
        {
            int count = i;
            rankTransaction.SetAdditionalBuyCondition(count, rankUpConditions[count].IsUnlocked);
        }
        SetEffect();
        SetResearch();
    }
    //Unlock
    public Unlock unlock;
    public virtual long unlockGuildLevel { get => 0; }

    public virtual BuildingKind kind { get; }
    public BuildingRank rank;
    public BuildingLevel level;
    public SpecifiedTransaction rankTransaction;
    public Transaction levelTransaction;
    public RankUpCondition[] rankUpConditions = new RankUpCondition[maxBuildingRank];
    public AccomplishBuilding[] accomplish = new AccomplishBuilding[maxBuildingRank];//rank分
    public RankUpCondition RankUpCondition() { return rankUpConditions[Rank()]; }
    public static readonly int maxBuildingRank = 5;
    public TownController townCtrl { get => game.townCtrl; }
    public long Rank() { return rank.value; }
    public virtual long maxRank { get => (long)townCtrl.maxRank.Value(); }
    public long Level() { return level.value; }
    public long MaxLevel() { return MaxLevel(Rank()); }//if (Rank() < 1) return 10;
    long MaxLevel(long rank) { return 20 * rank; }
    public virtual void SetCost() { }
    public virtual void SetEffect() { }
    public virtual void SetResearch() { }
    public List<BuildingPassiveEffect> passiveEffectList = new List<BuildingPassiveEffect>();
    public List<BuildingPassiveEffect> passiveRankEffectList = new List<BuildingPassiveEffect>();
    public virtual string NameString() { return ""; }
    public virtual string LevelEffectString() { return ""; }
    public double PrimaryCost(double level) { return Math.Ceiling(4 + Math.Pow(200, Math.Log(1 + level, 6)) * Math.Pow(3, level / 100) * 1 * Math.Max(1, Math.Pow(2d, (level - 20) / 20d)) * Math.Max(0.5d, 1 - townCtrl.levelCostReduction.Value())); }
    public double SecondaryCost(double level) { return Math.Ceiling(Math.Pow(200, Math.Log(1 + level, 9)) * Math.Pow(3, level / 100) * 5 * Math.Max(1, Math.Pow(2d, (level - 20) / 20d)) * Math.Max(0.5d, 1 - townCtrl.levelCostReduction.Value())); }
    public double PrimaryCost(double level, int tier)//BuildingのTier:1行目=0, 2行目(Blacksmithなど)=1
    {
        switch (tier)
        {
            case 0: tempLevelFactor = 0; break;
            case 1: tempLevelFactor = 3; break;
            case 2: tempLevelFactor = 15; break;
            case 3: tempLevelFactor = 25; break;
        }
        return PrimaryCost(tempLevelFactor + level * ((MaxLevel(5) - tempLevelFactor) / MaxLevel(5)));
    }
    double tempLevelFactor;
    public double SecondaryCost(long level, long rank)
    {
        if (level < MaxLevel(rank)) return 0;
        return SecondaryCost((1d + (double)level - (double)MaxLevel(rank)) * (100d / (100d - (double)MaxLevel(rank))));//Rank1からなら100/80=5/4
    }
    //Research
    public bool[] isResearch
    {
        get
        {
            return new bool[] { main.SR.IsBuildingResearchStone[(int)kind], main.SR.IsBuildingResearchCrystal[(int)kind], main.SR.IsBuildingResearchLeaf[(int)kind] };
        }
    }
    bool[] IsResearchSaveArray(ResourceKind kind)
    {
        switch (kind)
        {
            case ResourceKind.Stone: return main.SR.IsBuildingResearchStone;
            case ResourceKind.Crystal: return main.SR.IsBuildingResearchCrystal;
            case ResourceKind.Leaf: return main.SR.IsBuildingResearchLeaf;
            default: return null;
        }
    }
    public BuildingResearchLevel[] researchLevels = new BuildingResearchLevel[Enum.GetNames(typeof(ResourceKind)).Length];
    public BuildingResearchExp[] researchExps = new BuildingResearchExp[Enum.GetNames(typeof(ResourceKind)).Length];
    public long ResearchLevel(ResourceKind kind) { return researchLevels[(int)kind].value; }
    public virtual (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement) { return ("", "", ""); }
    public string ResearchEffectString(ResourceKind kind, bool isShowIncrement = false)
    {
        switch (kind)
        {
            case ResourceKind.Stone: return ResearchEffectString(isShowIncrement).stone;
            case ResourceKind.Crystal: return ResearchEffectString(isShowIncrement).crystal;
            case ResourceKind.Leaf: return ResearchEffectString(isShowIncrement).leaf;
        }
        return "";
    }
    public bool CanResearch(ResourceKind kind)
    {
        if (researchLevels[(int)kind].IsMaxed()) return false;
        if (!townCtrl.CanResearch()) return false;
        return true;
    }
    public void SwitchResearch(ResourceKind kind)
    {
        if (isResearch[(int)kind])
        { IsResearchSaveArray(kind)[(int)this.kind] = false;  return; }
        if (!CanResearch(kind)) return;
        IsResearchSaveArray(kind)[(int)this.kind] = true;
    }
    public int CurrentResearchingNum()
    {
        int tempNum = 0;
        for (int i = 0; i < isResearch.Length; i++)
        {
            if (isResearch[i]) tempNum++;
        }
        return tempNum;
    }
    public void Update(double deltaTime)
    {
        for (int i = 0; i < isResearch.Length; i++)
        {
            if (isResearch[i]) researchExps[i].Progress(deltaTime);
        }
    }
}

public class BuildingRank : INTEGER
{
    public BuildingRank(BUILDING building, Func<long> maxValue)
    {
        this.building = building;
        kind = building.kind;
        this.maxValue = maxValue;
    }
    public override void Increase(long increment)
    {
        building.accomplish[value].RegisterTime();
        base.Increase(increment);
    }
    public BUILDING building;
    public BuildingKind kind;
    public override long value { get => main.SR.buildingRanks[(int)kind]; set => main.SR.buildingRanks[(int)kind] = value; }
}

public class BuildingLevel : INTEGER
{
    public BuildingLevel(BuildingKind kind, Func<long> maxLevel)
    {
        this.kind = kind;
        maxValue = maxLevel;
    }
    public BuildingKind kind;
    public override long value { get => main.SR.buildingLevels[(int)kind]; set => main.SR.buildingLevels[(int)kind] = value; }
}

public class BuildingResearchLevel : INTEGER
{
    public BuildingResearchLevel(BUILDING building, ResourceKind resourceKind)
    {
        this.building = building;
        this.resourceKind = resourceKind;
        maxValue = building.Level;
    }
    public BUILDING building;
    public BuildingKind kind { get => building.kind; }
    public ResourceKind resourceKind;
    long[] SaveArray()
    {
        switch (resourceKind)
        {
            case ResourceKind.Stone:
                return main.SR.buildingResearchLevelsStone;
            case ResourceKind.Crystal:
                return main.SR.buildingResearchLevelsCrystal;
            case ResourceKind.Leaf:
                return main.SR.buildingResearchLevelsLeaf;
        }
        return main.SR.buildingResearchLevelsStone;
    }
    public override long value { get => SaveArray()[(int)kind]; set => SaveArray()[(int)kind] = value; }
}
public class BuildingResearchExp : EXP
{
    public BuildingResearchExp(BuildingResearchLevel level)
    {
        this.level = level;
        requiredValue = RequiredTimesec;
        kind = level.kind;
        resourceKind = level.resourceKind;
    }
    double RequiredTimesec(long level)
    {
        return 14400 + 3600 * Math.Pow(1 + level, 2) + 7200 * Math.Pow(level / 10, 4);//4h + 1h * (1+lv)^2 + 2h * X
        //Lv=10 : 128h / power:10 = 12.8h
        //Lv=50 : 2500h+1250h / power:50 = 70.0h
        //Lv=100 : 10000h+20000h / power:100 = 300.0h
    }
    double RequiredTimesec() { return RequiredTimesec(level.value); }
    public double Percent()
    {
        return value / RequiredTimesec();
    }
    public double OriginalTimeleft() { return RequiredTimesec() - value; }
    double Timeleft(double powerFactor)
    {
        return OriginalTimeleft() / Math.Max(1, powerFactor);
    }
    public double Timeleft() { return Timeleft(researchPower); }
    double[] SaveArray()
    {
        switch (resourceKind)
        {
            case ResourceKind.Stone:
                return main.SR.buildingResearchExpsStone;
            case ResourceKind.Crystal:
                return main.SR.buildingResearchExpsCrystal;
            case ResourceKind.Leaf:
                return main.SR.buildingResearchExpsLeaf;
        }
        return main.SR.buildingResearchExpsStone;
    }
    ResourceKind resourceKind;
    BuildingKind kind;
    public double researchPower { get => game.townCtrl.researchPower[(int)resourceKind].Value(); }
    public void Progress(double deltaTimesec)
    {
        Increase(researchPower * deltaTimesec);
    }
    public override double value { get => SaveArray()[(int)kind]; set => SaveArray()[(int)kind] = value; }
}

public class RankUpCondition : Unlock
{
    public void RegisterCondition(Func<bool> condition, Func<string> descriptionString)
    {
        RegisterCondition(condition);
        descriptionStringList.Add(descriptionString);
    }
    private List<Func<string>> descriptionStringList = new List<Func<string>>();
    public string DescriptionString()
    {
        string tempStr = optStr;
        for (int i = 0; i < conditionList.Count; i++)
        {
            tempStr += "- ";
            if (conditionList[i]()) tempStr += "<color=green>";
            else tempStr += "<color=red>";
            tempStr += descriptionStringList[i]();
            tempStr += "</color>\n";
        }
        return tempStr;
    }
}
//Passive
public class BuildingPassiveEffect
{
    public BuildingPassiveEffect(INTEGER level, long requiredLevel, Action<Func<bool>> registerInfoAction, string descriptionString)
    {
        this.level = level;
        this.requiredLevel = requiredLevel;
        if (registerInfoAction != null) registerInfoAction(IsActive);
        this.descriptionString = descriptionString;
    }
    INTEGER level;
    private bool IsActive() { return level.value >= requiredLevel; }
    public long requiredLevel;
    private string descriptionString;
    public string DescriptionString()
    {
        string tempStr = optStr;
        if (IsActive()) tempStr += "<color=green>";
        else tempStr += "<color=white>";
        tempStr += tDigit(requiredLevel) + " : " + descriptionString;
        tempStr += "</color>";
        return tempStr;
    }
}
//Accomplish
public class AccomplishBuilding : ACCOMPLISH
{
    public AccomplishBuilding(BUILDING building, int rank)
    {
        this.building = building;
        this.rank = rank;
    }
    BUILDING building;
    int rank;
    public override double accomplishedFirstTime { get => main.SR.accomplishedFirstTimesBuilding[rank + 10 * (int)building.kind]; set => main.SR.accomplishedFirstTimesBuilding[rank + 10 * (int)building.kind] = value; }
    public override double accomplishedTime { get => main.SR.accomplishedTimesBuilding[rank + 10 * (int)building.kind]; set => main.SR.accomplishedTimesBuilding[rank + 10 * (int)building.kind] = value; }
    public override double accomplishedBestTime { get => main.SR.accomplishedBestTimesBuilding[rank + 10 * (int)building.kind]; set => main.SR.accomplishedBestTimesBuilding[rank + 10 * (int)building.kind] = value; }
}

public class StatueOfHeroes : BUILDING
{
    public override BuildingKind kind => BuildingKind.StatueOfHeroes;
    public override long maxRank => maxBuildingRank;
    public override string NameString()
    {
        return "Statue of Heroes";
    }
    public override string LevelEffectString()
    {
        return optStr + "EXP Gain <color=green>+ " + percent(EffectValue()) + "</color>";
    }
    double EffectValue() { return Level() * 0.05 * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[0].Value(); }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.guildCtrl.Level() >= 5, () => "Guild Level 5");
        rankTransaction.AddCost(0, game.essenceCtrl.Essence(EssenceKind.EssenceOfLife), 5);
        rankUpConditions[1].RegisterCondition(() => game.guildCtrl.Level() >= 20, () => "Guild Level 20");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.SlimeBall), 50);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.ManaSeed), 10);
        rankUpConditions[2].RegisterCondition(() => game.guildCtrl.Level() >= 40, () => "Guild Level 40");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.SlimeBall), 300);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.ManaSeed), 300);
        rankUpConditions[3].RegisterCondition(() => game.guildCtrl.Level() >= 65, () => "Guild Level 65");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 1);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.SlimeBall), 10000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.ManaSeed), 5000);
        rankUpConditions[4].RegisterCondition(() => game.guildCtrl.Level() >= 100, () => "Guild Level 100");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 5);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.SlimeBall), 500000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.ManaSeed), 100000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (level) => PrimaryCost(level, 0));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (level) => SecondaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (level) => SecondaryCost(level, 4));
    }
    public override void SetEffect()
    {
        game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(Town, Add, EffectValue));

        townCtrl.maxRank.RegisterMultiplier(new MultiplierInfo(Town, Add, () => Rank()));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, null, "Max Rank for all other buildings to 1"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, null, "Max Rank for all other buildings to 2"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, null, "Max Rank for all other buildings to 3"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, null, "Max Rank for all other buildings to 4"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, null, "Max Rank for all other buildings to 5"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.1d, x)), "Guild EXP Gain + 10%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.skillCtrl.globalSkillProfGainRate.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.20d, x)), "Skill Prof. Gain from Global Slot + 20%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(Town, Mul, () => 0.50d, x)), "Gold Cap + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(Town, Add, () => 0.5d, x)), "Town Material Gain + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => game.questCtrl.Quest(QuestKindDaily.EC2).unlockConditions.Add(x), "Unlock new Epic Coin Daily Quest"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.skillCtrl.globalSkillProfGainRate.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.30d, x)), "Skill Prof. Gain from Global Slot + 30%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(Town, Mul, () => 1.00d, x)), "Gold Cap + 100%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(Town, Add, () => 0.5d, x)), "Town Material Gain + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 45, (x) => game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(Town, Mul, () => 2.00d, x)), "Gold Cap + 200%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.guildCtrl.expGain.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.2d, x)), "Guild EXP Gain + 20%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 55, (x) => game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(Town, Mul, () => 3.00d, x)), "Gold Cap + 300%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(Town, Add, () => 1.0d, x)), "Town Material Gain + 100%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.skillCtrl.globalSkillProfGainRate.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.50d, x)), "Skill Prof. Gain from Global Slot + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(Town, Add, () => 1.0d, x)), "Town Material Gain + 100%"));
    }
    public override void SetResearch()
    {
        game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Stone) * 0.10d));
        game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Crystal) * 0.05));
        townCtrl.levelCostReduction.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Leaf) * 0.0075));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Resource Gain <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.10d) + "</color>" + " ( + " + percent(0.10d) + " / Lv )",
                "Gold Cap <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>" + " ( + " + percent(0.05d) + " / Lv )",
                "Reduce the cost of all building levels by <color=green>" + percent(ResearchLevel(ResourceKind.Leaf) * 0.0075) + "</color>" + " ( + " + percent(0.0075, 3) + " / Lv )"
                );
        }
        return (
            "Resource Gain <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.10d) + "</color>",
            "Gold Cap <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>",
            "Reduce the cost of all building levels by <color=green>" + percent(ResearchLevel(ResourceKind.Leaf) * 0.0075) + "</color>"
            );
    }
}
public class Cartographer : BUILDING
{
    public override BuildingKind kind => BuildingKind.Cartographer;
    public override string NameString()
    {
        return "Cartographer";
    }
    public override string LevelEffectString()
    {
        return optStr + "Gold Gain <color=green>+ " + percent(EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.areaCtrl.Area(AreaKind.SlimeVillage, 4).completedNum.TotalCompletedNum() > 0, () => "Clear " + game.areaCtrl.Area(AreaKind.SlimeVillage, 4).Name(true, false));
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.MonsterFluid), 10);
        rankUpConditions[1].RegisterCondition(() => game.areaCtrl.Dungeon(AreaKind.MagicSlimeCity, 2).completedNum.TotalCompletedNum() > 0, () => "Clear " + game.areaCtrl.Dungeon(AreaKind.MagicSlimeCity, 2).Name(true, false));
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.MonsterFluid), 50);
        rankUpConditions[2].RegisterCondition(() => game.areaCtrl.Dungeon(AreaKind.BatCave, 1).completedNum.TotalCompletedNum() > 0, () => "Clear " + game.areaCtrl.Dungeon(AreaKind.BatCave, 1).Name(true, false));
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.MonsterFluid), 250);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.EternalFlame), 50);
        rankUpConditions[3].RegisterCondition(() => game.areaCtrl.Dungeon(AreaKind.FairyGarden, 3).completedNum.TotalCompletedNum() > 0, () => "Clear " + game.areaCtrl.Dungeon(AreaKind.FairyGarden, 3).Name(true, false));
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 1);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.MonsterFluid), 1000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.EternalFlame), 500);
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        //rankUpConditions[4].RegisterCondition(() => game.areaCtrl.Dungeon(AreaKind.MagicSlimeCity, 1).completedNum.TotalCompletedNum() > 0, () => "Clear " + game.areaCtrl.Dungeon(AreaKind.MagicSlimeCity, 1).Name(true, false));
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 5);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.MonsterFluid), 5000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.EternalFlame), 5000);

        //rank
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (level) => PrimaryCost(level, 0));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (level) => SecondaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (level) => SecondaryCost(level, 4));
    }
    double EffectValue() { return Level() * 0.05 * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[1].Value(); }
    public override void SetEffect()
    {
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, EffectValue));

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => game.questCtrl.Quest(QuestKindDaily.Cartographer1).unlockConditions.Add(x), "Unlock new Cartographer Daily Quest"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => game.questCtrl.Quest(QuestKindDaily.Cartographer2).unlockConditions.Add(x), "Unlock new Cartographer Daily Quest"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => game.questCtrl.Quest(QuestKindDaily.Cartographer3).unlockConditions.Add(x), "Unlock new Cartographer Daily Quest"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => game.questCtrl.Quest(QuestKindDaily.Cartographer4).unlockConditions.Add(x), "Unlock new Cartographer Daily Quest"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => game.questCtrl.Quest(QuestKindDaily.Cartographer5).unlockConditions.Add(x), "Unlock new Cartographer Daily Quest"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.MagicSlimeCity], (x) => { game.areaCtrl.unlocks[(int)AreaKind.MagicSlimeCity].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.EnchantedCloth).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.SpiderMaze], (x) => { game.areaCtrl.unlocks[(int)AreaKind.SpiderMaze].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.SpiderSilk).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.BatCave], (x) => { game.areaCtrl.unlocks[(int)AreaKind.BatCave].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.BatWing).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.FairyGarden], (x) => { game.areaCtrl.unlocks[(int)AreaKind.FairyGarden].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.FairyDust).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => { game.questCtrl.portalOrbBonusFromDailyQuest.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Portal Orb Gain from Daily Quest + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.FoxShrine], (x) => { game.areaCtrl.unlocks[(int)AreaKind.FoxShrine].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.FoxTail).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.DevilFishLake], (x) => { game.areaCtrl.unlocks[(int)AreaKind.DevilFishLake].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.FishScales).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => { game.questCtrl.portalOrbBonusFromDailyQuest.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Portal Orb Gain from Daily Quest + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.TreantDarkForest], (x) => { game.areaCtrl.unlocks[(int)AreaKind.TreantDarkForest].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.CarvedBranch).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.FlameTigerVolcano], (x) => { game.areaCtrl.unlocks[(int)AreaKind.FlameTigerVolcano].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.ThickFur).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => { game.questCtrl.portalOrbBonusFromDailyQuest.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Portal Orb Gain from Daily Quest + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, areaUnlockLevels[(int)AreaKind.UnicornIsland], (x) => { game.areaCtrl.unlocks[(int)AreaKind.UnicornIsland].RegisterCondition(x); game.shopCtrl.Material(MaterialKind.UnicornHorn).unlock.RegisterCondition(x); }, "Unlock new region and Shop's material"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 90, (x) => { game.questCtrl.portalOrbBonusFromDailyQuest.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Portal Orb Gain from Daily Quest + 1"));
    }
    public static long[] areaUnlockLevels = new long[] { 0, 5, 10, 20, 30, 45, 55, 65, 75, 85 };
    //public static long[] areaUnlockLevels = new long[] { 0, 5, 15, 25, 35, 45, 55, 65, 75, 85};
    public override void SetResearch()
    {
        for (int i = 0; i < game.areaCtrl.dungeonList.Count; i++)
        {
            game.areaCtrl.dungeonList[i].addLimitTime.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 5));
        }
        game.areaCtrl.areaDebuffReduction.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal) * 0.0075d));
        //game.areaCtrl.maxAreaExpLevel.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone)));
        //game.areaCtrl.maxAreaMoveSpeedLevel.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal)));
        game.areaCtrl.townMaterialGainBonus.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Leaf)));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Dungeon's Time Limit <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 5) + " sec</color>" + " ( + " + tDigit(5) + " / Lv )",
                "Reduce Field Debuff effect by<color=green> " + percent(ResearchLevel(ResourceKind.Crystal) * 0.0075d) + "</color>" + " ( + " + percent(0.0075d, 3) + " / Lv )",
                "Town Material Gain from clearing areas <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Leaf)) + "</color>" + " ( + " + tDigit(1) + " / Lv )"
                );
        }
        return (
            "Dungeon's Time Limit <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 5) + " sec</color>",
            "Reduce Field Debuff effect by<color=green> " + percent(ResearchLevel(ResourceKind.Crystal) * 0.0075d) + "</color>",
            "Town Material Gain from clearing areas <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Leaf)) + "</color>"
            );
    }

}
public class AlchemistsHut : BUILDING
{
    public override BuildingKind kind => BuildingKind.AlchemistsHut;
    public override string NameString()
    {
        return "Alchemist's Hut";
    }
    public override string LevelEffectString()
    {
        return optStr + "Material's Max Stock # <color=green>+ " + tDigit(EffectValue()) + "</color> in Shop";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 30, () => "Expand Mysterious Water Cap to 30");
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.SlimeBall), 2);
        rankUpConditions[1].RegisterCondition(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 120, () => "Expand Mysterious Water Cap to 120");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.ManaSeed), 20);
        rankUpConditions[2].RegisterCondition(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 200, () => "Expand Mysterious Water Cap to 200");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 20);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.EternalFlame), 20);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.AncientBattery), 20);
        rankUpConditions[3].RegisterCondition(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 350, () => "Expand Mysterious Water Cap to 350");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 1);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 100);
        rankUpConditions[4].RegisterCondition(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 500, () => "Expand Mysterious Water Cap to 500");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 5);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 500);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (level) => PrimaryCost(level, 0));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (level) => SecondaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (level) => SecondaryCost(level, 4));
    }
    double EffectValue() { return Level() * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[2].Value(); }
    public override void SetEffect()
    {
        game.shopCtrl.materialStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, EffectValue));

        //本来は、Unlock Quest for new catalyst
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Mana).unlock.RegisterCondition(x), "Unlock new Catalyst"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) =>
        {
            game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Frost).unlock.RegisterCondition(x);
            game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Flame).unlock.RegisterCondition(x);
            game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Storm).unlock.RegisterCondition(x);
        }
        , "Unlock new Catalyst"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Soul).unlock.RegisterCondition(x), "Unlock new Catalyst"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Sun).unlock.RegisterCondition(x), "Unlock new Catalyst"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => game.alchemyCtrl.catalystCtrl.Catalyst(CatalystKind.Void).unlock.RegisterCondition(x), "Unlock new Catalyst"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.05, x)), "Mysterious Water Gain + 0.05 / sec"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 100, x)), "Max Mysterious Water Cap + 100"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Utility Inventory Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.shopCtrl.sellPriceRate.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.1d, x)), "Increase Sell Price in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => game.alchemyCtrl.maxPurificationLevel.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 50, x)), "Purification's Max Level + 50"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.05, x)), "Mysterious Water Gain + 0.05 / sec"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Utility Inventory Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 150, x)), "Max Mysterious Water Cap + 150"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.alchemyCtrl.maxWaterPreservationLevel.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Water Preservation's Max Level + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 55, (x) => game.shopCtrl.sellPriceRate.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.2d, x)), "Increase Sell Price in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 250, x)), "Max Mysterious Water Cap + 250"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 65, (x) => game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Utility Inventory Slot + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.alchemyCtrl.maxPurificationLevel.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 100, x)), "Purification's Max Level + 100"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 75, (x) => game.alchemyCtrl.maxWaterPreservationLevel.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Water Preservation's Max Level + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.shopCtrl.Material(MaterialKind.MonsterFluid).unlock.RegisterCondition(x), "Unlock Monster Fluid in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 85, (x) => game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 500, x)), "Max Mysterious Water Cap + 500"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 100, (x) =>
        {
            game.shopCtrl.Material(MaterialKind.FlameShard).unlock.RegisterCondition(x);
            game.shopCtrl.Material(MaterialKind.FrostShard).unlock.RegisterCondition(x);
            game.shopCtrl.Material(MaterialKind.LightningShard).unlock.RegisterCondition(x);
            game.shopCtrl.Material(MaterialKind.NatureShard).unlock.RegisterCondition(x);
            game.shopCtrl.Material(MaterialKind.PoisonShard).unlock.RegisterCondition(x);
        },
        "Unlock Rare Materials in Shop"));
        //passiveEffectList.Add(new BuildingPassiveEffect(level, 100, (x) => game.shopCtrl.Material(MaterialKind.BlackPearl).unlock.RegisterCondition(x), "Unlock Black Pearl in Shop"));
        game.shopCtrl.Material(MaterialKind.BlackPearl).unlock.RegisterCondition(() => false);//別のものでアンロックできるようにする
    }

    public override void SetResearch()
    {
        game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 0.01));
        game.alchemyCtrl.catalystCtrl.essenceProductionMultiplier.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 0.01));
        game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Leaf) * 5));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Mysterious Water Gain <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 0.01, 2) + " / sec</color>" + " ( + " + tDigit(0.01, 3) + " / Lv )",
                "Essence Conversion Rate <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.01) + "</color>" + " ( + " + percent(0.01) + " / Lv )",
                "Max Mysterious Water Cap <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Leaf) * 5) + "</color>" + " ( + " + tDigit(5) + " / Lv )"
                );
        }
        return (
            "Mysterious Water Gain <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 0.01, 2) + " / sec</color>" ,
            "Essence Conversion Rate <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.01) + "</color>",
            "Max Mysterious Water Cap <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Leaf) * 5) + "</color>"
            );
    }
}
public class Blacksmith : BUILDING
{
    public override BuildingKind kind => BuildingKind.Blacksmith;
    public override long unlockGuildLevel => 25;
    public override string NameString()
    {
        return "Blacksmith";
    }
    public override string LevelEffectString()
    {
        return optStr + "Multiply Equipment's Effect by <color=green>" + percent(1 + EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.equipmentCtrl.DictionaryTotalPoint() >= 20, () => "Dictionary Point Gained : " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " / 20");
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.SlimeBall), 50);
        rankUpConditions[1].RegisterCondition(() => game.equipmentCtrl.DictionaryTotalPoint() >= 100, () => "Dictionary Point Gained : " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " / 100");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.SlimeBall), 500);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.AncientBattery), 10);
        rankUpConditions[2].RegisterCondition(() => game.equipmentCtrl.DictionaryTotalPoint() >= 250, () => "Dictionary Point Gained : " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " / 250");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.SlimeBall), 1000);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.AncientBattery), 50);
        rankUpConditions[3].RegisterCondition(() => game.equipmentCtrl.DictionaryTotalPoint() >= 1500, () => "Dictionary Point Gained : " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " / 1500");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 2);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.SlimeBall), 5000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.AncientBattery), 100);
        rankUpConditions[4].RegisterCondition(() => game.equipmentCtrl.DictionaryTotalPoint() >= 10000, () => "Dictionary Point Gained : " + tDigit(game.equipmentCtrl.DictionaryTotalPoint()) + " / 10000");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 10);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.SlimeBall), 10000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.AncientBattery), 1000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (level) => PrimaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (level) => SecondaryCost(level, 3));
    }
    double EffectValue() { return Level() * 0.01d * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[0].Value(); }
    void UnlockCraft(int rarity, Func<bool> condition)
    {
        switch (rarity)
        {
            case 2:
                game.craftCtrl.CraftScroll(EnchantKind.OptionLevelup).unlock.RegisterCondition(condition);
                game.craftCtrl.CraftScroll(EnchantKind.OptionLottery).unlock.RegisterCondition(condition);
                for (int i = 0; i < game.craftCtrl.enchantScroll2List.Count; i++)
                {
                    game.craftCtrl.enchantScroll2List[i].unlock.RegisterCondition(condition);
                }
                break;
            case 3:
                game.craftCtrl.CraftScroll(EnchantKind.OptionLevelMax).unlock.RegisterCondition(condition);
                for (int i = 0; i < game.craftCtrl.enchantScroll3List.Count; i++)
                {
                    game.craftCtrl.enchantScroll3List[i].unlock.RegisterCondition(condition);
                }
                break;
            case 4:
                game.craftCtrl.CraftScroll(EnchantKind.OptionCopy).unlock.RegisterCondition(condition);
                for (int i = 0; i < game.craftCtrl.enchantScroll4List.Count; i++)
                {
                    game.craftCtrl.enchantScroll4List[i].unlock.RegisterCondition(condition);
                }
                break;
            case 5:
                game.craftCtrl.CraftScroll(EnchantKind.ExpandEnchantSlot).unlock.RegisterCondition(condition);
                for (int i = 0; i < game.craftCtrl.enchantScroll5List.Count; i++)
                {
                    game.craftCtrl.enchantScroll5List[i].unlock.RegisterCondition(condition);
                }
                break;
        }
    }
    public override void SetEffect()
    {
        game.equipmentCtrl.effectMultiplier.RegisterMultiplier(new MultiplierInfo(Town, Add, EffectValue));

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => { }, "Unlock [Craft] in Lab tab"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => UnlockCraft(2, x), "Unlock new craftable Scrolls"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => UnlockCraft(3, x), "Unlock new craftable Scrolls"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => UnlockCraft(4, x), "Unlock new craftable Scrolls"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => UnlockCraft(5, x), "Unlock new craftable Scrolls"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.equipmentCtrl.DictionaryUpgrade(DictionaryUpgradeKind.EnchantedEffectChance1).unlock.RegisterCondition(x), "Unlock a new Dictionary Upgrade"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Equipment Inventory Slot + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.equipmentCtrl.autoDisassembleAvailableNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Auto-Disassemble Equipment Slot + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.equipmentCtrl.canCrafts[(int)EquipmentRarity.Common].RegisterCondition(x), "Enable to craft Common EQ in Dictionary"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => game.inventoryCtrl.enchantUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 3, x)), "Enchant Inventory Slot + 3"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Equipment Inventory Slot + 10"));
        //passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.equipmentCtrl.autoDisassembleAvailableNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Auto-Disassemble Equipment Slot + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.equipmentCtrl.canCrafts[(int)EquipmentRarity.Uncommon].RegisterCondition(x), "Enable to craft Uncommon Equipment"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 45, (x) => game.inventoryCtrl.enchantUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 3, x)), "Enchant Inventory Slot + 3"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.equipmentCtrl.DictionaryUpgrade(DictionaryUpgradeKind.EnchantedEffectChance2).unlock.RegisterCondition(x), "Unlock a new Dictionary Upgrade"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 10, x)), "Equipment Inventory Slot + 10"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.equipmentCtrl.canCrafts[(int)EquipmentRarity.Rare].RegisterCondition(x), "Enable to craft Rare Equipment"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 75, (x) => game.inventoryCtrl.enchantUnlockedNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Enchant Inventory Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.equipmentCtrl.DictionaryUpgrade(DictionaryUpgradeKind.EnchantedEffectChance3).unlock.RegisterCondition(x), "Unlock a new Dictionary Upgrade"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 90, (x) => game.equipmentCtrl.canCrafts[(int)EquipmentRarity.SuperRare].RegisterCondition(x), "Enable to craft Super Rare Equipment"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 100, (x) => game.equipmentCtrl.canCrafts[(int)EquipmentRarity.Epic].RegisterCondition(x), "Enable to craft Epic Equipment"));
    }
    public override void SetResearch()
    {
        game.equipmentCtrl.autoDisassembleAvailableNum.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 2));
        game.equipmentCtrl.disassembleMultiplier.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal) * 0.05d));
        game.craftCtrl.costReduction.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Leaf) * 0.0075d));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Auto-Disassemble EQ Slot <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 2) + "</color>" + " ( + " + tDigit(2) + " / Lv )",
                "Material Gain on Disassembling EQ <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>" + " ( + " + percent(0.05) + " / Lv )",
                "Reduce the cost of crafting by <color=green>" + percent(ResearchLevel(ResourceKind.Leaf) * 0.0075d) + "</color>" + " ( + " + percent(0.0075d, 3) + " / Lv )"
                );
        }
        return (
            "Auto-Disassemble EQ Slot <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Stone) * 2) + "</color>",
            "Material Gain on Disassembling EQ <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>",
            "Reduce the cost of crafting by <color=green>" + percent(ResearchLevel(ResourceKind.Leaf) * 0.0075d) + "</color>"
            );
    }

}
public class Temple : BUILDING
{
    public override BuildingKind kind => BuildingKind.Temple;
    public override long unlockGuildLevel => 20;
    public override string NameString()
    {
        return "Temple";
    }
    public override string LevelEffectString()
    {
        return optStr + "Rebirth Point Gain <color=green>+ " + percent(EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.rebirthCtrl.TotalRebirthNum(0) >= 3, () => "Total Rebirth Tier 1 # : " + tDigit(game.rebirthCtrl.TotalRebirthNum(0)) + " / 3");
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.ManaSeed), 20);
        rankUpConditions[1].RegisterCondition(() => game.rebirthCtrl.TotalRebirthNum(1) >= 5, () => "Total Rebirth Tier 2 # : " + tDigit(game.rebirthCtrl.TotalRebirthNum(1)) + " / 5");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 10);
        rankUpConditions[2].RegisterCondition(() => game.rebirthCtrl.TotalRebirthNum(2) >= 20, () => "Total Rebirth Tier 3 # : " + tDigit(game.rebirthCtrl.TotalRebirthNum(2)) + " / 20");
        rankUpConditions[2].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 200);
        rankUpConditions[3].RegisterCondition(() => game.rebirthCtrl.TotalRebirthNum(3) >= 25, () => "Total Rebirth Tier 4 # : " + tDigit(game.rebirthCtrl.TotalRebirthNum(3)) + " / 25");
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 2);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 2500);
        rankUpConditions[4].RegisterCondition(() => game.rebirthCtrl.TotalRebirthNum(4) >= 100, () => "Total Rebirth Tier 5 # : " + tDigit(game.rebirthCtrl.TotalRebirthNum(4)) + " / 100");
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 10);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 10000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (level) => PrimaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (level) => SecondaryCost(level, 3));
    }
    double EffectValue() { return Level() * 0.01 * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[1].Value(); }
    public override void SetEffect()
    {
        game.rebirthCtrl.pointMultiplier.RegisterMultiplier(new MultiplierInfo(Town, Add, EffectValue));

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => game.rebirthCtrl.unlocks[1].RegisterCondition(x), "Unlock Tier 2 Rebirth"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => game.rebirthCtrl.unlocks[2].RegisterCondition(x), "Unlock Tier 3 Rebirth"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => game.rebirthCtrl.unlocks[3].RegisterCondition(x), "Unlock Tier 4 Rebirth"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => game.rebirthCtrl.unlocks[4].RegisterCondition(x), "Unlock Tier 5 Rebirth"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => game.rebirthCtrl.unlocks[5].RegisterCondition(x), "Unlock Tier 6 Rebirth"));

        //Blessing
        //HPは最初から解禁
        //passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.shopCtrl.Blessing(BlessingKind.Hp).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.shopCtrl.Blessing(BlessingKind.Atk).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.shopCtrl.Blessing(BlessingKind.MAtk).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.shopCtrl.Blessing(BlessingKind.MoveSpeed).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.shopCtrl.blessingStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Increase Blessing's Available # + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.shopCtrl.Blessing(BlessingKind.SkillProficiency).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.shopCtrl.Blessing(BlessingKind.EquipProficiency).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.shopCtrl.blessingStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Increase Blessing's Available # + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.shopCtrl.Blessing(BlessingKind.GoldGain).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 90, (x) => game.shopCtrl.Blessing(BlessingKind.ExpGain).unlock.RegisterCondition(x), "Unlock new Blessing in Shop"));
    }
    public override void SetResearch()
    {
        game.blessingInfoCtrl.effectMultiplier.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 0.025));
        game.blessingInfoCtrl.duration.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal) * 5));
        game.rebirthCtrl.requiredHeroLevelReduction.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Leaf)));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Multiply Blessing's effect by <color=green>" + percent(1 + ResearchLevel(ResourceKind.Stone) * 0.025) + "</color>" + " ( + " + percent(0.025) + " / Lv )",
                "Blessing's duration <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal) * 5) + " sec</color>" + " ( + " + tDigit(5) + " / Lv )",
                "Rebirth's level requirements <color=green>- " + tDigit(ResearchLevel(ResourceKind.Leaf)) + "</color>" + " ( - " + tDigit(1) + " / Lv ) Minimum Lv 50"
                );
        }
        return (
            "Multiply Blessing's effect by <color=green>" + percent(1 + ResearchLevel(ResourceKind.Stone) * 0.025) + "</color>",
            "Blessing's duration <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal) * 5) + " sec</color>",
            "Rebirth's level requirements <color=green>- " + tDigit(ResearchLevel(ResourceKind.Leaf)) + "</color> ( Minimum Lv 50 )"
            );
    }
}
public class Trapper : BUILDING
{
    public override BuildingKind kind => BuildingKind.Trapper;
    public override long unlockGuildLevel => 15;
    public override string NameString()
    {
        return "Trapper";
    }
    public override string LevelEffectString()
    {
        return optStr + "Capturable Monster Max Lv : <color=green>+ " + tDigit(EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.monsterCtrl.TotalDefeatedNum() > 100000, () => "Defeat any monsters : " + tDigit(game.monsterCtrl.TotalDefeatedNum()) + " / " + tDigit(100000));
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.MonsterFluid), 25);
        rankUpConditions[1].RegisterCondition(() => game.monsterCtrl.TotalDefeatedNum() > 500000, () => "Defeat any monsters : " + tDigit(game.monsterCtrl.TotalDefeatedNum()) + " / " + tDigit(500000));
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.FrostShard), 25);
        rankUpConditions[2].RegisterCondition(() => game.monsterCtrl.TotalDefeatedNum() > 1000000, () => "Defeat any monsters : " + tDigit(game.monsterCtrl.TotalDefeatedNum()) + " / " + tDigit(1000000));
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.LightningShard), 50);
        rankUpConditions[3].RegisterCondition(() => game.monsterCtrl.TotalDefeatedNum() > 5000000, () => "Defeat any monsters : " + tDigit(game.monsterCtrl.TotalDefeatedNum()) + " / " + tDigit(5000000));
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 2);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.FlameShard), 100);
        rankUpConditions[4].RegisterCondition(() => game.monsterCtrl.TotalDefeatedNum() > 10000000, () => "Defeat any monsters : " + tDigit(game.monsterCtrl.TotalDefeatedNum()) + " / " + tDigit(10000000));
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 10);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.NatureShard), 500);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.PoisonShard), 500);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (level) => PrimaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (level) => SecondaryCost(level, 3));
    }
    double EffectValue() { return Level() * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[2].Value(); }
    public override void SetEffect()
    {
        for (int i = 0; i < game.monsterCtrl.monsterCapturableLevel.Length; i++)
        {
            game.monsterCtrl.monsterCapturableLevel[i].RegisterMultiplier(new MultiplierInfo(Base, Add, () => EffectValue()));
        }

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => { game.shopCtrl.Trap(PotionKind.IceRope).unlock.RegisterCondition(x); }, "Unlock new Trap in Shop"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => { game.shopCtrl.Trap(PotionKind.ThunderRope).unlock.RegisterCondition(x); }, "Unlock new Trap in Shop"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => { game.shopCtrl.Trap(PotionKind.FireRope).unlock.RegisterCondition(x); }, "Unlock new Trap in Shop"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => { game.shopCtrl.Trap(PotionKind.LightRope).unlock.RegisterCondition(x); }, "Unlock new Trap in Shop"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => { game.shopCtrl.Trap(PotionKind.DarkRope).unlock.RegisterCondition(x); }, "Unlock new Trap in Shop"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(Town, Add, () => 0.25d, x)), "Taming Point Gain + 25%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.shopCtrl.trapStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 20, x)), "Trap's Max Stock # + 20 in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.monsterCtrl.petActiveCap.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Active Pet Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.potionCtrl.trapCooltimeReduction.RegisterMultiplier(new MultiplierInfo(Town, Add, () => -2, x)), "Decrease Trap's cooltime by 2 sec"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(Town, Add, () => 0.25d, x)), "Taming Point Gain + 25%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.shopCtrl.trapStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 20, x)), "Trap's Max Stock # + 20 in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.monsterCtrl.petActiveCap.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Active Pet Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.potionCtrl.trapCooltimeReduction.RegisterMultiplier(new MultiplierInfo(Town, Add, () => -2, x)), "Decrease Trap's cooltime by 2 sec"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 45, (x) => game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(Town, Add, () => 0.50d, x)), "Taming Point Gain + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.monsterCtrl.petActiveCap.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Active Pet Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.shopCtrl.trapStockNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 40, x)), "Trap's Max Stock # + 40 in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.potionCtrl.trapCooltimeReduction.RegisterMultiplier(new MultiplierInfo(Town, Add, () => -2, x)), "Decrease Trap's cooltime by 2 sec"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.monsterCtrl.petActiveCap.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 5, x)), "Active Pet Slot + 5"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 90, (x) => game.potionCtrl.trapCooltimeReduction.RegisterMultiplier(new MultiplierInfo(Town, Add, () => -2, x)), "Decrease Trap's cooltime by 2 sec"));
        //passiveEffectList.Add(new BuildingPassiveEffect(level, 100, (x) => game.potionCtrl.trapEffectMultiplier.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.30d, x)), "Increase Capture Chance + 30.00%"));
    }

    public override void SetResearch()
    {
        game.monsterCtrl.colorMaterialDropChance.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 0.00002));
        game.shopCtrl.restockNumTrap.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal)));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Leaf) * 0.02d));

    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Rare Material Drop Chance <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.00002, 4) + "</color>" + " ( + " + percent(0.00002, 4) + " / Lv )",
                "Restock amount of traps in Shop <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal)) + "</color>" + " ( + " + tDigit(1) + " / Lv )",
                "Multiply Taming Point Gain by<color=green> " + percent(ResearchLevel(ResourceKind.Leaf) * 0.02d) + "</color>" + " ( + " + percent(0.02) + " / Lv )"
                );
        }
        return (
            "Rare Material Drop Chance <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.00002,4) + "</color>",
            "Restock amount of traps in Shop <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal)) + "</color>",
            "Multiply Taming Point Gain by<color=green> " + percent(ResearchLevel(ResourceKind.Leaf) * 0.02d) + "</color>"
            );
    }

}
public class SlimeBank : BUILDING
{
    public override BuildingKind kind => BuildingKind.SlimeBank;
    public override long unlockGuildLevel => 35;
    public override string NameString()
    {
        return "Slime Bank";
    }
    public override string LevelEffectString()
    {
        return "Slime Coin Gain <color=green>+ " + percent(EffectValue()) + "</color>";// optStr + "Multiply Equipment's Effect by <color=green>" + percent(1 + EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        //Slimeのキャプチャー数
        rankUpConditions[0].RegisterCondition(() => game.monsterCtrl.CapturedNum(MonsterSpecies.Slime) >= 50, () => "Captured Slimes : " + tDigit(game.monsterCtrl.CapturedNum(MonsterSpecies.Slime)) + " / 50");
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.SlimeBall), 200);
        rankUpConditions[1].RegisterCondition(() => game.monsterCtrl.CapturedNum(MonsterSpecies.Slime) >= 500, () => "Captured Slimes : " + tDigit(game.monsterCtrl.CapturedNum(MonsterSpecies.Slime)) + " / 500");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.SlimeBall), 500);
        rankUpConditions[2].RegisterCondition(() => game.monsterCtrl.CapturedNum(MonsterSpecies.Slime) >= 5000, () => "Captured Slimes : " + tDigit(game.monsterCtrl.CapturedNum(MonsterSpecies.Slime)) + " / 5000");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.SlimeBall), 5000);
        rankUpConditions[3].RegisterCondition(() => game.monsterCtrl.CapturedNum(MonsterSpecies.Slime) >= 50000, () => "Captured Slimes : " + tDigit(game.monsterCtrl.CapturedNum(MonsterSpecies.Slime)) + " / 50000");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 3);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.SlimeBall), 50000);
        rankUpConditions[4].RegisterCondition(() => game.monsterCtrl.CapturedNum(MonsterSpecies.Slime) >= 500000, () => "Captured Slimes : " + tDigit(game.monsterCtrl.CapturedNum(MonsterSpecies.Slime)) + " / 500000");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 15);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.SlimeBall), 500000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (level) => PrimaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (level) => SecondaryCost(level, 3));
    }
    double EffectValue() { return Level() * 0.01d * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[0].Value(); }
    public override void SetEffect()
    {
        //effect
        game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Mul, EffectValue));

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => { }, "Unlock Slime Bank in Upgrade tab"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => {
            RegisterUnlockSB(SlimeBankUpgradeKind.MysteriousWaterGain, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.SPD, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.FireRes, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.IceRes, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.ThunderRes, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.LightRes, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.DarkRes, x);
        }, "Unlock new Slime Bank upgrades"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => {
            RegisterUnlockSB(SlimeBankUpgradeKind.MaterialFinder, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.DebuffRes, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.SkillProf, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.EquipmentProf, x);
        }, "Unlock new Slime Bank upgrades"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => {
            RegisterUnlockSB(SlimeBankUpgradeKind.ShopTimer, x);
            RegisterUnlockSB(SlimeBankUpgradeKind.CritDamage, x);
        }, "Unlock new Slime Bank upgrades"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => {
            RegisterUnlockSB(SlimeBankUpgradeKind.ResearchPower, x);
        }, "Unlock new Slime Bank upgrades"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00050d, x)), "Slime Coin Efficiency + 0.050%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 10, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.10d, x)), "Gold Gain + 10%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00075d, x)), "Slime Coin Efficiency + 0.075%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 20, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.20d, x)), "Gold Gain + 20%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00100d, x)), "Slime Coin Efficiency + 0.100%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 30, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.30d, x)), "Gold Gain + 30%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00150d, x)), "Slime Coin Efficiency + 0.150%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 40, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.40d, x)), "Gold Gain + 40%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 45, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00200d, x)), "Slime Coin Efficiency + 0.200%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 50, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.50d, x)), "Gold Gain + 50%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 55, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00250d, x)), "Slime Coin Efficiency + 0.250%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 60, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.75d, x)), "Gold Gain + 75%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 65, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00300d, x)), "Slime Coin Efficiency + 0.300%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 70, (x) => game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1.00d, x)), "Gold Gain + 100%"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 80, (x) => game.resourceCtrl.slimeCoinEfficiency.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 0.00500d, x)), "Slime Coin Efficiency + 0.500%"));
    }
    public override void SetResearch()
    {
        game.resourceCtrl.slimeCoinInterest.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Stone) * 0.001d));
        game.shopCtrl.restockNumMaterial.RegisterMultiplier(new MultiplierInfo(TownResearch, Add, () => ResearchLevel(ResourceKind.Crystal)));
        game.resourceCtrl.slimeCoinCap.RegisterMultiplier(new MultiplierInfo(TownResearch, Mul, ()=> ResearchLevel(ResourceKind.Leaf) * 0.020d));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Grants interest on currently held Slime Coin every 10 minutes : <color=green>" + percent(ResearchLevel(ResourceKind.Stone) * 0.001d) + "</color>" + " ( + " + percent(0.001) + " / Lv )",
                "Restock amount of materials in Shop <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal)) + "</color>" + " ( + " + tDigit(1) + " / Lv )",
                "Multiply Slime Coin Cap by <color=green>" + percent(1 + ResearchLevel(ResourceKind.Leaf) * 0.020d) + "</color>" + " ( + " + percent(0.020) + " / Lv )"
                );
        }
        return (
            "Grants interest on currently held Slime Coin every 10 minutes : <color=green>" + percent(ResearchLevel(ResourceKind.Stone) * 0.001d) + "</color>",
            "Restock amount of materials in Shop <color=green>+ " + tDigit(ResearchLevel(ResourceKind.Crystal)) + "</color>",
            "Multiply Slime Coin Cap by <color=green>" + percent(1+ ResearchLevel(ResourceKind.Leaf) * 0.020d) + "</color>"
            );
    }
    void RegisterUnlockSB(SlimeBankUpgradeKind kind, Func<bool> condition)
    {
        game.upgradeCtrl.SlimeBankUpgrade(kind).unlock.RegisterCondition(condition);
    }
}
public class MysticArena : BUILDING
{
    public override BuildingKind kind => BuildingKind.MysticArena;
    public override long unlockGuildLevel => 40;
    public override string NameString()
    {
        return "Mystic Arena";
    }
    public override string LevelEffectString()
    {
        return "";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).IsClearedOnce(), () => "Clear Raid Challenge [" + game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).NameString() + "]");
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 50);
        rankUpConditions[1].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 100);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.Ectoplasm), 20);
        rankUpConditions[2].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 1000);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 100);
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 3);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 5000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 500);
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 15);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 10000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Ectoplasm), 2000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (level) => PrimaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (level) => SecondaryCost(level, 3));
    }
    //Effect : AreaDebuffを減少させる、など
}
public class ArcaneResearcher : BUILDING
{
    public override BuildingKind kind => BuildingKind.ArcaneResearcher;
    public override long unlockGuildLevel => 50;//45;
    public override string NameString()
    {
        return "Arcane Researcher";
    }
    public override string LevelEffectString()
    {
        return "Multiply Research Power by<color=green> " + percent(1+EffectValue()) + "</color>";
    }
    public override void SetCost()
    {
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.OilOfSlime), 1000);
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.EnchantedCloth), 1000);
        rankTransaction.AddCost(0, game.materialCtrl.Material(MaterialKind.SpiderSilk), 1000);

        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.BlackPearl), 1);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.BatWing), 2500);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.FairyDust), 2500);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.UnmeltingIce), 30);
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.Ectoplasm), 3);

        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.BlackPearl), 2);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.FoxTail), 5000);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.FishScales), 5000);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.AncientBattery), 50);
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 20);

        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 3);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.CarvedBranch), 10000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.ThickFur), 10000);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 100);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Stardust), 5);

        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 15);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.UnicornHorn), 10000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 100);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 100);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 10);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (level) => PrimaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (level) => SecondaryCost(level, 3));
    }
    double EffectValue() { return Level() * 0.05d * townCtrl.townLevelEffectMultiplier.Value() * townCtrl.townLevelEffectMultipliers[2].Value(); }
    public override void SetEffect()
    {
        //Effect
        for (int i = 0; i < townCtrl.researchPower.Length; i++)
        {
            townCtrl.researchPower[i].RegisterMultiplier(new MultiplierInfo(Town, Mul, EffectValue));
        }

        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 1, (x) => { townCtrl.maxResearchNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Researchable # + 1"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 2, (x) => { townCtrl.maxResearchNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Researchable # + 1"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 3, (x) => { townCtrl.maxResearchNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Researchable # + 1"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 4, (x) => { townCtrl.maxResearchNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Researchable # + 1"));
        passiveRankEffectList.Add(new BuildingPassiveEffect(rank, 5, (x) => { townCtrl.maxResearchNum.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)); }, "Researchable # + 1"));

        passiveEffectList.Add(new BuildingPassiveEffect(level, 5, (x) => RegisterUnlock(0, x), "Enable to convert Town Materials in Shop"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 15, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Converted Town Material Gain + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 25, (x) => RegisterUnlock(1, x), "Unlock new convertable Town Materials"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 35, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Converted Town Material Gain + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 45, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Converted Town Material Gain + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 55, (x) => RegisterUnlock(2, x), "Unlock new convertable Town Materials"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 65, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Converted Town Material Gain + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 75, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 1, x)), "Converted Town Material Gain + 1"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 85, (x) => RegisterUnlock(3, x), "Unlock new convertable Town Materials"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 95, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 2, x)), "Converted Town Material Gain + 2"));
        passiveEffectList.Add(new BuildingPassiveEffect(level, 100, (x) => game.shopCtrl.convertTownMaterialAmount.RegisterMultiplier(new MultiplierInfo(Town, Add, () => 2, x)), "Converted Town Material Gain + 2"));
    }
    void RegisterUnlock(int tier, Func<bool> condition)
    {
        switch (tier)
        {
            case 0:
                game.shopCtrl.TownMaterial(TownMaterialKind.MudBrick).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.PineLog).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.JasperShard).unlock.RegisterCondition(condition);
                break;
            case 1:
                game.shopCtrl.TownMaterial(TownMaterialKind.LimestoneBrick).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.MapleLog).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.OpalShard).unlock.RegisterCondition(condition);
                break;
            case 2:
                game.shopCtrl.TownMaterial(TownMaterialKind.MarbleBrick).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.AshLog).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.OnyxShard).unlock.RegisterCondition(condition);
                break;
            case 3:
                game.shopCtrl.TownMaterial(TownMaterialKind.GraniteBrick).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.MahoganyLog).unlock.RegisterCondition(condition);
                game.shopCtrl.TownMaterial(TownMaterialKind.JadeShard).unlock.RegisterCondition(condition);
                break;

        }
    }
    public override void SetResearch()
    {
        townCtrl.researchPower[(int)ResourceKind.Stone].RegisterMultiplier(new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Stone) * 0.05d));
        townCtrl.researchPower[(int)ResourceKind.Crystal].RegisterMultiplier(new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Crystal) * 0.05d));
        townCtrl.researchPower[(int)ResourceKind.Leaf].RegisterMultiplier(new MultiplierInfo(TownResearch, Mul, () => ResearchLevel(ResourceKind.Leaf) * 0.05d));
    }
    public override (string stone, string crystal, string leaf) ResearchEffectString(bool isShowIncrement)
    {
        if (isShowIncrement)
        {
            return (
                "Stone Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.05d) + "</color>" + " ( + " + percent(0.05) + " / Lv )",
                "Crystal Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>" + " ( + " + percent(0.05) + " / Lv )",
                "Leaf Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Leaf) * 0.05d) + "</color>" + " ( + " + percent(0.05) + " / Lv )"
                );
        }
        return (
            "Stone Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Stone) * 0.05d) + "</color>",
            "Crystal Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Crystal) * 0.05d) + "</color>",
            "Leaf Research Power <color=green>+ " + percent(ResearchLevel(ResourceKind.Leaf) * 0.05d) + "</color>"
            );
    }
}
public class Tavern : BUILDING
{
    public override BuildingKind kind => BuildingKind.Tavern;
    public override long unlockGuildLevel => 70;//65;
    public override string NameString()
    {
        return "Tavern";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankUpConditions[1].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 50);
        rankUpConditions[2].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 200);
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 4);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 500);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 500);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 2000);
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 20);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Ectoplasm), 5000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 5000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 20000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (level) => PrimaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (level) => SecondaryCost(level, 3));
    }
}
public class Dojo : BUILDING
{
    public override BuildingKind kind => BuildingKind.Dojo;
    public override long unlockGuildLevel => 65;//60;
    public override string NameString()
    {
        return "Dojo";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankUpConditions[1].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 50);
        rankUpConditions[2].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 200);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 50);
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 4);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 500);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 2000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 500);
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 20);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Ectoplasm), 5000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 20000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 5000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (level) => PrimaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (level) => SecondaryCost(level, 3));
    }

    //Level Milestone : Global Skill Slot +

}
public class AdventuringParty : BUILDING
{
    public override BuildingKind kind => BuildingKind.AdventuringParty;
    public override long unlockGuildLevel => 55;
    public override string NameString()
    {
        return "Adventuring Party";
    }
    public override void SetCost()
    {
        rankUpConditions[0].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(1, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankUpConditions[1].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 50);
        rankUpConditions[2].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(2, game.materialCtrl.Material(MaterialKind.Ectoplasm), 200);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 50);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 50);
        rankUpConditions[3].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.BlackPearl), 4);
        rankTransaction.AddCost(3, game.materialCtrl.Material(MaterialKind.Ectoplasm), 2000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 500);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 500);
        rankUpConditions[4].RegisterCondition(() => false, () => "Stay tuned for future updates!");
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.BlackPearl), 20);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Ectoplasm), 20000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.VoidEgg), 5000);
        rankTransaction.AddCost(4, game.materialCtrl.Material(MaterialKind.Stardust), 5000);

        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (level) => PrimaryCost(level, 3));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard), (level) => SecondaryCost(level, 1));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (level) => SecondaryCost(level, 2));
        levelTransaction.SetAnotherResource(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (level) => SecondaryCost(level, 3));
    }
}
//public class Fairgrounds : BUILDING
//{
//    public override BuildingKind kind => BuildingKind.Fairgrounds; public override string NameString()
//    {
//        return "Fairgrounds";
//    }

//}

public enum BuildingKind
{
    StatueOfHeroes,//MudBrick(1)
    Cartographer,//PineLog(1)
    AlchemistsHut,//JasperShard(1)
    Blacksmith,//LimestoneBrick(2)
    Temple,//MapleLog(2)
    Trapper,//OpalShard(2)
    SlimeBank,//MarbleBrick(3)
    MysticArena,//AshLog(3)
    ArcaneResearcher,//OnyxShard(3)
    Tavern,//JadeShard(4)
    Dojo,//MahoganyLog(4)
    AdventuringParty,//GraniteBrick(4)
    //Fairgrounds,
}
