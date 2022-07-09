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
    public int checkedIEH1Achievement;
    public int receivedIEH1Achievement;
    public bool isReceivedIEH1DLCIEH2SupportPack;
}

public class IEH1PlayerBonusUI : MonoBehaviour
{
    [SerializeField] Button openButton, quitButton;
    [SerializeField] TextMeshProUGUI receivedText;
    public OpenCloseUI openCloseUI;

    bool isChecked;// => checkedAchievementNum > 0;//SteamAchievement, DLCの情報をとって来れるのは１起動につき１回までとする
    int checkedAchievementNum { get => main.S.checkedIEH1Achievement; set => main.S.checkedIEH1Achievement = Math.Max(main.S.checkedIEH1Achievement, value); }
    bool isPurchasedDLC;

    // Start is called before the first frame update
    void Start()
    {
        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(quitButton);

        openCloseUI.openActions.Add(CheckBonus);

        //AcceptedQuestLimitの登録
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.questCtrl.AcceptableNum(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1, () => main.S.isReceivedIEH1DLCIEH2SupportPack));
        }
    }

    string tempStr;
    async void CheckBonus()
    {
        openButton.interactable = false;
        quitButton.interactable = false;
        tempStr = "";
        if (!isChecked)
        {
            //Severから情報を取得
            if (isTrying)
            {
                quitButton.interactable = true;
                return;
            }
            isTrying = true;
            receivedText.text = "Data is being acquired...";
            //SteamAchievement
            if (!SteamManager.Initialized)
            {
                receivedText.text = "Steam Server doesn't work currently. Please reboot the game and try it again.";
                openButton.interactable = true;
                quitButton.interactable = true;
                return;
            }
            checkedAchievementNum = await game.getAchievementInfoIEH1.AchievementsAchievedCount();
            //DLC IEH2SupportPack
            if (game.getOwnerShipIEH1DLC_IEH2SupportPack == null) game.getOwnerShipIEH1DLC_IEH2SupportPack = new GetOwnerShip("F6724F6EF14E3AE95B7B7F14E53BEEC3", "1580930");
            else if (!game.getOwnerShipIEH1DLC_IEH2SupportPack.isGotInfo)
            {
                game.getOwnerShipIEH1DLC_IEH2SupportPack.Initialize();
                await UniTask.WaitUntil(() => game.getOwnerShipIEH1DLC_IEH2SupportPack.isGotInfo);
            }
            isPurchasedDLC = game.getOwnerShipIEH1DLC_IEH2SupportPack.isOwn;
            if (checkedAchievementNum < 1)
            {
                tempStr += "Steam Achievement Failed.\nPlease check followings:\nSteam Profile > Edit Profile > Privacy Settings > Game Details : [Public]\n\n";
            }
            isTrying = false;
            isChecked = true;
        }
        //実際に獲得する処理
        if (!main.S.isReceivedIEH1DLCIEH2SupportPack && isPurchasedDLC)
        {
            tempStr += "You have received:\n<color=green><sprite=\"EpicCoin\" index=0> 5500 Epic Coin\nAccepted Quest Limit + 1</color>\n\n";
            game.epicStoreCtrl.epicCoin.Increase(5500);
            main.S.isReceivedIEH1DLCIEH2SupportPack = true;
        }
        //EpicStoreによる補正追加
        int tempAchievementNum = checkedAchievementNum + (int)game.epicStoreCtrl.Item(EpicStoreKind.IEH1BonusTalisman).purchasedNum.value;
        int tempGetTalismanNum = Math.Min(tempAchievementNum, ReceivableNumTalisman()) - main.S.receivedIEH1Achievement;
        if (tempGetTalismanNum > 0)
        {
            if (!game.inventoryCtrl.CanCreatePotion(PotionKind.AscendedFromIEH1, tempGetTalismanNum))
            {
                tempStr += "You need " + tDigit(1 + (int)(tempGetTalismanNum / 10)) + " utility slots!";
            }
            else
            {
                tempStr += "You have received:\n<color=green>" + tDigit(tempGetTalismanNum) + " Talisman [ " + localized.PotionName(PotionKind.AscendedFromIEH1) + " ]";
                game.inventoryCtrl.CreatePotion(PotionKind.AscendedFromIEH1, tempGetTalismanNum);
                main.S.receivedIEH1Achievement += tempGetTalismanNum;//Math.Max(main.S.receivedIEH1Achievement, tempGetTalismanNum);
            }　
        }
        else if (tempAchievementNum > 0)
        {
            tempStr += "You have already received all of the current bonuses.";
        }
        receivedText.text = tempStr;
        quitButton.interactable = true;
        openButton.interactable = true;
    }

    public static int ReceivableNumTalisman()
    {
        if (game.achievementCtrl.IsAchievedAnyHeroRebirth(2)) return 20;
        if (game.achievementCtrl.IsAchievedAnyHeroRebirth(1)) return 15;
        if (game.achievementCtrl.IsAchievedAnyHeroRebirth(0)) return 10;
        return 5;
    }

    bool isTrying = false;
}
