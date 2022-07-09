using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public class TamerSkill : ClassSkill
{
    public TamerSkill()
    {
        skills[0] = new SonnetAttack(HeroKind.Tamer, 0);
        skills[1] = new AttackingOrder(HeroKind.Tamer, 1);
        skills[2] = new RushOrder(HeroKind.Tamer, 2);
        skills[3] = new DefensiveOrder(HeroKind.Tamer, 3);
        skills[4] = new SoothingBallad(HeroKind.Tamer, 4);
        skills[5] = new OdeOfFriendship(HeroKind.Tamer, 5);
        skills[6] = new AnthemOfEnthusiasm(HeroKind.Tamer, 6);
        skills[7] = new FeedChilli(HeroKind.Tamer, 7);
        skills[8] = new BreedingKnowledge(HeroKind.Tamer, 8);
        skills[9] = new TuneOfTotalTaming(HeroKind.Tamer, 9);
    }
}

public class SonnetAttack : SKILL
{
    public SonnetAttack(HeroKind heroKind, int id) : base(heroKind, id)
    {
        //passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Enables to summon one pet while this skill equipped", SetSummonSlot));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, GlobalStats.LeafGain, MultiplierType.Mul, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, GlobalStats.LeafGain, MultiplierType.Mul, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, GlobalStats.LeafGain, MultiplierType.Mul, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, GlobalStats.LeafGain, MultiplierType.Mul, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, Stats.SkillProficiencyGain, MultiplierType.Add, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Pet Summon Slot + 1 while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.SkillProficiencyGain, MultiplierType.Add, 0.20));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.HP, MultiplierType.Add, 1000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.SkillProficiencyGain, MultiplierType.Add, 0.30));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Pet Summon Slot + 1 while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, Stats.SkillProficiencyGain, MultiplierType.Add, 0.40));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
        if (rank.value <= 0) rank.ChangeValue(1);
    }
    public override Element element => Element.Light;
    public override string Description()
    {
        return optStrL + "Enables to summon pets\n- Pet's Basic Attack Damage Multiplier : " + percent(Damage()) + " ( + " + percent(IncrementDamagePerLevel()) + " / Lv )";
    }
    public override void SetAttack(BATTLE_CONTROLLER battleCtrl, BATTLE myself, Func<BATTLE> target, List<BATTLE> targetList)
    {
        base.SetAttack(battleCtrl, myself, target, targetList);
        AttackList(battleCtrl).Add(new Attack(battleCtrl, targetList, (x) => EffectInitPosition(SkillEffectCenter.Myself, x, target), (x) => Damage(x) * game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value(), () => IsCrit(myself), () => 35f, () => 1, () => element, () => LotteryDebuff(), () => target().move.position));
    }
    double SummonPetNum()
    {
        if (level.value >= 200) return 3;
        if (level.value >= 100) return 2;
        return 1;
    }
    public override void SetBuff(HeroKind heroKind)
    {
        game.statsCtrl.heroes[(int)heroKind].summonPetSlot.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Skill, MultiplierType.Add, SummonPetNum, () => IsEquipped(heroKind)));
    }
    public override void Attack(BATTLE battle)
    {
        base.Attack(battle);
        //SlimeBall
        if (!game.IsUI(battle.battleCtrl.heroKind) || game.skillCtrl.baseAttackSlimeBall[(int)heroKind].Value() <= 0) return;
        AttackArray(battle.battleCtrl)[1].ThrowAttack(battle, null, true);
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
        //UI
        if (effectUIAction != null)
        {
            effectUIAction(attackLists[(int)heroKind][0], SpriteSourceUI.sprite.skillEffects[(int)this.heroKind][id]);
            effectUIActionWithDirection(attackLists[(int)heroKind][1], SpriteSourceUI.sprite.challengeAttackEffects[0], () => attackLists[(int)heroKind][1].throwVec);
        }
    }
    public override Debuff debuff
    {
        get
        {
            if (game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value() > 0) return Debuff.Poison;
            return Debuff.Nothing;
        }
    }
    public override double DebuffChance()
    {
        return game.skillCtrl.baseAttackPoisonChance[(int)heroKind].Value();
    }
}

