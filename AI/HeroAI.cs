//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HeroAI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HeroAI : UnitAI
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

	public HeroAI(UnitInstance a_unit) : base(a_unit)
	{
	}


	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);
		if (m_active)
		{
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
