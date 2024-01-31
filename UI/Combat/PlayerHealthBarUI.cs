//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// PlayerHealthbarUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using TMPro;
using UnityEngine;

public class PlayerHealthBarUI : HealthbarUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---

	[SerializeField]
	private TMP_Text m_healthText;


	//--- NonSerialized ---


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	protected override void UpdatePosition()
	{
	}

	protected override void UpdateFill()
	{
		base.UpdateFill();

		m_healthText.text = Mathf.Max(0f, m_unitInstance.CurrentHealth) + " / " + m_unitInstance.MaxHealth;
	}

	#endregion Runtime Functions
}
