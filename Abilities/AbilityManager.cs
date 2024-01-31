using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AbilityManager
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private List<AbilityInstance> m_abilities = new List<AbilityInstance>();


	private static AbilityManager sm_instance;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	public static AbilityManager Instance { get { return sm_instance; } }
	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AbilityManager()
	{
		sm_instance = this;
	}

	public void Update(float a_deltaTime)
	{
		for (int i = m_abilities.Count - 1; i >= 0; i--)
		{
			var ability = m_abilities[i];
			ability.Update(a_deltaTime);
			if (ability.IsCompleted)
			{
				ability.Cleanup();
				m_abilities.RemoveAt(i);
			}
		}
	}

	public void Cleanup()
	{
		foreach (var ability in m_abilities)
		{
			ability.Cleanup();
		}
		sm_instance = null;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
