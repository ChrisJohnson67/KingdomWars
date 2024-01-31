using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ProjectileDisplay
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ProjectileDisplay : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private CollisionTriggerHelper m_triggerHelper;

	private int m_teamTag;
	private UnitInstance m_target;
	private int m_speed;
	private Vector3 m_initialDirection;
	public Action<ProjectileDisplay, UnitInstance> m_onHit;
	private bool m_tagged = false;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Action<ProjectileDisplay, UnitInstance> a_onHit, int a_speed, Vector3 a_direction, int a_teamTag, UnitInstance a_target)
	{
		m_teamTag = a_teamTag;
		m_onHit = a_onHit;
		m_speed = a_speed;
		m_target = a_target;
		m_initialDirection = a_direction;
		m_triggerHelper.Attach(TriggerEnter);
	}

	private void FixedUpdate()
	{
		if (m_target != null && m_target.IsDead)
		{
			return;
		}
		Vector3 direction = m_initialDirection;
		if (m_target != null && m_target.Model != null)
		{
			//lock onto the target
			direction = (m_target.Model.AttackNode.position - transform.position).normalized;
		}
		transform.position += Time.fixedDeltaTime * m_speed * direction;
		transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
	}

	private void TriggerEnter(Collider a_collider)
	{
		if (m_tagged)
			return;

		if (a_collider.gameObject.CompareTag("Unit"))
		{
			var unit = a_collider.gameObject.GetComponentInParent<UnitModel>();
			bool hitTarget = m_target != null && unit != null && !unit.UnitInstance.IsDead && unit.UnitInstance == m_target;
			if (hitTarget || (unit != null && !unit.UnitInstance.IsDead && !unit.UnitInstance.IsUnitOnSameTeam(m_teamTag)))
			{
				m_onHit?.Invoke(this, unit.UnitInstance);
				m_tagged = true;
			}
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (m_triggerHelper == null)
		{
			m_triggerHelper = GetComponentInChildren<CollisionTriggerHelper>();
		}
	}
#endif

	#endregion Callbacks

}
