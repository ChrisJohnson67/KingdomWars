using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum State
	{
		Initialized,
		WaitingForDelay,
		Processing,
		Completed
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected AbilityEffectTemplate m_template;
	protected float m_timer = 0f;
	protected State m_state = State.Initialized;
	protected AbilityContextData m_context;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public AbilityEffectTemplate Template { get { return m_template; } }
	public bool Completed { get { return m_state == State.Completed; } }
	public bool Processing { get { return m_state == State.Processing; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AbilityEffectInstance(AbilityEffectTemplate a_template, AbilityContextData a_context)
	{
		m_template = a_template;
		m_state = State.Initialized;
		m_context = a_context;
	}

	public virtual void StartEffect()
	{
		m_timer = 0f;
		m_state = State.WaitingForDelay;
		if (m_template.Delay == 0f)
		{
			m_state = State.Processing;
			ApplyEffect();
		}
	}

	public virtual void ApplyEffect()
	{
		if (m_template.FXObject != null)
		{
			switch (m_template.FXPos)
			{
				case AbilityEffectTemplate.FXPosition.Source:
					var fx = FXObject.Create(m_template.FXObject, GameManager.Instance.FXParent);
					fx.transform.position = m_context.Source.Model.AttackNode.position;
					fx.SetFollowTarget(m_context.Source.Model.transform);
					break;

				case AbilityEffectTemplate.FXPosition.Targets:
					foreach (var target in m_context.Targets)
					{
						fx = FXObject.Create(m_template.FXObject, GameManager.Instance.FXParent);
						fx.transform.position = target.Model.AttackNode.position;
					}

					break;

				case AbilityEffectTemplate.FXPosition.RoomCenter:
					fx = FXObject.Create(m_template.FXObject, GameManager.Instance.FXParent);
					fx.transform.position = CombatManager.Instance.CurrentRoom.Model.transform.position;

					break;
			}
		}

		if (m_template.Sound != null)
		{
			m_template.Sound.Play();
		}
	}

	protected void CompleteEffect()
	{
		m_state = State.Completed;
	}

	public virtual void Update(float a_deltaTime)
	{
		if (m_state == State.WaitingForDelay)
		{
			m_timer += a_deltaTime;
			if (m_timer >= m_template.Delay && m_template.ApplyEffectAfterDelay)
			{
				m_state = State.Processing;
				ApplyEffect();
			}
		}
	}

	public List<UnitInstance> GetTargets()
	{
		if (m_template.Target != m_context.TargetSelection)
		{
			return AbilityTemplate.GetTargets(m_context.Source, m_template.Target);
		}
		else
		{
			return m_context.Targets;
		}
	}


	public virtual void Cleanup()
	{

	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
