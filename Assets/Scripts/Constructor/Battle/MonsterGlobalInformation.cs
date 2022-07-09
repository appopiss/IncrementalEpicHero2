using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static MonsterParameter;
using static GameController;
using static UsefulMethod;

public partial class Save
{
    //Pet
    public long[] monsterPetRanks;//[color + 10 * species]
    public long[] monsterPetLevels;
    public double[] monsterPetExps;
    public long[] monsterPetLoyalty;
    public double[] monsterPetLoyaltyExp;

    public double[] monsterPetTamingPoints;//[color + 10 * species]
    public bool[] monsterPetIsActives;//[color + 10 * species]
}

public partial class SaveR
{
    public double[] monsterDefeatedNumsSlime;//[color + 10 * heroKind]
    public double[] monsterDefeatedNumsMagicSlime;
    public double[] monsterDefeatedNumsSpider;
    public double[] monsterDefeatedNumsBat;
    public double[] monsterDefeatedNumsFairy;
    public double[] monsterDefeatedNumsFox;
    public double[] monsterDefeatedNumsDevilFish;
    public double[] monsterDefeatedNumsTreant;
    public double[] monsterDefeatedNumsFlameTiger;
    public double[] monsterDefeatedNumsUnicorn;
    public double[] monsterDefeatedNumsMimic;
    public double[] monsterDefeatedNumsChallenge;

    public double[] monsterCapturedNumsSlime;
    public double[] monsterCapturedNumsMagicSlime;
    public double[] monsterCapturedNumsSpider;
    public double[] monsterCapturedNumsBat;
    public double[] monsterCapturedNumsFairy;
    public double[] monsterCapturedNumsFox;
    public double[] monsterCapturedNumsDevilFish;
    public double[] monsterCapturedNumsTreant;
    public double[] monsterCapturedNumsFlameTiger;
    public double[] monsterCapturedNumsUnicorn;
    public double[] monsterCapturedNumsMimic;
    public double[] monsterCapturedNumsChallenge;

    //Mutant
    public double[] monsterMutantDefeatedNumsSlime;//[color + 10 * heroKind]
    public double[] monsterMutantDefeatedNumsMagicSlime;
    public double[] monsterMutantDefeatedNumsSpider;
    public double[] monsterMutantDefeatedNumsBat;
    public double[] monsterMutantDefeatedNumsFairy;
    public double[] monsterMutantDefeatedNumsFox;
    public double[] monsterMutantDefeatedNumsDevilFish;
    public double[] monsterMutantDefeatedNumsTreant;
    public double[] monsterMutantDefeatedNumsFlameTiger;
    public double[] monsterMutantDefeatedNumsUnicorn;
    public double[] monsterMutantDefeatedNumsMimic;
    public double[] monsterMutantDefeatedNumsChallenge;

    public double[] monsterMutantCapturedNumsSlime;
    public double[] monsterMutantCapturedNumsMagicSlime;
    public double[] monsterMutantCapturedNumsSpider;
    public double[] monsterMutantCapturedNumsBat;
    public double[] monsterMutantCapturedNumsFairy;
    public double[] monsterMutantCapturedNumsFox;
    public double[] monsterMutantCapturedNumsDevilFish;
    public double[] monsterMutantCapturedNumsTreant;
    public double[] monsterMutantCapturedNumsFlameTiger;
    public double[] monsterMutantCapturedNumsUnicorn;
    public double[] monsterMutantCapturedNumsMimic;
    public double[] monsterMutantCapturedNumsChallenge;

    //PetSummon
    public MonsterSpecies[] summonSpecies;//[id + 10 * heroKind]
    public MonsterColor[] summonColor;//[id + 10 * heroKind]
    public bool[] isSetSummonPet;//[id + 10 * heroKind]

}

public class ChallengeMonsterGlobalInformation : MonsterGlobalInformation
{
    public ChallengeMonsterGlobalInformation(ChallengeMonsterKind kind)
    {
        this.challengeMonsterKind = kind;
        species = MonsterSpecies.ChallengeBoss;
        color = MonsterColor.Boss;
        for (int i = 0; i < defeatedNums.Length; i++)
        {
            int count = i;
            defeatedNums[count] = new MonsterDefeatedNumber(kind, (HeroKind)count);
            defeatedMutantNums[count] = new MonsterDefeatedNumber(kind, (HeroKind)count, true);
            capturedNums[count] = new MonsterCapturedNumber(kind, (HeroKind)count);
            capturedMutantNums[count] = new MonsterCapturedNumber(kind, (HeroKind)count, true);
        }
        pet = new MonsterPet(this, kind);
    }
    public override string Name()
    {
        switch (challengeMonsterKind)
        {
            case ChallengeMonsterKind.SlimeKing:
                return "Florzporb, The Slime King";
            case ChallengeMonsterKind.WindowQueen:
                return "Arachnetta, The Widow Queen";
            case ChallengeMonsterKind.Golem:
                return "Guardian Kor, The Stone Giant";
                //case ChallengeMonsterKind.FairyQueen:
                //    return "Fairy Queen";
                //case ChallengeMonsterKind.Ninetale:
                //    return "Ninetale";
                //case ChallengeMonsterKind.Octobaddie:
                //    return "Octobaddie";
        }
        return challengeMonsterKind.ToString();
    }
}

public class MonsterGlobalInformation
{
    public MonsterGlobalInformation()
    {
    }
    public MonsterGlobalInformation(MonsterSpecies species, MonsterColor color)
    {
        this.species = species;
        this.color = color;
        for (int i = 0; i < defeatedNums.Length; i++)
        {
            int count = i;
            defeatedNums[count] = new MonsterDefeatedNumber(species, color, (HeroKind)count);
            defeatedMutantNums[count] = new MonsterDefeatedNumber(species, color, (HeroKind)count, true);
            capturedNums[count] = new MonsterCapturedNumber(species, color, (HeroKind)count);
            capturedMutantNums[count] = new MonsterCapturedNumber(species, color, (HeroKind)count, true);
        }
        pet = new MonsterPet(this, species, color);
    }
    public void Start()
    {
        pet.Start();
    }

