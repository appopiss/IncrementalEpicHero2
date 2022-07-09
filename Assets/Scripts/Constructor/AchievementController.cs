using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using static GameController;
using static Main;
using static MultiplierKind;
using static MultiplierType;
using static AchievementKind;
using static UsefulMethod;
using static Localized;

public partial class Save
{
    public bool[] isGotAchievements;//[AchievementKind]
    public bool[] isGotAchievementRewards;
    public double[] achievementPlaytimes;
}

public class AchievementController 
{
    public AchievementController()
    {
    }

    void SetAchievement()
    {
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.ClearTutorial, AchievementRewardKind.EpicCoin, 5000, "Complete all the tutorial quests", () => game.questCtrl.Quest(QuestKindGlobal.AreaPrestige).isCleared));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.UnlockWizard, AchievementRewardKind.Nitro, 1000, "Unlock Wizard", () => game.guildCtrl.Member(HeroKind.Wizard).IsUnlocked()));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.UnlockAngel, AchievementRewardKind.Nitro, 2000, "Unlock Angel", () => game.guildCtrl.Member(HeroKind.Angel).IsUnlocked()));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.UnlockThief, AchievementRewardKind.Nitro, 3000, "Unlock Thief", () => game.guildCtrl.Member(HeroKind.Thief).IsUnlocked()));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.UnlockArcher, AchievementRewardKind.Nitro, 4000, "Unlock Archer", () => game.guildCtrl.Member(HeroKind.Archer).IsUnlocked()));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.UnlockTamer, AchievementRewardKind.Nitro, 5000, "Unlock Tamer", () => game.guildCtrl.Member(HeroKind.Tamer).IsUnlocked()));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro1000, AchievementRewardKind.GoldCap, 0.01d, "Consume " + tDigit(1000) + " nitro in total", () => main.S.nitroConsumed, 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro10000, AchievementRewardKind.GoldCap, 0.02d, "Consume " + tDigit(10000) + " nitro in total", () => main.S.nitroConsumed, 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro100000, AchievementRewardKind.GoldCap, 0.03d, "Consume " + tDigit(100000) + " nitro in total", () => main.S.nitroConsumed, 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro1000000, AchievementRewardKind.GoldCap, 0.04d, "Consume " + tDigit(1000000) + " nitro in total", () => main.S.nitroConsumed, 1000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro10000000, AchievementRewardKind.GoldCap, 0.05d, "Consume " + tDigit(10000000) + " nitro in total", () => main.S.nitroConsumed, 10000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Nitro100000000, AchievementRewardKind.GoldCap, 0.06d, "Consume " + tDigit(100000000) + " nitro in total", () => main.S.nitroConsumed, 100000000));

        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Capture100, AchievementRewardKind.GoldCap, 0.01d, "Capture " + tDigit(100) + " monsters in total", () => game.monsterCtrl.CapturedNum(), 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Capture1000, AchievementRewardKind.GoldCap, 0.02d, "Capture " + tDigit(1000) + " monsters in total", () => game.monsterCtrl.CapturedNum(), 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Capture10000, AchievementRewardKind.GoldCap, 0.03d, "Capture " + tDigit(10000) + " monsters in total", () => game.monsterCtrl.CapturedNum(), 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Capture100000, AchievementRewardKind.GoldCap, 0.04d, "Capture " + tDigit(100000) + " monsters in total", () => game.monsterCtrl.CapturedNum(), 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Capture1000000, AchievementRewardKind.GoldCap, 0.05d, "Capture " + tDigit(1000000) + " monsters in total", () => game.monsterCtrl.CapturedNum(), 1000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest1, AchievementRewardKind.EpicCoin, 500, "Open " + tDigit(1) + " Treasure Chest", () => main.S.openedChestNum, 1));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest10, AchievementRewardKind.EpicCoin, 1000, "Open " + tDigit(10) + " Treasure Chests", () => main.S.openedChestNum, 10));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest100, AchievementRewardKind.EpicCoin, 1500, "Open " + tDigit(100) + " Treasure Chests", () => main.S.openedChestNum, 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest1000, AchievementRewardKind.EpicCoin, 2000, "Open " + tDigit(1000) + " Treasure Chests", () => main.S.openedChestNum, 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest10000, AchievementRewardKind.EpicCoin, 2500, "Open " + tDigit(10000) + " Treasure Chests", () => main.S.openedChestNum, 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest100000, AchievementRewardKind.EpicCoin, 5000, "Open " + tDigit(100000) + " Treasure Chests", () => main.S.openedChestNum, 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Chest1000000, AchievementRewardKind.EpicCoin, 10000, "Open " + tDigit(1000000) + " Treasure Chests", () => main.S.openedChestNum, 1000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic1, AchievementRewardKind.ExpGain, 0.01d, "Defeat " + tDigit(1) + " Mimic", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 1));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic10, AchievementRewardKind.ExpGain, 0.02d, "Defeat " + tDigit(10) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 10));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic100, AchievementRewardKind.ExpGain, 0.04d, "Defeat " + tDigit(100) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic1000, AchievementRewardKind.ExpGain, 0.08d, "Mimic " + tDigit(1000) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic10000, AchievementRewardKind.ExpGain, 0.16d, "Mimic " + tDigit(10000) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic100000, AchievementRewardKind.ExpGain, 0.32d, "Mimic " + tDigit(100000) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Mimic1000000, AchievementRewardKind.ExpGain, 0.64d, "Mimic " + tDigit(1000000) + " Mimics", () => game.monsterCtrl.GlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal).DefeatedNum(), 1000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm1, AchievementRewardKind.ResourceGain, 0.10d, "Vanquish " + tDigit(1) + " swarm", () => main.S.swarmClearedNum, 1));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm10, AchievementRewardKind.ResourceGain, 0.20d, "Vanquish " + tDigit(10) + " swarms", () => main.S.swarmClearedNum, 10));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm50, AchievementRewardKind.ResourceGain, 0.30d, "Vanquish " + tDigit(50) + " swarms", () => main.S.swarmClearedNum, 50));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm100, AchievementRewardKind.ResourceGain, 0.40d, "Vanquish " + tDigit(100) + " swarms", () => main.S.swarmClearedNum, 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm500, AchievementRewardKind.ResourceGain, 0.50d, "Vanquish " + tDigit(500) + " swarms", () => main.S.swarmClearedNum, 500));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm1000, AchievementRewardKind.ResourceGain, 0.60d, "Vanquish " + tDigit(1000) + " swarms", () => main.S.swarmClearedNum, 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm5000, AchievementRewardKind.ResourceGain, 0.70d, "Vanquish " + tDigit(5000) + " swarms", () => main.S.swarmClearedNum, 5000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm10000, AchievementRewardKind.ResourceGain, 0.80d, "Vanquish " + tDigit(10000) + " swarms", () => main.S.swarmClearedNum, 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm100000, AchievementRewardKind.ResourceGain, 0.90d, "Vanquish " + tDigit(100000) + " swarms", () => main.S.swarmClearedNum, 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Swarm1000000, AchievementRewardKind.ResourceGain, 1.00d, "Vanquish " + tDigit(1000000) + " swarms", () => main.S.swarmClearedNum, 1000000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk40075km, AchievementRewardKind.GoldCap, 0.01d, "Walk around the earth [40,075,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 40075000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk40075km2, AchievementRewardKind.GoldCap, 0.02d, "Walk around the earth twice [80,150,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 2 * 40075000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk40075km3, AchievementRewardKind.GoldCap, 0.03d, "Walk around the earth 3 times [120,225,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 3 * 40075000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk40075km5, AchievementRewardKind.GoldCap, 0.04d, "Walk around the earth 5 times [200,375,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 5 * 40075000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk384400km, AchievementRewardKind.GoldCap, 0.05d, "Walk to the moon [384,400,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 384400000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk384400km2, AchievementRewardKind.GoldCap, 0.07d, "Walk to the moon and back [786,800,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 2 * 384400000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.Walk149600000km, AchievementRewardKind.GoldCap, 0.10d, "Walk to the sun [149,600,000,000 meters]", () => game.statsCtrl.TotalMovedDistance() / 100, 149600000000 * 100d / 100));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.EpicCoin1000, AchievementRewardKind.GoldCap, 0.01d, "Spend 1000 Epic Coin", () => main.S.epicCoinConsumed, 1000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.EpicCoin10000, AchievementRewardKind.GoldCap, 0.02d, "Spend 10000 Epic Coin", () => main.S.epicCoinConsumed, 10000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.EpicCoin100000, AchievementRewardKind.GoldCap, 0.03d, "Spend 100000 Epic Coin", () => main.S.epicCoinConsumed, 100000));
        achievementListGeneral.Add(new GeneralAchievement(AchievementKind.EpicCoin1000000, AchievementRewardKind.GoldCap, 0.04d, "Spend 1000000 Epic Coin", () => main.S.epicCoinConsumed, 1000000));
        achievementList.AddRange(achievementListGeneral);

        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockMagicslime, AchievementRewardKind.EpicCoin, 500, "Unlock " + localized.AreaName(AreaKind.MagicSlimeCity), () => game.areaCtrl.IsUnlocked(AreaKind.MagicSlimeCity)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockSpider, AchievementRewardKind.EpicCoin, 1000, "Unlock " + localized.AreaName(AreaKind.SpiderMaze), () => game.areaCtrl.IsUnlocked(AreaKind.SpiderMaze)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockBat, AchievementRewardKind.EpicCoin, 1500, "Unlock " + localized.AreaName(AreaKind.BatCave), () => game.areaCtrl.IsUnlocked(AreaKind.BatCave)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockFairy, AchievementRewardKind.EpicCoin, 2000, "Unlock " + localized.AreaName(AreaKind.FairyGarden), () => game.areaCtrl.IsUnlocked(AreaKind.FairyGarden)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockFox, AchievementRewardKind.EpicCoin, 2500, "Unlock " + localized.AreaName(AreaKind.FoxShrine), () => game.areaCtrl.IsUnlocked(AreaKind.FoxShrine)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockDevilfish, AchievementRewardKind.EpicCoin, 3000, "Unlock " + localized.AreaName(AreaKind.DevilFishLake), () => game.areaCtrl.IsUnlocked(AreaKind.DevilFishLake)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockTreant, AchievementRewardKind.EpicCoin, 3500, "Unlock " + localized.AreaName(AreaKind.TreantDarkForest), () => game.areaCtrl.IsUnlocked(AreaKind.TreantDarkForest)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockFlametiger, AchievementRewardKind.EpicCoin, 4000, "Unlock " + localized.AreaName(AreaKind.FlameTigerVolcano), () => game.areaCtrl.IsUnlocked(AreaKind.FlameTigerVolcano)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.UnlockUnicorn, AchievementRewardKind.EpicCoin, 4500, "Unlock " + localized.AreaName(AreaKind.UnicornIsland), () => game.areaCtrl.IsUnlocked(AreaKind.UnicornIsland)));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueSlime, AchievementRewardKind.EQInventorySlot, 1, "Gain all Slime Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Slime), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueMagicslime, AchievementRewardKind.EQInventorySlot, 1, "Gain all Magicslime Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Magicslime), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueSpider, AchievementRewardKind.EQInventorySlot, 1, "Gain all Spider Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Spider), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueBat, AchievementRewardKind.EQInventorySlot, 1, "Gain all Bat Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Bat), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueFairy, AchievementRewardKind.EQInventorySlot, 1, "Gain all Fairy Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Fairy), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueFox, AchievementRewardKind.EQInventorySlot, 1, "Gain all Fox Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Fox), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueDevilfish, AchievementRewardKind.EQInventorySlot, 1, "Gain all Devilfish Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Devilfish), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueTreant, AchievementRewardKind.EQInventorySlot, 1, "Gain all Treant Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Treant), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueFlametiger, AchievementRewardKind.EQInventorySlot, 1, "Gain all Flametiger Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Flametiger), 8));
        achievementListArea.Add(new GeneralAchievement(AchievementKind.EquipUniqueUnicorn, AchievementRewardKind.EQInventorySlot, 1, "Gain all Unicorn Set Unique Equipment", () => game.equipmentCtrl.UniqueSetIsGotNum(EquipmentSetKind.Unicorn), 8));
        achievementList.AddRange(achievementListArea);

        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e6, AchievementRewardKind.GoldCap, 0.0100d, "Gain 1 million Gold in total", () => main.S.totalGold, 1e6, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e9, AchievementRewardKind.GoldCap, 0.0150d, "Gain 1 billion Gold in total", () => main.S.totalGold, 1e9, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e10, AchievementRewardKind.GoldCap, 0.0200d, "Gain 1e10 Gold in total", () => main.S.totalGold, 1e10, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e11, AchievementRewardKind.GoldCap, 0.0250d, "Gain 1e11 Gold in total", () => main.S.totalGold, 1e11, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e12, AchievementRewardKind.GoldCap, 0.0300d, "Gain 1e12 Gold in total", () => main.S.totalGold, 1e12, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e13, AchievementRewardKind.GoldCap, 0.0350d, "Gain 1e13 Gold in total", () => main.S.totalGold, 1e13, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e14, AchievementRewardKind.GoldCap, 0.0400d, "Gain 1e14 Gold in total", () => main.S.totalGold, 1e14, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e15, AchievementRewardKind.GoldCap, 0.0450d, "Gain 1e15 Gold in total", () => main.S.totalGold, 1e15, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e16, AchievementRewardKind.GoldCap, 0.0500d, "Gain 1e16 Gold in total", () => main.S.totalGold, 1e16, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e17, AchievementRewardKind.GoldCap, 0.0550d, "Gain 1e17 Gold in total", () => main.S.totalGold, 1e17, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e18, AchievementRewardKind.GoldCap, 0.0600d, "Gain 1e18 Gold in total", () => main.S.totalGold, 1e18, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e19, AchievementRewardKind.GoldCap, 0.0650d, "Gain 1e19 Gold in total", () => main.S.totalGold, 1e19, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e20, AchievementRewardKind.GoldCap, 0.0700d, "Gain 1e20 Gold in total", () => main.S.totalGold, 1e20, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e22, AchievementRewardKind.GoldCap, 0.0750d, "Gain 1e22 Gold in total", () => main.S.totalGold, 1e22, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e24, AchievementRewardKind.GoldCap, 0.0800d, "Gain 1e24 Gold in total", () => main.S.totalGold, 1e24, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e26, AchievementRewardKind.GoldCap, 0.0850d, "Gain 1e26 Gold in total", () => main.S.totalGold, 1e26, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e28, AchievementRewardKind.GoldCap, 0.0900d, "Gain 1e28 Gold in total", () => main.S.totalGold, 1e28, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Gold1e30, AchievementRewardKind.GoldCap, 0.0950d, "Gain 1e30 Gold in total", () => main.S.totalGold, 1e30, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e10, AchievementRewardKind.ResourceGain, 0.10d, "Gain 1e10 Stone in total", () => main.S.totalStone, 1e10, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e20, AchievementRewardKind.ResourceGain, 0.20d, "Gain 1e20 Stone in total", () => main.S.totalStone, 1e20, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e30, AchievementRewardKind.ResourceGain, 0.30d, "Gain 1e30 Stone in total", () => main.S.totalStone, 1e30, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e40, AchievementRewardKind.ResourceGain, 0.40d, "Gain 1e40 Stone in total", () => main.S.totalStone, 1e40, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e50, AchievementRewardKind.ResourceGain, 0.50d, "Gain 1e50 Stone in total", () => main.S.totalStone, 1e50, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e60, AchievementRewardKind.ResourceGain, 0.60d, "Gain 1e60 Stone in total", () => main.S.totalStone, 1e60, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e70, AchievementRewardKind.ResourceGain, 0.70d, "Gain 1e70 Stone in total", () => main.S.totalStone, 1e70, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e80, AchievementRewardKind.ResourceGain, 0.80d, "Gain 1e80 Stone in total", () => main.S.totalStone, 1e80, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e90, AchievementRewardKind.ResourceGain, 0.90d, "Gain 1e90 Stone in total", () => main.S.totalStone, 1e90, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e100, AchievementRewardKind.ResourceGain, 1.00d, "Gain 1e100 Stone in total", () => main.S.totalStone, 1e100, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e110, AchievementRewardKind.ResourceGain, 1.10d, "Gain 1e110 Stone in total", () => main.S.totalStone, 1e110, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e120, AchievementRewardKind.ResourceGain, 1.20d, "Gain 1e120 Stone in total", () => main.S.totalStone, 1e120, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e130, AchievementRewardKind.ResourceGain, 1.30d, "Gain 1e130 Stone in total", () => main.S.totalStone, 1e130, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e140, AchievementRewardKind.ResourceGain, 1.40d, "Gain 1e140 Stone in total", () => main.S.totalStone, 1e140, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e150, AchievementRewardKind.ResourceGain, 1.50d, "Gain 1e150 Stone in total", () => main.S.totalStone, 1e150, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e160, AchievementRewardKind.ResourceGain, 1.60d, "Gain 1e160 Stone in total", () => main.S.totalStone, 1e160, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e170, AchievementRewardKind.ResourceGain, 1.70d, "Gain 1e170 Stone in total", () => main.S.totalStone, 1e170, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e180, AchievementRewardKind.ResourceGain, 1.80d, "Gain 1e180 Stone in total", () => main.S.totalStone, 1e180, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e190, AchievementRewardKind.ResourceGain, 1.90d, "Gain 1e190 Stone in total", () => main.S.totalStone, 1e190, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Stone1e200, AchievementRewardKind.ResourceGain, 2.00d, "Gain 1e200 Stone in total", () => main.S.totalStone, 1e200, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e10, AchievementRewardKind.ResourceGain, 0.10d, "Gain 1e10 Crystal in total", () => main.S.totalCrystal, 1e10, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e20, AchievementRewardKind.ResourceGain, 0.20d, "Gain 1e20 Crystal in total", () => main.S.totalCrystal, 1e20, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e30, AchievementRewardKind.ResourceGain, 0.30d, "Gain 1e30 Crystal in total", () => main.S.totalCrystal, 1e30, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e40, AchievementRewardKind.ResourceGain, 0.40d, "Gain 1e40 Crystal in total", () => main.S.totalCrystal, 1e40, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e50, AchievementRewardKind.ResourceGain, 0.50d, "Gain 1e50 Crystal in total", () => main.S.totalCrystal, 1e50, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e60, AchievementRewardKind.ResourceGain, 0.60d, "Gain 1e60 Crystal in total", () => main.S.totalCrystal, 1e60, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e70, AchievementRewardKind.ResourceGain, 0.70d, "Gain 1e70 Crystal in total", () => main.S.totalCrystal, 1e70, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e80, AchievementRewardKind.ResourceGain, 0.80d, "Gain 1e80 Crystal in total", () => main.S.totalCrystal, 1e80, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e90, AchievementRewardKind.ResourceGain, 0.90d, "Gain 1e90 Crystal in total", () => main.S.totalCrystal, 1e90, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e100, AchievementRewardKind.ResourceGain, 1.00d, "Gain 1e100 Crystal in total", () => main.S.totalCrystal, 1e100, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e110, AchievementRewardKind.ResourceGain, 1.10d, "Gain 1e110 Crystal in total", () => main.S.totalCrystal, 1e110, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e120, AchievementRewardKind.ResourceGain, 1.20d, "Gain 1e120 Crystal in total", () => main.S.totalCrystal, 1e120, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e130, AchievementRewardKind.ResourceGain, 1.30d, "Gain 1e130 Crystal in total", () => main.S.totalCrystal, 1e130, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e140, AchievementRewardKind.ResourceGain, 1.40d, "Gain 1e140 Crystal in total", () => main.S.totalCrystal, 1e140, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e150, AchievementRewardKind.ResourceGain, 1.50d, "Gain 1e150 Crystal in total", () => main.S.totalCrystal, 1e150, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e160, AchievementRewardKind.ResourceGain, 1.60d, "Gain 1e160 Crystal in total", () => main.S.totalCrystal, 1e160, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e170, AchievementRewardKind.ResourceGain, 1.70d, "Gain 1e170 Crystal in total", () => main.S.totalCrystal, 1e170, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e180, AchievementRewardKind.ResourceGain, 1.80d, "Gain 1e180 Crystal in total", () => main.S.totalCrystal, 1e180, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e190, AchievementRewardKind.ResourceGain, 1.90d, "Gain 1e190 Crystal in total", () => main.S.totalCrystal, 1e190, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Crystal1e200, AchievementRewardKind.ResourceGain, 2.00d, "Gain 1e200 Crystal in total", () => main.S.totalCrystal, 1e200, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e10, AchievementRewardKind.ResourceGain, 0.10d, "Gain 1e10 Leaf in total", () => main.S.totalLeaf, 1e10, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e20, AchievementRewardKind.ResourceGain, 0.20d, "Gain 1e20 Leaf in total", () => main.S.totalLeaf, 1e20, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e30, AchievementRewardKind.ResourceGain, 0.30d, "Gain 1e30 Leaf in total", () => main.S.totalLeaf, 1e30, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e40, AchievementRewardKind.ResourceGain, 0.40d, "Gain 1e40 Leaf in total", () => main.S.totalLeaf, 1e40, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e50, AchievementRewardKind.ResourceGain, 0.50d, "Gain 1e50 Leaf in total", () => main.S.totalLeaf, 1e50, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e60, AchievementRewardKind.ResourceGain, 0.60d, "Gain 1e60 Leaf in total", () => main.S.totalLeaf, 1e60, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e70, AchievementRewardKind.ResourceGain, 0.70d, "Gain 1e70 Leaf in total", () => main.S.totalLeaf, 1e70, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e80, AchievementRewardKind.ResourceGain, 0.80d, "Gain 1e80 Leaf in total", () => main.S.totalLeaf, 1e80, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e90, AchievementRewardKind.ResourceGain, 0.90d, "Gain 1e90 Leaf in total", () => main.S.totalLeaf, 1e90, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e100, AchievementRewardKind.ResourceGain, 1.00d, "Gain 1e100 Leaf in total", () => main.S.totalLeaf, 1e100, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e110, AchievementRewardKind.ResourceGain, 1.10d, "Gain 1e110 Leaf in total", () => main.S.totalLeaf, 1e110, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e120, AchievementRewardKind.ResourceGain, 1.20d, "Gain 1e120 Leaf in total", () => main.S.totalLeaf, 1e120, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e130, AchievementRewardKind.ResourceGain, 1.30d, "Gain 1e130 Leaf in total", () => main.S.totalLeaf, 1e130, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e140, AchievementRewardKind.ResourceGain, 1.40d, "Gain 1e140 Leaf in total", () => main.S.totalLeaf, 1e140, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e150, AchievementRewardKind.ResourceGain, 1.50d, "Gain 1e150 Leaf in total", () => main.S.totalLeaf, 1e150, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e160, AchievementRewardKind.ResourceGain, 1.60d, "Gain 1e160 Leaf in total", () => main.S.totalLeaf, 1e160, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e170, AchievementRewardKind.ResourceGain, 1.70d, "Gain 1e170 Leaf in total", () => main.S.totalLeaf, 1e170, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e180, AchievementRewardKind.ResourceGain, 1.80d, "Gain 1e180 Leaf in total", () => main.S.totalLeaf, 1e180, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e190, AchievementRewardKind.ResourceGain, 1.90d, "Gain 1e190 Leaf in total", () => main.S.totalLeaf, 1e190, true));
        achievementListCurrency.Add(new GeneralAchievement(AchievementKind.Leaf1e200, AchievementRewardKind.ResourceGain, 2.00d, "Gain 1e200 Leaf in total", () => main.S.totalLeaf, 1e200, true));
        achievementList.AddRange(achievementListCurrency);

        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv20, AchievementRewardKind.PortalOrb, 5, "Reach Guild Level 20", () => game.guildCtrl.MaxLevelReached(), 20));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv40, AchievementRewardKind.PortalOrb, 10, "Reach Guild Level 40", () => game.guildCtrl.MaxLevelReached(), 40));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv60, AchievementRewardKind.PortalOrb, 15, "Reach Guild Level 60", () => game.guildCtrl.MaxLevelReached(), 60));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv80, AchievementRewardKind.PortalOrb, 20, "Reach Guild Level 80", () => game.guildCtrl.MaxLevelReached(), 80));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv100, AchievementRewardKind.PortalOrb, 25, "Reach Guild Level 100", () => game.guildCtrl.MaxLevelReached(), 100));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv120, AchievementRewardKind.PortalOrb, 30, "Reach Guild Level 120", () => game.guildCtrl.MaxLevelReached(), 120));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv140, AchievementRewardKind.PortalOrb, 35, "Reach Guild Level 140", () => game.guildCtrl.MaxLevelReached(), 140));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv160, AchievementRewardKind.PortalOrb, 40, "Reach Guild Level 160", () => game.guildCtrl.MaxLevelReached(), 160));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv180, AchievementRewardKind.PortalOrb, 45, "Reach Guild Level 180", () => game.guildCtrl.MaxLevelReached(), 180));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv200, AchievementRewardKind.PortalOrb, 50, "Reach Guild Level 200", () => game.guildCtrl.MaxLevelReached(), 200));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv220, AchievementRewardKind.PortalOrb, 60, "Reach Guild Level 220", () => game.guildCtrl.MaxLevelReached(), 220));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv240, AchievementRewardKind.PortalOrb, 70, "Reach Guild Level 240", () => game.guildCtrl.MaxLevelReached(), 240));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv260, AchievementRewardKind.PortalOrb, 80, "Reach Guild Level 260", () => game.guildCtrl.MaxLevelReached(), 260));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv280, AchievementRewardKind.PortalOrb, 90, "Reach Guild Level 280", () => game.guildCtrl.MaxLevelReached(), 280));
        achievementListGuild.Add(new GeneralAchievement(AchievementKind.GLv300, AchievementRewardKind.PortalOrb, 100, "Reach Guild Level 300", () => game.guildCtrl.MaxLevelReached(), 300));
        achievementList.AddRange(achievementListGuild);

        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Florzporb, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Florzporb Lv 100]", () => game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).IsClearedOnce()));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Arachnetta, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Arachnetta Lv 150]", () => game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).IsClearedOnce()));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.GurdianKor, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Gurdian Kor Lv 200]", () => game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).IsClearedOnce()));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Nostro, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Nostro Lv 250]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.LadyEmelda, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Lady Emelda Lv 300]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.NariSune, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Nari Sune Lv 350]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Octobaddie, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Octobaddie Lv 400]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Bananoon, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Bananoon Lv 450]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Glorbliorbus, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Glorbliorbus Lv 500]", () => false));
        achievementListChallenge.Add(new GeneralAchievement(AchievementKind.Gankyu, AchievementRewardKind.EQInventorySlot, 1, "Clear Raid Boss Battle [Gankyu Lv 550]", () => false));
        achievementList.AddRange(achievementListChallenge);

        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv50, AchievementRewardKind.GoldCap, 0.01d, "Reach total Potion level 50", () => game.potionCtrl.TotalPotionLevel(), 50));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv250, AchievementRewardKind.GoldCap, 0.02d, "Reach total Potion level 250", () => game.potionCtrl.TotalPotionLevel(), 250));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv600, AchievementRewardKind.GoldCap, 0.03d, "Reach total Potion level 600", () => game.potionCtrl.TotalPotionLevel(), 600));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv1250, AchievementRewardKind.GoldCap, 0.04d, "Reach total Potion level 1250", () => game.potionCtrl.TotalPotionLevel(), 1250));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv2500, AchievementRewardKind.GoldCap, 0.05d, "Reach total Potion level 2500", () => game.potionCtrl.TotalPotionLevel(), 2500));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.PotionLv3000, AchievementRewardKind.GoldCap, 0.06d, "Reach total Potion level 3000", () => game.potionCtrl.TotalPotionLevel(), 3000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint100000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 100K Alchemy Points in total", () => main.S.totalAlchemyPointGained, 100000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint1000000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 1M Alchemy Points in total", () => main.S.totalAlchemyPointGained, 1000000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint10000000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 10M Alchemy Points in total", () => main.S.totalAlchemyPointGained, 10000000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint100000000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 100M Alchemy Points in total", () => main.S.totalAlchemyPointGained, 100000000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint1000000000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 1B Alchemy Points in total", () => main.S.totalAlchemyPointGained, 1000000000));
        achievementListAlchemy.Add(new GeneralAchievement(AchievementKind.AlchemyPoint10000000000, AchievementRewardKind.UtilityInventorySlot, 1, "Gain 10B Alchemy Points in total", () => main.S.totalAlchemyPointGained, 10000000000));
        achievementList.AddRange(achievementListAlchemy);

        achievementListEquip.Add(new GeneralAchievement(AchievementKind.EquipGain1000, AchievementRewardKind.EQInventorySlot, 1, "Gain 1000 Equipment in total", () => main.S.totalEquipmentGained, 1000));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.EquipGain10000, AchievementRewardKind.EQInventorySlot, 1, "Gain 10000 Equipment in total", () => main.S.totalEquipmentGained, 10000));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.EquipGain100000, AchievementRewardKind.EQInventorySlot, 1, "Gain 100000 Equipment in total", () => main.S.totalEquipmentGained, 100000));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.EquipGain1000000, AchievementRewardKind.EQInventorySlot, 1, "Gain 1000000 Equipment in total", () => main.S.totalEquipmentGained, 1000000));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.EquipGain10000000, AchievementRewardKind.EQInventorySlot, 1, "Gain 10000000 Equipment in total", () => main.S.totalEquipmentGained, 10000000));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Warrior", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Warrior].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Wizard", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Wizard].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Angel", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Angel].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Thief", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Thief].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Archer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Archer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon8Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Weapon Equipment Slot of Tamer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Tamer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Warrior", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Warrior].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Wizard", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Wizard].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Angel", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Angel].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Thief", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Thief].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Archer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Archer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon16Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Weapon Equipment Slot of Tamer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Tamer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Warrior", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Warrior].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Wizard", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Wizard].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Angel", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Angel].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Thief", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Thief].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Archer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Archer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Weapon24Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Weapon Equipment Slot of Tamer", () => game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Tamer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Warrior", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Warrior].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Wizard", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Wizard].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Angel", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Angel].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Thief", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Thief].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Archer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Archer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor8Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Armor Equipment Slot of Tamer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Tamer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Warrior", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Warrior].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Wizard", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Wizard].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Angel", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Angel].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Thief", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Thief].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Archer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Archer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor16Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Armor Equipment Slot of Tamer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Tamer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Warrior", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Warrior].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Wizard", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Wizard].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Angel", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Angel].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Thief", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Thief].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Archer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Archer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Armor24Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Armor Equipment Slot of Tamer", () => game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Tamer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Warrior", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Warrior].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Wizard", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Wizard].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Angel", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Angel].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Thief", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Thief].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Archer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Archer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry8Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 8 Jewelry Equipment Slot of Tamer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Tamer].Value(), 8));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Warrior", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Warrior].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Wizard", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Wizard].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Angel", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Angel].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Thief", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Thief].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Archer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Archer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry16Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 16 Jewelry Equipment Slot of Tamer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Tamer].Value(), 16));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Warrior, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Warrior", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Warrior].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Wizard, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Wizard", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Wizard].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Angel, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Angel", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Angel].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Thief, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Thief", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Thief].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Archer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Archer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Archer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Jewelry24Tamer, AchievementRewardKind.EQInventorySlot, 1, "Have 24 Jewelry Equipment Slot of Tamer", () => game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Tamer].Value(), 24));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Warrior, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Warrior", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Warrior].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Wizard, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Wizard", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Wizard].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Angel, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Angel", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Angel].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Thief, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Thief", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Thief].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Archer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Archer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Archer].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility2Tamer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 2 Utility Equipment Slot of Tamer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Tamer].Value(), 2));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Warrior, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Warrior", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Warrior].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Wizard, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Wizard", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Wizard].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Angel, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Angel", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Angel].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Thief, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Thief", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Thief].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Archer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Archer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Archer].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility4Tamer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 4 Utility Equipment Slot of Tamer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Tamer].Value(), 4));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Warrior, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Warrior", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Warrior].Value(), 6));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Wizard, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Wizard", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Wizard].Value(), 6));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Angel, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Angel", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Angel].Value(), 6));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Thief, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Thief", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Thief].Value(), 6));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Archer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Archer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Archer].Value(), 6));
        achievementListEquip.Add(new GeneralAchievement(AchievementKind.Utility6Tamer, AchievementRewardKind.UtilityInventorySlot, 1, "Have 6 Utility Equipment Slot of Tamer", () => game.inventoryCtrl.equipPotionUnlockedNum[(int)HeroKind.Tamer].Value(), 6));
        achievementList.AddRange(achievementListEquip);

        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Warrior, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Warrior", () => game.statsCtrl.SkillSlotNum(HeroKind.Warrior).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Wizard, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Wizard", () => game.statsCtrl.SkillSlotNum(HeroKind.Wizard).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Angel, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Angel", () => game.statsCtrl.SkillSlotNum(HeroKind.Angel).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Thief, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Thief", () => game.statsCtrl.SkillSlotNum(HeroKind.Thief).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Archer, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Archer", () => game.statsCtrl.SkillSlotNum(HeroKind.Archer).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.ClassSkill8Tamer, AchievementRewardKind.ResourceGain, 2.00d, "Have 8 Class Skill Slot Slot of Tamer", () => game.statsCtrl.SkillSlotNum(HeroKind.Tamer).Value(), 8));
        achievementListSkill.Add(new GeneralAchievement(AchievementKind.GlobalSkill8, AchievementRewardKind.ResourceGain, 10.00d, "Have 8 Global Class Skill Slot", () => game.statsCtrl.globalSkillSlotNum.Value(), 8));
        achievementList.AddRange(achievementListSkill);

        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Warrior, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Warrior", () => game.rebirthCtrl.Rebirth(HeroKind.Warrior, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Wizard, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Wizard", () => game.rebirthCtrl.Rebirth(HeroKind.Wizard, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Angel, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Angel", () => game.rebirthCtrl.Rebirth(HeroKind.Angel, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Thief, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Thief", () => game.rebirthCtrl.Rebirth(HeroKind.Thief, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Archer, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Archer", () => game.rebirthCtrl.Rebirth(HeroKind.Archer, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth1Tamer, AchievementRewardKind.PortalOrb, 3, "Perform Tier 1 Rebirth of Tamer", () => game.rebirthCtrl.Rebirth(HeroKind.Tamer, 0).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Warrior, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Warrior", () => game.rebirthCtrl.Rebirth(HeroKind.Warrior, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Wizard, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Wizard", () => game.rebirthCtrl.Rebirth(HeroKind.Wizard, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Angel, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Angel", () => game.rebirthCtrl.Rebirth(HeroKind.Angel, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Thief, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Thief", () => game.rebirthCtrl.Rebirth(HeroKind.Thief, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Archer, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Archer", () => game.rebirthCtrl.Rebirth(HeroKind.Archer, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth2Tamer, AchievementRewardKind.PortalOrb, 5, "Perform Tier 2 Rebirth of Tamer", () => game.rebirthCtrl.Rebirth(HeroKind.Tamer, 1).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Warrior, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Warrior", () => game.rebirthCtrl.Rebirth(HeroKind.Warrior, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Wizard, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Wizard", () => game.rebirthCtrl.Rebirth(HeroKind.Wizard, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Angel, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Angel", () => game.rebirthCtrl.Rebirth(HeroKind.Angel, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Thief, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Thief", () => game.rebirthCtrl.Rebirth(HeroKind.Thief, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Archer, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Archer", () => game.rebirthCtrl.Rebirth(HeroKind.Archer, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth3Tamer, AchievementRewardKind.PortalOrb, 10, "Perform Tier 3 Rebirth of Tamer", () => game.rebirthCtrl.Rebirth(HeroKind.Tamer, 2).totalRebirthNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Warrior, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Warrior", () => false));// game.rebirthCtrl.Rebirth(HeroKind.Warrior, 3).totalRebirthNum >= 1));;
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Wizard, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Wizard", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Wizard, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Angel, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Angel", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Angel, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Thief, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Thief", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Thief, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Archer, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Archer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Archer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth4Tamer, AchievementRewardKind.PortalOrb, 15, "Perform Tier 4 Rebirth of Tamer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Tamer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Warrior, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Warrior", () => false));// game.rebirthCtrl.Rebirth(HeroKind.Warrior, 3).totalRebirthNum >= 1));;
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Wizard, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Wizard", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Wizard, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Angel, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Angel", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Angel, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Thief, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Thief", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Thief, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Archer, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Archer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Archer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth5Tamer, AchievementRewardKind.PortalOrb, 20, "Perform Tier 5 Rebirth of Tamer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Tamer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Warrior, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Warrior", () => false));// game.rebirthCtrl.Rebirth(HeroKind.Warrior, 3).totalRebirthNum >= 1));;
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Wizard, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Wizard", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Wizard, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Angel, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Angel", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Angel, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Thief, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Thief", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Thief, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Archer, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Archer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Archer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Rebirth6Tamer, AchievementRewardKind.PortalOrb, 30, "Perform Tier 6 Rebirth of Tamer", () => false));//game.rebirthCtrl.Rebirth(HeroKind.Tamer, 3).totalRebirthNum >= 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Ascension1, AchievementRewardKind.Nitro, 5000, "Perform Tier 1 World Ascension", () => game.ascensionCtrl.worldAscensions[0].performedNum, 1));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Ascension2, AchievementRewardKind.Nitro, 10000, "Perform Tier 2 World Ascension", () => false));
        achievementListRebirth.Add(new GeneralAchievement(AchievementKind.Ascension3, AchievementRewardKind.Nitro, 15000, "Perform Tier 3 World Ascension", () => false));
        achievementList.AddRange(achievementListRebirth);

        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime1day, AchievementRewardKind.ExpGain, 0.01d, "Play IEH2 for 1 day playtime", () => main.allTime , 1 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime2day, AchievementRewardKind.ExpGain, 0.02d, "Play IEH2 for 2 day playtime", () => main.allTime , 2 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime3day, AchievementRewardKind.ExpGain, 0.03d, "Play IEH2 for 3 day playtime", () => main.allTime , 3 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime4day, AchievementRewardKind.ExpGain, 0.04d, "Play IEH2 for 4 day playtime", () => main.allTime , 4 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime5day, AchievementRewardKind.ExpGain, 0.05d, "Play IEH2 for 5 day playtime", () => main.allTime , 5 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime1week, AchievementRewardKind.ExpGain, 0.06d, "Play IEH2 for 1 week playtime", () => main.allTime , 1 * 7 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime2week, AchievementRewardKind.ExpGain, 0.07d, "Play IEH2 for 2 week playtime", () => main.allTime , 2 * 7 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime3week, AchievementRewardKind.ExpGain, 0.08d, "Play IEH2 for 3 week playtime", () => main.allTime , 3 * 7 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime1month, AchievementRewardKind.ExpGain, 0.09d, "Play IEH2 for 1 month playtime", () => main.allTime , 1 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime2month, AchievementRewardKind.ExpGain, 0.10d, "Play IEH2 for 2 month playtime", () => main.allTime , 2 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime3month, AchievementRewardKind.ExpGain, 0.11d, "Play IEH2 for 3 month playtime", () => main.allTime , 3 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime4month, AchievementRewardKind.ExpGain, 0.12d, "Play IEH2 for 4 month playtime", () => main.allTime , 4 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime5month, AchievementRewardKind.ExpGain, 0.13d, "Play IEH2 for 5 month playtime", () => main.allTime , 5 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime6month, AchievementRewardKind.ExpGain, 0.14d, "Play IEH2 for 6 month playtime", () => main.allTime , 6 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime7month, AchievementRewardKind.ExpGain, 0.15d, "Play IEH2 for 7 month playtime", () => main.allTime , 7 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime8month, AchievementRewardKind.ExpGain, 0.16d, "Play IEH2 for 8 month playtime", () => main.allTime , 8 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime9month, AchievementRewardKind.ExpGain, 0.17d, "Play IEH2 for 9 month playtime", () => main.allTime , 9 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime10month, AchievementRewardKind.ExpGain, 0.18d, "Play IEH2 for 10 month playtime", () => main.allTime , 10 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime11month, AchievementRewardKind.ExpGain, 0.19d, "Play IEH2 for 11 month playtime", () => main.allTime , 11 * 30 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime1year, AchievementRewardKind.ExpGain, 0.20d, "Play IEH2 for 1 year playtime", () => main.allTime , 1 * 365 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime2year, AchievementRewardKind.ExpGain, 0.21d, "Play IEH2 for 2 year playtime", () => main.allTime , 2 * 365 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime3year, AchievementRewardKind.ExpGain, 0.22d, "Play IEH2 for 3 year playtime", () => main.allTime , 3 * 365 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime4year, AchievementRewardKind.ExpGain, 0.23d, "Play IEH2 for 4 year playtime", () => main.allTime , 4 * 365 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime5year, AchievementRewardKind.ExpGain, 0.24d, "Play IEH2 for 5 year playtime", () => main.allTime , 5 * 365 * 24 * 60 * 60));
        achievementListPlaytime.Add(new GeneralAchievement(AchievementKind.Playtime10year, AchievementRewardKind.ExpGain, 0.25d, "Play IEH2 for 10 year playtime", () => main.allTime , 10 * 365 * 24 * 60 * 60));
        achievementList.AddRange(achievementListPlaytime);
    }
    public void Start()
    {
        SetAchievement();
        for (int i = 0; i < achievementList.Count; i++)
        {
            achievementList[i].Start();
        }
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Achievement, MultiplierType.Mul, GoldGainBonus));
    }

    public double GoldGainBonus()
    {
        return TotalClearNum() * 0.01d;
    }
    public List<GeneralAchievement> achievementList = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListGeneral = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListArea = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListCurrency = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListGuild = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListChallenge = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListAlchemy = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListEquip = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListSkill = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListRebirth = new List<GeneralAchievement>();
    public List<GeneralAchievement> achievementListPlaytime = new List<GeneralAchievement>();

    public GeneralAchievement Achievement(AchievementKind kind)
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            if (achievementList[i].kind == kind) return achievementList[i];
        }
        return achievementList[0];
    }
    //Rebirthのチェック
    public bool IsAchievedAnyHeroRebirth(int tier)
    {
        switch (tier)
        {
            case 0: return Achievement(Rebirth1Warrior).isCleared || Achievement(Rebirth1Wizard).isCleared || Achievement(Rebirth1Angel).isCleared || Achievement(Rebirth1Thief).isCleared || Achievement(Rebirth1Archer).isCleared || Achievement(Rebirth1Tamer).isCleared;
            case 1: return Achievement(Rebirth2Warrior).isCleared || Achievement(Rebirth2Wizard).isCleared || Achievement(Rebirth2Angel).isCleared || Achievement(Rebirth2Thief).isCleared || Achievement(Rebirth2Archer).isCleared || Achievement(Rebirth2Tamer).isCleared;
            case 2: return Achievement(Rebirth3Warrior).isCleared || Achievement(Rebirth3Wizard).isCleared || Achievement(Rebirth3Angel).isCleared || Achievement(Rebirth3Thief).isCleared || Achievement(Rebirth3Archer).isCleared || Achievement(Rebirth3Tamer).isCleared;
            case 3: return Achievement(Rebirth4Warrior).isCleared || Achievement(Rebirth4Wizard).isCleared || Achievement(Rebirth4Angel).isCleared || Achievement(Rebirth4Thief).isCleared || Achievement(Rebirth4Archer).isCleared || Achievement(Rebirth4Tamer).isCleared;
            case 4: return Achievement(Rebirth5Warrior).isCleared || Achievement(Rebirth5Wizard).isCleared || Achievement(Rebirth5Angel).isCleared || Achievement(Rebirth5Thief).isCleared || Achievement(Rebirth5Archer).isCleared || Achievement(Rebirth5Tamer).isCleared;
            case 5: return Achievement(Rebirth6Warrior).isCleared || Achievement(Rebirth6Wizard).isCleared || Achievement(Rebirth6Angel).isCleared || Achievement(Rebirth6Thief).isCleared || Achievement(Rebirth6Archer).isCleared || Achievement(Rebirth6Tamer).isCleared;
        }
        return false;
    }


    public int TotalClearNum()
    {
        int tempNum = 0;
        for (int i = 0; i < achievementList.Count; i++)
        {
            if (achievementList[i].isCleared) tempNum++;
        }
        return tempNum;
    }
    public void CheckAchieve()//毎秒か毎分呼ぶ
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            achievementList[i].CheckAchieve();
        }
    }
    public bool CanClaimReward()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            if (achievementList[i].CanClaimRewad()) return true;
        }
        return false;
    }
    public void ClaimReward()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            achievementList[i].ClaimReward();
        }
    }
}

