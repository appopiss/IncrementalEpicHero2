using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using System;
using Cysharp.Threading.Tasks;

public partial class Save
{
    //統計
    public long[] ascensionNum;//[tier]
    public double[] ascensionPlayTime;//[tier]

    public double[] ascensionPoints;//[tier]

    public double[] bestAscensionPlayTime;//[tier]
    public double[] accomplishedFirstTimeAscension;//[tier]
    public double[] accomplishedTimeAscension;//[tier]
    public double[] accomplishedBestTimeAscension;//[tier]
}


public class AscensionController
{
    public AscensionController()
    {
        worldAscensions[0] = new WorldAscensionTier1();
        //worldAscensions[1] = new WorldAscension();
        //worldAscensions[2] = new WorldAscension();
    }
    public void Start()
    {
        for (int i = 0; i < worldAscensions.Length; i++)
        {
            worldAscensions[i].Start();
        }
    }
    public WorldAscension[] worldAscensions = new WorldAscension[1];
}

public class WorldAscensionTier1 : WorldAscension
{
    public override int tier => 0;
    public override void ResetSave()
    {
        base.ResetSave();
    }
    public override void SetUpgrade()
    {
        upgradeList.Add(new WAU_GuildExpGain(this));
        upgradeList.Add(new WAU_AreaClearCount(this));
        upgradeList.Add(new WAU_ActiveHero(this));
        upgradeList.Add(new WAU_SkillProfGain(this));
        upgradeList.Add(new WAU_PreRebirthTier1(this));
        upgradeList.Add(new WAU_PreRebirthTier2(this));
        upgradeList.Add(new WAU_RebirthTier1BonusCap(this));
        upgradeList.Add(new WAU_RebirthTier2BonusCap(this));
        upgradeList.Add(new WAU_PointGainBonus(this));
    }
    public override void SetMilestone() 
    {
        milestoneList.Add(new WAM_BuildingLevel(this));
        milestoneList.Add(new WAM_MissionClearNum(this));
        milestoneList.Add(new WAM_UpgradeLevel(this));
        milestoneList.Add(new WAM_MoveDistance(this));
        milestoneList.Add(new WAM_DictionaryPoint(this));
        milestoneList.Add(new WAM_DisassembleEquipment(this));
        milestoneList.Add(new WAM_RebirthPointGainTier1(this));
        milestoneList.Add(new WAM_RebirthPointGainTier2(this));
    }
    bool IsWithinTimesec(double bestTime, double targetTimesec)
    {
        return bestTime > 0 && bestTime <= targetTimesec;
    }
    public override void SetMission()
    {
        missionList.Add(new WorldAscensionMission(this, 0, 1, "Perform any hero's Tier 1 Rebirth within 3 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(0), 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 1, 1, "Perform any hero's Tier 1 Rebirth within 1 hour", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(0), 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 2, 1, "Perform any hero's Tier 1 Rebirth within 20 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(0), 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 3, 2, "Perform any hero's Tier 1 Rebirth within 5 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(0), 5 * 60d)));
        //missionList.Add(new WorldAscensionMission(this, 4, 1, "Perform any hero's Tier 2 Rebirth within 8 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(1), 8 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 4, 1, "Perform any hero's Tier 2 Rebirth within 3 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(1), 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 5, 1, "Perform any hero's Tier 2 Rebirth within 1 hour", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(1), 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 6, 2, "Perform any hero's Tier 2 Rebirth within 20 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(1), 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 7, 3, "Perform any hero's Tier 2 Rebirth within 5 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(1), 5 * 60d)));
        //missionList.Add(new WorldAscensionMission(this, 9, 1, "Perform any hero's Tier 3 Rebirth within 24 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 24 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 8, 1, "Perform any hero's Tier 3 Rebirth within 8 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 8 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 9, 2, "Perform any hero's Tier 3 Rebirth within 3 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 10, 2, "Perform any hero's Tier 3 Rebirth within 1 hours", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 11, 3, "Perform any hero's Tier 3 Rebirth within 20 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 12, 5, "Perform any hero's Tier 3 Rebirth within 5 mins", () => IsWithinTimesec(game.rebirthCtrl.AccomplishBestTime(2), 5 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 13, 1, "Clear Raid Boss [Florzporb Lv 100] within 8 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).accomplish.accomplishedBestTime, 8 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 14, 1, "Clear Raid Boss [Florzporb Lv 100] within 3 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).accomplish.accomplishedBestTime, 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 15, 1, "Clear Raid Boss [Florzporb Lv 100] within 1 hour", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).accomplish.accomplishedBestTime, 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 16, 2, "Clear Raid Boss [Florzporb Lv 100] within 20 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).accomplish.accomplishedBestTime, 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 17, 2, "Clear Raid Boss [Florzporb Lv 100] within 5 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).accomplish.accomplishedBestTime, 5 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 18, 1, "Clear Raid Boss [Arachnetta Lv 150] within 8 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).accomplish.accomplishedBestTime, 8 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 19, 1, "Clear Raid Boss [Arachnetta Lv 150] within 3 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).accomplish.accomplishedBestTime, 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 20, 2, "Clear Raid Boss [Arachnetta Lv 150] within 1 hour", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).accomplish.accomplishedBestTime, 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 21, 2, "Clear Raid Boss [Arachnetta Lv 150] within 20 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).accomplish.accomplishedBestTime, 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 22, 3, "Clear Raid Boss [Arachnetta Lv 150] within 5 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.WindowQueenRaid150).accomplish.accomplishedBestTime, 5 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 23, 1, "Clear Raid Boss [Guardian Kor Lv 200] within 8 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).accomplish.accomplishedBestTime, 8 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 24, 1, "Clear Raid Boss [Guardian Kor Lv 200] within 3 hours", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).accomplish.accomplishedBestTime, 3 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 25, 2, "Clear Raid Boss [Guardian Kor Lv 200] within 1 hour", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).accomplish.accomplishedBestTime, 1 * 60 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 26, 3, "Clear Raid Boss [Guardian Kor Lv 200] within 20 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).accomplish.accomplishedBestTime, 20 * 60d)));
        missionList.Add(new WorldAscensionMission(this, 27, 5, "Clear Raid Boss [Guardian Kor Lv 200] within 5 mins", () => IsWithinTimesec(game.challengeCtrl.Challenge(ChallengeKind.GolemRaid200).accomplish.accomplishedBestTime, 5 * 60d)));
        //missionList.Add(new WorldAscensionMission(this, 30, 1, "Get to Guild Level 10 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[10].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[10].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 28, 1, "Get to Guild Level 10 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[10].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[10].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 29, 1, "Get to Guild Level 15 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[15].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[15].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 30, 1, "Get to Guild Level 15 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[15].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[15].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 31, 1, "Get to Guild Level 20 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 32, 1, "Get to Guild Level 20 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 33, 2, "Get to Guild Level 20 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[20].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 34, 1, "Get to Guild Level 25 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 35, 1, "Get to Guild Level 25 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 36, 2, "Get to Guild Level 25 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 37, 4, "Get to Guild Level 25 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[25].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 38, 1, "Get to Guild Level 30 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 39, 1, "Get to Guild Level 30 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 40, 2, "Get to Guild Level 30 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 41, 3, "Get to Guild Level 30 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 42, 5, "Get to Guild Level 30 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[30].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 43, 1, "Get to Guild Level 35 within 24 hours", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 24 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 44, 2, "Get to Guild Level 35 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 45, 3, "Get to Guild Level 35 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 46, 4, "Get to Guild Level 35 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 47, 6, "Get to Guild Level 35 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 48, 8, "Get to Guild Level 35 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[35].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 49, 2, "Get to Guild Level 40 within 24 hours", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 24 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 50, 3, "Get to Guild Level 40 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 51, 4, "Get to Guild Level 40 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 52, 5, "Get to Guild Level 40 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 53, 7, "Get to Guild Level 40 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 54, 10, "Get to Guild Level 40 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[40].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 55, 3, "Get to Guild Level 45 within 24 hours", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 24 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 56, 4, "Get to Guild Level 45 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 57, 5, "Get to Guild Level 45 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 58, 9, "Get to Guild Level 45 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 59, 11, "Get to Guild Level 45 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 60, 15, "Get to Guild Level 45 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[45].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 61, 4, "Get to Guild Level 50 within 24 hours", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 24 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 62, 5, "Get to Guild Level 50 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 63, 6, "Get to Guild Level 50 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 64, 8, "Get to Guild Level 50 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 65, 12, "Get to Guild Level 50 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 66, 20, "Get to Guild Level 50 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[50].accomplishedBestTime <= 5 * 60d));
        missionList.Add(new WorldAscensionMission(this, 67, 5, "Get to Guild Level 55 within 24 hours", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 24 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 68, 6, "Get to Guild Level 55 within 8 hours", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 8 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 69, 7, "Get to Guild Level 55 within 3 hours", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 3 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 70, 9, "Get to Guild Level 55 within 1 hour", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 1 * 60 * 60d));
        missionList.Add(new WorldAscensionMission(this, 71, 14, "Get to Guild Level 55 within 20 mins", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 20 * 60d));
        missionList.Add(new WorldAscensionMission(this, 72, 25, "Get to Guild Level 55 within 5 mins", () => game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime > 0 && game.guildCtrl.accomplishGuildLevels[55].accomplishedBestTime <= 5 * 60d));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 5, "Preserve Unique Equipment on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 10, "Preserve Catalyst Level on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 15, "Preserve Statue of Heroes Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 20, "Preserve Cartographer Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 25, "Preserve Alchemist's Hut Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 30, "Preserve Trapper Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 35, "Preserve Temple Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 40, "Preserve Blacksmith Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 45, "Preserve Slime Bank Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 50, "Preserve Mystic Arena Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 55, "Preserve Arcane Researcher Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 60, "Preserve Adventuring Party Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 65, "Preserve Dojo Rank on World Ascension"));
        missionMilestoneList.Add(new WorldAscensionMissionMilestone(MissionClearNum, 70, "Preserve Tavern Rank on World Ascension"));
    }
}

