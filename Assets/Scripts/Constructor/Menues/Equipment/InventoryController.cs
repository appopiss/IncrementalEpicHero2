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
    public int[] equipmentId;//[slotId], equipmentの登録番号のようなもの
    public int[] enchantId;//[slotId], enchantの登録番号
    public int[] potionId;//[slotId], potionの登録番号
}

public class InventoryController
{
    public InventoryController()
    {
        SetUnlockedNum();
        CheckInitializedId();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i] = new EquipmentSlot(i);
        }
        for (int i = 0; i < enchantSlots.Length; i++)
        {
            enchantSlots[i] = new EnchantSlot(i);
        }
        for (int i = 0; i < potionSlots.Length; i++)
        {
            potionSlots[i] = new PotionSlot(i);
        }
    }
    public void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Start();
        }
        for (int i = 0; i < potionSlots.Length; i++)
        {
            potionSlots[i].Start();
        }
        UpdateCanCreatePotion();
        UpdatePotionEquipNum();
    }

    public void UpdatePerSec(float deltaTime)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].UpdatePerSec(deltaTime);
        }
    }

    public int SetItemEquippedNum(EquipmentSetKind kind, HeroKind heroKind)
    {
        int tempNum = 0;
        EquipmentKind[] tempKinds = game.equipmentCtrl.setItemArray[(int)kind];
        for (int i = 0; i < tempKinds.Length; i++)
        {
            for (int j = 0; j < equipmentSlots.Length; j++)
            {
                if (equipmentSlots[j].equipment.IsEquipped(heroKind) && equipmentSlots[j].equipment.globalInfo.kind == tempKinds[i])
                {
                    tempNum++;
                    break;
                }
            }
        }
        return tempNum;
    }

    //EquipmentId, EnchantId, PotionIdの初期化チェック
    void CheckInitializedId()
    {
        int tempCount = 0;
        for (int i = 0; i < main.SR.equipmentId.Length; i++)
        {
            if (main.SR.equipmentId[i] == 0) tempCount++;
            if (tempCount > 1)
            {
                for (int j = 0; j < main.SR.equipmentId.Length; j++)
                {
                    main.SR.equipmentId[j] = j;
                }
                break;
            }
        }
        tempCount = 0;
        for (int i = 0; i < main.SR.enchantId.Length; i++)
        {
            if (main.SR.enchantId[i] == 0) tempCount ++;
            if (tempCount > 1)
            {
                for (int j = 0; j < main.SR.enchantId.Length; j++)
                {
                    main.SR.enchantId[j] = j;
                }
                break;
            }
        }
        tempCount = 0;
        for (int i = 0; i < main.SR.potionId.Length; i++)
        {
            if (main.SR.potionId[i] == 0) tempCount++;
            if (tempCount > 1)
            {
                for (int j = 0; j < main.SR.potionId.Length; j++)
                {
                    main.SR.potionId[j] = j;
                }
                break;
            }
        }

    }

    //Equipment
    public void CreateEquipment(EquipmentKind kind, int optionNum, long monsterLevel)
    {
        if (kind == EquipmentKind.Nothing) return;
        for (int i = 0; i < equipInventorySlotId; i++)
        {
            if (IsEmpty(equipmentSlots[i]))
            {
                equipmentSlots[i].equipment.CreateNew(kind, optionNum, monsterLevel);
                break;
            }
        }
        if (slotUIAction != null) slotUIAction();
        game.equipmentCtrl.globalInformations[(int)kind].isGotOnce = true;
    }
    public bool CanCreateEquipment()//空きスロットがあるか？
    {
        //for (int i = 0; i < equipmentSlots.Length; i++)
        for (int i = 0; i < equipInventorySlotId; i++)
        {
            if (IsEmpty(equipmentSlots[i])) return true;
        }
        return false;
    }
    bool IsEmpty(EquipmentSlot slot)
    {
        return slot.isUnlocked && slot.equipment.kind == EquipmentKind.Nothing;
    }
    //Mission用:何か装備をつけているかどうか
    public bool IsEquippedEquipment(HeroKind heroKind)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].equipment.IsEquipped(heroKind)) return true;
            //if (equipmentSlots[i].isEquipmentSlot && equipmentSlots[i].heroKind == heroKind && equipmentSlots[i].equipment.kind != EquipmentKind.Nothing) return true;
        }
        return false;
    }

    //Enchant
    public void CreateEnchant(EnchantKind kind, EquipmentEffectKind effectKind = EquipmentEffectKind.Nothing, long level = 0, double proficiencyTimesec = 0)
    {
        if (kind == EnchantKind.Nothing) return;
        for (int i = 0; i < enchantSlotId; i++)
        {
            if (IsEmpty(enchantSlots[i]))
            {
                enchantSlots[i].enchant.Create(kind, effectKind, level, proficiencyTimesec);
                break;
            }
        }
        if (slotUIAction != null) slotUIAction();
    }
    public bool CanCreateEnchant(long num = 1)//空きスロットnum個があるか？
    {
        long tempNum = 0;
        for (int i = 0; i < enchantSlots.Length; i++)
        {
            if (IsEmpty(enchantSlots[i])) { tempNum++; if (tempNum >= num) return true; }
        }
        return false;
    }
    bool IsEmpty(EnchantSlot slot)
    {
        return slot.isUnlocked && slot.enchant.kind == EnchantKind.Nothing;
    }

    //Enchant実行処理
    public void TryEnchant(EquipmentEnchant tempEnchant, Equipment tempEquipment, int targetOptionId = 0)
    {
        switch (tempEnchant.kind)
        {
            case EnchantKind.OptionAdd:
                if (!tempEquipment.CanOptionAdd()) return;
                tempEquipment.OptionAdd(tempEnchant.effectKind, tempEnchant.effectLevel);
                break;
            case EnchantKind.OptionLevelup:
                if (!tempEquipment.CanOptionLevelup(targetOptionId)) return;
                tempEquipment.OptionLevelup(targetOptionId, false);
                break;
            case EnchantKind.OptionLevelMax:
                if (!tempEquipment.CanOptionLevelup(targetOptionId)) return;
                tempEquipment.OptionLevelup(targetOptionId, true);
                break;
            case EnchantKind.OptionLottery:
                if (!tempEquipment.IsOptionSelectable(targetOptionId)) return;
                tempEquipment.OptionRelottery(targetOptionId);
                break;
            case EnchantKind.OptionDelete:
                if (!tempEquipment.IsOptionSelectable(targetOptionId)) return;
                tempEquipment.OptionDelete(targetOptionId);
                break;
            case EnchantKind.OptionExtract:
                if (!tempEquipment.IsOptionSelectable(targetOptionId)) return;
                tempEnchant.Delete();
                tempEquipment.OptionExtract(targetOptionId);
                if (slotUIAction != null) slotUIAction();
                return;//これだけ順番が大事なのでここに書く
            case EnchantKind.OptionCopy:
                if (!tempEquipment.IsOptionSelectable(targetOptionId)) return;
                tempEnchant.Delete();
                tempEquipment.OptionExtract(targetOptionId, true);
                if (slotUIAction != null) slotUIAction();
                return;
            case EnchantKind.ExpandEnchantSlot:
                if (!tempEquipment.CanOptionExpand()) return;
                tempEquipment.OptionExpand();
                break;
            case EnchantKind.InstantProf:
                if (tempEquipment.globalInfo.levels[(int)game.currentHero].IsMaxed()) return;
                tempEquipment.GetProficiency(tempEnchant.proficiencyTimesec, game.currentHero, true);
                break;
        }
        tempEnchant.Delete();
        if (slotUIAction != null) slotUIAction();
    }

    //Potion
    public void CreatePotion(PotionGlobalInformation globalInfo, long num)
    {
        if (num <= 0) return;
        //Talismanの場合
        if (globalInfo.type == PotionType.Talisman)
        {
            //PetQoLでAuto-Disassembleの場合
            switch (globalInfo.talismanRarity)
            {
                case TalismanRarity.Common:
                    if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.DisassembleTalismanCommon))
                    {
                        game.alchemyCtrl.talismanFragment.Increase(globalInfo.DisassembleGoldNum() * num);
                        globalInfo.disassembledNum.Increase(num);
                        return;
                    }
                    break;
            }
        }
        CreatePotion(globalInfo.kind, num);
    }
    public void CreatePotion(PotionKind kind, long num)
    {
        if (num <= 0) return;
        Potion tempPotion;
        //まず同じPotionがあるかどうか探す
        for (int i = 0; i < potionSlotId; i++)
        {
            tempPotion = potionSlots[i].potion;
            if (tempPotion.kind == kind && !tempPotion.stackNum.IsMaxed())
            {
                if (!tempPotion.IsFull(num).isMax)
                {
                    tempPotion.Create(kind, num);
                    SlotUIActionWithPotionSlot();
                    return;
                }
                num = tempPotion.IsFull(num).leftNum;//こっちが先
                tempPotion.CreateMax(kind);
            }
        }
        //空きスロットを探す
        for (int i = 0; i < potionSlotId; i++)
        {
            if (IsEmpty(potionSlots[i]))
            {
                tempPotion = potionSlots[i].potion;
                if (!tempPotion.IsFull(num).isMax)
                {
                    tempPotion.Create(kind, num);
                    SlotUIActionWithPotionSlot();
                    return;
                }
                num = tempPotion.IsFull(num).leftNum;//こっちが先
                tempPotion.CreateMax(kind);
            }
        }
        SlotUIActionWithPotionSlot();
    }
    //処理が重いのでCanCreatePotionはSlotUIを更新するときに一回呼ぶことにした
    long[] canCreatePotionNums = new long[Enum.GetNames(typeof(PotionKind)).Length];
    long[] tempCanCreatePotionNums = new long[Enum.GetNames(typeof(PotionKind)).Length];
    public void UpdateCanCreatePotion()
    {
        for (int i = 0; i < tempCanCreatePotionNums.Length; i++)
        {
            tempCanCreatePotionNums[i] = 0;
        }
        //まず空きスロットの数を確認し、同時に、Potionごとに同じPotionがあるかどうか探す
        long tempEmptySlotNum = 0;
        Potion tempPotion;
        for (int i = 0; i < potionSlotId; i++)
        {
            if (IsEmpty(potionSlots[i])) tempEmptySlotNum++;
            for (int j = 1; j < tempCanCreatePotionNums.Length; j++)//Nothingを除くので1からスタート
            {
                tempPotion = potionSlots[i].potion;
                if ((int)tempPotion.kind == j && !tempPotion.stackNum.IsMaxed())
                {
                    tempCanCreatePotionNums[j] += tempPotion.leftEmptyNum;
                }
            }
        }
        //最後に、空きスロット分を加算して、同期する
        for (int i = 0; i < tempCanCreatePotionNums.Length; i++)
        {
            tempCanCreatePotionNums[i] += tempEmptySlotNum * (long)game.potionCtrl.maxStackNum.Value();
            canCreatePotionNums[i] = tempCanCreatePotionNums[i];
        }
    }
    public bool CanCreatePotion(PotionGlobalInformation globalInfo, long num)
    {
        //Talismanの場合
        if (globalInfo.type == PotionType.Talisman)
        {
            //PetQoLでAuto-Disassembleの場合
            switch (globalInfo.talismanRarity)
            {
                case TalismanRarity.Common:
                    if (game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.DisassembleTalismanCommon)) return true;
                    break;
            }
        }
        return CanCreatePotion(globalInfo.kind, num);
    }
    public bool CanCreatePotion(PotionKind kind, long num)//空きスロットがあるか？
    {
        return canCreatePotionNums[(int)kind] >= num;
        //Potion tempPotion;
        ////まず同じPotionがあるかどうか探す
        //for (int i = 0; i < potionSlotId; i++)
        //{
        //    tempPotion = potionSlots[i].potion;
        //    if (tempPotion.kind == kind && !tempPotion.stackNum.IsMaxed())
        //    {
        //        if (!tempPotion.IsFull(num).isMax)
        //            return true;
        //        num = tempPotion.IsFull(num).leftNum;//こっちが先
        //    }
        //}
        ////空きスロットを探す
        //for (int i = 0; i < potionSlotId; i++)
        //{
        //    if (IsEmpty(potionSlots[i]))
        //    {
        //        tempPotion = potionSlots[i].potion;
        //        if (!tempPotion.IsFull(num).isMax)
        //            return true;
        //        num = tempPotion.IsFull(num).leftNum;//こっちが先
        //    }
        //}
        //return false;
    }
    //AlchemyQueue用：Inventoryにそのポーションがあり、Stack#がMaxになっているかどうか
    public bool IsOnePotionStackMaxInInventory(PotionKind kind)
    {
        for (int i = 0; i < potionSlotId; i++)
        {
            Potion potion = potionSlots[i].potion;
            if (potion.kind == kind && potion.stackNum.IsMaxed()) return true;
        }
        return false;
    }
    bool IsEmpty(PotionSlot slot)//Inventoryで空きがあるかどうか
    {
        return slot.isUnlocked && !slot.isEquipSlot && slot.potion.kind == PotionKind.Nothing;
    }
    bool IsEmptyEquipSlot(PotionSlot slot)//EquipSlotで空きがあるかどうか
    {
        return slot.isUnlocked && slot.isEquipSlot && slot.potion.kind == PotionKind.Nothing;
    }
    bool IsEquippedPotion(PotionSlot slot, PotionKind kind)
    {
        return slot.isUnlocked && slot.isEquipSlot && slot.potion.kind == kind;
    }

    //Potionごと・ヒーローごとの装備数をアップデート
    public double PotionEquipNum(PotionKind kind, HeroKind heroKind) { return potionEquipNums[SaveId(kind, heroKind)]; }
    double[] potionEquipNums = new double[Enum.GetNames(typeof(PotionKind)).Length * Enum.GetNames(typeof(HeroKind)).Length];
    double[] tempPotionEquipNums = new double[Enum.GetNames(typeof(PotionKind)).Length * Enum.GetNames(typeof(HeroKind)).Length];
    int SaveId(PotionKind kind, HeroKind heroKind) { return (int)kind + (int)heroKind * Enum.GetNames(typeof(PotionKind)).Length; }
    public void UpdatePotionEquipNum()
    {
        for (int i = 0; i < tempPotionEquipNums.Length; i++)
        {
            tempPotionEquipNums[i] = 0;
        }
        Potion potion;
        for (int i = 0; i < potionSlots.Length; i++)
        {            
            if (potionSlots[i].isEquipSlot)
            {
                tempPotionEquipNums[SaveId(potionSlots[i].potion.kind, potionSlots[i].heroKind)] += potionSlots[i].potion.stackNum.value;
            }
        }
        for (int i = 0; i < potionEquipNums.Length; i++)
        {
            potionEquipNums[i] = tempPotionEquipNums[i];
        }
    }

    //PotionのConsume
    public void ConsumePotion(PotionConsumeConditionKind consumeKind, BATTLE battle, bool isSimulated)
    {
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            potionSlots[i].potion.LotteryConsume(consumeKind, battle, isSimulated);
        }
    }
    public void ConsumePotion(PotionKind potionKind, BATTLE battle, bool isSimulated)
    {
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            potionSlots[i].potion.LotteryConsume(potionKind, battle, isSimulated);
        }
    }
    //Potionを持っているかの判定
    public bool IsEquippedPotion(PotionKind kind, HeroKind heroKind)
    {
        if (kind == PotionKind.Nothing) return false;
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            if (potionSlots[i].heroKind == heroKind && potionSlots[i].potion.kind == kind && potionSlots[i].potion.stackNum.value > 0) return true;
        }
        return false;
    }
    public bool IsEquippedPotion(PotionType type, HeroKind heroKind)
    {
        if (type == PotionType.Nothing) return false;
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            if (potionSlots[i].heroKind == heroKind && potionSlots[i].potion.globalInfo.type == type && potionSlots[i].potion.stackNum.value > 0) return true;
        }
        return false;
    }

    //Delete処理
    public void Delete(Equipment equipment)
    {
        equipment.Delete();
        if (slotUIAction != null) slotUIAction();
    }
    public void Delete(EquipmentEnchant enchant)
    {
        enchant.Delete();
        if (slotUIAction != null) slotUIAction();
    }
    public void Delete(Potion potion)
    {
        potion.Delete();
        //SlotUIActionWithPotionSlot();
    }
    //Disassemble
    public void Disassemble(Equipment equipment)
    {
        if (equipment.isLocked) return;
        equipment.globalInfo.DisassembleMaterialKind().Increase(equipment.DisassembleMaterialNum());
        main.SR.disassembledEquipmentNums[(int)equipment.kind]++;
        main.S.disassembledEquipmentNums[(int)equipment.kind]++;
        main.SR.townMatGainFromdisassemble[(int)equipment.kind] += equipment.DisassembleMaterialNum();
        main.S.townMatGainFromdisassemble[(int)equipment.kind] += equipment.DisassembleMaterialNum();
        Delete(equipment);
    }
    public void Disassemble(EquipmentEnchant enchant)
    {
        game.resourceCtrl.gold.Increase(enchant.DisassembleGoldNum());
        Delete(enchant);
    }
    public void Disassemble(Potion potion)
    {
        if (!potion.globalInfo.canDisassemble) return;
        if (potion.globalInfo.type == PotionType.Talisman)
            game.alchemyCtrl.talismanFragment.Increase(potion.DisassembleGoldNum());
        else
            game.resourceCtrl.gold.Increase(potion.DisassembleGoldNum());
        potion.globalInfo.disassembledNum.Increase(potion.stackNum.value);
        Delete(potion);
    }
    //移動
    public void MoveEquipment(int fromId, int toId)
    {
        EquipmentSlot toSlot = equipmentSlots[toId];
        if (!toSlot.isUnlocked) return;
        EquipmentSlot fromSlot = equipmentSlots[fromId];
        Equipment tempEquipment = toSlot.equipment;
        if (fromSlot.isEquipmentSlot && tempEquipment.kind != EquipmentKind.Nothing)
        {
            if (tempEquipment.globalInfo.part != fromSlot.equipmentPart) return;
            if (!tempEquipment.CanEquip(fromSlot.heroKind)) return;
        }
        tempEquipment = fromSlot.equipment;
        if (toSlot.isEquipmentSlot)
        {
            if (tempEquipment.globalInfo.part != toSlot.equipmentPart) return;
            if (!tempEquipment.CanEquip(toSlot.heroKind)) return;
        }
        int tempEquipmentId = fromSlot.equipmentId;
        fromSlot.ChangePlace(toSlot.equipment, toSlot.equipmentId);
        toSlot.ChangePlace(tempEquipment, tempEquipmentId);
    }
    public void MoveEnchant(int fromId, int toId)
    {
        EnchantSlot toSlot = enchantSlots[toId];
        if (!toSlot.isUnlocked) return;
        EnchantSlot fromSlot = enchantSlots[fromId];
        EquipmentEnchant tempEnchant = fromSlot.enchant;
        int tempEnchantId = fromSlot.enchantId;
        fromSlot.ChangePlace(toSlot.enchant, toSlot.enchantId);
        toSlot.ChangePlace(tempEnchant, tempEnchantId);
    }

    //PotionのStack#を指定して移動
    public void MovePotion(int fromId, int toId, long stackNum)
    {
        if (fromId == toId) return;//同じスロット同士は考えない
        PotionSlot toSlot = potionSlots[toId];
        if (!toSlot.isUnlocked) return;
        PotionSlot fromSlot = potionSlots[fromId];
        Potion tempPotion = fromSlot.potion;
        stackNum = Math.Min(stackNum, tempPotion.stackNum.value);
        tempPotion.StackNumCheckToDelete();
        toSlot.potion.StackNumCheckToDelete();
        //同じポーションの場合
        if (toSlot.potion.kind == tempPotion.kind && !toSlot.potion.stackNum.IsMaxed())
        {
            if (!toSlot.potion.IsFull(stackNum).isMax)
            {
                toSlot.potion.Create(tempPotion.kind, stackNum);
                tempPotion.stackNum.Increase(-stackNum);
                tempPotion.StackNumCheckToDelete();
                UpdateCanCreatePotion();
                UpdatePotionEquipNum();
                return;
            }
            tempPotion.stackNum.ChangeValue(toSlot.potion.IsFull(stackNum).leftNum);
            toSlot.potion.CreateMax(tempPotion.kind);
            UpdateCanCreatePotion();
            UpdatePotionEquipNum();
            return;
        }
        bool isTalisman = toSlot.potion.globalInfo.type == PotionType.Talisman || fromSlot.potion.globalInfo.type == PotionType.Talisman;
        bool isTrap = toSlot.potion.globalInfo.type == PotionType.Trap || fromSlot.potion.globalInfo.type == PotionType.Trap;
        //同じtypeのポーションは複数装備できない。ただしTalismanは、同じ種類でなければOK
        if (isTalisman)
        {
            if (
            (toSlot.isEquipSlot && fromSlot.potion.globalInfo.type == PotionType.Talisman && IsEquippedPotion(fromSlot.potion.kind, toSlot.heroKind))
            ||
            (fromSlot.isEquipSlot && toSlot.potion.globalInfo.type == PotionType.Talisman && IsEquippedPotion(toSlot.potion.kind, fromSlot.heroKind))
            )
            {
                if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.kind != fromSlot.potion.kind)
                {
                    GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same Talisman.</color>", 0, true);
                    UpdateCanCreatePotion();
                    UpdatePotionEquipNum();
                    return;
                }
            }
        }
        //EpicStore[Multitrapper]を購入している場合は複数Trapを装備できる。ただし同じものはダメ
        else if (game.epicStoreCtrl.Item(EpicStoreKind.Multitrapper).IsPurchased() && isTrap)
        {
            if ((toSlot.isEquipSlot && fromSlot.potion.globalInfo.type == PotionType.Trap && IsEquippedPotion(fromSlot.potion.kind, toSlot.heroKind))
            ||
            (fromSlot.isEquipSlot && toSlot.potion.globalInfo.type == PotionType.Trap && IsEquippedPotion(toSlot.potion.kind, fromSlot.heroKind))
            )
            {
                if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.kind != fromSlot.potion.kind)
                {
                    GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same Trap.</color>", 0, true);
                    UpdateCanCreatePotion();
                    UpdatePotionEquipNum();
                    return;
                }
            }
        }
        //else//Talismanの場合はもう検討しているので、それ以外について。。。を追加する
        //{
            if ((toSlot.isEquipSlot && (fromSlot.potion.globalInfo.type != PotionType.Talisman) && IsEquippedPotion(fromSlot.potion.globalInfo.type, toSlot.heroKind)) || (fromSlot.isEquipSlot && (toSlot.potion.globalInfo.type != PotionType.Talisman) && IsEquippedPotion(toSlot.potion.globalInfo.type, fromSlot.heroKind)))//fromSlot.potion.globalInfo.type != PotionType.Trap &&
        {
                if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.globalInfo.type != fromSlot.potion.globalInfo.type)
                {
                    GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same type.</color>", 0, true);
                    UpdateCanCreatePotion();
                    UpdatePotionEquipNum();
                    return;
                }
            }
        //}

        if (toSlot.potion.kind == PotionKind.Nothing)//stack#の一部を移動させる場合
        {
            //int tempPotionId = fromSlot.potionId;
            toSlot.potion.Create(tempPotion.kind, stackNum);
            tempPotion.stackNum.Increase(-stackNum);
            tempPotion.StackNumCheckToDelete();
            //UpdateCanCreatePotion();
            UpdatePotionEquipNum();
        }
        else //fromPotion以外のPotionへ持って行く場合
        {
            //スロットの交換
            int tempPotionId = fromSlot.potionId;
            fromSlot.ChangePlace(toSlot.potion, toSlot.potionId);
            toSlot.ChangePlace(tempPotion, tempPotionId);
            UpdateCanCreatePotion();
            UpdatePotionEquipNum();
        }
    }
    //Potionの移動
    public void MovePotion(int fromId, int toId)
    {
        MovePotion(fromId, toId, 100);
        //PotionSlot toSlot = potionSlots[toId];
        //if (!toSlot.isUnlocked) return;
        //PotionSlot fromSlot = potionSlots[fromId];
        //Potion tempPotion = fromSlot.potion;
        //tempPotion.StackNumCheckToDelete();
        //toSlot.potion.StackNumCheckToDelete();
        ////同じポーションの場合
        //if (toSlot.potion.kind == tempPotion.kind && !toSlot.potion.stackNum.IsMaxed())
        //{
        //    if (!toSlot.potion.IsFull(tempPotion.stackNum.value).isMax)
        //    {
        //        toSlot.potion.Create(tempPotion.kind, tempPotion.stackNum.value);
        //        tempPotion.Delete();
        //        UpdateCanCreatePotion();
        //        return;
        //    }
        //    tempPotion.stackNum.ChangeValue(toSlot.potion.IsFull(tempPotion.stackNum.value).leftNum);
        //    toSlot.potion.CreateMax(tempPotion.kind);
        //    UpdateCanCreatePotion();
        //    return;
        //}
        //bool isTalisman = toSlot.potion.globalInfo.type == PotionType.Talisman || fromSlot.potion.globalInfo.type == PotionType.Talisman;
        //bool isTrap = toSlot.potion.globalInfo.type == PotionType.Trap || fromSlot.potion.globalInfo.type == PotionType.Trap;

        ////同じtypeのポーションは複数装備できない。ただしTalismanは、同じ種類出なければOK
        //if (isTalisman)
        //{
        //    if (
        //    (toSlot.isEquipSlot && fromSlot.potion.globalInfo.type == PotionType.Talisman && IsEquippedPotion(fromSlot.potion.kind, toSlot.heroKind))
        //    ||
        //    (fromSlot.isEquipSlot && toSlot.potion.globalInfo.type == PotionType.Talisman && IsEquippedPotion(toSlot.potion.kind, fromSlot.heroKind))
        //    ){
        //        if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.kind != fromSlot.potion.kind)
        //        {
        //            GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same Talisman.</color>", 0, true);
        //            UpdateCanCreatePotion();
        //            return;
        //        }

        //    }
        //}
        ////EpicStore[Multitrapper]を購入している場合は複数Trapを装備できる。ただし同じものはダメ
        //else if (game.epicStoreCtrl.Item(EpicStoreKind.Multitrapper).IsPurchased() && isTrap)
        //{
        //    if((toSlot.isEquipSlot && fromSlot.potion.globalInfo.type == PotionType.Trap && IsEquippedPotion(fromSlot.potion.kind, toSlot.heroKind))
        //    ||
        //    (fromSlot.isEquipSlot && toSlot.potion.globalInfo.type == PotionType.Trap && IsEquippedPotion(toSlot.potion.kind, fromSlot.heroKind))
        //    )
        //    {
        //        if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.kind != fromSlot.potion.kind)
        //        {
        //            GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same Trap.</color>", 0, true);
        //            UpdateCanCreatePotion();
        //            return;
        //        }
        //    }
        //}
        //else//Talismanの場合はもう検討しているので、それ以外について。。。を追加する
        //{
        //    if ((toSlot.isEquipSlot && IsEquippedPotion(fromSlot.potion.globalInfo.type, toSlot.heroKind)) || (fromSlot.isEquipSlot && IsEquippedPotion(toSlot.potion.globalInfo.type, fromSlot.heroKind)))
        //    {
        //        if (!(toSlot.isEquipSlot && fromSlot.isEquipSlot) && toSlot.potion.globalInfo.type != fromSlot.potion.globalInfo.type)
        //        {
        //            GameControllerUI.gameUI.logCtrlUI.Log("<color=red>You cannot equip the same type.</color>", 0, true);
        //            UpdateCanCreatePotion();
        //            return;
        //        }
        //    }
        //}
        ////かなり面倒なので実装断念。。。
        //////MaxStack#が違わないかどうか確認する（Talisman）
        ////if (toSlot.potion.stackNum.maxValue() != fromSlot.potion.stackNum.maxValue())
        ////{
        ////    if(toSlot.potion.stackNum.maxValue() >= fromSlot.potion.stackNum.value)

        ////    return;
        ////}

        ////スロットの交換
        //int tempPotionId = fromSlot.potionId;
        //fromSlot.ChangePlace(toSlot.potion, toSlot.potionId);
        //toSlot.ChangePlace(tempPotion, tempPotionId);
        //UpdateCanCreatePotion();
    }
    //ダブルクリックによる移動
    public void MoveEquipmentShortcut(int slotId, HeroKind heroKind)
    {
        EquipmentSlot slot = equipmentSlots[slotId];
        if (!slot.isUnlocked) return;
        if (!slot.equipment.CanEquip(heroKind)) return;
        if (slot.isEquipmentSlot)//装備スロットから外す場合
        {
            for (int i = 0; i < equipInventorySlotId; i++)
            {
                int count = i;
                if (IsEmpty(equipmentSlots[count]))
                {
                    MoveEquipment(slotId, count);
                    return;
                }
            }
            return;
        }
        for (int i = equipInventorySlotId; i < equipmentSlots.Length; i++)
        {
            int count = i;
            if (IsEmpty(equipmentSlots[count]) && equipmentSlots[count].heroKind == heroKind && equipmentSlots[count].equipmentPart == slot.equipment.globalInfo.part)
            {
                MoveEquipment(slotId, count);
                return;
            }
        }
    }
    public void MovePotionShortcut(int slotId, HeroKind heroKind)
    {
        PotionSlot slot = potionSlots[slotId];
        if (!slot.isUnlocked) return;
        if (slot.isEquipSlot)//装備スロットから外す場合
        {
            if (!CanCreatePotion(slot.potion.kind, slot.potion.stackNum.value)) return;
            CreatePotion(slot.potion.kind, slot.potion.stackNum.value);
            slot.potion.Delete();
            return;
        }
        //まず同じポーションを装備しているかどうか
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            int count = i;
            if (potionSlots[count].heroKind == heroKind && IsEquippedPotion(potionSlots[count], slot.potion.kind))
            {
                MovePotion(slotId, potionSlots[count].slotId);
                return;
            }
        }
        //EmptySlot
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            int count = i;
            if (potionSlots[count].heroKind == heroKind && IsEmptyEquipSlot(potionSlots[count]))
            {
                MovePotion(slotId, potionSlots[count].slotId);
                return;
            }
        }
    }
    //PetによるAuto補充
    public void AutoRestorePotion(int slotId)
    {
        if (potionSlots[slotId].potion.globalInfo.type == PotionType.Talisman) return;
        for (int i = 0; i < potionSlots.Length; i++)
        {
            if (potionSlots[i].potion.kind != PotionKind.Nothing && potionSlots[i].potion.globalInfo.type != PotionType.Talisman && !potionSlots[slotId].potion.stackNum.IsMaxed() && potionSlots[i].potion.IsInventory() && potionSlots[i].potion.kind == potionSlots[slotId].potion.kind)
            {
                MovePotion(i, slotId);
                return;
            }
        }
    }
    public void AutoRestorePotion()//装備している全てのPotionに対して行う
    {
        if (!game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.EquipPotion)) return;
        for (int i = potionSlotId; i < potionSlots.Length; i++)
        {
            AutoRestorePotion(i);
        }
    }

    public Action slotUIAction;
    public void SlotUIActionWithPotionSlot()
    {
        if (slotUIAction != null) slotUIAction();
        UpdateCanCreatePotion();
        UpdatePotionEquipNum();
    }
    public EquipmentSlot[] equipmentSlots = new EquipmentSlot[allEquipmentSlotId];
    public EnchantSlot[] enchantSlots = new EnchantSlot[enchantSlotId];
    public PotionSlot[] potionSlots = new PotionSlot[allPotionSlotId];
    //Inventory開放数
    public Multiplier equipInventoryUnlockedNum;
    public Multiplier[] equipWeaponUnlockedNum = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] equipArmorUnlockedNum = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier[] equipJewelryUnlockedNum = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];
    public Multiplier enchantUnlockedNum;
    public Multiplier potionUnlockedNum;
    public Multiplier[] equipPotionUnlockedNum = new Multiplier[Enum.GetNames(typeof(HeroKind)).Length];

    void SetUnlockedNum()
    {
        equipInventoryUnlockedNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 26));
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            equipWeaponUnlockedNum[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
            equipArmorUnlockedNum[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
            equipJewelryUnlockedNum[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        }
        enchantUnlockedNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 6));
        potionUnlockedNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 6));
        for (int i = 0; i < equipPotionUnlockedNum.Length; i++)
        {
            equipPotionUnlockedNum[i] = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        }
    }
}

