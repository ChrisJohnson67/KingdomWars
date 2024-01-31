using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomSpellTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Room/RoomSpellTemplate")]
public class RoomSpellTemplate : RoomChoiceTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(AbilityTemplate), "Ability", "")]
	private int m_abilityTID;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public AbilityTemplate Ability { get { return AssetCacher.Instance.CacheAsset<AbilityTemplate>(m_abilityTID); } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

}