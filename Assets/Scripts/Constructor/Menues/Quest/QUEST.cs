using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static SpriteSourceUI;
using System;

public partial class Save
{
    //DailyQuest用
    public DailyQuestRarity[] dailyQuestRarities;//[QuestKindDaily]
    public MonsterSpecies[] dailyQuestMonsterSpecies;//[QuestKindDaily]
    public AreaKind[] dailyQuestAreaKind;//[QuestKindDaily]
    public int[] dailyQuestAreaId;//[QuestKindDaily]

    public bool[] isClearedQuestsDaily;
    public bool[] isAcceptedQuestsDaily;
    public double[] initDefeatedNumQuestsDaily;
    public double[] initCompletedAreaNumQuestsDaily;

    //統計
    public double totalGeneralQuestClearedNum;
    public double[] totalGeneralQuestClearedNums;//[GuestKindGeneral]
    public double[] maxReachedGeneralQuestClearedNums;//[GuestKindGeneral]

    //FavoriteQuest
    public bool[] isFavoriteQuestWarrior;//[QuestKindGeneral]
    public bool[] isFavoriteQuestWizard;//[QuestKindGeneral]
    public bool[] isFavoriteQuestAngel;//[QuestKindGeneral]
    public bool[] isFavoriteQuestThief;//[QuestKindGeneral]
    public bool[] isFavoriteQuestArcher;//[QuestKindGeneral]
    public bool[] isFavoriteQuestTamer;//[QuestKindGeneral]

}
public partial class SaveR
{
    //統計（WAでリセット）
    public double totalGeneralQuestClearedNum;
    public double[] totalGeneralQuestClearedNums;//[GuestKindGeneral]
    public double[] generalQuestClearNumsPerClass;//[HeroKind]

    //GeneralQuest用Clear
    public bool[] isClearedQuestGeneralOnce;//[questKindGeneral]

    //Clear
    public bool[] isClearedQuestsGlobal;
    public bool[] isClearedQuestsTitleWarrior;
    public bool[] isClearedQuestsTitleWizard;
    public bool[] isClearedQuestsTitleAngel;
    public bool[] isClearedQuestsTitleThief;
    public bool[] isClearedQuestsTitleArcher;
    public bool[] isClearedQuestsTitleTamer;
    public bool[] isClearedQuestsGeneralWarrior;
    public bool[] isClearedQuestsGeneralWizard;
    public bool[] isClearedQuestsGeneralAngel;
    public bool[] isClearedQuestsGeneralThief;
    public bool[] isClearedQuestsGeneralArcher;
    public bool[] isClearedQuestsGeneralTamer;

    //受諾
    public bool[] isAcceptedQuestsGlobal;
    public bool[] isAcceptedQuestsTitleWarrior;
    public bool[] isAcceptedQuestsTitleWizard;
    public bool[] isAcceptedQuestsTitleAngel;
    public bool[] isAcceptedQuestsTitleThief;
    public bool[] isAcceptedQuestsTitleArcher;
    public bool[] isAcceptedQuestsTitleTamer;
    public bool[] isAcceptedQuestsGeneralWarrior;
    public bool[] isAcceptedQuestsGeneralWizard;
    public bool[] isAcceptedQuestsGeneralAngel;
    public bool[] isAcceptedQuestsGeneralThief;
    public bool[] isAcceptedQuestsGeneralArcher;
    public bool[] isAcceptedQuestsGeneralTamer;

    //Defeat&Capture
    public double[] initDefeatedNumQuestsGlobal;
    public double[] initDefeatedNumQuestsTitleWarrior;
    public double[] initDefeatedNumQuestsTitleWizard;
    public double[] initDefeatedNumQuestsTitleAngel;
    public double[] initDefeatedNumQuestsTitleThief;
    public double[] initDefeatedNumQuestsTitleArcher;
    public double[] initDefeatedNumQuestsTitleTamer;
    public double[] initDefeatedNumQuestsGeneralWarrior;
    public double[] initDefeatedNumQuestsGeneralWizard;
    public double[] initDefeatedNumQuestsGeneralAngel;
    public double[] initDefeatedNumQuestsGeneralThief;
    public double[] initDefeatedNumQuestsGeneralArcher;
    public double[] initDefeatedNumQuestsGeneralTamer;

    //CompleteArea
    //public double[] initCompletedAreaNumQuestsGlobal;
    public double[] initCompletedAreaNumQuestsTitleWarrior;
    public double[] initCompletedAreaNumQuestsTitleWizard;
    public double[] initCompletedAreaNumQuestsTitleAngel;
    public double[] initCompletedAreaNumQuestsTitleThief;
    public double[] initCompletedAreaNumQuestsTitleArcher;
    public double[] initCompletedAreaNumQuestsTitleTamer;
    public double[] initCompletedAreaNumQuestsGeneralWarrior;
    public double[] initCompletedAreaNumQuestsGeneralWizard;
    public double[] initCompletedAreaNumQuestsGeneralAngel;
    public double[] initCompletedAreaNumQuestsGeneralThief;
    public double[] initCompletedAreaNumQuestsGeneralArcher;
    public double[] initCompletedAreaNumQuestsGeneralTamer;

    //Titles
    //Porter:MovedDistance
    public double[] initMovedDistanceQuestTitle;//[heroKind]
    //ElementDamage
    public double[] initPhysicalSkillTriggeredNumQuestTitle;//[heroKind]
    public double[] initFireSkillTriggeredNumQuestTitle;//[heroKind]
    public double[] initIceSkillTriggeredNumQuestTitle;//[heroKind]
    public double[] initThunderSkillTriggeredNumQuestTitle;//[heroKind]
    public double[] initLightSkillTriggeredNumQuestTitle;//[heroKind]
    public double[] initDarkSkillTriggeredNumQuestTitle;//[heroKind]
    //Survival
    public double[] survivalNumQuestTitle;//[heroKind]

}
public partial class Save
{
    //Tutorial:Equip
    public bool isGotFirstEQ;
}

