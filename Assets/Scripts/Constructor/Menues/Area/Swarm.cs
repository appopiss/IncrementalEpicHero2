using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static Localized;
using Cysharp.Threading.Tasks;

public partial class Save
{
    public double lastSwarmPlaytime;
    public double swarmClearedNum;
    public double[] swarmBestScores;//[SwarmRarity]
}

public class Swarm
{
    public Swarm(AREA area)
    {
        this.area = area;
        lottery = new Lottery(rarityChance);
        score = new SwarmScore();
    }
    public AREA area;
    public bool isSwarm;
    public SwarmRarity rarity;
    public double timesecLeft => Math.Max(0, swarmTimesec + main.S.lastSwarmPlaytime - main.allTime);
    double swarmTimesec => 30 * 60;//30min
    public double clearNumLeft => Math.Max(0, requiredClearNum + startClearNum - area.completedNum.TotalCompletedNum());
    double startClearNum;
    double requiredClearNum
    {
        get
        {
            switch (rarity)
            {
                case SwarmRarity.Regular: return 50;
                case SwarmRarity.Large: return 150;
                case SwarmRarity.Epic: return 300;
            }
            return 10000;
        }
    }
    public int spawnNumFactor => 3 + (int)rarity;
    public int minimumSpawnNum => 10 + (int)rarity * 5;
    public double mutantSpawnChance => 0.00025d * (area.level.value + 1);//MutantSpawn確率
    Lottery lottery;
    public double[] rarityChance = new double[] { 0.85d, 0.13d, 0.02d };
    NUMBER scoreRewardMaterial;
    double scoreRewardMatNum;
    PotionGlobalInformation rewardTalisman => game.potionCtrl.GlobalInfo(MonsterParameter.SpeciesTalismanKind(area.kind));
    int rewardTalismanNum => 1 + (int)rarity;

    //Score
    public SwarmScore score;
    public void IncreaseScoreOnAreaClear(double factor = 1)//backgroundEfficiency
    {
        score.Increase(scoreIncrementPerAreaClear * factor);
    }
    public void IncreaseScoreOnMutantDefeat(double factor = 1)
    {
        score.Increase(scoreIncrementPerMutantDefeat * factor);
    }
    public void IncreaseScoreOnVanquish(double factor = 1)
    {
        score.Increase(scoreIncrementPerTimeLeftMin * Math.Floor(timesecLeft / 60));
    }
    public double scoreIncrementPerAreaClear => area.level.value + 1;//Regular:50~500, Large:150~1500, Epic:300~3000
    public double scoreIncrementPerMutantDefeat => 10;//
    public double scoreIncrementPerTimeLeftMin => 10;//Max10~300
    public void Clear()
    {
        IncreaseScoreOnVanquish();
        main.S.swarmClearedNum++;
        double score = this.score.value;
        main.S.swarmBestScores[(int)rarity] = Math.Max(main.S.swarmBestScores[(int)rarity], score);
        //Confirmを出す
        if (!SettingMenuUI.Toggle(ToggleKind.DisableSwarmResult).thisToggle.isOn)
        {
            gameUI.logCtrlUI.Log("<color=green>Vanquished the swarm!");
            var confirmUI = new ConfirmUI(gameUI.popupCtrlUI.defaultConfirm);
            confirmUI.SetUI(ClearConfirmString(score));
            confirmUI.okButton.onClick.AddListener(() => confirmUI.Hide());
        }
        GetReward(score);
        //Initialize();
        area.areaCtrl.FinishSwarm();
    }
    void GetReward(double score)
    {
        //これは別のClaimボタンでClaimできるように後に変更する?

        game.inventoryCtrl.CreatePotion(rewardTalisman, rewardTalismanNum);
        //if (rarity == SwarmRarity.Epic) game.areaCtrl.portalOrb.Increase(1);
        if (score >= 100) game.battleCtrl.blessingCtrl.Blessing(BlessingKind.ExpGain).StartBlessing(15 * 60);
        if (score >= 200) scoreRewardMaterial.Increase(scoreRewardMatNum);
        if (score >= 300) game.alchemyCtrl.talismanFragment.Increase(5);
        if (score >= 400) game.battleCtrl.blessingCtrl.Blessing(BlessingKind.GoldGain).StartBlessing(15 * 60);
        if (score >= 500) game.areaCtrl.portalOrb.Increase(1);
        if (score >= 750) game.inventoryCtrl.CreateEnchant(EnchantKind.OptionExtract);
        if (score >= 1000) ;
        if (score >= 1500) game.inventoryCtrl.CreateEnchant(EnchantKind.OptionLevelup);
        if (score >= 2000) ;
        if (score >= 2500) game.inventoryCtrl.CreateEnchant(EnchantKind.OptionLevelMax);
        if (score >= 3000) ;

    }
    //Reward
    //Clear : Talisman
    //Score :
    //100 : EXP Blessing (Duration : 1hour)
    //200 : Town Mat (そのエリアの獲得量x50）//Swarm開始時に量が決定される
    //300 : 5 Talisman Fragment
    //400 : Gold Gain Blessing (Duration : 1hour)
    //500 : 1 Portal Orb
    //750 : Extract Scroll
    //1000 : Talisman (Uncommon) [xxx]
    //1500 : Level Up Scroll
    //2000 : Talisman (Rare) [xxx]
    //2500 : Level Max Scroll
    //3000 : Talisman (Super Rare) [xxx]

