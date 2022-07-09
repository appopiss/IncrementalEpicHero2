using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public class OfflineBonus
{
    public OfflineBonus()
    {
    }
    public void SetOfflineTimesec()
    {
        if (main.isHacked) return;
        float tempDeltaTime = DeltaTimeFloat(main.lastTimeLocal);
        if (tempDeltaTime < 0 || tempDeltaTime >= 365 * 86400) offlineTimesec = 0;
        else offlineTimesec = tempDeltaTime;
        //main.Save();
    }

    public async void CalculateOfflineBonus()
    {
        await UniTask.WaitUntil(() => offlineTimesec >= 0);
        AREA area = game.battleCtrl.areaBattle.CurrentArea();
        await game.simulationCtrl.SimulateAverage(area);
        foreach (var item in area.rewardMaterial)
        {
            areaRewardMaterials.Add(item.Key, item.Value(game.currentHero) * game.simulationCtrl.simulatedClearNumPerSec * offlineTimesec * gainFactor);
        }
        exp = game.simulationCtrl.simulatedExpPerSec * offlineTimesec * gainFactor;
        gold = game.simulationCtrl.simulatedGoldPerSec * offlineTimesec * gainFactor;
        areaClearNum = game.simulationCtrl.simulatedClearNumPerSec * offlineTimesec * gainFactor;
        area.InitializeSimulation();
        isFinishedCalculation = true;
    }
    public bool isFinishedCalculation;
    public double gainFactor => Math.Max(0.10d, game.guildCtrl.Member(game.currentHero).backgroundGainRate.Value());
    public double exp, gold, areaClearNum;
    public Dictionary<NUMBER, double> areaRewardMaterials = new Dictionary<NUMBER, double>();

    public void GetOfflineBonus(OfflineBonusKind kind)
    {
        if (main.isHacked) return;
        switch (kind)
        {
            case OfflineBonusKind.Nitro:
                game.nitroCtrl.nitro.Increase(NitroGain());
                break;
            case OfflineBonusKind.Playtime:
                main.allTime += offlineTimesec;
                main.allTimeWorldAscension += offlineTimesec;
                //ShopのRestock
                game.shopCtrl.Restock((float)offlineTimesec);
                //TownBuildingのResearch
                game.townCtrl.ProgressResaerch(offlineTimesec);
                //Expedition
                game.expeditionCtrl.Progress(offlineTimesec);
                //CurrentHeroのExp,Gold,TownMat
                game.resourceCtrl.gold.Increase(gold);
                game.statsCtrl.Exp(game.currentHero).IncreaseWithoutLimit(exp);
                foreach (var item in areaRewardMaterials)
                {
                    item.Key.Increase(item.Value);
                }
                game.battleCtrl.areaBattle.CurrentArea().completedNum.Increase(areaClearNum);
                break;
        }
    }
    public double NitroGain()
    {
        if (offlineTimesec <= 10) return 0;
        if (game.nitroCtrl.nitro.IsMaxed()) return 0;
        game.nitroCtrl.nitroCap.Calculate();
        return Math.Min(game.nitroCtrl.nitroCap.Value() - game.nitroCtrl.nitro.value, offlineTimesec / 4d);
    }

    public double offlineTimesec = -1;
}

public enum OfflineBonusKind
{
    Nitro,
    Playtime,
}
