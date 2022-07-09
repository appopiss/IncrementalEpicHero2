using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static Parameter;
using System;

public class TitleQuest
{
}

//SkillMaster
public class TitleQuest_SkillMaster1 : QUEST
{
    public TitleQuest_SkillMaster1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.SkillMaster1;
        rewardExp = () => 300;
        rewardTitleKind = TitleKind.SkillMaster;
        rewardTitleLevel = 1;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.skillCtrl.Skill(heroKind, 0).level.value >= 10);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearGeneralQuest));
    }
    public override void ClaimAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
}
public class TitleQuest_SkillMaster2 : QUEST
{
    public TitleQuest_SkillMaster2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.SkillMaster2;
        rewardExp = () => 75000;
        rewardTitleKind = TitleKind.SkillMaster;
        rewardTitleLevel = 2;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.skillCtrl.Skill(heroKind, 0).level.value >= 50);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.SkillMaster1, heroKind));
    }
    public override void ClaimAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
}
public class TitleQuest_SkillMaster3 : QUEST
{
    public TitleQuest_SkillMaster3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.SkillMaster3;
        rewardExp = () => 1e8d;
        rewardTitleKind = TitleKind.SkillMaster;
        rewardTitleLevel = 3;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.skillCtrl.Skill(heroKind, 0).level.value >= 250);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.SkillMaster2, heroKind));
    }
    public override void ClaimAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
}
public class TitleQuest_SkillMaster4 : QUEST
{
    public TitleQuest_SkillMaster4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.SkillMaster4;
        rewardExp = () => 5e9;
        rewardTitleKind = TitleKind.SkillMaster;
        rewardTitleLevel = 4;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsGetAllSkill);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.SkillMaster3, heroKind));
    }
    bool IsGetAllSkill()
    {
        for (int i = 0; i < Enum.GetNames(typeof(SkillKindWarrior)).Length; i++)
        {
            if (game.skillCtrl.Skill(heroKind, i).level.value < 250) return false;
        }
        return true;
    }
    public override void ClaimAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
}


//MonsterDistinguisher
public class TitleQuest_MonsterDistinguisher1 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher1;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal);
        defeatRequredDefeatNum = () => 20;
        rewardGold = () => 200;
        rewardExp = () => 500;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class TitleQuest_MonsterDistinguisher2 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher2;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Blue);
        defeatRequredDefeatNum = () => 100;
        rewardGold = () => 500;
        rewardExp = () => 5000;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MonsterDistinguisher1, heroKind));
    }
}
public class TitleQuest_MonsterDistinguisher3 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher3;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Yellow);
        defeatRequredDefeatNum = () => 250;
        rewardGold = () => 1000;
        rewardExp = () => 50000;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 30;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MonsterDistinguisher2, heroKind));
    }
}
public class TitleQuest_MonsterDistinguisher4 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher4;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Blue);
        defeatRequredDefeatNum = () => 1000;
        rewardExp = () => 500000;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 60;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MonsterDistinguisher3, heroKind));
    }
}
public class TitleQuest_MonsterDistinguisher5 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher5;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Red);
        defeatRequredDefeatNum = () => 5000;
        rewardExp = () => 30000000;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 140;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MonsterDistinguisher4, heroKind));
    }
}
public class TitleQuest_MonsterDistinguisher6 : DefeatQuest
{
    public TitleQuest_MonsterDistinguisher6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MonsterDistinguisher6;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.DevifFish, MonsterColor.Green);
        defeatRequredDefeatNum = () => 50000;
        rewardExp = () => 2e9;
        rewardTitleKind = TitleKind.MonsterDistinguisher;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 300;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MonsterDistinguisher5, heroKind));
    }
}

//EquipSlot
public class TitleQuest_EquipmentSlotWeapon1 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon1;
        rewardExp = () => 8500;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 10;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Equip));
    }
    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon) >= 1;
    }
}

public class TitleQuest_EquipmentSlotWeapon2 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon2;
        rewardExp = () => 150000;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 30;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon1, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon) >= 5;

    }
}
public class TitleQuest_EquipmentSlotWeapon3 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon3;
        rewardExp = () => 650000;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 60;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon2, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon) >= 15;
    }
}
public class TitleQuest_EquipmentSlotWeapon4 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon4;
        rewardExp = () => 5000000;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 100;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon3, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon) >= 30;
    }
}
public class TitleQuest_EquipmentSlotWeapon5 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon5;
        rewardExp = () => 5e8;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 200;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon4, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon, 15) >= 20;
    }
}
public class TitleQuest_EquipmentSlotWeapon6 : QUEST
{
    public TitleQuest_EquipmentSlotWeapon6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotWeapon6;
        rewardExp = () => 1e10;
        rewardTitleKind = TitleKind.EquipmentSlotWeapon;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 300;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon5, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Weapon, 20) >= 50;
    }
}


