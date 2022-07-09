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

public class SceneUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Image image;
    TextMeshProUGUI text;
    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        image = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        SetActive(gameObject, false);
    }

    public async Task SwitchHero(HeroKind heroKind)
    {
        SetActive(gameObject, true);
        image.color = Color.black;
        text.text = "<i>Switching to " + localized.Hero(heroKind);
        for (int i = 0; i < 20; i++)
        {
            canvasGroup.alpha += 1 / 20f;
            await UniTask.DelayFrame(1);
        }
        canvasGroup.alpha = 1;
        game.Initialize();
        gameUI.Initialize();
        game.battleCtrl.areaBattle.Start();
        image.sprite = sprite.heroesPortrait[(int)heroKind];
        for (int i = 0; i < 30; i++)
        {
            image.color += Color.white / 30f;
            await UniTask.DelayFrame(1);
        }
        image.color = Color.white;
        for (int i = 0; i < 3; i++)
        {
            text.text += ".";
            await UniTask.DelayFrame(60);
        }
        gameUI.menuUI.menuButtons[0].onClick.Invoke();
        for (int i = 0; i < 30; i++)
        {
            image.color -= (Color.white - Color.black) / 30f;
            await UniTask.DelayFrame(1);
        }
        image.color = Color.black;
        for (int i = 0; i < 20; i++)
        {
            canvasGroup.alpha -= 1 / 20f;
            await UniTask.DelayFrame(1);
        }
        canvasGroup.alpha = 0;
        SetActive(gameObject, false);
    }
}
