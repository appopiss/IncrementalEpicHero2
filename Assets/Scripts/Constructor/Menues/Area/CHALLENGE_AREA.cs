using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static ChallengeMonsterKind;

public class CHALLENGE_AREA : AREA
{
    public override bool isChallenge => true;
    public CHALLENGE challenge;
    //public virtual ChallengeMonsterKind challengeMonsterKind => ChallengeMonsterKind.SlimeKing;
    public override long MinLevel() { return minLevel; }
    public override long MaxLevel() { return maxLevel; }
    public CHALLENGE_AREA(CHALLENGE challenge)//AreaController areaCtrl)
    {
        //this.areaCtrl = areaCtrl;
        swarm = new Swarm(this);
        this.challenge = challenge;
        id = (int)challenge.challengeMonsterKind;
        SetAllWave();
        SetMonsterSpawns();
    }
    public override void ClearAction(HERO_BATTLE heroBattle, bool isCleared, double gold, double exp)
    {
        //if (ElapsedTime(heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated) < 1f) return;//1秒以下はバグと判断する
        //bestGold = Math.Max(bestGold, gold);
        //bestExp = Math.Max(bestExp, exp);
        challenge.Initialize();
        if (!isCleared) return;
        challenge.ClearAction();
        //if (bestTime <= 1f) bestTime = ElapsedTime(heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated);
        //else bestTime = Mathf.Min(bestTime, ElapsedTime(heroBattle.battleCtrl.timecount, heroBattle.battleCtrl.isSimulated));
    }
    public override string Name(bool isRegion = true, bool isLevel = true, bool isShort = false)
    {
        return challenge.TitleUIString();
    }
    public override bool CanStart()
    {
        if (!IsUnlocked()) return false;
        if (game.challengeCtrl.IsTryingChallenge()) return false;
        return true;
    }
    public override void StartAction(BATTLE_CONTROLLER battleCtrl, float startTime, bool isSimulated)
    {
        if (isSimulated)
        {
            InitializeSimulation();
            simulatedEnterTime = startTime;
            return;
        }
        enterTime[(int)battleCtrl.heroKind] = startTime;
        //areaCtrl.portalOrb.Decrease(RequiredPortalOrb());
    }

}
