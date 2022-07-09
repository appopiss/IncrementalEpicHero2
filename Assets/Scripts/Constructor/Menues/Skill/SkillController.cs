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
    public int[] currentStanceId;//[heroKind]
}

public class SkillController
{
    public SKILL[] skillsOneDimensionArray;//スキルをただ羅列しただけ（multiplierの一括登録などに使う）
    public ClassSkill[] classSkills = new ClassSkill[Enum.GetNames(typeof(HeroKind)).Length];
    private NullSkill nullSkill = new NullSkill();

    public Multiplier[] skillLevelBonus = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] skillLevelBonusFromHolyArch = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] skillRankCostFactors = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    //↓これはRB4 upgradeで0.95^upgradeLevelぐらいにする
    //public Multiplier[] skillRankCostIncrementFactors = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] skillPassiveShareFactors = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];

    public Multiplier[] skillCooltimeReduction = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public SkillController()
    {
        for (int i = 0; i < skillRankCostFactors.Length; i++)
        {
            skillRankCostFactors[i] = new Multiplier();// new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        }
        //for (int i = 0; i < skillRankCostIncrementFactors.Length; i++)
        //{
        //    skillRankCostIncrementFactors[i] = new Multiplier();// new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        //}
        for (int i = 0; i < skillCooltimeReduction.Length; i++)
        {
            skillCooltimeReduction[i] = new Multiplier(() => 0.50, () => 0d);
        }
        classSkills[(int)HeroKind.Warrior] = new WarriorSkill();
        classSkills[(int)HeroKind.Wizard] = new WizardSkill();
        classSkills[(int)HeroKind.Angel] = new AngelSkill();
        classSkills[(int)HeroKind.Thief] = new ThiefSkill();
        classSkills[(int)HeroKind.Archer] = new ArcherSkill();
        classSkills[(int)HeroKind.Tamer] = new TamerSkill();
        List<SKILL> tempList = new List<SKILL>();
        for (int i = 0; i < classSkills.Length; i++)
        {
            tempList.AddRange(classSkills[i].skills);
        }
        skillsOneDimensionArray = tempList.ToArray();

        for (int i = 0; i < skillLevelBonus.Length; i++)
        {
            skillLevelBonus[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 0));
            skillLevelBonus[i].calculateSpanModifier = 2.5d;
            skillLevelBonusFromHolyArch[i] = new Multiplier();
            //skillLevelBonus[i].calculateSpanModifier = 1.0d;
        }
        for (int i = 0; i < skillPassiveShareFactors.Length; i++)
        {
            skillPassiveShareFactors[i] = new Multiplier();
        }
        for (int i = 0; i < baseAttackPoisonChance.Length; i++)
        {
            baseAttackPoisonChance[i] = new Multiplier();
        }
        for (int i = 0; i < baseAttackSlimeBall.Length; i++)
        {
            baseAttackSlimeBall[i] = new Multiplier();
        }
        globalSkillProfGainRate = new Multiplier();
    }

    //外部アクセス用
    public SKILL Skill(HeroKind heroKind, int id)
    {
        return SkillArray(heroKind)[id];
    }
    public SKILL[] SkillArray(HeroKind heroKind)
    {
        return classSkills[(int)heroKind].skills;
    }
    public SKILL NullSkill()
    {
        return nullSkill;
    }

    public void SetTrigger(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        for (int i = 0; i < skillsOneDimensionArray.Length; i++)
        {
            skillsOneDimensionArray[i].SetTrigger(battleCtrl, myself, target, targetList);
        }
    }
    public void ResetSimulatedTrigger()
    {
        for (int i = 0; i < skillsOneDimensionArray.Length; i++)
        {
            skillsOneDimensionArray[i].simulatedAttackList = new List<Attack>();
        }
    }

    public Multiplier globalSkillProfGainRate;

    public void AutoRankup()
    {
        if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.SkillRankUp)) return;
        for (int i = 0; i < skillsOneDimensionArray.Length; i++)
        {
            skillsOneDimensionArray[i].rankupTransaction.Buy(true);
        }
    }

    public Multiplier[] baseAttackPoisonChance = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] baseAttackSlimeBall = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
}

public class NullSkill : SKILL
{
    public NullSkill(HeroKind heroKind = HeroKind.Warrior, int id = -1) : base(heroKind, id)
    {
    }
}

public class ClassSkill
{
    public virtual HeroKind heroKind { get; }
    public SKILL[] skills = new SKILL[Enum.GetNames(typeof(SkillKindWarrior)).Length];
    public Stance[] stances = new Stance[0];
    public int currentStanceId { get => main.SR.currentStanceId[(int)heroKind]; set => main.SR.currentStanceId[(int)heroKind] = value; }
    public void SwitchStance(int id)
    {
        if (!CansSwitchStance(id)) return;
        currentStanceId = id;
    }
    public bool CansSwitchStance(int id)
    {
        if (id < 0 || id >= stances.Length) return false;
        return true;
    }
}


