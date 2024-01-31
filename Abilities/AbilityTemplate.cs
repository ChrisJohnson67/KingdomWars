using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/AbilityTemplate")]
public class AbilityTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions
	public enum CombatTarget
	{
		Self,
		TargetedUnit,
		Monsters,
		Heroes,
		Enemies,
		Allies,
		All,
		None
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected float m_castTime = 0f;

	[SerializeField]
	protected float m_cooldown = 0f;

	[SerializeField]
	protected CombatTarget m_selectionTarget;

	[SerializeField]
	protected List<AbilityEffectTemplate> m_effects;

	[SerializeField]
	protected bool m_passive = false;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public float CastTime { get { return m_castTime; } }
	public List<AbilityEffectTemplate> Effects { get { return m_effects; } }
	public float Cooldown { get { return m_cooldown; } }
	public CombatTarget SelectionTarget { get { return m_selectionTarget; } }
	public bool Passive { get { return m_passive; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public static List<UnitInstance> GetTargets(UnitInstance a_source, AbilityTemplate.CombatTarget a_target)
	{
		List<UnitInstance> targets = new List<UnitInstance>();
		switch (a_target)
		{
			case CombatTarget.Self:
				if (a_source != null)
					targets.Add(a_source);
				break;

			case CombatTarget.TargetedUnit:
				if (a_source == CombatManager.Instance.HeroUnit)
				{
					targets.Add(CombatManager.Instance.GetHeroTargetedUnit());
				}
				else
				{
					targets.Add(CombatManager.Instance.HeroUnit);
				}
				break;

			case CombatTarget.Monsters:
				targets.AddRange(CombatManager.Instance.GetEnemies());
				break;

			case CombatTarget.Heroes:
				targets.AddRange(CombatManager.Instance.GetHeroes());
				break;

			case CombatTarget.Allies:
				if (a_source != null)
					targets.AddRange(CombatManager.Instance.GetAlliesFromUnit(a_source));
				break;

			case CombatTarget.Enemies:
				if (a_source != null)
					targets.AddRange(CombatManager.Instance.GetEnemiesFromUnit(a_source));
				break;

			case CombatTarget.All:
				targets.AddRange(CombatManager.Instance.GetAllUnits(false));
				break;
		}
		return targets;
	}

	public int GetFinalDamage(UnitInstance a_source, UnitInstance a_target, int a_damageTypeTID, float a_damage)
	{
		if (a_source != null)
		{
			//check if there is additional damage bonuses from buffs or talents
			int additionalDamage = a_source.CalculateAdditionalDamage(a_damageTypeTID, a_damage, a_target);
			return (int)(a_damage + additionalDamage);
		}
		return (int)a_damage;
	}

	public int GetFinalHeal(UnitInstance a_source, UnitInstance a_target, float a_heal)
	{
		if (a_source != null)
		{
			//check if there is additional damage bonuses from buffs or talents
			int addHeal = a_source.CalculateAdditionalHeal(a_heal, a_target);
			return (int)(a_heal + addHeal);
		}
		return (int)a_heal;
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
