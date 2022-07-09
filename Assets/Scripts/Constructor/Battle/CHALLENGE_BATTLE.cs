using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static MonsterParameter;
using static GameController;
using static UsefulMethod;
using static MonsterSpecies;
using static MonsterColor;

public class CHALLENGE_BATTLE : MONSTER_BATTLE
{
    public override MonsterGlobalInformation globalInformation { get => game.monsterCtrl.GlobalInformationChallengeBoss(challengeMonsterKind); }

    public CHALLENGE_BATTLE(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
        species = ChallengeBoss;
        spawnId = new ArrayId(0, () => battleCtrl.monsters.Length - 1);
    }

    //攻撃パターン　SetAttack()
    //実際の攻撃時の処理　UpdateTriggerSkill()

    //雑魚敵の出現
    public ArrayId spawnId;
    public void SpawnAnotherMonster(MonsterSpecies species, MonsterColor color, long level, double difficulty, Vector2 position)
    {
        battleCtrl.monsters[spawnId.value].Spawn(species, color, level, difficulty, position);
        spawnId.Increase();
    }

    //Spawn
    public void Spawn(long level, double difficulty, Vector2 position)
    {
        this.level = level;
        this.difficulty = difficulty;
        Activate();
        if (spawnUIAction != null) spawnUIAction();
        move.MoveTo(position);
        SetAttack();
    }

    //UI
    public override bool CanShowStats(HeroKind heroKind)
    {
        return true;// level <= game.statsCtrl.MonsterDistinguishMaxLevel(heroKind).Value();
    }
}
public enum AttackColor
{
    Blue,//Normal
    Green,//回復系
    Yellow,//特殊
    Red,//強力な攻撃
    Purple,//毒・Dark系
    Gray,//Debuff系
    Orange,//特殊
}