    //Pet
    public MonsterPet pet;

    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    int colorId { get { if (species == MonsterSpecies.ChallengeBoss) return (int)challengeMonsterKind; return (int)color; } }
    public Element AttackElement() { return monsterAttackElements[(int)species][colorId]; }
    public double Hp(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (color == MonsterColor.Metal) return monsterStats[(int)species][colorId][0] * level;
        double tempValue = monsterStats[(int)species][colorId][0];
        if (isPet)
        {
            tempValue *= pet.Level() + 1
            + 1.00d * Math.Pow(level / 5d, 2)
            ;
            tempValue *= 1d + 0.05d * pet.loyalty.value;
            tempValue *= game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).Mul();
        }
        else
        {
            tempValue *= level
            + 1.00d * Math.Pow(level / 5d, 2)
            + 2.50d * Math.Pow(level / 10d, 3)
            + 5.00d * Math.Pow(level / 20d, 4)
            + 25.00d * Math.Pow(level / 40d, 5)
            + 100.00d * Math.Pow(level / 80d, 8)
            + 1000.00d * Math.Pow(level / 120d, 10)
            + 50000.00d * Math.Pow(level / 200d, 15)
            + 250000.00d * Math.Pow(level / 300d, 20)//ver00020100以前は200000.00d
            ;
            tempValue *= Math.Pow(10, difficulty / 10d);
        }
        return Math.Floor(tempValue);
    }

    public double Mp(long level, double difficulty, bool isPet, HeroKind heroKind) { return 10; }
    public double Atk(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (color == MonsterColor.Metal) return monsterStats[(int)species][colorId][2] * level;
        double tempValue = monsterStats[(int)species][colorId][2];
        if (isPet)
        {
            tempValue *= (1d + pet.Level() * 0.75d);
            tempValue *= 1d + 0.05d * pet.loyalty.value;
            tempValue *= game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).Mul();
            tempValue *= game.statsCtrl.heroes[(int)heroKind].summonPetATKMATKMultiplier.Value();
        }
        else
        {
            tempValue *= 1d + level * 0.75d
                + 20.0d * Math.Pow(level / 100d, 3)
                + 100.0d * Math.Pow(level / 250d, 5)//ver00020200で追加
                ;
        }
        tempValue *= Math.Pow(2, difficulty / 10d);
        return tempValue;
    }
    public double MAtk(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (color == MonsterColor.Metal) return monsterStats[(int)species][colorId][3] * level;
        double tempValue = monsterStats[(int)species][colorId][3];
        if (isPet)
        {
            tempValue *= (1d + pet.Level() * 0.75d);
            tempValue *= 1d + 0.05d * pet.loyalty.value;
            tempValue *= game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).Mul();
        }
        else
        {
            tempValue *= 1d + level * 0.75d
                + 20.0d * Math.Pow(level / 100d, 3)
                + 100.0d * Math.Pow(level / 250d, 5)//ver00020200で追加
                ;
        }
        tempValue *= Math.Pow(2, difficulty / 10d);
        return tempValue;
    }
    public double Def(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (color == MonsterColor.Metal) return monsterStats[(int)species][colorId][4];
        double tempValue = monsterStats[(int)species][colorId][4];
        if (isPet)
        {
            tempValue *= pet.Level();
            tempValue *= 1d + 0.05d * pet.loyalty.value;
            tempValue *= game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).Mul();
        }
        else
        {
            tempValue *= level
                + 10.0d * Math.Pow(level / 100d, 3)
                + 50.0d * Math.Pow(level / 250d, 5)//ver00020200で追加
                ;
        }
        tempValue *= Math.Pow(2, difficulty / 10d);
        return tempValue;
    }
    public double MDef(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (color == MonsterColor.Metal) return monsterStats[(int)species][colorId][5];
        double tempValue = monsterStats[(int)species][colorId][5];
        if (isPet)
        {
            tempValue *= pet.Level();
            tempValue *= 1d + 0.05d * pet.loyalty.value;
            tempValue *= game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).Mul();
            tempValue *= game.statsCtrl.heroes[(int)heroKind].summonPetATKMATKMultiplier.Value();
        }
        else
        {
            tempValue *= level
                + 10.0d * Math.Pow(level / 100d, 3)
                + 50.0d * Math.Pow(level / 250d, 5)//ver00020200で追加
                ;
        }
        tempValue *= Math.Pow(2, difficulty / 10d);
        return tempValue;
    }
    public double Spd(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet)
            return monsterStats[(int)species][colorId][6]
                    * (1d + 0.05d * pet.loyalty.value)
                    * game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).Mul()
                    * game.statsCtrl.heroes[(int)heroKind].summonPetSPDMoveSpeedMultiplier.Value();
        return monsterStats[(int)species][colorId][6];
    }
    public double Fire(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return monsterStats[(int)species][colorId][7] * game.statsCtrl.HeroStats(heroKind, Stats.FireRes).Mul();
        return monsterStats[(int)species][colorId][7];
    }
    public double Ice(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return monsterStats[(int)species][colorId][8] * game.statsCtrl.HeroStats(heroKind, Stats.IceRes).Mul();
        return monsterStats[(int)species][colorId][8];
    }
    public double Thunder(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return monsterStats[(int)species][colorId][9] * game.statsCtrl.HeroStats(heroKind, Stats.ThunderRes).Mul();
        return monsterStats[(int)species][colorId][9];
    }
    public double Light(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return monsterStats[(int)species][colorId][10] * game.statsCtrl.HeroStats(heroKind, Stats.LightRes).Mul();
        return monsterStats[(int)species][colorId][10];
    }
    public double Dark(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return monsterStats[(int)species][colorId][11] * game.statsCtrl.HeroStats(heroKind, Stats.DarkRes).Mul();
        return monsterStats[(int)species][colorId][11];
    }
    public double PhyCrit(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return (0.01 * Math.Log(1 + pet.Level(), 2) + game.statsCtrl.heroes[(int)heroKind].summonPetCriticalChanceAdder.Value())
                * game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).Mul();
        return 0.01d * Math.Log(1 + level, 2);
    }
    public double MagCrit(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (isPet) return (0.01 * Math.Log(1 + pet.Level(), 2) + game.statsCtrl.heroes[(int)heroKind].summonPetCriticalChanceAdder.Value())
                * game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).Mul();
        return 0.01d * Math.Log(1 + level, 2);
    }
    public float Range()
    {
        if (species == MonsterSpecies.Mimic) return 10000;
        switch (AttackElement())
        {
            case Element.Physical:
                return 80;
            case Element.Fire:
                return 200;
            case Element.Ice:
                return 100;
            case Element.Thunder:
                return 300;
            case Element.Light:
                return 350;
            case Element.Dark:
                return 80;
            default:
                return 80;
        }
    }
    public double Damage(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        if (AttackElement() == Element.Physical) return Atk(level, difficulty, isPet, heroKind);
        else return MAtk(level, difficulty, isPet, heroKind);
    }
    public double AttackIntervalSec(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        return Mp(level, difficulty, isPet, heroKind) / Spd(level, difficulty, isPet, heroKind);
    }
    public float MoveSpeed(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        //Petの部分のSpdはfalse（petじゃない）にしてるため、Multiplierはかかってないから、この計算であってます！
        if (isPet) return (125f + 5 * (float)Spd(level, difficulty, false, heroKind)) * (float)game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).Mul() * (float)game.statsCtrl.heroes[(int)heroKind].summonPetSPDMoveSpeedMultiplier.Value();
        return 125f + 5 * (float)Spd(level, difficulty, isPet, heroKind);
    }
    public Debuff Debuff()
    {
        if (AttackElement() == Element.Thunder) return global::Debuff.Electric;
        if (AttackElement() == Element.Ice) return global::Debuff.SpdDown;
        switch (species)
        {
            case MonsterSpecies.Spider:
                return global::Debuff.Poison;
            case MonsterSpecies.Bat:
                if (color == MonsterColor.Red) return global::Debuff.Knockback;
                if (color == MonsterColor.Purple) return global::Debuff.Knockback;
                if (color == MonsterColor.Boss) return global::Debuff.Knockback;
                return global::Debuff.DefDown;
            case MonsterSpecies.Fairy:
                return global::Debuff.MdefDown;
            case MonsterSpecies.Fox:
                if (color == MonsterColor.Blue) return global::Debuff.ThunderResDown;
                if (color == MonsterColor.Purple) return global::Debuff.LightResDown;
                return global::Debuff.MatkDown;
            case MonsterSpecies.DevifFish:
                if (color == MonsterColor.Red) return global::Debuff.IceResDown;
                if (color == MonsterColor.Green) return global::Debuff.DarkResDown;
                return global::Debuff.AtkDown;
            //FlameTigerはFireResDown
        }
        return global::Debuff.Nothing;
    }
    public double DebuffChance(long level, double difficulty, bool isPet, HeroKind heroKind)
    {
        double tempValue = Math.Log(2 + level / 10d, 2) * (1 + 2 * Convert.ToInt16(color == MonsterColor.Boss));
        switch (Debuff())
        {
            case global::Debuff.Nothing:
                return 0;
            case global::Debuff.AtkDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.MatkDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.DefDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.MdefDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.SpdDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.Stop:
                tempValue *= 0.001d;//0.1%
                break;
            case global::Debuff.Electric:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.Poison:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.Death:
                tempValue *= 0.001d;//0.1%
                break;
            case global::Debuff.Knockback:
                tempValue *= 0.005d;//0.5%
                break;
            case global::Debuff.FireResDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.IceResDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.ThunderResDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.LightResDown:
                tempValue *= 0.01d;//1%
                break;
            case global::Debuff.DarkResDown:
                tempValue *= 0.01d;//1%
                break;
        }
        return tempValue;
    }
    public double Exp(long level, double difficulty)
    {
        return 20 +
            3 * (
            level
            + Math.Pow(level / 10d, 2)
            + Math.Pow(level / 50d, 3)
            + Math.Pow(level / 100d, 4)
            //+ 1.00d * Math.Pow(level / 60d, 5)
            )
            * Math.Pow(2, difficulty / 10d)
            * ColorFactor(color)
            * SpeciesFactor(species);
    }
    public double Gold(long level, double difficulty) { return Math.Log(1 + level, 2) * ColorFactor(color) * SpeciesFactor(species); }
    public MaterialKind dropSpeciesMaterial => MonsterParameter.Material(species);
    public MaterialKind dropColorMaterial => MonsterParameter.Material(color);
    public MonsterDefeatedNumber[] defeatedNums = new MonsterDefeatedNumber[Enum.GetNames(typeof(HeroKind)).Length];
    public MonsterDefeatedNumber[] defeatedMutantNums = new MonsterDefeatedNumber[Enum.GetNames(typeof(HeroKind)).Length];
    public MonsterCapturedNumber[] capturedNums = new MonsterCapturedNumber[Enum.GetNames(typeof(HeroKind)).Length];
    public MonsterCapturedNumber[] capturedMutantNums = new MonsterCapturedNumber[Enum.GetNames(typeof(HeroKind)).Length];
    public double DefeatedNum(bool isMutant = false)
    {
        double tempNum = 0;
        for (int i = 0; i < defeatedNums.Length; i++)
        {
            if (isMutant) tempNum += defeatedMutantNums[i].value;
            else tempNum += defeatedNums[i].value;
        }
        return tempNum;
    }
    public double CapturedNum(bool isMutant = false)
    {
        double tempNum = 0;
        for (int i = 0; i < capturedNums.Length; i++)
        {
            if (isMutant) tempNum += capturedMutantNums[i].value;
            else tempNum += capturedNums[i].value;
        }
        return tempNum;
    }
    public void CaptureAction(HeroKind heroKind, double capturedNum = 1, bool isMutant = false)
    {
        if (WithinRandom(game.monsterCtrl.captureTripleChance[(int)heroKind].Value()))
        {
            capturedNum *= 3;
            GameControllerUI.gameUI.logCtrlUI.Log("<color=orange>TRIPLE CAPTURE!");
        }
        capturedNums[(int)heroKind].Increase(capturedNum);
        if (isMutant) capturedMutantNums[(int)heroKind].Increase(capturedNum);
        pet.IncreaseTamingPoint(heroKind, capturedNum);
    }

    //UI
    public virtual string Name()//未
    {
        return Localized.localized.MonsterName(species, color);
    }
}

