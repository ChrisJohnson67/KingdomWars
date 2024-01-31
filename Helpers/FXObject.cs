using UnityEngine;

[RequireComponent(typeof(TemplateID))]
public class FXObject : MonoBehaviour
{
	[SerializeField]
	private float m_liveTime;

	private Transform m_followTarget;
	private float m_timer = 0f;
	private bool m_pauseTimer;

	private void Awake()
	{
		CombatManager.OnRoomEnded += OnRoomEnd;
		CombatManager.OnRoomStart += OnRoomStart;
	}
	private void OnDestroy()
	{
		CombatManager.OnRoomEnded -= OnRoomEnd;
		CombatManager.OnRoomStart -= OnRoomStart;
	}

	private void Update()
	{
		if (CombatManager.Instance == null && m_pauseTimer)
		{
			GameManager.Instance.DeleteObject(gameObject);
			return;
		}

		if (m_followTarget != null)
		{
			transform.position = m_followTarget.position;
		}

		if (m_liveTime > 0f && !m_pauseTimer)
		{
			m_timer += Time.deltaTime;

			if (m_timer >= m_liveTime)
			{
				GameManager.Instance.DeleteObject(gameObject);
			}
		}
	}

	public void SetFollowTarget(Transform a_target)
	{
		m_followTarget = a_target;
	}

	private void OnRoomStart()
	{
		m_pauseTimer = false;
	}

	private void OnRoomEnd()
	{
		m_pauseTimer = true;
	}

	public static FXObject Create(int a_fxTID, Transform a_parent)
	{
		return AssetCacher.Instance.InstantiateComponent<FXObject>(a_fxTID, a_parent);
	}

	public static FXObject Create(GameObject a_fx, Transform a_parent)
	{
		var obj = Instantiate(a_fx, a_parent);
		return obj.GetComponent<FXObject>();
	}
}
