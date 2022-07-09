using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static Parameter;
using System;

public class GeneralQuest
{
}
public class GeneralQuest_Area0_0 : CompleteAreaQuest
{
    public GeneralQuest_Area0_0(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteArea0_0;
        completeTargetArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 0);
        areaRequredCompletedNum = () =>  1;
        rewardExp = () => 250;
        rewardGold = () => 100;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGlobal.AbilityVIT));
    }
}
public class GeneralQuest_Area0_1 : CompleteAreaQuest
{
    public GeneralQuest_Area0_1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteArea0_1;
        completeTargetArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 1);
        areaRequredCompletedNum = () =>  1;
        rewardGold = () => 500;
        rewardExp = () => 1000;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_0, heroKind));
    }
}
public class GeneralQuest_Area0_2 : CompleteAreaQuest
{
    public GeneralQuest_Area0_2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteArea0_2;
        completeTargetArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 2);
        areaRequredCompletedNum = () =>  1;
        rewardGold = () => 750;
        rewardExp = () => 5500;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_1, heroKind));
    }
}
public class GeneralQuest_Area0_3 : CompleteAreaQuest
{
    public GeneralQuest_Area0_3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteArea0_3;
        completeTargetArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 3);
        areaRequredCompletedNum = () =>  1;
        rewardGold = () => 1000;
        rewardExp = () => 25000;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_2, heroKind));
    }
}

public class GeneralQuest_DefeatNormalSlime1 : DefeatQuest
{
    public GeneralQuest_DefeatNormalSlime1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatNormalSlime1;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal);
        defeatRequredDefeatNum = () => 100;
        rewardGold = () => 500;
        rewardExp = () => 1500;
        rewardMaterial.Add(() => MaterialKind.OilOfSlime, () => 5);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_0, heroKind));
    }
}
public class GeneralQuest_DefeatNormalSlime2 : DefeatQuest
{
    public GeneralQuest_DefeatNormalSlime2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatNormalSlime2;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 10;
        rewardGold = () => 2000;
        rewardExp = () => 7500;
        rewardMaterial.Add(() => MaterialKind.OilOfSlime, () => 50);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatNormalSlime1, heroKind));
    }
}
public class GeneralQuest_DefeatNormalSlime3 : DefeatQuest
{
    public GeneralQuest_DefeatNormalSlime3(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatNormalSlime3;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal);
        defeatRequredDefeatNum = () => 10000;
        unlockHeroLevel = () => 30;
        rewardGold = () => 3000;
        rewardMaterial.Add(() => MaterialKind.OilOfSlime, () => 500);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatNormalSlime2, heroKind));
    }
}

public class GeneralQuest_OilOfSlime : BringQuest
{
    public GeneralQuest_OilOfSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.BringOilOfSlime;
        questingArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 0);
        bringRequiredMaterials.Add(MaterialKind.OilOfSlime, 30);
        unlockHeroLevel = () => 10;
        rewardGold = () => 1000;
        rewardExp = () => 20000;
        rewardMaterial.Add(() => MaterialKind.MonsterFluid, () => 5);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_0, heroKind));
    }
}

