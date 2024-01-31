using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// MonsterGroupTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Room/MonsterGroupTemplate")]
public class MonsterGroupTemplate : RoomChoiceTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(MonsterTemplate), "MonsterTemplate", "")]
	private List<int> m_monsterTIDs;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<int> MonsterTIDs { get { return m_monsterTIDs; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

}