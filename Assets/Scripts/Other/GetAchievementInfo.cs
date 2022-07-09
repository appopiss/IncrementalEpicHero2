using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
//using UniRx;

public class GetAchievementInfo// : MonoBehaviour
{
    private static readonly string MyWebAPIKey = "DFAEF99139241EFA486E81B20EA530D3";
    [SerializeField] private string appId = "1530340";//IEH1
    public double TotalPlaytimeAsSecond { get; private set; }

    public GetAchievementInfo(string appId)
    {
        this.appId = appId;
    }
    // Start is called before the first frame update
    //void Start()
    //{
    //    //Observable.Interval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
    //    //{
    //    //    Debug.Log(IsAchievementUnlocked("ACHIEVEMENT_PHASE3"));
    //    //});
    //}


    public async UniTask<bool> IsAchievementUnlocked(string apiName)
    {
        var result = await GetAchievementsInfo();
        var achievement = result.Where(_ => _.apiName == apiName).Single();
        //Debug.Log(achievement.apiName);
        //Debug.Log(achievement.achieved);
        if (achievement == null) return false;
        return achievement.achieved == 1;
    }

    private async UniTask<IEnumerable<Info>> GetAchievementsInfo()
    {
        CSteamID userId = SteamUser.GetSteamID();
        ulong id = userId.m_SteamID;
        var url = @"https://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/?key="
        + MyWebAPIKey + "&steamid=" + id + "&appid=" + appId + "&format=json";
        var result = await SteamIAP.httpClient.GetAsync(url).AsUniTask();
        string s = await result.Content.ReadAsStringAsync();
        JObject jObject = JObject.Parse(s);
        if (jObject["playerstats"]["achievements"] == null) return null;
        string games = jObject["playerstats"]["achievements"].ToString();
        var resultArray = JsonConvert.DeserializeObject<List<Info>>(games);
        return resultArray;
    }
    public async UniTask<int> AchievementsAchievedCount()
    {
        if (!SteamManager.Initialized) return 0;
        var result = await GetAchievementsInfo();
        if (result == null) return 0;
        return result.Where(_=>_.achieved == 1).Count();
    }
}

class Info
{
    public string apiName { get; set; }
    public int achieved { get; set; }
    public long unlockTime { get; set; }
}
