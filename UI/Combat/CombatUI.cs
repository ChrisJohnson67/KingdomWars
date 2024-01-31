//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CombatUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using DarkTonic.MasterAudio;
using Platform.UIManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatUI : FullscreenUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public const string c_DetailTag = "Detail";
	public const string c_UnitTag = "Unit";

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private PlayerHealthBarUI m_healthBar;

	[SerializeField, TemplateIDField(typeof(AbilityCombatEntry), "Ability entry", "")]
	private int m_abilityTID;

	[SerializeField]
	private Transform m_abilityParent;

	[SerializeField, TemplateIDField(typeof(ItemCombatEntry), "Item entry", "")]
	private int m_itemTID;

	[SerializeField]
	private Transform m_itemParent;

	[SerializeField]
	private TMP_Text m_chargesRemainText;

	[SerializeField]
	private DangerDisplay m_dangerDisplay;

	[SerializeField]
	private Transform m_statParent;

	[SerializeField, TemplateIDField(typeof(StatEntry), "Stat entry", "")]
	private int m_statTID;

	[SerializeField]
	private Transform m_choiceEntryParent;

	[SerializeField, TemplateIDField(typeof(RoomChoiceCombatEntry), "Room choice entry", "")]
	private int m_roomChoiceTID;

	//--- NonSerialized ---
	private RaycastHit[] m_raycastHits = new RaycastHit[4];
	private List<AbilityCombatEntry> m_abilityEntries = new List<AbilityCombatEntry>();
	private List<ItemCombatEntry> m_itemEntries = new List<ItemCombatEntry>();
	private List<StatEntry> m_statEntries = new List<StatEntry>();
	private List<RoomChoiceCombatEntry> m_choiceEntries = new List<RoomChoiceCombatEntry>();
	private CombatStartPopup m_startPopup;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		InputSystem.TouchDownEvent += OnTouchDown;
		CombatManager.OnSetupComplete += SetupComplete;
		CombatManager.OnRoomStart += OnRoomStart;
		CombatManager.OnRoomEnded += OnRoomEnd;
		CombatManager.OnRoomPause += OnRoomPause;

		MasterAudio.ChangePlaylistByName("Combat");
	}

	private void OnDestroy()
	{
		InputSystem.TouchDownEvent -= OnTouchDown;
		CombatManager.OnSetupComplete -= SetupComplete;
		CombatManager.OnRoomStart -= OnRoomStart;
		CombatManager.OnRoomEnded -= OnRoomEnd;
		CombatManager.OnRoomPause -= OnRoomPause;
		if (UIManager.HasInstance)
			UIManager.Instance.CloseUI(GameManager.Instance.UISettings.TooltipPopup);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GenericPopupUI.CreatePopup("Quit Combat? You will lose hero rating.", OnQuit, OnCancel);
		}
	}

	private void OnQuit()
	{
		CombatManager.Instance.Quit();
	}

	private void OnCancel()
	{

	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void SetupComplete()
	{
		m_healthBar.Init(CombatManager.Instance.HeroUnit);

		foreach (var ability in CombatManager.Instance.ContextData.HeroTemplate.Abilities)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<AbilityCombatEntry>(m_abilityTID, m_abilityParent);
			entry.Init(this, ability);
			m_abilityEntries.Add(entry);
		}

		foreach (var itemTID in CombatManager.Instance.ContextData.Items)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<ItemCombatEntry>(m_itemTID, m_itemParent);
			entry.Init(this, AssetCacher.Instance.CacheAsset<AbilityTemplate>(itemTID));
			m_itemEntries.Add(entry);
		}

		EnableButtons(false);

		CombatManager.Instance.HeroUnit.OnAbilityChargeChange += UpdateAbilityCharges;

		var stats = CombatManager.Instance.HeroUnit.CurrentStats.Stats;
		foreach (var stat in stats)
		{
			if (stat.Template.TID == StatSettings.MaxHealthStatTID)
				continue;

			var statEntry = AssetCacher.Instance.InstantiateComponent<StatEntry>(m_statTID, m_statParent);
			statEntry.Init(stat);
			m_statEntries.Add(statEntry);
		}
		UpdateAbilityCharges();
	}

	public void UseAbility(AbilityTemplate a_ability)
	{
		CombatManager.Instance.HeroUnit.CreateAbility(a_ability.TID, false, false);

		GameManager.Instance.KingdomSettings.AbilityUseSound.Play();
	}

	public void UseItemAbility(AbilityTemplate a_ability)
	{
		CombatManager.Instance.HeroUnit.CreateAbility(a_ability.TID, false, true);

		GameManager.Instance.KingdomSettings.AbilityUseSound.Play();
	}

	private void UpdateAbilityCharges()
	{
		m_chargesRemainText.text = "Ability Charges: " + CombatManager.Instance.HeroUnit.AbilityCharges;
	}

	private void EnableButtons(bool a_enable)
	{
		foreach (var ability in m_abilityEntries)
		{
			ability.SetInRoomCombat(a_enable);
		}

		foreach (var item in m_itemEntries)
		{
			item.SetInRoomCombat(a_enable);
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	private void OnRoomStart()
	{
		EnableButtons(true);
	}

	private void OnRoomEnd()
	{
		EnableButtons(false);

		m_dangerDisplay.Clear();

		DestroyChoiceEntries();
	}

	private void OnRoomPause(bool a_pause)
	{
		if (a_pause)
		{
			m_startPopup = UIManager.Instance.OpenUI<CombatStartPopup>(GameManager.Instance.UISettings.CombatStartPopup);

			if (!CombatManager.Instance.IsLastRoom)
			{
				m_dangerDisplay.InitList(CombatManager.Instance.GetCurrentKingdomRoomData.GetDangers());
			}

			var currentKingdomData = CombatManager.Instance.GetCurrentKingdomRoomData;
			CreateChoiceEntry(currentKingdomData.CoreTID);
			CreateChoiceEntry(currentKingdomData.MonsterGroupTID);
			CreateChoiceEntry(currentKingdomData.SpellTID);
		}
		else
		{
			if (m_startPopup != null)
			{
				m_startPopup.CloseUI();
			}
		}
	}

	private void DestroyChoiceEntries()
	{
		foreach (var entry in m_choiceEntries)
		{
			Destroy(entry.gameObject);
		}
		m_choiceEntries.Clear();
	}

	private void CreateChoiceEntry(int a_choiceTID)
	{
		var entry = AssetCacher.Instance.InstantiateComponent<RoomChoiceCombatEntry>(m_roomChoiceTID, m_choiceEntryParent);
		if (entry != null)
		{
			entry.Init(AssetCacher.Instance.CacheAsset<RoomChoiceTemplate>(a_choiceTID));
			m_choiceEntries.Add(entry);
		}
	}

	public void OnExitCombat()
	{
		CombatManager.Instance.Cleanup();
		CloseUI();
		UIManager.Instance.OpenUI<TitleScreenUI>(GameManager.Instance.UISettings.TitleScreenTID);
	}

	public void OnTouchDown(Vector2 a_touchPoint)
	{
		var ray = CombatManager.Instance.Camera.ScreenPointToRay(a_touchPoint);
		int hits = Physics.RaycastNonAlloc(ray, m_raycastHits, 50);
		for (int i = 0; i < hits; i++)
		{
			var hitData = m_raycastHits[i];
			if (hitData.collider.CompareTag(c_UnitTag))
			{
				var unitModel = hitData.transform.GetComponent<UnitModel>();
				if (CombatManager.Instance.HeroUnit != unitModel.UnitInstance && !unitModel.UnitInstance.IsDead)
				{
					CombatManager.Instance.SetTargetedUnit(unitModel.UnitInstance);
				}
				break;
			}
			else if (hitData.collider.CompareTag(c_DetailTag))
			{
				var detail = hitData.transform.GetComponent<DetailObjectHelper>();
				detail.OnClicked();
			}
		}
	}

	#endregion Callbacks

}