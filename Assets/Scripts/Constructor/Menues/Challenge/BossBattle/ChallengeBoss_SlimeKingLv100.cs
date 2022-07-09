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

public class ChallengeRaidBoss_SlimeKingLv100 : CHALLENGE_RAIDBOSS
{
    public override ChallengeKind kind => SlimeKingRaid100;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public override string NameString()
    {
        return "Florzporb" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override void SetArea()
    {
        area = new ChallengeArea_SlimeKingLv100(this);
    }
    public override string RewardString()
    {
        return "Talisman [ " + localized.PotionName(PotionKind.FlorzporbDoll) + " ]";
    }
    public override void SetReward()
    {
        //Soloの解禁
        game.challengeCtrl.Challenge(SlimeKingSingle100).unlock.RegisterCondition(IsClearedOnce, "Raid [ " + NameString() + " ]");
    }
    public override void ClaimAction()
    {
        ClaimActionRewardRaidTalisman(PotionKind.FlorzporbDoll);
    }
    public override string DescriptionString()
    {
        return "Florzporb, the King of the Slimes, was once a human king who ruled over what is now the Slime Village and Magicslime City. It's merely hearsay now, for all records were destroyed, but they say it was his hubris against the gods that brought this terrible curse upon his land. Great caution should be utilized when approaching him, for he still commands the ability to summon a variety of slimes to his side to defend him from any would be assassins.";
    }
}
public class ChallengeSingleBoss_SlimeKingLv100 : CHALLENGE_SINGLEBOSS
{
    public override ChallengeKind kind => SlimeKingSingle100;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
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
            if (ClaimActionEnchantScroll(EnchantKind.OptionAdd, EquipmentEffectKind.EXPGain, 10))
                isReceivedRewardComplete = true;
        }
    }
    public override string NameString()
    {
        return "Florzporb" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override void SetArea()
    {
        area = new ChallengeArea_SlimeKingLv100(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return ClassExclusiveRewardStringEquipmentSlot(EquipmentPart.Weapon);
    }
    public override string CompleteRewardString()
    {
        return localized.EnchantName(EnchantKind.OptionAdd) + " [ " + localized.EquipmentEffectName(EquipmentEffectKind.EXPGain) + " Lv 10 ]";
    }
    public override void SetReward()
    {
        //Handicapの解禁
        game.challengeCtrl.Challenge(HCSlimeKing100).unlock.RegisterCondition(IsClearedOnce, "Solo [ " + NameString() + " ]");

        //Class
        SetRewardEquipmentSlot(EquipmentPart.Weapon);
    }
}

public class ChallengeHandicap_SlimeKingLv100 : CHALLENGE_HANDICAPPED
{
    public override ChallengeKind kind => HCSlimeKing100;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public ChallengeHandicap_SlimeKingLv100()
    {
        handicapKindList.Add(Only1Weapon);
        handicapKindList.Add(Only2ClassSkill);
    }
    public override string NameString()
    {
        return "Florzporb" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override string ClearCondition()
    {
        return "Defeat " + NameString();
    }
    public override void SetArea()
    {
        area = new ChallengeArea_SlimeKingLv100(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return new string[]
        {
            localized.BasicStats(BasicStatsKind.HP) + " + " + percent(0.10),
            localized.BasicStats(BasicStatsKind.MP) + " + " + percent(0.10),
            localized.BasicStats(BasicStatsKind.ATK) + " + " + percent(0.10),
            localized.BasicStats(BasicStatsKind.MATK) + " + " + percent(0.10),
            localized.BasicStats(BasicStatsKind.DEF) + " + " + percent(0.10),
            localized.BasicStats(BasicStatsKind.MDEF) + " + " + percent(0.10),
        };
    }
    public override string CompleteRewardString()
    {
        return "Talisman [ Placeholder ]";// + localized.PotionName(PotionKind.GuildMembersEmblem) + " ] ";
    }
    public override void SetReward()
    {
        //Class
        game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardWarrior));
        game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardWizard));
        game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardAngel));
        game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardThief));
        game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardArcher));
        game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(Challenge, Mul, () => 0.10, () => isReceivedRewardTamer));
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



