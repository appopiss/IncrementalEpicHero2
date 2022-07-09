using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;

//public partial class SaveR
//{
//    //public double[] accomplishedTimes;//今回のWorldAscensionでのTime
//    //public double[] accomplishedBestTimes;
//}

//public class StatisticsController
//{

//}

public class ACCOMPLISH
{
    public virtual double accomplishedFirstTime { get; set; }//初めて
    public virtual double accomplishedTime { get; set; }
    public virtual double accomplishedBestTime { get; set; }
    public virtual void RegisterTime()
    {
        if (accomplishedTime <= 0) accomplishedTime = main.allTimeWorldAscension;
        else accomplishedTime = Math.Min(accomplishedTime, main.allTimeWorldAscension);
        if (accomplishedBestTime <= 0)//初めて
        {
            accomplishedBestTime = accomplishedTime;
            accomplishedFirstTime = accomplishedTime;
        }
        else accomplishedBestTime = Math.Min(accomplishedBestTime, accomplishedTime);
    }
}


//public enum AccomplishKind
//{
    //BuildingRank1HeroOfStatue,
    //BuildingRank2HeroOfStatue,
    //BuildingRank3HeroOfStatue,
    //BuildingRank4HeroOfStatue,
    //BuildingRank5HeroOfStatue,
    //BuildingRank1Cartographer,
    //BuildingRank2Cartographer,
    //BuildingRank3Cartographer,
    //BuildingRank4Cartographer,
    //BuildingRank5Cartographer,
    //BuildingRank1AlchemistHuts,
    //BuildingRank2AlchemistHuts,
    //BuildingRank3AlchemistHuts,
    //BuildingRank4AlchemistHuts,
    //BuildingRank5AlchemistHuts,
    //BuildingRank1Blacksmith,
    //BuildingRank2Blacksmith,
    //BuildingRank3Blacksmith,
    //BuildingRank4Blacksmith,
    //BuildingRank5Blacksmith,
    //BuildingRank1Temple,
    //BuildingRank2Temple,
    //BuildingRank3Temple,
    //BuildingRank4Temple,
    //BuildingRank5Temple,
    //BuildingRank1Trapper,
    //BuildingRank2Trapper,
    //BuildingRank3Trapper,
    //BuildingRank4Trapper,
    //BuildingRank5Trapper,
    //BuildingRank1SlimeBank,
    //BuildingRank2SlimeBank,
    //BuildingRank3SlimeBank,
    //BuildingRank4SlimeBank,
    //BuildingRank5SlimeBank,
    //BuildingRank1MysticArena,
    //BuildingRank2MysticArena,
    //BuildingRank3MysticArena,
    //BuildingRank4MysticArena,
    //BuildingRank5MysticArena,
    //BuildingRank1ArcaneResearcher,
    //BuildingRank2ArcaneResearcher,
    //BuildingRank3ArcaneResearcher,
    //BuildingRank4ArcaneResearcher,
    //BuildingRank5ArcaneResearcher,
    //BuildingRank1Tavern,
    //BuildingRank2Tavern,
    //BuildingRank3Tavern,
    //BuildingRank4Tavern,
    //BuildingRank5Tavern,
    //BuildingRank1Dojo,
    //BuildingRank2Dojo,
    //BuildingRank3Dojo,
    //BuildingRank4Dojo,
    //BuildingRank5Dojo,
    //BuildingRank1AdventuringParty,
    //BuildingRank2AdventuringParty,
    //BuildingRank3AdventuringParty,
    //BuildingRank4AdventuringParty,
    //BuildingRank5AdventuringParty,

    //DefeatRaidSlimeKingLv100,
    //DefeatRaidSpiderQueenLv150,
    //DefeatRaidGolemLv200,

//}