public class AttackingOrder : SKILL
{
    public AttackingOrder(HeroKind heroKind, int id) : base(heroKind, id)
    {
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MP, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Mul, 0.01d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.01));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.ATK, MultiplierType.Mul, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MATK, MultiplierType.Mul, 0.02d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.ATK, MultiplierType.Mul, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MATK, MultiplierType.Mul, 0.03d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 110, BasicStatsKind.ATK, MultiplierType.Mul, 0.04d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.MATK, MultiplierType.Mul, 0.04d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 130, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Multiplies this skill's Damage Multiplier by 300%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Order;
    public override string Description()
    {
        return optStrL + "Gives the summoned pets an attack order regardless their cooldown\n- Pet's Attack Damage Multiplier : " + percent(Damage()) + " ( + " + percent(IncrementDamagePerLevel()) + " / Lv )";
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return 0;// Damage() * 100d;
    }
    double tempMul;
    public override double Damage()
    {
        if (level.value >= 250) tempMul = 48;
        else if (level.value >= 200) tempMul = 16;
        else if (level.value >= 150) tempMul = 8;
        else if (level.value >= 100) tempMul = 4;
        else if (level.value >= 50) tempMul = 2;
        else tempMul = 1;
        return base.Damage() * tempMul;
    }
    PET_BATTLE pet;
    public override void DoOrder(BATTLE battle, bool isUI)
    {
        for (int i = 0; i < battle.battleCtrl.pets.Length; i++)
        {
            pet = battle.battleCtrl.pets[i];
            if (pet.isAlive)
            {
                pet.Attack(Damage());
                //Loyalty
                pet.globalInformation.pet.loyaltyExp.Increase(game.statsCtrl.heroes[(int)battle.heroKind].loyaltyPoingGain.Value() * skillAbuseMpPercents[(int)battle.heroKind]);
            }
        }
    }
}

public class RushOrder : SKILL
{
    public RushOrder(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.AttackingOrder, 90);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.ATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MATK, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.ATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 110, BasicStatsKind.MATK, MultiplierType.Mul, 0.20d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.ATK, MultiplierType.Mul, 0.20d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 130, BasicStatsKind.MATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Order;
    public override string Description()
    {
        return optStrL + "Gives the summoned pets an order to let them rush to random monsters\n- Pet's Attack Damage Multiplier : " + percent(Damage()) + " ( + " + percent(IncrementDamagePerLevel()) + " / Lv )";
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return 0;// Damage() * 100d;
    }
    double tempMul;
    public override double Damage()
    {
        //if (level.value >= 250) tempMul = 48;
        if (level.value >= 200) tempMul = 16;
        else if (level.value >= 150) tempMul = 8;
        else if (level.value >= 100) tempMul = 4;
        else if (level.value >= 50) tempMul = 2;
        else tempMul = 1;
        return base.Damage() * tempMul;
    }
    BATTLE[] tempTargets = new BATTLE[Parameter.maxPetSpawnNum];
    PET_BATTLE[] tempPets = new PET_BATTLE[Parameter.maxPetSpawnNum];
    bool[] isAttacked = new bool[Parameter.maxPetSpawnNum];
    bool isTry;
    public override async void AttackWithTime(BATTLE battle, bool isUI)
    {
        //まず、ターゲットを決める
        for (int i = 0; i < tempPets.Length; i++)
        {
            tempPets[i] = battle.battleCtrl.pets[i];
            isAttacked[i] = false;
            if (tempPets[i].isAlive)
            {
                tempTargets[i] = battle.RandomTarget();
                //この時点でLoyalty獲得
                tempPets[i].globalInformation.pet.loyaltyExp.Increase(game.statsCtrl.heroes[(int)battle.heroKind].loyaltyPoingGain.Value() * skillAbuseMpPercents[(int)battle.heroKind]);
            }
        }
        //ターゲットまで走り、攻撃する
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < tempPets.Length; j++)
            {
                if (tempPets[j].isAlive && tempTargets[j].isAlive && !isAttacked[j])
                {
                    if (tempPets[j].IsWithinRange(tempPets[j], tempTargets[j], tempPets[j].range))
                    {
                        tempPets[j].Attack(Damage());
                        isAttacked[j] = true;
                    }
                    else
                    {
                        tempPets[j].Move(tempTargets[j].move.position - tempPets[j].move.position, battle.battleCtrl.deltaTime * 5);
                        isTry = true;
                    }
                }
            }
            if (!isTry) break;
            isTry = false;
            if (!battle.battleCtrl.isSimulated) await UniTask.DelayFrame(1);
        }
    }
    public override void Attack(BATTLE battle)
    {
    }
    public override void ShowEffectUI(HeroKind heroKind)
    {
    }
    public override void DoOrder(BATTLE battle, bool isUI)
    {
    }
}

