using Photon.Pun;
using Photon.Realtime;
using Platform.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomNetworkingManager : MonoBehaviourPunCallbacks
{
	#region Definitions

	private static readonly LogSubSystem SubSys = LogSubSystem.Create("PhotonNetworkingManager", LogLevel.All);
	private const int c_NoRoomFoundCode = 32760;
	private const string c_Delimiter = "|";

	public enum Intent
	{
		RequestingLevels,
		PublishingLevel,
		RequestingLeaderboard,
		SendingCombatResult,
		HostingServer
	}

	public enum ConnectionState
	{
		NotConnected,
		Connecting,
		JoiningServer,
		Connected,
	}

	#endregion Definitions

	#region Private Fields

	[SerializeField]
	private string m_serverRoomName = "Server";

	[SerializeField]
	private int m_maxConnectionTime = 60;

	/// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
	[SerializeField]
	private string m_gameVersion = "1";

	private Intent m_intent;
	private ConnectionState m_connectState = ConnectionState.NotConnected;
	private bool m_disconnectedByChoice = false;
	private Coroutine m_kickCR;
	private KingdomServerManager m_serverManager;
	private KingdomData m_kingdomToPublish;
	private int m_kingdomId;
	private bool m_combatWin;
	private bool m_hasBonus;
	private Action<bool> m_onRequestComplete;
	public static Action<int, int, int> OnPlayerLevelUpdated;
	public static Action OnLevelUpdatesComplete;

	public static Action<bool> OnServerHosting;


	public bool IsConnectedToNetwork { get { return m_connectState != ConnectionState.NotConnected; } }
	public ConnectionState ConnectState { get { return m_connectState; } }

	public static Action<DisconnectCause> OnNetworkDisconnected;

	private static KingdomNetworkingManager sm_instance;


	#endregion

	public static KingdomNetworkingManager Instance { get { return sm_instance; } }

	#region MonoBehaviour CallBacks
	private void Awake()
	{
		sm_instance = this;
		PhotonNetwork.UseRpcMonoBehaviourCache = true;
	}

	private void OnDestroy()
	{
		sm_instance = null;
	}

	#endregion


	#region Public Methods

	/// <summary>
	/// Start the connection to Photon
	/// </summary>
	public void RequestLevels(Action<bool> a_onRequestComplete)
	{
		m_intent = Intent.RequestingLevels;
		m_onRequestComplete = a_onRequestComplete;
		ConnectToPhoton();
	}

	public void PublishLevel(KingdomData a_data, Action<bool> a_onRequestComplete)
	{
		m_kingdomToPublish = a_data;
		m_onRequestComplete = a_onRequestComplete;
		m_intent = Intent.PublishingLevel;
		ConnectToPhoton();
	}

	public void ReportCombat(int a_kingdomId, bool a_win, bool a_hasBonus, Action<bool> a_onRequestComplete)
	{
		m_kingdomId = a_kingdomId;
		m_combatWin = a_win;
		m_hasBonus = a_hasBonus;
		m_onRequestComplete = a_onRequestComplete;
		m_intent = Intent.SendingCombatResult;
		ConnectToPhoton();
	}

	public void RequestLeaderboard(Action<bool> a_onRequestComplete)
	{
		m_onRequestComplete = a_onRequestComplete;
		m_intent = Intent.RequestingLeaderboard;
		ConnectToPhoton();
	}

	public void HostServer()
	{
		m_intent = Intent.HostingServer;
		ConnectToPhoton();
	}

	/// <summary>
	/// Disconnect completely from Photon
	/// </summary>
	public void Disconnect(bool a_requestReceived)
	{
		m_disconnectedByChoice = true;
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		if (m_kickCR != null)
		{
			StopCoroutine(m_kickCR);
			m_kickCR = null;
		}
		m_connectState = ConnectionState.NotConnected;
		Cleanup();
		m_onRequestComplete?.Invoke(!a_requestReceived);

		if (m_serverManager != null)
		{
			OnServerHosting?.Invoke(false);
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Start the connection process.
	/// - If already connected, we attempt joining a random room
	/// - if not yet connected, Connect this application instance to Photon Cloud Network
	/// </summary>
	private void ConnectToPhoton()
	{
		m_connectState = ConnectionState.Connecting;
		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.IsConnected)
		{
			ApplyIntent();
		}
		else
		{
			// keep track of the will to join a room, because when we come back from the game we will get a callback - OnConnectedToMaster - that we are connected, so we need to know what to do then
			var connecting = PhotonNetwork.ConnectUsingSettings();

			//add the service url to Photon's GameVersion to separate players connected to Photon playing on different services
			var clientConfig = ClientConfig.Instance();
			var serviceURL = clientConfig.Get<string>("ServiceURL");
			if (!string.IsNullOrEmpty(serviceURL))
			{
				//remove the prefix
				int index = serviceURL.IndexOf("//");
				if (index > 0 && index < serviceURL.Length)
				{
					serviceURL = serviceURL.Substring(index, serviceURL.Length - index);
				}

				//remove the postfix
				index = serviceURL.IndexOf(".");
				if (index > 0 && index < serviceURL.Length)
				{
					serviceURL = serviceURL.Substring(0, index);
				}
			}
			PhotonNetwork.GameVersion = serviceURL + "_" + m_gameVersion;
			log.Debug(SubSys.Status() + "Game Version: " + PhotonNetwork.GameVersion);
			if (!connecting)
			{
				log.Debug(SubSys.Error() + "Failed to connect to Photon");
			}
		}
	}

	private IEnumerator ConnectCR()
	{
		yield return null;
		ConnectToPhoton();
	}

	/// <summary>
	/// Perform the requested intent before we had connected to Photon
	/// </summary>
	private void ApplyIntent()
	{
		switch (m_intent)
		{
			case Intent.RequestingLevels:
			case Intent.PublishingLevel:
			case Intent.RequestingLeaderboard:
			case Intent.SendingCombatResult:
				JoinServerRoom();
				m_kickCR = StartCoroutine(KickFromServerCR());
				break;

			case Intent.HostingServer:
				CreateServerRoom();
				break;
		}

	}

	private IEnumerator KickFromServerCR()
	{
		yield return new WaitForSeconds(m_maxConnectionTime);
		m_kickCR = null;
		Disconnect(false);
	}

	/// <summary>
	/// Attempt to join a random existing room. If it fails, it will try to host a match
	/// </summary>
	private void JoinServerRoom()
	{
		m_connectState = ConnectionState.JoiningServer;
		// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
		var searching = PhotonNetwork.JoinRoom(m_serverRoomName);
		if (!searching)
		{
			log.Debug(SubSys.Error() + "Failed to join a random room. Retrying...");
			Disconnect(true);
		}
	}

	/// <summary>
	/// Create the server room
	/// </summary>
	private void CreateServerRoom()
	{
		m_connectState = ConnectionState.JoiningServer;
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 0;
		options.IsOpen = true;
		var roomCreated = PhotonNetwork.CreateRoom(m_serverRoomName, options);
		if (!roomCreated)
		{
			log.Debug(SubSys.Error() + "Failed to host server");
			Disconnect(false);
		}
	}
	#endregion

	#region MonoBehaviourPunCallbacks Callbacks

	/// <summary>
	/// Callback from Photon when the client has just connected to the Photon servers
	/// </summary>
	public override void OnConnectedToMaster()
	{
		//PhotonNetwork.NickName = SaveManager.Instance.PlayerName;
		log.Debug(SubSys.Status() + "Connected to the Photon server");
		// we don't want to do anything if we are not attempting to join a room.
		// this case where m_connecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.
		if (m_connectState == ConnectionState.Connecting)
		{
			ApplyIntent();
		}
		else
		{
			Disconnect(false);
		}
	}

	/// <summary>
	/// Callback from Photon when the client is disconnected from the network
	/// </summary>
	/// <param name="cause"></param>
	public override void OnDisconnected(DisconnectCause a_cause)
	{
		log.Debug(SubSys.Status() + "Disconnected from Photon with reason: " + a_cause.ToString());
		m_connectState = ConnectionState.NotConnected;
		if (a_cause == DisconnectCause.MaxCcuReached)
		{

		}
		else if (a_cause != DisconnectCause.DisconnectByClientLogic || !m_disconnectedByChoice)
		{
			//MessageBox.OK(GameManager.Instance.LocalizationSettings.DisconnectedFromNetworkUnknowReason);
		}

		OnNetworkDisconnected?.Invoke(a_cause);

		Cleanup();
	}

	/// <summary>
	/// Callback when the client has failed to join a room or no rooms exist. If none exist, create a room
	/// </summary>
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		log.Debug(SubSys.Status() + "OnJoinRoomFailed() was called by PUN.");

		if (m_connectState != ConnectionState.JoiningServer)
		{
			log.Debug(SubSys.Error() + "Found no room to join, about to create a new one, but canceled. Disconnecting...");
			Disconnect(false);
			return;
		}
		else
		{
			Disconnect(false);
			log.Debug(SubSys.Error() + "OnJoinRoomFailed failed with casue: " + message);
		}
	}

	/// <summary>
	/// Callback when the client has failed to create a room
	/// </summary>
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		log.Debug(SubSys.Error() + "Creating a room failed with casue: " + message);
		Disconnect(false);
	}

	/// <summary>
	/// Callback when the client has successfully hosted a room
	/// </summary>
	public override void OnCreatedRoom()
	{
		log.Debug(SubSys.Status() + "OnCreatedRoom() was called by PUN");
		m_connectState = ConnectionState.Connected;

		m_serverManager = new KingdomServerManager();

		OnServerHosting?.Invoke(true);
	}

	/// <summary>
	/// Callback when the client has successfully joined another room hosted by another player
	/// </summary>
	public override void OnJoinedRoom()
	{
		if (m_intent == Intent.HostingServer)
		{
			return;
		}

		if (m_connectState != ConnectionState.JoiningServer)
		{
			log.Debug(SubSys.Error() + "Joined a room, but was not searching for a match. Disconnecting...");
			Disconnect(false);
			return;
		}
		log.Debug(SubSys.Status() + "OnJoinedRoom() called by PUN. Now this client is in a room. Master Client: " + PhotonNetwork.IsMasterClient);
		m_connectState = ConnectionState.Connected;

		switch (m_intent)
		{
			case Intent.RequestingLevels:
				SendMasterClientLevelsRequest();
				break;

			case Intent.PublishingLevel:
				SendMasterClientPublishedLevelsRequest();
				break;

			case Intent.SendingCombatResult:
				SendMasterClientCombatResultRequest();
				break;

			case Intent.RequestingLeaderboard:
				SendMasterClientLeaderboardRequest();
				break;
		}
	}

	/// <summary>
	/// Callback when this client detects that another player has left the current room
	/// </summary>
	/// <param name="a_otherPlayer"></param>
	public override void OnPlayerLeftRoom(Player a_otherPlayer)
	{
		log.Debug(SubSys.Status() + "OnPlayerLeftRoom() " + a_otherPlayer.NickName);
	}

	/// <summary>
	/// Callback when this client is hosting a room and it detects that another player has joined the room. 
	/// The player joining the room will not see this callback
	/// </summary>
	public override void OnPlayerEnteredRoom(Player a_newPlayer)
	{
		log.Debug(SubSys.Status() + "OnPlayerEnteredRoom() " + a_newPlayer.NickName); // not seen if you're the player connecting
	}

	#endregion

	#region RPC Functions

	#region Send Level Download Request
	private void SendMasterClientLevelsRequest()
	{
		//send count of levels already seen
		var downloadedLevelCount = SaveManager.GetLevelCount();
		photonView.RPC(nameof(RpcReceiveRequestLevels), RpcTarget.MasterClient, downloadedLevelCount);
	}

	[PunRPC]
	private void RpcReceiveRequestLevels(int a_levelCount, PhotonMessageInfo a_info)
	{
		GetLevelsAndSend(a_levelCount, a_info);
	}

	private void GetLevelsAndSend(int a_levelCount, PhotonMessageInfo a_info)
	{
		var stringList = SaveManager.GetLevelStringsToSend(a_levelCount, true) as object;
		var updatedLevelStringList = SaveManager.GetUpdatedLevelsString(a_levelCount, true) as object;
		photonView.RPC(nameof(RpcReceiveLevels), a_info.Sender, stringList, updatedLevelStringList);
	}

	[PunRPC]
	private void RpcReceiveLevels(object a_data, object a_updatedData, PhotonMessageInfo a_info)
	{
		if (a_data != null)
		{
			var stringList = a_data as string[];
			SaveManager.AddNewLevelStringsfromServer(stringList, false);


			var myLevels = SaveManager.GetMyLevels();

			var updatedStringList = a_updatedData as string[];
			for (int i = 0; i < updatedStringList.Length; i++)
			{
				string update = updatedStringList[i];
				KingdomData.DeserializeUpdatedKingdomString(update, out int levelID, out int wins, out int losses);

				var kingdomData = GameManager.Instance.CachedKingdomData.Find(x => x.LevelID == levelID);
				if (kingdomData != null)
				{
					if (myLevels.Find(x => x.LevelID == levelID) != null)
					{
						OnPlayerLevelUpdated(levelID, wins - kingdomData.Wins, losses - kingdomData.Losses);
					}

					kingdomData.Wins = wins;
					kingdomData.Losses = losses;
					SaveManager.SaveUpdatedLevel(kingdomData, GameManager.Instance.CachedKingdomData.IndexOf(kingdomData), false);
				}
			}

			OnLevelUpdatesComplete?.Invoke();
		}
		Disconnect(true);
	}

	#endregion Send Level Download Request

	#region Send Published Level

	private void SendMasterClientPublishedLevelsRequest()
	{
		photonView.RPC(nameof(RpcReceivePublishedLevels), RpcTarget.MasterClient, m_kingdomToPublish.SerializeData(false));
	}

	[PunRPC]
	private void RpcReceivePublishedLevels(string a_levelData, PhotonMessageInfo a_info)
	{
		var kingdomData = new KingdomData();
		kingdomData.Deserialize(a_levelData);
		m_serverManager.AddKingdomData(kingdomData);
		photonView.RPC(nameof(RpcRequestReceived), a_info.Sender);
	}



	#endregion Send Published Level

	#region Send Combat Results

	private void SendMasterClientCombatResultRequest()
	{
		var downloadedLevelCount = SaveManager.GetLevelCount();
		photonView.RPC(nameof(RpcReceiveCombatResult), RpcTarget.MasterClient, m_kingdomId, m_combatWin, downloadedLevelCount, m_hasBonus);
	}

	[PunRPC]
	private void RpcReceiveCombatResult(int a_kingdomID, bool a_win, int a_levelCount, bool a_hasBonus, PhotonMessageInfo a_info)
	{
		m_serverManager.ReportWin(a_info.Sender.NickName, a_kingdomID, a_win, a_hasBonus);

		GetLevelsAndSend(a_levelCount, a_info);
	}

	#endregion Send Combat Results

	#region Send LeaderboardRequest
	private void SendMasterClientLeaderboardRequest()
	{
		photonView.RPC(nameof(RpcReceiveLeaderboardRequest), RpcTarget.MasterClient);
	}

	[PunRPC]
	private void RpcReceiveLeaderboardRequest(PhotonMessageInfo a_info)
	{
		var leaderboard = m_serverManager.GetLeaderboardToSend();
		photonView.RPC(nameof(RpcReceiveLeaderboard), a_info.Sender, leaderboard as object);
	}


	[PunRPC]
	private void RpcReceiveLeaderboard(object a_data, PhotonMessageInfo a_info)
	{
		var leaderboardArray = a_data as string[];
		List<PlayerLeaderboardData> list = new List<PlayerLeaderboardData>();
		for (int i = 0; i < leaderboardArray.Length; i++)
		{
			var data = leaderboardArray[i];
			list.Add(PlayerLeaderboardData.DeserializeData(data));
		}
		GameManager.Instance.UpdateCachedLeaderboard(list);
		Disconnect(true);
	}

	#endregion Send LeaderboardRequest

	[PunRPC]
	private void RpcRequestReceived(PhotonMessageInfo a_info)
	{
		Disconnect(true);
	}

	#endregion RPC Functions

	public void Cleanup()
	{

	}

	#region CSR Test Functions


	#endregion CSR Test Functions
}
