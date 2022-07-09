using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UsefulMethod;

public class ActivateOnStartUI : MonoBehaviour
{
    public GameObject[] gameObjects;
    private void Awake()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            SetActive(gameObjects[i], true);
        }
    }
}
