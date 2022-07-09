using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SwitchTabUI
{
    public SwitchTabUI(Button[] buttons, bool isTextColorChange = false, int firstOpenId = -1, Action openAction = null, Action<int> openActionWithId = null)
    {
        this.buttons.AddRange(buttons);
        this.isTextColorChange = isTextColorChange;
        for (int i = 0; i < this.buttons.Count; i++)
        {
            int count = i;
            this.buttons[count].onClick.AddListener(() => Open(count));
        }
        if (openAction != null) this.openAction = openAction;
        if (openActionWithId != null) this.openActionWithId = openActionWithId;
        if (firstOpenId != -1)
        {
            this.firstOpenId = firstOpenId;
            this.buttons[this.firstOpenId].onClick.Invoke();
        }
    }
    public List<Button> buttons = new List<Button>();
    public bool isTextColorChange;
    public virtual int currentId { get; set; }
    public int firstOpenId = 0;//最初に選択しておくボタンの番号
    public Action openAction;
    public Action<int> openActionWithId;
    public bool isNotTextMeshPro;
    void Open(int id)
    {
        currentId = id;
        if (isTextColorChange)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Color color = i == id ? Color.yellow : Color.white;
                if (isNotTextMeshPro) buttons[i].gameObject.GetComponentInChildren<Text>().color = color;
                else buttons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;
            }
        }
        if (openAction != null) openAction();
        if (openActionWithId != null) openActionWithId(id);
    }
}
