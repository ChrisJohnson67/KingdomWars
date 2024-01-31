using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ColldierHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ColliderHelper3D : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private Collider m_collider;

	private Action<UnitInstance> m_onTriggerEnter;
	private Action<UnitInstance> m_onTriggerExit;
	private Action<UnitInstance> m_onCollisionEnter;
	private Action<UnitInstance> m_onCollisionExit;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Action<UnitInstance> a_onTriggerEnter, Action<UnitInstance> a_onTriggerExit, float a_radius = -1f)
	{
		m_onTriggerEnter = a_onTriggerEnter;
		m_onTriggerExit = a_onTriggerExit;
		var sphere = m_collider as SphereCollider;
		if (sphere != null && a_radius != -1f)
		{
			sphere.radius = a_radius;
		}
	}

	public void InitCollision(Action<UnitInstance> a_onCollisionEnter, Action<UnitInstance> a_onCollisionExit, float a_radius = -1f)
	{
		m_onCollisionEnter = a_onCollisionEnter;
		m_onCollisionExit = a_onCollisionExit;
		var sphere = m_collider as SphereCollider;
		if (sphere != null && a_radius != -1f)
		{
			sphere.radius = a_radius;
		}
	}

	private void OnTriggerEnter(Collider a_collider)
	{
		var unit = a_collider.gameObject.GetComponentInParent<UnitModel>();
		if (unit != null)
		{
			UnitCollided(unit.UnitInstance);
		}
	}

	private void UnitCollided(UnitInstance a_unit)
	{
		m_onTriggerEnter?.Invoke(a_unit);
	}

	private void OnTriggerExit(Collider a_collider)
	{
		var unit = a_collider.gameObject.GetComponentInParent<UnitModel>();
		if (unit != null)
		{
			m_onTriggerExit?.Invoke(unit.UnitInstance);
		}
	}

	private void OnCollisionEnter(Collision a_other)
	{
		if (a_other.gameObject.CompareTag(GameManager.c_UnitTag))
		{
			var unit = a_other.gameObject.GetComponent<UnitModel>();
			if (unit != null)
			{
				m_onCollisionEnter?.Invoke(unit.UnitInstance);
			}
		}
	}

	private void OnCollisionExit(Collision a_other)
	{
		if (a_other.gameObject.CompareTag(GameManager.c_UnitTag))
		{
			var unit = a_other.gameObject.GetComponent<UnitModel>();
			if (unit != null)
			{
				m_onCollisionExit?.Invoke(unit.UnitInstance);
			}
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR

	private void OnValidate()
	{
		if (m_collider == null)
			m_collider = GetComponent<Collider>();
	}

#endif
}
