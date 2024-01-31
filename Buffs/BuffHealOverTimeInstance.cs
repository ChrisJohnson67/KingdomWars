//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffHealOverTimeInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffHealOverTimeInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private BuffHealOverTimeTemplate m_timeTemplate;
	private int m_ticksRemaining = 0;
	private float m_timer = 0f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public BuffHealOverTimeInstance(BuffHealOverTimeTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
		m_timeTemplate = a_template;
		m_ticksRemaining = (int)(a_data.Duration / m_timeTemplate.SecondsEveryTrigger);
	}

	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);

		m_timer += a_deltaTime;
		if (m_timer >= m_timeTemplate.SecondsEveryTrigger)
		{
			m_timer -= m_timeTemplate.SecondsEveryTrigger;
			m_ticksRemaining--;
			m_context.Target.ApplyHeal(m_context.Target, (int)m_timeTemplate.Heal.GetHeal(m_context.Target, m_context.Target));
		}
	}


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
