using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Text;

public class HelpControllerUI : MonoBehaviour
{
    [SerializeField] Button openButton, quitButton, claimButton;
    [SerializeField] Button[] kindButtons, tabButtons;
    TextMeshProUGUI[] tabTexts;
    [SerializeField] TextMeshProUGUI titleText, mainText, achievRewardText;//guildLevelText, townText, challengeText;
    [SerializeField] Scrollbar scrollBar;
    public OpenCloseUI openCloseUI;
    public SwitchTabUI helpStatsSwitchTabUI;
    public SwitchTabUI switchTabUI;
    [SerializeField] GameObject[] achievIcons;

    // Start is called before the first frame update
    void Start()
    {
        helpStatsSwitchTabUI = new SwitchTabUI(kindButtons, true, -1);
        tabTexts = new TextMeshProUGUI[tabButtons.Length];
        for (int i = 0; i < tabTexts.Length; i++)
        {
            tabTexts[i] = tabButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            //tabButtons[i].onClick.AddListener(() => { SetUI(); AdjustScrollBar(); });
        }
        openCloseUI = new OpenCloseUI(gameObject);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(quitButton);
        switchTabUI = new SwitchTabUI(tabButtons, true);
        openCloseUI.openActions.Add(SetUI);
        switchTabUI.openAction = SetUI;
        kindButtons[0].onClick.AddListener(() => { tabButtons[0].onClick.Invoke(); });
        kindButtons[1].onClick.AddListener(() => { tabButtons[(int)HelpKind.S_General].onClick.Invoke(); });
        kindButtons[2].onClick.AddListener(() => { tabButtons[(int)HelpKind.A_All].onClick.Invoke(); });

        kindButtons[0].onClick.Invoke();

        claimButton.onClick.AddListener(() => { game.achievementCtrl.ClaimReward(); ShowAchievIcon(); SetUI(); });
    }
    //async void AdjustScrollBar()
    //{
    //    scrollBar.value = 1;
    //    //mainText.text = HelpString((HelpKind)switchTabUI.currentId);
    //    await UniTask.DelayFrame(1);
    //    SetActive(mainText.gameObject, false);
    //    await UniTask.DelayFrame(1);
    //    SetActive(mainText.gameObject, true);
    //}

    public void ShowAchievIcon()
    {
        bool tempBool = !SettingMenuUI.Toggle(ToggleKind.DisableNotificationAchievement).isOn && game.achievementCtrl.CanClaimReward();
        for (int i = 0; i < achievIcons.Length; i++)
        {
            SetActive(achievIcons[i].gameObject, tempBool);
        }
    }
    int count, countSec;
    public void UpdateUI()
    {
        count++;
        if (count >= 60 * 50)//１分
        {
            ShowAchievIcon();
            count = 0;
        }
        if (!openCloseUI.isOpen) return;
        titleText.text = helpStatsSwitchTabUI.currentId switch
        {
            0 => "HELP",
            1 => "STATISTICS",
            2 => "ACHIEVEMENTS",
            _ => ""
        };
        bool canShow = false;
        for (int i = 0; i < tabTexts.Length; i++)
        {
            int count = i;
            switch (helpStatsSwitchTabUI.currentId)
            {
                case 0:
                    canShow = count <= (int)HelpKind.EpicStore;
                    break;
                case 1:
                    canShow = count >= (int)HelpKind.S_General && count < (int)HelpKind.A_All;
                    break;
                case 2:
                    canShow = count >= (int)HelpKind.A_All && count < Enum.GetNames(typeof(HelpKind)).Length;
                    break;
            }
            SetActive(tabButtons[count].gameObject, canShow);
            if (canShow) 
                tabTexts[i].text = HelpName((HelpKind)i);
        }

        if (helpStatsSwitchTabUI.currentId == 2)
            claimButton.interactable = game.achievementCtrl.CanClaimReward();
        //countSec++;
        //if (countSec >= 50)//１秒
        //{
        //    SetUI();
        //    countSec = 0;
        //}
    }


    async void SetUI()
    {
        mainText.text = HelpString((HelpKind)switchTabUI.currentId);
        switch (helpStatsSwitchTabUI.currentId)
        {
            case 0:
                SetActive(claimButton.gameObject, false);
                SetActive(achievRewardText.gameObject, false);
                break;
            case 1:
                SetActive(claimButton.gameObject, false);
                SetActive(achievRewardText.gameObject, false);
                break;
            case 2:
                SetActive(claimButton.gameObject, true);
                //claimButton.interactable = game.achievementCtrl.CanClaimReward();
                SetActive(achievRewardText.gameObject, true);
                achievRewardText.text = AchievementRewardString((HelpKind)switchTabUI.currentId);
                break;
        }
        scrollBar.value = 1;
        await UniTask.DelayFrame(1);
        SetActive(mainText.gameObject, false);
        await UniTask.DelayFrame(1);
        SetActive(mainText.gameObject, true);
    }

