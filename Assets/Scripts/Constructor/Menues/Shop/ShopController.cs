using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static PotionParameter;
using static UsefulMethod;

public partial class SaveR
{
    public long[] purchasedNumMaterials;
    public long[] purchasedNumTraps;
    public long purchasedNumBlessing;
    public long[] purchasedNumTownMaterials;
    public float shopRestockTimecount;

    public bool[] isAutoBuyBlessings;
}

public class ShopController
{
    public void Start()
    {
        for (int i = 0; i < shop_MaterialList.Count; i++)
        {
            shop_MaterialList[i].Start();
        }
        for (int i = 0; i < shop_TrapList.Count; i++)
        {
            shop_TrapList[i].Start();
        }
        for (int i = 0; i < shop_BlessingList.Count; i++)
        {
            shop_BlessingList[i].Start();
        }
    }
    public ShopController()
    {
        //autoBuyBlessingSlotNum = new Multiplier();

        materialStockNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        trapStockNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 20));
        blessingStockNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        sellPriceRate = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0.1));
        restockTimesec = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 10 * 60));

        //Restockの際にRestockする量
        restockNumMaterial = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        restockNumTrap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        restockNumBlessing = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        //TownMaterialのConvertで得られる量
        convertTownMaterialAmount = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        for (int i = 0; i <= (int)MaterialKind.UnicornHorn; i++)
        {
            int count = i;
            shop_MaterialList.Add(new Shop_Material(this, (MaterialKind)count));
        }
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.ThrowingNet));
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.IceRope));
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.ThunderRope));
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.FireRope));
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.LightRope));
        shop_TrapList.Add(new Shop_Trap(this, PotionKind.DarkRope));
        for (int i = 0; i < Enum.GetNames(typeof(BlessingKind)).Length; i++)
        {
            int count = i;
            shop_BlessingList.Add(new Shop_Blessing(this, (BlessingKind)count));
        }
        for (int i = 0; i < Enum.GetNames(typeof(TownMaterialKind)).Length; i++)
        {
            TownMaterialKind kind = (TownMaterialKind)i;
            if ((int)kind % 5 != 4)
                shop_TownMaterialList.Add(new Shop_TownMaterial(this, kind));
        }
        shopItemList.AddRange(shop_MaterialList);
        shopItemList.AddRange(shop_TrapList);
        shopItemList.AddRange(shop_BlessingList);
        shopItemList.AddRange(shop_TownMaterialList);
    }
    public List<SHOPITEM> shopItemList = new List<SHOPITEM>();
    public List<Shop_Material> shop_MaterialList = new List<Shop_Material>();
    public Shop_Material Material(MaterialKind kind)
    {
        for (int i = 0; i < shop_MaterialList.Count; i++)
        {
            if (shop_MaterialList[i].materialKind == kind) return shop_MaterialList[i];
        }
        return shop_MaterialList[0];
    }
    public List<Shop_Trap> shop_TrapList = new List<Shop_Trap>();
    public Shop_Trap Trap(PotionKind kind)
    {
        for (int i = 0; i < shop_TrapList.Count; i++)
        {
            if (shop_TrapList[i].potionKind == kind) return shop_TrapList[i];
        }
        return shop_TrapList[0];
    }
    public List<Shop_Blessing> shop_BlessingList = new List<Shop_Blessing>();
    public Shop_Blessing Blessing(BlessingKind kind)
    {
        for (int i = 0; i < shop_BlessingList.Count; i++)
        {
            if (shop_BlessingList[i].blessingKind == kind) return shop_BlessingList[i];
        }
        return shop_BlessingList[0];
    }
    public List<Shop_TownMaterial> shop_TownMaterialList = new List<Shop_TownMaterial>();
    public Shop_TownMaterial TownMaterial(TownMaterialKind kind)
    {
        for (int i = 0; i < shop_TownMaterialList.Count; i++)
        {
            if (shop_TownMaterialList[i].materialKind == kind) return shop_TownMaterialList[i];
        }
        return shop_TownMaterialList[0];
    }


    public Multiplier materialStockNum;
    public Multiplier trapStockNum;
    public Multiplier blessingStockNum;
    public Multiplier sellPriceRate;
    public Multiplier restockTimesec;

    public Multiplier restockNumMaterial;
    public Multiplier restockNumTrap;
    public Multiplier restockNumBlessing;

    //public Multiplier autoBuyBlessingSlotNum;
    //public int CurrentAutoBuyBlessingSlotNum()
    //{
    //    int tempNum = 0;
    //    for (int i = 0; i < shop_BlessingList.Count; i++)
    //    {
    //        if (shop_BlessingList[i].isAutoBuy) tempNum++;
    //    }
    //    return tempNum;
    //}
    //public void AdjustAutoBuyBlessingNum()
    //{
    //    if (CurrentAutoBuyBlessingSlotNum() > autoBuyBlessingSlotNum.Value())
    //    {
    //        for (int i = 0; i < shop_BlessingList.Count; i++)
    //        {
    //            shop_BlessingList[i].isAutoBuy = false;
    //        }
    //    }
    //}
    //public bool CanAssignAutoBuyBlessing()
    //{
    //    return CurrentAutoBuyBlessingSlotNum() < autoBuyBlessingSlotNum.Value();
    //}

    public Multiplier convertTownMaterialAmount;
    public float timecount { get => main.SR.shopRestockTimecount; set => main.SR.shopRestockTimecount = value; }
    public float restockTimeleft { get => (float)restockTimesec.Value() - timecount; }
    public void Update()
    {
        Restock(Time.deltaTime);
        //timecount += Time.deltaTime;
        //if (timecount >= restockTimesec.Value())
        //{
        //    Restock();
        //    timecount = 0;
        //}
    }
    public void Restock(float deltaTime)//OfflineBonus用
    {
        timecount += deltaTime;
        for (int i = 0; i < 1000; i++)
        {
            if (timecount >= restockTimesec.Value())
            {
                Restock();
                timecount -= (float)restockTimesec.Value();
            }
            else
                break;
        }
    }
    void Restock()
    {
        for (int i = 0; i < shop_MaterialList.Count; i++)
        {
            shop_MaterialList[i].purchasedNum.Increase(-(long)restockNumMaterial.Value());
        }
        for (int i = 0; i < shop_TrapList.Count; i++)
        {
            shop_TrapList[i].purchasedNum.Increase(-(long)restockNumTrap.Value());
        }
        for (int i = 0; i < shop_BlessingList.Count; i++)
        {
            shop_BlessingList[i].purchasedNum.Increase(-(long)restockNumBlessing.Value());
        }
    }

    public void AutoBuy()
    {
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialSlime))
            Material(MaterialKind.OilOfSlime).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialMagicSlime))
            Material(MaterialKind.EnchantedCloth).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialSpider))
            Material(MaterialKind.SpiderSilk).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialBat))
            Material(MaterialKind.BatWing).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialFairy))
            Material(MaterialKind.FairyDust).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialFox))
            Material(MaterialKind.FoxTail).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialDevilfish))
            Material(MaterialKind.FishScales).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialTreant))
            Material(MaterialKind.CarvedBranch).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialFlametiger))
            Material(MaterialKind.ThickFur).buyTransaction.Buy(true);
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopMaterialUnicorn))
            Material(MaterialKind.UnicornHorn).buyTransaction.Buy(true);

        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapNormal))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.ThrowingNet)))
                Trap(PotionKind.ThrowingNet).buyTransaction.Buy(true);
        }
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapIce))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.IceRope)))
                Trap(PotionKind.IceRope).buyTransaction.Buy(true);
        }
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapThunder))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.ThunderRope)))
                Trap(PotionKind.ThunderRope).buyTransaction.Buy(true);
        }
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapFire))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.FireRope)))
                Trap(PotionKind.FireRope).buyTransaction.Buy(true);
        }
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapLight))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.LightRope)))
                Trap(PotionKind.LightRope).buyTransaction.Buy(true);
        }
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyShopTrapDark))
        {
            if (!(SettingMenuUI.Toggle(ToggleKind.AdvancedAutoBuyTrap).isOn && game.inventoryCtrl.IsOnePotionStackMaxInInventory(PotionKind.DarkRope)))
                Trap(PotionKind.DarkRope).buyTransaction.Buy(true);
        }

        //for (int i = 0; i < shop_BlessingList.Count; i++)
        //{
        //    if (shop_BlessingList[i].isAutoBuy) shop_BlessingList[i].Buy();
        //}
        //isAutoBuyの指定は取りやめて、ランダムで選ばれるようにしてみる
        if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.BuyBlessing))
            shop_BlessingList[UnityEngine.Random.Range(0, shop_BlessingList.Count)].buyTransaction.Buy(true);
    }
}

