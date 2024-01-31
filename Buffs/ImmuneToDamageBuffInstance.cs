//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ImmuneToDamageBuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ImmuneToDamageBuffInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public ImmuneToDamageBuffInstance(ImmuneToDamageBuffTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
	}

	protected override void ApplyBuff()
	{
		base.ApplyBuff();

		m_context.Target.SetImmuneToDmg(true);
	}

	public override void RemoveBuff()
	{
		base.RemoveBuff();
		m_context.Target.SetImmuneToDmg(false);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
