using System.Collections;
using System.Collections.Generic;
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
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class DropControllerUI : MonoBehaviour
{
    [SerializeField] GameObject[] dropResources, dropMaterials, dropEquipments, dropChests;
    DropResourceUI[] dropResourcesUI;
    DropMaterialUI[] dropMaterialsUI;
    DropEquipmentUI[] dropEquipmentsUI;
    DropChestUI[] dropChestsUI;
    Func<BattleController> battleCtrl;
    private void Awake()
    {
        battleCtrl = () => game.battleCtrl;
        dropResourcesUI = new DropResourceUI[dropResources.Length];
        dropMaterialsUI = new DropMaterialUI[dropMaterials.Length];
        dropEquipmentsUI = new DropEquipmentUI[dropEquipments.Length];
        dropChestsUI = new DropChestUI[dropChests.Length];
        for (int i = 0; i < dropResources.Length; i++)
        {
            dropResourcesUI[i] = new DropResourceUI(battleCtrl, dropResources[i], i);
        }
        for (int i = 0; i < dropMaterials.Length; i++)
        {
            dropMaterialsUI[i] = new DropMaterialUI(battleCtrl, dropMaterials[i], i);
        }
        for (int i = 0; i < dropEquipments.Length; i++)
        {
            dropEquipmentsUI[i] = new DropEquipmentUI(battleCtrl, dropEquipments[i], i);
        }
        for (int i = 0; i < dropChestsUI.Length; i++)
        {
            dropChestsUI[i] = new DropChestUI(battleCtrl, dropChests[i], i);
        }
    }
    public void Initialize()
    {
        for (int i = 0; i < dropResources.Length; i++)
        {
            dropResourcesUI[i].Initialize();
        }
        for (int i = 0; i < dropMaterials.Length; i++)
        {
            dropMaterialsUI[i].Initialize();
        }
        for (int i = 0; i < dropEquipments.Length; i++)
        {
            dropEquipmentsUI[i].Initialize();
        }
        for (int i = 0; i < dropChestsUI.Length; i++)
        {
            dropChestsUI[i].Initialize();
        }
    }
}

public class DropChestUI
{
    public void Initialize()
    {
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
    }
    public DropChestUI(Func<BattleController> battleCtrl, GameObject thisObject, int id)
    {
        this.battleCtrl = battleCtrl;
        this.thisObject = thisObject;
        thisRect = thisObject.GetComponent<RectTransform>();
        thisImage = thisObject.GetComponent<Image>();
        thisButton = thisObject.GetComponent<Button>();
        thisText = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        thisTextRect = thisText.gameObject.GetComponent<RectTransform>();
        initPosition = thisTextRect.anchoredPosition;
        this.id = id;
        thisGenerator = () => battleCtrl().chestGenerators[id];
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
        thisButton.onClick.AddListener(Get);
        SetActive(thisText.gameObject, false);
        SetActive(thisObject, false);
    }
    Vector2 initPosition;
    void SetUI()
    {
        SetActive(thisObject, true);
        //thisImage.sprite = sprite. resourcesTrans[(int)thisGenerator().kind];
        thisRect.anchoredPosition = thisGenerator().position;
        isGot = false;
    }

    bool isGot = false;
    async void Get()
    {
        if (isGot) return;
        isGot = true;
        SetActive(thisText.gameObject, true);
        thisTextRect.anchoredPosition = initPosition;
        thisText.text = thisGenerator().currentChest.OpenString();
        gameUI.logCtrlUI.Log(thisGenerator().currentChest.OpenString());
        for (int i = 0; i < 25; i++)
        {
            thisTextRect.anchoredPosition += Vector2.up;
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        }
        thisGenerator().Get();
        InitializeUI();
    }
    void InitializeUI()
    {
        thisRect.anchoredPosition = thisGenerator().position;
        SetActive(thisText.gameObject, false);
        SetActive(thisObject, false);
        isGot = false;

    }

    Func<BattleController> battleCtrl;
    int id;
    Func<TreasureChestGenerator> thisGenerator;
    GameObject thisObject;
    RectTransform thisRect;
    Image thisImage;
    TextMeshProUGUI thisText;
    RectTransform thisTextRect;
    Button thisButton;
}

public class DropResourceUI
{
    public void Initialize()
    {
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
    }
    public DropResourceUI(Func<BattleController> battleCtrl, GameObject thisObject, int id)
    {
        this.battleCtrl = battleCtrl;
        this.thisObject = thisObject;
        thisRect = thisObject.GetComponent<RectTransform>();
        thisImage = thisObject.GetComponent<Image>();
        this.id = id;
        thisGenerator = () => battleCtrl().resourceGenerators[id];
        SetHoverAction();
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
        setFalse(thisObject);
    }
    void SetUI()
    {
        setActive(thisObject);
        thisImage.sprite = sprite.resourcesTrans[(int)thisGenerator().kind];
        thisRect.anchoredPosition = thisGenerator().position;
        isGot = false;
    }

