using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using System;
using Cysharp.Threading.Tasks;


public partial class Save
{
    public double epicCoin;
    public double epicCoinConsumed;
    public long[] epicStorePurchasedNum;
}

public class EpicStoreController
{
    public EpicCoin epicCoin;
    public List<EPIC_STORE> qol1List = new List<EPIC_STORE>();//最大8つ
    public List<EPIC_STORE> qol2List = new List<EPIC_STORE>();
    public List<EPIC_STORE> qol3List = new List<EPIC_STORE>();
    public List<EPIC_STORE> qol4List = new List<EPIC_STORE>();
    public List<EPIC_STORE> special1List = new List<EPIC_STORE>();
    public List<EPIC_STORE> special2List = new List<EPIC_STORE>();
    public List<EPIC_STORE> special3List = new List<EPIC_STORE>();
    public List<EPIC_STORE> ambition1List = new List<EPIC_STORE>();
    public List<EPIC_STORE> ambition2List = new List<EPIC_STORE>();
    public List<EPIC_STORE> ambition3List = new List<EPIC_STORE>();
    public List<EPIC_STORE> itemList = new List<EPIC_STORE>();
    public EPIC_STORE[][] itemArray = new EPIC_STORE[10][];
    public EpicStoreController()
    {
        epicCoin = new EpicCoin();

        special1List.Add(new EpicStore_NitroMax(this));
        special1List.Add(new EpicStore_AbilityReset(this));
        special1List.Add(new EpicStore_RebirthTier1UpgradeReset(this));
        special1List.Add(new EpicStore_RebirthTier2UpgradeReset(this));
        special1List.Add(new EpicStore_RebirthTier3UpgradeReset(this));
        special1List.Add(new EpicStore_GuildAbilityReset(this));
        special1List.Add(new EpicStore_WorldAscensionTier1UpgradeReset(this));
        //special2List.Add(new EpicStore_RebirthTier4UpgradeReset(this));
        //special2List.Add(new EpicStore_RebirthTier5UpgradeReset(this));
        //special2List.Add(new EpicStore_RebirthTier6UpgradeReset(this));
        qol1List.Add(new EpicStore_DailyQuestECSlot(this));
        qol1List.Add(new EpicStore_DailyQuestRarity(this));
        qol1List.Add(new EpicStore_QueueUpgrade(this));
        qol1List.Add(new EpicStore_QueueAlchemy(this));
        qol1List.Add(new EpicStore_SuperQueueUpgrade(this));
        qol1List.Add(new EpicStore_SuperQueueAlchemy(this));
        qol1List.Add(new EpicStore_EquipHeroAccess(this));
        qol1List.Add(new EpicStore_Convene(this));

        qol2List.Add(new EpicStore_AutoAbilityPreset(this));
        qol2List.Add(new EpicStore_ActivablePet(this));
        qol2List.Add(new EpicStore_DisassembleEQSlot(this));
        qol2List.Add(new EpicStore_DisassembleEQExclude(this));
        qol2List.Add(new EpicStore_AlchemyUnlimitedDisassemble(this));
        qol2List.Add(new EpicStore_CraftUnlimitedDisassemble(this));
        qol2List.Add(new EpicStore_UtilityKeeper(this));

        qol3List.Add(new EpicStore_AdvancedRebirth(this));
        qol3List.Add(new EpicStore_FavoriteArea(this));
        qol3List.Add(new EpicStore_BestExpPerSec(this));
        qol3List.Add(new EpicStore_FavoriteQuest(this));
        qol3List.Add(new EpicStore_SkillLessMpAvailable(this));
        qol3List.Add(new EpicStore_EQLessAbilityAvailable(this));

        qol4List.Add(new EpicStore_SwarmChaser(this));
        qol4List.Add(new EpicStore_LimitSlotAutoBuyNet(this));

        ambition1List.Add(new EpicStore_EQInventory(this));
        ambition1List.Add(new EpicStore_UtilityInventory(this));
        ambition1List.Add(new EpicStore_EQWeaponSlot(this));
        ambition1List.Add(new EpicStore_EQArmorSlot(this));
        ambition1List.Add(new EpicStore_EQJewelrySlot(this));
        ambition1List.Add(new EpicStore_EQPotionSlot(this));
        ambition1List.Add(new EpicStore_SkillSlot(this));
        ambition1List.Add(new EpicStore_GlobalSkillSlot(this));

        ambition2List.Add(new EpicStore_ExpeditionTeamSlot(this));
        ambition2List.Add(new EpicStore_DictionaryReset(this));
        ambition2List.Add(new EpicStore_AreaPrestigeReset(this));
        ambition2List.Add(new EpicStore_Multitrapper(this));

        ambition3List.Add(new EpicStore_IEH1BonusTalisman(this));


        itemList.AddRange(qol1List);
        itemList.AddRange(qol2List);
        itemList.AddRange(qol3List);
        itemList.AddRange(qol4List);
        itemList.AddRange(special1List);
        itemList.AddRange(special2List);
        itemList.AddRange(special3List);
        itemList.AddRange(ambition1List);
        itemList.AddRange(ambition2List);
        itemList.AddRange(ambition3List);

        itemArray[0] = qol1List.ToArray();
        itemArray[1] = qol2List.ToArray();
        itemArray[2] = qol3List.ToArray();
        itemArray[3] = qol4List.ToArray();
        itemArray[4] = special1List.ToArray();
        itemArray[5] = special2List.ToArray();
        itemArray[6] = special3List.ToArray();
        itemArray[7] = ambition1List.ToArray();
        itemArray[8] = ambition2List.ToArray();
        itemArray[9] = ambition3List.ToArray();

        OrderItems();
    }
    public void Start()
    {
        for (int i = 0; i < itemOrderArray.Length; i++)
        {
            itemOrderArray[i].Start();
        }
    }

