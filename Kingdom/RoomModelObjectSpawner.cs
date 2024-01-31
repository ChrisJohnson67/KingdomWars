using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Name
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Description of Name behavior.
/// </summary>
public class RoomModelObjectSpawner : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum ObjectSpawnType
	{
		Small,
		Med,
		Large,
		Wall
	}

	[Serializable]
	public class ObjectSpawnInfo
	{
		[SerializeField]
		private ObjectSpawnType m_spawnType;

		[SerializeField]
		private float m_blankChance = 0f;

		[SerializeField]
		private List<Transform> m_parents;

		public ObjectSpawnType SpawnType { get { return m_spawnType; } }
		public List<Transform> Parents { get { return m_parents; } }
		public float BlankChance { get { return m_blankChance; } }
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private bool m_spawnOnAwake = false;

	[SerializeField]
	private bool m_spawnAnim = true;

	[SerializeField]
	private List<ObjectSpawnInfo> m_spawnInfoList = new List<ObjectSpawnInfo>();

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		if (m_spawnOnAwake)
		{
			SpawnObjects(m_spawnAnim);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void SpawnObjects(bool a_playSpawnAnim)
	{
		foreach (var spawnList in m_spawnInfoList)
		{
			var spawnData = GameManager.Instance.KingdomSettings.GetRoomObjects(spawnList.SpawnType, spawnList.Parents.Count);
			if (spawnData != null && spawnData.Count > 0)
			{
				for (int i = 0; i < spawnData.Count; i++)
				{
					var spawn = spawnData[i];

					var blankHit = RandomHelpers.GetRandomValue(0f, 1f);
					if (blankHit >= spawnList.BlankChance)
					{
						var obj = AssetCacher.Instance.InstantiateComponent<DetailObjectHelper>(spawn);
						if (obj != null)
						{
							obj.transform.SetParent(spawnList.Parents[i], false);
							obj.Spawn(m_spawnAnim && a_playSpawnAnim);
						}
					}
				}
			}
		}
	}


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}