public class TitleQuest_EquipmentSlotArmor1 : QUEST
{
    public TitleQuest_EquipmentSlotArmor1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor1;
        rewardExp = () => 8500;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 15;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Equip));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor) >= 1;
    }
}

public class TitleQuest_EquipmentSlotArmor2 : QUEST
{
    public TitleQuest_EquipmentSlotArmor2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor2;
        rewardExp = () => 150000;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 35;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor1, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor) >= 5;
    }
}
public class TitleQuest_EquipmentSlotArmor3 : QUEST
{
    public TitleQuest_EquipmentSlotArmor3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor3;
        rewardExp = () => 650000;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 70;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor2, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor) >= 15;
    }
}
public class TitleQuest_EquipmentSlotArmor4 : QUEST
{
    public TitleQuest_EquipmentSlotArmor4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor4;
        rewardExp = () => 5000000;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 110;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor3, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor) >= 30;
    }
}
public class TitleQuest_EquipmentSlotArmor5 : QUEST
{
    public TitleQuest_EquipmentSlotArmor5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor5;
        rewardExp = () => 5e8;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 220;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor4, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor, 15) >= 20;
    }
}
public class TitleQuest_EquipmentSlotArmor6 : QUEST
{
    public TitleQuest_EquipmentSlotArmor6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotArmor6;
        rewardExp = () => 1e10;
        rewardTitleKind = TitleKind.EquipmentSlotArmor;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 330;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotArmor5, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Armor, 20) >= 50;
    }
}


public class TitleQuest_EquipmentSlotJewelry1 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry1;
        rewardExp = () => 8500;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 20;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Equip));
    }
    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry) >= 1;
    }
}

public class TitleQuest_EquipmentSlotJewelry2 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry2;
        rewardExp = () => 150000;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 40;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry1, heroKind));
    }
    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry) >= 5;
    }
}
public class TitleQuest_EquipmentSlotJewelry3 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry3;
        rewardExp = () => 600000;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 80;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry2, heroKind));
    }
    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry) >= 15;
    }
}
public class TitleQuest_EquipmentSlotJewelry4 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry4;
        rewardExp = () => 5000000;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 120;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry3, heroKind));
    }
    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry) >= 30;
    }
}
public class TitleQuest_EquipmentSlotJewelry5 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry5;
        rewardExp = () => 5e8;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 230;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry4, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry, 15) >= 20;
    }
}
public class TitleQuest_EquipmentSlotJewelry6 : QUEST
{
    public TitleQuest_EquipmentSlotJewelry6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentSlotJewelry6;
        rewardExp = () => 1e10;
        rewardTitleKind = TitleKind.EquipmentSlotJewelry;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 340;
    }
    public override void StartQuest()
    {
        clearConditions.Add(IsLevelMaxedEquipment);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotJewelry5, heroKind));
    }

    bool IsLevelMaxedEquipment()
    {
        return game.equipmentCtrl.LevelMaxedNum(heroKind, EquipmentPart.Jewelry, 20) >= 50;
    }
}

public class TitleQuest_PotionSlot1 : BringQuest
{
    public TitleQuest_PotionSlot1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PotionSlot1;
        rewardTitleKind = TitleKind.PotionSlot;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 50;
        bringRequiredMaterials.Add(MaterialKind.SlimeBall, 10);
        bringRequiredMaterials.Add(MaterialKind.ManaSeed, 10);
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_PotionSlot2 : BringQuest
{
    public TitleQuest_PotionSlot2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PotionSlot2;
        rewardTitleKind = TitleKind.PotionSlot;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 250;
        bringRequiredMaterials.Add(MaterialKind.SlimeBall, 500);
        bringRequiredMaterials.Add(MaterialKind.ManaSeed, 500);
        bringRequiredMaterials.Add(MaterialKind.UnmeltingIce, 250);
        bringRequiredMaterials.Add(MaterialKind.EternalFlame, 250);
        bringRequiredMaterials.Add(MaterialKind.AncientBattery, 250);
        rewardPortalOrb = () => 3;
    }

    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.PotionSlot1, heroKind));
    }
}
public class TitleQuest_PotionSlot3 : BringQuest
{
    public TitleQuest_PotionSlot3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PotionSlot3;
        rewardTitleKind = TitleKind.PotionSlot;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 500;
        bringRequiredMaterials.Add(MaterialKind.SlimeBall, 10000);
        bringRequiredMaterials.Add(MaterialKind.ManaSeed, 10000);
        bringRequiredMaterials.Add(MaterialKind.UnmeltingIce, 5000);
        bringRequiredMaterials.Add(MaterialKind.EternalFlame, 5000);
        bringRequiredMaterials.Add(MaterialKind.AncientBattery, 5000);
        bringRequiredMaterials.Add(MaterialKind.Ectoplasm, 1000);
        bringRequiredMaterials.Add(MaterialKind.Stardust, 1000);
        rewardPortalOrb = () => 5;
    }

    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.PotionSlot1, heroKind));
    }
}

