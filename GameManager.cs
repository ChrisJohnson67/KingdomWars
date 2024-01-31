using Photon.Pun;
using Platform.CSR;
using Platform.UIManagement;
using Platform.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

	public const string c_UnitTag = "UnitTag";


	[SerializeField]
	private Canvas m_mainCanvas = null;

	[SerializeField]
	private Camera m_uiCamera;

	[SerializeField]
	private RectTransform m_canvasParent;

	[SerializeField]
	private Transform m_fxParent;

	[SerializeField]
	private Transform m_unitParent;

	[SerializeField]
	private KingdomUISettings m_uiSettings = null;

	[SerializeField]
	private AudioSettings m_audioSettings;

	[SerializeField]
	private StatSettings m_statSettings;

	[SerializeField]
	private KingdomSettings m_kingdomSettings;

	[SerializeField]
	private CombatSettings m_combatSettings;


	private CombatManager m_combatManager;
	private List<PlayerLeaderboardData> m_cachedLeaderboard = new List<PlayerLeaderboardData>();
	private List<KingdomData> m_cachedKingdomData = new List<KingdomData>();

	public Canvas MainCanvas { get { return m_mainCanvas; } }
	public RectTransform CanvasParent { get { return m_canvasParent; } }
	public Transform FXParent { get { return m_fxParent; } }
	public Transform UnitParent { get { return m_unitParent; } }
	public KingdomUISettings UISettings { get { return m_uiSettings; } }
	public AudioSettings AudioSettings { get { return m_audioSettings; } }
	public StatSettings StatSettings { get { return m_statSettings; } }
	public KingdomSettings KingdomSettings { get { return m_kingdomSettings; } }
	public CombatSettings CombatSettings { get { return m_combatSettings; } }

	public List<PlayerLeaderboardData> CachedLeaderboard { get => m_cachedLeaderboard; set => m_cachedLeaderboard = value; }
	public List<KingdomData> CachedKingdomData { get => m_cachedKingdomData; set => m_cachedKingdomData = value; }

	private IEnumerator Start()
	{
		LoadingScreenUI.Show();
		yield return null;

		Shader.WarmupAllShaders();
		CSRManager.Instance.VerifyCreate();

		UIManager.Instance.OpenUI<TitleScreenUI>(GameManager.Instance.UISettings.TitleScreenTID);
		yield return null;
		LoadingScreenUI.Hide();

		PhotonNetwork.NickName = SaveManager.GetPlayerName();

		m_cachedKingdomData.AddRange(SaveManager.GetDownloadedLevels(false));
	}

	public void StartCombat(CombatContextData a_data)
	{
		m_combatManager = new CombatManager();
		m_combatManager.InitCombat(a_data);
	}

	public void EndCombat()
	{
		m_combatManager = null;
	}

	private void Update()
	{
		if (m_combatManager != null)
		{
			m_combatManager.UpdateCombat(Time.deltaTime);
		}
	}

	public void UpdateCachedLeaderboard(List<PlayerLeaderboardData> a_data)
	{
		CachedLeaderboard.Clear();
		CachedLeaderboard.AddRange(a_data);
	}

	public void AddCachedKingdomData(KingdomData a_data)
	{
		if (!m_cachedKingdomData.Contains(a_data))
			CachedKingdomData.Add(a_data);
	}

	public void SetDepthCameraActive(bool a_active)
	{
		UIUtils.SetActive(m_uiCamera, a_active);
	}

	public void DeleteObject(GameObject a_object, float a_time)
	{
		if (a_object != null)
		{
			GameObject.Destroy(a_object, a_time);
		}
	}

	public void DeleteObject(GameObject a_object)
	{
		if (a_object != null)
		{
			GameObject.Destroy(a_object);
		}
	}

	public void DeleteObject(Component a_comp)
	{
		if (a_comp != null)
		{
			DeleteObject(a_comp.gameObject);
		}
	}
}
