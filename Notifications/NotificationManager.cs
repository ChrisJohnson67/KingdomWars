using Platform.UIManagement;
using Platform.Utility;
using System.Collections.Generic;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// NotificationManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class NotificationManager : MonoSingleton<NotificationManager>
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public class KingdomUpdateData
	{
		public int LevelID { get; set; }
		public int WinDiff { get; set; }
		public int LossDiff { get; set; }

		public KingdomUpdateData(int a_id, int a_win, int a_loss)
		{
			LevelID = a_id;
			WinDiff = a_win;
			LossDiff = a_loss;
		}
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---


	//--- NonSerialized ---
	private List<KingdomUpdateData> m_updateList = new List<KingdomUpdateData>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	protected override void Awake()
	{
		base.Awake();

		KingdomNetworkingManager.OnPlayerLevelUpdated += OnPlayerLevelUpdated;
		KingdomNetworkingManager.OnLevelUpdatesComplete += OnLevelUpdatesComplete;
		TitleScreenUI.OnTitleOpen += OnTitleScreenOpen;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		KingdomNetworkingManager.OnPlayerLevelUpdated -= OnPlayerLevelUpdated;
		KingdomNetworkingManager.OnLevelUpdatesComplete -= OnLevelUpdatesComplete;
		TitleScreenUI.OnTitleOpen -= OnTitleScreenOpen;
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void OnPlayerLevelUpdated(int a_id, int a_wins, int a_losses)
	{
		if (a_wins <= 0 && a_losses <= 0)
			return;

		m_updateList.Add(new KingdomUpdateData(a_id, a_wins, a_losses));
	}

	public void QueuePopups()
	{
		if (m_updateList.Count > 0)
		{
			if (UIManager.Instance.IsOpen<NotificationPopup>())
			{
				return;
			}
			var entry = UIManager.Instance.OpenUI<NotificationPopup>(GameManager.Instance.UISettings.NotifTID);
			if (entry != null)
			{
				var data = m_updateList[0];
				m_updateList.RemoveAt(0);
				entry.Init(data, QueuePopups);
			}
		}
	}

	private void OnLevelUpdatesComplete()
	{
		var titleScreen = UIManager.Instance.GetUI<TitleScreenUI>();
		if (titleScreen != null)
		{
			QueuePopups();
		}
	}

	private void OnTitleScreenOpen(bool a_open)
	{
		if (a_open)
		{
			QueuePopups();
		}
		else
		{
			m_updateList.Clear();
		}
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}