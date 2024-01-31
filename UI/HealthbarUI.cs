using Platform.UIManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HealthbarUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HealthbarUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected Image m_fillImage;

	[SerializeField]
	protected Image m_damageFillImage;

	[SerializeField]
	protected Transform m_buffParent;

	[SerializeField]
	protected BuffEntry m_buffEntryPrefab;

	[SerializeField]
	protected float m_damageBeginWaitTime = 1f;

	[SerializeField]
	protected float m_damageMinTime = 0.2f;

	[SerializeField]
	protected float m_damageMaxTime = 0.6f;

	//--- NonSerialized ---
	protected UnitInstance m_unitInstance;
	protected List<BuffEntry> m_buffEntries = new List<BuffEntry>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(UnitInstance a_target)
	{
		m_unitInstance = a_target;
		m_unitInstance.OnAddBuff += OnBuffAdded;
		m_unitInstance.OnDamageTaken += UpdateHealthbar;
		m_unitInstance.OnHealApplied += OnHealed;
		m_fillImage.fillAmount = 1f;
		UpdateFill();
		UpdatePosition();
	}

	public void Clear()
	{
		if (m_unitInstance != null)
		{
			m_unitInstance.OnAddBuff -= OnBuffAdded;
			m_unitInstance.OnDamageTaken -= UpdateHealthbar;
			m_unitInstance.OnHealApplied -= OnHealed;
		}
	}

	protected void Update()
	{
		if (m_unitInstance != null && m_unitInstance.Model != null && !m_unitInstance.IsDead)
		{
			UpdatePosition();
		}
	}

	protected virtual void UpdatePosition()
	{
		var position = CameraHelpers.WorldToUISpace(CombatManager.Instance.Camera, GameManager.Instance.CanvasParent, m_unitInstance.Model.HealthNode.position);
		transform.localPosition = position;
	}

	protected virtual void UpdateFill()
	{
		if (m_unitInstance != null && m_fillImage != null)
		{
			m_damageFillImage.fillAmount = m_fillImage.fillAmount;
			UIUtils.SetActive(m_damageFillImage, true);
			m_fillImage.fillAmount = Mathf.Max(0f, m_unitInstance.CurrentHealth / m_unitInstance.CurrentStats.GetMaxHealth());

			if (m_damageFillImage.fillAmount > m_fillImage.fillAmount)
			{
				StartCoroutine(DisplayDamageCR());
			}
		}
	}

	protected IEnumerator DisplayDamageCR()
	{
		yield return new WaitForSeconds(m_damageBeginWaitTime);
		float timer = 0f;
		float startFill = m_damageFillImage.fillAmount;
		float endFill = m_fillImage.fillAmount;
		float damageTime = Mathf.Lerp(m_damageMinTime, m_damageMaxTime, 1f - (endFill / startFill));
		while (timer < damageTime)
		{
			float newFill = Mathf.SmoothStep(startFill, endFill, timer / damageTime);
			m_damageFillImage.fillAmount = newFill;
			yield return null;

			timer += Time.deltaTime;
		}
		m_damageFillImage.fillAmount = endFill;

		UIUtils.SetActive(m_damageFillImage, false);
	}

	protected void UpdateHealthbar(UnitInstance a_unitDamaged, int a_damage, DamageAppliedInfo a_damageInfo, UnitInstance a_source)
	{
		UpdateFill();
	}

	private void OnHealed(UnitInstance a_unit, int a_amount)
	{
		UpdateFill();
	}

	public void RefreshBuffs()
	{
		//refresh all buffs
		foreach (var entry in m_buffEntries)
		{
			entry.Refresh();
		}
	}

	protected void OnBuffAdded(UnitInstance a_unit, BuffInstance a_buff, bool a_stacksOnly)
	{
		if (m_buffParent == null || m_buffEntryPrefab == null)
			return;

		bool createNewBuff = !a_stacksOnly;
		if (!createNewBuff)
		{
			RefreshBuffs();
		}
		else
		{
			if (a_buff.Template.ShowInUI)
			{
				var entry = GameObject.Instantiate<BuffEntry>(m_buffEntryPrefab, m_buffParent);
				if (entry != null)
				{
					entry.Init(this, a_buff);
					m_buffEntries.Add(entry);
				}
			}
		}
	}

	public void RemoveBuffEntry(BuffEntry a_entry)
	{
		m_buffEntries.Remove(a_entry);
		a_entry.Clear();
		GameManager.Instance.DeleteObject(a_entry);
	}

	public void ExitCombat()
	{
		foreach (var entry in m_buffEntries)
		{
			entry.Clear();
			GameManager.Instance.DeleteObject(entry);
		}
		m_buffEntries.Clear();
	}

	public void DestroyUI()
	{
		Clear();
		GameManager.Instance.DeleteObject(gameObject);
	}

	public static HealthbarUI Create(UnitInstance a_target)
	{
		var healthbarUI = UIManager.Instance.OpenUI<HealthbarUI>(GameManager.Instance.UISettings.HealthbarTID);
		healthbarUI.Init(a_target);
		return healthbarUI;
	}

	#endregion Runtime Functions
}
