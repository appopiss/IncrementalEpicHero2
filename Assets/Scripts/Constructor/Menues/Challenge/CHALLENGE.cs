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

public partial class SaveR
{
    public bool[] isClearedChallenge;//[10 * challengeKind + (int)heroKind)]
    public bool[] isReceivedRewardsChallenge;//[10 * challengeKInd + id]//id:0=Clear Once, 1~6=class, 7=Complete

    public double[] accomplishedFirstTimesChallenge;//[challengeKind]
    public double[] accomplishedTimesChallenge;//[challengeKind]
    public double[] accomplishedBestTimesChallenge;//[challengeKind]
}

public class AccomplishChallenge : ACCOMPLISH
{
    public AccomplishChallenge(CHALLENGE challenge)
    {
        this.challenge = challenge;
    }
    CHALLENGE challenge;
    public override double accomplishedFirstTime { get => main.SR.accomplishedFirstTimesChallenge[(int)challenge.kind]; set => main.SR.accomplishedFirstTimesChallenge[(int)challenge.kind] = value; }
    public override double accomplishedTime { get => main.SR.accomplishedTimesChallenge[(int)challenge.kind]; set => main.SR.accomplishedTimesChallenge[(int)challenge.kind] = value; }
    public override double accomplishedBestTime { get => main.SR.accomplishedBestTimesChallenge[(int)challenge.kind]; set => main.SR.accomplishedBestTimesChallenge[(int)challenge.kind] = value; }
}

