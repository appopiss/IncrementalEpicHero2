using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static Parameter;
using System;

//重要：クエストを追加するときは、リリース後は必ずenumの最後・配列の最後に追加する。途中に追加してはいけない
public class QuestController
{
    public double[] DailyQuestRarityChance()
    {
        if (game.epicStoreCtrl.Item(EpicStoreKind.DailyQuestRarity).IsPurchased(2))
            return dailyQuestRarityChance2;
        if (game.epicStoreCtrl.Item(EpicStoreKind.DailyQuestRarity).IsPurchased(1))
            return dailyQuestRarityChance1;
        return dailyQuestRarityChance0;
    }

    public QuestController()
    {
        for (int i = 0; i < acceptableNum.Length; i++)
        {
            acceptableNum[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 2));
        }
        portalOrbBonusFromDailyQuest = new Multiplier();
        for (int i = 0; i < generalQuestClearGain.Length; i++)
        {
            generalQuestClearGain[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        }

        globalQuestListTutorial.Add(new GlobalQuest_Tutorial1(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial2(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial3(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial4(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial5(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial6(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial7(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial8(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial9(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial10(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial11(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial12(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial13(this, HeroKind.Warrior));
        globalQuestListTutorial.Add(new GlobalQuest_Tutorial14(this, HeroKind.Warrior));
        globalQuestList.AddRange(globalQuestListTutorial);
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade1(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade2(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade3(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade4(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade5(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade6(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade7(this, HeroKind.Warrior));
        globalQuestListUpgrade.Add(new GlobalQuest_Upgrade8(this, HeroKind.Warrior));
        globalQuestList.AddRange(globalQuestListUpgrade);
        globalQuestListNitro.Add(new GlobalQuest_Nitro1(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro2(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro3(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro4(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro5(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro6(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro7(this, HeroKind.Warrior));
        globalQuestListNitro.Add(new GlobalQuest_Nitro8(this, HeroKind.Warrior));
        globalQuestList.AddRange(globalQuestListNitro);
        globalQuestListCapture.Add(new GlobalQuest_Capture1(this, HeroKind.Warrior));
        globalQuestListCapture.Add(new GlobalQuest_Capture2(this, HeroKind.Warrior));
        globalQuestListCapture.Add(new GlobalQuest_Capture3(this, HeroKind.Warrior));
        globalQuestListCapture.Add(new GlobalQuest_Capture4(this, HeroKind.Warrior));
        globalQuestListCapture.Add(new GlobalQuest_Capture5(this, HeroKind.Warrior));
        globalQuestList.AddRange(globalQuestListCapture);
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy1(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy2(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy3(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy4(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy5(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy6(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy7(this, HeroKind.Warrior));
        globalQuestListAlchemy.Add(new GlobalQuest_Alchemy8(this, HeroKind.Warrior));
        globalQuestList.AddRange(globalQuestListAlchemy);

        globalQuests = new QUEST[Enum.GetNames(typeof(QuestKindGlobal)).Length];
        //配列に登録するときにEnumの順番に直す
        for (int i = 0; i < globalQuests.Length; i++)
        {
            for (int j = 0; j < globalQuestList.Count; j++)
            {
                if (globalQuestList[j].kindGlobal == (QuestKindGlobal)i)
                {
                    globalQuests[i] = globalQuestList[j];
                    break;
                }
            }
        }

        dailyQuestList.Add(new DailyQuest_EC1(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_EC2(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_EC3(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_EC4(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_Cartographer1(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_Cartographer2(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_Cartographer3(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_Cartographer4(this, HeroKind.Warrior));
        dailyQuestList.Add(new DailyQuest_Cartographer5(this, HeroKind.Warrior));

        dailyQuests = new QUEST[Enum.GetNames(typeof(QuestKindDaily)).Length];
        //配列に登録するときにEnumの順番に直す
        for (int i = 0; i < dailyQuests.Length; i++)
        {
            for (int j = 0; j < dailyQuestList.Count; j++)
            {
                if (dailyQuestList[j].kindDaily == (QuestKindDaily)i)
                {
                    dailyQuests[i] = dailyQuestList[j];
                    break;
                }
            }
        }

        titleQuests = new QUEST[Enum.GetNames(typeof(HeroKind)).Length][];
        generalQuests = new QUEST[Enum.GetNames(typeof(HeroKind)).Length][];

        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            titleQuestList[i] = new List<QUEST>();
            //titleQuestListSkill[i] = new List<QUEST>();
            //titleQuestListStudy[i] = new List<QUEST>();
            //titleQuestListEquipSlot[i] = new List<QUEST>();
            //titleQuestListAttack[i] = new List<QUEST>();
            //titleQuestListPorter[i] = new List<QUEST>();
            //titleQuestListAlchemist[i] = new List<QUEST>();
            //titleQuestListEquipProf[i] = new List<QUEST>();
            //titleQuestListResistance[i] = new List<QUEST>();
            //titleQuestListSurvival[i] = new List<QUEST>();
            //titleQuestListRebirth[i] = new List<QUEST>();

            titleQuestList[i].Add(new TitleQuest_SkillMaster1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_SkillMaster2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_SkillMaster3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_SkillMaster4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MonsterDistinguisher6(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotWeapon6(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotArmor6(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentSlotJewelry6(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PotionSlot1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PotionSlot2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PotionSlot3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PhysicalAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PhysicalAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PhysicalAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_PhysicalAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkAttack1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkAttack2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkAttack3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkAttack4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Porter6(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Alchemist1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Alchemist2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Alchemist3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Alchemist4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Alchemist5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentProf1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentProf2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentProf3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentProf4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_EquipmentProf5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MetalHunter1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MetalHunter2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MetalHunter3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_MetalHunter4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireResistance1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireResistance2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireResistance3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireResistance4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_FireResistance5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceResistance1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceResistance2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceResistance3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceResistance4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_IceResistance5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderResistance1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderResistance2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderResistance3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderResistance4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_ThunderResistance5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightResistance1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightResistance2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightResistance3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightResistance4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_LightResistance5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkResistance1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkResistance2(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkResistance3(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkResistance4(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_DarkResistance5(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Survival1(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Survival2(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Survival3(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Survival4(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Cooperation1(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Cooperation2(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Cooperation3(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester1(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester2(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester3(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester4(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester5(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester6(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester7(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester8(this, (HeroKind)i));
            titleQuestList[i].Add( new TitleQuest_Quester9(this, (HeroKind)i));
            titleQuestList[i].Add(new TitleQuest_Quester10(this, (HeroKind)i));

            titleQuests[i] = new QUEST[Enum.GetNames(typeof(QuestKindTitle)).Length];
            //配列に登録するときにEnumの順番に直す
            for (int k = 0; k < titleQuests[i].Length; k++)
            {
                for (int j = 0; j < titleQuestList[i].Count; j++)
                {
                    if (titleQuestList[i][j].kindTitle == (QuestKindTitle)k)
                    {
                        titleQuests[i][k] = titleQuestList[i][j];
                        break;
                    }
                }
            }

            generalQuestList[i] = new List<QUEST>();
            generalQuestList[i].Add(new GeneralQuest_Area0_0(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Area0_1(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Area0_2(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Area0_3(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatNormalSlime1(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatNormalSlime2(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatNormalSlime3(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_OilOfSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatRedSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatNormalMagicSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatRedMagicSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatGreenMagicSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon0_0(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon0_1(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon0_2(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatYellowBat(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatRedBat(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatGreenBat(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatPurpleBat(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_DefeatBlueBat(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_BringToEnchantShard(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon2_0(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon2_1(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureNormalSpider(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_Dungeon2_2(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureYellowSlime(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureNormalFairy(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureBlueFairy(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureYellowFairy(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureRedFairy(this, (HeroKind)i));
            generalQuestList[i].Add(new GeneralQuest_CaptureGreenFairy(this, (HeroKind)i));

            generalQuests[i] = new QUEST[Enum.GetNames(typeof(QuestKindGeneral)).Length];
            //配列に登録するときにEnumの順番に直す
            for (int k = 0; k < generalQuests[i].Length; k++)
            {
                for (int j = 0; j < generalQuestList[i].Count; j++)
                {
                    if (generalQuestList[i][j].kindGeneral == (QuestKindGeneral)k)
                    {
                        generalQuests[i][k] = generalQuestList[i][j];
                        break;
                    }
                }
            }

        }

        SetTitleEffect();
    }
    public void Start()
    {
        for (int i = 0; i < globalQuests.Length; i++)
        {
            globalQuests[i].Start();
        }
        for (int i = 0; i < dailyQuests.Length; i++)
        {
            dailyQuests[i].Start();
        }
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            for (int j = 0; j < titleQuests[i].Length; j++)
            {
                titleQuests[i][j].Start();
            }
            for (int j = 0; j < generalQuests[i].Length; j++)
            {
                generalQuests[i][j].Start();
                generalQuests[i][j].SetMastery();
            }
        }
    }
    public bool IsExistCanClaimQuest(HeroKind heroKind)
    {
        for (int i = 0; i < Enum.GetNames(typeof(QuestKind)).Length; i++)
        {
            if (IsExistCanClaimQuest((QuestKind)i, heroKind)) return true;
        }
        return false;
    }
    public bool IsExistCanClaimQuest(QuestKind kind, HeroKind heroKind)
    {
        for (int i = 0; i < QuestArray(kind,heroKind).Length; i++)
        {
            if (QuestArray(kind, heroKind)[i].CanClaim()) return true;
        }
        return false;
    }
    public QUEST Quest(QuestKind kind, HeroKind heroKind, int id)
    {
        return QuestArray(kind, heroKind)[id];
    }
    public QUEST Quest(QuestKindGlobal kindGlobal)
    {
        return QuestArray(QuestKind.Global, HeroKind.Warrior)[(int)kindGlobal];
    }
    public QUEST Quest(QuestKindDaily kindDaily)
    {
        return QuestArray(QuestKind.Daily, HeroKind.Warrior)[(int)kindDaily];
    }
    public QUEST Quest(QuestKindTitle kindTitle, HeroKind heroKind)
    {
        return QuestArray(QuestKind.Title, heroKind)[(int)kindTitle];
    }
    public QUEST Quest(QuestKindGeneral kindGeneral, HeroKind heroKind)
    {
        return QuestArray(QuestKind.General, heroKind)[(int)kindGeneral];
    }
    public QUEST[] QuestArray(QuestKind kind, HeroKind heroKind)
    {
        switch (kind)
        {
            case QuestKind.Global:
                return globalQuests;
            case QuestKind.Daily:
                return dailyQuests;
            case QuestKind.Title:
                return titleQuests[(int)heroKind];
            case QuestKind.General:
                return generalQuests[(int)heroKind];
        }
        return null;
    }
    public int ClearedNum(QuestKind questKind, HeroKind heroKind)
    {
        int tempNum = 0;
        for (int i = 0; i < QuestArray(questKind, heroKind).Length; i++)
        {
            if (QuestArray(questKind, heroKind)[i].isCleared) tempNum++;
        }
        return tempNum;
    }

    public List<QUEST> globalQuestList = new List<QUEST>();
    public List<QUEST> globalQuestListTutorial = new List<QUEST>();
    public List<QUEST> globalQuestListUpgrade = new List<QUEST>();
    public List<QUEST> globalQuestListNitro = new List<QUEST>();
    public List<QUEST> globalQuestListCapture = new List<QUEST>();
    public List<QUEST> globalQuestListAlchemy = new List<QUEST>();

    public List<QUEST> dailyQuestList = new List<QUEST>();

    public List<QUEST>[] titleQuestList = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListSkill = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListStudy = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListEquipSlot = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListAttack = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListPorter = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListAlchemist = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListEquipProf = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListResistance = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListSurvival = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] titleQuestListRebirth = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];

    public List<QUEST>[] generalQuestList = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListSlime = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListMagicslime = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListSpider = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListBat = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListFairy = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListFox = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListDevilfish = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListTreant = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListFlametiger = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];
    public List<QUEST>[] generalQuestListUnicorn = new List<QUEST>[Enum.GetNames(typeof(HeroKind)).Length];

    QUEST[] globalQuests;//[QuestKindGlobal]
    QUEST[] dailyQuests;//[QuestKindDaily]
    QUEST[][] titleQuests;//[HeroKind][QuestKindTitle]
    QUEST[][] generalQuests;//[HeroKind][QuestKindGeneral]
    public Multiplier portalOrbBonusFromDailyQuest;
    private Multiplier[] acceptableNum = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier AcceptableNum(HeroKind heroKind) { return acceptableNum[(int)heroKind]; }
    QUEST quest;
    public double AcceptedNum(HeroKind heroKind)
    {
        double tempNum = 0;
        for (int i = 0; i < titleQuests[(int)heroKind].Length; i++)
        {
            quest = titleQuests[(int)heroKind][i];
            if (!quest.isCleared && quest.isAccepted) tempNum++;
        }
        for (int i = 0; i < generalQuests[(int)heroKind].Length; i++)
        {
            quest = generalQuests[(int)heroKind][i];
            if (!quest.isCleared && quest.isAccepted)
                tempNum += quest.acceptNumModifier;
            //else if (generalQuests[(int)heroKind][i].isFavorite) tempNum++;
        }
        return tempNum;
    }
    public bool CanAccept(QUEST quest, HeroKind heroKind)
    {
        if (quest.kind == QuestKind.Global || quest.kind == QuestKind.Daily) return true;
        return AcceptedNum(heroKind) + quest.acceptNumModifier <= AcceptableNum(heroKind).Value();
    }
    public bool CanFavorite(QUEST quest, HeroKind heroKind)
    {
        if (quest.kind != QuestKind.General) return false;
        return TotalFavoriteNum(heroKind) + quest.acceptNumModifier <= AcceptableNum(heroKind).Value();
    }

    //Title
    public long TitleLevel(HeroKind heroKind, TitleKind kind)
    {
        long tempLevel = 0;
        for (int i = 0; i < titleQuests[(int)heroKind].Length; i++)
        {
            if (titleQuests[(int)heroKind][i].rewardTitleKind == kind && titleQuests[(int)heroKind][i].isCleared)
                tempLevel++;
        }
        return tempLevel;
    }

    public (double main, double sub) TitleEffectValue(HeroKind heroKind, TitleKind kind) { return Parameter.TitleEffectValue(kind, TitleLevel(heroKind, kind)); }
    void SetTitleEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(TitleKind)).Length; i++)
        {
            SetTitleEffect((TitleKind)i);
        }
    }
    void SetTitleEffect(TitleKind kind)
    {
        switch (kind)
        {
            case TitleKind.MonsterDistinguisher:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.MonsterDistinguishMaxLevel((HeroKind)count).RegisterMultiplier(info);
                    var subInfo = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.MonsterCaptureMaxLevelIncrement((HeroKind)count).RegisterMultiplier(subInfo);
                }
                break;
            case TitleKind.EquipmentSlotWeapon:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.inventoryCtrl.equipWeaponUnlockedNum[i].RegisterMultiplier(info);
                }
                break;
            case TitleKind.EquipmentSlotArmor:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.inventoryCtrl.equipArmorUnlockedNum[i].RegisterMultiplier(info);
                }
                break;
            case TitleKind.EquipmentSlotJewelry:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.inventoryCtrl.equipJewelryUnlockedNum[i].RegisterMultiplier(info);
                }
                break;
            case TitleKind.PotionSlot:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.inventoryCtrl.equipPotionUnlockedNum[i].RegisterMultiplier(info);
                }
                break;
            case TitleKind.EquipmentProficiency:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.EquipmentProficiencyGain).RegisterMultiplier(info);
                }
                break;
            case TitleKind.SkillMaster:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.SkillSlotNum((HeroKind)count).RegisterMultiplier(info);
                    var subInfo = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.SkillProficiencyGain).RegisterMultiplier(subInfo);
                }
                break;
            case TitleKind.Survival:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    Func<bool> condition = () =>
                    {
                        if (!game.battleCtrls[count].isActiveBattle) return false;
                        if (game.battleCtrls[count].hero.HpPercent() >= TitleEffectValue((HeroKind)count, kind).main) return false;
                        return true;
                    };
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => 0.50d, condition);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.PhysCritChance).RegisterMultiplier(info);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.MagCritChance).RegisterMultiplier(info);
                }
                break;
            case TitleKind.FireResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.FireRes).RegisterMultiplier(info);
                    var infosub = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.ElementInvalid((HeroKind)count, Element.Fire).RegisterMultiplier(infosub);
                }
                break;
            case TitleKind.IceResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.IceRes).RegisterMultiplier(info);
                    var infosub = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.ElementInvalid((HeroKind)count, Element.Ice).RegisterMultiplier(infosub);
                }
                break;
            case TitleKind.ThunderResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.ThunderRes).RegisterMultiplier(info);
                    var infosub = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.ElementInvalid((HeroKind)count, Element.Thunder).RegisterMultiplier(infosub);
                }
                break;
            case TitleKind.LightResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.LightRes).RegisterMultiplier(info);
                    var infosub = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.ElementInvalid((HeroKind)count, Element.Light).RegisterMultiplier(infosub);
                }
                break;
            case TitleKind.DarkResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.DarkRes).RegisterMultiplier(info);
                    var infosub = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).sub);
                    game.statsCtrl.ElementInvalid((HeroKind)count, Element.Dark).RegisterMultiplier(infosub);
                }
                break;
            case TitleKind.DebuffResistance:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.DebuffRes).RegisterMultiplier(info);
                }
                break;
            case TitleKind.MoveSpeed:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.HeroStats((HeroKind)count, Stats.MoveSpeed).RegisterMultiplier(info);
                }
                break;
            case TitleKind.Alchemist:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);//, () => game.guildCtrl.Member((HeroKind)count).isPlaying); これは常にON
                    game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(info);
                }
                break;
            case TitleKind.BreakingTheLimit:
                //未
                break;
            case TitleKind.PhysicalDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Physical).RegisterMultiplier(info);
                }
                break;
            case TitleKind.FireDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Fire).RegisterMultiplier(info);
                }
                break;
            case TitleKind.IceDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Ice).RegisterMultiplier(info);
                }
                break;
            case TitleKind.ThunderDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Thunder).RegisterMultiplier(info);
                }
                break;
            case TitleKind.LightDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Light).RegisterMultiplier(info);
                }
                break;
            case TitleKind.DarkDamage:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Mul, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.statsCtrl.ElementDamage((HeroKind)count, Element.Dark).RegisterMultiplier(info);
                }
                break;
            case TitleKind.Cooperation:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    game.guildCtrl.Member((HeroKind)count).backgroundGainRate.RegisterMultiplier(info);
                }
                break;
            case TitleKind.MetalHunter:
                //直接かいている
                break;
            case TitleKind.Quester:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    int count = i;
                    var info = new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => TitleEffectValue((HeroKind)count, kind).main);
                    generalQuestClearGain[count].RegisterMultiplier(info);
                }
                break;
        }
    }
    public Multiplier[] generalQuestClearGain = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];

    //Favorite
    public bool isFavoriteActivated { get => game.epicStoreCtrl.Item(EpicStoreKind.FavoriteQuest).IsPurchased(); }// game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.AcceptGeneralQuest); }

    public void AcceptFavorite()
    {
        if (!isFavoriteActivated) return;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            if (game.battleCtrls[i].isActiveBattle)
            {
                for (int j = 0; j < generalQuests[i].Length; j++)
                {
                    if (generalQuests[i][j].isFavorite)
                        generalQuests[i][j].Accept();
                }
            }
        }
    }
    public void ClaimFavorite()
    {
        if (!isFavoriteActivated) return;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            if (game.battleCtrls[i].isActiveBattle)
            {
                for (int j = 0; j < generalQuests[i].Length; j++)
                {
                    if (generalQuests[i][j].isFavorite)
                        generalQuests[i][j].Claim();
                }
            }
        }
    }
    public double TotalFavoriteNum(HeroKind heroKind)
    {
        double tempNum = 0;
        for (int j = 0; j < generalQuests[(int)heroKind].Length; j++)
        {
            if (generalQuests[(int)heroKind][j].isFavorite)
                tempNum += generalQuests[(int)heroKind][j].acceptNumModifier;
        }
        return tempNum;
    }
}
