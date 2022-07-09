using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UsefulMethod;
using Cysharp.Threading.Tasks;


public enum PlatformKind
{
    Steam,
}

public class Main : MonoBehaviour
{
    public PlatformKind platform;
    public readonly static int version = 00030104;//00.00.01.00
    public double allTime { get => S.allTime; set => S.allTime = value; }
    public double allTimeWorldAscension { get => S.allTimeWorldAscension; set => S.allTimeWorldAscension = value; }
    public double allTimeRealtime { get => S.allTimeRealtime; set => S.allTimeRealtime = value; }
    public DateTime birthTime
    {
        get { return DateTime.FromBinary(Convert.ToInt64(S.birthDate)); }
        set { S.birthDate = value.ToBinary().ToString(); }
    }
    [NonSerialized]
    public static DateTime releaseTime = DateTime.Parse("7/22/2022 6:00:00 AM");
    //public DateTime lastTimeServer//最後にプレイしたサーバー時間。
    //{
    //    get { return DateTime.FromBinary(Convert.ToInt64(S.lastTimeServer)); }
    //    set { S.lastTimeServer = value.ToBinary().ToString(); }// Debug.Log(value.ToString()); }
    //}
    public DateTime lastServerTimeGotServerTime//以前のゲーム起動時にServerTimeを取得した時のServerTime
    {
        get {
            if (S.lastServerTimeGotServerTime == "") return DateTime.Now;
            return DateTime.FromBinary(Convert.ToInt64(S.lastServerTimeGotServerTime)); }
        set { S.lastServerTimeGotServerTime = value.ToBinary().ToString(); }// Debug.Log(value.ToString()); }
    }
    public DateTime lastLocalTimeGotServerTime//以前のゲーム起動時にServerTimeを取得した時のLocalTime
    {
        get {
            if (S.lastLocalTimeGotServerTime == "") return DateTime.Now;
            return DateTime.FromBinary(Convert.ToInt64(S.lastLocalTimeGotServerTime)); }
        set { S.lastLocalTimeGotServerTime = value.ToBinary().ToString(); }// Debug.Log(value.ToString()); }
    }
    public DateTime lastTimeLocal//最後にプレイしたローカル時間
    {
        get {
            if (S.lastTimeLocal == "") return DateTime.Now;
            return DateTime.FromBinary(Convert.ToInt64(S.lastTimeLocal)); }
        set { S.lastTimeLocal = value.ToBinary().ToString(); }// Debug.Log(value.ToString()); }
    }
    public DateTime lastDailyTime//最後にDailyActionをした日付
    {
        get { return DateTime.FromBinary(Convert.ToInt64(S.lastDailyTime)); }
        set { S.lastDailyTime = value.ToBinary().ToString(); }
    }

    [SerializeField] public SaveR SR;//Reincarnationでリセットする
    [SerializeField] public Save S;
    public SaveDeclare SD;
    public saveCtrl saveCtrl;
    [NonSerialized] public static Main main;

    public GetReport inAppRestore;

    //public DateTime currentTime
    //{
    //    get
    //    {
    //        if (platform == PlatformKind.Steam && serverTime.isInitialized)
    //            return serverTime.currentTime;
    //        return DateTime.Now;
    //    }
    //}
    public DateTime currentLocalTime => DateTime.Now;
    public IdleLibrary.ServerTime serverTime;

    //Debug?
    public float timescale { get => Time.timeScale; }
    public double calculateSpanTimeSec => 0.50d * timescale;

    private void Awake()
    {
        main = this;
        serverTime = new IdleLibrary.ServerTime();
    }
    private void Start()
    {
        GetServerTime();
    }

    public bool isHacked;
    