    public string NameString()
    {
        switch (rarity)
        {
            case SwarmRarity.Regular: return "Regular Swarm";
            case SwarmRarity.Large: return "Large Swarm";
            case SwarmRarity.Epic: return "Epic Swarm";
        }
        return "Swarm";
    }
    public string RewardString()
    {
        if (!isSwarm) return "";
        string tempStr = optStr + "<size=18><u>Vanquish Reward</u>"
            + "\n<size=16>- Talisman [ " + rewardTalisman.Name() + " ]";
        if (rarity != SwarmRarity.Regular) tempStr += " x" + tDigit(rewardTalismanNum);
        if (!game.inventoryCtrl.CanCreatePotion(rewardTalisman, rewardTalismanNum)) tempStr += "\n<color=yellow>You need an empty Utility Slot</color>";

        tempStr += optStr + "\n\n<size=18><u>Score Reward</u> ( Current Score : <color=green>" + tDigit(score.value) + "</color> )";
        tempStr += optStrL + "\n<size=16>- 100 : EXP Gain Blessing (Duration 15 mins)"
            + "\n- 200 : " + tDigit(scoreRewardMatNum) + " " + scoreRewardMaterial.Name()
            + "\n- 300 : 5 Talisman Fragments"
            + "\n- 400 : Gold Gain Blessing (Duration 15 mins)"
            + "\n- 500 : 1 Portal Orb"
            + "\n- 750 : 1 " + localized.EnchantName(EnchantKind.OptionExtract)
            + "\n- 1000 : Talisman (Uncommon) [Placeholder]"
            + "\n- 1500 : 1 " + localized.EnchantName(EnchantKind.OptionLevelup)
            + "\n- 2000 : Talisman (Rare) [Placeholder]"
            + "\n- 2500 : 1 " + localized.EnchantName(EnchantKind.OptionLevelMax)
            + "\n- 3000 : Talisman (Super Rare) [Placeholder]";
        tempStr += optStrL + "\n\n<size=18><u>Score Criteria</u>"
    + "\n<size=16>- On Area Clear : + " + tDigit(scoreIncrementPerAreaClear)
    + "\n- On Defeat Mutants : + " + tDigit(scoreIncrementPerMutantDefeat) + " (Spawn Chance : " + percent(mutantSpawnChance) + ")"
    + "\n- On Vanquish the Swarm : + " + tDigit(scoreIncrementPerTimeLeftMin) + " x [Time Left (minute)]";
        if (game.ascensionCtrl.worldAscensions[0].performedNum >= 1)
            tempStr += "\n<color=yellow>The higher Area Difficulty, the more Score Gain</color>";
        return tempStr;
    }
    string ClearConfirmString(double score)
    {
        string tempStr = optStr + "Vanquished the " + rarity.ToString() + " Swarm!";
        tempStr += "\nTime : " + DoubleTimeToDate(main.allTime - main.S.lastSwarmPlaytime);
        tempStr += "\nScore : " + tDigit(score);
        tempStr += "\nYou received:<color=green>";
        tempStr += "\nTalisman [ " + rewardTalisman.Name() + " ]";
        if (rarity != SwarmRarity.Regular) tempStr += " x" + tDigit(rewardTalismanNum);
        if (score >= 100) tempStr += "\nEXP Gain Blessing (Duration 15 mins)";
        if (score >= 200) tempStr += "\n" + tDigit(scoreRewardMatNum) + " " + scoreRewardMaterial.Name();
        if (score >= 300) tempStr += "\n5 Talisman Fragments";
        if (score >= 400) tempStr += "\nGold Gain Blessing (Duration 15 mins)";
        if (score >= 500) tempStr += "\n1 Portal Orb";
        if (score >= 750) tempStr += "\n1 " + localized.EnchantName(EnchantKind.OptionExtract);
        if (score >= 1000) tempStr += "\nTalisman (Uncommon) [Placeholder]";
        if (score >= 1500) tempStr += "\n1 " + localized.EnchantName(EnchantKind.OptionLevelup);
        if (score >= 2000) tempStr += "\nTalisman (Rare) [Placeholder]";
        if (score >= 2500) tempStr += "\n1 " + localized.EnchantName(EnchantKind.OptionLevelMax);
        if (score >= 3000) tempStr += "\nTalisman (Super Rare) [Placeholder]";
        return tempStr;
    }