public class EquipmentSlot
{
    public bool isEquipmentSlot;//これがEQスロットである
    public HeroKind heroKind;
    public EquipmentPart equipmentPart;
    public Equipment equipment;
    public int slotId;//inventoryの並ぶ順番と同期している
    public int equipmentId { get => main.SR.equipmentId[slotId]; set => main.SR.equipmentId[slotId] = value; }//equipmentの登録番号のようなもの/
    public EquipmentSlot(int slotId)
    {
        this.slotId = slotId;
        SetSlotKind();
        equipment = new Equipment(equipmentId);
    }
    public void Start()
    {
        equipment.SetEffect();
        equipment.UpdateSlotId(slotId);
    }
    void SetSlotKind()
    {
        if (slotId < equipInventorySlotId) return;//Inventoryの場合
        isEquipmentSlot = true;
        int tempId = slotId - equipInventorySlotId;
        heroKind = (HeroKind)(tempId / (equipPartSlotId * Enum.GetNames(typeof(EquipmentPart)).Length));
        equipmentPart = (EquipmentPart)((tempId % (equipPartSlotId * Enum.GetNames(typeof(EquipmentPart)).Length)) / equipPartSlotId);
    }
    public void ChangePlace(Equipment equipment, int equipmentId)
    {
        this.equipment = equipment;
        this.equipmentId = equipmentId;
        this.equipment.UpdateSlotId(slotId);
    }    