public class CHALLENGE
{
    public CHALLENGE()
    {
        unlock = new Unlock();
        accomplish = new AccomplishChallenge(this);
        SetArea();
    }
    public void Start()
    {
        SetReward();
    }
    public HeroKind currentHeroKind => game.currentHero;
    int saveId => (int)currentHeroKind + (int)kind * 10;
    public bool isClearedCurrentHero { get => main.SR.isClearedChallenge[saveId]; set => main.SR.isClearedChallenge[saveId] = value; }
    public bool IsCleared(HeroKind heroKind) { return main.SR.isClearedChallenge[(int)heroKind + 10 * (int)kind]; }
    public bool IsClearedOnce()//1度でもクリアしたことがあるか
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (IsCleared((HeroKind)count)) return true;
        }
        return false;
    }
    public bool IsClearedCompleted()//全員クリアしてるか
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (!IsCleared((HeroKind)count)) return false;
        }
        return true;
    }
    public virtual ChallengeType type => RaidBossBattle;
    public virtual ChallengeKind kind => SlimeKingRaid100;
    public virtual ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public List<ChallengeHandicapKind> handicapKindList = new List<ChallengeHandicapKind>();
    //StartBattleはこの順番でないといけない。
    public virtual void StartBattle() { game.battleCtrl.areaBattle.StartChallenge(area); isTryingThisChallenge = true; }//Startボタンを押した時
    public void Quit() { if (!isTryingThisChallenge) return; game.battleCtrl.areaBattle.QuitCurrentArea(); Initialize(); }//Quitボタンを押した時
    public bool CanStart() { return unlock.IsUnlocked() && area.CanStart(); }
    public virtual void Initialize()
    {
        isTryingThisChallenge = false;
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].isTryingRaid = false;
        }
    }
    public bool isReceivedRewardOnce { get => main.SR.isReceivedRewardsChallenge[0 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[0 + (int)kind * 10] = value; }
    public bool isReceivedRewardWarrior { get => main.SR.isReceivedRewardsChallenge[1 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[1 + (int)kind * 10] = value; }
    public bool isReceivedRewardWizard { get => main.SR.isReceivedRewardsChallenge[2 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[2 + (int)kind * 10] = value; }
    public bool isReceivedRewardAngel { get => main.SR.isReceivedRewardsChallenge[3 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[3 + (int)kind * 10] = value; }
    public bool isReceivedRewardThief { get => main.SR.isReceivedRewardsChallenge[4 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[4 + (int)kind * 10] = value; }
    public bool isReceivedRewardArcher { get => main.SR.isReceivedRewardsChallenge[5 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[5 + (int)kind * 10] = value; }
    public bool isReceivedRewardTamer { get => main.SR.isReceivedRewardsChallenge[6 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[6 + (int)kind * 10] = value; }
    public bool isReceivedRewardComplete { get => main.SR.isReceivedRewardsChallenge[7 + (int)kind * 10]; set => main.SR.isReceivedRewardsChallenge[7 + (int)kind * 10] = value; }
    public bool IsReceivedRewardClass(HeroKind heroKind)
    {
        switch (heroKind)
        {
            case HeroKind.Warrior: return isReceivedRewardWarrior;
            case HeroKind.Wizard: return isReceivedRewardWizard;
            case HeroKind.Angel: return isReceivedRewardAngel;
            case HeroKind.Thief: return isReceivedRewardThief;
            case HeroKind.Archer: return isReceivedRewardArcher;
            case HeroKind.Tamer: return isReceivedRewardTamer;
        }
        return false;
    }
    public double rewardMultiplier => game.challengeCtrl.permanentRewardMultiplier.Value();
    public virtual void SetReward() { }
    //public virtual bool GetOneTimeReward() { return false; }//Rewardを正常に受け取れたかどうかをboolで返す
    public virtual void SetArea() { }
    public CHALLENGE_AREA area;
    public bool isTryingThisChallenge;
    public Unlock unlock;
    public AccomplishChallenge accomplish;
    public virtual void ClearAction() { }
    public virtual void ClaimAction()//Rewardの受け取り
    {
        if (IsClearedOnce()) isReceivedRewardOnce = true;
        if (IsCleared(HeroKind.Warrior)) isReceivedRewardWarrior = true;
        if (IsCleared(HeroKind.Wizard)) isReceivedRewardWizard = true;
        if (IsCleared(HeroKind.Angel)) isReceivedRewardAngel = true;
        if (IsCleared(HeroKind.Thief)) isReceivedRewardThief = true;
        if (IsCleared(HeroKind.Archer)) isReceivedRewardArcher = true;
        if (IsCleared(HeroKind.Tamer)) isReceivedRewardTamer = true;
        if (IsClearedCompleted()) isReceivedRewardComplete = true;
    }
    public bool CanClaim()
    {
        if (!isReceivedRewardOnce && IsClearedOnce()) return true;
        if (!isReceivedRewardWarrior && IsCleared(HeroKind.Warrior)) return true;
        if (!isReceivedRewardWizard && IsCleared(HeroKind.Wizard)) return true;
        if (!isReceivedRewardAngel && IsCleared(HeroKind.Angel)) return true;
        if (!isReceivedRewardThief && IsCleared(HeroKind.Thief)) return true;
        if (!isReceivedRewardArcher && IsCleared(HeroKind.Archer)) return true;
        if (!isReceivedRewardTamer && IsCleared(HeroKind.Tamer)) return true;
        if (!isReceivedRewardComplete && IsClearedCompleted()) return true;
        return false;
    }
    public virtual string NameString() { return ""; }
    public virtual string ClearConditionString() { return ""; }
    public virtual string InformationString()
    {
        string tempStr = optStr + "<size=20><u>Information</u><size=18>";
        tempStr += "\n- Region : " + localized.AreaName(area.kind);
        bool isDebuff = false;
        tempStr += "\n- Field Debuff : ";
        for (int i = 0; i < area.debuffElement.Length; i++)
        {
            int count = i;
            if (area.debuffElement[count] != 0)
            {
                if (isDebuff) tempStr += ", ";
                tempStr += "<sprite=\"stats\" index=" + (6 + count).ToString() + ">" + percent(area.debuffElement[count], 0);
                isDebuff = true;
            }
        }
        if (area.debuffPhyCrit != 0)
        {
            if (isDebuff) tempStr += ", ";
            tempStr += "PhysCrit " + percent(area.debuffPhyCrit, 0);
            isDebuff = true;
        }
        if (area.debuffMagCrit != 0)
        {
            if (isDebuff) tempStr += ", ";
            tempStr += "MagCrit " + percent(area.debuffMagCrit, 0);
            isDebuff = true;
        }
        if (!isDebuff) tempStr += "Nothing";
        return tempStr;
    }
    public virtual string RewardString() { return ""; }
    public virtual string RewardInfoString() { return ""; }
    public virtual string DescriptionString() { return ""; }
    //public virtual string ClassExclusiveRewardInfoString() { return ""; }
    public virtual string[] ClassExclusiveRewardString() { return new string[] { "", "", "", "", "", "" }; }
    public virtual string CompleteRewardString() { return ""; }
    //public virtual string CompleteRewardInfoString() { return ""; }
    public virtual string TitleUIString() { return ""; }
    public string InfoUIString()
    {
        string tempStr = optStr;
        tempStr += ClearConditionString();
        tempStr += "\n\n";
        tempStr += InformationString();
        tempStr += "\n\n";
        tempStr += RewardInfoString();
        tempStr += "\n\n";
        if (DescriptionString() != "")
        {
            tempStr += "<size=18>" + DescriptionString();
            tempStr += "\n\n";
        }
        if (accomplish.accomplishedBestTime <= 0) return tempStr;
        tempStr += "<size=20><u>Clear Playtime</u><size=18>";
        tempStr += "\n- Best : <color=green>" + DoubleTimeToDate(accomplish.accomplishedBestTime) + "</color>";
        tempStr += "\n- This World Ascension : <color=green>" + DoubleTimeToDate(accomplish.accomplishedTime) + "</color>";
        tempStr += "\n- First : <color=green>" + DoubleTimeToDate(accomplish.accomplishedFirstTime) + "</color>";
        return tempStr;
    }

    //Reward用
    public void ClaimActionRewardRaidTalisman(PotionKind kind, int num = 1)
    {
        if (!isReceivedRewardOnce && IsClearedOnce())
        {
            if (ClaimActionTalisman(kind, num))
            {
                isReceivedRewardOnce = true;
            }
        }
        if (IsCleared(HeroKind.Warrior)) isReceivedRewardWarrior = true;
        if (IsCleared(HeroKind.Wizard)) isReceivedRewardWizard = true;
        if (IsCleared(HeroKind.Angel)) isReceivedRewardAngel = true;
        if (IsCleared(HeroKind.Thief)) isReceivedRewardThief = true;
        if (IsCleared(HeroKind.Archer)) isReceivedRewardArcher = true;
        if (IsCleared(HeroKind.Tamer)) isReceivedRewardTamer = true;
        if (IsClearedCompleted()) isReceivedRewardComplete = true;
    }
    public void ClaimActionClassBadgeTalisman(HeroKind heroKind, int num = 1)
    {
        if (!IsReceivedRewardClass(heroKind) && IsCleared(heroKind))
        {
            if (ClaimActionClassBadgeTalismanBool(heroKind, num))
            {
                switch (heroKind)
                {
                    case HeroKind.Warrior: isReceivedRewardWarrior = true;  break;
                    case HeroKind.Wizard: isReceivedRewardWizard = true; break;
                    case HeroKind.Angel: isReceivedRewardAngel = true; break;
                    case HeroKind.Thief: isReceivedRewardThief = true; break;
                    case HeroKind.Archer: isReceivedRewardArcher = true; break;
                    case HeroKind.Tamer: isReceivedRewardTamer = true; break;
                }
            }
        }
    }
    bool ClaimActionClassBadgeTalismanBool(HeroKind heroKind, int num = 1)
    {
        PotionKind tempKind = PotionKind.Nothing;
        switch (heroKind)
        {
            case HeroKind.Warrior: tempKind = PotionKind.WarriorsBadge; break;
            case HeroKind.Wizard: tempKind = PotionKind.WizardsBadge; break;
            case HeroKind.Angel: tempKind = PotionKind.AngelsBadge; break;
            case HeroKind.Thief: tempKind = PotionKind.ThiefsBadge; break;
            case HeroKind.Archer: tempKind = PotionKind.ArchersBadge; break;
            case HeroKind.Tamer: tempKind = PotionKind.TamersBadge; break;
        }
        return ClaimActionTalisman(tempKind, num);
    }
    public bool ClaimActionTalisman(PotionKind kind, int num = 1)
    {
        if (!game.inventoryCtrl.CanCreatePotion(kind, num))
        {
            GameControllerUI.gameUI.logCtrlUI.Log(optStr + "<color=orange>Utility " + localized.Basic(BasicWord.FullInventory), 0, true);
            return false;
            //Inventoryがいっぱいの時は受け取れない
        }
        game.inventoryCtrl.CreatePotion(kind, num);
        return true;
    }
    public bool ClaimActionEnchantScroll(EnchantKind kind, EquipmentEffectKind effectKind, long level)
    {
        if (!game.inventoryCtrl.CanCreateEnchant())
        {
            GameControllerUI.gameUI.logCtrlUI.Log(optStr + "<color=orange>Enchant " + localized.Basic(BasicWord.FullInventory));
            return false;
            //Inventoryがいっぱいの時は受け取れない
        }
        game.inventoryCtrl.CreateEnchant(kind, effectKind, level);
        return true;
    }

    public string[] ClassExclusiveRewardStringClassBadgeTalisman()
    {
        return new string[]
        {
            "Talisman [ " + localized.PotionName(PotionKind.WarriorsBadge) + " ] ",
            "Talisman [ " + localized.PotionName(PotionKind.WizardsBadge) + " ] ",
            "Talisman [ " + localized.PotionName(PotionKind.AngelsBadge) + " ] ",
            "Talisman [ " + localized.PotionName(PotionKind.ThiefsBadge) + " ] ",
            "Talisman [ " + localized.PotionName(PotionKind.ArchersBadge) + " ] ",
            "Talisman [ " + localized.PotionName(PotionKind.TamersBadge) + " ] ",
        };
    }
    public string[] ClassExclusiveRewardStringEquipmentSlot(EquipmentPart part)
    {
        switch (part)
        {
            case EquipmentPart.Weapon:
                return new string[]
                {
                    "Warrior's Equipment Weapon Slot + 1",
                    "Wizard's Equipment Weapon Slot + 1",
                    "Angel's Equipment Weapon Slot + 1",
                    "Thief's Equipment Weapon Slot + 1",
                    "Archer's Equipment Weapon Slot + 1",
                    "Tamer's Equipment Weapon Slot + 1",
                };
            case EquipmentPart.Armor:
                return new string[]
                {
                    "Warrior's Equipment Armor Slot + 1",
                    "Wizard's Equipment Armor Slot + 1",
                    "Angel's Equipment Armor Slot + 1",
                    "Thief's Equipment Armor Slot + 1",
                    "Archer's Equipment Armor Slot + 1",
                    "Tamer's Equipment Armor Slot + 1",
                };
            case EquipmentPart.Jewelry:
                return new string[]
                {
                    "Warrior's Equipment Jewelry Slot + 1",
                    "Wizard's Equipment Jewelry Slot + 1",
                    "Angel's Equipment Jewelry Slot + 1",
                    "Thief's Equipment Jewelry Slot + 1",
                    "Archer's Equipment Jewelry Slot + 1",
                    "Tamer's Equipment Jewelry Slot + 1",
                };
        }
        return new string[] { };
    }
    public void SetRewardEquipmentSlot(EquipmentPart part)
    {
        switch (part)
        {
            case EquipmentPart.Weapon:
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardWarrior));
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardWizard));
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardAngel));
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardThief));
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardArcher));
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardTamer));
                break;
            case EquipmentPart.Armor:
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,() => isReceivedRewardWarrior));
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardWizard));
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardAngel));
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardThief));
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardArcher));
                game.inventoryCtrl.equipArmorUnlockedNum[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardTamer));
                break;
            case EquipmentPart.Jewelry:
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,() => isReceivedRewardWarrior));
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardWizard));
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardAngel));
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1,  () => isReceivedRewardThief));
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardArcher));
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(Challenge, Add, () => 1, () => isReceivedRewardTamer));
                break;
        }
    }

}

