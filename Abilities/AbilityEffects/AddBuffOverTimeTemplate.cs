using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/AddBuffOverTimeTemplate")]
public class AddBuffOverTimeTemplate : AddBuffEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected float m_triggerEverySeconds = 5f;


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public float TriggerEverySeconds { get { return m_triggerEverySeconds; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new AddBuffOverTimeEffectInstance(this, a_context);
	}

	


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
#endif
}
