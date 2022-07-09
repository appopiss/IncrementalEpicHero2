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
using Cysharp.Threading.Tasks;

public class ChallengeRaidBoss_GolemLv200 : CHALLENGE_RAIDBOSS
{
    public override ChallengeKind kind => GolemRaid200;
    public override ChallengeMonsterKind challengeMonsterKind => Golem;
    public override string NameString()
    {
        return "Guardian Kor" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override void SetArea()
    {
        area = new ChallengeArea_GolemLv200(this);
    }
    public override string RewardString()
    {
        return "Talisman [ " + localized.PotionName(PotionKind.GuardianKorDoll) + " ]";
    }
    public override void SetReward()
    {
        //Soloの解禁
        game.challengeCtrl.Challenge(GolemSingle200).unlock.RegisterCondition(IsClearedOnce, "Raid [ " + NameString() + " ]");
    }
    public override void ClaimAction()
    {
        ClaimActionRewardRaidTalisman(PotionKind.GuardianKorDoll);
    }
    public override string DescriptionString()
    {
        return "Guardian Kor was originally built to guard the Royal Vault of the dwarven kingdom. However, no one has heard from or seen a dwarf in over a century. Their cities abandoned, ruins all that remain, and their creations left to rust and decay. This faithful guardian, however, through use of some aberrant magic and technological marvel, remains as hearty and capable as ever. The vault he once guarded has collapsed, all of its treasures lost into a deep magma flow, but still he guards its entrance as is his purpose and none shall bar him from performing it.";
    }
}
public class ChallengeSingleBoss_GolemLv200 : CHALLENGE_SINGLEBOSS
{
    public override ChallengeKind kind => GolemSingle200;
    public override ChallengeMonsterKind challengeMonsterKind => Golem;
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
            if (ClaimActionEnchantScroll(EnchantKind.OptionAdd, EquipmentEffectKind.SkillProficiency, 10))
                isReceivedRewardComplete = true;
        }
    }
    public override string NameString()
    {
        return "Guardian Kor" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override void SetArea()
    {
        area = new ChallengeArea_GolemLv200(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return ClassExclusiveRewardStringEquipmentSlot(EquipmentPart.Jewelry);

    }
    public override string CompleteRewardString()
    {
        return localized.EnchantName(EnchantKind.OptionAdd) + " [ " + localized.EquipmentEffectName(EquipmentEffectKind.SkillProficiency) + " Lv 10 ]";
    }
    public override void SetReward()
    {
        //Handicapの解禁
        game.challengeCtrl.Challenge(HCGolem200).unlock.RegisterCondition(IsClearedOnce, "Solo [ " + NameString() + " ]");
        //Class
        SetRewardEquipmentSlot(EquipmentPart.Jewelry);
    }
}

public class ChallengeHandicap_GolemLv200 : CHALLENGE_HANDICAPPED
{
    public override ChallengeKind kind => HCGolem200;
    public override ChallengeMonsterKind challengeMonsterKind => Golem;
    public ChallengeHandicap_GolemLv200()
    {
        handicapKindList.Add(Only1Jewelry);
        handicapKindList.Add(Only2ClassSkillAnd1Global);
    }
    public override string NameString()
    {
        return "Guardian Kor" + " Lv " + tDigit(area.MaxLevel()); ;
    }
    public override string ClearCondition()
    {
        return "Defeat " + NameString();
    }
    public override void SetArea()
    {
        area = new ChallengeArea_GolemLv200(this);
    }
    public override string[] ClassExclusiveRewardString()
    {
        return new string[]
        {
            localized.BasicStats(BasicStatsKind.HP) + " + " + percent(0.30),
            localized.BasicStats(BasicStatsKind.MP) + " + " + percent(0.30),
            localized.BasicStats(BasicStatsKind.ATK) + " + " + percent(0.30),
            localized.BasicStats(BasicStatsKind.MATK) + " + " + percent(0.30),
            localized.BasicStats(BasicStatsKind.DEF) + " + " + percent(0.30),
            localized.BasicStats(BasicStatsKind.MDEF) + " + " + percent(0.30),
        };
    }
    public override string CompleteRewardString()
    {
        return "Talisman [ Placeholder ]";// + localized.PotionName(PotionKind.GuildMembersEmblem) + " ] ";
    }
    public override void SetReward()
    {
        //Class
        game.statsCtrl.SetEffect(BasicStatsKind.HP, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardWarrior));
        game.statsCtrl.SetEffect(BasicStatsKind.MP, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardWizard));
        game.statsCtrl.SetEffect(BasicStatsKind.ATK, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardAngel));
        game.statsCtrl.SetEffect(BasicStatsKind.MATK, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardThief));
        game.statsCtrl.SetEffect(BasicStatsKind.DEF, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardArcher));
        game.statsCtrl.SetEffect(BasicStatsKind.MDEF, new MultiplierInfo(Challenge, Mul, () => 0.30, () => isReceivedRewardTamer));
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


