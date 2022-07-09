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

public class TownMenuUI : MENU_UI
{
    [SerializeField] TextMeshProUGUI researchingText;
    [SerializeField] Button[] buildings;
    public BuildingUI[] buildingsUI;
    [SerializeField] GameObject[] materials;
    public TownMaterialUI[] materialsUI;
    [SerializeField] TextMeshProUGUI nameText, infoLeftText, infoRightText;
    [SerializeField] Button rankupButton, levelupButton;
    [SerializeField] Button[] researchButtons;
    SwitchTabUI switchBuildingUI;
    BUILDING currentBuilding { get=> game.townCtrl.Building((BuildingKind)switchBuildingUI.currentId); }
    public PopupUI popupUI;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;

    // Start is called before the first frame update
    void Start()
    {
        buildingsUI = new BuildingUI[buildings.Length];
        for (int i = 0; i < buildingsUI.Length; i++)
        {
            int count = i;
            buildingsUI[i] = new BuildingUI(buildings[i].gameObject, game.townCtrl.Building((BuildingKind)count));
        }
        materialsUI = new TownMaterialUI[materials.Length];
        for (int i = 0; i < materialsUI.Length; i++)
        {
            int count = i;
            materialsUI[i] = new TownMaterialUI(materials[count], game.townCtrl.TownMaterial((TownMaterialKind)count));
        }
        switchBuildingUI = new SwitchTabUI(buildings, true);
        buildings[0].onClick.Invoke();
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Click));
        }

        rankupButton.onClick.AddListener(() => currentBuilding.rankTransaction.Buy());
        rankupButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.QuestClaim));
        levelupButton.onClick.AddListener(() => currentBuilding.levelTransaction.Buy());
        levelupButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
        for (int i = 0; i < researchButtons.Length; i++)
        {
            int count = i;
            researchButtons[i].onClick.AddListener(() => currentBuilding.SwitchResearch((ResourceKind)count));
        }

        //Popup
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.SetTargetObject(rankupButton.gameObject, RankUpString);
        popupUI.SetTargetObject(levelupButton.gameObject, LevelUpString);
        for (int i = 0; i < researchButtons.Length; i++)
        {
            int count = i;
            popupUI.SetTargetObject(researchButtons[count].gameObject, () => ResearchString((ResourceKind)count));
        }
        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.MagicSlime, MonsterColor.Purple).pet, petQoLIcon, popupUI, "Auto-Level");
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        for (int i = 0; i < buildingsUI.Length; i++)
        {
            buildingsUI[i].UpdateUI();
        }
        for (int i = 0; i < materialsUI.Length; i++)
        {
            materialsUI[i].UpdateUI();
        }

        researchingText.text = "Researching : " + tDigit(game.townCtrl.CurrentResearchingNum()) + " / " + tDigit(game.townCtrl.maxResearchNum.Value());
        nameText.text = NameString();
        infoLeftText.text = InfoLeftString();
        infoRightText.text = InfoRightString();

        rankupButton.interactable = currentBuilding.rankTransaction.CanBuy();
        levelupButton.interactable = currentBuilding.levelTransaction.CanBuy();
        for (int i = 0; i < researchButtons.Length; i++)
        {
            int count = i;
            researchButtons[i].interactable = currentBuilding.CanResearch((ResourceKind)count) || currentBuilding.isResearch[count];
        }

        popupUI.UpdateUI();
        petQoLUI.UpdateUI();
    }

    string NameString()
    {
        return optStr + "<size=24>"
            + currentBuilding.NameString()
            + "<size=20>  < <color=orange>Rank " + tDigit(currentBuilding.Rank())
            + "</color> >  <color=green>Lv " + tDigit(currentBuilding.Level())
            + "</color> / " + tDigit(currentBuilding.MaxLevel());
    }
    string InfoLeftString()
    {
        string tempStr = optStr + "<size=20><u>Level Effect</u><size=18>";
        tempStr += "\n- " + currentBuilding.LevelEffectString();
        tempStr += "\n\n<size=20><u>Researchable Effect</u><size=18>";
        for (int i = 0; i < currentBuilding.researchLevels.Length; i++)
        {
            int count = i;
            //tempStr += "\n<sprite=\"resource\" index=" + (2 + count) + ">Lv " + tDigit(currentBuilding.ResearchLevel((ResourceKind)count)) + " : ";
            //tempStr += percent(currentBuilding.researchExps[count].Percent());
            //tempStr += " ( " + DoubleTimeToDate(currentBuilding.researchExps[count].Timeleft()) + " left )";
            tempStr += "\n- " + currentBuilding.ResearchEffectString((ResourceKind)count);
        }
        tempStr += "\n\n<size=20><u>Rank Milestone</u><size=18>";
        for (int i = 0; i < currentBuilding.passiveRankEffectList.Count; i++)
        {
            tempStr += "\n" + currentBuilding.passiveRankEffectList[i].DescriptionString();
        }
        return tempStr;
    }
    string InfoRightString()
    {
        string tempStr = optStr + "<size=20><u>Level Milestone</u><size=18>";
        for (int i = 0; i < currentBuilding.passiveEffectList.Count; i++)
        {
            tempStr += "\n" + currentBuilding.passiveEffectList[i].DescriptionString();
        }
        return tempStr;
    }
    string RankUpString()
    {
        string tempStr = optStr + "<size=20>" + currentBuilding.NameString() + " < <color=orange>Rank " + tDigit(currentBuilding.Rank()) + "</color> >";
        tempStr += "\n\n<u>Max Rank</u>\n<size=18>";
        tempStr += "- Rank " + tDigit(currentBuilding.rank.maxValue());
        tempStr += "\n\n<size=20><u>Required Condition to Rank Up</u><size=18>";
        tempStr += "\n" + currentBuilding.RankUpCondition().DescriptionString();
        tempStr += "\n<size=20><u>Cost to Rank Up</u><size=18>";
        tempStr += "\n" + currentBuilding.rankTransaction.DescriptionString();
        return tempStr;
    }
    string LevelUpString()
    {
        string tempStr = optStr + "<size=20>" + currentBuilding.NameString() + " < <color=green>Lv " + tDigit(currentBuilding.Level()) + "</color> >";
        tempStr += "\n\n<u>Max Level</u>\n<size=18>";
        tempStr += "- Lv " + tDigit(currentBuilding.level.maxValue());
        if (currentBuilding.level.IsMaxed()) tempStr += "\n\n<color=red>Increase Rank to expand the level cap</color>";
        else
        {
            tempStr += "\n\n<size=20><u>Cost to Level Up" + "  ( <color=green>+ " + tDigit(currentBuilding.levelTransaction.LevelIncrement()) + "</color> ) " + "</u><size=18>";
            tempStr += "\n" + currentBuilding.levelTransaction.DescriptionString();
        }
        return tempStr;
    }
    string ResearchString(ResourceKind kind)
    {
        string tempStr = optStr + "<size=20>" + localized.ResourceName(kind) + " Research < <color=green>Lv " + tDigit(currentBuilding.ResearchLevel(kind)) + "</color> >";
        tempStr += "\n\n<u>Max Level</u>\n<size=18>";
        tempStr += "- Lv " + tDigit(currentBuilding.researchLevels[(int)kind].maxValue());
        tempStr += "\n\n<size=20><u>Effect</u><size=18>";
        tempStr += "\n- " + currentBuilding.ResearchEffectString(kind, true);
        tempStr += "\n\n<size=20><u>Progress</u><size=18>";
        tempStr += "\n- Researching : ";
        if (currentBuilding.isResearch[(int)kind]) tempStr += "<color=green>ON</color>";
        else tempStr += "OFF";
        tempStr += " ( " + DoubleTimeToDate(currentBuilding.researchExps[(int)kind].Timeleft()) + " left )";
        tempStr += "\n- Research EXP : " + tDigit(currentBuilding.researchExps[(int)kind].value) + " / " + tDigit(currentBuilding.researchExps[(int)kind].RequiredExp(), 2) + " ( " + percent(currentBuilding.researchExps[(int)kind].Percent()) + " )";
        tempStr += "\n- " + localized.ResourceName(kind) + " Research Power : <color=green>+ " + tDigit(currentBuilding.researchExps[(int)kind].researchPower, 2) + " exp / sec</color>";
        return tempStr;
    }
}