public class ChallengeArea_SlimeKingLv100 : CHALLENGE_AREA
{
    public ChallengeArea_SlimeKingLv100(CHALLENGE challenge) : base(challenge)//AreaController areaCtrl) : base(areaCtrl)
    {
        kind = AreaKind.SlimeVillage;
        debuffPhyCrit = -0.10d;
        debuffMagCrit = -0.10d;
        //id = (int)challengeMonsterKind;
    }
    //public override ChallengeMonsterKind challengeMonsterKind => ChallengeMonsterKind.SlimeKing;
    public override long minLevel => 100;
    public override long maxLevel => 100;
    public override void SetWave()
    {
        wave.Add(ChallengeWave(SlimeKing));
    }
}

public class Battle_SlimeKing : CHALLENGE_BATTLE
{
    public override float moveSpeed => 0;
    public override float range => 1000;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public Battle_SlimeKing(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
    }
    long yellowCount;
    public override void Activate()
    {
        base.Activate();
        currentAttackColor = AttackColor.Yellow;
        yellowCount = 0;
    }
    public override void SetAttack()
    {
        //Blue
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 50f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 1000));
        //Red
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => move.position, (x) => Damage(), IsCrit, () => 150f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 500));
    }
    public override void UpdateTriggerSkill()
    {
        if (!isMpCharged) return;
        switch (currentAttackColor)
        {
            case AttackColor.Blue://通常攻撃（スライムボール）
                attack[0].ThrowAttack(this, () => Vector2.up, true);
                attack[1].ThrowAttack(this, () => Vector2.right, true);
                attack[2].ThrowAttack(this, () => Vector2.down, true);
                attack[3].ThrowAttack(this, () => Vector2.left, true);
                attack[4].ThrowAttack(this, () => Vector2.up + Vector2.right, true);
                attack[5].ThrowAttack(this, () => Vector2.down + Vector2.right, true);
                attack[6].ThrowAttack(this, () => Vector2.up + Vector2.left, true);
                attack[7].ThrowAttack(this, () => Vector2.down + Vector2.left, true);
                if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null)
                {
                    skillEffectUIAction(attack[0], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[1], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[2], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[3], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[4], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[5], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[6], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                    skillEffectUIAction(attack[7], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                }
                break;
            case AttackColor.Yellow://仲間呼び
                MonsterColor tempColor = Normal;
                if (HpPercent() < 0.20d) tempColor = Purple;
                else if (HpPercent() < 0.40d) tempColor = Green;
                else if (HpPercent() < 0.60d) tempColor = Red;
                else if (HpPercent() < 0.75d) tempColor = Yellow;
                else if (HpPercent() < 0.90d) tempColor = Blue;
                for (int i = 0; i < SpawnNum(); i++)
                {
                    SpawnSlime(tempColor);
                }
                yellowCount++;
                break;
            case AttackColor.Green://回復
                Heal(hp * 0.01d);//1%回復
                for (int i = 1; i < battleCtrl.monsters.Length; i++)
                {
                    battleCtrl.monsters[i].FullHeal();
                }
                break;
            case AttackColor.Red:
                attack[8].ThrowAttack(this, null, true);
                if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null)
                {
                    skillEffectUIAction(attack[8], SpriteSourceUI.sprite.challengeAttackEffects[(int)SlimeKing]);
                }
                break;
        }
        currentMp.ChangeValue(0);

        //Colorの変更
        float tempRand = UnityEngine.Random.Range(0f, 1f);
        if (battleCtrl.MonsterAliveNum() <= 3) currentAttackColor = AttackColor.Yellow;
        else if (currentAttackColor == AttackColor.Yellow)
        {
            if (tempRand < 0.50d) currentAttackColor = AttackColor.Green;
            else if (tempRand < 0.90d) currentAttackColor = AttackColor.Blue;
            else currentAttackColor = AttackColor.Red;
        }
        else
        {
            if (tempRand < 0.10d) currentAttackColor = AttackColor.Green;
            else if (tempRand < 0.50d) currentAttackColor = AttackColor.Blue;
            else currentAttackColor = AttackColor.Red;
        }
    }
    double SpawnNum()
    {
        return Math.Min(Math.Min(level / 10d, 30), level / 20d + yellowCount);
    }
    void SpawnSlime(MonsterColor color) { SpawnAnotherMonster(Slime, color, level, difficulty, Parameter.RandomVec() * UnityEngine.Random.Range(100, 400)); }
}