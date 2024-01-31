//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealPerDebuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HealPerDebuffInstance : HealEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions



	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected HealPerDebuffTemplate m_healDebuffTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public HealPerDebuffInstance(HealPerDebuffTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_healDebuffTemplate = a_template;
	}

	protected override void ApplyHeal(UnitInstance a_target, float a_rollValue)
	{
		base.ApplyHeal(a_target, a_rollValue);

		int debuffs = 0;
		foreach (var buff in a_target.CurrentBuffs)
		{
			if (buff.Template.IsDebuff)
			{
				debuffs++;
			}
		}

		CombatManager.Instance.ApplyHealToUnit(m_context.Source, a_target, debuffs * m_healDebuffTemplate.HealPerDebuff);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