    public EPIC_STORE Item(EpicStoreKind kind)
    {
        return itemOrderArray[(int)kind];
        //for (int i = 0; i < itemList.Count; i++)
        //{
        //    if (itemList[i].kind == kind) return itemList[i];
        //}
        //return itemList[0];
    }
    //キャッシュする
    EPIC_STORE TempItem(EpicStoreKind kind)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].kind == kind) return itemList[i];
        }
        return itemList[0];
    }
    EPIC_STORE[] itemOrderArray = new EPIC_STORE[Enum.GetNames(typeof(EpicStoreKind)).Length];
    void OrderItems()
    {
        for (int i = 0; i < itemOrderArray.Length; i++)
        {
            EpicStoreKind kind = (EpicStoreKind)i;
            itemOrderArray[i] = TempItem(kind);
        }
    }

    public void CheckHack()
    {
        double temp = (main.S.epicCoin + main.S.epicCoinConsumed) * 162464;
        Debug.Log(tDigit(temp));
        Debug.Log(tDigit(main.S.wasd));
        if (temp > main.S.wasd)
        {
            main.S.epicCoin = 0;
            //main.S.epicCoinConsumed = 0;
            main.S.wasd = 0;
        }
    }
}

public class EpicStore_DailyQuestECSlot : EPIC_STORE
{
    public EpicStore_DailyQuestECSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.DailyQuestECSlot;
    public override double price => 2500 * Math.Pow(3, purchasedNum.value);
    public override long purchaseLimitNum => 2;
    public override string NameString()
    {
        return "Additional Epic Coin Daily Quest";
    }
    public override string EffectString()
    {
        return "Unlock a new Epic Coin Daily Quest.";
    }
    public override void SetEffect()
    {
        game.questCtrl.Quest(QuestKindDaily.EC3).unlockConditions.Add(() => IsPurchased(1));
        game.questCtrl.Quest(QuestKindDaily.EC4).unlockConditions.Add(() => IsPurchased(2));
    }
}
public class EpicStore_DailyQuestRarity : EPIC_STORE
{
    public EpicStore_DailyQuestRarity(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.DailyQuestRarity;
    public override double price => 7500;
    public override long purchaseLimitNum => 2;
    public override string NameString()
    {
        return "Improving Minimum Daily Quest Rarity";
    }
    public override string EffectString()
    {
        string tempStr = optStr + "Improve Daily Quest rarity chances: ";
        if (IsPurchased(1))
        {
            //tempStr += "\nCurrent : ";
            //tempStr += "Common (<color=green>Removed</color>)";
            //tempStr += ", Uncommon (" + percent(Parameter.dailyQuestRarityChance1[1], 0) + ")";
            //tempStr += ", Rare (" + percent(Parameter.dailyQuestRarityChance1[2], 0) + ")";
            //tempStr += ", Super Rare (" + percent(Parameter.dailyQuestRarityChance1[3], 0) + ")";
            //tempStr += ", Epic (" + percent(Parameter.dailyQuestRarityChance1[4], 0) + ")";
            //tempStr += "\nAfter Purchase : ";
            tempStr += "\nCommon ( Removed to <color=green>Removed</color> )";
            tempStr += ",  Uncommon ( " + percent(Parameter.dailyQuestRarityChance1[1], 0) + " to <color=green>Removed</color> )";
            tempStr += ",  Rare ( " + percent(Parameter.dailyQuestRarityChance1[2], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance2[2], 0) + "</color> )";
            tempStr += ",  Super Rare ( " + percent(Parameter.dailyQuestRarityChance1[3], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance2[3], 0) + "</color> )";
            tempStr += ",  Epic ( " + percent(Parameter.dailyQuestRarityChance1[4], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance2[4], 0) + "</color> )";
            //tempStr += "\nCommon ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[0]) + " ) : " + percent(Parameter.dailyQuestRarityChance1[0], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance2[0], 0);
            //tempStr += ", Uncommon ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[1]) + " ) : " + percent(Parameter.dailyQuestRarityChance1[1], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance2[1], 0);
            //tempStr += ", Rare ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[2]) + " ) : " + percent(Parameter.dailyQuestRarityChance1[2], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance2[2], 0);
            //tempStr += ", Super Rare ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[3]) + " ) : " + percent(Parameter.dailyQuestRarityChance1[3], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance2[3], 0);
            //tempStr += ", Epic ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[4]) + " ) : " + percent(Parameter.dailyQuestRarityChance1[4], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance2[4], 0);
        }
        else//一度も購入してない
        {
            //tempStr += "\nCurrent : ";
            //tempStr += "Common ( " + percent(Parameter.dailyQuestRarityChance0[0], 0) + " )";
            //tempStr += ", Uncommon ( " + percent(Parameter.dailyQuestRarityChance0[1], 0) + " )";
            //tempStr += ", Rare ( " + percent(Parameter.dailyQuestRarityChance0[2], 0) + " )";
            //tempStr += ", Super Rare ( " + percent(Parameter.dailyQuestRarityChance0[3], 0) + " )";
            //tempStr += ", Epic ( " + percent(Parameter.dailyQuestRarityChance0[4], 0) + " )";
            //tempStr += "\nAfter Purchase : ";
            tempStr += "\nCommon ( " + percent(Parameter.dailyQuestRarityChance0[0], 0) + " to <color=green>Removed</color> )";
            tempStr += ",  Uncommon ( " + percent(Parameter.dailyQuestRarityChance0[1], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance1[1], 0) + "</color> )";
            tempStr += ",  Rare ( " + percent(Parameter.dailyQuestRarityChance0[2], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance1[2], 0) + "</color> )";
            tempStr += ",  Super Rare ( " + percent(Parameter.dailyQuestRarityChance0[3], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance1[3], 0) + "</color> )";
            tempStr += ",  Epic ( " + percent(Parameter.dailyQuestRarityChance0[4], 0) + " to <color=green>" + percent(Parameter.dailyQuestRarityChance1[4], 0) + "</color> )";
            //tempStr += "\nCommon ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[0]) + " ) : " + percent(Parameter.dailyQuestRarityChance0[0], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance1[0], 0);
            //tempStr += ", Uncommon ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[1]) + " ) : " + percent(Parameter.dailyQuestRarityChance0[1], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance1[1], 0);
            //tempStr += ", Rare ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[2]) + " ) : " + percent(Parameter.dailyQuestRarityChance0[2], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance1[2], 0);
            //tempStr += ", Super Rare ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[3]) + " ) : " + percent(Parameter.dailyQuestRarityChance0[3], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance1[3], 0);
            //tempStr += ", Epic ( <sprite=\"EpicCoin\" index=0> " + tDigit(Parameter.dailyQuestRewardEC[4]) + " ) : " + percent(Parameter.dailyQuestRarityChance0[4], 0);
            //tempStr += " to " + percent(Parameter.dailyQuestRarityChance1[4], 0);
        }
        tempStr += "\nThis effect is active from tomorrow.";
        return tempStr;
    }
}
public class EpicStore_NitroMax : EPIC_STORE
{
    public EpicStore_NitroMax(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.NitroMax;
    public override double price => 500;
    public override void PurchaseAction()
    {
        game.nitroCtrl.nitro.ToMaxWithoutLimit();
    }
    public override string NameString()
    {
        return "Nitro Max Charger";
    }
    public override string EffectString()
    {
        string tempStr = optStrL + "You will instantly gain Nitro in the amount of Nitro Cap. The amount of nitro in excess of the Nitro Cap is also retained. \nCurrent Gain per Purchase : <color=green>" + tDigit(game.nitroCtrl.nitroCap.Value()) + " Nitro</color>";
        if (game.nitroCtrl.nitro.IsMaxed()) tempStr += "\n<color=yellow>You cannot purchase this item until Nitro has fallen below the cap.</color>";
        return tempStr;
    }
    public override bool BuyCondition()
    {
        return !game.nitroCtrl.nitro.IsMaxed();
    }
}
public class EpicStore_AbilityReset : EPIC_STORE
{
    public EpicStore_AbilityReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AbilityReset;
    public override double price => 200;
    public override bool isFreeTheFirst => true;
    public override void PurchaseAction()
    {
        game.statsCtrl.ResetAbilityPoint(game.currentHero, true);
    }
    public override string NameString()
    {
        return "Ability Reset";
    }
    public override string EffectString()
    {
        return "Resets Ability and restore Ability Points of current playing Hero.";
    }
}
public class EpicStore_WorldAscensionTier1UpgradeReset : EPIC_STORE
{
    ConfirmUI confirmUI;
    public EpicStore_WorldAscensionTier1UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override void Start()
    {
        confirmUI = new ConfirmUI(GameControllerUI.gameUI.popupCtrlUI.defaultConfirm);
    }
    public override EpicStoreKind kind => EpicStoreKind.WorldAscensionTier1UpgradeReset;
    public override double price => 5500;
    public override void PurchaseAction()
    {
        confirmUI.SetUI("YOU WILL LOSE CURRENT PROGRESS\nYou're about to reset your World Ascension points, which will reset your game as though you performed Tier 1 World Ascension, but you won't receive any bonuses.");
        SetActive(confirmUI.quitButton.gameObject, true);
        confirmUI.okButton.onClick.RemoveAllListeners();
        confirmUI.quitButton.onClick.RemoveAllListeners();
        confirmUI.quitButton.onClick.AddListener(confirmUI.Hide);
        confirmUI.quitButton.onClick.AddListener(() => game.epicStoreCtrl.epicCoin.Increase(price));
        confirmUI.okButton.onClick.AddListener(() =>
        {
            confirmUI.SetUI("Okay, we warned you, you are sure you want to do this? We recommend you only do this right AFTER you perform World Ascension so you don't lose progress...");
            confirmUI.okButton.onClick.RemoveAllListeners();
            if (game.ascensionCtrl.worldAscensions[0].PointGain() >= 1)
                confirmUI.okButton.onClick.AddListener(() =>
                {
                    confirmUI.SetUI("Oh ho ho, you just lost " + tDigit(game.ascensionCtrl.worldAscensions[0].PointGain()) + " points, silly n00bian! Next time perform your reset before you progress into a new World Ascension! Now we mock you from the shadows, hahAHahHaaaa....wheeze... okay we're done here.");
                    SetActive(confirmUI.quitButton.gameObject, false);
                    confirmUI.okButton.onClick.RemoveAllListeners();
                    confirmUI.okButton.onClick.AddListener(() =>
                    {
                        game.ascensionCtrl.worldAscensions[0].Reset();
                        confirmUI.Hide();
                    });
                });
            else
            {
                confirmUI.okButton.onClick.AddListener(() =>
                {
                    game.ascensionCtrl.worldAscensions[0].Reset();
                    confirmUI.Hide();
                });
            }
        });
    }
    public override string NameString()
    {
        return "Tier 1 World Ascension Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 1 World Ascension Upgrades and restore Tier 1 Points. It also performs a reset equivalent to Tier 1 World Ascension without gaining points from current progress. The best time to use this would be right after World Ascension.";
    }
}

public class EpicStore_AreaPrestigeReset : EPIC_STORE
{
    public EpicStore_AreaPrestigeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AreaPrestigeRest;
    public override double price => 15000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Area/Dungeon Prestige Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Enables a function of resetting Area/Dungeon Prestige Upgrades permanently. You will be able to reset them and restore its points whenever you want. When you perform reset, the area/dungeon difficulty will be back to 1 automatically.";
    }
}


public class EpicStore_RebirthTier1UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier1UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier1UpgradeReset;
    public override double price => 500;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 0).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 1 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 1 Rebirth Upgrades and restore points of current playing Hero.";
    }
}
public class EpicStore_RebirthTier2UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier2UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier2UpgradeReset;
    public override double price => 750;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 1).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 2 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 2 Rebirth Upgrades and restore points of current playing Hero.";    }
}
public class EpicStore_RebirthTier3UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier3UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier3UpgradeReset;
    public override double price => 1000;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 2).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 3 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 3 Rebirth Upgrades and restore points of current playing Hero." +
            " Keep in mind that items in the expanded equipment slots are not available until you unlock the slots again.";
    }
}
public class EpicStore_RebirthTier4UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier4UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier4UpgradeReset;
    public override double price => 1250;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 3).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 4 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 4 Rebirth Upgrades and restore Rebirth points of current playing Hero.";
    }
}
public class EpicStore_RebirthTier5UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier5UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier5UpgradeReset;
    public override double price => 1500;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 4).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 5 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 5 Rebirth Upgrades and restore Rebirth points of current playing Hero.";
    }
}
public class EpicStore_RebirthTier6UpgradeReset : EPIC_STORE
{
    public EpicStore_RebirthTier6UpgradeReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.RebirthTier1UpgradeReset;
    public override double price => 2000;
    public override void PurchaseAction()
    {
        game.rebirthCtrl.Rebirth(game.currentHero, 5).ResetRebirthUpgrade();
    }
    public override string NameString()
    {
        return "Tier 6 Rebirth Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Resets Tier 6 Rebirth Upgrades and restore Rebirth points of current playing Hero.";
    }
}


