//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RemoveBuffEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RemoveBuffEffectInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private RemoveBuffEffectTemplate m_removeTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public RemoveBuffEffectInstance(RemoveBuffEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_removeTemplate = a_template;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		var targets = GetTargets();
		foreach (var target in targets)
		{
			foreach (var buffTemplate in m_removeTemplate.BuffsToRemove)
			{
				target.ForceRemoveBuff(buffTemplate);
			}
		}

		CompleteEffect();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
