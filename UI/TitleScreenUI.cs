//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// TitleScreenUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using DarkTonic.MasterAudio;
using Platform.UIManagement;
using System;
using TMPro;
using UnityEngine;

public class TitleScreenUI : FullscreenUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GameObject m_displayPrefab;

	[SerializeField]
	private GameObject m_host;

	[SerializeField]
	private GameObject m_nameObj;

	[SerializeField]
	private TMP_Text m_nametext;


	//--- NonSerialized ---
	private GameObject m_displayObject;

	public static Action<bool> OnTitleOpen;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		m_displayObject = Instantiate(m_displayPrefab);
		UIUtils.SetActive(m_host, false);

		MasterAudio.ChangePlaylistByName("MX_Title");

		KingdomNetworkingManager.OnServerHosting += OnServerHost;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		KingdomNetworkingManager.OnServerHosting -= OnServerHost;

		OnTitleOpen?.Invoke(false);
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		string name = SaveManager.GetPlayerName();
		UIUtils.SetActive(m_nameObj, !string.IsNullOrEmpty(name));
		m_nametext.text = name;

		OnTitleOpen?.Invoke(true);

		NotificationManager.Instance.QueuePopups();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
		{
			AudioManager.Instance.MusicVolume = Mathf.Max(0f, AudioManager.Instance.MusicVolume / 2f);
			AudioManager.Instance.SoundVolume = Mathf.Max(AudioManager.Instance.SoundVolume / 2f);
		}

		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.U))
		{
			AudioManager.Instance.MusicVolume = Mathf.Min(1f, AudioManager.Instance.MusicVolume * 2f);
			AudioManager.Instance.SoundVolume = Mathf.Min(1f, AudioManager.Instance.SoundVolume * 2f);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	protected override void Clear()
	{
		base.Clear();
		Destroy(m_displayObject.gameObject);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnCreateButton()
	{
		if (string.IsNullOrEmpty(SaveManager.GetPlayerName()))
		{
			var popup = UIManager.Instance.OpenUI<ChooseNamePopup>(GameManager.Instance.UISettings.NamePopupTID);
			popup.Init(OnCreateKingdom);
		}
		else
		{
			OnCreateKingdom();
		}

	}

	private void OnCreateKingdom()
	{
		CloseUI();
		UIManager.Instance.OpenUI<RoomCreationUI>(GameManager.Instance.UISettings.RoomCreationTID);
	}

	public void OnConquerButton()
	{
		if (string.IsNullOrEmpty(SaveManager.GetPlayerName()))
		{
			var popup = UIManager.Instance.OpenUI<ChooseNamePopup>(GameManager.Instance.UISettings.NamePopupTID);
			popup.Init(OnBattle);
		}
		else
		{
			OnBattle();
		}
	}

	private void OnBattle()
	{
		CloseUI();
		var ui = UIManager.Instance.OpenUI<LevelSelectUI>(GameManager.Instance.UISettings.LevelSelectUITID);
		ui.Init();
	}

	public void OnQuitButton()
	{
		Application.Quit();
	}

	public void OnLeaderboardButton()
	{
		UIManager.Instance.OpenUI<LeaderboardUI>(GameManager.Instance.UISettings.LeaderboardUITID);
	}

	public void OnMyLevelsButton()
	{
		UIManager.Instance.OpenUI<MyLevelsUI>(GameManager.Instance.UISettings.LevelUI);
	}

	private void OnServerHost(bool a_host)
	{
		UIUtils.SetActive(m_host, a_host);
	}

	#endregion Callbacks

}