public class EpicStore_GuildAbilityReset : EPIC_STORE
{
    public EpicStore_GuildAbilityReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.GuildAbilityReset;
    public override double price => 2500;
    public override bool isFreeTheFirst => true;
    public override void PurchaseAction()
    {
        game.guildCtrl.ResetGuildAbility();
    }
    public override string NameString()
    {
        return "Guild Ability Reset";
    }
    public override string EffectString()
    {
        return "Resets Guild Ability and restore Guild Ability Points." +
            " Keep in mind that items in the expanded inventory slots are not available until you unlock the slots again." +
            " You also may need to allocate Mysterious Water into catalysts again.";
    }
}
public class EpicStore_DictionaryReset : EPIC_STORE
{
    public EpicStore_DictionaryReset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.DictionaryReset;
    public override double price => 3500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Dictionary Upgrade Reset";
    }
    public override string EffectString()
    {
        return "Enables a function of resetting Dictionary Upgrades permanently. You will be able to reset them and restore its points whenever you want.";
    }
}
public class EpicStore_ActivablePet : EPIC_STORE
{
    public EpicStore_ActivablePet(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.PetActivableNum;
    public override double price => 5000;
    public override long purchaseLimitNum => 5;
    public override string NameString()
    {
        return "Active Pet Slot + 5";
    }
    public override string EffectString()
    {
        return "Increases Active Pet Slot by 5.";
    }
    public override void SetEffect()
    {
        game.monsterCtrl.petActiveCap.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => 5 * purchasedNum.value, () => IsPurchased()));
    }
}

