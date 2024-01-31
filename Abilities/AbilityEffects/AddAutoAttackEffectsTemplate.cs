using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Effects/AddAutoAttackEffectsTemplate")]
public class AddAutoAttackEffectsTemplate : AbilityEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(AbilityEffectTemplate), "Effects", "")]
	private List<int> m_effects;

	[SerializeField]
	private float m_duration;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	public List<int> Effects { get => m_effects; }
	public float Duration { get { return m_duration; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new AddAutoAttackEffectsInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
