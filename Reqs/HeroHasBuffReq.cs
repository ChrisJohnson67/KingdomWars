using Platform.ReqResults;
using Platform.ReqResultsV2;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HasBuffReq
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HeroHasBuffReq : Requirement
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(BuffTemplate), "Buff", "")]
	private int m_buffTID;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override bool TestRequirement(EventParticipant a_participant)
	{
		return CombatManager.Instance.HeroUnit.ContainsBuff(m_buffTID);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}