public class EpicStore_DisassembleEQSlot : EPIC_STORE
{
    public EpicStore_DisassembleEQSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.DisassembleEQSlot;
    public override double price => 2000;
    public override long purchaseLimitNum => 5;
    public override string NameString()
    {
        return "Auto-Disassemble Equipment Slot + 5";
    }
    public override string EffectString()
    {
        return "Increases Auto-Disassemble Equipment Slot by 5. Click an equipment in dictionary to assign/remove auto-disassemble when picked up.";
    }
    public override void SetEffect()
    {
        game.equipmentCtrl.autoDisassembleAvailableNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.Pet, MultiplierType.Add, () => 5 * purchasedNum.value, () => IsPurchased()));
    }
}
public class EpicStore_DisassembleEQExclude : EPIC_STORE
{
    public EpicStore_DisassembleEQExclude(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.DisassembleEQExclude;
    public override double price => 5500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Disassembling Equipment";
    }
    public override string EffectString()
    {
        return "Unleashes a toggle in Equipment Dictionary that enables a function of excluding enchanted equipment to auto-dissasemble.";
    }
}
public class EpicStore_AlchemyUnlimitedDisassemble : EPIC_STORE
{
    public EpicStore_AlchemyUnlimitedDisassemble(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AlchemyUnlimitedDisassemble;
    public override double price => 5500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Disassemble Potion";
    }
    public override string EffectString()
    {
        return "Allows the Auto-Disassemble function from pets to ignore the potion inventory spaces when auto-disassembling potions. (You need the relevant pet activated to make this work)";
        //return "Enables unlimitedly product and disassemble potions at once regardless the empty slots. ( You need to activate the pet to enable the immediate disassemble )";
    }
}
public class EpicStore_CraftUnlimitedDisassemble : EPIC_STORE
{
    public EpicStore_CraftUnlimitedDisassemble(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.CraftUnlimitedDisassemble;
    public override double price => 9500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Disassemble Crafted Equipment";
    }
    public override string EffectString()
    {
        return "Allows the Auto-Disassemble function from pets to use the Multiplier on the top left when auto-disassembling crafted equipment. (You need the relevant pet activated to make this work)";
        //return "Enables unlimitedly product and disassemble potions at once regardless the empty slots. ( You need to activate the pet to enable the immediate disassemble )";
    }
}
public class EpicStore_QueueUpgrade : EPIC_STORE
{
    public EpicStore_QueueUpgrade(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.UpgradeQueue;
    public override double price => 500 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 5;
    public override string NameString()
    {
        return "Upgrade Queue + 5";
    }
    public override string EffectString()
    {
        return "Increase Upgrade's Available Queue by 5. This queue is available for Slime Bank Upgrades too. Right-Click on an upgrade to assign Queue, while Shift + Left-Click removes Queue.";
    }
    public override void SetEffect()
    {
        game.upgradeCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => 5 * purchasedNum.value, () => IsPurchased()));
    }
}
public class EpicStore_SuperQueueUpgrade : EPIC_STORE
{
    public EpicStore_SuperQueueUpgrade(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.SuperQueueUpgrade;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Super Queue";
    }
    public override string EffectString()
    {
        return "Enables to hit \"Q\" key on an upgrade to assign Super Queue by exchanging 10 queue that permanently buy it, while Shift + \"Q\" key removes it.";
    }
}
public class EpicStore_QueueAlchemy : EPIC_STORE
{
    public EpicStore_QueueAlchemy(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AlchemyQueue;
    public override double price => 500 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 5;
    public override string NameString()
    {
        return "Alchemy Queue + 10";
    }
    public override string EffectString()
    {
        return "Increase Alchemy's Available Queue by 10. Right-Click on an upgrade to assign Queue, while Shift + Left-Click removes Queue.";
    }
    public override void SetEffect()
    {
        game.potionCtrl.availableQueue.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => 10 * purchasedNum.value, () => IsPurchased()));
    }
}
public class EpicStore_SuperQueueAlchemy : EPIC_STORE
{
    public EpicStore_SuperQueueAlchemy(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.SuperQueueAlchemy;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Super Queue for Alchemy";
    }
    public override string EffectString()
    {
        return "Enables to hit \"Q\" key on an potion to assign Super Queue by exchanging 10 queue that permanently alchemise it until one Utility Inventory slot is filled with the potion, while Shift + \"Q\" key removes it.";
    }
}
public class EpicStore_LimitSlotAutoBuyNet : EPIC_STORE
{
    public EpicStore_LimitSlotAutoBuyNet(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.LimitSlotAutoBuyNet;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Buy Traps";
    }
    public override string EffectString()
    {
        return "Unlocks a toggle in Settings tab that modify pets' [ Auto-buy Traps in shop ] active ability to stop buying nets after at least one Utility inventory slot is filled with the net. Turning the toggle off will allow all trap-buying pets to fill your Utility inventory with nets again.";
    }
}
public class EpicStore_SkillSlot : EPIC_STORE
{
    public EpicStore_SkillSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.SkillSlot;
    public override double price => 25000;
    public override long purchaseLimitNum => 1;
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            game.statsCtrl.SkillSlotNum((HeroKind)count).RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
        }
    }
    public override void PurchaseAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
    public override string NameString()
    {
        return "Class Skill Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Class Skill Slot for all heroes.";
    }
}
public class EpicStore_GlobalSkillSlot : EPIC_STORE
{
    public EpicStore_GlobalSkillSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.GlobalSkillSlot;
    public override double price => 45000;
    public override long purchaseLimitNum => 1;
    public override void SetEffect()
    {
        game.statsCtrl.globalSkillSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
    }
    public override void PurchaseAction()
    {
        GameControllerUI.gameUI.battleStatusUI.SetSkillSlot();
    }
    public override string NameString()
    {
        return "Global Skill Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Global Skill Slot for all heroes.";
    }
}
public class EpicStore_EQInventory : EPIC_STORE
{
    public EpicStore_EQInventory(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQInventory;
    public override double price => 5000;
    public override long purchaseLimitNum => 5;
    public override void SetEffect()
    {
        game.inventoryCtrl.equipInventoryUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => 10 * purchasedNum.value, () => IsPurchased()));
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.slotUIAction();
    }

    public override string NameString()
    {
        return "Equipment Inventory Slot + 10";
    }
    public override string EffectString()
    {
        return "Unleashes 10 Equipment Inventory Slot.";
    }
}
public class EpicStore_EQWeaponSlot : EPIC_STORE
{
    public EpicStore_EQWeaponSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQWeaponSlot;
    public override double price => 5000 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 3;
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            game.inventoryCtrl.equipWeaponUnlockedNum[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
        }
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.slotUIAction();
    }
    public override string NameString()
    {
        return "Equipment Weapon Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Equipment Weapon Slot for all heroes.";
    }
}
public class EpicStore_EQArmorSlot : EPIC_STORE
{
    public EpicStore_EQArmorSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQArmorSlot;
    public override double price => 5000 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 3;
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            game.inventoryCtrl.equipArmorUnlockedNum[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
        }
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.slotUIAction();
    }
    public override string NameString()
    {
        return "Equipment Armor Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Equipment Armor Slot for all heroes.";
    }
}
public class EpicStore_EQJewelrySlot : EPIC_STORE
{
    public EpicStore_EQJewelrySlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQJewelrySlot;
    public override double price => 5000 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 3;
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            game.inventoryCtrl.equipJewelryUnlockedNum[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
        }
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.slotUIAction();
    }
    public override string NameString()
    {
        return "Equipment Jewelry Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Equipment Jewelry Slot for all heroes.";
    }
}
public class EpicStore_EQPotionSlot : EPIC_STORE
{
    public EpicStore_EQPotionSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQPotionSlot;
    public override double price => 10000 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 2;
    public override void SetEffect()
    {
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            game.inventoryCtrl.equipPotionUnlockedNum[count].RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
        }
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }
    public override string NameString()
    {
        return "Equip Utility Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Equip Utility Slot for all heroes.";
    }
}
public class EpicStore_UtilityInventory : EPIC_STORE
{
    public EpicStore_UtilityInventory(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.UtilityInventory;
    public override double price => 5000;
    public override long purchaseLimitNum => 10;
    public override void SetEffect()
    {
        game.inventoryCtrl.potionUnlockedNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => 5 * purchasedNum.value, () => IsPurchased()));
    }
    public override void PurchaseAction()
    {
        SlotUIAction();
    }
    async void SlotUIAction()
    {
        await UniTask.DelayFrame(60);
        game.inventoryCtrl.SlotUIActionWithPotionSlot();
    }
    public override string NameString()
    {
        return "Utility Inventory Slot + 5";
    }
    public override string EffectString()
    {
        return "Unleashes 5 Utility Inventory Slot.";
    }
}

