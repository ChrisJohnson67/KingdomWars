using System.Collections.Generic;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// KingdomServerManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class KingdomServerManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private List<KingdomData> m_kingdomDataList = new List<KingdomData>();
	private List<PlayerLeaderboardData> m_leaderboardData = new List<PlayerLeaderboardData>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<KingdomData> KingdomDataList { get { return m_kingdomDataList; } }
	public int KingdomListCount { get { return m_kingdomDataList.Count; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public KingdomServerManager()
	{
		m_kingdomDataList.AddRange(SaveManager.GetDownloadedLevels(true));
		m_leaderboardData.AddRange(SaveManager.GetLeaderboard());
	}

	public void AddKingdomData(KingdomData a_data)
	{
		if (a_data == null)
			return;

		var kingdom = m_kingdomDataList.Find(x => x.LevelID.Equals(a_data.LevelID));
		if (kingdom == null)
		{
			m_kingdomDataList.Add(a_data);
			SaveManager.SaveDownloadedLevels_Server(m_kingdomDataList);
		}
	}

	public void ReportWin(string a_playerName, int a_kingdomID, bool a_win, bool a_hasBonus)
	{
		var kingdomData = m_kingdomDataList.Find(x => x != null && x.LevelID == a_kingdomID);
		if (kingdomData != null)
		{
			kingdomData.ReportWin(a_win);
			SaveManager.SaveUpdatedLevel(kingdomData, m_kingdomDataList.IndexOf(kingdomData), true);
		}

		var leaderboardData = m_leaderboardData.Find(x => x != null && x.PlayerName.Equals(a_playerName));
		if (leaderboardData == null)
		{
			leaderboardData = new PlayerLeaderboardData(a_playerName);
			m_leaderboardData.Add(leaderboardData);
		}
		leaderboardData.ReportWin(a_win, a_hasBonus);
		SaveManager.SaveLeaderboard_Server(m_leaderboardData);
	}

	public string[] GetLeaderboardToSend()
	{
		List<string> list = new List<string>();
		foreach (var player in m_leaderboardData)
		{
			list.Add(player.SerializeData());
		}

		return list.ToArray();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}