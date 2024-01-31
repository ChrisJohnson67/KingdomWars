using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ItemSelectPanel
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ItemSelectPanel : LevelSelectPanel
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

	[SerializeField, TemplateIDField(typeof(ItemSelectEntry), "Item Choice Entry", "")]
	protected int m_itemEntryTID;

	[SerializeField]
	private List<ItemSelectEntry> m_backpackSlotEntries;

	//--- NonSerialized ---
	private List<ItemSelectEntry> m_entries = new List<ItemSelectEntry>();

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

		m_confirmButton.SetInteractive(false);

		var itemTIDs = GameManager.Instance.KingdomSettings.ItemTemplateTIDs;
		int index = 0;
		foreach (var itemTID in itemTIDs)
		{
			var entry = AssetCacher.Instance.InstantiateComponent<ItemSelectEntry>(m_itemEntryTID);
			if (entry != null)
			{
				entry.Init(this, itemTID, index);
				entry.transform.SetParent(m_listParent, false);
				m_entries.Add(entry);
				index++;
			}
		}

		index = 0;
		foreach (var slot in m_backpackSlotEntries)
		{
			slot.Init(this, tid.NULL, index);
			index++;
		}
	}

	public void OnItemSelected(int a_itemTID)
	{
		for (int i = 0; i < m_backpackSlotEntries.Count; i++)
		{
			if (m_backpackSlotEntries[i].ItemTID == tid.NULL)
			{
				m_backpackSlotEntries[i].SetItemTID(a_itemTID);
				break;
			}
		}

		bool allItems = true;
		foreach (var slot in m_backpackSlotEntries)
		{
			if (slot.ItemTID == tid.NULL)
			{
				allItems = false;
				break;
			}
		}
		m_confirmButton.SetInteractive(allItems);
	}

	public void OnItemRemoved(ItemSelectEntry a_entry)
	{
		m_confirmButton.SetInteractive(false);

		var entry = m_entries.Find(x => x.ItemTID == a_entry.ItemTID);
		if (entry != null)
		{
			entry.AddCount(1);
		}
	}

	public override void OnBackButton()
	{
		Destroy(gameObject);
		m_parent.OpenHeroSelectPanel();
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirmButton()
	{
		List<int> items = new List<int>();
		foreach (var slot in m_backpackSlotEntries)
		{
			items.Add(slot.ItemTID);
		}
		m_parent.StartCombat(items);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}