//Defensive : Passive : 装備中DEF/MDEF UP
public class DefensiveOrder : SKILL
{
    public DefensiveOrder(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.AttackingOrder, 150);
        requiredSkills.Add((int)SkillKindTamer.RushOrder, 24);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.DEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MDEF, MultiplierType.Add, 50));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.DEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MDEF, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.DEF, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.MDEF, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.DEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.MDEF, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.DEF, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, BasicStatsKind.MDEF, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Mul, 0.45d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Multiplies this skill's Damage Multiplier by 200%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Add, 2000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.HP, MultiplierType.Mul, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    double tempMul;
    public override double Damage()
    {
        //if (level.value >= 250) tempMul = 48;
        //else if (level.value >= 200) tempMul = 16;
        if (level.value >= 200) tempMul = 8;
        else if (level.value >= 100) tempMul = 4;
        else if (level.value >= 50) tempMul = 2;
        else tempMul = 1;
        return base.Damage() * tempMul;
    }
    public override SkillType type => SkillType.Order;
    public override string Description()
    {
        return optStrL + "Gives the summoned pets an order to let them come back to the hero and attack the closest monster to the hero\n- Pet's Attack Damage Multiplier : " + percent(Damage()) + " ( + " + percent(IncrementDamagePerLevel()) + " / Lv )";
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return 0;
    }
    PET_BATTLE pet;
    public override void DoOrder(BATTLE battle, bool isUI)
    {
        for (int i = 0; i < battle.battleCtrl.pets.Length; i++)
        {
            pet = battle.battleCtrl.pets[i];
            if (pet.isAlive)
            {
                //戻ってくる
                pet.move.MoveTo(battle.move.position + Parameter.RandomVec() * 50f, true);
                pet.Attack(Damage());
                //Loyalty
                pet.globalInformation.pet.loyaltyExp.Increase(game.statsCtrl.heroes[(int)battle.heroKind].loyaltyPoingGain.Value() * skillAbuseMpPercents[(int)battle.heroKind]);
            }
        }
    }
}

public class SoothingBallad : SKILL
{
    public SoothingBallad(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.SonnetAttack, 60);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.HP, MultiplierType.Mul, 0.05));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.HP, MultiplierType.Add, 1000));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "25% chance to double Heal Point every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MP, MultiplierType.Add, 250));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.HP, MultiplierType.Mul, 0.10));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "25% chance to cure debuffs every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.HP, MultiplierType.Mul, 0.15));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "5% chance to additionally restore 10% of Max HP"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.HP, MultiplierType.Add, 1500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MP, MultiplierType.Mul, 0.10d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.HP, MultiplierType.Mul, 0.25));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "0.5% chance to Full Heal every trigger"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Heal;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override string Description()
    {
        return "Restore the summoned pets' HP";
    }
    public override void DoHeal(BATTLE battle)
    {
        for (int i = 0; i < battle.battleCtrl.pets.Length; i++)
        {
            if (battle.battleCtrl.pets[i].isAlive)
            {
                LotteryHeal(battle.battleCtrl.pets[i]);
            }
        }
    }
    public void LotteryHeal(BATTLE battle)
    {
        if (level.value >= 50 && WithinRandom(0.25d)) battle.Heal(HealPoint() * 2);
        else battle.Heal(HealPoint() * skillAbuseMpPercents[(int)battle.heroKind]);
        if (level.value >= 100 && WithinRandom(0.25d)) battle.CureDebuff();
        if (level.value >= 150 && WithinRandom(0.05d)) battle.Heal(battle.hp * 0.10d);
        if (level.value >= 250 && WithinRandom(0.005d)) battle.FullHeal();
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        if (isDisplay) return HealPoint();
        return HealPoint() * skillAbuseMpPercents[(int)myself.heroKind];
    }
}

