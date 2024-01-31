//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AddBuffOverTimeEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AddBuffOverTimeEffectInstance : AddBuffEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions



	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected float m_addBuffTimer;
	protected AddBuffOverTimeTemplate m_timeTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AddBuffOverTimeEffectInstance(AddBuffOverTimeTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_timeTemplate = a_template;
	}

	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);

		m_addBuffTimer += a_deltaTime;

		if(m_addBuffTimer >= m_timeTemplate.TriggerEverySeconds)
		{
			m_addBuffTimer -= m_timeTemplate.TriggerEverySeconds;
			ApplyEffect();
		}
	}


	protected override void CompleteAddBuff()
	{
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