public class GeneralQuest_DefeatRedSlime : DefeatQuest
{
    public GeneralQuest_DefeatRedSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatRedSlime;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Red);
        defeatRequredDefeatNum = () => 50;
        rewardGold = () => 2000;
        unlockHeroLevel = () => 25;
        rewardMaterial.Add(() => MaterialKind.MonsterFluid, () => 5);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_0, heroKind));
    }
}
public class GeneralQuest_Dungeon0_0 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon0_0(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon0_0;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SlimeVillage, 0);
        areaRequredCompletedNum = () => 1;
        rewardGold = () => 3000;
        rewardExp = () => 100000;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_Dungeon0_1 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon0_1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon0_1;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SlimeVillage, 1);
        areaRequredCompletedNum = () => 1;
        rewardGold = () => 4000;
        rewardExp = () => 500000;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteDungeon0_0, heroKind));
    }
}
public class GeneralQuest_Dungeon0_2 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon0_2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon0_2;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SlimeVillage, 2);
        areaRequredCompletedNum = () => 1;
        rewardGold = () => 5000;
        rewardExp = () => 2000000;
        rewardMaterialNumber.Add(() => game.materialCtrl.Material(MaterialKind.EnchantedShard), () => 1);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteDungeon0_1, heroKind));
    }
}
public class GeneralQuest_DefeatNormalMagicSlime : DefeatQuest
{
    public GeneralQuest_DefeatNormalMagicSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatNormalMagicSlime;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Blue);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 70;
        rewardGold = () => 3000;
        rewardMaterial.Add(() => MaterialKind.EnchantedCloth, () => 50);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.MagicSlimeCity].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_DefeatRedMagicSlime : DefeatQuest
{
    public GeneralQuest_DefeatRedMagicSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatRedMagicSlime;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Red);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 80;
        rewardGold = () => 4000;
        rewardMaterial.Add(() => MaterialKind.FlameShard, () => 5);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.MagicSlimeCity].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_DefeatGreenMagicSlime : DefeatQuest
{
    public GeneralQuest_DefeatGreenMagicSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatGreenMagicSlime;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Green);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 125;
        rewardGold = () => 5000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.MagicSlimeCity].IsUnlocked()); 
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_DefeatYellowBat : DefeatQuest
{
    public GeneralQuest_DefeatYellowBat(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatYellowBat;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Yellow);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 125;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.BatCave].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_DefeatRedBat : DefeatQuest
{
    public GeneralQuest_DefeatRedBat(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatRedBat;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Red);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 200;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatYellowBat, heroKind));
    }
}
public class GeneralQuest_DefeatGreenBat : DefeatQuest
{
    public GeneralQuest_DefeatGreenBat(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatGreenBat;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Green);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 250;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatRedBat, heroKind));
    }
}
public class GeneralQuest_DefeatPurpleBat : DefeatQuest
{
    public GeneralQuest_DefeatPurpleBat(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatPurpleBat;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Purple);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 300;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value())); ;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatGreenBat, heroKind));
    }
}
public class GeneralQuest_DefeatBlueBat : DefeatQuest
{
    public GeneralQuest_DefeatBlueBat(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.DefeatBlueBat;
        defeatTargetMonster = game.monsterCtrl.GlobalInformation(MonsterSpecies.Bat, MonsterColor.Blue);
        defeatRequredDefeatNum = () => 1000;
        unlockHeroLevel = () => 100;
        rewardGold = () => 5000;
        rewardMaterial.Add(() => MaterialKind.BatWing, () => 50);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.BatCave].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_BringToEnchantShard : BringQuest
{
    public GeneralQuest_BringToEnchantShard(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.BringToEnchantShard;
        questingArea = game.areaCtrl.Area(AreaKind.SlimeVillage, 0);
        bringRequiredMaterials.Add(MaterialKind.OilOfSlime, 200);
        bringRequiredMaterials.Add(MaterialKind.EnchantedCloth, 200);
        bringRequiredMaterials.Add(MaterialKind.SpiderSilk, 200);
        unlockHeroLevel = () => 100;
        rewardGold = () => 5000;
        rewardMaterialNumber.Add(() => game.materialCtrl.Material(MaterialKind.EnchantedShard), () => 1);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.BatCave].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.DefeatBlueBat, heroKind));
    }
}
public class GeneralQuest_Dungeon2_0 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon2_0(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon2_0;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SpiderMaze, 0);
        areaRequredCompletedNum = () => 1;
        unlockHeroLevel = () => 90;
        rewardGold = () => 5000;
        rewardExp = () => 2000000;
        rewardMaterial.Add(() => MaterialKind.SpiderSilk, () => 50);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.SpiderMaze].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_Dungeon2_1 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon2_1(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon2_1;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SpiderMaze, 1);
        areaRequredCompletedNum = () => 1;
        unlockHeroLevel = () => 110;
        rewardGold = () => 10000;
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteDungeon2_0, heroKind));
    }
}
public class GeneralQuest_CaptureNormalSpider : CaptureQuest
{
    public GeneralQuest_CaptureNormalSpider(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureNormalSpider;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Spider, MonsterColor.Normal);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 125;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.PineLog), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteDungeon2_1, heroKind));
    }
}
public class GeneralQuest_Dungeon2_2 : CompleteAreaQuest
{
    public GeneralQuest_Dungeon2_2(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CompleteDungeon2_2;
        completeTargetArea = game.areaCtrl.Dungeon(AreaKind.SpiderMaze, 2);
        areaRequredCompletedNum = () => 1;
        unlockHeroLevel = () => 170;
        rewardGold = () => 10000;
        rewardMaterialNumber.Add(() => game.materialCtrl.Material(MaterialKind.EnchantedShard), () => 1);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CaptureNormalSpider, heroKind));
    }
}
public class GeneralQuest_CaptureYellowSlime : CaptureQuest
{
    public GeneralQuest_CaptureYellowSlime(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureYellowSlime;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Slime, MonsterColor.Yellow);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 250;
        rewardGold = () => 20000;
        rewardMaterialNumber.Add(() => game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), () => (long)(200 * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value()));
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.shopCtrl.Trap(PotionKind.ThunderRope).unlock.IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}


