using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class BattleControllerUI : MonoBehaviour
{
    Func<BattleController> battleCtrl;

    [SerializeField] GameObject hero;
    [SerializeField] GameObject[] heroAllys;
    [SerializeField] GameObject[] pets;
    [SerializeField] GameObject[] monsters;
    [SerializeField] GameObject[] challengeMonsters;
    //Challenge用を追加
    HeroUI heroUI;
    HeroAllyUI[] heroAllysUI;
    PetUI[] petsUI;
    MonsterUI[] monsterUI;
    MonsterUI[] challengeMonstersUI;
    [SerializeField] GameObject battleResult;
    BattleResultUI battleResultUI;
    [SerializeField] TextMeshProUGUI topText;
    public MonsterStatsPopupUI monsterStatsPopupUI;
    public PetStatsPopupUI petStatsPopupUI;
    public Image fieldImage;
    [SerializeField] Button prevRegionButton, prevAreaButton, nextRegionButton, nextAreaButton, leftButton, rightButton;
    TextMeshProUGUI prevRegionText, prevAreaText, nextRegionText, nextAreaText;
    [SerializeField] TextMeshProUGUI counterText, difficultyText;

    [SerializeField] Button quickAccessButton;
    TextMeshProUGUI quickAccessText;
    [SerializeField] CanvasGroup quickAccessCanvas;
    [SerializeField] GameObject[] petQoLIcons;
    PetQoLUI[] petQoLUIs;

    [SerializeField] Button conveneButton;
    GameObject conveneHideObject;
    public PopupUI popupUI;
    public PopupUI popupUISpiderQoL;
    public GameObject logCanvasObject;
    [SerializeField] GameObject swarmObject;
    TextMeshProUGUI swarmText;
    Button swarmButton;

    public Button areaInfoButton;
    [SerializeField] GameObject areaInfo;
    public AreaInfoUI areaInfoUI;

    public GameObject trapParticle;

    //DungeonBlessing
    public DungeonBlessingConfirmUI dungeonBlessingConfirmUI;

    public AreaPrestigeUpgradePopupUI areaPrestigeUpgradePopupUI;

    private void Awake()
    {
        battleCtrl = () => game.battleCtrl;
        heroUI = new HeroUI(battleCtrl, hero);
        heroAllysUI = new HeroAllyUI[heroAllys.Length];
        for (int i = 0; i < heroAllysUI.Length; i++)
        {
            int count = i;
            heroAllysUI[i] = new HeroAllyUI(()=>battleCtrl().heroAllys[count], battleCtrl, heroAllys[count]);
        }
        petsUI = new PetUI[pets.Length];
        for (int i = 0; i < pets.Length; i++)
        {
            petsUI[i] = new PetUI(battleCtrl, pets[i], i);
        }
        monsterUI = new MonsterUI[monsters.Length];
        for (int i = 0; i < monsters.Length; i++)
        {
            monsterUI[i] = new MonsterUI(battleCtrl, monsters[i], i);
        }
        challengeMonstersUI = new MonsterUI[challengeMonsters.Length];
        //for (int i = 0; i < challengeMonstersUI.Length; i++)
        //{
        //    int count = i;
        //    challengeMonstersUI[i] = new MonsterUI(battleCtrl, challengeMonsters[count], count, true);
        //}
        //Challengeは個別で
        challengeMonstersUI[(int)ChallengeMonsterKind.SlimeKing] = new MonsterUI(battleCtrl, challengeMonsters[(int)ChallengeMonsterKind.SlimeKing], (int)ChallengeMonsterKind.SlimeKing, true);
        challengeMonstersUI[(int)ChallengeMonsterKind.WindowQueen] = new MonsterUI(battleCtrl, challengeMonsters[(int)ChallengeMonsterKind.WindowQueen], (int)ChallengeMonsterKind.WindowQueen, true);
        challengeMonstersUI[(int)ChallengeMonsterKind.Golem] = new ChallengeBossUI_Golem(battleCtrl, challengeMonsters[(int)ChallengeMonsterKind.Golem], (int)ChallengeMonsterKind.Golem, true);
        challengeMonstersUI[(int)ChallengeMonsterKind.Golem].SetUI();

        battleResultUI = new BattleResultUI(battleCtrl, battleResult);
        fieldImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();

        nextRegionText = nextRegionButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        nextAreaText = nextAreaButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        prevRegionText = prevRegionButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        prevAreaText = prevAreaButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        //外部アクセスがあるためここで呼ぶ
        areaInfoUI = new AreaInfoUI(areaInfo);
        areaInfoUI.thisOpenClose.SetOpenButton(areaInfoButton);
        areaInfoButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.AreaInfo));

        swarmText = swarmObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        swarmButton = swarmObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        //Popup
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUI.isPreventSetCorner = true;
        popupUISpiderQoL = new PopupUI(gameUI.popupCtrlUI.defaultFixedWidthPopup);
        monsterStatsPopupUI = new MonsterStatsPopupUI(gameUI.popupCtrlUI.monsterStats);
        petStatsPopupUI = new PetStatsPopupUI(gameUI.popupCtrlUI.monsterStats);
        areaPrestigeUpgradePopupUI = new AreaPrestigeUpgradePopupUI(gameUI.popupCtrlUI.defaultPopup);

        quickAccessText = quickAccessButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        heroUI.Start();
        spriteId = new ArrayId(0, () => 2);
        ChangeSprite();

        //AreaNextButton
        nextRegionButton.onClick.AddListener(() => GoToNextArea(true));
        nextAreaButton.onClick.AddListener(() => GoToNextArea(false));
        //nextButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.AreaSelect));
        prevRegionButton.onClick.AddListener(() => GoToPrevArea(true));
        prevAreaButton.onClick.AddListener(() => GoToPrevArea(false));
        //prevButton.onClick.AddListener(() => gameUI.soundUI.Play(SoundEffect.AreaSelect));
        leftButton.onClick.AddListener(() => areaInfoUI.ChangeAreaLevel(-1));
        rightButton.onClick.AddListener(() => areaInfoUI.ChangeAreaLevel(1));

        //Popup
        for (int i = 0; i < monsterUI.Length; i++)
        {
            int count = i;
            monsterStatsPopupUI.SetTargetObject(monsterUI[count].thisObject, () => monsterStatsPopupUI.SetUI(() => monsterUI[count].monsterBattle));
        }
        for (int i = 0; i < challengeMonstersUI.Length; i++)
        {
            int count = i;
            monsterStatsPopupUI.SetTargetObject(challengeMonstersUI[count].thisObject, () => monsterStatsPopupUI.SetUI(() => challengeMonstersUI[count].monsterBattle));
        }
        for (int i = 0; i < areaInfoUI.monsters.Length; i++)
        {
            int count = i;
            monsterStatsPopupUI.SetTargetObject(areaInfoUI.monsters[count], () => monsterStatsPopupUI.SetUI(() => areaInfoUI.monsterBattles[count]));
        }
        for (int i = 0; i < areaInfoUI.prestigeUpradesUI.Length; i++)
        {
            int count = i;
            areaPrestigeUpgradePopupUI.SetTargetObject(areaInfoUI.prestigeUpgrades[count], () => areaPrestigeUpgradePopupUI.ShowAction(areaInfoUI.prestigeUpradesUI[count].upgrade));
        }
        popupUI.SetTargetObject(nextRegionButton.gameObject, () => NextAreaPopupString(true));
        popupUI.SetTargetObject(nextAreaButton.gameObject, () => NextAreaPopupString(false));
        popupUI.SetTargetObject(prevRegionButton.gameObject, () => PrevAreaPopupString(true));
        popupUI.SetTargetObject(prevAreaButton.gameObject, () => PrevAreaPopupString(false));
        popupUI.additionalShowCondition = () => popupUI.thisText.text != "";
        for (int i = 0; i < petsUI.Length; i++)
        {
            int count = i;
            petStatsPopupUI.SetTargetObject(petsUI[count].thisObject, () => petStatsPopupUI.SetUI(() => petsUI[count].petBattle));
        }

        //Swarm
        swarmButton.onClick.AddListener(() => game.battleCtrl.areaBattle.Start(game.areaCtrl.currentSwarmArea));
        popupUI.SetTargetObject(swarmObject, SwarmPopupString);

        dungeonBlessingConfirmUI = new DungeonBlessingConfirmUI(gameUI.popupCtrlUI.dungeonBlessingConfirm);

        //PetQoL
        petQoLUIs = new PetQoLUI[petQoLIcons.Length];
        petQoLUIs[0] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Normal).pet, petQoLIcons[0], popupUI, "Auto-Pickup Resources");
        petQoLUIs[1] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Blue).pet, petQoLIcons[1], popupUI, "Auto-Pickup Materials"); ;
        petQoLUIs[2] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Slime, MonsterColor.Red).pet, petQoLIcons[2], popupUI, "Auto-Pickup Equipment");
        petQoLUIs[3] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Spider, MonsterColor.Normal).pet, petQoLIcons[3], popupUISpiderQoL, "Auto-Capture Monsters");
        petQoLUIs[4] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Blue).pet, petQoLIcons[4], popupUI, "Auto-Retry Dungeons");
        petQoLUIs[5] = new PetQoLUI(game.monsterCtrl.GlobalInformation(MonsterSpecies.Fairy, MonsterColor.Red).pet, petQoLIcons[5], popupUI, "Auto-Open Chests");
        conveneHideObject = conveneButton.gameObject.transform.GetChild(0).gameObject;
        conveneButton.onClick.AddListener(Convene);
        popupUI.SetTargetObject(conveneButton.gameObject, ConvenePopupString);

        quickAccessButton.onClick.AddListener(() => SwitchShowQuickAccessCanvas(!isShowQuickAccess));
        SwitchShowQuickAccessCanvas(false);
    }
    bool isShowQuickAccess;
    void SwitchShowQuickAccessCanvas(bool show)
    {
        isShowQuickAccess = show;
        quickAccessCanvas.alpha = Convert.ToInt16(show);
        quickAccessCanvas.interactable = show;
        quickAccessCanvas.blocksRaycasts = show;
        quickAccessText.text = show ? "▲" : "▼";
    }
    string tempStr;
    string ConvenePopupString()
    {        
        if (game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased())
        {
            tempStr = "<size=20>Convene<size=18>\n- Click to convene all passive heroes in background.\n- You cannot use this in Dungeon or Challenge.";
            if (game.epicStoreCtrl.Item(EpicStoreKind.FavoriteArea).IsPurchased())
                tempStr += "\n- Shift + Click to make all heroes go to their favorite area.";
        }
        else
            tempStr = "<size=20><sprite=\"locks\" index=0> Epic Store [ Convene ]";
        return tempStr;
    }
    void Convene()
    {
        if (currentArea.isDungeon) return;
        if (currentArea.isChallenge) return;
        if (isShiftPressed)
        {
            for (int i = 0; i < game.battleCtrls.Length; i++)
            {
                int count = i;
                if (game.battleCtrls[i].isActiveBattle)
                    game.battleCtrls[i].areaBattle.Start(game.rebirthCtrl.FavoriteArea((HeroKind)count));
            }
            return;
        }        
        game.autoCtrl.Convene(currentArea);
    }

    public void Initialize()
    {
        heroUI.Initialize();
        for (int i = 0; i < heroAllysUI.Length; i++)
        {
            heroAllysUI[i].Initialize();
        }
        for (int i = 0; i < petsUI.Length; i++)
        {
            petsUI[i].Initialize();
        }
        for (int i = 0; i < monsterUI.Length; i++)
        {
            monsterUI[i].Initialize();
        }
        for (int i = 0; i < challengeMonstersUI.Length; i++)
        {
            challengeMonstersUI[i].Initialize();
        }

        game.battleCtrl.blessingCtrl.selectBlessingTask = dungeonBlessingConfirmUI.ShowWindow;
        game.battleCtrl.areaBattle.fieldUIAction = SetUI;
        battleResultUI.Initialize();
    }

    async void GoToNextArea(bool isRegion)
    {
        game.battleCtrl.areaBattle.Start(NextArea(isRegion));
        //AreaKind areaKind = game.battleCtrl.areaBattle.currentAreaKind;
        //int id = Math.Min(game.areaCtrl.areas[(int)areaKind].Length, game.battleCtrl.areaBattle.currentAreaId + 1);
        //game.battleCtrl.areaBattle.Start(areaKind, id);
        //await UniTask.DelayFrame(2);
        //if (NextAreaPopupString(isRegion) == "") popupUI.DelayHide();
        //else popupUI.DelayShow();
    }
    async void GoToPrevArea(bool isRegion)
    {
        game.battleCtrl.areaBattle.Start(PrevArea(isRegion));
        //AreaKind areaKind = game.battleCtrl.areaBattle.currentAreaKind;
        //int id = Math.Max(0, game.battleCtrl.areaBattle.currentAreaId - 1);
        //game.battleCtrl.areaBattle.Start(areaKind, id);
        //await UniTask.DelayFrame(2);
        //if (PrevAreaPopupString(isRegion) == "") popupUI.DelayHide();
        //else popupUI.DelayShow();
    }
    //AREA nextArea;
    string NextAreaPopupString(bool isRegion)
    {
        //if (game.battleCtrl.areaBattle.currentAreaId >= 7) return "";
        //nextArea = game.areaCtrl.Area(game.battleCtrl.areaBattle.currentAreaKind, game.battleCtrl.areaBattle.currentAreaId + 1);
        //string tempStr = optStr;
        //tempStr += "<size=20>" + nextArea.Name() + "<size=18>";
        //if (!nextArea.IsUnlocked())
        //{
        //    tempStr += "\n<sprite=\"locks\" index=0> Required : ";
        //    foreach (var item in nextArea.requiredCompleteNum)
        //    {
        //        tempStr += optStr + "\n- " + localized.Basic(BasicWord.Area) + " " + tDigit(item.Key + 1) + " Completed # " + tDigit(item.Value);
        //    }
        //}
        if (NextArea(isRegion) == game.areaCtrl.nullArea) return "";
        tempStr = optStr + "<size=20>" + NextArea(isRegion).Name() + "<size=18>";
        if (!NextArea(isRegion).CanStart())
        {
            tempStr += "\n<sprite=\"locks\" index=0> Required : ";
            if (isRegion)
            {
                tempStr += "Building [Cartographer] Lv " + tDigit(Cartographer.areaUnlockLevels[(int)NextArea(isRegion).kind]);
            }
            else
            {
                foreach (var item in NextArea(isRegion).requiredCompleteNum)
                {
                    tempStr += optStr + "\n- " + localized.Basic(BasicWord.Area) + " " + tDigit(item.Key + 1) + " Completed # " + tDigit(item.Value);
                }
            }
        }
        return tempStr;
    }
    //AREA prevArea;
    string PrevAreaPopupString(bool isRegion)
    {
        //if (game.battleCtrl.areaBattle.currentAreaId <= 0) return "";
        //prevArea = game.areaCtrl.Area(game.battleCtrl.areaBattle.currentAreaKind, game.battleCtrl.areaBattle.currentAreaId - 1);
        //string tempStr = optStr;
        //tempStr += "<size=20>" + prevArea.Name() + "<size=18>";
        //if (!prevArea.IsUnlocked())
        //{
        //    tempStr += "\n<sprite=\"locks\" index=0> Required : ";
        //    foreach (var item in prevArea.requiredCompleteNum)
        //    {
        //        tempStr += optStr + "\n- " + localized.Basic(BasicWord.Area) + " " + tDigit(item.Key + 1) + " Completed # " + tDigit(item.Value);
        //    }
        //}
        if (PrevArea(isRegion) == game.areaCtrl.nullArea) return "";
        tempStr = optStr + "<size=20>" + PrevArea(isRegion).Name() + "<size=18>";
        if (!PrevArea(isRegion).CanStart())
        {
            tempStr += "\n<sprite=\"locks\" index=0> Required : ";
            if (isRegion)
            {
                tempStr += "Building [Cartographer] Lv " + tDigit(Cartographer.areaUnlockLevels[(int)PrevArea(isRegion).kind]);
            }
            else
            {
                foreach (var item in PrevArea(isRegion).requiredCompleteNum)
                {
                    tempStr += optStr + "\n- " + localized.Basic(BasicWord.Area) + " " + tDigit(item.Key + 1) + " Completed # " + tDigit(item.Value);
                }
            }
        }
        return tempStr;
    }

    AREA currentArea => game.battleCtrl.areaBattle.CurrentArea();
    bool isAscendedOnce;
    bool isNormalArea;
    public void UpdateUI()
    {
        monsterStatsPopupUI.UpdateUI();
        petStatsPopupUI.UpdateUI();
        areaPrestigeUpgradePopupUI.UpdateUI();

        if (gameUI.worldMapUI.thisOpenClose.isOpen) return;
        heroUI.UpdateUI();
        for (int i = 0; i < heroAllysUI.Length; i++)
        {
            heroAllysUI[i].UpdateUI();
        }
        for (int i = 0; i < petsUI.Length; i++)
        {
            petsUI[i].UpdateUI();
        }
        for (int i = 0; i < monsterUI.Length; i++)
        {
            monsterUI[i].UpdateUI();
        }
        for (int i = 0; i < challengeMonstersUI.Length; i++)
        {
            challengeMonstersUI[i].UpdateUI();
        }

        topText.text = AreaString();
        isNormalArea = !currentArea.isDungeon && !currentArea.isChallenge;
        isAscendedOnce = main.S.ascensionNum[0] > 0;
        SetActive(leftButton.gameObject, isAscendedOnce && isNormalArea);
        SetActive(rightButton.gameObject, isAscendedOnce && isNormalArea);
        SetActive(difficultyText.gameObject, isAscendedOnce && isNormalArea);
        if (isAscendedOnce && isNormalArea)
        {
            leftButton.interactable = !area.level.IsMined();
            rightButton.interactable = !area.level.IsMaxed();
            difficultyText.text = optStr + "Dif. " +  tDigit(area.level.value + 1);
        }
        areaInfoUI.UpdateUI();

        timeCount++;
        if (timeCount >= 25)//25フレームごと
        {
            ChangeSprite();
            timeCount = 0;
        }
        popupUI.UpdateUI();
        popupUISpiderQoL.UpdateUI();
        //Dungeon
        SetActive(counterText.gameObject, currentArea.isDungeon);
        SetActive(nextRegionButton.gameObject, isNormalArea);
        SetActive(nextAreaButton.gameObject, isNormalArea);
        SetActive(prevRegionButton.gameObject, isNormalArea);
        SetActive(prevAreaButton.gameObject, isNormalArea);
        SetActive(areaInfoButton.gameObject, !currentArea.isChallenge);
        SetActive(gameUI.worldMapUI.openButton.gameObject, !currentArea.isChallenge);
        SetActive(swarmObject, game.areaCtrl.isSwarm);
        if (game.areaCtrl.isSwarm)
        {
            swarmText.text = SwarmString(game.areaCtrl.currentSwarmArea);
            swarmButton.interactable = currentArea.isChallenge || !currentArea.swarm.isSwarm;
        }

        for (int i = 0; i < petQoLUIs.Length; i++)
        {
            if (i < 4 || currentArea.isDungeon)
                petQoLUIs[i].UpdateUI();
            SetActive(petQoLIcons[i], i < 4 || currentArea.isDungeon);
        }
        conveneButton.interactable = game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased() && !currentArea.isDungeon && !currentArea.isChallenge;
        SetActive(conveneHideObject, !game.epicStoreCtrl.Item(EpicStoreKind.Convene).IsPurchased());
        if (currentArea.isDungeon)
        {
            counterText.text = optStr + "<size=24><sprite=\"counter\" index=0> " + DoubleTimeToDate(currentArea.TimeLeft(battleCtrl()));
            return;
        }
        //AreaNextButton
        //areaId = currentArea.id;//game.battleCtrl.areaBattle.currentAreaId;
        //areaKind = currentArea.kind;//game.battleCtrl.areaBattle.currentAreaKind;
        nextRegionText.text = NextButtonString(true);//areaId < game.areaCtrl.areas[(int)areaKind].Length - 1 ? tDigit(areaId + 1 + 1) : "";
        nextAreaText.text = NextButtonString(false);//areaId < game.areaCtrl.areas[(int)areaKind].Length - 1 ? tDigit(areaId + 1 + 1) : "";
        prevRegionText.text = PrevButtonString(true);//areaId > 0 ? tDigit(areaId - 1 + 1) : "";
        prevAreaText.text = PrevButtonString(false);//areaId > 0 ? tDigit(areaId - 1 + 1) : "";
        nextRegionButton.interactable = NextArea(true) != game.areaCtrl.nullArea && NextArea(true).CanStart();// areaId < game.areaCtrl.areas[(int)areaKind].Length - 1 && game.areaCtrl.Area(areaKind, areaId + 1).IsUnlocked();
        nextAreaButton.interactable = NextArea(false) != game.areaCtrl.nullArea && NextArea(false).CanStart();// areaId < game.areaCtrl.areas[(int)areaKind].Length - 1 && game.areaCtrl.Area(areaKind, areaId + 1).IsUnlocked();
        prevRegionButton.interactable = PrevArea(true) != game.areaCtrl.nullArea && PrevArea(true).CanStart(); //areaId > 0 && game.areaCtrl.Area(areaKind, areaId - 1).IsUnlocked();
        prevAreaButton.interactable = PrevArea(false) != game.areaCtrl.nullArea && PrevArea(false).CanStart(); //areaId > 0 && game.areaCtrl.Area(areaKind, areaId - 1).IsUnlocked();

    }
    AREA NextArea(bool isRegion)
    {
        if (isRegion)
        {
            if((int)areaKind < game.areaCtrl.areas.Length - 1)
               return game.areaCtrl.Area(currentArea.kind + 1, 0);
            return game.areaCtrl.nullArea;
        }
        if (areaId < game.areaCtrl.areas[(int)areaKind].Length - 1)
            return game.areaCtrl.Area(currentArea.kind, areaId + 1);
        //if ((int)areaKind < game.areaCtrl.areas.Length - 1)
        //    return game.areaCtrl.Area(currentArea.kind + 1, 0);
        return game.areaCtrl.nullArea;
    }
    AREA PrevArea(bool isRegion)
    {
        if (isRegion)
        {
            if ((int)areaKind > 0)
                return game.areaCtrl.Area(currentArea.kind - 1, 0);// game.areaCtrl.areas[(int)areaKind].Length - 1);
            return game.areaCtrl.nullArea;
        }
        if (areaId > 0)
            return game.areaCtrl.Area(currentArea.kind, areaId - 1);
        //if ((int)areaKind > 0)
        //    return game.areaCtrl.Area(currentArea.kind - 1, game.areaCtrl.areas[(int)areaKind].Length - 1);
        return game.areaCtrl.nullArea;
    }
    string NextButtonString(bool isRegion)
    {
        AREA area = NextArea(isRegion);
        if (area == game.areaCtrl.nullArea) return "";
        if (isRegion) return optStr + "<sprite=\"monsters\" index=" + (int)area.kind + ">";
        if (!area.CanStart()) return "<sprite=\"locks\" index=0>" + tDigit(area.id + 1);
        return tDigit(area.id + 1);
        //if (areaId < game.areaCtrl.areas[(int)areaKind].Length - 1)
        //    return optStr + "<sprite=\"monsters\" index=" + (int)currentArea.kind + "> " + tDigit(areaId + 1 + 1);
        //if ((int)areaKind < game.areaCtrl.areas.Length - 1)
        //    return optStr + "<sprite=\"monsters\" index=" + ((int)currentArea.kind + 1).ToString() + "> " + tDigit(0 + 1);
        //return "";
    }
    string PrevButtonString(bool isRegion)
    {
        AREA area = PrevArea(isRegion);
        if (area == game.areaCtrl.nullArea) return "";
        if (isRegion) return optStr + "<sprite=\"monsters\" index=" + (int)area.kind + ">";
        if (!area.CanStart()) return "<sprite=\"locks\" index=0>" + tDigit(area.id + 1);
        return tDigit(area.id + 1);

        //if (areaId > 0)
        //    return optStr + "<sprite=\"monsters\" index=" + (int)currentArea.kind + "> " + tDigit(areaId - 1 + 1);
        //if ((int)areaKind > 0)
        //    return optStr + "<sprite=\"monsters\" index=" + ((int)currentArea.kind - 1).ToString() + "> " + tDigit(game.areaCtrl.areas[(int)areaKind].Length - 1 + 1);
        //return "";
    }

    int areaId => currentArea.id;
    AreaKind areaKind => currentArea.kind;
    float timeCount;
    private void Update()
    {
        monsterStatsPopupUI.Update();
    }

    public void SetUI(AREA area)
    {
        ChangeFieldSprite(area);
        areaInfoUI.SetUI();
    }
    void ChangeFieldSprite(AREA area)
    {
        if (area.isChallenge) fieldImage.sprite = sprite.fieldChallenge[area.id];
        else if (area.isDungeon) fieldImage.sprite = sprite.fieldsDungeon[(int)area.kind][area.id];
        else fieldImage.sprite = sprite.fields[(int)area.kind][area.id];
    }
    string SwarmString(AREA area)
    {
        string tempStr = optStr;
        tempStr += "<size=20><color=yellow><i>Swarming!!</i></color>";
        tempStr += "\n<size=18><sprite=\"counter\" index=0> " + DoubleTimeToDate(area.swarm.timesecLeft);
        return tempStr;
    }
    string SwarmPopupString()
    {
        AREA area = game.areaCtrl.currentSwarmArea;
        string tempStr = optStrL + "<size=20>" + SwarmRarityString(area.swarm.rarity)+ " MONSTER SWARMS OCCURRING!"
            + "<size=18>"
            + "\n- Location : " + area.Name(true, false) + " ( click to go )"
            + "\n- Monster Spawns : " + tDigit(area.swarm.spawnNumFactor) + "x ( Minimum : " + tDigit(area.swarm.minimumSpawnNum) + " )"
            + "\n- Required Area Clear #s Left to Vanquish : " + tDigit(Math.Ceiling(area.swarm.clearNumLeft))
            + "\n- Time Left : " + DoubleTimeToDate(area.swarm.timesecLeft)
            + "\n\n" + area.swarm.RewardString();
        return tempStr;
    }
    string SwarmRarityString(SwarmRarity rarity)
    {
        switch (rarity)
        {
            case SwarmRarity.Regular: return "REGULAR";
            case SwarmRarity.Large: return "<color=orange>LARGE</color>";
            case SwarmRarity.Epic: return "<color=orange>EPIC</color>";
        }
        return rarity.ToString();
    }
    AREA area;
    string AreaString()
    {
        area = game.battleCtrl.areaBattle.CurrentArea();
        string tempStr = optStr + "<size=24>" + area.Name(true) + " <size=20>( Lv " + tDigit(area.MinLevel()) + " ~ " + tDigit(area.MaxLevel()) + " )";
        if (!battleCtrl().areaBattle.CurrentArea().isChallenge && area.swarm.isSwarm) tempStr += "  <i><color=yellow>Swarming!!</color></i>";
        tempStr += optStr + "</i><size=18>\n" + localized.Basic(BasicWord.Wave) + " : " + tDigit(battleCtrl().areaBattle.CurrentWave()) + " / " + tDigit(area.MaxWaveNum());
        if (!battleCtrl().areaBattle.CurrentArea().isChallenge)
        {
            tempStr += optStr + "    " + localized.Basic(BasicWord.CompletedNum) + " " + tDigit(area.completedNum.value) + "  ( Total " + tDigit(area.completedNum.TotalCompletedNum()) + " )";
            if (area.swarm.isSwarm) tempStr += "  <i><color=yellow>" + tDigit(Math.Ceiling(area.swarm.clearNumLeft)) + "</color></i>";
            tempStr += "\n";
            for (int i = 0; i < game.battleCtrls.Length; i++)
            {
                if (game.battleCtrls[i].isActiveBattle && game.battleCtrls[i].areaBattle.CurrentArea() == area)
                    tempStr += optStr + "<sprite=\"heroes\" index=" + i + ">";
            }
        }
        return tempStr;
    }

    int spriteCount = 0;
    ArrayId spriteId;
    void ChangeSprite()//0.5秒ごとに呼ぶ
    {
        if (battleCtrl().hero.isAlive) heroUI.SetSprite((int)spriteId.value);
        for (int i = 0; i < battleCtrl().heroAllys.Count; i++)
        {
            if(battleCtrl().heroAllys[i].isAlive) heroAllysUI[i].SetSprite((int)spriteId.value);
        }
        for (int i = 0; i < battleCtrl().pets.Length; i++)
        {
            if (battleCtrl().pets[i].isAlive) petsUI[i].SetSprite((int)spriteId.value);
        }
        for (int i = 0; i < battleCtrl().monsters.Length; i++)
        {
            if (battleCtrl().monsters[i].isAlive) monsterUI[i].SetSprite((int)spriteId.value);
        }
        spriteCount++;
        if (spriteCount >= 3)//Challenge
        {
            for (int i = 0; i < challengeMonstersUI.Length; i++)
            {
                if (battleCtrl().challengeMonsters[i].isAlive) challengeMonstersUI[i].SetSprite((int)spriteId.value);
            }
            spriteCount = 0;
        }
        spriteId.Increase();
    }

    public void CaptureParticle(Vector2 position)
    {
        if (SettingMenuUI.Toggle(ToggleKind.DisableParticle).isOn) return;
        trapParticle.GetComponent<RectTransform>().anchoredPosition = position;
        trapParticle.GetComponent<ParticleSystem>().Play();
    }
}

