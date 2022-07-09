using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public partial class Save
{
    public int version;

    public string lastServerTimeGotServerTime;//以前のゲーム起動時にServerTimeを取得した時のServerTime
    public string lastLocalTimeGotServerTime;//以前のゲーム起動時にServerTimeを取得した時のLocalTime
    //public string lastTimeServer;//SteamServerのTime
    public string lastTimeLocal;//LocalのTime
    public string birthDate;
    //public bool isContinuePlay;
    public double allTime;
    public double allTimeRealtime;
    public double allTimeWorldAscension;//今回のWorldAscensionでの時間s
    public string lastDailyTime;//最後にDailyActionをした日付

    public float SEVolume;
    public float BGMVolume;

    //暗号化
    public double wasd;
}