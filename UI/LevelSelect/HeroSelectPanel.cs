using System.Collections.Generic;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HeroSelectPanel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class HeroSelectPanel : LevelSelectPanel
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected Transform m_listParent;

	[SerializeField]
	private GlowButton m_confirmButton;

	[SerializeField, TemplateIDField(typeof(HeroSelectEntry), "Hero Choice Entry", "")]
	protected int m_heroEntryTID;

	[SerializeField, TemplateIDField(typeof(AbilitySelectEntry), "Ability Entry", "")]
	protected int m_abilityEntryTID;

	[SerializeField]
	protected Transform m_abilityParent;

	[SerializeField]
	protected List<RenderTexture> m_renTextList;

	[SerializeField, TemplateIDField(typeof(RenderTextureHelper), "Ren Tex", "")]
	private int m_renderTextureHelperTID;

	[SerializeField]
	private Transform m_statParent;

	[SerializeField, TemplateIDField(typeof(StatEntry), "Stat entry", "")]
	private int m_statTID;

	[SerializeField]
	private TMP_Text m_abilitycharges;

	//--- NonSerialized ---
	private List<HeroSelectEntry> m_entries = new List<HeroSelectEntry>();
	private List<AbilitySelectEntry> m_abilityEntries = new List<AbilitySelectEntry>();
	protected HeroSelectEntry m_selectedUnitEntry;
	protected List<RenderTextureHelper> m_renTexObjects = new List<RenderTextureHelper>();
	private List<StatEntry> m_statEntries = new List<StatEntry>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnDestroy()
	{
		foreach (var ren in m_renTexObjects)
		{
			if (ren != null)
				Destroy(ren.gameObject);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override void Init(LevelSelectUI a_parent)
	{
		base.Init(a_parent);

		m_confirmButton.SetInteractive(false);

		var heroTIDs = GameManager.Instance.KingdomSettings.HeroTemplateTIDs;
		int index = 0;
		foreach (var heroTID in heroTIDs)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<HeroSelectEntry>(m_heroEntryTID);
			if (entry != null)
			{
				entry.Init(this, heroTID, m_renTextList[index]);
				entry.transform.SetParent(m_listParent, false);
				m_entries.Add(entry);

				var renTexObject = AssetCacher.Instance.InstantiateComponent<RenderTextureHelper>(m_renderTextureHelperTID);
				renTexObject.transform.position = new Vector3(0f, (index + 1) * -10000f, 0f);
				var unitTemplate = AssetCacher.Instance.CacheAsset<UnitTemplate>(heroTID);
				renTexObject.Init(unitTemplate.UnitModel);
				renTexObject.SetRenderTexture(m_renTextList[index]);
				m_renTexObjects.Add(renTexObject);

				index++;
			}
		}

		OnHeroSelected(m_entries[0]);
	}

	public void OnHeroSelected(HeroSelectEntry a_entry)
	{
		if (m_selectedUnitEntry == a_entry)
			return;

		if (m_selectedUnitEntry != null)
		{
			m_selectedUnitEntry.SetSelected(false);
		}
		m_selectedUnitEntry = a_entry;
		m_selectedUnitEntry.SetSelected(true);
		m_confirmButton.SetInteractive(m_selectedUnitEntry.HeroTID != tid.NULL);
		var unitTemplate = AssetCacher.Instance.CacheAsset<UnitTemplate>(m_selectedUnitEntry.HeroTID);
		m_parent.UnitPreviewed(unitTemplate);

		foreach (var statEntry in m_statEntries)
		{
			Destroy(statEntry.gameObject);
		}
		m_statEntries.Clear();

		var stats = unitTemplate.UnitStatTemplate.BaseStatDataList;
		foreach (var stat in stats)
		{
			if (stat.StatTemplate.TID == StatSettings.MaxHealthStatTID)
				continue;

			var statEntry = AssetCacher.Instance.InstantiateComponent<StatEntry>(m_statTID, m_statParent);
			statEntry.InitTemplate(stat);
			m_statEntries.Add(statEntry);
		}

		m_abilitycharges.text = unitTemplate.AbilityCharges.ToString();

		//create abilities
		UpdateAbilities();
	}

	private void UpdateAbilities()
	{
		foreach (var ability in m_abilityEntries)
		{
			Destroy(ability.gameObject);
		}
		m_abilityEntries.Clear();

		if (m_selectedUnitEntry != null)
		{
			var unitTemplate = AssetCacher.Instance.CacheAsset<UnitTemplate>(m_selectedUnitEntry.HeroTID);
			foreach (var abilityTID in unitTemplate.Abilities)
			{
				var entry = AssetCacher.Instance.InstantiateComponent<AbilitySelectEntry>(m_abilityEntryTID, m_abilityParent);
				entry.Init(abilityTID);
				m_abilityEntries.Add(entry);
			}
		}

	}

	public override void OnBackButton()
	{
		Destroy(gameObject);
		m_parent.OpenLevelListPanel();
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirmButton()
	{
		m_parent.SelectUnit(AssetCacher.Instance.CacheAsset<UnitTemplate>(m_selectedUnitEntry.HeroTID));
		Destroy(gameObject);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}