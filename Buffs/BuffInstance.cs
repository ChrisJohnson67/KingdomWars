using System;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected BuffTemplate m_template;
	protected BuffContextData m_context;
	protected bool m_started = false;
	protected bool m_canRemove = false;
	protected int m_stacks;
	protected float m_duration = 0f;
	protected AddBuffEffectTemplate.RemoveTrigger m_removeTrigger;

	public Action OnBuffRemoved;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public BuffTemplate Template { get { return m_template; } }
	public bool ReadyToRemove { get { return m_canRemove; } }
	public int Stacks { get { return m_stacks; } }
	public float DurationRemaining { get { return m_duration; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public BuffInstance(BuffTemplate a_template, BuffContextData a_context)
	{
		m_template = a_template;
		m_context = a_context;
		m_duration = a_context.Duration;
		m_removeTrigger = a_context.RemoveTrigger;
	}

	public virtual void BuffAdded()
	{
		if ((m_template.Trigger == BuffTemplate.BuffTrigger.Immediate || m_template.Trigger == BuffTemplate.BuffTrigger.OnStackAmount) && MeetsReqs())
		{
			ApplyBuff();
			if (m_template.Trigger != BuffTemplate.BuffTrigger.OnStackAmount)
			{
				TriggerBuffEffects();
			}
		}
	}

	public void AddStacks(int a_stackAmount)
	{
		m_stacks += a_stackAmount;
		if (m_template.Trigger == BuffTemplate.BuffTrigger.OnStackAmount && m_stacks >= m_template.StackAmountTrigger)
		{
			TriggerBuffEffects();
			RemoveBuff();
		}
	}

	public virtual void AddDuration(float a_duration)
	{
		m_duration += a_duration;
	}

	public virtual void OnSourceTookDamage(DamageAppliedInfo a_damageInfo)
	{
		if (m_template.Trigger == BuffTemplate.BuffTrigger.OnDamage)
		{
			ApplyBuff();
			TriggerBuffEffects();
		}
	}

	protected virtual bool MeetsReqs()
	{
		return !m_started;
	}

	public virtual void OnSourceDied()
	{
		if (m_template.Trigger == BuffTemplate.BuffTrigger.OnDeath && MeetsReqs())
		{
			ApplyBuff();
			TriggerBuffEffects();
		}
	}

	public virtual void OnSourceHealed(int a_healAmount)
	{
		if (m_template.Trigger == BuffTemplate.BuffTrigger.OnHeal && MeetsReqs())
		{
			ApplyBuff();
			TriggerBuffEffects();
		}
	}

	protected virtual void ApplyBuff()
	{
		m_started = true;
	}

	public void CompleteRoomCombat()
	{
		if (m_removeTrigger == AddBuffEffectTemplate.RemoveTrigger.RoomEnd)
		{
			m_canRemove = true;
		}
	}

	/// <summary>
	/// Occurs at the beginning of a unit's turn
	/// </summary>
	public virtual void Update(float a_deltaTime)
	{
		if (m_context.IsTimed && m_started)
		{
			m_duration -= a_deltaTime;
			if (ShouldTriggerBuffEffects(a_deltaTime))
			{
				TriggerBuffEffects();
			}
			if (m_duration <= 0f)
			{
				DurationComplete();
			}
		}
	}

	protected virtual void TriggerBuffEffects()
	{
		if (m_template.Trigger == BuffTemplate.BuffTrigger.OnStackAmount)
		{
			m_stacks = 0;
		}
	}

	protected virtual bool ShouldTriggerBuffEffects(float a_deltaTime)
	{
		return false;
	}

	protected virtual void DurationComplete()
	{
		m_canRemove = m_context.IsTimed && m_duration <= 0f;
	}


	public virtual void RemoveBuff()
	{
		m_canRemove = true;
		OnBuffRemoved?.Invoke();
		Cleanup();
	}

	public virtual void Cleanup()
	{
		m_started = false;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
