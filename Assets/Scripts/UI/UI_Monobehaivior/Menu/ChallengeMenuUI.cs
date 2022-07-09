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

public class ChallengeMenuUI : MENU_UI
{
    public HeroKind heroKind { get => game.currentHero; }
    [SerializeField] Button[] selectKindButtons;
    [SerializeField] Button[] challengeButtons;
    TextMeshProUGUI[] challengeButtonTexts;
    GameObject[] challengeButtonLockObjects;
    GameObject[] challengeButtonIconCs;
    TextMeshProUGUI[] challengeButtonLockTexts;
    [SerializeField] TextMeshProUGUI nameText, infoText;
    [SerializeField] Button startButton, quitButton, claimButton;
    [SerializeField] Image challengeBossImage;
    SwitchTabUI kindSwitchTabUI, challengeSwitchTabUI;
    CHALLENGE challenge;
    CHALLENGE_BATTLE[] challengeMonsters = new CHALLENGE_BATTLE[Enum.GetNames(typeof(ChallengeMonsterKind)).Length];
    CHALLENGE_BATTLE currentChallengeBattle;
    [SerializeField] GameObject iconC, iconI;
    public void Start()
    {
        challengeButtonTexts = new TextMeshProUGUI[challengeButtons.Length];
        challengeButtonLockObjects = new GameObject[challengeButtons.Length];
        challengeButtonIconCs = new GameObject[challengeButtons.Length];
        challengeButtonLockTexts = new TextMeshProUGUI[challengeButtons.Length];

        for (int i = 0; i < challengeButtonTexts.Length; i++)
        {
            challengeButtonTexts[i] = challengeButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            challengeButtonIconCs[i] = challengeButtons[i].gameObject.transform.GetChild(1).gameObject;
            challengeButtonLockObjects[i] = challengeButtons[i].gameObject.transform.GetChild(2).gameObject;
            challengeButtonLockTexts[i] = challengeButtonLockObjects[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }
        kindSwitchTabUI = new SwitchTabUI(selectKindButtons, true);
        kindSwitchTabUI.openActionWithId = SwitchKind;
        challengeSwitchTabUI = new SwitchTabUI(challengeButtons, true);
        challengeSwitchTabUI.openActionWithId = (x) => SetInfo(game.challengeCtrl.challengeArray[kindSwitchTabUI.currentId][x]);

        currentChallengeBattle = new CHALLENGE_BATTLE(game.battleCtrl);
        InstantiateChallengeMonster();
        gameUI.battleCtrlUI.monsterStatsPopupUI.SetTargetObject(challengeBossImage.gameObject, () => gameUI.battleCtrlUI.monsterStatsPopupUI.SetUI(() => currentChallengeBattle));

        selectKindButtons[0].onClick.Invoke();
        challengeButtons[0].onClick.Invoke();
        startButton.onClick.AddListener(() => challenge.StartBattle());
        quitButton.onClick.AddListener(() => challenge.Quit());
        claimButton.onClick.AddListener(() => challenge.ClaimAction());
    }
    //ここに追加//これはInfoに表示する用
    public void InstantiateChallengeMonster()
    {
        challengeMonsters[(int)ChallengeMonsterKind.SlimeKing] = new Battle_SlimeKing(game.battleCtrl);
        challengeMonsters[(int)ChallengeMonsterKind.WindowQueen] = new Battle_WindowQueen(game.battleCtrl);
        challengeMonsters[(int)ChallengeMonsterKind.Golem] = new Battle_Golem(game.battleCtrl);
    }
    void SetInfo(CHALLENGE challenge)
    {
        this.challenge = challenge;

        //BossBattleの場合
        SetActive(challengeBossImage.gameObject, challenge.type == ChallengeType.RaidBossBattle || challenge.type == ChallengeType.SingleBossBattle);
        if (!challengeBossImage.gameObject.activeSelf) return;
        challengeBossImage.sprite = sprite.challenge1[(int)challenge.challengeMonsterKind];
        currentChallengeBattle = challengeMonsters[(int)challenge.challengeMonsterKind]; //game.battleCtrl.challengeMonsters[(int)challenge.challengeMonsterKind];
        currentChallengeBattle.Spawn(challenge.area.MaxLevel(), 0, Parameter.hidePosition);
    }
    void SwitchKind(int id)
    {
        for (int i = 0; i < challengeButtons.Length; i++)
        {
            int count = i;
            if (count < game.challengeCtrl.challengeArray[kindSwitchTabUI.currentId].Length)
            {
                SetActive(challengeButtons[i].gameObject, true);
                challengeButtonTexts[i].text = game.challengeCtrl.challengeArray[kindSwitchTabUI.currentId][count].NameString();
                if (id == 0) challengeButtons[i].gameObject.GetComponent<Image>().sprite = sprite.globalquestButton;
                else challengeButtons[i].gameObject.GetComponent<Image>().sprite = sprite.generalquestButton;
            }
            else SetActive(challengeButtons[i].gameObject, false);
        }
        challengeButtons[0].onClick.Invoke();
    }
    public override void UpdateUI()
    {
        base.UpdateUI();
        nameText.text = challenge.TitleUIString();
        infoText.text = challenge.InfoUIString();

        if (challenge.isTryingThisChallenge)
        {
            SetActive(startButton.gameObject, false);
            SetActive(quitButton.gameObject, true);
            quitButton.interactable = challenge.isTryingThisChallenge && !game.battleCtrl.areaBattle.isShowingResultPanel;
        }
        else
        {
            SetActive(startButton.gameObject, true);
            SetActive(quitButton.gameObject, false);
            startButton.interactable = challenge.CanStart();
        }
        claimButton.interactable = challenge.CanClaim();

        //ClearIcon
        bool isClear = challenge.type == ChallengeType.RaidBossBattle ? challenge.IsClearedOnce() : challenge.IsClearedCompleted();
        SetActive(iconC, isClear);
        SetActive(iconI, challenge.CanClaim());
        for (int i = 0; i < challengeButtons.Length; i++)
        {
            int count = i;
            if (challengeButtons[count].gameObject.activeSelf)
            {
                CHALLENGE challenge = game.challengeCtrl.challengeArray[kindSwitchTabUI.currentId][count];
                bool isUnlocked = challenge.unlock.IsUnlocked();
                SetActive(challengeButtonLockObjects[count], !isUnlocked);
                if (!isUnlocked) challengeButtonLockTexts[count].text = challenge.unlock.LockString();
                isClear = challenge.type == ChallengeType.RaidBossBattle ? challenge.IsClearedOnce() : challenge.IsClearedCompleted();
                SetActive(challengeButtonIconCs[count], isClear);
            }
        }
    }
}
