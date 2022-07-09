using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static UsefulMethod;
using static SkillParameter;
using static GameController;
using System;

public partial class SaveR
{
    public bool[] isEquippedWarriorSkillForWarrior;
    public bool[] isEquippedWizardSkillForWarrior;
    public bool[] isEquippedAngelSkillForWarrior;
    public bool[] isEquippedThiefSkillForWarrior;
    public bool[] isEquippedArcherSkillForWarrior;
    public bool[] isEquippedTamerSkillForWarrior;

    public bool[] isEquippedWarriorSkillForWizard;
    public bool[] isEquippedWizardSkillForWizard;
    public bool[] isEquippedAngelSkillForWizard;
    public bool[] isEquippedThiefSkillForWizard;
    public bool[] isEquippedArcherSkillForWizard;
    public bool[] isEquippedTamerSkillForWizard;

    public bool[] isEquippedWarriorSkillForAngel;
    public bool[] isEquippedWizardSkillForAngel;
    public bool[] isEquippedAngelSkillForAngel;
    public bool[] isEquippedThiefSkillForAngel;
    public bool[] isEquippedArcherSkillForAngel;
    public bool[] isEquippedTamerSkillForAngel;

    public bool[] isEquippedWarriorSkillForThief;
    public bool[] isEquippedWizardSkillForThief;
    public bool[] isEquippedAngelSkillForThief;
    public bool[] isEquippedThiefSkillForThief;
    public bool[] isEquippedArcherSkillForThief;
    public bool[] isEquippedTamerSkillForThief;

    public bool[] isEquippedWarriorSkillForArcher;
    public bool[] isEquippedWizardSkillForArcher;
    public bool[] isEquippedAngelSkillForArcher;
    public bool[] isEquippedThiefSkillForArcher;
    public bool[] isEquippedArcherSkillForArcher;
    public bool[] isEquippedTamerSkillForArcher;

    public bool[] isEquippedWarriorSkillForTamer;
    public bool[] isEquippedWizardSkillForTamer;
    public bool[] isEquippedAngelSkillForTamer;
    public bool[] isEquippedThiefSkillForTamer;
    public bool[] isEquippedArcherSkillForTamer;
    public bool[] isEquippedTamerSkillForTamer;
}

public class SkillSet
{
    public BATTLE_CONTROLLER battleCtrl;
    public HeroKind currentHero;
    public Multiplier maxSkillSlotNum { get => game.statsCtrl.SkillSlotNum(currentHero); }
    public Multiplier maxGlobalSkillSlotNum { get => game.statsCtrl.globalSkillSlotNum; }

    public SkillSet(BATTLE_CONTROLLER battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        this.currentHero = battleCtrl.heroKind;
        SetMpRegenerate();
    }
    public void Initialize()
    {
        UpdateCurrentSkillSet();
        CheckMaxNum();
        UpdateCurrentSkillSet();
    }

