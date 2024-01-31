using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityContextData
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AbilityContextData
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	public UnitInstance Source { get; set; }
	public List<UnitInstance> Targets { get; set; }
	public AbilityTemplate.CombatTarget TargetSelection { get; set; }
	public AbilityInstance AbilityInstance { get; set; }

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AbilityContextData() { }

	public AbilityContextData(AbilityContextData a_context)
	{
		Source = a_context.Source;
		Targets = a_context.Targets;
		TargetSelection = a_context.TargetSelection;
		AbilityInstance = a_context.AbilityInstance;
	}

	public Vector3 GetAbilityPosition()
	{
		if (Targets != null && Targets.Count > 0)
		{
			return Targets[0].Model.transform.position;
		}
		return Vector3.zero;
	}


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
