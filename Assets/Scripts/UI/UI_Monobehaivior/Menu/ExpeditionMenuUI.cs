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

public class ExpeditionMenuUI : MENU_UI
{
    [SerializeField] GameObject[] expeditionTeams;
    ExpeditionTeamUI[] expeditionTeamUIs;
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] GameObject[] expeditions;
    ExpeditionUI[] expeditionUIs;
    [SerializeField] GameObject milestoneObject;
    public PopupUI popupUI;
    public ExpeditionPetSelectWindowUI petSelectUI;

    private void Awake()
    {
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        petSelectUI = new ExpeditionPetSelectWindowUI(gameUI.popupCtrlUI.expeditionPetSelectWindow);
    }
    private void Start()
    {
        expeditionTeamUIs = new ExpeditionTeamUI[expeditionTeams.Length];
        for (int i = 0; i < expeditionTeamUIs.Length; i++)
        {
            expeditionTeamUIs[i] = new ExpeditionTeamUI(this, game.expeditionCtrl.expeditions[i], expeditionTeams[i]);
        }
        expeditionUIs = new ExpeditionUI[expeditions.Length];
        for (int i = 0; i < expeditionUIs.Length; i++)
        {
            expeditionUIs[i] = new ExpeditionUI(this, game.expeditionCtrl.globalInfoList[i], expeditions[i]);
        }
        popupUI.SetTargetObject(milestoneObject, game.expeditionCtrl.ExpeditionMilestoneString);
        popupUI.SetTargetObject(statsText.gameObject, game.expeditionCtrl.ExpeditionMilestoneString);

        openClose.openActions.Add(SetUI);

        SetUI();
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        statsText.text = game.expeditionCtrl.StatsString();
        for (int i = 0; i < expeditionTeamUIs.Length; i++)
        {
            expeditionTeamUIs[i].UpdateUI();
        }
        for (int i = 0; i < expeditionUIs.Length; i++)
        {
            expeditionUIs[i].UpdateUI();
        }
        popupUI.UpdateUI();

        count++;
        if (count >= 60)
        {
            SetUI();
            count = 0;
        }
    }
    int count;

    void SetUI()
    {
        for (int i = 0; i < expeditionTeamUIs.Length; i++)
        {
            expeditionTeamUIs[i].SetUI();
        }
    }
}

public class ExpeditionUI
{
    public ExpeditionUI(ExpeditionMenuUI menuUI, ExpeditionGlobalInformation globalInfo, GameObject gameObject)
    {
        this.menuUI = menuUI;
        this.globalInfo = globalInfo;
        this.gameObject = gameObject;
        fillImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        menuUI.popupUI.SetTargetObject(gameObject, PopupString);

    }
    public void UpdateUI()
    {
        text.text = optStr + "<size=18>" + globalInfo.NameString(true) + "\n<size=16>EXP : " + globalInfo.exp.Description(true);
        fillImage.fillAmount = (float)globalInfo.exp.Percent();
    }
    public string PopupString()
    {
        string tempStr = optStr + "<size=20>" + globalInfo.NameString(true);
        tempStr += "\n<size=18>- EXP : " + globalInfo.exp.Description(true);
        tempStr += "\n\n" + globalInfo.InfoString();
        tempStr += "\n\n" + globalInfo.EffectString();
        tempStr += "\n\n" + globalInfo.PassiveEffectString();
        return tempStr;
    }
    public ExpeditionMenuUI menuUI;
    public ExpeditionGlobalInformation globalInfo;
    public GameObject gameObject;
    public Image fillImage;
    public TextMeshProUGUI text;
}

