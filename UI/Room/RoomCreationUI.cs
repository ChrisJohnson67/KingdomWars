using DarkTonic.MasterAudio;
using Platform.UIManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomCreationUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomCreationUI : FullscreenUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum CreationState
	{
		None,
		Intro,
		CreateRoom,
		ChoosingCore,
		ChoosingMonsters,
		ChoosingSpell,
		CompletingRoom,
		CompletingKingdom
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_titleText;

	[SerializeField]
	private Button m_confirmButton;

	[SerializeField]
	private Button m_rerollButton;

	[SerializeField]
	private TMP_Text m_rerollText;

	[SerializeField]
	private TMP_Text m_roomCounterText;

	[SerializeField, TemplateIDField(typeof(RoomChoiceEntry), "Choice Entry", "")]
	private int m_roomChoiceEntryTID;

	[SerializeField]
	private Transform m_choiceParent;

	[SerializeField, TemplateIDField(typeof(CameraController), "Camera Controller", "")]
	private int m_cameraTID;

	[SerializeField]
	private DangerDisplay m_dangerDisplay;

	[SerializeField]
	private GameObject m_dangerObjectPanel;

	[SerializeField]
	private SpecificSoundGroup m_soundSpellCreate;


	//--- NonSerialized ---
	private KingdomData m_kingdomData;
	private KingdomRoomData m_currentRoomData;
	private List<RoomModel> m_roomModels = new List<RoomModel>();
	private List<UnitModel> m_unitModels = new List<UnitModel>();
	private CreationState m_state = CreationState.None;
	private int m_rerolls;
	private int m_currentRoomIndex;
	private BaseRoomModel m_introRoom;
	private RoomModel m_currentRoomModel;
	private int m_chosenRoomChoiceTID;
	private RoomChoiceEntry m_selectedEntry;
	private CameraController m_cameraController;
	private List<UnitModel> m_displayedUnitModels = new List<UnitModel>();
	private RaycastHit[] m_raycastHits = new RaycastHit[4];
	private List<RoomChoiceEntry> m_choiceEntries = new List<RoomChoiceEntry>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void Awake()
	{
		InputSystem.TouchDownEvent += OnTouchDown;

		MasterAudio.ChangePlaylistByName("KingdomCreation");
	}

	private void OnDestroy()
	{
		InputSystem.TouchDownEvent -= OnTouchDown;
	}

	private void Start()
	{
		m_introRoom = AssetCacher.Instance.InstantiateComponent<BaseRoomModel>(GameManager.Instance.KingdomSettings.IntroRoomModel);
		m_kingdomData = KingdomData.CreateNewKingdomToPublish();
		m_cameraController = AssetCacher.Instance.InstantiateComponent<CameraController>(m_cameraTID);
		//m_cameraController.MoveTo(Vector3.zero, null);
		EnableRerollButton(false);
		ActivateConfirmButton(false);
		UIUtils.SetActive(m_confirmButton, false);
		UIUtils.SetActive(m_dangerObjectPanel, false);
		UIUtils.SetActive(m_titleText, false);
		m_rerolls = GameManager.Instance.KingdomSettings.Rerolls;
		m_rerollText.text = m_rerolls.ToString();
		m_roomCounterText.text = (m_currentRoomIndex + 1).ToString() + "/" + GameManager.Instance.KingdomSettings.RoomNumber;
		SwitchState(CreationState.Intro);
	}

	protected override void Clear()
	{
		base.Clear();

		Destroy(m_cameraController.gameObject);
		foreach (var room in m_roomModels)
		{
			room.Cleanup();
			Destroy(room.gameObject);
		}

		foreach (var unit in m_unitModels)
		{
			unit.Cleanup();
			Destroy(unit.gameObject);
		}

		foreach (var unit in m_displayedUnitModels)
		{
			unit.Cleanup();
			Destroy(unit.gameObject);
		}

		if (m_currentRoomModel != null)
		{
			m_currentRoomModel.Cleanup();
			Destroy(m_currentRoomModel.gameObject);
		}

		if (m_introRoom != null)
		{
			m_introRoom.Cleanup();
			Destroy(m_introRoom.gameObject);
		}

		UIManager.Instance.CloseUI(GameManager.Instance.UISettings.RoomNamePopupUITID);
	}

	private void SwitchState(CreationState a_state)
	{
		if (m_state != a_state)
		{
			m_state = a_state;
			InitState();
		}
	}

	private void InitState()
	{
		switch (m_state)
		{
			case CreationState.Intro:
				DoIntroSequence();
				break;

			case CreationState.CreateRoom:
				CreateRoom();
				break;

			case CreationState.ChoosingCore:
				PickCoreChoices();
				break;

			case CreationState.ChoosingMonsters:
				PickMonsterChoices();
				break;

			case CreationState.ChoosingSpell:
				PickSpellChoices();
				break;

			case CreationState.CompletingRoom:
				CompleteRoom();
				break;

			case CreationState.CompletingKingdom:
				CompleteKingdom();
				break;
		}
	}

	private void DoIntroSequence()
	{
		WaitTime(GameManager.Instance.KingdomSettings.KingdomIntroTime, () =>
		{
			MoveCameraFromIntroToNextRoom();
		});
	}

	private void CreateRoom()
	{
		int roomModelTID = m_currentRoomIndex == GameManager.Instance.KingdomSettings.RoomNumber - 1 ? GameManager.Instance.KingdomSettings.CastleRoomModel : GameManager.Instance.KingdomSettings.CreationBasicRoomModel;
		m_currentRoomModel = AssetCacher.Instance.InstantiateComponent<RoomModel>(roomModelTID);
		m_roomModels.Add(m_currentRoomModel);
		if (m_roomModels.Count > 1)
		{
			var previousModel = m_roomModels[m_currentRoomIndex - 1];
			m_currentRoomModel.transform.position = previousModel.EndConnectorTransform.position - m_currentRoomModel.StartConnectorTransform.position;
		}
		else
		{
			m_currentRoomModel.transform.position = m_introRoom.EndConnectorTransform.position - m_currentRoomModel.StartConnectorTransform.position;
		}

		m_currentRoomModel.PlayDropAnimForRoom();

		m_roomCounterText.text = (m_currentRoomIndex + 1).ToString() + "/" + GameManager.Instance.KingdomSettings.RoomNumber;

		m_currentRoomData = new KingdomRoomData();

		WaitTime(GameManager.Instance.KingdomSettings.RoomIntroTime, () =>
		{
			m_currentRoomModel.SpawnDetailObjects(true);
			WaitTime(GameManager.Instance.KingdomSettings.RoomSpawnObjectTime, () =>
			{
				UIUtils.SetActive(m_dangerObjectPanel, true);
				SwitchState(CreationState.ChoosingCore);
			});
		});
	}

	private void PickCoreChoices()
	{
		m_currentRoomModel.DestroyCoreModel();
		DisplayTitle(GameManager.Instance.KingdomSettings.ChooseCoreTitle);
		UIUtils.SetActive(m_titleText, true);


		DisplayChoices(GameManager.Instance.KingdomSettings.RoomCoreTIDs, true);
	}

	private void SpawnRoomCore(int a_coreTID)
	{
		m_currentRoomData.CoreTID = a_coreTID;
		m_chosenRoomChoiceTID = a_coreTID;

		m_currentRoomModel.SpawnRoomCore(a_coreTID);
		m_currentRoomModel.PlayDropAnimForCore();
	}

	private void PickMonsterChoices()
	{
		DestroyMonsters();
		bool lastRoom = m_currentRoomIndex == GameManager.Instance.KingdomSettings.RoomNumber - 1;
		string display = lastRoom ? GameManager.Instance.KingdomSettings.ChooseBossTitle : GameManager.Instance.KingdomSettings.ChooseMonstersTitle;
		DisplayTitle(display);

		var list = lastRoom ? GameManager.Instance.KingdomSettings.BossMonsterGroupTIDs : GameManager.Instance.KingdomSettings.MonsterGroupTIDs;
		DisplayChoices(list);
	}

	private void SpawnMonsters(int a_monsterGroupTID)
	{
		m_currentRoomData.MonsterGroupTID = a_monsterGroupTID;
		m_chosenRoomChoiceTID = a_monsterGroupTID;

		//remove old monsters
		DestroyMonsters();

		m_currentRoomModel.PlayDropAnimForMonsters();

		var monsterGroupTemplate = AssetCacher.Instance.CacheAsset<MonsterGroupTemplate>(a_monsterGroupTID);
		for (int i = 0; i < monsterGroupTemplate.MonsterTIDs.Count; i++)
		{
			var monsterTID = monsterGroupTemplate.MonsterTIDs[i];
			var positionTransform = m_currentRoomModel.GetMonsterPosition(i);
			var monsterTemplate = AssetCacher.Instance.CacheAsset<MonsterTemplate>(monsterTID);
			var unitModel = AssetCacher.Instance.InstantiateComponent<UnitModel>(monsterTemplate.UnitModel, positionTransform);
			if (unitModel != null)
			{
				unitModel.Init(null);
				unitModel.LookAt(m_currentRoomModel.PlayerSpawn.position);
				m_displayedUnitModels.Add(unitModel);
			}
		}
	}

	private void DestroyMonsters()
	{
		foreach (var monster in m_displayedUnitModels)
		{
			monster.Cleanup();
			GameObject.Destroy(monster.gameObject);
		}
		m_displayedUnitModels.Clear();
	}

	private void ConfirmMonsters()
	{
		foreach (var monster in m_displayedUnitModels)
		{
			m_unitModels.Add(monster);
		}
		m_displayedUnitModels.Clear();
	}

	private void PickSpellChoices()
	{
		DisplayTitle(GameManager.Instance.KingdomSettings.ChooseSpellTitle);

		DisplayChoices(GameManager.Instance.KingdomSettings.RoomSpellTIDs);
	}

	private void SpawnSecretSpell(int a_spellTID)
	{
		m_currentRoomData.SpellTID = a_spellTID;
		m_chosenRoomChoiceTID = a_spellTID;

		if (m_soundSpellCreate != null)
		{
			m_soundSpellCreate.Play();
		}

		var fx = FXObject.Create(GameManager.Instance.KingdomSettings.SpellFXTID, GameManager.Instance.FXParent);
		if (fx != null)
			fx.transform.position = m_currentRoomModel.transform.position;
	}

	private void CompleteRoom()
	{
		m_kingdomData.RoomDataList.Add(m_currentRoomData);
		m_currentRoomIndex++;
		if (m_currentRoomIndex >= GameManager.Instance.KingdomSettings.RoomNumber)
		{
			SwitchState(CreationState.CompletingKingdom);
		}
		else
		{
			SwitchState(CreationState.CreateRoom);
		}
	}

	private void CompleteKingdom()
	{
		UIUtils.SetActive(m_confirmButton, false);
		var roomPopup = UIManager.Instance.OpenUI<RoomNamePopup>(GameManager.Instance.UISettings.RoomNamePopupUITID);
		roomPopup.Init(OnKingdomNameComplete);
	}

	private void OnKingdomNameComplete(string a_name)
	{
		m_kingdomData.SetTextDetails(SaveManager.GetPlayerName(), a_name);

		SaveManager.SavePublishedLevel(m_kingdomData);
		UploadLevel();
	}

	private void UploadLevel()
	{
		EnableInteraction(false);
		KingdomNetworkingManager.Instance.PublishLevel(m_kingdomData, OnPublishedLevel);
	}

	private void OnPublishedLevel(bool a_error)
	{
		EnableInteraction(true);
		if (a_error)
		{
			GenericPopupUI.CreatePopup("There was an error uploading your level. Try again?", UploadLevel, OnUploadCancel);
		}
		else
		{
			SaveManager.AddGold(-GameManager.Instance.KingdomSettings.CreationGoldCost);
			GenericPopupUI.CreatePopup("Successfully uploaded! Would you like to playtest your level?", OnPlaytestLevel, OnNoPlaytest);
		}
	}

	private void OnNoPlaytest()
	{
		CloseUI();
		UIManager.Instance.OpenUI<TitleScreenUI>(GameManager.Instance.UISettings.TitleScreenTID);
	}

	private void OnPlaytestLevel()
	{
		CloseUI();

		var ui = UIManager.Instance.OpenUI<LevelSelectUI>(GameManager.Instance.UISettings.LevelSelectUITID);
		ui.InitAsPlaytest(m_kingdomData);
	}

	private void OnUploadCancel()
	{
		GenericPopupUI.CreatePopup("Would you like to playtest your level?", OnPlaytestLevel, OnNoPlaytest);
	}

	private void DisplayTitle(string a_text)
	{
		UIUtils.SetActive(m_titleText, true);
		m_titleText.text = a_text;
	}

	private void ActivateConfirmButton(bool a_activate)
	{
		m_confirmButton.SetInteractive(a_activate);
	}

	private void DisplayChoices(List<int> a_choiceTIDs, bool a_roomCores = false)
	{
		StartCoroutine(DisplayChoicesCR(a_choiceTIDs, a_roomCores));
	}

	private IEnumerator DisplayChoicesCR(List<int> a_choiceTIDs, bool a_roomCores)
	{
		EnableInteraction(false);
		RemoveChoices();

		var waitTime = new WaitForSeconds(GameManager.Instance.KingdomSettings.ChoiceDisplayDelay);
		var indexList = RandomHelpers.GetUniqueRandomIndexes(GameManager.Instance.KingdomSettings.RoomChoices, a_choiceTIDs.Count);
		for (int i = 0; i < GameManager.Instance.KingdomSettings.RoomChoices && i < indexList.Count; i++)
		{
			int choiceTID = a_choiceTIDs[indexList[i]];
			var choiceEntry = AssetCacher.Instance.InstantiateComponent<RoomChoiceEntry>(m_roomChoiceEntryTID, m_choiceParent);
			if (choiceEntry != null)
			{
				choiceEntry.Init(this, AssetCacher.Instance.CacheAsset<RoomChoiceTemplate>(choiceTID));
				if (a_roomCores)
					choiceEntry.SetGemIconSize();

				m_choiceEntries.Add(choiceEntry);

				yield return waitTime;
			}
		}
		EnableRerollButton(true);
		EnableInteraction(true);
		UIUtils.SetActive(m_confirmButton, true);
		m_rerollButton.SetInteractive(true);
	}

	private void RemoveChoices()
	{
		foreach (var entry in m_choiceEntries)
		{
			entry.Cleanup();
			GameObject.Destroy(entry.gameObject);
		}
		m_choiceEntries.Clear();
		m_selectedEntry = null;
	}

	public void ChoseChoice(RoomChoiceEntry a_entry)
	{
		if (m_selectedEntry == a_entry)
			return;

		if (m_selectedEntry != null)
		{
			m_selectedEntry.SetSelected(false);
		}
		m_selectedEntry = a_entry;
		m_selectedEntry.SetSelected(true);
		ActivateConfirmButton(true);
		switch (m_state)
		{
			case CreationState.ChoosingCore:
				SpawnRoomCore(m_selectedEntry.ChoiceTemplate.TID);
				break;

			case CreationState.ChoosingMonsters:
				SpawnMonsters(m_selectedEntry.ChoiceTemplate.TID);
				break;

			case CreationState.ChoosingSpell:
				SpawnSecretSpell(m_selectedEntry.ChoiceTemplate.TID);
				break;
		}
	}

	private void EnableRerollButton(bool a_enable)
	{
		UIUtils.SetActive(m_rerollButton, a_enable);
	}

	private void MoveCameraFromIntroToNextRoom()
	{
		var roomModelPrefab = AssetCacher.Instance.CacheAsset<RoomModel>(GameManager.Instance.KingdomSettings.BasicRoomModel);
		m_cameraController.MoveTo(m_introRoom.EndConnectorTransform.position + roomModelPrefab.transform.position - roomModelPrefab.StartConnectorTransform.position, () =>
		{
			SwitchState(CreationState.CreateRoom);
		});
	}

	private void MoveCameraToNextRoom()
	{
		m_cameraController.MoveTo(m_currentRoomModel.transform.position + m_currentRoomModel.EndConnectorTransform.position - m_currentRoomModel.StartConnectorTransform.position, ReadyForNextRoom);
	}

	private void ReadyForNextRoom()
	{
		SwitchState(CreationState.CompletingRoom);
	}

	private void WaitTime(float a_time, Action a_finishedAction)
	{
		GameManager.Instance.StartCoroutine(DelayTimeCR(a_time, a_finishedAction));
	}

	private IEnumerator DelayTimeCR(float a_time, Action a_finishedAction)
	{
		float timer = 0f;
		while (timer <= a_time)
		{
			yield return null;
			timer += Time.deltaTime;
		}

		a_finishedAction?.Invoke();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirmClicked()
	{
		ActivateConfirmButton(false);
		RemoveChoices();
		UIUtils.SetActive(m_titleText, false);
		UIUtils.SetActive(m_confirmButton, false);
		m_rerollButton.SetInteractive(false);

		var roomchoiceTemplate = AssetCacher.Instance.CacheAsset<RoomChoiceTemplate>(m_chosenRoomChoiceTID);
		m_dangerDisplay.InitList(roomchoiceTemplate.DangerTIDs);
		m_chosenRoomChoiceTID = tid.NULL;

		switch (m_state)
		{
			case CreationState.ChoosingCore:
				SwitchState(CreationState.ChoosingMonsters);
				break;

			case CreationState.ChoosingMonsters:
				ConfirmMonsters();
				SwitchState(CreationState.ChoosingSpell);
				break;

			case CreationState.ChoosingSpell:
				EnableRerollButton(false);
				if (m_currentRoomIndex == GameManager.Instance.KingdomSettings.RoomNumber - 1)
				{
					ReadyForNextRoom();
				}
				else
				{
					MoveCameraToNextRoom();
				}
				break;
		}

	}

	public void OnRerollButton()
	{
		m_rerolls--;
		m_rerollButton.SetInteractive(m_rerolls > 0);
		m_rerollText.text = m_rerolls.ToString();
		ActivateConfirmButton(false);
		InitState();
	}

	public void OnTouchDown(Vector2 a_touchPoint)
	{
		var ray = m_cameraController.Camera.ScreenPointToRay(a_touchPoint);
		int hits = Physics.RaycastNonAlloc(ray, m_raycastHits, 50);
		for (int i = 0; i < hits; i++)
		{
			var hitData = m_raycastHits[i];
			if (hitData.collider.CompareTag(CombatUI.c_DetailTag))
			{
				var detail = hitData.transform.GetComponent<DetailObjectHelper>();
				detail.OnClicked();
				break;
			}
		}
	}

	#endregion Callbacks

}