using Platform.UIManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// GenericPopupUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class GenericPopupUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_description;

	[SerializeField]
	private Button m_cancelButton;


	//--- NonSerialized ---
	private Action m_onConfirm;
	private Action m_onCancel = null;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(string a_desc, Action a_onConfirm, Action a_onCancel = null)
	{
		m_description.text = a_desc;
		m_onConfirm = a_onConfirm;
		m_onCancel = a_onCancel;

		UIUtils.SetActive(m_cancelButton, m_onCancel != null);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirm()
	{
		UIManager.Instance.CloseUI(this);
		m_onConfirm?.Invoke();
	}

	public void OnCancel()
	{
		UIManager.Instance.CloseUI(this);
		m_onCancel?.Invoke();
	}

	#endregion Callbacks

	public static GenericPopupUI CreatePopup(string a_desc, Action a_onConfirm, Action a_onCancel = null)
	{
		var popup = UIManager.Instance.OpenUI<GenericPopupUI>(GameManager.Instance.UISettings.GenericPopupUITID);
		popup.Init(a_desc, a_onConfirm, a_onCancel);
		return popup;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}