public class ExpeditionTeamUI
{
    public ExpeditionTeamUI(ExpeditionMenuUI menuUI, Expedition expedition, GameObject gameObject)
    {
        this.menuUI = menuUI;
        this.expedition = expedition;
        this.gameObject = gameObject;
        backgroundObject = gameObject.transform.GetChild(0).gameObject;
        progressImage = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < petsUI.Length; i++)
        {
            petsUI[i] = new ExpeditionPetUI(this, i, gameObject.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject);
        }
        rewardText = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        rareChanceText = gameObject.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        dropDown = gameObject.transform.GetChild(6).gameObject.GetComponent<TMP_Dropdown>();
        leftButton = gameObject.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        rightButton = gameObject.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        timeText = gameObject.transform.GetChild(7).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        progressText = gameObject.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();
        startButton = gameObject.transform.GetChild(9).gameObject.GetComponent<Button>();
        startButtonText = startButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        iconI = startButton.gameObject.transform.GetChild(1).gameObject;
        //statsText = gameObject.transform.GetChild(10).gameObject.GetComponent<TextMeshProUGUI>();
        //gainedText = gameObject.transform.GetChild(11).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = gameObject.transform.GetChild(10).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        dropDown.onValueChanged.AddListener((id) => expedition.ChangeKind(game.expeditionCtrl.globalInfoList[id].kind));
        dropDown.SetValueWithoutNotify(game.expeditionCtrl.globalInfoList.IndexOf(expedition.globalInfo));
        startButton.onClick.AddListener(() => expedition.StartButtonAction());
        leftButton.onClick.AddListener(() => expedition.timeId.Decrease());
        rightButton.onClick.AddListener(() => expedition.timeId.Increase());

        menuUI.popupUI.SetTargetObject(startButton.gameObject, StartButtonPopupString);
        menuUI.popupUI.SetTargetObject(backgroundObject, PopupString);
        menuUI.popupUI.SetTargetObject(leftButton.gameObject, PopupString);
        menuUI.popupUI.SetTargetObject(rightButton.gameObject, PopupString);

