using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static MultiplierKind;
using static MultiplierType;

public partial class SaveR
{
    public double[] townMaterials;
}

public class TownController
{
    public TownController()
    {
        maxRank = new Multiplier();
        for (int i = 0; i < townMaterials.Length; i++)
        {
            townMaterials[i] = new TownMaterial((TownMaterialKind)i);
        }
        for (int i = 0; i < townMaterialGainMultiplier.Length; i++)
        {
            townMaterialGainMultiplier[i] = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        }
        levelCostReduction = new Multiplier(() => 0.90d, () => 0);
        for (int i = 0; i < researchPower.Length; i++)
        {
            researchPower[i] = new Multiplier(() => 1000, () => 1);
        }
        maxResearchNum = new Multiplier(new MultiplierInfo(Base, Add, () => 1));

        townLevelEffectMultiplier = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        for (int i = 0; i < townLevelEffectMultipliers.Length; i++)
        {
            townLevelEffectMultipliers[i] = new Multiplier(new MultiplierInfo(Base, Add, () => 1));
        }
    }
    public void Start()
    {
        buildings[(int)BuildingKind.StatueOfHeroes] = new StatueOfHeroes();
        buildings[(int)BuildingKind.Blacksmith] = new Blacksmith();
        buildings[(int)BuildingKind.Cartographer] = new Cartographer();
        buildings[(int)BuildingKind.Tavern] = new Tavern();
        buildings[(int)BuildingKind.Temple] = new Temple();
        buildings[(int)BuildingKind.Dojo] = new Dojo();
        buildings[(int)BuildingKind.Trapper] = new Trapper();
        buildings[(int)BuildingKind.MysticArena] = new MysticArena();
        buildings[(int)BuildingKind.SlimeBank] = new SlimeBank();
        buildings[(int)BuildingKind.ArcaneResearcher] = new ArcaneResearcher();
        buildings[(int)BuildingKind.AlchemistsHut] = new AlchemistsHut();
        buildings[(int)BuildingKind.AdventuringParty] = new AdventuringParty();
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].Start();
        }
        for (int i = 0; i < researchPower.Length; i++)
        {
            int count = i;
            researchPower[i].RegisterMultiplier(new MultiplierInfo(Base, Add, () => Math.Log10(10 + game.resourceCtrl.Resource((ResourceKind)count).value)));
        }
    }
    public Multiplier maxRank;
    public TownMaterial[] townMaterials = new TownMaterial[Enum.GetNames(typeof(TownMaterialKind)).Length];
    public TownMaterial TownMaterial(TownMaterialKind kind)
    {
        return townMaterials[(int)kind];
    }
    public Multiplier[] townMaterialGainMultiplier = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public void SetEffectTownMatGain(MultiplierInfo info)
    {
        for (int i = 0; i < townMaterialGainMultiplier.Length; i++)
        {
            townMaterialGainMultiplier[i].RegisterMultiplier(info);
        }
    }
    public double GlobalTownMaterialGainMultiplier()//平均をとる
    {
        double tempValue = 0;
        for (int i = 0; i < townMaterialGainMultiplier.Length; i++)
        {
            tempValue += townMaterialGainMultiplier[i].Value();
        }
        return tempValue / townMaterialGainMultiplier.Length;
    }
    public Multiplier townLevelEffectMultiplier;
    public Multiplier[] townLevelEffectMultipliers = new Multiplier[3];//[Brick, Log, Shard]
    public Multiplier levelCostReduction;
    public BUILDING[] buildings = new BUILDING[Enum.GetNames(typeof(BuildingKind)).Length];
    public BUILDING Building(BuildingKind kind)
    {
        return buildings[(int)kind];
    }
    //Research
    public int CurrentResearchingNum()
    {
        int tempNum = 0;
        for (int i = 0; i < buildings.Length; i++)
        {
            tempNum += buildings[i].CurrentResearchingNum();
        }
        return tempNum;
    }
    public Multiplier[] researchPower = new Multiplier[Enum.GetNames(typeof(ResourceKind)).Length];
    public Multiplier maxResearchNum;
    public bool CanResearch()
    {
        return CurrentResearchingNum() < maxResearchNum.Value();
    }
    public void Update()
    {
        ProgressResaerch(Time.deltaTime);
        count++;
        if (count > 300)
        {
            AutoLevelUp();
            count = 0;
        }
    }
    int count;
    public void ProgressResaerch(double deltaTime)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].Update(deltaTime);
        }
    }

    public long TotalBuildingLevel()
    {
        long tempLevel = 0;
        for (int i = 0; i < buildings.Length; i++)
        {
            tempLevel += buildings[i].level.value;
        }
        return tempLevel;
    }

    public void AutoLevelUp()
    {
        if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.TownLevelUp)) return;
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].levelTransaction.Buy(true);
        }
    }

}

public class TownMaterial : NUMBER
{
    public TownMaterialKind kind;
    public TownMaterial(TownMaterialKind kind)
    {
        this.kind = kind;
    }
    public override void Increase(double increment)
    {
        //AreaInfoに表示するため、AreaのRewardに書くことにした
        //Disassembleで得られる値はtownMaterialGainMultiplierの効果を受けない
        //increment *= game.townCtrl.townMaterialGainMultiplier.Value();
        base.Increase(increment);
    }
    public override double value { get => main.SR.townMaterials[(int)kind]; set => main.SR.townMaterials[(int)kind] = value; }
    public override string Name()
    {
        switch (kind)
        {
            case TownMaterialKind.MudBrick:
                return "Mud Brick";
            case TownMaterialKind.LimestoneBrick:
                return "Limestone Brick";
            case TownMaterialKind.MarbleBrick:
                return "Marble Brick";
            case TownMaterialKind.GraniteBrick:
                return "Granite Brick";
            case TownMaterialKind.BasaltBrick:
                return "Basalt Brick";
            case TownMaterialKind.PineLog:
                return "Pine Log";
            case TownMaterialKind.MapleLog:
                return "Maple Log";
            case TownMaterialKind.AshLog:
                return "Ash Log";
            case TownMaterialKind.MahoganyLog:
                return "Mahogany Log";
            case TownMaterialKind.RosewoodLog:
                return "Rosewood Log";
            case TownMaterialKind.JasperShard:
                return "Jasper Shard";
            case TownMaterialKind.OpalShard:
                return "Opal Shard";
            case TownMaterialKind.OnyxShard:
                return "Onyx Shard";
            case TownMaterialKind.JadeShard:
                return "Jade Shard";
            case TownMaterialKind.SapphireShard:
                return "Sapphire Shard";
        }
        return kind.ToString();
    }
}

public enum TownMaterialKind
{
    MudBrick,//Statue1
    LimestoneBrick,//Blacksmith1
    MarbleBrick,//Slime Bank1
    GraniteBrick,//Tavern1
    BasaltBrick,
    PineLog,//Cartographer1
    MapleLog,//Temple1
    AshLog,//Mystic Arena1
    MahoganyLog,//Dojo1
    RosewoodLog,
    JasperShard,//Alchemist1
    OpalShard,//Trapper1
    OnyxShard,//ArcaneResearcher1
    JadeShard,//Adventureing Party
    SapphireShard,
}

