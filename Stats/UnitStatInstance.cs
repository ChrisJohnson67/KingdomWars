using System.Collections.Generic;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitStatInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class UnitStatInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected UnitStatTemplate m_unitStatTemplate;

	protected List<StatInstance> m_stats = new List<StatInstance>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<StatInstance> Stats { get { return m_stats; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public UnitStatInstance(UnitStatTemplate a_unitStatTemplate)
	{
		m_unitStatTemplate = a_unitStatTemplate;
		foreach (var stat in m_unitStatTemplate.BaseStatDataList)
		{
			m_stats.Add(stat.CreateStatInstance());
		}
	}

	public StatInstance GetStat(int a_statTID)
	{
		return m_stats.Find(x => x.Template.TID == a_statTID);
	}

	public float GetCurrentAmountOfStat(int a_statTID)
	{
		var stat = GetStat(a_statTID);
		if (stat == null)
		{
			return 0f;
		}
		return stat.CurrentAmount;
	}

	public float GetMaxHealth()
	{
		var stat = GetStat(StatSettings.MaxHealthStatTID);
		if (stat != null)
		{
			return stat.CurrentAmount;
		}
		return 0f;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