//ElementAttack
public class TitleQuest_PhysicalAttack1 : QUEST
{
    public TitleQuest_PhysicalAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PhysicalAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.PhysicalDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value - main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        unlockConditions.Add(() => game.areaCtrl.Area(AreaKind.SlimeVillage, 3).IsUnlocked());
    }
}
public class TitleQuest_PhysicalAttack2 : QUEST
{
    public TitleQuest_PhysicalAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PhysicalAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.PhysicalDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value - main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.PhysicalAttack1, heroKind));
    }
}
public class TitleQuest_PhysicalAttack3 : QUEST
{
    public TitleQuest_PhysicalAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PhysicalAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.PhysicalDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value - main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.PhysicalAttack2, heroKind));
    }
}
public class TitleQuest_PhysicalAttack4 : QUEST
{
    public TitleQuest_PhysicalAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.PhysicalAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.PhysicalDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Physical).value - main.SR.initPhysicalSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.PhysicalAttack3, heroKind));
    }
}
public class TitleQuest_FireAttack1 : QUEST
{
    public TitleQuest_FireAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.FireDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value - main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Guild));
    }
}
public class TitleQuest_FireAttack2 : QUEST
{
    public TitleQuest_FireAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.FireDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value - main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireAttack1, heroKind));
    }
}
public class TitleQuest_FireAttack3 : QUEST
{
    public TitleQuest_FireAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.FireDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value - main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireAttack2, heroKind));
    }
}
public class TitleQuest_FireAttack4 : QUEST
{
    public TitleQuest_FireAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.FireDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Fire).value - main.SR.initFireSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireAttack3, heroKind));
    }
}

public class TitleQuest_IceAttack1 : QUEST
{
    public TitleQuest_IceAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.IceDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value - main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        unlockConditions.Add(() => game.areaCtrl.Area(AreaKind.MagicSlimeCity, 1).IsUnlocked());
    }
}
public class TitleQuest_IceAttack2 : QUEST
{
    public TitleQuest_IceAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.IceDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value - main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceAttack1, heroKind));
    }
}
public class TitleQuest_IceAttack3 : QUEST
{
    public TitleQuest_IceAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.IceDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value - main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceAttack2, heroKind));
    }
}
public class TitleQuest_IceAttack4 : QUEST
{
    public TitleQuest_IceAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.IceDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Ice).value - main.SR.initIceSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceAttack3, heroKind));
    }
}

public class TitleQuest_ThunderAttack1 : QUEST
{
    public TitleQuest_ThunderAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.ThunderDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value - main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        unlockConditions.Add(() => game.areaCtrl.Area(AreaKind.MagicSlimeCity, 1).IsUnlocked());
    }
}
public class TitleQuest_ThunderAttack2 : QUEST
{
    public TitleQuest_ThunderAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.ThunderDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value - main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderAttack1, heroKind));
    }
}
public class TitleQuest_ThunderAttack3 : QUEST
{
    public TitleQuest_ThunderAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.ThunderDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value - main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderAttack2, heroKind));
    }
}
public class TitleQuest_ThunderAttack4 : QUEST
{
    public TitleQuest_ThunderAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.ThunderDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Thunder).value - main.SR.initThunderSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderAttack3, heroKind));
    }
}

public class TitleQuest_LightAttack1 : QUEST
{
    public TitleQuest_LightAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.LightDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value - main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        unlockConditions.Add(() => game.areaCtrl.Area(AreaKind.MagicSlimeCity, 1).IsUnlocked());
    }
}
public class TitleQuest_LightAttack2 : QUEST
{
    public TitleQuest_LightAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.LightDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value - main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightAttack1, heroKind));
    }
}
public class TitleQuest_LightAttack3 : QUEST
{
    public TitleQuest_LightAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.LightDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value - main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightAttack2, heroKind));
    }
}
public class TitleQuest_LightAttack4 : QUEST
{
    public TitleQuest_LightAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.LightDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Light).value - main.SR.initLightSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightAttack3, heroKind));
    }
}

