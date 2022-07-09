using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static Parameter;
using System;

public class GlobalQuest
{
}
public class GlobalQuest_Tutorial1 : QUEST
{
    public GlobalQuest_Tutorial1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.AbilityVIT;
        rewardExp = () => 300;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.statsCtrl.Ability(HeroKind.Warrior, AbilityKind.Vitality).Point() >= 1);
    }
}
public class GlobalQuest_Tutorial2 : QUEST
{
    public GlobalQuest_Tutorial2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.ClearGeneralQuest;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_0, HeroKind.Warrior).isCleared);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.AbilityVIT));
    }
}
public class GlobalQuest_Tutorial3 : QUEST
{
    public GlobalQuest_Tutorial3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.ClearTitleQuest;
        rewardNitro = () => 100;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.questCtrl.Quest(QuestKindTitle.SkillMaster1, HeroKind.Warrior).isCleared);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearGeneralQuest));
    }
}
public class GlobalQuest_Tutorial4 : QUEST
{
    public GlobalQuest_Tutorial4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.UpgradeResource;
        rewardGold = () => 200;
        rewardNitro = () => 200;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 25);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 1).unlockConditions.Add(() => this.isCleared);
    }
}
public class GlobalQuest_Tutorial5 : QUEST
{
    public GlobalQuest_Tutorial5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Equip;
        rewardGold = () => 500;
        rewardNitro = () => 300;
        rewardMaterial.Add(() => MaterialKind.OilOfSlime, () => 5);
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsEquip);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.UpgradeResource));
    }
    bool IsEquip()
    {
        for (int i = 0; i < game.equipmentCtrl.globalInformations.Length; i++)
        {
            if (game.equipmentCtrl.globalInformations[i].isGotOnce) return true;
        }
        return false;
    }
}

public class GlobalQuest_Tutorial6 : QUEST
{
    public GlobalQuest_Tutorial6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Alchemy;
        rewardGold = () => 500;
        rewardNitro = () => 400;
        rewardGuildExp = () => 1000;
        //rewardMaterial.Add(() => MaterialKind.OilOfSlime, () => 10);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.GlobalInfo(PotionKind.MinorHealthPotion).productedNum.value > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Equip));
    }
}

public class GlobalQuest_Tutorial7 : QUEST
{
    public GlobalQuest_Tutorial7(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Guild;
        rewardGold = () => 1000;
        rewardNitro = () => 500;
        rewardGuildExp = () => 1500;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.guildCtrl.Level() >= 5);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class GlobalQuest_Tutorial8 : QUEST
{
    public GlobalQuest_Tutorial8(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Town;
        rewardGold = () => 1500;
        rewardNitro = () => 750;
        rewardGuildExp = () => 2000;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.townCtrl.Building(BuildingKind.Cartographer).Level() >= 5);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Guild));
    }
    public override void RewardAction()
    {
        base.RewardAction();
        game.battleCtrl.blessingCtrl.Blessing(BlessingKind.ExpGain).StartBlessing(30 * 60);
    }
}
public class GlobalQuest_Tutorial9 : QUEST
{
    public GlobalQuest_Tutorial9(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Research;
        rewardGold = () => 2500;
        rewardNitro = () => 1000;
        rewardGuildExp = () => 2500;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.townCtrl.Building(BuildingKind.Cartographer).ResearchLevel(ResourceKind.Leaf) >= 1);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Town));
    }
    public override void RewardAction()
    {
        base.RewardAction();
        game.battleCtrl.blessingCtrl.Blessing(BlessingKind.GoldGain).StartBlessing(30 * 60);
    }
}
public class GlobalQuest_Tutorial10 : QUEST
{
    public GlobalQuest_Tutorial10(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Rebirth;
        rewardGold = () => 5000;
        rewardGuildExp = () => 3000;
        rewardNitro = () => 2000;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.rebirthCtrl.TotalRebirthNum(0) >= 1);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Research));
    }
    public override void RewardAction()
    {
        base.RewardAction();
        game.battleCtrl.blessingCtrl.Blessing(BlessingKind.ExpGain).StartBlessing(30 * 60);
    }
}
public class GlobalQuest_Tutorial11 : QUEST
{
    public GlobalQuest_Tutorial11(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Challenge;
        rewardGold = () => 10000;
        rewardGuildExp = () => 4000;
        rewardNitro = () => 3000;
        rewardPortalOrb = () => 10;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.challengeCtrl.Challenge(ChallengeKind.SlimeKingRaid100).IsClearedOnce());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Rebirth));
    }
}
public class GlobalQuest_Tutorial12 : QUEST
{
    public GlobalQuest_Tutorial12(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.Expedition;
        rewardGold = () => 20000;
        rewardGuildExp = () => 5000;
        rewardNitro = () => 4000;
        rewardPortalOrb = () => 10;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.expeditionCtrl.TotalCompletedNum() > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Challenge));
    }
    public override void SetRewardEffect()
    {
        game.expeditionCtrl.unlockedExpeditionSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 1, () => isCleared));
    }
}

