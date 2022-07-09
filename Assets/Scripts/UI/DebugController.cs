using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UsefulMethod;
using static GameController;
using TMPro;
using System;

public class DebugController : MonoBehaviour
{
    //public GameObject titleObject;
    public GameObject enchantBox;
    public GameObject stoneBox;
    public GameObject abilityPointBox;
    public GameObject petBox;
    public Slider timescaleSlider;
    public TextMeshProUGUI timescaleText;

    public Button debugShowButton;
    //public Button nitromaxButton;
    //public Button guildAbilityResetButton;
    //public Button hardresetButton;
    //public Button dictionaryResetButton;


    void MaxSkill()
    {
        for (int j = 0; j < game.skillCtrl.classSkills.Length; j++)
        {
            for (int i = 0; i < game.skillCtrl.classSkills[j].skills.Length; i++)
            {
                game.skillCtrl.classSkills[j].skills[i].rank.ToMax();
                game.skillCtrl.classSkills[j].skills[i].level.ToMax();
            }
        }
    }
    private void Start()
    {
        //SetActive(titleObject, true);
        //titleObject.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
        debugShowButton.onClick.AddListener(Show);
        //debugShowButton.onClick.AddListener(() =>
        //{
        //    for (int i = 0; i < game.equipmentCtrl.equipments.Length; i++)
        //    {
        //        game.equipmentCtrl.equipments[i].globalInfo.isGotOnce = true;
        //    }
        //});
        //debugShowButton.onClick.AddListener(() => game.areaCtrl.portalOrb.Increase(1));
        //debugShowButton.onClick.AddListener(() => game.epicStoreCtrl.epicCoin.Increase(10000));
        //debugShowButton.onClick.AddListener(() =>
        //{
        //    for (int i = 0; i < game.equipmentCtrl.globalInformations.Length; i++)
        //    {
        //        game.equipmentCtrl.globalInformations[i].isGotOnce = true;
        //    }
        //});
        //debugShowButton.onClick.AddListener(() => Main.main.S.totalMovedDistance[0] += 1000000000);
        //debugShowButton.onClick.AddListener(()=>
        //{
        //    for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        //    {
        //        HeroKind heroKind = (HeroKind)i;
        //        for (int j = 0; j < game.questCtrl.QuestArray(QuestKind.General, heroKind).Length; j++)
        //        {
        //            game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isAccepted = false;
        //            game.questCtrl.QuestArray(QuestKind.General, heroKind)[j].isCleared = false;
        //        }
        //        for (int j = 0; j < game.questCtrl.QuestArray(QuestKind.Title, heroKind).Length; j++)
        //        {
        //            game.questCtrl.QuestArray(QuestKind.Title, heroKind)[j].isAccepted = false;
        //            game.questCtrl.QuestArray(QuestKind.Title, heroKind)[j].isCleared = false;
        //        }
        //    }
        //});
        enchantBox.GetComponent<Button>().onClick.AddListener(() =>
        {
            for (int i = 0; i < game.potionCtrl.talismans.Count; i++)
            {
                int count = i;
                game.inventoryCtrl.CreatePotion(game.potionCtrl.talismans[count].kind, 1);

            }
        });
        //全てのスキルをRankMax,LevelMax
        //debugShowButton.onClick.AddListener(MaxSkill);

        //memory解放？
        debugShowButton.onClick.AddListener(() => GameControllerUI.gameUI.UnloadUnusedAsset());
        debugShowButton.onClick.AddListener(() => System.GC.Collect());

        petBox.GetComponent<Button>().onClick.AddListener(() =>
        {
            for (int i = 0; i < game.monsterCtrl.globalInfoList.Count; i++)
            {
                game.monsterCtrl.globalInfoList[i].defeatedNums[0].Increase(1);
                game.monsterCtrl.globalInfoList[i].pet.rank.Increase(1);
            }
        }
        );

        //TopのBox
        stoneBox.GetComponent<Button>().onClick.AddListener(() => game.resourceCtrl.Resource(ResourceKind.Stone).ToMax());
        stoneBox.GetComponent<Button>().onClick.AddListener(() => game.resourceCtrl.Resource(ResourceKind.Crystal).ToMax());
        stoneBox.GetComponent<Button>().onClick.AddListener(() => game.resourceCtrl.Resource(ResourceKind.Leaf).ToMax());
        stoneBox.GetComponent<Button>().onClick.AddListener(() => game.nitroCtrl.nitro.ToMax());
        stoneBox.GetComponent<Button>().onClick.AddListener(()=>
        {
            for (int i = 0; i < Enum.GetNames(typeof(MaterialKind)).Length; i++)
            {
                game.materialCtrl.Material((MaterialKind)i).ToMax();
            }
        });
        stoneBox.GetComponent<Button>().onClick.AddListener(() =>
        {
            for (int i = 0; i < Enum.GetNames(typeof(EssenceKind)).Length; i++)
            {
                game.essenceCtrl.Essence((EssenceKind)i).ToMax();
            }
        });
        stoneBox.GetComponent<Button>().onClick.AddListener(() =>
        {
            for (int i = 0; i < Enum.GetNames(typeof(TownMaterialKind)).Length; i++)
            {
                game.townCtrl.TownMaterial((TownMaterialKind)i).ToMax();
            }
        }); stoneBox.GetComponent<Button>().onClick.AddListener(() => game.alchemyCtrl.alchemyPoint.ToMax());
        //stoneBox.GetComponent<Button>().onClick.AddListener(() => game.monsterCtrl.monsterMilk.ToMax());
        stoneBox.GetComponent<Button>().onClick.AddListener(() => game.guildCtrl.level.Increase(70));

        stoneBox.GetComponent<Button>().onClick.AddListener(() =>
        {
            MaxSkill();
            //Main.main.SR.combatRangeId[(int)HeroKind.Tamer] = 5;
            //for (int i = 0; i < game.skillCtrl.SkillArray(HeroKind.Tamer).Length; i++)
            //{
            //    game.skillCtrl.SkillArray(HeroKind.Tamer)[i].rank.ChangeValue(1);
            //    game.skillCtrl.SkillArray(HeroKind.Tamer)[i].level.ChangeValue(1);
            //}
        });

        //hardresetButton.onClick.AddListener(HardReset);
        timescaleSlider.value = game.nitroCtrl.nitroTimescale;
        timescaleText.text = game.nitroCtrl.nitroTimescale.ToString("F1");
        timescaleSlider.onValueChanged.AddListener(ChangeTimescale);
        setFalse(enchantBox);
        setFalse(stoneBox);
        setFalse(abilityPointBox);
        setFalse(timescaleSlider.gameObject);
        setFalse(petBox);
        //setFalse(hardresetButton.gameObject);

        //For Beta Release
        setFalse(debugShowButton.gameObject);

        //nitromaxButton.onClick.AddListener(() => game.nitroCtrl.nitro.ToMax());
        //guildAbilityResetButton.onClick.AddListener(() => game.guildCtrl.ResetGuildAbility());
        //dictionaryResetButton.onClick.AddListener(game.equipmentCtrl.ResetDictionaryUpgrade);
    }