public class DungeonBlessingConfirmUI : ConfirmUI
{
    public TextMeshProUGUI descriptionText;
    public Button[] selectButtons = new Button[3];
    public DungeonBlessingConfirmUI(GameObject thisObject) : base(thisObject)
    {
        descriptionText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        selectButtons[0] = thisObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        selectButtons[1] = thisObject.transform.GetChild(4).gameObject.GetComponent<Button>();
        selectButtons[2] = thisObject.transform.GetChild(5).gameObject.GetComponent<Button>();
    }
    static int showTime = 15;
    public async Task ShowWindow()
    {
        bool isSelected = false;
        List<BLESSING> tempBlessingKindList = game.battleCtrl.blessingCtrl.blessings.OrderBy(a => Guid.NewGuid()).ToList();
        selectButtons[0].onClick.RemoveAllListeners();
        selectButtons[1].onClick.RemoveAllListeners();
        selectButtons[2].onClick.RemoveAllListeners();
        selectButtons[0].onClick.AddListener(() => { tempBlessingKindList[0].StartBlessing(); isSelected = true; Hide(); });
        selectButtons[1].onClick.AddListener(() => { tempBlessingKindList[1].StartBlessing(); isSelected = true; Hide(); });
        selectButtons[2].onClick.AddListener(() => { tempBlessingKindList[2].StartBlessing(); isSelected = true; Hide(); });
        selectButtons[0].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = BlessingString(tempBlessingKindList[0]);
        selectButtons[1].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = BlessingString(tempBlessingKindList[1]);
        selectButtons[2].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = BlessingString(tempBlessingKindList[2]);
        DelayShow();
        for (int i = 0; i < showTime; i++)
        {
            descriptionText.text = optStr + "Choose one blessing! ( " + DoubleTimeToDate(showTime - i) + " left )";
            await UniTask.Delay(1000, true);
            if (isSelected) break;
            if (i == showTime - 1) selectButtons[0].onClick.Invoke();
        }
        Hide();
    }
    string BlessingString(BLESSING blessing)
    {
        string tempStr = optStr + "<size=20>";
        tempStr += blessing.NameString() + "<size=18>  ( Duration : " + DoubleTimeToDate(blessing.duration) + " )";
        tempStr += "<size=18>";
        tempStr += "\n- " + blessing.EffectString();
        if (blessing.SubEffectString() != "") tempStr += "\n- " + blessing.SubEffectString();
        return tempStr;
    }
}

