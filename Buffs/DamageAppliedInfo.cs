using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DamageAppliedInfo
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public struct DamageAppliedInfo
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public struct DamageInfoType
	{
		public int DamageAmount;
		public int DamageType;
		public bool Crit;

		public DamageInfoType(int a_amt, int a_damageType, bool a_crit)
		{
			DamageAmount = a_amt;
			DamageType = a_damageType;
			Crit = a_crit;
		}

		public void SetAmt(int a_amt)
		{
			DamageAmount = a_amt;
		}
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private List<DamageInfoType> m_damageTypes;
	public bool ApplyOnDamageTrigger;

	private static DamageAppliedInfo sm_sharedDamageInfo;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public List<DamageInfoType> DamageTypes { get { return m_damageTypes; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public static DamageAppliedInfo GetDamageInfo(bool a_applyOnDamageTrigger)
	{
		sm_sharedDamageInfo.Clear();
		sm_sharedDamageInfo.ApplyOnDamageTrigger = a_applyOnDamageTrigger;
		return sm_sharedDamageInfo;
	}

	private void Clear()
	{
		if (m_damageTypes != null)
		{
			m_damageTypes.Clear();
		}
		else
		{
			m_damageTypes = new List<DamageInfoType>();
		}
	}

	public void AddDamage(int a_damageType, int a_damageAmt, bool a_crit)
	{
		if (a_damageAmt == 0)
			return;

		if (m_damageTypes == null)
			m_damageTypes = new List<DamageInfoType>();

		m_damageTypes.Add(new DamageInfoType(a_damageAmt, a_damageType, a_crit));
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