public class GeneralQuest_CaptureNormalFairy : CaptureQuest
{
    public GeneralQuest_CaptureNormalFairy(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureNormalFairy;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Fairy, MonsterColor.Normal);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 110;
        rewardGold = () => 10000;
        rewardPotion.Add(() => PotionKind.IceRope, () => 20);
    }
    public override void StartQuest()
    {
        unlockConditions.Add(() => game.areaCtrl.unlocks[(int)AreaKind.FairyGarden].IsUnlocked());
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CompleteArea0_3, heroKind));
    }
}
public class GeneralQuest_CaptureBlueFairy : CaptureQuest
{
    public GeneralQuest_CaptureBlueFairy(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureBlueFairy;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Fairy, MonsterColor.Blue);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 150;
        rewardGold = () => 10000;
        rewardPotion.Add(() => PotionKind.ThunderRope, () => 20);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CaptureNormalFairy, heroKind));
    }
}
public class GeneralQuest_CaptureYellowFairy : CaptureQuest
{
    public GeneralQuest_CaptureYellowFairy(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureYellowFairy;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Fairy, MonsterColor.Yellow);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 180;
        rewardGold = () => 10000;
        rewardPotion.Add(() => PotionKind.FireRope, () => 20);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CaptureBlueFairy, heroKind));
    }
}
public class GeneralQuest_CaptureRedFairy : CaptureQuest
{
    public GeneralQuest_CaptureRedFairy(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureRedFairy;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Fairy, MonsterColor.Red);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 215;
        rewardGold = () => 10000;
        rewardPotion.Add(() => PotionKind.LightRope, () => 20);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CaptureRedFairy, heroKind));
    }
}
public class GeneralQuest_CaptureGreenFairy : CaptureQuest
{
    public GeneralQuest_CaptureGreenFairy(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        kind = QuestKind.General;
        kindGeneral = QuestKindGeneral.CaptureGreenFairy;
        captureTargetMonster = game.monsterCtrl.Monster(MonsterSpecies.Fairy, MonsterColor.Green);
        captureRequiredNum = () => 100;
        unlockHeroLevel = () => 300;
        rewardGold = () => 10000;
        rewardPotion.Add(() => PotionKind.DarkRope, () => 20);
    }
    public override void StartQuest()
    {
        unlockQuests.Add(game.questCtrl.Quest(QuestKindGeneral.CaptureGreenFairy, heroKind));
    }
}
