using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static DictionaryParameter;
using static EquipmentParameter;
using static UsefulMethod;

public partial class Save
{
    public double totalEquipmentGained;
}
public partial class SaveR
{
    public double dictionaryUpgradePoint;
    public long[] dictionaryUpgradeLevels;
}

public class EquipmentController
{
    public EquipmentController()
    {
        for (int i = 0; i < globalInformations.Length; i++)
        {
            globalInformations[i] = new EquipmentGlobalInformation((EquipmentKind)i);
        }
        for (int i = 0; i < canCrafts.Length; i++)
        {
            canCrafts[i] = new Unlock();
        }

        //Dictionary
        dictionaryUpgradeEffectMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, ()=>1));
        for (int i = 0; i < equipments.Length; i++)
        {
            equipments[i] = new DictionaryEquipment((EquipmentKind)i);
        }
        dictionaryPointLeft = new DictionaryUpgradePointLeft();
        for (int i = 0; i < dictionaryUpgrades.Length; i++)
        {
            dictionaryUpgrades[i] = new DictionaryUpgrade(this, (DictionaryUpgradeKind)i, dictionaryPointLeft);
        }
        for (int i = 0; i < maxLevels.Length; i++)
        {
            maxLevels[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => maxLevelForEachHero));
        }
        effectMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        autoDisassembleAvailableNum = new Multiplier();
        disassembleMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        for (int i = 0; i < Enum.GetNames(typeof(EquipmentSetKind)).Length; i++)
        {
            int count = i;
            setItemArray[i] = SetItemList((EquipmentSetKind)count).ToArray();
        }
    }
    public void Start()
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            equipments[i].Start();
        }
    }
    public EquipmentGlobalInformation[] globalInformations = new EquipmentGlobalInformation[Enum.GetNames(typeof(EquipmentKind)).Length];

    public EquipmentGlobalInformation GlobalInfo(EquipmentKind kind)
    {
        if(globalInformations.Length < 1) { Debug.Log("Error"); return null; }
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (globalInformations[i].kind == kind) return globalInformations[i];
        }
        return globalInformations[0];
    }

    public EquipmentKind[][] setItemArray = new EquipmentKind[Enum.GetNames(typeof(EquipmentSetKind)).Length][];
    List<EquipmentKind> SetItemList(EquipmentSetKind kind)
    {
        List<EquipmentKind> tempList = new List<EquipmentKind>();
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (globalInformations[i].setKind == kind)
                tempList.Add(globalInformations[i].kind);
        }
        return tempList;
    }


    public int LevelMaxedNum(HeroKind heroKind, EquipmentPart part, long requiredLevel = 10)
    {
        int tempNum = 0;
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (globalInformations[i].part == part)
            {
                if (globalInformations[i].levels[(int)heroKind].value >= requiredLevel) tempNum++;
            }
        }
        return tempNum;
    }
    public int TotalLevelMaxedNum(HeroKind heroKind)
    {
        int tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(EquipmentPart)).Length; i++)
        {
            int count = i;
            tempNum += LevelMaxedNum(heroKind, (EquipmentPart)count);
        }
        return tempNum;
    }
    public int TotalLevelMaxedNum()
    {
        int tempNum = 0;
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            tempNum += TotalLevelMaxedNum((HeroKind)count);
        }
        return tempNum;
    }

    public double UniqueSetIsGotNum(EquipmentSetKind setKind)
    {
        int tempNum = 0;
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (globalInformations[i].setKind == setKind && globalInformations[i].isGotOnce) tempNum++;
        }
        return tempNum;
    }

    //Dictionary用
    public Multiplier dictionaryUpgradeEffectMultiplier;
    public DictionaryEquipment[] equipments = new DictionaryEquipment[Enum.GetNames(typeof(EquipmentKind)).Length];
    public DictionaryUpgradePointLeft dictionaryPointLeft;
    public void GetDictionaryPoint(EquipmentKind equipmentKind)
    {
        dictionaryPointLeft.Increase(PointIncrement(Rarity(equipmentKind)));
    }
    double PointIncrement(EquipmentRarity rarity) { return 1 + (int)rarity; }
    public DictionaryUpgrade[] dictionaryUpgrades = new DictionaryUpgrade[Enum.GetNames(typeof(DictionaryUpgradeKind)).Length];
    public DictionaryUpgrade DictionaryUpgrade(DictionaryUpgradeKind kind) { return dictionaryUpgrades[(int)kind]; }
    public double DictionaryTotalPoint()
    {
        double tempPoint = dictionaryPointLeft.value;
        for (int i = 0; i < dictionaryUpgrades.Length; i++)
        {
            tempPoint += dictionaryUpgrades[i].transaction.TotalCostConsumed();
        }
        //ver0.1.2.12 RB3を実装したので、この計算では合わなくなった
        //double tempPoint = 0;
        //for (int i = 0; i < globalInformations.Length; i++)
        //{
        //    for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
        //    {
        //        if (globalInformations[i].levels[j].IsMaxed()) tempPoint += PointIncrement(globalInformations[i].rarity);
        //    }
        //}
        return tempPoint;
    }
    public void ResetDictionaryUpgrade()
    {
        dictionaryPointLeft.ChangeValue(DictionaryTotalPoint());
        for (int i = 0; i < dictionaryUpgrades.Length; i++)
        {
            dictionaryUpgrades[i].level.ChangeValue(0);
        }
    }

    //effect
    public Multiplier effectMultiplier;
    public Multiplier[] maxLevels = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];//[HeroKind]
    //craft
    public Unlock[] canCrafts = new Unlock[Enum.GetNames(typeof(EquipmentRarity)).Length];

    //Disassemble
    public Multiplier disassembleMultiplier;
    public Multiplier autoDisassembleAvailableNum;
    public int AutoDisassembleActiveNum()
    {
        int tempNum = 0;
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (globalInformations[i].isAutoDisassemble) tempNum++;
        }
        return tempNum;
    }
    public bool CanAssignAutoDisassemble()
    {
        return AutoDisassembleActiveNum() < autoDisassembleAvailableNum.Value();
    }
    public void AdjustAssignedAutoDisassemble()
    {
        for (int i = 0; i < globalInformations.Length; i++)
        {
            if (AutoDisassembleActiveNum() <= autoDisassembleAvailableNum.Value()) return;
            globalInformations[globalInformations.Length - i - 1].isAutoDisassemble = false;
        }
    }
    public void RemoveAllAutoDisassemble()
    {
        for (int i = 0; i < globalInformations.Length; i++)
        {
            globalInformations[i].isAutoDisassemble = false;
        }
    }
}

