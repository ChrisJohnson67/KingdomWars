using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LeaderboardEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class LeaderboardEntry : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_score;

	[SerializeField]
	private TMP_Text m_wins;

	[SerializeField]
	private TMP_Text m_losses;

	[SerializeField]
	private TMP_Text m_name;

	[SerializeField]
	private TMP_Text m_desc;

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

	public void Init(int a_score, string a_wins, string a_losses, string a_name, string a_desc)
	{
		if (m_score != null)
			m_score.text = a_score.ToString();

		if (m_wins != null)
			m_wins.text = a_wins;

		if (m_losses != null)
			m_losses.text = a_losses;

		if (m_name != null)
			m_name.text = a_name;

		if (m_desc != null)
			m_desc.text = a_desc;
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