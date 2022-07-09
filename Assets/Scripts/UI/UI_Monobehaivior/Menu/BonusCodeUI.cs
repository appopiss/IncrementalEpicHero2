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
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;


public partial class Save
{
    public bool[] isReceivedBonusCodes;//[kind]
}
public class BonusCodeUI : MonoBehaviour
{
    [SerializeField] Button openButton, quitButton;
    [SerializeField] TextMeshProUGUI receivedText;
    [SerializeField] TMP_InputField inputField;
    public OpenCloseUI openCloseUI;

    // Start is called before the first frame update
    void Start()
    {
        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(quitButton);

        openCloseUI.openActions.Add(() => receivedText.text = "");
        inputField.onEndEdit.AddListener(OnEndInputedAction);
    }

    void OnEndInputedAction(string code)
    {
        ////Debug
        //if (code == "Achievement")
        //{
        //    receivedText.text = "Steam : " + SteamManager.Initialized;
        //        //+ "\nRequestedStats : " + game.steamAchievement.m_bRequestedStats
        //        //+ "\nStatsValid : " + game.steamAchievement.m_bStatsValid;

        //    return;
        //}
        //if (code == "Achievement1")
        //{
        //    receivedText.text = "SteamAchievement is null? : " + (game.steamAchievement == null).ToString();
        //    Steamworks.SteamUserStats.SetAchievement("Playtest");
        //    return;
        //}


        BonusCodeKind bonusCodeKind = BonusCodeKind.Nothing;
        for (int i = 0; i < Enum.GetNames(typeof(BonusCodeKind)).Length; i++)
        {
            if (code == ((BonusCodeKind)i).ToString())
            {
                bonusCodeKind = (BonusCodeKind)i;
                break;
            }
        }
        if (bonusCodeKind == BonusCodeKind.Nothing)
        {
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0:
                    receivedText.text = "<color=red>This code is not correct.</color>";
                    break;
                case 1:
                    receivedText.text = "<color=red>You silly goose, that's not a thing!</color>";
                    break;
                case 2:
                    receivedText.text = "<color=red>Are you entering a code for the right game? This is Incremental Epic Hero 2.</color>";
                    break;
                case 3:
                    receivedText.text = "<color=red>What are you, sixty? Find a code from this century please.</color>";
                    break;
                default:
                    receivedText.text = "<color=red>This code is not correct.</color>";
                    break;
            }

            return;
        }
        if (bonusCodeKind == BonusCodeKind.DebugMode162464)
        {
            SetActive(gameUI.debugCtrl.debugShowButton.gameObject, true);
            receivedText.text = "Debug Mode";
            return;
        }
        if (bonusCodeKind == BonusCodeKind.DebugModeEC1624)
        {
            game.epicStoreCtrl.epicCoin.Increase(100000);
            receivedText.text = "100000 Epic Coin";
            return;
        }
        //if (bonusCodeKind == BonusCodeKind.HardReset)
        //{
        //    SetActive(gameUI.debugCtrl.hardresetButton.gameObject, true);
        //    receivedText.text = "Enabled Hard Reset";
        //    return;
        //}
        //if (bonusCodeKind == BonusCodeKind.RestorePurchase)
        //{
        //    main.inAppRestore.RestoreAction();
        //    receivedText.text = "Restore succeeded!";
        //    return;
        //}
        if (main.S.isReceivedBonusCodes[(int)bonusCodeKind])
        {
            receivedText.text = "<color=red>You have already used this bonus code.</color>";
            return;
        }
        string tempStr = "";
        switch (bonusCodeKind)
        {
            case BonusCodeKind.IEH2hapiwaku:
                game.epicStoreCtrl.epicCoin.Increase(1000);
                tempStr = "<sprite=\"epiccoin\" index=0> 1000 Epic Coin</color>";
                break;
            case BonusCodeKind.DLCStarter:
                //return;
                //if (!main.S.isPlaytestBeforeJune10) return;
                main.S.isDlcStarterPack = true;
                game.nitroCtrl.nitro.IncreaseWithoutLimit(5000);
                game.areaCtrl.portalOrb.Increase(5);
                tempStr = "DLC Starter Pack";
                break;
            case BonusCodeKind.DLCNitro:
                //if (!main.S.isPlaytestBeforeJune10) return;
                main.S.isDlcNitroPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                game.nitroCtrl.nitroCap.Calculate();
                game.nitroCtrl.speed.ToMax();
                tempStr = "DLC Nitro Pack";
                break;
            case BonusCodeKind.DLCGlobalSkill:
                //return;
                //if (!main.S.isPlaytestBeforeJune10) return;
                main.S.isDlcGlobalSkillSlotPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                tempStr = "DLC Global Skill Slot Pack";
                gameUI.battleStatusUI.SetSkillSlot();
                break;
            case BonusCodeKind.DLCInventory:
                //if (!main.S.isPlaytestBeforeJune10) return;
                main.S.isDlcInventorySlotPack = true;
                game.epicStoreCtrl.epicCoin.Increase(5500);
                game.areaCtrl.portalOrb.Increase(10);
                game.inventoryCtrl.slotUIAction();
                tempStr = "DLC Inventory Slot Expansion Pack";
                break;
            default:
                receivedText.text = "<color=red>This code is not correct.</color>";
                return;
        }
        SetReceiveString(tempStr);
        main.S.isReceivedBonusCodes[(int)bonusCodeKind] = true;
    }

    void SetReceiveString(string receiveString)
    {
        receivedText.text = "You have received:\n<color=green>";
        receivedText.text += receiveString;
    }
}

public enum BonusCodeKind//削除したり順番を入れ替えたりしてはいけない
{
    Nothing,
    DebugMode162464,
    DebugModeEC1624,
    IEH2hapiwaku,
    HardReset,
    RestorePurchase,
    DLCStarter,
    DLCNitro,
    DLCGlobalSkill,
    DLCInventory,
}