public class EquipmentGenerator : DROP_GENERATOR
{
    public EquipmentGenerator(HeroKind heroKind) : base(heroKind)
    {
    }

    public void Drop(long monsterLevel, Vector2 position)
    {
        main.S.totalEquipmentGained++;
        this.monsterLevel = monsterLevel;
        LotteryEquipment(monsterLevel);
        LotteryOptionNum();
        if (isAutoGet)
        {
            if (TryGet()) return;
        }
        if (position.x > moveRangeX / 2f || position.x < -moveRangeX / 2f || position.y > moveRangeY / 2f - 50f || position.y < -moveRangeY / 2f)
            position = Vector2.zero;
        this.position = position;
        if (game.IsUI(heroKind) && dropUIAction != null) dropUIAction();
    }
    //UniqueDrop
    public void Drop(EquipmentKind kind, long monsterLevel, Vector2 position)
    {
        if (kind == EquipmentKind.Nothing) return;
        main.S.totalEquipmentGained++;
        this.monsterLevel = monsterLevel;
        this.kind = kind;
        LotteryOptionNum();
        if (isAutoGet)
        {
            if (TryGet()) return;
        }
        if (position.x > moveRangeX / 2f || position.x < -moveRangeX / 2f || position.y > moveRangeY / 2f - 50f || position.y < -moveRangeY / 2f)
            position = Vector2.zero;
        this.position = position;
        if (game.IsUI(heroKind) && dropUIAction != null) dropUIAction();
    }
    void LotteryEquipment(long monsterLevel)
    {
        //EQ決定
        List<EquipmentKind> lotteryEquipmentList = new List<EquipmentKind>();
        for (int i = 1; i < game.equipmentCtrl.globalInformations.Length; i++)//i=1からなのは、Nothingを除外するため
        {
            if (game.equipmentCtrl.globalInformations[i].setKind == EquipmentSetKind.Nothing && game.equipmentCtrl.globalInformations[i].requiredAbilities[0].requiredValue <= monsterLevel)
                lotteryEquipmentList.Add(game.equipmentCtrl.globalInformations[i].kind);
        }
        if (lotteryEquipmentList.Count <= 0) return;
        double[] tempChanceArray = new double[lotteryEquipmentList.Count];
        for (int i = 0; i < tempChanceArray.Length; i++)
        {
            tempChanceArray[i] = 1;
        }
        Lottery lottery = new Lottery(tempChanceArray);
        kind = lotteryEquipmentList[lottery.SelectedId()];
    }
    void LotteryOptionNum()
    {
        //Option数の決定
        int temp = UnityEngine.Random.Range(0, Parameter.randomAccuracy);
        if (temp < game.statsCtrl.OptionEffectChance(heroKind, 2).Value() * Parameter.randomAccuracy) optionNum = 3;
        else if (temp < game.statsCtrl.OptionEffectChance(heroKind, 1).Value() * Parameter.randomAccuracy) optionNum = 2;
        else if (temp < game.statsCtrl.OptionEffectChance(heroKind, 0).Value() * Parameter.randomAccuracy) optionNum = 1;
        else optionNum = 0;
    }

