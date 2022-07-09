using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SpriteSourceUI;
using static UsefulMethod;
using static Localized;
using static GameControllerUI;

public class PetQoLUI
{
    public PetQoLUI(MonsterPet pet, GameObject petIconFrame, PopupUI popupUI, string NameString = "")
    {
        this.pet = pet;
        iconFrame = petIconFrame.GetComponent<Image>();
        if (pet == null) return;
        icon = iconFrame.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        button = iconFrame.gameObject.GetComponent<Button>();
        checkmark = iconFrame.gameObject.transform.GetChild(1).gameObject;
        lockObject = iconFrame.gameObject.transform.GetChild(2).gameObject;
        button.onClick.AddListener(OnClick);
        popupUI.SetTargetObject(iconFrame.gameObject, () => PopupString(NameString));
        icon.sprite = sprite.monsters[0][(int)pet.species][(int)pet.color];
    }
    public void UpdateUI()
    {
        if (pet == null)
        {
            SetActive(iconFrame.gameObject, false);
            return;
        }
        SetActive(lockObject, pet.rank.value < 1);
        iconFrame.color = pet.isActive ? Color.green : Color.white;
        SetActive(checkmark, pet.isActive);
        button.interactable = pet.CanSwitchActive();
    }
    void OnClick()
    {
        if (pet == null) return;
        pet.SwitchActive();
        gameUI.menuUI.MenuUI(MenuKind.Bestiary).GetComponent<BestiaryMenuUI>().SetActiveUI();
    }
    string PopupString(string NameString = "")
    {
        string tempStr = optStr + "<size=20>" + "Pet QoL";//NameString;
        if (pet.rank.value < 1) tempStr += optStr + "   \n<sprite=\"locks\" index=0> " + pet.globalInfo.Name() + " Pet Rank 1";
        if (pet.isActive) tempStr += "  <color=green>Active</color>";
        tempStr += "<size=18>\n- ";
        tempStr += localized.PetActiveEffect(pet.activeEffectKind);
        //else
        //{
        //    tempStr += "\n" + localized.PetActiveEffect(pet.activeEffectKind);
        //}
        return tempStr;
    }

    public MonsterPet pet;
    public Image iconFrame, icon;
    public GameObject checkmark, lockObject;
    public Button button;

}