public class CHALLENGE_SINGLEBOSS : CHALLENGE
{
    public override ChallengeType type => SingleBossBattle;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public override void StartBattle()
    {
        base.StartBattle();
    }
    public override void ClearAction()//複雑な報酬体型の場合はこれごとoverride
    {
        Initialize();
        isClearedCurrentHero = true;
        if (IsClearedCompleted()) accomplish.RegisterTime();
    }
    public override string TitleUIString()
    {
        return "Solo Boss Battle : " + NameString();
    }
    public override string ClearConditionString()
    {
        string tempStr = optStr + "<size=20><u>" + localized.Basic(BasicWord.ClearCondition) + "</u><size=18>";
        tempStr += "\n- Defeat " + NameString();
        return tempStr;
    }
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
            if(isReceivedRewardComplete) tempStr += "<color=green>";
            else tempStr += "<color=orange>";
        }
        tempStr += "\n- " + CompleteRewardString() + "</color>";
        return tempStr;
    }
}

public class CHALLENGE_RAIDBOSS : CHALLENGE
{
    public override ChallengeType type => RaidBossBattle;
    public override ChallengeMonsterKind challengeMonsterKind => SlimeKing;
    public override void StartBattle()
    {
        base.StartBattle();
        int positionId = 0;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if((HeroKind)count != game.battleCtrl.heroKind && game.battleCtrls[count].isActiveBattle)
            {
                game.battleCtrls[count].isTryingRaid = true;
                game.battleCtrl.HeroAlly((HeroKind)count).Activate(Parameter.heroAllyPositions[positionId]);
                positionId++;
            }
        }
    }

    public override void ClearAction()//複雑な報酬体型の場合はこれごとoverride
    {
        Initialize();
        //isClearedCurrentHero = true;
        //Raidの場合、全員がクリアしたことにする
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            main.SR.isClearedChallenge[i + 10 * (int)kind] = true;
        }
        if (IsClearedOnce()) accomplish.RegisterTime();
    }
    public override string TitleUIString()
    {
        return "Raid Boss Battle : " + NameString();
    }
    public override string ClearConditionString()
    {
        string tempStr = optStr + "<size=20><u>" + localized.Basic(BasicWord.ClearCondition) + "</u><size=18>";
        tempStr += "\n- Defeat " + NameString();
        return tempStr;
    }
    public override string InformationString()
    {
        string tempStr = base.InformationString();
        tempStr += "\n- Participant : " + localized.Hero(game.currentHero);
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            int count = i;
            if ((int)game.currentHero != count && game.battleCtrls[i].isActiveBattle) tempStr += ", " + localized.Hero((HeroKind)count);
        }
        tempStr += "\n<color=yellow>All heroes that are currently active will join the battle!</color>";
        return tempStr;
    }
    public override string RewardInfoString()
    {
        string tempStr = optStr + "<size=20><u>First Clear Reward</u><size=18>";
        if (IsClearedOnce())
        {
            if (isReceivedRewardOnce) tempStr += "<color=green>";
            else tempStr += "<color=orange>";
        }
        tempStr += "\n- " + RewardString();
        //tempStr += "\n- Unleash Solo Boss Battle of " + NameString();
        tempStr += "</color>";
        return tempStr;
    }
}

