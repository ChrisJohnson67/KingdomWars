using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ItemCombatEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ItemCombatEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GlowButton m_button;

	[SerializeField]
	private GameObject m_disableObject;


	//--- NonSerialized ---
	private AbilityTemplate m_abilityTemplate;
	private CombatUI m_parent;
	private bool m_used = false;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(CombatUI a_parent, AbilityTemplate a_ability)
	{
		base.Init(a_ability);
		m_abilityTemplate = a_ability;
		m_parent = a_parent;
	}

	protected void Refresh()
	{
		bool inCombat = CombatManager.Instance.InRoomCombatState;
		m_button.SetInteractive(!m_used && inCombat);
		UIUtils.SetActive(m_disableObject, m_used || !inCombat);
		UIUtils.SetActive(m_icon, !m_used);
	}

	public void SetInRoomCombat(bool a_inCombat)
	{
		Refresh();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		if (!m_used)
		{
			m_parent.UseItemAbility(m_abilityTemplate);
			m_used = true;
			Refresh();
		}
	}

	public override void OnTooltipInfo(TooltipInfo a_info)
	{
		base.OnTooltipInfo(a_info);
		a_info.DontShow = m_used;
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}