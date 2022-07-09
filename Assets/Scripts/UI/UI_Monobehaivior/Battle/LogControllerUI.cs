using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static GameController;
using static UsefulMethod;
using static Localized;
using static Main;
using Cysharp.Threading.Tasks;

public class LogControllerUI : MonoBehaviour
{
    Func<BattleController> battleCtrl;
    public GameObject[] textObjects;
    LogText[] logTexts;
    ArrayId id;

    private void Awake()
    {
        battleCtrl = () => game.battleCtrl;
        logTexts = new LogText[textObjects.Length];
        for (int i = 0; i < logTexts.Length; i++)
        {
            logTexts[i] = new LogText(textObjects[i]);
        }
        id = new ArrayId(0, () => logTexts.Length);
    }
    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            game.statsCtrl.Exp(heroKind).logUIAction = SetLogUIExp;
        }
        game.resourceCtrl.gold.logUIAction = SetLogUIGold;
        game.guildCtrl.level.logUIAction = () => { if (SettingMenuUI.Toggle(ToggleKind.DisableGuildLog).isOn) return; Log("<color=green>Guild Level Up!!!", 6f); };
        game.guildCtrl.exp.logUIAction = (increment) => { if (SettingMenuUI.Toggle(ToggleKind.DisableGuildLog).isOn) return; Log(optStr + "<color=orange>Guild EXP + " + tDigit(increment), 6f); };
        game.resourceCtrl.Resource(ResourceKind.Stone).logUIAction = SetLogUIStone;
        game.resourceCtrl.Resource(ResourceKind.Crystal).logUIAction = SetLogUICrystal;
        game.resourceCtrl.Resource(ResourceKind.Leaf).logUIAction = SetLogUILeaf;
        for (int i = 0; i < Enum.GetNames(typeof(MaterialKind)).Length; i++)
        {
            game.materialCtrl.Material((MaterialKind)i).logUIAction = ShowMaterialLog;
        }
    }
    public void UpdateUI()
    {
        count++;
        if (count >= 5)
        {
            ShowWaitLog();
            count = 0;
        }
    }
    int count;
    public void Log(string text, float showtimesec = 0, bool isForcedShow = false)
    {
        if (!isForcedShow && SettingMenuUI.Toggle(ToggleKind.DisableAnyLog).isOn) return;
        if (showtimesec == 0) showtimesec = Parameter.defaultLogShowTimesec;
        logTexts[id.value].SetInfo(text, showtimesec);
        id.Increase();
    }

    //Gold,Expは数フレーム待って合計値を出す（あとでこの間隔を制御できるようにする,Settingで。）
    double tempGold, tempExp, tempStone, tempCrystal, tempLeaf;
    void SetLogUIGold(double value)
    {
        tempGold += value;
    }
    void SetLogUIExp(double value)
    {
        tempExp += value;
    }
    void SetLogUIStone(double value)
    {
        tempStone += value;
    }
    void SetLogUICrystal(double value)
    {
        tempCrystal += value;
    }
    void SetLogUILeaf(double value)
    {
        tempLeaf += value;
    }
    void ShowMaterialLog(MaterialKind kind, double value)
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableMaterialLog]) return;
        string tempStr = optStr + localized.Basic(BasicWord.Gained) + "<color=green>" + " " + localized.Material(kind) + " * " + tDigit(value);
        Log(tempStr, Parameter.defaultLogShowTimesec);
    }
    void ShowGoldLog()
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableGoldLog]) return;
        if (tempGold <= 0) return;
        string tempStr = optStr + localized.Basic(BasicWord.Gold) +  " + " + tDigit(tempGold);
        Log(tempStr, Parameter.defaultLogShowTimesec);
        tempGold = 0;
    }
    void ShowExpLog()
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableExpLog]) return;
        if (tempExp <= 0) return;
        string tempStr = optStr + "EXP + " + tDigit(tempExp);
        Log(tempStr, Parameter.defaultLogShowTimesec);
        tempExp = 0;
    }
    void ShowStoneLog()
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableResourceLog]) return;
        if (tempStone <= 0) return;
        string tempStr = optStr + localized.Basic(BasicWord.Gained) + "<color=green>" + " " + localized.ResourceName(ResourceKind.Stone) +  " * " + tDigit(tempStone);
        Log(tempStr, Parameter.defaultLogShowTimesec);
        tempStone = 0;
    }
    void ShowCrystalLog()
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableResourceLog]) return;
        if (tempCrystal <= 0) return;
        string tempStr = optStr + localized.Basic(BasicWord.Gained) + "<color=green>" + " " + localized.ResourceName(ResourceKind.Crystal) + " * " + tDigit(tempCrystal);
        Log(tempStr, Parameter.defaultLogShowTimesec);
        tempCrystal = 0;
    }
    void ShowLeafLog()
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableResourceLog]) return;
        if (tempLeaf <= 0) return;
        string tempStr = optStr + localized.Basic(BasicWord.Gained) + "<color=green>" + " " + localized.ResourceName(ResourceKind.Leaf) + " * " + tDigit(tempLeaf);
        Log(tempStr, Parameter.defaultLogShowTimesec);
        tempLeaf = 0;
    }
    void ShowWaitLog()//5フレームごとに呼ぶ
    {
        ShowGoldLog();
        ShowExpLog();
        ShowStoneLog();
        ShowCrystalLog();
        ShowLeafLog();
    }


}

public class LogText
{
    private readonly int fadeoutsmoothness = 10;//= 10 frame/sec
    private float showTime = 3.0f;//Default Showing Time
    private TextMeshProUGUI thisText;
    private GameObject thisObject;
    public bool isActive;

    public LogText(GameObject textObject)
    {
        thisObject = textObject;
        thisText = textObject.GetComponent<TextMeshProUGUI>();
        thisText.color = Color.clear;
    }
    public void SetInfo(string text, float timesec)
    {
        isActive = true;
        thisObject.transform.SetSiblingIndex(0);
        thisText.color = Color.white;
        thisText.text = text;
        showTime = timesec;
        FadeOut();
    }
    async void FadeOut()
    {
        await UniTask.Delay(500);
        while (true)
        {
            if (isActive)
            {
                await UniTask.Delay(1000 / fadeoutsmoothness);
                thisText.color -= Color.black / showTime / (float)fadeoutsmoothness;
                if (thisText.color.a <= 0)
                {
                    thisText.color = Color.clear;
                    isActive = false;
                }
            }
            else
                break;
        }
    }
}