public class GlobalQuest_Tutorial13 : QUEST
{
    public GlobalQuest_Tutorial13(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.WorldAscension;
        //rewardGold = () => 10000;
        //rewardGuildExp = () => 5000;
        rewardNitro = () => 5000;
        rewardPortalOrb = () => 15;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.ascensionCtrl.worldAscensions[0].performedNum > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Expedition));
    }
}
public class GlobalQuest_Tutorial14 : QUEST
{
    public GlobalQuest_Tutorial14(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        kindGlobal = QuestKindGlobal.AreaPrestige;
        //rewardGold = () => 10000;
        //rewardGuildExp = () => 5000;
        rewardNitro = () => 5000;
        rewardPortalOrb = () => 20;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.areaCtrl.Area(AreaKind.SlimeVillage, 0).prestige.upgrades[0].level.value > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.WorldAscension));
    }
}

public class GlobalQuest_Upgrade1 : QUEST
{
    public GlobalQuest_Upgrade1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade1;
        rewardGold = () => 1000;
        //rewardGuildExp = () => 2000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 50);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.UpgradeResource));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade2 : QUEST
{
    public GlobalQuest_Upgrade2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade2;
        rewardGold = () => 2000;
        //rewardGuildExp = () => 3000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 100);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade1));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade3 : QUEST
{
    public GlobalQuest_Upgrade3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade3;
        rewardGold = () => 3000;
        //rewardGuildExp = () => 4000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 150);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade2));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade4 : QUEST
{
    public GlobalQuest_Upgrade4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade4;
        rewardGold = () => 5000;
        //rewardGuildExp = () => 5000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 200);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade3));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade5 : QUEST
{
    public GlobalQuest_Upgrade5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade5;
        rewardGold = () => 10000;
        //rewardGuildExp = () => 10000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 250);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade4));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade6 : QUEST
{
    public GlobalQuest_Upgrade6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade6;
        rewardGold = () => 25000;
        //rewardGuildExp = () => 20000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 300);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade5));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade7 : QUEST
{
    public GlobalQuest_Upgrade7(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade7;
        rewardGold = () => 50000;
        //rewardGuildExp = () => 30000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 400);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade6));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}
public class GlobalQuest_Upgrade8 : QUEST
{
    public GlobalQuest_Upgrade8(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Upgrade;
        kindGlobal = QuestKindGlobal.Upgrade8;
        rewardGold = () => 100000;
        //rewardGuildExp = () => 50000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.upgradeCtrl.Upgrade(UpgradeKind.Resource, 0).level.value >= 500);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Upgrade7));
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5, () => isCleared));
    }
}

public class GlobalQuest_Nitro1 : QUEST
{
    public GlobalQuest_Nitro1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro1;
        rewardNitro = () => 500;
        //rewardGuildExp = () => 500;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class GlobalQuest_Nitro2 : QUEST
{
    public GlobalQuest_Nitro2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro2;
        rewardNitro = () => 1000;
        //rewardGuildExp = () => 1000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 5000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro1));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 1000, () => isCleared));
    }
}
public class GlobalQuest_Nitro3 : QUEST
{
    public GlobalQuest_Nitro3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro3;
        rewardNitro = () => 2000;
        //rewardGuildExp = () => 2000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 30000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro2));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 2000, () => isCleared));
    }
}
public class GlobalQuest_Nitro4 : QUEST
{
    public GlobalQuest_Nitro4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro4;
        rewardNitro = () => 3000;
        //rewardGuildExp = () => 3000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 150000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro3));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 3000, () => isCleared));
    }
}
public class GlobalQuest_Nitro5 : QUEST
{
    public GlobalQuest_Nitro5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro5;
        rewardNitro = () => 4000;
        //rewardGuildExp = () => 4000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 500000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro4));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 4000, () => isCleared));
    }
}
public class GlobalQuest_Nitro6 : QUEST
{
    public GlobalQuest_Nitro6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro6;
        rewardNitro = () => 5000;
        //rewardGuildExp = () => 5000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 1000000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro5));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 5000, () => isCleared));
    }
}
public class GlobalQuest_Nitro7 : QUEST
{
    public GlobalQuest_Nitro7(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro7;
        rewardNitro = () => 6000;
        //rewardGuildExp = () => 6000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 5000000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro6));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 6000, () => isCleared));
    }
}
public class GlobalQuest_Nitro8 : QUEST
{
    public GlobalQuest_Nitro8(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Nitro;
        kindGlobal = QuestKindGlobal.Nitro8;
        rewardNitro = () => 7000;
        //rewardGuildExp = () => 7000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.S.nitroConsumed >= 10000000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Nitro7));
        game.nitroCtrl.nitroCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 7000, () => isCleared));
    }
}

