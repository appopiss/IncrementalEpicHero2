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
using static MonsterSpecies;
using static MonsterColor;
using static ChallengeHandicapKind;

public class ChallengeRaidBoss_WindowQueenLv150 : CHALLENGE_RAIDBOSS
{
    public override ChallengeKind kind => WindowQueenRaid150;
    public override ChallengeMonsterKind challengeMonsterKind => WindowQueen;
    public override string NameString()
    {
        return "Arachnetta" + " Lv " + tDigit(area.MaxLevel());
    }
    public override void SetArea()
    {
        area = new ChallengeArea_WindowQueenLv150(this);
    }
    public override string RewardString()
    {
        return "Talisman [ " + localized.PotionName(PotionKind.ArachnettaDoll) + " ]";
    }
    public override void SetReward()
    {
        //Soloの解禁
        game.challengeCtrl.Challenge(WindowQueenSingle150).unlock.RegisterCondition(IsClearedOnce, "Raid [ " + NameString() + " ]");
    }
    public override void ClaimAction()
    {
        ClaimActionRewardRaidTalisman(PotionKind.ArachnettaDoll);
    }
    public override string DescriptionString()
    {
        return "Arachnetta, the Widow Queen, is a monstrous creature that appeared suddenly over a decade ago. Her countless minions overwhelmed the Tressel Estate and all of its many orchards and villages. No one has survived her occupation. She currently has made the hedge maze behind the Tressel Estate Mansion her brooding ground. Any adventurers attempting to exterminate her should be aware that her poison is perhaps the most deadly in the land, and she has only made meals of all of the great heroes sent to slay her.";
    }
}

public class ChallengeSingleBoss_WindowQueenLv150 : CHALLENGE_SINGLEBOSS
{
    public override ChallengeKind kind => WindowQueenSingle150;
    public override ChallengeMonsterKind challengeMonsterKind => WindowQueen;
    public override string NameString()
    {
        return "Arachnetta" + " Lv " + tDigit(area.MaxLevel());
    }
    public override void SetArea()
    {
        area = new ChallengeArea_WindowQueenLv150(this);
    }
    public override void ClaimAction()
    {
        base.ClaimAction();
        //SkillSlotの場合は解放UIの処理が必要
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
    public override string[] ClassExclusiveRewardString()
    {
        return new string[]
        {
            "Warrior Class Skill Slot + 1",
            "Wizard Class Skill Slot + 1",
            "Angel Class Skill Slot + 1",
            "Thief Class Skill Slot + 1",
            "Archer Class Skill Slot + 1",
            "Tamer Class Skill Slot + 1",
        };
        //Idea: InventorySlot, EQSlot, SkillSlot,
    }
    public override string CompleteRewardString()
    {
        return "Global Skill Slot + 1";
    }
    public override void SetReward()
    {
        //Handicapの解禁
        game.challengeCtrl.Challenge(HCWindowQueen150).unlock.RegisterCondition(IsClearedOnce, "Solo [ " + NameString() + " ]");
        //Class
        game.statsCtrl.SkillSlotNum(HeroKind.Warrior).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardWarrior));
        game.statsCtrl.SkillSlotNum(HeroKind.Wizard).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardWizard));
        game.statsCtrl.SkillSlotNum(HeroKind.Angel).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,   () => isReceivedRewardAngel));
        game.statsCtrl.SkillSlotNum(HeroKind.Thief).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,   () => isReceivedRewardThief));
        game.statsCtrl.SkillSlotNum(HeroKind.Archer).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardArcher));
        game.statsCtrl.SkillSlotNum(HeroKind.Tamer).RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,   () => isReceivedRewardTamer));
        //Complete
        game.statsCtrl.globalSkillSlotNum.RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardComplete));
    }
}
public class ChallengeHandicap_WindowQueenLv150 : CHALLENGE_HANDICAPPED
{
    public override ChallengeKind kind => HCWindowQueen150;
    public override ChallengeMonsterKind challengeMonsterKind => WindowQueen;
    public ChallengeHandicap_WindowQueenLv150()
    {
        handicapKindList.Add(Only1Armor);
        handicapKindList.Add(Only2ClassSkill);
    }
    public override string NameString()
    {
        return "Arachnetta" + " Lv " + tDigit(area.MaxLevel());
    }
    public override string ClearCondition()
    {
        return "Defeat " + NameString();
    }
    public override void SetArea()
    {
        area = new ChallengeArea_WindowQueenLv150(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return new string[]
        {
            localized.BasicStats(BasicStatsKind.HP) + " + " + percent(0.20),
            localized.BasicStats(BasicStatsKind.MP) + " + " + percent(0.20),
            localized.BasicStats(BasicStatsKind.ATK) + " + " + percent(0.20),
            localized.BasicStats(BasicStatsKind.MATK) + " + " + percent(0.20),
            localized.BasicStats(BasicStatsKind.DEF) + " + " + percent(0.20),
            localized.BasicStats(BasicStatsKind.MDEF) + " + " + percent(0.20),
        };
    }
    public override string CompleteRewardString()
    {
        return "Talisman [ Placeholder ]";// + localized.PotionName(PotionKind.GuildMembersEmblem) + " ] ";
    }
    public override void SetReward()
    {
        //Class
        game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardWarrior));
        game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardWizard));
        game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardAngel));
        game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardThief));
        game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardArcher));
        game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(Challenge, Mul, () => 0.20, () => isReceivedRewardTamer));
    }
    public override void ClaimAction()
    {
        if (IsClearedOnce()) isReceivedRewardOnce = true;
        if (IsCleared(HeroKind.Warrior)) isReceivedRewardWarrior = true;
        if (IsCleared(HeroKind.Wizard)) isReceivedRewardWizard = true;
        if (IsCleared(HeroKind.Angel)) isReceivedRewardAngel = true;
        if (IsCleared(HeroKind.Thief)) isReceivedRewardThief = true;
        if (IsCleared(HeroKind.Archer)) isReceivedRewardArcher = true;
        if (IsCleared(HeroKind.Tamer)) isReceivedRewardTamer = true;
        if (!isReceivedRewardComplete && IsClearedCompleted())
        {
            //if (ClaimActionTalisman(PotionKind.GuildMembersEmblem))
            //    isReceivedRewardComplete = true;
        }
    }
}