public class AreaInfoUI
{
    TextMeshProUGUI totalCompletedNumText, infoText, prestigeLevelText, areaInfoText, scoreText, missionText, missionRewardText, prestigePointText;
    GameObject areaPrestigeHideObject;
    Button areaPrestigeResetButton;
    PopupUI popupUI;
    public AreaInfoUI(GameObject gameObject)
    {
        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        thisObject = gameObject;
        thisOpenClose = new OpenCloseUI(thisObject, true, true);
        thisOpenClose.SetCloseButton(thisObject.transform.GetChild(0).gameObject.GetComponent<Button>());
        infoText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        totalCompletedNumText = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        prestigeLevelText = thisObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();

        leftButton = thisObject.transform.GetChild(5).gameObject.GetComponent<Button>();
        rightButton = thisObject.transform.GetChild(6).gameObject.GetComponent<Button>();
        leftButton.onClick.AddListener(() => ChangeAreaLevel(-1));
        rightButton.onClick.AddListener(() => ChangeAreaLevel(1));

        areaInfoText = thisObject.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        scoreText = thisObject.transform.GetChild(10).gameObject.GetComponent<TextMeshProUGUI>();
        missionText = thisObject.transform.GetChild(11).gameObject.GetComponent<TextMeshProUGUI>();
        missionRewardText = thisObject.transform.GetChild(12).gameObject.GetComponent<TextMeshProUGUI>();
        uniqueEQobject = thisObject.transform.GetChild(8).gameObject;
        uniqueEQquestion = uniqueEQobject.transform.GetChild(1).gameObject;
        attemptNoEQButton = thisObject.transform.GetChild(14).gameObject.GetComponent<Button>();
        //attemptNoEQText = attemptNoEQButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        attemptOnlyBaseButton = thisObject.transform.GetChild(15).gameObject.GetComponent<Button>();
        //attemptOnlyBaseText = attemptOnlyBaseButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        areaPrestigeResetButton = thisObject.transform.GetChild(17).gameObject.GetComponent<Button>();
        areaPrestigeHideObject = thisObject.transform.GetChild(18).gameObject;

        monsters = new GameObject[thisObject.transform.GetChild(9).gameObject.transform.childCount];
        monsterBattles = new MonsterBattle[monsters.Length];
        for (int i = 0; i < monsters.Length; i++)
        {
            int count = i;
            monsters[i] = thisObject.transform.GetChild(9).gameObject.transform.GetChild(count).gameObject;
            monsterBattles[i] = new MonsterBattle(game.battleCtrl);
            //monsterBattles[i] = new MonsterBattle(game.battleCtrl);
        }
        thisArea = () => game.battleCtrl.areaBattle.CurrentArea();
        prestigePointText = thisObject.transform.GetChild(13).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        prestigeUpgrades = new GameObject[thisObject.transform.GetChild(13).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.childCount];
        for (int i = 0; i < prestigeUpgrades.Length; i++)
        {
            int count = i;
            prestigeUpgrades[i] = thisObject.transform.GetChild(13).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(count).gameObject;
        }
        prestigeUpradesUI = new AreaPrestigeUpgradeUI[prestigeUpgrades.Length];
        for (int i = 0; i < prestigeUpradesUI.Length; i++)
        {
            int count = i;
            prestigeUpradesUI[i] = new AreaPrestigeUpgradeUI(prestigeUpgrades[count], () => thisArea(), count);
        }

        SetUI();

        eqMenuUI = gameUI.menuUI.MenuUI(MenuKind.Equip).GetComponent<EquipMenuUI>();

        //AttemptMission
        attemptNoEQButton.onClick.AddListener(() =>
        {
            game.battleCtrl.AttemptMissionNoEQ(thisArea());
            thisOpenClose.Close();
            if (gameUI.worldMapUI.thisOpenClose.isOpen) gameUI.worldMapUI.thisOpenClose.Close();
            gameUI.menuUI.MenuUI(MenuKind.Equip).GetComponent<EquipMenuUI>().SetUI();
        });
        attemptOnlyBaseButton.onClick.AddListener(() =>
        {
            game.battleCtrl.AttemptMissionOnlyBase(thisArea());
            thisOpenClose.Close();
            if (gameUI.worldMapUI.thisOpenClose.isOpen) gameUI.worldMapUI.thisOpenClose.Close();
            gameUI.battleStatusUI.SetSkillSlot();
        });
        areaPrestigeResetButton.onClick.AddListener(() => thisArea().prestige.ResetUpgrade());
        popupUI.SetTargetObject(areaPrestigeResetButton.gameObject, () => optStr + "<sprite=\"locks\" index=0> " + game.epicStoreCtrl.Item(EpicStoreKind.AreaPrestigeRest).NameString() + " in Epic Store", () => !game.epicStoreCtrl.Item(EpicStoreKind.AreaPrestigeRest).IsPurchased());
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
    }

