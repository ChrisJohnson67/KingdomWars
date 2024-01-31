using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatSettings
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Settings related to the UI.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CombatSettings")]
public class CombatSettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables


	[SerializeField, TemplateIDField(typeof(BuffTemplate), "Stunned Buff", "")]
	private int m_stunnedBuff;

	[SerializeField]
	private TagTemplate m_allyParty;

	[SerializeField]
	private TagTemplate m_enemyParty;

	[SerializeField, TemplateIDField(typeof(TagTemplate), "Phys Damage Type", "")]
	private int m_physDamageTypeTID;

	[SerializeField]
	private float m_unitMoveSpeed = 5f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int StunnedBuff { get { return m_stunnedBuff; } }
	public int AllyPartyTID { get { return m_allyParty.TID; } }
	public int EnemyPartyTID { get { return m_enemyParty.TID; } }
	public float UnitMoveSpeed { get { return m_unitMoveSpeed; } }
	public int PhysDamageTypeTID { get { return m_physDamageTypeTID; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	#endregion Runtime Functions
}
