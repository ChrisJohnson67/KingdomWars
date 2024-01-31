using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RaycastTargetHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public static class RaycastTargetHelper2D
{
	//~~~~~ Variables ~~~~~
	#region Variables

	private static RaycastHit2D[] sm_raycastHits = new RaycastHit2D[4];
	private static Collider2D[] sm_circleColliderHits = new Collider2D[8];

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public static T GetTarget2D<T>(Vector2 a_position, string a_tag) where T : Component
	{
		var hits = Physics2D.RaycastNonAlloc(a_position, Vector2.zero, sm_raycastHits);
		for (int i = 0; i < hits; i++)
		{
			var data = sm_raycastHits[i];
			if (data.transform.gameObject.CompareTag(a_tag))
			{
				var component = data.transform.GetComponentInParent<T>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static List<T> GetTargetsInCircle2D<T>(Vector2 a_position, float a_radius, string a_tag) where T : Component
	{
		List<T> targetsHit = new List<T>();
		var hits = Physics2D.OverlapCircleNonAlloc(a_position, a_radius, sm_circleColliderHits);
		for (int i = 0; i < hits; i++)
		{
			var data = sm_circleColliderHits[i];
			if (data.gameObject.CompareTag(a_tag))
			{
				var component = data.gameObject.GetComponentInParent<T>();
				if (component != null)
				{
					targetsHit.Add(component);
				}
			}
		}
		return targetsHit;
	}

	#endregion Runtime Functions
}