public class SHOPITEM
{
    public SHOPITEM(ShopController shopCtrl)
    {
        this.shopCtrl = shopCtrl;
        unlock = new Unlock();
    }
    public ShopController shopCtrl;
    public virtual ShopKind kind { get; }
    public virtual NUMBER item { get; }
    public virtual NUMBER currency { get; }
    public INTEGER purchasedNum;
    public Unlock unlock;
    public Transaction buyTransaction;
    public Transaction sellTransaction;
    public virtual void BuyAction(long num) { }
    public virtual void SellAction(long num) { }
    public virtual double Price() { return 1e300; }
    public virtual double SellPrice() { return Price() * shopCtrl.sellPriceRate.Value(); }
    public virtual long LimitNum() { return 0; }
    public long stockNum { get => LimitNum() - purchasedNum.value; }
    public bool IsStocked() { return stockNum > 0; }
    public void Buy() { buyTransaction.Buy(); }
    public void Sell() { sellTransaction.Buy(); }
    public virtual void Start() { }
}

//Blessing
public class Shop_Blessing : SHOPITEM
{
    public Shop_Blessing(ShopController shopCtrl, BlessingKind blessingKind) : base(shopCtrl)
    {
        this.blessingKind = blessingKind;
        purchasedNum = new Shop_PurchasedNumBlessing(this, LimitNum);
        buyTransaction = new Transaction(purchasedNum, currency, Price, () => 0, true);
        buyTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        buyTransaction.additionalBuyActionWithLevelIncrement = BuyAction;
    }
    public override void Start()
    {
        //buyTransaction.SetAdditionalBuyCondition(() => game.inventoryCtrl.CanCreatePotion(potionKind, buyTransaction.LevelIncrement()));
    }
    //public void AssignAutoBuy()
    //{
    //    if (isAutoBuy)
    //    {
    //        isAutoBuy = false;
    //        return;
    //    }        ss
    //    if (!unlock.IsUnlocked()) return;
    //    if (!shopCtrl.CanAssignAutoBuyBlessing()) return;
    //    isAutoBuy = true;
    //}
    public bool isAutoBuy { get => main.SR.isAutoBuyBlessings[(int)blessingKind]; set => main.SR.isAutoBuyBlessings[(int)blessingKind] = value; }
    public BlessingKind blessingKind;
    public override ShopKind kind { get => ShopKind.Blessing; }
    public BLESSING blessing { get => game.battleCtrl.blessingCtrl.Blessing(blessingKind); }
    public override NUMBER currency { get => game.resourceCtrl.gold; }
    public override void BuyAction(long num)
    {
        blessing.StartBlessing();
    }
    public override long LimitNum() { return (long)shopCtrl.blessingStockNum.Value(); }
    public override double Price() { return 10000; }
}
public class Shop_PurchasedNumBlessing : INTEGER
{
    public Shop_PurchasedNumBlessing(Shop_Blessing shopItem, Func<long> maxValue)
    {
        this.shopItem = shopItem;
        this.maxValue = maxValue;
    }
    public Shop_Blessing shopItem;
    public override long value { get => main.SR.purchasedNumBlessing; set => main.SR.purchasedNumBlessing = value; }
}