    public void Start()
    {

        isSwarm = true;
        score.ChangeValue(0);
        rarity = (SwarmRarity)lottery.SelectedId();
        startClearNum = area.completedNum.TotalCompletedNum();
        main.S.lastSwarmPlaytime = main.allTime;
        foreach (var item in area.rewardMaterial)
        {
            scoreRewardMaterial = item.Key;
            scoreRewardMatNum = item.Value(game.currentHero) * 50d / (1 + area.level.value * area.areaCtrl.townMaterialGainPerDifficultyMultiplier.Value());
        }
        //SwarmChaser
        if (game.epicStoreCtrl.Item(EpicStoreKind.SwarmChaser).IsPurchased() && SettingMenuUI.Toggle(ToggleKind.SwarmChaser).isOn)
        {
            //game.battleCtrl.areaBattle.Start(game.areaCtrl.currentSwarmArea);
            //Dungeon中の場合も考えて、currentAreaKindとcurrentAreaIdを変更するのみにする
            game.battleCtrl.areaBattle.currentAreaKind = game.areaCtrl.currentSwarmArea.kind;
            game.battleCtrl.areaBattle.currentAreaId = game.areaCtrl.currentSwarmArea.id;
            //game.battleCtrl.areaBattle.fieldUIAction(game.battleCtrl.areaBattle.CurrentArea());
            if (!game.battleCtrl.areaBattle.CurrentArea().isDungeon && !game.battleCtrl.areaBattle.CurrentArea().isChallenge)
                game.battleCtrl.areaBattle.Start(game.battleCtrl.areaBattle.CurrentArea());

            if (game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased())
                game.autoCtrl.Convene(game.areaCtrl.currentSwarmArea);
        }
        CheckFinish();
    }


    public void Initialize()
    {
        isSwarm = false;
        //SwarmChaserが終わったら自動でFavorite Areaへいく
        if (game.epicStoreCtrl.Item(EpicStoreKind.SwarmChaser).IsPurchased() && SettingMenuUI.Toggle(ToggleKind.SwarmChaser).isOn && game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).IsPurchased())
        {
            bool isPurchasedConvene = game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased();
            for (int i = 0; i < game.battleCtrls.Length; i++)
            {
                int count = i;
                if (count == (int)game.currentHero)
                {
                    //game.battleCtrls[i].areaBattle.Start(game.rebirthCtrl.FavoriteArea((HeroKind)count));
                    //Dungeon中の場合も考えて、currentAreaKindとcurrentAreaIdを変更するのみにする
                    game.battleCtrl.areaBattle.currentAreaKind = game.rebirthCtrl.FavoriteArea((HeroKind)count).kind;
                    game.battleCtrl.areaBattle.currentAreaId = game.rebirthCtrl.FavoriteArea((HeroKind)count).id;
                    if (!game.battleCtrl.areaBattle.CurrentArea().isDungeon && !game.battleCtrl.areaBattle.CurrentArea().isChallenge)
                        game.battleCtrl.areaBattle.Start(game.battleCtrl.areaBattle.CurrentArea());
                }
                else if (isPurchasedConvene)
                {
                    if (game.battleCtrls[i].isActiveBattle)
                        game.battleCtrls[i].areaBattle.Start(game.rebirthCtrl.FavoriteArea((HeroKind)count));
                }
            }
        }
    }
    async void CheckFinish()
    {
        while (true)
        {
            await UniTask.DelayFrame(60);
            if (!isSwarm) break;
            if (clearNumLeft <= 0)
            {
                Clear();
                break;
            }
            if (timesecLeft <= 0)
            {
                area.areaCtrl.FinishSwarm();
                break;
            }
        }
    }
}
public class SwarmScore : NUMBER
{
    //Saveする必要はない
}
public enum SwarmRarity
{
    Regular,
    Large,
    Epic,
}
