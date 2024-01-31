using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// SaveDataContainer
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public static class SaveManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	private const string c_GoldName = "gold";

	private const string c_ServerPrefix = "server_";
	private const string c_SavedLevelPrefix = "savedLevel";

	#endregion Definitions


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public static List<KingdomData> GetDownloadedLevels(bool a_server)
	{
		List<KingdomData> levels = new List<KingdomData>();
		int levelCount = GetLevelCount();
		for (int i = 0; i < levelCount; i++)
		{
			var level = PlayerPrefs.GetString(GetSavedLevelString(i, a_server));
			if (!string.IsNullOrEmpty(level))
			{
				levels.Add(KingdomData.DeserializeData(level));
			}
		}
		return levels;
	}

	public static void SaveDownloadedLevels_Server(List<KingdomData> a_levels)
	{
		int levelCount = GetLevelCount();
		for (int i = levelCount; i < a_levels.Count; i++)
		{
			var level = a_levels[i];
			if (level != null)
			{
				PlayerPrefs.SetString(GetSavedLevelString(levelCount, true), level.SerializeData(false));
				levelCount += 1;
			}
		}
		SetLevelCount(levelCount);
	}

	public static void SaveUpdatedLevel(KingdomData a_level, int a_index, bool a_server)
	{
		PlayerPrefs.SetString(GetSavedLevelString(a_index, a_server), a_level.SerializeData(false));
	}

	private static string GetSavedLevelString(int a_index, bool a_server)
	{
		return (a_server ? c_ServerPrefix : string.Empty) + c_SavedLevelPrefix + a_index;
	}

	public static string[] GetLevelStringsToSend(int a_levelCount, bool a_server)
	{
		int levelCount = GetLevelCount();
		List<string> levels = new List<string>();
		for (int i = a_levelCount; i < levelCount; i++)
		{
			var level = PlayerPrefs.GetString(GetSavedLevelString(i, a_server));
			if (!string.IsNullOrEmpty(level))
			{
				levels.Add(level);
			}
		}
		return levels.ToArray();
	}

	public static string[] GetUpdatedLevelsString(int a_levelCount, bool a_server)
	{
		List<string> levels = new List<string>();
		for (int i = 0; i < a_levelCount; i++)
		{
			var level = PlayerPrefs.GetString(GetSavedLevelString(i, a_server));
			if (!string.IsNullOrEmpty(level))
			{
				level = KingdomData.ConvertFullKingdomToUpdateValues(level);
				levels.Add(level);
			}
		}
		return levels.ToArray();
	}

	public static void AddNewLevelStringsfromServer(string[] a_levelStrings, bool a_server)
	{
		if (a_levelStrings == null)
			return;

		int levelCount = GetLevelCount();
		foreach (var level in a_levelStrings)
		{
			string levelIdString = level.Substring(0, level.IndexOf(KingdomData.c_Delim));
			int levelID = tid.NULL;
			if (!string.IsNullOrEmpty(levelIdString))
			{
				Int32.TryParse(levelIdString, out levelID);
			}
			if (levelID != tid.NULL)
			{
				PlayerPrefs.SetString(GetSavedLevelString(levelCount, a_server), level);
				levelCount += 1;
				GameManager.Instance.AddCachedKingdomData(KingdomData.DeserializeData(level));
			}

		}
		SetLevelCount(levelCount);
	}

	public static int GetLevelCount()
	{
		return PlayerPrefs.GetInt("levelCount", 0);
	}

	private static void SetLevelCount(int a_count)
	{
		PlayerPrefs.SetInt("levelCount", a_count);
	}

	public static void SavePublishedLevel(KingdomData a_data)
	{
		var myLevels = GetMyLevels();
		int index = myLevels.Count;
		PlayerPrefs.SetString("myLevel" + index, a_data.SerializeData(true));
	}

	public static List<KingdomData> GetMyLevels()
	{
		List<KingdomData> levels = new List<KingdomData>();
		for (int i = 0; i < 20; i++)
		{
			var level = PlayerPrefs.GetString("myLevel" + i, string.Empty);
			if (!string.IsNullOrEmpty(level))
			{
				levels.Add(KingdomData.DeserializeData(level));
			}
		}
		return levels;
	}

	public static List<PlayerLeaderboardData> GetLeaderboard()
	{
		List<PlayerLeaderboardData> list = new List<PlayerLeaderboardData>();
		int count = GetLeaderboardCount();
		for (int i = 0; i < count; i++)
		{
			var level = PlayerPrefs.GetString("leaderboard" + i);
			if (!string.IsNullOrEmpty(level))
			{
				list.Add(PlayerLeaderboardData.DeserializeData(level));
			}
		}
		return list;
	}

	public static int GetLeaderboardCount()
	{
		return PlayerPrefs.GetInt("leaderboardCount", 0); ;
	}

	public static void SaveLeaderboard_Server(List<PlayerLeaderboardData> a_list)
	{
		int count = GetLeaderboardCount();
		for (int i = 0; i < a_list.Count; i++)
		{
			var level = a_list[i];
			if (level != null)
			{
				PlayerPrefs.SetString("leaderboard" + i, level.SerializeData());
				count += 1;
			}
		}
		PlayerPrefs.SetInt("leaderboardCount", count);
	}

	public static string GetPlayerName()
	{
		return PlayerPrefs.GetString("playerName", string.Empty);
	}

	public static void SetPlayerName(string a_name)
	{
		PlayerPrefs.SetString("playerName", a_name);
	}

	public static void ClearSavedData()
	{
		for (int i = 0; i < 300; i++)
		{
			PlayerPrefs.DeleteKey("savedLevel" + i);
		}
		for (int i = 0; i < 300; i++)
		{
			PlayerPrefs.DeleteKey(c_ServerPrefix + "savedLevel" + i);
		}
		for (int i = 0; i < 50; i++)
		{
			PlayerPrefs.DeleteKey("myLevel" + i);
		}
		PlayerPrefs.DeleteKey("levelCount");
		PlayerPrefs.DeleteKey("playerName");
		for (int i = 0; i < 200; i++)
		{
			PlayerPrefs.DeleteKey("leaderboard" + i);
		}
		PlayerPrefs.DeleteKey("leaderboardCount");
		PlayerPrefs.DeleteKey(c_GoldName);
	}

	public static void AddGold(int a_amount)
	{
		PlayerPrefs.SetInt(c_GoldName, GetGoldAmount() + a_amount);
	}

	public static int GetGoldAmount()
	{
		return PlayerPrefs.GetInt(c_GoldName, 0);
	}

	#endregion Runtime Functions
}