public class OdeOfFriendship : SKILL
{
    public OdeOfFriendship(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.SonnetAttack, 96);
        requiredSkills.Add((int)SkillKindTamer.SoothingBallad, 18);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 5, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "This skill's chance effect + 20%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 75, BasicStatsKind.MP, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's chance effect + 20%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, BasicStatsKind.MP, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "This skill's chance effect + 20%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, BasicStatsKind.MP, MultiplierType.Mul, 0.35d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's chance effect + 20%"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MP, MultiplierType.Mul, 0.45d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.MP, MultiplierType.Mul, 0.65d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.Nothing;
    public override SkillEffectCenter effectCenter => SkillEffectCenter.Myself;
    public override string Description()
    {
        return optStrL + percent(Chance()) + " chance that each summoned pet randomly uses a skill that the hero equips every " + tDigit(Interval(game.battleCtrl.hero), 3) + " " + Localized.localized.Basic(BasicWord.Sec)
            + "\n- Pets use no MP to trigger a skill regardless its cooldown"
            + "\n- Skill Proficiency Gain Rate by the pet use : " + percent(BuffPercent()) + " ( + " + percent(IncrementBuffPercentPerLevel(), 3) + " / Lv )"
            ;         
    }
    public override void SetBuff(HeroKind heroKind)
    {
        //var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        //game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(info);
    }
    public override void DoBuff(BATTLE battle)
    {
        for (int i = 0; i < battle.battleCtrl.pets.Length; i++)
        {
            if (battle.battleCtrl.pets[i].isAlive && WithinRandom(Chance()))
            {
                battle.TriggerRandomSkill(battle.battleCtrl.pets[i], BuffPercent());
            }
        }
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return 0;
    }
    public override double IncrementBuffPercentPerLevel()
    {
        return incrementDamage;
    }
    public override double BuffPercent()
    {
        return Math.Min(1, base.BuffPercent());
    }
    double Chance()
    {
        if (level.value >= 200) return 1.0d;
        if (level.value >= 150) return 0.8d;
        if (level.value >= 100) return 0.6d;
        if (level.value >= 50) return 0.4d;
        return 0.2d;
    }
}

public class AnthemOfEnthusiasm : SKILL
{
    public AnthemOfEnthusiasm(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.SonnetAttack, 120);
        requiredSkills.Add((int)SkillKindTamer.OdeOfFriendship, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.MP, MultiplierType.Add, 250));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.ATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.MATK, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, Stats.MoveSpeed, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.MATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.ATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MATK, MultiplierType.Mul, 0.20d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, BasicStatsKind.ATK, MultiplierType.Mul, 0.20d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, Stats.MoveSpeed, MultiplierType.Mul, 0.025d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 110, BasicStatsKind.ATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.MATK, MultiplierType.Mul, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 130, BasicStatsKind.ATK, MultiplierType.Mul, 0.35d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.MATK, MultiplierType.Mul, 0.35d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Pet's Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, Stats.MoveSpeed, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "Pet's Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.MATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, BasicStatsKind.ATK, MultiplierType.Mul, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.Nothing;
    public override string Description()
    {
        return optStr + "The summoned pet's ATK & MATK : + " + percent(BuffPercent(), 3)
            + " ( + " + percent(IncrementBuffPercentPerLevel(), 3) + " / Lv )";
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.heroes[(int)heroKind].summonPetATKMATKMultiplier.RegisterMultiplier(info);
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, CritChance, () => IsEquipped(heroKind));
        game.statsCtrl.heroes[(int)heroKind].summonPetCriticalChanceAdder.RegisterMultiplier(info);
    }
    double CritChance()
    {
        if (level.value >= 200) return 0.10d;
        if (level.value >= 150) return 0.05d;
        return 0;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class FeedChilli : SKILL
{
    public FeedChilli(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.OdeOfFriendship, 60);
        requiredSkills.Add((int)SkillKindTamer.AnthemOfEnthusiasm, 48);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Pet's Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.SPD, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, "Pet's Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.SPD, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Pet's Critical Chance + 5% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, BasicStatsKind.SPD, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, BasicStatsKind.ATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MATK, MultiplierType.Mul, 0.15d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Pet's Critical Chance + 10% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 120, BasicStatsKind.SPD, MultiplierType.Add, 400));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 140, BasicStatsKind.ATK, MultiplierType.Mul, 0.60d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 160, BasicStatsKind.MATK, MultiplierType.Mul, 0.60d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 180, BasicStatsKind.SPD, MultiplierType.Add, 500));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, BasicStatsKind.MATK, MultiplierType.Mul, 1.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, BasicStatsKind.ATK, MultiplierType.Mul, 1.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Pet's Critical Chance + 25% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Buff;
    public override Buff buff => Buff.Nothing;
    public override string Description()
    {
        return optStr + "The summoned pet's Attack Speed & Move Speed : + " + percent(BuffPercent(), 3)
            + " ( + " + percent(IncrementBuffPercentPerLevel(), 4) + " / Lv )";
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, BuffPercent, () => IsEquipped(heroKind));
        game.statsCtrl.heroes[(int)heroKind].summonPetSPDMoveSpeedMultiplier.RegisterMultiplier(info);
        info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Add, CritChance, () => IsEquipped(heroKind));
        game.statsCtrl.heroes[(int)heroKind].summonPetCriticalChanceAdder.RegisterMultiplier(info);
    }
    double CritChance()
    {
        if (level.value >= 250) return 0.50d;
        if (level.value >= 100) return 0.25d;
        if (level.value >= 50) return 0.15d;
        if (level.value >= 30) return 0.10d;
        if (level.value >= 10) return 0.05d;
        return 0;
    }
    public override double TotalDps(BATTLE myself, bool isDisplay = false)
    {
        return BuffPercent() * 100d;
    }
}

