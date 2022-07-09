using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoverActionUI : MonoBehaviour
{
    public Action hoverAction;
    public Action onMouseEnterAction;
    public Action onMouseExitAction;
    private void OnMouseOver()
    {
        Debug.Log("a");
        if (hoverAction != null) hoverAction();
    }
    private void OnMouseEnter()
    {
        Debug.Log("b");
        if (onMouseEnterAction != null) onMouseEnterAction();
    }
    private void OnMouseExit()
    {
        Debug.Log("c");
        if (onMouseExitAction != null) onMouseExitAction();
    }
}
