using System;
using System.Collections.Generic;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ApplyAreaDamageEffectTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable, CreateAssetMenu(menuName = "Effects/FireProjectileAtTargetEffectTemplate")]
public class FireProjectileAtTargetEffectTemplate : DamageEffectTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected int m_speed;

	[SerializeField]
	protected List<AbilityEffectTemplate> m_effectsAppliedOnHit = new List<AbilityEffectTemplate>();

	[SerializeField]
	protected ProjectileDisplay m_projectileObject;

	[SerializeField]
	protected float m_travellingDistance = -1f;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int Speed { get { return m_speed; } }
	public ProjectileDisplay ProjectileObject { get { return m_projectileObject; } }
	public List<AbilityEffectTemplate> EffectsAppliedOnHit { get { return m_effectsAppliedOnHit; } }
	public float TravellingDistance { get { return m_travellingDistance; } }
	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override AbilityEffectInstance CreateInstance(AbilityContextData a_context)
	{
		return new FireProjectileAtTargetEffectInstance(this, a_context);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
