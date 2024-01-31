using System;
using UnityEngine;
using UnityEngine.Events;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ColliderHelper2D
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[RequireComponent(typeof(Collider2D))]
public class ColliderHelper2D : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	[Serializable]
	public class UnitEvent : UnityEvent<UnitInstance> { }

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, Tag]
	private string m_tagToCompare;

	[SerializeField]
	private UnitEvent m_onTriggerEnter;

	[SerializeField]
	private UnitEvent m_onTriggerExit;

	[SerializeField]
	private UnitEvent m_onCollisionEnter;

	[SerializeField]
	private UnitEvent m_onCollisionExit;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void OnTriggerEnter2D(Collider2D a_collider)
	{
		CheckObjectForEvent(a_collider.gameObject, m_onTriggerEnter);
	}

	private void OnTriggerExit2D(Collider2D a_collider)
	{
		CheckObjectForEvent(a_collider.gameObject, m_onTriggerExit);
	}

	private void OnCollisionEnter2D(Collision2D a_other)
	{
		CheckObjectForEvent(a_other.gameObject, m_onCollisionEnter);
	}

	private void OnCollisionExit2D(Collision2D a_other)
	{
		CheckObjectForEvent(a_other.gameObject, m_onCollisionExit);
	}

	private void CheckObjectForEvent(GameObject a_object, UnityEvent<UnitInstance> a_callback)
	{
		if (a_object == null)
			return;

		if (a_object.CompareTag(m_tagToCompare))
		{
			var unit = a_object.GetComponent<UnitModel>();
			if (unit != null)
			{
				a_callback?.Invoke(unit.UnitInstance);
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
	}

#endif
}
