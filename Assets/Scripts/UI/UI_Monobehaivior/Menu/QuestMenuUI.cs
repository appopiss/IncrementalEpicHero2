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

public class QuestMenuUI : MENU_UI
{
    private void Awake()
    {
        selectKindButtonQuestIconObjects = new GameObject[selectKindButtons.Length];
        for (int i = 0; i < selectKindButtonQuestIconObjects.Length; i++)
        {
            selectKindButtonQuestIconObjects[i] = selectKindButtons[i].gameObject.transform.GetChild(1).gameObject;
        }

        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
    }
    public void Start()
    {
        questUI = new QuestUI(this, information);
        buttonTableUI = new QuestButtonTableUI(this, buttonTable);

        kindSwitchTabUI = new SwitchTabUI(selectKindButtons, true, -1, SwitchButton);
        selectKindButtons[0].onClick.Invoke();
        buttonTableUI.questButtonsUI[0].thisButton.onClick.Invoke();
        hideCompletedQuestToggleUI = SettingMenuUI.Toggle(ToggleKind.HideCompletedQuest);

        questingButton.onClick.AddListener(() => game.battleCtrl.areaBattle.Start(questUI.thisQuest().questingArea));

        popupUI.SetTargetObject(acceptableNumText.gameObject, () => AcceptableTextPopupString());
        popupUI.SetTargetObject(questUI.toTitleButton.gameObject, () => ToTitleButtonPopupString());
        popupUI.SetTargetObject(masteryIcon, () => questUI.MasteryPopupString());
        popupUI.SetTargetObject(questingButton.gameObject, () => QuestingPopupString());
    }
    string QuestingPopupString()
    {
        AREA area = questUI.thisQuest().questingArea;
        string tempStr = optStr + "<size=20>Click to go to ";
        if (!area.CanStart()) tempStr += "<sprite=\"locks\" index=0> ";
        tempStr += area.Name(true, true);
        if (area.isDungeon) tempStr += optStr + "\n<size=18><color=yellow>You need " + tDigit(area.RequiredPortalOrb()) + " Portal Orb to enter this area (You have " + tDigit(game.areaCtrl.portalOrb.value) + ")</color>";
        return tempStr;
    }
    string ToTitleButtonPopupString()
    {
        string tempStr = optStr;
        if (currentOpenQuestKind == QuestKind.Title) tempStr = "Go to see Titles that you acquired.";
        else if(currentOpenQuestKind == QuestKind.General)
        {
            if (game.epicStoreCtrl.Item(EpicStoreKind.FavoriteQuest).IsPurchased())
            {
                if (questUI.thisQuest().isFavorite) tempStr += "Remove the Favorite";
                else tempStr += "Assign the Favorite";
            }
            else tempStr += "<sprite=\"locks\" index=0> [Favorite Quest] in Epic Store";
        }
        return tempStr;
    }
    string AcceptableTextPopupString()
    {
        string tempStr = optStr;
        tempStr += "<size=18>Title Quests and General Quests are limited to number of Accepted Quests." +
            "\nYou can increase the limit through a Rebirth upgrade.";
        return tempStr;
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        selectKindButtons[2].interactable = game.questCtrl.Quest(QuestKindGlobal.ClearGeneralQuest).isCleared;
        selectKindButtons[1].interactable = game.questCtrl.Quest(QuestKindGlobal.ClearTitleQuest).isCleared;
        selectKindButtons[3].interactable = game.questCtrl.Quest(QuestKindGlobal.AbilityVIT).isCleared;

        for (int i = 0; i < selectKindButtonQuestIconObjects.Length; i++)
        {
            int count = i;
            SetActive(selectKindButtonQuestIconObjects[count], game.questCtrl.IsExistCanClaimQuest((QuestKind)count, game.currentHero));
        }
        if (currentOpenQuestKind == QuestKind.Global || currentOpenQuestKind == QuestKind.Daily) acceptableNumText.text = "";
        else acceptableNumText.text = AcceptedNumString();
        SetActive(masteryIcon, currentOpenQuestKind == QuestKind.General);
        buttonTableUI.UpdateUI();
        questUI.UpdateUI();
        hideCompletedQuestToggleUI.UpdateUI();
        popupUI.UpdateUI();

        //Scroll
        SetActive(scrollTrans, Input.mouseScrollDelta.y != 0);
    }
    double favoriteNum;
    string AcceptedNumString()
    {
        string tempStr = optStr + "Accepted : ";
        if (game.questCtrl.AcceptedNum(heroKind) * 10 % 10 == 0)
            tempStr += tDigit(game.questCtrl.AcceptedNum(heroKind));
        else tempStr += tDigit(game.questCtrl.AcceptedNum(heroKind), 1);
        favoriteNum = game.questCtrl.TotalFavoriteNum(heroKind);
        if (favoriteNum > 0)
        {
            if (favoriteNum * 10 % 10 == 0)
                tempStr += " (F" + tDigit(favoriteNum) + ")";
            else
                tempStr += " (F" + tDigit(favoriteNum, 1) + ")";
        }
        tempStr += " / " + tDigit(game.questCtrl.AcceptableNum(game.currentHero).Value());
        return tempStr;
    }
    public override void Initialize()
    {
        SwitchButton();
    }
    void SwitchButton()
    {
        buttonTableUI.SetUI(currentOpenQuestKind);
        buttonTableUI.questButtonsUI[currentOpenQuestId[(int)currentOpenQuestKind]].thisButton.onClick.Invoke();
    }
    QuestKind currentOpenQuestKind { get => (QuestKind)kindSwitchTabUI.currentId; }
    int[] currentOpenQuestId = new int[Enum.GetNames(typeof(QuestKind)).Length];
    public void SwitchOpenQuestId(int id)
    {
        currentOpenQuestId[(int)currentOpenQuestKind] = id;
        for (int i = 0; i < buttonTableUI.questButtonsUI.Length; i++)
        {
            if (buttonTableUI.questButtonsUI[i].id == id) buttonTableUI.questButtonsUI[i].thisText.color = Color.yellow;
            else buttonTableUI.questButtonsUI[i].thisText.color = Color.white;
        }
    }
    [SerializeField] TextMeshProUGUI acceptableNumText;
    [SerializeField] Button[] selectKindButtons;
    GameObject[] selectKindButtonQuestIconObjects;
    [SerializeField] GameObject buttonTable;
    QuestButtonTableUI buttonTableUI;
    [SerializeField] GameObject information, masteryIcon;
    public Button questingButton;
    public QuestUI questUI;
    public HeroKind heroKind { get => game.currentHero; }
    public SwitchTabUI kindSwitchTabUI;
    ToggleUI hideCompletedQuestToggleUI;
    public PopupUI popupUI;
    [SerializeField] GameObject scrollTrans;
}

