using Platform.UIManagement;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatStartPopup
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class CombatStartPopup : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_timerText;

	[SerializeField]
	private string m_text;

	//--- NonSerialized ---
	private int m_lastCount = -1;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void Update()
	{
		if (CombatManager.Instance == null)
		{
			CloseUI();
			return;
		}
		int newCount = (int)GameManager.Instance.KingdomSettings.RoomPauseTime - Mathf.CeilToInt(CombatManager.Instance.RoomPauseTimer);
		if (newCount != m_lastCount)
		{
			m_timerText.text = m_text + newCount;
			m_lastCount = newCount;
		}
	}

	public void CloseUI()
	{
		UIManager.Instance.CloseUI(this);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnStartCombat()
	{
		CombatManager.Instance.SkipRoomPause();
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}