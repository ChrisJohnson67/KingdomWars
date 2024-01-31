using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HealEffectInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions



	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected bool m_canApplyHeal = true;
	protected HealEffectTemplate m_healTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public HealEffectInstance(HealEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_healTemplate = a_template as HealEffectTemplate;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		var targets = GetTargets();
		float rollValue = UnityEngine.Random.Range(0f, 1f);
		foreach (var target in targets)
		{
			ApplyHeal(target, rollValue);
		}

		CompleteEffect();
	}

	protected virtual void ApplyHeal(UnitInstance a_target, float a_rollValue)
	{
		var minDmg = m_healTemplate.GetMinHeal(m_context.Source, a_target);
		var maxDmg = m_healTemplate.GetMaxHeal(m_context.Source, a_target);
		var heal = Mathf.Lerp(minDmg, maxDmg, a_rollValue);

		heal = m_context.AbilityInstance.Template.GetFinalHeal(m_context.Source, a_target, heal);
		CombatManager.Instance.ApplyHealToUnit(m_context.Source, a_target, heal);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
