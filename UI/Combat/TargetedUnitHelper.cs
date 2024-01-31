using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// TargetedUnitHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class TargetedUnitHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GameObject m_targetPrefab;

	//--- NonSerialized ---
	private UnitInstance m_target;
	private GameObject m_targetObject;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		CombatManager.OnUnitTargetSwitched += OnTargetSwitch;
	}

	private void OnDestroy()
	{
		CombatManager.OnUnitTargetSwitched -= OnTargetSwitch;
	}

	private void Update()
	{
		if (m_target != null)
		{
			var position = CameraHelpers.WorldToUISpace(CombatManager.Instance.Camera, GameManager.Instance.CanvasParent, m_target.Model.ChatNode.position);
			m_targetObject.transform.localPosition = position;
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	private void OnTargetSwitch(UnitInstance a_target)
	{
		m_target = a_target;
		if (m_target == null)
		{
			if (m_targetObject != null)
			{
				Destroy(m_targetObject.gameObject);
			}
			return;
		}
		if (m_targetObject == null)
		{
			m_targetObject = Instantiate(m_targetPrefab, transform);
		}

		var position = CameraHelpers.WorldToUISpace(CombatManager.Instance.Camera, GameManager.Instance.CanvasParent, m_target.Model.ChatNode.position);
		m_targetObject.transform.localPosition = position;
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}