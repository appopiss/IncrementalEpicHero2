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
using static InventoryParameter;
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public class EquipmentTalismanUI : MonoBehaviour
{
    [SerializeField] GameObject[] talismans;
    TalismanUpgradeUI[] talismansUI;
    [SerializeField] TextMeshProUGUI fragmentText;
    //[SerializeField] TextMeshProUGUI passiveText;
    public OpenCloseUI openCloseUI;
    public Button openButton, closeButton;
    public TalismanUpgradePopupUI talismanPopupUI;
    [SerializeField] GameObject petQoLIcon;
    PetQoLUI petQoLUI;
    PopupUI popupUI;
    private void Start()
    {
        talismansUI = new TalismanUpgradeUI[Math.Min(talismans.Length, game.potionCtrl.talismans.Count)];
        for (int i = 0; i < talismans.Length; i++)
        {
            if (i < talismansUI.Length)
                talismansUI[i] = new TalismanUpgradeUI(talismans[i], game.potionCtrl.talismans[i]);
            else
                SetActive(talismans[i], false);
        }

        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(closeButton);

        talismanPopupUI = new TalismanUpgradePopupUI(gameUI.popupCtrlUI.equipment);
        for (int i = 0; i < talismansUI.Length; i++)
        {
            int count = i;
            talismanPopupUI.SetTargetObject(talismans[count], () => talismanPopupUI.SetUI(talismansUI[count].talisman));
        }
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        petQoLUI = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.DevifFish, MonsterColor.Purple).pet, petQoLIcon, popupUI, "Auto-Disassemble Talismans");
    }
    public void UpdateUI()
    {
        fragmentText.text = optStr + "Talisman Fragments : " + tDigit(game.alchemyCtrl.talismanFragment.value);
        //passiveText.text = 
        for (int i = 0; i < talismansUI.Length; i++)
        {
            talismansUI[i].UpdateUI();
        }
        talismanPopupUI.UpdateUI();
        popupUI.UpdateUI();
        petQoLUI.UpdateUI();
    }
    //string PassiveString()
    //{
    //    string tempStr;
    //    for (int i = 0; i < ; i++)
    //    {

    //    }
    //}
}

public class TalismanUpgradeUI
{
    public TalismanUpgradeUI(GameObject gameObject, Talisman talisman)
    {
        thisObject = gameObject;
        this.talisman = talisman;
        iconImage = thisObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        levelUpButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        disassembledNumText = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        lockObject = thisObject.transform.GetChild(4).gameObject;
        levelUpButton.onClick.AddListener(() => talisman.levelTransaction.Buy());
        levelUpButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.Upgrade));
        iconImage.sprite = sprite.potions[(int)talisman.kind];
    }
    public void UpdateUI()
    {
        SetActive(lockObject, !talisman.isUnlocked);
        if (talisman.isUnlocked)
        {
            levelText.text = optStr + "<color=green>Lv " + tDigit(talisman.level.value);
            SetActive(levelUpButton.gameObject, true);
            levelUpButton.interactable = talisman.levelTransaction.CanBuy();
            disassembledNumText.text = optStr + "#" + tDigit(talisman.disassembledNum.value);
        }
        else
        {
            levelText.text = "";
            SetActive(levelUpButton.gameObject, false);
            return;
        }
    }
    public Talisman talisman;
    public GameObject thisObject;
    public Image iconImage;
    public TextMeshProUGUI levelText, disassembledNumText;
    public Button levelUpButton;
    public GameObject lockObject;
}

public class TalismanUpgradePopupUI : PopupUI
{
    public TalismanUpgradePopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        nameText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        description = thisObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void SetUI(Talisman talisman)
    {
        this.talisman = talisman;
        if (talisman.kind == PotionKind.Nothing || !talisman.isUnlocked) return;
        icon.sprite = sprite.potions[(int)talisman.kind];
        UpdateText();
        DelayShow();
    }
    void UpdateText()
    {
        nameText.text = NameString();
        description.text = DescriptionString();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        //if (thisPotion().globalInfo.kind == PotionKind.Nothing)
        //{
        //    DelayHide();
        //    return;
        //}
        base.UpdateUI();
        UpdateText();
    }
    string NameString()
    {
        string tempString = optStr + "<size=20>" + localized.PotionName(talisman.kind) + " < <color=green>Lv " + tDigit(talisman.level.value) + "</color> >";
        tempString += optStr + "<size=18>\n<color=green>Lv " + tDigit(talisman.level.value) + "</color> / " + tDigit(talisman.level.maxValue());
        tempString += optStr + "\n\n<size=18>" + "Type : Talisman (" + talisman.talismanRarity.ToString() + ")";
        tempString += optStr + "\n<size=18>" + "Disassembled # : " + tDigit(talisman.disassembledNum.value);
        return tempString;
    }
    string DescriptionString()
    {
        string tempString = optStrL + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>"
        + "\n- " + localized.PotionConsume(talisman.consumeKind, talisman.consumeChance)
        + "\n- Remains active while equipped in Utility slot"
        + "\n- Fragments Gain when disassembled : " + tDigit(talisman.DisassembleGoldNum())
        + " ( + " + tDigit(talisman.talismanDisassembleFragmentNumPerLevel) + " / Level )";

        tempString += "\n\n";
        //Effect
        tempString += optStr + "<size=20><u>Equipped Effect per Stack #</u><size=18>";
        tempString += optStr + "\n- Current : " + localized.PotionEffect(talisman.kind, talisman.effectValue);
        tempString += optStrL + "\n- Next : " + localized.PotionEffect(talisman.kind, talisman.nextEffectValue) + "  ( <color=green>Lv " + tDigit(talisman.levelTransaction.ToLevel()) + "</color> ) "; ;
        //CostToLevelUp
        tempString += optStr + "\n\n<size=20><u>" + localized.Basic(BasicWord.LevelupCost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(talisman.levelTransaction.Cost()) + " Fragments";
        tempString += optStr + "  ( <color=green>Lv " + tDigit(talisman.levelTransaction.ToLevel()) + "</color> ) ";

        if (talisman.talismanRarity == TalismanRarity.Epic)
            tempString += "\n\n<color=yellow>This Talisman cannot be disassembled.</color>";
        else
        {
            //PassiveEffectOnDisassemble
            tempString += optStr + "\n\n" + "<size=20><u>Passive Effect per Disassembled #</u><size=18>";
            tempString += optStr + "\n- " + localized.PotionEffect(talisman.kind, talisman.PassiveEffectValue(1), true);
            tempString += optStr + "\n\n" + "<size=20><u>Current Total Passive Effect</u><size=18>";
            tempString += optStr + "\n- " + localized.PotionEffect(talisman.kind, talisman.passiveEffectValue, true);
        }
        return tempString;
    }


    public Talisman talisman;
    public Image icon;
    public TextMeshProUGUI nameText, description;
}