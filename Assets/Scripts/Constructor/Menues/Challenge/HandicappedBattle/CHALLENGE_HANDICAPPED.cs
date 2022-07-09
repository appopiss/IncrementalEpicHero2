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
using static ChallengeType;
using static MultiplierKind;
using static MultiplierType;

public class CHALLENGE_HANDICAPPED : CHALLENGE
{
    //派生先でHandicapKindListを設定する
    public override ChallengeType type => HandicappedBattle;
    public override void StartBattle()
    {
        //StartBattleはこの順番でないといけない。
        game.battleCtrl.StartHandicappedChallenge(area, handicapKindList);
        isTryingThisChallenge = true;
    }

    public override void ClearAction()//複雑な報酬体型の場合はこれごとoverride
    {
        Initialize();
        isClearedCurrentHero = true;
        if (IsClearedCompleted()) accomplish.RegisterTime();
    }
    public override string TitleUIString()
    {
        return "Handicapped Battle : " + NameString();
    }
    public override string ClearConditionString()
    {
        string tempStr = optStr + "<size=20><u>" + localized.Basic(BasicWord.ClearCondition) + "</u><size=18>";
        tempStr += "\n- " + ClearCondition() + " with handicaps";
        tempStr += "\n- Monster Level : Lv " + tDigit(area.MinLevel()) + " ~ " + " Lv " + tDigit(area.MaxLevel());
        tempStr += "\n\n<size=20><u>Handicaps</u><size=18>";
        for (int i = 0; i < handicapKindList.Count; i++)
        {
            int count = i;
            tempStr += "\n- " + localized.HandicapString(handicapKindList[count]);
        }
        return tempStr;
    }
    public virtual string ClearCondition() { return ""; }
    public override string RewardInfoString()
    {
        string tempStr = optStr + "<size=20><u>First Clear Reward</u><size=18>";
        for (int i = 0; i < ClassExclusiveRewardString().Length; i++)
        {
            int count = i;
            if (IsCleared((HeroKind)count))
            {
                if (IsReceivedRewardClass((HeroKind)count))
                    tempStr += "<color=green>";
                else tempStr += "<color=orange>";
            }
            tempStr += "\n- " + localized.Hero((HeroKind)count) + " : " + ClassExclusiveRewardString()[i] + "</color>";
        }
        tempStr += optStr + "\n\n<size=20><u>All Complete Reward</u><size=18>";
        if (IsClearedCompleted())
        {
            if (isReceivedRewardComplete) tempStr += "<color=green>";
            else tempStr += "<color=orange>";
        }
        tempStr += "\n- " + CompleteRewardString() + "</color>";
        return tempStr;
    }
}

//追加
public class ChallengeHandicap_Arena1 : CHALLENGE_HANDICAPPED
{
    //Slime
    public override ChallengeKind kind => HCArena1;
    public ChallengeHandicap_Arena1()
    {
        handicapKindList.Add(ChallengeHandicapKind.OnlyJewelry);
        handicapKindList.Add(ChallengeHandicapKind.OnlyClassSkill);
    }
    public override string NameString()
    {
        return "Mystic Arena 1F";
    }
    public override string ClearCondition()
    {
        return "Clear " + NameString();
    }
    public override void SetArea()
    {
        area = new ChallengeArea_Arena1(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return ClassExclusiveRewardStringClassBadgeTalisman();
    }
    public override string CompleteRewardString()
    {
        return "Talisman [Placeholder]";//"Resource Gain + " + percent(1.00);
    }
    public override void ClaimAction()
    {
        if (IsClearedOnce()) isReceivedRewardOnce = true;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            ClaimActionClassBadgeTalisman(heroKind);
        }
        if (IsClearedCompleted()) isReceivedRewardComplete = true;
    }
    public override void SetReward()
    {
        //Complete
        //game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(Challenge, Mul, () => 1.00, () => isReceivedRewardComplete));
    }
}
public class ChallengeArea_Arena1 : CHALLENGE_AREA
{
    public ChallengeArea_Arena1(CHALLENGE challenge) : base(challenge)//AreaController areaCtrl) : base(areaCtrl)
    {
        kind = AreaKind.SlimeVillage;
    }
    public override long minLevel => 125;
    public override long maxLevel => 125;
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        for (int i = 0; i < 2; i++)
        {
            wave.Add(RandomWave(10, MonsterColor.Blue));
        }
        for (int i = 0; i < 2; i++)
        {
            wave.Add(RandomWave(10, MonsterColor.Yellow));
        }
        for (int i = 0; i < 2; i++)
        {
            wave.Add(RandomWave(10, MonsterColor.Red));
        }
        for (int i = 0; i < 2; i++)
        {
            wave.Add(RandomWave(10, MonsterColor.Green));
        }
        for (int i = 0; i < 2; i++)
        {
            wave.Add(RandomWave(10, MonsterColor.Purple));
        }
        wave.Add(RandomWave(10));
        wave.Add(DefaultWave(1, MonsterColor.Boss));
        wave.Add(RandomWave(10));
        wave.Add(DefaultWave(1, MonsterColor.Boss));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(2, MonsterColor.Boss));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(2, MonsterColor.Boss));
        wave.Add(RandomWave(10));
        wave.Add(RandomWave(3, MonsterColor.Boss, MaxLevel));
    }
}


