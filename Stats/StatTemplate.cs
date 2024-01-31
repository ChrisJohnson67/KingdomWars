using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// StatTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/StatTemplate")]
public class StatTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private bool m_onlyPositive = true;

	[SerializeField]
	private bool m_multiplyInUIBy100 = false;

	[SerializeField]
	private bool m_showDecimal = false;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public bool OnlyPositive { get { return m_onlyPositive; } }
	public bool MultiplyInUI { get { return m_multiplyInUIBy100; } }
	public bool ShowDecimal { get { return m_showDecimal; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public StatInstance CreateStatInstance(float a_baseAmount)
	{
		return new StatInstance(this, a_baseAmount);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
