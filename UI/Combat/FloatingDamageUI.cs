using Platform.UIManagement;
using System.Collections;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// FloatingDamageUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class FloatingDamageUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_damageText;

	[SerializeField]
	private Color m_healColor;

	[SerializeField]
	private float m_lifetime = 1f;

	[SerializeField]
	private float m_fadeTime = 0.5f;

	[SerializeField]
	private Vector3 m_minVelocityRange;

	[SerializeField]
	private Vector3 m_maxVelocityRange;

	[SerializeField]
	private CanvasGroup m_cg;

	//--- NonSerialized ---
	private float m_timer = 0f;
	private Vector3 m_velocity;
	private Vector3 m_startPosition;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void InitDamage(DamageAppliedInfo.DamageInfoType a_damageInfo, Vector3 a_startPosition)
	{
		var damageType = AssetCacher.Instance.CacheAsset<TagTemplate>(a_damageInfo.DamageType);
		if (damageType != null)
		{
			m_damageText.color = damageType.Color;
		}
		Init(a_startPosition, a_damageInfo.DamageAmount, damageType != null ? damageType.Color : Color.white);
	}

	public void InitHeal(int a_heal, Vector3 a_startPosition)
	{
		Init(a_startPosition, a_heal, m_healColor);
	}

	private void Init(Vector3 a_startPosition, int a_amount, Color a_color)
	{
		m_startPosition = a_startPosition;
		var position = CameraHelpers.WorldToUISpace(CombatManager.Instance.Camera, GameManager.Instance.CanvasParent, m_startPosition);
		transform.localPosition = position;
		m_velocity = new Vector3(RandomHelpers.GetRandomValue(m_minVelocityRange.x, m_maxVelocityRange.x), RandomHelpers.GetRandomValue(m_minVelocityRange.y, m_maxVelocityRange.y),
									RandomHelpers.GetRandomValue(m_minVelocityRange.z, m_maxVelocityRange.z));
		m_damageText.text = a_amount.ToString();
		m_damageText.color = a_color;
	}

	public void Clear()
	{
	}

	private void Update()
	{
		if (CombatManager.Instance != null && CombatManager.Instance.InCombat)
		{
			var addedPosition = m_velocity * Time.deltaTime;
			m_startPosition = m_startPosition + addedPosition;
			var position = CameraHelpers.WorldToUISpace(CombatManager.Instance.Camera, GameManager.Instance.CanvasParent, m_startPosition);
			transform.localPosition = position;
		}
		else
		{
			CloseUI();
			return;
		}

		m_timer += Time.deltaTime;
		if (m_timer >= m_lifetime)
		{
			FadeAndClose();
		}
	}

	private void CloseUI()
	{
		Clear();
		GameManager.Instance.DeleteObject(gameObject);
	}

	private void FadeAndClose()
	{
		StartCoroutine(FadeOutCR());
	}

	private IEnumerator FadeOutCR()
	{
		float timer = 0f;
		while (timer < m_fadeTime)
		{

			var newFade = Mathf.Lerp(1f, 0f, timer / m_fadeTime);
			m_cg.alpha = newFade;
			yield return null;
			timer += Time.deltaTime;
		}
		CloseUI();
	}

	public static FloatingDamageUI CreateDamage(DamageAppliedInfo.DamageInfoType a_damageInfo, UnitInstance a_target)
	{
		if (a_target != null && a_target.Model != null)
		{
			var ui = UIManager.Instance.OpenUI<FloatingDamageUI>(GameManager.Instance.UISettings.FloatingDamageTID);
			ui.InitDamage(a_damageInfo, a_target.Model.ChatNode.position);
			return ui;
		}
		return null;
	}

	public static FloatingDamageUI CreateHeal(int a_heal, UnitInstance a_target)
	{
		if (a_target != null && a_target.Model != null)
		{
			var ui = UIManager.Instance.OpenUI<FloatingDamageUI>(GameManager.Instance.UISettings.FloatingDamageTID);
			ui.InitHeal(a_heal, a_target.Model.ChatNode.position);
			return ui;
		}
		return null;
	}

	#endregion Runtime Functions
}
