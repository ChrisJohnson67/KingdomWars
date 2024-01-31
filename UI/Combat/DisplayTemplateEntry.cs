using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DisplayTemplateEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DisplayTemplateEntry : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected Image m_icon;

	[SerializeField]
	protected TMP_Text m_nameText;

	[SerializeField]
	protected TMP_Text m_descText;

	//--- NonSerialized ---
	protected DisplayTemplate m_template;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public virtual void Init(DisplayTemplate a_template)
	{
		m_template = a_template;

		if (m_template != null)
		{
			if (m_icon != null)
				m_icon.sprite = m_template.DisplayIcon;

			if (m_nameText != null)
				m_nameText.text = m_template.DisplayName;

			if (m_descText != null)
				m_descText.text = m_template.Description;
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public virtual void OnTooltipInfo(TooltipInfo a_info)
	{
		if (m_template != null)
		{
			a_info.Name = m_template.DisplayName;
			a_info.Desc = m_template.GetTooltipDesc();
			a_info.BuffTemplates = m_template.BuffsToDisplay;
		}
		else
		{
			a_info.DontShow = true;
		}
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}