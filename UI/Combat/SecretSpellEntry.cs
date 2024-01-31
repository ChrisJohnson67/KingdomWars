using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// SecretSpellEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class SecretSpellEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_timerText;

	[SerializeField]
	private Transform m_displayParent;

	[SerializeField]
	private Image m_fillImage;


	//--- NonSerialized ---
	private RoomSpellTemplate m_roomSpellTemplate;
	private int m_lastTime = -1;
	private float m_timer = 0f;
	private float m_totalSpellTime;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		CombatManager.OnRoomStart += OnRoomStart;
		UIUtils.SetActive(m_displayParent, false);
	}

	private void OnDestroy()
	{
		CombatManager.OnRoomStart -= OnRoomStart;
	}

	private void Update()
	{
		if (CombatManager.Instance == null || !CombatManager.Instance.InRoomCombatState)
			return;

		int newTime = Mathf.CeilToInt(m_timer);
		if (newTime != m_lastTime)
		{
			m_timerText.text = (newTime.ToString());
			m_lastTime = newTime;
		}
		if (newTime <= 0)
		{
			UIUtils.SetActive(m_displayParent, false);
		}

		if (newTime >= 0)
		{
			m_fillImage.fillAmount = m_timer / m_totalSpellTime;
		}

		m_timer -= Time.deltaTime;
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(RoomSpellTemplate a_ability)
	{
		base.Init(a_ability);
		m_roomSpellTemplate = a_ability;
		m_totalSpellTime = m_roomSpellTemplate.Ability.CastTime;
		m_timer = m_totalSpellTime;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	private void OnRoomStart()
	{
		Init(AssetCacher.Instance.CacheAsset<RoomSpellTemplate>(CombatManager.Instance.GetCurrentKingdomRoomData.SpellTID));
		UIUtils.SetActive(m_displayParent, true);
		m_lastTime = -1;
	}

	public override void OnTooltipInfo(TooltipInfo a_info)
	{
		base.OnTooltipInfo(a_info);
		a_info.DontShow = m_timer <= 0f;
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}