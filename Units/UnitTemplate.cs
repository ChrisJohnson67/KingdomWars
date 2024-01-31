using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/UnitTemplate")]
public class UnitTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(UnitModel), "Model", "")]
	private int m_unitModelTID;

	[SerializeField]
	private int m_unlockCost;

	[SerializeField, TemplateIDField(typeof(AbilityTemplate), "Auto Attack", "")]
	private int m_autoAttackAbility;

	[SerializeField]
	private List<AbilityTemplate> m_startAbilities;

	[SerializeField]
	private List<AbilityTemplate> m_abilities;

	[SerializeField]
	private UnitStatTemplate m_unitStatTemplate;

	[SerializeField]
	private AITemplate m_aiTemplate;

	[SerializeField]
	private int m_abilityCharges = 5;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int UnitModel { get => m_unitModelTID; }
	public int UnlockCost { get { return m_unlockCost; } }
	public int AutoAttackAbility { get { return m_autoAttackAbility; } }
	public UnitStatTemplate UnitStatTemplate { get { return m_unitStatTemplate; } }
	public List<AbilityTemplate> StartAbilities { get { return m_startAbilities; } }
	public List<AbilityTemplate> Abilities { get { return m_abilities; } }
	public AITemplate AITemplate { get { return m_aiTemplate; } }
	public int AbilityCharges { get { return m_abilityCharges; } }
	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
