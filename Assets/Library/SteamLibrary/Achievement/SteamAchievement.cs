#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using static Main;
using System;

// This is a port of StatsAndAchievements.cpp from SpaceWar, the official Steamworks Example.
public class SteamAchievement //: MonoBehaviour
{
	public enum Achievement : int
	{
		Playtest,
		Tutorial,
		Hero3,
		Hero6,
		Magicslime,
		Sider,
		Bat,
		Fairy,
		Fox,
		Devilfish,
		Treant,
		Flametiger,
		Unicorn,
		Swarm1,
		Swarm10000,
		//Florzporb,
		//Arachnetta,
		//GurdianKor,
		//Nostro,
		//LadyEmelda,
		//NariSune,
		//Octobaddie,
		//Bananoon,
		//Glorbliorbus,
		//Gankyu,
		AlchemyMaster,
		AlchemyGrandmaster,
		ClassSkill8,
		GlobalSkill8,
		Rebirth1,
		Rebirth2,
		Rebirth3,
		Rebirth4,
		Rebirth5,
		Rebirth6,
		Ascension1,
		Ascension2,
		Ascension3,
		Walk,
		Walk2,
		Chest10000,
		EpicCoin10000,
	};

	private Achievement_t[] m_Achievements = new Achievement_t[] {
new Achievement_t(Achievement.Playtest, "Finding OP Methods Before It Was Cool", "Join in IEH2 Playtest before June 10th (This Achievement is only for IEH2 Playtest)"),
new Achievement_t(Achievement.Tutorial, "The Beginning of Adventure", "Complete all the tutorial quests"),
new Achievement_t(Achievement.Hero3, "Back to Your Roots", "Unlock the 3 heroes from IEH1 - Warrior, Wizard and Angel"),
new Achievement_t(Achievement.Hero6, "Guild Master", "Unlock all 6 heroes - Warrior, Wizard, Angel, Thief, Archer and Tamer"),
new Achievement_t(Achievement.Magicslime, "Hero from the Village", "Unlock Magicslime City"),
new Achievement_t(Achievement.Sider, "Where Is the Exit?", "Unlock Spider Maze"),
new Achievement_t(Achievement.Bat, "Where Is My Torch?", "Unlock Bat Cave"),
new Achievement_t(Achievement.Fairy, "Can I Find the Sacred Sword Here?", "Unlock Fairy Garden"),
new Achievement_t(Achievement.Fox, "What Cute Foxes! They Don't Bite Do They?", "Unlock Fox Shirine"),
new Achievement_t(Achievement.Devilfish, "The Fairies Said the Sacred Sword Would Be at the Bottom Here!", "Unlock Devilfish Lake"),
new Achievement_t(Achievement.Treant, "Knock Knock? Who's there? Wood. Wood Who? Woodn't You Like to Know", "Unlock Treant Darkforest"),
new Achievement_t(Achievement.Flametiger, "Flame on!", "Unlock Flametiger Volcano"),
new Achievement_t(Achievement.Unicorn, "My Little Unicorn Island!", "Unlock Unicorn Island"),
new Achievement_t(Achievement.Swarm1, "Sweet Little Swarming", "Vanquish a swarm"),
new Achievement_t(Achievement.Swarm10000, "Swarm Master", "Vanquish swarms 10000 times"),
//new Achievement_t(Achievement.Florzporb, "Is This What a True Slime Killer Feels Like?", "Clear Raid Boss Battle Challenge [ Florzporb Lv 100 ]"),
//new Achievement_t(Achievement.Arachnetta, "The Queen Is Finally Dead!", "Clear Raid Boss Battle Challenge [ Arachnetta Lv 150 ]"),
//new Achievement_t(Achievement.GurdianKor, "Ashes to Ashes, Stone to Dust", "Clear Raid Boss Battle Challenge [ Guardian Kor Lv 200 ]"),
//new Achievement_t(Achievement.Nostro, "That Fight Really Sucked", "Clear Raid Boss Battle Challenge [ Nostro Lv 250 ]"),
//new Achievement_t(Achievement.LadyEmelda, "She Wouldn't Give Me the Sacred Sword", "Clear Raid Boss Battle Challenge [ Lady Emelda Lv 300 ]"),
//new Achievement_t(Achievement.NariSune, "Nine Tails, but None as Good as THIS Story", "Clear Raid Boss Battle Challenge [ Nari Sune Lv 350 ]"),
//new Achievement_t(Achievement.Octobaddie, "Octobaddie? More like Octodaddy", "Clear Raid Boss Battle Challenge [ Octobaddie Lv 400 ]"),
//new Achievement_t(Achievement.Bananoon, "Heard You Like Bananas", "Clear Raid Boss Battle Challenge [ Bananoon Lv 450 ]"),
//new Achievement_t(Achievement.Glorbliorbus, "No, THIS Is What a True Slime Killer Feels Like!", "Clear Raid Boss Battle Challenge [ Glorbliorbus Lv 500 ]"),
//new Achievement_t(Achievement.Gankyu, "Stuff of Nightmares", "Clear Raid Boss Battle Challenge [ Gankyu Lv 550 ]"),
new Achievement_t(Achievement.AlchemyMaster, "Alchemist Master", "Reach Total Potion Level 1250"),
new Achievement_t(Achievement.AlchemyGrandmaster, "Alchemist Grandmaster", "Reach Total Potion Level 3000"),
new Achievement_t(Achievement.ClassSkill8, "Class Skill Master", "Have 8 Class Skill Slots on any heroes"),
new Achievement_t(Achievement.GlobalSkill8, "Global Skill Master", "Have 8 Global Skill Slots on any heroes"),
new Achievement_t(Achievement.Rebirth1, "Baby Steps", "Perform Tier 1 Rebirth once on any heroes"),
new Achievement_t(Achievement.Rebirth2, "Learning to Walk", "Perform Tier 2 Rebirth once on any heroes"),
new Achievement_t(Achievement.Rebirth3, "Who Needs Equipment Levels Anyways?", "Perform Tier 3 Rebirth once on any heroes"),
new Achievement_t(Achievement.Rebirth4, "Tier 4 Rebirth", "Perform Tier 4 Rebirth once on any heroes"),
new Achievement_t(Achievement.Rebirth5, "Tier 5 Rebirth", "Perform Tier 5 Rebirth once on any heroes"),
new Achievement_t(Achievement.Rebirth6, "Tier 6 Rebirth", "Perform Tier 6 Rebirth once on any heroes"),
new Achievement_t(Achievement.Ascension1, "Is the Tutorial Finally Done?", "Perform Tier 1 World Ascension once"),
new Achievement_t(Achievement.Ascension2, "Welcome to the Multiverse", "Perform Tier 2 World Ascension once"),
new Achievement_t(Achievement.Ascension3, "Turn Left", "Perform Tier 3 World Ascension once"),
new Achievement_t(Achievement.Walk, "Walk Around the World", "Walk 40,075 km in total"),
new Achievement_t(Achievement.Walk2, "Walk to the Moon", "Walk 384,400 km in total"),
new Achievement_t(Achievement.Chest10000, "Our Very Own Indiana Jones", "Open 10000 Tresure Chests"),
new Achievement_t(Achievement.EpicCoin10000, "Hey Big Spender", "Spend more than 10,000 Epic Coin at Epic Store"),
};
	// Our GameID
	private CGameID m_GameID;

