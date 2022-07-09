using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;

public partial class SaveR
{
    public EnchantKind[] enchantKinds;//[enchantId]
    public EquipmentEffectKind[] enchantEffectKinds;//[enchantId]
    public long[] enchantEffectLevels;

    public double[] enchantProficiencyTimesec;//[enchantId]
}
public class EquipmentEnchant
{
    public EquipmentEnchant(int id)
    {
        this.id = id;
        
    }
    public void Create(EnchantKind kind, EquipmentEffectKind effectKind, long effectLevel, double proficiencyTimesec)
    {
        this.kind = kind;
        this.effectKind = effectKind;
        this.effectLevel = Math.Min(EquipmentParameter.MaxLevel(effectKind), effectLevel);
        this.proficiencyTimesec = proficiencyTimesec;
    }
    public void Delete()
    {
        kind = EnchantKind.Nothing;
        effectKind = EquipmentEffectKind.Nothing;
        effectLevel = 0;
        proficiencyTimesec = 0;
    }
    int slotId;
    public void UpdateSlotId(int slotId)
    {
        this.slotId = slotId;
    }
    public int id;
    public EnchantKind kind { get => main.SR.enchantKinds[id]; set => main.SR.enchantKinds[id] = value; }
    public EquipmentEffectKind effectKind { get => main.SR.enchantEffectKinds[id]; set => main.SR.enchantEffectKinds[id] = value; }
    public long effectLevel { get => main.SR.enchantEffectLevels[id]; set => main.SR.enchantEffectLevels[id] = value; }
    public double proficiencyTimesec { get => main.SR.enchantProficiencyTimesec[id]; set => main.SR.enchantProficiencyTimesec[id] = value; }

    public double DisassembleGoldNum()
    {
        return 1000 + 500 * effectLevel;
    }
}
public enum EnchantKind
{
    Nothing,
    OptionAdd,
    OptionDelete,
    OptionExtract,
    OptionLottery,//effectValueを再抽選
    OptionLevelup,
    OptionLevelMax,
    OptionCopy,
    ExpandEnchantSlot,
    //ver0.3.1.3以降追加
    InstantProf,//即Proficiency獲得（完全消費）
    //錬成
}
