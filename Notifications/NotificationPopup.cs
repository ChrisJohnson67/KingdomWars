using Platform.UIManagement;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// NotificationPopup
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class NotificationPopup : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private TMP_Text m_windesc;

	[SerializeField]
	private TMP_Text m_lossdesc;

	[SerializeField]
	private string m_winText;

	[SerializeField]
	private string m_lossText;

	[SerializeField]
	private float m_aliveTime = 3f;

	//--- NonSerialized ---
	private Action m_onComplete;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
		StartCoroutine(DestroyCR());
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(NotificationManager.KingdomUpdateData a_updateData, Action a_onComplete)
	{
		m_onComplete = a_onComplete;

		var kingdomData = GameManager.Instance.CachedKingdomData.Find(x => x.LevelID == a_updateData.LevelID);
		if (kingdomData == null)
		{
			CloseUI();
		}

		UIUtils.SetActive(m_lossdesc, a_updateData.LossDiff > 0 && kingdomData != null);
		UIUtils.SetActive(m_windesc, a_updateData.WinDiff > 0 && kingdomData != null);


		if (kingdomData != null)
		{
			m_lossdesc.text = a_updateData.LossDiff.ToString() + m_lossText + kingdomData.Title + "!";
			m_windesc.text = a_updateData.WinDiff.ToString() + m_winText + kingdomData.Title + "!";
		}
	}

	private IEnumerator DestroyCR()
	{
		yield return new WaitForSeconds(m_aliveTime);

		UIUtils.SetTrigger(m_animator, "tClose");
	}

	private void OnCloseComplete()
	{
		CloseUI();
	}

	public void CloseUI()
	{
		m_onComplete?.Invoke();
		UIManager.Instance.CloseUI(this);
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