    public void SetUI()
    {
        int index = 0;
        for (int j = 0; j < Enum.GetNames(typeof(MonsterSpecies)).Length; j++)
        {
            int countJ = j;
            for (int i = 0; i < thisArea().isSpawnMonsters[j].Length; i++)
            {
                int countI = i;
                if (thisArea().isSpawnMonsters[j][i])
                {
                    monsterBattles[index].Spawn((MonsterSpecies)countJ, (MonsterColor)countI, thisArea().MaxLevel(), thisArea().difficulty, Parameter.hidePosition);
                    if (index < monsters.Length - 1) index++;
                    else break;
                }
            }            
        }
        for (int i = 0; i < monsters.Length; i++)
        {
            int count = i;
            SetActive(monsters[count], count < index);
            if (count < index)
                monsters[count].GetComponent<Image>().sprite = sprite.monsters[0][(int)monsterBattles[count].species][(int)monsterBattles[count].color];
        }
        uniqueEQobject.GetComponent<Image>().sprite = sprite.equipments[(int)thisArea().uniqueEquipmentKind];
    }

    EquipMenuUI eqMenuUI;

    public void UpdateUI()
    {
        if (!thisOpenClose.isOpen) return;
        eqMenuUI.dictionaryUI.popupUI.UpdateUI();

        infoText.text = InfoString();
        totalCompletedNumText.text = optStr + "Total Completed # <color=green>" + tDigit(thisArea().completedNum.TotalCompletedNum(), 1);
        scoreText.text = ScoreString();
        areaInfoText.text = AreaInfoString();
        missionText.text = MissionString();
        missionRewardText.text = MissionRewardString();

        SetActive(prestigeLevelText.gameObject, main.S.ascensionNum[0] > 0);
        SetActive(leftButton.gameObject, main.S.ascensionNum[0] > 0);
        SetActive(rightButton.gameObject, main.S.ascensionNum[0] > 0);
        if(main.S.ascensionNum[0] > 0)
        {
            prestigeLevelText.text = optStr + "Difficulty " + tDigit(thisArea().level.value + 1);
            leftButton.interactable = !thisArea().level.IsMined();
            rightButton.interactable = !thisArea().level.IsMaxed();
        }

        SetActive(uniqueEQobject, !thisArea().isDungeon);
        SetActive(uniqueEQquestion, !uniqueEQ.globalInfo.isGotOnce);
        SetActive(attemptNoEQButton.gameObject, thisArea().missionUnlockedNum.Value() >= 3);
        SetActive(attemptOnlyBaseButton.gameObject, thisArea().missionUnlockedNum.Value() >= 4);

        SetActive(areaPrestigeHideObject, main.S.ascensionNum[0] < 1);
        if (main.S.ascensionNum[0] < 1) return;
        //if (thisArea().isDungeon) return;
        prestigePointText.text = optStr + "Area Prestige Upgrades  ( Points left : " + tDigit(thisArea().prestige.point.value) + " )";
        for (int i = 0; i < prestigeUpradesUI.Length; i++)
        {
            prestigeUpradesUI[i].UpdateUI();
        }
        areaPrestigeResetButton.interactable = game.epicStoreCtrl.Item(EpicStoreKind.AreaPrestigeRest).IsPurchased();
        popupUI.UpdateUI();
        //if (thisArea().missionUnlockedNum.Value() >= 3)
        //    attemptNoEQText.color = game.battleCtrl.isAttemptNoEQ ? Color.green : Color.white;
        //if (thisArea().missionUnlockedNum.Value() >= 4)
        //    attemptOnlyBaseText.color = game.battleCtrl.isAttemptNoEQ ? Color.green : Color.white;

    }