    //獲得する時（拾った時）に呼ぶ
    public bool TryGet()
    {
        //AutoDisassemble
        if (game.equipmentCtrl.globalInformations[(int)kind].isAutoDisassemble && !isExcludeEnchant)
        {
            game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialKind().Increase(game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum());
            main.SR.disassembledEquipmentNums[(int)kind]++;
            main.S.disassembledEquipmentNums[(int)kind]++;
            main.SR.townMatGainFromdisassemble[(int)kind] += game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum();
            main.S.townMatGainFromdisassemble[(int)kind] += game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum();
            Initialize();
            return true;
        }

        if (!game.inventoryCtrl.CanCreateEquipment()) return false;
        game.inventoryCtrl.CreateEquipment(kind, optionNum, monsterLevel);
        Initialize();
        return true;
    }
    //public override void Get()
    //{
    //    //AutoDisassemble
    //    if (game.equipmentCtrl.globalInformations[(int)kind].isAutoDisassemble && !isExcludeEnchant)
    //    {
    //        game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialKind().Increase(game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum());
    //        main.SR.disassembledEquipmentNums[(int)kind]++;
    //        main.S.disassembledEquipmentNums[(int)kind]++;
    //        main.SR.townMatGainFromdisassemble[(int)kind] += game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum();
    //        main.S.townMatGainFromdisassemble[(int)kind] += game.equipmentCtrl.globalInformations[(int)kind].DisassembleMaterialNum();
    //        Initialize();
    //        return;
    //    }

    //    if (!game.inventoryCtrl.CanCreateEquipment()) return;
    //    game.inventoryCtrl.CreateEquipment(kind, optionNum, monsterLevel);
    //    Initialize();
    //}
    bool isExcludeEnchant => optionNum > 0 && main.S.isToggleOn[(int)ToggleKind.AutoDisassembleExcludeEnchanted];
    public override void Initialize()
    {
        kind = EquipmentKind.Nothing;
        optionNum = 0;
        position = Parameter.hidePosition;
        if (game.IsUI(heroKind) && initializeUIAction != null) initializeUIAction();
    }