	// Did we get the stats from Steam?
	public bool m_bRequestedStats;
	public bool m_bStatsValid;

	// Should we store stats this frame?
	private bool m_bStoreStats;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserStatsStored_t> m_UserStatsStored;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	public SteamAchievement()
    {
        Start();
    }

    void Start()//OnEnable() //public void Start()
	{
		if (!SteamManager.Initialized)
			return;

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;

		isInitialized = true;
	}
	bool isInitialized;

	public void UpdateCheck() //Update
	{
		if (!SteamManager.Initialized)
			return;

        if (!isInitialized)
        {
			Start();
			return;
        }

		if (!m_bRequestedStats)
		{
			// Is Steam Loaded? if no, can't get stats, done
			if (!SteamManager.Initialized)
			{
				m_bRequestedStats = true;
				return;
			}

			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}

		if (!m_bStatsValid)
			return;

		// Get info from sources

		// Evaluate achievements
		foreach (Achievement_t achievement in m_Achievements)
		{
			if (achievement.m_bAchieved)
				continue;

			if (achievement.ThisCondition())
			{
				UnlockAchievement(achievement);
			}
		}

		//Store stats in the Steam database if necessary
		if (m_bStoreStats)
		{

			bool bSuccess = SteamUserStats.StoreStats();
			// If this failed, we never sent anything to the server, try
			// again later.
			m_bStoreStats = !bSuccess;
		}
	}


	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private void UnlockAchievement(Achievement_t achievement)
	{
		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

		// Store stats end of frame
		m_bStoreStats = true;
	}

	//-----------------------------------------------------------------------------
	// Purpose: We have stats data from Steam. It is authoritative, so update
	//			our data with those results now.
	//-----------------------------------------------------------------------------
	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("Received stats and achievements from Steam¥n");

				m_bStatsValid = true;

				// load achievements
				foreach (Achievement_t ach in m_Achievements)
				{
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret)
					{
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "¥nIs it registered in the Steam Partner site?");
					}
				}

				// load stats
				//SteamUserStats.GetStat("NumGames", out m_nTotalGamesPlayed);
			}
			else
			{
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Our stats data was stored!
	//-----------------------------------------------------------------------------
	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
			{
				// One or more stats we set broke a constraint. They've been reverted,
				// and we should re-iterate the values now to keep in sync.
				Debug.Log("StoreStats - some failed to validate");
				// Fake up a callback here so that we re-load the values.
				UserStatsReceived_t callback = new UserStatsReceived_t();
				callback.m_eResult = EResult.k_EResultOK;
				callback.m_nGameID = (ulong)m_GameID;
				OnUserStatsReceived(callback);
			}
			else
			{
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (0 == pCallback.m_nMaxProgress)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;
		public Func<bool> ThisCondition = () => false;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc)
		{
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
			IGetAchievementCondition condition = new IGetAchievementConditions();
			ThisCondition = () => condition.GetConditionsById((int)m_eAchievementID);
			//ThisCondition = conditions.SteamAchievementConditions[(int)m_eAchievementID];
		}
	}
}
#endif