public class TitleQuest_DarkAttack1 : QUEST
{
    public TitleQuest_DarkAttack1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkAttack1;
        rewardExp = () => 30000;
        rewardTitleKind = TitleKind.DarkDamage;
        rewardTitleLevel = 1;
        elementTriggeredRequiredNum = 5000;
    }
    public override void AcceptAction()
    {
        main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value - main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
        unlockConditions.Add(() => game.areaCtrl.Area(AreaKind.MagicSlimeCity, 1).IsUnlocked());
    }
}
public class TitleQuest_DarkAttack2 : QUEST
{
    public TitleQuest_DarkAttack2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkAttack2;
        rewardExp = () => 1500000;
        rewardTitleKind = TitleKind.DarkDamage;
        rewardTitleLevel = 2;
        elementTriggeredRequiredNum = 25000;
        unlockHeroLevel = () => 50;
    }
    public override void AcceptAction()
    {
        main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value - main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkAttack1, heroKind));
    }
}
public class TitleQuest_DarkAttack3 : QUEST
{
    public TitleQuest_DarkAttack3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkAttack3;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.DarkDamage;
        rewardTitleLevel = 3;
        elementTriggeredRequiredNum = 500000;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value - main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkAttack2, heroKind));
    }
}
public class TitleQuest_DarkAttack4 : QUEST
{
    public TitleQuest_DarkAttack4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkAttack4;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.DarkDamage;
        rewardTitleLevel = 4;
        elementTriggeredRequiredNum = 5000000;
        unlockHeroLevel = () => 150;
    }
    public override void AcceptAction()
    {
        main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind] = game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value;
    }
    public override double elementTriggeredNum { get => game.statsCtrl.ElementSkillTriggeredNum(heroKind, Element.Dark).value - main.SR.initDarkSkillTriggeredNumQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => elementTriggeredNum >= elementTriggeredRequiredNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkAttack3, heroKind));
    }
}


//Porter
public class TitleQuest_Porter1 : QUEST
{
    public TitleQuest_Porter1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter1;
        rewardExp = () => 10000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 10;
        porterRequiredMovedDistance = 1e5;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class TitleQuest_Porter2 : QUEST
{
    public TitleQuest_Porter2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter2;
        rewardExp = () => 50000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 25;
        porterRequiredMovedDistance = 1e6;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Porter1, heroKind));
    }
}
public class TitleQuest_Porter3 : QUEST
{
    public TitleQuest_Porter3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter3;
        rewardExp = () => 500000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 35;
        porterRequiredMovedDistance = 1e7;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Porter2, heroKind));
    }
}
public class TitleQuest_Porter4 : QUEST
{
    public TitleQuest_Porter4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter4;
        rewardExp = () => 2000000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 50;
        porterRequiredMovedDistance = 1e8;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Porter3, heroKind));
    }
}
public class TitleQuest_Porter5 : QUEST
{
    public TitleQuest_Porter5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter5;
        rewardExp = () => 5000000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 80;
        porterRequiredMovedDistance = 1e9;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Porter4, heroKind));
    }
}
public class TitleQuest_Porter6 : QUEST
{
    public TitleQuest_Porter6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Porter6;
        rewardExp = () => 80000000;
        rewardTitleKind = TitleKind.MoveSpeed;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 150;
        porterRequiredMovedDistance = 1e10;
    }
    public override void AcceptAction()
    {
        main.SR.initMovedDistanceQuestTitle[(int)heroKind] = game.statsCtrl.MovedDistance(heroKind).value;
    }
    public override double movedDistance { get => game.statsCtrl.MovedDistance(heroKind).value - main.SR.initMovedDistanceQuestTitle[(int)heroKind]; }
    public override void StartQuest()
    {
        clearConditions.Add(() => movedDistance >= porterRequiredMovedDistance);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Porter5, heroKind));
    }
}

