using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityCombatEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AbilityCombatEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private GlowButton m_button;

	[SerializeField]
	private GameObject m_disableObject;

	[SerializeField]
	private TMP_Text m_chargesText;

	[SerializeField]
	private Image m_cooldownObject;

	//--- NonSerialized ---
	private AbilityTemplate m_abilityTemplate;
	private CombatUI m_parent;
	private bool m_cooldown;

	public static Action OnAbilityUsed;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		AbilityCombatEntry.OnAbilityUsed += OnAbilityUsedCallback;
	}

	private void OnDestroy()
	{
		AbilityCombatEntry.OnAbilityUsed -= OnAbilityUsedCallback;
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(CombatUI a_parent, AbilityTemplate a_ability)
	{
		base.Init(a_ability);
		m_abilityTemplate = a_ability;
		m_parent = a_parent;
		UIUtils.SetActive(m_cooldownObject, false);

		CombatManager.Instance.HeroUnit.OnAbilityChargeChange += Refresh;
		CombatManager.Instance.HeroUnit.OnStunned += OnStun;
	}

	protected void Refresh()
	{
		bool inCombat = CombatManager.Instance.InRoomCombatState;
		m_button.SetInteractive(CanUseAbility() && inCombat);
		UIUtils.SetActive(m_disableObject, !inCombat);

		m_chargesText.text = CombatManager.Instance.HeroUnit.AbilityCharges.ToString();
	}

	public void SetInRoomCombat(bool a_inCombat)
	{
		Refresh();
	}

	protected bool CanUseAbility()
	{
		return CombatManager.Instance.HeroUnit.HasAbilityCharges && !CombatManager.Instance.HeroUnit.IsStunned && !m_cooldown;
	}

	private IEnumerator ShowCooldownCR()
	{
		UIUtils.SetActive(m_cooldownObject, true);
		float timer = 0f;
		while (timer <= 0.25)
		{

			var fill = Mathf.Lerp(1f, 0f, timer / 0.25f);
			m_cooldownObject.fillAmount = fill;
			yield return null;
			timer += Time.deltaTime;

		}
		m_cooldown = false;
		UIUtils.SetActive(m_cooldownObject, false);
		Refresh();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		if (CanUseAbility())
		{
			m_cooldown = true;
			m_parent.UseAbility(m_abilityTemplate);
			Refresh();
			OnAbilityUsed?.Invoke();
		}
	}

	private void OnAbilityUsedCallback()
	{
		StartCoroutine(ShowCooldownCR());
	}

	private void OnStun(bool a_stunned)
	{
		Refresh();
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}