using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using static GameController;

public class SimulationController
{
    public SimulationController()
    {
        for (int i = 0; i < battleCtrls.Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            battleCtrls[i] = new BATTLE_CONTROLLER(heroKind);
        }
        //battleCtrl = new BATTLE_CONTROLLER(game.currentHero);
    }
    public void Start()
    {
        //battleCtrl.Start();
        for (int i = 0; i < battleCtrls.Length; i++)
        {
            battleCtrls[i].Start();
        }
    }
    public void Initialize()
    {
        //battleCtrl = new BATTLE_CONTROLLER(game.currentHero);
        battleCtrl.Start();
        //battleCtrl.heroKind = game.currentHero;
        //battleCtrl.skillSet = game.battleCtrl.skillSet;

    }
    public bool isSimulating;//現在計算中かどうか
    BATTLE_CONTROLLER battleCtrl => battleCtrls[(int)game.currentHero];
    BATTLE_CONTROLLER[] battleCtrls = new BATTLE_CONTROLLER[Enum.GetNames(typeof(HeroKind)).Length];

    static int simulateFlamePerRealFlame = 60;//1Flameに240回（4秒分）->負荷を減らすために60回(1秒分）にしてみる
    static int simulateMaxTimesec = 5;//Simulationにかける時間は１エリア最大5秒


    //基本のSimulation
    public async Task Simulate(AREA area, bool isQuick = false)
    {
        if (area.isDungeon) battleCtrl.areaBattle.Start(area);
        else battleCtrl.areaBattle.Start(area.kind, area.id);
        int tempLoopNum = simulateMaxTimesec * 60;
        if (isQuick) tempLoopNum /= 60;
        for (int j = 0; j < tempLoopNum; j++)
        {
            for (int i = 0; i < simulateFlamePerRealFlame; i++)
            {
                if (battleCtrl.areaBattle.isFinishedSimulation) return;
                battleCtrl.Update();
            }
            await UniTask.DelayFrame(1);
        }
        //もし時間内にシミュレーションが完了しなかった場合は自動的にNotClearになる
        battleCtrl.areaBattle.QuitCurrentArea();
    }
    //AreaのSimulation
    public async Task Simulate(AreaKind areaKind, bool isDungeon, bool isQuick = false)//isQuickはautoBestExp/sec用
    {
        if (isSimulating) return;
        isSimulating = true;
        if (isDungeon)
        {
            //for (int i = 0; i < game.areaCtrl.dungeons[(int)areaKind].Length; i++)
            //{
            //    game.areaCtrl.dungeons[(int)areaKind][i].InitializeSimulation();
            //}
            for (int i = 0; i < game.areaCtrl.dungeons[(int)areaKind].Length; i++)
            {
                int count = i;
                await Simulate(game.areaCtrl.dungeons[(int)areaKind][count], isQuick);
            }
        }
        else
        {
            //for (int i = 0; i < game.areaCtrl.areas[(int)areaKind].Length; i++)
            //{
            //    game.areaCtrl.areas[(int)areaKind][i].InitializeSimulation();
            //}
            for (int i = 0; i < game.areaCtrl.areas[(int)areaKind].Length; i++)
            {
                int count = i;
                await Simulate(game.areaCtrl.areas[(int)areaKind][count], isQuick);
            }
        }
        isSimulating = false;
    }

    //１つのAreaのSimulation（10回平均)
    public async Task SimulateAverage(AREA area, bool isQuick = false, int trialNum = 10)
    {
        isSimulating = true;
        double totalClearNum = 0;
        double totalTime = 0;
        double totalGold = 0;
        double totalExp = 0;
        for (int i = 0; i < trialNum; i++)
        {
            await Simulate(area, isQuick);
            if (area.simulatedIsClear) totalClearNum++;
            totalTime += area.simulatedTime;
            totalGold += area.simulatedGold;
            totalExp += area.simulatedExp;
        }
        simulatedClearNumPerSec = totalClearNum / totalTime;
        simulatedGoldPerSec = totalGold / totalTime;
        simulatedExpPerSec = totalExp / totalTime;
        isSimulating = false;
    }
    public double simulatedClearNumPerSec;
    public double simulatedGoldPerSec;
    public double simulatedExpPerSec;
}