public class MonsterPet
{
    public MonsterPet(MonsterGlobalInformation globalInfo, MonsterSpecies species, MonsterColor color)
    {
        this.globalInfo = globalInfo;
        this.species = species;
        this.color = color;
        rank = new MonsterPetRank(species, color, MaxRank);
        level = new MonsterPetLevel(species, color, MaxLevel);
        exp = new MonsterPetExp(species, color, RequiredExp, level);
        loyalty = new MonsterPetLoyalty(species, color);
        loyaltyExp = new MonsterPetLoyaltyExp(species, color, LoyaltyRequiredExp, loyalty);
        tamingPoint = new MonsterPetTamingPoint(species, color, rank, RequiredTamingPoint);
    }
    public MonsterPet(MonsterGlobalInformation globalInfo, ChallengeMonsterKind challengeMonsterKind)
    {
        this.globalInfo = globalInfo;
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        rank = new MonsterPetRank(challengeMonsterKind);
        level = new MonsterPetLevel(challengeMonsterKind, MaxLevel);
        exp = new MonsterPetExp(challengeMonsterKind, RequiredExp, level);
        loyalty = new MonsterPetLoyalty(challengeMonsterKind);
        loyaltyExp = new MonsterPetLoyaltyExp(challengeMonsterKind, LoyaltyRequiredExp, loyalty);
        tamingPoint = new MonsterPetTamingPoint(challengeMonsterKind, rank, RequiredTamingPoint);
    }

