using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public partial class Save
{
    public bool isReceivedIEH2PlaytestBonus;
}

public class IEH2PlaytestBonusUI : MonoBehaviour
{
    [SerializeField] Button openButton, quitButton;
    [SerializeField] TextMeshProUGUI receivedText;
    public OpenCloseUI openCloseUI;

    bool isChecked;
    bool isTrying;
    int achievementNum = -1;
    async void CheckBonus()
    {
        if (main.S.isReceivedIEH2PlaytestBonus)
        {
            receivedText.text = "You have already received.";
            return;
        }
        openButton.interactable = false;
        quitButton.interactable = false;
        string tempStr = "";
        if (!isChecked)
        {
            //Serverから情報を取得
            if (isTrying)
            {
                quitButton.interactable = true;
                return;
            }
            isTrying = true;
            receivedText.text = "Data is being acquired...";
            //SteamAchievement
            if (!SteamManager.Initialized)
            {
                receivedText.text = "Steam Server doesn't work currently. Please reboot the game and try it again.";
                openButton.interactable = true;
                quitButton.interactable = true;
                return;
            }
            CheckTimeout();
            achievementNum = await game.getAchievementInfoIEH2Playtest.AchievementsAchievedCount();
            if (achievementNum < 1)
            {
                receivedText.text = "Failed.\nPlease check followings:\nSteam Profile > Edit Profile > Privacy Settings > Game Details : [Public]";
                openButton.interactable = true;
                quitButton.interactable = true;
                return;
            }
            isTrying = false;
            isChecked = true;
        }
        //実際に獲得する処理
        double epicCoin = 100 * achievementNum;
        double portalOrb = achievementNum;
        receivedText.text = "You have received:\n<color=green><sprite=\"EpicCoin\" index=0> " + tDigit(epicCoin) + " Epic Coin\n" + tDigit(portalOrb) + " Portal Orb</color>";
        game.epicStoreCtrl.epicCoin.Increase(epicCoin);
        game.areaCtrl.portalOrb.Increase(portalOrb);
        main.S.isReceivedIEH2PlaytestBonus = true;
        quitButton.interactable = true;
        openButton.interactable = true;
    }

    async void CheckTimeout()
    {
        for (int i = 0; i < 60; i++)//60秒
        {
            await UniTask.Delay(1000);
            if (achievementNum >= 0) return;
        }
        receivedText.text = "Failed.\nPlease check followings:\nSteam Profile > Edit Profile > Privacy Settings > Game Details : [Public]";
        //openButton.interactable = true;
        quitButton.interactable = true;
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(quitButton);

        openCloseUI.openActions.Add(CheckBonus);
    }
}
