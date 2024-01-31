using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// StatTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/UnitStatTemplate")]
public class UnitStatTemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	[Serializable]
	public class UnitBaseStatData
	{
		[SerializeField]
		private StatTemplate m_statTemplate;

		[SerializeField]
		private float m_baseAmount;

		public StatTemplate StatTemplate { get { return m_statTemplate; } }
		public float BaseAmount { get { return m_baseAmount; } }

		public StatInstance CreateStatInstance()
		{
			return m_statTemplate.CreateStatInstance(m_baseAmount);
		}
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<UnitBaseStatData> m_baseStatDataList;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<UnitBaseStatData> BaseStatDataList { get { return m_baseStatDataList; } }


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public UnitStatInstance CreateInstance()
	{
		return new UnitStatInstance(this);
	}

	public float GetCurrentAmountOfStatFromTemplate(int a_statTID)
	{
		var stat = m_baseStatDataList.Find(x => x.StatTemplate.TID == a_statTID);
		if (stat != null)
		{
			return stat.BaseAmount;
		}
		return 0f;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
