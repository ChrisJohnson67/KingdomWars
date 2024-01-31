using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LevelListEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class LevelListEntry : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_nameText;

	[SerializeField]
	private TMP_Text m_authorText;

	[SerializeField]
	private TMP_Text m_winsText;

	[SerializeField]
	private TMP_Text m_lossesText;

	[SerializeField]
	private DangerDisplay m_dangerDisplay;

	[SerializeField]
	private GameObject m_bonusRatingObj;

	[SerializeField]
	private GameObject m_difficultObj;


	//--- NonSerialized ---
	private KingdomData m_kingdomData;
	private LevelListPanel m_parent;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(LevelListPanel a_parent, KingdomData a_data)
	{
		m_kingdomData = a_data;
		m_parent = a_parent;

		m_nameText.text = a_data.Title;
		m_authorText.text = a_data.Author;

		string winKingdom = "Kingdom Victories: ";
		string lossKingdom = "Kingdom Defeats: ";

		m_winsText.text = winKingdom + a_data.Wins.ToString();
		m_lossesText.text = lossKingdom + a_data.Losses.ToString();

		UIUtils.SetActive(m_bonusRatingObj, a_data.Losses == 0);

		bool winThreshold = (a_data.Losses == 0 && a_data.Wins > 2) || (a_data.Losses > 0 && a_data.Wins / a_data.Losses >= 3f);
		UIUtils.SetActive(m_difficultObj, winThreshold);

		if (m_dangerDisplay != null)
		{
			m_dangerDisplay.InitList(m_kingdomData.GetDangers());
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		m_parent.OnLevelSelected(m_kingdomData);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}