public class EpicStore_ExpeditionTeamSlot : EPIC_STORE
{
    public EpicStore_ExpeditionTeamSlot(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.ExpeditionTeamSlot;
    public override double price => 7500 * Math.Pow(2, purchasedNum.value);
    public override long purchaseLimitNum => 2;
    public override void SetEffect()
    {
        game.expeditionCtrl.unlockedExpeditionSlotNum.RegisterMultiplier(new MultiplierInfo(MultiplierKind.EpicStore, MultiplierType.Add, () => purchasedNum.value, () => IsPurchased()));
    }
    public override string NameString()
    {
        return "Expedition Team Slot + 1";
    }
    public override string EffectString()
    {
        return "Unleashes 1 Expedition Team Slot.";
    }
}



public class EpicStore_SkillLessMpAvailable : EPIC_STORE
{
    public EpicStore_SkillLessMpAvailable(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.SkillLessMpAvailable;
    public override double price => 4000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Skill Tenacity";
    }
    public override string EffectString()
    {
        return "Unleashes a toggle in Skill tab that allows you to trigger a skill as soon as its cooltime filled regardless of its MP Consumption, with a proportional reduction in the effect and proficiency gain if MP Consumption isn't met : ([hero's current MP] / [skill's MP Consumption])%, with a minimum of 10%";
    }
}
public class EpicStore_EQLessAbilityAvailable : EPIC_STORE
{
    public EpicStore_EQLessAbilityAvailable(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.EQLessAbilityAvailable;
    public override double price => 4000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Equipment Tenacity";
    }
    public override string EffectString()
    {
        return "Allows you to equip equipment that you do not have the requirements for, with a proportional reduction in the effect : ([Hero Level and Abilities] / [Equipment's required Ability])%, with a minimum of 10%";
        //return "Enables to activate Equipment that has ability-requirements that are unmet but still equipped just after rebirthing or reseting ability points, with % penalty of its effect and proficiency gain. Penalty : ([Hero Level and Abilities] / [Equipment's required Ability])%, with a minimum of 10%";
    }
}
public class EpicStore_AdvancedRebirth : EPIC_STORE
{
    public EpicStore_AdvancedRebirth(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AdvancedRebirth;
    public override double price => 1500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Rebirth [ Timing ]";
    }
    public override string EffectString()
    {
        return "Enables to customize Rebirth Point and Hero Level that you execute Auto-Rebirth. (You need the relevant pet activated to make auto-rebirth work)";
    }
}
public class EpicStore_FavoriteArea : EPIC_STORE
{
    public EpicStore_FavoriteArea(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.FavoriteArea;
    public override double price => 1500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Rebirth [ Favorite Area ]";
    }
    public override string EffectString()
    {
        return "Enables to customize Favorite Area that you will start in just after rebirth. (You need the relevant pet activated to make auto-rebirth work)";
    }
}
public class EpicStore_BestExpPerSec : EPIC_STORE
{
    public EpicStore_BestExpPerSec(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.BestExpPerSec;
    public override double price => 3500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto-Rebirth [ Traveling Best EXP Area ]";
    }
    public override string EffectString()
    {
        return "Unleashes a toggle that enables to automatically go to the best EXP/sec area every 25th Hero Level. This function is available only for the current playing hero.  (You need the relevant pet activated to make auto-rebirth work)";
    }
}
public class EpicStore_AutoAbilityPreset : EPIC_STORE
{
    public EpicStore_AutoAbilityPreset(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.AutoAbilityPreset;
    public override double price => 3500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Advanced Auto Ability Point Adder";
    }
    public override string EffectString()
    {
        return "Enables the option to set maximum amount of Ability Points for each Ability in the Auto Ability Point Adder tab. The Auto Ability Point Adder will not allocate ability points over the maximum.  (You need to unlock the Auto Ability Point Adder first, through the tutorial quest)";
    }
}
public class EpicStore_FavoriteQuest : EPIC_STORE
{
    public EpicStore_FavoriteQuest(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.FavoriteQuest;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Favorite Quest";
    }
    public override string EffectString()
    {
        return "Enables to assign a General Quest as a favorite, which will automatically be accepted and cleared when the requirements are met. The available slots of Favorite Quest is the same as your acceptable quest limit.";
    }
}
public class EpicStore_Multitrapper : EPIC_STORE
{
    public EpicStore_Multitrapper(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.Multitrapper;
    public override double price => 10000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Multitrapper";
    }
    public override string EffectString()
    {
        return "Enables to equip and use multiple kinds of traps to capture multiple color monsters at a time.";
    }
}
public class EpicStore_UtilityKeeper : EPIC_STORE
{
    public EpicStore_UtilityKeeper(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.UtilityKeeper;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Utility Keeper";
    }
    public override string EffectString()
    {
        return "Enables to keep equipping utility items even at stack # 0. This allows the Pet Active Effect of Normal Magicslime to remain active.";
    }
}
public class EpicStore_Convene : EPIC_STORE
{
    public EpicStore_Convene(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.Convene;
    public override double price => 5000;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Convene";
    }
    public override string EffectString()
    {
        return "Unlocks a button on battlefield screen that convenes all passive heroes in background to the area where the current playing hero is. Purchasing [Favorite Area] makes all heroes go to their favorite area when you Shift + Click the button.";
    }
}
public class EpicStore_SwarmChaser : EPIC_STORE
{
    public EpicStore_SwarmChaser(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.SwarmChaser;
    public override double price => 9500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Swarm Chaser";
    }
    public override string EffectString()
    {
        return "Unlocks a toggle in Settings tab that enables to automatically go to a Swarming Area. During Swarms, [Auto-Rebirth] and [Traveling Best EXP Area] are disabled. Purchasing [Convene] makes all passive heroes go. Purchasing [Favorite Area] makes all heroes go to their favorite area just after the Swarm.";
    }
}
public class EpicStore_EquipHeroAccess : EPIC_STORE
{
    public EpicStore_EquipHeroAccess(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override void PurchaseAction()
    {
        game.inventoryCtrl.slotUIAction();
    }
    public override EpicStoreKind kind => EpicStoreKind.EquipHeroAccess;
    public override double price => 3500;
    public override long purchaseLimitNum => 1;
    public override string NameString()
    {
        return "Easy Access";
    }
    public override string EffectString()
    {
        return "Unlocks buttons in Equip tab that allows you to access to the equipment slots of background heroes without switching heroes.";
    }
}
public class EpicStore_IEH1BonusTalisman : EPIC_STORE
{
    public EpicStore_IEH1BonusTalisman(EpicStoreController epicStoreCtrl) : base(epicStoreCtrl)
    {
    }
    public override void PurchaseAction()
    {
    }
    public override EpicStoreKind kind => EpicStoreKind.IEH1BonusTalisman;
    public override double price => 2500;
    public override long purchaseLimitNum => 20 - main.S.checkedIEH1Achievement;
    public override bool isOnetimePurchase => false;
    public override string NameString()
    {
        return "Bribes for IEH1";
    }
    public override string EffectString()
    {
        return "Gives you 1 fictitious IEH1 Steam Achievement that enables you to claim an extra IEH1 Player Bonus in Settings > Bonus tab without actually getting the achievement in IEH1. You can claim at most 20 IEH1 Steam Achievements (real + fictitious) in total.";
    }
    public override bool BuyCondition()
    {
        //実際のSteamAchievement獲得数とたしあわせて20個まで購入できる
        if (main.S.checkedIEH1Achievement + purchasedNum.value >= 20) return false;
        return base.BuyCondition();
    }
}


public class EpicCoin : NUMBER
{
    public override double value { get => main.S.epicCoin; set => main.S.epicCoin = value; }
    public override string Name()
    {
        return "Epic Coin";
    }
    public override void Increase(double increment)
    {
        base.Increase(increment);
        main.S.wasd += increment * 162464;
    }
    public override void Decrease(double decrement)
    {
        base.Decrease(decrement);
        main.S.epicCoinConsumed += decrement;
    }
}

public class EPIC_STORE
{
    public EPIC_STORE(EpicStoreController epicStoreCtrl)
    {
        this.epicStoreCtrl = epicStoreCtrl;
        purchasedNum = new EpicStorePurchasedNum(this);
        transaction = new Transaction(purchasedNum, epicStoreCtrl.epicCoin, Price);
        transaction.isBuyOne = true;
        transaction.SetAdditionalBuyCondition(CanPurchase);
        transaction.additionalBuyActionWithLevelIncrement = (x) => PurchaseAction();
        //transaction.additionalBuyAction = () => GameControllerUI.gameUI.soundUI.Play(SoundEffect.Upgrade);
        SetEffect();
    }
    public virtual void Start() { }
    public EpicStoreController epicStoreCtrl;
    public virtual EpicStoreKind kind => EpicStoreKind.Nothing;
    public EpicStorePurchasedNum purchasedNum;
    public Transaction transaction;
    public bool IsPurchased(long num = 1) { return purchasedNum.value >= num; }
    public bool IsSoldOut() { return IsPurchased(purchaseLimitNum); }
    public virtual bool BuyCondition() { return true; }
    bool CanPurchase() { return !IsSoldOut() && BuyCondition(); }
    public virtual long purchaseLimitNum => 1000000;
    public virtual bool isOnetimePurchase => purchaseLimitNum <= 1;
    public virtual double price => 100;//debug
    public virtual bool isFreeTheFirst => false;
    public virtual void PurchaseAction() { }
    public virtual void SetEffect() { }
    public virtual string NameString() { return ""; }
    public virtual string EffectString() { return ""; }

