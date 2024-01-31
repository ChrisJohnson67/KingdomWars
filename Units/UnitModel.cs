using System;
using System.Collections;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitModel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class UnitModel : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_MovingAnim = "bMoving";
	public const string c_AttackAnim = "tAttack2";
	public const string c_Attack2Anim = "tAttack1";
	public const string c_RangeAttack1Anim = "tRangedAttack1";
	public const string c_AbilityAnim = "tAbility";
	public const string c_PulseAnim = "tPulse";
	public const string c_DieAnim = "tDefeat";

	public enum AttackAnimType
	{
		Melee,
		ShootOneHanded,
		ShootTwoHanded
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_healthNode;

	[SerializeField]
	private Transform m_attackNode;

	[SerializeField]
	private Transform m_selectionNode;

	[SerializeField]
	private Transform m_chatNode;

	[SerializeField]
	private Transform m_projectileNode;

	[SerializeField]
	private Transform m_baseTransform;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private float m_attackAnimationLength;

	[SerializeField]
	private AttackAnimType m_AttackAnimType = AttackAnimType.Melee;

	//--- NonSerialized ---
	protected UnitInstance m_unitInstance;
	protected HealthbarUI m_healthBar;
	protected bool m_moving = false;
	protected Transform m_selfTransform;

	public static Action<UnitModel> OnUnitHovered;
	public static Action<UnitModel> OnUnitStopHovered;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Transform HealthNode { get { return m_healthNode; } }
	public Transform AttackNode { get { return m_attackNode; } }
	public Transform SelectionNode { get { return m_selectionNode; } }
	public Transform ProjectileNode { get { return m_projectileNode; } }
	public Transform ChatNode { get { return m_chatNode; } }
	public UnitInstance UnitInstance { get { return m_unitInstance; } }
	public HealthbarUI HealthBar { get { return m_healthBar; } }
	public Vector3 Position { get { return transform.position; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		m_selfTransform = transform;
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(UnitInstance a_instance)
	{
		m_unitInstance = a_instance;
		if (m_unitInstance != null)
		{
			m_unitInstance.OnDamageTaken += OnDamageTaken;
		}
	}

	public void Cleanup()
	{
		if (m_unitInstance != null)
		{
			m_unitInstance.OnDamageTaken -= OnDamageTaken;
		}
		if (m_healthBar != null)
		{
			m_healthBar.DestroyUI();
			m_healthBar = null;
		}
		StopAllCoroutines();
	}

	public void SetPosition(Vector3 a_pos)
	{
		m_selfTransform.position = a_pos;
	}

	public void SetRotation(Vector3 a_rotation)
	{
		m_selfTransform.rotation = Quaternion.Euler(a_rotation);
	}

	public void LookAt(Vector3 a_pos)
	{
		transform.rotation = Quaternion.LookRotation(new Vector3(a_pos.x, 0f, a_pos.z) - new Vector3(transform.position.x, 0f, transform.position.z));
	}

	public void SetTargetSelected(bool a_selected)
	{
		if (a_selected)
			UIUtils.SetTrigger(m_animator, c_PulseAnim);
	}


	public void SetMoving(bool a_moving)
	{
		if (m_moving != a_moving)
		{
			m_moving = a_moving;
			UIUtils.SetBool(m_animator, c_MovingAnim, a_moving);
		}
	}

	public void MoveToPosition(Vector3 a_newPos, Action a_onMoveComplete)
	{
		SetMoving(true);
		LookAt(a_newPos);
		StartCoroutine(MovePlayerCR(a_newPos, a_onMoveComplete));
	}

	private IEnumerator MovePlayerCR(Vector3 a_pos, Action a_onComplete)
	{
		Vector3 startingPlayerPosition = Position;
		float time = 0f;
		var distance = Vector3.Distance(startingPlayerPosition, a_pos);
		var duration = distance / GameManager.Instance.CombatSettings.UnitMoveSpeed;
		while (time < duration)
		{
			var newPos = Vector3.Lerp(startingPlayerPosition, a_pos, time / duration);
			SetPosition(newPos);
			time += Time.deltaTime;
			yield return null;
		}
		SetPosition(a_pos);
		SetMoving(false);
		a_onComplete?.Invoke();
	}

	public void PlayAttackAnim(float a_waitTime = 1f)
	{
		string attackAnim = string.Empty;
		switch (m_AttackAnimType)
		{
			case AttackAnimType.Melee:
				var randomVal = RandomHelpers.GetRandomValue(0, 2);
				attackAnim = randomVal == 0 ? c_AttackAnim : c_Attack2Anim;
				break;

			case AttackAnimType.ShootOneHanded:
				attackAnim = c_RangeAttack1Anim;
				break;
		}
		UIUtils.SetTrigger(m_animator, attackAnim);
		if (m_animator != null)
		{
			//m_animator.speed = m_attackAnimationLength / a_waitTime;

			//StartCoroutine(SetAnimatorSpeedNormalCR(a_waitTime));
		}
	}

	public void PlayAbilityAnim(float a_waitTime)
	{
		UIUtils.SetTrigger(m_animator, c_AbilityAnim);
		if (m_animator != null)
		{
			//m_animator.speed = m_abilityAnimationLength / a_waitTime;

			//StartCoroutine(SetAnimatorSpeedNormalCR(a_waitTime));
		}
	}

	private IEnumerator SetAnimatorSpeedNormalCR(float a_waitTime)
	{
		float timer = 0f;
		while (timer < a_waitTime)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		if (m_animator != null)
		{
			m_animator.speed = 1f;
		}
	}

	private void OnDamageTaken(UnitInstance a_unitDamaged, int a_damage, DamageAppliedInfo a_damageInfo, UnitInstance a_source)
	{
		if (m_healthBar == null && m_unitInstance != CombatManager.Instance.HeroUnit)
		{
			m_healthBar = HealthbarUI.Create(m_unitInstance);
		}
	}

	public void ExitCombat()
	{
		if (m_healthBar != null)
		{
			m_healthBar.ExitCombat();
		}
	}

	public void UnitTurnStart()
	{
		m_healthBar.RefreshBuffs();
	}

	public void PlayDeathAnim()
	{
		UIUtils.SetTrigger(m_animator, c_DieAnim);
	}

	public void OnDeathComplete()
	{
		m_unitInstance.CompleteDeath();
		if (m_healthBar != null)
		{
			m_healthBar.DestroyUI();
		}
	}

	private void OnMouseEnter()
	{
		OnUnitHovered?.Invoke(this);
	}

	private void OnMouseExit()
	{
		OnUnitStopHovered?.Invoke(this);
	}

	public void SetScale(float a_scale)
	{
		m_selfTransform.localScale = new Vector3(a_scale, a_scale, a_scale);
	}

	public void SetScale(Vector3 a_scale)
	{
		m_selfTransform.localScale = a_scale;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (m_healthNode == null)
		{
			var obj = gameObject.FindObjectWithName("healthNode");
			if (obj != null)
			{
				m_healthNode = obj.transform;
			}
		}

		if (m_animator == null)
			m_animator = GetComponentInChildren<Animator>();

		if (m_baseTransform == null)
			m_baseTransform = transform;
	}
#endif
}