public class TitleQuest_Alchemist1 : BringQuest
{
    public TitleQuest_Alchemist1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Alchemist1;
        //rewardExp = () => 15000;
        bringRequiredMaterials.Add(MaterialKind.OilOfSlime, 100);
        rewardTitleKind = TitleKind.Alchemist;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 20;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_Alchemist2 : BringQuest
{
    public TitleQuest_Alchemist2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Alchemist2;
        //rewardExp = () => 150000;
        bringRequiredMaterials.Add(MaterialKind.MonsterFluid, 20);
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 10);
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 10);
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 10);
        rewardTitleKind = TitleKind.Alchemist;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 40;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Alchemist1, heroKind));
    }
}
public class TitleQuest_Alchemist3 : BringQuest
{
    public TitleQuest_Alchemist3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Alchemist3;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.MonsterFluid, 100);
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 50);
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 50);
        rewardTitleKind = TitleKind.Alchemist;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 60;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Alchemist2, heroKind));
    }
}
public class TitleQuest_Alchemist4 : BringQuest
{
    public TitleQuest_Alchemist4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Alchemist4;
        //rewardExp = () => 3000000;
        bringRequiredMaterials.Add(MaterialKind.MonsterFluid, 1000);
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 1000);
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 1000);
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 1000);
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 1000);
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 1000);
        rewardTitleKind = TitleKind.Alchemist;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 80;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Alchemist3, heroKind));
    }
}
public class TitleQuest_Alchemist5 : BringQuest
{
    public TitleQuest_Alchemist5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Alchemist5;
        //rewardExp = () => 10000000;
        bringRequiredMaterials.Add(MaterialKind.MonsterFluid, 5000);
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 5000);
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 5000);
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 5000);
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 5000);
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 5000);
        rewardTitleKind = TitleKind.Alchemist;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 100;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Alchemist4, heroKind));
    }
}

public class TitleQuest_EquipmentProf1 : QUEST
{
    public TitleQuest_EquipmentProf1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentProf1;
        rewardExp = () => 2500000;
        rewardTitleKind = TitleKind.EquipmentProficiency;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 75;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.battleCtrl.EquipmentDroppingNum() >= 5);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentSlotWeapon1, heroKind));
    }
}
public class TitleQuest_EquipmentProf2 : QUEST
{
    public TitleQuest_EquipmentProf2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentProf2;
        rewardExp = () => 50000000;
        rewardTitleKind = TitleKind.EquipmentProficiency;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 150;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.battleCtrl.EquipmentDroppingNum() >= 10);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentProf1, heroKind));
    }
}
public class TitleQuest_EquipmentProf3 : QUEST
{
    public TitleQuest_EquipmentProf3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentProf3;
        rewardExp = () => 2e9;
        rewardTitleKind = TitleKind.EquipmentProficiency;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 250;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.battleCtrl.EquipmentDroppingNum() >= 15);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentProf2, heroKind));
    }
}
public class TitleQuest_EquipmentProf4 : QUEST
{
    public TitleQuest_EquipmentProf4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentProf4;
        rewardExp = () => 1e11;
        rewardTitleKind = TitleKind.EquipmentProficiency;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 350;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.battleCtrl.EquipmentDroppingNum() >= 20);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentProf3, heroKind));
    }
}
public class TitleQuest_EquipmentProf5 : QUEST
{
    public TitleQuest_EquipmentProf5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.EquipmentProf5;
        rewardExp = () => 5e15;
        rewardTitleKind = TitleKind.EquipmentProficiency;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 500;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.battleCtrl.EquipmentDroppingNum() >= 30);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.EquipmentProf4, heroKind));
    }
}


public class TitleQuest_MetalHunter1 : DefeatQuest
{
    public TitleQuest_MetalHunter1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MetalHunter1;
        rewardTitleKind = TitleKind.MetalHunter;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 25;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Metal);
        defeatRequredDefeatNum = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.SkillMaster1, heroKind));
    }
}
public class TitleQuest_MetalHunter2 : DefeatQuest
{
    public TitleQuest_MetalHunter2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MetalHunter2;
        rewardTitleKind = TitleKind.MetalHunter;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 50;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Metal);
        defeatRequredDefeatNum = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MetalHunter1, heroKind));
    }
}
public class TitleQuest_MetalHunter3 : DefeatQuest
{
    public TitleQuest_MetalHunter3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MetalHunter3;
        rewardTitleKind = TitleKind.MetalHunter;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 80;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Metal);
        defeatRequredDefeatNum = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MetalHunter2, heroKind));
    }
}
public class TitleQuest_MetalHunter4 : DefeatQuest
{
    public TitleQuest_MetalHunter4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.MetalHunter4;
        rewardTitleKind = TitleKind.MetalHunter;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 120;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Metal);
        defeatRequredDefeatNum = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.MetalHunter3, heroKind));
    }
}

