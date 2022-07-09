using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteSourceUI : MonoBehaviour
{
    public static SpriteSourceUI sprite;
    //Nitro
    public Sprite[] nitro;
    //Hero
    public Sprite[] heroesWholebody;
    //SwitchHero
    public Sprite[] heroesPortrait;
    //Resource
    public Sprite[] resourcesBackgroundBlack;
    public Sprite[] resourcesTrans;
    //Material
    public Sprite[] materials;
    //Status
    public Sprite skillSlot;
    public Sprite lockedSlot;
    public Sprite autoMove, manualMove;
    //SkillIcon
    public Sprite[] warriorSkills;
    public Sprite[] wizardSkills;
    public Sprite[] angelSkills;
    public Sprite[] thiefSkills;
    public Sprite[] archerSkills;
    public Sprite[] tamerSkills;
    //SkillEffect
    public Sprite[] warriorSkillEffects;
    public Sprite[] wizardSkillEffects;
    public Sprite[] angelSkillEffects;
    public Sprite[] thiefSkillEffects;
    public Sprite[] archerSkillEffects;
    public Sprite[] tamerSkillEffects;
    //AttackEffect
    public Sprite[] elementAttackEffects;//[element]
    public Sprite[] challengeAttackEffects;//[challengeMonsterKind]
    //Equipment 
    public Sprite[] equipments;
    //public Sprite[] eqCommon;
    //public Sprite[] eqUncommon;
    //public Sprite[] eqRare;
    //public Sprite[] eqSuperRare;
    //public Sprite[] eqEpic;
    //public Sprite[] eqSet;
    public Sprite inventorySlot, weaponSlot, armorSlot, jewelrySlot, potionSlot;
    public Sprite[] enchants;
    public Sprite[] equipmentDrops;//[rarity]
    //Alchemy
    public Sprite[] catalysts;
    public Sprite[] potions;

    public Sprite questionmarkSlot;

    //Quest
    public Sprite[] questIcons;
    public Sprite globalquestButton, generalquestButton;
    //Battle
    public Sprite[] heroAvaters1;//[HeroKind]
    public Sprite[] heroAvaters2;
    public Sprite[] slimes1;//[color]
    public Sprite[] slimes2;
    public Sprite[] magicSlimes1;
    public Sprite[] magicSlimes2;
    public Sprite[] spider1;
    public Sprite[] spider2;
    public Sprite[] bat1;
    public Sprite[] bat2;
    public Sprite[] fairy1;
    public Sprite[] fairy2;
    public Sprite[] fox1;
    public Sprite[] fox2;
    public Sprite[] devilFish1;
    public Sprite[] devilFish2;
    public Sprite[] treant1;
    public Sprite[] treant2;
    public Sprite[] flameTiger1;
    public Sprite[] flameTiger2;
    public Sprite[] unicorn1;
    public Sprite[] unicorn2;
    public Sprite[] mimic1;
    public Sprite[] mimic2;
    public Sprite[] challenge1;
    public Sprite[] challenge2;

    //Field
    public Sprite[] fieldSlime;
    public Sprite[] fieldMagicSlime;
    public Sprite[] fieldSpider;
    public Sprite[] fieldBat;
    public Sprite[] fieldFairy;
    public Sprite[] fieldFox;
    public Sprite[] fieldDevilfish;
    public Sprite[] fieldTreant;
    public Sprite[] fieldFlametiger;
    public Sprite[] fieldUnicorn;

    public Sprite[] fieldChallenge;

    public Sprite[] fieldDungeonSlime;
    public Sprite[] fieldDungeonMagicSlime;
    public Sprite[] fieldDungeonSpider;
    public Sprite[] fieldDungeonBat;
    public Sprite[] fieldDungeonFairy;
    public Sprite[] fieldDungeonFox;
    public Sprite[] fieldDungeonDevilfish;
    public Sprite[] fieldDungeonTreant;
    public Sprite[] fieldDungeonFlametiger;
    public Sprite[] fieldDungeonUnicorn;

    //Blessing
    public Sprite[] blessing;
    public Sprite[] debuff;

    //EpicStore
    public Sprite[] epicStoreIcons;

    [NonSerialized] public Sprite[][] heroAvaters;//[1,2][HeroKind]
    [NonSerialized] public Sprite[][][] monsters;//[1or2][Species][Color]
    [NonSerialized] public Sprite[][] skillEffects;//[HeroKind][SkillId]
    [NonSerialized] public Sprite[][] skillIcons;//[HeroKind][SkillId]
    [NonSerialized] public Sprite[][] fields;//[AreaKind][AreaId]
    [NonSerialized] public Sprite[][] fieldsDungeon;//[AreaKind][AredId]
    private void Awake()
    {
        sprite = this;

        heroAvaters = new Sprite[][]
        {
            heroAvaters1,
            heroAvaters2
        };
        monsters = new Sprite[][][]
        {
            new Sprite[][]
            {
                slimes1,
                magicSlimes1,
                spider1,
                bat1,
                fairy1,
                fox1,
                devilFish1,
                treant1,
                flameTiger1,
                unicorn1,
                mimic1,
                challenge1
            },
            new Sprite[][]
            {
                slimes2,
                magicSlimes2,
                spider2,
                bat2,
                fairy2,
                fox2,
                devilFish2,
                treant2,
                flameTiger2,
                unicorn2,
                mimic2,
                challenge2
            }
        };
        skillEffects = new Sprite[][]
        {
            warriorSkillEffects,
            wizardSkillEffects,
            angelSkillEffects,
            thiefSkillEffects,
            archerSkillEffects,
            tamerSkillEffects,
        };
        skillIcons = new Sprite[][]
        {
            warriorSkills,
            wizardSkills,
            angelSkills,
            thiefSkills,
            archerSkills,
            tamerSkills
        };
        fields = new Sprite[][]
        {
            fieldSlime,
            fieldMagicSlime,
            fieldSpider,
            fieldBat,
            fieldFairy,
            fieldFox,
            fieldDevilfish,
            fieldTreant,
            fieldFlametiger,
            fieldUnicorn,
        };
        fieldsDungeon = new Sprite[][]
        {
            fieldDungeonSlime,
            fieldDungeonMagicSlime,
            fieldDungeonSpider,
            fieldDungeonBat,
            fieldDungeonFairy,
            fieldDungeonFox,
            fieldDungeonDevilfish,
            fieldDungeonTreant,
            fieldDungeonFlametiger,
            fieldDungeonUnicorn,
        };
        //equipments = new List<Sprite>();
        //equipments.AddRange(eqCommon);
        //equipments.AddRange(eqUncommon);
        //equipments.AddRange(eqRare);
        //equipments.AddRange(eqSuperRare);
        //equipments.AddRange(eqEpic);
        //equipments.AddRange(eqSet);
    }
}
