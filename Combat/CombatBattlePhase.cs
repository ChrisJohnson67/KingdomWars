using System;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public partial class CombatManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum BattleState
	{
		Setup,
		WalkToStartOfRoom,
		RoomPauseWait,
		BeginCombat,
		EndCombat,
		WalkToEndOfRoom,
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private float m_roomPauseTimer;

	public static Action OnUnitUsedAbility;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public float RoomPauseTimer { get { return m_roomPauseTimer; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void DoInitBattleState(int a_setupState)
	{
		switch ((BattleState)a_setupState)
		{
			case BattleState.Setup:
				SetupNextRoom();
				break;

			case BattleState.WalkToStartOfRoom:
				HeroWalkToStartOfRoom();
				break;

			case BattleState.RoomPauseWait:
				RoomPauseWait();
				break;

			case BattleState.BeginCombat:
				BeginRoomCombat();
				break;

			case BattleState.EndCombat:
				CompleteRoomCombat();
				break;

			case BattleState.WalkToEndOfRoom:
				HeroWalkToEndOfRoom();
				break;
		}
	}

	private void UpdateBattleState(float a_deltaTime)
	{
		switch ((BattleState)m_subState)
		{
			case BattleState.BeginCombat:

				foreach (var unit in m_unitList)
				{
					unit.Update(a_deltaTime);
				}

				m_currentRoom.Update(a_deltaTime);

				bool combatEnded = HasRoomCombatEnded();
				if (combatEnded)
				{
					ChangeSubState((int)BattleState.EndCombat);
				}
				break;

			case BattleState.RoomPauseWait:

				m_roomPauseTimer += a_deltaTime;
				if (m_roomPauseTimer >= GameManager.Instance.KingdomSettings.RoomPauseTime)
				{
					OnRoomPause?.Invoke(false);
					ChangeSubState((int)BattleState.BeginCombat);
				}

				break;
		}
	}

	private void SetupNextRoom()
	{
		m_currentRoom = m_rooms[m_roomIndex];
		ChangeSubState((int)BattleState.WalkToStartOfRoom);
	}

	private void BeginRoomCombat()
	{
		m_currentRoom.StartRoomCombat();
		m_heroUnit.StartRoomCombat();
		OnRoomStart?.Invoke();
	}

	private bool HasRoomCombatEnded()
	{
		bool ended = false;
		if (m_currentRoom.AreAllEnemiesDead())
		{
			m_heroWon = true;
			ended = true;
		}
		else if (m_heroUnit.IsDead)
		{
			m_heroWon = false;
			ended = true;
		}
		return ended;
	}

	private void CompleteRoomCombat()
	{
		m_currentRoom.CompleteRoomCombat();
		m_heroUnit.CompleteRoomCombat();

		OnRoomEnded?.Invoke();

		if (!m_heroWon)
		{
			ChangeState(State.EndCombat);
		}
		else
		{
			DelayTime(GameManager.Instance.KingdomSettings.RoomOutroTime, OnRoomOutroComplete);
		}
	}

	private void OnRoomOutroComplete()
	{
		if (IsLastRoom)
		{
			ChangeState(State.EndCombat);
		}
		else
		{
			ChangeSubState((int)BattleState.WalkToEndOfRoom);
		}
	}

	private void HeroWalkToStartOfRoom()
	{
		m_heroUnit.Model.MoveToPosition(m_currentRoom.Model.PlayerSpawn.position, () =>
		{
			ChangeSubState((int)BattleState.RoomPauseWait);
		});
	}

	private void HeroWalkToEndOfRoom()
	{
		m_roomIndex++;
		m_currentRoom.Model.MovePlayerThroughWaypoints(m_heroUnit, () =>
		{
			ChangeSubState((int)BattleState.Setup);
		});
	}

	private void RoomPauseWait()
	{
		m_roomPauseTimer = 0f;
		OnRoomPause?.Invoke(true);
	}

	public void SkipRoomPause()
	{
		OnRoomPause?.Invoke(false);
		ChangeSubState((int)BattleState.BeginCombat);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