public class WorldAscensionPoint : NUMBER
{
    public WorldAscensionPoint(WorldAscension wa)
    {
        this.wa = wa;
    }
    public WorldAscension wa;
    public override double value { get => main.S.ascensionPoints[(int)wa.tier]; set => main.S.ascensionPoints[(int)wa.tier] = value; }
}
public class AccomplishWorldAscension : ACCOMPLISH
{
    public AccomplishWorldAscension(WorldAscension wa)
    {
        this.wa = wa;
    }
    public WorldAscension wa;
    public override double accomplishedFirstTime { get => main.S.accomplishedFirstTimeAscension[wa.tier]; set => main.S.accomplishedFirstTimeAscension[wa.tier] = value; }
    public override double accomplishedTime { get => main.S.accomplishedTimeAscension[wa.tier]; set => main.S.accomplishedTimeAscension[wa.tier] = value; }
    public override double accomplishedBestTime { get => main.S.accomplishedBestTimeAscension[wa.tier]; set => main.S.accomplishedBestTimeAscension[wa.tier] = value; }
    public override void RegisterTime()
    {
        if (accomplishedTime <= 0) accomplishedTime = main.allTime;
        else accomplishedTime = Math.Min(accomplishedTime, main.allTime);
        if (accomplishedBestTime <= 0)//初めて
        {
            accomplishedBestTime = accomplishedTime;
            accomplishedFirstTime = accomplishedTime;
        }
        else accomplishedBestTime = Math.Min(accomplishedBestTime, accomplishedTime);
    }
}

