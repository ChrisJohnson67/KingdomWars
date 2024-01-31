using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DetailObjectHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DetailObjectHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_randomMaxWaitTime = 0f;

	//--- NonSerialized ---
	private Animator m_animator;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	public void Spawn(bool a_spawnAnim)
	{
		m_animator = transform.parent.GetComponent<Animator>();
		if (a_spawnAnim)
		{
			StartCoroutine(WaitToSpawnCR());
		}
		else
		{
			UIUtils.SetTrigger(m_animator, "tSkip");
		}
	}
	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private IEnumerator WaitToSpawnCR()
	{
		float time = RandomHelpers.GetRandomValue(0f, m_randomMaxWaitTime);
		if (time > 0f)
			yield return new WaitForSeconds(time);

		UIUtils.SetTrigger(m_animator, "tSpawn");
	}

	public void OnClicked()
	{
		UIUtils.SetTrigger(m_animator, "tPulse");
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