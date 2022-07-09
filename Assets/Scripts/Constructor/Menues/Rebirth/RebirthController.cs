using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static RebirthParameter;
using System;
using Cysharp.Threading.Tasks;

public partial class Save
{
    //統計
    public double[] totalRebirthNums;//[10*tier + heroKind]
    public double[] bestRebirthPlaytimes;//[10*tier + heroKind]

    //WAでもリセットしない
    public bool[] isAutoRebirthOns;//[heroKind]
    public long[] autoRebirthLevels;//[10*tier + heroKind]
    public double[] autoRebirthPoints;//[10*tier + heroKind]
    public int[] autoRebirthTiers;//[heroKind]
    public AreaKind[] favoriteAreaKinds;//[heroKind]
    public int[] favoriteAreaIds;//[heroKind]

    public bool[] isBestExpSecAreas;//[heroKind]
    public bool[] isAutoAbilityPointPresets;//[heroKind]
    public long[] autoAbilityPointPresetsVIT;//[heroKind]
    public long[] autoAbilityPointPresetsSTR;//[heroKind]
    public long[] autoAbilityPointPresetsINT;//[heroKind]
    public long[] autoAbilityPointPresetsAGI;//[heroKind]
    public long[] autoAbilityPointPresetsLUK;//[heroKind]
    public long[] autoAbilityPointMaxVIT;//[heroKind]
    public long[] autoAbilityPointMaxSTR;//[heroKind]
    public long[] autoAbilityPointMaxINT;//[heroKind]
    public long[] autoAbilityPointMaxAGI;//[heroKind]
    public long[] autoAbilityPointMaxLUK;//[heroKind]

    public double[] accomplishedFirstTimesRebirth;//[10*tier + heroKind]
    public double[] accomplishedTimesRebirth;//[10*tier + heroKind]
    public double[] accomplishedBestTimesRebirth;
}
public partial class SaveR
{
    //統計
    //WorldAscensionでリセット
    public long[] rebirthMaxHeroLevels;//[heroKind]

    public long[] rebirthNumsWarrior;//[tier]
    public long[] rebirthNumsWizard;//[tier]
    public long[] rebirthNumsAngel;//[tier]
    public long[] rebirthNumsThief;//[tier]
    public long[] rebirthNumsArcher;//[tier]
    public long[] rebirthNumsTamer;//[tier]
    public double[] rebirthPlayTimesWarrior;//[tier]
    public double[] rebirthPlayTimesWizard;//[tier]
    public double[] rebirthPlayTimesAngel;//[tier]
    public double[] rebirthPlayTimesThief;//[tier]
    public double[] rebirthPlayTimesArcher;//[tier]
    public double[] rebirthPlayTimesTamer;//[tier]
    public long[] rebirthMaxHeroLevelsWarrior;//[tier]Rebirthでの最大到達
    public long[] rebirthMaxHeroLevelsWizard;//[tier]
    public long[] rebirthMaxHeroLevelsAngel;//[tier]
    public long[] rebirthMaxHeroLevelsThief;//[tier]
    public long[] rebirthMaxHeroLevelsArcher;//[tier]
    public long[] rebirthMaxHeroLevelsTamer;//[tier]
    public double[] rebirthPointsWarrior;//[tier]
    public double[] rebirthPointsWizard;//[tier]
    public double[] rebirthPointsAngel;//[tier]
    public double[] rebirthPointsThief;//[tier]
    public double[] rebirthPointsArcher;//[tier]
    public double[] rebirthPointsTamer;//[tier]
    public long[] rebirthUpgradeLevelsWarrior;//[RebirthUpgradeKind]
    public long[] rebirthUpgradeLevelsWizard;//[RebirthUpgradeKind]
    public long[] rebirthUpgradeLevelsAngel;//[RebirthUpgradeKind]
    public long[] rebirthUpgradeLevelsThief;//[RebirthUpgradeKind]
    public long[] rebirthUpgradeLevelsArcher;//[RebirthUpgradeKind]
    public long[] rebirthUpgradeLevelsTamer;//[RebirthUpgradeKind]
}
public class RebirthController
{
    public bool IsAutoRebirth(HeroKind heroKind)
    {
        return main.S.isAutoRebirthOns[(int)heroKind];
    }
    public bool IsTravelBestArea(HeroKind heroKind)
    {
        if (!IsAutoRebirth(heroKind)) return false;
        if (SettingMenuUI.Toggle(ToggleKind.SwarmChaser).isOn && game.areaCtrl.isSwarm) return false;
        return main.S.isBestExpSecAreas[(int)heroKind];
    }
    public int AutoRebirthTier(HeroKind heroKind)
    {
        return main.S.autoRebirthTiers[(int)heroKind];
    }
    public AREA FavoriteArea(HeroKind heroKind)
    {
        //if (IsAutoRebirth(heroKind))
            return game.areaCtrl.Area(main.S.favoriteAreaKinds[(int)heroKind], main.S.favoriteAreaIds[(int)heroKind]);
        //return game.areaCtrl.Area(AreaKind.SlimeVillage, 0);
    }

    public void AutoRebirth()
    {
        if (SettingMenuUI.Toggle(ToggleKind.SwarmChaser).isOn && game.areaCtrl.isSwarm) return;
        for (int i = 0; i < rebirthList.Count; i++)
        {
            rebirthList[i].AutoRebirth();
        }
    }

