using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealPerDebuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/HealPerDebuffTemplate")]
public class HealPerDebuffTemplate : HealEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected int m_healPerDebuff;


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int HealPerDebuff { get { return m_healPerDebuff; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new HealPerDebuffInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
#endif
}
