//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffDamageOverTimeInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffDamageOverTimeInstance : BuffDamageInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private BuffDamageOverTimeTemplate m_timeTemplate;
	private int m_ticksRemaining = 0;
	private float m_timer = 0f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public BuffDamageOverTimeInstance(BuffDamageOverTimeTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
		m_timeTemplate = a_template;
		m_ticksRemaining =  (int)(a_data.Duration / m_timeTemplate.SecondsEveryTrigger);
	}

	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);

		m_timer += a_deltaTime;
		if (m_timer >= m_timeTemplate.SecondsEveryTrigger)
		{
			m_timer -= m_timeTemplate.SecondsEveryTrigger;
			m_ticksRemaining--;
			TriggerBuffEffects();
		}
	}

	public override void AddDuration(float a_duration)
	{
		base.AddDuration(a_duration);

		m_ticksRemaining = (int)(m_duration / m_timeTemplate.SecondsEveryTrigger);
	}

	protected override void DurationComplete()
	{
		m_canRemove = m_context.IsTimed && m_duration <= 0f && m_ticksRemaining <= 0;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
