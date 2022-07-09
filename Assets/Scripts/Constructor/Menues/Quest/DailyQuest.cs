using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static Parameter;
using System;

public class DailyQuest
{
}
public class DailyQuest_EC1 : DefeatQuest
{
    public DailyQuest_EC1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.EC1;//1000,2500,6000,14000,32000
        //defeatRequredDefeatNum = () => (1000 + (int)dailyQuestRarity * 250) * Math.Pow(2, (int)dailyQuestRarity);
        defeatRequredDefeatNum = () => dailyQuestDefeatNum[(int)dailyQuestRarity];
        rewardEC = () => dailyQuestRewardEC[(int)dailyQuestRarity];
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_EC2 : CaptureQuest
{
    public DailyQuest_EC2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.EC2;//50,120,280,640,1440
        //captureRequiredNum = () => (50 + (int)dailyQuestRarity * 10) * Math.Pow(2, (int)dailyQuestRarity);
        captureRequiredNum = () => dailyQuestCaptureNum[(int)dailyQuestRarity];
        rewardEC = () => dailyQuestRewardEC[(int)dailyQuestRarity];
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_EC3 : DefeatQuest
{
    public DailyQuest_EC3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.EC3;//1000,2500,6000,14000,32000
        //defeatRequredDefeatNum = () => (1000 + (int)dailyQuestRarity * 250) * Math.Pow(2, (int)dailyQuestRarity);
        defeatRequredDefeatNum = () => dailyQuestDefeatNum[(int)dailyQuestRarity];
        rewardEC = () => dailyQuestRewardEC[(int)dailyQuestRarity];
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_EC4 : CaptureQuest
{
    public DailyQuest_EC4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.EC4;//50,120,280,640,1440
        //captureRequiredNum = () => (50 + (int)dailyQuestRarity * 10) * Math.Pow(2, (int)dailyQuestRarity);
        captureRequiredNum = () => dailyQuestCaptureNum[(int)dailyQuestRarity];
        rewardEC = () => dailyQuestRewardEC[(int)dailyQuestRarity];
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}

public class DailyQuest_Cartographer1 : CompleteAreaQuest
{
    public DailyQuest_Cartographer1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.Cartographer1;//100,300,800,2000,4800
        //areaRequredCompletedNum = () => (100 + (int)dailyQuestRarity * 50) * Math.Pow(2, (int)dailyQuestRarity);
        areaRequredCompletedNum = () => dailyQuestAreaClearNum[(int)dailyQuestRarity];
        rewardPortalOrb = () => dailyQuestRewardPortalOrb[(int)dailyQuestRarity] + questCtrl.portalOrbBonusFromDailyQuest.Value();
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_Cartographer2 : CompleteAreaQuest
{
    public DailyQuest_Cartographer2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.Cartographer2;//100,300,800,2000,4800
        //areaRequredCompletedNum = () => (100 + (int)dailyQuestRarity * 50) * Math.Pow(2, (int)dailyQuestRarity);
        areaRequredCompletedNum = () => dailyQuestAreaClearNum[(int)dailyQuestRarity];
        rewardPortalOrb = () => dailyQuestRewardPortalOrb[(int)dailyQuestRarity] + questCtrl.portalOrbBonusFromDailyQuest.Value();
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_Cartographer3 : CompleteAreaQuest
{
    public DailyQuest_Cartographer3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.Cartographer3;//100,300,800,2000,4800
        //areaRequredCompletedNum = () => (100 + (int)dailyQuestRarity * 50) * Math.Pow(2, (int)dailyQuestRarity);
        areaRequredCompletedNum = () => dailyQuestAreaClearNum[(int)dailyQuestRarity];
        rewardPortalOrb = () => dailyQuestRewardPortalOrb[(int)dailyQuestRarity] + questCtrl.portalOrbBonusFromDailyQuest.Value();
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_Cartographer4 : CompleteAreaQuest
{
    public DailyQuest_Cartographer4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.Cartographer4;//100,300,800,2000,4800
        //areaRequredCompletedNum = () => (100 + (int)dailyQuestRarity * 50) * Math.Pow(2, (int)dailyQuestRarity);
        areaRequredCompletedNum = () => dailyQuestAreaClearNum[(int)dailyQuestRarity];
        rewardPortalOrb = () => dailyQuestRewardPortalOrb[(int)dailyQuestRarity] + questCtrl.portalOrbBonusFromDailyQuest.Value();
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class DailyQuest_Cartographer5 : CompleteAreaQuest
{
    public DailyQuest_Cartographer5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Daily;
        kindDaily = QuestKindDaily.Cartographer5;//100,300,800,2000,4800
        //areaRequredCompletedNum = () => (100 + (int)dailyQuestRarity * 50) * Math.Pow(2, (int)dailyQuestRarity);
        areaRequredCompletedNum = () => dailyQuestAreaClearNum[(int)dailyQuestRarity];
        rewardPortalOrb = () => dailyQuestRewardPortalOrb[(int)dailyQuestRarity] + questCtrl.portalOrbBonusFromDailyQuest.Value();
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}

