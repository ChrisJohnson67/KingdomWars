using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ModifyDamageBuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class ModifyDamageBuffTemplate : BuffTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected StatTemplate m_affectedStat;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public StatTemplate AffectedStat { get { return m_affectedStat; } }


	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
