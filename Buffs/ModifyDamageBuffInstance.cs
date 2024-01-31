//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ModifyDamageBuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class ModifyDamageBuffInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public ModifyDamageBuffTemplate DamageTemplate { get { return m_template as ModifyDamageBuffTemplate; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public ModifyDamageBuffInstance(ModifyDamageBuffTemplate a_template, BuffContextData a_context) : base(a_template, a_context)
	{
	}

	public abstract int CalculateAdditionalDamage(int a_damageTypeTID, float a_damage, UnitInstance a_source, UnitInstance a_target);

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}