    void SetMpRegenerate()
    {
        //for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        //{
            for (int j = 0; j < game.skillCtrl.skillsOneDimensionArray.Length; j++)
            {
                int count = j;
                game.statsCtrl.MpRegenerate(currentHero).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Skill, MultiplierType.Add, game.skillCtrl.skillsOneDimensionArray[count].GainMp , () => IsEquipped(game.skillCtrl.skillsOneDimensionArray[count])));
            }
        //}
    }

    public void SetUI(Action equipAction)
    {
        this.equipAction = equipAction;
        this.equipAction();
    }

    public SKILL[] currentSkillSet = new SKILL[SkillParameter.maxSkillSlotNum];
    public SKILL[] currentGlobalSkillSet = new SKILL[SkillParameter.maxGlobalSkillSlotNum];
    public int currentEquippingNum { get => Math.Min(currentRealEquippingNum, battleCtrl.limitedSkillNum); }
    int currentRealEquippingNum;
    public int currentGlobalEquippingNum { get => Math.Min(currentRealGlobalEquippingNum, battleCtrl.limitedGlobalSkillNum); }
    int currentRealGlobalEquippingNum;

    public void UpdateCurrentSkillSet()
    {
        int tempId = 0;
        int tempGlobalId = 0;
        for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
        {
            for (int i = 0; i < Enum.GetNames(typeof(SkillKindWarrior)).Length; i++)
            {
                if (IsEquipped(game.skillCtrl.Skill((HeroKind)j, i)))
                {
                    if (j == (int)currentHero)
                    {
                        currentSkillSet[tempId] = game.skillCtrl.Skill((HeroKind)j, i);
                        tempId++;
                    }
                    else
                    {
                        currentGlobalSkillSet[tempGlobalId] = game.skillCtrl.Skill((HeroKind)j, i);
                        tempGlobalId++;
                    }
                }
            }
        }
        for (int i = tempId; i < currentSkillSet.Length; i++)
        {
            currentSkillSet[i] = game.skillCtrl.NullSkill();
        }
        for (int i = tempGlobalId; i < currentGlobalSkillSet.Length; i++)
        {
            currentGlobalSkillSet[i] = game.skillCtrl.NullSkill();
        }
        currentRealEquippingNum = tempId;
        currentRealGlobalEquippingNum = tempGlobalId;
        if (game.IsUI(currentHero) && equipAction != null) equipAction();
    }

    public bool IsEquipped(SKILL skill)
    {
        return isEquippedSkillArray[(int)currentHero][(int)skill.heroKind][skill.id];
    }

    public void EquipOrRemove(SKILL skill)
    {
        if (skill == game.skillCtrl.NullSkill()) return; 
        if (IsEquipped(skill)) Remove(skill);
        else Equip(skill);
    }

    public void Equip(SKILL skill)
    {
        if (CanEquip(skill))
        {
            isEquippedSkillArray[(int)currentHero][(int)skill.heroKind][skill.id] = true;
        }
        CheckMaxNum();
        UpdateCurrentSkillSet();
    }

    public void Remove(SKILL skill)
    {
        if (CanRemove(skill))
        {
            isEquippedSkillArray[(int)currentHero][(int)skill.heroKind][skill.id] = false;
        }
        UpdateCurrentSkillSet();
    }
    public bool CanEquip(SKILL skill)
    {
        if (IsEquipped(skill)) return false;
        if (!skill.CanEquip()) return false;
        if (skill.heroKind == currentHero)
            return currentEquippingNum < maxSkillSlotNum.Value();
        return currentGlobalEquippingNum < maxGlobalSkillSlotNum.Value();
    }
    public bool CanRemove(SKILL skill)
    {
        if (skill == game.skillCtrl.NullSkill()) return false;
        if (skill.heroKind == currentHero && skill.id <= 0) return false;
        return true;
        //if (!IsEquipped(skill)) return false;
        //if (skill.heroKind == currentHero)
        //    return currentEquippingNum > 1;
        //return currentGlobalEquippingNum > 1;
    }

    void CheckInitialized()
    {
        if (currentEquippingNum <= 0) isEquippedSkillArray[(int)currentHero][(int)currentHero][0] = true;
    }
    public void CheckMaxNum()
    {
        if (currentEquippingNum > maxSkillSlotNum.Value())
        {
            for (int i = 0; i < isEquippedSkillArray[(int)currentHero][(int)currentHero].Length; i++)
            {
                isEquippedSkillArray[(int)currentHero][(int)currentHero][i] = false;
            }
        }
        if (currentGlobalEquippingNum > maxGlobalSkillSlotNum.Value())
        {
            for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
            {
                if (j != (int)currentHero)
                {
                    for (int i = 0; i < isEquippedSkillArray[(int)currentHero][(int)currentHero].Length; i++)
                    {
                        isEquippedSkillArray[(int)currentHero][j][i] = false;
                    }
                }
            }
        }
        CheckInitialized();
    }

    //BaseSkill以外のスキルを装備しているかどうか
    public bool IsEquippedAnySkill()//BaseSkillは除く
    {
        if (currentEquippingNum > 1) return true;
        if (currentGlobalEquippingNum > 0) return true;
        return false;
    }

    public Action equipAction = null;
    public bool[][][] isEquippedSkillArray =
        new bool[][][]
        {
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForWarrior,
                    main.SR.isEquippedWizardSkillForWarrior,
                    main.SR.isEquippedAngelSkillForWarrior,
                    main.SR.isEquippedThiefSkillForWarrior,
                    main.SR.isEquippedArcherSkillForWarrior,
                    main.SR.isEquippedTamerSkillForWarrior,
                },
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForWizard,
                    main.SR.isEquippedWizardSkillForWizard,
                    main.SR.isEquippedAngelSkillForWizard,
                    main.SR.isEquippedThiefSkillForWizard,
                    main.SR.isEquippedArcherSkillForWizard,
                    main.SR.isEquippedTamerSkillForWizard,
                },
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForAngel,
                    main.SR.isEquippedWizardSkillForAngel,
                    main.SR.isEquippedAngelSkillForAngel,
                    main.SR.isEquippedThiefSkillForAngel,
                    main.SR.isEquippedArcherSkillForAngel,
                    main.SR.isEquippedTamerSkillForAngel,
                },
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForThief,
                    main.SR.isEquippedWizardSkillForThief,
                    main.SR.isEquippedAngelSkillForThief,
                    main.SR.isEquippedThiefSkillForThief,
                    main.SR.isEquippedArcherSkillForThief,
                    main.SR.isEquippedTamerSkillForThief,
                },
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForArcher,
                    main.SR.isEquippedWizardSkillForArcher,
                    main.SR.isEquippedAngelSkillForArcher,
                    main.SR.isEquippedThiefSkillForArcher,
                    main.SR.isEquippedArcherSkillForArcher,
                    main.SR.isEquippedTamerSkillForArcher,
                },
                new bool[][]
                {
                    main.SR.isEquippedWarriorSkillForTamer,
                    main.SR.isEquippedWizardSkillForTamer,
                    main.SR.isEquippedAngelSkillForTamer,
                    main.SR.isEquippedThiefSkillForTamer,
                    main.SR.isEquippedArcherSkillForTamer,
                    main.SR.isEquippedTamerSkillForTamer,
                },
    };
}
