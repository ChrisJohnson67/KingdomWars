using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffDamageTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Buffs/BuffDamageTemplate")]
public class BuffDamageTemplate : BuffTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_DamageMadlib = "$DMG$";
	public const string c_HealthTypeMadlib = "$HEALTHTYPE$";
	public const string c_CurrentHealth = "current";
	public const string c_MaxHealth = "max";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private DamageToExecute m_damageToExecute;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public DamageToExecute DamageToExecute { get { return m_damageToExecute; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override BuffInstance CreateBuffInstance(BuffContextData a_context)
	{
		return new BuffDamageInstance(this, a_context);
	}

	//public override string PopulateMadlibs(AbilityTemplate a_abilityTemplate, UnitInstance a_source, int a_rarityLevel, string a_text)
	//{
	//	var color = m_damageToExecute.DamageType.Color;
	//	float damage = 0f;
	//	if (a_source != null)
	//	{
	//		damage = m_damageToExecute.GetDamage(a_source, null);
	//	}
	//	if (m_damageToExecute.IsDealingPercentHP)
	//	{
	//		damage = damage * 100f;
	//	}
	//	if (a_source != null && a_abilityTemplate != null)
	//	{
	//		damage = a_abilityTemplate.GetFinalDamage(a_source, null, m_damageToExecute.DamageTypeTID, damage);
	//	}
	//
	//	a_text = UISettings.InsertMadlibWithColor(a_text, c_DamageMadlib, ((int)damage).ToString(), color);
	//	a_text = UISettings.InsertMadlib(a_text, c_HealthTypeMadlib, m_damageToExecute.PercentAsMaxHealth ? c_MaxHealth : c_CurrentHealth);
	//
	//	return base.PopulateMadlibs(a_abilityTemplate, a_source, a_rarityLevel, a_text);
	//}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
