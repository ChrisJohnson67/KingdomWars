using System;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomNamePopup
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomNamePopup : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_InputField m_inputField;

	[SerializeField]
	private GameObject m_confirmButton;

	[SerializeField]
	private GameObject m_spinnerObject;

	//--- NonSerialized ---
	private Action<string> m_onComplete;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Action<string> a_onComplete)
	{
		m_onComplete = a_onComplete;
		m_inputField.text = "Kingdom" + RandomHelpers.GetRandomValue(100, 1000);
		m_inputField.Select();
		m_inputField.ActivateInputField();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirmClicked()
	{
		m_inputField.SetInteractive(false);
		m_onComplete?.Invoke(m_inputField.text);
		UIUtils.SetActive(m_confirmButton, false);
		UIUtils.SetActive(m_spinnerObject, true);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}