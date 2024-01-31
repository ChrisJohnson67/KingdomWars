//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffDamageInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffDamageInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private BuffDamageTemplate m_damageTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public BuffDamageInstance(BuffDamageTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
		m_damageTemplate = a_template;
	}

	protected void DealDamage()
	{
		var damageInfo = DamageAppliedInfo.GetDamageInfo(m_template.Trigger != BuffTemplate.BuffTrigger.OnDamage);
		int damage = (int)m_damageTemplate.DamageToExecute.GetDamage(m_context.Source, m_context.Target);

		//roll for crit
		bool willCrit = false;
		if (m_damageTemplate.DamageToExecute.CritChance > 0f)
		{
			willCrit = m_damageTemplate.DamageToExecute.CritChance >= RandomHelpers.GetRandomValue(0f, 1f);
			damage *= 2;
		}

		damageInfo.AddDamage(m_damageTemplate.DamageToExecute.DamageTypeTID, damage, willCrit);
		CombatManager.Instance.ApplyDamageToUnit(m_context.Source, m_context.Target, damageInfo);
	}

	protected override void TriggerBuffEffects()
	{
		base.TriggerBuffEffects();

		DealDamage();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