public class BringQuest : QUEST
{
    public BringQuest(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        type = QuestType.Bring;
    }
    public override void ClaimAction()
    {
        foreach (var item in bringRequiredMaterials)
        {
            TargetMaterial(item.Key).Decrease(item.Value);
        }
        foreach (var item in bringRequiredResources)
        {
            TargetResource(item.Key).Decrease(item.Value);
        }
    }
}

public class DefeatQuest : QUEST
{
    public DefeatQuest(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        type = QuestType.Defeat;
    }
    public override void AcceptAction()
    {
        initDefeatedNum = defeatTargetMonsterDefeatNum;
    }
}

public class CompleteAreaQuest : QUEST
{
    public CompleteAreaQuest(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        type = QuestType.AreaComplete;
    }
    public override void AcceptAction()
    {
        initCompletedAreaNum = completeTargetArea.completedNum.TotalCompletedNum();
    }
}

public class CaptureQuest : QUEST
{
    public CaptureQuest(QuestController questCtrl, HeroKind heroKind) : base(questCtrl, heroKind)
    {
        type = QuestType.Capture;
    }
    public override void AcceptAction()
    {
        //CaptureとDefeatを同じSave先にするため、Defeatを使ってる。
        initDefeatedNum = captureTargetMonsterCapturedNum;
    }
}


public class QUEST
{
    public QUEST(QuestController questCtrl, HeroKind heroKind)
    {
        rewardExpMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        rebirthPointGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        this.questCtrl = questCtrl;
        this.heroKind = heroKind;

        //heroLevelFactor = () =>
        //{
        //    if (unlockHeroLevel() >= 1000) return 0;
        //    if (unlockHeroLevel() >= 200) return Parameter.RequiredExp((long)(200d + (unlockHeroLevel() - 200d) * (1 - 0.0005d * unlockHeroLevel())));
        //    return unlockHeroLevel();
        //};
        rewardExp = () =>
        {
            if (kind == QuestKind.Title)
            {
                if (type == QuestType.Bring) return 0;
                return Math.Floor(Parameter.RequiredExp((long)HeroLevelFactor()) / 100d) * 50d;
            }
            if (kind == QuestKind.General)
            {
                if (type == QuestType.Bring) return Math.Floor(Parameter.RequiredExp((long)HeroLevelFactor()) / 100d) * 5d;
                return Math.Floor(Parameter.RequiredExp((long)HeroLevelFactor()) / 100d) * 20d;
            }
            return 0;
        };
        questingArea = game.areaCtrl.nullArea;
        SetQuetingArea();
    }
    double HeroLevelFactor()
    {
        if (unlockHeroLevel() >= 1000) return 0;
        if (unlockHeroLevel() >= 200) return 200d + (unlockHeroLevel() - 200d) * (1 - 0.0005d * unlockHeroLevel());
        return unlockHeroLevel();
    }
    public void Start()
    {
        SetQuetingArea();
        StartQuest();
        SetRewardEffect();
    }
    public virtual void StartQuest() {}//clearConditionやunlockConditionなどを追加
    public bool IsEnoughAcceptCondition()//レベルを満たしているかつアンロックしてある
    {
        return IsEnoughHeroLevel() && IsUnlocked();
    }
    public bool IsUnlocked()
    {
        for (int i = 0; i < unlockConditions.Count; i++)
        {
            if (!unlockConditions[i]()) return false;
        }
        for (int i = 0; i < unlockQuests.Count; i++)
        {
            //GeneralQuestの場合は、一度でもクリアしたことがあれば解禁する（他のHeroでもOK）
            if (kind == QuestKind.General)
            {
                if (!unlockQuests[i].isClearedOnce) return false;
            }
            else
            {
                if (!unlockQuests[i].isCleared) return false;
            }
        }
        return true;
    }
    public bool IsEnoughHeroLevel()
    {
        if (game.statsCtrl.Level(heroKind) >= unlockHeroLevel()) return true;
        if (main.SR.rebirthMaxHeroLevels[(int)heroKind] >= unlockHeroLevel()) return true;//Rebirth前に解禁してれば表示
        return false;
    }
    //Accept
    public bool AcceptOrClaimInteractable()
    {
        if (CanClaim()) return true;
        if (CanAccept()) return true;
        return false;
    }
    public void AcceptOrClaim()
    {
        if (CanClaim())
        {
            Claim();
            return;
        }
        Accept();
    }
    public bool CanAccept()//クエスト受諾数に空きがある
    {
        if (isCleared) return false;
        if (isAccepted) return false;
        //if (isFavorite) return true;
        return IsEnoughAcceptCondition() && questCtrl.CanAccept(this, heroKind);
    }
    public bool CanFavorite()
    {
        if (isFavorite) return true;
        if (kind != QuestKind.General) return false;
        if (!game.epicStoreCtrl.Item(EpicStoreKind.FavoriteQuest).IsPurchased()) return false;
        //if (isAccepted) return true;
        return IsEnoughAcceptCondition() && questCtrl.CanFavorite(this, heroKind);
    }
    public void Accept()
    {
        if (!CanAccept()) return;
        isAccepted = true;
        AcceptAction();
    }
    public void AssignRemoveFavorite()
    {
        if (isFavorite)
        {
            isFavorite = false;
            return;
        }
        if (!CanFavorite()) return;
        isFavorite = true;
    }
        
