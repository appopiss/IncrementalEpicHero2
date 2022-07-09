using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;

public partial class Save
{
    public bool isUsedSimulationOnce;
}

public class AreaTableUI
{
    AreaKind kind;
    GameObject thisObject;
    Button closeButton;
    Button simulateButton;
    TextMeshProUGUI simulateText;
    Button[] dungeonSwitchButtons;
    public OpenCloseUI thisOpenClose;
    public AreaUI[] areasUI;
    EquipMenuUI eqMenuUI;
    public PopupUI lockUniqueEQPopupUI;
    SwitchTabUI dungeonSwitchTabUI;
    public bool isDungeon { get => dungeonSwitchTabUI.currentId > 0; }
    public PopupUI simulationPopupUI;

    public AreaTableUI(GameObject thisObject, GameObject areaTableObject, Button quitButton, Button[] dungeonSwitchButtons, Button simulateButton)
    {
        this.thisObject = thisObject;
        lockUniqueEQPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        this.dungeonSwitchButtons = dungeonSwitchButtons;
        dungeonSwitchTabUI = new SwitchTabUI(this.dungeonSwitchButtons, true, -1, SetUI);
        areasUI = new AreaUI[areaTableObject.transform.childCount];
        for (int i = 0; i < areasUI.Length; i++)
        {
            int count = i;
            areasUI[i] = new AreaUI(this, areaTableObject.transform.GetChild(count).gameObject, () => kind, () => count);
        }
        closeButton = quitButton;
        this.simulateButton = simulateButton;
        simulateText = simulateButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        simulateButton.onClick.AddListener(() => { main.S.isUsedSimulationOnce = true; simulateText.color = Color.white; game.simulationCtrl.Simulate(kind, isDungeon); });
        SetOpenClose();
        this.dungeonSwitchButtons[0].onClick.Invoke();
        eqMenuUI = gameUI.menuUI.MenuUI(MenuKind.Equip).GetComponent<EquipMenuUI>();
        simulationPopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        simulationPopupUI.SetTargetObject(simulateButton.gameObject, () => SimulationPopupString());
    }

    int countSec;
    bool isGreen;
    public void UpdateUI()
    {
        if (!thisOpenClose.isOpen) return;

        if (!main.S.isUsedSimulationOnce)
        {
            countSec++;
            if (countSec > 50)
            {
                if (isGreen)
                {
                    simulateText.color = Color.white;
                    isGreen = false;
                }
                else
                {
                    simulateText.color = Color.green;
                    isGreen = true;
                }
                countSec = 0;
            }
        }

        lockUniqueEQPopupUI.UpdateUI();
        eqMenuUI.dictionaryUI.popupUI.UpdateUI();
        simulateButton.interactable = !game.simulationCtrl.isSimulating;
        for (int i = 0; i < areasUI.Length; i++)
        {
            areasUI[i].UpdateUI();
        }
        simulationPopupUI.UpdateUI();
    }
    public void Update()
    {
        if (!thisOpenClose.isOpen) return;
        if (GameControllerUI.isShiftPressed && Input.GetKeyDown(KeyCode.S)) simulateButton.onClick.Invoke();
    }
    public void SetUI(AreaKind kind)
    {
        this.kind = kind;
        SetUI();
    }
    void SetUI()
    {
        for (int i = 0; i < areasUI.Length; i++)
        {
            areasUI[i].SetUI();
        }
    }
    void SetOpenClose()
    {
        thisOpenClose = new OpenCloseUI(thisObject, true, true);
        for (int i = 0; i < gameUI.worldMapUI.areaObjects.Length; i++)
        {
            thisOpenClose.SetOpenButton(gameUI.worldMapUI.areaObjects[i].GetComponent<Button>());
        }
        thisOpenClose.SetCloseButton(closeButton);
        thisOpenClose.SetCloseButton(gameUI.worldMapUI.closeButton);
        thisOpenClose.openActions.Add(() => gameUI.worldMapUI.UpdateText((int)kind));
        thisOpenClose.closeActions.Add(() => gameUI.worldMapUI.UpdateText(-1));
    }