    public double Price(long purchasedNum)
    {
        if (isFreeTheFirst && purchasedNum == 0) return 0;
        return price;
    }
}

public class EpicStorePurchasedNum : INTEGER
{
    public EPIC_STORE item;
    public EpicStorePurchasedNum(EPIC_STORE item)
    {
        this.item = item;
    }
    public override long value { get => main.S.epicStorePurchasedNum[(int)item.kind]; set => main.S.epicStorePurchasedNum[(int)item.kind] = value; }
}

public enum EpicStoreKind
{
    Nothing,
    NitroMax,
    AbilityReset,
    GuildAbilityReset,
    DictionaryReset,
    DisassembleEQExclude,
    AlchemyUnlimitedDisassemble,
    SuperQueueUpgrade,
    SuperQueueAlchemy,
    SkillSlot,
    GlobalSkillSlot,
    EQInventory,
    EQWeaponSlot,
    EQArmorSlot,
    EQJewelrySlot,
    SkillLessMpAvailable,//MPが足りない場合、足りない分のダメージ%で攻撃する
    EQLessAbilityAvailable,//Rebirth後、Abilityが足りなくても装備続行、Effect%
    AdvancedRebirth,//RebirthPoint, RebirthLevelを設定できる
    FavoriteArea,
    BestExpPerSec,
    AutoAbilityPreset,
    UpgradeQueue,//+5
    AlchemyQueue,//+10
    DisassembleEQSlot,//+10
    RebirthTier1UpgradeReset,
    RebirthTier2UpgradeReset,
    RebirthTier3UpgradeReset,
    RebirthTier4UpgradeReset,
    RebirthTier5UpgradeReset,
    RebirthTier6UpgradeReset,
    PetActivableNum,
    DailyQuestECSlot,//２回購入
    DailyQuestRarity,//２回購入
    EQPotionSlot,
    UtilityInventory,
    WorldAscensionTier1UpgradeReset,
    FavoriteQuest,
    Multitrapper,
    UtilityKeeper,
    Convene,
    SwarmChaser,
    EquipHeroAccess,
    CraftUnlimitedDisassemble,
    LimitSlotAutoBuyNet,
    AreaPrestigeRest,
    ExpeditionTeamSlot,
    IEH1BonusTalisman,
    //未追加
    //PortalOrb,
}