//Trap
public class Shop_Trap : SHOPITEM
{
    public Shop_Trap(ShopController shopCtrl, PotionKind potionKind) : base(shopCtrl)
    {
        this.potionKind = potionKind;
        purchasedNum = new Shop_PurchasedNumTrap(this, LimitNum);
        buyTransaction = new Transaction(purchasedNum, currency, Price, () => 0, true);
        buyTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        buyTransaction.additionalBuyActionWithLevelIncrement = BuyAction;
    }
    public override void Start()
    {
        buyTransaction.SetAdditionalBuyCondition((num) => game.inventoryCtrl.CanCreatePotion(potionKind, num));
    }
    public PotionKind potionKind;
    public override ShopKind kind { get => ShopKind.Potion; }
    public PotionGlobalInformation globalInfo { get => game.potionCtrl.GlobalInfo(potionKind); }
    public override NUMBER currency { get => game.resourceCtrl.gold; }
    public override void BuyAction(long num)
    {
        game.inventoryCtrl.CreatePotion(potionKind, num);
    }
    public override long LimitNum() { return (long)shopCtrl.trapStockNum.Value(); }
    public override double Price()
    {
        switch (potionKind)
        {
            case PotionKind.ThrowingNet:
                return 500;
            case PotionKind.IceRope:
                return 1000;
            case PotionKind.ThunderRope:
                return 2000;
            case PotionKind.FireRope:
                return 4000;
            case PotionKind.LightRope:
                return 8000;
            case PotionKind.DarkRope:
                return 16000;
        }
        return 1e300d;
    }
}
public class Shop_PurchasedNumTrap : INTEGER
{
    public Shop_PurchasedNumTrap(Shop_Trap shopItem, Func<long> maxValue)
    {
        this.shopItem = shopItem;
        this.maxValue = maxValue;
    }
    public Shop_Trap shopItem;
    public override long value { get => main.SR.purchasedNumTraps[(int)shopItem.potionKind]; set => main.SR.purchasedNumTraps[(int)shopItem.potionKind] = value; }
}

