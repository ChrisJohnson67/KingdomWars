using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LevelSelectPanel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class LevelSelectPanel : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected string m_titleText;

	//--- NonSerialized ---
	protected LevelSelectUI m_parent;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public virtual void Init(LevelSelectUI a_parent)
	{
		m_parent = a_parent;
		m_parent.SetTitle(m_titleText);
	}

	public abstract void OnBackButton();

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