    string InfoString()
    {
        string tempStr = optStr + "<size=24>" + thisArea().Name(true, false) + "</size>   ";
        for (int i = 0; i < game.battleCtrls.Length; i++)
        {
            if (game.battleCtrls[i].isActiveBattle && game.battleCtrls[i].areaBattle.CurrentArea() == thisArea())
                tempStr += optStr + "<sprite=\"heroes\" index=" + i + ">";
        }
        return tempStr;
    }
    string AreaInfoString()
    {
        string tempStr = optStr + "<size=20><u>Area Info</u>";
        tempStr += "\n- Max Wave : " + tDigit(thisArea().MaxWaveNum());
        tempStr += "\n- Monster Level : Lv " + tDigit(thisArea().MinLevel()) + " ~ " + tDigit(thisArea().MaxLevel());
        if (thisArea().isDungeon)
        {
            bool isDebuff = false;
            tempStr += "\n- Field Debuff : ";
            for (int i = 0; i < thisArea().debuffElement.Length; i++)
            {
                int count = i;
                if (thisArea().debuffElement[count] != 0)
                {
                    tempStr += "<sprite=\"stats\" index=" + (6 + count).ToString() + ">" + percent(thisArea().debuffElement[count],0) + "  ";
                    isDebuff = true;
                }
            }
            if (thisArea().debuffPhyCrit != 0)
            {
                tempStr += "PhysCrit " + percent(thisArea().debuffPhyCrit,0) + "  ";
                isDebuff = true;
            }
            if (thisArea().debuffMagCrit != 0)
            {
                tempStr += "MagCrit " + percent(thisArea().debuffMagCrit,0);
                isDebuff = true;
            }
            if (!isDebuff) tempStr += "Nothing";
            tempStr += "\n- You need " + tDigit(thisArea().RequiredPortalOrb()) + " " + localized.Basic(BasicWord.PortalOrb) + " to enter this dungeon";
        }
        else
        {
            int rewardCount = 0;
            tempStr += optStr + "\n- Unique Equipment Drop Chance : <color=green>" + percent(thisArea().uniqueEquipmentDropChance.Value(), 3) + "</color>";
            tempStr += optStr + "\n- Reward per clear : <color=green>";
            foreach (var item in thisArea().rewardMaterial)
            {
                if (rewardCount >= 1)
                {
                    if (rewardCount == 1)
                    {
                        if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
                        tempStr += "\n</color>- Reward per clear : <color=green>";
                    }
                    else tempStr += ", ";
                    tempStr += tDigit(item.Value(game.currentHero)) + " " + item.Key.Name();
                }
                else
                {
                    tempStr += tDigit(item.Value(game.currentHero)) + " " + item.Key.Name();
                }
                rewardCount++;
            }
            if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
            tempStr += "</color>";
        }
        return tempStr;
    }
    string ScoreString()
    {
        string tempStr = optStr;
        tempStr += "<size=20><u>Score</u>";
        tempStr += "\n- " + localized.Basic(BasicWord.CompletedNum) + " <color=green>" + tDigit(thisArea().completedNum.value, 1);
        if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( + " + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " / clear )";
        tempStr += "</color>";
        if (main.S.ascensionNum[0] > 0) tempStr += "  <color=yellow>[ Reach # " + tDigit(thisArea().nextMilestoneCompletedNum) + " to gain 1 Area Prestige Point ]</color>";
        tempStr += "\n- " + "Best Time Completed : <color=green>" + tDigit(Math.Ceiling(thisArea().bestTime * 10) / 10d, 1) + " sec</color>";
        tempStr += "\n- " + "Best Total Gold Gained : <color=green>" + tDigit(thisArea().bestGold) + "</color>";
        tempStr += "\n- " + "Best Total EXP Gained : <color=green>" + tDigit(thisArea().bestExp) + "</color>";
        return tempStr;
    }
    string MissionString()
    {
        string tempStr = optStr;
        if (thisArea().isDungeon)
        {
            tempStr += optStr + "<size=20><u>" + localized.Basic(BasicWord.Reward) + "</u>";
            if (thisArea().rewardExp() > 0)
            {
                double tempExp = thisArea().rewardExp() * game.statsCtrl.HeroStats(game.currentHero, Stats.ExpGain).Value() + game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue();
                tempStr += optStr + "\n- " + tDigit(tempExp) + " EXP";
                if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
                tempExp *= 1 + thisArea().ClearCountBonus(game.currentHero);
                long tempLevel = game.statsCtrl.EstimatedLevel(game.currentHero, tempExp).level;
                long tempLevelIncrement = tempLevel - game.statsCtrl.HeroLevel(game.currentHero).value;
                if (tempLevelIncrement < 30)
                    tempStr += "  [ Lv " + tDigit(tempLevel) + " : EXP " + percent(game.statsCtrl.EstimatedLevel(game.currentHero, tempExp).expPercent, 0) + " ]</color>";
                else
                    tempStr += "  [ Lv " + tDigit(tempLevel) + " : <color=yellow>MAX</color> ]</color>";
            }
            if (thisArea().rewardGold() > 0)
            {
                tempStr += optStr + "\n- " + tDigit(thisArea().rewardGold() * game.statsCtrl.GoldGain().Value() + game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue()) + " " + localized.Basic(BasicWord.Gold);
                if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
            }
            //if (thisArea().rewardEC() > 0) tempStr += optStr + "\n- " + tDigit(thisQuest().rewardEC()) + " " + localized.Basic(BasicWord.EpicCoin);

            if (thisArea().rewardEnchantKind() != EnchantKind.Nothing)
            {
                tempStr += optStr + "\n- " + localized.EnchantName(thisArea().rewardEnchantKind());
                if (thisArea().rewardEnchantKind() == EnchantKind.OptionAdd)
                    tempStr += optStr + " [ " + localized.EquipmentEffectName(thisArea().rewardEnchantEffectKind()) + " Lv " + tDigit(thisArea().rewardEnchantLevel()) + " ]";
                if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
                tempStr += "</color>";
            }
            foreach (var item in thisArea().rewardPotion)
            {
                tempStr += optStr + "\n- " + tDigit(item.Value()) + " " + localized.PotionName(item.Key());
                if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
                tempStr += "</color>";
            }
            foreach (var item in thisArea().rewardMaterial)
            {
                tempStr += optStr + "\n- " + tDigit(item.Value(game.currentHero)) + " " + item.Key.Name();
                if (thisArea().ClearCountBonus(game.currentHero) > 0) tempStr += " ( x" + tDigit(1 + thisArea().ClearCountBonus(game.currentHero)) + " )";
                tempStr += "</color>";
            }

            //First Reward
            if (thisArea().rewardEnchantKindFirst() != EnchantKind.Nothing)
            {
                if (thisArea().completedNum.isReceivedFirstReward) tempStr += "<color=green>";
                else tempStr += "<color=yellow>";
                tempStr += optStr + "\n- " + localized.EnchantName(thisArea().rewardEnchantKindFirst());
                if (thisArea().rewardEnchantKindFirst() == EnchantKind.OptionAdd)
                    tempStr += optStr + " [ " + localized.EquipmentEffectName(thisArea().rewardEnchantEffectKindFirst()) + " Lv " + tDigit(thisArea().rewardEnchantLevelFirst()) + " ]";
                tempStr += optStr + "  ( one-time reward )";
            }
            foreach (var item in thisArea().rewardPotionFirst)
            {
                if (thisArea().completedNum.isReceivedFirstReward) tempStr += "<color=green>";
                else tempStr += "<color=yellow>";
                tempStr += optStr + "\n- " + tDigit(item.Value()) + " " + localized.PotionName(item.Key());
                tempStr += optStr + "  ( one-time reward )";
            }
            foreach (var item in thisArea().rewardMaterialFirst)
            {
                if (thisArea().completedNum.isReceivedFirstReward) tempStr += "<color=green>";
                else tempStr += "<color=yellow>";
                tempStr += optStr + "\n- " + tDigit(item.Value(game.currentHero)) + " " + item.Key.Name();
                tempStr += optStr + "  ( one-time reward )";
            }
        }
        else
        {
            tempStr += "<size=20><u>Mission</u>";
            for (int i = 0; i < Math.Min(thisArea().missionListArray[thisArea().level.value].Count, thisArea().missionUnlockedNum.Value()); i++)
            {
                tempStr += "\n- " + thisArea().missionListArray[thisArea().level.value][i].Description();
            }
        }
        return tempStr;
    }
    string MissionRewardString()
    {
        if (thisArea().isDungeon) return "";
        string tempStr = optStr;
        tempStr += "<size=20>";
        for (int i = 0; i < Math.Min(thisArea().missionListArray[thisArea().level.value].Count, thisArea().missionUnlockedNum.Value()); i++)
        {
            tempStr += thisArea().missionListArray[thisArea().level.value][i].RewardDescription();
        }
        return tempStr;
    }

    public GameObject thisObject;
    public OpenCloseUI thisOpenClose;
    Button leftButton, rightButton;
    public GameObject[] prestigeUpgrades;
    public AreaPrestigeUpgradeUI[] prestigeUpradesUI;
    public GameObject[] monsters;
    public MonsterBattle[] monsterBattles;
    public Func<AREA> thisArea;
    public GameObject uniqueEQobject;
    GameObject uniqueEQquestion;
    public Equipment uniqueEQ { get => game.equipmentCtrl.equipments[(int)thisArea().uniqueEquipmentKind]; }
    Button attemptNoEQButton, attemptOnlyBaseButton;
    TextMeshProUGUI attemptNoEQText, attemptOnlyBaseText;
}

public class AreaPrestigeUpgradeUI
{
    public AreaPrestigeUpgradeUI(GameObject gameObject, Func<AREA> area, int id)
    {
        thisObject = gameObject;
        this.area = area;
        this.id = id;
        nameText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        levelText = thisObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        plusButton = thisObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        plusButton.onClick.AddListener(() => upgrade.transaction.Buy());
    }
    public void UpdateUI()
    {
        if (id >= area().prestige.upgrades.Count)
        {
            SetActive(thisObject, false);
            return;
        }
        else
            SetActive(thisObject, true);
        plusButton.interactable = upgrade.transaction.CanBuy();
        nameText.text = localized.AreaPrestigeUpgrade(upgrade).name;
        levelText.text = optStr + "<color=green>Lv " + tDigit(upgrade.level.value);
    }
    int id;
    public Func<AREA> area;
    GameObject thisObject;
    public AreaPrestigeUpgrade upgrade { get => area().prestige.upgrades[Math.Min(id, area().prestige.upgrades.Count - 1)]; }
    TextMeshProUGUI nameText, levelText;
    Button plusButton;
}
public class AreaPrestigeUpgradePopupUI : PopupUI
{
    public AreaPrestigeUpgradePopupUI(GameObject thisObject) : base(thisObject)
    {
    }
    public void ShowAction(AreaPrestigeUpgrade upgrade)
    {
        SetUI(() => DescriptionString(upgrade));
    }
    string DescriptionString(AreaPrestigeUpgrade upgrade)
    {
        string tempString = optStr + "<size=20>" + localized.AreaPrestigeUpgrade(upgrade).name + " < <color=green>Lv " + tDigit(upgrade.level.value) + "</color> ><size=18>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Information) + "</u><size=18>";
        tempString += optStr + "\n- Max Level : Lv " + tDigit(upgrade.level.maxValue());
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Effect) + "</u><size=18>";
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Current) + " : " + localized.AreaPrestigeUpgrade(upgrade).currentEffect;
        tempString += optStr + "\n- " + localized.Basic(BasicWord.Next) + " : " + localized.AreaPrestigeUpgrade(upgrade).nextEffect + " ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        //if (upgrade.kind == AreaPrestigeUpgradeKind.ExpBonus) tempString += "\nCurrent EXP Bonus Total : <color=green>+ " + percent(game.statsCtrl.HeroStats(game.currentHero, Stats.ExpGain).Add(MultiplierKind.AreaPrestige)) + "</color>";
        //if (upgrade.kind == AreaPrestigeUpgradeKind.MoveSpeedBonus) tempString += "\nCurrent Move Speed Bonus Total : <color=green>+ " + percent(game.statsCtrl.HeroStats(game.currentHero, Stats.MoveSpeed).Add(MultiplierKind.AreaPrestige)) + "</color>";
        tempString += "\n\n";
        tempString += optStr + "<size=20><u>" + localized.Basic(BasicWord.Cost) + "</u><size=18>";
        tempString += optStr + "\n- " + tDigit(upgrade.transaction.Cost()) + " Points ( <color=green>Lv " + tDigit(upgrade.transaction.ToLevel()) + "</color> )";
        return tempString;
    }
}

