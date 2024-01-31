using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// StatInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class StatInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected StatTemplate m_template;
	protected float m_baseAmount;
	protected float m_additionalAmount;
	protected float m_additionalPercentIncrease;

	protected float m_currentAmount;
	protected bool m_dirty = true;

	public Action OnChanged;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public StatTemplate Template { get { return m_template; } }
	public float CurrentAmount { get { return m_dirty ? CalculateStat() : m_currentAmount; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public StatInstance(StatTemplate a_template, float a_baseAmount)
	{
		m_template = a_template;
		m_baseAmount = a_baseAmount;
		m_dirty = true;
	}

	public void Add(float a_additionalAmount, float a_addPercentIncrease)
	{
		m_additionalAmount += a_additionalAmount;
		m_additionalPercentIncrease += a_addPercentIncrease;
		m_dirty = true;
		OnChanged?.Invoke();
	}

	public void Subtract(float a_additionalAmount, float a_addPercentIncrease)
	{
		m_additionalAmount -= a_additionalAmount;
		m_additionalPercentIncrease -= a_addPercentIncrease;
		m_dirty = true;
		OnChanged?.Invoke();
	}

	protected float CalculateStat()
	{
		float rawAmount = m_baseAmount + m_additionalAmount;
		rawAmount = m_template.OnlyPositive ? Mathf.Max(0f, rawAmount) : rawAmount;
		m_currentAmount = rawAmount * (1f + m_additionalPercentIncrease);
		return m_currentAmount;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