public class QuestButtonTableUI//左側のボタンの部分
{
    public QuestButtonTableUI(QuestMenuUI questMenuUI, GameObject gameObject)
    {
        this.questMenuUI = questMenuUI;
        thisObject = gameObject;
        questCtrl = game.questCtrl;
        questButtons = new GameObject[thisObject.transform.childCount];
        for (int i = 0; i < questButtons.Length; i++)
        {
            questButtons[i] = thisObject.transform.GetChild(i).gameObject;
        }
        questButtonsUI = new QuestButtonUI[questButtons.Length];
        for (int i = 0; i < questButtonsUI.Length; i++)
        {
            questButtonsUI[i] = new QuestButtonUI(questMenuUI, questButtons[i], i);
        }
    }
    public void SetUI(QuestKind questKind)
    {
        this.questKind = questKind;
        for (int i = 0; i < questButtonsUI.Length; i++)
        {
            int count = i;
            if (i < questCtrl.QuestArray(questKind, questMenuUI.heroKind).Length)
            {
                switch (questKind)
                {
                    case QuestKind.Global:
                        questButtonsUI[i].SetUI(questCtrl.globalQuestList[count]);
                        break;
                    case QuestKind.Daily:
                        questButtonsUI[i].SetUI(questCtrl.dailyQuestList[count]);
                        break;
                    case QuestKind.Title:
                        questButtonsUI[i].SetUI(questCtrl.titleQuestList[(int)questMenuUI.heroKind][count]);
                        break;
                    case QuestKind.General:
                        questButtonsUI[i].SetUI(questCtrl.generalQuestList[(int)questMenuUI.heroKind][count]);
                        break;
                    default:
                        questButtonsUI[i].SetUI(questCtrl.Quest(questKind, questMenuUI.heroKind, count));
                        break;
                }
            }
            else
                questButtonsUI[i].Disable();
        }
        //Sort();
    }
    public void UpdateUI()
    {        
        for (int i = 0; i < questButtonsUI.Length; i++)
        {
            questButtonsUI[i].UpdateUI();
        }
    }
    QuestController questCtrl;
    QuestMenuUI questMenuUI;
    GameObject thisObject;
    QuestKind questKind;
    GameObject[] questButtons;
    public QuestButtonUI[] questButtonsUI;

