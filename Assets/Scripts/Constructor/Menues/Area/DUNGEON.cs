using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;

public class DUNGEON : AREA
{
    public override bool isDungeon => true;
    public override float limitTime => 5 * 60 + (float)addLimitTime.Value();//5分
    public DUNGEON(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        uniqueEquipmentKind = EquipmentKind.Nothing;
    }
    public override void SetAllWave()
    {
        while (true)
        {
            if (wave.Count >= defaultMaxWaveNum + level.maxValue() * 20) break;
            SetWave();
        }
    }
    public override double RewardExp()
    {
        if (averageMonsterLevel < 100)
            return Parameter.RequiredExp((long)averageMonsterLevel);
        //100以上の場合は微妙になっていく
        if (averageMonsterLevel < 1000)
            return Parameter.RequiredExp((long)(100d + (averageMonsterLevel - 100d) * (1 - 0.0005d * averageMonsterLevel)));//200で190, 250で231, 300で270、400:340、500:400換算
        //1000以上は今のところ考えない
        return 0;
    }
    public override double RewardGold()
    {
        //return 0;
        return Math.Floor(averageMonsterLevel * 80d / 1000d) * 1000d;
    }
    public override double TownMaterialRewardNum(HeroKind heroKind)
    {
        return Math.Floor(
            (Math.Pow(averageMonsterLevel, 1.15d) + averageMonsterLevel * 3 + areaCtrl.townMaterialGainBonus.Value() + areaCtrl.townMaterialGainBonusClass[(int)heroKind].Value())
            * areaCtrl.townMaterialDungeonRewardMultiplier.Value()
            * game.townCtrl.townMaterialGainMultiplier[(int)heroKind].Value());
    }
    double averageMonsterLevel => (MaxLevel() + MinLevel()) / 2d;
} 

