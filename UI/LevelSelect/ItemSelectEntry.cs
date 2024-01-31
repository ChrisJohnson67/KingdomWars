using Platform.UIManagement;
using TMPro;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ItemSelectEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ItemSelectEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private bool m_isBackpackSlot;

	[SerializeField]
	private TMP_Text m_countText;

	//--- NonSerialized ---
	private int m_itemTID;
	private int m_index;
	private int m_count;
	private ItemSelectPanel m_parent;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int ItemTID { get { return m_itemTID; } set { m_itemTID = value; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(ItemSelectPanel a_parent, int a_itemTID, int a_index)
	{
		m_parent = a_parent;
		m_index = a_index;
		m_count = 3;

		UIUtils.SetActive(m_countText, !m_isBackpackSlot);
		SetItemTID(a_itemTID);
		UpdateCountText();
	}

	public void SetItemTID(int a_itemTID)
	{
		m_itemTID = a_itemTID;
		base.Init(AssetCacher.Instance.CacheAsset<AbilityTemplate>(m_itemTID));
		UIUtils.SetActive(m_icon, m_itemTID != tid.NULL);
	}

	private void UpdateCountText()
	{
		m_countText.text = "x" + m_count;
	}

	public void AddCount(int a_add)
	{
		m_count += a_add;
		UpdateCountText();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnClicked()
	{
		if (m_isBackpackSlot)
		{
			m_parent.OnItemRemoved(this);
			SetItemTID(tid.NULL);
			UIUtils.SetActive(m_icon, false);
			UIManager.Instance.CloseUI(GameManager.Instance.UISettings.TooltipPopup);
		}
		else
		{
			if (m_count > 0)
			{
				AddCount(-1);
			}
			m_parent.OnItemSelected(m_itemTID);
		}

	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}