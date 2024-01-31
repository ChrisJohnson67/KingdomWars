//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitAI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public abstract class UnitAI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected UnitInstance m_unitInstance;
	protected bool m_active = false;
	protected float m_autoAttackTimer = 0f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public UnitInstance UnitInstance { get { return m_unitInstance; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public UnitAI(UnitInstance a_unit)
	{
		m_unitInstance = a_unit;
		ResetAutoAttackTimer(true);
	}

	public void ActivateAI(bool a_activate)
	{
		m_active = a_activate;
		m_autoAttackTimer = 0f;
		if (!m_active)
		{
			ResetAutoAttackTimer();
		}

		foreach (var ability in m_unitInstance.Template.StartAbilities)
		{
			m_unitInstance.CreateAbility(ability.TID, false, true);
		}
	}

	public virtual void Update(float a_deltaTime)
	{
		if (m_active)
		{
			m_autoAttackTimer -= a_deltaTime;
			if (m_autoAttackTimer <= 0f)
			{
				ExecuteAutoAttack();
				ResetAutoAttackTimer();
			}
		}
	}

	protected void ResetAutoAttackTimer(bool a_randomize = false)
	{
		m_autoAttackTimer += 1f / m_unitInstance.CurrentStats.GetCurrentAmountOfStat(StatSettings.AttackSpeedStatTID);
		if (a_randomize)
		{
			m_autoAttackTimer = RandomHelpers.GetRandomValue(m_autoAttackTimer / 2f, m_autoAttackTimer);
		}
	}

	protected virtual void ExecuteAutoAttack()
	{
		if (m_unitInstance.CanCastAbility(true))
		{
			m_unitInstance.CreateAbility(m_unitInstance.Template.AutoAttackAbility, true, true);
			m_unitInstance.Model.PlayAttackAnim();
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