public enum ChallengeType
{
    RaidBossBattle,
    SingleBossBattle,
    OnslaughtBattle,
    HandicappedBattle,
    TimedBattle,
}
public enum ChallengeKind
{
    SlimeKingRaid100,
    SlimeKingSingle100,
    //SlimeKingRaid200,
    //SlimeKingSingle200,
    WindowQueenRaid150,
    WindowQueenSingle150,
    GolemRaid200,
    GolemSingle200,
    //GolemRaid400,
    //GolemSingle400,
    //GoldenBat250,
    //Bat250,
    //FairyQueen300,
    //Ninetale350,
    //Octobaddie400,
    //Treant450
    //Flametiger500
    //Unicorn550

    //Handicap:HC
    HCArena1,
    //HCMagicSlime,
    //HC
    HCSlimeKing100,
    HCWindowQueen150,
    HCGolem200,

    HCArena2,
    //追加する場合は下に追加。順番は変えてはいけない
}

//組み合わせもあり得る
public enum ChallengeHandicapKind
{
    OnlyWeapon,
    OnlyArmor,
    OnlyJewelry,
    Only1EQforAllPart,//各パート1EQまで
    Only1Weapon,
    Only1Armor,
    Only1Jewelry,
    NoEQ,
    OnlyClassSkill,//GlobalSkillSlot禁止
    OnlyBaseAndGlobal,//BaseSkillとGlobalSlotのみ
    Only2ClassSkillAnd1Global,
    Only2ClassSkill,
    OnlyBaseSkill,
    DamageLimit,//全ての与えるダメージが１になる//未
    DisableManualMove,//ManualMove無効//未
    
}