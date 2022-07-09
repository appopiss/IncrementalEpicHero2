using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Steamworks;

public partial class Save
{
    public HeroKind currentHero;

    public double[] playtimes;//[heroKind]
    public double[] playtimesRealTime;//[heroKind]
}
public partial class SaveR
{
    //WAでリセット
    public double[] playtimes;//[heroKind]
    public double[] playtimesRealTime;//[heroKind]
}

public class GameController : MonoBehaviour
{    
    public HeroKind currentHero { get => main.S.currentHero; set => main.S.currentHero = value; }

    public static GameController game;
    public StatsController statsCtrl;
    public BattleController battleCtrl { get => battleCtrls[(int)currentHero]; }
    public BattleController[] battleCtrls = new BattleController[Enum.GetNames(typeof(HeroKind)).Length];
    public BlessingInfoController blessingInfoCtrl;
    public SkillController skillCtrl;
    public AreaController areaCtrl;
    public MissionController missionCtrl;
    public ResourceController resourceCtrl;
    public UpgradeController upgradeCtrl;
    public EquipmentController equipmentCtrl;
    public InventoryController inventoryCtrl;
    public MonsterController monsterCtrl;
    public GuildController guildCtrl;
    public QuestController questCtrl;
    public MaterialController materialCtrl;
    public EssenceController essenceCtrl;
    public AlchemyController alchemyCtrl;
    public PotionController potionCtrl;
    public CatalystController catalystCtrl;
    public CraftController craftCtrl;
    public TownController townCtrl;
    public ShopController shopCtrl;
    public RebirthController rebirthCtrl;
    public ChallengeController challengeCtrl;
    public ExpeditionController expeditionCtrl;
    public AscensionController ascensionCtrl;
    public EpicStoreController epicStoreCtrl;
    public NitroController nitroCtrl;
    public AchievementController achievementCtrl;
    public OfflineBonus offlineBonus;

    public InAppPurchaseController inAppPurchaseCtrl;
    public GetAchievementInfo getAchievementInfoIEH1;
    public GetAchievementInfo getAchievementInfoIEH2Playtest;
    public GetOwnerShip getOwnerShipIEH1DLC_IEH2SupportPack;
    public DLCController dlcController;
    public SteamAchievement steamAchievement;

    public SimulationController simulationCtrl;
    public AutomationController autoCtrl;

    public bool IsUI(HeroKind heroKind)
    {
        if (BackgroundModeUI.isBackgroundMode) return false;
        return heroKind == currentHero;
    }
    async void CreateInstance()
    {
        game = this;
        statsCtrl = new StatsController();
        townCtrl = new TownController();
        resourceCtrl = new ResourceController();
        materialCtrl = new MaterialController();
        essenceCtrl = new EssenceController();
        alchemyCtrl = new AlchemyController();
        areaCtrl = new AreaController();
        missionCtrl = new MissionController();
        potionCtrl = new PotionController();
        catalystCtrl = new CatalystController();
        craftCtrl = new CraftController();
        shopCtrl = new ShopController();
        upgradeCtrl = new UpgradeController();
        monsterCtrl = new MonsterController();
        skillCtrl = new SkillController();
        equipmentCtrl = new EquipmentController();
        inventoryCtrl = new InventoryController();
        guildCtrl = new GuildController();
        questCtrl = new QuestController();
        rebirthCtrl = new RebirthController();
        challengeCtrl = new ChallengeController();
        expeditionCtrl = new ExpeditionController();
        ascensionCtrl = new AscensionController();
        epicStoreCtrl = new EpicStoreController();
        blessingInfoCtrl = new BlessingInfoController();
        nitroCtrl = new NitroController();
        for (int i = 0; i < battleCtrls.Length; i++)
        {
            battleCtrls[i] = new BattleController((HeroKind)i);
        }
        //いずれPetだけで戦える⇨ではなく、あくまでClassを選択。どこのAreaを周回するかを選ぶ。Rein後はClassが分身して６たい以上自動化。

        simulationCtrl = new SimulationController();
        autoCtrl = new AutomationController();

        achievementCtrl = new AchievementController();

        inAppPurchaseCtrl = new InAppPurchaseController();
        getAchievementInfoIEH1 = new GetAchievementInfo("1530340");
        getAchievementInfoIEH2Playtest = new GetAchievementInfo("1956700");
        getOwnerShipIEH1DLC_IEH2SupportPack = new GetOwnerShip("F6724F6EF14E3AE95B7B7F14E53BEEC3", "1580930");
        dlcController = new DLCController();

        offlineBonus = new OfflineBonus();
        //if (main.platform == PlatformKind.Steam)
        //await UniTask.WaitUntil(() => GameControllerUI.gameUI.titleSceneUI.isLoaded);
        offlineBonus.SetOfflineTimesec();
        //steamAchievement = new SteamAchievement();

        await UniTask.WaitUntil(() => SteamManager.Initialized);
        steamAchievement = new SteamAchievement();
    }