//Slime
public class Dungeon0_0 : DUNGEON
{
    public override long minLevel => 20;
    public override long maxLevel => 25;
    public Dungeon0_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 100);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x) * 5);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                wave.Add(RandomWave(1));
            }
            wave.Add(DefaultWave(2, MonsterColor.Blue));
        }
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                wave.Add(RandomWave(1));
            }
            wave.Add(DefaultWave(2, MonsterColor.Yellow));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(1, MonsterColor.Purple, MaxLevel));
    }
}
public class Dungeon0_1 : DUNGEON
{
    public override long minLevel => 40;
    public override long maxLevel => 55;
    public Dungeon0_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 250);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x) * 5);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Purple));//50
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon0_2 : DUNGEON
{
    public override long minLevel => 70;
    public override long maxLevel => 80;
    public Dungeon0_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x) * 5);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(3));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Rare));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon0_3 : DUNGEON
{
    public override long minLevel => 130;
    public override long maxLevel => 150;
    public Dungeon0_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.MonsterFluid), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.MonsterFluid), (x) => materialRewardNum * 10);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(4));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(6, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
//MagicSlime
public class Dungeon1_0 : DUNGEON
{
    public override long minLevel => 35;
    public override long maxLevel => 45;
    public Dungeon1_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 250);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.JasperShard), (x) => TownMaterialRewardNum(x) * 5);

        debuffElement[(int)Element.Fire] = -0.5;
        debuffElement[(int)Element.Ice] = -0.5;
        debuffElement[(int)Element.Thunder] = -0.5;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Normal;
    public override void SetWave()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                wave.Add(RandomWave(1));
            }
            wave.Add(DefaultWave(2, MonsterColor.Blue));
        }
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                wave.Add(RandomWave(1));
            }
            wave.Add(DefaultWave(2, MonsterColor.Yellow));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(3, MonsterColor.Red, MaxLevel));
    }
}
public class Dungeon1_1 : DUNGEON
{
    public override long minLevel => 55;
    public override long maxLevel => 70;
    public Dungeon1_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MudBrick), (x) => TownMaterialRewardNum(x) * 5);

        debuffElement[(int)Element.Fire] = -0.5;
        debuffElement[(int)Element.Ice] = -0.5;
        debuffElement[(int)Element.Thunder] = -0.5;
        debuffElement[(int)Element.Light] = -0.5;
        debuffElement[(int)Element.Dark] = -0.5;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(1));
        }
        wave.Add(DefaultWave(2, MonsterColor.Purple));//50
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon1_2 : DUNGEON
{
    public override long minLevel => 80;
    public override long maxLevel => 100;
    public Dungeon1_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 1000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.PineLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Fire] = -0.75;
        debuffElement[(int)Element.Ice] = -0.75;
        debuffElement[(int)Element.Thunder] = -0.75;
        debuffElement[(int)Element.Light] = -0.75;
        debuffElement[(int)Element.Dark] = -0.75;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(5, MonsterColor.Purple));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon1_3 : DUNGEON
{
    public override long minLevel => 150;
    public override long maxLevel => 170;
    public Dungeon1_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.FrostShard), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.FrostShard), (x) => materialRewardNum * 10);
        debuffElement[(int)Element.Ice] = -1.00;
    }
    public override void SetWave()
    {
        for (int i = 0; i < 50; i++)
        {
            wave.Add(RandomWave(3, MonsterColor.Blue));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterColor.Blue));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(7, MonsterColor.Blue));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(9, MonsterColor.Blue));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon1_4 : DUNGEON
{
    public override long minLevel => 170;
    public override long maxLevel => 190;
    public Dungeon1_4(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.LightningShard), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.LightningShard), (x) => materialRewardNum * 10);
        debuffElement[(int)Element.Thunder] = -1.00;
    }
    public override void SetWave()
    {
        for (int i = 0; i < 50; i++)
        {
            wave.Add(RandomWave(3, MonsterColor.Yellow));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterColor.Yellow));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(7, MonsterColor.Yellow));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(9, MonsterColor.Yellow));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon1_5 : DUNGEON
{
    public override long minLevel => 190;
    public override long maxLevel => 210;
    public Dungeon1_5(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(4, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.FlameShard), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.FlameShard), (x) => materialRewardNum * 10);
        debuffElement[(int)Element.Fire] = -1.00;
    }
    public override void SetWave()
    {
        for (int i = 0; i < 50; i++)
        {
            wave.Add(RandomWave(3, MonsterColor.Red));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterColor.Red));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(7, MonsterColor.Red));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(9, MonsterColor.Red));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon1_6 : DUNGEON
{
    public override long minLevel => 210;
    public override long maxLevel => 230;
    public Dungeon1_6(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(6, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.NatureShard), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.NatureShard), (x) => materialRewardNum * 10);
        debuffElement[(int)Element.Light] = -1.00;
    }
    public override void SetWave()
    {
        for (int i = 0; i < 50; i++)
        {
            wave.Add(RandomWave(3, MonsterColor.Green));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterColor.Green));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(7, MonsterColor.Green));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(9, MonsterColor.Green));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon1_7 : DUNGEON
{
    public override long minLevel => 230;
    public override long maxLevel => 250;
    public Dungeon1_7(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.PoisonShard), (x) => materialRewardNum);
        rewardMaterialFirst.Add(game.materialCtrl.Material(MaterialKind.PoisonShard), (x) => materialRewardNum * 10);
        debuffElement[(int)Element.Dark] = -1.00;
    }
    public override void SetWave()
    {
        for (int i = 0; i < 50; i++)
        {
            wave.Add(RandomWave(3, MonsterColor.Purple));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterColor.Purple));
        }
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(7, MonsterColor.Purple));
        }
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(9, MonsterColor.Purple));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}
//Spider
public class Dungeon2_0 : DUNGEON
{
    public override long minLevel => 90;
    public override long maxLevel => 110;
    public Dungeon2_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 2000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -0.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        for (int i = 0; i < 99; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(10, MonsterRarity.Common, MaxLevel));
    }
}
public class Dungeon2_1 : DUNGEON
{
    public override long minLevel => 110;
    public override long maxLevel => 130;
    public Dungeon2_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -0.50;
        debuffElement[(int)Element.Fire] = -0.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(3, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(3, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(7));
        }
        wave.Add(DefaultWave(5, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(7));
        }
        wave.Add(DefaultWave(5, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(5, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon2_2 : DUNGEON
{
    public override long minLevel => 170;
    public override long maxLevel => 200;
    public Dungeon2_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -0.50;
        debuffElement[(int)Element.Fire] = -0.50;
        debuffElement[(int)Element.Dark] = -0.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(7));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(7, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(3, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon2_3 : DUNGEON
{
    public override long minLevel => 275;
    public override long maxLevel => 295;
    public Dungeon2_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(6, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardPotion.Add(() => PotionKind.ThrowingNet, () => (long)materialRewardNum * 50);
        rewardEnchantKindFirst = () => EnchantKind.OptionAdd;
        rewardEnchantEffectKindFirst = () => EquipmentEffectKind.TamingPoint;
        rewardEnchantLevelFirst = () => level.value + 1;
        debuffElement[(int)Element.Fire] = -1.00;
        debuffElement[(int)Element.Dark] = -1.00;
        debuffPhyCrit = -0.50;
        debuffMagCrit = -0.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(6));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(8, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(4, MonsterColor.Boss, MaxLevel));
    }
}

//Bat
public class Dungeon3_0 : DUNGEON
{
    public override long minLevel => 120;
    public override long maxLevel => 150;
    public Dungeon3_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(0, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.OpalShard), (x) => TownMaterialRewardNum(x) * 5);
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        for (int i = 0; i < 99; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterRarity.Boss, MaxLevel));
    }
}
public class Dungeon3_1 : DUNGEON
{
    public override long minLevel => 200;
    public override long maxLevel => 230;
    public Dungeon3_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(1, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MapleLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffPhyCrit = -0.20;
        debuffMagCrit = -0.20;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(3, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(3, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(3, MonsterColor.Purple));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(3, MonsterColor.Red));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(3, MonsterColor.Green));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon3_2 : DUNGEON
{
    public override long minLevel => 250;
    public override long maxLevel => 270;
    public Dungeon3_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 2500);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick), (x) => TownMaterialRewardNum(x) * 5);
        debuffPhyCrit = -0.50;
        debuffMagCrit = -0.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon3_3 : DUNGEON
{
    public override long minLevel => 300;
    public override long maxLevel => 320;
    public Dungeon3_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardEnchantKind = () => EnchantKind.OptionAdd;
        rewardEnchantEffectKind = () => EquipmentEffectKind.ATKAdder;
        rewardEnchantLevel = () => level.value + 1;
        rewardEnchantKindFirst = () => EnchantKind.OptionLevelup;
        debuffPhyCrit = -1.00;
        debuffMagCrit = -1.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(4));
        }
        wave.Add(DefaultWave(3, MonsterColor.Boss, MaxLevel));
    }
}