    public void Start()
    {
        SetEffect();
        //levelTransaction = new Transaction(level, game.monsterCtrl.monsterMilk, (x) => 100 * Math.Pow(2, x / 10d));
    }

    public bool IsUnlocked()
    {
        return rank.value > 0;
    }
    public long Level()
    {
        return level.value;
    }
    public long MaxLevel()
    {
        return rank.value * 10;
    }
    public long MaxRank()
    {
        return 50;
    }
    public double RequiredExp(long level)
    {
        return Parameter.RequiredExp(level);
    }
    public double LoyaltyRequiredExp(long level)//level=Loyalty(max100)
    {
        return 1000 * Math.Pow(1 + level, 2);
    }
    //public double RequiredExp()
    //{
    //    return RequiredExp(level.value);
    //}
    //public double ExpPercent()
    //{
    //    return exp.value / RequiredExp();
    //}

    public bool IsSummon(HeroKind heroKind)
    {
        return game.monsterCtrl.IsSummonPet(heroKind, globalInfo);
    }
    public bool IsSummon()//いずれかのHeroがSummonしているかどうか
    {
        return game.monsterCtrl.IsSummonPet(globalInfo);
    }
    public bool SummonButtonInteractable(HeroKind heroKind)
    {
        if (IsSummon(heroKind)) return true;
        if (rank.value < 1) return false;
        if (IsExpedition()) return false;
        if (IsSummon()) return false;
        return game.monsterCtrl.CanSetSummonPet(heroKind);
    }
    public bool IsExpedition()
    {
        return game.monsterCtrl.IsExpeditionPet(globalInfo);
    }
    public bool CanSetExpedition()
    {
        if (IsSummon()) return false;
        if (rank.value < 1) return false;
        if (IsExpedition()) return false;
        return true;
    }
    //public bool LevelButtonInteractable()
    //{
    //    return levelTransaction.CanBuy();
    //}

    public MonsterGlobalInformation globalInfo;
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public MonsterPetRank rank;
    public MonsterPetLevel level;
    public MonsterPetExp exp;
    public MonsterPetLoyalty loyalty;
    public MonsterPetLoyaltyExp loyaltyExp;
    public MonsterPetTamingPoint tamingPoint;
    int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;
    public bool isActive { get => main.S.monsterPetIsActives[saveId]; set => main.S.monsterPetIsActives[saveId] = value; }
    public PetActiveEffectKind activeEffectKind { get => petActiveEffectKinds[(int)species][(int)color]; }
    public PetPassiveEffectKind passiveEffectKind { get => petPassiveEffectKinds[(int)species][(int)color]; }
    //public Transaction levelTransaction;

