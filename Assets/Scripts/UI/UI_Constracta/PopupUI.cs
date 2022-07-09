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

public class PopupUI
{
    public GameObject thisObject;
    public RectTransform thisRect;
    public CanvasGroup thisCanvasGroup;

    public bool isShow;// { get => thisCanvasGroup.alpha }
    public Func<bool> additionalShowCondition;
    //public Action additionalActionUpdate;
    public bool isPreventSetCorner;

    public PopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null)
    {
        if (additionalShowCondition != null) this.additionalShowCondition = additionalShowCondition;
        this.thisObject = thisObject;
        thisRect = thisObject.GetComponent<RectTransform>();
        thisCanvasGroup = thisObject.GetComponent<CanvasGroup>();
        Hide();
        CheckMouseOver();
    }
    private async void CheckMouseOver()
    {
        while (true)
        {
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
            if (isShow)
            {
                if (currentShowObjectId < 0) Hide();
                if (currentShowObjectId >= 0 && currentShowObjectId < targetObjectList.Count && !targetObjectList[Math.Max(0, currentShowObjectId)].activeInHierarchy) Hide();
                //float correction = gameUI.autoCanvasScaler.mouseCorrection;
                //Vector3 position = Input.mousePosition * correction;
                //if (position.x <= 150f) Hide();
                //if (position.y <= 0) Hide();
                //if (position.x >= gameUI.autoCanvasScaler.defaultScreenSize.x) Hide();
                //if (position.y >= gameUI.autoCanvasScaler.defaultScreenSize.y) Hide();
                if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape)) Hide();
            }
        }
    }
    public List<GameObject> targetObjectList = new List<GameObject>();
    public int currentShowObjectId = -1;
    public void SetId(int id) { currentShowObjectId = id; }
    public virtual void SetTargetObject(GameObject targetObject, Action showAction = null)
    {
        //子要素のUIも全てチェックする
        if (targetObject.transform.childCount > 0)
        {
            //foreach (var item in targetObject.GetComponentsInChildren<Image>())
            //{
            //    if (item.raycastTarget)
            //        SetTargetObjectAction(item.gameObject, showAction);
            //}
            foreach (var item in targetObject.GetComponentsInChildren<Button>())
            {
                SetTargetObjectAction(item.gameObject, showAction);
                //Buttonの場合は、クリックした時にテキストの長さが変更されるケースがあるため、PopupWindowを更新する
                item.GetComponent<Button>().onClick.AddListener(ResizeWindow);
            }
            foreach (var item in targetObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                //item.raycastTarget = true;
                if (item.raycastTarget)
                    SetTargetObjectAction(item.gameObject, showAction);
            }
        }
        SetTargetObjectAction(targetObject, showAction);
        if (targetObject.GetComponent<Button>() != null) targetObject.GetComponent<Button>().onClick.AddListener(ResizeWindow);
    }
    public void SetTargetObjectAction(GameObject targetObject, Action showAction = null)
    {
        if (targetObject.GetComponent<EventTrigger>() == null) targetObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        int tempId = targetObjectList.Count;
        entry.callback.AddListener((data) => { SetId(tempId); if (showAction != null) showAction(); else DelayShow(); });
        exit.callback.AddListener((data) => { SetId(-1); DelayHide(); });
        targetObject.GetComponent<EventTrigger>().triggers.Add(entry);
        targetObject.GetComponent<EventTrigger>().triggers.Add(exit);
        targetObjectList.Add(targetObject);
    }
    public void SetTargetObject(GameObject targetObject, Func<string> showString, Func<bool> showCondition = null)
    {
        //子要素のUIも全てチェックする
        if (targetObject.transform.childCount > 0)
        {
            //foreach (var item in targetObject.GetComponentsInChildren<Image>())
            //{
            //    if (item.raycastTarget)
            //        SetTargetObjectAction(item.gameObject, showString);
            //}
            foreach (var item in targetObject.GetComponentsInChildren<Button>())
            {
                SetTargetObjectAction(item.gameObject, showString, showCondition);
                //Buttonの場合は、クリックした時にテキストの長さが変更されるケースがあるため、PopupWindowを更新する
                item.GetComponent<Button>().onClick.AddListener(ResizeWindow);
            }
            foreach (var item in targetObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                //item.raycastTarget = true;
                if (item.raycastTarget)
                    SetTargetObjectAction(item.gameObject, showString, showCondition);
            }
        }
        SetTargetObjectAction(targetObject, showString, showCondition);
        if (targetObject.GetComponent<Button>() != null) targetObject.GetComponent<Button>().onClick.AddListener(ResizeWindow);
    }
    public void SetTargetObjectAction(GameObject targetObject, Func<string> showString, Func<bool> showCondition)
    {
        if (targetObject.GetComponent<EventTrigger>() == null) targetObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        int tempId = targetObjectList.Count;
        entry.callback.AddListener((data) =>
        {
            if (showCondition != null && !showCondition())
                return;
            SetId(tempId);
            SetUI(showString);
        });
        exit.callback.AddListener((data) => { SetId(-1); DelayHide(); });
        targetObject.GetComponent<EventTrigger>().triggers.Add(entry);
        targetObject.GetComponent<EventTrigger>().triggers.Add(exit);
        targetObjectList.Add(targetObject);
    }

    async void ResizeWindow()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        DelayShow();
    }

    public TextMeshProUGUI thisText;
    Func<string> textString;
    public virtual void SetUI(Func<string> textString)
    {
        thisText = thisObject.GetComponentInChildren<TextMeshProUGUI>();
        this.textString = textString;
        UpdateText(textString());
        DelayShow();
    }
    public void UpdateText(string textString)
    {
        thisText.text = textString;
    }

    public async void DelayShow()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        setFalse(thisObject);
        setActive(thisObject);
        Show();
    }
    public void DelayHide()
    {
        Hide();
    }
    public virtual void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        if (textString != null) UpdateText(textString());
        SetMouseFollow();
        //SetCorner();
    }
    public virtual void Update() { }
    void Show()
    {
        if (additionalShowCondition != null && !additionalShowCondition())
        {
            Hide();
            return;
        }
        isShow = true;
        thisCanvasGroup.alpha = 1;
    }
    void Hide()
    {
        isShow = false;
        thisCanvasGroup.alpha = 0;
        SetId(-1);
    }
    public void CheckMousePosition(Vector3 position)
    {
        if (position.x <= 150f && position.y < gameUI.autoCanvasScaler.defaultScreenSize.y - 100f) Hide();
        if (position.y <= 0) Hide();
        if (position.x >= gameUI.autoCanvasScaler.defaultScreenSize.x) Hide();
        if (position.y >= gameUI.autoCanvasScaler.defaultScreenSize.y) Hide();
    }
    public virtual void SetMouseFollow()
    {
        //Windowサイズが規定オーバーの場合は強制的にSetCornerにする
        if (!isPreventSetCorner && (thisRect.sizeDelta.x >= gameUI.autoCanvasScaler.defaultScreenSize.x * 0.5f || thisRect.sizeDelta.y >= gameUI.autoCanvasScaler.defaultScreenSize.y * 0.5f))
        {
            SetCorner();
            return;
        }

        thisRect.anchorMin = Vector2.one * 0.5f;
        thisRect.anchorMax = Vector2.one * 0.5f;
        thisRect.pivot = Vector2.one * 0.5f;
        float correction = gameUI.autoCanvasScaler.mouseCorrection;
        Vector3 position = Input.mousePosition * correction;
        CheckMousePosition(position);
        position -= gameUI.autoCanvasScaler.defaultScreenSize * 0.5f;
        if (position.y >= 0 && position.x >= 0)//第一象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x, -thisRect.sizeDelta.y) * 0.6f;
        else if (position.y >= 0 && position.x < 0)//第二象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x, -thisRect.sizeDelta.y) * 0.6f;
        else if (position.y < 0 && position.x >= 0)//第四象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x, thisRect.sizeDelta.y) * 0.6f;
        else if (position.y < 0 && position.x < 0)//第三象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x, thisRect.sizeDelta.y) * 0.6f;
    }
    public virtual void SetCorner()
    {
        float correction = gameUI.autoCanvasScaler.mouseCorrection;
        Vector3 position = Input.mousePosition;
        CheckMousePosition(position * correction);
        if (position.x < Screen.width / 2)
        {
            if (position.y < Screen.height * (1 / 3f))
            {
                thisRect.anchorMin = Vector2.right;
                thisRect.anchorMax = Vector2.right;
                thisRect.pivot = Vector2.right;
                thisRect.anchoredPosition = -Vector2.right * 80f + Vector2.up * 80f;
            }
            else
            {
                thisRect.anchorMin = Vector2.one;
                thisRect.anchorMax = Vector2.one;
                thisRect.pivot = Vector2.one;
                thisRect.anchoredPosition = -Vector2.one * 80f;
            }
        }
        else
        {
            if (position.y < Screen.height * (1 / 3f))
            {
                thisRect.anchorMin = Vector2.zero;
                thisRect.anchorMax = Vector2.zero;
                thisRect.pivot = Vector2.zero;
                thisRect.anchoredPosition = Vector2.one * 80f;
            }
            else
            {
                thisRect.anchorMin = Vector2.up;
                thisRect.anchorMax = Vector2.up;
                thisRect.pivot = Vector2.up;
                thisRect.anchoredPosition = Vector2.right * 80f + Vector2.down * 80f;
            }
        }
    }
}