    public bool isUnlocked//このスロットが開放されている
    {
        get
        {
            if (!isEquipmentSlot) return slotId < game.inventoryCtrl.equipInventoryUnlockedNum.Value();
            switch (equipmentPart)
            {
                case EquipmentPart.Weapon:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.inventoryCtrl.equipWeaponUnlockedNum[(int)heroKind].Value();
                case EquipmentPart.Armor:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.inventoryCtrl.equipArmorUnlockedNum[(int)heroKind].Value();
                case EquipmentPart.Jewelry:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.inventoryCtrl.equipJewelryUnlockedNum[(int)heroKind].Value();
            }
            return false;
        }
    }
    public bool isAvailableEQ//Challenge,Missionなどで制限されているか
    {
        get
        {
            if (!isEquipmentSlot) return true;
            switch (equipmentPart)
            {
                case EquipmentPart.Weapon:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.battleCtrls[(int)heroKind].limitedEQWeaponNum;
                case EquipmentPart.Armor:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.battleCtrls[(int)heroKind].limitedEQArmorNum;
                case EquipmentPart.Jewelry:
                    return (slotId - equipInventorySlotId) % equipPartSlotId < game.battleCtrls[(int)heroKind].limitedEQJewelryNum;
            }
            return false;
        }
    }

    public void UpdatePerSec(double deltaTime)
    {
        if (isEquipped && game.battleCtrls[(int)heroKind].isActiveBattle)
            equipment.GetProficiency(deltaTime, heroKind);
    }
    public bool isEquipped => isUnlocked && equipment.IsEquipped(heroKind);
}