    public RebirthController()
    {
        tier1AbilityPointBonusLevelCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => tierHeroLevel[1]));
        tier2AbilityPointBonusLevelCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => tierHeroLevel[2]));

        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            rebirth[count] = new Rebirth[3];//Tierの数ぶん
            rebirth[count][0] = new Rebirth(this, 0, (HeroKind)count);//tier0
            rebirth[count][1] = new Rebirth(this, 1, (HeroKind)count);//tier1
            rebirth[count][2] = new Rebirth(this, 2, (HeroKind)count);//tier2
            //rebirth[count][3] = new Rebirth(this, 3, (HeroKind)count);//tier3
            //rebirth[count][4] = new Rebirth(this, 4, (HeroKind)count);//tier4
            //rebirth[count][5] = new Rebirth(this, 5, (HeroKind)count);//tier5
            rebirthList.AddRange(rebirth[count]);
        }
        for (int i = 0; i < unlocks.Length; i++)
        {
            unlocks[i] = new Unlock();
        }
        pointMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        requiredHeroLevelReduction = new Multiplier(() => 100, () => 0);
    }
    public void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            for (int j = 0; j < rebirth[i].Length; j++)
            {
                rebirth[i][j].Start();
            }
        }
    }
    public Rebirth[][] rebirth = new Rebirth[Enum.GetNames(typeof(HeroKind)).Length][];
    public List<Rebirth> rebirthList = new List<Rebirth>();
    public Rebirth Rebirth(HeroKind heroKind, int tier)
    {
        if (tier >= rebirth[(int)heroKind].Length) return rebirth[(int)heroKind][0];
        return rebirth[(int)heroKind][tier];
    }
    public Unlock[] unlocks = new Unlock[6];
    public Multiplier pointMultiplier;
    public Multiplier requiredHeroLevelReduction;
    public Multiplier tier1AbilityPointBonusLevelCap;
    public Multiplier tier2AbilityPointBonusLevelCap;

    public long TotalRebirthNum(HeroKind heroKind, int tier)
    {
        return Rebirth(heroKind, tier).rebirthNum;
    }
    public long TotalRebirthNum(int tier)//全てのClassの積算
    {
        long tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            tempNum += TotalRebirthNum((HeroKind)count, tier);
        }
        return tempNum;
    }

    public double TotalRebirthPoint(HeroKind heroKind, int tier)
    {
        return Rebirth(heroKind, tier).TotalRebirthPoint();
    }
    public double TotalRebirthPoint(int tier)
    {
        double tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            tempNum += TotalRebirthPoint((HeroKind)count, tier);
        }
        return tempNum;
    }

    public void AutoRebirthUpgradeExp()
    {
        if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthUpgradeEXP)) return;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.ExpGain).transaction.Buy(true);
        }
    }

    public double AccomplishBestTime(int tier)
    {
        double tempTime = 1e20d;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            if (Rebirth(heroKind, tier).accomplish.accomplishedBestTime > 0) tempTime = Math.Min(tempTime, Rebirth(heroKind, tier).accomplish.accomplishedBestTime);
        }
        return tempTime;
    }
}

//Accomplish
public class AccomplishRebirth : ACCOMPLISH
{
    Rebirth rebirth;
    public override double accomplishedFirstTime { get => main.S.accomplishedFirstTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier]; set => main.S.accomplishedFirstTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier] = value; }
    public override double accomplishedTime { get => main.S.accomplishedTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier]; set => main.S.accomplishedTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier] = value; }
    public override double accomplishedBestTime { get => main.S.accomplishedBestTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier]; set => main.S.accomplishedBestTimesRebirth[(int)rebirth.heroKind + 10 * rebirth.tier] = value; }
    public AccomplishRebirth(Rebirth rebirth)
    {
        this.rebirth = rebirth;
    }
}

public class Rebirth
{
    public RebirthController rebirthCtrl;
    public AccomplishRebirth accomplish;
    public Multiplier additionalAbilityPoint;
    public Multiplier bonusEffectFactorOneDownTier;
    //public Multiplier initialRebirthPointOneDownTier;
    public Multiplier rebirthPointGainFactor;
    public List<RebirthPointKind> rebirthPointKinds = new List<RebirthPointKind>();

