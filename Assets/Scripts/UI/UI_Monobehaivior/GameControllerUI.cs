using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static UsefulMethod;
using static GameController;

public class GameControllerUI : MonoBehaviour
{
    public static GameControllerUI gameUI;
    public AutoCanvasScaler autoCanvasScaler;
    public SoundUI soundUI;
    public TitleSceneUI titleSceneUI;
    public SceneUI sceneUI;
    public SkillEffectUI skillEffectUI;
    public BattleStatusUI battleStatusUI;
    public BattleControllerUI battleCtrlUI;
    public WorldMapUI worldMapUI;
    public MenuControllerUI menuUI;
    public ResourceControllerUI resourceCtrlUI;
    public LogControllerUI logCtrlUI;
    public DropControllerUI dropCtrlUI;
    public PopupControllerUI popupCtrlUI;
    public HelpControllerUI helpCtrlUI;
    public EpicStoreMenuUI epicStorUI;
    public BackgroundModeUI backgroundModeUI;
    public SaveManager saveManager;
    public Canvas performanceModeCanvas;
    public GameObject rightCanvas, leftCanvas, frameCanvas, photonCanvas, popupCanvas, epicStoreCanvas, helpCanvas, backgroundModeCanvas;
    public BonusCodeUI bonusCodeUI;
    public OfflineBonusUI offlineBonusUI;

    public Photon.Chat.Demo.ChatGui chatGUI;

    public DebugController debugCtrl;

    public Vector3 screenSize { get { return Vector2.right * Screen.width + Vector2.up * Screen.height; } }

    //これは大変重い処理だが、たまーーーに呼ぶといい。
    public void UnloadUnusedAsset()
    {
        Resources.UnloadUnusedAssets();
    }

    private void Awake()
    {
        gameUI = this;
        //フレームレート
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        //UnloadUnusedAsset();
    }

    void Start()
    {
        //ShowOfflineBonus();
    }
    public async void ShowOfflineBonus()//TitleSceneUIで呼ぶことにした
    {
        await UniTask.WaitUntil(() => game.offlineBonus.offlineTimesec >= 0 || Main.main.isHacked);        
        if (Main.main.isHacked)
        {
            var confirm = new ConfirmUI(popupCtrlUI.offlineBonusConfirm);
            confirm.SetUI(
                "The clock could not be obtained correctly." +
                "\nClose the game, set the computer clock correctly, and open the game again." +
                "\nYou may need to wait until Last Local Time." +
                "\n- Last Local Time : " + Main.main.lastTimeLocal.ToString("MM/dd/yyyy HH:mm") +
                //"\nLast Local Time on opening the game : " + Main.main.lastLocalTimeGotServerTime.ToString("MM/dd/yyyy HH:mm") + 
                //"\nLast Server Time on opening the game : " + Main.main.lastServerTimeGotServerTime.ToString("MM/dd/yyyy HH:mm") + 
                "\n- Current Local Time : " + Main.main.currentLocalTime.ToString("MM/dd/yyyy HH:mm") +
                "\n- Current Server Time : " + Main.main.serverTime.currentTime.ToString("MM/dd/yyyy HH:mm")
                );
            //confirm.quitButton.interactable = false;
            SetActive(confirm.okButton.gameObject, false);
            return;
        }
        if (game.offlineBonus.offlineTimesec <= 10)
        {
            return;
        }
        offlineBonusUI.SetUI();
    }

    public void Initialize()
    {
        menuUI.Initialize();
        battleStatusUI.Initialize();
        battleCtrlUI.Initialize();
        dropCtrlUI.Initialize();
        skillEffectUI.Initialize();
    }

    int timescaleCount;
    private void FixedUpdate()
    {
        timescaleCount++;
        if (timescaleCount < Time.timeScale) return;
        timescaleCount = 0;
        if (BackgroundModeUI.isBackgroundMode)
        {
            backgroundModeUI.UpdateUI();
            return;
        }
        menuUI.UpdateUI();
        resourceCtrlUI.UpdateUI();
        helpCtrlUI.UpdateUI();
        epicStorUI.UpdateUI();
        if (Main.main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        skillEffectUI.UpdateUI();
        battleStatusUI.UpdateUI();
        battleCtrlUI.UpdateUI();
        logCtrlUI.UpdateUI();
        worldMapUI.UpdateUI();
        saveManager.UpdateUI();

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    game.areaCtrl.StartSwarm();
        //    Debug.Log(game.areaCtrl.currentSwarmArea.Name());
        //}
    }
    private void Update()
    {
        if (bonusCodeUI.openCloseUI.isOpen)
        {
            isShiftPressed = false;
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            isShiftPressed = true;
            if (Input.GetKeyDown(KeyCode.P))
                SettingMenuUI.Toggle(ToggleKind.PerformanceMode).thisToggle.isOn = !SettingMenuUI.Toggle(ToggleKind.PerformanceMode).thisToggle.isOn;
        }
        else isShiftPressed = false;
    }
    public static bool isShiftPressed;

}