public class ChallengeArea_GolemLv200 : CHALLENGE_AREA
{
    public ChallengeArea_GolemLv200(CHALLENGE challenge) : base(challenge)//AreaController areaCtrl) : base(areaCtrl)
    {
        kind = AreaKind.MagicSlimeCity;
        debuffElement[(int)Element.Fire] = -1.0;
        debuffPhyCrit = -0.50d;
        debuffMagCrit = -0.50d;
        //id = (int)challengeMonsterKind;
    }
    public override long minLevel => 200;
    public override long maxLevel => 200;
    public override void SetWave()
    {
        wave.Add(ChallengeWave(Golem, null, null, 0, -1.2f));
    }
}

public class Battle_Golem : CHALLENGE_BATTLE
{
    public override float moveSpeed => 0;
    public override float range => 1000;
    //public double defendFactor = 1.0d;
    public bool isPhyInvalid, isMagInvalid;
    //public override double def => base.def * defendFactor;
    //public override double mdef => base.mdef * defendFactor;
    public override double physicalInvalidChance => Convert.ToInt16(isPhyInvalid);
    public override double fireInvalidChance => Convert.ToInt16(isMagInvalid);
    public override double iceInvalidChance => Convert.ToInt16(isMagInvalid);
    public override double thunderInvalidChance => Convert.ToInt16(isMagInvalid);
    public override double lightInvalidChance => Convert.ToInt16(isMagInvalid);
    public override double darkInvalidChance => Convert.ToInt16(isMagInvalid);
    public override double maxInvalidChance => 1.0d;
    public override ChallengeMonsterKind challengeMonsterKind => Golem;
    public Battle_Golem(BATTLE_CONTROLLER battleCtrl) : base(battleCtrl)
    {
    }
    Vector2 targetPosition = Parameter.heroInitPosition;
    public override void Activate()
    {
        base.Activate();
        if (WithinRandom(0.5f)) currentAttackColor = AttackColor.Purple;
        else currentAttackColor = AttackColor.Blue;
        isPhyInvalid = false;
        isMagInvalid = false;
        //defendFactor = 1.0d;
    }
    public override void SetAttack()
    {
        //Red
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => targetPosition, (x) => Damage(), ()=>true, () => 200f, () => 1, () => Element.Fire, () => Debuff.AtkDown));

        //Gray
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(400f ,400f), (x) => Damage()*0.5d, IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MatkDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(-400f, 400f), (x) => Damage() * 0.5d, IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MatkDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(-400f, -400f), (x) => Damage() * 0.5d, IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MatkDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(400f, -400f), (x) => Damage() * 0.5d, IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MatkDown, () => Target().move.position, () => 750));

        //Orange
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(-350f, 400f), (x) => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(-175f, 400f), (x) => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MdefDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0000f, 400f), (x) => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.AtkDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0175f, 400f), (x) => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.MatkDown, () => Target().move.position, () => 750));
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0350f, 400f), (x) => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.DefDown, () => Target().move.position, () => 750));
        //attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0300f, 400f), () => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.AtkDown, () => Target().move.position, () => 750));
        //attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0250f, 400f), () => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.AtkDown, () => Target().move.position, () => 750));
        //attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => new Vector2(0350f, 400f), () => Damage(), IsCrit, () => 100f, () => 1, () => attackElement, () => Debuff.AtkDown, () => Target().move.position, () => 750));
    }
    public override void UpdateTriggerSkill()
    {
        if (!isMpCharged) return;
        switch (currentAttackColor)
        {
            case AttackColor.Blue://PhysicalInvalid
                isPhyInvalid = true;
                isMagInvalid = false;
                if (game.IsUI(battleCtrl.heroKind) && attackUIActionList[0] != null) attackUIActionList[0]();
                break;
            case AttackColor.Purple://MagInvalid
                isPhyInvalid = false;
                isMagInvalid = true;
                if (game.IsUI(battleCtrl.heroKind) && attackUIActionList[1] != null) attackUIActionList[1]();
                break;
            case AttackColor.Red://Heroのいる場所に爆発攻撃
                attack[0].NormalAttack(this);
                if (game.IsUI(battleCtrl.heroKind) && particleEffectUIAction != null)
                    particleEffectUIAction(attack[0], ParticleEffectKind.GolemBomb);
                break;
            case AttackColor.Gray://岩を投げる
                attack[1].ThrowAttack(this, null, false);
                attack[2].ThrowAttack(this, null, false);
                attack[3].ThrowAttack(this, null, false);
                attack[4].ThrowAttack(this, null, false);
                if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null)
                {
                    skillEffectUIAction(attack[1], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                    skillEffectUIAction(attack[2], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                    skillEffectUIAction(attack[3], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                    skillEffectUIAction(attack[4], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                }
                break;
            case AttackColor.Orange://岩がランダムに降ってくる
                int rand = UnityEngine.Random.Range(0, 5);
                for (int j = 0; j < 5; j++)
                {
                    int count = j;
                    if (j != rand)
                    {
                        attack[count + 5].ThrowAttack(this, () => Vector2.down, true);
                        if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null) skillEffectUIAction(attack[count + 5], SpriteSourceUI.sprite.challengeAttackEffects[(int)challengeMonsterKind]);
                    }
                }
                break;
        }
        currentMp.ChangeValue(0);

        //Colorの変更
        targetPosition = Target().move.position;
        float tempRand = UnityEngine.Random.Range(0f, 1f);
        switch (currentAttackColor)
        {
            case AttackColor.Blue:
                currentAttackColor = AttackColor.Gray;
                break;
            case AttackColor.Purple:
                currentAttackColor = AttackColor.Red;
                break;
            default:
                if (isPhyInvalid)
                {
                    if (tempRand < 0.30f) currentAttackColor = AttackColor.Purple;
                    else if (tempRand < 0.60f) currentAttackColor = AttackColor.Red;
                    else currentAttackColor = AttackColor.Gray;
                }
                else
                {
                    if (tempRand < 0.30f) currentAttackColor = AttackColor.Blue;
                    else if (tempRand < 0.60f) currentAttackColor = AttackColor.Gray;
                    else currentAttackColor = AttackColor.Red;
                }
                if (WithinRandom(0.33f)) currentAttackColor = AttackColor.Orange;
                break;
        }
    }
}

public class ChallengeBossUI_Golem : MonsterUI
{
    public ChallengeBossUI_Golem(Func<BattleController> battleCtrl, GameObject gameObject, int id, bool isChallenge = false) : base(battleCtrl, gameObject, id, isChallenge)
    {
        physicalProtect = thisObject.transform.GetChild(5).gameObject;
        magicalProtect = thisObject.transform.GetChild(6).gameObject;
    }
    public override void SetUI()
    {
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].challengeMonsters[(int)Golem].attackUIActionList.Add(() => InstantiateProtection(true));
            game.battleCtrls[i].challengeMonsters[(int)Golem].attackUIActionList.Add(() => InstantiateProtection(false));
        }
    }
    public GameObject physicalProtect, magicalProtect;
    public void InstantiateProtection(bool isPhysical = true)
    {
        SetActive(physicalProtect, isPhysical);
        SetActive(magicalProtect, !isPhysical);
    }
    public override void SetSprite()
    {
        SetActive(physicalProtect, false);
        SetActive(magicalProtect, false);
        base.SetSprite();
    }
}
