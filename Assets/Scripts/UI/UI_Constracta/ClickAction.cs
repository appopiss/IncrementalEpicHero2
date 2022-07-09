using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickAction
{
    public ClickAction(GameObject gameObject, Action leftClickAction = null, Action rightClickAction = null)
    {
        this.gameObject = gameObject;
        this.leftClickAction = leftClickAction;
        this.rightClickAction = rightClickAction;

        if (gameObject.GetComponent<Button>() != null)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => leftClickAction());
            var click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerUp;
            click.callback.AddListener((x) =>
            {
                if (Input.GetMouseButtonUp(1))
                {
                    if (rightClickAction != null)
                        rightClickAction();
                }
            });
            if (gameObject.GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>().triggers.Add(click);
            else gameObject.GetComponent<EventTrigger>().triggers.Add(click);
        }
        else
        {
            var click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerUp;
            click.callback.AddListener((x) => Action());
            if (gameObject.GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>().triggers.Add(click);
            else gameObject.GetComponent<EventTrigger>().triggers.Add(click);
        }
    }
    void Action()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (rightClickAction != null)
                rightClickAction();
        }
        else
        {
            if (leftClickAction != null)
                leftClickAction();
        }
    }
    GameObject gameObject;
    Action leftClickAction, rightClickAction;
}