    string SimulationPopupString()
    {
        if (game.simulationCtrl.isSimulating) return "Now Simulating...";
        return "You can simulate the results of battles in each area at once.\n<size=18>Hotkey : Shift + S";
    }
}
public class AreaUI
{
    AreaTableUI areaTableUI;
    Func<AreaKind> kind;
    Func<int> id;
    public Func<AREA> thisArea;
    public GameObject thisObject;
    public GameObject uniqueEQobject;
    GameObject uniqueEQquestion;
    public Equipment uniqueEQ { get => game.equipmentCtrl.equipments[(int)thisArea().uniqueEquipmentKind]; }
    Button thisButton;
    public Button areaInfoButton, leftButton, rightButton;
    TextMeshProUGUI description;
    GameObject lockObject;
    TextMeshProUGUI lockText;
    Image backgroundImage;
    static Color areaColor = new Color(0, 50 / 255f, 50 / 255f);
    static Color dungeonColor = new Color(50 / 255f, 0, 50 / 255f);

    public AreaUI(AreaTableUI areaTableUI, GameObject thisObject, Func<AreaKind> kind, Func<int> id)
    {
        this.areaTableUI = areaTableUI;
        this.thisObject = thisObject;
        this.kind = kind;
        this.id = id;
        thisArea = () =>
        {
            if (areaTableUI.isDungeon)
            {
                if (id() < game.areaCtrl.dungeons[(int)kind()].Length) return game.areaCtrl.Dungeon(kind(), id());
                else return game.areaCtrl.nullArea;
            }
            if (id() < game.areaCtrl.areas[(int)kind()].Length) return game.areaCtrl.Area(kind(), id());
            else return game.areaCtrl.nullArea;
        };
        thisButton = thisObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        backgroundImage = thisButton.gameObject.GetComponent<Image>();
        description = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        uniqueEQobject = thisObject.transform.GetChild(2).gameObject;
        uniqueEQquestion = uniqueEQobject.transform.GetChild(1).gameObject;
        areaInfoButton = thisObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        leftButton = thisObject.transform.GetChild(4).gameObject.GetComponent<Button>();
        rightButton = thisObject.transform.GetChild(5).gameObject.GetComponent<Button>();
        lockObject = thisObject.transform.GetChild(6).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        leftButton.onClick.AddListener(() => ChangeAreaLevel(-1));
        rightButton.onClick.AddListener(() => ChangeAreaLevel(1));
        thisButton.onClick.AddListener(StartArea);
        thisButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.AreaSelect));
        SetUI();
        areaTableUI.lockUniqueEQPopupUI.SetTargetObject(uniqueEQquestion, UniqueEQLockString);
    }
    void StartArea()
    {
        if (thisArea() == game.areaCtrl.nullArea) return;
        if (thisArea().isDungeon) game.battleCtrl.areaBattle.Start(thisArea());
        else game.battleCtrl.areaBattle.Start(kind(), id());
        areaTableUI.thisOpenClose.Close();
        gameUI.worldMapUI.thisOpenClose.Close();
    }
    public void SetUI()
    {
        if (thisArea() == game.areaCtrl.nullArea) return;
        
        if (thisArea().isDungeon)
        {
            backgroundImage.color = dungeonColor;
            return;
        }
        uniqueEQobject.GetComponent<Image>().sprite = sprite.equipments[(int)thisArea().uniqueEquipmentKind];
        backgroundImage.color = areaColor;
    }
    public void UpdateUI()
    {
        SetActive(thisObject, thisArea() != game.areaCtrl.nullArea);
        if (thisArea() == game.areaCtrl.nullArea) return;

        SetActive(lockObject, !thisArea().IsUnlocked());
        thisButton.interactable = thisArea().CanStart();
        if (!thisArea().IsUnlocked())
        {
            lockText.text = LockString();
            return;
        }
        description.text = DescriptionString();
        SetActive(uniqueEQobject, !thisArea().isDungeon);
        SetActive(uniqueEQquestion, !uniqueEQ.globalInfo.isGotOnce);
        SetActive(leftButton.gameObject, main.S.ascensionNum[0] > 0);
        SetActive(rightButton.gameObject, main.S.ascensionNum[0] > 0);
        if (main.S.ascensionNum[0] > 0)
        {
            leftButton.interactable = !thisArea().level.IsMined();
            rightButton.interactable = !thisArea().level.IsMaxed();
        }
    }
    public void ChangeAreaLevel(long levelIncrement)
    {
        bool[] tempIsActive = new bool[game.battleCtrls.Length];
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            tempIsActive[i] = game.battleCtrls[i].isActiveBattle;
            if (tempIsActive[i] && game.battleCtrls[i].areaBattle.CurrentArea() == thisArea())
                game.battleCtrls[i].isActiveBattle = false;
        }
        thisArea().level.Increase(levelIncrement);
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            if (tempIsActive[i] && game.battleCtrls[i].areaBattle.CurrentArea() == thisArea())
            {
                game.battleCtrls[i].areaBattle.Start();
                game.battleCtrls[i].isActiveBattle = true;
            }
        }
        //既にSimulateしている場合はSimulateしなおす
        if (thisArea().isSimulated)
        {
            game.simulationCtrl.Simulate(thisArea());
        }
    }
    string LockString()
    {
        string tempString = optStr + "<size=20>";
        tempString += optStr + "<sprite=\"locks\" index=0> Required : ";
        foreach (var item in thisArea().requiredCompleteNum)
        {
            tempString += optStr + "\n- " + localized.Basic(BasicWord.Area) + " " + tDigit(item.Key + 1) + " Completed # " + tDigit(item.Value);
        }
        return tempString;
    }
    string UniqueEQLockString()
    {
        return optStr + "Unique Equipment\n- Drop Chance : <color=green>" + percent(thisArea().uniqueEquipmentDropChance.Value(), 3);
    }
    string tempString;
    double rewardTownMat;
    string DescriptionString()
    {
        tempString = optStr + "<size=20>";
        tempString += optStr + thisArea().Name(false) + "  ( Monster Level : Lv" + tDigit(thisArea().MinLevel()) + " ~ Lv " + tDigit(thisArea().MaxLevel()) + " )  ";
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            if (game.battleCtrls[i].isActiveBattle && game.battleCtrls[i].areaBattle.CurrentArea() == thisArea())
                tempString += optStr + "<sprite=\"heroes\" index=" + i + ">";
        }
        tempString += optStr + "\n- Completed # <color=green>" + tDigit(thisArea().completedNum.value) + "</color>  ( Total <color=green>" + tDigit(thisArea().completedNum.TotalCompletedNum()) + "</color> )";
        if (thisArea().isDungeon) tempString += "\n- You need " + tDigit(thisArea().RequiredPortalOrb()) + " " + localized.Basic(BasicWord.PortalOrb) + " to enter this dungeon";
        else
        {
            tempString += optStr + "  [ Mission : " + tDigit(thisArea().ClearedMissionNum(thisArea().level.value, true)) + " / " + tDigit(thisArea().missionUnlockedNum.Value()) + " ]";
            int count = 0;

            double clearMultiplier = 1 + thisArea().ClearCountBonus(game.currentHero);
            foreach (var item in thisArea().rewardMaterial)
            {
                if (count < 1)
                {
                    rewardTownMat = item.Value(game.currentHero) * clearMultiplier;
                    tempString += optStr + "\n- Reward : <color=green>" + tDigit(rewardTownMat) + " " + item.Key.Name() + "</color>";
                }
                else if (count == 1)
                {
                    if (thisArea().id == 7)
                    {
                        rewardTownMat = item.Value(game.currentHero) * clearMultiplier;
                        tempString += "<color=green>, " + tDigit(rewardTownMat) + " of each Town Mat</color>";
                    }
                    break;
                }
                count++;
            }
        }
        //else tempString += "\n- Reward per clear : <color=green>" + tDigit(thisArea().rewardMaterial .Value()) + " " + item.Key.Name() + "</color>";
        //else tempString += "\n- Unique Equipment Drop Chance : <color=green>" + percent(thisArea().uniqueEquipmentDropChance.Value(), 3) + "</color>";
        if (thisArea().isSimulated)
        {
            double tempTownMatPerSec = rewardTownMat / thisArea().simulatedTime;
            if (thisArea().simulatedIsClear && !thisArea().isDungeon && tempTownMatPerSec >= 0.1)
                tempString += " (<color=green>" + tDigit(tempTownMatPerSec, 1) + "/s</color>)";
            tempString += "\n";
            if (thisArea().simulatedIsClear)
                tempString += "<color=green>Clearable</color> ( Time : <color=green>" + DoubleTimeToDate(thisArea().simulatedTime) + "</color>,  Gold/sec : <color=green>" + tDigit(thisArea().simulatedGoldPerSec, 1) + "</color>,  EXP/sec : <color=green>" + tDigit(thisArea().simulatedExpPerSec, 1) + "</color> )</color>";
            else tempString += "<color=red>Currently almost impossible to clear ( Wave Reached :  " + tDigit(thisArea().simulatedWave) + " )";
        }
        return tempString;
    }
}
