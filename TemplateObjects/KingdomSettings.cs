using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// KingdomSettings
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/KingdomSettings")]
public class KingdomSettings : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	[Serializable]
	public class ObjectSpawnContainer
	{
		[SerializeField]
		private RoomModelObjectSpawner.ObjectSpawnType m_spawnType;

		[SerializeField, TemplateIDField(typeof(GameObject), "Room Object", "")]
		private List<int> m_objectTIDs;

		public RoomModelObjectSpawner.ObjectSpawnType SpawnType { get { return m_spawnType; } }
		public List<int> ObjectTIDs { get { return m_objectTIDs; } }
	}

	[Serializable]
	public class KingdomDataPresetData
	{
		[SerializeField]
		private int m_levelId;

		[SerializeField]
		private string m_author;

		[SerializeField]
		private string m_title;

		[SerializeField]
		private int m_wins;

		[SerializeField]
		private int m_losses;

		[SerializeField]
		private List<KingdomRoomData> m_roomDataList = new List<KingdomRoomData>();

		public int LevelID { get { return m_levelId; } }
		public string Author { get { return m_author; } }
		public string Title { get { return m_title; } }
		public int Wins { get { return m_wins; } }
		public int Losses { get { return m_losses; } }
		public List<KingdomRoomData> RoomList { get { return m_roomDataList; } }
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private List<ObjectSpawnContainer> m_spawnContainers = new List<ObjectSpawnContainer>();

	[SerializeField]
	private int m_roomNumber = 5;

	[SerializeField]
	private int m_roomChoices = 3;

	[SerializeField]
	private int m_rerolls = 3;

	[SerializeField]
	private int m_goldCreationCost = 100;

	[SerializeField]
	private int m_itemSlots = 3;

	[SerializeField]
	private int m_kingdomWinPoints = 10;

	[SerializeField]
	private int m_kingdomLossPoints = 5;

	[SerializeField]
	private int m_heroWinPoints = 10;

	[SerializeField]
	private int m_bonusWinRateTrigger = 2;

	[SerializeField]
	private int m_heroLossPoints = 8;

	[SerializeField]
	private int m_bonusPoints = 5;

	[SerializeField, TemplateIDField(typeof(BaseRoomModel), "Intro Room Model", "")]
	private int m_introRoomModel;

	[SerializeField, TemplateIDField(typeof(RoomModel), "Creation Basic Room Model", "")]
	private int m_creationBasicRoomModel;

	[SerializeField, TemplateIDField(typeof(RoomModel), "Basic Room Model", "")]
	private int m_basicRoomModel;

	[SerializeField, TemplateIDField(typeof(RoomModel), "Castle Room Model", "")]
	private int m_castleRoomModel;

	[SerializeField, TemplateIDField(typeof(CameraController), "Camera Controller", "")]
	private int m_cameracontrollerTID;

	[SerializeField, TemplateIDField(typeof(RoomCoreTemplate), "Core Templates", "")]
	private List<int> m_roomCoreTIDs;

	[SerializeField, TemplateIDField(typeof(MonsterGroupTemplate), "MonsterGroup Templates", "")]
	private List<int> m_monsterGroupTIDs;

	[SerializeField, TemplateIDField(typeof(MonsterGroupTemplate), "Boss MonsterGroup Templates", "")]
	private List<int> m_bossMonsterGroupTIDs;

	[SerializeField, TemplateIDField(typeof(RoomSpellTemplate), "Secret Spell Templates", "")]
	private List<int> m_roomSpellTIDs;

	[SerializeField, TemplateIDField(typeof(UnitTemplate), "Hero Templates", "")]
	private List<int> m_heroTemplateTIDs;

	[SerializeField, TemplateIDField(typeof(AbilityTemplate), "Item Templates", "")]
	private List<int> m_itemTemplateTIDs;

	[SerializeField]
	private SpecificSoundGroup m_DeathSound;

	[SerializeField, TemplateIDField(typeof(FXObject), "Room spell FX", "")]
	private int m_spellFXTID;

	[SerializeField]
	private SpecificSoundGroup m_spellSound;

	[SerializeField]
	private SpecificSoundGroup m_AbilityUseSound;

	[SerializeField]
	private List<KingdomDataPresetData> m_presetKingdoms = new List<KingdomDataPresetData>();

	[Header("Text")]
	[SerializeField]
	private string m_chooseCoreTitle;

	[SerializeField]
	private string m_chooseMonstersTitle;

	[SerializeField]
	private string m_chooseBossTitle;


	[SerializeField]
	private string m_chooseSpellTitle;

	[Header("Timings")]

	[SerializeField]
	private float m_kingdomIntroTime = 1.5f;

	[SerializeField]
	private float m_roomIntroTime = 0.5f;

	[SerializeField]
	private float m_roomSpawnObjectTime = 0.5f;

	[SerializeField]
	private float m_choiceDisplayDelay = 0.33f;

	[SerializeField]
	private float m_roomOutroTime = 1f;

	[SerializeField]
	private float m_combatEndTime = 2f;

	[SerializeField]
	private float m_roomPauseTime = 10f;
	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int RoomNumber { get { return m_roomNumber; } }
	public int RoomChoices { get { return m_roomChoices; } }
	public int ItemSlots { get { return m_itemSlots; } }
	public int CastleRoomModel { get { return m_castleRoomModel; } }
	public int CreationBasicRoomModel { get { return m_creationBasicRoomModel; } }
	public int BasicRoomModel { get { return m_basicRoomModel; } }
	public int IntroRoomModel { get { return m_introRoomModel; } }
	public int CameracontrollerTID { get { return m_cameracontrollerTID; } }
	public int Rerolls { get { return m_rerolls; } }
	public int BonusPoints { get { return m_bonusPoints; } }
	public int CreationGoldCost { get { return m_goldCreationCost; } }
	public List<int> RoomCoreTIDs { get { return m_roomCoreTIDs; } }
	public List<int> MonsterGroupTIDs { get { return m_monsterGroupTIDs; } }
	public List<int> BossMonsterGroupTIDs { get { return m_bossMonsterGroupTIDs; } }
	public List<int> RoomSpellTIDs { get { return m_roomSpellTIDs; } }
	public List<int> HeroTemplateTIDs { get { return m_heroTemplateTIDs; } }
	public List<int> ItemTemplateTIDs { get { return m_itemTemplateTIDs; } }
	public SpecificSoundGroup DeathSound { get { return m_DeathSound; } }
	public int SpellFXTID { get { return m_spellFXTID; } }
	public SpecificSoundGroup SpellSound { get { return m_spellSound; } }
	public SpecificSoundGroup AbilityUseSound { get { return m_AbilityUseSound; } }


	public string ChooseCoreTitle { get { return m_chooseCoreTitle; } }
	public string ChooseMonstersTitle { get { return m_chooseMonstersTitle; } }
	public string ChooseBossTitle { get { return m_chooseBossTitle; } }
	public string ChooseSpellTitle { get { return m_chooseSpellTitle; } }

	public float KingdomIntroTime { get { return m_kingdomIntroTime; } }
	public float RoomIntroTime { get { return m_roomIntroTime; } }
	public float RoomSpawnObjectTime { get { return m_roomSpawnObjectTime; } }
	public float ChoiceDisplayDelay { get { return m_choiceDisplayDelay; } }
	public float RoomOutroTime { get { return m_roomOutroTime; } }
	public float CombatEndTime { get { return m_combatEndTime; } }
	public float RoomPauseTime { get { return m_roomPauseTime; } }

	public int KingdomWinPoints { get => m_kingdomWinPoints; set => m_kingdomWinPoints = value; }
	public int KingdomLossPoints { get => m_kingdomLossPoints; set => m_kingdomLossPoints = value; }
	public int HeroWinPoints { get => m_heroWinPoints; set => m_heroWinPoints = value; }
	public int BonusWinRateTrigger { get => m_bonusWinRateTrigger; set => m_bonusWinRateTrigger = value; }
	public int HeroLossPoints { get => m_heroLossPoints; set => m_heroLossPoints = value; }

	#endregion Accessors

	public List<int> GetRoomObjects(RoomModelObjectSpawner.ObjectSpawnType a_type, int a_amount)
	{
		var objects = new List<int>();
		var container = m_spawnContainers.Find(x => x.SpawnType == a_type);
		if (container != null && container.ObjectTIDs.Count > 0)
		{
			for (int i = 0; i < a_amount; i++)
			{
				var obj = container.ObjectTIDs[UnityEngine.Random.Range(0, container.ObjectTIDs.Count)];
				if (obj != tid.NULL)
					objects.Add(obj);
			}
		}
		return objects;
	}

	public List<KingdomData> GetPresetKingdomDataList()
	{
		List<KingdomData> kingdomList = new List<KingdomData>();
		foreach (var preset in m_presetKingdoms)
		{
			var kingdomData = new KingdomData();
			kingdomData.SetLevelId(preset.LevelID);
			kingdomData.SetTextDetails(preset.Author, preset.Title);
			kingdomData.Wins = preset.Wins;
			kingdomData.Losses = preset.Losses;
			foreach (var room in preset.RoomList)
			{
				kingdomData.RoomDataList.Add(room);
			}
			kingdomList.Add(kingdomData);
		}
		return kingdomList;
	}
}