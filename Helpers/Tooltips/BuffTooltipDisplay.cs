//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTooltipDisplay
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffTooltipDisplay : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---


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

	public override void Init(DisplayTemplate a_template)
	{
		base.Init(a_template);

		if (m_descText != null)
			m_descText.text = m_template.GetTooltipDesc();
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