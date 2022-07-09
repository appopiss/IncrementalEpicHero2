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

public enum MenuKind
{
    Ability,
    Quest,
    Skill,
    Upgrade,
    Equip,
    Lab,
    Guild,
    Bestiary,
    Town,
    Shop,
    Rebirth,
    Challenge,
    Expedition,
    Ascension,
    Setting,
}
public class MenuControllerUI : MonoBehaviour
{
    public Button[] menuButtons;
    TextMeshProUGUI[] menuTexts;
    GameObject[] menuIconIs;
    public MENU_UI[] menuesUI;

    public MENU_UI MenuUI(MenuKind kind)
    {
        return menuesUI[(int)kind];
    }

    public MENU_UI CurrentOpenedMenuUI()
    {
        for (int i = 0; i < menuesUI.Length; i++)
        {
            if (menuesUI[i].openClose.isOpen) return menuesUI[i];
        }
        return menuesUI[0];
    }

    private void Awake()
    {
        menuTexts = new TextMeshProUGUI[menuButtons.Length];
        for (int i = 0; i < menuTexts.Length; i++)
        {
            menuTexts[i] = menuButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
        menuIconIs = new GameObject[menuButtons.Length];
        for (int i = 0; i < menuIconIs.Length; i++)
        {
            menuIconIs[i] = menuButtons[i].gameObject.transform.GetChild(1).gameObject;
        }

        for (int i = 0; i < menuesUI.Length; i++)
        {
            menuesUI[i].menuKind = (MenuKind)i;
            menuesUI[i].SetOpenClose();
        }
        //menuesUI[0].openClose.Open();
        //これはInitializeに書くことにした
        //for (int i = 0; i < menuButtons.Length; i++)
        //{
        //    int count = i;
        //    menuButtons[i].onClick.AddListener(() => { gameUI.soundUI.Play(SoundEffect.Click);});
        //}
    }
    private void Start()
    {
        //OpenAction(0);
    }
    public void OpenAction(int id)
    {
        for (int i = 0; i < menuesUI.Length; i++)
        {
            int count = i;
            SetActive(menuesUI[count].gameObject, count == id);
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < menuesUI.Length; i++)
        {
            menuesUI[i].Initialize();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int tempId = currentOpenedMenuId;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                for (int i = 0; i < menuesUI.Length; i++)
                {
                    if (currentOpenedMenuId <= 0)
                    {
                        tempId = menuesUI.Length - 1;//Setting
                        break;
                    }
                    tempId--;
                    if (IsUnlocked((MenuKind)tempId)) break;
                }
            }
            else
            {
                for (int i = 0; i < menuesUI.Length; i++)
                {
                    if (currentOpenedMenuId >= menuesUI.Length - 1)
                    {
                        tempId = 0;//Ability
                        break;
                    }
                    tempId++;
                    if (IsUnlocked((MenuKind)tempId)) break;
                }
            }
            menuButtons[tempId].onClick.Invoke();
            currentOpenedMenuId = tempId;
        }
    }
    public int currentOpenedMenuId;
    public void UpdateUI()
    {
        //Multiplierx1などのtoggleのため、常に呼ぶ必要がある
        SettingMenuUI.popupUI.UpdateUI();
        SkillMenuUI.skillPopupUI.UpdateUI();

        for (int i = 0; i < menuesUI.Length; i++)
        {
            int count = i;
            menuButtons[i].interactable = IsUnlocked((MenuKind)count);
            if (IsUnlocked((MenuKind)count))
                menuTexts[i].text = localized.Menu((MenuKind)count);
            else
                menuTexts[i].text = "???";
            if (menuesUI[i].openClose.isOpen)
            {
                menuesUI[i].UpdateUI();
                currentOpenedMenuId = i;
            }
        }

        //重さ軽減
        count++;
        if (count > 60)
        {
            SetActive(menuIconIs[(int)MenuKind.Quest], !SettingMenuUI.Toggle(ToggleKind.DisableNotificationQuest).isOn && game.questCtrl.IsExistCanClaimQuest(game.currentHero));
            SetActive(menuIconIs[(int)MenuKind.Lab], IsUnlocked(MenuKind.Lab) && !SettingMenuUI.Toggle(ToggleKind.DisableNotificationLab).isOn && game.alchemyCtrl.CanExpandMysteriousWater());
            SetActive(menuIconIs[(int)MenuKind.Expedition], IsUnlocked(MenuKind.Expedition) && !SettingMenuUI.Toggle(ToggleKind.DisableNotificationExpedition).isOn && game.expeditionCtrl.CanClaim());
            count = 0;
        }
    }
    int count;

    bool IsUnlocked(MenuKind kind)
    {
        switch (kind)
        {
            case MenuKind.Skill:
                return game.questCtrl.Quest(QuestKindGlobal.ClearGeneralQuest).isCleared;
            case MenuKind.Upgrade:
                return game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest).isCleared;
            case MenuKind.Equip:
                return game.questCtrl.Quest(QuestKindGlobal.UpgradeResource).isCleared;
            case MenuKind.Lab:
                return game.questCtrl.Quest(QuestKindGlobal.Equip).isCleared;
            case MenuKind.Guild:
                return game.questCtrl.Quest(QuestKindGlobal.Alchemy).isCleared;
            case MenuKind.Ability:
                break;
            case MenuKind.Quest:
                break;
            case MenuKind.Setting:
                break;
            case MenuKind.Bestiary:
                return game.questCtrl.Quest(QuestKindGlobal.Alchemy).isCleared;
            case MenuKind.Rebirth:
                return game.questCtrl.Quest(QuestKindGlobal.Research).isCleared;
            case MenuKind.Challenge:
                return game.questCtrl.Quest(QuestKindGlobal.Rebirth).isCleared;
            case MenuKind.Ascension:
                return game.questCtrl.Quest(QuestKindGlobal.Expedition).isCleared;
            case MenuKind.Town:
                return game.questCtrl.Quest(QuestKindGlobal.Guild).isCleared;
            case MenuKind.Shop:
                return game.questCtrl.Quest(QuestKindGlobal.Guild).isCleared;
            case MenuKind.Expedition:
                return game.questCtrl.Quest(QuestKindGlobal.Challenge).isCleared;
        }
        return true;
    }
}

public class MENU_UI : MonoBehaviour
{
    [NonSerialized] public OpenCloseUI openClose;
    [NonSerialized] public MenuKind menuKind;
    public virtual void SetOpenClose()
    {        
        openClose = new OpenCloseUI(gameObject, false, false, true);
        for (int i = 0; i < gameUI.menuUI.menuButtons.Length; i++)
        {
            if (i == (int)menuKind) openClose.SetOpenButton(gameUI.menuUI.menuButtons[(int)menuKind]);
            else openClose.SetCloseButton(gameUI.menuUI.menuButtons[i]);
        }
        openClose.openActions.Add(() => gameUI.menuUI.OpenAction((int)menuKind));
    }
    public virtual void UpdateUI()
    {
        if (!openClose.isOpen || gameUI.helpCtrlUI.openCloseUI.isOpen || gameUI.epicStorUI.openCloseUI.isOpen) return;
    }

    public virtual void Initialize() { }
}