public class TitleQuest_FireResistance1 : BringQuest
{
    public TitleQuest_FireResistance1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireResistance1;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 10);
        rewardTitleKind = TitleKind.FireResistance;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 70;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_FireResistance2 : BringQuest
{
    public TitleQuest_FireResistance2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireResistance2;
        //rewardExp = () => 45000000;
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 100);
        rewardTitleKind = TitleKind.FireResistance;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 140;
        rewardPortalOrb = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireResistance1, heroKind));
    }
}
public class TitleQuest_FireResistance3 : BringQuest
{
    public TitleQuest_FireResistance3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireResistance3;
        //rewardExp = () => 2e8d;
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 1000);
        rewardTitleKind = TitleKind.FireResistance;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 210;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireResistance2, heroKind));
    }
}
public class TitleQuest_FireResistance4 : BringQuest
{
    public TitleQuest_FireResistance4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireResistance4;
        //rewardExp = () => 1e9d;
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 5000);
        rewardTitleKind = TitleKind.FireResistance;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 280;
        rewardPortalOrb = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireResistance3, heroKind));
    }
}
public class TitleQuest_FireResistance5 : BringQuest
{
    public TitleQuest_FireResistance5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.FireResistance5;
        //rewardExp = () => 5e9d;
        bringRequiredMaterials.Add(MaterialKind.FlameShard, 20000);
        rewardTitleKind = TitleKind.FireResistance;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.FireResistance4, heroKind));
    }
}
public class TitleQuest_IceResistance1 : BringQuest
{
    public TitleQuest_IceResistance1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceResistance1;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 10);
        rewardTitleKind = TitleKind.IceResistance;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 70;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_IceResistance2 : BringQuest
{
    public TitleQuest_IceResistance2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceResistance2;
        //rewardExp = () => 45000000;
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 100);
        rewardTitleKind = TitleKind.IceResistance;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 140;
        rewardPortalOrb = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceResistance1, heroKind));
    }
}
public class TitleQuest_IceResistance3 : BringQuest
{
    public TitleQuest_IceResistance3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceResistance3;
        //rewardExp = () => 2e8d;
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 1000);
        rewardTitleKind = TitleKind.IceResistance;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 210;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceResistance2, heroKind));
    }
}
public class TitleQuest_IceResistance4 : BringQuest
{
    public TitleQuest_IceResistance4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceResistance4;
        //rewardExp = () => 1e9d;
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 5000);
        rewardTitleKind = TitleKind.IceResistance;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 280;
        rewardPortalOrb = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceResistance3, heroKind));
    }
}
public class TitleQuest_IceResistance5 : BringQuest
{
    public TitleQuest_IceResistance5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.IceResistance5;
        //rewardExp = () => 5e9d;
        bringRequiredMaterials.Add(MaterialKind.FrostShard, 20000);
        rewardTitleKind = TitleKind.IceResistance;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.IceResistance4, heroKind));
    }
}
public class TitleQuest_ThunderResistance1 : BringQuest
{
    public TitleQuest_ThunderResistance1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderResistance1;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 10);
        rewardTitleKind = TitleKind.ThunderResistance;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 70;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_ThunderResistance2 : BringQuest
{
    public TitleQuest_ThunderResistance2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderResistance2;
        //rewardExp = () => 45000000;
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 100);
        rewardTitleKind = TitleKind.ThunderResistance;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 140;
        rewardPortalOrb = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderResistance1, heroKind));
    }
}
public class TitleQuest_ThunderResistance3 : BringQuest
{
    public TitleQuest_ThunderResistance3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderResistance3;
        //rewardExp = () => 2e8d;
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 1000);
        rewardTitleKind = TitleKind.ThunderResistance;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 210;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderResistance2, heroKind));
    }
}
public class TitleQuest_ThunderResistance4 : BringQuest
{
    public TitleQuest_ThunderResistance4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderResistance4;
        //rewardExp = () => 1e9d;
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 5000);
        rewardTitleKind = TitleKind.ThunderResistance;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 280;
        rewardPortalOrb = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderResistance3, heroKind));
    }
}
public class TitleQuest_ThunderResistance5 : BringQuest
{
    public TitleQuest_ThunderResistance5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.ThunderResistance5;
        //rewardExp = () => 5e9d;
        bringRequiredMaterials.Add(MaterialKind.LightningShard, 20000);
        rewardTitleKind = TitleKind.ThunderResistance;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.ThunderResistance4, heroKind));
    }
}
public class TitleQuest_LightResistance1 : BringQuest
{
    public TitleQuest_LightResistance1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightResistance1;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 10);
        rewardTitleKind = TitleKind.LightResistance;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 70;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_LightResistance2 : BringQuest
{
    public TitleQuest_LightResistance2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightResistance2;
        //rewardExp = () => 45000000;
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 100);
        rewardTitleKind = TitleKind.LightResistance;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 140;
        rewardPortalOrb = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightResistance1, heroKind));
    }
}
public class TitleQuest_LightResistance3 : BringQuest
{
    public TitleQuest_LightResistance3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightResistance3;
        //rewardExp = () => 2e8d;
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 1000);
        rewardTitleKind = TitleKind.LightResistance;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 210;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightResistance2, heroKind));
    }
}
public class TitleQuest_LightResistance4 : BringQuest
{
    public TitleQuest_LightResistance4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightResistance4;
        //rewardExp = () => 1e9d;
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 5000);
        rewardTitleKind = TitleKind.LightResistance;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 280;
        rewardPortalOrb = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightResistance3, heroKind));
    }
}
public class TitleQuest_LightResistance5 : BringQuest
{
    public TitleQuest_LightResistance5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.LightResistance5;
        //rewardExp = () => 5e9d;
        bringRequiredMaterials.Add(MaterialKind.NatureShard, 20000);
        rewardTitleKind = TitleKind.LightResistance;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.LightResistance4, heroKind));
    }
}
public class TitleQuest_DarkResistance1 : BringQuest
{
    public TitleQuest_DarkResistance1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkResistance1;
        //rewardExp = () => 1000000;
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 10);
        rewardTitleKind = TitleKind.DarkResistance;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 70;
        rewardPortalOrb = () => 1;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Alchemy));
    }
}
public class TitleQuest_DarkResistance2 : BringQuest
{
    public TitleQuest_DarkResistance2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkResistance2;
        //rewardExp = () => 45000000;
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 100);
        rewardTitleKind = TitleKind.DarkResistance;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 140;
        rewardPortalOrb = () => 2;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkResistance1, heroKind));
    }
}
public class TitleQuest_DarkResistance3 : BringQuest
{
    public TitleQuest_DarkResistance3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkResistance3;
        //rewardExp = () => 2e8d;
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 1000);
        rewardTitleKind = TitleKind.DarkResistance;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 210;
        rewardPortalOrb = () => 3;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkResistance2, heroKind));
    }
}
public class TitleQuest_DarkResistance4 : BringQuest
{
    public TitleQuest_DarkResistance4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkResistance4;
        //rewardExp = () => 1e9d;
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 5000);
        rewardTitleKind = TitleKind.DarkResistance;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 280;
        rewardPortalOrb = () => 4;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkResistance3, heroKind));
    }
}
public class TitleQuest_DarkResistance5 : BringQuest
{
    public TitleQuest_DarkResistance5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.DarkResistance5;
        //rewardExp = () => 5e9d;
        bringRequiredMaterials.Add(MaterialKind.PoisonShard, 20000);
        rewardTitleKind = TitleKind.DarkResistance;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        rewardPortalOrb = () => 5;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.DarkResistance4, heroKind));
    }
}