    public bool isAutoRebirthOn { get => rebirthCtrl.IsAutoRebirth(heroKind) && rebirthCtrl.AutoRebirthTier(heroKind) == tier; }
    public long autoRebirthLevel { get => main.S.autoRebirthLevels[10 * tier + (int)heroKind]; set => main.S.autoRebirthLevels[10 * tier + (int)heroKind] = value; }
    public double autoRebirthPoint { get => main.S.autoRebirthPoints[10 * tier + (int)heroKind]; set => main.S.autoRebirthPoints[10 * tier + (int)heroKind] = value; }
    public Rebirth(RebirthController rebirthCtrl, int tier, HeroKind heroKind)
    {
        this.rebirthCtrl = rebirthCtrl;
        this.tier = tier;
        this.heroKind = heroKind;
        Awake();
    }
    public void Awake()
    {
        accomplish = new AccomplishRebirth(this);
        rebirthPoint = new RebirthPoint(heroKind, tier);
        additionalAbilityPoint = new Multiplier(true);
        additionalAbilityPoint.minValue = () => 0;
        bonusEffectFactorOneDownTier = new Multiplier(true);
        initRebirthPoint = new Multiplier();
        //initialRebirthPointOneDownTier = new Multiplier(true);
        rebirthPointGainFactor = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        switch (tier)
        {
            case 0://Tier1
                additionalAbilityPoint.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => Math.Min(rebirthCtrl.tier1AbilityPointBonusLevelCap.Value(), maxHeroLevel) - tierHeroLevel[0]));
                rebirthPointKinds.Add(RebirthPointKind.HeroLevel);
                rebirthPointKinds.Add(RebirthPointKind.Quest);
                rebirthPointKinds.Add(RebirthPointKind.Move);
                rebirthUpgrades = new RebirthUpgrade[5];
                rebirthUpgrades[0] = new RebirthUpgrade(this, RebirthUpgradeKind.ExpGain);
                rebirthUpgrades[1] = new RebirthUpgrade(this, RebirthUpgradeKind.EQRequirement);
                rebirthUpgrades[2] = new RebirthUpgrade(this, RebirthUpgradeKind.QuestAcceptableNum);
                switch (heroKind)
                {
                    case HeroKind.Warrior:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicAtk);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.StoneGain);
                        break;
                    case HeroKind.Wizard:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicMAtk);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.CrystalGain);
                        break;
                    case HeroKind.Angel:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicHp);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.LeafGain);
                        break;
                    case HeroKind.Thief:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicDef);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.StoneGoldCap);
                        break;
                    case HeroKind.Archer:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicMDef);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.CrystalGoldCap);
                        break;
                    case HeroKind.Tamer:
                        rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.BasicMp);
                        rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.LeafGoldCap);
                        break;
                }
                break;
            case 1://Tier2
                additionalAbilityPoint.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => Math.Min(rebirthCtrl.tier2AbilityPointBonusLevelCap.Value(), maxHeroLevel) - tierHeroLevel[1], () => rebirthNum > 0));
                bonusEffectFactorOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0.1d * Math.Pow(rebirthNum, 2 / 3d)));
                //initialRebirthPointOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => rebirthNum * 500));
                rebirthPointKinds.Add(RebirthPointKind.SkillLevel);
                rebirthPointKinds.Add(RebirthPointKind.HeroLevel);
                rebirthPointKinds.Add(RebirthPointKind.Quest);
                rebirthPointKinds.Add(RebirthPointKind.Move);
                rebirthUpgrades = new RebirthUpgrade[8];
                rebirthUpgrades[0] = new RebirthUpgrade(this, RebirthUpgradeKind.SkillProfGain);
                rebirthUpgrades[1] = new RebirthUpgrade(this, RebirthUpgradeKind.SkillRankCostReduction);
                rebirthUpgrades[2] = new RebirthUpgrade(this, RebirthUpgradeKind.ClassSkillSlot);
                rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.ShareSkillPassive);
                rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.T1ExpGainBoost);
                rebirthUpgrades[5] = new RebirthUpgrade(this, RebirthUpgradeKind.T1RebirthPointGainBoost);
                switch (heroKind)
                {
                    case HeroKind.Warrior:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicAtkBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1StoneGainBoost);
                        break;
                    case HeroKind.Wizard:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicMAtkBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1CrystalGainBoost);
                        break;
                    case HeroKind.Angel:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicHpBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1LeafGainBoost);
                        break;
                    case HeroKind.Thief:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicDefBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1StoneGoldCapBoost);
                        break;
                    case HeroKind.Archer:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicMDefBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1CrystalGoldCapBoost);
                        break;
                    case HeroKind.Tamer:
                        rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T1BasicMpBoost);
                        rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T1LeafGoldCapBoost);
                        break;
                }
                break;
            case 2://Tier3
                additionalAbilityPoint.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => Math.Min(tierHeroLevel[3], maxHeroLevel) - tierHeroLevel[2], () => rebirthNum > 0));
                bonusEffectFactorOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0.1d * Math.Pow(rebirthNum, 2 / 3d)));
                //initialRebirthPointOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => rebirthNum * 500));
                rebirthPointKinds.Add(RebirthPointKind.EQLevel);
                rebirthPointKinds.Add(RebirthPointKind.SkillLevel);
                rebirthPointKinds.Add(RebirthPointKind.HeroLevel);
                rebirthPointKinds.Add(RebirthPointKind.Quest);
                rebirthPointKinds.Add(RebirthPointKind.Move);
                rebirthUpgrades = new RebirthUpgrade[10];
                rebirthUpgrades[0] = new RebirthUpgrade(this, RebirthUpgradeKind.EQProfGain);
                rebirthUpgrades[1] = new RebirthUpgrade(this, RebirthUpgradeKind.EQLevelCap);
                rebirthUpgrades[2] = new RebirthUpgrade(this, RebirthUpgradeKind.EQWeaponSlot);
                rebirthUpgrades[3] = new RebirthUpgrade(this, RebirthUpgradeKind.EQArmorSlot);
                rebirthUpgrades[4] = new RebirthUpgrade(this, RebirthUpgradeKind.EQJewelrySlot);
                rebirthUpgrades[5] = new RebirthUpgrade(this, RebirthUpgradeKind.T2ExpGainBoost);
                rebirthUpgrades[6] = new RebirthUpgrade(this, RebirthUpgradeKind.T2SkillProfGainBoost);
                rebirthUpgrades[7] = new RebirthUpgrade(this, RebirthUpgradeKind.T2RebirthPointGainBoost);
                switch (heroKind)
                {
                    case HeroKind.Warrior:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicAtkBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2StoneGainBoost);
                        break;
                    case HeroKind.Wizard:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicMAtkBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2CrystalGainBoost);
                        break;
                    case HeroKind.Angel:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicHpBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2LeafGainBoost);
                        break;
                    case HeroKind.Thief:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicDefBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2StoneGoldCapBoost);
                        break;
                    case HeroKind.Archer:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicMDefBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2CrystalGoldCapBoost);
                        break;
                    case HeroKind.Tamer:
                        rebirthUpgrades[8] = new RebirthUpgrade(this, RebirthUpgradeKind.T2BasicMpBoost);
                        rebirthUpgrades[9] = new RebirthUpgrade(this, RebirthUpgradeKind.T2LeafGoldCapBoost);
                        break;
                }
                break;
            default:
                break;
        }
    }
    public void Start()
    {
        //Upgrade
        for (int i = 0; i < rebirthUpgrades.Length; i++)
        {
            rebirthUpgrades[i].Start();
        }
        //Bonusの適用
        if (tier <= 0) return;
        rebirthCtrl.Rebirth(heroKind, tier - 1).additionalAbilityPoint.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => bonusEffectFactorOneDownTier.Value()));
        rebirthCtrl.Rebirth(heroKind, tier - 1).bonusEffectFactorOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => bonusEffectFactorOneDownTier.Value()));
        //rebirthCtrl.Rebirth(heroKind, tier - 1).initialRebirthPointOneDownTier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => bonusEffectFactorOneDownTier.Value()));
    }

    //外部アクセス
    public RebirthUpgrade Upgrade(RebirthUpgradeKind kind)
    {
        for (int i = 0; i < rebirthUpgrades.Length; i++)
        {
            if (rebirthUpgrades[i].kind == kind) return rebirthUpgrades[i];
        }
        return null;
    }

    public void AutoRebirth()
    {
        if (CanAutoRebirth())
            DoRebirth();
    }
    public bool CanAutoRebirth()
    {
        if (!isAutoRebirthOn) return false;
        if (!game.guildCtrl.Member(heroKind).isActive) return false;
        if (autoRebirthPoint > RebirthPointGain()) return false;
        if (autoRebirthLevel > game.statsCtrl.HeroLevel(heroKind).value) return false;
        return true;
    }

    bool isTrying;
    public void DoRebirth()
    {
        if (!CanRebirth()) return;
        isTrying = true;
        game.battleCtrls[(int)heroKind].isActiveBattle = false;

        //統計
        rebirthNum++;
        totalRebirthNum++;

        if (bestRebirthPlayTime > 0)//２回目以降
            bestRebirthPlayTime = Math.Min(RebirthPlaytime(), bestRebirthPlayTime);
        else //初めてRebirthした時
            bestRebirthPlayTime = main.allTime;

        rebirthPlayTime = main.allTime;
        //下のTierもPlaytimeをリセットする
        for (int i = 0; i < tier; i++)
        {
            rebirthCtrl.Rebirth(heroKind, i).rebirthPlayTime = main.allTime;
        }

        maxHeroLevel = Math.Max(maxHeroLevel, game.statsCtrl.HeroLevel(heroKind).value);
        main.SR.rebirthMaxHeroLevels[(int)heroKind] = Math.Max(main.SR.rebirthMaxHeroLevels[(int)heroKind], maxHeroLevel);
        accomplish.RegisterTime();

        GetPoint();
        ResetSave();
        GetBonus();

        //QoLこれは毎２秒呼ぶことにした
        //game.questCtrl.AcceptFavorite(heroKind);

        //UI
        if (game.IsUI(heroKind) && rebirthUIAction != null) rebirthUIAction();
        game.battleCtrls[(int)heroKind].areaBattle.Start(rebirthCtrl.FavoriteArea(heroKind));
        game.battleCtrls[(int)heroKind].isActiveBattle = true;
        isTrying = false;
    }
    public Action rebirthUIAction;
    public long heroLevel => (long)Math.Ceiling(Math.Max(50, tierHeroLevel[tier] - rebirthCtrl.requiredHeroLevelReduction.Value()));
    public bool CanRebirth()
    {
        if (isTrying) return false;
        if (!rebirthCtrl.unlocks[tier].IsUnlocked()) return false;
        if (game.challengeCtrl.IsTryingChallenge()) return false;
        if (game.battleCtrls[(int)heroKind].areaBattle.CurrentArea().isDungeon) return false;
        if (game.areaCtrl.areaClearedNums[(int)heroKind] < 1) return false;
        return game.statsCtrl.Level(heroKind) >= heroLevel;
    }
    public double RebirthPlaytime() { return main.allTime - rebirthPlayTime; }

    void GetPoint()
    {
        rebirthPoint.Increase(RebirthPointGain());
    }

    public double RebirthPointGain()
    {
        double tempPoint = 0;
        for (int i = 0; i < rebirthPointKinds.Count; i++)
        {
            int count = i;
            tempPoint += RebirthPointGain(rebirthPointKinds[count]);
        }
        return Math.Floor(tempPoint);
    }
    public double RebirthPointGain(RebirthPointKind kind)
    {
        double tempPoint = 0;
        switch (kind)
        {
            case RebirthPointKind.HeroLevel:
                long heroLevel = game.statsCtrl.HeroLevel(heroKind).value;
                if (tier > 0) heroLevel = Math.Max(0, heroLevel - tierHeroLevel[tier]);
                tempPoint = 0.05d * heroLevel * (1 + heroLevel) * Math.Pow(2d, heroLevel / 200d);
                //Lv100=1010pt, Lv110=1221pt, Lv120=1452pt, Lv200=4020
                break;
            case RebirthPointKind.SkillLevel://BaseSkillのレベルの累計値(Lv100で5050*0.05=252.5pt, Lv200で1005)
                //過去のもの:tempPoint = 0.05d * 0.5d * game.skillCtrl.Skill(heroKind, 0).level.value * (game.skillCtrl.Skill(heroKind, 0).level.value + 1);
                for (int i = 0; i < 10; i++)
                {
                    int count = i;
                    long level = game.skillCtrl.Skill(heroKind, count).level.value;
                    if (tier > 1) level = Math.Max(0, level - 100 * (tier - 1));//Tier 3 : Skill Lv - 100. Tier 4 : -200, Tier5:-300, Tier6:-400
                    double profDifficulty = game.skillCtrl.Skill(heroKind, count).profDifficulty;
                    double tempTempPoint = 0.50d * level * (level + 1) * (1 + profDifficulty * 1.35d) * 0.75d * Math.Pow(2d, level / 300d);//1.50dから変更
                    if (i == 0) tempPoint += 0.030d * tempTempPoint;//BaseSkillは比重が少しだけ大きい（もともと0.040にしてた）
                    else tempPoint += 0.010d * tempTempPoint;
                }
                break;
            case RebirthPointKind.EQLevel:
                for (int i = 0; i < game.equipmentCtrl.globalInformations.Length; i++)
                {
                    long eqLevel = game.equipmentCtrl.globalInformations[i].levels[(int)heroKind].value;
                    if (tier > 2) eqLevel = Math.Max(0, eqLevel - 5 * (tier - 2));
                    double rarity = 1 + (int)game.equipmentCtrl.globalInformations[i].rarity * 3.0d;//もともと*2.5dはなし->さらに3.0dにした
                    if (eqLevel >= 1) tempPoint += 0.1d * 0.50d * eqLevel * (eqLevel + 1) * rarity;//もともと2*eqLevel*rarity
                }
                break;
            case RebirthPointKind.Quest://GeneralQuestクリア数
                for (int i = 0; i < game.questCtrl.QuestArray(QuestKind.General, heroKind).Length; i++)
                {
                    if (game.questCtrl.QuestArray(QuestKind.General, heroKind)[i].isCleared)
                        tempPoint += game.questCtrl.QuestArray(QuestKind.General, heroKind)[i].rebirthPointGain;
                        //tempPoint += 30d + game.questCtrl.QuestArray(QuestKind.General, heroKind)[i].unlockHeroLevel() / 5d;//もともとは/4d
                }
                break;
            case RebirthPointKind.Move:
                tempPoint = game.statsCtrl.MovedDistance(heroKind, true).value / 50000d;
                break;
        }
        tempPoint *= rebirthPointGainFactor.Value();
        tempPoint *= rebirthCtrl.pointMultiplier.Value();
        return tempPoint;
    }

    void ResetSave()
    {
        //AreaClear#forRB
        game.areaCtrl.areaClearedNums[(int)heroKind] = 0;

        //HeroLevel
        game.statsCtrl.Exp(heroKind).ChangeValue(0);
        game.statsCtrl.HeroLevel(heroKind).ChangeValue(0);
        //Ability
        game.statsCtrl.ResetAbilityPoint(heroKind);
        //GeneralQuest
        for (int i = 0; i < game.questCtrl.QuestArray(QuestKind.General, heroKind).Length; i++)
        {
            game.questCtrl.QuestArray(QuestKind.General, heroKind)[i].isAccepted = false;
            game.questCtrl.QuestArray(QuestKind.General, heroKind)[i].isCleared = false;
        }
        //MoveDistance
        game.statsCtrl.MovedDistance(heroKind, true).ChangeValue(0);

        //Tier2
        if (tier < 1) return;
        //SkillLevel
        long tempToLevel = 0;
        //Tier3のボーナス効果によって、Baseのスキルレベルの初期値が変更される
        if (rebirthCtrl.Rebirth(heroKind, 2).rebirthNum > 0)
        {
            tempToLevel = (long)rebirthCtrl.Rebirth(heroKind, 2).additionalAbilityPoint.Value();
        }
        for (int i = 0; i < game.skillCtrl.SkillArray(heroKind).Length; i++)
        {
            game.skillCtrl.SkillArray(heroKind)[i].proficiency.ChangeValue(0);
            if (i == 0)
                game.skillCtrl.SkillArray(heroKind)[i].level.ChangeValue(tempToLevel);
            else
                game.skillCtrl.SkillArray(heroKind)[i].level.ChangeValue(0);
        }

        //Tier3
        if (tier < 2) return;
        //EQLevel
        for (int i = 0; i < game.equipmentCtrl.globalInformations.Length; i++)
        {
            game.equipmentCtrl.globalInformations[i].proficiencies[(int)heroKind].ChangeValue(0);
            game.equipmentCtrl.globalInformations[i].levels[(int)heroKind].ChangeValue(0);
            game.equipmentCtrl.globalInformations[i].levels[(int)heroKind].isMaxedThisRebirth = false;
        }

        //rebirthCtrl.Rebirth(heroKind, 0).rebirthNum = 0;
        //rebirthCtrl.Rebirth(heroKind, 0).ResetRebirthUpgrade();
        //↓これはかかなくてもOK
        //rebirthCtrl.Rebirth(heroKind, 0).rebirthPoint.ChangeValue(0);
    }
    void GetBonus()
    {
        //AbilityPoint
        game.statsCtrl.AbilityPointLeft(heroKind).ChangeValue(rebirthCtrl.Rebirth(heroKind, 0).additionalAbilityPoint.Value());
        switch (tier)
        {
            case 0://Tier1
                break;
            case 1://Tier2
                //RebirthPointの初期値（Tier1）
                //rebirthCtrl.Rebirth(heroKind, 0).rebirthPoint.ChangeValue(initialRebirthPointOneDownTier.Value());
                break;
            case 2://Tier3
                break;
        }
    }

    //Upgradeのリセット
    public void ResetRebirthUpgrade()
    {
        rebirthPoint.ChangeValue(TotalRebirthPoint());
        ResetRebirthUpgradeLevel();
    }
    void ResetRebirthUpgradeLevel()
    {
        for (int i = 0; i < rebirthUpgrades.Length; i++)
        {
            rebirthUpgrades[i].level.ChangeValue(0);
        }
        //UI
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.slotUIAction();
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].skillSet.CheckMaxNum();
            game.battleCtrls[i].skillSet.UpdateCurrentSkillSet();
        }
    }
    public double TotalRebirthPoint()
    {
        double tempPoint = rebirthPoint.value;
        for (int i = 0; i < rebirthUpgrades.Length; i++)
        {
            tempPoint += rebirthUpgrades[i].transaction.TotalCostConsumed();
        }
        return tempPoint;
    }

    public HeroKind heroKind;
    public int tier;
    public RebirthPoint rebirthPoint;
    public Multiplier initRebirthPoint;
    //WAUpgradeによって初期値が変わる
    public void ResetRebirthPoint()
    {
        rebirthPoint.ChangeValue(initRebirthPoint.Value());
    }
    public RebirthUpgrade[] rebirthUpgrades;
    public long rebirthNum
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.rebirthNumsWarrior[tier];
                case HeroKind.Wizard:
                    return main.SR.rebirthNumsWizard[tier];
                case HeroKind.Angel:
                    return main.SR.rebirthNumsAngel[tier];
                case HeroKind.Thief:
                    return main.SR.rebirthNumsThief[tier];
                case HeroKind.Archer:
                    return main.SR.rebirthNumsArcher[tier];
                case HeroKind.Tamer:
                    return main.SR.rebirthNumsTamer[tier];
                default:
                    return 0;
            }
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.rebirthNumsWarrior[tier] = value; break;
                case HeroKind.Wizard:
                    main.SR.rebirthNumsWizard[tier] = value; break;
                case HeroKind.Angel:
                    main.SR.rebirthNumsAngel[tier] = value; break;
                case HeroKind.Thief:
                    main.SR.rebirthNumsThief[tier] = value; break;
                case HeroKind.Archer:
                    main.SR.rebirthNumsArcher[tier] = value; break;
                case HeroKind.Tamer:
                    main.SR.rebirthNumsTamer[tier] = value; break;
            }
        }
    }
    public double totalRebirthNum { get => main.S.totalRebirthNums[(int)heroKind + 10 * tier]; set => main.S.totalRebirthNums[(int)heroKind + 10 * tier] = value; }
    public double rebirthPlayTime
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.rebirthPlayTimesWarrior[tier];
                case HeroKind.Wizard:
                    return main.SR.rebirthPlayTimesWizard[tier];
                case HeroKind.Angel:
                    return main.SR.rebirthPlayTimesAngel[tier];
                case HeroKind.Thief:
                    return main.SR.rebirthPlayTimesThief[tier];
                case HeroKind.Archer:
                    return main.SR.rebirthPlayTimesArcher[tier];
                case HeroKind.Tamer:
                    return main.SR.rebirthPlayTimesTamer[tier];
                default:
                    return 0;
            }
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.rebirthPlayTimesWarrior[tier] = value; break;
                case HeroKind.Wizard:
                    main.SR.rebirthPlayTimesWizard[tier] = value; break;
                case HeroKind.Angel:
                    main.SR.rebirthPlayTimesAngel[tier] = value; break;
                case HeroKind.Thief:
                    main.SR.rebirthPlayTimesThief[tier] = value; break;
                case HeroKind.Archer:
                    main.SR.rebirthPlayTimesArcher[tier] = value; break;
                case HeroKind.Tamer:
                    main.SR.rebirthPlayTimesTamer[tier] = value; break;
            }
        }
    }
    public double bestRebirthPlayTime { get => main.S.bestRebirthPlaytimes[(int)heroKind + 10 * tier]; set => main.S.bestRebirthPlaytimes[(int)heroKind + 10 * tier] = value; }
    public long maxHeroLevel
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.rebirthMaxHeroLevelsWarrior[tier];
                case HeroKind.Wizard:
                    return main.SR.rebirthMaxHeroLevelsWizard[tier];
                case HeroKind.Angel:
                    return main.SR.rebirthMaxHeroLevelsAngel[tier];
                case HeroKind.Thief:
                    return main.SR.rebirthMaxHeroLevelsThief[tier];
                case HeroKind.Archer:
                    return main.SR.rebirthMaxHeroLevelsArcher[tier];
                case HeroKind.Tamer:
                    return main.SR.rebirthMaxHeroLevelsTamer[tier];
                default:
                    return 0;
            }
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.rebirthMaxHeroLevelsWarrior[tier] = value; break;
                case HeroKind.Wizard:
                    main.SR.rebirthMaxHeroLevelsWizard[tier] = value; break;
                case HeroKind.Angel:
                    main.SR.rebirthMaxHeroLevelsAngel[tier] = value; break;
                case HeroKind.Thief:
                    main.SR.rebirthMaxHeroLevelsThief[tier] = value; break;
                case HeroKind.Archer:
                    main.SR.rebirthMaxHeroLevelsArcher[tier] = value; break;
                case HeroKind.Tamer:
                    main.SR.rebirthMaxHeroLevelsTamer[tier] = value; break;
            }
        }
    }
}