public class HeroUI : BattleUI
{
    public override bool isAlive { get => battleCtrl().hero.isAlive; }
    ParticleSystem levelupParticle;
    GoUpTextUI levelupTextUI;
    Image auraImage;
    RectTransform rangeRect;

    public override void Initialize()
    {
        battleCtrl().hero.SetDamageTextUI(thisDamageText.ShowText);
        battleCtrl().hero.initUIAction = () => SetSprite(0);
        game.statsCtrl.HeroLevel(battleCtrl().hero.heroKind).levelUpUIAction = ShowLevelup;
        for (int i = 0; i < game.rebirthCtrl.rebirth[(int)battleCtrl().hero.heroKind].Length; i++)
        {
            game.rebirthCtrl.rebirth[(int)battleCtrl().hero.heroKind][i].rebirthUIAction = ShowRebirth;
        }
    }
    public HeroUI(Func<BattleController> battleCtrl, GameObject gameObject) : base(battleCtrl, gameObject)
    {
        levelupParticle = gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        levelupTextUI = new GoUpTextUI(gameObject.transform.GetChild(4).gameObject, Vector2.up * 50f, 60f);
        auraImage = gameObject.transform.GetChild(5).gameObject.GetComponent<Image>();
        rangeRect = gameObject.transform.GetChild(6).gameObject.GetComponent<RectTransform>();
        setFalse(levelupTextUI.thisObject);
        setFalse(auraImage.gameObject);
    }
    public void Start()
    {
        //skillPopupUI = gameUI.menuUI.MenuUI(MenuKind.Skill).GetComponent<SkillMenuUI>().skillPopupUI;
    }
    public override void SetSprite(int id)
    {
        thisImage.sprite = sprite.heroAvaters[id][(int)battleCtrl().hero.heroKind];
    }
    public override void UpdateMove()
    {
        thisRect.anchoredPosition = battleCtrl().hero.move.position;
        if (gameUI.battleStatusUI.combatRangePopupUI.isShow)
        {
            SetActive(rangeRect.gameObject, true);
            rangeRect.sizeDelta = Vector2.one * battleCtrl().hero.range * 2;
        }
        else if (!main.S.isToggleOn[(int)ToggleKind.DisableCombatRange] && skillPopupUI.isShow && skillPopupUI.thisSkill.type == SkillType.Attack)
        {
            SetActive(rangeRect.gameObject, true);
            rangeRect.sizeDelta = Vector2.one * (float)skillPopupUI.thisSkill.range * 2;
        }
        else SetActive(rangeRect.gameObject, false);
    }
    SkillPopupUI skillPopupUI => SkillMenuUI.skillPopupUI;
    Element auraElement;
    public override void UpdateAura()
    {
        if (game.inventoryCtrl.IsEquippedPotion(PotionKind.IcyAuraDraught, battleCtrl().heroKind) || battleCtrl().CurrentSlayerElement() == Element.Ice)
        {
            if (auraElement == Element.Ice) return;
            auraElement = Element.Ice;
            SetActive(auraImage.gameObject, true);
            auraImage.color = Color.cyan;
        }
        else if (game.inventoryCtrl.IsEquippedPotion(PotionKind.BlazingAuraDraught, battleCtrl().heroKind) || battleCtrl().CurrentSlayerElement() == Element.Fire)
        {
            if (auraElement == Element.Fire) return;
            auraElement = Element.Fire;
            SetActive(auraImage.gameObject, true);
            auraImage.color = Color.red;
        }
        else if (game.inventoryCtrl.IsEquippedPotion(PotionKind.WhirlingAuraDraught, battleCtrl().heroKind) || battleCtrl().CurrentSlayerElement() == Element.Thunder)
        {
            if (auraElement == Element.Thunder) return;
            auraElement = Element.Thunder;
            SetActive(auraImage.gameObject, true);
            auraImage.color = Color.yellow;
        }
        else
        {
            if (auraElement == Element.Physical) return;
            auraElement = Element.Physical;
            SetActive(auraImage.gameObject, false);
        }
    }
    void ShowLevelup(long levelIncrement)
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAnyLog).isOn) return;
        if (!SettingMenuUI.Toggle(ToggleKind.DisableParticle).isOn)
        {
            if (!levelupParticle.isPlaying)
            {
                levelupParticle.Play();
                gameUI.soundUI.Play(SoundEffect.LevelUp);
            }
        }
        if (levelIncrement > 1)
            levelupTextUI.ShowText(optStr + "<color=green>Level Up!!! ( + " + tDigit(levelIncrement) + " )");
        else
            levelupTextUI.ShowText(optStr + "<color=green>Level Up!!!");
    }
    void ShowRebirth()
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAnyLog).isOn) return;
        if (!SettingMenuUI.Toggle(ToggleKind.DisableParticle).isOn)
        {
            if (!levelupParticle.isPlaying)
            {
                levelupParticle.Play();
                gameUI.soundUI.Play(SoundEffect.LevelUp);
            }
        }
        levelupTextUI.ShowText("<color=green>You have Rebirthed!");
    }
}

public class HeroAllyUI : BattleUI
{
    public override bool isAlive { get => battleCtrl().hero.isAlive; }
    Func<HERO_BATTLE> heroBattle;
    public SliderUI hpSlider;

    public override void Initialize()
    {
        heroBattle().SetDamageTextUI(thisDamageText.ShowText);
        heroBattle().initUIAction = () => SetSprite(0);
    }
    public HeroAllyUI(Func<HERO_BATTLE> heroBattle, Func<BattleController> battleCtrl, GameObject gameObject) : base(battleCtrl, gameObject)
    {
        this.heroBattle = heroBattle;
        hpSlider = new SliderUI(gameObject.transform.GetChild(3).gameObject, () => heroBattle().HpPercent());
    }
    public override void SetSprite(int id)
    {
        thisImage.sprite = sprite.heroAvaters[id][(int)heroBattle().heroKind];
    }
    public override void UpdateMove()
    {
        thisRect.anchoredPosition = heroBattle().move.position;
    }
    public override void UpdateSlider()
    {
        hpSlider.UpdateUI();
    }
}


public class PetUI : BattleUI
{
    public override bool isAlive { get => petBattle.isAlive; }
    public int id;
    public SliderUI hpSlider;
    public SliderUI mpSlider;
    public PET_BATTLE petBattle { get => battleCtrl().pets[id]; }
    GoUpTextUI levelupTextUI;

    public override void Initialize()
    {
        petBattle.SetDamageTextUI(thisDamageText.ShowText);
        petBattle.SetSpawnUI(SetSprite);
    }
    public PetUI(Func<BattleController> battleCtrl, GameObject gameObject, int id) : base(battleCtrl, gameObject)
    {
        this.id = id;
        hpSlider = new SliderUI(gameObject.transform.GetChild(4).gameObject, () => petBattle.HpPercent());
        mpSlider = new SliderUI(gameObject.transform.GetChild(5).gameObject, () => petBattle.MpPercent());
        levelupTextUI = new GoUpTextUI(gameObject.transform.GetChild(3).gameObject, Vector2.up * 50f, 40f);
        setFalse(levelupTextUI.thisObject);
    }
    Color tempColor;
    void SetSprite()
    {
        SetSprite(0);
        //thisRect.localScale = petBattle.color == MonsterColor.Boss ? Vector3.one * 1.5f : Vector3.one;
        thisRect.localScale = Vector3.one * (0.75f + petBattle.globalInformation.pet.loyalty.value / 400f);
        if (game.statsCtrl.heroes[(int)petBattle.battleCtrl.heroKind].summonPetSPDMoveSpeedMultiplier.Value() > 1)//FeedChilli
            tempColor = Color.red;
        else
            tempColor = Color.white;
        thisImage.color = tempColor;
        //petBattle.globalInformation.pet.level.levelUpUIAction = ShowLevelup;
    }
    public override void SetSprite(int id)
    {
        thisImage.sprite = sprite.monsters[id][(int)petBattle.species][(int)petBattle.color];
        if (game.statsCtrl.heroes[(int)petBattle.battleCtrl.heroKind].summonPetSPDMoveSpeedMultiplier.Value() > 1)//FeedChilli
            thisImage.color = Color.red;
        else if (petBattle.debuffings[(int)Debuff.Stop].isDebuff)
            thisImage.color = Color.grey;
        else if (petBattle.debuffings[(int)Debuff.SpdDown].isDebuff)
            thisImage.color = Color.cyan;
        else if (petBattle.debuffings[(int)Debuff.Electric].isDebuff)
            thisImage.color = Color.yellow;
        else
            thisImage.color = Color.white;
    }
    //void ShowLevelup(long levelIncrement)
    //{
    //    gameUI.soundUI.Play(SoundEffect.LevelUp);
    //    if (levelIncrement > 1)
    //        levelupTextUI.ShowText(optStr + "<color=green>Level Up! ( + " + tDigit(levelIncrement) + " )");
    //    else
    //        levelupTextUI.ShowText(optStr + "<color=green>Level Up!");
    //}
    public override void UpdateMove()
    {
        thisRect.anchoredPosition = petBattle.move.position;
    }
    public override void UpdateSlider()
    {
        hpSlider.UpdateUI();
        mpSlider.UpdateUI();
    }
    public override void UpdateDebuff()
    {
        if (petBattle.move.isStun)
            thisImage.color = Color.grey;
    }
}


public class MonsterUI : BattleUI
{
    public override bool isAlive { get => monsterBattle.isAlive; }
    public int id;
    public bool isChallenge;
    int spriteColorId
    {
        get
        {
            if (isChallenge) return (int)monsterBattle.challengeMonsterKind;
            return (int)monsterBattle.color;
        }
    }
    public SliderUI hpSlider;
    TextMeshProUGUI hpText;//challengeのみ
    public SliderUI mpSlider;
    Image mpSliderImage;//challengeのみ

