using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// TransformAnimHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class TransformAnimHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_verticalFloatRange;

	[SerializeField]
	private float m_verticalFloatTime;

	[SerializeField]
	private Vector3 m_rotationSpeed;

	[SerializeField]
	private bool m_UI;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
		if (m_verticalFloatTime > 0f)
		{
			StartCoroutine(FloatCR());
		}
		if (m_rotationSpeed != Vector3.zero)
		{
			StartCoroutine(RotateCR());
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private IEnumerator FloatCR()
	{
		bool floatingUp = true;


		while (true)
		{
			float timer = 0f;
			var posVec = transform.localPosition;
			var startPos = posVec.y;
			var endPos = posVec.y + ((floatingUp ? 1f : -1f ) * m_verticalFloatRange);

			while (timer < m_verticalFloatTime)
			{
				var newPos = Mathf.SmoothStep(startPos, endPos, timer / m_verticalFloatTime);
				SetPosition(newPos);
				yield return null;
				timer += Time.deltaTime;
			}
			SetPosition(endPos);
			floatingUp = !floatingUp;
		}
	}

	private void SetPosition(float a_pos)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, a_pos, transform.localPosition.z);
	}

	private IEnumerator RotateCR()
	{
		while (true)
		{
			transform.Rotate(m_rotationSpeed * Time.deltaTime);
			yield return null;
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