using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DangerDisplay
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DangerDisplay : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected List<DangerParent> m_entryParents;

	[SerializeField, TemplateIDField(typeof(DangerEntry), "Entry", "")]
	private int m_dangerEntryTID;


	//--- NonSerialized ---
	protected List<DangerEntry> m_entries = new List<DangerEntry>();
	protected List<DangerParent> m_sortedParents;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		foreach (var parent in m_entryParents)
		{
			UIUtils.SetActive(parent, false);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void InitList(List<int> a_dangers, bool a_preSorted = false)
	{
		m_sortedParents = new List<DangerParent>(m_entryParents);
		foreach (var danger in a_dangers)
		{
			var parent = GetDangerParent(danger);
			if (parent != null && (!parent.DisplayCount || (parent.DisplayCount && !parent.HasDangerEntry)))
			{
				var entry = GameObjectPool.GetFromPool<DangerEntry>(m_dangerEntryTID);
				if (entry != null)
				{
					entry.transform.SetParent(parent.transform, false);
					if (parent.DisplayCount)
						entry.transform.SetAsFirstSibling();

					entry.Init(danger);
					UIUtils.SetActive(entry, true);
					m_entries.Add(entry);
				}
			}
		}

		if (!a_preSorted)
		{
			m_sortedParents.Sort(SortEntries);
			foreach (var parent in m_sortedParents)
			{
				parent.transform.SetAsLastSibling();
			}
		}
	}

	private int SortEntries(DangerParent a_left, DangerParent a_right)
	{
		if (a_left.Count == a_right.Count)
		{
			return a_left.DangerTID.CompareTo(a_right.DangerTID);
		}
		return a_right.Count.CompareTo(a_left.Count);
	}

	private DangerParent GetDangerParent(int a_dangerTID)
	{
		var parentInfo = m_entryParents.Find(x => x.DangerTID == a_dangerTID);
		if (parentInfo != null)
		{
			if (!parentInfo.CanDisplayMore())
			{
				return null;
			}

			parentInfo.AddDanger(a_dangerTID);
			return parentInfo;
		}

		//find next available parent
		foreach (var parent in m_entryParents)
		{
			if (parent.DangerTID == tid.NULL)
			{
				parent.AddDanger(a_dangerTID);
				return parent;
			}
		}
		return null;
	}

	public void Clear()
	{
		foreach (var parent in m_entryParents)
		{
			parent.Clear();
			UIUtils.SetActive(parent, false);
		}

		foreach (var entry in m_entries)
		{
			GameObjectPool.ReleaseToPool(entry);
		}
		m_entries.Clear();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}