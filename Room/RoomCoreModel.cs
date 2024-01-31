using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomCoreModel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[RequireComponent(typeof(TemplateID))]
public class RoomCoreModel : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private MeshRenderer m_mesh;


	//--- NonSerialized ---
	private RoomCoreTemplate m_template;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(RoomCoreTemplate a_template)
	{
		m_template = a_template;
	}

	public void Cleanup()
	{

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