        List<string> kindStringList = new List<string>();
        for (int i = 0; i < game.expeditionCtrl.globalInfoList.Count; i++)
        {
            kindStringList.Add(game.expeditionCtrl.globalInfoList[i].NameString());
        }
        dropDown.ClearOptions();
        dropDown.AddOptions(kindStringList);
    }
    string StartButtonPopupString()
    {
        if (!expedition.isStarted)
        {
            if (expedition.PetNum() < 1) return "<size=18>Select pets to let them go to expedition";
            return optStr + "<size=18><u>Cost to Start</u>\n- <sprite=\"resource\" index=0> " + tDigit(expedition.RequiredGold()) + " Gold";
        }
        if (!expedition.isFinished)
            return "<size=18>Cancel this expedition\n<color=yellow>- You won't get any reward.</color>";
        return "<size=18>Claim to receive followings:\n<color=green>" + expedition.RewardString(true) + "</color>";
    }

    string PopupString()
    {
        string tempStr = optStr + "<size=20>" + expedition.NameString();
        tempStr += optStr + "\n\n<size=18><u>Current Expedition</u>";
        tempStr += optStr + "\n- " + expedition.globalInfo.NameString(true);
        tempStr += optStr + "\n- Expedition EXP : " + expedition.globalInfo.exp.Description(true);
        tempStr += "\n\n<u>Stats</u>";
        tempStr += optStr + "\n- Expedition Speed : <color=green>x" + tDigit(expedition.TimeSpeed(), 3) + "</color>  ( + 0.001 / Team Level )";
        tempStr += optStr + "\n- Pet EXP Gain : <color=green>+ " + tDigit(expedition.ExpGainPerSec(), 2) + " / sec" + "</color>  ( + 0.10 / Expedition Level )";
        tempStr += optStr + "\n- Expedition EXP Gain : <color=green>+ " + tDigit(expedition.globalInfo.ExpGainPerSecPerPet() * expedition.PetNum(), 2) + " / sec" + "</color>  ( + " + tDigit(expedition.globalInfo.ExpGainPerSecPerPet(), 2) + " / Team Member # )";
        tempStr += optStr + "\n- Rare Material Gain Chance : <color=green>" + percent(expedition.RareChance()) + "</color>  ( [Mutant Captured #]<sup>2/3</sup> x [Hour] x 0.01% )";
        tempStr += optStr + "\n\n" + expedition.RewardString(false);
        tempStr += optStr + "\n<color=yellow>The amount increases based on team ranks, expedition levels and time.</color>";
        tempStr += "\n\n<u>Progress</u>";
        tempStr += "\n- Time Left : <color=green>" + DoubleTimeToDate(expedition.TimesecLeft()) + "</color>";
        tempStr += "\n- Total Walked Distance : <color=green>" + meter(main.S.expeditionMovedDistance[expedition.id]) + "</color>";
        return tempStr;
    }

    public void SetUI()
    {
        for (int i = 0; i < dropDown.options.Count; i++)
        {
            dropDown.options[i].text = game.expeditionCtrl.globalInfoList[i].NameString();
        }
        dropDown.captionText.text = game.expeditionCtrl.globalInfoList[dropDown.value].NameString();
        dropDown.SetValueWithoutNotify(game.expeditionCtrl.globalInfoList.IndexOf(expedition.globalInfo));
        for (int i = 0; i < petsUI.Length; i++)
        {
            petsUI[i].SetUI();
        }
    }

    public void UpdateUI()
    {
        SetActive(lockObject, !expedition.unlock.IsUnlocked());
        if (!expedition.unlock.IsUnlocked()) return;
        progressImage.fillAmount = (float)expedition.ProgressPercent();
        nameText.text = expedition.NameString();
        rewardText.text = expedition.RewardString(true);
        dropDown.interactable = !expedition.isStarted;
        //statsText.text = expedition.StatsString();
        //gainedText.text = expedition.GainedString();
        rareChanceText.text = "";
        timeText.text = optStr + tDigit(expedition.timeHour, 1) + " h";
        progressText.text = expedition.isFinished ? "<color=green>Completed!</color>" : optStr + "<size=16><color=green>Speed x" + tDigit(expedition.TimeSpeed(), 3) + "</color> <size=18> " + DoubleTimeToDate(expedition.TimesecLeft()) + " left";
        startButtonText.text = expedition.StartButtonString();

        leftButton.interactable = !expedition.isStarted;
        rightButton.interactable = !expedition.isStarted;
        startButton.interactable = expedition.StartButtonInteractable();

        SetActive(iconI, expedition.CanClaim());

        for (int i = 0; i < petsUI.Length; i++)
        {
            petsUI[i].UpdateUI();
        }
    }
    public ExpeditionMenuUI menuUI;
    public Expedition expedition;
    public GameObject gameObject, backgroundObject, lockObject, iconI;
    public ExpeditionPetUI[] petsUI = new ExpeditionPetUI[3];
    public Image progressImage;
    public TextMeshProUGUI nameText, rewardText, rareChanceText, timeText, progressText, startButtonText, lockText, statsText, gainedText;
    public Button leftButton, rightButton, startButton;
    public TMP_Dropdown dropDown;
}
public class ExpeditionPetUI
{
    public ExpeditionPetUI(ExpeditionTeamUI expeditionUI, int slotId, GameObject gameObject)
    {
        this.expeditionUI = expeditionUI;
        this.slotId = slotId;
        this.gameObject = gameObject;
        petImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        levelText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        expFillImage = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        selectButton = gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        buttonImage = selectButton.gameObject.GetComponent<Image>();
        lockObject = gameObject.transform.GetChild(4).gameObject;

        selectButton.onClick.AddListener(SelectButtonAction);

        expeditionUI.menuUI.popupUI.SetTargetObject(gameObject, PopupString);//, () => pet.isSet);, () => !pet.unlock.IsUnlocked()
    }
    public void SelectButtonAction()
    {
        if (pet.isSet)
            pet.Remove();
        else
        {
            expeditionUI.menuUI.petSelectUI.SetUI(pet, this);
            ////Debug
            //pet.Set(MonsterSpecies.Slime, MonsterColor.Blue);
        }

        SetUI();
    }
    public void SetUI()
    {
        buttonImage.color = !pet.isSet ? Color.white : Color.clear;
        SetActive(petImage.gameObject, pet.isSet);
        if (pet.isSet)
            petImage.sprite = sprite.monsters[0][(int)pet.species][(int)pet.color];
    }
    bool isUnlocked;
    public void UpdateUI()
    {
        isUnlocked = expedition.pets[slotId].unlock.IsUnlocked();
        SetActive(lockObject, !isUnlocked);
        selectButton.interactable = isUnlocked && !expedition.isStarted;
        if (!isUnlocked) return;
        if (!pet.isSet) return;
        levelText.text = optStr + "<color=green>Lv " + tDigit(pet.pet.level.value) + "</color>";
        expFillImage.fillAmount = (float)pet.pet.exp.Percent();
    }
    public string PopupString()
    {
        string tempStr;
        if (pet.isSet)
        {
            tempStr = optStr + "<size=20>" + pet.pet.globalInfo.Name() + " < <color=green>Lv " + tDigit(pet.pet.Level()) + "</color> > <color=orange>Rank " + tDigit(pet.pet.rank.value) + "</color>";
            tempStr += "<size=18>\n- EXP : " + pet.pet.exp.Description(true);
            tempStr += "<size=18>\n- Taming Point : " + pet.pet.tamingPoint.Description(true);
        }
        else if (pet.unlock.IsUnlocked())
        {
            tempStr = "Select a pet";
        }
        else
        {
            tempStr = pet.unlock.LockString();
        }
        return tempStr;
    }

