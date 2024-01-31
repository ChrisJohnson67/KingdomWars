using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// KingdomData
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[Serializable]
public class KingdomRoomData
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, TemplateIDField(typeof(RoomCoreTemplate), "Room Core", "")]
	private int m_coreTID;

	[SerializeField, TemplateIDField(typeof(MonsterGroupTemplate), "Monsters", "")]
	private int m_monsterGroupTID;

	[SerializeField, TemplateIDField(typeof(RoomSpellTemplate), "Spell", "")]
	private int m_spellTID;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int CoreTID { get { return m_coreTID; } set { m_coreTID = value; } }
	public int MonsterGroupTID { get { return m_monsterGroupTID; } set { m_monsterGroupTID = value; } }
	public int SpellTID { get { return m_spellTID; } set { m_spellTID = value; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public List<int> GetDangers()
	{
		List<int> dangerList = new List<int>();
		var template = AssetCacher.Instance.CacheAsset<RoomCoreTemplate>(m_coreTID);
		if(template != null)
		{
			template.GetDangers(dangerList);
		}

		var mtemplate = AssetCacher.Instance.CacheAsset<MonsterGroupTemplate>(m_monsterGroupTID);
		if (mtemplate != null)
		{
			mtemplate.GetDangers(dangerList);
		}

		var stemplate = AssetCacher.Instance.CacheAsset<RoomSpellTemplate>(m_spellTID);
		if (stemplate != null)
		{
			stemplate.GetDangers(dangerList);
		}
		return dangerList;
	}

	public void SerializeData(StringBuilder a_builder)
	{
		a_builder.Append(m_coreTID).Append(KingdomData.c_Delim);
		a_builder.Append(m_monsterGroupTID).Append(KingdomData.c_Delim);
		a_builder.Append(m_spellTID).Append(KingdomData.c_Delim);
	}

	public static KingdomData DeserializeData(string a_data)
	{
		KingdomData kingdom = new KingdomData();
		kingdom.Deserialize(a_data);
		return kingdom;
	}

	public void Deserialize(string[] a_data, ref int a_index)
	{
		m_coreTID = Int32.Parse(a_data[a_index++]);
		m_monsterGroupTID = Int32.Parse(a_data[a_index++]);
		m_spellTID = Int32.Parse(a_data[a_index++]);
	}

	public static KingdomData CreateNewKingdomToPublish()
	{
		KingdomData kingdom = new KingdomData();
		var levelNumber = UnityEngine.Random.Range(1, Int32.MaxValue);
		kingdom.SetLevelId(levelNumber);
		return kingdom;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks


}