//Material
public class Shop_Material : SHOPITEM
{
    public Shop_Material(ShopController shopCtrl, MaterialKind kind) : base(shopCtrl)
    {
        materialKind = kind;
        purchasedNum = new Shop_PurchasedNumMaterial(this, LimitNum);
        buyTransaction = new Transaction(purchasedNum, currency, Price, () => 0, true);
        buyTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        buyTransaction.additionalBuyActionWithLevelIncrement = BuyAction;
        sellTransaction = new Transaction(new INTEGER(), item, () => 1, () => 0, true);
        sellTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        sellTransaction.additionalBuyActionWithLevelIncrement = SellAction;
    }
    public MaterialKind materialKind;
    public override ShopKind kind { get => ShopKind.Material; }
    public override NUMBER item { get => game.materialCtrl.Material(materialKind); }
    public override NUMBER currency { get => game.resourceCtrl.gold; }
    public override void BuyAction(long num) { item.Increase(num); }
    public override void SellAction(long num) { game.resourceCtrl.gold.IncreaseWithoutMultiplier(num * SellPrice()); }
    public override long LimitNum() { return (long)shopCtrl.materialStockNum.Value(); }
    public override double Price()
    {
        switch (materialKind)
        {
            case MaterialKind.MonsterFluid:
                return 100000;
            case MaterialKind.FlameShard:
                return 250000;
            case MaterialKind.FrostShard:
                return 250000;
            case MaterialKind.LightningShard:
                return 250000;
            case MaterialKind.NatureShard:
                return 500000;
            case MaterialKind.PoisonShard:
                return 500000;
            case MaterialKind.BlackPearl:
                return 10000000;
            case MaterialKind.OilOfSlime:
                return 1000;
            case MaterialKind.EnchantedCloth:
                return 1500;
            case MaterialKind.SpiderSilk:
                return 2000;
            case MaterialKind.BatWing:
                return 2500;
            case MaterialKind.FairyDust:
                return 3000;
            case MaterialKind.FoxTail:
                return 3500;
            case MaterialKind.FishScales:
                return 4000;
            case MaterialKind.CarvedBranch:
                return 4500;
            case MaterialKind.ThickFur:
                return 5000;
            case MaterialKind.UnicornHorn:
                return 5500;
        }
        return 1e300d;
    }
}
public class Shop_PurchasedNumMaterial : INTEGER
{
    public Shop_PurchasedNumMaterial(Shop_Material shopItem, Func<long> maxValue)
    {
        this.shopItem = shopItem;
        this.maxValue = maxValue;
    }
    public Shop_Material shopItem;
    public override long value { get => main.SR.purchasedNumMaterials[(int)shopItem.materialKind]; set => main.SR.purchasedNumMaterials[(int)shopItem.materialKind] = value; }
}

