//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ImmunetoDebuffsBuffInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ImmunetoDebuffsBuffInstance : BuffInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private ImmuneToDebuffsBuffTemplate m_statTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public ImmunetoDebuffsBuffInstance(ImmuneToDebuffsBuffTemplate a_template, BuffContextData a_data) : base(a_template, a_data)
	{
		m_statTemplate = a_template;
	}

	protected override void ApplyBuff()
	{
		base.ApplyBuff();

		m_context.Target.SetImmuneToDebuffs(true);
	}

	public override void RemoveBuff()
	{
		base.RemoveBuff();
		m_context.Target.SetImmuneToDebuffs(false);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
