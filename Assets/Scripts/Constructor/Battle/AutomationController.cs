using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static SpriteSourceUI;
using static MonsterParameter;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class AutomationController
{
    public AutomationController()
    {
        tourArea = new AutoTourArea();
    }
    //float count;
    //public void Update()
    //{
    //    //count += Time.deltaTime;
    //    //if (count >= 10f)//5 * 60f)//5分
    //    //{
    //    //    Debug.Log("Tour");
    //    //    tourArea.UpdateBestArea();
    //    //    count = 0;
    //    //}
    //}
    public AutoTourArea tourArea;
    public void Convene(AREA area)
    {
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            if (i != (int)game.currentHero)
            {
                if (game.battleCtrls[i].isActiveBattle)
                    game.battleCtrls[i].areaBattle.Start(area);
            }
        }
    }
}
public class AutoTourArea
{
    static int saveSlot = (int)Parameter.maxHeroLevel / 25;
    AREA[] bestAreas = new AREA[saveSlot];
    public double[] lastSimulatedPlaytime = new double[saveSlot];
    static double simulateTimeSpan = 7200;//2時間
    public void UpdateBestArea(long level)
    {
        int id = (int)level / 25;
        if (main.allTime < lastSimulatedPlaytime[id] + simulateTimeSpan)
        {
            bestArea = bestAreas[id];
            GoToBestArea();
            return;
        }
        UpdateBestArea(id);
    }
    //とても重い処理なので、各レベルで2時間に１回まで発動できるようにする
    bool isTrying;
    public async void UpdateBestArea(int id)
    {
        if (isTrying) return;
        isTrying = true;
        await SimulateAllArea();
        bestAreas[id] = bestArea;
        lastSimulatedPlaytime[id] = main.allTime;
        GoToBestArea();
        isTrying = false;
    }

    HeroKind heroKind => game.currentHero;
    double bestExpPerSec = 0;
    AREA bestArea = game.areaCtrl.nullArea;
    async Task SimulateAllArea()
    {
        bestExpPerSec = 0;
        bestArea = game.battleCtrls[(int)heroKind].areaBattle.currentArea;
        for (int i = 0; i < Enum.GetNames(typeof(AreaKind)).Length; i++)
        {
            AreaKind areaKind = (AreaKind)i;
            await game.simulationCtrl.Simulate(areaKind, false, true);
            for (int j = 0; j < game.areaCtrl.areas[(int)areaKind].Length; j++)
            {
                AREA tempArea = game.areaCtrl.areas[(int)areaKind][j];
                if (tempArea.simulatedIsClear && tempArea.simulatedExpPerSec > bestExpPerSec)
                {
                    bestExpPerSec = Math.Max(bestExpPerSec, tempArea.simulatedExpPerSec);
                    bestArea = tempArea;
                }
            }
        }
    }
    void GoToBestArea()
    {
        if (bestArea == game.battleCtrls[(int)heroKind].areaBattle.CurrentArea()) return;
        game.battleCtrls[(int)heroKind].areaBattle.Start(bestArea);
    }
}
