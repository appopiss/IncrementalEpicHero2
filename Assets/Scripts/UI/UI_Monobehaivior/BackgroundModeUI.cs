using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static GameController;
using static GameControllerUI;
using static Localized;
using static Main;
using TMPro;

public class BackgroundModeUI : MonoBehaviour
{
    [SerializeField] Button startButton, quitButton;
    [SerializeField] TextMeshProUGUI infoText;
    private void Start()
    {
        startButton.onClick.AddListener(() => SwitchBackgroundMode(true));
        quitButton.onClick.AddListener(() => SwitchBackgroundMode(false));
        SwitchBackgroundMode(false);
    }
    public static bool isBackgroundMode = false;
    void SwitchBackgroundMode(bool toBackground)
    {
        isBackgroundMode = toBackground;
        SetActive(gameObject, toBackground);
        SetActive(gameUI.rightCanvas, !toBackground);
        SetActive(gameUI.leftCanvas, !toBackground);
        SetActive(gameUI.frameCanvas, !toBackground);
        SetActive(gameUI.photonCanvas, !toBackground);
        SetActive(gameUI.popupCanvas, !toBackground);
        SetActive(gameUI.epicStoreCanvas, !toBackground);
        SetActive(gameUI.helpCanvas, !toBackground);
        if (isBackgroundMode) UpdateText();
    }
    public void UpdateUI()
    {
        count++;
        if (count >= 60)
        {
            UpdateText();
            count = 0;
        }
    }
    int count;
    void UpdateText()
    {
        infoText.text = InfoString();
    }
    string InfoString()
    {
        string tempStr = optStr;
        tempStr += optStr + "Total Realtime Played : " + DoubleTimeToDate(main.allTimeRealtime);
        tempStr += optStr + "\nTotal Time Played : " + DoubleTimeToDate(main.allTime);
        tempStr += optStr + "\nGuild Level : " + tDigit(game.guildCtrl.Level());
        for (int i = 0; i < 6; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            tempStr += optStr + "\n" + localized.Hero(heroKind) + " Level : " + tDigit(game.statsCtrl.HeroLevel(heroKind).value);
        }
        return tempStr;
    }
}