    public bool CanCancel()
    {
        if (isCleared) return false;
        if (isFavorite) return false;
        return isAccepted;
    }
    public void Cancel()
    {
        if (!CanCancel()) return;
        isAccepted = false;
    }
    //Claim
    public bool CanClaim()
    {
        if (!isAccepted) return false;
        if (isCleared) return false;
        for (int i = 0; i < clearConditions.Count; i++)
        {
            if (!clearConditions[i]()) return false;
        }
        if (type == QuestType.Bring)
        {
            foreach (var item in bringRequiredMaterials)
            {
                if (TargetMaterial(item.Key).value < item.Value) return false;
            }
            foreach (var item in bringRequiredResources)
            {
                if (TargetResource(item.Key).value < item.Value) return false;
            }
        }
        if (type == QuestType.Defeat)
        {
            if (DefeatTargetMonsterDefeatedNum() < defeatRequredDefeatNum()) return false;
        }
        if (type == QuestType.Capture)
        {
            if (CaptureTargetMonsterCapturedNum() < captureRequiredNum()) return false;
        }
        if (type == QuestType.AreaComplete)
        {
            if (AreaCompletedNum() < areaRequredCompletedNum()) return false;
        }
        return true;
    }
    public void Claim()
    {
        if (!CanClaim()) return;
        isCleared = true;
        if (kind == QuestKind.General)
        {
            isClearedOnce = true;
            double clearNum = questCtrl.generalQuestClearGain[(int)heroKind].Value();
            main.S.totalGeneralQuestClearedNum += clearNum;
            main.SR.totalGeneralQuestClearedNum += clearNum;
            main.SR.generalQuestClearNumsPerClass[(int)heroKind] += clearNum;
            totalClearedNumGeneral += clearNum;
            totalClearedNumGeneralThisAscension += clearNum;
            maxReachedClearedNumGeneral = Math.Max(maxReachedClearedNumGeneral, totalClearedNumGeneralThisAscension);
        }
        isAccepted = false;
        ClaimAction();
        GetReward();
    }
    public void GetReward()
    {
        if (RewardExp() > 0) game.statsCtrl.Exp(heroKind).Increase(RewardExp(), false);
        if (rewardGuildExp() > 0) game.guildCtrl.exp.Increase(rewardGuildExp());
        if (rewardGold() > 0) game.resourceCtrl.gold.Increase(rewardGold());
        if (rewardNitro() > 0)
        {
            game.nitroCtrl.nitroCap.Calculate();
            game.nitroCtrl.nitro.IncreaseWithoutLimit(rewardNitro());
        }
        if (rewardEC() > 0) game.epicStoreCtrl.epicCoin.Increase(rewardEC());
        if (rewardPortalOrb() > 0) game.areaCtrl.portalOrb.Increase(rewardPortalOrb());
        foreach (var item in rewardMaterial)
        {
            game.materialCtrl.Material(item.Key()).Increase(item.Value());
        }
        foreach (var item in rewardMaterialNumber)
        {
            item.Key().Increase(item.Value());
        }
        foreach (var item in rewardPotion)
        {
            game.inventoryCtrl.CreatePotion(item.Key(), item.Value());
        }
        RewardAction();
        if (rewardUIAction != null) rewardUIAction();
    }