    public double BaseTamingPointGainPerCapture()
    {
        return (1 + Math.Log(1 + globalInfo.DefeatedNum() / 10000, 2));
    }
    public double TamingPointGainPerCapture(HeroKind heroKind)
    {
         return BaseTamingPointGainPerCapture() * game.statsCtrl.HeroStats(heroKind, Stats.TamingPointGain).Value();
    }
    public void IncreaseTamingPoint(HeroKind heroKind, double capturedNum = 1)
    {
        tamingPoint.Increase(TamingPointGainPerCapture(heroKind) * capturedNum);
    }
    public double RequiredTamingPoint()
    {
        return RequiredTamingPoint(rank.value);
    }
    public double RequiredTamingPoint(long rank)
    {
        return 10 * SpeciesFactor(species) * ColorFactor(color) * Math.Pow(1d + (int)color, 2d + 0.10d * (int)species) * Math.Pow(1.5d, rank);
    }
    public float TamingPointPercent()
    {
        return (float)(tamingPoint.value / RequiredTamingPoint());
    }
    public bool CanSwitchActive()
    {
        if (isActive)
        {
            //AutoRBに関して追加の条件
            if (species == MonsterSpecies.Fairy && color == MonsterColor.Normal)
            {
                if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier2))
                    return false;
                if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier3))
                    return false;
                if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier4))
                    return false;
                if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier5))
                    return false;
                if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier6))
                    return false;
            }
            return true;
        }
        if (!IsUnlocked()) return false;

        //AutoRBに関して追加の条件
        if (species == MonsterSpecies.Fox && color == MonsterColor.Green)
        {
            if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.RebirthTier1))
                return false;
        }
        //ここにTier3以降のPetを追加する

        if (game.monsterCtrl.PetActiveNum() >= game.monsterCtrl.petActiveCap.Value()) return false;
        return true;
    }
    public void SwitchActive()
    {
        if (!CanSwitchActive()) return;
        isActive = !isActive;
        game.monsterCtrl.UpdateIsPetActive();
        if (isActive) ActiveAction();
        else InactiveAction();
    }
    //ActiveEffect
    void ActiveAction()
    {
        switch (activeEffectKind)
        {
            case PetActiveEffectKind.GetResource:
                for (int i = 0; i < game.battleCtrls.Length; i++)
                {
                    game.battleCtrls[i].GetDropResource();
                }
                break;
            case PetActiveEffectKind.GetMaterial:
                for (int i = 0; i < game.battleCtrls.Length; i++)
                {
                    game.battleCtrls[i].GetDropMaterial();
                }
                break;
            case PetActiveEffectKind.GetEquipment:
                for (int i = 0; i < game.battleCtrls.Length; i++)
                {
                    game.battleCtrls[i].GetDropEquipment();
                }
                break;
        }
    }
    void InactiveAction()
    {
        switch (activeEffectKind)
        {
            case PetActiveEffectKind.UpgradeQueue:
                game.upgradeCtrl.AdjustAssignedQueue();
                break;
            case PetActiveEffectKind.AlchemyQueue:
                game.potionCtrl.AdjustAssignedQueue();
                break;
            case PetActiveEffectKind.DisassembleEquipment:
                game.equipmentCtrl.AdjustAssignedAutoDisassemble();
                break;
            //case PetActiveEffectKind.DisassembleTalismanCommon:
            //    game.shopCtrl.AdjustAutoBuyBlessingNum();
                //break;
            case PetActiveEffectKind.RebirthTier1:
                for (int i = 0; i < main.S.isAutoRebirthOns.Length; i++)
                {
                    main.S.isAutoRebirthOns[i] = false;
                }
                break;
            case PetActiveEffectKind.RebirthTier2:
                for (int i = 0; i < main.S.autoRebirthTiers.Length; i++)
                {
                    if (main.S.autoRebirthTiers[i] == 1)
                    {
                        GameControllerUI.gameUI.menuUI.MenuUI(MenuKind.Rebirth).GetComponent<RebirthMenuUI>().tierDropdown.value = 0;
                    }
                }
                break;
            case PetActiveEffectKind.RebirthTier3:
                for (int i = 0; i < main.S.autoRebirthTiers.Length; i++)
                {
                    if (main.S.autoRebirthTiers[i] == 2)
                        GameControllerUI.gameUI.menuUI.MenuUI(MenuKind.Rebirth).GetComponent<RebirthMenuUI>().tierDropdown.value = 0;
                }
                break;
            case PetActiveEffectKind.RebirthTier4:
                for (int i = 0; i < main.S.autoRebirthTiers.Length; i++)
                {
                    if (main.S.autoRebirthTiers[i] == 3)
                        GameControllerUI.gameUI.menuUI.MenuUI(MenuKind.Rebirth).GetComponent<RebirthMenuUI>().tierDropdown.value = 0;
                }
                break;
            case PetActiveEffectKind.RebirthTier5:
                for (int i = 0; i < main.S.autoRebirthTiers.Length; i++)
                {
                    if (main.S.autoRebirthTiers[i] == 4)
                        GameControllerUI.gameUI.menuUI.MenuUI(MenuKind.Rebirth).GetComponent<RebirthMenuUI>().tierDropdown.value = 0;
                }
                break;
            case PetActiveEffectKind.RebirthTier6:
                for (int i = 0; i < main.S.autoRebirthTiers.Length; i++)
                {
                    if (main.S.autoRebirthTiers[i] == 5)
                        GameControllerUI.gameUI.menuUI.MenuUI(MenuKind.Rebirth).GetComponent<RebirthMenuUI>().tierDropdown.value = 0;
                }
                break;

        }
    }

    //PassiveEffect
    void SetEffect()
    {
        switch (activeEffectKind)
        {
            case PetActiveEffectKind.UpgradeQueue:
                game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => 5, () => isActive));
                break;
            case PetActiveEffectKind.AlchemyQueue:
                game.potionCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => 10, () => isActive));
                break;
            case PetActiveEffectKind.DisassembleEquipment:
                game.equipmentCtrl.autoDisassembleAvailableNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => 10, () => isActive));
                break;
            //case PetActiveEffectKind.BuyBlessing:
            //    game.shopCtrl.autoBuyBlessingSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => 1, () => isActive));
            //    break;
        }
        switch (passiveEffectKind)
        {
            case PetPassiveEffectKind.ResourceGain:
                game.statsCtrl.SetEffectResourceGain(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.OilOfSlimeDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Slime].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.EnchantedClothDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.MagicSlime].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.SpiderSilkDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Spider].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.BatWingDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Bat].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.FairyDustDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Fairy].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.FoxTailDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Fox].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.FishScalesDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.DevifFish].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.CarvedBranchDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Treant].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.ThickFurDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.FlameTiger].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.UnicornHornDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Unicorn].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.PotionEffect:
                game.potionCtrl.effectMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.TamingPointGain:
                game.statsCtrl.SetEffect(Stats.TamingPointGain, new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.GoldCap:
                game.resourceCtrl.goldCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.GoldGain:
                game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.ExpGain:
                game.statsCtrl.SetEffect(Stats.ExpGain, new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.DoubleMaterialChance:
                game.materialCtrl.doubleMaterialChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.GoldGainOnDisassemblePotion:
                game.potionCtrl.disassembleGoldGainMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.DisassembleTownMatGain:
                game.equipmentCtrl.disassembleMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.TownMatGainFromDungeonReward:
                game.areaCtrl.townMaterialDungeonRewardMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.EquipProfGain:
                game.statsCtrl.SetEffect(Stats.EquipmentProficiencyGain, new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.MysteriousWaterGain:
                game.alchemyCtrl.mysteriousWaterProductionPerSec.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.ChestPortalOrbChance:
                game.areaCtrl.chestPortalOrbChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.SkillProfGain:
                game.statsCtrl.SetEffect(Stats.SkillProficiencyGain, new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
            case PetPassiveEffectKind.TownMatGain:
                game.townCtrl.SetEffectTownMatGain(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.ResearchPowerStone:
                game.townCtrl.researchPower[0].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.ResearchPowerCrystal:
                game.townCtrl.researchPower[1].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.ResearchPowerLeaf:
                game.townCtrl.researchPower[2].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.CatalystCriticalChance:
                game.catalystCtrl.criticalChanceMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Mul, () => effectValue));
                break;
            case PetPassiveEffectKind.MysteriousWaterCap:
                game.alchemyCtrl.maxMysteriousWaterExpandedCapNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => effectValue));
                break;
        }
    }
    public double effectValue => PetPassiveEffectValue(passiveEffectKind, rank.value) * (1d + loyalty.value / 100d);
    public double effectIncrementValue => (PetPassiveEffectValue(passiveEffectKind, rank.value + 1) - PetPassiveEffectValue(passiveEffectKind, rank.value)) * (1d + loyalty.value / 100d);

    public string ActiveEffectString()
    {
        return Localized.localized.PetActiveEffect(activeEffectKind);
    }
    public string PassiveEffectString()
    {
        switch (passiveEffectKind)
        {
            case PetPassiveEffectKind.ResourceGain:
                return "Resource Gain <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.OilOfSlimeDropChance:
                return Localized.localized.Material(MaterialKind.OilOfSlime) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.EnchantedClothDropChance:
                return Localized.localized.Material(MaterialKind.EnchantedCloth) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.SpiderSilkDropChance:
                return Localized.localized.Material(MaterialKind.SpiderSilk) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.BatWingDropChance:
                return Localized.localized.Material(MaterialKind.BatWing) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.FairyDustDropChance:
                return Localized.localized.Material(MaterialKind.FairyDust) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.FoxTailDropChance:
                return Localized.localized.Material(MaterialKind.FoxTail) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.FishScalesDropChance:
                return Localized.localized.Material(MaterialKind.FishScales) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.CarvedBranchDropChance:
                return Localized.localized.Material(MaterialKind.CarvedBranch) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.ThickFurDropChance:
                return Localized.localized.Material(MaterialKind.ThickFur) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.UnicornHornDropChance:
                return Localized.localized.Material(MaterialKind.UnicornHorn) + " Drop Chance <color=green>+ " + percent(effectValue, 3) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.PotionEffect:
                return "Potion Effect + <color=green>" + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue, 3) + " / Rank )";
            case PetPassiveEffectKind.TamingPointGain:
                return "Taming Point Gain + <color=green>" + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.GoldCap:
                return "Multiply Gold Cap by <color=green>" + percent(1 + effectValue) + "</color> ( + " + percent(effectIncrementValue,3) + " / Rank )";
            case PetPassiveEffectKind.GoldGain:
                return "Gold Gain + <color=green>" + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue,3) + " / Rank )";
            case PetPassiveEffectKind.ExpGain:
                return "EXP Gain + <color=green>" + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.DoubleMaterialChance:
                return "Lucky Material Chance for Doubled Material Gain : <color=green>" + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue,4) + " / Rank )";
            case PetPassiveEffectKind.GoldGainOnDisassemblePotion:
                return "Gold Gain on Disassembling Potions <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.DisassembleTownMatGain:
                return "Town Material Gain on Disassembling Equipment <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.TownMatGainFromDungeonReward:
                return "Town Material Gain from Dungeon Reward <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.EquipProfGain:
                return "Equipment Proficiency Gain <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.MysteriousWaterGain:
                return "Mysterious Water Gain <color=green>+ " + tDigit(effectValue, 3) + " / sec</color> ( + " + tDigit(effectIncrementValue, 3) + " / Rank )";
            case PetPassiveEffectKind.ChestPortalOrbChance:
                return "Finding Portal Orb from Chest Chance : <color=green>" + percent(effectValue, 4) + "</color> ( + " + percent(effectIncrementValue, 4) + " / Rank )";
            case PetPassiveEffectKind.SkillProfGain:
                return "Skill Proficiency Gain <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.TownMatGain:
                return "Multiply Town Material Gain by <color=green>" + percent(1 + effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.ResearchPowerStone:
                return "Stone Research Power <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue,3) + " / Rank )";
            case PetPassiveEffectKind.ResearchPowerCrystal:
                return "Crystal Research Power <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue,3) + " / Rank )";
            case PetPassiveEffectKind.ResearchPowerLeaf:
                return "Leaf Research Power <color=green>+ " + percent(effectValue) + "</color> ( + " + percent(effectIncrementValue,3) + " / Rank )";
            case PetPassiveEffectKind.Nothing:
                return "Stay tuned for future updates!";
            case PetPassiveEffectKind.CatalystCriticalChance:
                return "Multiplies Critical Chance of Catalyst by <color=green>" + percent(1 + effectValue) + "</color> ( + " + percent(effectIncrementValue) + " / Rank )";
            case PetPassiveEffectKind.MysteriousWaterCap:
                return "Max Mysterious Water Cap <color=green>+ " + tDigit(effectValue) + "</color> ( + " + tDigit(effectIncrementValue, 2) + " / Rank )";
        }
        return "";
    }
}

