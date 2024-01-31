using TMPro;
using UnityEngine;
using UnityEngine.UI;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BuffEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class BuffEntry : MonoBehaviour
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
	private TMP_Text m_buffStackAmount;

	[SerializeField]
	private Image m_durationFill;

	//--- NonSerialized ---
	private HealthbarUI m_parent;
	private BuffInstance m_buffInstance;
	private float m_originalDuration;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(HealthbarUI a_parent, BuffInstance a_buffInstance)
	{
		m_parent = a_parent;
		m_buffInstance = a_buffInstance;

		m_icon.sprite = m_buffInstance.Template.DisplayIcon;
		UIUtils.SetActive(m_buffStackAmount, m_buffInstance.Template.CanStack);
		UIUtils.SetActive(m_durationFill, m_buffInstance.DurationRemaining > 0f);
		m_originalDuration = m_buffInstance.DurationRemaining;

		Refresh();

		m_buffInstance.OnBuffRemoved += OnRemoved;
	}

	private void Update()
	{
		if (m_buffInstance.DurationRemaining > 0f && m_originalDuration > 0f)
		{
			m_durationFill.fillAmount = m_buffInstance.DurationRemaining / m_originalDuration;
		}
	}

	public void Refresh()
	{
		if (m_buffInstance.Template.CanStack)
		{
			m_buffStackAmount.text = m_buffInstance.Stacks.ToString();
		}
	}

	public void OnRemoved()
	{
		m_buffInstance.OnBuffRemoved -= OnRemoved;
		m_parent.RemoveBuffEntry(this);
	}

	public void Clear()
	{
	}

	public void OnShowTooltip(TooltipInfo a_info)
	{
		a_info.Name = m_buffInstance.Template.DisplayName;
		a_info.Desc = m_buffInstance.Template.GetTooltipDesc();
	}

	#endregion Runtime Functions
}
