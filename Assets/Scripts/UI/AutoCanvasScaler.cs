using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCanvasScaler : MonoBehaviour
{
    CanvasScaler canvasScaler;
    public static int width = 1920;//16
    public static int height = 1080;//9
    public float gameRatio;

    float thisRatio;
    float lastRatio;
    public float mouseCorrection = 1;
    public Vector3 defaultScreenSize { get { return Vector2.right * width + Vector2.up * height; } }

    // Start is called before the first frame update
    void Start()
    {
        gameRatio = GetRatio(width, height);
        canvasScaler = GetComponent<CanvasScaler>();
        UpdateCorrection();
        StartCoroutine(LoopCoroutine());
    }

    float GetRatio(int width, int height)
    {
        return (float)width / height;
    }

    void Judge()
    {
        thisRatio = GetRatio(Screen.width, Screen.height);
        if (thisRatio.Equals(lastRatio))
        {
            return;
        }
        lastRatio = thisRatio;

        int tempWidth = Screen.width;
        int tempHeight = Screen.height;
        bool tempIsFullscreen = Screen.fullScreen;

        //PCの画面サイズより大きい場合は修正
        if (tempWidth > Screen.currentResolution.width)
            tempWidth = Screen.currentResolution.width;
        if (tempHeight > Screen.currentResolution.height)
            tempHeight = Screen.currentResolution.height;

        if (GetRatio(tempWidth, tempHeight) >= gameRatio)
        {
            //heightに合わせる (横に長い)
            canvasScaler.matchWidthOrHeight = 1f;
            //if (!Screen.fullScreen)
            //{
            //    tempHeight = (int)(Mathf.Floor(tempHeight / 9) * 9);
            //    tempWidth = (int)(tempHeight * gameRatio);
            //}
        }
        else
        {
            //widthに合わせる (縦に長い)
            canvasScaler.matchWidthOrHeight = 0f;
            //if (!Screen.fullScreen)
            //{
            //    tempWidth = (int)(Mathf.Floor(tempWidth / 16) * 16);
            //    tempHeight = (int)(tempWidth / gameRatio);
            //}
        }

        //Screen.SetResolution(tempWidth, tempHeight, tempIsFullscreen);
        UpdateCorrection();
    }

    public void AdjustWindowSize()
    {
        int tempWidth = Screen.width;
        int tempHeight = Screen.height;
        bool tempIsFullscreen = Screen.fullScreen;
        if (tempWidth > Screen.currentResolution.width * 0.9f)
            tempWidth = (int)(Screen.currentResolution.width * 0.9f);
        if (tempHeight > Screen.currentResolution.height * 0.9f)
            tempHeight = (int)(Screen.currentResolution.height * 0.9f);
        if (GetRatio(tempWidth, tempHeight) >= gameRatio)
        {
            //heightに合わせる (横に長い)
            canvasScaler.matchWidthOrHeight = 1f;
            if (!Screen.fullScreen)
            {
                tempHeight = (int)(Mathf.Floor(tempHeight / 9) * 9);
                tempWidth = (int)(tempHeight * gameRatio);
            }
        }
        else
        {
            //widthに合わせる (縦に長い)
            canvasScaler.matchWidthOrHeight = 0f;
            if (!Screen.fullScreen)
            {
                tempWidth = (int)(Mathf.Floor(tempWidth / 16) * 16);
                tempHeight = (int)(tempWidth / gameRatio);
            }
        }
        Screen.SetResolution(tempWidth, tempHeight, tempIsFullscreen);
        UpdateCorrection();
    }

    void UpdateCorrection()
    {
        if (thisRatio >= gameRatio)
            mouseCorrection = (float)height / Screen.height;
        else
            mouseCorrection = (float)width / Screen.width;
    }

    WaitForSeconds interval = new WaitForSeconds(1f);
    IEnumerator LoopCoroutine()
    {
        while (true)
        {
            yield return interval;
            Judge();
        }
    }
}
