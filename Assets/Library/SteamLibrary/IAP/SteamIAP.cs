#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Net.Http;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class SteamIAP
{
    //private void Start()
    //{
    //    InAppPurchase inAppPurchase = GameController.game.inAppPurchaseCtrl.inAppPuchaseList[0];
    //    Initialize(inAppPurchase.id.ToString(), inAppPurchase.priceCent, inAppPurchase.description, inAppPurchase.successAction);
    //}
    string itemId = "";
    string amount = "";
    string description = "";
    Action OnApproved = () => { };
    public void Initialize(Button purchaseButton, string itemId, string amount, string description, Action OnApproved)
    {
        this.itemId = itemId;
        this.amount = amount;
        this.description = description;
        this.OnApproved = OnApproved;
        purchaseButton.onClick.AddListener(Buy);
    }

#if !DISABLESTEAMWORKS
    //??????SteamWebAPIKey?????????????B
    public static readonly string SteamWebAPIKey = "B28EB1C319D81B6009B437FDF042264F";
    //?f?|??ID?????????????B
    public static readonly string appId = "1690710";
    /*
    //Steam?????F?R?[???o?b?N?????BOnUserRespondedToTxn???????????????o?^?????????????B
    private Callback<MicroTxnAuthorizationResponse_t> responseCallback;
    async void OnUserRespondedToTxn(MicroTxnAuthorizationResponse_t txn_callback)
    {
        if (txn_callback.m_bAuthorized == 1)
        {
            //?Q?[?????????????????????????B
            var parameters_purshaceComplete = new Dictionary<string, string>()
             {
            {"key", SteamWebAPIKey },
            {"orderid", txn_callback.m_ulOrderID.ToString() },
            {"appid", appId },
              };
            var result_purchaseComplete = await httpClient.PostAsync($"https://partner.steam-api.com/ISteamMicroTxnSandbox/FinalizeTxn/v2/"
                , new FormUrlEncodedContent(parameters_purshaceComplete));
            //result = OK?????????w?????????????????A?A?C?e?????t?^???????B
            string str_purshaceComplete = await result_purchaseComplete.Content.ReadAsStringAsync();
            JObject jObject_purchaseComplete = JObject.Parse(str_purshaceComplete);
            string isPurchaseCompletedStr = jObject_purchaseComplete["response"]["result"].ToString();
            if (isPurchaseCompletedStr == "OK")
            {
                isPurchaseApprovedBySteam = 1;
            }
            else
            {
                isPurchaseApprovedBySteam = -1;
            }
        }
        else
        {
            isPurchaseApprovedBySteam = -1;
        }
    }
    */
    //?I?[?o?[???C?????Q?[?????????????????R?[???o?b?N?????B
    private Callback<GameOverlayActivated_t> overlayCallback;
    void OnOverlayAction(GameOverlayActivated_t overlay_callback)
    {
        isOverlay = overlay_callback.m_bActive == 1 ? true : false;
    }
    static bool isOverlay;
    public static bool isTxn;
    //1 ... ???????????????B 2... ???????????????B
    public static int isPurchaseApprovedBySteam = 0;
    //?????????p?l?????\???????N???b?N?????????????????????B
    public GameObject ClickBlockPanel;

    //HttpClient
    public static HttpClient httpClient = new HttpClient();
    //?{?^?????????????????????????B
    async void Buy()
    {
        isTxn = true;
        isPurchaseApprovedBySteam = 0;
        //SteamId????
        CSteamID userId = SteamUser.GetSteamID();
        ulong id = userId.m_SteamID;
        //?????????????????B
        string language = SteamApps.GetCurrentGameLanguage();
        //var result = await httpClient.GetAsync(@"https://partner.steam-api.com/ISteamMicroTxnSandbox/GetUserInfo/v2/?key=" + SteamWebAPIKey
        //    + "&steamid=" + id).AsUniTask();
        var result = await httpClient.GetAsync(@"https://partner.steam-api.com/ISteamMicroTxn/GetUserInfo/v2/?key=" + SteamWebAPIKey
    + "&steamid=" + id).AsUniTask();
        string s = await result.Content.ReadAsStringAsync();
        JObject jObject = JObject.Parse(s);
        //?????????B
        string currency = jObject["response"]["params"]["currency"].ToString();
        Debug.Log(currency);
        //???????????A?B?????????????B
        //Debug.Log(jObject["response"]["params"]["state"]);
        //???????O?????????????????????B
        //Debug.Log(jObject["response"]["params"]["country"]);
        //???????L??id?????????????B
        int orderId = UnityEngine.Random.Range(1, int.MaxValue);
        //Debug.Log(SteamWebAPIKey);
        //Debug.Log(orderId.ToString());
        //Debug.Log(id.ToString());
        //Debug.Log(appId);
        //Debug.Log(1.ToString());
        //Debug.Log(itemId);
        //Debug.Log(1.ToString());
        //Debug.Log(amount);
        //Debug.Log(description);
        var parameters = new Dictionary<string, string>()
            {
            {"key", SteamWebAPIKey },
            {"orderid", orderId.ToString() },
            {"steamid", id.ToString() },
            {"appid", appId },
            {"itemcount", 1.ToString() },
            {"language", "en" },
            {"currency", "USD" },
            {"itemid[0]", itemId },
            {"qty[0]", 1.ToString() },
            {"amount[0]", amount },
            {"description[0]", description },
            };
        var content = new FormUrlEncodedContent(parameters);
        Debug.Log(content);
        try
        {
            //var result2 = await httpClient.PostAsync($"https://partner.steam-api.com/ISteamMicroTxnSandbox/InitTxn/v3/"
            //    , content);
            var result2 = await httpClient.PostAsync($"https://partner.steam-api.com/ISteamMicroTxn/InitTxn/v3/"
    , content);

        }
        catch (TaskCanceledException e)
        {
            isPurchaseApprovedBySteam = -1;
        }

        CheckCountSec();
        await UniTask.WaitUntil(() => isPurchaseApprovedBySteam != 0);
        if (isPurchaseApprovedBySteam == -1)
        {
            isTxn = false;
            var confirm = new ConfirmUI(GameControllerUI.gameUI.popupCtrlUI.defaultConfirm);
            confirm.SetUI("Failed.\nPlease try again after a while.");
            confirm.okButton.onClick.RemoveAllListeners();
            confirm.okButton.onClick.AddListener(() => confirm.Hide());
            return;
        }
        else if (isPurchaseApprovedBySteam == 1)
        {
            OnApproved();
        }
        isTxn = false;
    }

    async void CheckCountSec()
    {
        //10秒たってもクライアントが起動しなかったら、メッセージを表示
        for (int i = 0; i < 10; i++)
        {
            await UniTask.Delay(1000);
            if (isPurchaseApprovedBySteam != 0)
            {
                return;
            }
        }
        if (isPurchaseApprovedBySteam == 0)
        {
            GameControllerUI.gameUI.epicStorUI.inAppFailedText.gameObject.SetActive(true);
            //isPurchaseApprovedBySteam = -1;
        }
    }
#else
    void Buy() { }
#endif

}

