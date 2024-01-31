using Platform.ReqResults;
using System;

[Serializable]
public class CombatData : EventData
{
	private AbilityContextData m_contextData;

	private UnitInstance m_target;

	public AbilityContextData ContextData { get { return m_contextData; } }
	public UnitInstance Target { get { return m_target; } }


	public CombatData(AbilityContextData a_contextData, UnitInstance a_target) : base()
	{
		m_contextData = a_contextData;
		m_target = a_target;
	}
}
