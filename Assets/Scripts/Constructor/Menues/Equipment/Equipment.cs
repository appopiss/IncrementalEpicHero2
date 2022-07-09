using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static InventoryParameter;
using static UsefulMethod;

public partial class SaveR
{
    public EquipmentKind[] equipmentKinds;//[equipmentId]
    public int[] equipmentOptionNums;//[equipmentId]オプションの個数（Enchant Slotを含む）
    public bool[] equipmentIsLocked;//[equipmentId]
}

public class DictionaryEquipment : Equipment
{
    public DictionaryEquipment(EquipmentKind kind)
    {
        thisKind = kind;
    }
    public void Start()
    {
        craftTransaction = new Transaction(new INTEGER(), globalInfo.DisassembleMaterialKind(), (x) => globalInfo.CraftCostMaterialNum());
        craftTransaction.SetAdditionalBuyCondition(() => CanCraft() && game.inventoryCtrl.CanCreateEquipment());
        //craftTransaction.isBuyOne = true;
        craftTransaction.additionalBuyActionWithLevelIncrement = (x) => game.inventoryCtrl.CreateEquipment(thisKind, 0, 1);
        craftTransaction.SetAdditionalBuyCondition((num) =>
        {
            //EpicStoreを購入していて、かつAuto-DisassembleOnCraftがONの場合は複数作れる
            if (game.epicStoreCtrl.Item(EpicStoreKind.CraftUnlimitedDisassemble).IsPurchased() && game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.AutoCraftDisassembleEQ))
                return true;
            if (num == 1) return true;
            return false;
        }
        );
        //後に、EnchantSlotありの装備をクラフトする場合はここ↑をいじる
    }
    EquipmentKind thisKind;
    public override EquipmentKind kind { get => thisKind; }
    public override int optionNum { get => 0;}
}

public class Equipment
{
    //Craft
    public Transaction craftTransaction;
    public bool CanCraft() { return game.equipmentCtrl.canCrafts[(int)globalInfo.rarity].IsUnlocked() && globalInfo.isGotOnce; }

