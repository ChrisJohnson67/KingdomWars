using System;
using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class UnitInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected UnitTemplate m_unitTemplate;
	protected UnitStatInstance m_currentStats;
	protected UnitModel m_unitModel;
	protected bool m_finishedDeathAnimation;
	protected int m_alignmentTag;
	protected List<BuffInstance> m_buffs = new List<BuffInstance>();
	protected List<AbilityInstance> m_abilities = new List<AbilityInstance>();
	protected List<AbilityEffectTemplate> m_additionalAutoAttackEffects = new List<AbilityEffectTemplate>();
	protected int m_currentHealth;
	private UnitAI m_unitAI;
	protected bool m_inCombat;
	protected int m_abilityChargesRemaining;
	protected bool m_immuneToDebuffs = false;
	protected bool m_stunned = false;
	protected bool m_canHeal = true;
	protected bool m_ignoreDamage = false;
	protected List<AbilityEffectTemplate> m_extraEffectsWorking = new List<AbilityEffectTemplate>();

	public Action<UnitInstance, int, DamageAppliedInfo, UnitInstance> OnDamageTaken;
	public Action OnUsedAbility;
	public Action<UnitInstance, BuffInstance, bool> OnAddBuff;
	public Action<UnitInstance, int> OnHealApplied;
	public Action OnAbilityChargeChange;
	public Action<bool> OnStunned;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public UnitTemplate Template { get { return m_unitTemplate; } }
	public UnitModel Model { get { return m_unitModel; } }
	public bool IsDead { get { return m_currentHealth <= 0; } }
	public bool ReadyForRemoval { get { return IsDead && m_finishedDeathAnimation && m_abilities.Count == 0; } }
	public int CurrentHealth { get { return m_currentHealth; } }
	public bool HasAbilityCharges { get { return m_abilityChargesRemaining > 0; } }
	public int AbilityCharges { get { return m_abilityChargesRemaining; } }
	public bool IsStunned { get { return m_stunned; } }
	public UnitStatInstance CurrentStats { get { return m_currentStats; } }
	public int AlignmentTag { get { return m_alignmentTag; } }
	public List<BuffInstance> CurrentBuffs { get { return m_buffs; } }
	public Vector3 Position { get { return m_unitModel != null ? m_unitModel.transform.position : Vector3.zero; } }
	public Vector3 Rotation { get { return m_unitModel != null ? m_unitModel.transform.localRotation.eulerAngles : Vector3.zero; } }
	public int MaxHealth { get { return (int)m_currentStats.GetCurrentAmountOfStat(StatSettings.MaxHealthStatTID); } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public UnitInstance(UnitTemplate a_template, int a_alignment)
	{
		m_unitTemplate = a_template;
		m_alignmentTag = a_alignment;
		m_currentStats = new UnitStatInstance(m_unitTemplate.UnitStatTemplate);
		m_currentHealth = (int)m_currentStats.GetCurrentAmountOfStat(StatSettings.MaxHealthStatTID);
		m_abilityChargesRemaining = m_unitTemplate.AbilityCharges;
		CreateAI();
	}

	protected virtual void CreateAI()
	{
		m_unitAI = new HeroAI(this);
	}

	public void Update(float a_deltaTime)
	{
		if (!m_inCombat)
			return;

		if (!IsDead)
		{
			for (int i = m_buffs.Count - 1; i >= 0; i--)
			{
				var buff = m_buffs[i];
				buff.Update(a_deltaTime);
			}
			CheckToRemoveBuffs();

			if (!m_stunned)
			{
				m_unitAI.Update(a_deltaTime);
			}
		}
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

	public void AddAbilityCharges(int a_num)
	{
		m_abilityChargesRemaining += a_num;
		OnAbilityChargeChange?.Invoke();
	}

	public void UsedAbility()
	{
		OnUsedAbility?.Invoke();
	}

	public void StartRoomCombat()
	{
		if (m_unitAI != null)
			m_unitAI.ActivateAI(true);

		m_inCombat = true;

	}

	public void CompleteRoomCombat()
	{
		if (m_unitAI != null)
			m_unitAI.ActivateAI(false);

		//remove buffs from room
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			var buff = m_buffs[i];
			buff.CompleteRoomCombat();
			if (buff.ReadyToRemove)
			{
				m_buffs.RemoveAt(i);
				buff.RemoveBuff();
			}
		}

		m_inCombat = false;
	}

	public UnitModel SpawnModel(Vector3 a_position, Vector3 a_rotation)
	{
		m_unitModel = AssetCacher.Instance.InstantiateComponent<UnitModel>(m_unitTemplate.UnitModel, GameManager.Instance.UnitParent);
		if (m_unitModel != null)
		{
			m_unitModel.Init(this);
			m_unitModel.SetPosition(a_position);
			m_unitModel.SetRotation(a_rotation);
		}
		return m_unitModel;
	}

	public int ApplyDamage(UnitInstance a_source, DamageAppliedInfo a_damageInfo)
	{
		int totalDamage = 0;
		//calulate new damage with defense
		foreach (var damageInfo in a_damageInfo.DamageTypes)
		{
			if (damageInfo.DamageType == GameManager.Instance.CombatSettings.PhysDamageTypeTID)
			{
				//armor is subtracted
				var defenseStat = GameManager.Instance.StatSettings.ArmorStat;
				totalDamage += damageInfo.DamageAmount - (int)(m_currentStats.GetCurrentAmountOfStat(defenseStat));
			}
			else
			{
				//resistance is multiplied
				var defenseStat = GameManager.Instance.StatSettings.GetDefenseStatForDamage(damageInfo.DamageType);
				totalDamage += (int)(damageInfo.DamageAmount * (1f - (m_currentStats.GetCurrentAmountOfStat(defenseStat) * 0.01)));
			}

			if (m_ignoreDamage || totalDamage < 0)
			{
				totalDamage = 0;
			}

			damageInfo.SetAmt(totalDamage);
			FloatingDamageUI.CreateDamage(damageInfo, this);
		}
		totalDamage = Math.Max(0, totalDamage);
		if (totalDamage > 0)
		{
			m_currentHealth -= totalDamage;
			OnDamageTaken?.Invoke(this, totalDamage, a_damageInfo, a_source);
			if (a_damageInfo.ApplyOnDamageTrigger)
			{
				TriggerDamageTakenBuffEffects(a_damageInfo);
			}
		}
		if (IsDead)
		{
			Die();
		}
		return totalDamage;
	}

	protected void Die()
	{
		TriggerDeathBuffEffects();
		if (IsDead)
		{
			GameManager.Instance.KingdomSettings.DeathSound.Play();
			m_unitModel.PlayDeathAnim();
		}
	}

	public void ApplyHeal(UnitInstance a_source, int a_heal)
	{
		if (IsDead || !m_canHeal)
			return;

		float maxHealth = m_currentStats.GetMaxHealth();
		if (a_heal > maxHealth - m_currentHealth)
		{
			a_heal = (int)maxHealth - m_currentHealth;
		}
		if (a_heal > 0)
		{
			m_currentHealth += a_heal;
			m_currentHealth = (int)Mathf.Min(m_currentHealth, maxHealth);
			OnHealApplied?.Invoke(this, a_heal);
			TriggerHealBuffEffects(a_heal);

			FloatingDamageUI.CreateHeal(a_heal, this);
		}
	}

	public void CompleteDeath()
	{
		m_finishedDeathAnimation = true;
	}

	public int CalculateAdditionalDamage(int a_damageTypeTID, float a_damage, UnitInstance a_target)
	{
		int additionalDamage = 0;
		foreach (var buff in m_buffs)
		{
			var damageBuff = buff as ModifyDamageBuffInstance;
			if (damageBuff != null)
			{
				additionalDamage += damageBuff.CalculateAdditionalDamage(a_damageTypeTID, a_damage, this, a_target);
			}
		}
		return additionalDamage;
	}

	public int CalculateAdditionalHeal(float a_heal, UnitInstance a_target)
	{
		return 0;
	}

	public void SelectedTarget(UnitInstance a_target)
	{
		if (a_target != null)
		{
			RotateTowards(a_target.Position);
		}
	}

	public void RotateTowards(Vector3 a_pos)
	{
		m_unitModel.LookAt(a_pos);
	}

	public void ExitCombat()
	{
		//remove buffs
		foreach (var buff in m_buffs)
		{
			buff.Cleanup();
		}
		m_buffs.Clear();

		foreach (var ability in m_abilities)
		{
			ability.Cleanup();
		}
		m_abilities.Clear();

		m_unitModel.ExitCombat();
	}

	#region Buff Functions

	public BuffInstance AddBuff(BuffContextData a_contextData, BuffTemplate a_buff, int a_stacks = 1)
	{
		if (m_immuneToDebuffs && a_buff.IsDebuff)
		{
			return null;
		}

		var buff = GetBuff(a_buff.TID);
		if (a_buff.CanStack && buff != null)
		{
			buff.AddStacks(a_stacks);
			OnAddBuff?.Invoke(this, buff, true);
		}
		else if (buff != null)
		{
			//add duration
			if (buff.Template.CanReapplyBuff)
			{
				buff.AddDuration(a_contextData.Duration);
				buff.AddStacks(a_stacks);
				OnAddBuff?.Invoke(this, buff, true);
			}
		}
		else
		{
			buff = a_buff.CreateBuffInstance(a_contextData);
			m_buffs.Add(buff);
			buff.BuffAdded();
			buff.AddStacks(a_stacks);

			OnAddBuff?.Invoke(this, buff, false);
		}
		return buff;
	}

	public void ForceRemoveBuff(int a_buffTID)
	{
		var buff = GetBuff(a_buffTID);
		if (buff != null)
		{
			buff.RemoveBuff();
			m_buffs.Remove(buff);
		}
	}

	private void CheckToRemoveBuffs()
	{
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			var buff = m_buffs[i];
			if (buff.ReadyToRemove)
			{
				m_buffs.RemoveAt(i);
				buff.RemoveBuff();
			}
		}
	}

	public bool ContainsBuff(int a_buffTID)
	{
		foreach (var buff in m_buffs)
		{
			if (buff.Template.TID == a_buffTID && !buff.ReadyToRemove)
			{
				return true;
			}
		}
		return false;
	}

	public BuffInstance GetBuff(int a_buffTID)
	{
		foreach (var buff in m_buffs)
		{
			if (buff.Template.TID == a_buffTID && !buff.ReadyToRemove)
			{
				return buff;
			}
		}
		return null;
	}

	public void TriggerDamageTakenBuffEffects(DamageAppliedInfo a_damage)
	{
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			var buff = m_buffs[i];
			buff.OnSourceTookDamage(a_damage);
		}
		CheckToRemoveBuffs();
	}


	public void TriggerDeathBuffEffects()
	{
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			var buff = m_buffs[i];
			buff.OnSourceDied();
		}
		CheckToRemoveBuffs();
	}

	public void TriggerHealBuffEffects(int a_healAmount)
	{
		for (int i = m_buffs.Count - 1; i >= 0; i--)
		{
			var buff = m_buffs[i];
			buff.OnSourceHealed(a_healAmount);
		}
		CheckToRemoveBuffs();
	}

	public void SetImmuneToDebuffs(bool a_immune)
	{
		m_immuneToDebuffs = a_immune;
	}

	public void SetStunned(bool a_stun)
	{
		m_stunned = a_stun;
		OnStunned?.Invoke(a_stun);
	}

	public void SetCanHeal(bool a_can)
	{
		m_canHeal = a_can;
	}

	public void SetImmuneToDmg(bool a_imm)
	{
		m_ignoreDamage = a_imm;
	}

	#endregion

	#region Ability Functions

	public bool CanCastAbility(bool a_isAutoAttack = false)
	{
		bool canCanst = (!a_isAutoAttack && m_abilityChargesRemaining > 0) || a_isAutoAttack;
		return canCanst && CombatManager.Instance != null && CombatManager.Instance.InRoomCombatState && m_inCombat && !IsDead;
	}

	public void AddAutoAttackEffects(List<AbilityEffectTemplate> a_extraEffects)
	{
		m_additionalAutoAttackEffects.AddRange(a_extraEffects);
	}

	public void RemoveAutoAttackEffects(AbilityEffectTemplate a_extraEffect)
	{
		var aaAbility = m_additionalAutoAttackEffects.Find(x => x.TID == a_extraEffect.TID);
		if (aaAbility != null)
			m_additionalAutoAttackEffects.Remove(a_extraEffect);
	}

	public AbilityInstance CreateAbility(int a_abilityTID, bool a_isAutoAttack, bool a_isFree)
	{
		if (a_abilityTID == tid.NULL)
			return null;

		var abilityTemplate = AssetCacher.Instance.CacheAsset<AbilityTemplate>(a_abilityTID);

		AbilityContextData context = new AbilityContextData();
		context.Source = this;
		context.Targets = AbilityTemplate.GetTargets(this, abilityTemplate.SelectionTarget);

		m_extraEffectsWorking.Clear();
		if (a_isAutoAttack)
		{
			m_extraEffectsWorking.AddRange(m_additionalAutoAttackEffects);
		}
		AbilityInstance ability = new AbilityInstance(abilityTemplate, context, 0f, m_extraEffectsWorking);
		m_abilities.Add(ability);
		ability.StartAbility();

		if (!a_isFree)
		{
			AddAbilityCharges(-1);
		}

		return ability;
	}

	#endregion Ability Functions

	public bool IsUnitOnSameTeam(UnitInstance a_other)
	{
		return IsUnitOnSameTeam(a_other.m_alignmentTag);
	}

	public bool IsUnitOnSameTeam(int a_teamTag)
	{
		return m_alignmentTag == a_teamTag;
	}

	public bool IsOwnedByPlayer()
	{
		return m_alignmentTag == GameManager.Instance.CombatSettings.AllyPartyTID;
	}

	public void CleanupAndDestroy()
	{
		if (m_unitModel != null)
		{
			m_unitModel.Cleanup();
			GameManager.Instance.DeleteObject(m_unitModel.gameObject);
		}

		foreach (var ability in m_abilities)
		{
			ability.Cleanup();
		}

		foreach (var buff in m_buffs)
		{
			buff.Cleanup();
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
