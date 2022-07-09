using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using Cysharp.Threading.Tasks;
using TMPro;

public class SwitchCanvasUI
{
    public SwitchCanvasUI(GameObject[] showCanvasObjects, Button[] buttons, bool isFadeIn = false, bool isGoUp = false, bool isTextColorChange = false, bool isFirstOpen = false)
    {
        openCloseUIs = new OpenCloseUI[showCanvasObjects.Length];
        for (int i = 0; i < showCanvasObjects.Length; i++)
        {
            int count = i;
            openCloseUIs[count] = new OpenCloseUI(showCanvasObjects[count], isFadeIn, isGoUp, isTextColorChange, isFirstOpen);
            for (int j = 0; j < buttons.Length; j++)
            {
                int countJ = j;
                if (countJ == count) openCloseUIs[count].SetOpenButton(buttons[countJ]);
                else openCloseUIs[count].SetCloseButton(buttons[countJ]);
            }
        }
    }
    public OpenCloseUI[] openCloseUIs;
}


public class OpenCloseUI
{
    public OpenCloseUI(GameObject showObject, bool isFadeIn = false, bool isGoUp = false, bool isTextColorChange = false, bool isFirstOpen = false)
    {
        this.isFadeIn = isFadeIn;
        this.isGoUp = isGoUp;
        this.isTextColorChange = isTextColorChange;
        this.isFirstOpen = isFirstOpen;
        Awake(showObject);
    }

    List<Button> openButtons = new List<Button>();
    List<Button> closeButtons = new List<Button>();
    CanvasGroup thisCanvasGroup;
    RectTransform thisRect;
    float initPosition;
    Vector2 initVect;
    public List<Action> openActions = new List<Action>();
    public List<Action> closeActions = new List<Action>();
    public bool isOpen;
    public bool isFirstOpen = false;
    public bool isFadeIn = false;
    public bool isGoUp = false;
    public bool isTextColorChange = false;
    int smoothness = 5;
    int animationFrame = 5;

    public void SetOpenButton(Button button)
    {
        openButtons.Add(button);
        button.onClick.AddListener(Open);
    }
    public void SetCloseButton(Button button)
    {
        closeButtons.Add(button);
        button.onClick.AddListener(Close);
    }
    public void SetCloseButton(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int count = i;
            closeButtons.Add(buttons[count]);
            buttons[count].onClick.AddListener(Close);
        }
    }

    public async void Open()
    {
        if (isOpen) return;
        isOpen = true;

        if (isFadeIn) Fade(true, smoothness);
        if (isGoUp) GoUp(true, smoothness);
        if (isFadeIn || isGoUp) await UniTask.DelayFrame(animationFrame, PlayerLoopTiming.FixedUpdate);
        Activate(true);
        if (isTextColorChange)
        {
            openButtons[0].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            for (int i = 0; i < closeButtons.Count; i++)
            {                
                closeButtons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
        }
        for (int i = 0; i < openActions.Count; i++)
        {
            if (openActions[i] != null) openActions[i]();
        }
    }
    public async void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        if (isFadeIn) Fade(false, smoothness);
        if (isGoUp) GoUp(false, smoothness);
        if (isFadeIn || isGoUp) await UniTask.DelayFrame(animationFrame, PlayerLoopTiming.FixedUpdate);
        Activate(false);
        for (int i = 0; i < closeActions.Count; i++)
        {
            if (closeActions[i] != null) closeActions[i]();
        }
    }

    void Activate(bool isActivate)
    {
        thisCanvasGroup.alpha = Convert.ToInt32(isActivate);
        thisCanvasGroup.interactable = isActivate;
        thisCanvasGroup.blocksRaycasts = isActivate;
        thisRect.anchoredPosition = initVect;
    }
    async void Fade(bool isFadeIn, int smoothness)
    {
        thisCanvasGroup.alpha = 0.25f * (1 + 3 * Convert.ToInt32(!isFadeIn));
        for (int i = 0; i < smoothness; i++)
        {
            thisCanvasGroup.alpha += 0.75f / smoothness * Mathf.Pow(-1, Convert.ToInt32(!isFadeIn));
            await UniTask.DelayFrame(animationFrame / smoothness, PlayerLoopTiming.FixedUpdate);
        }
    }
    async void GoUp(bool isGoUp, int smoothness)
    {
        if (isGoUp) thisRect.anchoredPosition = initVect + thisRect.sizeDelta.y * (1f / 10f) * Vector2.down;
        for (int i = 0; i < smoothness; i++)
        {
            thisRect.anchoredPosition += thisRect.sizeDelta.y * (1f / 10f) * Vector2.up / smoothness * Mathf.Pow(-1, Convert.ToInt32(!isGoUp)) / Main.main.timescale;
            await UniTask.DelayFrame(animationFrame / smoothness, PlayerLoopTiming.FixedUpdate);
        }
        if(isGoUp) thisRect.anchoredPosition = initVect;
    }

    public void Awake(GameObject gameObject)//外部から呼ぶ
    {
        thisCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        thisRect = gameObject.GetComponent<RectTransform>();
        initVect = thisRect.anchoredPosition;

        if (isFirstOpen) Open();
        else Activate(false);
    }

}