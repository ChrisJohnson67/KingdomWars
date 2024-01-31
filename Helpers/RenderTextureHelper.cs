using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RenderTextureHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RenderTextureHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_unitParent;

	[SerializeField]
	private Camera m_camera;

	//--- NonSerialized ---
	private UnitModel m_unit;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(int a_unitModelTID)
	{
		if (m_unit != null)
		{
			Destroy(m_unit.gameObject);
		}
		if (a_unitModelTID != tid.NULL)
		{
			m_unit = AssetCacher.Instance.InstantiateComponent<UnitModel>(a_unitModelTID, m_unitParent);
		}
	}

	public void SetRenderTexture(RenderTexture a_tex)
	{
		if (m_camera != null)
			m_camera.targetTexture = a_tex;
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