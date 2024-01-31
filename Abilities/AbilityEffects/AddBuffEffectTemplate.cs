using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AddBuffEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/AddBuffEffectTemplate")]
public class AddBuffEffectTemplate : AbilityEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_Duration = "$TURNS$";
	public const string c_StackMadlib = "$STACKS$";
	public const string c_ChanceMadlib = "$CHANCE$";

	public enum RemoveTrigger
	{
		Duration,
		RoomEnd,
		Never
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected List<BuffTemplate> m_buffsAdded = new List<BuffTemplate>();

	[SerializeField]
	protected float m_duration = 0f;

	[SerializeField]
	protected int m_stackAmount = 1;

	[SerializeField]
	protected RemoveTrigger m_removeTrigger;

	[SerializeField, Range(0f, 1f)]
	protected float m_chance = 1f;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<BuffTemplate> BuffsAdded { get { return m_buffsAdded; } }
	public int StackAmount { get { return m_stackAmount; } }
	public float Chance { get { return m_chance; } }
	public float Duration { get { return m_duration; } }
	public RemoveTrigger RemoveTriggerCondition { get { return m_removeTrigger; } }
	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new AddBuffEffectInstance(this, a_context);
	}

	//public override string PopulateMadlibs(AbilityTemplate a_abilityTemplate, UnitInstance a_source, string a_text)
	//{
	//	foreach (var buff in m_buffsAdded)
	//	{
	//		a_text = buff.PopulateMadlibs(a_abilityTemplate, a_source, a_text);
	//	}
	//
	//	if (m_turnDuration > 0)
	//	{
	//		a_text = UISettings.InsertMadlib(a_text, c_Duration, m_turnDuration);
	//	}
	//	a_text = UISettings.InsertMadlib(a_text, c_StackMadlib, m_stackAmount);
	//	a_text = UISettings.InsertMadlib(a_text, c_ChanceMadlib, (int)(m_chance * 100f));
	//	return a_text;
	//}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
