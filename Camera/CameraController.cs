using System;
using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CameraController
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class CameraController : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_movementTransform;

	[SerializeField]
	private Camera m_camera;

	[SerializeField]
	private float m_moveTime;

	[SerializeField]
	private float m_zoomTime;

	[SerializeField]
	private float m_zoomSpeed;

	[SerializeField]
	private SpecificSoundGroup m_soundMove;

	//--- NonSerialized ---
	private Transform m_followTransform;
	private bool m_zooming = false;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Camera Camera { get { return m_camera; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages
	private void OnDisable()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void SetFollowTransform(Transform a_transform)
	{
		m_followTransform = a_transform;
	}

	private void Update()
	{
		if (m_followTransform == null || m_zooming)
			return;

		m_movementTransform.position = m_followTransform.position;
	}

	public void TranslateCamera(Vector3 a_position, Action a_onComplete)
	{
		MoveTo(m_movementTransform.position + a_position, a_onComplete);
	}

	public void MoveTo(Vector3 a_position, Action a_onComplete)
	{
		if (m_soundMove != null)
			m_soundMove.Play();

		StartCoroutine(MoveCR(a_position, a_onComplete));
	}

	private IEnumerator MoveCR(Vector3 a_position, Action a_onComplete)
	{
		var timer = 0f;
		var startPosition = m_movementTransform.position;
		var endPosition = a_position;
		while (timer < m_moveTime)
		{
			var time = timer / m_moveTime;
			float newX = Mathf.SmoothStep(startPosition.x, endPosition.x, time);
			float newY = Mathf.SmoothStep(startPosition.y, endPosition.y, time);
			float newZ = Mathf.SmoothStep(startPosition.z, endPosition.z, time);
			m_movementTransform.position = new Vector3(newX, newY, newZ);
			yield return null;
			timer += Time.deltaTime;
		}
		m_movementTransform.position = endPosition;
		a_onComplete?.Invoke();
	}

	public void ZoomOut(Action a_onComplete)
	{
		m_zooming = true;
		if (m_soundMove != null)
			m_soundMove.Play();
		StartCoroutine(ZoomOutCR(a_onComplete));
	}


	public IEnumerator ZoomOutCR(Action a_onComplete)
	{
		var timer = 0f;
		var direction = m_camera.transform.forward;
		while (timer < m_zoomTime)
		{
			transform.position -= direction * m_zoomSpeed * Time.deltaTime;
			yield return null;
			timer += Time.deltaTime;
		}
		a_onComplete?.Invoke();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