    public string HelpName(HelpKind kind)
    {
        switch (kind)
        {
            case HelpKind.Heroes: return "Heroes";
            case HelpKind.Leveling: return "Leveling";
            case HelpKind.Hotkeys: return "Hotkeys";
            case HelpKind.WorldMap: return "World Map";
            case HelpKind.Battle: return "Battle";
            case HelpKind.Capture: return "Capture";
            case HelpKind.Blessing: return "Blessing";
            case HelpKind.Debuff: return "Debuff";
            case HelpKind.Ability: return "Ability";
            case HelpKind.Title: return "Title";
            case HelpKind.Quest: return "Quest";
            case HelpKind.Skill: return "Skill";
            case HelpKind.Upgrade: return "Upgrade";
            case HelpKind.Equip: return "Equip";
            case HelpKind.Lab: return "Lab";
            case HelpKind.Guild: return "Guild";
            case HelpKind.Town: return "Town";
            case HelpKind.Bestiary: return "Bestiary";
            case HelpKind.Shop: return "Shop";
            case HelpKind.Rebirth: return "Rebirth";
            case HelpKind.Challenge: return "Challenge";
            case HelpKind.Expedition: return "Expedition";
            case HelpKind.WorldAscension: return "World Ascension";
            case HelpKind.EpicStore: return "Epic Store";
            case HelpKind.S_General: return "General";
            case HelpKind.S_GuildLevel: return "Guild";
            case HelpKind.S_Town: return "Town";
            case HelpKind.S_Rebirth: return "Rebirth";
            case HelpKind.S_Challenge: return "Challenge";
            case HelpKind.S_WorldAscension: return "World Ascension";
            case HelpKind.A_All: return "All";
            case HelpKind.A_General:return "General";
            case HelpKind.A_Area:return "Area";
            case HelpKind.A_Currency: return"Currency";
            case HelpKind.A_Guild:return "Guild";
            case HelpKind.A_Challenge:return "Challenge";
            case HelpKind.A_Alchemy:return "Alchemy";
            case HelpKind.A_Equip:return "Equip";
            case HelpKind.A_Skill:return "Skill";
            case HelpKind.A_Rebirth:return "Rebirth";
            case HelpKind.A_Playtime:return "Playtime";
        }
        return kind.ToString();
    }
    public string HelpString(HelpKind kind)
    {
        string tempStr = optStr + "<size=20>" + HelpName(kind) + "<size=18>\n\n";
        switch (kind)
        {
            case HelpKind.Heroes:
                tempStr += "<u>Warrior</u>";
                tempStr += "\n- Warrior is a <color=orange>Physical Damage</color> type hero that focuses on beating down monsters that come at him. He likes to use swords as his main weapon. Most skills are low range and limited AOE skills. He excels at fighting single target monsters/challenge bosses.";
                tempStr += "\n\n";
                tempStr += "<u>Wizard</u>";
                tempStr += "\n- Wizard is a <color=orange>Magical Damage</color> type hero with plenty of AOE skills. Her main weapon is the staff. She excels at fighting big groups of monsters though her low health can be a challenge.";
                tempStr += "\n\n";
                tempStr += "<u>Angel</u>";
                tempStr += "\n- Angel is the support hero with both <color=orange>Physical Damage</color> and <color=orange>Light Damage</color> type skills along with buffs. Her main weapon is her Wings. She focuses on healing and giving buffs to herself and other heroes. Her lower damage output can be a challenge.";
                tempStr += "\n\n";
                tempStr += "<u>Thief</u>";
                tempStr += "\n- Thief is a sneaky hero with a mix of <color=orange>Physical Damage</color> and <color=orange>Dark Damage</color> skills. He has a mix of both ranged and melee skills. His two main focuses are Speed and Equipment drops, making him an excellent farmer. He has lower health and defense that can make it a challenge.";
                tempStr += "\n\n";
                tempStr += "<u>Archer</u>";
                tempStr += "\n- Archer is the kiting hero with a mix of <color=orange>Physical Damage</color> and <color=orange>Magical Damage</color> type skills while focusing on long range and keeping the monsters away. His low health can be a challenge.";
                tempStr += "\n\n";
                tempStr += "<u>Tamer</u>";
                tempStr += "\n- ";
                tempStr += "\n\n";
                break;
            case HelpKind.Leveling:
                tempStr += "Leveling your hero is extremely important. You’ll get <color=orange>Ability Points</color> and <color=orange>Guild Level EXP</color> every time your hero levels up.";
                tempStr += "\n\nYou can gain EXP for your hero by killing monsters, claiming quest rewards or completing dungeons. Each time you gain EXP your hero can gain <color=orange>a max of 30 levels</color>, quests and dungeons show a MAX indicator in their rewards when you hit this limit so you don't waste exp.";
                tempStr += "\n\n";
                tempStr += "\n";
                tempStr += "<u>Tips to Level Up Faster</u>";
                tempStr += "\n\n- Use the <color=orange>Simulation</color> button for each Area under the World Map. This will let you see your rough estimate of Time to Complete | Gold/Sec | Exp/Sec for each Area you have unlocked. This is a great way to determine where you should go to level your hero.";
                tempStr += "\n\n- Increasing the level of the Town building - <color=orange>Statue of Heroes</color> will give a nice bonus to EXP gain every time you level it.";
                tempStr += "\n\n- Make sure to unlock <color=orange>Dungeons</color> in the different areas and check the <color=orange>Rewards</color> part under the Dungeon detail tab for the EXP rewards. It can be a huge boost to your hero’s level.";
                tempStr += "\n\n- While in the dungeons, look for <color=orange>Treasure Chests</color> and hope you get the <color=orange>EXP Blessing</color>, which will boost your exp gains for a short time. This will also increase the EXP Reward - if you have the blessing at the time of completion. ";
                tempStr += "\n\n- When you start to Tier 1 Rebirth, make sure to upgrade the EXP Multiplier as much as you can. ";
                tempStr += "\n\n- Achieving Rank 2 of the <color=orange>Alchemist's Hut</color> town building will unlock a EXP potion, that your heroes can wear in the utility slot for an increase of EXP Gain.";
                tempStr += "\n\n- There are also unique items in some of the regions that have EXP Gain as part of their effects. ";
                break;
            case HelpKind.WorldMap:
                tempStr += "Clicking the Map Icon on the top right of the screen will take you to the World Map. This is where you can see the number of Portal Orbs you have and Hovering over the Ribbon will show you the Mission Milestones.";
                tempStr += "\n\n";
                tempStr += "<u>Regions</u>";
                tempStr += "\n- There are a total of 10 regions in the world. Each region has 8 areas and several dungeons. You can click each region in the map to access the different areas/dungeons.";
                tempStr += "\n- Every third region will change the Tier of Town Materials that are given to the areas as rewards. IE – Slime Village and Magicslime City give Tier 1 Town Materials. Spider Maze and Bat City give Tier 2 Town Materials";
                tempStr += "\n\n";
                tempStr += "<u>Simulations</u>";
                tempStr += "\n- Clicking on the Simulation button or using the shortcut (Shift + S), will simulate an attempt at clearing the Area/Dungeon.";
                tempStr += "\n- It will also provide an estimate of the Time it’ll take to clear, Gold/Sec, and EXP/Sec that you’ll get when clearing the area/dungeon.";
                tempStr += "\n- This does not count the reward Gold/EXP that you get from dungeon clearing.";
                tempStr += "\n\n";
                tempStr += "<u>Areas</u>";
                tempStr += "\n- Areas are composed of 10 waves of monsters by default. Every time you clear the area, you will receive Town Materials as a reward.";
                tempStr += "\n- To unlock new areas in the regions, take a look at the requirements and clear what is required.";
                tempStr += "\n- The Town Material rewards go in a cycled order. 1st Area will give Bricks, 2nd Area will give Logs, and 3rd will give Shards. Then the cycle continues up to the 8th Area, the 8th area gives a split of the 3 different Town Materials.";
                tempStr += "\n- The next Region will carry on with the cycle starting with the next Town Material after the Previous Region Area 7.";
                tempStr += "\n\n";
                tempStr += "<u>Area Missions</u>";
                tempStr += "\n- Each Area has two missions at the start, that can be completed for Epic Coins and goes towards the Mission Milestone count. This can be upgraded in the Area Prestige, read below.";
                tempStr += "\n\n";
                tempStr += "<u>Area Prestige</u>";
                tempStr += "\n- You must complete a World Ascension first for this Prestige to unlock.";
                tempStr += "\n- To get Area Prestige points you must clear the Area a certain number of times – the Area Details window will tell you the required number of clears per point.";
                tempStr += "\n- There are 6 different upgrades that you can choose.";
                tempStr += "\n  - Area Prestige : This allows you to increase the difficulty up to 10. Each new difficulty has the same missions, but higher monster levels and increased # of waves.";
                tempStr += "\n  - Explorer’s Boon : Increases Clear counts by 1 per clear. ";
                tempStr += "\n  - EXP Bonus : This increases your EXP Gains by a % for that entire region.";
                tempStr += "\n  - Mission Challenger : Adds another mission to each Area, up to 5 max missions per Area.";
                tempStr += "\n  - A Moment to Breathe : Reduces wave length by 5 each upgrade. Minimum number of waves is hard capped at 10.";
                tempStr += "\n  - Move Speed Bonus : Increases your Move speed % for that entire region.";
                tempStr += "\n\n";
                tempStr += "<u>Dungeon Areas</u>";
                tempStr += "\n- Every region has its own set of dungeons that can be unlocked. ";
                tempStr += "\n- Each entry to a dungeon consumes a number of Portal Orbs pending on the Dungeon. ";
                tempStr += "\n- You have a limited amount of time to clear the dungeon, you can increase the time through Town Research, Treasure Chests, and Dungeon Area Prestige upgrades.";
                tempStr += "\n- Dungeons are a great source of reward Gold, EXP, Enchanted Shards, and other materials. They are extremely useful in getting your first rebirth level requirement. ";
                tempStr += "\n\n";
                tempStr += "<u>Dungeon Prestige</u>";
                tempStr += "\n- You must complete a World Ascension first to unlock this prestige option. ";
                tempStr += "\n- Like Area Prestige, you must clear the dungeons a certain number of times before gaining Prestige Points.";
                tempStr += "\n- Dungeon Prestige upgrades";
                tempStr += "\n  - Area Prestige : This will increase the difficulty of the dungeon up to 10. Each new difficulty raises the monster level and increased wave count. It also increases the Rewards you receive upon clearing. ";
                tempStr += "\n  - Additional Time : This will increase the Dungeon Clear timer for another 60 seconds per upgrade.";
                tempStr += "\n  - Portal Key : This reduces the number of Orbs that are needed to enter the dungeon. Minimum Portal Orb is 1. ";
                tempStr += "\n  - Treasure Hunter : This increases the chance of getting a treasure chest from a monster kill while in the dungeon. ";
                tempStr += "\n  - Metal Chaser : Increases the chance of spawning a Metal Monster.";
                break;
            case HelpKind.Battle:
                tempStr += "<u>Battle System</u>";
                tempStr += "\n- While playing as one of the six available heroes, the battle continues on non-stop.";
                tempStr += "\n- You can also activate heroes in the background after you get [Proof of Rebirth] titles.";
                tempStr += " Tier 1 Rebirth must be completed on that hero to clear the Title [Proof of Rebirth 1], Tier 2 Rebirth must be completed on that hero to clear the [Proof of Rebirth 2]. ";
                tempStr += "\n- Heroes in the background gain everything as if they were active, but at a reduced rate. Called Passive in the Guild tab.";
                tempStr += "\n- Gaining higher [Proof of Rebirth] titles increase the efficiency of that hero while they are active in the background and not the currently played hero.";
                tempStr += "\n\n";
                tempStr += "<u>Combat Range</u>";
                tempStr += "\n- Combat Range is a value that indicates how close the hero will get to the monster in battle.";
                tempStr += "\n- You can change the combat range of current playing hero by clicking the button on bottom right, above the HP Bar.";
                tempStr += "\n- Most of the hero's skills have a range, the heroes must be in range before triggering the skill on a Monster.";
                tempStr += "\n- You can hover over a skill to see the range on the battlefield, you can also turn this option off in the Settings tab.";
                tempStr += "\n\n";
                tempStr += "<u>Leveling</u>";
                tempStr += "\n- Gaining any kind of EXP has a limit of only providing up to 30 levels to that hero at a time. This includes defeating monsters, quests rewards, and dungeon rewards. See the Leveling Help tab for more information on leveling.";
                tempStr += "\n\n";
                tempStr += "<u>Damage Output</u>";
                tempStr += "\n- First, Skill's deal damage is calculated: [Hero ATK/MATK] * [Skill-specific Damage Multiplier]";
                tempStr += "\n- Second, when a monster received skill's attack: the damage is reduced by monster's DEF & element resistance %";
                tempStr += "\n- After that, the damage is multiplied by [Physical/Fire/Ice/Thunder/Light/Dark Damage%] and [Specific-monster-damage% from unique EQ effect]";
                tempStr += "\n\n";
                tempStr += "<u>Damage Input</u>";
                tempStr += "\n- First, Monster ATK/MATK reduced by Heros DEF/MDEF";
                tempStr += "\n- Second, the damage is reduced by [Resistance%] (MAX:90%)";
                tempStr += "\n- If the resistance % is negative, the received damage is increased by -[Resistance&] (no limit)";
                tempStr += "\n\n";
                break;
            case HelpKind.Capture:
                tempStr += "Are you tired of mindlessly killing all of the creatures you come across? How would you like to try to cooperate with them by shoving them into a net and forcing them to do what you tell them?";
                tempStr += "\n\n";
                tempStr += "<u>Titles</u>";
                tempStr += "\n- Well, you can't capture a monster without understanding it. Your hero needs to hold the <color=orange>Monster Study</color> title or capture attempts can't succeed.";
                tempStr += "\n\n";
                tempStr += "<u>Traps</u>";
                tempStr += "\n- You will also need to be equipped with the correct kind of <color=orange>Trap</color> for the monster you are trying to capture, which you can buy in <color=orange>Shop</color> tab. Different colored monsters have different strengths and weaknesses, so only the right net will successfully hold a monster.";
                tempStr += "\n\n";
                tempStr += "<u>Strength</u>";
                tempStr += "\n- Even though you know how to capture a monster, there's no point unless you're strong enough to actually hold on to the monster until it agrees to do what you say. Your hero's level and their knowledge of monsters will determine just how strong a monster can be so you can still capture it. As you progress through the game, there may be other mechanics that improve your technique so you can capture stronger monsters.";
                tempStr += "\n\n";
                tempStr += "<u>Technique</u>";
                tempStr += "\n- Once you are equipped with an appropriate trap and adorned with the appropriate title, simply <color=orange>Right-Click</color> on a monster in the play area before your hero kills it. If it is the right color and not too strong, you should succeed in your capture attempt and the traps will enter a cooldown period until you can capture another monster. Catch enough of a specific type of monster and you can increase its rank as a pet, see the Bestiary section for more details on that.";
                tempStr += "\n- Again, progress into the game may lead you to discover more efficient techniques for capturing monsters, but these form the basics that any hero needs to know.";
                break;
            case HelpKind.Blessing:
                tempStr += "<u>Buffs aka Blessings</u>";
                tempStr += "\nThese buffs can be purchased from the shop; HP Blessing is the first unlocked blessing that’s purchasable.";
                tempStr += " All others are achieved through the Temple town building.";
                tempStr += " You are also able to increase the effect% and duration of the blessing through Researching in the Temple town building.";
                tempStr += " These blessings are also attainable by opening Treasure Chests in Dungeons at a random chance.";
                tempStr += "\n\nAPPLYING THE SAME BUFF WILL JUST REAPPLY THE BLESSING - IT DOES NOT STACK";
                tempStr += "\n\n- HP Blessing – Multiplies your max HP stat by a % for a certain duration.";
                tempStr += "\n- ATK Blessing – Multiplies your total ATK stat by a % for a certain duration.";
                tempStr += "\n- MATK Blessing – Multiplies your total MATK stat by a % for a certain duration.";
                tempStr += "\n- Move Speed Blessing – Multiplies your Move Speed stat by a % for a certain duration.";
                tempStr += "\n- Skill Proficiency Blessing - Multiplies your Skill Proficiency by a % for a certain duration.";
                tempStr += "\n- Equipment Proficiency - Multiplies your Equipment Proficiency by a % for a certain duration.";
                tempStr += "\n- Gold Gain Blessing - Multiplies your Gold Gain Stat by a % for a certain duration.";
                tempStr += "\n- EXP Gain Blessing - Multiplies your EXP Gain stat by a % for a certain duration";
                tempStr += "\n\n";
                break;
            case HelpKind.Debuff:
                tempStr += "<u>Debuffs</u>";
                tempStr += "\nHeroes and monsters can both apply these debuffs to the other.";
                tempStr += " Heroes will have to use certain skills that apply the debuff.";
                tempStr += " There are also field debuffs. Checking the Map Detail tab will show what the field debuff and the % will be when you enter the Area/Dungeon.";
                tempStr += " You can reduce the Field Debuff effects by a Cartographer Research.";
                tempStr += " There are potions and equipment that can help with Element Resistance to counter Element Debuffs.";
                tempStr += "\n";
                tempStr += "\n- ATK Down – Lowers the opponents ATK by 50% for a certain duration.";
                tempStr += "\n- MATK Down – Lowers the opponents MATK by 50 % for a certain duration.";
                tempStr += "\n- DEF Down – Lowers the opponents DEF stat by 50 % for a certain duration.";
                tempStr += "\n- MDEF Down – Lowers the opponents MDEF stat by 50 % for a certain duration.";
                tempStr += "\n- SPD Down – Lowers the opponents SPD stat and Move Speed by 50 % for a certain duration.";
                tempStr += "\n- Freeze – Opponents Attacks and Movement stops for 1 Second.";
                tempStr += "\n- Electric – Opponent receives an extra 10 % damage.";
                tempStr += "\n- Poison – Deals Damage every second to the opponent for a certain duration. (The amount of Poison Damage is based on the skill that provides the debuff)";
                tempStr += "\n- Death – Instantly kills the target.Skills that have this debuff, has a very low chance of triggering.";
                tempStr += "\n- Knockback – Opponent gets knocked back and stunned for 0.5 second.";
                tempStr += "\n- Fire Resistance Down – Lowers opponents Fire Resist stat by 100 % for a certain duration.";
                tempStr += "\n- Ice Resistance Down – Lowers opponents Ice Resist stat by 100 % for a certain duration.";
                tempStr += "\n- Thunder Resistance Down – Lowers opponents Thunder Resist stat by 100 % for a certain duration.";
                tempStr += "\n- Light Resistance Down – Lowers opponents Light Resist stat by 100 % for a certain duration.";
                tempStr += "\n- Dark Resistance Down – Lowers opponents Dark Resist stat by 100 % for a certain duration.";
                tempStr += "\n- Gravity – Pulls opponents towards the center of the field.";
                break;
            case HelpKind.Hotkeys:
                tempStr += "<u>General</u>";
                tempStr += "\n- <color=orange>+</color> / <color=orange>-</color> keys : Increases / Decreases the purchase multiplier";
                tempStr += "\n- <color=orange>W,A,S,D</color> or <color=orange>Arrow</color> keys : Manually moves the current playing hero";
                tempStr += "\n- <color=orange>Tab</color> / <color=orange>Ctrl + Tab</color> : Opens the next / previous tab";
                tempStr += "\n- <color=orange>T</color> or </color=orange>Esc</color> : Removes the current tooltip (useful if a tooltip get stuck)";
                tempStr += "\n- <color=orange>Shift + P</color> : Switches Performance Mode";
                tempStr += "\n\n";
                tempStr += "<u>Quest Tab</u>";
                tempStr += "\n- <color=orange>Right Click</color> or <color=orange>Shift + Left Click</color> on a quest button : Accept or Claim the quest";
                tempStr += "<u>Upgrade Tab</u>";
                tempStr += "\n- <color=orange>Right Click</color> : Adds queues to upgrades (Note: This requires having upgrade queues from Pet effects or an Epic Store purchase)";
                tempStr += "\n- <color=orange>Shift + Right Click</color> : Removes queues from upgrades";
                tempStr += "\n- <color=orange>Q</color> : Adds Super Queue to upgrades by exchanging 10 queues (Note: This requires an Epic Store purchase)";
                tempStr += "\n\n";
                tempStr += "<u>Equip Tab</u>";
                tempStr += "\n- <color=orange>Left Double Click</color> : An equipment or utility in the inventory will equip it, if there’s an open slot.";
                tempStr += "\n- While Holding <color=orange>Shift + D</color> and <color=orange>Left Double Click</color> on <color=orange>Item</color> : A piece of equipment / utility item will Disassemble the item.";
                tempStr += "\n- While Holding <color=orange>Shift + D</color> and <color=orange>Left Double Click</color> on <color=orange>Disassemble Box</color> : Disassembles all items in Equipment Inventory except for locked items.";
                tempStr += "\n- <color=orange>L</color> : Locks / Unlocks to disassemble the item.";
                tempStr += "\n- While Holding <color=orange>Shift</color> and <color=orange>Left Click</color> on <color=orange>Sort Button</color> : Sorts in reverse order.";
                tempStr += "\n- While Holding <color=orange>Shift</color> and Drag and drop a utility item : Splits its Stack # by the amount of multiplier at top left.";
                tempStr += "\n- <color=orange>Left Click</color> an equipment in dictionary : Adds the clicked equipment to the Auto-Disassemble (Noted by the D) list. Left Click again to remove. (Note: This requires town upgrades or pet effects)";
                tempStr += "\n- <color=orange>Shift + C</color> an equipment in dictionary : Crafts the item. (Note: This requires a town upgrade)";
                tempStr += "\n\n";
                tempStr += "<u>Lab Tab</u>";
                tempStr += "\n- <color=orange>Right Click</color> an potion: Adds queues to mix potions (Note: This requires having alchemy queues from Pet effects or an Epic Store purchase)";
                tempStr += "\n- <color=orange>Shift + Right Click</color> : Removes queues from potions";
                tempStr += "\n- <color=orange>Q</color> : Adds Super Queue to potions by exchanging 10 queues (Note: This requires an Epic Store purchase)";
                tempStr += "\n\n";
                break;
            case HelpKind.Ability:
                tempStr += "<u>Stats</u>";
                tempStr += "\n- VIT : Increases HP | DEF | MDEF";
                tempStr += "\n- STR : Increases ATK | DEF";
                tempStr += "\n- INT : Increases MP | MATK | MDEF";
                tempStr += "\n- AGI : Increases MP | SPD | Move Speed";
                tempStr += "\n- LUK : Increases Critical Chance | Drop Chance";
                tempStr += "\n\n";
                tempStr += "Stats are calculated as below.";
                tempStr += "\n- HP = [BASE] + [INCREMENT] x VIT  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.HP) + "</color>";
                tempStr += "\n- MP = [BASE] + [INCREMENT] x (INT+AGI)/2  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.MP) + "</color>";
                tempStr += "\n- ATK = [BASE] + [INCREMENT] x STR  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.ATK) + "</color>";
                tempStr += "\n- MATK = [BASE] + [INCREMENT] x INT  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.MATK) + "</color>";
                tempStr += "\n- DEF = [BASE] + [INCREMENT] x (VIT+STR)/2  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.DEF) + "</color>";
                tempStr += "\n- MDEF = [BASE] + [INCREMENT] x (VIT+INT)/2  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.MDEF) + "</color>";
                tempStr += "\n- SPD = [BASE] + [INCREMENT] x AGI  :  <color=orange>" + localized.BasicStatsDescription(BasicStatsKind.SPD) + "</color>";
                tempStr += "\n- Physical Critical Chance = [BASE] + [INCREMENT] x LUK  :  <color=orange>Chance of doing 2.0x or more damage</color>";
                tempStr += "\n- Magical Critical Chance = [BASE] + [INCREMENT] x LUK  :  <color=orange>Chance of doing 2.0x or more damage.</color>";
                tempStr += "\n- Equipment Drop Chance = [BASE] + [INCREMENT] x LUK^(2/3)  :  <color=orange>Increases Equipment Drop Chance ( Does not include Uniques )</color>";
                tempStr += "\n- Move Speed = [BASE] + [INCREMENT] x AGI^(2/3)  :  <color=orange>Increases how fast you move</color>";
                tempStr += "\n\n";
                tempStr += "<u>[BASE] Basic Stats</u>";
                for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
                {
                    int countJ = j;
                    tempStr += "\n- " + localized.Hero((HeroKind)countJ) + " : ";
                    for (int i = 0; i < Enum.GetNames(typeof(BasicStatsKind)).Length; i++)
                    {
                        int countI = i;
                        tempStr += localized.BasicStats((BasicStatsKind)countI) + " " + tDigit(Parameter.baseStats[countJ][countI], 1) + " | ";
                    }
                }
                tempStr += "\n\n";
                tempStr += "<u>[INCREMENT] Basic Stats</u>";
                for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
                {
                    int countJ = j;
                    tempStr += "\n- " + localized.Hero((HeroKind)countJ) + " : ";
                    for (int i = 0; i < Enum.GetNames(typeof(BasicStatsKind)).Length; i++)
                    {
                        int countI = i;
                        tempStr += localized.BasicStats((BasicStatsKind)countI) + " " + tDigit(Parameter.stats[countJ][countI], 1) + " | ";
                    }
                }
                tempStr += "\n\n";
                tempStr += "<u>[BASE] Stats</u>";
                for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
                {
                    int countJ = j;
                    tempStr += "\n- " + localized.Hero((HeroKind)countJ) + " : ";
                    tempStr += "Phy Crit Chance " + percent(Parameter.baseStats[countJ][7], 3) + " | ";
                    tempStr += "Mag Crit Chance" + percent(Parameter.baseStats[countJ][8], 3) + " | ";
                    tempStr += "EQ Drop Chance " + percent(Parameter.baseStats[countJ][10], 3) + " | ";
                    tempStr += localized.Stat(Stats.MoveSpeed) + " " + meter(Parameter.baseStats[countJ][11]) + " / sec";
                }
                tempStr += "\n\n";
                tempStr += "<u>[INCREMENT] Stats</u>";
                for (int j = 0; j < Enum.GetNames(typeof(HeroKind)).Length; j++)
                {
                    int countJ = j;
                    tempStr += "\n- " + localized.Hero((HeroKind)countJ) + " : ";
                    tempStr += "Phy Crit Chance " + percent(Parameter.stats[countJ][7], 3) + " | ";
                    tempStr += "Mag Crit Chance" + percent(Parameter.stats[countJ][8], 3) + " | ";
                    tempStr += "EQ Drop Chance " + percent(Parameter.stats[countJ][10], 3) + " | ";
                    tempStr += localized.Stat(Stats.MoveSpeed) + " " + meter(Parameter.stats[countJ][11]) + " / sec";
                }
                tempStr += "\n\n";
                break;
            case HelpKind.Title:
                tempStr += "Titles provide unique bonuses to the specific hero. They are acquired through <color=orange>Title Quests</color>. Under the Main Ability Tab, you can click on Title on the right-hand side to see your current hero’s titles and bonuses.";
                tempStr += "\n\n";
                break;
            case HelpKind.Quest:
                tempStr += "<u>Global Quest</u>";
                tempStr += "\n- Global quests form the introduction to the game, and are required to unlock new features once you are ready according to Hapiwaku.";
                tempStr += "\n- They are shared among all heroes.";
                tempStr += "\n- They do not count towards the Accepted Quest limit.";
                tempStr += "\n- They do not get reset ever.";
                tempStr += "\n\n";
                tempStr += "<u>Daily Quest</u>";
                tempStr += "\n- Daily quests are a reliable source of free Epic Coins and Portal Orbs.";
                tempStr += "\n- They are shared among all heroes.";
                tempStr += "\n- They do not count towards the Accepted Quest limit.";
                tempStr += "\n- They reset at midnight in the timezone of your Steam account.";
                tempStr += "\n\n";
                tempStr += "<u>Title Quest</u>";
                tempStr += "\n- Title quests give bonuses to each hero that complete the quest. Check regularly to see if any of your heroes are eligible for new or improved titles!";
                tempStr += "\n- Each hero needs to complete a title quest to apply the title to themselves only.";
                tempStr += "\n- They DO count towards the Accepted Quest limit.";
                tempStr += "\n- They don't reset until a later World Ascension tier.";
                tempStr += "\n\n";
                tempStr += "<u>General Quest</u>";
                tempStr += "\n- General quests provide things for your heroes to do to prove how heroic they are, and provide experience, gold or materials as a reward. But mostly proof of how heroic they are.";
                tempStr += "\n- Each hero needs to complete general quests themselves. No riding on coattails!";
                tempStr += "\n- They DO count towards the Accepted Quest limit.";
                tempStr += "\n- The quests reset on Rebirth and World Ascension, but some of the rewards may persist.";
                tempStr += "\n\n";
                tempStr += "<u>Accepted Quest Limit</u>";
                tempStr += "\n- Each hero has a limit to the number of title and general quests (combined) that they can accept at the same time.";
                tempStr += "\n- Once you are at that limit, you need to complete or cancel a quest to be able to accept a new quest.";
                tempStr += "\n- You can increase the limit through a Rebirth upgrade.";
                tempStr += "\n\n";
                tempStr += "<u>Favorite Quest (Epic Store Purchase)</u>";
                tempStr += "\n- This allows you to assign general quests as favorites which will count towards Accepted Quest Limit. Favorite quests will be automatically accepted and then cleared when the requirements are met.";
                tempStr += "\n\n";
                break;
            case HelpKind.Skill:
                tempStr += "Each Hero has 10 different skills that can be used during game play by placing them into available Class Skill Slots and Global Skill Slots. You can only use other hero skills in Global Skill Slots, however. " +
                    "\n\nTo start using skills, you must first get the Rank to 1. To upgrade the rank, you will have to have the required amount of Resource. You can look at what’s required by hovering over the resource icon next to the skill. " +
                    "After ranking up the skill, it will add 5 Max levels to the associated skill. " +
                    "\n\nTo increase the level of the skill, you must use the skill by putting it in an open Class Skill Slot. You can increase the speed of leveling, by increasing your Skill Proficiency. " +
                    "Each skill has different milestones attached to the current level of the skill. These milestones provide passive bonuses to your hero. These passives only work for its skill's hero in default. You will be able to share it with other heroes through the Tier 2 Rebirth Upgrade. " +
                    "\n\nMore Class Skill Slots and Global Skill Slots can be unlocked through gameplay.";
                tempStr += "\n\n";
                break;
            case HelpKind.Upgrade:
                tempStr += "<u>Resource Upgrade</u>";
                tempStr += "\n- The resource tab allows you to increase the amount of dropped resource from monsters by purchasing with gold. There are also quests that will unlock new resource gain upgrades.";
                tempStr += "\n\n";
                tempStr += "<u>Stats Upgrade</u>";
                tempStr += "\n- The Stats tab provides bonuses to different stats for all heroes. At the cost of resources and gold.";
                tempStr += "\n\n";
                tempStr += "<u>Gold Cap Upgrade</u>";
                tempStr += "\n- The gold cap upgrade tab allows you to increase your Gold Cap at the expense of resources. Each resource has its own upgrade that you can level up to increase your Gold Cap.";
                tempStr += "\n\n";
                tempStr += "<u>Slime Bank Upgrade</u>";
                tempStr += "\n- Unlocked after reaching Guild Level 35, these provide you with new upgrades at the cost of Slime Coins, allowing you to upgrade different aspects of the game.";
                tempStr += "\n\n";
                tempStr += "<u>Slime Coin</u>";
                tempStr += "\n- Slime Coins are acquired after having maxed out your Gold Cap. Overflow gold gets turned into Slime Coins. You can also research your town building Slime Bank, to start gaining interest on your Slime Coins.";
                tempStr += "\n\n";
                break;
            case HelpKind.Equip:
                tempStr += "<u>Equipment</u>";
                //tempStr += "\n- The Equip tab is where you will find all your equipment that you’ve picked up from the battlefield. To pick up the equipment on the battlefield, just left click the item. " +
                //    "To equip the equipment, simply left click hold and drag a piece of equipment in the Equipment Inventory to an open slot of the correct part of equipment. " +
                //    "While the item is equipped, it will gain Equipment Proficiency which can raise the level of the item, increasing its power. After reaching Level 10 on that specific hero, you will gain a Passive. " +
                //    "If you run out of space in your equipment inventory, you can drag and drop the item to the Disassemble bar and reclaim town materials. " +
                //    "In the equip tab there is also the Enchant Inventory and Utility Inventory – See Lab Help for more info on Enchants and Utilities.";
                tempStr += "\n- The Equip tab is where you will find all your equipment that you’ve picked up from the battlefield. To pick up the equipment on the battlefield, just left click the item.";
                tempStr += "\n- To equip the equipment, simply left click hold and drag a piece of equipment in the Equipment Inventory to an open slot of the correct type of equipment.";
                tempStr += "\n- While the item is equipped, it will gain Equipment Proficiency which can raise the level of the item, increasing its power. After reaching Level 10 (Mastery) on that specific hero, you will gain a Passive. All heroes that wear that item, gain that passive as well.";
                tempStr += "\n- If you run out of space in your equipment inventory, you can drag and drop the item to the Disassemble bar and reclaim town materials.";
                tempStr += "\n- In the Equip tab there is also the Enchant Inventory and Utility Inventory – See Lab Help for more info on Enchants and Utilities.";
                tempStr += "\n\n";
                tempStr += "<u>Dictionary</u>";
                //tempStr += "\n- The dictionary tab under Equips will show you all the current equipment that you’ve found in the game so far. " +
                //    "Each time you level up a piece of equipment to level 10 (on a hero) you will gain dictionary points that can be used to then upgrade your choice of heroes Equipment Proficiency or Treasure hunting which increases your chance at finding items in the battlefield. " +
                //    "Rarity of the equipment plays a part in the amount of Upgrade points. Common : 1, Uncommon: 2, Rare: 3, Super Rare : 4, and Epic : 5. " +
                //    "In the Dictionary you will be able to set the equipment to auto-disassemble once you’ve got the right upgrade, by clicking on the Item. " +
                //    "You’ll see a D at the bottom left of the icon, to indicate Disassemble. The Auto-disassemble will happen when you have an open equip inventory slot, and you click on the item in the battlefield. " +
                //    "Upon an upgrade in the Town, you will be able to craft the equipment as well.";
                tempStr += "\n- The Dictionary tab under Equips will show you all the current equipment that you’ve found in the game so far.";
                tempStr += "\n- Each time you level up a piece of equipment to level 10 (on a hero) you will gain dictionary points that can be used to then upgrade your choice of heroes Equipment Proficiency or Treasure Hunting which increases your chance at finding items in the battlefield.";
                tempStr += "\n- Rarity of the equipment plays a part in the amount of Upgrade points. Common : 1, Uncommon : 2, Rare : 3, Super Rare : 4, and Epic : 5.";
                tempStr += "\n- In the Dictionary you will be able to set the equipment to auto-disassemble once you’ve got the right upgrade, by clicking on the Item. You’ll see a D at the bottom left of the icon, to indicate Disassemble. The Auto-disassemble will happen when you have an open equip inventory slot, and you click on the item in the battlefield.";
                tempStr += "\n- Upon an upgrade in the Town, you will be able to craft the equipment as well.";
                tempStr += "\n\n";
                tempStr += "<u>Talisman</u>";
                tempStr += "\n- Talismans are unique items that provide bonuses to your hero. They can be equipped and are never consumed. They can increase their bonuses by adding more of the same talisman to create a stack. The stack max amount is based on your Alchemy Upgrade [Deeper Capacity]";
                tempStr += "\n\n";
                break;
            case HelpKind.Lab:
                tempStr += "<u>Alchemy Tab</u>";
                tempStr += "\n\nMysterious Water per Second";
                tempStr += "\n- This is how fast your bar fills. The higher the number / sec the faster the bar fills.";
                tempStr += "\n\nExpand Cap";
                tempStr += "\n- Click this button to expand the Mysterious Water cap.";
                tempStr += "\n- You can only expand when you have filled the bar completely to the max. Example: It will show 3 / 3.";
                tempStr += "\n- Mysterious Water Cap is the max amount of Mysterious Water you can have.";
                tempStr += "\n- Hover over the Bar to determine your current max cap.";
                tempStr += "\n- There are several ways to increase this through the Town buildings.";
                tempStr += "\n\nCatalysts";
                tempStr += "\n- Catalysts are unlocked by having the required Mysterious Water and Material and clicking the +sign.";
                tempStr += "\n- To equip the catalyst after unlocked, simply click on it. You’ll see double line border around it. And two Essences will show up below.";
                tempStr += "\n- Catalysts also have a chance for a critical effect, that will provide a special Alchemy Item, that will be used for upgrades in the Town.";
                tempStr += "\n\nEssence";
                tempStr += "\n- Each Catalyst that you unlock will have two different Essences attached to the catalyst.";
                tempStr += "\n- Each Conversion will provide an item of the Essence. Which can then be used to mix potions.";
                tempStr += "\n\nConversion";
                tempStr += "\n- Conversion happens when the catalyst reaches 1.0 on the bar itself.";
                tempStr += "\n- To increase the time, it takes to fill the essence bar, add more Mysterious water / second to the essence by clicking on the ^ arrow.";
                tempStr += "\n- You need to have at least 0.1 Mysterious Water / sec to apply to the Essence.";
                tempStr += "\n\nMix Potions";
                tempStr += "\n- This is where you can spend your essences to craft potions.";
                tempStr += "\n\nAlchemy Points";
                tempStr += "\n- You receive points from mixing potion, you can then use these points to upgrade the potion levels themselves, or other alchemy upgrades in the Upgrades sub tab, right next to Mix Potions.";
                tempStr += "\n\n";
                tempStr += "\n\n";
                tempStr += "<u>Craft Tab</u>";
                tempStr += "\nThe main “currency” to craft the enchantment scrolls are the Enchanted Shards you can get from completing dungeons. It is possible to lower the costs of these crafts from the Blacksmith (GLv 25) Town building, using the research. ";
                tempStr += "\n\nScrolls";
                tempStr += "\n- These scrolls allow you to change already enchanted equipment. More scrolls can be unlocked by upgrading the Blacksmith.";
                tempStr += "\n\nEnchant Scrolls";
                tempStr += "\n- These will enhance your equipment if they have an enchant slot. They cost color materials and enchanted shards. More scrolls can be unlocked by upgrading the Blacksmith.";
                tempStr += "\n- To use these enchant scrolls, simply drag the Enchant Scroll from the Enchant Inventory to a piece of equipment in the Inventory that has an Enchant Slot open.";
                break;
            case HelpKind.Guild:
                tempStr += "This is where you can see your current Guild level, all the guild members, and upgrade your guild. ";
                tempStr += "\n\n";
                tempStr += "<u>Guild Level</u>";
                tempStr += "\n- Guild levels will unlock town buildings and certain Guild levels are requirements for Ranking town buildings.";
                tempStr += "\n- To gain Guild EXP, you must level your heroes. Every level your heroes get, will also give Guild EXP. ( MATH: Guild EXP Gain = [200 + 5 * herolevel] * multiplier % )";
                tempStr += "\n\n";
                tempStr += "<u>Members</u>";
                tempStr += "\n- Here you can see what each of your guild members are currently doing.";
                tempStr += "\n- You can activate up to 3 heroes at the start of the game. You can have only one active at a time, this is your current playing hero. The others that are activated work in a Passive mode. They continue to do everything as if they were active, but at a reduced rate. ";
                tempStr += "\n- You can hover over the Passive/Active/Inactive button to determine your Background Efficiency %. This can be upgraded by completing a Proof of Rebirth 1/2/3/4/5/6 title quest.";
                tempStr += "\n- You can hover over each hero to see where they are, and their exp gain in the past minute";
                tempStr += "\n\n";
                tempStr += "<u>Guild Ability</u>";
                tempStr += "\n- Every time you level up your guild, you receive an ability point that allows you to upgrade your guild.";
                tempStr += "\n- Hover over each unlocked upgrade to see what they do for your entire guild. ";
                tempStr += "\n- You can unlock more Guild Abilities through World Ascension tiers. ";
                break;
            case HelpKind.Town:
                tempStr += "The Town has 12 different buildings that you can rank and level up. A lot of different upgrades and content is unlocked throughout the Town. ";
                tempStr += "\n\n";
                tempStr += "<u>Town Materials</u>";
                tempStr += "\n- There are a total of 5 Tiers of Town Materials. You get town materials upon clearing an Area in any of the regions.";
                tempStr += "\n- Each tier of town materials is split up into 2 regions each. Starting with Tier 1, being in Slime Village and Magicslime City. ";
                tempStr += "\n- You need these Town Materials to level up each of the buildings in the town. ";
                tempStr += "\n\n";
                tempStr += "<u>Town Buildings</u>";
                tempStr += "\n- Town Buildings are unlocked by achieving the required Guild Level.";
                tempStr += "\n- To start leveling the buildings, you must achieve Rank 1 first. Hover over the Rank Up button to see the requirements for the town buildings.";
                tempStr += "\n- Once it’s rank 1, you can level up to level 20, before needing to Rank Up again.";
                tempStr += "\n- Each building has an Effect, Researchable Effects, Rank and Level Milestones.";
                tempStr += "\n\n";
                tempStr += "<u>Town Effect</u>";
                tempStr += "\n- Town Effect is a bonus that is gained every time you level up the building. ";
                tempStr += "\n\n";
                tempStr += "<u>Town Research</u>";
                tempStr += "\n- You have 3 different Researches to choose from for each building. Based on the 3 Resources.";
                tempStr += "\n- At the start of the game, you can only have one research going at a time, across the entire town.";
                tempStr += "\n- You must at least have Level 1 in the town building before being able to click on a Resource icon within the town building, to start the research.";
                tempStr += "\n- To increase the Research Speed, you must increase the number of Resources you have.";
                tempStr += "\n- Research Power (EXP/sec) MATH: Log10([Current Resource Amount])";
                tempStr += "\n\n";
                tempStr += "<u>Town Rank Milestones</u>";
                tempStr += "\n- Each town Rank milestone will unlock additional content based around the building itself.";
                tempStr += "\n- Hover over Rank Up to see the requirements to rank up the building. ";
                tempStr += "\n- Ranking up will add additional town material costs to level up the building.";
                tempStr += "\n\n";
                tempStr += "<u>Town Level Milestones</u>";
                tempStr += "\n- Every town has level milestones too, that will upgrade things surrounding the town building’s purpose. Leveling up the buildings will take resources.";
                tempStr += "\n- The First column of buildings will all take Bricks, second column will all take Logs, and the last column all will take Shards for leveling up. ";
                tempStr += "\n\n";
                break;
            case HelpKind.Bestiary:
                tempStr += "Bestiary is where you can see all the monsters you’ve encountered so far. You will be able to see how many of each monster that you’ve killed and captured. ";
                tempStr += "\n\n";
                tempStr += "It’s also a great way to see what loot they drop and at what rate.  ";
                tempStr += "\n\n";
                tempStr += "You will be able to see their Active Effect as well – To get this activated you must get to Pet Rank 1 at the very least, this can be achieved by capturing the monster with the correct trap.";
                tempStr += "\n\n";
                tempStr += "The Pet Passive effect also takes being captured at least to Rank 1.  Every rank following will increase the passive effect.";
                tempStr += "\n\n";
                tempStr += "Hovering over the Pet Rank will show you the tooltip for the Taming Gain breakdown. Increasing the number of kills of that monster will increase the Defeated part of the calculations.";
                tempStr += "\n\n";
                tempStr += "Pet leveling happens from the Tamer hero, calling the pets forward to fight for her. Sending your pets on Distant Expeditions can also level them up. Learn more about Expeditions by going to the Help > Expedition.";
                tempStr += "\n\n";
                break;
            case HelpKind.Shop:
                tempStr += "Everything in the Shop Tab restocks on the same 10-minute timer. ";
                tempStr += "\n\n";
                tempStr += "<u>Material Tab</u>";
                tempStr += "\n- Here is where you can buy Materials that drop from Monsters. You can also sell the materials back for gold.";
                tempStr += "\n- There are certain pets that allow for the materials to be auto bought when activated. ";
                tempStr += "\n- You can increase the number of materials per Restock timer by town Research under the Slime Bank Town Building.";
                tempStr += "\n\n";
                tempStr += "<u>Trap Tab</u>";
                tempStr += "\n- The trap tab allows you to buy traps to capture monsters. [Help > Capture].";
                tempStr += "\n- To get new traps, you must rank up the Town Building [Trapper]. ";
                tempStr += "\n- You can also increase the number of traps per Restock timer by town research under the Trapper Town Building.";
                tempStr += "\n\n";
                tempStr += "<u>Blessing Tab</u>";
                tempStr += "\n- Here you can purchase blessings. These blessings can buff certain stats by 150% (at base) and last for 3 minutes (at base).";
                tempStr += "\n- You can increase the number of blessings per restock by the Town Building [Temple] by getting certain level milestones.";
                tempStr += "\n- You can increase the blessing effect % with Town Research under the Temple Town Building.";
                tempStr += "\n- You can also increase the duration of the blessing with Town Research under the Temple Town Building. ";
                tempStr += "\n- You can purchase 1 Blessing per Restock timer as a default.";
                tempStr += "\n\n";
                tempStr += "<u>Town Material Tab</u>";
                tempStr += "\n- Unlocked from the Town Building [Arcane Researcher]. ";
                tempStr += "\n- This is where you can turn in higher tier Town materials for a lower tier Town material. ";
                tempStr += "\n- Starting off at a 10:1 ratio. This can be increased by Level Milestones in the Arcane Researcher town building. ";
                tempStr += "\n\n";
                break;
            case HelpKind.Rebirth: return "Rebirth";
            case HelpKind.Challenge: return "Challenge";
            case HelpKind.Expedition: return "Expedition";
            case HelpKind.WorldAscension: return "World Ascension";
            case HelpKind.EpicStore: return "Epic Store";
            case HelpKind.S_General:
                tempStr += GeneralString();
                break;
            case HelpKind.S_GuildLevel:
                tempStr += GuildLevelString();
                break;
            case HelpKind.S_Town:
                tempStr += TownString();
                break;
            case HelpKind.S_Rebirth:
                tempStr += RebirthString();
                break;
            case HelpKind.S_Challenge:
                tempStr += ChallengeString();
                break;
            case HelpKind.S_WorldAscension:
                tempStr += WorldAscensionString();
                break;
            case HelpKind.A_All: return AchievementString(kind);
            case HelpKind.A_General: return AchievementString(kind);
            case HelpKind.A_Area: return AchievementString(kind);
            case HelpKind.A_Currency: return AchievementString(kind);
            case HelpKind.A_Guild: return AchievementString(kind);
            case HelpKind.A_Challenge: return AchievementString(kind);
            case HelpKind.A_Alchemy: return AchievementString(kind);
            case HelpKind.A_Equip: return AchievementString(kind);
            case HelpKind.A_Skill: return AchievementString(kind);
            case HelpKind.A_Rebirth: return AchievementString(kind);
            case HelpKind.A_Playtime: return AchievementString(kind);
        }
        return tempStr;
    }