public class ChallengeArea_WindowQueenLv150 : CHALLENGE_AREA
{
    public ChallengeArea_WindowQueenLv150(CHALLENGE challenge) : base(challenge)//AreaController areaCtrl) : base(areaCtrl)
    {
        kind = AreaKind.SpiderMaze;
        debuffElement[(int)Element.Dark] = -1.0;
        debuffPhyCrit = -0.25d;
        //debuffMagCrit = -0.20d;
    }
    public override long minLevel => 150;
    public override long maxLevel => 150;
    public override void SetWave()
    {       
        wave.Add(ChallengeWave(WindowQueen));
    }
}

public class Battle_WindowQueen : CHALLENGE_BATTLE
{
    public override float moveSpeed => 0;
    public override float range => 1000;
    public override ChallengeMonsterKind challengeMonsterKind => WindowQueen;
    public Battle_WindowQueen(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
    }
    public override void Activate()
    {
        base.Activate();
        currentAttackColor = AttackColor.Purple;
    }
    public override void SetAttack()
    {
        //通常攻撃
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => Target().move.position, (x) => Damage() * 0.50d, IsCrit, () => 50f, () => 1, () => attackElement, () => LotteryDebuff()));
        //毒ガス
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage() * 0.25d, IsCrit, () => 150f, () => 1, () => attackElement, () => Debuff.Poison));
        //Venom SPDDown
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 80f, () => 1, () => attackElement, () => Debuff.SpdDown, () => Target().move.position, () => 1000));
    }
    public override void UpdateTriggerSkill()
    {
        if (!isMpCharged) return;
        switch (currentAttackColor)
        {
            case AttackColor.Blue://通常攻撃
                attack[0].NormalAttack(this);
                if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null)
                    skillEffectUIAction(attack[0], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                break;
            case AttackColor.Purple://毒ガス
                attack[1].LoopAttack(this, 2.5f, 0.5f);
                if (game.IsUI(battleCtrl.heroKind) && particleEffectUIAction != null)
                    particleEffectUIAction(attack[1], ParticleEffectKind.PoisonGas);
                break;
            case AttackColor.Gray://Venom(SPD Down)
                attack[2].ThrowAttack(this, null, true);
                if (game.IsUI(battleCtrl.heroKind) && particleEffectUIAction != null)
                    particleEffectUIAction(attack[2], ParticleEffectKind.Venom);
                break;
            case AttackColor.Yellow://仲間呼び
                MonsterColor tempColor = Normal;
                if (HpPercent() < 0.20d) tempColor = Purple;
                else if (HpPercent() < 0.40d) tempColor = Green;
                else if (HpPercent() < 0.60d) tempColor = Red;
                else if (HpPercent() < 0.75d) tempColor = Yellow;
                else if (HpPercent() < 0.90d) tempColor = Blue;
                for (int i = 0; i < 10; i++)
                {
                    SpawnSpider(tempColor);
                }
                break;
        }
        currentMp.ChangeValue(0);

        //Colorの変更
        float tempRand = UnityEngine.Random.Range(0f, 1f);
        if (Distance(this, Target()) > 150f)//毒ガスが当たらない範囲にいる場合
        {
            if (tempRand < 0.10f) currentAttackColor = AttackColor.Blue;
            else if (tempRand < 0.60f) currentAttackColor = AttackColor.Gray;
            else currentAttackColor = AttackColor.Yellow;
        }
        else//毒ガスが当たる範囲にいる場合
        {
            if (tempRand < 0.60f) currentAttackColor = AttackColor.Purple;
            else if (tempRand < 0.85f) currentAttackColor = AttackColor.Gray;
            else currentAttackColor = AttackColor.Yellow;
        }
        if (currentAttackColor == AttackColor.Yellow && battleCtrl.MonsterAliveNum() >= 10) currentAttackColor = AttackColor.Gray;
    }
    void SpawnSpider(MonsterColor color) { SpawnAnotherMonster(Spider, color, level, difficulty, Parameter.RandomVec() * UnityEngine.Random.Range(100, 400)); }
}