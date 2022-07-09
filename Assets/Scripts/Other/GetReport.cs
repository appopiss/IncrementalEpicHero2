using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Steamworks;
using System;

//Steamでの課金購入情報を取得するクラス
public class GetReport : MonoBehaviour
{
    bool isGotReport;
    public void RestoreAction()
    {
        if (isTryGetReport) return;
        isTryGetReport = true;
        GameController.game.inAppPurchaseCtrl.Restore(inAppPurchasedNum);
        isTryGetReport = false;
    }
    public void GetInAppPurchaseReport()
    {
        if (isGotReport) return;
        StartCoroutine(GetReportRequest());
        isGotReport = true;
    }
    public bool CanRestore()
    {
        return GameController.game.inAppPurchaseCtrl.CanRestore(inAppPurchasedNum);
    }

    long[] inAppPurchasedNum = new long[Enum.GetNames(typeof(InAppPurchaseKind)).Length];
    bool isTryGetReport;
    IEnumerator GetReportRequest()
    {
        if (isTryGetReport) yield break;
        isTryGetReport = true;

        CSteamID userId = SteamUser.GetSteamID();

        var ReportCount = 1000;
        var ReportStartTime = ToRfc3339String(new DateTime(2022, 07, 01, 00, 00, 00), DateTimeKind.Local);

        for (int i = 0; i < inAppPurchasedNum.Length; i++)
        {
            inAppPurchasedNum[i] = 0;
        }

        while (ReportCount >= 1000)
        {
            UnityWebRequest request = GetRequest(ReportStartTime);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                break;
            }
            else
            {

                var respons = JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
                //Debug.Log(respons.response.@params.orders[respons.response.@params.count - 1].timecreated);

                ReportCount = respons.response.@params.count;

                for (int i = 0; i < ReportCount; i++)
                {
                    if (respons.response.@params.orders[i].steamid == userId.ToString())
                    {
                        for (int t = 0; t < respons.response.@params.orders[i].items.Count; t++)
                        {
                            if (respons.response.@params.orders[i].status == "Succeeded")
                            {
                                inAppPurchasedNum[respons.response.@params.orders[i].items[t].itemid - 100]++;//-100を追加した
                            }
                        }

                    }
                }

                //追加
                if (respons.response.@params.count < 1)
                {
                    Debug.Log("Purchase Count 0");
                    yield break;
                }
                //追加ここまで
                var nt = respons.response.@params.orders[respons.response.@params.count - 1].time;
                ReportStartTime = ToRfc3339String(new DateTime(nt.Year, nt.Month, nt.Day, nt.Hour, nt.Minute, nt.AddSeconds(1).Second), DateTimeKind.Unspecified);
            }
        }
        isTryGetReport = false;
    }

    public UnityWebRequest GetRequest(string time)
    {
        string url = string.Format("https://partner.steam-api.com/ISteamMicroTxn/GetReport/v4/?key={0}&appid={1}&time={2}", SteamIAP.SteamWebAPIKey, SteamIAP.appId, time);
        UnityWebRequest request = UnityWebRequest.Get(url);
        Debug.Log(url);
        return request;
    }

    public string ToRfc3339String(DateTime dt, DateTimeKind kind)
    {
        return System.Xml.XmlConvert.ToString(new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, kind), System.Xml.XmlDateTimeSerializationMode.Utc);
    }

    //public void SetItemCount()
    //{
    //    for (int i = 0; i < inAppPurchasedNum.Length; i++)
    //    {
    //        int count = i;
    //        //EpicCoinを調整する処理
    //        for (int j = 0; j < GameController.game.inAppPurchaseCtrl.; j++)
    //        {

    //        }
            
    //        //InAppPurchaseのGetEpicCoin()を呼ぶ。
    //        //InAppPurchaseでmain.S.inAppPurchaseNumを参照できるようにする
    //    }
    //}
}

public class Root
{
    public Response response { get; set; }
}

public class Response
{
    public string result { get; set; }
    public Params @params { get; set; }
}

public class Params
{
    public int count { get; set; }
    public List<Order> orders { get; set; }
}

public class Order
{
    public string orderid { get; set; }
    public string transid { get; set; }
    public string steamid { get; set; }
    public string status { get; set; }
    public string currency { get; set; }
    public DateTime time { get; set; }
    public string country { get; set; }
    public string usstate { get; set; }
    public DateTime timecreated { get; set; }
    public List<Item> items { get; set; }
}


public class Item
{
    public int itemid { get; set; }
    public int qty { get; set; }
    public int amount { get; set; }
    public int vat { get; set; }
    public string itemstatus { get; set; }
}