public class WorldAscension
{
    public WorldAscension()
    {
        point = new WorldAscensionPoint(this);
        pointGainBonus = new Multiplier();
        accomplish = new AccomplishWorldAscension(this);
        SetUpgrade();
        SetMilestone();
    }
    public void Start()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeList[i].Start();
        }
        for (int i = 0; i < milestoneList.Count; i++)
        {
            milestoneList[i].Start();
        }
        SetMission();
    }
    public virtual int tier { get; }//0,1,2
    public long performedNum { get => main.S.ascensionNum[tier]; set => main.S.ascensionNum[tier] = value; }
    public double lastPlayTime { get => main.S.ascensionPlayTime[tier]; set => main.S.ascensionPlayTime[tier] = value; }
    public WorldAscensionPoint point;
    public List<WorldAscensionUpgrade> upgradeList = new List<WorldAscensionUpgrade>();
    public virtual void SetUpgrade() { }
    public List<WorldAscensionMilestone> milestoneList = new List<WorldAscensionMilestone>();
    public virtual void SetMilestone() { }
    public List<WorldAscensionMission> missionList = new List<WorldAscensionMission>();
    public List<WorldAscensionMissionMilestone> missionMilestoneList = new List<WorldAscensionMissionMilestone>();
    public virtual void SetMission() { }
    public void CheckAccomplishementClear()
    {
        for (int i = 0; i < missionList.Count; i++)
        {
            missionList[i].CheckClear();
        }
    }
    public Multiplier pointGainBonus;
    public long MissionClearNum()
    {
        long tempNum = 0;
        for (int i = 0; i < missionList.Count; i++)
        {
            if (missionList[i].condition()) tempNum++;
        }
        return tempNum;
    }

    public AccomplishWorldAscension accomplish;
    public double bestPlayTime { get => main.S.bestAscensionPlayTime[tier]; set => main.S.bestAscensionPlayTime[tier] = value; }
    public double WorldAscensionPlaytime() { return main.allTime - lastPlayTime; }
    public long TotalMilestoneLevel()
    {
        long tempLevel = 0;
        for (int i = 0; i < milestoneList.Count; i++)
        {
            tempLevel += milestoneList[i].CurrentLevel();
        }
        return tempLevel;
    }
    public async void CalculateTotalMilestoneLevel()
    {
        for (int i = 0; i < milestoneList.Count; i++)
        {
            await milestoneList[i].CalculateCurrentLevel();
        }
    }
    public long NextMilestoneLevelToPerform() { return Math.Min(milestoneList.Count * 10, 5 + performedNum); }
    public void ResetUpgrade()
    {
        point.ChangeValue(TotalUpgradePoint());
        ResetUpgradeLevel();
    }
    void ResetUpgradeLevel()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeList[i].level.ChangeValue(0);
        }
    }
    public double TotalUpgradePoint()
    {
        double tempPoint = point.value;
        for (int i = 0; i < upgradeList.Count; i++)
        {
            tempPoint += upgradeList[i].transaction.TotalCostConsumed();
        }
        return tempPoint;
    }
    bool isTrying;
    //EpicStore[World Ascension Upgrade Reset]用
    public async void Reset()
    {
        if (isTrying) return;
        isTrying = true;
        ResetUpgrade();
        game.isPause = true;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.guildCtrl.Member(heroKind).SwitchActive(false);
        }
        lastPlayTime = main.allTime;
        main.allTimeWorldAscension = 0;

        ResetSave();

        game.guildCtrl.Member(HeroKind.Warrior).SwitchPlay();
        await UniTask.DelayFrame(5);
        isTrying = false;
        Initialize.InitializeSaveR();
    }

    public async void Perform()//実際にWorldAscensionを実行する処理
    {
        if (!CanPerform()) return;
        isTrying = true;
        game.isPause = true;

        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.guildCtrl.Member(heroKind).SwitchActive(false);
        }

        //統計データなど
        performedNum++;
        if (bestPlayTime > 0)//２回目以降
            bestPlayTime = Math.Min(WorldAscensionPlaytime(), bestPlayTime);
        else
            bestPlayTime = main.allTime;
        lastPlayTime = main.allTime;
        main.allTimeWorldAscension = 0;
        accomplish.RegisterTime();

        GetPoint();
        UpdateMaxLevelReachedMilestone();

        ResetSave();
        //GetBonus

        //Initialize
        //game.alchemyCtrl.mysteriousWaterExpandedCapNum.Increase(1);
        //game.guildCtrl.Member(game.currentHero).SwitchActive(true);
        game.guildCtrl.Member(HeroKind.Warrior).SwitchPlay();
        await UniTask.DelayFrame(5);
        isTrying = false;
        Initialize.InitializeSaveR();
        //SwitchHeroをシーン読み込みなしにする場合は、これプラス、Initialize.InitializeSaveR()を呼ぶ必要がある
    }
    public bool CanPerform() { return !isTrying && TotalMilestoneLevel() >= NextMilestoneLevelToPerform(); }

    public void GetPoint()
    {
        point.Increase(PointGain());
    }
    public void UpdateMaxLevelReachedMilestone()
    {
        for (int i = 0; i < milestoneList.Count; i++)
        {
            milestoneList[i].UpdateMaxLevelReachedMilestone();
        }
    }
    
    public double PointGain()
    {
        double tempPoint = 0;
        for (int i = 0; i < milestoneList.Count; i++)
        {
            tempPoint += milestoneList[i].pointGain;
        }
        return tempPoint;
    }

    public virtual void ResetSave()
    {
        //統計
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            main.SR.playtimes[i] = 0;
            main.SR.playtimesRealTime[i] = 0;
        }
        //Hero固有
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            //HeroLevel
            game.statsCtrl.Exp(heroKind).ChangeValue(0);
            game.statsCtrl.HeroLevel(heroKind).ChangeValue(0);
            //Ability
            game.statsCtrl.ResetAbilityPoint(heroKind);
            //Quest
            for (int j = 0; j < game.questCtrl.QuestArray(QuestKind.General, heroKind).Length; j++)
            {
                game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isAccepted = false;
                game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isCleared = false;
                game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isClearedOnce = false;
                game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isFavorite = false;
            }
            for (int j = 0; j < game.questCtrl.QuestArray(QuestKind.Title, heroKind).Length; j++)
            {
                game.questCtrl.QuestArray(QuestKind.Title, heroKind)[j].isAccepted = false;
            }
            //MoveDistance
            game.statsCtrl.MovedDistance(heroKind, true).ChangeValue(0);
            main.S.totalMovedDistance[(int)heroKind] = 0;
            //game.statsCtrl.MovedDistance(heroKind, false).ChangeValue(0);
        }
        main.S.totalMovedDistancePet = 0;
        main.SR.movedDistancePet = 0;

        //DailyQuestのうちクリアしてないもののAccept情報を消す
        for (int i = 0; i < game.questCtrl.QuestArray(QuestKind.Daily, HeroKind.Warrior).Length; i++)
        {
            game.questCtrl.QuestArray(QuestKind.Daily, HeroKind.Warrior)[i].isAccepted = false;
        }

        //GeneralQuestClear#
        main.SR.totalGeneralQuestClearedNum = 0;
        for (int i = 0; i < main.SR.totalGeneralQuestClearedNums.Length; i++)
        {
            main.SR.totalGeneralQuestClearedNums[i] = 0;
        }

        //Skill
        for (int i = 0; i < game.skillCtrl.skillsOneDimensionArray.Length; i++)
        {
            SKILL skill = game.skillCtrl.skillsOneDimensionArray[i];
            skill.proficiency.ChangeValue(0);
            skill.level.ChangeValue(0);
            skill.level.maxReachedLevel = 0;
            if (skill.id == 0) skill.rank.ChangeValue(1);
            else skill.rank.ChangeValue(0);
        }
        for (int i = 0; i < main.SR.isEquippedWarriorSkillForWarrior.Length; i++)
        {
            main.SR.isEquippedWarriorSkillForWarrior[i] = false;
            main.SR.isEquippedWizardSkillForWarrior[i] = false;
            main.SR.isEquippedAngelSkillForWarrior[i] = false;
            main.SR.isEquippedThiefSkillForWarrior[i] = false;
            main.SR.isEquippedArcherSkillForWarrior[i] = false;
            main.SR.isEquippedTamerSkillForWarrior[i] = false;

            main.SR.isEquippedWarriorSkillForWizard[i] = false;
            main.SR.isEquippedWizardSkillForWizard[i] = false;
            main.SR.isEquippedAngelSkillForWizard[i] = false;
            main.SR.isEquippedThiefSkillForWizard[i] = false;
            main.SR.isEquippedArcherSkillForWizard[i] = false;
            main.SR.isEquippedTamerSkillForWizard[i] = false;

            main.SR.isEquippedWarriorSkillForAngel[i] = false;
            main.SR.isEquippedWizardSkillForAngel[i] = false;
            main.SR.isEquippedAngelSkillForAngel[i] = false;
            main.SR.isEquippedThiefSkillForAngel[i] = false;
            main.SR.isEquippedArcherSkillForAngel[i] = false;
            main.SR.isEquippedTamerSkillForAngel[i] = false;

            main.SR.isEquippedWarriorSkillForThief[i] = false;
            main.SR.isEquippedWizardSkillForThief[i] = false;
            main.SR.isEquippedAngelSkillForThief[i] = false;
            main.SR.isEquippedThiefSkillForThief[i] = false;
            main.SR.isEquippedArcherSkillForThief[i] = false;
            main.SR.isEquippedTamerSkillForThief[i] = false;

            main.SR.isEquippedWarriorSkillForArcher[i] = false;
            main.SR.isEquippedWizardSkillForArcher[i] = false;
            main.SR.isEquippedAngelSkillForArcher[i] = false;
            main.SR.isEquippedThiefSkillForArcher[i] = false;
            main.SR.isEquippedArcherSkillForArcher[i] = false;
            main.SR.isEquippedTamerSkillForArcher[i] = false;

            main.SR.isEquippedWarriorSkillForTamer[i] = false;
            main.SR.isEquippedWizardSkillForTamer[i] = false;
            main.SR.isEquippedAngelSkillForTamer[i] = false;
            main.SR.isEquippedThiefSkillForTamer[i] = false;
            main.SR.isEquippedArcherSkillForTamer[i] = false;
            main.SR.isEquippedTamerSkillForTamer[i] = false;
        }
        //Equipment
        game.equipmentCtrl.dictionaryPointLeft.ChangeValue(0);
        for (int i = 0; i < game.equipmentCtrl.dictionaryUpgrades.Length; i++)
        {
            game.equipmentCtrl.dictionaryUpgrades[i].level.ChangeValue(0);
        }
        for (int i = 0; i < game.equipmentCtrl.globalInformations.Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
            {
                game.equipmentCtrl.globalInformations[i].proficiencies[j].ChangeValue(0);
                game.equipmentCtrl.globalInformations[i].levels[j].ChangeValue(0);
                game.equipmentCtrl.globalInformations[i].levels[j].isMaxedThisRebirth = false;
                //game.equipmentCtrl.globalInformations[i].levels[j].isMaxed = false; MaxEffectは残る
            }
        }
        for (int i = 0; i < main.SR.disassembledEquipmentNums.Length; i++)
        {
            main.SR.disassembledEquipmentNums[i] = 0;
        }
        for (int i = 0; i < main.SR.townMatGainFromdisassemble.Length; i++)
        {
            main.SR.townMatGainFromdisassemble[i] = 0;
        }
        //EnchantSlotのないEQは削除される
        for (int i = 0; i < game.inventoryCtrl.equipmentSlots.Length; i++)
        {
            Equipment equipment = game.inventoryCtrl.equipmentSlots[i].equipment;
            if (equipment.totalOptionNum.Value() < 1)
            {
                //UniqueEQの場合、WAMissionのClear状況によってはReserveされる
                if (equipment.isSetItem && missionMilestoneList[0].isActive)
                    ;
                else
                    equipment.Delete();
            }
        }
        //Resource
        game.resourceCtrl.gold.ChangeValue(0);
        game.resourceCtrl.slimeCoin.ChangeValue(0);
        for (int i = 0; i < game.resourceCtrl.resources.Length; i++)
        {
            game.resourceCtrl.resources[i].ChangeValue(0);
        }
        //Material
        //for (int i = 0; i < game.materialCtrl.materials.Length; i++)
        //{
        //    game.materialCtrl.materials[i].ChangeValue(0);
        //}
        for (int i = 0; i < game.townCtrl.townMaterials.Length; i++)
        {
            game.townCtrl.townMaterials[i].ChangeValue(0);
        }
        //GuildLevel
        game.guildCtrl.exp.ChangeValue(0);
        game.guildCtrl.level.ChangeValue(0);
        game.guildCtrl.abilityPointLeft.ChangeValue(0);
        for (int i = 0; i < game.guildCtrl.abilities.Length; i++)
        {
            game.guildCtrl.abilities[i].level.ChangeValue(0);
        }
        for (int i = 0; i < game.guildCtrl.accomplishGuildLevels.Length; i++)
        {
            game.guildCtrl.accomplishGuildLevels[i].accomplishedTime = 0;
        }
        //Town
        for (int i = 0; i < game.townCtrl.buildings.Length; i++)
        {
            BUILDING building = game.townCtrl.buildings[i];
            building.level.ChangeValue(0);
            bool isReserve = false;
            switch (building.kind)
            {
                case BuildingKind.StatueOfHeroes: isReserve = missionMilestoneList[2].isActive; break;
                case BuildingKind.Cartographer: isReserve = missionMilestoneList[3].isActive; break;
                case BuildingKind.AlchemistsHut: isReserve = missionMilestoneList[4].isActive; break;
                case BuildingKind.Blacksmith: isReserve = missionMilestoneList[5].isActive; break;
                case BuildingKind.Temple: isReserve = missionMilestoneList[6].isActive; break;
                case BuildingKind.Trapper: isReserve = missionMilestoneList[7].isActive; break;
                case BuildingKind.SlimeBank: isReserve = missionMilestoneList[8].isActive; break;
                case BuildingKind.MysticArena: isReserve = missionMilestoneList[9].isActive; break;
                case BuildingKind.ArcaneResearcher: isReserve = missionMilestoneList[10].isActive; break;
                    //case BuildingKind.Tavern:
                    //    break;
                    //case BuildingKind.Dojo:
                    //    break;
                    //case BuildingKind.AdventuringParty:
                    //    break;
            }
            if (!isReserve) building.rank.ChangeValue(0);
            for (int j = 0; j < building.accomplish.Length; j++)
            {
                building.accomplish[j].accomplishedTime = 0;
            }
            //Research Lv/EXPはリセットしない
            //for (int j = 0; j < building.isResearch.Length; j++)
            //{
            //    //building.researchExps[j].ChangeValue(0);
            //    //building.researchLevels[j].ChangeValue(0);
            //    building.isResearch[j] = false;
            //}
        }
        for (int j = 0; j < main.SR.IsBuildingResearchStone.Length; j++)
        {
            main.SR.IsBuildingResearchStone[j] = false;
            main.SR.IsBuildingResearchCrystal[j] = false;
            main.SR.IsBuildingResearchLeaf[j] = false;
        }
        //Upgrade
        for (int i = 0; i < game.upgradeCtrl.upgradeList.Count; i++)
        {
            game.upgradeCtrl.upgradeList[i].level.ChangeValue(0);
            //Queueはリセット
            game.upgradeCtrl.upgradeList[i].queue = 0;
            game.upgradeCtrl.upgradeList[i].isSuperQueued = false;
        }
        //Lab
        for (int i = 0; i < game.catalystCtrl.catalysts.Count; i++)
        {
            Catalyst catalyst = game.catalystCtrl.catalysts[i];
            catalyst.isEquipped = false;
            catalyst.RemoveWater();
            for (int j = 0; j < catalyst.essenceProductionList.Count; j++)
            {
                catalyst.essenceProductionList[j].progress.ChangeValue(0);
            }
            if (missionMilestoneList[1].isActive)
                ;
            else
                catalyst.level.ChangeValue(0);
        }
        //AlchemyPoint, AlchemyUpgradeはリセットしない
        //game.alchemyCtrl.alchemyPoint.ChangeValue(0);
        //for (int i = 0; i < game.alchemyCtrl.alchemyUpgrades.Count; i++)
        //{
        //    game.alchemyCtrl.alchemyUpgrades[i].level.ChangeValue(0);
        //}
        game.alchemyCtrl.mysteriousWaterProgress.ChangeValue(0);
        game.alchemyCtrl.mysteriousWater.ChangeValue(0);
        game.alchemyCtrl.mysteriousWaterExpandedCapNum.ChangeValue(0);
        //Queueはリセット
        for (int i = 0; i < game.potionCtrl.globalInformations.Count; i++)
        {
            game.potionCtrl.globalInformations[i].queue = 0;
            game.potionCtrl.globalInformations[i].isSuperQueued = false;
        }
        //for (int i = 0; i < main.SR.potionLevels.Length; i++)
        //{
        //    //main.SR.potionLevels[i] = 0;
        //    //potionProducted#は残す
        //}
        //for (int i = 0; i < game.essenceCtrl.essences.Length; i++)
        //{
        //    game.essenceCtrl.essences[i].ChangeValue(0);
        //}
        //Shop
        for (int i = 0; i < game.shopCtrl.shopItemList.Count; i++)
        {
            game.shopCtrl.shopItemList[i].purchasedNum.ChangeValue(0);
        }
        game.shopCtrl.timecount = 0;
        //Rebirth
        for (int i = 0; i < game.rebirthCtrl.rebirthList.Count; i++)
        {
            Rebirth rebirth = game.rebirthCtrl.rebirthList[i];
            rebirth.rebirthNum = 0;
            rebirth.rebirthPlayTime = main.allTime;
            rebirth.maxHeroLevel = 0;
            rebirth.ResetRebirthPoint();
            for (int j = 0; j < rebirth.rebirthUpgrades.Length; j++)
            {
                rebirth.rebirthUpgrades[j].level.ChangeValue(0);
            }
            rebirth.accomplish.accomplishedTime = 0;
        }
        //Bestiary
        game.monsterCtrl.CheckPetActiveNum();

        //Challenge
        for (int i = 0; i < main.SR.isClearedChallenge.Length; i++)
        {
            main.SR.isClearedChallenge[i] = false;
            main.SR.isReceivedRewardsChallenge[i] = false;
        }
        for (int i = 0; i < game.challengeCtrl.challengeList.Count; i++)
        {
            game.challengeCtrl.challengeList[i].accomplish.accomplishedTime = 0;
        }

        //Expedition : 各TeamのProgressはリセット。Expedition自体のLvやEXPは継続
        for (int i = 0; i < main.S.expeditionPetSpecies.Length; i++)
        {
            main.S.expeditionPetSpecies[i] = MonsterSpecies.Slime;
            main.S.expeditionPetColors[i] = MonsterColor.Normal;
            main.S.expeditionPetIsSet[i] = false;
        }
        for (int i = 0; i < game.expeditionCtrl.expeditions.Length; i++)
        {
            Expedition expedition = game.expeditionCtrl.expeditions[i];
            expedition.progress.ChangeValue(0);
            expedition.isStarted = false;
            expedition.timeId.ChangeValue(0);
            expedition.movedDistance = 0;
        }

        //WorldMap/Areaのみ。Dungeonはリセットしない
        for (int i = 0; i < game.areaCtrl.areaList.Count; i++)
        {
            AREA area = game.areaCtrl.areaList[i];
            area.level.ChangeValue(0);
            area.prestige.point.ChangeValue(0);
            for (int j = 0; j < area.prestige.upgrades.Count; j++)
            {
                area.prestige.upgrades[j].level.ChangeValue(0);
            }            
        }
        //Areaのみリセット
        for (int i = 0; i < AreaParameter.firstDungeonIdForSave; i++)
        {
            main.SR.areaCompleteNumsSlime[i] = 0;
            main.SR.areaCompleteNumsMagicSlime[i] = 0;
            main.SR.areaCompleteNumsSpider[i] = 0;
            main.SR.areaCompleteNumsBat[i] = 0;
            main.SR.areaCompleteNumsFairy[i] = 0;
            main.SR.areaCompleteNumsFox[i] = 0;
            main.SR.areaCompleteNumsDevilFish[i] = 0;
            main.SR.areaCompleteNumsTreant[i] = 0;
            main.SR.areaCompleteNumsFlameTiger[i] = 0;
            main.SR.areaCompleteNumsUnicorn[i] = 0;
            main.SR.areaBestTimesSlime[i] = 0;
            main.SR.areaBestTimesMagicSlime[i] = 0;
            main.SR.areaBestTimesSpider[i] = 0;
            main.SR.areaBestTimesBat[i] = 0;
            main.SR.areaBestTimesFairy[i] = 0;
            main.SR.areaBestTimesFox[i] = 0;
            main.SR.areaBestTimesDevilFish[i] = 0;
            main.SR.areaBestTimesTreant[i] = 0;
            main.SR.areaBestTimesFlameTiger[i] = 0;
            main.SR.areaBestTimesUnicorn[i] = 0;
            main.SR.areaBestGoldsSlime[i] = 0;
            main.SR.areaBestGoldsMagicSlime[i] = 0;
            main.SR.areaBestGoldsSpider[i] = 0;
            main.SR.areaBestGoldsBat[i] = 0;
            main.SR.areaBestGoldsFairy[i] = 0;
            main.SR.areaBestGoldsFox[i] = 0;
            main.SR.areaBestGoldsDevilFish[i] = 0;
            main.SR.areaBestGoldsTreant[i] = 0;
            main.SR.areaBestGoldsFlameTiger[i] = 0;
            main.SR.areaBestGoldsUnicorn[i] = 0;
            main.SR.areaBestExpsSlime[i] = 0;
            main.SR.areaBestExpsMagicSlime[i] = 0;
            main.SR.areaBestExpsSpider[i] = 0;
            main.SR.areaBestExpsBat[i] = 0;
            main.SR.areaBestExpsFairy[i] = 0;
            main.SR.areaBestExpsFox[i] = 0;
            main.SR.areaBestExpsDevilFish[i] = 0;
            main.SR.areaBestExpsTreant[i] = 0;
            main.SR.areaBestExpsFlameTiger[i] = 0;
            main.SR.areaBestExpsUnicorn[i] = 0;
        }
        //AreaもDungeonもFirstReceiveはリセット
        for (int i = 0; i < main.SR.areaIsReceivedFirstRewardSlime.Length; i++)
        {
            main.SR.areaIsReceivedFirstRewardSlime[i] = false;
            main.SR.areaIsReceivedFirstRewardMagicSlime[i] = false;
            main.SR.areaIsReceivedFirstRewardSpider[i] = false;
            main.SR.areaIsReceivedFirstRewardBat[i] = false;
            main.SR.areaIsReceivedFirstRewardFairy[i] = false;
            main.SR.areaIsReceivedFirstRewardFox[i] = false;
            main.SR.areaIsReceivedFirstRewardDevilFish[i] = false;
            main.SR.areaIsReceivedFirstRewardTreant[i] = false;
            main.SR.areaIsReceivedFirstRewardFlameTiger[i] = false;
            main.SR.areaIsReceivedFirstRewardUnicorn[i] = false;
        }
        //Mission
        for (int i = 0; i < main.SR.isClearedMission.Length; i++)
        {
            main.SR.isClearedMission[i] = false;
        }

        //Battle
        for (int i = 0; i < main.SR.currentAreaKind.Length; i++)
        {
            main.SR.currentAreaKind[i] = AreaKind.SlimeVillage;
            main.SR.currentAreaId[i] = 0;
        }
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].areaBattle.currentWave = 0;
        }

        //QoL系 (Auto-disassemble EQなど）
        game.equipmentCtrl.AdjustAssignedAutoDisassemble();
        for (int i = 0; i < game.autoCtrl.tourArea.lastSimulatedPlaytime.Length; i++)
        {
            game.autoCtrl.tourArea.lastSimulatedPlaytime[i] = 0;
        }
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            main.S.favoriteAreaKinds[i] = AreaKind.SlimeVillage;
            main.S.favoriteAreaIds[i] = 0;
        }

        //Save
        main.SR.isInitialized = false;
    }
}

