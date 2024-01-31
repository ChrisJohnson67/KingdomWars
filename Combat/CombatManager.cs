using Platform.UIManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public partial class CombatManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions
	public enum State
	{
		Setup,
		BattlePhase,
		EndCombat,
		Completed
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private CombatContextData m_contextData;
	private State m_state = State.Setup;
	private int m_subState;
	private int m_roomIndex;
	private UnitInstance m_targetedUnit;
	private UnitInstance m_heroUnit;
	private List<UnitInstance> m_unitList = new List<UnitInstance>();
	private List<RoomInstance> m_rooms = new List<RoomInstance>();
	private RoomInstance m_currentRoom;
	private BaseRoomModel m_introRoom;
	private CameraController m_cameraController;
	private bool m_heroWon = false;

	public static Action<State> OnCombatStateChange;
	public static Action<State, int> OnCombatSubStateChange;
	public static Action OnEndCombat;
	public static Action<UnitInstance> OnBeginUnitTurn;
	public static Action<UnitInstance> OnUnitDamaged;
	public static Action<UnitInstance> OnUnitTargetSwitched;
	public static Action OnRoomEnded;
	public static Action OnRoomStart;
	public static Action<bool> OnRoomPause;
	public static Action OnSetupComplete;


	private static CombatManager sm_instance;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	public Camera Camera { get { return m_cameraController.Camera; } }
	public bool InCombat { get { return m_state != State.Completed; } }
	public CombatContextData ContextData { get { return m_contextData; } }
	public bool InRoomCombatState { get { return m_state == State.BattlePhase && m_subState == (int)CombatManager.BattleState.BeginCombat; } }
	public bool IsLastRoom { get { return m_roomIndex == GameManager.Instance.KingdomSettings.RoomNumber - 1; } }
	public RoomInstance CurrentRoom { get { return m_currentRoom; } }
	public KingdomRoomData GetCurrentKingdomRoomData { get { return m_contextData.KingdomData.RoomDataList[m_roomIndex]; } }
	public KingdomRoomData GetNextKingdomRoomData { get { return m_contextData.KingdomData.RoomDataList[m_roomIndex + 1]; } }
	public UnitInstance HeroUnit { get { return m_heroUnit; } }

	public static CombatManager Instance { get { return sm_instance; } }
	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void InitCombat(CombatContextData a_initData)
	{
		sm_instance = this;
		m_contextData = a_initData;
		UIManager.Instance.OpenUI<CombatUI>(GameManager.Instance.UISettings.CombatUITID);
		ChangeState(State.Setup);
	}

	private void ChangeState(State a_state)
	{
		m_state = a_state;
		m_subState = 0;
		InitSubState();
		OnCombatStateChange?.Invoke(a_state);
	}

	private void ChangeSubState(int a_subState)
	{
		var state = m_state;
		m_subState = a_subState;
		InitSubState();
		OnCombatSubStateChange?.Invoke(state, a_subState);
	}

	private void InitSubState()
	{
		switch (m_state)
		{
			case State.Setup:
				DoInitSetupState(m_subState);
				break;

			case State.BattlePhase:
				DoInitBattleState(m_subState);
				break;

			case State.EndCombat:
				EndCombat();
				break;

			case State.Completed:
				GameManager.Instance.EndCombat();
				break;
		}
	}

	public void UpdateCombat(float a_deltaTime)
	{
		if (!InCombat)
			return;

		switch (m_state)
		{
			case State.Setup:
				break;

			case State.BattlePhase:
				UpdateTargetedUnit();
				UpdateBattleState(a_deltaTime);
				break;
		}
	}

	private void UpdateTargetedUnit()
	{
		if ((m_targetedUnit == null || m_targetedUnit.IsDead) && m_currentRoom != null)
		{
			foreach (var unit in m_unitList)
			{
				if (unit != m_heroUnit && !unit.IsDead)
				{
					SetTargetedUnit(unit);
					return;
				}
			}
			SetTargetedUnit(null);
		}
	}

	public void SetTargetedUnit(UnitInstance a_unit)
	{
		if (m_targetedUnit == a_unit)
			return;

		if (a_unit == null || !a_unit.IsDead)
		{
			m_targetedUnit = a_unit;
			m_heroUnit.SelectedTarget(m_targetedUnit);
			OnUnitTargetSwitched?.Invoke(m_targetedUnit);
		}
	}

	public void ApplyDamageToUnit(UnitInstance a_source, UnitInstance a_target, DamageAppliedInfo a_damageInfo)
	{
		int totalDamage = a_target.ApplyDamage(a_source, a_damageInfo);
		OnUnitDamaged?.Invoke(a_target);
	}

	public void ApplyHealToUnit(UnitInstance a_source, UnitInstance a_target, float a_healAmount)
	{
		a_target.ApplyHeal(a_source, (int)a_healAmount);
	}

	private void DelayTime(float a_time, Action a_finishedAction)
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

	public List<UnitInstance> GetAllUnits(bool a_allowDead)
	{
		var listToFill = new List<UnitInstance>();
		foreach (var unit in m_unitList)
		{
			if (a_allowDead || (!a_allowDead && !unit.IsDead))
			{
				listToFill.Add(unit);
			}
		}
		return listToFill;
	}

	public void AddUnit(UnitInstance a_unit)
	{
		if (!m_unitList.Contains(a_unit))
			m_unitList.Add(a_unit);
	}

	public void RemoveUnit(UnitInstance a_unit)
	{
		if (m_unitList.Contains(a_unit))
			m_unitList.Remove(a_unit);
	}

	public UnitInstance GetHeroTargetedUnit()
	{
		return m_targetedUnit;
	}

	public List<UnitInstance> GetEnemies()
	{
		return m_unitList.FindAll(x => !x.IsDead && x != m_heroUnit);
	}

	public List<UnitInstance> GetHeroes()
	{
		return new List<UnitInstance>() { m_heroUnit };
	}

	public List<UnitInstance> GetAlliesFromUnit(UnitInstance a_unit)
	{
		return m_unitList.FindAll(x => !x.IsDead && x.AlignmentTag == a_unit.AlignmentTag);
	}

	public List<UnitInstance> GetEnemiesFromUnit(UnitInstance a_unit)
	{
		return m_unitList.FindAll(x => !x.IsDead && x.AlignmentTag != a_unit.AlignmentTag);
	}

	private void EndCombat()
	{
		m_cameraController.ZoomOut(OnZoomComplete);
	}

	private void OnZoomComplete()
	{
		DelayTime(GameManager.Instance.KingdomSettings.CombatEndTime, () =>
		{
			AwardPlayer();
		});
	}

	private void AwardPlayer()
	{
		if (!m_contextData.Playtesting)
		{
			KingdomNetworkingManager.Instance.ReportCombat(m_contextData.KingdomData.LevelID, m_heroWon, m_contextData.HasBonus, null);

			var ui = UIManager.Instance.OpenUI<VictoryUI>(GameManager.Instance.UISettings.VicotryUITID);
			ui.Init(m_heroWon, OnResultUIComplete);
		}
		else
		{
			OnResultUIComplete(false);
		}
	}

	private void OnResultUIComplete(bool a_retry)
	{
		Cleanup();
		UIManager.Instance.CloseUI(GameManager.Instance.UISettings.CombatUITID);

		if (!a_retry)
		{
			UIManager.Instance.OpenUI<TitleScreenUI>(GameManager.Instance.UISettings.TitleScreenTID);
		}
		else
		{
			var ui = UIManager.Instance.OpenUI<LevelSelectUI>(GameManager.Instance.UISettings.LevelSelectUITID);
			ui.InitAsretry(m_contextData.KingdomData);
		}
	}

	public void CSRWin()
	{
		m_heroWon = true;
		ChangeState(State.EndCombat);
	}

	public void Quit()
	{
		m_heroWon = false;
		ChangeState(State.EndCombat);
	}


	public void Cleanup()
	{
		if (!InCombat)
			return;

		OnEndCombat?.Invoke();

		m_heroUnit.CleanupAndDestroy();

		foreach (var room in m_rooms)
		{
			room.Cleanup();
		}

		if (m_introRoom != null)
		{
			m_introRoom.Cleanup();
			GameObject.Destroy(m_introRoom.gameObject);
		}

		GameObject.Destroy(m_cameraController.gameObject);
		ChangeState(State.Completed);

		sm_instance = null;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
