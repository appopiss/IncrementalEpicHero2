using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static UsefulMethod;
using static MaterialKind;

public class CraftController
{
    public CraftController()
    {
        costReduction = new Multiplier(() => 0.90d, () => 0);
        scrollList.Add(new CRAFT_SCROLL(this));
        scrollList.Add(new CraftDeleteScroll(this));
        scrollList.Add(new CraftExtractScroll(this));
        scrollList.Add(new CraftLotteryScroll(this));
        scrollList.Add(new CraftLevelUpScroll(this));
        scrollList.Add(new CraftLevelMaxScroll(this));
        scrollList.Add(new CraftCopyScroll(this));
        scrollList.Add(new CraftExpandScroll(this));
        for (int i = 0; i < Enum.GetNames(typeof(EquipmentEffectKind)).Length; i++)
        {
            int count = i;
            enchantScrollList.Add(new CraftEnchantScroll(this, (EquipmentEffectKind)count));
        }
        for (int i = 1; i < enchantScrollList.Count; i++)
        {
            int count = i;
            switch (EquipmentParameter.RarityFactor(enchantScrollList[count].kind))
            {
                case 1:
                    enchantScroll1List.Add(enchantScrollList[count]);
                    break;
                case 2:
                    enchantScroll2List.Add(enchantScrollList[count]);
                    break;
                case 3:
                    enchantScroll3List.Add(enchantScrollList[count]);
                    break;
                case 4:
                    enchantScroll4List.Add(enchantScrollList[count]);
                    break;
                case 5:
                    enchantScroll5List.Add(enchantScrollList[count]);
                    break;

            }
        }
    }
    public Multiplier costReduction;
    //public List<CRAFT> craftLists = new List<CRAFT>();
    public List<CRAFT_SCROLL> scrollList = new List<CRAFT_SCROLL>();
    public List<CraftEnchantScroll> enchantScrollList = new List<CraftEnchantScroll>();
    //Rarity
    public List<CraftEnchantScroll> enchantScroll1List = new List<CraftEnchantScroll>();
    public List<CraftEnchantScroll> enchantScroll2List = new List<CraftEnchantScroll>();
    public List<CraftEnchantScroll> enchantScroll3List = new List<CraftEnchantScroll>();
    public List<CraftEnchantScroll> enchantScroll4List = new List<CraftEnchantScroll>();
    public List<CraftEnchantScroll> enchantScroll5List = new List<CraftEnchantScroll>();

    public CRAFT_SCROLL CraftScroll(EnchantKind kind, EquipmentEffectKind effectKind = EquipmentEffectKind.Nothing)
    {
        if (kind == EnchantKind.OptionAdd)
        {
            for (int i = 0; i < enchantScrollList.Count; i++)
            {
                if (enchantScrollList[i].kind == effectKind) return enchantScrollList[i];
            }
            return enchantScrollList[0];
        }
        for (int i = 0; i < scrollList.Count; i++)
        {
            if (scrollList[i].enchantKind == kind) return scrollList[i];
        }
        return scrollList[0];
    }
}

public class CraftDeleteScroll : CRAFT_SCROLL
{
    public CraftDeleteScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }
    public override EnchantKind enchantKind => EnchantKind.OptionDelete;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 5);
        transaction.SetAnotherResource(game.materialCtrl.Material(SlimeBall), (x) => 10);
    }
}
public class CraftExtractScroll : CRAFT_SCROLL
{
    public CraftExtractScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.OptionExtract;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 20);
        transaction.SetAnotherResource(game.materialCtrl.Material(ManaSeed), (x) => 10);
    }
}
public class CraftLotteryScroll : CRAFT_SCROLL
{
    public CraftLotteryScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.OptionLottery;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 20);
        transaction.SetAnotherResource(game.materialCtrl.Material(UnmeltingIce), (x) => 10);
    }
}
public class CraftLevelUpScroll : CRAFT_SCROLL
{
    public CraftLevelUpScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.OptionLevelup;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 100);
        transaction.SetAnotherResource(game.materialCtrl.Material(EternalFlame), (x) => 10);
    }
}
public class CraftLevelMaxScroll : CRAFT_SCROLL
{
    public CraftLevelMaxScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.OptionLevelMax;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 500);
        transaction.SetAnotherResource(game.materialCtrl.Material(AncientBattery), (x) => 10);
    }
}
public class CraftCopyScroll : CRAFT_SCROLL
{
    public CraftCopyScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.OptionCopy;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 2000);
        transaction.SetAnotherResource(game.materialCtrl.Material(Ectoplasm), (x) => 10);
    }
}
public class CraftExpandScroll : CRAFT_SCROLL
{
    public CraftExpandScroll(CraftController craftCtrl) : base(craftCtrl)
    {
        SetCost();
    }

    public override EnchantKind enchantKind => EnchantKind.ExpandEnchantSlot;
    public override void SetCost()
    {
        transaction.SetAnotherResource(game.materialCtrl.Material(EnchantedShard), (x) => 10000);
        transaction.SetAnotherResource(game.materialCtrl.Material(Stardust), (x) => 10);
    }
}
