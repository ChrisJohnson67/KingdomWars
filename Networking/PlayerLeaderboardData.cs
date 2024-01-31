//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// PlayerLeaderboardData
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using System;
using System.Text;

public class PlayerLeaderboardData
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_Delim = "_";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables


	//--- NonSerialized ---
	private string m_playerName;
	private int m_wins;
	private int m_losses;
	private int m_rating;

	public string PlayerName { get => m_playerName; set => m_playerName = value; }
	public int Wins { get => m_wins; set => m_wins = value; }
	public int Losses { get => m_losses; set => m_losses = value; }

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions
	public PlayerLeaderboardData()
	{
	}

	public PlayerLeaderboardData(string a_name)
	{
		PlayerName = a_name;
	}

	public void ReportWin(bool a_win, bool a_hasBonus)
	{
		if (a_win)
		{
			m_wins++;
			int bonus = a_hasBonus ? GameManager.Instance.KingdomSettings.BonusPoints : 0;
			m_rating += GameManager.Instance.KingdomSettings.HeroWinPoints + bonus;
		}
		else
		{
			m_losses++;
			m_rating -= GameManager.Instance.KingdomSettings.HeroLossPoints;
			m_rating = Math.Max(0, m_rating);
		}


	}

	public int GetRating()
	{
		return m_rating;
	}

	public string SerializeData()
	{
		StringBuilder data = new StringBuilder();
		data.Append(m_playerName).Append(c_Delim);
		data.Append(m_wins).Append(c_Delim);
		data.Append(m_losses).Append(c_Delim);
		data.Append(m_rating);
		return data.ToString();
	}

	public static PlayerLeaderboardData DeserializeData(string a_data)
	{
		PlayerLeaderboardData data = new PlayerLeaderboardData();
		data.Deserialize(a_data);
		return data;
	}

	public void Deserialize(string a_data)
	{
		var dataValues = a_data.Split(c_Delim);
		int index = 0;
		m_playerName = dataValues[index++];
		m_wins = Int32.Parse(dataValues[index++]);
		m_losses = Int32.Parse(dataValues[index++]);
		m_rating = Int32.Parse(dataValues[index++]);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}