using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu, Serializable]
public class StatSettings : TemplateObject
{
	[Serializable]
	public class DamageDefenseType
	{
		[SerializeField]
		private TagTemplate m_damageType;

		[SerializeField, TemplateIDField(typeof(StatTemplate), "Defense Stat", "")]
		private int m_defenseStat;

		public TagTemplate DamageType { get { return m_damageType; } }
		public int DefenseStat { get { return m_defenseStat; } }
	}

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Auto Attack", "")]
	private int m_autoAttackDamageStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Attack Speed", "")]
	private int m_attackSpeedStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Max Health", "")]
	private int m_maxHealthStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Armor Stat", "")]
	private int m_armorStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Fire Res", "")]
	private int m_fireResStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Ice Res", "")]
	private int m_iceResStat;

	[SerializeField, TemplateIDField(typeof(StatTemplate), "Nature Res", "")]
	private int m_natureResStat;

	[SerializeField]
	private List<DamageDefenseType> m_damageDefenseTypes = new List<DamageDefenseType>();

	public int AutoAttackDamageStat { get => m_autoAttackDamageStat; }
	public int AttackSpeedStat { get => m_attackSpeedStat; }
	public int MaxHealthStat { get => m_maxHealthStat; }
	public int ArmorStat { get => m_armorStat; }
	public int FireResStat { get => m_fireResStat; }
	public int IceResStat { get => m_iceResStat; }
	public int NatureResStat { get => m_natureResStat; }

	public static int AttackStatTID { get { return GameManager.Instance.StatSettings.AutoAttackDamageStat; } }
	public static int AttackSpeedStatTID { get { return GameManager.Instance.StatSettings.AttackSpeedStat; } }
	public static int MaxHealthStatTID { get { return GameManager.Instance.StatSettings.MaxHealthStat; } }
	public static int ArmorStatTID { get { return GameManager.Instance.StatSettings.ArmorStat; } }
	public static int FireResTID { get { return GameManager.Instance.StatSettings.FireResStat; } }
	public static int IceResTID { get { return GameManager.Instance.StatSettings.IceResStat; } }
	public static int NatureResTID { get { return GameManager.Instance.StatSettings.NatureResStat; } }

	public int GetDefenseStatForDamage(int a_damageType)
	{
		foreach (var type in m_damageDefenseTypes)
		{
			if (type.DamageType.TID == a_damageType)
			{
				return type.DefenseStat;
			}
		}
		return tid.NULL;
	}
}