public enum RebirthPointKind
{
    HeroLevel,//Tier1
    SkillLevel,//Tier2
    EQLevel,//Tier3
    Quest,//共通
    Move,//共通
}

public class RebirthPoint : NUMBER
{
    public RebirthPoint(HeroKind heroKind, int tier)
    {
        this.heroKind = heroKind;
        this.tier = tier;
    }
    int tier;
    HeroKind heroKind;
    public override double value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.rebirthPointsWarrior[tier];
                case HeroKind.Wizard:
                    return main.SR.rebirthPointsWizard[tier];
                case HeroKind.Angel:
                    return main.SR.rebirthPointsAngel[tier];
                case HeroKind.Thief:
                    return main.SR.rebirthPointsThief[tier];
                case HeroKind.Archer:
                    return main.SR.rebirthPointsArcher[tier];
                case HeroKind.Tamer:
                    return main.SR.rebirthPointsTamer[tier];
                default:
                    return 0;
            }
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.rebirthPointsWarrior[tier] = value; break;
                case HeroKind.Wizard:
                    main.SR.rebirthPointsWizard[tier] = value; break;
                case HeroKind.Angel:
                    main.SR.rebirthPointsAngel[tier] = value; break;
                case HeroKind.Thief:
                    main.SR.rebirthPointsThief[tier] = value; break;
                case HeroKind.Archer:
                    main.SR.rebirthPointsArcher[tier] = value; break;
                case HeroKind.Tamer:
                    main.SR.rebirthPointsTamer[tier] = value; break;
            }
        }
    }
}
public class RebirthUpgrade
{
    public RebirthUpgrade(Rebirth rebirth, RebirthUpgradeKind kind)
    {
        this.rebirth = rebirth;
        this.kind = kind;
        level = new RebirthUpgradeLevel(kind, heroKind, () => Upgrade(kind, 0).maxLevel);
        transaction = new Transaction(level, rebirth.rebirthPoint, Cost);
        transaction.isOnBuyOneToggle = () => main.S.isToggleOn[(int)ToggleKind.BuyOneRebirthUpgrade];
        transaction.additionalBuyAction = BuyAction;
        effectMultiFactor = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
    }
    double Cost(long level)
    {
        double tempValue = 0;
        if (Upgrade(kind, this.level.value).isLinear)
            tempValue = Upgrade(kind, this.level.value).initCost + level * Upgrade(kind, this.level.value).baseCost;
        else tempValue = Upgrade(kind, this.level.value).initCost * Math.Pow(Upgrade(kind, this.level.value).baseCost, level);

        //Balance用。Levelが上がるにつれてちょっとキツくする
        tempValue *= Math.Pow(2d, level / 50d);//Lv50...x3.6
        tempValue *= Math.Pow(2d, level / 100d);//Lv100...x11.3
        tempValue *= Math.Pow(2d, level / 200d);//Lv200...x128
        if (level >= 20)
            tempValue *= Math.Pow(5d, (double)(level - 20) / 20d);
        return tempValue;
    }