public class GeneralAchievement
{
    public GeneralAchievement(AchievementKind kind, AchievementRewardKind rewardKind, double rewardAmount, string name, Func<bool> condition)
    {
        this.kind = kind;
        this.name = name;
        this.condition = condition;
        this.rewardKind = rewardKind;
        this.rewardAmount = rewardAmount;
    }

    public GeneralAchievement(AchievementKind kind, AchievementRewardKind rewardKind, double rewardAmount, string name, Func<double> value, double targetValue, bool isExponential = false)
    {
        this.kind = kind;
        this.name = name;
        this.value = value;
        this.targetValue = targetValue;
        this.isExponential = isExponential;
        this.condition = () => value() >= targetValue;
        this.rewardKind = rewardKind;
        this.rewardAmount = rewardAmount;
    }

    public AchievementKind kind;
    public AchievementRewardKind rewardKind;
    public double rewardAmount;
    string name;
    Func<bool> condition;
    Func<double> value;
    double targetValue;
    bool isExponential;
    public bool isCleared { get => main.S.isGotAchievements[(int)kind]; set => main.S.isGotAchievements[(int)kind] = value; }
    public bool isGotReward { get => main.S.isGotAchievementRewards[(int)kind]; set => main.S.isGotAchievementRewards[(int)kind] = value; }
    public double playtime { get => main.S.achievementPlaytimes[(int)kind]; set => main.S.achievementPlaytimes[(int)kind] = value; }
    public void Start()
    {
        switch (rewardKind)
        {
            case AchievementRewardKind.GoldCap:
                game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(Achievement, Mul, () => rewardAmount, () => isGotReward));
                break;
            case AchievementRewardKind.ResourceGain:
                game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(Achievement, Mul, () => rewardAmount, () => isGotReward));
                break;
            case AchievementRewardKind.ExpGain:
                game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(Achievement, Mul, () => rewardAmount, () => isGotReward));
                break;
            case AchievementRewardKind.EQInventorySlot:
                game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(Achievement, Add, () => rewardAmount, () => isGotReward));
                break;
            case AchievementRewardKind.UtilityInventorySlot:
                game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(Achievement, Add, () => rewardAmount, () => isGotReward));
                break;
        }
    }
    public void Achieve()
    {
        isCleared = true;
        playtime = main.allTime;
    }
    public void CheckAchieve()
    {
        if (isCleared) return;
        if (condition()) Achieve();
    }
    public bool CanClaimRewad()
    {
        if (!isCleared) return false;
        if (isGotReward) return false;
        return true;
    }
    public void ClaimReward()
    {
        if (!CanClaimRewad()) return;
        isGotReward = true;
        switch (rewardKind)
        {
            case AchievementRewardKind.EpicCoin:
                game.epicStoreCtrl.epicCoin.Increase(rewardAmount);
                break;
            case AchievementRewardKind.PortalOrb:
                game.areaCtrl.portalOrb.Increase(rewardAmount);
                break;
            case AchievementRewardKind.Nitro:
                game.nitroCtrl.nitro.IncreaseWithoutLimit(rewardAmount);
                break;
        }
    }
    public double ProgressPercent()
    {
        if (targetValue == 0) return Convert.ToInt16(condition());
        if (isExponential) return Math.Min(1, Math.Log10(value()) / Math.Log10(targetValue));
        return Math.Min(1, value() / targetValue);
    }
    string tempStr;
    public string NameString()
    {
        //tempStr = isCleared ? "<color=green>" : "<color=white>";
        if (isCleared)
        {
            if (isGotReward) tempStr = "<color=green>";
            else tempStr = "<color=orange>";
        }
        else tempStr = "<color=white>";

        tempStr += optStr + "- " + name;
        if (targetValue > 0)
        {
            //if (isExponential)
            //    tempStr += optStr + "  [ " + tDigit(value()) + " / " + tDigit(targetValue) + " | " + percent(ProgressPercent(), 0) + " (Power of 10)]";
            //else
                tempStr += optStr + "  [ " + tDigit(value()) + " / " + tDigit(targetValue) + " | " + percent(ProgressPercent(), 0) + " ]"; 
        }
        if (isCleared)
        {
            tempStr += optStr + "  ( " + DoubleTimeToDate(playtime, true) + " )";
        }
        tempStr += "</color>";
        return tempStr;
    }
    string tempStr2;
    public string RewardString()
    {
        if (isCleared)
        {
            if (isGotReward) tempStr2 = "<color=green>";
            else tempStr2 = "<color=orange>";
        }
        else tempStr2 = "<color=white>";
        switch (rewardKind)
        {
            case AchievementRewardKind.EpicCoin:
                tempStr2 += "<sprite=\"EpicCoin\" index=0> " + tDigit(rewardAmount) + " Epic Coin";
                break;
            case AchievementRewardKind.PortalOrb:
                tempStr2 += tDigit(rewardAmount) + " Portal Orb";
                break;
            case AchievementRewardKind.Nitro:
                tempStr2 += tDigit(rewardAmount) + " Nitro";
                break;
            case AchievementRewardKind.GoldCap:
                tempStr2 += "Gold Cap + " + percent(rewardAmount);
                break;
            case AchievementRewardKind.ResourceGain:
                tempStr2 += "Resource Gain + " + percent(rewardAmount);
                break;
            case AchievementRewardKind.ExpGain:
                tempStr2 += "EXP Gain + " + percent(rewardAmount);
                break;
            case AchievementRewardKind.EQInventorySlot:
                tempStr2 += "Equipment Inventory Slot + " + tDigit(rewardAmount);
                break;
            case AchievementRewardKind.UtilityInventorySlot:
                tempStr2 += "Utility Inventory Slot + " + tDigit(rewardAmount);
                break;
        }
        tempStr2 += "</color>";
        return tempStr2;
    }
}

