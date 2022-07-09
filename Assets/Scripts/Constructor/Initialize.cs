using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;

public partial class SaveR
{
    public bool isInitialized;//このゲームを初めてプレイした時にtrue（Prestigeでリセット）
}
public partial class Save
{
    public bool isInitialized;//このゲームを初めてプレイした時にtrue

    public bool isPlaytestBeforeJune10;//Playtestが終了したら消す
    public bool isAdjustVer00030001;
    public bool isAdjustVer00030002;
    public bool isAdjustVer00030005;
}

//全てのスクリプトの中で最後に呼ばれる（DebugCtrlを除く）
public class Initialize : MonoBehaviour
{
    public void Awake()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-us");
    }
    public void Start()
    {
        InitializeSave();
        InitializeSaveR();
        main.SR.isInitialized = true;
        main.S.isInitialized = true;
        //DailyAction(); mainでのみ行う
        Time.timeScale = 1.0f;
        game.Initialize();
        gameUI.Initialize();
        gameUI.titleSceneUI.StartAnimation();
        //OfflineBonus
        game.offlineBonus.CalculateOfflineBonus();
        gameUI.ShowOfflineBonus();

        //UI（初回開いた時にオブジェクトがちらつかないように、一度全てのTabを開いておく）
        InitializeUI();

        if (!main.S.isAdjustVer00030005)
        {
            if (game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).IsPurchased())
            {
                game.epicStoreCtrl.epicCoin.Increase(5000);
                game.epicStoreCtrl.Item(EpicStoreKind.AutoAbilityPreset).purchasedNum.ChangeValue(0);
            }
            for (int i = 0; i < main.S.expeditionPetSpecies.Length; i++)
            {
                main.S.expeditionPetSpecies[i] = MonsterSpecies.Slime;
                main.S.expeditionPetColors[i] = MonsterColor.Normal;
                main.S.expeditionPetIsSet[i] = false;
            }
            for (int i = 0; i < game.expeditionCtrl.expeditions.Length; i++)
            {
                Expedition expedition = game.expeditionCtrl.expeditions[i];
                expedition.progress.ChangeValue(0);
                expedition.isStarted = false;
                expedition.timeId.ChangeValue(0);
                expedition.movedDistance = 0;
            }
            main.S.isAdjustVer00030005 = true;
        }
        if (!main.S.isAdjustVer00030002)
        {
            for (int i = 0; i < game.inventoryCtrl.potionSlots.Length; i++)
            {
                if (game.inventoryCtrl.potionSlots[i].potion.kind == PotionKind.AscendedFromIEH1)
                    game.inventoryCtrl.potionSlots[i].potion.Delete();
            }
            main.S.receivedIEH1Achievement = 0;
            main.S.checkedIEH1Achievement = 0;
            main.S.isAdjustVer00030002 = true;
        }
        if (!main.S.isAdjustVer00030001)
        {
            main.S.wasd += (main.S.epicCoin + main.S.epicCoinConsumed) * 162464;
            main.S.isAdjustVer00030001 = true;
        }
    }
    async void InitializeUI()
    {
        for (int i = 0; i < gameUI.menuUI.menuButtons.Length; i++)
        {
            gameUI.menuUI.menuButtons[i].onClick.Invoke();
            await UniTask.DelayFrame(5);
        }
        gameUI.menuUI.menuButtons[0].onClick.Invoke();
        for (int i = 0; i < gameUI.menuUI.menuButtons.Length; i++)
        {
            gameUI.menuUI.menuButtons[i].onClick.AddListener(() => { gameUI.soundUI.Play(SoundEffect.Click); });
        }
    }
    void InitializeSave()//ゲームを初めてプレイした時のみ呼び出す
    {
        if (main.S.isInitialized) return;

        //スクリーンサイズ
        gameUI.autoCanvasScaler.AdjustWindowSize();

        SettingMenuUI.Toggle(ToggleKind.BGM).thisToggle.isOn = true;
        gameUI.soundUI.bgmSource.Stop();
        SettingMenuUI.Toggle(ToggleKind.SFX).thisToggle.isOn = true;
        //gameUI.menuUI.MenuUI(MenuKind.Setting).GetComponent<SettingMenuUI>().Toggle(ToggleKind.BGM).thisToggle.isOn = true;
        //gameUI.menuUI.MenuUI(MenuKind.Setting).GetComponent<SettingMenuUI>().Toggle(ToggleKind.SFX).thisToggle.isOn = true;
        gameUI.menuUI.MenuUI(MenuKind.Setting).GetComponent<SettingMenuUI>().SoundSlider(SoundSliderKind.BGM).thisSlider.value = 0.25f;
        gameUI.menuUI.MenuUI(MenuKind.Setting).GetComponent<SettingMenuUI>().SoundSlider(SoundSliderKind.SFX).thisSlider.value = 0.35f;

        for (int i = 0; i < game.rebirthCtrl.rebirthList.Count; i++)
        {
            int count = i;
            game.rebirthCtrl.rebirthList[count].autoRebirthLevel = game.rebirthCtrl.rebirthList[count].heroLevel;
        }
        //NitroSpeed
        game.nitroCtrl.maxNitroSpeed.Calculate();
        game.nitroCtrl.speed.ChangeValue(2.0d);

        //CombatRangeの初期値
        main.SR.combatRangeId[(int)HeroKind.Warrior] = 0;
        main.SR.combatRangeId[(int)HeroKind.Wizard] = 1;
        main.SR.combatRangeId[(int)HeroKind.Angel] = 3;
        main.SR.combatRangeId[(int)HeroKind.Thief] = 0;
        main.SR.combatRangeId[(int)HeroKind.Archer] = 5;
        main.SR.combatRangeId[(int)HeroKind.Tamer] = 5;

        //if (main.platform == PlatformKind.Steam)
        //    await UniTask.WaitUntil(() => main.serverTime.isInitialized);
        main.birthTime = main.currentLocalTime;

        //ここで一度DailyResetをしておく
        DailyReset();

        //Playtest用（リリース時に消す）
        if (main.S.isPlaytestBeforeJune10)
        {
            game.nitroCtrl.nitro.IncreaseWithoutLimit(3000);
        }
    }
    public static void InitializeSaveR()//WorldAscension後にも1度呼び出す
    {
        if (main.SR.isInitialized) return;

        game.guildCtrl.Member(HeroKind.Warrior).SwitchActive(true);
        game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.Calculate();
        game.alchemyCtrl.mysteriousWaterExpandedCapNum.Increase(1);
    }

    public static void DailyAction()
    {
        //Debug.Log(main.lastDailyTime.ToString());
        //if (main.platform == PlatformKind.Steam)
        //    await UniTask.WaitUntil(() => main.serverTime.isInitialized);
        if (main.S.lastDailyTime == "")//何も入ってない場合
        {
            DailyReset();
            return;
        }
        if (main.S.lastDailyTime != "" && main.currentLocalTime.Date <= main.lastDailyTime.Date) return;
        DailyReset();
    }
    static void DailyReset()
    {
        main.lastDailyTime = main.currentLocalTime;
        //DailyQuest
        for (int i = 0; i < game.questCtrl.QuestArray(QuestKind.Daily, HeroKind.Warrior).Length; i++)
        {
            game.questCtrl.QuestArray(QuestKind.Daily, HeroKind.Warrior)[i].ResetDailyQuest();
        }
    }
}

