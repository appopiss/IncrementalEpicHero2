using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using Cysharp.Threading.Tasks;
using static Main;

public partial class Save
{
    public double monsterMilk;
}
public class MonsterController
{
    public MonsterController()
    {
        //monsterMilk = new MonsterMilk();
        globalInformations = new MonsterGlobalInformation[Enum.GetNames(typeof(MonsterSpecies)).Length][];
        for (int i = 0; i < 1 + (int)MonsterSpecies.Unicorn; i++)
        {
            globalInformations[i] = new MonsterGlobalInformation[Enum.GetNames(typeof(MonsterColor)).Length];
            for (int j = 0; j < globalInformations[i].Length; j++)
            {
                globalInformations[i][j] = new MonsterGlobalInformation((MonsterSpecies)i, (MonsterColor)j);
                globalInfoList.Add(globalInformations[i][j]);
            }
        }
        //Mimic
        globalInformations[(int)MonsterSpecies.Mimic] = new MonsterGlobalInformation[1];
        globalInformations[(int)MonsterSpecies.Mimic][0] = new MonsterGlobalInformation(MonsterSpecies.Mimic, MonsterColor.Normal);
        globalInfoList.Add(globalInformations[(int)MonsterSpecies.Mimic][0]);
        //Challenge
        globalInformations[(int)MonsterSpecies.ChallengeBoss] = new MonsterGlobalInformation[Enum.GetNames(typeof(ChallengeMonsterKind)).Length];
        for (int i = 0; i < globalInformations[(int)MonsterSpecies.ChallengeBoss].Length; i++)
        {
            globalInformations[(int)MonsterSpecies.ChallengeBoss][i] = new ChallengeMonsterGlobalInformation((ChallengeMonsterKind)i);
            globalInfoList.Add(globalInformations[(int)MonsterSpecies.ChallengeBoss][i]);
        }

        for (int i = 0; i < speciesMaterialDropChance.Length; i++)
        {
            speciesMaterialDropChance[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => MonsterParameter.dropChanceBase));
        }
        colorMaterialDropChance = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => MonsterParameter.colorDropChanceBase));
        //monsterCaptureChance = new Multiplier();
        //tamingPointGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        for (int i = 0; i < captureTripleChance.Length; i++)
        {
            captureTripleChance[i] = new Multiplier();
        }
        for (int i = 0; i < monsterCapturableLevel.Length; i++)
        {
            int count = i;
            monsterCapturableLevel[i] = new Multiplier(() => 1000, () => 1);
            monsterCapturableLevel[i].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => game.statsCtrl.HeroLevel((HeroKind)count).value - 100));
            monsterCapturableLevel[i].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Title, MultiplierType.Add, () => game.statsCtrl.MonsterCaptureMaxLevelIncrement((HeroKind)count).Value()));
        }

        petActiveCap = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 5));
        trapNotConsumedChance = new Multiplier();

        //for (int i = 0; i < summonPetArray.Length; i++)
        //{
        //    for (int j = 0; j < summonPetArray[i].Length; j++)
        //    {
        //        summonPetArray[i][j] = 
        //    }
        //}

        UpdateIsPetActive();
    }
    public void Start()
    {
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            globalInfoList[i].Start();
        }
    }

    public Multiplier petActiveCap;
    public int PetActiveNum()
    {
        int tempNum = 0;
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            if (globalInfoList[i].pet.isActive) tempNum++;
        }
        return tempNum;
    }
    public void CheckPetActiveNum()
    {
        petActiveCap.Calculate();
        if (PetActiveNum() <= petActiveCap.Value()) return;
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            if (globalInfoList[i].pet.isActive) globalInfoList[i].pet.SwitchActive();
            if (PetActiveNum() <= petActiveCap.Value()) return;
        }
    }

    MonsterGlobalInformation[][] globalInformations;
    public MonsterGlobalInformation Monster(MonsterSpecies species, MonsterColor color) { return GlobalInformation(species, color); }
    public MonsterGlobalInformation GlobalInformation(MonsterSpecies species, MonsterColor color)
    {
        return globalInformations[(int)species][(int)color];
    }
    public MonsterGlobalInformation GlobalInformationChallengeBoss(ChallengeMonsterKind kind)
    {
        return globalInformations[(int)MonsterSpecies.ChallengeBoss][(int)kind];
    }
    public List<MonsterGlobalInformation> globalInfoList = new List<MonsterGlobalInformation>();
    public Multiplier[] speciesMaterialDropChance = new Multiplier[Enum.GetNames(typeof(MonsterSpecies)).Length];
    public Multiplier colorMaterialDropChance;
    //public Multiplier monsterCaptureChance;
    public Multiplier trapNotConsumedChance;
    public Multiplier[] monsterCapturableLevel = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    //public Multiplier tamingPointGainMultiplier;
    public Multiplier[] captureTripleChance = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public double TotalDefeatedNum()
    {
        double tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(MonsterSpecies)).Length; i++)
        {
            int count = i;
            tempNum += TotalDefeatedNum((MonsterSpecies)count);
        }
        return tempNum;
    }
    public double TotalDefeatedNum(MonsterSpecies species)
    {
        double tempNum = 0;
        for (int i = 0; i < globalInformations[(int)species].Length; i++)
        {
            tempNum += globalInformations[(int)species][i].DefeatedNum();
        }
        return tempNum;
    }
    public double CapturedNum()
    {
        double tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(MonsterSpecies)).Length; i++)
        {
            int count = i;
            tempNum += CapturedNum((MonsterSpecies)count);
        }
        return tempNum;
    }
    public double CapturedNum(MonsterSpecies species)
    {
        double tempNum = 0;
        for (int i = 0; i < globalInformations[(int)species].Length; i++)
        {
            tempNum += globalInformations[(int)species][i].CapturedNum();
        }
        return tempNum;
    }

    //Pet
    //public MonsterMilk monsterMilk;
    private bool[] isPetActive = new bool[Enum.GetNames(typeof(PetActiveEffectKind)).Length];
    private bool[] tempIsPetActive = new bool[Enum.GetNames(typeof(PetActiveEffectKind)).Length];
    public void UpdateIsPetActive()
    {
        for (int i = 0; i < tempIsPetActive.Length; i++)
        {
            tempIsPetActive[i] = false;
        }
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            if (globalInfoList[i].pet.isActive) tempIsPetActive[(int)globalInfoList[i].pet.activeEffectKind] = true;
        }
        for (int i = 0; i < isPetActive.Length; i++)
        {
            isPetActive[i] = tempIsPetActive[i];
        }
    }
    public bool IsPetActiveEffectKind(PetActiveEffectKind kind)
    {
        return isPetActive[(int)kind];
    }
    //PetSummon
    public bool CanSetSummonPet(HeroKind heroKind)
    {
        for (int i = 0; i < game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value(); i++)
        {
            if (!game.statsCtrl.heroes[(int)heroKind].summonPets[i].isSet)
                return true;
        }
        return false;
    }
    public void SetOrRemoveSummonPet(HeroKind heroKind, MonsterGlobalInformation globalInfo)
    {
        if (globalInfo.pet.IsSummon(heroKind)) RemoveSummonPet(heroKind, globalInfo);
        else SetSummonPet(heroKind, globalInfo);
    }
    public void SetSummonPet(HeroKind heroKind, MonsterGlobalInformation globalInfo)
    {
        for (int i = 0; i < game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value(); i++)
        {
            if (!game.statsCtrl.heroes[(int)heroKind].summonPets[i].isSet)
            {
                game.statsCtrl.heroes[(int)heroKind].summonPets[i].SetPet(globalInfo.species, globalInfo.color);
                return;
            }
        }
    }
    public void RemoveSummonPet(HeroKind heroKind, MonsterGlobalInformation globalInfo)
    {
        for (int i = 0; i < game.statsCtrl.heroes[(int)heroKind].summonPets.Length; i++)
        {
            if (game.statsCtrl.heroes[(int)heroKind].summonPets[i].isSet && game.statsCtrl.heroes[(int)heroKind].summonPets[i].pet == globalInfo.pet)
            {
                game.statsCtrl.heroes[(int)heroKind].summonPets[i].RemovePet();
                return;
            }
        }
    }
    public bool IsSummonPet(HeroKind heroKind, MonsterGlobalInformation globalInfo)
    {
        for (int i = 0; i < game.statsCtrl.heroes[(int)heroKind].summonPets.Length; i++)
        {
            if (i < game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value() && game.statsCtrl.heroes[(int)heroKind].summonPets[i].isSet && game.statsCtrl.heroes[(int)heroKind].summonPets[i].pet == globalInfo.pet)
                return true;
        }
        return false;
    }
    //いずれかのHeroがSummonしているかどうか
    public bool IsSummonPet(MonsterGlobalInformation globalInfo)
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            HeroKind heroKind = (HeroKind)i;
            if (IsSummonPet(heroKind, globalInfo)) return true;
        }
        return false;
    }
    public int CurrentSummonPetNum(HeroKind heroKind)
    {
        int tempNum = 0;
        for (int i = 0; i < game.statsCtrl.heroes[(int)heroKind].summonPets.Length; i++)
        {
            if (game.statsCtrl.heroes[(int)heroKind].summonPets[i].isSet)
                tempNum++;
        }
        return (int)Math.Min(game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value(), tempNum);
    }
    public bool IsExpeditionPet(MonsterGlobalInformation globalInfo)
    {
        Expedition expedition;
        for (int i = 0; i < game.expeditionCtrl.expeditions.Length; i++)
        {
            expedition = game.expeditionCtrl.expeditions[i];
            if (expedition.unlock.IsUnlocked())
            {
                for (int j = 0; j < expedition.pets.Length; j++)
                {
                    if (expedition.pets[j].isSet && expedition.pets[j].pet == globalInfo.pet) return true;
                }
            }
        }
        return false;
    }
    //public int MonsterSummonNum(HeroKind heroKind)
    //{
    //    int tempNum = 0;
    //    for (int i = 0; i < globalInfoList.Count; i++)
    //    {
    //        if (globalInfoList[i].pet.isSummon[(int)heroKind].isSummon) tempNum++;
    //    }
    //    return tempNum;
    //}
    //public bool CanSummon(HeroKind heroKind)
    //{
    //    return MonsterSummonNum(heroKind) < game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value();
    //}
    //public void AdjustSummonNum(HeroKind heroKind)
    //{
    //    for (int i = 0; i < globalInfoList.Count; i++)
    //    {
    //        if (MonsterSummonNum(heroKind) <= game.statsCtrl.heroes[(int)heroKind].summonPetSlot.Value())
    //            return;
    //        globalInfoList[i].pet.isSummon[(int)heroKind].isSummon = false;
    //    }
    //}
    //public MonsterPet[][] summonPetArray = new MonsterPet[Enum.GetNames(typeof(HeroKind)).Length][];

    //public async void PetAutoActiveEffect()
    //{
    //    //await UniTask.DelayFrame(5);
    //    game.upgradeCtrl.BuyByQueue();
    //    await UniTask.DelayFrame(5);
    //    game.alchemyCtrl.AutoExpandCap();
    //    await UniTask.DelayFrame(5);
    //    //game.questCtrl.AutoAccept();
    //    //await UniTask.DelayFrame(5);
    //    //game.questCtrl.AutoClaim();
    //    //await UniTask.DelayFrame(5);
    //    game.potionCtrl.BuyByQueue();
    //    await UniTask.DelayFrame(5);
    //    game.shopCtrl.AutoBuy();
    //    await UniTask.DelayFrame(5);
    //    game.skillCtrl.AutoRankup();
    //    await UniTask.DelayFrame(5);
    //    game.rebirthCtrl.AutoRebirth();
    //    await UniTask.DelayFrame(5);
    //    game.rebirthCtrl.AutoRebirthUpgradeExp();
    //    await UniTask.DelayFrame(5);
    //    game.statsCtrl.AutoAddAbilityPoint();
    //}

}

//public class MonsterMilk : NUMBER
//{
//    public override double value { get => main.S.monsterMilk; set => main.S.monsterMilk = value; }
//    public override string Name()
//    {
//        return "Monster Milk";
//    }
//}