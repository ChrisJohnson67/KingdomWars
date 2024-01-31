using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealAppliedInfo
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable]
public class HealToExecute
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private StatTemplate m_scalingStat;

	[SerializeField]
	private bool m_useStatAsBaseAmount = true;

	[SerializeField]
	private int m_flatHeal;

	[SerializeField]
	private float m_percentHeal;

	[SerializeField]
	private bool m_percentAsMaxHealth;

	[SerializeField]
	protected float m_healMultiplier = 1f;

	public StatTemplate ScalingStat { get => m_scalingStat; }
	public int FlatHeal { get => m_flatHeal; }
	public float PercentHeal { get => m_percentHeal; }
	public bool PercentAsMaxHealth { get => m_percentAsMaxHealth; }
	public float HealMultiplier { get { return m_healMultiplier; } set { m_healMultiplier = value; } }
	public bool IsDealingPercentHP { get { return m_percentHeal > 0f; } }

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public float GetHeal(UnitInstance a_source, UnitInstance a_target)
	{
		float totalHeal = m_flatHeal;
		if (m_useStatAsBaseAmount && m_scalingStat != null && !IsDealingPercentHP)
		{
			if (a_source != null)
			{
				totalHeal += a_source.CurrentStats.GetCurrentAmountOfStat(m_scalingStat.TID);
			}
		}
		if (IsDealingPercentHP)
		{
			if (a_target != null)
			{
				totalHeal += (PercentAsMaxHealth ? a_target.CurrentStats.GetMaxHealth() : a_target.CurrentHealth) * PercentHeal;
			}
			else
			{
				return totalHeal += PercentHeal;
			}
		}

		totalHeal *= m_healMultiplier;
		return Mathf.Round(totalHeal);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
