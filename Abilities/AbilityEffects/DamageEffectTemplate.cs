using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DamageEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/DamageEffectTemplate")]
public class DamageEffectTemplate : AbilityEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected DamageToExecute m_damage;

	[SerializeField]
	protected string m_minDamageMadlib = "$MINDMG$";

	[SerializeField]
	protected string m_maxDamageMadlib = "$MAXDMG$";

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public DamageToExecute Damage { get { return m_damage; } }
	public string MaxDamageMadlib { get { return m_maxDamageMadlib; } }
	public string MinDamageMadlib { get { return m_minDamageMadlib; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new DamageEffectInstance(this, a_context);
	}

	public float GetMinDamage(UnitInstance a_source, UnitInstance a_target)
	{
		float damage = 0f;
		damage = m_damage.GetDamage(a_source, a_target);
		if (m_damage.IsDealingPercentHP)
		{
			return damage;
		}
		else
		{
			return damage;
		}
	}

	public float GetMaxDamage(UnitInstance a_source, UnitInstance a_target)
	{
		float damage = 0f;
		damage = m_damage.GetDamage(a_source, a_target);
		if (m_damage.IsDealingPercentHP)
		{
			return damage;
		}
		else
		{
			return damage;
		}
	}

	//public override string PopulateMadlibs(AbilityTemplate a_abilityTemplate, UnitInstance a_source, int a_rarityLevel, string a_text)
	//{
	//	var rarityData = GetRarityData(a_rarityLevel);
	//	var color = rarityData.DamageToExecute.DamageType.Color;
	//	if (!string.IsNullOrEmpty(m_minDamageMadlib))
	//	{
	//		var minDmg = GetMinDamage(a_source, null, a_rarityLevel);
	//		if (rarityData.DamageToExecute.IsDealingPercentHP)
	//		{
	//			minDmg = minDmg * 100f;
	//		}
	//		if (a_source != null)
	//		{
	//			minDmg = a_abilityTemplate.GetFinalDamage(a_source, null, rarityData.DamageToExecute.DamageTypeTID, minDmg);
	//		}
	//
	//		a_text = UISettings.InsertMadlibWithColor(a_text, m_minDamageMadlib, ((int)minDmg).ToString(), color);
	//	}
	//
	//	if (!string.IsNullOrEmpty(m_maxDamageMadlib))
	//	{
	//		var maxDmg = GetMaxDamage(a_source, null, a_rarityLevel);
	//		if (a_source != null)
	//		{
	//			maxDmg = a_abilityTemplate.GetFinalDamage(a_source, null, rarityData.DamageToExecute.DamageTypeTID, maxDmg);
	//		}
	//
	//		a_text = UISettings.InsertMadlibWithColor(a_text, m_maxDamageMadlib, ((int)maxDmg).ToString(), color);
	//	}
	//	return a_text;
	//}



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	public void OnValidate()
	{
		if (m_damage != null && m_damage.DamageMultiplier == 0f)
		{
			m_damage.DamageMultiplier = 1f;
		}
	}
#endif
}