    public void Start()
    {
        SetEffect();
    }
    void SetEffect()
    {
        switch (kind)
        {
            //Tier1
            case RebirthUpgradeKind.ExpGain:
                game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                break;
            case RebirthUpgradeKind.EQRequirement:
                game.statsCtrl.LevelForEquipment(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.QuestAcceptableNum:
                game.questCtrl.AcceptableNum(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.BasicAtk:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.ATK, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.BasicMAtk:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.MATK, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.BasicHp:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.HP, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.BasicDef:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.DEF, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.BasicMDef:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.MDEF, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.BasicMp:
                game.statsCtrl.SetEffectBasicStatsPerLevel(BasicStatsKind.MP, new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.StoneGain:
                game.statsCtrl.ResourceGain(ResourceKind.Stone).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.CrystalGain:
                game.statsCtrl.ResourceGain(ResourceKind.Crystal).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.LeafGain:
                game.statsCtrl.ResourceGain(ResourceKind.Leaf).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.StoneGoldCap:
                game.upgradeCtrl.goldcapMultipliers[(int)ResourceKind.Stone].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.CrystalGoldCap:
                game.upgradeCtrl.goldcapMultipliers[(int)ResourceKind.Crystal].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.LeafGoldCap:
                game.upgradeCtrl.goldcapMultipliers[(int)ResourceKind.Leaf].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            //Tier2
            case RebirthUpgradeKind.SkillProfGain:
                game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                break;
            case RebirthUpgradeKind.SkillRankCostReduction:
                //ここで「Add」として登録するため、初期値が１になっている
                game.skillCtrl.skillRankCostFactors[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.ClassSkillSlot:
                game.statsCtrl.SkillSlotNum(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.ShareSkillPassive:
                game.skillCtrl.skillPassiveShareFactors[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T1ExpGainBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.ExpGain).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T1RebirthPointGainBoost:
                rebirthCtrl.Rebirth(heroKind, 0).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T1BasicAtkBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicAtk).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1BasicMAtkBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicMAtk).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1BasicHpBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicHp).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1BasicDefBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicDef).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1BasicMDefBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicMDef).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1BasicMpBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.BasicMp).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1StoneGainBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.StoneGain).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1CrystalGainBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.CrystalGain).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1LeafGainBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.LeafGain).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1StoneGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.StoneGoldCap).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1CrystalGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.CrystalGoldCap).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T1LeafGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 0).Upgrade(RebirthUpgradeKind.LeafGoldCap).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;

            //Tier3
            case RebirthUpgradeKind.EQLevelCap:
                game.equipmentCtrl.maxLevels[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.EQProfGain:
                game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Mul, () => effectValue));
                break;
            case RebirthUpgradeKind.EQWeaponSlot:
                game.inventoryCtrl.equipWeaponUnlockedNum[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.EQArmorSlot:
                game.inventoryCtrl.equipArmorUnlockedNum[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.EQJewelrySlot:
                game.inventoryCtrl.equipJewelryUnlockedNum[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T2ExpGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1ExpGainBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T2SkillProfGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.SkillProfGain).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T2RebirthPointGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                break;
            case RebirthUpgradeKind.T2BasicAtkBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicAtkBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2BasicMAtkBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicMAtkBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2BasicHpBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicHpBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2BasicDefBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicDefBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2BasicMDefBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicMDefBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2BasicMpBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1BasicMpBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2StoneGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1StoneGainBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2CrystalGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1CrystalGainBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2LeafGainBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1LeafGainBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2StoneGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1StoneGoldCapBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2CrystalGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1CrystalGoldCapBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
            case RebirthUpgradeKind.T2LeafGoldCapBoost:
                rebirthCtrl.Rebirth(heroKind, 1).Upgrade(RebirthUpgradeKind.T1LeafGoldCapBoost).effectMultiFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Rebirth, MultiplierType.Add, () => effectValue));
                isGlobalEffect = true;
                break;
        }
    }
    public void BuyAction()
    {
        switch (kind)
        {
            case RebirthUpgradeKind.ClassSkillSlot:
                GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
                break;
        }
    }
    public Rebirth rebirth;
    public RebirthController rebirthCtrl { get => rebirth.rebirthCtrl; }
    public int tier { get => rebirth.tier; }
    public RebirthUpgradeKind kind;
    public HeroKind heroKind { get => rebirth.heroKind; }
    public RebirthUpgradeLevel level;
    public Transaction transaction;
    public bool isGlobalEffect;
    public Multiplier effectMultiFactor;
    public double effectValue { get => Upgrade(kind, level.value).effectValue * effectMultiFactor.Value(); }
    public double nextEffectValue { get => Upgrade(kind, transaction.ToLevel()).effectValue * effectMultiFactor.Value(); }
}
public class RebirthUpgradeLevel : INTEGER
{
    public RebirthUpgradeLevel(RebirthUpgradeKind kind, HeroKind heroKind, Func<long> maxValue)
    {
        this.kind = kind;
        this.heroKind = heroKind;
        this.maxValue = maxValue;
    }
    RebirthUpgradeKind kind;
    HeroKind heroKind;
    public override long value
    {
        get
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    return main.SR.rebirthUpgradeLevelsWarrior[(int)kind];
                case HeroKind.Wizard:
                    return main.SR.rebirthUpgradeLevelsWizard[(int)kind];
                case HeroKind.Angel:
                    return main.SR.rebirthUpgradeLevelsAngel[(int)kind];
                case HeroKind.Thief:
                    return main.SR.rebirthUpgradeLevelsThief[(int)kind];
                case HeroKind.Archer:
                    return main.SR.rebirthUpgradeLevelsArcher[(int)kind];
                case HeroKind.Tamer:
                    return main.SR.rebirthUpgradeLevelsTamer[(int)kind];
            }
            return 0;
        }
        set
        {
            switch (heroKind)
            {
                case HeroKind.Warrior:
                    main.SR.rebirthUpgradeLevelsWarrior[(int)kind] = value; break;
                case HeroKind.Wizard:
                    main.SR.rebirthUpgradeLevelsWizard[(int)kind] = value; break;
                case HeroKind.Angel:
                    main.SR.rebirthUpgradeLevelsAngel[(int)kind] = value; break;
                case HeroKind.Thief:
                    main.SR.rebirthUpgradeLevelsThief[(int)kind] = value; break;
                case HeroKind.Archer:
                    main.SR.rebirthUpgradeLevelsArcher[(int)kind] = value; break;
                case HeroKind.Tamer:
                    main.SR.rebirthUpgradeLevelsTamer[(int)kind] = value; break;
            }
        }
    }
}

