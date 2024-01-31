using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// FireProjectileAtTargetEffectInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class FireProjectileAtTargetEffectInstance : DamageEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private List<ProjectileDisplay> m_projectiles = new List<ProjectileDisplay>();
	private Vector3 m_startPosition;
	private FireProjectileAtTargetEffectTemplate m_projectileTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public FireProjectileAtTargetEffectTemplate ProjectileDamageTemplate { get { return m_template as FireProjectileAtTargetEffectTemplate; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public FireProjectileAtTargetEffectInstance(FireProjectileAtTargetEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
	}

	public override void StartEffect()
	{
		base.StartEffect();

		m_startPosition = m_context.Source.Position;

		//create preview display
		m_projectileTemplate = ProjectileDamageTemplate;

		foreach (var target in m_context.Targets)
		{
			if (!m_context.Source.IsDead)
			{
				var projectile = GameObject.Instantiate<ProjectileDisplay>(m_projectileTemplate.ProjectileObject, GameManager.Instance.FXParent);
				projectile.Init(OnProjectileHit, m_projectileTemplate.Speed, target.Model.AttackNode.position - m_context.Source.Model.ProjectileNode.position, m_context.Source.AlignmentTag, target);
				m_projectiles.Add(projectile);

				projectile.transform.position = m_context.Source.Model.AttackNode.position;
			}
		}
		CheckToCompleteEffect();
	}

	public void OnProjectileHit(ProjectileDisplay a_projectile, UnitInstance a_unit)
	{
		if (a_unit != null)
		{
			var dmg = m_context.AbilityInstance.Template.GetFinalDamage(m_context.Source, m_context.Targets[0], m_projectileTemplate.Damage.DamageTypeTID, m_projectileTemplate.Damage.GetDamage(m_context.Source, m_context.Targets[0]));
			if (dmg > 0)
			{
				var dmgInfo = new DamageAppliedInfo();
				dmgInfo.AddDamage(dmg, m_damageTemplate.Damage.DamageTypeTID, false);
				a_unit.ApplyDamage(m_context.Source, dmgInfo);
			}
		}

		RemoveProjectile(a_projectile);

		AbilityContextData hitContext = new AbilityContextData(m_context);
		hitContext.Targets.Clear();
		hitContext.Targets.Add(a_unit);
		foreach (var effect in m_projectileTemplate.EffectsAppliedOnHit)
		{
			var effectInstance = effect.CreateInstance(hitContext);
			hitContext.AbilityInstance.AddEffect(effectInstance);
		}
		CheckToCompleteEffect();
	}

	private void RemoveProjectile(ProjectileDisplay a_projectile)
	{
		if (m_projectiles.Contains(a_projectile))
			m_projectiles.Remove(a_projectile);

		if (a_projectile != null)
			GameManager.Instance.DeleteObject(a_projectile.gameObject);
	}

	private void CheckToCompleteEffect()
	{
		if (m_projectiles.Count == 0)
		{
			CompleteEffect();
		}
	}

	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);

		foreach (var proj in m_projectiles)
		{
			bool projectileOutOfRange = proj != null && ((m_projectileTemplate.TravellingDistance > 0f &&
															Vector3.Distance(proj.transform.position, m_startPosition) > m_projectileTemplate.TravellingDistance));
			if (projectileOutOfRange ||
				(m_context.Targets.Count == 0 && m_context.Targets[0].IsDead))
			{
				RemoveProjectile(proj);
			}
		}
		CheckToCompleteEffect();
	}

	protected override void CompleteDamageEffect()
	{
	}

	public override void Cleanup()
	{
		base.Cleanup();

		foreach (var proj in m_projectiles)
		{
			if (proj != null)
				GameManager.Instance.DeleteObject(proj.gameObject);
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
