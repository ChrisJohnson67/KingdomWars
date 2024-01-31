using System;
using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AbilityInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum State
	{
		Initialized,
		CastingAbility,
		Processing,
		Completed
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private AbilityTemplate m_template;
	protected List<AbilityEffectInstance> m_effects = new List<AbilityEffectInstance>();
	protected List<AbilityEffectTemplate> m_extraEffects = new List<AbilityEffectTemplate>();
	protected int m_effectIndex;
	protected State m_state;
	protected float m_castTime = 0f;
	protected AbilityContextData m_contextData;

	public Action OnCastingComplete;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public AbilityTemplate Template { get { return m_template; } }
	public bool IsCasting { get { return m_state == State.CastingAbility; } }
	public bool CastingComplete { get { return m_state == State.Processing || IsCompleted; } }
	public bool IsCompleted { get { return m_state == State.Completed; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AbilityInstance(AbilityTemplate a_template, AbilityContextData a_contextData, float a_adjustedCastTime, List<AbilityEffectTemplate> a_extraEffects)
	{
		a_contextData.AbilityInstance = this;
		m_contextData = a_contextData;
		m_template = a_template;
		m_castTime = a_adjustedCastTime;
		m_extraEffects = a_extraEffects;
		m_contextData.TargetSelection = m_template.SelectionTarget;
		m_state = State.Initialized;
	}

	public void StartAbility()
	{
		if (m_state == State.Initialized)
		{
			foreach (var effect in m_template.Effects)
			{
				if (effect.MeetsStartRequirements(m_contextData))
				{
					var instance = effect.CreateInstance(m_contextData);
					m_effects.Add(instance);
				}
			}
			if (m_extraEffects != null)
			{
				foreach (var effect in m_extraEffects)
				{
					if (effect.MeetsStartRequirements(m_contextData))
					{
						var instance = effect.CreateInstance(m_contextData);
						m_effects.Add(instance);
					}
				}
			}
			m_state = State.CastingAbility;
			CheckToStartAbility();
		}
	}

	public void Cleanup()
	{
		foreach (var effect in m_effects)
		{
			effect.Cleanup();
		}
	}

	private void ApplyNextEffect()
	{
		if (m_effectIndex >= m_effects.Count)
		{
			m_state = State.Completed;
			return;
		}
		var effect = m_effects[m_effectIndex];
		if (effect != null)
		{
			effect.StartEffect();
			m_state = State.Processing;
			CheckToStartNextEffect();
		}
	}

	public void Update(float a_deltaTime)
	{
		if (m_state == State.CastingAbility)
		{
			m_castTime += a_deltaTime;
			CheckToStartAbility();
		}

		if (m_state == State.Processing)
		{
			foreach (var effect in m_effects)
			{
				if (!effect.Completed)
					effect.Update(a_deltaTime);
			}

			CheckToStartNextEffect();
		}
	}

	private void CheckToStartAbility()
	{
		if (m_castTime >= m_template.CastTime)
		{
			OnCastingComplete?.Invoke();
			ApplyNextEffect();
		}
	}

	private void CheckToStartNextEffect()
	{
		var currentEffect = GetCurrentEffect();
		if (currentEffect != null && currentEffect.Completed)
		{
			m_effectIndex++;
			ApplyNextEffect();
		}
	}

	public void AddEffect(AbilityEffectInstance a_instance)
	{
		m_effects.Add(a_instance);
		if (m_state == State.Completed)
			m_state = State.Processing;
	}

	public void RemoveEffect(AbilityEffectTemplate a_template)
	{
		var effect = m_extraEffects.Find(x => x.TID == a_template.TID);
		if (effect != null)
			m_extraEffects.Remove(effect);
	}

	private AbilityEffectInstance GetCurrentEffect()
	{
		if (m_effectIndex < m_effects.Count)
		{
			return m_effects[m_effectIndex];
		}
		return null;
	}


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