    string GeneralString()
    {
        string tempStr = optStr;
        tempStr += "<size=20><u>Playtime</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (main.S.playtimes[count] > 0)
            {
                tempStr += "\n" + localized.Hero((HeroKind)count);
                tempStr += "\n- In-Game Playtime : " + DoubleTimeToDate(main.S.playtimes[count]);
                if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                    tempStr += "  ( This World Ascension : " + DoubleTimeToDate(main.SR.playtimes[count]) + " )";
                tempStr += "\n- Real-time Active Playtime : " + DoubleTimeToDate(main.S.playtimesRealTime[count]);
                if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                    tempStr += "  ( This World Ascension : " + DoubleTimeToDate(main.SR.playtimesRealTime[count]) + " )";
            }
        }
        tempStr += "\nTotal";
        tempStr += "\n- In-Game Playtime : " + DoubleTimeToDate(main.allTime);
        tempStr += "\n- Real-time Active Played : " + DoubleTimeToDate(main.allTimeRealtime);
        tempStr += "\n\n";
        tempStr += "<size=20><u>Total Currency Gained</u><size=18>";
        tempStr += "\n- Gold : " + tDigit(main.S.totalGold);
        tempStr += "\n- Stone : " + tDigit(main.S.totalStone);
        tempStr += "\n- Crystal : " + tDigit(main.S.totalCrystal);
        tempStr += "\n- Leaf : " + tDigit(main.S.totalLeaf);
        tempStr += "\n- Slime Coin : " + tDigit(main.S.totalSlimeCoin);
        tempStr += "\n\n";
        tempStr += "<size=20><u>Max Hero Level Reached</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            tempStr += "\n- " + localized.Hero((HeroKind)count) + " : Lv " + tDigit(main.S.maxHeroLevelReached[count]);
        }
        tempStr += "\n\n";
        tempStr += "<size=20><u>Total Walked Distance</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            tempStr += "\n- " + localized.Hero((HeroKind)count) + " : " + meter(main.S.movedDistance[count]) + " | This Ascension " + meter(main.S.totalMovedDistance[count]) + " | This Rebirth " + meter(main.SR.movedDistance[count]);
        }
        tempStr += "\n- Pet : " + meter(main.S.movedDistancePet) + " | This Ascension " + meter(main.S.totalMovedDistancePet);// + " | This Rebirth " + meter(main.SR.movedDistancePet);
        tempStr += "\nTotal : " + meter(game.statsCtrl.TotalMovedDistance(false)) + " | This Ascension " + meter(game.statsCtrl.TotalMovedDistance(true));
        tempStr += "\n\n";
        tempStr += "<size=20><u>Others</u><size=18>";
        tempStr += "\n- Total Swarms Vanquished : " + tDigit(main.S.swarmClearedNum);
        tempStr += "\n- Total Treasure Chest Opened # : " + tDigit(main.S.openedChestNum);
        tempStr += "\n- Total Equipment Gained : " + tDigit(main.S.totalEquipmentGained);
        double tempValue = 0;
        for (int i = 0; i < main.S.disassembledEquipmentNums.Length; i++)
        {
            tempValue += main.S.disassembledEquipmentNums[i];
        }
        tempStr += "\n- Total Equipment Disassembled : " + tDigit(tempValue);        
        tempStr += "\n- Total Alchemy Point Gained : " + tDigit(main.S.totalAlchemyPointGained);
        tempStr += "\n- Total Captured Monsters # : " + tDigit(game.monsterCtrl.CapturedNum());
        tempStr += "\n\n";
        return tempStr;
    }
    string GuildLevelString()
    {
        string tempStr = optStr;
        tempStr += "Max Guild Level Reached : Lv " + tDigit(main.S.maxGuildLevel);
        tempStr += "\n\n";
        tempStr += "<size=20><u>Accomplished Playtime of Guild Level</u><size=18>";
        for (int i = 0; i < game.guildCtrl.accomplishGuildLevels.Length; i++)
        {
            if (game.guildCtrl.accomplishGuildLevels[i].accomplishedBestTime > 0)
            {
                tempStr += "\n- Lv " + tDigit(i) + " : ";
                if (game.guildCtrl.accomplishGuildLevels[i].accomplishedTime > 0)
                    tempStr += DoubleTimeToDate(game.guildCtrl.accomplishGuildLevels[i].accomplishedTime);
                if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                {
                    tempStr += " | Best : " + DoubleTimeToDate(game.guildCtrl.accomplishGuildLevels[i].accomplishedBestTime);
                    tempStr += " | First : " + DoubleTimeToDate(game.guildCtrl.accomplishGuildLevels[i].accomplishedFirstTime);
                }
            }
        }
        return tempStr;
    }
    string TownString()
    {
        string tempStr = optStr;
        tempStr += "<size=20><u>Town Material Gain Breakdowns</u><size=18>";
        tempStr += optStr + "\n" + game.townCtrl.townMaterialGainMultiplier[(int)game.currentHero].BreakdownString(true);
        tempStr += "\n\n";
        tempStr += "<size=20><u>Town Material Gain from Disassembling Equipment Breakdowns</u><size=18>";
        tempStr += optStr + "\n" + game.equipmentCtrl.disassembleMultiplier.BreakdownString(true);
        tempStr += "\n\n";
        tempStr += optStr + "<size=20><u>Accomplished Playtime of Building Rank</u><size=18>";
        for (int i = 0; i < game.townCtrl.buildings.Length; i++)
        {
            BUILDING building = game.townCtrl.buildings[i];
            if (building.accomplish[0].accomplishedBestTime > 0)
            {
                tempStr += "\n" + building.NameString();
                for (int j = 0; j < building.accomplish.Length; j++)
                {
                    int countJ = j;
                    if (building.accomplish[countJ].accomplishedBestTime > 0)
                    {
                        tempStr += "\n- Rank " + tDigit(j + 1) + " : ";
                        if (building.accomplish[countJ].accomplishedTime > 0)
                            tempStr += DoubleTimeToDate(building.accomplish[countJ].accomplishedTime);
                        if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                        {
                            tempStr += " | Best : " + DoubleTimeToDate(building.accomplish[countJ].accomplishedBestTime);
                            tempStr += " | First : " + DoubleTimeToDate(building.accomplish[countJ].accomplishedFirstTime);
                        }
                    }

                }
            }
        }
        return tempStr;
    }
    string ChallengeString()
    {
        string tempStr = optStr + "<size=20><u>Accomplished Playtime of Challenge</u><size=18>";
        for (int i = 0; i < game.challengeCtrl.challengeList.Count; i++)
        {
            CHALLENGE challenge = game.challengeCtrl.challengeList[i];
            if (challenge.accomplish.accomplishedBestTime > 0)
            {
                tempStr += "\n- " + challenge.TitleUIString() + " : ";
                tempStr += DoubleTimeToDate(challenge.accomplish.accomplishedTime);
                if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                {
                    tempStr += " | Best : " + DoubleTimeToDate(challenge.accomplish.accomplishedBestTime);
                    tempStr += " | First : " + DoubleTimeToDate(challenge.accomplish.accomplishedFirstTime);
                }
            }
        }
        return tempStr;
    }
    string RebirthString()
    {
        string tempStr = optStr + "<size=20><u>Total Rebirth #</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if(game.rebirthCtrl.Rebirth((HeroKind)count, 0).totalRebirthNum > 0)
            {
                tempStr += "\n" + localized.Hero((HeroKind)count);
                for (int j = 0; j < game.rebirthCtrl.rebirth[count].Length; j++)
                {
                    int countJ = j;
                    Rebirth rebirth = game.rebirthCtrl.Rebirth((HeroKind)count, countJ);
                    if (rebirth.totalRebirthNum > 0)
                    {
                        tempStr += "\n- Tier " + tDigit(countJ + 1) + " : " + tDigit(rebirth.rebirthNum);
                        if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                        {
                            tempStr += " | Total : " + tDigit(rebirth.totalRebirthNum);
                        }
                    }
                }
            }
        }
        tempStr += "\n\n";
        tempStr += optStr + "<size=20><u>Best Rebirth Time</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            if (game.rebirthCtrl.Rebirth((HeroKind)count, 0).totalRebirthNum > 0)
            {
                tempStr += "\n" + localized.Hero((HeroKind)count);
                for (int j = 0; j < game.rebirthCtrl.rebirth[count].Length; j++)
                {
                    int countJ = j;
                    Rebirth rebirth = game.rebirthCtrl.Rebirth((HeroKind)count, countJ);
                    if (rebirth.totalRebirthNum > 0)
                        tempStr += "\n- Tier " + tDigit(countJ + 1) + " : " + DoubleTimeToDate(rebirth.bestRebirthPlayTime);
                }
            }
        }
        tempStr += "\n\n";
        tempStr += "<size=20><u>Accomplished Playtime of Rebirth</u><size=18>";
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int count = i;
            Rebirth rebirth = game.rebirthCtrl.Rebirth((HeroKind)count, 0);
            if (rebirth.accomplish.accomplishedBestTime > 0)
            {
                tempStr += "\n" + localized.Hero((HeroKind)count);
                for (int j = 0; j < game.rebirthCtrl.rebirth[count].Length; j++)
                {
                    int countJ = j;
                    rebirth = game.rebirthCtrl.Rebirth((HeroKind)count, countJ);
                    if (rebirth.totalRebirthNum > 0)
                    {
                        tempStr += "\n- Tier " + tDigit(countJ + 1) + " : ";
                        if (rebirth.accomplish.accomplishedTime > 0)
                            tempStr += DoubleTimeToDate(rebirth.accomplish.accomplishedTime);
                        if (game.ascensionCtrl.worldAscensions[0].performedNum > 0)
                        {
                            tempStr += " | Best : " + DoubleTimeToDate(rebirth.accomplish.accomplishedBestTime);
                            tempStr += " | First : " + DoubleTimeToDate(rebirth.accomplish.accomplishedFirstTime);
                        }
                    }
                }
            }
        }
        return tempStr;
    }
    string WorldAscensionString()
    {
        string tempStr = optStr + "<size=20><u>Accomplished Playtime of World Ascension</u><size=18>";
        tempStr += "\n- Tier 1 : " + DoubleTimeToDate(game.ascensionCtrl.worldAscensions[0].accomplish.accomplishedTime);
        return tempStr;
    }

    
    string tempStrAchievement;
    string AchievementString(HelpKind helpKind)
    {        
        StringBuilder tempStringBuilder = new StringBuilder(4096);
        tempStringBuilder.Clear();
        tempStringBuilder.Append(optStr + "<size=20>TOTAL CLEARED # <color=green>" + tDigit(game.achievementCtrl.TotalClearNum()) + "</color> / " + tDigit(game.achievementCtrl.achievementList.Count));
        tempStringBuilder.Append(optStr + "\nTotal Clear Bonus : Gold Gain <color=green>+ " + percent(game.achievementCtrl.GoldGainBonus(), 0) + "</color> ( + 1% / clear )");
        tempStringBuilder.Append("\n\n<u>Achievements</u><size=18>");
        //string tempStr = optStr + "<size=20>TOTAL CLEARED # <color=green>" + tDigit(game.achievementCtrl.TotalClearNum()) + "</color> / " + tDigit(game.achievementCtrl.achievementList.Count);
        //tempStr += optStr + "\nTotal Clear Bonus : Gold Gain <color=green>+ " + percent(game.achievementCtrl.GoldGainBonus(), 0) + "</color> ( + 1% / clear )";
        //tempStr += "\n\n<u>Achievements</u><size=18>";
        switch (helpKind)
        {
            case HelpKind.A_All:
                for (int i = 0; i < game.achievementCtrl.achievementList.Count; i++)
                {
                    //tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(optStr + "\n" + game.achievementCtrl.achievementList[i].NameString());
                }
                break;
            case HelpKind.A_General:
                for (int i = 0; i < game.achievementCtrl.achievementListGeneral.Count; i++)
                {
                    //tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(optStr + "\n" + game.achievementCtrl.achievementListGeneral[i].NameString());
                }
                break;
            case HelpKind.A_Area:
                for (int i = 0; i < game.achievementCtrl.achievementListArea.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListArea[i].NameString());
                }
                break;
            case HelpKind.A_Currency:
                for (int i = 0; i < game.achievementCtrl.achievementListCurrency.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListCurrency[i].NameString());
                }
                break;
            case HelpKind.A_Guild:
                for (int i = 0; i < game.achievementCtrl.achievementListGuild.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListGuild[i].NameString());
                }
                break;
            case HelpKind.A_Challenge:
                for (int i = 0; i < game.achievementCtrl.achievementListChallenge.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListChallenge[i].NameString());
                }
                break;
            case HelpKind.A_Alchemy:
                for (int i = 0; i < game.achievementCtrl.achievementListAlchemy.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListAlchemy[i].NameString());
                }
                break;
            case HelpKind.A_Equip:
                for (int i = 0; i < game.achievementCtrl.achievementListEquip.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListEquip[i].NameString());
                }
                break;
            case HelpKind.A_Skill:
                for (int i = 0; i < game.achievementCtrl.achievementListSkill.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListSkill[i].NameString());
                }
                break;
            case HelpKind.A_Rebirth:
                for (int i = 0; i < game.achievementCtrl.achievementListRebirth.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListRebirth[i].NameString());
                }
                break;
            case HelpKind.A_Playtime:
                for (int i = 0; i < game.achievementCtrl.achievementListPlaytime.Count; i++)
                {
                    tempStringBuilder.Append("\n");
                    tempStringBuilder.Append(game.achievementCtrl.achievementListPlaytime[i].NameString());
                }
                break;

        }
        return tempStringBuilder.ToString();
    }
    string AchievementRewardString(HelpKind helpKind)
    {
        StringBuilder tempStringBuilderReward = new StringBuilder(2048);
        tempStringBuilderReward.Clear();
        tempStringBuilderReward.Append("<size=20>   ");
        tempStringBuilderReward.Append("\n   ");
        tempStringBuilderReward.Append("\n\n<u>Reward</u><size=18>");
        //string tempStr = optStr + "<size=20>   ";
        //tempStr += optStr + "\n   ";
        //tempStr += "\n\n<u>Reward</u><size=18>";
        switch (helpKind)
        {
            case HelpKind.A_All:
                for (int i = 0; i < game.achievementCtrl.achievementList.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementList[i].RewardString());
                }
                break;
            case HelpKind.A_General:
                for (int i = 0; i < game.achievementCtrl.achievementListGeneral.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListGeneral[i].RewardString());
                }
                break;
            case HelpKind.A_Area:
                for (int i = 0; i < game.achievementCtrl.achievementListArea.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListArea[i].RewardString());
                }
                break;
            case HelpKind.A_Currency:
                for (int i = 0; i < game.achievementCtrl.achievementListCurrency.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListCurrency[i].RewardString());
                }
                break;
            case HelpKind.A_Guild:
                for (int i = 0; i < game.achievementCtrl.achievementListGuild.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListGuild[i].RewardString());
                }
                break;
            case HelpKind.A_Challenge:
                for (int i = 0; i < game.achievementCtrl.achievementListChallenge.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListChallenge[i].RewardString());
                }
                break;
            case HelpKind.A_Alchemy:
                for (int i = 0; i < game.achievementCtrl.achievementListAlchemy.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListAlchemy[i].RewardString());
                }
                break;
            case HelpKind.A_Equip:
                for (int i = 0; i < game.achievementCtrl.achievementListEquip.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListEquip[i].RewardString());
                }
                break;
            case HelpKind.A_Skill:
                for (int i = 0; i < game.achievementCtrl.achievementListSkill.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListSkill[i].RewardString());
                }
                break;
            case HelpKind.A_Rebirth:
                for (int i = 0; i < game.achievementCtrl.achievementListRebirth.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListRebirth[i].RewardString());
                }
                break;
            case HelpKind.A_Playtime:
                for (int i = 0; i < game.achievementCtrl.achievementListPlaytime.Count; i++)
                {
                    tempStringBuilderReward.Append("\n");
                    tempStringBuilderReward.Append(game.achievementCtrl.achievementListPlaytime[i].RewardString());
                }
                break;

        }
        return tempStringBuilderReward.ToString();
    }
}

public enum HelpKind
{
    Heroes,
    Leveling,
    Hotkeys,
    WorldMap,
    Battle,
    Capture,
    Blessing,
    Debuff,
    Ability,
    Title,
    Quest,
    Skill,
    Upgrade,
    Equip,
    Lab,
    Guild,
    Town,
    Bestiary,
    Shop,
    Rebirth,
    Challenge,
    Expedition,
    WorldAscension,
    EpicStore,
    S_General,//Statistics
    S_GuildLevel,
    S_Town,
    S_Rebirth,
    S_Challenge,
    S_WorldAscension,
    A_All,//Achievement
    A_General,
    A_Area,
    A_Currency,
    A_Guild,
    A_Challenge,
    A_Alchemy,
    A_Equip,
    A_Skill,
    A_Rebirth,
    A_Playtime,
}

//public enum 