//Survival
public class TitleQuest_Survival1 : QUEST
{
    public TitleQuest_Survival1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Survival1;
        rewardExp = () => 10000000;
        rewardTitleKind = TitleKind.Survival;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 100;
    }
    public override void AcceptAction()
    {
        main.SR.survivalNumQuestTitle[(int)heroKind] = 0;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.SR.survivalNumQuestTitle[(int)heroKind] >= 500);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest));
    }
}
public class TitleQuest_Survival2 : QUEST
{
    public TitleQuest_Survival2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Survival2;
        rewardExp = () => 3e8d;
        rewardTitleKind = TitleKind.Survival;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 200;
    }
    public override void AcceptAction()
    {
        main.SR.survivalNumQuestTitle[(int)heroKind] = 0;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.SR.survivalNumQuestTitle[(int)heroKind] >= 5000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Survival1, heroKind));
    }
}
public class TitleQuest_Survival3 : QUEST
{
    public TitleQuest_Survival3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Survival3;
        rewardExp = () => 3e9d;
        rewardTitleKind = TitleKind.Survival;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 300;
    }
    public override void AcceptAction()
    {
        main.SR.survivalNumQuestTitle[(int)heroKind] = 0;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.SR.survivalNumQuestTitle[(int)heroKind] >= 50000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Survival2, heroKind));
    }
}
public class TitleQuest_Survival4 : QUEST
{
    public TitleQuest_Survival4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Survival4;
        rewardExp = () => 1e10d;
        rewardTitleKind = TitleKind.Survival;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 400;
    }
    public override void AcceptAction()
    {
        main.SR.survivalNumQuestTitle[(int)heroKind] = 0;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => main.SR.survivalNumQuestTitle[(int)heroKind] >= 500000);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Survival3, heroKind));
    }
}
public class TitleQuest_Cooperation1 : QUEST
{
    public TitleQuest_Cooperation1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Cooperation1;
        rewardExp = () => 5000000d;
        rewardTitleKind = TitleKind.Cooperation;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 50;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.rebirthCtrl.Rebirth(heroKind, 0).rebirthNum > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.Research));
    }
}
public class TitleQuest_Cooperation2 : QUEST
{
    public TitleQuest_Cooperation2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Cooperation2;
        rewardExp = () => 1.5e8d;
        rewardTitleKind = TitleKind.Cooperation;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 150;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.rebirthCtrl.Rebirth(heroKind, 1).rebirthNum > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Cooperation1, heroKind));
    }
}
public class TitleQuest_Cooperation3 : QUEST
{
    public TitleQuest_Cooperation3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Cooperation3;
        rewardExp = () => 1.5e9d;
        rewardTitleKind = TitleKind.Cooperation;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 250;
    }
    public override void StartQuest()
    {
        clearConditions.Add(() => game.rebirthCtrl.Rebirth(heroKind, 2).rebirthNum > 0);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Cooperation2, heroKind));
    }
}