    Expedition expedition => expeditionUI.expedition;
    public GameObject gameObject;
    public ExpeditionTeamUI expeditionUI;
    public int slotId;
    public ExpeditionPet pet => expedition.pets[slotId];
    public Image petImage, buttonImage, expFillImage;
    public Button selectButton;
    public GameObject lockObject;
    public TextMeshProUGUI levelText;
}

public class ExpeditionPetSelectWindowUI : ConfirmUI
{
    public GameObject[] pets;
    public ExpeditionPetSelectUI[] petsUI;

    public ExpeditionPetSelectWindowUI(GameObject thisObject) : base(thisObject)
    {
        pets = new GameObject[thisObject.transform.GetChild(3).childCount];
        petsUI = new ExpeditionPetSelectUI[pets.Length];
        for (int i = 0; i < pets.Length; i++)
        {
            pets[i] = thisObject.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject;
            petsUI[i] = new ExpeditionPetSelectUI(pets[i]);
        }
    }

    public void SetUI(ExpeditionPet expeditionPet, ExpeditionPetUI petUI)
    {
        MonsterPet pet;
        int petId = 0;
        bool isSetAll = false;
        for (int i = 0; i < petsUI.Length; i++)
        {
            if (isSetAll)
            {
                SetActive(petsUI[i].gameObject, false);
            }
            else
            {
                for (int j = petId; j < game.monsterCtrl.globalInfoList.Count; j++)
                {
                    pet = game.monsterCtrl.globalInfoList[j].pet;
                    if (pet.CanSetExpedition())
                    {
                        SetActive(petsUI[i].gameObject, true);
                        petsUI[i].SetUI(expeditionPet, pet, petUI, this);
                        petId = j + 1;
                        break;
                    }
                    //SetできるPetがいなくなった場合
                    if (j == game.monsterCtrl.globalInfoList.Count - 1)
                    {
                        isSetAll = true;
                        SetActive(petsUI[i].gameObject, false);
                    }
                }
            }
        }
        DelayShow();
    }
}

public class ExpeditionPetSelectUI//ConfirmWindowの中のペットアイコン
{
    public GameObject gameObject;
    public Button button;
    public Image image;
    public TextMeshProUGUI text;
    public ExpeditionPetSelectUI(GameObject gameObject)
    {
        this.gameObject = gameObject;
        button = gameObject.GetComponent<Button>();
        image = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public ExpeditionPet expeditionPet;
    public MonsterPet pet;
    public void SetUI(ExpeditionPet expeditionPet, MonsterPet pet, ExpeditionPetUI petUI, ExpeditionPetSelectWindowUI windowUI)
    {
        this.expeditionPet = expeditionPet;
        this.pet = pet;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            expeditionPet.Set(pet.species, pet.color);
            petUI.SetUI();
            windowUI.Hide();
        });
        image.sprite = sprite.monsters[0][(int)pet.species][(int)pet.color];
        text.text = optStr + "<color=orange>Rank " + tDigit(pet.rank.value) + "</color>\n<color=green>Lv " + tDigit(pet.level.value) + "\n/ " + tDigit(pet.level.maxValue()) + "</color>";
    }
}