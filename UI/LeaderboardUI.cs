using Platform.UIManagement;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LeaderboardUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class LeaderboardUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_kingdomEntryParent;

	[SerializeField, TemplateIDField(typeof(LeaderboardEntry), "king entry", "")]
	private int m_kingdomEntryTID;

	[SerializeField]
	private Transform m_heroEntryParent;

	[SerializeField, TemplateIDField(typeof(LeaderboardEntry), "hero entry", "")]
	private int m_heroEntryTID;

	[SerializeField]
	private GameObject m_spinnerObject;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void OnEnable()
	{
		if (KingdomNetworkingManager.Instance.IsConnectedToNetwork)
		{
			CreateEntries();
		}
		else
		{
			UIUtils.SetActive(m_spinnerObject, true);
			KingdomNetworkingManager.Instance.RequestLeaderboard(OnRequestLeaderboard);
		}
	}

	private void OnRequestLeaderboard(bool a_error)
	{
		if (this == null)
			return;

		if (a_error)
			GenericPopupUI.CreatePopup("Could not update from server", CreateEntries);
		else
			CreateEntries();
	}

	private void CreateEntries()
	{
		UIUtils.SetActive(m_spinnerObject, false);
		var kingdomLevels = new List<KingdomData>(GameManager.Instance.CachedKingdomData);
		string winKingdom = "W: ";
		string lossKingdom = "L: ";
		string winHero = "Kingdoms Conquered: ";
		string lossHero = "Hero Deaths: ";

		kingdomLevels.Sort(SortKingdoms);
		foreach (var level in kingdomLevels)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<LeaderboardEntry>(m_kingdomEntryTID, m_kingdomEntryParent);
			entry.Init(level.GetRating(), winKingdom + level.Wins, lossKingdom + level.Losses, level.Title, level.Author);
		}

		var heroesList = GameManager.Instance.CachedLeaderboard;
		heroesList.Sort(SortHeroes);

		foreach (var player in heroesList)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<LeaderboardEntry>(m_heroEntryTID, m_heroEntryParent);
			entry.Init(player.GetRating(), winHero + player.Wins, lossHero + player.Losses, player.PlayerName, string.Empty);
		}
	}

	private int SortKingdoms(KingdomData a_left, KingdomData a_right)
	{
		if (a_left == a_right)
			return 0;

		int leftRate = a_left.GetRating();
		int rightRate = a_right.GetRating();
		if (leftRate == rightRate)
		{
			if (a_left.Wins == a_right.Wins)
			{
				return a_left.LevelID.CompareTo(a_right.LevelID);
			}
			return a_left.Wins.CompareTo(a_right.Wins);
		}

		return rightRate.CompareTo(leftRate);
	}

	private int SortHeroes(PlayerLeaderboardData a_left, PlayerLeaderboardData a_right)
	{
		if (a_left == a_right)
			return 0;

		int leftRate = a_left.GetRating();
		int rightRate = a_right.GetRating();
		if (leftRate == rightRate)
		{
			if (a_left.Wins == a_right.Wins)
			{
				return a_left.PlayerName.CompareTo(a_right.PlayerName);
			}
			return a_left.Wins.CompareTo(a_right.Wins);
		}

		return rightRate.CompareTo(leftRate);
	}

	public void CloseUI()
	{
		UIManager.Instance.CloseUI(this);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}