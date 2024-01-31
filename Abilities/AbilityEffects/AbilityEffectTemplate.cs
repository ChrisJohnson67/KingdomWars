using Platform.ReqResults;
using Platform.ReqResultsV2;
using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable]
public abstract class AbilityEffectTemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum FXPosition
	{
		Source,
		Targets,
		RoomCenter
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected float m_delayTime = 0f;

	[SerializeField]
	protected bool m_applyEffectAfterDelay = true;

	[SerializeField]
	protected AbilityTemplate.CombatTarget m_target;

	[SerializeField]
	protected RequirementList m_startRequirements = new RequirementList();

	[SerializeField]
	protected GameObject m_fxObject;

	[SerializeField]
	private SpecificSoundGroup m_sound;

	[SerializeField]
	protected FXPosition m_fxPosition = FXPosition.Source;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public float Delay { get { return m_delayTime; } }
	public AbilityTemplate.CombatTarget Target { get { return m_target; } }
	public bool ApplyEffectAfterDelay { get { return m_applyEffectAfterDelay; } }
	public GameObject FXObject { get { return m_fxObject; } }
	public FXPosition FXPos { get { return m_fxPosition; } }
	public SpecificSoundGroup Sound { get { return m_sound; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public abstract AbilityEffectInstance CreateInstance(AbilityContextData a_context);

	public bool MeetsStartRequirements(AbilityContextData a_contextData)
	{
		var eventInfo = new EventParticipant();
		eventInfo.AddData(new CombatData(a_contextData, null));
		return m_startRequirements.TestRequirement(eventInfo);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
