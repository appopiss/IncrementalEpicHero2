#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UniRx;

public class GetOwnerShip// : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    [SerializeField] string publisherKey = "F6724F6EF14E3AE95B7B7F14E53BEEC3";//"2D7C0084A8F45AE79A3EEAF167EA9D4F";
    [SerializeField] private string appId = "1580930";//IEH2SupportPack

    public GetOwnerShip(string publisherKey, string appId)
    {
        this.publisherKey = publisherKey;
        this.appId = appId;
        Initialize();
    }
    //private void Awake()
    //{
    //    Initialize();
    //}

    public bool isGotInfo;
    public async void Initialize()
    {
        await UniTask.WaitUntil(() => SteamManager.Initialized);
        var result = await GetIsAppOwned();
        Debug.Log(result);
        isOwn = result;
        isGotInfo = true;
    }
    public bool isOwn;

    private async UniTask<bool> GetIsAppOwned()
    {
        CSteamID userId = SteamUser.GetSteamID();
        ulong id = userId.m_SteamID;
        var result = await SteamIAP.httpClient.GetAsync(@"https://partner.steam-api.com/ISteamUser/CheckAppOwnership/v2/?key="
        + publisherKey + "&steamid=" + id + "&appid=" + appId + "&format=json").AsUniTask();
        string s = await result.Content.ReadAsStringAsync();
        JObject jObject = JObject.Parse(s);
        string own = jObject["appownership"]["ownsapp"].ToString();
        var ownership = own == "True" ? true : false;
        return ownership;
    }

#endif
}
