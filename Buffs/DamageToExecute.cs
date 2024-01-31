using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DamageAppliedInfo
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable]
public class DamageToExecute
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private StatTemplate m_scalingStat;

	[SerializeField]
	private TagTemplate m_damageType;

	[SerializeField]
	private bool m_useStatAsBaseAmount = true;

	[SerializeField]
	private int m_flatDamage;

	[SerializeField]
	private float m_percentDamage;

	[SerializeField]
	private bool m_percentAsMaxHealth;

	[SerializeField]
	protected float m_damageMultiplier = 1f;

	[SerializeField, Range(0f, 1f)]
	protected float m_critChance = 0f;

	public StatTemplate ScalingStat { get => m_scalingStat; }
	public int FlatDamage { get => m_flatDamage; }
	public float PercentDamage { get => m_percentDamage; }
	public bool PercentAsMaxHealth { get => m_percentAsMaxHealth; }
	public TagTemplate DamageType { get { return m_damageType; } }
	public int DamageTypeTID { get { return m_damageType.TID; } }
	public float DamageMultiplier { get { return m_damageMultiplier; } set { m_damageMultiplier = value; } }
	public bool IsDealingPercentHP { get { return m_percentDamage > 0f; } }
	public float CritChance { get { return m_critChance; } }

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public float GetDamage(UnitInstance a_source, UnitInstance a_target)
	{
		float totalDamage = m_flatDamage;
		if (m_useStatAsBaseAmount && m_scalingStat != null && !IsDealingPercentHP)
		{
			if (a_source != null)
			{
				totalDamage += a_source.CurrentStats.GetCurrentAmountOfStat(m_scalingStat.TID);
			}
		}
		if (IsDealingPercentHP)
		{
			if (a_target != null)
			{
				totalDamage += (PercentAsMaxHealth ? a_target.CurrentStats.GetMaxHealth() : a_target.CurrentHealth) * PercentDamage;
			}
			else
			{
				return totalDamage += PercentDamage;
			}
		}

		totalDamage *= m_damageMultiplier;

		return Mathf.Round(totalDamage);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
