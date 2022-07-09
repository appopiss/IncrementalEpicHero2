using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public class OfflineBonusUI : MonoBehaviour
{
    [SerializeField] Button[] selectButtons;
    CanvasGroup thisCanvasGroup;
    [SerializeField] TextMeshProUGUI titleText, nitroText, playtimeText, playtimebonusText;

    public bool isShow;

    private void Awake()
    {
        thisCanvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        selectButtons[0].onClick.AddListener(() => { main.Save(); game.offlineBonus.GetOfflineBonus(OfflineBonusKind.Nitro); SetActive(gameObject, false); game.isPause = false; });
        selectButtons[0].onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.QuestClaim));
        selectButtons[1].onClick.AddListener(() => { main.Save(); game.offlineBonus.GetOfflineBonus(OfflineBonusKind.Playtime); SetActive(gameObject, false); game.isPause = false; });
        selectButtons[1].onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.QuestClaim));
        Hide();
    }
    public async void SetUI()
    {
        await UniTask.WaitUntil(() => game.offlineBonus.isFinishedCalculation);
        game.resourceCtrl.goldCap.Calculate();
        game.nitroCtrl.nitroCap.Calculate();
        titleText.text = TitleString();
        nitroText.text = NitroString();
        playtimeText.text = PlaytimeString();
        playtimebonusText.text = PlaytimeBonusString();
        DelayShow();
    }


    string TitleString()
    {
        string tempStr = "<size=34><b>OFFLINE BONUS</b><size=24>";
        tempStr += "\n\nYou have left for <color=green>" + DoubleTimeToDate(game.offlineBonus.offlineTimesec) + "</color>";
        tempStr += "\nSelect an offline bonus from the followings:";
        return tempStr;
    }
    string NitroString()
    {
        double nitroGain = game.offlineBonus.NitroGain();
        string tempStr = "<size=24><b><color=#00FFFF>[ Nitro Offline Bonus ]</b></color><size=20>";
        tempStr += "\n\nYou will gain <color=green>" + tDigit(nitroGain) + " Nitro</color>";
        if (nitroGain <= 0) tempStr += "<color=yellow>\nYou are at current max Nitro.</color>";
        else if (game.nitroCtrl.nitro.IsMaxed(nitroGain)) tempStr += "<color=yellow> (Maxed)</color>";
        tempStr += "\n\n<size=18>1 Nitro is gained per 4 offline seconds.\nIn-Game Playtime won't increase.";
        return tempStr;
    }
    string PlaytimeString()
    {
        string tempStr = "<size=24><b><color=#00FFFF>[ Passive Playtime Offline Bonus ]</b></color><size=20>";
        tempStr += "\n\nYou will gain the following:";
        return tempStr;
    }
    string PlaytimeBonusString()
    {
        string tempStr = "<size=20><color=green>In-Game Playtime : " + DoubleTimeToDate(game.offlineBonus.offlineTimesec) + "</color>";
        tempStr += "\n   - Town Research Progress";
        tempStr += "\n   - Shop Restock Timer";
        tempStr += "\n   - Expedition Progress";
        tempStr += "\n\n<color=green>Active Hero Progress : " + localized.Hero(game.currentHero) + " at " + percent(game.offlineBonus.gainFactor) + "</color>";
        tempStr += "\n   - " + game.battleCtrl.areaBattle.CurrentArea().Name(true, true, false) + " Clear # " + tDigit(game.offlineBonus.areaClearNum, 1);
        double exp = game.offlineBonus.exp;
        long tempLevel = game.statsCtrl.EstimatedLevel(game.currentHero, exp, true).level;
        long tempLevelIncrement = tempLevel - game.statsCtrl.HeroLevel(game.currentHero).value;
        //if (tempLevelIncrement < 30)
            tempStr += "\n   - " + tDigit(exp, 1) + " " + localized.Hero(game.currentHero) + " EXP ( Lv " + tDigit(tempLevel) + " : EXP " + percent(game.statsCtrl.EstimatedLevel(game.currentHero, exp, true).expPercent, 0) + " )";
        //else
        //    tempStr += "\n   - " + tDigit(exp, 1) + " " + localized.Hero(game.currentHero) + " EXP ( Lv " + tDigit(tempLevel) + " : <color=yellow>MAX</color> )";
        tempStr += "\n   - " + tDigit(game.offlineBonus.gold, 1) + " Gold";
        foreach (var item in game.offlineBonus.areaRewardMaterials)
        {
            tempStr += "\n   - " + tDigit(item.Value) + " " + item.Key.Name();
        }
        //tempStr += "\n<color=yellow><size=18>Calculated based on Background Gain Efficiency</color>";
        return tempStr;
    }

    public async void DelayShow()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        setFalse(gameObject);
        setActive(gameObject);
        Show();
    }
    void Show()
    {
        isShow = true;
        thisCanvasGroup.alpha = 1;
        thisCanvasGroup.interactable = true;
        thisCanvasGroup.blocksRaycasts = true;
    }
    void Hide()
    {
        isShow = false;
        thisCanvasGroup.alpha = 0;
        thisCanvasGroup.interactable = false;
        thisCanvasGroup.blocksRaycasts = false;
    }
}
