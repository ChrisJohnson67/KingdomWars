using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomCoreTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Room/RoomCoreTemplate")]
public class RoomCoreTemplate : RoomChoiceTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(AbilityTemplate), "Ability", "")]
	private int m_abilityTID;

	[SerializeField, TemplateIDField(typeof(RoomCoreModel), "Core Model", "")]
	private int m_coreModelTID;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public AbilityTemplate Ability { get { return AssetCacher.Instance.CacheAsset<AbilityTemplate>(m_abilityTID); } }
	public int CoreModel { get { return m_coreModelTID; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

}