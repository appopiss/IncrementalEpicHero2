using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static UsefulStatic;
using static Main;

public class SaveDeclare :MonoBehaviour {

	// Use this for initialization
	void Awake () {

		//InAppPurchase
		InitializeArray(ref main.S.inAppPurchasedNum, Enum.GetNames(typeof(InAppPurchaseKind)).Length);

		//Tutorial
		//InitializeArray(ref main.S.isDoneTutorialArrows, Mathf.Max(20, Enum.GetNames(typeof(TutorialArrowKind)).Length));

		//Setting
		InitializeArray(ref main.S.isToggleOn, Mathf.Max(20, Enum.GetNames(typeof(ToggleKind)).Length));
		InitializeArray(ref main.S.isReceivedBonusCodes, Mathf.Max(20, Enum.GetNames(typeof(BonusCodeKind)).Length));
		//Resource
		InitializeArray(ref main.SR.resources, Enum.GetNames(typeof(ResourceKind)).Length);
		InitializeArray(ref main.SR.materials, Enum.GetNames(typeof(MaterialKind)).Length);
		//Achievement
		InitializeArray(ref main.S.isGotAchievements, Enum.GetNames(typeof(AchievementKind)).Length);
		InitializeArray(ref main.S.isGotAchievementRewards, Enum.GetNames(typeof(AchievementKind)).Length);
		InitializeArray(ref main.S.achievementPlaytimes, Enum.GetNames(typeof(AchievementKind)).Length);

		//統計
		InitializeArray(ref main.S.playtimes, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.playtimesRealTime, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.playtimes, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.playtimesRealTime, Enum.GetNames(typeof(HeroKind)).Length);

		InitializeArray(ref main.S.totalMovedDistance, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.movedDistance, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.maxHeroLevelReached, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.physicalTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.fireTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.iceTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.thunderTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.lightTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.darkTriggeredNum, Enum.GetNames(typeof(HeroKind)).Length);

		//統計（Rebirthでリセット）
		InitializeArray(ref main.SR.movedDistance, Enum.GetNames(typeof(HeroKind)).Length);
        //統計（World Ascensionでリセット）
        InitializeArray(ref main.SR.rebirthMaxHeroLevels, Enum.GetNames(typeof(HeroKind)).Length);


		InitializeArray(ref main.S.warriorSkillTriggeredNum, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.S.wizardSkillTriggeredNum, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.S.angelSkillTriggeredNum, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.S.thiefSkillTriggeredNum, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.S.archerSkillTriggeredNum, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.S.tamerSkillTriggeredNum, Enum.GetNames(typeof(SkillKindTamer)).Length);

		//Ability
		InitializeArray(ref main.SR.heroLevel, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.heroExp, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPoints, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPointsVitality, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPointsStrength, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPointsIntelligence, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPointsAgility, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.abilityPointsLuck, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.combatRangeId, Enum.GetNames(typeof(HeroKind)).Length);

		//Upgrade
		InitializeArray(ref main.SR.upgradeLevelsResource, Parameter.resourceUpgradeTier);
		InitializeArray(ref main.SR.upgradeLevelsBasicStats, Enum.GetNames(typeof(BasicStatsKind)).Length);
		InitializeArray(ref main.SR.upgradeLevelsGoldCap, Enum.GetNames(typeof(ResourceKind)).Length);
		InitializeArray(ref main.SR.upgradeLevelsSlimebank, Enum.GetNames(typeof(SlimeBankUpgradeKind)).Length);
		InitializeArray(ref main.S.upgradeQueuesResource, Parameter.resourceUpgradeTier);
		InitializeArray(ref main.S.upgradeQueuesBasicStats, Enum.GetNames(typeof(BasicStatsKind)).Length);
		InitializeArray(ref main.S.upgradeQueuesGoldCap, Enum.GetNames(typeof(ResourceKind)).Length);
		InitializeArray(ref main.S.upgradeQueuesSlimebank, Enum.GetNames(typeof(SlimeBankUpgradeKind)).Length);
		InitializeArray(ref main.S.upgradeIsSuperQueuesResource, Parameter.resourceUpgradeTier);
		InitializeArray(ref main.S.upgradeIsSuperQueuesBasicStats, Enum.GetNames(typeof(BasicStatsKind)).Length);
		InitializeArray(ref main.S.upgradeIsSuperQueuesGoldCap, Enum.GetNames(typeof(ResourceKind)).Length);
		InitializeArray(ref main.S.upgradeIsSuperQueuesSlimebank, Enum.GetNames(typeof(SlimeBankUpgradeKind)).Length);

		//Battle
		InitializeArray(ref main.SR.currentAreaKind, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.currentAreaId, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.isActiveBattle, Enum.GetNames(typeof(HeroKind)).Length);

        //Monster
		InitializeArray(ref main.SR.monsterDefeatedNumsSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsMagicSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsSpider, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsBat, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsFairy, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsFox, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsDevilFish, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsTreant, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsFlameTiger, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsUnicorn, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsMimic, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterDefeatedNumsChallenge, Enum.GetNames(typeof(ChallengeMonsterKind)).Length * 10);

		InitializeArray(ref main.SR.monsterMutantDefeatedNumsSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsMagicSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsSpider, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsBat, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsFairy, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsFox, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsDevilFish, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsTreant, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsFlameTiger, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsUnicorn, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsMimic, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantDefeatedNumsChallenge, Enum.GetNames(typeof(ChallengeMonsterKind)).Length * 10);

		InitializeArray(ref main.SR.monsterCapturedNumsSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsMagicSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsSpider, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsBat, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsFairy, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsFox, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsDevilFish, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsTreant, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsFlameTiger, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsUnicorn, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsMimic, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterCapturedNumsChallenge, Enum.GetNames(typeof(ChallengeMonsterKind)).Length * 10);

		InitializeArray(ref main.SR.monsterMutantCapturedNumsSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsMagicSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsSpider, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsBat, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsFairy, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsFox, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsDevilFish, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsTreant, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsFlameTiger, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsUnicorn, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsMimic, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.monsterMutantCapturedNumsChallenge, Enum.GetNames(typeof(ChallengeMonsterKind)).Length * 10);

		//MonsterPet
		InitializeArray(ref main.S.monsterPetIsActives, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetRanks, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetLevels, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetExps, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetLoyalty, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetLoyaltyExp, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.S.monsterPetTamingPoints, Enum.GetNames(typeof(MonsterSpecies)).Length * 10 + Enum.GetNames(typeof(ChallengeMonsterKind)).Length);
		InitializeArray(ref main.SR.summonSpecies, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.summonColor, Enum.GetNames(typeof(HeroKind)).Length * 10);
		InitializeArray(ref main.SR.isSetSummonPet, Enum.GetNames(typeof(HeroKind)).Length * 10);

		//Expedition
		InitializeArray(ref main.S.isStartedExpedition, 10);
		InitializeArray(ref main.S.expeditionProgress, 10);
		InitializeArray(ref main.S.expeditionTimeId, 10);
		InitializeArray(ref main.S.expeditionMovedDistance, 10);
		InitializeArray(ref main.S.expeditionPetSpecies, 10 * 5);
		InitializeArray(ref main.S.expeditionPetColors, 10 * 5);
		InitializeArray(ref main.S.expeditionPetIsSet, 10 * 5);
		InitializeArray(ref main.S.expeditionKinds, 10 * 5);
		InitializeArray(ref main.S.expeditionLevels, Enum.GetNames(typeof(ExpeditionKind)).Length);
		InitializeArray(ref main.S.expeditionExps, Enum.GetNames(typeof(ExpeditionKind)).Length);
		InitializeArray(ref main.S.expeditionCompletedNums, Enum.GetNames(typeof(ExpeditionKind)).Length);
		InitializeArray(ref main.S.expeditionTimes, Enum.GetNames(typeof(ExpeditionKind)).Length);
		//InitializeArray(ref main.SR.isSummonMonsterPetsSlime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsMagicslime, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsSpider, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsBat, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsFairy, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsFox, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsDevilfish, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsTreant, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsFlametiger, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsUnicorn, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsMimic, Enum.GetNames(typeof(HeroKind)).Length * 10);
		//InitializeArray(ref main.SR.isSummonMonsterPetsChallenge, Enum.GetNames(typeof(ChallengeMonsterKind)).Length * 10);

		//Area
		InitializeArray(ref main.S.swarmBestScores, Enum.GetNames(typeof(SwarmRarity)).Length);

		InitializeArray(ref main.SR.areaPrestigePointsSlime, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsMagicSlime, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsSpider, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsBat, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsFairy, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsFox, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsDevilFish, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsTreant, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsFlameTiger, AreaParameter.firstAreaIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigePointsUnicorn, AreaParameter.firstAreaIdForSave * 2);

		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaPrestigeUpgradeLevelsUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		//Mission
		InitializeArray(ref main.SR.isClearedMission, AreaParameter.firstMissionIdForSave * AreaParameter.firstAreaIdForSave * AreaParameter.firstLevelIdForSave * Enum.GetNames(typeof(AreaKind)).Length);
		InitializeArray(ref main.S.isClearedMission, AreaParameter.firstMissionIdForSave * AreaParameter.firstAreaIdForSave * AreaParameter.firstLevelIdForSave * Enum.GetNames(typeof(AreaKind)).Length);

		InitializeArray(ref main.SR.currentAreaLevelsSlime, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsMagicSlime, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsSpider, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsBat, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsFairy, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsFox, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsDevilFish, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsTreant, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsFlameTiger, AreaParameter.firstLevelIdForSave * 2);
		InitializeArray(ref main.SR.currentAreaLevelsUnicorn, AreaParameter.firstLevelIdForSave * 2);

		InitializeArray(ref main.SR.areaIsReceivedFirstRewardSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaIsReceivedFirstRewardUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		InitializeArray(ref main.SR.areaCompleteNumsSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaCompleteNumsUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		InitializeArray(ref main.SR.areaBestTimesSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestTimesUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		InitializeArray(ref main.SR.areaBestGoldsSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestGoldsUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		InitializeArray(ref main.SR.areaBestExpsSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsMagicSlime, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsSpider, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsBat, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsFairy, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsFox, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsDevilFish, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsTreant, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsFlameTiger, AreaParameter.firstDungeonIdForSave * 2);
		InitializeArray(ref main.SR.areaBestExpsUnicorn, AreaParameter.firstDungeonIdForSave * 2);

		//Challenge
		InitializeArray(ref main.SR.isClearedChallenge, Enum.GetNames(typeof(ChallengeKind)).Length * 10);
		InitializeArray(ref main.SR.isReceivedRewardsChallenge, Enum.GetNames(typeof(ChallengeKind)).Length * 10);
		InitializeArray(ref main.SR.accomplishedFirstTimesChallenge, Enum.GetNames(typeof(ChallengeKind)).Length);
		InitializeArray(ref main.SR.accomplishedTimesChallenge, Enum.GetNames(typeof(ChallengeKind)).Length);
		InitializeArray(ref main.SR.accomplishedBestTimesChallenge, Enum.GetNames(typeof(ChallengeKind)).Length);

		//Equipment,Inventory
		InitializeArray(ref main.S.disassembledEquipmentNums, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.disassembledEquipmentNums, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.townMatGainFromdisassemble, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.townMatGainFromdisassemble, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsAutoDisassemble, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsGotOnce, Enum.GetNames(typeof(EquipmentKind)).Length);

		InitializeArray(ref main.SR.equipmentLevelsWarrior, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentLevelsWizard, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentLevelsAngel, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentLevelsThief, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentLevelsArcher, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentLevelsTamer, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyWarrior, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyWizard, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyAngel, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyThief, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyArcher, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentProficiencyTamer, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedWarrior, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedWizard, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedAngel, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedThief, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedArcher, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.SR.equipmentIsMaxedTamer, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedWarrior, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedWizard, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedAngel, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedThief, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedArcher, Enum.GetNames(typeof(EquipmentKind)).Length);
		InitializeArray(ref main.S.equipmentIsMaxedTamer, Enum.GetNames(typeof(EquipmentKind)).Length);
		//EquipOption

		InitializeArray(ref main.SR.equipmentId, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipmentKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipmentOptionNums, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipmentIsLocked, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment1stOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment2ndOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment3rdOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment4thOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment5thOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment6thOptionEffectKinds, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment1stOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment2ndOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment3rdOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment4thOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment5thOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment6thOptionLevels, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment1stOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment2ndOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment3rdOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment4thOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment5thOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		InitializeArray(ref main.SR.equipment6thOptionEffectValues, InventoryParameter.allEquipmentSlotId);
		//Enchant
		InitializeArray(ref main.SR.enchantId, InventoryParameter.enchantSlotId);
		InitializeArray(ref main.SR.enchantKinds, InventoryParameter.enchantSlotId);
		InitializeArray(ref main.SR.enchantEffectKinds, InventoryParameter.enchantSlotId);
		InitializeArray(ref main.SR.enchantEffectLevels, InventoryParameter.enchantSlotId);
		InitializeArray(ref main.SR.enchantProficiencyTimesec, InventoryParameter.enchantSlotId);
		//Dictionary
		InitializeArray(ref main.SR.dictionaryUpgradeLevels, Enum.GetNames(typeof(DictionaryUpgradeKind)).Length);
		//Potion
		InitializeArray(ref main.S.potionQueues, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.S.potionIsSuperQueues, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.SR.potionId, InventoryParameter.allPotionSlotId);
		InitializeArray(ref main.SR.potionKinds, InventoryParameter.allPotionSlotId);
		InitializeArray(ref main.SR.potionStackNums, InventoryParameter.allPotionSlotId);
		InitializeArray(ref main.SR.potionLevels, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.SR.potionProductedNums, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.SR.potionDisassembledNums, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.SR.alchemyUpgradeLevels, Enum.GetNames(typeof(AlchemyUpgradeKind)).Length);
		//Catalyst
		InitializeArray(ref main.SR.catalystLevels, Enum.GetNames(typeof(CatalystKind)).Length);
		InitializeArray(ref main.SR.isEquippedCatarysts, Enum.GetNames(typeof(CatalystKind)).Length);
		InitializeArray(ref main.SR.essenceWaterPerSecs, Enum.GetNames(typeof(EssenceKind)).Length);
		InitializeArray(ref main.SR.essenceProgresses, Enum.GetNames(typeof(EssenceKind)).Length);
		InitializeArray(ref main.SR.essences, Enum.GetNames(typeof(EssenceKind)).Length);
		//Craft
		InitializeArray(ref main.SR.craftEnchantScrollLevels, Enum.GetNames(typeof(EquipmentEffectKind)).Length);
		//Skill
		InitializeArray(ref main.SR.currentStanceId, Enum.GetNames(typeof(HeroKind)).Length);

		InitializeArray(ref main.SR.warriorSkillLevel, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.wizardSkillLevel, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.angelSkillLevel, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.thiefSkillLevel, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.archerSkillLevel, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.tamerSkillLevel, Enum.GetNames(typeof(SkillKindTamer)).Length);
		InitializeArray(ref main.SR.warriorMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.wizardMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.angelMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.thiefMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.archerMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.tamerMaxReachedSkillLevel, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.warriorSkillRank, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.wizardSkillRank, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.angelSkillRank, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.thiefSkillRank, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.archerSkillRank, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.tamerSkillRank, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.warriorSkillProficiency, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.wizardSkillProficiency, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.angelSkillProficiency, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.thiefSkillProficiency, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.archerSkillProficiency, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.tamerSkillProficiency, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForWarrior, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForWarrior, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForWarrior, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForWarrior, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForWarrior, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForWarrior, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForWizard, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForWizard, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForWizard, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForWizard, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForWizard, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForWizard, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForAngel, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForAngel, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForAngel, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForAngel, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForAngel, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForAngel, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForThief, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForThief, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForThief, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForThief, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForThief, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForThief, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForArcher, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForArcher, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForArcher, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForArcher, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForArcher, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForArcher, Enum.GetNames(typeof(SkillKindTamer)).Length);

		InitializeArray(ref main.SR.isEquippedWarriorSkillForTamer, Enum.GetNames(typeof(SkillKindWarrior)).Length);
		InitializeArray(ref main.SR.isEquippedWizardSkillForTamer, Enum.GetNames(typeof(SkillKindWizard)).Length);
		InitializeArray(ref main.SR.isEquippedAngelSkillForTamer, Enum.GetNames(typeof(SkillKindAngel)).Length);
		InitializeArray(ref main.SR.isEquippedThiefSkillForTamer, Enum.GetNames(typeof(SkillKindThief)).Length);
		InitializeArray(ref main.SR.isEquippedArcherSkillForTamer, Enum.GetNames(typeof(SkillKindArcher)).Length);
		InitializeArray(ref main.SR.isEquippedTamerSkillForTamer, Enum.GetNames(typeof(SkillKindTamer)).Length);

		//Town
		InitializeArray(ref main.SR.townMaterials, Enum.GetNames(typeof(TownMaterialKind)).Length);
		InitializeArray(ref main.SR.buildingLevels, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingRanks, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchLevelsStone, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchLevelsCrystal, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchLevelsLeaf, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchExpsStone, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchExpsCrystal, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.buildingResearchExpsLeaf, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.IsBuildingResearchStone, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.IsBuildingResearchCrystal, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.IsBuildingResearchLeaf, Enum.GetNames(typeof(BuildingKind)).Length);
		InitializeArray(ref main.SR.accomplishedFirstTimesBuilding, Enum.GetNames(typeof(BuildingKind)).Length * 10);//10は、本当は5(maxBuildingRank)でOK
		InitializeArray(ref main.SR.accomplishedTimesBuilding, Enum.GetNames(typeof(BuildingKind)).Length * 10);//10は、本当は5(maxBuildingRank)でOK
		InitializeArray(ref main.SR.accomplishedBestTimesBuilding, Enum.GetNames(typeof(BuildingKind)).Length * 10);

		//Shop
		InitializeArray(ref main.SR.purchasedNumMaterials, Enum.GetNames(typeof(MaterialKind)).Length);
		InitializeArray(ref main.SR.purchasedNumTraps, Enum.GetNames(typeof(PotionKind)).Length);
		InitializeArray(ref main.SR.purchasedNumTownMaterials, Enum.GetNames(typeof(TownMaterialKind)).Length);
		InitializeArray(ref main.SR.isAutoBuyBlessings, Enum.GetNames(typeof(BlessingKind)).Length);

		//Guild
		InitializeArray(ref main.SR.guildAbilityLevels, Enum.GetNames(typeof(GuildAbilityKind)).Length);
		InitializeArray(ref main.SR.accomplishedFirstTimesGuildLevel, (int)GuildParameter.maxGuildLevel + 1);
		InitializeArray(ref main.SR.accomplishedTimesGuildLevel, (int)GuildParameter.maxGuildLevel + 1);
		InitializeArray(ref main.SR.accomplishedBestTimesGuildLevel, (int)GuildParameter.maxGuildLevel + 1);

		//Quest
		InitializeArray(ref main.S.dailyQuestRarities, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.dailyQuestMonsterSpecies, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.dailyQuestAreaKind, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.dailyQuestAreaId, Enum.GetNames(typeof(QuestKindDaily)).Length);

		InitializeArray(ref main.S.isClearedQuestsDaily, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.isAcceptedQuestsDaily, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.initDefeatedNumQuestsDaily, Enum.GetNames(typeof(QuestKindDaily)).Length);
		InitializeArray(ref main.S.initCompletedAreaNumQuestsDaily, Enum.GetNames(typeof(QuestKindDaily)).Length);

		InitializeArray(ref main.S.totalGeneralQuestClearedNums, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.totalGeneralQuestClearedNums, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.generalQuestClearNumsPerClass, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.maxReachedGeneralQuestClearedNums, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestWarrior, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestWizard, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestAngel, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestThief, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestArcher, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.S.isFavoriteQuestTamer, Enum.GetNames(typeof(QuestKindGeneral)).Length);


		InitializeArray(ref main.SR.isClearedQuestGeneralOnce, Enum.GetNames(typeof(QuestKindGeneral)).Length);


		InitializeArray(ref main.SR.isClearedQuestsGlobal, Enum.GetNames(typeof(QuestKindGlobal)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleWarrior, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleWizard, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleAngel, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleThief, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleArcher, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsTitleTamer, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralWarrior, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralWizard, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralAngel, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralThief, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralArcher, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isClearedQuestsGeneralTamer, Enum.GetNames(typeof(QuestKindGeneral)).Length);

		InitializeArray(ref main.SR.isAcceptedQuestsGlobal, Enum.GetNames(typeof(QuestKindGlobal)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleWarrior, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleWizard, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleAngel, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleThief, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleArcher, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsTitleTamer, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralWarrior, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralWizard, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralAngel, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralThief, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralArcher, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.isAcceptedQuestsGeneralTamer, Enum.GetNames(typeof(QuestKindGeneral)).Length);

		InitializeArray(ref main.SR.initDefeatedNumQuestsGlobal, Enum.GetNames(typeof(QuestKindGlobal)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleWarrior, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleWizard, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleAngel, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleThief, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleArcher, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsTitleTamer, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralWarrior, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralWizard, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralAngel, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralThief, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralArcher, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initDefeatedNumQuestsGeneralTamer, Enum.GetNames(typeof(QuestKindGeneral)).Length);

		//InitializeArray(ref main.SR.initCompletedAreaNumQuestsGlobal, Enum.GetNames(typeof(QuestKindGlobal)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleWarrior, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleWizard, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleAngel, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleThief, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleArcher, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsTitleTamer, Enum.GetNames(typeof(QuestKindTitle)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralWarrior, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralWizard, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralAngel, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralThief, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralArcher, Enum.GetNames(typeof(QuestKindGeneral)).Length);
		InitializeArray(ref main.SR.initCompletedAreaNumQuestsGeneralTamer, Enum.GetNames(typeof(QuestKindGeneral)).Length);

		InitializeArray(ref main.SR.initMovedDistanceQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);

        InitializeArray(ref main.SR.initPhysicalSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.initFireSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.initIceSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.initThunderSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.initDarkSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.SR.initLightSkillTriggeredNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);

		InitializeArray(ref main.SR.survivalNumQuestTitle, Enum.GetNames(typeof(HeroKind)).Length);

		//Rebirth
		InitializeArray(ref main.SR.rebirthNumsWarrior, 10);
		InitializeArray(ref main.SR.rebirthNumsWizard, 10);
		InitializeArray(ref main.SR.rebirthNumsAngel, 10);
		InitializeArray(ref main.SR.rebirthNumsThief, 10);
		InitializeArray(ref main.SR.rebirthNumsArcher, 10);
		InitializeArray(ref main.SR.rebirthNumsTamer, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesWarrior, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesWizard, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesAngel, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesThief, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesArcher, 10);
		InitializeArray(ref main.SR.rebirthPlayTimesTamer, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsWarrior, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsWizard, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsAngel, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsThief, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsArcher, 10);
		InitializeArray(ref main.SR.rebirthMaxHeroLevelsTamer, 10);
		InitializeArray(ref main.SR.rebirthPointsWarrior, 10);
		InitializeArray(ref main.SR.rebirthPointsWizard, 10);
		InitializeArray(ref main.SR.rebirthPointsAngel, 10);
		InitializeArray(ref main.SR.rebirthPointsThief, 10);
		InitializeArray(ref main.SR.rebirthPointsArcher, 10);
		InitializeArray(ref main.SR.rebirthPointsTamer, 10);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsWarrior, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsWizard, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsAngel, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsThief, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsArcher, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.SR.rebirthUpgradeLevelsTamer, Enum.GetNames(typeof(RebirthUpgradeKind)).Length);
		InitializeArray(ref main.S.accomplishedFirstTimesRebirth, 10 * 10);
		InitializeArray(ref main.S.accomplishedTimesRebirth, 10 * 10);
		InitializeArray(ref main.S.accomplishedBestTimesRebirth, 10 * 10);
		InitializeArray(ref main.S.bestRebirthPlaytimes, 10 * 10);
		InitializeArray(ref main.S.totalRebirthNums, 10 * 10);

		InitializeArray(ref main.S.isAutoRebirthOns, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoRebirthPoints, 10 * 10);
		InitializeArray(ref main.S.autoRebirthLevels, 10 * 10);
		InitializeArray(ref main.S.autoRebirthTiers, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.favoriteAreaKinds, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.favoriteAreaIds, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.isBestExpSecAreas, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.isAutoAbilityPointPresets, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointPresetsVIT, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointPresetsSTR, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointPresetsINT, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointPresetsAGI, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointPresetsLUK, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointMaxVIT, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointMaxSTR, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointMaxINT, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointMaxAGI, Enum.GetNames(typeof(HeroKind)).Length);
		InitializeArray(ref main.S.autoAbilityPointMaxLUK, Enum.GetNames(typeof(HeroKind)).Length);

		//WorldAscension
		InitializeArray(ref main.S.ascensionNum, 3);
		InitializeArray(ref main.S.ascensionPlayTime, 3);
		InitializeArray(ref main.S.ascensionPoints, 3);
		InitializeArray(ref main.S.isGotRewardWAAccomplishments, 100);
		InitializeArray(ref main.S.worldAscensionUpgradeLevels, Enum.GetNames(typeof(AscensionUpgradeKind)).Length);
		InitializeArray(ref main.S.ascensionMilestoneLevelReached, Enum.GetNames(typeof(WorldAscensionMiletoneKind)).Length);
		InitializeArray(ref main.S.bestAscensionPlayTime, 3);
		InitializeArray(ref main.S.accomplishedFirstTimeAscension, 3);
		InitializeArray(ref main.S.accomplishedTimeAscension, 3);
		InitializeArray(ref main.S.accomplishedBestTimeAscension, 3);
		//EpicStore
		InitializeArray(ref main.S.epicStorePurchasedNum, Enum.GetNames(typeof(EpicStoreKind)).Length);
	}

	// Use this for initialization
	void Start () {

	}
}