public class GlobalQuest_Capture1 : QUEST
{
    public GlobalQuest_Capture1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Capture;
        kindGlobal = QuestKindGlobal.Capture1;
        //rewardGuildExp = () => 1000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.monsterCtrl.CapturedNum() >= 10);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Guild));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.10d, () => isCleared));
    }
}
public class GlobalQuest_Capture2 : QUEST
{
    public GlobalQuest_Capture2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Capture;
        kindGlobal = QuestKindGlobal.Capture2;
        //rewardGuildExp = () => 5000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.monsterCtrl.CapturedNum() >= 1000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Capture1));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.20d, () => isCleared));
    }
}
public class GlobalQuest_Capture3 : QUEST
{
    public GlobalQuest_Capture3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Capture;
        kindGlobal = QuestKindGlobal.Capture3;
        //rewardGuildExp = () => 10000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.monsterCtrl.CapturedNum() >= 10000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Capture2));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.30d, () => isCleared));
    }
}
public class GlobalQuest_Capture4 : QUEST
{
    public GlobalQuest_Capture4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Capture;
        kindGlobal = QuestKindGlobal.Capture4;
        //rewardGuildExp = () => 30000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.monsterCtrl.CapturedNum() >= 100000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Capture3));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.40d, () => isCleared));
    }
}
public class GlobalQuest_Capture5 : QUEST
{
    public GlobalQuest_Capture5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Capture;
        kindGlobal = QuestKindGlobal.Capture5;
        //rewardGuildExp = () => 50000;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.monsterCtrl.CapturedNum() >= 1000000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Capture4));
        game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.50d, () => isCleared));
    }
}


public class GlobalQuest_Alchemy1 : QUEST
{
    public GlobalQuest_Alchemy1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy1;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 100);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Town));
        game.catalystCtrl.criticalChanceMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.25d, () => isCleared));
    }
}
public class GlobalQuest_Alchemy2 : QUEST
{
    public GlobalQuest_Alchemy2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy2;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 200);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy1));
        game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 100, () => isCleared));
    }
}
public class GlobalQuest_Alchemy3 : BringQuest
{
    public GlobalQuest_Alchemy3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy3;
        bringRequiredMaterials.Add(MaterialKind.SlimeBall, 10);
        bringRequiredMaterials.Add(MaterialKind.ManaSeed, 10);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 300);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy2));
        game.potionCtrl.potionMaxLevel.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 25, () => isCleared));
    }
}
public class GlobalQuest_Alchemy4 : BringQuest
{
    public GlobalQuest_Alchemy4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy4;
        bringRequiredMaterials.Add(MaterialKind.UnmeltingIce, 10);
        bringRequiredMaterials.Add(MaterialKind.EternalFlame, 10);
        bringRequiredMaterials.Add(MaterialKind.AncientBattery, 10);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 500);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy3));
        game.catalystCtrl.catalystLevelCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 25, () => isCleared));
    }
}
public class GlobalQuest_Alchemy5 : QUEST
{
    public GlobalQuest_Alchemy5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy5;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 750);
        clearConditions.Add(() => game.alchemyCtrl.mysteriousWaterCap.Value() >= 300);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy4));
        game.catalystCtrl.catalystCostReduction.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.25, () => isCleared));
    }
}
public class GlobalQuest_Alchemy6 : BringQuest
{
    public GlobalQuest_Alchemy6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy6;
        bringRequiredMaterials.Add(MaterialKind.Ectoplasm, 30);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 1000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy5));
        game.catalystCtrl.criticalChanceMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Mul, () => 0.25d, () => isCleared));
        game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 250, () => isCleared));
    }
}
public class GlobalQuest_Alchemy7 : BringQuest
{
    public GlobalQuest_Alchemy7(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy7;
        bringRequiredMaterials.Add(MaterialKind.Stardust, 100);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 1500);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy6));
        game.potionCtrl.potionMaxLevel.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 25, () => isCleared));
        game.catalystCtrl.catalystLevelCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 25, () => isCleared));
    }
}
public class GlobalQuest_Alchemy8 : BringQuest
{
    public GlobalQuest_Alchemy8(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Global;
        globalQuestType = GlobalQuestType.Alchemy;
        kindGlobal = QuestKindGlobal.Alchemy8;
        bringRequiredMaterials.Add(MaterialKind.VoidEgg, 1000);
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.potionCtrl.TotalPotionLevel() >= 2000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy7));
        game.alchemyCtrl.alchemyPointGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 1, () => isCleared));
        game.catalystCtrl.catalystCostReduction.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.25, () => isCleared));
    }
}