public class MonsterPetLoyalty : INTEGER
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override long value { get => main.S.monsterPetLoyalty[saveId]; set => main.S.monsterPetLoyalty[saveId] = value; }
    public MonsterPetLoyalty(MonsterSpecies species, MonsterColor color)//, Func<long> maxLevel)
    {
        this.species = species;
        this.color = color;
        maxValue = () => 100;
    }
    public MonsterPetLoyalty(ChallengeMonsterKind challengeMonsterKind)//, Func<long> maxLevel)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        maxValue = () => 100;
    }
}
public class MonsterPetLoyaltyExp : EXP
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override double value { get => main.S.monsterPetLoyaltyExp[saveId]; set => main.S.monsterPetLoyaltyExp[saveId] = value; }

    public MonsterPetLoyaltyExp(MonsterSpecies species, MonsterColor color, Func<long, double> requiredExp, INTEGER level)
    {
        this.species = species;
        this.color = color;
        requiredValue = requiredExp;
        this.level = level;
    }
    public MonsterPetLoyaltyExp(ChallengeMonsterKind challengeMonsterKind, Func<long, double> requiredExp, INTEGER level)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        requiredValue = requiredExp;
        this.level = level;
    }
}


public class MonsterPetLevel : INTEGER
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override long value { get => main.S.monsterPetLevels[saveId]; set => main.S.monsterPetLevels[saveId] = value; }
    public MonsterPetLevel(MonsterSpecies species, MonsterColor color, Func<long> maxLevel)
    {
        this.species = species;
        this.color = color;
        maxValue = maxLevel;
    }
    public MonsterPetLevel(ChallengeMonsterKind challengeMonsterKind, Func<long> maxLevel)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        maxValue = maxLevel;
    }

    //public Action<long> levelUpUIAction;
    //public override void Increase(long increment)
    //{
    //    base.Increase(increment);
    //    //if (levelUpUIAction != null) levelUpUIAction(increment);
    //}
}

