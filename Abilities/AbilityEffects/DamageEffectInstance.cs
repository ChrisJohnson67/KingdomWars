using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DamageEffectInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions



	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected bool m_canApplyDamage = true;
	protected DamageEffectTemplate m_damageTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public DamageEffectInstance(DamageEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_damageTemplate = a_template as DamageEffectTemplate;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		var targets = GetTargets();
		float rollValue = UnityEngine.Random.Range(0f, 1f);
		foreach (var target in targets)
		{
			ApplyDamage(target, rollValue);
		}


		CompleteDamageEffect();
	}

	protected virtual void CompleteDamageEffect()
	{
		CompleteEffect();
	}

	protected virtual void ApplyDamage(UnitInstance a_target, float a_rollValue)
	{
		var minDmg = m_damageTemplate.GetMinDamage(m_context.Source, a_target);
		var maxDmg = m_damageTemplate.GetMaxDamage(m_context.Source, a_target);
		var damage = Mathf.Lerp(minDmg, maxDmg, a_rollValue);

		damage = m_context.AbilityInstance.Template.GetFinalDamage(m_context.Source, a_target, m_damageTemplate.Damage.DamageTypeTID, damage);

		//roll for crit
		bool willCrit = false;
		if (m_damageTemplate.Damage.CritChance > 0f)
		{
			willCrit = m_damageTemplate.Damage.CritChance >= RandomHelpers.GetRandomValue(0f, 1f);
			damage *= 2f;
		}

		var damageInfo = DamageAppliedInfo.GetDamageInfo(true);
		damageInfo.AddDamage(m_damageTemplate.Damage.DamageTypeTID, (int)damage, willCrit);
		CombatManager.Instance.ApplyDamageToUnit(m_context.Source, a_target, damageInfo);

	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
