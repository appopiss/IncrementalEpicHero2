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

public class ResourceControllerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText, slimeCoinText, stoneText, crystalText, leafText, nitroText;
    [SerializeField] TextMeshProUGUI guildText;
    [SerializeField] Button nitroButton;
    Image nitroImage;
    [SerializeField] Button multibuyButton, helpButtons;
    MultibuyUI multibuyUI;
    Image goldFillImage, stoneFillImage, crystalFillImage, leafFillImage, slimeCoinFillImage, nitroFillImage, guildFillImage;
    //PopupUI nitroPopupUI;
    //PopupUI slimecoinPopupUI;
    //PopupUI goldPopupUI, stonePopup, crystalPopupUI, leafPopupUI;
    //PopupUI guildPopupUI;
    PopupUI popupUI;

    private void Start()
    {
        multibuyUI = new MultibuyUI(multibuyButton);

        nitroButton.onClick.AddListener(SwitchNitro);
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(multibuyButton.gameObject, MultibuyPopupString);
        popupUI.SetTargetObject(nitroButton.gameObject, NitroPopupString);
        popupUI.SetTargetObject(slimeCoinText.gameObject, SlimeCoinPopupString);
        popupUI.SetTargetObject(goldText.gameObject, GoldPopupString);
        popupUI.SetTargetObject(stoneText.gameObject, () => ResourcePopupString(ResourceKind.Stone));
        popupUI.SetTargetObject(crystalText.gameObject, () => ResourcePopupString(ResourceKind.Crystal));
        popupUI.SetTargetObject(leafText.gameObject, () => ResourcePopupString(ResourceKind.Leaf));
        popupUI.SetTargetObject(guildText.gameObject, GuildPopupString);

        goldFillImage = goldText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        stoneFillImage = stoneText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        crystalFillImage = crystalText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        leafFillImage = leafText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        slimeCoinFillImage = slimeCoinText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        nitroFillImage = nitroButton.gameObject.transform.parent.gameObject.GetComponent<Image>();
        guildFillImage = guildText.gameObject.transform.parent.gameObject.GetComponent<Image>();
        nitroImage = nitroButton.gameObject.GetComponent<Image>();
        game.nitroCtrl.switchUIAction = () => { nitroImage.sprite = sprite.nitro[Convert.ToInt32(game.nitroCtrl.isActive)]; };
    }

    void SwitchNitro()
    {
        game.nitroCtrl.SwitchActive();
    }

    public void UpdateUI()
    {
        goldText.text = optStr + "<sprite=\"resource\" index=0>  " + tDigit(Math.Floor(game.resourceCtrl.gold.value)) + " / " + tDigit(Math.Floor(game.resourceCtrl.goldCap.Value()));
        slimeCoinText.text = optStr + "<sprite=\"resource\" index=1>  " + tDigit(Math.Floor(game.resourceCtrl.slimeCoin.value)) + " / " + tDigit(Math.Floor(game.resourceCtrl.slimeCoinCap.Value()));
        stoneText.text = optStr + "<sprite=\"resource\" index=2>  " + tDigit(Math.Floor(game.resourceCtrl.Resource(ResourceKind.Stone).value));
        crystalText.text = optStr + "<sprite=\"resource\" index=3>  " + tDigit(Math.Floor(game.resourceCtrl.Resource(ResourceKind.Crystal).value));
        leafText.text = optStr + "<sprite=\"resource\" index=4>  " + tDigit(Math.Floor(game.resourceCtrl.Resource(ResourceKind.Leaf).value));
        nitroText.text = tDigit(Math.Floor(game.nitroCtrl.nitro.value)) + " / " + tDigit(Math.Floor(game.nitroCtrl.nitroCap.Value()));
        guildText.text = optStr + "<size=20>GLv " + tDigit(game.guildCtrl.Level()) + " <size=16>( EXP : " + tDigit(game.guildCtrl.exp.value) + " / " + tDigit(game.guildCtrl.RequiredExp()) + " )";
        popupUI.UpdateUI();

        goldFillImage.fillAmount = game.resourceCtrl.GoldPercent();
        stoneFillImage.fillAmount = (float)game.resourceCtrl.Resource(ResourceKind.Stone).value / (float)game.upgradeCtrl.Upgrade(UpgradeKind.GoldCap, 0).transaction.cost[0].Cost();
        crystalFillImage.fillAmount = (float)game.resourceCtrl.Resource(ResourceKind.Crystal).value / (float)game.upgradeCtrl.Upgrade(UpgradeKind.GoldCap, 1).transaction.cost[0].Cost();
        leafFillImage.fillAmount = (float)game.resourceCtrl.Resource(ResourceKind.Leaf).value / (float)game.upgradeCtrl.Upgrade(UpgradeKind.GoldCap, 2).transaction.cost[0].Cost();
        slimeCoinFillImage.fillAmount = (float)game.resourceCtrl.slimeCoin.Percent();
        nitroFillImage.fillAmount = (float)game.nitroCtrl.nitro.Percent();
        guildFillImage.fillAmount = game.guildCtrl.ExpPercent();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) { TransactionCost.SwitchMultibuyNum(true); multibuyUI.SetUI(); }
        else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Semicolon) ) { TransactionCost.SwitchMultibuyNum(); multibuyUI.SetUI(); }

        if (popupUI.isShow && main.S.isDlcNitroPack)
        {
            //NitroSpeed
            if (isShiftPressed && Input.GetKeyDown(KeyCode.UpArrow))
                game.nitroCtrl.speed.Increase(0.1d);
            else if (isShiftPressed && Input.GetKeyDown(KeyCode.DownArrow))
                game.nitroCtrl.speed.Increase(-0.1d);
        }
    }

    string NitroPopupString()
    {
        string tempStr = optStr + "<size=20>Nitro Booster  ";
        if (game.nitroCtrl.isActive) tempStr += "<color=green>ON  [ Game Speed x" + tDigit(game.nitroCtrl.speed.value, 1) + " ]</color>";
        tempStr += "\n<size=18>- You gain nitro while offline";
        tempStr += "\n- Currently you have : " + tDigit(game.nitroCtrl.nitro.value, 1) + " Nitro";
        tempStr += "\n- Current Cap : " + tDigit(game.nitroCtrl.nitroCap.Value(), 1);
        tempStr += "\nEpic Store Item [Nitro Max Charger] can give nitro beyond the Nitro Cap.";//Cap is the maximum amount of nitro that can be gained at one time.\nYou can keep nitro that is gained over nitro cap.</color>";
        tempStr += "\n\n<u>Information</u>";
        tempStr += "\n- Click to switch ON/OFF";
        tempStr += "\n- While Nitro Booster ON, Game Speed is <color=green>x" + tDigit(game.nitroCtrl.speed.value, 1) + "</color> while consuming <color=green>" + tDigit(game.nitroCtrl.speed.value - 1, 1) + " Nitro per sec</color>";
        if (main.S.isDlcNitroPack) tempStr += "\n<color=yellow>- Shift + Up/Down arrow key to increase/decrease the speed.</color>";
        return tempStr;
    }
    string SlimeCoinPopupString()
    {
        string tempStr = optStr + "<size=20>Slime Coin<size=18>";
        tempStr += "\n- Gained while Gold is overflowing from Gold Cap";
        tempStr += "\n- Used for Slime Bank Upgrades, which is unlocked by Slime Bank at Guild Level 35";
        tempStr += "\n- Currently you have : " + tDigit(game.resourceCtrl.slimeCoin.value);
        tempStr += "\n- Gained in the last 10 minutes : <color=green>" + tDigit(game.resourceCtrl.slimeCoin.TotalGainInLastMinute()) + "</color>";
        tempStr += "\n- Current Cap : " + tDigit(game.resourceCtrl.slimeCoinCap.Value());
        tempStr += "\n- Gain Efficiency : <color=green>" + percent(game.resourceCtrl.slimeCoinEfficiency.Value(), 3) + "</color> of overflowed Gold";
        //Breakdowns
        //tempStr += "\n";//<u>" + localized.Basic(BasicWord.Additive) + "</u><size=18>";
        //for (int i = 0; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        //{
        //    if (game.resourceCtrl.slimeCoinEfficiency.Add((MultiplierKind)i) != 0)
        //    {
        //        tempStr += "\n- " + localized.StatsBreakdown((MultiplierKind)i);
        //        tempStr += " : " + 
        //    }
        //}
        if (game.resourceCtrl.slimeCoinEfficiency.Mul() <= 1) return tempStr;
        tempStr += "\n\n<u>Gain Efficiency Breakdowns</u>";
        tempStr += "\n- Base from Upgrade : " + percent(game.resourceCtrl.slimeCoinEfficiency.Add(), 3);
        tempStr += "\n- Town : * " + percent(game.resourceCtrl.slimeCoinEfficiency.Mul(), 2);
        return tempStr;
    }
    string GoldPopupString()
    {
        string tempStr = optStr + "<size=20>Gold</size><size=18>";
        tempStr += "\n- Gained when you defeat a monster";
        tempStr += "\n- Currently you have : " + tDigit(game.resourceCtrl.gold.value);
        tempStr += "\n- Gained in the last minute : <color=green>" + tDigit(game.resourceCtrl.gold.TotalGainInLastMinute()) + "</color>";
        tempStr += "\n- Total Gold Gained : " + tDigit(main.S.totalGold);
        tempStr += "\n- Current Cap : " + tDigit(game.resourceCtrl.goldCap.Value());
        tempStr += "\n\n<u>Gold Cap Breakdowns</u>";
        tempStr += optStr + "\n" + game.resourceCtrl.goldCap.BreakdownString();
        //Multiplier multi = game.resourceCtrl.goldCap;
        //tempStr += optStr + "\n- " + localized.StatsBreakdown(MultiplierKind.Base) + " : " + tDigit(multi.Add(MultiplierKind.Base));
        //for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        //{
        //    if (multi.Add((MultiplierKind)i) != 0)
        //        tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : + " + tDigit(multi.Add((MultiplierKind)i), NumberType.normal);
        //}
        //for (int i = 1; i < Enum.GetNames(typeof(MultiplierKind)).Length; i++)
        //{
        //    if (multi.Mul((MultiplierKind)i) != 1)
        //        tempStr += optStr + "\n- " + localized.StatsBreakdown((MultiplierKind)i) + " : * " + percent(multi.Mul((MultiplierKind)i), 3);
        //}
        return tempStr;
    }
    string ResourcePopupString(ResourceKind kind)
    {
        string tempStr = optStr + "<size=20>" + localized.ResourceName(kind) + "</size>";
        tempStr += "\n<size=18>- Currently you have : " + tDigit(game.resourceCtrl.Resource(kind).value);
        tempStr += "\n- Gained in the last minute : <color=green>" + tDigit(game.resourceCtrl.Resource(kind).TotalGainInLastMinute()) + "</color>";
        tempStr += "\n- Any monster drops <color=green>" + tDigit(game.statsCtrl.ResourceGain(kind).Value()) + "</color> " + localized.ResourceName(kind) +  " when defeated by ";
        switch (kind)
        {
            case ResourceKind.Stone:
                tempStr += localized.Hero(HeroKind.Warrior) + " or " + localized.Hero(HeroKind.Thief);
                break;
            case ResourceKind.Crystal:
                tempStr += localized.Hero(HeroKind.Wizard) + " or " + localized.Hero(HeroKind.Archer);
                break;
            case ResourceKind.Leaf:
                tempStr += localized.Hero(HeroKind.Angel) + " or " + localized.Hero(HeroKind.Tamer);
                break;
        }
        tempStr += "\n\n<u>Gain Breakdowns</u>";
        tempStr += optStr + "\n" + game.statsCtrl.ResourceGain(kind).BreakdownString();
        return tempStr;
    }
    string GuildPopupString()
    {
        string tempStr = optStr + "<size=20>" + localized.Basic(BasicWord.GuildLevel) + " : <color=green>Lv " + tDigit(game.guildCtrl.Level()) + "</color>";
        tempStr += "\n<size=18>- EXP : " + tDigit(game.guildCtrl.exp.value) + " / " + tDigit(game.guildCtrl.RequiredExp()) + "  ( " + percent(game.guildCtrl.ExpPercent(), 3) + " )";
        tempStr += "\n- Gained in the last hour : <color=green>" + tDigit(game.guildCtrl.exp.TotalGainInLastMinute())
             + " ( " + percent(game.guildCtrl.exp.TotalGainInLastMinute() / game.guildCtrl.RequiredExp(), 4) + " )</color>";
        tempStr += "\n- Guild EXP Gain when " + localized.Hero(game.currentHero) + " level up : <color=green>" + tDigit(game.guildCtrl.exp.ExpGain(game.statsCtrl.Level(game.currentHero), 1))
            + " ( " + percent(game.guildCtrl.exp.ExpGain(game.statsCtrl.Level(game.currentHero), 1) / game.guildCtrl.RequiredExp(), 4) + " )</color>";
        tempStr += "\n\n<u>Guild EXP Gain Breakdowns</u>";
        tempStr += optStr + "\n" + game.guildCtrl.expGain.BreakdownString(true);

        return tempStr;
    }
    string MultibuyPopupString()
    {
        string tempStr = optStr + "<size=20>Multiplier<size=18>";
        tempStr += "\n- You can use this to purchase or upgrade <color=green>x" + tDigit(TransactionCost.MultibuyNum()) + " times</color> at once";
        tempStr += "\n- Left-Click or \"+\" key to increase the amount";
        tempStr += "\n- Right-Click or \"-\" key to decrease the amount";
        return tempStr;
    }
}

public class MultibuyUI
{
    public MultibuyUI(Button button)
    {
        thisButton = button;
        thisText = button.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        //thisButton.onClick.AddListener(Change);
        SetUI();

        var clickAction = new ClickAction(button.gameObject, () => { TransactionCost.SwitchMultibuyNum(); SetUI(); }, () => { TransactionCost.SwitchMultibuyNum(true); SetUI(); });
        //var click = new EventTrigger.Entry();
        //click.eventID = EventTriggerType.PointerUp;
        //click.callback.AddListener((x) => Change());
        //thisButton.gameObject.GetComponent<EventTrigger>().triggers.Add(click);
    }
    //void Change()
    //{
    //    if (Input.GetMouseButtonUp(1))
    //        TransactionCost.SwitchMultibuyNum(true);
    //    else TransactionCost.SwitchMultibuyNum();
    //    SetUI();
    //}
    public void SetUI()
    {
        thisText.text = optStr + "x " + tDigit(TransactionCost.MultibuyNum());
    }
    Button thisButton;
    TextMeshProUGUI thisText;
}