public class TitleQuest_Quester1 : QUEST
{
    public TitleQuest_Quester1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester1;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 1;
        unlockHeroLevel = () => 150;
        questerRequiredClearNum = 100;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Cooperation1, heroKind));
    }
}
public class TitleQuest_Quester2 : QUEST
{
    public TitleQuest_Quester2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester2;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 2;
        unlockHeroLevel = () => 200;
        questerRequiredClearNum = 500;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester1, heroKind));
    }
}
public class TitleQuest_Quester3 : QUEST
{
    public TitleQuest_Quester3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester3;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 3;
        unlockHeroLevel = () => 250;
        questerRequiredClearNum = 2000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester2, heroKind));
    }
}
public class TitleQuest_Quester4 : QUEST
{
    public TitleQuest_Quester4(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester4;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 4;
        unlockHeroLevel = () => 300;
        questerRequiredClearNum = 4000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester3, heroKind));
    }
}
public class TitleQuest_Quester5 : QUEST
{
    public TitleQuest_Quester5(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester5;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 5;
        unlockHeroLevel = () => 350;
        questerRequiredClearNum = 10000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester4, heroKind));
    }
}
public class TitleQuest_Quester6 : QUEST
{
    public TitleQuest_Quester6(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester6;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 6;
        unlockHeroLevel = () => 400;
        questerRequiredClearNum = 20000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester5, heroKind));
    }
}
public class TitleQuest_Quester7 : QUEST
{
    public TitleQuest_Quester7(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester7;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 7;
        unlockHeroLevel = () => 450;
        questerRequiredClearNum = 50000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester6, heroKind));
    }
}
public class TitleQuest_Quester8 : QUEST
{
    public TitleQuest_Quester8(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester8;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 8;
        unlockHeroLevel = () => 500;
        questerRequiredClearNum = 100000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester7, heroKind));
    }
}
public class TitleQuest_Quester9 : QUEST
{
    public TitleQuest_Quester9(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester9;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 9;
        unlockHeroLevel = () => 550;
        questerRequiredClearNum = 250000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester8, heroKind));
    }
}
public class TitleQuest_Quester10 : QUEST
{
    public TitleQuest_Quester10(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.Title;
        kindTitle = QuestKindTitle.Quester10;
        rewardTitleKind = TitleKind.Quester;
        rewardTitleLevel = 10;
        unlockHeroLevel = () => 600;
        questerRequiredClearNum = 500000;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = main.SR.generalQuestClearNumsPerClass[(int)heroKind];
    }
    public override double questerClearNum { get => main.SR.generalQuestClearNumsPerClass[(int)heroKind] - initDefeatedNum; }
    public override void StartQuest()
    {
        clearConditions.Add(() => questerClearNum >= questerRequiredClearNum);
        unlockQuests.Add(game.questCtrl.Quest(QuestKindTitle.Quester9, heroKind));
    }
}