    public Equipment(int id = 0)
    {
        this.id = id;
        for (int i = 0; i < optionEffects.Length; i++)
        {
            optionEffects[i] = new EquipmentOptionEffect(id, i);
        }
        totalOptionNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => optionNum));
    }

    int slotId;
    public void UpdateSlotId(int slotId)
    {
        this.slotId = slotId;
    }
    //新規作成
    public void CreateNew(EquipmentKind kind, int optionNum, long monsterLevel)
    {
        Delete();//念の為呼ぶ
        createId++;//新規EQとして効果を登録するため
        this.kind = kind;
        this.optionNum = optionNum;
        //Option追加
        for (int i = 0; i < optionEffects.Length; i++)
        {
            optionEffects[i].SetInfo(id, i);
            //if (i < totalOptionNum.Value())//totalOptionNumにアクセスすると、計算が間に合わなくて正しい値が帰ってこない
            if (i < optionNum)//よって、これが正解
                optionEffects[i].CreateNew(monsterLevel, createId);
        }
        //Effectを適用
        SetEffect();
    }
    //エンチャント
    //オプション追加
    public void OptionAdd(EquipmentEffectKind effectKind, long level)
    {
        for (int i = 0; i < totalOptionNum.Value(); i++)
        {
            int count = i;
            if (optionEffects[count].CanEnchant())
            {
                optionEffects[count].Enchant(effectKind, level, createId);
                for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
                {
                    SetEffect((HeroKind)j, optionEffects[count].kind, () => optionEffects[count].effectValue, optionEffects[count].createId);
                }
                return;
            }
        }
    }
    public bool CanOptionAdd()
    {
        for (int i = 0; i < totalOptionNum.Value(); i++)
        {
            if (optionEffects[i].CanEnchant())
            {
                return true;
            }
        }
        return false;
    }
    //オプションレベルアップ
    public void OptionLevelup(int optionId, bool isMax)
    {
        if (isMax) optionEffects[optionId].level.ToMax();
        else optionEffects[optionId].level.Increase(1);
        OptionRelottery(optionId);
    }
    public bool CanOptionLevelup(int optionId)
    {
        return !optionEffects[optionId].level.IsMaxed();
    }
    //オプション効果再抽選
    public void OptionRelottery(int optionId)
    {
        optionEffects[optionId].LotteryEffectValue();
    }
    //オプション削除
    public void OptionDelete(int optionId)
    {
        optionEffects[optionId].Delete();
    }
    public bool IsOptionSelectable(int optionId)
    {
        return optionEffects[optionId].kind != EquipmentEffectKind.Nothing;
    }
    //オプション抽出
    public void OptionExtract(int optionId, bool isCopy = false)
    {
        game.inventoryCtrl.CreateEnchant(EnchantKind.OptionAdd, optionEffects[optionId].kind, optionEffects[optionId].level.value);
        if (!isCopy) OptionDelete(optionId);
    }
    //オプション枠増加
    public bool CanOptionExpand()
    {
        return optionNum < 6;//最大6つまでOK
    }
    public void OptionExpand()
    {
        if (CanOptionExpand())
            optionNum++;
    }

    int createId;//DeleteしたらEffectの登録も消す（無効にする）必要があるため、Createするたびに更新する
    public void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            for (int j = 0; j < globalInfo.effects.Count; j++)
            {
                int count = j;
                SetEffect((HeroKind)i, globalInfo.effects[count].kind, () => globalInfo.effects[count].EffectValue(Level()), createId);
            }
            for (int j = 0; j < optionEffects.Length; j++)
            {
                int count = j;
                SetEffect((HeroKind)i, optionEffects[count].kind, () => optionEffects[count].effectValue, optionEffects[count].createId);
            }
            for (int j = 0; j < globalInfo.levelMaxEffects.Length; j++)
            {
                int count = j;
                SetEffect((HeroKind)i, globalInfo.levelMaxEffects[count].kind, () => globalInfo.levelMaxEffects[count].EffectValue(0), createId, () => globalInfo.levels[count].isMaxed);
            }
        }
    }

    public void Delete()
    {
        kind = EquipmentKind.Nothing;
        for (int i = 0; i < optionEffects.Length; i++)
        {
            optionEffects[i].Delete();
        }
        optionNum = 0;
        isReachedMax = false;
        isLocked = false;
        info = null;
    }
    
    bool isReachedMax;
    public bool isLocked { get => main.SR.equipmentIsLocked[id]; set => main.SR.equipmentIsLocked[id] = value; }
    public void Lock()
    {
        if (kind != EquipmentKind.Nothing)
            isLocked = !isLocked;
    }

    public int id;
    public virtual EquipmentKind kind { get => main.SR.equipmentKinds[id]; set => main.SR.equipmentKinds[id] = value; }
    public virtual int optionNum { get => main.SR.equipmentOptionNums[id]; set => main.SR.equipmentOptionNums[id] = value; }
    public Multiplier totalOptionNum;//MasteryEffectを含めたオプション数（最大６ -> BlacksmithのExpandScrollにより最大７）
    public EquipmentGlobalInformation globalInfo { get => game.equipmentCtrl.GlobalInfo(kind); }
    public EquipmentOptionEffect[] optionEffects = new EquipmentOptionEffect[EquipmentParameter.maxOptionEffectNum];
    public long Level() { return globalInfo.TotalLevel(); }

    //ThiefのEnchantSlotを抜きにして、EnchantEffectがあるかどうか
    public bool HasEnchantedEffect()
    {
        if (optionNum > 0) return true;
        if (totalOptionNum.Value() < 1) return false;
        for (int i = 0; i < optionEffects.Length; i++)
        {
            if (optionEffects[i].kind != EquipmentEffectKind.Nothing) return true;
        }
        return false;
    }
    //要求レベル
    public long RequiredLevel()
    {
        long tempValue = globalInfo.requiredAbilities[0].requiredValue;
        for (int i = 0; i < optionEffects.Length; i++)
        {
            tempValue += optionEffects[i].RequiredLevelIncrement();
        }
        return tempValue;
    }
    //要求レベルに足りているかどうか
    public bool IsEnoughLevel(HeroKind heroKind)
    {
        return game.statsCtrl.LevelForEquipment(heroKind).Value() >= RequiredLevel();
    }

    public bool CanEquip(HeroKind heroKind)
    {
        //Equipment Abuse(EpicStore)を購入済みの場合は無条件にtrue
        if (game.epicStoreCtrl.Item(EpicStoreKind.EQLessAbilityAvailable).IsPurchased()) return true;
        if (!IsEnoughLevel(heroKind)) return false;
        for (int i = 1; i < globalInfo.requiredAbilities.Count; i++)
        {
            if (!globalInfo.requiredAbilities[i].IsEnough(heroKind)) return false;
        }
        return true;
    }
    //装備しているかどうか
    public bool IsEquipped(HeroKind heroKind)
    {
        if (kind == EquipmentKind.Nothing) return false;
        if (!CanEquip(heroKind)) return false;
        if (slotId < equipInventorySlotId) return false;
        if (!game.inventoryCtrl.equipmentSlots[slotId].isAvailableEQ) return false;

        switch (heroKind)
        {
            case HeroKind.Warrior:
                return slotId - equipInventorySlotId < equipPartSlotId * 3;
            case HeroKind.Wizard:
                return slotId - equipInventorySlotId >= equipPartSlotId * 3 && slotId - equipInventorySlotId < 2 * equipPartSlotId * 3;
            case HeroKind.Angel:
                return slotId - equipInventorySlotId >= 2 * equipPartSlotId * 3 && slotId - equipInventorySlotId < 3 * equipPartSlotId * 3;
            case HeroKind.Thief:
                return slotId - equipInventorySlotId >= 3 * equipPartSlotId * 3 && slotId - equipInventorySlotId < 4 * equipPartSlotId * 3;
            case HeroKind.Archer:
                return slotId - equipInventorySlotId >= 4 * equipPartSlotId * 3 && slotId - equipInventorySlotId < 5 * equipPartSlotId * 3;
            case HeroKind.Tamer:
                return slotId - equipInventorySlotId >= 5 * equipPartSlotId * 3 && slotId - equipInventorySlotId < 6 * equipPartSlotId * 3;
        }
        return false;
    }

    //分解
    public double DisassembleMaterialNum()
    {
        return globalInfo.DisassembleMaterialNum() * (1 + optionNum) * game.equipmentCtrl.disassembleMultiplier.Value();
    }

    public bool isSetItem => globalInfo.setKind != EquipmentSetKind.Nothing;
    public double EffectMultiplier(HeroKind heroKind)
    {
        if (!isSetItem) return 1;
        switch (game.inventoryCtrl.SetItemEquippedNum(globalInfo.setKind, heroKind))
        {
            case 2:
                return 1.05d;
            case 3:
                return 1.10d;
            case 4:
                return 1.15d;//return 1.20d;
            case 5:
                return 1.20d;//return 1.35d;
            case 6:
                return 1.25d;//return 1.50d;
            case 7:
                return 1.35d;//return 1.70d;
            case 8:
                return 1.50d;//return 2.00d;
            default:
                return 1;
        }
    }
    double EffectValue(double baseEffectValue, HeroKind heroKind)
    {
        return baseEffectValue * EffectMultiplier(heroKind) * EQAbusePercent(heroKind);
    }
    public double EQAbusePercent(HeroKind heroKind)
    {
        double tempValue = 1;
        if (game.epicStoreCtrl.Item(EpicStoreKind.EQLessAbilityAvailable).IsPurchased())
        {
            double denominator = 1;
            //HeroLevel
            double tempReduction = Math.Min(1.0d, game.statsCtrl.LevelForEquipment(heroKind).Value() / RequiredLevel());
            //Ability
            for (int i = 1; i < globalInfo.requiredAbilities.Count; i++)
            {
                denominator++;
                int count = i;
                tempReduction += Math.Min(1.0d, (double)game.statsCtrl.Ability(heroKind, globalInfo.requiredAbilities[count].kind).Point() / globalInfo.requiredAbilities[count].requiredValue);
            }
            tempValue *= tempReduction / denominator;//これで平均をとる
        }
        tempValue = Math.Max(0.1d, tempValue);
        tempValue = Math.Min(1.0d, tempValue);
        return tempValue;
    }
    public void GetProficiency(double increment, HeroKind heroKind, bool isFixedAmount = false)//isFixedAmount=true:Abuseの影響を受けない
    {
        if (globalInfo.levels[(int)heroKind].IsMaxed() && isReachedMax) return;
        else isReachedMax = false;
        if (!isFixedAmount) increment *= EQAbusePercent(heroKind);
        globalInfo.proficiencies[(int)heroKind].Increase(increment);

        if (globalInfo.levels[(int)heroKind].IsMaxed())
        {
            isReachedMax = true;
            if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.EquipNonMaxedEQ))
            {
                for (int i = 0; i < game.inventoryCtrl.equipmentSlots.Length; i++)
                {
                    EquipmentSlot slot = game.inventoryCtrl.equipmentSlots[i];
                    if (!slot.isEquipmentSlot && slot.equipment.kind != EquipmentKind.Nothing && slot.equipment.globalInfo.part == globalInfo.part && slot.equipment.CanEquip(heroKind) && !slot.equipment.globalInfo.levels[(int)heroKind].IsMaxed())
                    {
                        game.inventoryCtrl.MoveEquipment(slotId, slot.slotId);
                        return;
                    }
                }
            }
        }
    }

    MultiplierInfo info;
    Func<bool> activateCondition;
    private void SetEffect(HeroKind heroKind, EquipmentEffectKind kind, Func<double> effectValue, int createId, Func<bool> additionalCondition = null)
    {
        activateCondition = () =>
        {
            if (additionalCondition != null && !additionalCondition()) return false;
            //「非アクティブでは無効」をここに書いてみる。が、非アクティブ時のStatsも表示する場合は、GlobalStatsの部分のみに書いたほうが良い
            if (!game.battleCtrls[(int)heroKind].isActiveBattle) return false;
            return this.createId == createId && IsEquipped(heroKind);
        };

        switch (kind)
        {
            case EquipmentEffectKind.Nothing://LevelMaxEffectの時だけadditionalConditionがnullでない
                if (additionalCondition != null && heroKind == HeroKind.Thief)//つまり、LevelMaxEffectの時だけこれを登録する。
                {
                    info = new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, effectValue, () => this.createId == createId && additionalCondition());
                    totalOptionNum.RegisterMultiplier(info);
                }
                break;
            case EquipmentEffectKind.HPAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MPAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ATKAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MATKAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DEFAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MDEFAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.HPMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.HP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MPMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MP).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ATKMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MATKMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DEFMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MDEFMultiplier:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ATKPropotion:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.ATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => game.statsCtrl.Level(heroKind) * EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MATKPropotion:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MATK).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => game.statsCtrl.Level(heroKind) * EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DEFPropotion:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.DEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => game.statsCtrl.Level(heroKind) * EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MDEFPropotion:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.MDEF).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => game.statsCtrl.Level(heroKind) * EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FireResistance:
                game.statsCtrl.ElementResistance(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.IceResistance:
                game.statsCtrl.ElementResistance(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ThunderResistance:
                game.statsCtrl.ElementResistance(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.LightResistance:
                game.statsCtrl.ElementResistance(heroKind, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DarkResistance:
                game.statsCtrl.ElementResistance(heroKind, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.PhysicalAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Physical).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FireAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.IceAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ThunderAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.LightAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DarkAbsorption:
                game.statsCtrl.ElementAbsorption(heroKind, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DebuffResistance:
                game.statsCtrl.HeroStats(heroKind, Stats.DebuffRes).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.PhysicalCritical:
                game.statsCtrl.HeroStats(heroKind, Stats.PhysCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MagicalCritical:
                game.statsCtrl.HeroStats(heroKind, Stats.MagCritChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.EXPGain:
                game.statsCtrl.HeroStats(heroKind, Stats.ExpGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SkillProficiency:
                game.statsCtrl.HeroStats(heroKind, Stats.SkillProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.EquipmentProficiency:
                game.statsCtrl.HeroStats(heroKind, Stats.EquipmentProficiencyGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MoveSpeedAdder:
                game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MoveSpeedMultiplier:
                game.statsCtrl.HeroStats(heroKind, Stats.MoveSpeed).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.GoldGain:
                game.statsCtrl.GoldGain().RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.StoneGain:
                game.statsCtrl.ResourceGain(ResourceKind.Stone).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.CrystalGain:
                game.statsCtrl.ResourceGain(ResourceKind.Crystal).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.LeafGain:
                game.statsCtrl.ResourceGain(ResourceKind.Leaf).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.WarriorSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.WizardSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.AngelSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ThiefSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ArcherSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.TamerSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.AllSkillLevel:
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Warrior].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Wizard].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Angel].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Thief].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Archer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                game.skillCtrl.skillLevelBonus[(int)HeroKind.Tamer].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SPDAdder:
                game.statsCtrl.BasicStats(heroKind, BasicStatsKind.SPD).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SlimeKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Slime).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MagicSlimeKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.MagicSlime).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SpiderKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Spider).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.BatKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Bat).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FairyKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Fairy).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FoxKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Fox).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DevilFishKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.DevifFish).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.TreantKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Treant).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FlameTigerKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.FlameTiger).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.UnicornKnowledge:
                game.statsCtrl.MonsterDamage(heroKind, MonsterSpecies.Unicorn).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.PhysicalDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Physical).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FireDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Fire).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.IceDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Ice).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ThunderDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Thunder).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.LightDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Light).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DarkDamage:
                game.statsCtrl.ElementDamage(heroKind, Element.Dark).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.EquipmentDropChance:
                game.statsCtrl.HeroStats(heroKind, Stats.EquipmentDropChance).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.ColorMaterialDropChance:
                game.monsterCtrl.colorMaterialDropChance.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SlimeDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Slime].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MagicSlimeDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.MagicSlime].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.SpiderDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Spider].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.BatDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Bat].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FairyDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Fairy].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FoxDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Fox].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.DevilFishDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.DevifFish].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.TreantDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Treant].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.FlameTigerDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.FlameTiger].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.UnicornDropChance:
                game.monsterCtrl.speciesMaterialDropChance[(int)MonsterSpecies.Unicorn].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.HpRegen:
                game.statsCtrl.HpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.MpRegen:
                game.statsCtrl.MpRegenerate(heroKind).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.TamingPoint:
                game.statsCtrl.HeroStats(heroKind, Stats.TamingPointGain).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.WarriorSkillRange://未
                break;
            case EquipmentEffectKind.WizardSkillRange:
                break;
            case EquipmentEffectKind.AngelSkillRange:
                break;
            case EquipmentEffectKind.ThiefSkillRange:
                break;
            case EquipmentEffectKind.ArcherSkillRange:
                break;
            case EquipmentEffectKind.TamerSkillRange:
                break;
            case EquipmentEffectKind.TownMatGain:
                game.townCtrl.townMaterialGainMultiplier[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.TownMatAreaClearGain://%ではなく＋１
                game.areaCtrl.townMaterialGainBonusClass[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            //case EquipmentEffectKind.TownMatDungeonRewardGain:
            //    game.areaCtrl.townMaterialDungeonRewardMultiplier.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
            //    break;
            case EquipmentEffectKind.RebirthPointGain1:
                game.rebirthCtrl.Rebirth(heroKind, 0).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.RebirthPointGain2:
                game.rebirthCtrl.Rebirth(heroKind, 1).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.RebirthPointGain3:
                game.rebirthCtrl.Rebirth(heroKind, 2).rebirthPointGainFactor.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Mul, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.CriticalDamage:
                game.statsCtrl.HeroStats(heroKind, Stats.CriticalDamage).RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
            case EquipmentEffectKind.BlessingEffect:
                game.blessingInfoCtrl.effectMultipliers[(int)heroKind].RegisterMultiplier(new MultiplierInfo(MultiplierKind.Equipment, MultiplierType.Add, () => EffectValue(effectValue(), heroKind), activateCondition));
                break;
        }
    }

}