    private QuestController questCtrl;
    public QuestKind kind;
    public QuestType type;
    public HeroKind heroKind;
    public QuestKindGlobal kindGlobal;
    public QuestKindTitle kindTitle;
    public QuestKindGeneral kindGeneral;
    public QuestKindDaily kindDaily;
    public DailyQuestRarity dailyQuestRarity { get => main.S.dailyQuestRarities[(int)kindDaily]; set => main.S.dailyQuestRarities[(int)kindDaily] = value; }
    public GlobalQuestType globalQuestType;//Sort用
    public double totalClearedNumGeneral {
        get
        {
            if (kind != QuestKind.General) return 0;
            return main.S.totalGeneralQuestClearedNums[(int)kindGeneral];
        }
        set => main.S.totalGeneralQuestClearedNums[(int)kindGeneral] = value; }
    public double totalClearedNumGeneralThisAscension {
        get
        {
            if (kind != QuestKind.General) return 0;
            return main.SR.totalGeneralQuestClearedNums[(int)kindGeneral];
        }
        set => main.SR.totalGeneralQuestClearedNums[(int)kindGeneral] = value; }
    public double maxReachedClearedNumGeneral
    {
        get
        {
            if (kind != QuestKind.General) return 0;
            return main.S.maxReachedGeneralQuestClearedNums[(int)kindGeneral];
        }
        set => main.S.maxReachedGeneralQuestClearedNums[(int)kindGeneral] = value;
    }
    public bool isClearedOnce { get { if (kind == QuestKind.General) return main.SR.isClearedQuestGeneralOnce[(int)kindGeneral]; return true; } set => main.SR.isClearedQuestGeneralOnce[(int)kindGeneral] = value; }
    public bool isCleared
    {
        get
        {
            switch (kind)
            {
                case QuestKind.Global:
                    return main.SR.isClearedQuestsGlobal[(int)kindGlobal];
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.isClearedQuestsTitleWarrior[(int)kindTitle];
                        case HeroKind.Wizard:
                            return main.SR.isClearedQuestsTitleWizard[(int)kindTitle];
                        case HeroKind.Angel:
                            return main.SR.isClearedQuestsTitleAngel[(int)kindTitle];
                        case HeroKind.Thief:
                            return main.SR.isClearedQuestsTitleThief[(int)kindTitle];
                        case HeroKind.Archer:
                            return main.SR.isClearedQuestsTitleArcher[(int)kindTitle];
                        case HeroKind.Tamer:
                            return main.SR.isClearedQuestsTitleTamer[(int)kindTitle];
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.isClearedQuestsGeneralWarrior[(int)kindGeneral];
                        case HeroKind.Wizard:
                            return main.SR.isClearedQuestsGeneralWizard[(int)kindGeneral];
                        case HeroKind.Angel:
                            return main.SR.isClearedQuestsGeneralAngel[(int)kindGeneral];
                        case HeroKind.Thief:
                            return main.SR.isClearedQuestsGeneralThief[(int)kindGeneral];
                        case HeroKind.Archer:
                            return main.SR.isClearedQuestsGeneralArcher[(int)kindGeneral];
                        case HeroKind.Tamer:
                            return main.SR.isClearedQuestsGeneralTamer[(int)kindGeneral];
                    }
                    break;
                case QuestKind.Daily:
                    return main.S.isClearedQuestsDaily[(int)kindDaily];
            }
            return false;
        }
        set
        {
            switch (kind)
            {
                case QuestKind.Global:
                    main.SR.isClearedQuestsGlobal[(int)kindGlobal] = value;
                    break;
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.isClearedQuestsTitleWarrior[(int)kindTitle] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.isClearedQuestsTitleWizard[(int)kindTitle] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.isClearedQuestsTitleAngel[(int)kindTitle] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.isClearedQuestsTitleThief[(int)kindTitle] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.isClearedQuestsTitleArcher[(int)kindTitle] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.isClearedQuestsTitleTamer[(int)kindTitle] = value;
                            break;
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.isClearedQuestsGeneralWarrior[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.isClearedQuestsGeneralWizard[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.isClearedQuestsGeneralAngel[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.isClearedQuestsGeneralThief[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.isClearedQuestsGeneralArcher[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.isClearedQuestsGeneralTamer[(int)kindGeneral] = value;
                            break;
                    }
                    break;
                case QuestKind.Daily:
                    main.S.isClearedQuestsDaily[(int)kindDaily] = value;
                    break;
            }
        }
    }
    public bool isAccepted
    {
        get
        {
            switch (kind)
            {
                case QuestKind.Global:
                    return main.SR.isAcceptedQuestsGlobal[(int)kindGlobal];
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.isAcceptedQuestsTitleWarrior[(int)kindTitle];
                        case HeroKind.Wizard:
                            return main.SR.isAcceptedQuestsTitleWizard[(int)kindTitle];
                        case HeroKind.Angel:
                            return main.SR.isAcceptedQuestsTitleAngel[(int)kindTitle];
                        case HeroKind.Thief:
                            return main.SR.isAcceptedQuestsTitleThief[(int)kindTitle];
                        case HeroKind.Archer:
                            return main.SR.isAcceptedQuestsTitleArcher[(int)kindTitle];
                        case HeroKind.Tamer:
                            return main.SR.isAcceptedQuestsTitleTamer[(int)kindTitle];
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.isAcceptedQuestsGeneralWarrior[(int)kindGeneral];
                        case HeroKind.Wizard:
                            return main.SR.isAcceptedQuestsGeneralWizard[(int)kindGeneral];
                        case HeroKind.Angel:
                            return main.SR.isAcceptedQuestsGeneralAngel[(int)kindGeneral];
                        case HeroKind.Thief:
                            return main.SR.isAcceptedQuestsGeneralThief[(int)kindGeneral];
                        case HeroKind.Archer:
                            return main.SR.isAcceptedQuestsGeneralArcher[(int)kindGeneral];
                        case HeroKind.Tamer:
                            return main.SR.isAcceptedQuestsGeneralTamer[(int)kindGeneral];
                    }
                    break;
                case QuestKind.Daily:
                    return main.S.isAcceptedQuestsDaily[(int)kindDaily];
            }
            return false;
        }
        set
        {
            switch (kind)
            {
                case QuestKind.Global:
                    main.SR.isAcceptedQuestsGlobal[(int)kindGlobal] = value;
                    break;
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.isAcceptedQuestsTitleWarrior[(int)kindTitle] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.isAcceptedQuestsTitleWizard[(int)kindTitle] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.isAcceptedQuestsTitleAngel[(int)kindTitle] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.isAcceptedQuestsTitleThief[(int)kindTitle] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.isAcceptedQuestsTitleArcher[(int)kindTitle] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.isAcceptedQuestsTitleTamer[(int)kindTitle] = value;
                            break;
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.isAcceptedQuestsGeneralWarrior[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.isAcceptedQuestsGeneralWizard[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.isAcceptedQuestsGeneralAngel[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.isAcceptedQuestsGeneralThief[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.isAcceptedQuestsGeneralArcher[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.isAcceptedQuestsGeneralTamer[(int)kindGeneral] = value;
                            break;
                    }
                    break;
                case QuestKind.Daily:
                    main.S.isAcceptedQuestsDaily[(int)kindDaily] = value;
                    break;
            }
        }
    }

    //Mastery
    public List<QuestMastery> masteryList = new List<QuestMastery>();
    public void SetMastery()
    {
        if (kind != QuestKind.General) return;
        masteryList.Add(new QuestMastery(this, "E", 100, "Reward EXP + 50%", (x) => rewardExpMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.50d, x))));
        masteryList.Add(new QuestMastery(this, "D", 1000, "Reward EXP + 50%", (x) => rewardExpMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.50d, x))));
        masteryList.Add(new QuestMastery(this, "C", 10000, "Rebirth Point Gain from this quest + 50%", (x) => rebirthPointGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.50d, x))));
        masteryList.Add(new QuestMastery(this, "B", 100000, "Rebirth Point Gain from this quest + 50%", (x) => rebirthPointGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Quest, MultiplierType.Add, () => 0.50d, x))));
        masteryList.Add(new QuestMastery(this, "A", 1000000, "Halves accept slot use"));
        masteryList.Add(new QuestMastery(this, "S", 10000000, "Eliminates accept slot use"));
    }
    public Multiplier rewardExpMultiplier;
    public Multiplier rebirthPointGainMultiplier;
    public double rebirthPointGain => (30d + unlockHeroLevel() / 5d) * rebirthPointGainMultiplier.Value();
    public double acceptNumModifier
    {
        get 
        {
            if (kind != QuestKind.General) return 1;
            if (masteryList[5].isReached) return 0;
            if (masteryList[4].isReached) return 0.5d;
            return 1;
        }
    }
    public string MasteryRankString()
    {
        string tempStr = "";
        for (int i = 0; i < masteryList.Count; i++)
        {
            if (!masteryList[i].isReached) return tempStr;
            tempStr = masteryList[i].masteryRankString;
        }
        return tempStr;
    }

    //Unlock
    public List<Func<bool>> unlockConditions = new List<Func<bool>>();
    public Func<long> unlockHeroLevel = () => 0;
    public List<QUEST> unlockQuests = new List<QUEST>();//前提条件
    public List<Func<bool>> clearConditions = new List<Func<bool>>();

    public virtual void AcceptAction() { }//Defeat数の記録など
    public virtual void ClaimAction() { }//素材の回収など
    public virtual void RewardAction() { }//Exp,Gold,EC以外の報酬（素材など）
    public virtual void SetRewardEffect() { }//基本的にはGlobalとTitleで使う
    public Action rewardUIAction;
    public Func<double> rewardExp = () => 0;
    public double RewardExp() { return rewardExp() * rewardExpMultiplier.Value() * LevelFactor(); }
    double LevelFactor()
    {
        if (unlockHeroLevel() <= 100)//1 + 1/25 x - 1/2500 x^2 (上に凸の二次関数）
            return 1d + unlockHeroLevel() / 25d - unlockHeroLevel() * unlockHeroLevel() / 2500d;
        return 1;
    }
    public Func<double> rewardGuildExp = () => 0;
    public Func<double> rewardGold = () => 0;
    public Func<double> rewardNitro = () => 0;
    public Func<long> rewardEC = () => 0;
    public Dictionary<Func<PotionKind>, Func<long>> rewardPotion = new Dictionary<Func<PotionKind>, Func<long>>();
    public Dictionary<Func<MaterialKind>, Func<long>> rewardMaterial = new Dictionary<Func<MaterialKind>, Func<long>>();
    public Dictionary<Func<NUMBER>, Func<long>> rewardMaterialNumber = new Dictionary<Func<NUMBER>, Func<long>>();
    public TitleKind rewardTitleKind;
    public long rewardTitleLevel;
    public Func<double> rewardPortalOrb = () => 0;

    //Bring
    public Dictionary<MaterialKind, double> bringRequiredMaterials = new Dictionary<MaterialKind, double>();
    public Dictionary<ResourceKind, double> bringRequiredResources = new Dictionary<ResourceKind, double>();
    public NUMBER TargetMaterial(MaterialKind kind)
    {
        return game.materialCtrl.Material(kind);
    }
    public NUMBER TargetResource(ResourceKind kind)
    {
        return game.resourceCtrl.Resource(kind);
    }

    //Capture
    public MonsterGlobalInformation captureTargetMonster;
    public Func<double> captureRequiredNum = () => 0;
    public double CaptureTargetMonsterCapturedNum() { if (!isAccepted || isCleared) return 0; return Math.Max(0, captureTargetMonsterCapturedNum - initCapturedNum); }
    public double captureTargetMonsterCapturedNum
    {
        get
        {
            if (kind == QuestKind.Daily) return game.monsterCtrl.CapturedNum(dailyTargetMonsterSpecies);
            return captureTargetMonster.capturedNums[(int)heroKind].value;
        }
    }
    public double initCapturedNum => initDefeatedNum;

    //Defeat
    public MonsterGlobalInformation defeatTargetMonster;
    public MonsterSpecies dailyTargetMonsterSpecies { get => main.S.dailyQuestMonsterSpecies[(int)kindDaily]; set => main.S.dailyQuestMonsterSpecies[(int)kindDaily] = value; }
    public Func<double> defeatRequredDefeatNum = () => 0;
    public double DefeatTargetMonsterDefeatedNum() { if (!isAccepted || isCleared) return 0; return Math.Max(0, defeatTargetMonsterDefeatNum - initDefeatedNum); }
    public double defeatTargetMonsterDefeatNum
    {
        get
        {
            if (kind == QuestKind.Daily) return game.monsterCtrl.TotalDefeatedNum(dailyTargetMonsterSpecies);
            if (kind == QuestKind.Global) return defeatTargetMonster.DefeatedNum();
            return defeatTargetMonster.defeatedNums[(int)heroKind].value;
        }
    }
    public double initDefeatedNum
    {
        get
        {
            switch (kind)
            {
                case QuestKind.Global:
                    return main.SR.initDefeatedNumQuestsGlobal[(int)kindGlobal];
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.initDefeatedNumQuestsTitleWarrior[(int)kindTitle];
                        case HeroKind.Wizard:
                            return main.SR.initDefeatedNumQuestsTitleWizard[(int)kindTitle];
                        case HeroKind.Angel:
                            return main.SR.initDefeatedNumQuestsTitleAngel[(int)kindTitle];
                        case HeroKind.Thief:
                            return main.SR.initDefeatedNumQuestsTitleThief[(int)kindTitle];
                        case HeroKind.Archer:
                            return main.SR.initDefeatedNumQuestsTitleArcher[(int)kindTitle];
                        case HeroKind.Tamer:
                            return main.SR.initDefeatedNumQuestsTitleTamer[(int)kindTitle];
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.initDefeatedNumQuestsGeneralWarrior[(int)kindGeneral];
                        case HeroKind.Wizard:
                            return main.SR.initDefeatedNumQuestsGeneralWizard[(int)kindGeneral];
                        case HeroKind.Angel:
                            return main.SR.initDefeatedNumQuestsGeneralAngel[(int)kindGeneral];
                        case HeroKind.Thief:
                            return main.SR.initDefeatedNumQuestsGeneralThief[(int)kindGeneral];
                        case HeroKind.Archer:
                            return main.SR.initDefeatedNumQuestsGeneralArcher[(int)kindGeneral];
                        case HeroKind.Tamer:
                            return main.SR.initDefeatedNumQuestsGeneralTamer[(int)kindGeneral];
                    }
                    break;
                case QuestKind.Daily:
                    return main.S.initDefeatedNumQuestsDaily[(int)kindDaily];
            }
            return 0;
        }
        set
        {
            switch (kind)
            {
                case QuestKind.Global:
                    main.SR.initDefeatedNumQuestsGlobal[(int)kindGlobal] = value;
                    break;
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.initDefeatedNumQuestsTitleWarrior[(int)kindTitle] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.initDefeatedNumQuestsTitleWizard[(int)kindTitle] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.initDefeatedNumQuestsTitleAngel[(int)kindTitle] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.initDefeatedNumQuestsTitleThief[(int)kindTitle] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.initDefeatedNumQuestsTitleArcher[(int)kindTitle] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.initDefeatedNumQuestsTitleTamer[(int)kindTitle] = value;
                            break;
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.initDefeatedNumQuestsGeneralWarrior[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.initDefeatedNumQuestsGeneralWizard[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.initDefeatedNumQuestsGeneralAngel[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.initDefeatedNumQuestsGeneralThief[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.initDefeatedNumQuestsGeneralArcher[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.initDefeatedNumQuestsGeneralTamer[(int)kindGeneral] = value;
                            break;
                    }
                    break;
                case QuestKind.Daily:
                    main.S.initDefeatedNumQuestsDaily[(int)kindDaily] = value;
                    break;
            }
        }
    }

    //CompleteArea
    public AREA completeTargetArea;
    public AreaKind dailyQuestAreaKind { get => main.S.dailyQuestAreaKind[(int)kindDaily]; set => main.S.dailyQuestAreaKind[(int)kindDaily] = value; }
    public int dailyQuestAreaId { get => main.S.dailyQuestAreaId[(int)kindDaily]; set => main.S.dailyQuestAreaId[(int)kindDaily] = value; }
    public Func<double> areaRequredCompletedNum = () => 0;
    public double AreaCompletedNum() { if (!isAccepted || isCleared) return 0; return Math.Max(0, areaCompletedNum - initCompletedAreaNum); }
    public double areaCompletedNum { get => completeTargetArea.completedNum.TotalCompletedNum(); }
    public double initCompletedAreaNum
    {
        get
        {
            switch (kind)
            {
                case QuestKind.Global:
                    return 0;// main.SR.initCompletedAreaNumQuestsGlobal[(int)kindGlobal];
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.initCompletedAreaNumQuestsTitleWarrior[(int)kindTitle];
                        case HeroKind.Wizard:
                            return main.SR.initCompletedAreaNumQuestsTitleWizard[(int)kindTitle];
                        case HeroKind.Angel:
                            return main.SR.initCompletedAreaNumQuestsTitleAngel[(int)kindTitle];
                        case HeroKind.Thief:
                            return main.SR.initCompletedAreaNumQuestsTitleThief[(int)kindTitle];
                        case HeroKind.Archer:
                            return main.SR.initCompletedAreaNumQuestsTitleArcher[(int)kindTitle];
                        case HeroKind.Tamer:
                            return main.SR.initCompletedAreaNumQuestsTitleTamer[(int)kindTitle];
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            return main.SR.initCompletedAreaNumQuestsGeneralWarrior[(int)kindGeneral];
                        case HeroKind.Wizard:
                            return main.SR.initCompletedAreaNumQuestsGeneralWizard[(int)kindGeneral];
                        case HeroKind.Angel:
                            return main.SR.initCompletedAreaNumQuestsGeneralAngel[(int)kindGeneral];
                        case HeroKind.Thief:
                            return main.SR.initCompletedAreaNumQuestsGeneralThief[(int)kindGeneral];
                        case HeroKind.Archer:
                            return main.SR.initCompletedAreaNumQuestsGeneralArcher[(int)kindGeneral];
                        case HeroKind.Tamer:
                            return main.SR.initCompletedAreaNumQuestsGeneralTamer[(int)kindGeneral];
                    }
                    break;
                case QuestKind.Daily:
                    return main.S.initCompletedAreaNumQuestsDaily[(int)kindDaily];
            }
            return 0;
        }
        set
        {
            switch (kind)
            {
                case QuestKind.Global:
                    //main.SR.initCompletedAreaNumQuestsGlobal[(int)kindGlobal] = value;
                    break;
                case QuestKind.Title:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.initCompletedAreaNumQuestsTitleWarrior[(int)kindTitle] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.initCompletedAreaNumQuestsTitleWizard[(int)kindTitle] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.initCompletedAreaNumQuestsTitleAngel[(int)kindTitle] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.initCompletedAreaNumQuestsTitleThief[(int)kindTitle] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.initCompletedAreaNumQuestsTitleArcher[(int)kindTitle] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.initCompletedAreaNumQuestsTitleTamer[(int)kindTitle] = value;
                            break;
                    }
                    break;
                case QuestKind.General:
                    switch (heroKind)
                    {
                        case HeroKind.Warrior:
                            main.SR.initCompletedAreaNumQuestsGeneralWarrior[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Wizard:
                            main.SR.initCompletedAreaNumQuestsGeneralWizard[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Angel:
                            main.SR.initCompletedAreaNumQuestsGeneralAngel[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Thief:
                            main.SR.initCompletedAreaNumQuestsGeneralThief[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Archer:
                            main.SR.initCompletedAreaNumQuestsGeneralArcher[(int)kindGeneral] = value;
                            break;
                        case HeroKind.Tamer:
                            main.SR.initCompletedAreaNumQuestsGeneralTamer[(int)kindGeneral] = value;
                            break;
                    }
                    break;
                case QuestKind.Daily:
                    main.S.initCompletedAreaNumQuestsDaily[(int)kindDaily] = value;
                    break;
            }
        }
    }

    //Porter(Title)
    public double porterRequiredMovedDistance;
    public virtual double movedDistance { get; }
    //ElementAttack
    public double elementTriggeredRequiredNum;
    public virtual double elementTriggeredNum { get; }
    //Quester
    public double questerRequiredClearNum;
    public virtual double questerClearNum { get; }

    //Questing Area
    public AREA questingArea;
    public void SetQuetingArea()//DailyQuestの時だけリセットのたびに呼ぶ
    {
        MonsterSpecies species;
        MonsterColor color;
        switch (type)
        {
            case QuestType.Other:
                break;
            case QuestType.Defeat:
                species = kind == QuestKind.Daily ? dailyTargetMonsterSpecies : defeatTargetMonster.species;
                color = kind == QuestKind.Daily ? MonsterColor.Normal : defeatTargetMonster.color;
                for (int i = 0; i < game.areaCtrl.areaList.Count; i++)
                {
                    AREA tempArea = game.areaCtrl.areaList[i];
                    if (!tempArea.isDungeon && !tempArea.isChallenge && tempArea.isSpawnMonsters[(int)species][(int)color])
                    {
                        questingArea = tempArea;
                        break;
                    }
                }
                break;
            case QuestType.Bring:
                break;
            case QuestType.AreaComplete:
                questingArea = completeTargetArea;
                break;
            case QuestType.Capture:
                species = kind == QuestKind.Daily ? dailyTargetMonsterSpecies : captureTargetMonster.species;
                color = kind == QuestKind.Daily ? MonsterColor.Normal : captureTargetMonster.color;
                for (int i = 0; i < game.areaCtrl.areaList.Count; i++)
                {
                    AREA tempArea = game.areaCtrl.areaList[i];
                    if (!tempArea.isDungeon && !tempArea.isChallenge && tempArea.isSpawnMonsters[(int)species][(int)color])
                    {
                        questingArea = tempArea;
                        break;
                    }
                }
                break;
        }
    }

    //DailyQuest
    public void ResetDailyQuest()
    {
        if (kind != QuestKind.Daily) return;
        if (!isCleared && isAccepted) return;//Accept中のものはリセットしない
        isCleared = false;
        isAccepted = false;
        initDefeatedNum = 0;
        initCompletedAreaNum = 0;
        //追加
        Lottery lottery = new Lottery(questCtrl.DailyQuestRarityChance());
        dailyQuestRarity = (DailyQuestRarity)lottery.SelectedId();
        //Species
        dailyTargetMonsterSpecies = (MonsterSpecies)game.areaCtrl.RandomAreaKind();
        dailyQuestAreaKind = game.areaCtrl.RandomAreaKind();
        dailyQuestAreaId = game.areaCtrl.RandomAreaId(dailyQuestAreaKind);
        completeTargetArea = game.areaCtrl.Area(dailyQuestAreaKind, dailyQuestAreaId);

        SetQuetingArea();
    }

    public bool isFavorite//FavoriteQuestとして割り当てられたかどうか
    {
        get
        {
            if (kind == QuestKind.General)
            {
                switch (heroKind)
                {
                    case HeroKind.Warrior:
                        return main.S.isFavoriteQuestWarrior[(int)kindGeneral];
                    case HeroKind.Wizard:
                        return main.S.isFavoriteQuestWizard[(int)kindGeneral];
                    case HeroKind.Angel:
                        return main.S.isFavoriteQuestAngel[(int)kindGeneral];
                    case HeroKind.Thief:
                        return main.S.isFavoriteQuestThief[(int)kindGeneral];
                    case HeroKind.Archer:
                        return main.S.isFavoriteQuestArcher[(int)kindGeneral];
                    case HeroKind.Tamer:
                        return main.S.isFavoriteQuestTamer[(int)kindGeneral];
                }
            }
            return false;
        }
        set
        {
            if (kind == QuestKind.General)
            {
                switch (heroKind)
                {
                    case HeroKind.Warrior: main.S.isFavoriteQuestWarrior[(int)kindGeneral] = value; break;
                    case HeroKind.Wizard: main.S.isFavoriteQuestWizard[(int)kindGeneral] = value; break;
                    case HeroKind.Angel: main.S.isFavoriteQuestAngel[(int)kindGeneral] = value; break;
                    case HeroKind.Thief: main.S.isFavoriteQuestThief[(int)kindGeneral] = value; break;
                    case HeroKind.Archer: main.S.isFavoriteQuestArcher[(int)kindGeneral] = value; break;
                    case HeroKind.Tamer: main.S.isFavoriteQuestTamer[(int)kindGeneral] = value; break;
                }
            }
        }
    }

    //UI
    public (string name, string client, string description, string condition, string reward, string unlock) String()
    {
        if (kind != QuestKind.General || !masteryList[0].isReached) return Localized.localized.Quest(this);
        return ("[" + MasteryRankString() + "] " + Localized.localized.Quest(this).name, Localized.localized.Quest(this).client, Localized.localized.Quest(this).description, Localized.localized.Quest(this).condition, Localized.localized.Quest(this).reward, Localized.localized.Quest(this).unlock);
    }
}

public class QuestMastery
{
    public QuestMastery(QUEST quest, string masteryRankString, double requiredNum, string rewardString, Action<Func<bool>> registerInfo = null)
    {
        this.quest = quest;
        this.masteryRankString = masteryRankString;
        this.requiredNum = requiredNum;
        this.rewardString = rewardString;
        if (registerInfo != null) registerInfo(() => isReached);
    }
    public QUEST quest;
    double clearNum => quest.maxReachedClearedNumGeneral;
    double requiredNum;
    public string masteryRankString;
    string rewardString;
    public bool isReached => clearNum >= requiredNum;
    public string String()
    {
        string tempStr = "";
        if (isReached) tempStr += "<color=green>";
        tempStr += optStr + masteryRankString + " (" + requiredNum.ToString() + ") : " + rewardString + "</color>";
        return tempStr;
    }
}

public enum QuestKind
{
    Global,
    Daily,
    Title,
    General,
}
public enum QuestType
{
    Other,
    Defeat,
    Bring,
    AreaComplete,
    Capture,
}
public enum QuestKindDaily
{
    EC1,
    EC2,
    EC3,
    EC4,
    Cartographer1,
    Cartographer2,
    Cartographer3,
    Cartographer4,
    Cartographer5,

    //Bring,
    //AreaComplete,
    //Capture,
}
public enum DailyQuestRarity
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    Epic
}
public enum QuestKindGlobal
{
    AbilityVIT,
    ClearGeneralQuest,
    ClearTitleQuest,
    UpgradeResource,
    Equip,
    Alchemy,
    Guild,
    Town,
    Research,
    Rebirth,
    Challenge,
    Expedition,
    WorldAscension,
    AreaPrestige,

    Upgrade1,
    Upgrade2,
    Upgrade3,
    Upgrade4,
    Upgrade5,
    Upgrade6,
    Upgrade7,
    Upgrade8,

    Nitro1,
    Nitro2,
    Nitro3,
    Nitro4,
    Nitro5,
    Nitro6,
    Nitro7,
    Nitro8,

    Capture1,
    Capture2,
    Capture3,
    Capture4,
    Capture5,

    Alchemy1,
    Alchemy2,
    Alchemy3,
    Alchemy4,
    Alchemy5,
    Alchemy6,
    Alchemy7,
    Alchemy8,

    //Enumの順番は変えない！追加するときは一番下に追加。
    //リストへの登録は表示順番通りにする。
}
public enum GlobalQuestType//Sort用
{
    Tutorial,
    Upgrade,
    Nitro,
    Capture,
    Alchemy,
    //Explore,//AreaやDungeonの開拓
}
public enum QuestKindTitle
{
    SkillMaster1,
    SkillMaster2,
    SkillMaster3,
    SkillMaster4,

    MonsterDistinguisher1,
    MonsterDistinguisher2,
    MonsterDistinguisher3,
    MonsterDistinguisher4,
    MonsterDistinguisher5,
    MonsterDistinguisher6,
    //MonsterDistinguisher7,

    EquipmentSlotWeapon1,
    EquipmentSlotWeapon2,
    EquipmentSlotWeapon3,
    EquipmentSlotWeapon4,
    EquipmentSlotWeapon5,
    EquipmentSlotWeapon6,

    EquipmentSlotArmor1,
    EquipmentSlotArmor2,
    EquipmentSlotArmor3,
    EquipmentSlotArmor4,
    EquipmentSlotArmor5,
    EquipmentSlotArmor6,

    EquipmentSlotJewelry1,
    EquipmentSlotJewelry2,
    EquipmentSlotJewelry3,
    EquipmentSlotJewelry4,
    EquipmentSlotJewelry5,
    EquipmentSlotJewelry6,

    PotionSlot1,
    PotionSlot2,
    PotionSlot3,

    PhysicalAttack1,
    PhysicalAttack2,
    PhysicalAttack3,
    PhysicalAttack4,
    FireAttack1,
    FireAttack2,
    FireAttack3,
    FireAttack4,
    IceAttack1,
    IceAttack2,
    IceAttack3,
    IceAttack4,
    ThunderAttack1,
    ThunderAttack2,
    ThunderAttack3,
    ThunderAttack4,
    LightAttack1,
    LightAttack2,
    LightAttack3,
    LightAttack4,
    DarkAttack1,
    DarkAttack2,
    DarkAttack3,
    DarkAttack4,

    Porter1,
    Porter2,
    Porter3,
    Porter4,
    Porter5,
    Porter6,
    Alchemist1,
    Alchemist2,
    Alchemist3,
    Alchemist4,
    Alchemist5,

    //ver0.0.4.0以後追加
    EquipmentProf1,//5
    EquipmentProf2,//10
    EquipmentProf3,//15
    EquipmentProf4,//20
    EquipmentProf5,//30

    MetalHunter1,//metal slime
    MetalHunter2,//metal slime
    MetalHunter3,//metal magic slime
    MetalHunter4,//metal spider
    //Localizedにかく、questのオブジェクト追加、controllerに記録

    FireResistance1,
    FireResistance2,
    FireResistance3,
    FireResistance4,
    FireResistance5,
    IceResistance1,
    IceResistance2,
    IceResistance3,
    IceResistance4,
    IceResistance5,
    ThunderResistance1,
    ThunderResistance2,
    ThunderResistance3,
    ThunderResistance4,
    ThunderResistance5,
    LightResistance1,
    LightResistance2,
    LightResistance3,
    LightResistance4,
    LightResistance5,
    DarkResistance1,
    DarkResistance2,
    DarkResistance3,
    DarkResistance4,
    DarkResistance5,

    Survival1,
    Survival2,
    Survival3,
    Survival4,

    Cooperation1,// Rebirth Tier 1
    Cooperation2,// Rebirth Tier 2
    Cooperation3,// Rebirth Tier 3

    //ver0.2.2.3以降追加
    Quester1,
    Quester2,
    Quester3,
    Quester4,
    Quester5,
    Quester6,
    Quester7,
    Quester8,
    Quester9,
    Quester10,

    //Cooperation4,//Rebirth Tier 4
    //Cooperation5,//Rebirth Tier 5

}
public enum QuestKindGeneral
{
    CompleteArea0_0,
    CompleteArea0_1,
    CompleteArea0_2,
    CompleteArea0_3,
    DefeatNormalSlime1,
    DefeatNormalSlime2,
    DefeatNormalSlime3,
    BringOilOfSlime,
    DefeatRedSlime,
    DefeatNormalMagicSlime,
    DefeatRedMagicSlime,
    DefeatGreenMagicSlime,
    CompleteDungeon0_0,
    CompleteDungeon0_1,
    CompleteDungeon0_2,

    DefeatYellowBat,
    DefeatRedBat,
    DefeatGreenBat,
    DefeatPurpleBat,
    DefeatBlueBat,
    BringToEnchantShard,

    CompleteDungeon2_0,
    CompleteDungeon2_1,
    CaptureNormalSpider,
    CompleteDungeon2_2,
    //BringEnchantedCloth,
    CaptureYellowSlime,

    CaptureNormalFairy,//BlueTrap獲得
    CaptureBlueFairy,//YellowTrap獲得
    CaptureYellowFairy,//RedTrap獲得
    CaptureRedFairy,//GreenTrap獲得
    CaptureGreenFairy,//PurpleTrap獲得
}

public enum TitleKind
{
    SkillMaster,//SlotとProf
    MonsterDistinguisher,//tipsとcapture
    EquipmentSlotWeapon,
    EquipmentSlotArmor,
    EquipmentSlotJewelry,
    PotionSlot,
    EquipmentProficiency,
    //SkillMaster,//SlotとProf（もともとここにあった）
    PhysicalDamage,
    FireDamage,
    IceDamage,
    ThunderDamage,
    LightDamage,
    DarkDamage,
    Alchemist,

    //ver0.0.4.0以後追加
    MetalHunter,
    Survival,
    FireResistance,
    IceResistance,
    ThunderResistance,
    LightResistance,
    DarkResistance,
    Cooperation,//background active

    DebuffResistance,
    MoveSpeed,
    BreakingTheLimit,
    //materialの獲得りょうUPのやつ

    Quester,
}