public class ChallengeHandicap_Arena2 : CHALLENGE_HANDICAPPED
{
    //MSlime Normal,Blue,Yellow,Red,
    public override ChallengeKind kind => HCArena2;
    public ChallengeHandicap_Arena2()
    {
        handicapKindList.Add(ChallengeHandicapKind.Only2ClassSkillAnd1Global);
    }
    public override string NameString()
    {
        return "Mystic Arena 2F";
    }
    public override string ClearCondition()
    {
        return "Clear " + NameString();
    }
    public override void SetArea()
    {
        area = new ChallengeArea_Arena2(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return ClassExclusiveRewardStringClassBadgeTalisman();
    }
    public override string CompleteRewardString()
    {
        return "Talisman [Placeholder]";//"Resource Gain + " + percent(1.00);
    }
    public override void ClaimAction()
    {
        if (IsClearedOnce()) isReceivedRewardOnce = true;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            ClaimActionClassBadgeTalisman(heroKind);
        }
        if (IsClearedCompleted()) isReceivedRewardComplete = true;
    }
    public override void SetReward()
    {
        //Complete
        //game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(Challenge, Mul, () => 1.00, () => isReceivedRewardComplete));
    }
}
public class ChallengeArea_Arena2 : CHALLENGE_AREA
{
    public ChallengeArea_Arena2(CHALLENGE challenge) : base(challenge)//AreaController areaCtrl) : base(areaCtrl)
    {
        kind = AreaKind.MagicSlimeCity;
        debuffElement[(int)Element.Fire] = -2.0;
        debuffElement[(int)Element.Ice] = -2.0;
        debuffElement[(int)Element.Thunder] = -2.0;
        debuffElement[(int)Element.Light] = -2.0;
        debuffElement[(int)Element.Dark] = -2.0;
        //AreaDebuff追加
    }
    public override long minLevel => 150;
    public override long maxLevel => 150;
    public override MonsterRarity defaultRarity => MonsterRarity.Rare;
    public override void SetWave()
    {
        //5waveか10waveごとにカラーをかえる？->その度に抵抗の装備をつけさせるのが目的

        //for (int i = 0; i < 2; i++)
        //{
        wave.Add(RandomWave(10, MonsterColor.Blue));
        //}
        //for (int i = 0; i < 2; i++)
        //{
        //    wave.Add(RandomWave(10, MonsterColor.Yellow));
        //}
        //for (int i = 0; i < 2; i++)
        //{
        //    wave.Add(RandomWave(10, MonsterColor.Red));
        //}
        //for (int i = 0; i < 2; i++)
        //{
        //    wave.Add(RandomWave(10, MonsterColor.Green));
        //}
        //for (int i = 0; i < 2; i++)
        //{
        //    wave.Add(RandomWave(10, MonsterColor.Purple));
        //}
        //wave.Add(RandomWave(10));
        //wave.Add(DefaultWave(1, MonsterColor.Boss));
        //wave.Add(RandomWave(10));
        //wave.Add(DefaultWave(1, MonsterColor.Boss));
        //wave.Add(RandomWave(10));
        //wave.Add(RandomWave(2, MonsterColor.Boss));
        //wave.Add(RandomWave(10));
        //wave.Add(RandomWave(2, MonsterColor.Boss));
        //wave.Add(RandomWave(10));
        //wave.Add(RandomWave(3, MonsterColor.Boss, MaxLevel));
    }
}