    void Awake()
    {
        isPause = true;
        CreateInstance();
    }
    void Start()
    {
        questCtrl.Start();
        rebirthCtrl.Start();
        alchemyCtrl.Start();
        potionCtrl.Start();
        inventoryCtrl.Start();
        upgradeCtrl.Start();
        townCtrl.Start();
        challengeCtrl.Start();
        missionCtrl.Start();
        shopCtrl.Start();
        equipmentCtrl.Start();
        guildCtrl.Start();
        monsterCtrl.Start();
        expeditionCtrl.Start();
        ascensionCtrl.Start();
        epicStoreCtrl.Start();
        for (int i = 0; i < battleCtrls.Length; i++)
        {
            battleCtrls[i].Start();
        }
        simulationCtrl.Start();
        achievementCtrl.Start();
        dlcController.Start();

    }
    public void Initialize()
    {
        areaCtrl.Initialize();
        simulationCtrl.Initialize();
        for (int i = 0; i < battleCtrls.Length; i++)
        {
            battleCtrls[i].skillSet.Initialize();
        }
        guildCtrl.Initialize();
    }
    float unscaledDeltaTime;
    float deltaTime;
    void Update()
    {
        if (main.isHacked) return;
        if (isPause) return;

        unscaledDeltaTime = Time.unscaledDeltaTime;
        deltaTime = Time.deltaTime;
        //統計:Playtime
        main.S.playtimesRealTime[(int)currentHero] += unscaledDeltaTime;
        main.SR.playtimesRealTime[(int)currentHero] += unscaledDeltaTime;

        for (int i = 0; i < battleCtrls.Length; i++)
        {
            battleCtrls[i].Update();
            if (battleCtrls[i].isActiveBattle)
            {
                main.S.playtimes[i] += deltaTime;
                main.SR.playtimes[(int)currentHero] += deltaTime;
            }
        }
        alchemyCtrl.Update();
        nitroCtrl.Update();
        shopCtrl.Update();
        townCtrl.Update();
        resourceCtrl.Update();
        guildCtrl.Update();
        expeditionCtrl.Update();

        timeCountSec += deltaTime;
        timeCountTwoSec += deltaTime;
        if (timeCountSec >= 1.0)//１.0秒ごとのUpdate
        {
            inventoryCtrl.UpdatePerSec(timeCountSec);
            //Auto
            AutoPerSec();
            timeCountSec = 0;
        }
        if (timeCountTwoSec >= 2.0)//2.0秒ごと
        {
            AutoPerTwoSec();
            timeCountTwoSec = 0;
        }
    }
    float timeCountSec, timeCountTwoSec;
    async void AutoPerSec()
    {
        game.upgradeCtrl.BuyByQueue();
        await UniTask.DelayFrame(1);
        game.potionCtrl.BuyByQueue();
        await UniTask.DelayFrame(1);
        game.shopCtrl.AutoBuy();
        await UniTask.DelayFrame(1);
        game.statsCtrl.AutoAddAbilityPoint();
        await UniTask.DelayFrame(1);
        game.inventoryCtrl.AutoRestorePotion();
    }
    async void AutoPerTwoSec()
    {
        game.alchemyCtrl.AutoExpandCap();
        await UniTask.DelayFrame(2);
        game.skillCtrl.AutoRankup();
        await UniTask.DelayFrame(2);
        game.rebirthCtrl.AutoRebirth();
        await UniTask.DelayFrame(2);
        game.rebirthCtrl.AutoRebirthUpgradeExp();
        questCtrl.AcceptFavorite();
        await UniTask.DelayFrame(2);
        questCtrl.ClaimFavorite();
    }
    public bool isPause = true;

    public async void LoadScene()
    {
        main.saveCtrl.setSaveKey();
        await UniTask.DelayFrame(5);
        SceneManager.LoadScene("Main");
    }
}
