using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomChoiceTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class RoomChoiceTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(DisplayTemplate), "Danger Templates", "")]
	protected List<int> m_dangerTIDs;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<int> DangerTIDs { get { return m_dangerTIDs; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void GetDangers(List<int> a_dangers)
	{
		a_dangers.AddRange(m_dangerTIDs);
	}

	#endregion Runtime Functions

}