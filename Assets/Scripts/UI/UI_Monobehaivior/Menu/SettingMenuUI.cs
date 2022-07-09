using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static InventoryParameter;
using static Localized;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public partial class Save
{
    public bool[] isToggleOn;
    public float bgmVolume, sfxVolume;
}

public class SettingMenuUI : MENU_UI
{
    public GameObject[] toggles;
    public static ToggleUI[] togglesUI;
    [SerializeField] GameObject[] soundSliders;
    public SoundSliderUI[] soundSlidersUI;
    [SerializeField] TextMeshProUGUI totalTimePlayedText, totalTimePlayedRealtimeText, versionText, serverTimeText;
    [SerializeField] Button discordButton;
    [SerializeField] GameObject backgroundModeButtonObject;
    public static PopupUI popupUI;
    [SerializeField] GameObject[] tabCanvas;
    [SerializeField] Button[] tabButtons;
    SwitchCanvasUI switchCanvasUI;
    [SerializeField] TextMeshProUGUI bonusCodeText, IEH1BonusText,  PlaytestBonusText;
    [SerializeField] Button hardResetButton;

    private void Awake()
    {
        togglesUI = new ToggleUI[toggles.Length];
        for (int i = 0; i < togglesUI.Length; i++)
        {
            togglesUI[i] = new ToggleUI(toggles[i], (ToggleKind)i);
        }
        soundSlidersUI = new SoundSliderUI[soundSliders.Length];
        for (int i = 0; i < soundSlidersUI.Length; i++)
        {
            soundSlidersUI[i] = new SoundSliderUI(soundSliders[i], (SoundSliderKind)i);
        }
        discordButton.onClick.AddListener(() => Application.OpenURL("https://discord.gg/QEpxWM2fv5"));

        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
    }
    private void Start()
    {
        popupUI.SetTargetObject(toggles[(int)ToggleKind.BuyOneGuildAbility], () => "Overrides Top-Left Multiplier to x1 for the upgrades below");
        popupUI.SetTargetObject(toggles[(int)ToggleKind.BuyOneRebirthUpgrade], () => "Overrides Top-Left Multiplier to x1 for the upgrades below");
        popupUI.SetTargetObject(toggles[(int)ToggleKind.BuyOneWorldAscensionUpgrade], () => "Overrides Top-Left Multiplier to x1 for the upgrades below");
        popupUI.SetTargetObject(toggles[(int)ToggleKind.BuyOneAreaPrestige1], () => "Overrides Top-Left Multiplier to x1 for the upgrades below");
        popupUI.SetTargetObject(toggles[(int)ToggleKind.BuyOneAreaPrestige2], () => "Overrides Top-Left Multiplier to x1 for the upgrades below");

        popupUI.SetTargetObject(backgroundModeButtonObject, () => "aka \"Quick, your boss is coming!\" Mode");
        for (int i = 0; i < togglesUI.Length; i++)
        {
            togglesUI[i].Start();
        }

        switchCanvasUI = new SwitchCanvasUI(tabCanvas, tabButtons, false, false, true);
        tabButtons[0].onClick.Invoke();

        hardResetButton.onClick.AddListener(() =>
        {
            var confirm = new ConfirmUI(gameUI.popupCtrlUI.defaultConfirm);
            confirm.SetUI("<size=24>Are you sure you want to Hard Reset?\n\n<size=20><color=yellow>You will lose EVERYTHING in the game.\nAfter Hard Reset, please reboot the game to stabilize the game performance. You can restore in-app purchases in Epic Store.");
            confirm.okButton.onClick.RemoveAllListeners();
            confirm.okButton.onClick.AddListener(() => HardReset());
        });
    }
    public static ToggleUI Toggle(ToggleKind kind)
    {
        return togglesUI[(int)kind];
    }
    public SoundSliderUI SoundSlider(SoundSliderKind kind)
    {
        return soundSlidersUI[(int)kind];
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        for (int i = 0; i < togglesUI.Length; i++)
        {
            togglesUI[i].UpdateUI();
        }
        Toggle(ToggleKind.SwarmChaser).thisToggle.interactable = game.epicStoreCtrl.Item(EpicStoreKind.SwarmChaser).IsPurchased();
        Toggle(ToggleKind.AdvancedAutoBuyTrap).thisToggle.interactable = game.epicStoreCtrl.Item(EpicStoreKind.LimitSlotAutoBuyNet).IsPurchased();

        //DisableLog
        Toggle(ToggleKind.DisableGoldLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;
        Toggle(ToggleKind.DisableExpLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;
        Toggle(ToggleKind.DisableResourceLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;
        Toggle(ToggleKind.DisableMaterialLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;
        Toggle(ToggleKind.DisableGuildLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;
        Toggle(ToggleKind.DisableCaptureLog).thisToggle.interactable = !Toggle(ToggleKind.DisableAnyLog).isOn;

        totalTimePlayedRealtimeText.text = optStr + "Real Playtime : " + DoubleTimeToDate(main.allTimeRealtime);
        totalTimePlayedText.text = optStr + "In-Game Playtime : " + DoubleTimeToDate(main.allTime);
        versionText.text = optStr + "Version " + (version / 1000000).ToString() + "." + ((version % 1000000) / 10000).ToString() + "." + ((version % 10000) / 100).ToString() + "." + (version % 100).ToString();
        serverTimeText.text = "Clock : " + main.currentLocalTime.ToString("MM/dd/yyyy HH:mm");

        IEH1BonusText.text = IEH1BonusString();
        PlaytestBonusText.text = IEH2PlaytestBonusString();
    }

    string tempStr;
    string IEH1BonusString()
    {
        tempStr = "If you have played IEH1, you can gain Epic Talisman ";
        if (main.S.receivedIEH1Achievement > 0) tempStr += "<color=green>";
        else tempStr += "<color=orange>";
        tempStr += "[ Proof of Ascension from IEH1 ]</color> based on the # of IEH1 Steam Achievements you have got.\nThe amount is limited by the progression of the game.";
        tempStr += "\n(From starting : 5, From Tier 1 Rebirth : 10, From Tier 2 : 15, From Tier 3 : 20)";
        tempStr += "\nYou have received : <color=green>" + tDigit(main.S.receivedIEH1Achievement) + "</color> / " + tDigit(IEH1PlayerBonusUI.ReceivableNumTalisman()) + " ( Achievement # " + tDigit(main.S.checkedIEH1Achievement);
        if (game.epicStoreCtrl.Item(EpicStoreKind.IEH1BonusTalisman).IsPurchased()) tempStr += " <color=green>+ " + tDigit(game.epicStoreCtrl.Item(EpicStoreKind.IEH1BonusTalisman).purchasedNum.value) + "</color>";
        tempStr += " )";
        tempStr += "\n\nIf you have purchased IEH1 DLC[IEH2 Support Pack], you can also get followings:";
        if (main.S.isReceivedIEH1DLCIEH2SupportPack) tempStr += "<color=green>";
        else tempStr += "<color=orange>";
        tempStr += "\n<sprite=\"EpicCoin\" index=0> 5500 Epic Coin, Accepted Quest Limit + 1</color>";
        return tempStr;
    }

    string tempStr2;
    string IEH2PlaytestBonusString()
    {
        tempStr2 = "If you have played the IEH2 Playtest, you can gain ";
        if (main.S.isReceivedIEH2PlaytestBonus) tempStr2 += "<color=green>";
        else tempStr2 += "<color=orange>";
        tempStr2 += "<sprite=\"EpicCoin\" index=0> Epic Coin</color> and ";
        if (main.S.isReceivedIEH2PlaytestBonus) tempStr2 += "<color=green>";
        else tempStr2 += "<color=orange>";
        tempStr2 += "Portal Orb</color> based on the # of IEH2 Playtest Steam Achievements you unlocked. ";
        return tempStr2;
    }

    void HardReset()
    {
        game.isPause = true;
        SetActive(gameUI.titleSceneUI.gameObject, true);
        bool isPlaytestBeforeJune10 = Main.main.S.isPlaytestBeforeJune10;

        main.SR = new SaveR();
        main.S = new Save();

        main.S.isPlaytestBeforeJune10 = isPlaytestBeforeJune10;
        main.Save();

        //PlayerPrefs.DeleteKey(keyList.resetSaveKey);
        //PlayerPrefs.DeleteKey(keyList.permanentSaveKey);
        SceneManager.LoadScene("Main");
    }
}

public class SoundSliderUI
{
    public SoundSliderUI(GameObject gameObject, SoundSliderKind kind)
    {
        this.kind = kind;
        thisObject = gameObject;
        thisSlider = thisObject.GetComponent<Slider>();
        thisSlider.onValueChanged.AddListener(ChangeVolume);
        thisSlider.value = kind == SoundSliderKind.BGM ? main.S.bgmVolume : main.S.sfxVolume;
    }
    void ChangeVolume(float value)
    {
        if (kind == SoundSliderKind.BGM)
        {
            gameUI.soundUI.ChangeBGMVolume(value);
            main.S.bgmVolume = value;
        }
        else if (kind == SoundSliderKind.SFX)
        {
            gameUI.soundUI.ChangeSFXVolume(value);
            main.S.sfxVolume = value;
        }
    }
    GameObject thisObject;
    public Slider thisSlider;
    SoundSliderKind kind;
}
public class ToggleUI
{
    public ToggleUI(GameObject gameObject, ToggleKind kind)
    {
        this.kind = kind;
        thisObject = gameObject;
        thisToggle = gameObject.GetComponent<Toggle>();
        thisToggle.onValueChanged.AddListener(ChangeIsOn);
        thisText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void Start()
    {
        thisToggle.isOn = isOn;
        gameUI.soundUI.bgmSource.Stop();
    }
    void ChangeIsOn(bool isOn)
    {
        this.isOn = isOn;
        switch (kind)
        {
            case ToggleKind.BGM:
                //if (!isOn) gameUI.soundUI.bgmSource.Stop();
                if (isOn) gameUI.soundUI.bgmSource.Play();
                else gameUI.soundUI.bgmSource.Stop();
                break;
            case ToggleKind.PerformanceMode:
                //SetActive(gameUI.rightCanvas, !isOn);
                gameUI.performanceModeCanvas.enabled = isOn;
                break;
            case ToggleKind.DarkenBattlefield:
                gameUI.battleCtrlUI.fieldImage.color = Color.white * (1f - 0.25f * Convert.ToInt16(isOn));
                break;
            case ToggleKind.DisableAnyLog:
                //SetActive(gameUI.battleCtrlUI.logCanvasObject, !isOn);
                gameUI.battleCtrlUI.logCanvasObject.GetComponent<Image>().enabled = !isOn;

                SettingMenuUI.Toggle(ToggleKind.DisableGoldLog).thisToggle.isOn = isOn;
                SettingMenuUI.Toggle(ToggleKind.DisableExpLog).thisToggle.isOn = isOn;
                SettingMenuUI.Toggle(ToggleKind.DisableResourceLog).thisToggle.isOn = isOn;
                SettingMenuUI.Toggle(ToggleKind.DisableMaterialLog).thisToggle.isOn = isOn;
                SettingMenuUI.Toggle(ToggleKind.DisableGuildLog).thisToggle.isOn = isOn;
                SettingMenuUI.Toggle(ToggleKind.DisableCaptureLog).thisToggle.isOn = isOn;

                break;
            case ToggleKind.BuyOneAreaPrestige1:
                SettingMenuUI.Toggle(ToggleKind.BuyOneAreaPrestige2).thisToggle.SetIsOnWithoutNotify(isOn);
                break;
            case ToggleKind.BuyOneAreaPrestige2:
                SettingMenuUI.Toggle(ToggleKind.BuyOneAreaPrestige1).thisToggle.SetIsOnWithoutNotify(isOn);
                break;
            case ToggleKind.SwarmChaser:
                if (isOn)
                {
                    if (game.areaCtrl.isSwarm)
                    {
                        game.battleCtrl.areaBattle.currentAreaKind = game.areaCtrl.currentSwarmArea.kind;
                        game.battleCtrl.areaBattle.currentAreaId = game.areaCtrl.currentSwarmArea.id;
                        game.battleCtrl.areaBattle.fieldUIAction(game.battleCtrl.areaBattle.CurrentArea());
                        if (game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased())
                            game.autoCtrl.Convene(game.areaCtrl.currentSwarmArea);
                    }
                }
                break;
            case ToggleKind.DisableNotificationAchievement:
                gameUI.helpCtrlUI.ShowAchievIcon();
                break;
        }
    }
    public void UpdateUI()
    {
        thisText.text = localized.Toggle(kind);
    }

    GameObject thisObject;
    public Toggle thisToggle;
    TextMeshProUGUI thisText;
    Slider thisSlider;
    public ToggleKind kind;
    public bool isOn { get => main.S.isToggleOn[(int)kind]; set => main.S.isToggleOn[(int)kind] = value; }
}

public enum SoundSliderKind
{
    BGM,
    SFX
}
public enum ToggleKind
{
    BGM,
    SFX,
    DisableDamageText,
    DisableGoldLog,
    FastResult,
    ShowDPS,
    ShowDetailStats,
    PerformanceMode,
    HideCompletedQuest,
    AutoDisassembleExcludeEnchanted,
    DisableCombatRange,
    DarkenBattlefield,
    BuyOneGuildAbility,
    BuyOneRebirthUpgrade,
    SkillLessMPAvailable,
    BuyOneWorldAscensionUpgrade,
    ShowEquipmentStar,
    DisableParticle,
    DisableAttackEffect,
    DisableAnyLog,
    BuyOneAreaPrestige1,
    BuyOneAreaPrestige2,
    SwarmChaser,
    AdvancedAutoBuyTrap,
    DisableNotificationAchievement,
    DisableNotificationQuest,
    DisableNotificationLab,
    DisableExpLog,
    DisableResourceLog,
    DisableMaterialLog,
    DisableGuildLog,
    DisableCaptureLog,
    DisableSwarmResult,
    DisableNotificationExpedition,
}