    public MONSTER_BATTLE monsterBattle
    {
        get
        {
            if (isChallenge) return battleCtrl().challengeMonsters[id];
            return battleCtrl().monsters[id];
        }
    }

    public override void Initialize()
    {
        monsterBattle.SetDamageTextUI(thisDamageText.ShowText);
        monsterBattle.SetSpawnUI(SetSprite);
    }
    public MonsterUI(Func<BattleController> battleCtrl, GameObject gameObject, int id, bool isChallenge = false) : base(battleCtrl, gameObject)
    {
        this.isChallenge = isChallenge;
        this.id = id;
        hpSlider = new SliderUI(gameObject.transform.GetChild(3).gameObject, () => monsterBattle.HpPercent());
        mpSlider = new SliderUI(gameObject.transform.GetChild(4).gameObject, () => monsterBattle.MpPercent());

        if (!isChallenge) return;
        hpText = hpSlider.thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        mpSliderImage = mpSlider.thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
    }
    public virtual void SetUI() { }
    Vector3 tempLocalScale;
    public virtual void SetSprite()
    {
        SetSprite(0);
        thisImage.color = Color.white;
        if (isChallenge) return;
        if (monsterBattle.color == MonsterColor.Boss) tempLocalScale = Vector3.one * 1.5f;
        else if (monsterBattle.color == MonsterColor.Metal) tempLocalScale = Vector3.one * 0.75f;
        else tempLocalScale = Vector3.one;
        if (monsterBattle.isMutant)
        {
            tempLocalScale *= 1.5f;
            //thisRect.rotation = Quaternion.Euler(Vector3.up * 180f);
        }
        //else
        //    thisRect.rotation = Quaternion.Euler(Vector3.zero);
        thisRect.localScale = tempLocalScale;
    }

    int count = 0;
    public override void SetSprite(int id)
    {
        if (monsterBattle.debuffings[(int)Debuff.Stop].isDebuff)
        {
            thisImage.color = Color.grey;
            return;
        }
        if (monsterBattle.debuffings[(int)Debuff.SpdDown].isDebuff)
        {
            thisImage.color = Color.cyan;
            if (count >= 1)//Spriteの変更時間を2倍にする
            {
                count = 0;
                return;
            }
            count++;
        }
        else if (monsterBattle.debuffings[(int)Debuff.Poison].isDebuff)
            thisImage.color = purple;
        else if (monsterBattle.debuffings[(int)Debuff.Electric].isDebuff)
            thisImage.color = Color.yellow;
        else
            thisImage.color = Color.white;
        thisImage.sprite = sprite.monsters[id][(int)monsterBattle.species][spriteColorId];
    }
    public override void UpdateMove()
    {
        thisRect.anchoredPosition = monsterBattle.move.position;
    }
    public override void UpdateSlider()
    {
        hpSlider.UpdateUI();
        mpSlider.UpdateUI();
        if (!isChallenge) return;
        hpText.text = tDigit(monsterBattle.currentHp.value, 1) + " / " + tDigit(monsterBattle.hp, 1) + " ( " + percent(monsterBattle.HpPercent(), 1) + " )";
        mpSliderImage.color = MpSliderColor(monsterBattle.currentAttackColor);
    }
    public override void UpdateDebuff()
    {
        if (monsterBattle.move.isStun)
            thisImage.color = Color.grey;
    }

    Color MpSliderColor(AttackColor color)
    {
        switch (color)
        {
            case AttackColor.Blue:
                return Color.blue;
            case AttackColor.Green:
                return green;
            case AttackColor.Yellow:
                return Color.yellow;
            case AttackColor.Red:
                return Color.red;
            case AttackColor.Purple:
                return purple;
            case AttackColor.Gray:
                return Color.gray;
            case AttackColor.Orange:
                return orange;
        }
        return Color.blue;
    }
}

public class BattleUI
{
    public Func<BattleController> battleCtrl;
    public GameObject thisObject;
    public RectTransform thisRect;
    public Image thisImage;
    public DamageTextUI thisDamageText;
    public virtual bool isAlive { get; }

    public virtual void Initialize() { }
    public BattleUI(Func<BattleController> battleCtrl, GameObject gameObject)
    {
        this.battleCtrl = battleCtrl;
        thisObject = gameObject;
        thisRect = gameObject.GetComponent<RectTransform>();
        thisImage = gameObject.GetComponent<Image>();
        thisDamageText = new DamageTextUI(thisObject);
        SetActive(thisObject, false);
    }

    public virtual void SetSprite(int id) { }
    public void UpdateUI()
    {
        if (isAlive) SetActive(thisObject, true);
        if (!thisObject.activeSelf) return;
        UpdateMove();
        if (!isAlive) { SetActive(thisObject, false); return; }
        UpdateSlider();
        UpdateDebuff();
        UpdateAura();
    }
    public virtual void UpdateMove() { }
    public virtual void UpdateSlider() { }
    public virtual void UpdateDebuff() { }
    public virtual void UpdateAura() { }
}

public class SliderUI//Sliderだと都合悪いので結局Image.fillにした
{
    //public Slider thisSlider;
    public GameObject thisObject;
    Image fillImage;
    Func<float> sliderValue = () => 0;
    public SliderUI(GameObject slider, Func<float> sliderValue)
    {
        thisObject = slider;
        fillImage = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        this.sliderValue = sliderValue;
    }
    public void UpdateUI()
    {
        fillImage.fillAmount = sliderValue();
    }
}

public class DamageTextUI
{
    GoUpTextUI[] damageTexts = new GoUpTextUI[3];
    Vector2[] resetPos = new Vector2[3]
    {
        new Vector2(-32,-16),
        Vector2.zero,
        new Vector2(32,16)
    };
    public DamageTextUI(GameObject gameObject)
    {
        for (int i = 0; i < damageTexts.Length; i++)
        {
            damageTexts[i] = new GoUpTextUI(gameObject.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().gameObject, resetPos[i]);
        }
        for (int i = 0; i < damageTexts.Length; i++)
        {
            setFalse(damageTexts[i].thisObject);
        }
        id = new ArrayId(0, () => damageTexts.Length);
    }
    ArrayId id;
    public void ShowText(string text)
    {
        if (main.S.isToggleOn[(int)ToggleKind.DisableDamageText]) return;
        damageTexts[id.value].ShowText(text);
        id.Increase();
    }
}
public class GoUpTextUI
{
    public GameObject thisObject;
    RectTransform thisRect;
    TextMeshProUGUI thisText;
    Vector2 resetPosition = Vector2.zero;
    float upValueY = 16f;

    public GoUpTextUI(GameObject gameObject, Vector2 resetPosition, float upValueY = 16f)
    {
        thisObject = gameObject;
        thisRect = gameObject.GetComponent<RectTransform>();
        thisText = gameObject.GetComponent<TextMeshProUGUI>();
        this.resetPosition = resetPosition;
        this.upValueY = upValueY;
        GoUp();
    }
    public void ShowText(string text)
    {
        setActive(thisObject);
        thisRect.anchoredPosition = resetPosition;
        thisText.text = text;
    }
    async void GoUp()
    {
        while (true)
        {
            if (thisObject.activeSelf)
            {
                thisRect.anchoredPosition += Vector2.up;
                if (thisRect.anchoredPosition.y >= resetPosition.y + upValueY)
                    SetActive(thisObject, false);
            }
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        }
    }
}

public class BattleResultUI
{
    Func<BATTLE_CONTROLLER> battleCtrl;
    Func<AREA_BATTLE> areaBattle;
    GameObject thisObject;
    CanvasGroup thisCanvasGroup;
    TextMeshProUGUI thisText;
    Image clockFillImage;
    int smoothness = 30;
    public BattleResultUI(Func<BATTLE_CONTROLLER> battleCtrl, GameObject thisObject)
    {
        this.battleCtrl = battleCtrl;
        this.thisObject = thisObject;
        thisCanvasGroup = thisObject.GetComponent<CanvasGroup>();
        thisText = thisObject.GetComponentInChildren<TextMeshProUGUI>();
        clockFillImage = thisObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        areaBattle = () => battleCtrl().areaBattle;
        thisCanvasGroup.alpha = 0;
    }
    public void Initialize()
    {
        areaBattle().clearedTask = () => ShowResult(true, areaBattle().CurrentArea().RealElapsedTime(battleCtrl().heroKind, battleCtrl().timecount, false), areaBattle().gold, areaBattle().exp, () => areaBattle().resources, () => areaBattle().materials, main.S.isToggleOn[(int)ToggleKind.FastResult]);
        areaBattle().failedTask = () => ShowResult(false, areaBattle().CurrentArea().RealElapsedTime(battleCtrl().heroKind, battleCtrl().timecount, false), areaBattle().gold, areaBattle().exp, () => areaBattle().resources, () => areaBattle().materials, main.S.isToggleOn[(int)ToggleKind.FastResult]);
    }

    public async Task ShowResult(bool isClear, float clearTime, double resultGold, double resultExp, Func<double[]> resultResources, Func<double[]> resultMaterials, bool isShort = false)
    {
        if (isShort) smoothness = (int)Math.Max(1, 10d / main.timescale);
        else smoothness = 30;

        clockFillImage.fillAmount = 0;

        string tempString = optStr + "<size=24>";
        tempString += isClear ? optStr + "<color=green>" + localized.Basic(BasicWord.Areacleared) : optStr + "<color=red>" + localized.Basic(BasicWord.Areafailed);
        tempString += "</color><size=20>";
        thisText.text = tempString;

        thisCanvasGroup.alpha = 0;
        for (int i = 0; i < smoothness; i++)
        {
            thisCanvasGroup.alpha += 1f / smoothness;
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
            if (!areaBattle().isShowingResultPanel) goto end;
        }
        thisCanvasGroup.alpha = 1f;
        await UniTask.DelayFrame(smoothness, PlayerLoopTiming.FixedUpdate);
        if (!areaBattle().isShowingResultPanel) goto end;
        thisText.text += optStr + "\n\n" + localized.Basic(BasicWord.CompltedTime) + " : " + tDigit(Math.Ceiling(clearTime * 10) / 10d, 1) + " " + localized.Basic(BasicWord.Sec);
        await UniTask.DelayFrame(smoothness, PlayerLoopTiming.FixedUpdate);
        if (!areaBattle().isShowingResultPanel) goto end;
        thisText.text += optStr + "\n\n" + localized.Basic(BasicWord.TotalGoldGained) + " : " + tDigit(resultGold);
        await UniTask.DelayFrame(smoothness, PlayerLoopTiming.FixedUpdate);
        if (!areaBattle().isShowingResultPanel) goto end;
        thisText.text += optStr + "\n\n" + localized.Basic(BasicWord.TotalExpGained) + " : " + tDigit(resultExp);
        await UniTask.DelayFrame(smoothness, PlayerLoopTiming.FixedUpdate);
        if (!areaBattle().isShowingResultPanel) goto end;
        thisText.text += optStr + "\n\n" + localized.Basic(BasicWord.TotalMaterialsGained) + " : <size=18>\n\n";
        for (int i = 0; i < resultResources().Length; i++)
        {
            if (resultResources()[i] > 0)
                thisText.text += optStr + localized.ResourceName((ResourceKind)i) + " * " + tDigit(resultResources()[i]) + "\n";
        }
        for (int i = 0; i < resultMaterials().Length; i++)
        {
            if (resultMaterials()[i] > 0)
                thisText.text += optStr + localized.Material((MaterialKind)i) + " * " + tDigit(resultMaterials()[i]) + "\n";
        }
        for (int i = 0; i < smoothness * 5; i++)
        {
            clockFillImage.fillAmount += 1f / (smoothness * 5);
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
            if (!areaBattle().isShowingResultPanel) goto end;
        }

        end:
            thisCanvasGroup.alpha = 0;
    }
}