public class MonsterPetExp : EXP
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override double value { get => main.S.monsterPetExps[saveId]; set => main.S.monsterPetExps[saveId] = value; }

    public MonsterPetExp(MonsterSpecies species, MonsterColor color, Func<long, double> requiredExp, INTEGER level)
    {
        this.species = species;
        this.color = color;
        requiredValue = requiredExp;
        this.level = level;
    }
    public MonsterPetExp(ChallengeMonsterKind challengeMonsterKind, Func<long, double> requiredExp, INTEGER level)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        requiredValue = requiredExp;
        this.level = level;
    }
}
public class MonsterPetRank : INTEGER
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override long value { get => main.S.monsterPetRanks[saveId]; set => main.S.monsterPetRanks[saveId] = value; }
    public MonsterPetRank(MonsterSpecies species, MonsterColor color, Func<long> maxValue)
    {
        this.maxValue = maxValue;
        this.species = species;
        this.color = color;
    }
    public MonsterPetRank(ChallengeMonsterKind challengeMonsterKind)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
    }
}
public class MonsterPetTamingPoint : EXP
{
    public MonsterSpecies species;
    public MonsterColor color;
    public ChallengeMonsterKind challengeMonsterKind;
    public int saveId => (int)color + 10 * (int)species + (int)challengeMonsterKind;//species==Chllengeの場合、color=0
    public override double value { get => main.S.monsterPetTamingPoints[saveId]; set => main.S.monsterPetTamingPoints[saveId] = value; }
    public MonsterPetTamingPoint(MonsterSpecies species, MonsterColor color, INTEGER rank, Func<long, double> requiredValue)
    {
        this.species = species;
        this.color = color;
        level = rank;
        this.requiredValue = requiredValue;
    }
    public MonsterPetTamingPoint(ChallengeMonsterKind challengeMonsterKind, INTEGER rank, Func<long, double> requiredValue)
    {
        this.species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        level = rank;
        this.requiredValue = requiredValue;
    }
}

public class MonsterDefeatedNumber : NUMBER
{
    public HeroKind heroKind;
    public MonsterSpecies species;
    public MonsterColor color;
    public bool isMutant;
    ChallengeMonsterKind challengeMonsterKind;
    int saveId => (int)color + 10 * (int)heroKind;
    int saveIdChallenge => (int)heroKind + 10 * (int)challengeMonsterKind;
    public MonsterDefeatedNumber(MonsterSpecies species, MonsterColor color, HeroKind heroKind, bool isMutant = false)
    {
        this.species = species;
        this.color = color;
        this.heroKind = heroKind;
        this.isMutant = isMutant;
    }
    public MonsterDefeatedNumber(ChallengeMonsterKind challengeMonsterKind, HeroKind heroKind, bool isMutant = false)
    {
        species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        this.isMutant = isMutant;
    }
    public override double value
    {
        get
        {
            switch (species)
            {
                case MonsterSpecies.Slime:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsSlime[saveId];
                    return main.SR.monsterDefeatedNumsSlime[saveId];
                case MonsterSpecies.MagicSlime:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsMagicSlime[saveId];
                    return main.SR.monsterDefeatedNumsMagicSlime[saveId];
                case MonsterSpecies.Spider:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsSpider[saveId];
                    return main.SR.monsterDefeatedNumsSpider[saveId];
                case MonsterSpecies.Bat:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsBat[saveId];
                    return main.SR.monsterDefeatedNumsBat[saveId];
                case MonsterSpecies.Fairy:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsFairy[saveId];
                    return main.SR.monsterDefeatedNumsFairy[saveId];
                case MonsterSpecies.Fox:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsFox[saveId];
                    return main.SR.monsterDefeatedNumsFox[saveId];
                case MonsterSpecies.DevifFish:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsDevilFish[saveId];
                    return main.SR.monsterDefeatedNumsDevilFish[saveId];
                case MonsterSpecies.Treant:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsTreant[saveId];
                    return main.SR.monsterDefeatedNumsTreant[saveId];
                case MonsterSpecies.FlameTiger:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsFlameTiger[saveId];
                    return main.SR.monsterDefeatedNumsFlameTiger[saveId];
                case MonsterSpecies.Unicorn:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsUnicorn[saveId];
                    return main.SR.monsterDefeatedNumsUnicorn[saveId];
                case MonsterSpecies.Mimic:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsMimic[saveId];
                    return main.SR.monsterDefeatedNumsMimic[saveId];
                case MonsterSpecies.ChallengeBoss:
                    if (isMutant) return main.SR.monsterMutantDefeatedNumsChallenge[saveIdChallenge];
                    return main.SR.monsterDefeatedNumsChallenge[saveIdChallenge];
                default:
                    return 0;
            }
        }
        set
        {
            switch (species)
            {
                case MonsterSpecies.Slime:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsSlime[saveId] = value;
                    else main.SR.monsterDefeatedNumsSlime[saveId] = value; break;
                case MonsterSpecies.MagicSlime:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsMagicSlime[saveId] = value;
                    else main.SR.monsterDefeatedNumsMagicSlime[saveId] = value; break;
                case MonsterSpecies.Spider:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsSpider[saveId] = value;
                    else main.SR.monsterDefeatedNumsSpider[saveId] = value; break;
                case MonsterSpecies.Bat:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsBat[saveId] = value;
                    else main.SR.monsterDefeatedNumsBat[saveId] = value; break;
                case MonsterSpecies.Fairy:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsFairy[saveId] = value;
                    else main.SR.monsterDefeatedNumsFairy[saveId] = value; break;
                case MonsterSpecies.Fox:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsFox[saveId] = value;
                    else main.SR.monsterDefeatedNumsFox[saveId] = value; break;
                case MonsterSpecies.DevifFish:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsDevilFish[saveId] = value;
                    else main.SR.monsterDefeatedNumsDevilFish[saveId] = value; break;
                case MonsterSpecies.Treant:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsTreant[saveId] = value;
                    else main.SR.monsterDefeatedNumsTreant[saveId] = value; break;
                case MonsterSpecies.FlameTiger:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsFlameTiger[saveId] = value;
                    else main.SR.monsterDefeatedNumsFlameTiger[saveId] = value; break;
                case MonsterSpecies.Unicorn:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsUnicorn[saveId] = value;
                    else main.SR.monsterDefeatedNumsUnicorn[saveId] = value; break;
                case MonsterSpecies.Mimic:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsMimic[saveId] = value;
                    else main.SR.monsterDefeatedNumsMimic[saveId] = value; break;
                case MonsterSpecies.ChallengeBoss:
                    if (isMutant) main.SR.monsterMutantDefeatedNumsChallenge[saveIdChallenge] = value;
                    else main.SR.monsterDefeatedNumsChallenge[saveIdChallenge] = value; break;
            }
        }
    }
}
public class SummonPet
{
    public HeroKind heroKind;
    public int id;
    public bool isSet { get => main.SR.isSetSummonPet[saveId]; set => main.SR.isSetSummonPet[saveId] = value; }
    public SummonPet(HeroKind heroKind, int id)
    {
        this.heroKind = heroKind;
        this.id = id;
    }
    int saveId => id + 10 * (int)heroKind;
    private MonsterSpecies species { get => main.SR.summonSpecies[saveId]; set => main.SR.summonSpecies[saveId] = value; }
    private MonsterColor color { get => main.SR.summonColor[saveId]; set => main.SR.summonColor[saveId] = value; }
    public MonsterPet pet => game.monsterCtrl.GlobalInformation(species, color).pet;
    public bool IsAvailable()
    {
        if (id >= game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value()) return false;
        //ここにExpeditionの条件
        return true;
    }
    public void SetPet(MonsterSpecies species, MonsterColor color)
    {

        isSet = true;
        this.species = species;
        this.color = color;
    }
    public void RemovePet()
    {
        isSet = false;
        this.species = MonsterSpecies.Slime;
        this.color = MonsterColor.Normal;
    }
}