public class BuildingUI
{
    public GameObject gameObject;
    public Button button;
    public TextMeshProUGUI text;
    public BUILDING building;
    public GameObject lockObject;
    public TextMeshProUGUI lockText;
    public BuildingUI(GameObject gameObject, BUILDING building)
    {
        this.gameObject = gameObject;
        this.building = building;
        button = gameObject.GetComponent<Button>();
        text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = gameObject.transform.GetChild(2).gameObject;
        lockText = lockObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateUI()
    {
        text.text = DescriptionString();
        button.interactable = building.unlock.IsUnlocked();
        SetActive(lockObject, !building.unlock.IsUnlocked());
        if (!building.unlock.IsUnlocked()) lockText.text = "<sprite=\"Locks\" index=0> Guild Level " + tDigit(building.unlockGuildLevel);
    }
    string DescriptionString()
    {
        string tempStr = optStr;
        tempStr += "<size=20>" + building.NameString() + "<size=18> ";
        tempStr += "\n<color=orange>Rank " + tDigit(building.Rank()) + "   <color=green>Lv " + tDigit(building.Level()) + "</color>  ";
        for (int i = 0; i < building.isResearch.Length; i++)
        {
            if (building.isResearch[i]) tempStr += "<sprite=\"resource\" index=" + (2 + i) + ">";
        }
        return tempStr;
    }
}

public class TownMaterialUI
{
    public TownMaterialUI(GameObject gameObject, TownMaterial material)
    {
        thisObject = gameObject;
        thisMaterial = material;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        valueText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateUI()
    {
        SetActive(thisObject, thisMaterial.value > 0);
        if (thisMaterial.value <= 0) return;
        nameText.text = thisMaterial.Name();
        valueText.text = tDigit(thisMaterial.value);
    }
    GameObject thisObject;
    TownMaterial thisMaterial;
    TextMeshProUGUI nameText, valueText;
}