    //Title順に並び替え
    public void Sort()
    {
        List<QuestButtonUI> questButtonsUIList = new List<QuestButtonUI>();
        for (int i = 0; i < questButtonsUI.Length; i++)
        {
            int count = i;
            if (questButtonsUI[i].isEnable) questButtonsUIList.Add(questButtonsUI[count]);
        }
        questButtonsUIList.Sort(CompareRequiredLevel);
        for (int i = 0; i < questButtonsUIList.Count; i++)
        {
            int count = i;
            questButtonsUI[i].SetUI(questCtrl.Quest(questKind, questMenuUI.heroKind, questButtonsUIList[count].id));
        }
    }

    int CompareRequiredLevel(QuestButtonUI x, QuestButtonUI y)
    {
        if ((int)x.thisQuest.rewardTitleKind > (int)y.thisQuest.rewardTitleKind) return 1;
        if ((int)x.thisQuest.rewardTitleKind < (int)y.thisQuest.rewardTitleKind) return -1;
        if ((int)x.thisQuest.globalQuestType > (int)y.thisQuest.globalQuestType) return 1;
        if ((int)x.thisQuest.globalQuestType < (int)y.thisQuest.globalQuestType) return -1;
        if (x.thisQuest.kindGlobal > y.thisQuest.kindGlobal) return 1;
        if (x.thisQuest.kindGlobal < y.thisQuest.kindGlobal) return -1;
        if (x.thisQuest.unlockHeroLevel() > y.thisQuest.unlockHeroLevel()) return 1;
        if (x.thisQuest.unlockHeroLevel() < y.thisQuest.unlockHeroLevel()) return -1;
        if (x.thisQuest.RewardExp() > y.thisQuest.RewardExp()) return 1;
        if (x.thisQuest.RewardExp() < y.thisQuest.RewardExp()) return -1;
        if (x.thisQuest.kindGeneral > y.thisQuest.kindGeneral) return 1;
        if (x.thisQuest.kindGeneral < y.thisQuest.kindGeneral) return -1;
        if (x.thisQuest.kindTitle > y.thisQuest.kindTitle) return 1;
        if (x.thisQuest.kindTitle < y.thisQuest.kindTitle) return -1;
        return 0;
    }

}
public class QuestButtonUI//ボタン自体
{
    public QuestButtonUI(QuestMenuUI questMenuUI, GameObject gameObject, int id)
    {
        this.questMenuUI = questMenuUI;
        thisObject = gameObject;
        this.id = id;
        thisImage = gameObject.GetComponent<Image>();
        thisText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        iconText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = thisObject.transform.GetChild(2).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        thisButton = thisObject.GetComponent<Button>();

        var clickAction = new ClickAction(gameObject, OpenQuest, () => { OpenQuest(); questMenuUI.questUI.acceptButton.onClick.Invoke(); });
        //thisButton.onClick.AddListener(OpenQuest);
    }
    public void SetUI(QUEST quest)
    {
        isEnable = true;
        thisQuest = quest;
        if (thisQuest.kind == QuestKind.Daily || thisQuest.kind == QuestKind.Global)
            thisImage.sprite = sprite.globalquestButton;
        else
            thisImage.sprite = sprite.generalquestButton;
    }
    public bool isEnable;
    public void Disable()
    {
        isEnable = false;
    }
    public void UpdateUI()
    {
        SetActive(thisObject, Enable());
        if (!Enable()) return;
        SetActive(lockObject, !Interactable());
        thisButton.interactable = Interactable();
        if (!Interactable())
        {
            lockText.text = optStr + "<sprite=\"locks\" index=0> " + localized.Basic(BasicWord.HeroLevel) + " " + tDigit(thisQuest.unlockHeroLevel());
            return;
        }
        if (thisQuest.isCleared) iconText.text = optStr + "<size=30><sprite=\"questIcons\" index=2>";
        else if (thisQuest.CanClaim()) iconText.text = optStr + "<size=30><sprite=\"questIcons\" index=1>";
        else if (thisQuest.isAccepted) iconText.text = optStr + "<size=30><sprite=\"questIcons\" index=0>";
        else iconText.text = "";
        if (thisQuest.isFavorite) thisText.text = "<color=green>(F)</color> " + thisQuest.String().name;
        else thisText.text = thisQuest.String().name;
    }
    