    public EquipmentKind kind;
    public int optionNum;
    long monsterLevel;
    public override bool isAutoGet => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetEquipment);
}

public class DictionaryUpgradePointLeft : NUMBER
{
    public override double value { get => main.SR.dictionaryUpgradePoint; set => main.SR.dictionaryUpgradePoint = value; }
}

public class DictionaryUpgrade
{
    public DictionaryUpgrade(EquipmentController equipmentCtrl, DictionaryUpgradeKind kind, DictionaryUpgradePointLeft pointLeft)
    {
        this.equipmentCtrl = equipmentCtrl;
        this.kind = kind;
        unlock = new Unlock();
        level = new DictionaryUpgradeLevel(kind, () => MaxLevel(kind));
        transaction = new Transaction(level, pointLeft, () => UpgradeCost(kind).initCost, () => UpgradeCost(kind).baseCost, UpgradeCost(kind).isLinier);
        transaction.SetAdditionalBuyCondition(unlock.IsUnlocked);
        SetEffect();
    }
    public Unlock unlock;
    public double effectValue { get => EffectValue(kind, level.value) * equipmentCtrl.dictionaryUpgradeEffectMultiplier.Value(); }
    public double nextEffectValue { get => EffectValue(kind, transaction.ToLevel()) * equipmentCtrl.dictionaryUpgradeEffectMultiplier.Value(); }
    void SetEffect()
    {
        switch (kind)
        {
            case DictionaryUpgradeKind.EquipmentProficiencyGainWarrior:
                game.statsCtrl.HeroStats(HeroKind.Warrior, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentProficiencyGainWizard:
                game.statsCtrl.HeroStats(HeroKind.Wizard, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentProficiencyGainAngel:
                game.statsCtrl.HeroStats(HeroKind.Angel, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentProficiencyGainThief:
                game.statsCtrl.HeroStats(HeroKind.Thief, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentProficiencyGainArcher:
                game.statsCtrl.HeroStats(HeroKind.Archer, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentProficiencyGainTamer:
                game.statsCtrl.HeroStats(HeroKind.Tamer, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EquipmentDropChance:
                game.statsCtrl.SetEffect(Stats.EquipmentDropChance, new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                break;
            case DictionaryUpgradeKind.EnchantedEffectChance1:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    game.statsCtrl.OptionEffectChance((HeroKind)i, 0).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                }
                break;
            case DictionaryUpgradeKind.EnchantedEffectChance2:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    game.statsCtrl.OptionEffectChance((HeroKind)i, 1).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                }
                break;
            case DictionaryUpgradeKind.EnchantedEffectChance3:
                for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
                {
                    game.statsCtrl.OptionEffectChance((HeroKind)i, 2).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Dictionary, MultiplierType.Add, () => effectValue));
                }
                break;
        }
    }

    EquipmentController equipmentCtrl;
    public DictionaryUpgradeKind kind;
    public DictionaryUpgradeLevel level;
    public Transaction transaction;
}
public class DictionaryUpgradeLevel : INTEGER
{
    public DictionaryUpgradeLevel(DictionaryUpgradeKind kind, Func<long> maxValue)
    {
        this.kind = kind;
        this.maxValue = maxValue;
    }
    DictionaryUpgradeKind kind;
    public override long value { get => main.SR.dictionaryUpgradeLevels[(int)kind]; set => main.SR.dictionaryUpgradeLevels[(int)kind] = value; }
}
public enum DictionaryUpgradeKind
{
    EquipmentProficiencyGainWarrior,
    EquipmentProficiencyGainWizard,
    EquipmentProficiencyGainAngel,
    EquipmentProficiencyGainThief,
    EquipmentProficiencyGainArcher,
    EquipmentProficiencyGainTamer,
    EquipmentDropChance,
    EnchantedEffectChance1,
    EnchantedEffectChance2,
    EnchantedEffectChance3,
}