public enum AchievementRewardKind
{
    EpicCoin,
    PortalOrb,
    Nitro,
    GoldCap,
    ResourceGain,
    ExpGain,
    EQInventorySlot,
    UtilityInventorySlot,
}

public enum AchievementKind
{
    //General
    ClearTutorial,
    UnlockWizard,
    UnlockAngel,
    UnlockThief,
    UnlockArcher,
    UnlockTamer,
    Nitro1000,
    Nitro10000,
    Nitro100000,
    Nitro1000000,
    Nitro10000000,
    Nitro100000000,
    Capture100,
    Capture1000,
    Capture10000,
    Capture100000,
    Capture1000000,
    Chest1,
    Chest10,
    Chest100,
    Chest1000,
    Chest10000,
    Chest100000,
    Chest1000000,
    Mimic1,
    Mimic10,
    Mimic100,
    Mimic1000,
    Mimic10000,
    Mimic100000,
    Mimic1000000,
    Walk40075km,
    Walk40075km2,//2倍
    Walk40075km3,//3倍
    Walk40075km5,//5倍
    Walk384400km,//月
    Walk384400km2,//月2倍
    Walk149600000km,//太陽
    EpicCoin1000,
    EpicCoin10000,
    EpicCoin100000,
    EpicCoin1000000,
    //Area
    UnlockMagicslime,
    UnlockSpider,
    UnlockBat,
    UnlockFairy,
    UnlockFox,
    UnlockDevilfish,
    UnlockTreant,
    UnlockFlametiger,
    UnlockUnicorn,
    EquipUniqueSlime,
    EquipUniqueMagicslime,
    EquipUniqueSpider,
    EquipUniqueBat,
    EquipUniqueFairy,
    EquipUniqueFox,
    EquipUniqueDevilfish,
    EquipUniqueTreant,
    EquipUniqueFlametiger,
    EquipUniqueUnicorn,
    //Currency
    Gold1e6,
    Gold1e9,
    Gold1e10,
    Gold1e11,
    Gold1e12,
    Gold1e13,
    Gold1e14,
    Gold1e15,
    Gold1e16,
    Gold1e17,
    Gold1e18,
    Gold1e19,
    Gold1e20,
    Gold1e22,
    Gold1e24,
    Gold1e26,
    Gold1e28,
    Gold1e30,
    Stone1e10,
    Stone1e20,
    Stone1e30,
    Stone1e40,
    Stone1e50,
    Stone1e60,
    Stone1e70,
    Stone1e80,
    Stone1e90,
    Stone1e100,
    Stone1e110,
    Stone1e120,
    Stone1e130,
    Stone1e140,
    Stone1e150,
    Stone1e160,
    Stone1e170,
    Stone1e180,
    Stone1e190,
    Stone1e200,
    Crystal1e10,
    Crystal1e20,
    Crystal1e30,
    Crystal1e40,
    Crystal1e50,
    Crystal1e60,
    Crystal1e70,
    Crystal1e80,
    Crystal1e90,
    Crystal1e100,
    Crystal1e110,
    Crystal1e120,
    Crystal1e130,
    Crystal1e140,
    Crystal1e150,
    Crystal1e160,
    Crystal1e170,
    Crystal1e180,
    Crystal1e190,
    Crystal1e200,
    Leaf1e10,
    Leaf1e20,
    Leaf1e30,
    Leaf1e40,
    Leaf1e50,
    Leaf1e60,
    Leaf1e70,
    Leaf1e80,
    Leaf1e90,
    Leaf1e100,
    Leaf1e110,
    Leaf1e120,
    Leaf1e130,
    Leaf1e140,
    Leaf1e150,
    Leaf1e160,
    Leaf1e170,
    Leaf1e180,
    Leaf1e190,
    Leaf1e200,
    //Guild
    GLv20,
    GLv40,
    GLv60,
    GLv80,
    GLv100,
    GLv120,
    GLv140,
    GLv160,
    GLv180,
    GLv200,
    GLv220,
    GLv240,
    GLv260,
    GLv280,
    GLv300,
    //Swarm
    Swarm1,
    Swarm10,
    Swarm50,
    Swarm100,
    Swarm500,
    Swarm1000,
    Swarm5000,
    Swarm10000,
    Swarm100000,
    Swarm1000000,
    Swarm10000000,
    //Challenge
    Florzporb,
    Arachnetta,
    GurdianKor,
    Nostro,
    LadyEmelda,
    NariSune,
    Octobaddie,
    Bananoon,
    Glorbliorbus,
    Gankyu,
    //Alchemy
    PotionLv50,
    PotionLv250,
    PotionLv600,
    PotionLv1250,
    PotionLv2500,
    PotionLv3000,
    AlchemyPoint100000,
    AlchemyPoint1000000,    //12850000
    AlchemyPoint10000000,
    AlchemyPoint100000000,
    AlchemyPoint1000000000,
    AlchemyPoint10000000000,
    //Equip
    EquipGain1000,
    EquipGain10000,
    EquipGain100000,
    EquipGain1000000,
    EquipGain10000000,
    Weapon8Warrior,
    Weapon8Wizard,
    Weapon8Angel,
    Weapon8Thief,
    Weapon8Archer,
    Weapon8Tamer,
    Weapon16Warrior,
    Weapon16Wizard,
    Weapon16Angel,
    Weapon16Thief,
    Weapon16Archer,
    Weapon16Tamer,
    Weapon24Warrior,
    Weapon24Wizard,
    Weapon24Angel,
    Weapon24Thief,
    Weapon24Archer,
    Weapon24Tamer,
    Armor8Warrior,
    Armor8Wizard,
    Armor8Angel,
    Armor8Thief,
    Armor8Archer,
    Armor8Tamer,
    Armor16Warrior,
    Armor16Wizard,
    Armor16Angel,
    Armor16Thief,
    Armor16Archer,
    Armor16Tamer,
    Armor24Warrior,
    Armor24Wizard,
    Armor24Angel,
    Armor24Thief,
    Armor24Archer,
    Armor24Tamer,
    Jewelry8Warrior,
    Jewelry8Wizard,
    Jewelry8Angel,
    Jewelry8Thief,
    Jewelry8Archer,
    Jewelry8Tamer,
    Jewelry16Warrior,
    Jewelry16Wizard,
    Jewelry16Angel,
    Jewelry16Thief,
    Jewelry16Archer,
    Jewelry16Tamer,
    Jewelry24Warrior,
    Jewelry24Wizard,
    Jewelry24Angel,
    Jewelry24Thief,
    Jewelry24Archer,
    Jewelry24Tamer,
    Utility2Warrior,
    Utility2Wizard,
    Utility2Angel,
    Utility2Thief,
    Utility2Archer,
    Utility2Tamer,
    Utility4Warrior,
    Utility4Wizard,
    Utility4Angel,
    Utility4Thief,
    Utility4Archer,
    Utility4Tamer,
    Utility6Warrior,
    Utility6Wizard,
    Utility6Angel,
    Utility6Thief,
    Utility6Archer,
    Utility6Tamer,
    //Skill
    ClassSkill8Warrior,
    ClassSkill8Wizard,
    ClassSkill8Angel,
    ClassSkill8Thief,
    ClassSkill8Archer,
    ClassSkill8Tamer,
    GlobalSkill8,
    //Rebirth
    Rebirth1Warrior,
    Rebirth1Wizard,
    Rebirth1Angel,
    Rebirth1Thief,
    Rebirth1Archer,
    Rebirth1Tamer,
    Rebirth2Warrior,
    Rebirth2Wizard,
    Rebirth2Angel,
    Rebirth2Thief,
    Rebirth2Archer,
    Rebirth2Tamer,
    Rebirth3Warrior,
    Rebirth3Wizard,
    Rebirth3Angel,
    Rebirth3Thief,
    Rebirth3Archer,
    Rebirth3Tamer,
    Rebirth4Warrior,
    Rebirth4Wizard,
    Rebirth4Angel,
    Rebirth4Thief,
    Rebirth4Archer,
    Rebirth4Tamer,
    Rebirth5Warrior,
    Rebirth5Wizard,
    Rebirth5Angel,
    Rebirth5Thief,
    Rebirth5Archer,
    Rebirth5Tamer,
    Rebirth6Warrior,
    Rebirth6Wizard,
    Rebirth6Angel,
    Rebirth6Thief,
    Rebirth6Archer,
    Rebirth6Tamer,
    Ascension1,
    Ascension2,
    Ascension3,
    //Playtime
    Playtime1day,
    Playtime2day,
    Playtime3day,
    Playtime4day,
    Playtime5day,
    Playtime1week,
    Playtime2week,
    Playtime3week,
    Playtime1month,
    Playtime2month,
    Playtime3month,
    Playtime4month,
    Playtime5month,
    Playtime6month,
    Playtime7month,
    Playtime8month,
    Playtime9month,
    Playtime10month,
    Playtime11month,
    Playtime1year,
    Playtime2year,
    Playtime3year,
    Playtime4year,
    Playtime5year,
    Playtime10year,
    //Enum追加するときは必ず最後に追加する。Listへの追加は順番不問
}