public class ConfirmUI
{
    public GameObject thisObject;
    public Button okButton, quitButton;
    public RectTransform thisRect;
    public CanvasGroup thisCanvasGroup;

    public bool isShow;

    public ConfirmUI(GameObject thisObject)
    {
        this.thisObject = thisObject;
        quitButton = thisObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        thisRect = thisObject.GetComponent<RectTransform>();
        thisCanvasGroup = thisObject.GetComponent<CanvasGroup>();
        if (thisObject.transform.childCount >= 4 && thisObject.transform.GetChild(3).gameObject.GetComponent<Button>() != null)
        {
            okButton = thisObject.transform.GetChild(3).gameObject.GetComponent<Button>();
            SetActive(okButton.gameObject, true);
            okButton.onClick.RemoveAllListeners();
        }
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(Hide);
        Hide();
    }
    public virtual void SetUI(string textString)
    {
        thisObject.GetComponentInChildren<TextMeshProUGUI>().text = textString;
        DelayShow();
    }
    public async void DelayShow()
    {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        setFalse(thisObject);
        setActive(thisObject);
        Show();
    }
    public virtual void UpdateUI()
    {
        if (!isShow) return;
    }
    public void Show()
    {
        isShow = true;
        thisCanvasGroup.alpha = 1;
        thisCanvasGroup.interactable = true;
        thisCanvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        isShow = false;
        thisCanvasGroup.alpha = 0;
        thisCanvasGroup.interactable = false;
        thisCanvasGroup.blocksRaycasts = false;
    }
}
