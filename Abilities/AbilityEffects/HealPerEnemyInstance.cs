//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealPerEnemyInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HealPerEnemyInstance : HealEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions



	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected HealPerEnemyTemplate m_healDebuffTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public HealPerEnemyInstance(HealPerEnemyTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_healDebuffTemplate = a_template;
	}

	protected override void ApplyHeal(UnitInstance a_target, float a_rollValue)
	{
		base.ApplyHeal(a_target, a_rollValue);

		var enemies = CombatManager.Instance.GetEnemiesFromUnit(CombatManager.Instance.HeroUnit);
		CombatManager.Instance.ApplyHealToUnit(m_context.Source, a_target, enemies.Count * m_healDebuffTemplate.HealPerTarget);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