    void SetHoverAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { Get(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(entry);
    }
    bool isGot = false;
    async void Get()
    {
        if (isGot) return;
        isGot = true;
        for (int i = 0; i < 5; i++)
        {
            thisRect.anchoredPosition += Vector2.up * 5;
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        }
        thisGenerator().Get();
        gameUI.soundUI.Play(SoundEffect.DropResource);
        InitializeUI();
    }
    void InitializeUI()
    {
        thisRect.anchoredPosition = thisGenerator().position;
        thisRect.localScale = Vector2.one;
        setFalse(thisObject);
        isGot = false;
    }

    Func<BattleController> battleCtrl;
    int id;
    Func<ResourceGenerator> thisGenerator;
    GameObject thisObject;
    RectTransform thisRect;
    Image thisImage;
}

public class DropMaterialUI
{
    public void Initialize()
    {
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
    }
    public DropMaterialUI(Func<BattleController> battleCtrl, GameObject thisObject, int id)
    {
        this.battleCtrl = battleCtrl;
        this.thisObject = thisObject;
        thisRect = thisObject.GetComponent<RectTransform>();
        thisImage = thisObject.GetComponent<Image>();
        this.id = id;
        thisGenerator = () => battleCtrl().materialGenerators[id];
        SetHoverAction();
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
        setFalse(thisObject);
    }
    void SetUI()
    {
        setActive(thisObject);
        thisImage.sprite = sprite.materials[(int)thisGenerator().kind];
        thisRect.anchoredPosition = thisGenerator().position;
        isGot = false;
    }

    void SetHoverAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { Get(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(entry);
    }
    bool isGot = false;
    async void Get()
    {
        if (isGot) return;
        isGot = true;
        for (int i = 0; i < 5; i++)
        {
            thisRect.anchoredPosition += Vector2.up * 5;
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        }
        thisGenerator().Get();
        gameUI.soundUI.Play(SoundEffect.DropResource);
        InitializeUI();
    }
    void InitializeUI()
    {
        thisRect.anchoredPosition = thisGenerator().position;
        thisRect.localScale = Vector2.one;
        setFalse(thisObject);
        isGot = false;
    }

    Func<BattleController> battleCtrl;
    int id;
    Func<MaterialGenerator> thisGenerator;
    GameObject thisObject;
    RectTransform thisRect;
    Image thisImage;
}

public class DropEquipmentUI
{
    public void Initialize()
    {
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
    }
    public DropEquipmentUI(Func<BattleController> battleCtrl, GameObject thisObject, int id)
    {
        this.battleCtrl = battleCtrl;
        this.thisObject = thisObject;
        thisButton = thisObject.GetComponent<Button>();
        thisRect = thisObject.GetComponent<RectTransform>();
        thisImage = thisObject.GetComponent<Image>();
        thisText = thisObject.GetComponentInChildren<TextMeshProUGUI>();
        animator = thisObject.GetComponent<Animator>();
        this.id = id;
        thisGenerator = () => battleCtrl().equipmentGenerators[id];
        SetHoverAction();
        thisGenerator().dropUIAction = SetUI;
        thisGenerator().initializeUIAction = InitializeUI;
        //thisButton.onClick.AddListener(Get);
        setFalse(thisObject);
    }
    void SetUI()
    {
        setActive(thisObject);
        thisRect.anchoredPosition = thisGenerator().position;
        thisImage.sprite = sprite.equipmentDrops[(int)EquipmentParameter.Rarity(thisGenerator().kind)];
        setActive(thisText.gameObject);
        animator.SetInteger("OptionNum", thisGenerator().optionNum);
        thisText.text = localized.EquipmentName(thisGenerator().kind);
        thisText.color = OptionColor();
        setFalse(thisText.gameObject);
        isGot = false;
    }
    Color OptionColor()
    {
        switch (thisGenerator().optionNum)
        {
            case 0:
                return Color.white;
            case 1:
                return Color.green;
            case 2:
                return Color.green + Color.blue;
            case 3:
                return Color.magenta;
        }
        return Color.white;
    }
    bool isGot = false;
    void Get()
    {
        if (isGot) return;
        isGot = true;
        if (!game.inventoryCtrl.CanCreateEquipment())
        {
            gameUI.logCtrlUI.Log(optStr + "<color=orange>" + localized.Basic(BasicWord.FullInventory), 0, true);
            isGot = false;
            return;
        }
        thisGenerator().TryGet();
        gameUI.soundUI.Play(SoundEffect.DropEquipment);
        InitializeUI();
    }
    void InitializeUI()
    {
        thisRect.anchoredPosition = thisGenerator().position;
        setFalse(thisObject);
        isGot = false;
    }
    void SetHoverAction()
    {
        if (thisObject.GetComponent<EventTrigger>() == null) thisObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        var exit = new EventTrigger.Entry();
        var down = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        exit.eventID = EventTriggerType.PointerExit;
        down.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { setActive(thisText.gameObject); if (Input.GetMouseButton(0)) Get(); });
        exit.callback.AddListener((data) => { setFalse(thisText.gameObject); });
        down.callback.AddListener((data) => { Get(); });
        thisObject.GetComponent<EventTrigger>().triggers.Add(entry);
        thisObject.GetComponent<EventTrigger>().triggers.Add(exit);
        thisObject.GetComponent<EventTrigger>().triggers.Add(down);
    }
    Func<BattleController> battleCtrl;
    int id;
    Func<EquipmentGenerator> thisGenerator;
    GameObject thisObject;
    Button thisButton;
    RectTransform thisRect;
    Image thisImage;
    TextMeshProUGUI thisText;
    Animator animator;
}