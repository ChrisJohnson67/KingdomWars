using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomModel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomModel : BaseRoomModel
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	private const string c_DropAnim = "tDrop";
	private const string c_RoomDropAnim = "tRoomDrop";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Animator m_roomAnimator;

	[SerializeField]
	private Animator m_coreParent;

	[SerializeField]
	private Animator m_monsterAnimator;

	[SerializeField]
	private List<Transform> m_monsterSpawnPositions = new List<Transform>();

	[SerializeField]
	private RoomModelObjectSpawner m_objectSpawner;

	[SerializeField, TemplateIDField(typeof(FXObject), "Spawn FX", "")]
	private int m_spawnFXTID;

	//--- NonSerialized ---
	protected RoomCoreModel m_coreModel;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init()
	{
	}

	public void SpawnDetailObjects(bool a_playSpawnAnim)
	{
		if (m_objectSpawner != null)
		{
			m_objectSpawner.SpawnObjects(a_playSpawnAnim);
		}
	}

	public void SpawnRoomCore(int a_coreTID)
	{
		DestroyCoreModel();
		var roomCoreTemplate = AssetCacher.Instance.CacheAsset<RoomCoreTemplate>(a_coreTID);
		m_coreModel = AssetCacher.Instance.InstantiateComponent<RoomCoreModel>(roomCoreTemplate.CoreModel, m_coreParent.transform);
	}

	public void DestroyCoreModel()
	{
		if (m_coreModel != null)
		{
			m_coreModel.Cleanup();
			Destroy(m_coreModel.gameObject);
		}
	}

	public void CompleteRoomCombat()
	{

	}

	public Transform GetMonsterPosition(int a_index)
	{
		if (a_index >= 0 && a_index < m_monsterSpawnPositions.Count)
		{
			return m_monsterSpawnPositions[a_index];
		}
		return transform;
	}

	public void PlayDropAnimForRoom()
	{
		UIUtils.SetTrigger(m_roomAnimator, c_RoomDropAnim);
	}

	public void PlayDropAnimForCore()
	{
		UIUtils.SetTrigger(m_coreParent, c_DropAnim);
	}

	public void PlayDropAnimForMonsters()
	{
		UIUtils.SetTrigger(m_monsterAnimator, c_DropAnim);
	}

	public override void Cleanup()
	{
		base.Cleanup();

		DestroyCoreModel();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnSpawnFX_Animation()
	{
		FXObject.Create(m_spawnFXTID, transform);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (m_objectSpawner == null)
		{
			transform.GetComponent<RoomModelObjectSpawner>();
		}
	}
#endif
}