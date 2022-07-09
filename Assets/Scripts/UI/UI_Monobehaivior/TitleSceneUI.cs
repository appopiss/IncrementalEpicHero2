using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static Localized;
using static SpriteSourceUI;
using static Main;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;


public partial class Save
{
    public bool isFirstWelcomed;
}
public class TitleSceneUI : MonoBehaviour
{
    Image titleImage, logoImage;
    Button button;
    public TextMeshProUGUI clickToStartText, hapiwakuText;
    public GameObject cautionObject, welcomeObject;
    public Button cautionOkayButton, discordButton, welcomeOkayButton;
    public GameObject loadingObject;
    public bool isLoaded;
    private void Awake()
    {
        titleImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        logoImage = gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
        clickToStartText = gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        hapiwakuText = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        button = logoImage.gameObject.GetComponent<Button>();
        //button.onClick.AddListener(StartGame);
        button.onClick.AddListener(() =>
        {
            if (main.S.isFirstWelcomed) StartGame();
            else SetActive(welcomeObject, true);
        }
        );//SetActive(cautionObject, true));
        button.interactable = false;
        //cautionOkayButton.onClick.AddListener(StartGame);
        //discordButton.onClick.AddListener(() => Application.OpenURL("https://discord.gg/QEpxWM2fv5"));
        welcomeOkayButton.onClick.AddListener(StartGame);
    }
    private void Start()
    {
        SetActive(gameObject, true);
        //logoImage.color = Color.clear;
        titleImage.color = Color.black;
        clickToStartText.color = Color.clear;
        hapiwakuText.color = Color.clear;
        logoImage.color = Color.white;
        logoImage.fillAmount = 0f;
    }
    public async void StartAnimation()
    {
        for (int i = 0; i < 120; i++)
        {
            //logoImage.color += Color.white / 60f;
            logoImage.fillAmount += 1 / 120f;
            //titleImage.color += Color.white / 120f;
            await UniTask.DelayFrame(1);
        }        
        logoImage.fillAmount = 1f;
        if (main.S.isToggleOn[(int)ToggleKind.BGM])
            gameUI.soundUI.bgmSource.Play();
        hapiwakuText.color = Color.white;
        await UniTask.DelayFrame(60);
        for (int i = 0; i < 120; i++)
        {
            titleImage.color += Color.white / 120f;
            await UniTask.DelayFrame(1);
        }
        await UniTask.DelayFrame(60);
        clickToStartText.color = Color.white;
        button.interactable = true;
        isLoaded = true;
    }
    async void StartGame()
    {
        main.S.isFirstWelcomed = true;
        //game.offlineBonus.GetOfflineBonus();

        if (main.isHacked) return;
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            game.battleCtrls[i].areaBattle.Start();
        }

        if (game.offlineBonus.offlineTimesec <= 10)
            game.isPause = false;
        else
        {
            if (!game.offlineBonus.isFinishedCalculation)
            {
                SetActive(loadingObject, true);
                loadingObject.GetComponent<Image>().color = Color.black * 128f / 255;
                await UniTask.WaitUntil(() => game.offlineBonus.isFinishedCalculation);
            }
        }

        SetActive(gameObject, false);
    }
}
