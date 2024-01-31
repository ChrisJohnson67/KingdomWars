using DarkTonic.MasterAudio;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LevelSelectUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class LevelSelectUI : FullscreenUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_title;

	[SerializeField, TemplateIDField(typeof(LevelListPanel), "List Panel", "")]
	private int m_levelListPanelTID;

	[SerializeField, TemplateIDField(typeof(HeroSelectPanel), "Hero Panel", "")]
	private int m_heroSelectPanelTID;

	[SerializeField, TemplateIDField(typeof(ItemSelectPanel), "Item Panel", "")]
	private int m_itemSelectPanelTID;

	[SerializeField]
	private Transform m_panelParent;

	[SerializeField]
	private DangerDisplay m_dangerKingdomDisplay;

	[SerializeField]
	private TMP_Text m_kingdomPanelName;

	[SerializeField]
	private TMP_Text m_kingdomPanelAuthor;

	[SerializeField, TemplateIDField(typeof(RenderTextureHelper), "Ren Tex", "")]
	private int m_renderTextureHelperTID;

	[SerializeField]
	private GameObject m_unitDisplayImage;

	//--- NonSerialized ---
	protected KingdomData m_selectedKingdomData;
	protected UnitTemplate m_selectedUnit;
	protected LevelSelectPanel m_currentPanel;
	protected bool m_playtesting = false;
	protected bool m_requesting;
	protected RenderTextureHelper m_renTexObject;
	protected CombatContextData m_contextData = new CombatContextData();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public bool IsRequesting { get { return m_requesting; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	void Start()
	{
		GameManager.Instance.SetDepthCameraActive(true);
		MasterAudio.ChangePlaylistByName("MX_Title");
	}

	private void OnDestroy()
	{
		if (GameManager.HasInstance)
			GameManager.Instance.SetDepthCameraActive(false);
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init()
	{
		UpdateFromServer();
		OpenLevelListPanel();
	}

	public void InitAsretry(KingdomData a_kingdomData)
	{
		m_playtesting = false;
		UpdateSelectedKingdom(a_kingdomData);
		OpenHeroSelectPanel();
	}

	public void InitAsPlaytest(KingdomData a_kingdomData)
	{
		m_playtesting = true;
		UpdateSelectedKingdom(a_kingdomData);
		OpenHeroSelectPanel();
	}

	private void UpdateFromServer()
	{
		m_requesting = false;
		if (!KingdomNetworkingManager.Instance.IsConnectedToNetwork)
		{
			KingdomNetworkingManager.Instance.RequestLevels(OnRequestingComplete);
			m_requesting = true;
		}
	}

	private void OnRequestingComplete(bool a_error)
	{
		if (this == null)
			return;

		if (a_error)
		{
			GenericPopupUI.CreatePopup("There was an error updating levels from server.", null);
		}
		m_requesting = false;
	}

	public void OpenLevelListPanel()
	{
		UpdateSelectedKingdom(null);
		m_currentPanel = AssetCacher.Instance.InstantiateComponent<LevelListPanel>(m_levelListPanelTID, m_panelParent);
		m_currentPanel.Init(this);
	}

	public void OpenHeroSelectPanel()
	{
		m_selectedUnit = null;
		UpdateRenTextDisplay(tid.NULL);
		m_currentPanel = AssetCacher.Instance.InstantiateComponent<HeroSelectPanel>(m_heroSelectPanelTID, m_panelParent);
		m_currentPanel.Init(this);
	}

	public void OpenItemSelectPanel()
	{
		m_currentPanel = AssetCacher.Instance.InstantiateComponent<ItemSelectPanel>(m_itemSelectPanelTID, m_panelParent);
		m_currentPanel.Init(this);
	}

	public void SetTitle(string a_text)
	{
		m_title.text = a_text;
	}

	public void SelectKingdom(KingdomData a_data)
	{
		UpdateSelectedKingdom(a_data);
		OpenHeroSelectPanel();
	}

	private void UpdateRenTextDisplay(int a_unitModel)
	{
		if (m_renTexObject == null)
		{
			m_renTexObject = AssetCacher.Instance.InstantiateComponent<RenderTextureHelper>(m_renderTextureHelperTID);
			m_renTexObject.transform.position = new Vector3(-10000f, 0f, 0f);
		}
		m_renTexObject.Init(a_unitModel);
		UIUtils.SetActive(m_renTexObject, a_unitModel != tid.NULL);
		UIUtils.SetActive(m_unitDisplayImage, a_unitModel != tid.NULL);
	}

	private void UpdateSelectedKingdom(KingdomData a_data)
	{
		m_selectedKingdomData = a_data;
		if (m_selectedKingdomData == null)
		{
			m_dangerKingdomDisplay.Clear();
			if (m_kingdomPanelName != null)
				m_kingdomPanelName.text = string.Empty;

			if (m_kingdomPanelAuthor != null)
				m_kingdomPanelAuthor.text = string.Empty;
		}
		else
		{
			m_dangerKingdomDisplay.InitList(m_selectedKingdomData.GetDangers());

			if (m_kingdomPanelName != null)
				m_kingdomPanelName.text = m_selectedKingdomData.Title;

			if (m_kingdomPanelAuthor != null)
				m_kingdomPanelAuthor.text = m_selectedKingdomData.Author;

			m_contextData.HasBonus = m_selectedKingdomData.Wins == 0;
		}
	}

	public void UnitPreviewed(UnitTemplate a_unit)
	{
	}

	public void SelectUnit(UnitTemplate a_unit)
	{
		m_selectedUnit = a_unit;
		UpdateRenTextDisplay(m_selectedUnit.UnitModel);
		OpenItemSelectPanel();
	}

	public void StartCombat(List<int> a_items)
	{
		m_contextData.HeroTemplate = m_selectedUnit;
		m_contextData.KingdomData = m_selectedKingdomData;
		m_contextData.Items = a_items;
		m_contextData.Playtesting = m_playtesting;
		CloseUI();
		GameManager.Instance.StartCombat(m_contextData);
	}

	protected override void Clear()
	{
		base.Clear();

		if (m_renTexObject != null)
			Destroy(m_renTexObject.gameObject);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnBackButton()
	{
		if (m_currentPanel != null)
		{
			m_currentPanel.OnBackButton();
		}
	}

	#endregion Callbacks

}