public class MonsterStatsPopupUI : PopupUI
{
    public MonsterStatsPopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        description = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public override void SetTargetObject(GameObject targetObject, Action showAction = null)
    {
        if (targetObject.GetComponent<EventTrigger>() == null) targetObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        int tempId = targetObjectList.Count;
        entry.callback.AddListener((data) => { SetId(tempId); if (showAction != null) showAction(); else DelayShow(); if (Input.GetMouseButton(1)) monsterBattle().TryCapture(game.battleCtrl.hero.heroKind); });
        exit.callback.AddListener((data) => { SetId(-1); DelayHide(); });
        targetObject.GetComponent<EventTrigger>().triggers.Add(entry);
        targetObject.GetComponent<EventTrigger>().triggers.Add(exit);
        targetObjectList.Add(targetObject);
    }

    public Func<MONSTER_BATTLE> monsterBattle;
    Image icon;
    TextMeshProUGUI description;
    public void SetUI(Func<MONSTER_BATTLE> monsterBattle)
    {
        if (!monsterBattle().CanShowStats(game.currentHero)) return;
        this.monsterBattle = monsterBattle;
        if (monsterBattle().species == MonsterSpecies.ChallengeBoss)
            icon.sprite = sprite.monsters[0][(int)monsterBattle().species][(int)monsterBattle().challengeMonsterKind];
        else
            icon.sprite = sprite.monsters[0][(int)monsterBattle().species][(int)monsterBattle().color];
        UpdateText();
        DelayShow();
    }
    public override void UpdateUI()
    {       
        if (!isShow) return;
        //CheckMouseOver();
        SetCorner();
        UpdateText();
    }
    public override void Update()
    {
        if (!isShow) return;
        //Capture
        if (Input.GetMouseButtonDown(1)) monsterBattle().TryCapture(game.battleCtrl.hero.heroKind);
    }
    void UpdateText()
    {
        description.text = DescriptionString();
    }
    string DescriptionString()
    {
        string tempStr = optStrL + "<size=20>" + monsterBattle().globalInformation.Name() + " < <color=green>Lv " + tDigit(monsterBattle().level) + "</color> >";
        if (monsterBattle().isMutant) tempStr += " <color=orange>Mutant</color>";
        tempStr += optStr + "\n\n" + localized.BasicStats(BasicStatsKind.HP) + " : " + tDigit(monsterBattle().currentHp.value, 1) + " / " + tDigit(monsterBattle().hp, 1) + " ( " + percent(monsterBattle().HpPercent(), 1) + " )";
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.ATK) + " : " + tDigit(monsterBattle().atk, 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MATK) + " : " + tDigit(monsterBattle().matk, 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.DEF) + " : " + tDigit(monsterBattle().def, 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MDEF) + " : " + tDigit(monsterBattle().mdef, 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.SPD) + " : " + tDigit(monsterBattle().spd, 1);
        //Resistance
        tempStr += optStr + "\n" + localized.Stat(Stats.FireRes) + " : " + ResistanceColorString(monsterBattle().fire);
        tempStr += optStr + "\n" + localized.Stat(Stats.IceRes) + " : " + ResistanceColorString(monsterBattle().ice);
        tempStr += optStr + "\n" + localized.Stat(Stats.ThunderRes) + " : " + ResistanceColorString(monsterBattle().thunder);
        tempStr += optStr + "\n" + localized.Stat(Stats.LightRes) + " : " + ResistanceColorString(monsterBattle().light);
        tempStr += optStr + "\n" + localized.Stat(Stats.DarkRes) + " : " + ResistanceColorString(monsterBattle().dark);
        tempStr += optStr + "\n\n" + localized.SkillEffect((EffectKind)monsterBattle().attackElement) + " : " + tDigit(monsterBattle().Damage(), 1);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Cooltime) + " : " + tDigit(monsterBattle().attackIntervalSec, 1) + " " + localized.Basic(BasicWord.Sec);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Range) + " : " + meter(monsterBattle().range);
        tempStr += optStr + "\n" + localized.Stat(Stats.MoveSpeed) + " : " + meter(monsterBattle().moveSpeed) + " / " + localized.Basic(BasicWord.Sec);
        tempStr += optStr + "\n\n" + "Damage to you" + " : " + tDigit(Damage(), 1);
        tempStr += optStr + " ( " + "DPS" + " : " + tDigit(Damage() / monsterBattle().attackIntervalSec, 2) + " ) ";
        tempStr += optStr + "\n" + "EXP : " + tDigit(monsterBattle().exp);
        tempStr += optStr + "\n" + localized.Basic(BasicWord.Gold) + " : " + tDigit(monsterBattle().gold);
        return tempStr;
    }
    string ResistanceColorString(double value)
    {
        if (value > 0) return "<color=green>" + percent(value, 1) + "</color>";
        if (value < 0) return "<color=red>" + percent(value, 1) + "</color>";
        return percent(value, 1);
    }
    double Damage()
    { 
        return game.battleCtrl.hero.CalculateDamage(monsterBattle().Damage(), monsterBattle().attackElement, false, true);
    }
}

public class PetStatsPopupUI : PopupUI
{
    public PetStatsPopupUI(GameObject thisObject, Func<bool> additionalShowCondition = null) : base(thisObject, additionalShowCondition)
    {
        icon = thisObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        description = thisObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public override void SetTargetObject(GameObject targetObject, Action showAction = null)
    {
        if (targetObject.GetComponent<EventTrigger>() == null) targetObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        int tempId = targetObjectList.Count;
        entry.callback.AddListener((data) => { SetId(tempId); if (showAction != null) showAction(); else DelayShow(); });
        exit.callback.AddListener((data) => { SetId(-1); DelayHide(); });
        targetObject.GetComponent<EventTrigger>().triggers.Add(entry);
        targetObject.GetComponent<EventTrigger>().triggers.Add(exit);
        targetObjectList.Add(targetObject);
    }

    public Func<PET_BATTLE> petBattle;
    Image icon;
    TextMeshProUGUI description;
    public void SetUI(Func<PET_BATTLE> petBattle)
    {
        if (!petBattle().CanShowStats(game.currentHero)) return;
        this.petBattle = petBattle;
        if (petBattle().species == MonsterSpecies.ChallengeBoss)
            icon.sprite = sprite.monsters[0][(int)petBattle().species][(int)petBattle().challengeMonsterKind];
        else
            icon.sprite = sprite.monsters[0][(int)petBattle().species][(int)petBattle().color];
        UpdateText();
        DelayShow();
    }
    public override void UpdateUI()
    {
        if (!isShow) return;
        //CheckMouseOver();
        SetCorner();
        UpdateText();
    }
    public override void Update()
    {
        if (!isShow) return;
    }
    void UpdateText()
    {
        description.text = DescriptionString();
    }
    string DescriptionString()
    {
        string tempStr = optStrL + "<size=20>" + petBattle().globalInformation.Name() + " < <color=green>Lv " + tDigit(petBattle().level) + "</color> >";
        tempStr += optStr + "\n\n" + localized.BasicStats(BasicStatsKind.HP) + " : " + tDigit(petBattle().currentHp.value, 1) + " / " + tDigit(petBattle().hp, 1) + " ( " + percent(petBattle().HpPercent(), 1) + " )";
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.ATK) + " : " + tDigit(petBattle().atk, 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MATK) + " : " + tDigit(petBattle().matk, 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.DEF) + " : " + tDigit(petBattle().def, 1);
        tempStr += optStr + "   " + localized.BasicStats(BasicStatsKind.MDEF) + " : " + tDigit(petBattle().mdef, 1);
        tempStr += optStr + "\n" + localized.BasicStats(BasicStatsKind.SPD) + " : " + tDigit(petBattle().spd, 1);
        //Resistance
        tempStr += optStr + "\n" + localized.Stat(Stats.FireRes) + " : " + ResistanceColorString(petBattle().fire);
        tempStr += optStr + "\n" + localized.Stat(Stats.IceRes) + " : " + ResistanceColorString(petBattle().ice);
        tempStr += optStr + "\n" + localized.Stat(Stats.ThunderRes) + " : " + ResistanceColorString(petBattle().thunder);
        tempStr += optStr + "\n" + localized.Stat(Stats.LightRes) + " : " + ResistanceColorString(petBattle().light);
        tempStr += optStr + "\n" + localized.Stat(Stats.DarkRes) + " : " + ResistanceColorString(petBattle().dark);
        tempStr += optStr + "\n\n" + localized.SkillEffect((EffectKind)petBattle().attackElement) + " : " + tDigit(petBattle().Damage(), 1);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Cooltime) + " : " + tDigit(petBattle().attackIntervalSec, 1) + " " + localized.Basic(BasicWord.Sec);
        tempStr += optStr + "\n" + localized.SkillEffect(EffectKind.Range) + " : " + meter(petBattle().range);
        tempStr += optStr + "\n" + localized.Stat(Stats.MoveSpeed) + " : " + meter(petBattle().moveSpeed) + " / " + localized.Basic(BasicWord.Sec);
        return tempStr;
    }
    string ResistanceColorString(double value)
    {
        if (value > 0) return "<color=green>" + percent(value, 1) + "</color>";
        if (value < 0) return "<color=red>" + percent(value, 1) + "</color>";
        return percent(value, 1);
    }
}