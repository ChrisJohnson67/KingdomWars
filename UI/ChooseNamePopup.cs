using Photon.Pun;
using Platform.UIManagement;
using System;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ChooseNamePopup
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ChooseNamePopup : MonoBehaviour
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

	//--- NonSerialized ---
	private Action m_onComplete;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Action a_onComplete)
	{
		m_onComplete = a_onComplete;
		m_inputField.text = "Majesty" + RandomHelpers.GetRandomValue(100, 1000);
		m_inputField.Select();
		m_inputField.ActivateInputField();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirmClicked()
	{
		m_inputField.SetInteractive(false);
		m_onComplete?.Invoke();
		UIUtils.SetActive(m_confirmButton, false);
		SaveManager.SetPlayerName(m_inputField.text);
		PhotonNetwork.NickName = m_inputField.text;
		UIManager.Instance.CloseUI(this);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}