    void ChangeTimescale(float value)
    {
        game.nitroCtrl.nitroTimescale = value;
        timescaleText.text = "x" + value.ToString("F1");
    }
    //void StartGame()
    //{
    //    //SetActive(titleObject, false);
    //    game.battleCtrls[0].areaBattle.Start();
    //    game.battleCtrls[1].areaBattle.Start();
    //    game.battleCtrls[2].areaBattle.Start();
    //    game.battleCtrls[3].areaBattle.Start();
    //    game.battleCtrls[4].areaBattle.Start();
    //    game.battleCtrls[5].areaBattle.Start();
    //}

    bool isShow;
    void Show()
    {
        if (isShow)
        {
            setFalse(enchantBox);
            setFalse(stoneBox);
            setFalse(abilityPointBox);
            setFalse(timescaleSlider.gameObject);        setFalse(petBox);
            setFalse(petBox);
            //setFalse(hardresetButton.gameObject);
            isShow = false;
            return;
        }

        //Swarm
        if(GameControllerUI.isShiftPressed) game.areaCtrl.StartSwarm();
        //Tamer
        if (Input.GetKey(KeyCode.T))
        {
            game.guildCtrl.level.Increase(70 - game.guildCtrl.level.value);
            Main.main.SR.combatRangeId[(int)HeroKind.Tamer] = 5;
            //game.monsterCtrl.monsterMilk.Increase(1000000000);
        }

        setActive(enchantBox);
        setActive(stoneBox);
        setActive(abilityPointBox);
        setActive(timescaleSlider.gameObject);
        setActive(petBox);
        //setActive(hardresetButton.gameObject);
        isShow = true;
    }

}
