using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Buffs/PreventHealBuffTemplate")]
public class PreventHealBuffTemplate : BuffTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors



	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override BuffInstance CreateBuffInstance(BuffContextData a_context)
	{
		return new PreventHealBuffInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
