using Platform.UIManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// LevelListPanel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class LevelListPanel : LevelSelectPanel
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
	protected GlowButton m_refreshButton;

	[SerializeField]
	protected GameObject m_spinnerObj;

	[SerializeField, TemplateIDField(typeof(LevelListEntry), "Level Entry", "")]
	protected int m_levelListTID;

	//--- NonSerialized ---
	private List<LevelListEntry> m_entries = new List<LevelListEntry>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages


	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public override void Init(LevelSelectUI a_parent)
	{
		base.Init(a_parent);

		UIUtils.SetActive(m_spinnerObj, m_parent.IsRequesting);
		if (!m_parent.IsRequesting)
		{
			CreateLevels();
		}
		else
		{
			StartCoroutine(WaitForUpdateCR());
		}
	}

	private IEnumerator WaitForUpdateCR()
	{
		while (m_parent.IsRequesting)
		{
			yield return null;
		}
		ClearEntries();
		CreateLevels();
	}

	private void CreateLevels()
	{
		UIUtils.SetActive(m_spinnerObj, false);

		var levels = new List<KingdomData>(GameManager.Instance.CachedKingdomData);
		levels.AddRange(GameManager.Instance.KingdomSettings.GetPresetKingdomDataList());

		var myLevels = SaveManager.GetMyLevels();

		levels.Sort(SortKingdoms);
		foreach (var level in levels)
		{
			if (myLevels.Find(x => x != null && x.LevelID == level.LevelID) == null)
			{
				var entry = AssetCacher.Instance.InstantiateComponent<LevelListEntry>(m_levelListTID);
				if (entry != null)
				{
					entry.Init(this, level);
					entry.transform.SetParent(m_listParent, false);
					m_entries.Add(entry);
				}
			}
		}
	}

	private int SortKingdoms(KingdomData a_left, KingdomData a_right)
	{
		if (a_left == a_right)
			return 0;

		int leftTotalPlays = a_left.Wins + a_left.Losses;
		int rightTotalPlays = a_right.Wins + a_right.Losses;

		if (leftTotalPlays == rightTotalPlays)
		{
			return a_left.Losses.CompareTo(a_right.Losses);
		}

		if (a_left.Losses == a_right.Losses)
		{
			return a_left.Wins.CompareTo(a_right.Wins);
		}

		return a_left.Losses.CompareTo(a_right.Losses);
	}

	private void ClearEntries()
	{
		foreach (var entry in m_entries)
		{
			GameObject.Destroy(entry);
		}
		m_entries.Clear();
	}

	public void OnLevelSelected(KingdomData a_data)
	{
		m_parent.SelectKingdom(a_data);
		Destroy(gameObject);
	}

	public override void OnBackButton()
	{
		m_parent.CloseUI();
		UIManager.Instance.OpenUI<TitleScreenUI>(GameManager.Instance.UISettings.TitleScreenTID);
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnRefreshButton()
	{
		m_refreshButton.SetInteractive(false);
		if (!KingdomNetworkingManager.Instance.IsConnectedToNetwork)
		{
			print("Requesting levels");
			ClearEntries();
			KingdomNetworkingManager.Instance.RequestLevels((a_error) =>
			{
				CreateLevels();
				m_refreshButton.SetInteractive(true);

			});
		}

	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}