public class MonsterCapturedNumber : NUMBER
{
    public HeroKind heroKind;
    public MonsterSpecies species;
    public MonsterColor color;
    ChallengeMonsterKind challengeMonsterKind;
    public bool isMutant;
    int saveId => (int)color + 10 * (int)heroKind;
    int saveIdChallenge => (int)heroKind + 10 * (int)challengeMonsterKind;
    public MonsterCapturedNumber(MonsterSpecies species, MonsterColor color, HeroKind heroKind, bool isMutant = false)
    {
        this.species = species;
        this.color = color;
        this.heroKind = heroKind;
        this.isMutant = isMutant;
    }
    public MonsterCapturedNumber(ChallengeMonsterKind challengeMonsterKind, HeroKind heroKind, bool isMutant = false)
    {
        species = MonsterSpecies.ChallengeBoss;
        this.challengeMonsterKind = challengeMonsterKind;
        this.isMutant = isMutant;
    }
    public override double value
    {
        get
        {
            switch (species)
            {
                case MonsterSpecies.Slime:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsSlime[saveId];
                    return main.SR.monsterCapturedNumsSlime[saveId];
                case MonsterSpecies.MagicSlime:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsMagicSlime[saveId];
                    return main.SR.monsterCapturedNumsMagicSlime[saveId];
                case MonsterSpecies.Spider:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsSpider[saveId];
                    return main.SR.monsterCapturedNumsSpider[saveId];
                case MonsterSpecies.Bat:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsBat[saveId];
                    return main.SR.monsterCapturedNumsBat[saveId];
                case MonsterSpecies.Fairy:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsFairy[saveId];
                    return main.SR.monsterCapturedNumsFairy[saveId];
                case MonsterSpecies.Fox:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsFox[saveId];
                    return main.SR.monsterCapturedNumsFox[saveId];
                case MonsterSpecies.DevifFish:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsDevilFish[saveId];
                    return main.SR.monsterCapturedNumsDevilFish[saveId];
                case MonsterSpecies.Treant:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsTreant[saveId];
                    return main.SR.monsterCapturedNumsTreant[saveId];
                case MonsterSpecies.FlameTiger:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsFlameTiger[saveId];
                    return main.SR.monsterCapturedNumsFlameTiger[saveId];
                case MonsterSpecies.Unicorn:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsUnicorn[saveId];
                    return main.SR.monsterCapturedNumsUnicorn[saveId];
                case MonsterSpecies.Mimic:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsMimic[saveId];
                    return main.SR.monsterCapturedNumsMimic[saveId];
                case MonsterSpecies.ChallengeBoss:
                    if (isMutant) return main.SR.monsterMutantCapturedNumsChallenge[saveIdChallenge];
                    return main.SR.monsterCapturedNumsChallenge[saveIdChallenge];
                default:
                    return 0;
            }
        }
        set
        {
            switch (species)
            {
                case MonsterSpecies.Slime:
                    if (isMutant) main.SR.monsterMutantCapturedNumsSlime[saveId] = value;
                    else main.SR.monsterCapturedNumsSlime[saveId] = value; break;
                case MonsterSpecies.MagicSlime:
                    if (isMutant) main.SR.monsterMutantCapturedNumsMagicSlime[saveId] = value;
                    else main.SR.monsterCapturedNumsMagicSlime[saveId] = value; break;
                case MonsterSpecies.Spider:
                    if (isMutant) main.SR.monsterMutantCapturedNumsSpider[saveId] = value;
                    else main.SR.monsterCapturedNumsSpider[saveId] = value; break;
                case MonsterSpecies.Bat:
                    if (isMutant) main.SR.monsterMutantCapturedNumsBat[saveId] = value;
                    else main.SR.monsterCapturedNumsBat[saveId] = value; break;
                case MonsterSpecies.Fairy:
                    if (isMutant) main.SR.monsterMutantCapturedNumsFairy[saveId] = value;
                    else main.SR.monsterCapturedNumsFairy[saveId] = value; break;
                case MonsterSpecies.Fox:
                    if (isMutant) main.SR.monsterMutantCapturedNumsFox[saveId] = value;
                    else main.SR.monsterCapturedNumsFox[saveId] = value; break;
                case MonsterSpecies.DevifFish:
                    if (isMutant) main.SR.monsterMutantCapturedNumsDevilFish[saveId] = value;
                    else main.SR.monsterCapturedNumsDevilFish[saveId] = value; break;
                case MonsterSpecies.Treant:
                    if (isMutant) main.SR.monsterMutantCapturedNumsTreant[saveId] = value;
                    else main.SR.monsterCapturedNumsTreant[saveId] = value; break;
                case MonsterSpecies.FlameTiger:
                    if (isMutant) main.SR.monsterMutantCapturedNumsFlameTiger[saveId] = value;
                    else main.SR.monsterCapturedNumsFlameTiger[saveId] = value; break;
                case MonsterSpecies.Unicorn:
                    if (isMutant) main.SR.monsterMutantCapturedNumsUnicorn[saveId] = value;
                    else main.SR.monsterCapturedNumsUnicorn[saveId] = value; break;
                case MonsterSpecies.Mimic:
                    if (isMutant) main.SR.monsterMutantCapturedNumsMimic[saveId] = value;
                    else main.SR.monsterCapturedNumsMimic[saveId] = value; break;
                case MonsterSpecies.ChallengeBoss:
                    if (isMutant) main.SR.monsterMutantCapturedNumsChallenge[saveIdChallenge] = value;
                    else main.SR.monsterCapturedNumsChallenge[saveIdChallenge] = value; break;
            }
        }
    }
}