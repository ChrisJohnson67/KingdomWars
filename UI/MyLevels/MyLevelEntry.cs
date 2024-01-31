using Platform.UIManagement;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// MyLevelEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class MyLevelEntry : MonoBehaviour
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


	//--- NonSerialized ---
	private KingdomData m_kingdomData;
	private MyLevelsUI m_parent;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(MyLevelsUI a_parent, KingdomData a_data)
	{
		m_kingdomData = a_data;
		m_parent = a_parent;

		m_nameText.text = a_data.Title;
		m_authorText.text = a_data.Author;

		string winKingdom = "Kingdom Victories: ";
		string lossKingdom = "Kingdom Defeats: ";

		m_winsText.text = winKingdom + a_data.Wins.ToString();
		m_lossesText.text = lossKingdom + a_data.Losses.ToString();

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
		GenericPopupUI.CreatePopup("Would you like to playtest your level?", OnPlaytestLevel, OnNoPlaytest);
	}

	private void OnNoPlaytest()
	{

	}

	private void OnPlaytestLevel()
	{
		m_parent.OnClose();
		var titleScreen = UIManager.Instance.GetUI<TitleScreenUI>();
		if (titleScreen != null)
			titleScreen.CloseUI();

		var ui = UIManager.Instance.OpenUI<LevelSelectUI>(GameManager.Instance.UISettings.LevelSelectUITID);
		ui.InitAsPlaytest(m_kingdomData);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}