//TownMaterial
public class Shop_TownMaterial : SHOPITEM
{
    public Shop_TownMaterial(ShopController shopCtrl, TownMaterialKind kind) : base(shopCtrl)
    {
        materialKind = kind;//Convertした結果得られるkind
        purchasedNum = new Shop_PurchasedNumTownMaterial(this, LimitNum);
        buyTransaction = new Transaction(purchasedNum, currency, Price, () => 0, true);
        buyTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        buyTransaction.additionalBuyActionWithLevelIncrement = BuyAction;
        //sellTransaction = new Transaction(new INTEGER(), item, () => 1, () => 0, true);
        //sellTransaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        //sellTransaction.additionalBuyActionWithLevelIncrement = SellAction;
    }
    public TownMaterialKind materialKind;
    public override ShopKind kind { get => ShopKind.TownMaterial; }
    public override NUMBER item { get => game.townCtrl.TownMaterial(materialKind); }
    public override NUMBER currency
    {
        get
        {
            switch (materialKind)
            {
                case TownMaterialKind.MudBrick:
                    return game.townCtrl.TownMaterial(TownMaterialKind.LimestoneBrick);
                case TownMaterialKind.LimestoneBrick:
                    return game.townCtrl.TownMaterial(TownMaterialKind.MarbleBrick);
                case TownMaterialKind.MarbleBrick:
                    return game.townCtrl.TownMaterial(TownMaterialKind.GraniteBrick);
                case TownMaterialKind.GraniteBrick:
                    return game.townCtrl.TownMaterial(TownMaterialKind.BasaltBrick);
                //case TownMaterialKind.BasaltBrick:
                //    break;
                case TownMaterialKind.PineLog:
                    return game.townCtrl.TownMaterial(TownMaterialKind.MapleLog);
                case TownMaterialKind.MapleLog:
                    return game.townCtrl.TownMaterial(TownMaterialKind.AshLog);
                case TownMaterialKind.AshLog:
                    return game.townCtrl.TownMaterial(TownMaterialKind.MahoganyLog);
                case TownMaterialKind.MahoganyLog:
                    return game.townCtrl.TownMaterial(TownMaterialKind.RosewoodLog);
                //case TownMaterialKind.RosewoodLog:
                //    break;
                case TownMaterialKind.JasperShard:
                    return game.townCtrl.TownMaterial(TownMaterialKind.OpalShard);
                case TownMaterialKind.OpalShard:
                    return game.townCtrl.TownMaterial(TownMaterialKind.OnyxShard);
                case TownMaterialKind.OnyxShard:
                    return game.townCtrl.TownMaterial(TownMaterialKind.JadeShard);
                case TownMaterialKind.JadeShard:
                    return game.townCtrl.TownMaterial(TownMaterialKind.SapphireShard);
                //case TownMaterialKind.SapphireShard:
                //    break;
                default:
                    return game.townCtrl.TownMaterial(materialKind);
            }
        }
    }
    public override void BuyAction(long num) { item.Increase(num * shopCtrl.convertTownMaterialAmount.Value()); }
    //public override void SellAction(long num) { currency.Increase(num * SellPrice()); }
    public override long LimitNum() { return 100000000000000; }
    public override double Price() { return 10d; }
}
public class Shop_PurchasedNumTownMaterial : INTEGER
{
    public Shop_PurchasedNumTownMaterial(Shop_TownMaterial shopItem, Func<long> maxValue)
    {
        this.shopItem = shopItem;
        this.maxValue = maxValue;
    }
    public Shop_TownMaterial shopItem;
    public override long value { get => main.SR.purchasedNumTownMaterials[(int)shopItem.materialKind]; set => main.SR.purchasedNumTownMaterials[(int)shopItem.materialKind] = value; }
}


public enum ShopKind
{
    Material,
    Potion,
    Blessing,
    TownMaterial,
}