public class BreedingKnowledge : SKILL
{
    public BreedingKnowledge(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.SonnetAttack, 12);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, BasicStatsKind.HP, MultiplierType.Add, 200));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, BasicStatsKind.MP, MultiplierType.Add, 100));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, BasicStatsKind.HP, MultiplierType.Add, 300));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, BasicStatsKind.MP, MultiplierType.Add, 150));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Loyalty Point Gain + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, Stats.ExpGain, MultiplierType.Add, 0.25d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, BasicStatsKind.MP, MultiplierType.Mul, 0.05d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "This skill's Monster Milk Gain + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 125, Stats.ExpGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 150, "Loyalty Point Gain + 50% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 175, GlobalStats.GoldGain, MultiplierType.Add, 0.50d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 200, "This skill's Monster Milk Gain + 1"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 225, Stats.ExpGain, MultiplierType.Add, 1.00d));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 250, "Loyalty Point Gain + 100% while this skill equipped"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Attack;
    public override Element element => Element.Physical;

    public override string Description()
    {
        return percent(MonsterMilkGainChance()) + " chance to gain " + tDigit(MonsterMilkGain()) + " Monster Milk every trigger ( + " + percent(0.001d) + " / Lv )";
    }
    public override void Attack(BATTLE battle)
    {
        if (WithinRandom(MonsterMilkGainChance()))
        {
            //game.monsterCtrl.monsterMilk.Increase(MonsterMilkGain() * skillAbuseMpPercents[(int)battle.heroKind]);
            GameControllerUI.gameUI.logCtrlUI.Log(optStr + Localized.localized.Basic(BasicWord.Gained) + "<color=green> Monster Milk * " + tDigit(MonsterMilkGain()));
        }
        base.Attack(battle);
    }
    public override void SetBuff(HeroKind heroKind)
    {
        var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, LoyaltyGain, () => IsEquipped(heroKind));
        game.statsCtrl.heroes[(int)heroKind].loyaltyPoingGain.RegisterMultiplier(info);

    }
    double MonsterMilkGainChance()
    {
        return Math.Min(1d, 0.10d + 0.001d * Level());
    }
    double MonsterMilkGain()
    {
        //ここに追加
        return 1;
    }
    double LoyaltyGain()
    {
        if (level.value >= 250) return 2;
        if (level.value >= 150) return 1;
        if (level.value >= 50) return 0.5;
        return 0;
    }
}

//TamingPointGain+ (Passive) 装備時のみ、あとTriple Captureも
public class TuneOfTotalTaming : SKILL
{
    public TuneOfTotalTaming(HeroKind heroKind, int id) : base(heroKind, id)
    {
        requiredSkills.Add((int)SkillKindTamer.SonnetAttack, 250);
        requiredSkills.Add((int)SkillKindTamer.BreedingKnowledge, 180);
        passiveEffectLists.Add(new SkillPassiveEffect(this, 10, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 20, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 30, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 40, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 50, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 60, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 70, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 80, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 90, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 100, "Placeholder"));
        passiveEffectLists.Add(new SkillPassiveEffect(this, 500, Stats.SkillProficiencyGain, MultiplierType.Add, 1.00));
    }
    public override SkillType type => SkillType.Attack;
    public override Element element => Element.Light;

    public override string Description()
    {
        return percent(TamingChance()) + " chance to capture the target monster without traps every trigger ( + " + percent(0.0001d) + " / Lv )" +
            "\n- To capture, the color trap must be unlocked in Shop with enough capturable level";
    }
    public override void Attack(BATTLE battle)
    {
        if (WithinRandom(TamingChance())) battle.Target().TryCapture(battle.heroKind, true);
        base.Attack(battle);
    }
    //public override void SetBuff(HeroKind heroKind)
    //{

    //    var info = new MultiplierInfo(MultiplierKind.Buff, MultiplierType.Mul, LoyaltyGain, () => IsEquipped(heroKind));
    //    game.statsCtrl.heroes[(int)heroKind].loyaltyPoingGain.RegisterMultiplier(info);
    //}
    double TamingChance()
    {
        return Math.Min(1d, 0.005d + 0.0001d * Level());
    }
    //double LoyaltyGain()
    //{
    //    return 0;
    //}
}
