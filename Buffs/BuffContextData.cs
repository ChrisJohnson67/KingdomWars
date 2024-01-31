//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffContextData
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffContextData
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	public UnitInstance Source { get; set; }
	public UnitInstance Target { get; set; }
	public float Duration { get; set; }
	public bool IsTimed { get { return Duration > 0f; } }
	public AddBuffEffectTemplate.RemoveTrigger RemoveTrigger { get; set; } 

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions


	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
