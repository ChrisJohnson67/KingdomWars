using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BaseRoomModel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BaseRoomModel : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected Transform m_playerSpawnTransform;

	[SerializeField]
	protected Transform m_startConnectorTransform;

	[SerializeField]
	protected Transform m_endConnectorTransform;

	[SerializeField]
	protected List<Transform> m_waypoints;

	//--- NonSerialized ---
	protected Action m_onComplete;
	protected int m_currentWaypoint;
	protected UnitInstance m_unit;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Transform StartConnectorTransform { get { return m_startConnectorTransform; } }
	public Transform EndConnectorTransform { get { return m_endConnectorTransform; } }
	public Transform PlayerSpawn { get { return m_playerSpawnTransform; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void MovePlayerThroughWaypoints(UnitInstance a_unit, Action a_onComplete)
	{
		m_onComplete = a_onComplete;
		m_unit = a_unit;
		MoveToNextWaypoint();

	}

	private void MoveToNextWaypoint()
	{
		if (m_currentWaypoint < m_waypoints.Count)
		{
			var nextWaypoint = m_waypoints[m_currentWaypoint];
			m_unit.Model.MoveToPosition(nextWaypoint.position, OnWaypointComplete);
		}
		else
		{
			m_onComplete?.Invoke();
		}
	}

	private void OnWaypointComplete()
	{
		m_currentWaypoint++;
		MoveToNextWaypoint();
	}

	public virtual void Cleanup()
	{

	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	#endregion Callbacks

}