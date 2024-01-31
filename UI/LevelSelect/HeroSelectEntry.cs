using Platform.UI;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HeroSelectEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HeroSelectEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private CoreRawImage m_image;

	[SerializeField]
	private GameObject m_selectedObject;

	//--- NonSerialized ---
	private int m_heroTID;
	private HeroSelectPanel m_parent;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int HeroTID { get { return m_heroTID; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(HeroSelectPanel a_parent, int a_heroTID, RenderTexture a_renText)
	{
		m_heroTID = a_heroTID;
		m_parent = a_parent;

		base.Init(AssetCacher.Instance.CacheAsset<UnitTemplate>(m_heroTID));

		if (m_image != null)
			m_image.texture = a_renText;
	}

	public void SetSelected(bool a_selected)
	{
		UIUtils.SetActive(m_selectedObject, a_selected);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		m_parent.OnHeroSelected(this);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}