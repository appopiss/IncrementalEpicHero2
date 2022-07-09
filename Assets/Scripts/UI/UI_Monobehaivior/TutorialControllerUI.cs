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
using System.Text;

public partial class Save
{
    public TutorialArrowKind currentTutorialArrow;
}
public class TutorialControllerUI : MonoBehaviour
{
    [SerializeField] GameObject[] arrows;
    [SerializeField] CanvasGroup infoCanvasGroup;
    [SerializeField] TextMeshProUGUI infoText;

    public TutorialArrowKind arrowKind { get => main.S.currentTutorialArrow; set => main.S.currentTutorialArrow = value; }

    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (!gameUI.titleSceneUI.isLoaded) return;
        if (arrowKind == TutorialArrowKind.Finish) SetActive(gameObject, false);
        for (int i = 0; i < arrows.Length; i++)
        {
            SetActive(arrows[i], i == (int)arrowKind && ActiveCondition());
        }
        if (NextArrowCondition()) arrowKind++;
    }

    bool NextArrowCondition()
    {
        switch (arrowKind)
        {
            case TutorialArrowKind.QuestTab:
                return gameUI.menuUI.MenuUI(MenuKind.Quest).openClose.isOpen;
            case TutorialArrowKind.AcceptButton:
                return game.questCtrl.Quest(QuestKindGlobal.AbilityVIT).isAccepted;
            case TutorialArrowKind.AbilityVIT:
                return game.statsCtrl.Ability(HeroKind.Warrior, AbilityKind.Vitality).point.value > 0;
            case TutorialArrowKind.GeneralQuest:
                return gameUI.menuUI.MenuUI(MenuKind.Quest).GetComponent<QuestMenuUI>().kindSwitchTabUI.currentId == 3;
            case TutorialArrowKind.SkillRank:
                return game.skillCtrl.Skill(HeroKind.Warrior, 0).rank.value > 1;
            case TutorialArrowKind.MysteriousWaterExpand:
                return game.alchemyCtrl.mysteriousWaterCap.Value() >= 3;
            case TutorialArrowKind.CatalystLevel:
                return game.catalystCtrl.Catalyst(CatalystKind.Slime).level.value > 0;
            case TutorialArrowKind.CatalystClick:
                return game.catalystCtrl.Catalyst(CatalystKind.Slime).isEquipped;
            case TutorialArrowKind.EssenceClick:
                return game.catalystCtrl.Catalyst(CatalystKind.Slime).essenceProductionList[0].allocatedWaterPerSec.value > 0;
            case TutorialArrowKind.PotionClick:
                return game.potionCtrl.GlobalInfo(PotionKind.MinorHealthPotion).productedNum.value > 0;
            case TutorialArrowKind.Finish:
                return false;
        }
        return false;
    }
    bool ActiveCondition()
    {
        switch (arrowKind)
        {
            case TutorialArrowKind.AbilityVIT:
                return game.statsCtrl.AbilityPointLeft(HeroKind.Warrior).value > 0;
            case TutorialArrowKind.GeneralQuest:
                return game.questCtrl.Quest(QuestKindGlobal.ClearGeneralQuest).isAccepted;
            case TutorialArrowKind.SkillRank:
                return game.questCtrl.Quest(QuestKindTitle.SkillMaster1, HeroKind.Warrior).isAccepted;
            case TutorialArrowKind.MysteriousWaterExpand:
                return game.questCtrl.Quest(QuestKindGlobal.Alchemy).isAccepted;
            case TutorialArrowKind.PotionClick:
                return game.potionCtrl.GlobalInfo(PotionKind.MinorHealthPotion).alchemyTransaction.CanBuy();
            case TutorialArrowKind.Finish:
                break;
        }
        return true;
    }
}

public enum TutorialArrowKind
{
    QuestTab,
    AcceptButton,
    AbilityVIT,
    GeneralQuest,
    SkillRank,
    MysteriousWaterExpand,
    CatalystLevel,
    CatalystClick,
    EssenceClick,
    PotionClick,
    Finish,
}
