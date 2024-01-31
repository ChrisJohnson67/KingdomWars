using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DangerParent
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DangerParent : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private bool m_displayCount;

	[SerializeField]
	private TMP_Text m_countText;

	[SerializeField]
	private int m_maxDisplay = 10;


	//--- NonSerialized ---
	private int m_dangerTID;
	private int m_count;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int Count { get { return m_count; } }
	public bool DisplayCount { get { return m_displayCount; } }
	public int DangerTID { get { return m_dangerTID; } }
	public int MaxDisplay { get { return m_maxDisplay; } }
	public bool HasDangerEntry { get { return m_count > 1; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void AddDanger(int a_dangerTID)
	{
		m_dangerTID = a_dangerTID;
		m_count++;

		if (m_countText != null)
			m_countText.text = "x" + m_count;

		UIUtils.SetActive(gameObject, true);
	}

	public bool CanDisplayMore()
	{
		return m_displayCount || m_count < m_maxDisplay;
	}

	public void Clear()
	{
		m_dangerTID = tid.NULL;
		m_count = 0;
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