    public async void GetServerTime()
    {
        //HackCheck : LocalTimeが前回の最終ローカルタイムよりも以前の場合
        if (DeltaTimeSec(currentLocalTime, lastTimeLocal) > 0)
        {
            isHacked = true;
            return;
        }
        if (main.platform != PlatformKind.Steam) return;
        serverTime.UpdateServerTime();
        await UniTask.WaitUntil(() => serverTime.isInitialized);
        //HackCheck
        //LocalTimeとServerTimeの誤差が１日以上ある場合
        if (DeltaTimeSec(currentLocalTime, serverTime.currentTime) > 86400 || DeltaTimeSec(serverTime.currentTime, currentLocalTime) > 86400)
        {
            isHacked = true;
            return;
        }
        ////以前に記録したServerTime:LocalTimeと比較して、誤差が10分以上ある場合
        //double deltaServerTime = DeltaTimeSec(lastServerTimeGotServerTime, serverTime.currentTime);
        //double deltaLocalTime = DeltaTimeSec(lastLocalTimeGotServerTime, currentLocalTime);
        //if (deltaServerTime - deltaServerTime > 10 * 60 || deltaServerTime - deltaServerTime > 10 * 60)
        //{
        //    isHacked = true;
        //    return;
        //}
        //lastServerTimeGotServerTime = serverTime.currentTime;
        //lastLocalTimeGotServerTime = currentLocalTime;
    }
    public void Save()
    {
        //if (currentLocalTime < lastTimeLocal)
        //Playtestの終了用
        if (currentLocalTime.CompareTo(releaseTime) == 1)
        {
            GameController.game.isPause = true;
            ConfirmUI confirmUI = new ConfirmUI(GameControllerUI.gameUI.popupCtrlUI.defaultConfirm);
            confirmUI.SetUI("Playtest has now ended.\nPlease watch Steam for the actual release very soon!\nPlease note, your save will not transfer to release.");
            return;
        }
        if (currentLocalTime.AddHours(1).CompareTo(lastTimeLocal) == -1)
        {
            isHacked = true;
            ConfirmUI confirmUI = new ConfirmUI(GameControllerUI.gameUI.popupCtrlUI.defaultConfirm);
            confirmUI.SetUI("The clock could not be obtained correctly.\nClose the game, check the computer clock, and open the game again.");
            return;
        }
        lastTimeLocal = currentLocalTime;
        saveCtrl.SavePerSec();
    }
    //void GetServerTime()
    //{
    //    serverTime.UpdateServerTime();
    //    lastTimeLocal = DateTime.Now;
    //    saveCtrl.SavePerSec();
    //    if (!serverTime.isInitialized) return;
    //    lastTimeServer = main.currentTime;
    //}

    // Start is called before the first frame update
    //void Start()
    //{

    ////初めてのプレイだったら現在の値を代入
    //if (!S.isContinuePlay)
    //{
    //    birthTime = currentTime;
    //    lastTime = currentTime;
    //    S.isContinuePlay = true;
    //}
    ////不正な時間が入っていたら現在の値を代入
    //if(lastTime < ReleaseTime || lastTime > currentTime)
    //{
    //    lastTime = currentTime;
    //}
    //}
    float deltaTime;
    double pauseTime;
    // Update is called once per frame
    void Update()
    {
        if (!GameControllerUI.gameUI.titleSceneUI.isLoaded) return;
        if (main.isHacked) return;
        deltaTime = Time.deltaTime;
        if (GameController.game.isPause)
        {
            pauseTime += deltaTime;//MultiplierのCalculateを稼働させるため、２秒間はpauseしない
            if (pauseTime >= 2) return;
        }
        if (pauseTime >= 2) pauseTime = 0;
        allTime += deltaTime;
        allTimeRealtime += Time.unscaledDeltaTime;
        allTimeWorldAscension += deltaTime;

        //Debug
        //if (GameController.game.steamAchievement != null) GameController.game.steamAchievement.UpdateCheck();
        //else GameController.game.steamAchievement = new SteamAchievement();

        //最初だけ微妙にずれてしまうバグの解消
        if (allTimeRealtime < 10) allTime = allTimeRealtime;

        timecount += deltaTime;
        if (timecount >= 3.0f)//3秒ごと
        {
            Save();
            timecountMinute++;

            //SteamAchievement
            if (GameController.game.steamAchievement == null) GameController.game.steamAchievement = new SteamAchievement();
            else GameController.game.steamAchievement.UpdateCheck();


            if (timecountMinute >= 20)//1分ごと
            {
                GameController.game.achievementCtrl.CheckAchieve();
                //if (GameController.game.steamAchievement != null) GameController.game.steamAchievement.UpdateCheck();
                Initialize.DailyAction();
                timecountSwarm++;
                if (timecountSwarm >= 35)//35分ごと
                {
                    GameController.game.areaCtrl.StartSwarm();
                    timecountSwarm = 0;
                }
                timecountMinute = 0;
            }
            timecount = 0;
        }
    }
    float timecount;
    int timecountSwarm;
    int timecountMinute;
}
