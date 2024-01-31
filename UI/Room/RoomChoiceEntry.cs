using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomChoiceEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomChoiceEntry : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_name;

	[SerializeField]
	private Image m_icon;

	[SerializeField]
	private TMP_Text m_description;

	[SerializeField]
	private DangerDisplay m_dangerDisplay;

	[SerializeField]
	private GameObject m_selectedObj;

	[SerializeField]
	private Vector2 m_gemSize;

	//--- NonSerialized ---
	private RoomCreationUI m_parent;
	private RoomChoiceTemplate m_choiceTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public RoomChoiceTemplate ChoiceTemplate { get { return m_choiceTemplate; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(RoomCreationUI a_parent, RoomChoiceTemplate a_choiceTemplate)
	{
		m_parent = a_parent;
		m_choiceTemplate = a_choiceTemplate;

		m_name.SetText(m_choiceTemplate.DisplayName);
		m_description.SetText(m_choiceTemplate.Description);
		m_icon.sprite = m_choiceTemplate.DisplayIcon;

		List<int> dangers = new List<int>();
		m_choiceTemplate.GetDangers(dangers);

		dangers.Sort(SortDangers);
		m_dangerDisplay.InitList(dangers, true);
	}

	private int SortDangers(int a_left, int a_right)
	{
		return a_right.CompareTo(a_left);
	}

	public void SetSelected(bool a_sel)
	{
		UIUtils.SetActive(m_selectedObj, a_sel);
	}

	public void SetGemIconSize()
	{
		m_icon.rectTransform.sizeDelta = m_gemSize;
	}

	public void Cleanup()
	{

	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		if (m_parent != null)
		{
			m_parent.ChoseChoice(this);
		}
	}

	public void Ontooltip(TooltipInfo a_info)
	{
		a_info.DontShow = m_choiceTemplate.BuffsToDisplay.Count == 0;
		a_info.BuffTemplates = m_choiceTemplate.BuffsToDisplay;
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}