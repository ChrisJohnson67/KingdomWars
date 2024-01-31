using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName ="Buffs/StatBuffTemplate")]
public class StatBuffTemplate : BuffTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<StatTemplate> m_statTemplates;

	[SerializeField]
	private float m_baseAddAmount;

	[SerializeField]
	private float m_percentageAddAmount;

	[SerializeField]
	protected bool m_canStatBuffStack = true;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	public List<StatTemplate> StatTemplates { get => m_statTemplates;  }
	public float BaseAddAmount { get => m_baseAddAmount;  }
	public float PercentageAddAmount { get => m_percentageAddAmount; }
	public bool CanStatBuffStack { get { return m_canStatBuffStack; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override BuffInstance CreateBuffInstance(BuffContextData a_context)
	{
		return new StatBuffInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
