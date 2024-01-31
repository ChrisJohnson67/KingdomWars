//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// StatBuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class StatBuffInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private StatBuffTemplate m_statTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public StatBuffInstance(StatBuffTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
		m_statTemplate = a_template;
	}

	protected override void ApplyBuff()
	{
		base.ApplyBuff();

		if (m_statTemplate.CanStatBuffStack)
		{
			foreach (var statTemplate in m_statTemplate.StatTemplates)
			{
				var stat = m_context.Target.CurrentStats.GetStat(statTemplate.TID);
				if (statTemplate.TID == StatSettings.MaxHealthStatTID)
				{
					//increase unit's current health
					int amount = m_statTemplate.BaseAddAmount > 0 ? (int)m_statTemplate.BaseAddAmount : ((int)(m_statTemplate.PercentageAddAmount * stat.CurrentAmount));
					m_context.Target.ApplyHeal(null, amount);
				}

				if (stat != null)
				{
					stat.Add(m_statTemplate.BaseAddAmount, m_statTemplate.PercentageAddAmount);
				}
			}
		}
		else
		{
			AddDuration(m_context.Duration);
		}
	}

	public override void RemoveBuff()
	{
		base.RemoveBuff();
		foreach (var statTemplate in m_statTemplate.StatTemplates)
		{
			var stat = m_context.Target.CurrentStats.GetStat(statTemplate.TID);
			if (stat != null)
			{
				stat.Subtract(m_statTemplate.BaseAddAmount, m_statTemplate.PercentageAddAmount);
			}
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
