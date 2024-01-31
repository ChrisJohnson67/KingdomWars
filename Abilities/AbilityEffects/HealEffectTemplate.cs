using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/HealEffectTemplate")]
public class HealEffectTemplate : AbilityEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_MinHealMadlib = "$MINHEAL$";
	public const string c_MaxHealMadlib = "$MAXHEAL$";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected HealToExecute m_heal = new HealToExecute();


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public HealToExecute Heal { get { return m_heal; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new HealEffectInstance(this, a_context);
	}

	public float GetMinHeal(UnitInstance a_source, UnitInstance a_target)
	{
		float heal = 0f;
		if (a_source != null)
		{
			heal = m_heal.GetHeal(a_source, a_target);
		}
		if (m_heal.IsDealingPercentHP)
		{
			return heal;
		}
		else
		{
			return heal;
		}
	}

	public float GetMaxHeal(UnitInstance a_source, UnitInstance a_target)
	{
		float heal = 0f;
		if (a_source != null)
		{
			heal = m_heal.GetHeal(a_source, a_target);
		}
		if (m_heal.IsDealingPercentHP)
		{
			return heal;
		}
		else
		{
			return heal;
		}
	}

	//public override string PopulateMadlibs(AbilityTemplate a_abilityTemplate, UnitInstance a_source, int a_rarityLevel, string a_text)
	//{
	//	var rarityData = GetRarityData(a_rarityLevel);
	//	if (!string.IsNullOrEmpty(c_MinHealMadlib))
	//	{
	//		var minDmg = GetMinHeal(a_source, null, a_rarityLevel);
	//		if (rarityData.HealToExecute.IsDealingPercentHP)
	//		{
	//			minDmg = minDmg * 100f;
	//		}
	//		if (a_source != null)
	//		{
	//			minDmg = a_abilityTemplate.GetFinalHeal(a_source, null, minDmg);
	//		}
	//
	//		a_text = UISettings.InsertMadlibWithColor(a_text, c_MinHealMadlib, ((int)minDmg).ToString(), GameManager.Instance.UISettings.HealColor);
	//	}
	//
	//	if (!string.IsNullOrEmpty(c_MaxHealMadlib))
	//	{
	//		var maxDmg = GetMaxHeal(a_source, null, a_rarityLevel);
	//		if (a_source != null)
	//		{
	//			maxDmg = a_abilityTemplate.GetFinalHeal(a_source, null, maxDmg);
	//		}
	//
	//		a_text = UISettings.InsertMadlibWithColor(a_text, c_MaxHealMadlib, ((int)maxDmg).ToString(), GameManager.Instance.UISettings.HealColor);
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
		if (m_heal != null && m_heal.HealMultiplier == 0f)
		{
			m_heal.HealMultiplier = 1f;
		}
	}
#endif
}
