using System;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ProjectileDisplay
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class CollisionTriggerHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private Action<Collider> m_onTrigger;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Attach(Action<Collider> a_onTrigger)
	{
		m_onTrigger = a_onTrigger;
	}
	private void OnTriggerEnter(Collider a_collider)
	{
		m_onTrigger?.Invoke(a_collider);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
