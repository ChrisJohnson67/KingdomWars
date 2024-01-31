//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using UnityEngine;

public partial class CombatManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum SetupState
	{
		SpawnKingdom,
		HeroWalkToRoom,
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables




	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void DoInitSetupState(int a_setupState)
	{
		switch ((SetupState)a_setupState)
		{
			case SetupState.SpawnKingdom:
				SpawnKingdom();
				break;

			case SetupState.HeroWalkToRoom:
				HeroWalkThroughIntroRoom();
				break;

		}
	}

	private void SpawnKingdom()
	{
		//spawn intro room
		m_introRoom = AssetCacher.Instance.InstantiateComponent<BaseRoomModel>(GameManager.Instance.KingdomSettings.IntroRoomModel);

		m_heroUnit = new UnitInstance(m_contextData.HeroTemplate, GameManager.Instance.CombatSettings.AllyPartyTID);
		m_heroUnit.SpawnModel(m_introRoom.PlayerSpawn.position, m_introRoom.PlayerSpawn.localRotation.eulerAngles);

		AddUnit(m_heroUnit);

		m_cameraController = AssetCacher.Instance.InstantiateComponent<CameraController>(GameManager.Instance.KingdomSettings.CameracontrollerTID);
		m_cameraController.SetFollowTransform(m_heroUnit.Model.transform);

		int roomIndex = 0;
		RoomModel previousRoomModel = null;
		foreach (var roomData in m_contextData.KingdomData.RoomDataList)
		{
			RoomInstance roomInstance = new RoomInstance(roomData);
			var position = Vector3.zero;
			if (previousRoomModel != null)
			{
				position = previousRoomModel.EndConnectorTransform.position;
			}
			else
			{
				position = m_introRoom.EndConnectorTransform.position;
			}
			previousRoomModel = roomInstance.SpawnModel(position, roomIndex);
			roomInstance.SetupForCombat();

			m_rooms.Add(roomInstance);

			roomIndex++;
		}

		OnSetupComplete?.Invoke();

		DelayTime(GameManager.Instance.KingdomSettings.KingdomIntroTime, OnIntroComplete);
	}

	private void OnIntroComplete()
	{
		ChangeSubState((int)SetupState.HeroWalkToRoom);
	}

	private void HeroWalkThroughIntroRoom()
	{
		m_introRoom.MovePlayerThroughWaypoints(m_heroUnit, () =>
		{
			ChangeState(State.BattlePhase);
		});
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
