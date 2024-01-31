using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// StatEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class StatEntry : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Image m_icon;

	[SerializeField]
	private TMP_Text m_amtText;


	//--- NonSerialized ---
	private StatInstance m_stat;
	private UnitStatTemplate.UnitBaseStatData m_statData;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnDestroy()
	{
		if (m_stat != null)
		{
			m_stat.OnChanged -= Refresh;
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(StatInstance a_stat)
	{
		m_stat = a_stat;
		m_icon.sprite = a_stat.Template.DisplayIcon;

		m_stat.OnChanged += Refresh;
		Refresh();
	}

	public void InitTemplate(UnitStatTemplate.UnitBaseStatData a_stat)
	{
		m_statData = a_stat;
		m_icon.sprite = a_stat.StatTemplate.DisplayIcon;


		string percent = a_stat.StatTemplate.MultiplyInUI ? " %" : string.Empty;
		if (a_stat.StatTemplate.ShowDecimal)
		{
			m_amtText.text = a_stat.BaseAmount.ToString("0.00") + percent;
		}
		else
		{
			m_amtText.text = ((int)(a_stat.BaseAmount)).ToString() + percent;
		}
	}

	private void Refresh()
	{
		string percent = m_stat.Template.MultiplyInUI ? " %" : string.Empty;
		if (m_stat.Template.ShowDecimal)
		{
			m_amtText.text = m_stat.CurrentAmount.ToString("0.00") + percent;
		}
		else
		{
			m_amtText.text = ((int)(m_stat.CurrentAmount)).ToString() + percent;
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnTooltip(TooltipInfo a_info)
	{
		if (m_statData != null)
		{
			a_info.Desc = m_statData.StatTemplate.DisplayName;
		}
		else if (m_stat != null)
		{
			a_info.Desc = m_stat.Template.DisplayName;
		}
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}