using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static Localized;
using static UsefulMethod;
using static ChallengeMonsterKind;
using static ChallengeKind;
using static MultiplierKind;
using static MultiplierType;

public class ChallengeController
{
    public ChallengeController()
    {
        permanentRewardMultiplier = new Multiplier(new MultiplierInfo(Base, Add, () => 1));

        raidbossList.Add(new ChallengeRaidBoss_SlimeKingLv100());
        raidbossList.Add(new ChallengeRaidBoss_WindowQueenLv150());
        raidbossList.Add(new ChallengeRaidBoss_GolemLv200());
        singlebossList.Add(new ChallengeSingleBoss_SlimeKingLv100());
        singlebossList.Add(new ChallengeSingleBoss_WindowQueenLv150());
        singlebossList.Add(new ChallengeSingleBoss_GolemLv200());
        handicapList.Add(new ChallengeHandicap_Arena1());
        handicapList.Add(new ChallengeHandicap_SlimeKingLv100());
        handicapList.Add(new ChallengeHandicap_WindowQueenLv150());
        handicapList.Add(new ChallengeHandicap_GolemLv200());
        //ここに追加


        challengeArray = new CHALLENGE[][]
        {
            raidbossList.ToArray(),
            singlebossList.ToArray(),
            handicapList.ToArray(),
            //ここに追加 
        };
        challengeList.AddRange(raidbossList);
        challengeList.AddRange(singlebossList);
        challengeList.AddRange(handicapList);
        //ここに追加 
    }
    public void Start()
    {
        for (int i = 0; i < challengeList.Count; i++)
        {
            challengeList[i].Start();
        }
    }
    public CHALLENGE[][] challengeArray;
    public List<CHALLENGE> raidbossList = new List<CHALLENGE>();
    public List<CHALLENGE> singlebossList = new List<CHALLENGE>();
    public List<CHALLENGE> handicapList = new List<CHALLENGE>();
    public List<CHALLENGE> challengeList = new List<CHALLENGE>();
    public Multiplier permanentRewardMultiplier;

    public CHALLENGE Challenge(ChallengeKind kind)
    {
        for (int i = 0; i < challengeList.Count; i++)
        {
            if (challengeList[i].kind == kind) return challengeList[i];
        }
        return challengeList[0];
    }
    public bool IsTryingChallenge()
    {
        for (int i = 0; i < challengeList.Count; i++)
        {
            if (challengeList[i].isTryingThisChallenge) return true;
        }
        return false;
    }
}
