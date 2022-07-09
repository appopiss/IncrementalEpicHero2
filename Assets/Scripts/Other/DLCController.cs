using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static GameControllerUI;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public partial class Save
{
    public bool isDlcStarterPack, isDlcNitroPack, isDlcGlobalSkillSlotPack, isDlcInventorySlotPack;
}
public class DLCController
{
    GetOwnerShip starterPack, nitroPack, globalSkillSlotPack, inventorySlotPack;

    public DLCController()
    {
        starterPack = new GetOwnerShip("B28EB1C319D81B6009B437FDF042264F", "2053800");
        nitroPack = new GetOwnerShip("B28EB1C319D81B6009B437FDF042264F", "2053801");
        globalSkillSlotPack = new GetOwnerShip("B28EB1C319D81B6009B437FDF042264F", "2053802");
        inventorySlotPack = new GetOwnerShip("B28EB1C319D81B6009B437FDF042264F", "2053803");
    }
    public void Start()
    {
        SetRegisterInfo();
        GetDLC();//Steamで公開してから
    }
    void SetRegisterInfo()
    {
        //StarterPack
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.questCtrl.AcceptableNum(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 1, () => main.S.isDlcStarterPack));
        }
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Mul, () => 0.25d, () => main.S.isDlcStarterPack));
        //NitroPack
        game.nitroCtrl.maxNitroSpeed.RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 1, () => main.S.isDlcNitroPack));
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Mul, () => 0.25d, () => main.S.isDlcNitroPack));
        //GlobalSkillSlotPack
        game.statsCtrl.globalSkillSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 1, () => main.S.isDlcGlobalSkillSlotPack));
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Mul, () => 0.25d, () => main.S.isDlcGlobalSkillSlotPack));
        //InventorySlotPack
        game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 50, () => main.S.isDlcInventorySlotPack));
        game.inventoryCtrl.enchantUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 10, () => main.S.isDlcInventorySlotPack));
        game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Add, () => 10, () => main.S.isDlcInventorySlotPack));
        game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.DLC, MultiplierType.Mul, () => 0.25d, () => main.S.isDlcInventorySlotPack));
    }
    async void GetDLC()
    {
        //StarterPack
        if (!main.S.isDlcStarterPack)
        {
            await UniTask.WaitUntil(() => starterPack.isGotInfo);
            if (starterPack.isOwn)
            {
                main.S.isDlcStarterPack = true;
                game.nitroCtrl.nitro.IncreaseWithoutLimit(5000);
                game.areaCtrl.portalOrb.Increase(5);
            }
        }
        //NitroPack
        if (!main.S.isDlcNitroPack)
        {
            await UniTask.WaitUntil(() => nitroPack.isGotInfo);
            if (nitroPack.isOwn)
            {
                main.S.isDlcNitroPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                game.nitroCtrl.nitroCap.Calculate();
                game.nitroCtrl.speed.ToMax();
            }
        }
        //GlobalSkillSlotPack
        if (!main.S.isDlcGlobalSkillSlotPack)
        {
            await UniTask.WaitUntil(() => globalSkillSlotPack.isGotInfo);
            if (globalSkillSlotPack.isOwn)
            {
                main.S.isDlcGlobalSkillSlotPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                gameUI.battleStatusUI.SetSkillSlot();
            }
        }
        //InventorySlotPack
        if (!main.S.isDlcInventorySlotPack)
        {
            await UniTask.WaitUntil(() => inventorySlotPack.isGotInfo);
            if (inventorySlotPack.isOwn)
            {
                main.S.isDlcInventorySlotPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                game.inventoryCtrl.slotUIAction();
            }
        }
    }
}
