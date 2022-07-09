using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static Main;
using static UsefulMethod;
using static Parameter;
using System;

public partial class Save
{
    public double openedChestNum;
}

public class TreasureChest
{
    public TreasureChest(BattleController battleCtrl, TreasureChestGenerator generator)
    {
        this.battleCtrl = battleCtrl;
        this.generator = generator;
        openedNum = new OpenedChestNum();
    }
    public void Open()
    {
        openedNum.Increase(1);
        EffectAction();
    }
    public virtual void EffectAction() { }
    public virtual string OpenString() { return ""; }
    public BattleController battleCtrl;
    public TreasureChestGenerator generator;
    public OpenedChestNum openedNum;
    public virtual ChestKind kind { get; }
    public long monsterLevel;
}

public class Chest_Mimic : TreasureChest
{
    public Chest_Mimic(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.Mimic;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) return;
        battleCtrl.areaBattle.SpawnMonster(MonsterSpecies.Mimic, MonsterColor.Normal, monsterLevel, 0, generator.position, false);
    }
    public override string OpenString()
    {
        return "<color=orange>The chest was Mimic!!!";
    }
}
public class Chest_Blessing : TreasureChest
{
    public Chest_Blessing(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.Blessing;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) return;
        Blessing();
    }
    async void Blessing() { await battleCtrl.areaBattle.CurrentArea().SelectBlessing(battleCtrl); }
    public override string OpenString()
    {
        return "<color=green>Blessing!";
    }
}
public class Chest_LimitTime : TreasureChest
{
    public Chest_LimitTime(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.ExpandLimitTime;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) battleCtrl.areaBattle.CurrentArea().simulatedAdditionalTime += 30;
        //else
            battleCtrl.areaBattle.CurrentArea().additionalTime[(int)battleCtrl.heroKind] += 30;
    }
    public override string OpenString()
    {
        return "<color=green>Time Limit + 30 sec!";
    }
}
public class Chest_PortalOrb : TreasureChest
{
    public Chest_PortalOrb(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.PortalOrb;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) return;
        game.areaCtrl.portalOrb.Increase(1);
    }
    public override string OpenString()
    {
        return "<color=green>1 Portal Orb!";
    }
}
public class Chest_TownMaterial : TreasureChest
{
    public Chest_TownMaterial(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.TownMaterial;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) return;
        foreach (var item in battleCtrl.areaBattle.CurrentArea().rewardMaterialFirst)
        {
            item.Key.Increase(Math.Ceiling(item.Value(battleCtrl.heroKind) / 25d));
            return;//１種類だけ
        }
    }
    public override string OpenString()
    {
        foreach (var item in battleCtrl.areaBattle.CurrentArea().rewardMaterialFirst)
        {
            return "<color=green>" + tDigit(Math.Ceiling(item.Value(battleCtrl.heroKind) / 25d)) + " " + item.Key.Name();
        }
        return "";
    }
}
public class Chest_Talisman : TreasureChest
{
    public Chest_Talisman(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.Talisman;
    public override void EffectAction()
    {
        //if (battleCtrl.isSimulated) return;
        PotionKind tempTalisman = MonsterParameter.SpeciesTalismanKind(battleCtrl.areaBattle.CurrentArea().kind);
        game.inventoryCtrl.CreatePotion(tempTalisman, 1);
    }
    public override string OpenString()
    {
        return "<color=green>Talisman!";
    }
}

public class Chest_Gold : TreasureChest
{
    public Chest_Gold(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
    {
    }
    public override ChestKind kind => ChestKind.Gold;
    public override void EffectAction()
    {
        double value = battleCtrl.areaBattle.CurrentArea().rewardGold() / 2d;
        game.resourceCtrl.gold.Increase(value);
    }
    public override string OpenString()
    {
        double value = battleCtrl.areaBattle.CurrentArea().rewardGold() / 2d * game.statsCtrl.GoldGain().Value() + game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();
        return optStr + "<color=green><sprite=\"resource\" index=0> " + tDigit(value) + " Gold";
    }
}


//public class Chest_Material : TreasureChest
//{
//    public Chest_Material(BattleController battleCtrl, TreasureChestGenerator generator) : base(battleCtrl, generator)
//    {
//    }
//    public override ChestKind kind => ChestKind.Material;
//    public override void EffectAction()
//    {
//        game.materialCtrl.Material(MaterialKind.)
//    }
//    public override string OpenString()
//    {
//        return "<color=green>Limited Time + 30 sec!";
//    }
//}


public class OpenedChestNum : NUMBER
{
    public override double value { get => main.S.openedChestNum; set => main.S.openedChestNum = value; }
}

public class TreasureChestGenerator : DROP_GENERATOR
{
    public TreasureChestGenerator(BattleController battleCtrl, HeroKind heroKind) : base(heroKind)
    {
        chests[0] = new Chest_Mimic(battleCtrl, this);
        chests[1] = new Chest_Blessing(battleCtrl, this);
        chests[2] = new Chest_LimitTime(battleCtrl, this);
        chests[3] = new Chest_TownMaterial(battleCtrl, this);
        chests[4] = new Chest_Gold(battleCtrl, this);
        chests[5] = new Chest_PortalOrb(battleCtrl, this);
        chests[6] = new Chest_Talisman(battleCtrl, this);
    }
    public bool isActive { get => num > 0; }
    public void Drop(long monsterLevel, Vector2 position)
    {
        num++;
        if (position.x > moveRangeX / 2f || position.x < -moveRangeX / 2f || position.y > moveRangeY / 2f - 50f || position.y < -moveRangeY / 2f)
            position = Vector2.zero;
        this.position = position;
        if (WithinRandom(0.001d)) currentChest = chests[5];
        else if (WithinRandom(game.areaCtrl.chestPortalOrbChance.Value())) currentChest = chests[4];
        else
        {
            int rand = UnityEngine.Random.Range(0, 5);
            currentChest = chests[rand];
            if (rand == 0) currentChest.monsterLevel = monsterLevel;
        }
        if (game.IsUI(heroKind) && dropUIAction != null) dropUIAction();

        //AutoOpenChest
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.OpenChest))
            Get();
    }
    public override void Get()
    {
        currentChest.Open();
        Initialize();
    }
    public TreasureChest currentChest;
    TreasureChest[] chests = new TreasureChest[Enum.GetNames(typeof(ChestKind)).Length];
    public ChestRarity chestRarity;
    //public BattleController battleCtrl { get => game.battleCtrls[(int)heroKind]; }
}

public enum ChestRarity
{
    Wooden,
    Silver,
    Golden,
}

public enum ChestKind
{
    Mimic,
    Blessing,
    ExpandLimitTime,
    TownMaterial,
    Gold,
    PortalOrb,
    Talisman,//Monsterのバッジ
    //Material,//Portal Orb, Portal Keyなど
    //Gold,
    //Equipment,
    //EnchantScroll,
}