public class EnchantSlot
{
    public EnchantSlot(int slotId)
    {
        this.slotId = slotId;
        enchant = new EquipmentEnchant(enchantId);
        enchant.UpdateSlotId(slotId);
    }
    public void ChangePlace(EquipmentEnchant enchant, int enchantId)
    {
        this.enchant = enchant;
        this.enchantId = enchantId;
        this.enchant.UpdateSlotId(slotId);
    }

    public int slotId;//inventoryの並ぶ順番と同期している
    public int enchantId { get => main.SR.enchantId[slotId]; set => main.SR.enchantId[slotId] = value; }//enchantの登録番号のようなもの/
    public EquipmentEnchant enchant;
    public bool isUnlocked { get { return slotId < game.inventoryCtrl.enchantUnlockedNum.Value(); } }
}


public class PotionSlot
{
    public PotionSlot(int slotId)
    {
        this.slotId = slotId;
        potion = new Potion(potionId);
    }
    public void Start()
    {
        //potion.SetEffect();
        potion.UpdateSlotId(slotId);
    }
    public void ChangePlace(Potion potion, int potionId)
    {
        this.potion = potion;
        this.potionId = potionId;
        this.potion.UpdateSlotId(slotId);
    }

    public int slotId;//inventoryの並ぶ順番と同期している
    public int potionId { get => main.SR.potionId[slotId]; set => main.SR.potionId[slotId] = value; }//potionの登録番号のようなもの/
    public Potion potion;
    public bool isUnlocked { get { if (isEquipSlot) return (slotId - potionSlotId) % equipPotionSlotId < game.inventoryCtrl.equipPotionUnlockedNum[(int)heroKind].Value(); return slotId < game.inventoryCtrl.potionUnlockedNum.Value(); } }
    public bool isEquipSlot { get => slotId >= potionSlotId; }
    public HeroKind heroKind { get { if (slotId < potionSlotId) return HeroKind.Warrior; return (HeroKind)((slotId - potionSlotId) / equipPotionSlotId); } }
}