    void OpenQuest()
    {
        questMenuUI.SwitchOpenQuestId(id);
        questMenuUI.questUI.SetUI(() => thisQuest);
        if (isShiftPressed) questMenuUI.questUI.acceptButton.onClick.Invoke();
    }
    bool Enable()
    {
        if (!isEnable) return false;
        if (main.S.isToggleOn[(int)ToggleKind.HideCompletedQuest] && thisQuest.isCleared) return false;
        return thisQuest.IsUnlocked();
    }
    bool Interactable()
    {
        return thisQuest.IsEnoughAcceptCondition();
    }
    QuestMenuUI questMenuUI;
    public QUEST thisQuest;
    GameObject thisObject;
    Image thisImage;
    public int id;
    public Button thisButton;
    public TextMeshProUGUI iconText, thisText;
    GameObject lockObject;
    TextMeshProUGUI lockText;//エリアによりsetActive、レベルにより解禁
}

public class QuestUI//右側のInformationの部分
{
    public QuestUI(QuestMenuUI questMenuUI, GameObject gameObject)
    {
        this.questMenuUI = questMenuUI;
        thisObject = gameObject;
        acceptButton = thisObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        cancelButton = thisObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        informationText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        acceptText = acceptButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        cancelText = cancelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        iconQ = thisObject.transform.GetChild(5).gameObject;
        iconI = thisObject.transform.GetChild(6).gameObject;
        iconC = thisObject.transform.GetChild(7).gameObject;
        toTitleButton = thisObject.transform.GetChild(8).gameObject.GetComponent<Button>();
        toTitleButtonText = toTitleButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        acceptButton.onClick.AddListener(() =>
        {
            if (thisQuest().CanClaim())
                gameUI.soundUI.Play(SoundEffect.QuestClaim);
            else gameUI.soundUI.Play(SoundEffect.Click);
            thisQuest().AcceptOrClaim(); }
        );
        cancelButton.onClick.AddListener(() => thisQuest().Cancel());
        cancelButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Click));
        toTitleButton.onClick.AddListener(() =>
        {
            if (thisQuest().kind == QuestKind.Title)
            {
                gameUI.menuUI.menuButtons[(int)MenuKind.Ability].onClick.Invoke();
                gameUI.menuUI.MenuUI(MenuKind.Ability).GetComponent<AbilityMenuUI>().titleOpenButton.onClick.Invoke();
                //questMenuUI.popupUI.DelayHide();
            }
            else if (thisQuest().kind == QuestKind.General)
                thisQuest().AssignRemoveFavorite();
        });
    }
    public void SetUI(Func<QUEST> quest)
    {
        thisQuest = quest;
    }

    QuestMenuUI questMenuUI;
    public Func<QUEST> thisQuest;
    public GameObject thisObject;
    public GameObject iconQ, iconI, iconC;
    public Button acceptButton;
    public Button cancelButton;
    public Button toTitleButton;
    public TextMeshProUGUI nameText, informationText, toTitleButtonText;
    public TextMeshProUGUI acceptText, cancelText;

    public void UpdateUI()
    {
        SetActive(iconC, thisQuest().isCleared);
        SetActive(iconQ, thisQuest().isAccepted && !thisQuest().isCleared);
        SetActive(iconI, thisQuest().CanClaim());
        SetActive(toTitleButton.gameObject, thisQuest().kind == QuestKind.Title || thisQuest().kind == QuestKind.General);
        if (toTitleButton.gameObject.activeSelf)
        {
            if (thisQuest().kind == QuestKind.Title)
            {
                toTitleButtonText.text = "Titles";
                toTitleButton.interactable = true;
            }
            else
            {
                toTitleButtonText.text = "Favorite";
                toTitleButton.interactable = thisQuest().CanFavorite();
            }
        }
        nameText.text = NameString();
        informationText.text = InformationString();
        acceptButton.interactable = thisQuest().AcceptOrClaimInteractable();
        if (thisQuest().isAccepted) acceptText.text = localized.Basic(BasicWord.Claim);
        else acceptText.text = localized.Basic(BasicWord.Accept);
        cancelButton.interactable = thisQuest().CanCancel();
        cancelText.text = localized.Basic(BasicWord.Cancel);
        if (thisQuest().questingArea == game.areaCtrl.nullArea)
        {
            SetActive(questMenuUI.questingButton.gameObject, false);
            return;
        }
        SetActive(questMenuUI.questingButton.gameObject, true);
        questMenuUI.questingButton.interactable = QuestingButtonInteractable();
    }
    //DungeonやChallenge中はダメ
    bool QuestingButtonInteractable()
    {
        if (!thisQuest().isAccepted) return false;
        if (thisQuest().isCleared) return false;
        if (!thisQuest().questingArea.CanStart()) return false;
        if (game.battleCtrl.areaBattle.CurrentArea().isDungeon) return false;
        if (game.battleCtrl.areaBattle.CurrentArea().isChallenge) return false;
        return true;
    }

    string NameString()
    {
        if (thisQuest().isFavorite)
            return optStr + thisQuest().String().name + "  ( <color=green>Favorite</color> )";
        return thisQuest().String().name;
    }
    string InformationString()
    {
        string tempString = optStr + "<size=20><u>" + localized.Basic(BasicWord.Client) + "</u><size=18>";
        tempString += "\n- " + thisQuest().String().client;
        tempString += "\n\n";
        tempString += "<size=20><u>" + localized.Basic(BasicWord.Description) + "</u><size=18>";
        tempString += "\n- " + thisQuest().String().description;
        tempString += "\n\n";
        //ClearCondition
        tempString += "<size=20><u>" + localized.Basic(BasicWord.ClearCondition) + "</u><size=18>";
        if (thisQuest().String().condition != "") tempString += "\n- " + thisQuest().String().condition;
        switch (thisQuest().type)
        {
            case QuestType.Other:
                switch (thisQuest().rewardTitleKind)
                {
                    case TitleKind.MoveSpeed:
                        tempString += "\n- Walk ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += meter(thisQuest().movedDistance) + " / ";
                        tempString += meter(thisQuest().porterRequiredMovedDistance);
                        break;
                    case TitleKind.PhysicalDamage:
                        tempString += "\n- Physical Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.FireDamage:
                        tempString += "\n- Fire Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.IceDamage:
                        tempString += "\n- Ice Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.ThunderDamage:
                        tempString += "\n- Thunder Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.LightDamage:
                        tempString += "\n- Light Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.DarkDamage:
                        tempString += "\n- Dark Attack # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().elementTriggeredNum) + " / ";
                        tempString += tDigit(thisQuest().elementTriggeredRequiredNum);
                        break;
                    case TitleKind.Quester:
                        tempString += "\n- General Quest Cleared # ";
                        if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().questerClearNum) + " / ";
                        tempString += tDigit(thisQuest().questerRequiredClearNum);
                        break;
                    default:
                        //なぜかoptStrをつけると改行されなくなった
                        //tempString += "\n- " + thisQuest().String().condition;
                        break;
                }
                break;
            case QuestType.Defeat:
                if (thisQuest().kind == QuestKind.Daily) tempString += "\n- Defeat any " + localized.MonsterSpeciesName(thisQuest().dailyTargetMonsterSpecies) + " ";
                else tempString += "\n- Defeat " + thisQuest().defeatTargetMonster.Name() + " ";
                if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().DefeatTargetMonsterDefeatedNum()) + " / ";
                tempString += tDigit(thisQuest().defeatRequredDefeatNum());
                break;
            case QuestType.Capture:
                if (thisQuest().kind == QuestKind.Daily) tempString += "\n- Capture any " + localized.MonsterSpeciesName(thisQuest().dailyTargetMonsterSpecies) + " ";
                else tempString += "\n- Capture " + thisQuest().captureTargetMonster.Name() + " ";
                if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().CaptureTargetMonsterCapturedNum()) + " / ";
                tempString += tDigit(thisQuest().captureRequiredNum());
                break;
            case QuestType.Bring:
                foreach (var item in thisQuest().bringRequiredResources)
                {
                    tempString += "\n- " + localized.ResourceName(item.Key) + " ";
                    if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().TargetResource(item.Key).value) + " / ";
                    tempString += tDigit(item.Value);
                }
                foreach (var item in thisQuest().bringRequiredMaterials)
                {
                    tempString += "\n- " + localized.Material(item.Key) + " ";
                    if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().TargetMaterial(item.Key).value) + " / ";
                    tempString += tDigit(item.Value);
                }
                break;
            case QuestType.AreaComplete:
                tempString += "\n- " + thisQuest().completeTargetArea.Name(true, false) + "  Clear # ";
                if (thisQuest().isAccepted && !thisQuest().isCleared) tempString += tDigit(thisQuest().AreaCompletedNum()) + " / ";
                tempString += tDigit(thisQuest().areaRequredCompletedNum());
                break;
        }
        tempString += "\n\n";
        //Reward
        tempString += "<size=20><u>" + localized.Basic(BasicWord.Reward) + "</u><size=18>";
        if (thisQuest().rewardGuildExp() > 0) tempString += "\n- " + tDigit(thisQuest().rewardGuildExp() * game.guildCtrl.expGain.Value()) + " Guild EXP";
        if (thisQuest().RewardExp() > 0)
        {
            double tempExp = thisQuest().RewardExp() * game.statsCtrl.HeroStats(game.currentHero, Stats.ExpGain).Value() + game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue();
            tempString += "\n- " + tDigit(tempExp) + " EXP";
            if (!thisQuest().isCleared)
            {
                long tempLevel = game.statsCtrl.EstimatedLevel(questMenuUI.heroKind, tempExp).level;
                long tempLevelIncrement = tempLevel - game.statsCtrl.HeroLevel(questMenuUI.heroKind).value;
                if (tempLevelIncrement < 30)
                    tempString += "  ( Lv " + tDigit(tempLevel) + " : EXP " + percent(game.statsCtrl.EstimatedLevel(questMenuUI.heroKind, tempExp).expPercent, 0) + " )</color>";
                else
                    tempString += "  ( Lv " + tDigit(tempLevel) + " : <color=yellow>MAX</color> )</color>";
            }
        }
        if (thisQuest().rewardGold() > 0) tempString += "\n- " + tDigit(thisQuest().rewardGold() * game.statsCtrl.GoldGain().Value() + game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue()) +  " " + localized.Basic(BasicWord.Gold);
        if (thisQuest().rewardNitro() > 0) tempString += "\n- " + tDigit(thisQuest().rewardNitro()) + " Nitro";
        if (thisQuest().rewardPortalOrb() > 0) tempString += "\n- " + tDigit(thisQuest().rewardPortalOrb()) + " " + localized.Basic(BasicWord.PortalOrb);
        if (thisQuest().rewardEC() > 0) tempString += "\n- " + tDigit(thisQuest().rewardEC()) + " " + localized.Basic(BasicWord.EpicCoin);
        foreach (var item in thisQuest().rewardMaterial)
        {
            tempString += "\n- " + tDigit(item.Value()) + " " + localized.Material(item.Key());
        }
        foreach (var item in thisQuest().rewardMaterialNumber)
        {
            tempString += "\n- " + tDigit(item.Value()) + " " + item.Key().Name();
        }
        foreach (var item in thisQuest().rewardPotion)
        {
            tempString += "\n- " + tDigit(item.Value()) + " " + localized.PotionName(item.Key());
            tempString += "<color=yellow> (Make sure you have enough slots before claim)</color>";
        }
        if (thisQuest().kind == QuestKind.Title) tempString += "\n- Title [ " + localized.Title(thisQuest().rewardTitleKind) + " Lv " + tDigit(thisQuest().rewardTitleLevel) + " ]";
        if (thisQuest().String().reward != "") tempString += "\n- " + thisQuest().String().reward;

        if (thisQuest().kind == QuestKind.Daily)
        {
            tempString += optStr + "\n\n\nLast Reset Time : " + main.lastDailyTime.ToString("MM/dd/yyyy HH:mm");
            if (thisQuest().isCleared || !thisQuest().isAccepted)
            {
                tempString += optStr + "\nNext Reset Time : " + main.lastDailyTime.AddDays(1).Date.ToString("MM/dd/yyyy 00:01");
            }
            else
            {
                tempString += optStr + "\nThis quest won't reset while active";
            }
        }
        return tempString;
    }
    int count;
    public string MasteryPopupString()
    {
        string tempString = "<size=20><u>Quest Mastery</u><size=18>";
        if (thisQuest().masteryList[0].isReached)
            tempString += "\n- Current Rank : <color=green>" + thisQuest().MasteryRankString() + "</color>";
        tempString += "\n- Total Cleared # : <color=green>" + tDigit(thisQuest().totalClearedNumGeneralThisAscension) + "</color>";
        if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
            tempString += "\n- Max Reached # : <color=green>" + tDigit(thisQuest().maxReachedClearedNumGeneral) + "</color>";
        tempString += "\n";
        for (int i = 0; i < thisQuest().masteryList.Count; i++)
        {
            tempString += "\n" + thisQuest().masteryList[i].String();
        }
        return tempString;
    }
}
