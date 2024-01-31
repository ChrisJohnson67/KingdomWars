using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Effects/RemoveBuffEffectTemplate")]
public class RemoveBuffEffectTemplate : AbilityEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(BuffTemplate), "buffs To Remove", "")]
	private List<int> m_buffsToRemove;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	public List<int> BuffsToRemove { get => m_buffsToRemove; }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new RemoveBuffEffectInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
