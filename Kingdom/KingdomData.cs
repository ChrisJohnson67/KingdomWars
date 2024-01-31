using System;
using System.Collections.Generic;
using System.Text;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// KingdomData
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class KingdomData
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_Delim = "_";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private int m_levelId;
	private string m_author;
	private string m_title;
	private List<KingdomRoomData> m_roomDataList = new List<KingdomRoomData>();
	private int m_wins;
	private int m_losses;
	private List<string> m_comments = new List<string>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int LevelID { get { return m_levelId; } }
	public List<KingdomRoomData> RoomDataList { get { return m_roomDataList; } }
	public string Author { get { return m_author; } }
	public string Title { get { return m_title; } }
	public List<string> Comments { get { return m_comments; } }

	public int Wins { get => m_wins; set => m_wins = value; }
	public int Losses { get => m_losses; set => m_losses = value; }


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public int GetRating()
	{
		return Math.Max(0, Wins * GameManager.Instance.KingdomSettings.KingdomWinPoints + Losses * GameManager.Instance.KingdomSettings.KingdomLossPoints);
	}

	public List<int> GetDangers()
	{
		List<int> dangerList = new List<int>();
		foreach (var roomData in m_roomDataList)
		{
			dangerList.AddRange(roomData.GetDangers());
		}
		return dangerList;
	}

	public void SetLevelId(int a_id)
	{
		m_levelId = a_id;
	}

	public void SetTextDetails(string a_author, string a_title)
	{
		m_author = a_author;
		m_title = a_title;
	}

	public void ReportWin(bool a_heroWin)
	{
		if (!a_heroWin)
			Wins++;
		else
			Losses++;
	}

	public string SerializeData(bool a_local)
	{
		StringBuilder data = new StringBuilder();
		data.Append(m_levelId).Append(c_Delim);
		data.Append(m_author).Append(c_Delim);
		data.Append(m_title).Append(c_Delim);

		foreach (var roomData in m_roomDataList)
		{
			roomData.SerializeData(data);
		}

		data.Append(Wins).Append(c_Delim);
		data.Append(Losses).Append(c_Delim);

		//serialize comments if local player levels
		if (a_local)
		{
			foreach (var comment in m_comments)
			{
				data.Append(comment).Append(c_Delim);
			}
		}

		//remove last delim
		data.Remove(data.Length - 1, 1);

		return data.ToString();
	}

	public static KingdomData DeserializeData(string a_data)
	{
		KingdomData kingdom = new KingdomData();
		kingdom.Deserialize(a_data);
		return kingdom;
	}

	public void Deserialize(string a_data)
	{
		var dataValues = a_data.Split(c_Delim);
		int index = 0;
		m_levelId = Int32.Parse(dataValues[index++]);
		m_author = dataValues[index++];
		m_title = dataValues[index++];

		for (int i = 0; i < GameManager.Instance.KingdomSettings.RoomNumber; i++)
		{
			var roomData = new KingdomRoomData();
			roomData.Deserialize(dataValues, ref index);
			m_roomDataList.Add(roomData);
		}

		m_wins = Int32.Parse(dataValues[index++]);
		m_losses = Int32.Parse(dataValues[index++]);

		for (; index < dataValues.Length; index++)
		{
			m_comments.Add(dataValues[index]);
		}
	}

	public static string ConvertFullKingdomToUpdateValues(string a_kingdomString)
	{
		StringBuilder builder = new StringBuilder();
		var stringVals = a_kingdomString.Split(c_Delim);
		int index = 0;
		builder.Append(stringVals[index++]).Append(c_Delim);
		index += 2;
		index += GameManager.Instance.KingdomSettings.RoomChoices * GameManager.Instance.KingdomSettings.RoomNumber;

		//add wins and losses
		builder.Append(stringVals[index++]).Append(c_Delim);
		builder.Append(stringVals[index++]);
		return builder.ToString();
	}

	public static void DeserializeUpdatedKingdomString(string a_data, out int a_levelID, out int a_wins, out int a_losses)
	{
		var dataValues = a_data.Split(c_Delim);
		int index = 0;
		a_levelID = Int32.Parse(dataValues[index++]);
		a_wins = Int32.Parse(dataValues[index++]);
		a_losses = Int32.Parse(dataValues[index++]);
	}

	public static KingdomData CreateNewKingdomToPublish()
	{
		KingdomData kingdom = new KingdomData();
		var levelNumber = UnityEngine.Random.Range(1, Int32.MaxValue);
		kingdom.SetLevelId(levelNumber);
		return kingdom;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks


}
