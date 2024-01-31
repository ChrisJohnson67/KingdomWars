using Platform.UIManagement;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// MyLevelsUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class MyLevelsUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_entryPArent;

	[SerializeField, TemplateIDField(typeof(MyLevelEntry), "Entry", "")]
	private int m_entryTID;


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Start()
	{
		var myLevels = SaveManager.GetMyLevels();
		foreach (var level in myLevels)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<MyLevelEntry>(m_entryTID, m_entryPArent);
			entry.Init(this, level);
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClose()
	{
		UIManager.Instance.CloseUI(this);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}