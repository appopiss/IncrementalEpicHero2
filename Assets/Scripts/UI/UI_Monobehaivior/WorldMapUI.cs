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

public class WorldMapUI : MonoBehaviour
{
    [NonSerialized] public OpenCloseUI thisOpenClose;
    public Button openButton;
    public Button closeButton;
    public TextMeshProUGUI topText;
    [SerializeField] GameObject missionMilestoneIcon;
    [SerializeField] TextMeshProUGUI portalCrystalText;

    public GameObject[] areaObjects;//[AreaKind]
    Button[] areaButtons;
    GameObject[][] areaClassIcons;//[AreaKind][HeroKind]
    public GameObject areaTableShow, areaTable;
    public Button areaTableQuitButton;
    public Button[] dungeonSwitchButtons;
    public Button simulateButton;
    public AreaTableUI areaTableUI;

    //Popup
    PopupUI missionMilestonePopupUI;
    WorldMapPopupUI worldmapPopupUI;

    [SerializeField] GameObject areaInfo;
    public AreaInfoUI areaInfoUI;

    private void Awake()
    {
        areaButtons = new Button[areaObjects.Length];
        for (int i = 0; i < areaButtons.Length; i++)
        {
            areaButtons[i] = areaObjects[i].GetComponent<Button>();
        }
        areaClassIcons = new GameObject[areaObjects.Length][];
        for (int i = 0; i < areaObjects.Length; i++)
        {
            areaClassIcons[i] = new GameObject[areaObjects[i].transform.GetChild(0).transform.childCount];
            for (int j = 0; j < areaClassIcons[i].Length; j++)
            {
                int count = j;
                areaClassIcons[i][j] = areaObjects[i].transform.GetChild(0).transform.GetChild(count).gameObject;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        areaTableUI = new AreaTableUI(areaTableShow, areaTable, areaTableQuitButton, dungeonSwitchButtons, simulateButton);
        for (int i = 0; i < areaButtons.Length; i++)
        {
            int count = i;
            areaButtons[count].onClick.AddListener(() => areaTableUI.SetUI((AreaKind)count));
            areaButtons[count].onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Map));
        }

        areaInfoUI = new AreaInfoUI(areaInfo);
        for (int i = 0; i < areaTableUI.areasUI.Length; i++)
        {
            int count = i;
            areaInfoUI.thisOpenClose.SetOpenButton(areaTableUI.areasUI[count].areaInfoButton);
            areaTableUI.areasUI[count].areaInfoButton.onClick.AddListener(() => { areaInfoUI.thisArea = () => areaTableUI.areasUI[count].thisArea(); areaInfoUI.SetUI(); });
            areaTableUI.areasUI[count].areaInfoButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.AreaInfo));
        }

        SetOpenClose();
        openButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Map));

        //Popup
        missionMilestonePopupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        missionMilestonePopupUI.SetTargetObject(missionMilestoneIcon, MissionMilestoneString);
        worldmapPopupUI = new WorldMapPopupUI(gameUI.popupCtrlUI.defaultPopup);
        for (int i = 0; i < areaObjects.Length; i++)
        {
            int count = i;
            worldmapPopupUI.SetTargetObject(areaObjects[count], () => worldmapPopupUI.SetUI(() => PopupDescription(count)));
        }
        for (int i = 0; i < areaInfoUI.monsters.Length; i++)
        {
            int count = i;
            gameUI.battleCtrlUI.monsterStatsPopupUI.SetTargetObject(areaInfoUI.monsters[count], () => gameUI.battleCtrlUI.monsterStatsPopupUI.SetUI(() => areaInfoUI.monsterBattles[count]));
        }
        for (int i = 0; i < areaInfoUI.prestigeUpradesUI.Length; i++)
        {
            int count = i;
            gameUI.battleCtrlUI.areaPrestigeUpgradePopupUI.SetTargetObject(areaInfoUI.prestigeUpgrades[count], () => gameUI.battleCtrlUI.areaPrestigeUpgradePopupUI.ShowAction(areaInfoUI.prestigeUpradesUI[count].upgrade));
        }
    }

    public void UpdateUI()
    {
        if (!thisOpenClose.isOpen) return;
        for (int i = 0; i < areaButtons.Length; i++)
        {
            int count = i;
            areaButtons[i].interactable = game.areaCtrl.IsUnlocked((AreaKind)count);
        }
        missionMilestonePopupUI.UpdateUI();
        worldmapPopupUI.UpdateUI();
        areaTableUI.UpdateUI();
        portalCrystalText.text = optStr + "You have <color=green>" + tDigit(game.areaCtrl.portalOrb.value) + "</color> " + localized.Basic(BasicWord.PortalOrb);
        areaInfoUI.UpdateUI();

        //AreaClassIcon
        for (int i = 0; i < areaObjects.Length; i++)
        {
            int countI = i;
            for (int j = 0; j < areaClassIcons[i].Length; j++)
            {
                int countJ = j;
                SetActive(areaClassIcons[i][j], IsActiveClassIcon((AreaKind)countI, (HeroKind)countJ));
            }
        }
    }
    private void Update()
    {
        areaTableUI.Update();
    }
    bool IsActiveClassIcon(AreaKind areaKind, HeroKind heroKind)
    {
        if (!game.battleCtrls[(int)heroKind].isActiveBattle) return false;
        if (game.battleCtrls[(int)heroKind].areaBattle.currentAreaKind != areaKind) return false;
        return true;
    }


    void SetOpenClose()
    {
        thisOpenClose = new OpenCloseUI(gameObject, true);
        thisOpenClose.SetOpenButton(openButton);
        thisOpenClose.SetCloseButton(closeButton);
        closeButton.onClick.AddListener(() => areaInfoUI.thisOpenClose.Close());
    }

    //void SetChangeText()
    //{
    //    for (int i = 0; i < areaObjects.Length; i++)
    //    {
    //        int count = i;
    //        var entry = new EventTrigger.Entry();
    //        var exit = new EventTrigger.Entry();
    //        entry.eventID = EventTriggerType.PointerEnter;
    //        exit.eventID = EventTriggerType.PointerExit;
    //        entry.callback.AddListener((data) => UpdateText(count));
    //        exit.callback.AddListener((data) => UpdateText(-1));
    //        areaObjects[i].AddComponent<EventTrigger>().triggers.Add(entry);
    //        areaObjects[i].GetComponent<EventTrigger>().triggers.Add(exit);
    //    }
    //}

    public void UpdateText(int id)
    {
        topText.text = TopTextString(id);
    }
    string TopTextString(int id)
    {
        if (id == -1) return localized.Basic(BasicWord.WorldMap);
        return localized.AreaName((AreaKind)id);
    }

    public string PopupDescription(int id)
    {
        if (!game.areaCtrl.IsUnlocked((AreaKind)id)) return optStr + "<sprite=\"locks\" index=0> Building [Cartographer] Lv " + tDigit(Cartographer.areaUnlockLevels[id]);
        return optStr + "<size=20>" + localized.AreaName((AreaKind)id);
    }

    string MissionMilestoneString()
    {
        string tempStr = optStr;
        tempStr += "<size=24>Area Mission Milestone <size=20> ( Total Cleared # <color=green>" + tDigit(game.missionCtrl.clearNum()) + "</color> )";
        for (int i = 0; i < game.missionCtrl.milestoneEffectList.Count; i++)
        {
            tempStr += "\n" + game.missionCtrl.milestoneEffectList[i].DescriptionString();
        }
        return tempStr;
    }
}

public class WorldMapPopupUI : PopupUI
{
    public WorldMapPopupUI(GameObject thisObject) : base(thisObject)
    {
    }
}