//Fairy
public class Dungeon4_0 : DUNGEON
{
    public override long minLevel => 250;
    public override long maxLevel => 275;
    public Dungeon4_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(4, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Fire] = -1.00;
        debuffElement[(int)Element.Ice] = -1.00;
        debuffElement[(int)Element.Thunder] = -1.00;
        debuffElement[(int)Element.Light] = -1.00;
        debuffElement[(int)Element.Dark] = -1.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 99; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterRarity.Boss, MaxLevel));
    }
}
public class Dungeon4_1 : DUNGEON
{
    public override long minLevel => 320;
    public override long maxLevel => 340;
    public Dungeon4_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Fire] = -1.50;
        debuffElement[(int)Element.Ice] = -1.50;
        debuffElement[(int)Element.Thunder] = -1.50;
        debuffElement[(int)Element.Light] = -1.50;
        debuffElement[(int)Element.Dark] = -1.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(9, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon4_2 : DUNGEON
{
    public override long minLevel => 350;
    public override long maxLevel => 370;
    public Dungeon4_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(6, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Fire] = -2.00;
        debuffElement[(int)Element.Ice] = -2.00;
        debuffElement[(int)Element.Thunder] = -2.00;
        debuffElement[(int)Element.Light] = -2.00;
        debuffElement[(int)Element.Dark] = -2.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon4_3 : DUNGEON
{
    public override long minLevel => 400;
    public override long maxLevel => 420;
    public Dungeon4_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 5000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardEnchantKind = () => EnchantKind.OptionAdd;
        rewardEnchantEffectKind = () => EquipmentEffectKind.MATKAdder;
        rewardEnchantLevel = () => level.value + 1;
        rewardEnchantKindFirst = () => EnchantKind.OptionLevelup;
        debuffElement[(int)Element.Fire] = -2.00;
        debuffElement[(int)Element.Ice] = -2.00;
        debuffElement[(int)Element.Thunder] = -2.00;
        debuffElement[(int)Element.Light] = -2.00;
        debuffElement[(int)Element.Dark] = -2.00;
        debuffPhyCrit = -1.00;
        debuffMagCrit = -1.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}

//ここから
//Fox
public class Dungeon5_0 : DUNGEON
{
    public override long minLevel => 340;
    public override long maxLevel => 360;
    public Dungeon5_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(2, 25000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Thunder] = -2.00;
        debuffElement[(int)Element.Light] = -2.00;
        debuffPhyCrit = -1.00;
        debuffMagCrit = -1.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(RandomWave(10));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
            wave.Add(RandomWave(2, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(1, MonsterRarity.Boss, MaxLevel));
    }
}
public class Dungeon5_1 : DUNGEON
{
    public override long minLevel => 375;
    public override long maxLevel => 400;
    public Dungeon5_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 20000);
        requiredCompleteNum.Add(4, 20000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.AshLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Thunder] = -3.50;
        debuffElement[(int)Element.Light] = -3.50;
        debuffPhyCrit = -1.50;
        debuffMagCrit = -1.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(4, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(9, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon5_2 : DUNGEON
{
    public override long minLevel => 420;
    public override long maxLevel => 440;
    public Dungeon5_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 25000);
        requiredCompleteNum.Add(6, 25000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Thunder] = -5.00;
        debuffElement[(int)Element.Light] = -5.00;
        debuffPhyCrit = -2.00;
        debuffMagCrit = -2.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterRarity.Common));
        }
        for (int i = 0; i < 29; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon5_3 : DUNGEON
{
    public override long minLevel => 460;
    public override long maxLevel => 480;
    public Dungeon5_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 50000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardEnchantKind = () => EnchantKind.OptionAdd;
        rewardEnchantEffectKind = () => EquipmentEffectKind.DEFAdder;
        rewardEnchantLevel = () => level.value + 1;
        rewardEnchantKindFirst = () => EnchantKind.OptionLevelup;
        debuffElement[(int)Element.Thunder] = -5.00;
        debuffElement[(int)Element.Light] = -5.00;
        debuffPhyCrit = -2.50;
        debuffMagCrit = -2.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}

//Devilfish
public class Dungeon6_0 : DUNGEON
{
    public override long minLevel => 395;
    public override long maxLevel => 415;
    public Dungeon6_0(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(3, 50000);
        requiredCompleteNum.Add(4, 50000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -3.50;
        debuffElement[(int)Element.Fire] = -3.50;
        debuffPhyCrit = -2.00;
        debuffMagCrit = -2.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Common;
    public override void SetWave()
    {
        wave.Add(RandomWave(10));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
            wave.Add(RandomWave(5, MonsterRarity.Uncommon));
        }
        wave.Add(DefaultWave(1, MonsterRarity.Boss, MaxLevel));
    }
}
public class Dungeon6_1 : DUNGEON
{
    public override long minLevel => 440;
    public override long maxLevel => 460;
    public Dungeon6_1(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(5, 50000);
        requiredCompleteNum.Add(6, 50000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -5.00;
        debuffElement[(int)Element.Fire] = -5.00;
        debuffElement[(int)Element.Dark] = -5.00;
        debuffPhyCrit = -2.50;
        debuffMagCrit = -2.50;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(2));
        }
        wave.Add(DefaultWave(5, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(7, MonsterRarity.Rare));
        for (int i = 0; i < 19; i++)
        {
            wave.Add(RandomWave(4, MonsterRarity.Common));
        }
        wave.Add(DefaultWave(9, MonsterRarity.Rare));
        for (int i = 0; i < 9; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
    }
}

public class Dungeon6_2 : DUNGEON
{
    public override long minLevel => 480;
    public override long maxLevel => 500;
    public Dungeon6_2(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 25000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardMaterial.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x));
        rewardMaterialFirst.Add(game.townCtrl.TownMaterial(TownMaterialKind.JadeShard), (x) => TownMaterialRewardNum(x) * 5);
        debuffElement[(int)Element.Ice] = -7.50;
        debuffElement[(int)Element.Fire] = -7.50;
        debuffElement[(int)Element.Dark] = -7.50;
        debuffPhyCrit = -3.00;
        debuffMagCrit = -3.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Uncommon;
    public override void SetWave()
    {
        for (int i = 0; i < 20; i++)
        {
            wave.Add(RandomWave(5, MonsterRarity.Common));
        }
        for (int i = 0; i < 29; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3, MonsterRarity.Rare));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
public class Dungeon6_3 : DUNGEON
{
    public override long minLevel => 520;
    public override long maxLevel => 540;
    public Dungeon6_3(AreaController areaCtrl, AreaKind kind, int id) : base(areaCtrl, kind, id)
    {
        requiredCompleteNum.Add(7, 50000);
        rewardMaterial.Add(game.materialCtrl.Material(MaterialKind.EnchantedShard), (x) => enchantedShardRewardNum);
        rewardEnchantKind = () => EnchantKind.OptionAdd;
        rewardEnchantEffectKind = () => EquipmentEffectKind.MDEFAdder;
        rewardEnchantLevel = () => level.value + 1;
        rewardEnchantKindFirst = () => EnchantKind.OptionLevelup;
        debuffElement[(int)Element.Ice] = -10.00;
        debuffElement[(int)Element.Fire] = -10.00;
        debuffElement[(int)Element.Dark] = -10.00;
        debuffPhyCrit = -4.00;
        debuffMagCrit = -4.00;
    }
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(3));
        }
        wave.Add(DefaultWave(1, MonsterColor.Boss, MaxLevel));
        for (int i = 0; i < 49; i++)
        {
            wave.Add(RandomWave(5));
        }
        wave.Add(DefaultWave(2, MonsterColor.Boss, MaxLevel));
    }
}
