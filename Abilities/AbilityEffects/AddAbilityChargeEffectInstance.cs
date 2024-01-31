//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AddAbilityChargeEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AddAbilityChargeEffectInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private AddAbilityChargeEffectTemplate m_removeTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AddAbilityChargeEffectInstance(AddAbilityChargeEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_removeTemplate = a_template;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		var targets = GetTargets();
		foreach (var target in targets)
		{
			target.AddAbilityCharges(m_removeTemplate.Charges);
		}

		CompleteEffect();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
