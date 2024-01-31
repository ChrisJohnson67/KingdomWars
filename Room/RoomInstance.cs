using System;
using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum RoomState
	{
		Dormant,
		InCombat,
		Complete,
		Creation
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected KingdomRoomData m_roomData;
	protected RoomModel m_roomModel;
	protected RoomState m_roomState = RoomState.Dormant;
	protected List<AbilityInstance> m_abilities = new List<AbilityInstance>();
	protected List<UnitInstance> m_monsters = new List<UnitInstance>();

	public Action OnUsedAbility;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public RoomModel Model { get { return m_roomModel; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public RoomInstance() { }

	public RoomInstance(KingdomRoomData a_roomData)
	{
		m_roomData = a_roomData;
	}

	public void SetupForCombat()
	{
		SpawnRoomCore();
		SpawnMonsters();
		m_roomModel.SpawnDetailObjects(false);
	}

	public void SpawnRoomCore()
	{
		m_roomModel.SpawnRoomCore(m_roomData.CoreTID);
	}

	public void SpawnMonsters()
	{
		//create the monsters
		var monsterGroupTemplate = AssetCacher.Instance.CacheAsset<MonsterGroupTemplate>(m_roomData.MonsterGroupTID);
		if (monsterGroupTemplate != null)
		{
			for (int i = 0; i < monsterGroupTemplate.MonsterTIDs.Count; i++)
			{
				var monsterTID = monsterGroupTemplate.MonsterTIDs[i];
				var positionTransform = m_roomModel.GetMonsterPosition(i);
				var monsterTemplate = AssetCacher.Instance.CacheAsset<MonsterTemplate>(monsterTID);
				CreateMonster(monsterTemplate, positionTransform);
			}
		}
	}

	protected virtual void CreateMonster(MonsterTemplate a_monsterTemplate, Transform a_positionTransform)
	{
		var monsterInstance = new MonsterInstance(a_monsterTemplate, GameManager.Instance.CombatSettings.EnemyPartyTID);
		var monsterModel = monsterInstance.SpawnModel(a_positionTransform.position, a_positionTransform.rotation.eulerAngles);
		monsterModel.transform.LookAt(m_roomModel.PlayerSpawn);
		m_monsters.Add(monsterInstance);
	}

	public void StartRoomCombat()
	{
		m_roomState = RoomState.InCombat;

		foreach (var monster in m_monsters)
		{
			CombatManager.Instance.AddUnit(monster);
			monster.StartRoomCombat();
		}

		//create the room core ability
		var roomCoreTemplate = AssetCacher.Instance.CacheAsset<RoomCoreTemplate>(m_roomData.CoreTID);
		if (roomCoreTemplate != null && roomCoreTemplate.Ability != null)
		{
			CreateAbility(roomCoreTemplate.Ability.TID);
		}

		CastSecretSpell();
	}

	public bool AreAllEnemiesDead()
	{
		foreach (var monster in m_monsters)
		{
			if (!monster.IsDead)
				return false;
		}
		return true;
	}

	public void Update(float a_deltaTime)
	{
		if (m_roomState != RoomState.InCombat)
			return;

		for (int i = m_abilities.Count - 1; i >= 0; i--)
		{
			var ability = m_abilities[i];
			ability.Update(a_deltaTime);
			if (ability.IsCompleted)
			{
				m_abilities.RemoveAt(i);
			}
		}
	}

	private void CastSecretSpell()
	{
		var roomSpellTemplate = AssetCacher.Instance.CacheAsset<RoomSpellTemplate>(m_roomData.SpellTID);
		if (roomSpellTemplate != null)
		{
			var secretSpell = CreateAbility(roomSpellTemplate.Ability.TID);
			secretSpell.OnCastingComplete += PlaySpellFX;
		}
	}

	private void PlaySpellFX()
	{
		GameManager.Instance.KingdomSettings.SpellSound.Play();
		var fx = FXObject.Create(GameManager.Instance.KingdomSettings.SpellFXTID, GameManager.Instance.FXParent);
		if (fx != null)
			fx.transform.position = m_roomModel.transform.position;
	}

	public void UsedAbility()
	{
		OnUsedAbility?.Invoke();
	}

	public RoomModel SpawnModel(Vector3 a_previousModelConnectorPosition, int a_roomIndex)
	{
		int roomModelTID = a_roomIndex == GameManager.Instance.KingdomSettings.RoomNumber - 1 ? GameManager.Instance.KingdomSettings.CastleRoomModel : GameManager.Instance.KingdomSettings.BasicRoomModel;
		m_roomModel = AssetCacher.Instance.InstantiateComponent<RoomModel>(roomModelTID);
		if (m_roomModel != null)
		{
			m_roomModel.Init();
			m_roomModel.transform.position = a_previousModelConnectorPosition - m_roomModel.StartConnectorTransform.position;
		}
		return m_roomModel;
	}

	public void CompleteRoomCombat()
	{
		foreach (var ability in m_abilities)
		{
			ability.Cleanup();
		}
		m_abilities.Clear();

		m_roomModel.CompleteRoomCombat();

		foreach (var monster in m_monsters)
		{
			CombatManager.Instance.RemoveUnit(monster);
			monster.CompleteRoomCombat();
		}
	}

	#region Ability Functions

	public bool CanCastAbility()
	{
		return m_roomState == RoomState.InCombat;
	}

	public AbilityInstance CreateAbility(int a_abilityTID)
	{
		if (a_abilityTID == tid.NULL)
			return null;

		if (!CanCastAbility())
			return null;

		var abilityTemplate = AssetCacher.Instance.CacheAsset<AbilityTemplate>(a_abilityTID);

		AbilityContextData context = new AbilityContextData();
		context.Targets = AbilityTemplate.GetTargets(null, abilityTemplate.SelectionTarget);

		AbilityInstance ability = new AbilityInstance(abilityTemplate, context, 0f, null);
		m_abilities.Add(ability);
		ability.StartAbility();

		return ability;
	}

	#endregion Ability Functions

	public void Cleanup()
	{
		if (m_roomModel != null)
		{
			m_roomModel.Cleanup();
			GameManager.Instance.DeleteObject(m_roomModel.gameObject);
		}

		foreach (var ability in m_abilities)
		{
			ability.Cleanup();
		}
		m_abilities.Clear();

		foreach (var unit in m_monsters)
		{
			unit.CleanupAndDestroy();
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
