using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RoomChoiceCombatEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RoomChoiceCombatEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Vector2 m_gemSize;

	//--- NonSerialized ---
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

	public void Init(RoomChoiceTemplate a_choiceTemplate)
	{
		m_choiceTemplate = a_choiceTemplate;

		base.Init(m_choiceTemplate);
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

	public override void OnTooltipInfo(TooltipInfo a_info)
	{
		a_info.Name = m_choiceTemplate.DisplayName;
		a_info.Desc = m_choiceTemplate.Description;
		a_info.BuffTemplates = m_choiceTemplate.BuffsToDisplay;
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}