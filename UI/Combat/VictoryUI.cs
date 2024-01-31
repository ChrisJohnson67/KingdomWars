using Platform.UIManagement;
using System;
using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// VictoryUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class VictoryUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GameObject m_vicotryObj;

	[SerializeField]
	private GameObject m_DefeatObj;

	[SerializeField]
	private GameObject m_winPointsObj;

	[SerializeField]
	private GameObject m_losePointsObj;

	[SerializeField]
	private GameObject m_retryButton;

	[SerializeField]
	private SpecificSoundGroup m_victorySound;

	[SerializeField]
	private SpecificSoundGroup m_DefeatSound;

	//--- NonSerialized ---
	private Action<bool> m_onContinue;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(bool a_win, Action<bool> a_onContinue)
	{
		UIUtils.SetActive(m_vicotryObj, a_win);
		UIUtils.SetActive(m_DefeatObj, !a_win);

		UIUtils.SetActive(m_winPointsObj, a_win);
		UIUtils.SetActive(m_losePointsObj, !a_win);

		UIUtils.SetActive(m_retryButton, !a_win);

		m_onContinue = a_onContinue;

		StartCoroutine(WaitToPlaySound(a_win));

	}

	private IEnumerator WaitToPlaySound(bool a_win)
	{
		yield return new WaitForSeconds(0.2f);
		if (a_win)
		{
			if (m_victorySound != null)
				m_victorySound.Play();
		}
		else
		{
			if (m_DefeatSound != null)
			{
				m_DefeatSound.Play();
			}
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnContinue()
	{
		UIManager.Instance.CloseUI(this);
		m_onContinue?.Invoke(false);
	}

	public void OnRetry()
	{
		UIManager.Instance.CloseUI(this);
		m_onContinue?.Invoke(true);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}