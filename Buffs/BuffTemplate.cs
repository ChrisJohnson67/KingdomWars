using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class BuffTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_StacksMadlib = "$STACK$";

	public enum BuffTrigger
	{
		Immediate,
		OnDamage,
		OnDeath,
		OnHeal,
		OnStackAmount,
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected bool m_canReapplyBuff = true;

	[SerializeField]
	protected bool m_canStack = false;

	[SerializeField]
	protected BuffTrigger m_trigger = BuffTrigger.Immediate;

	[SerializeField]
	protected int m_stackAmountTrigger = 0;

	[SerializeField]
	protected bool m_showInUI = true;

	[SerializeField]
	private bool m_isDebuff;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public BuffTrigger Trigger { get { return m_trigger; } }
	public bool CanReapplyBuff { get { return m_canReapplyBuff; } }
	public bool CanStack { get { return m_canStack; } }
	public int StackAmountTrigger { get { return m_stackAmountTrigger; } }
	public bool ShowInUI { get { return m_showInUI; } }
	public bool IsDebuff { get { return m_isDebuff; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public abstract BuffInstance CreateBuffInstance(BuffContextData a_context);

	//public virtual string PopulateMadlibs(AbilityTemplate a_abilityTemplate, UnitInstance a_source, int a_rarityLevel, string a_text)
	//{
	//	a_text = UISettings.InsertMadlib(a_text, c_StacksMadlib, m_stackAmountTrigger);
	//	return a_text;
	//}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
