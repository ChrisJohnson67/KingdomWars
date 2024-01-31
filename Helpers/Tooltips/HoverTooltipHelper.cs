using Platform.UIManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// HoverTooltip
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[RequireComponent(typeof(ClickArea))]
public class HoverTooltipHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---

	[SerializeField, TemplateIDField(typeof(TooltipPopupUI), "Tooltip OVerride", "")]
	private int m_overridePopupTID;


	[SerializeField]
	private string m_text;

	[SerializeField, TemplateIDField(typeof(DisplayTemplate), "Tooltip Template", "")]
	private int m_tooltipTemplate;

	[SerializeField]
	private UnityEvent<TooltipInfo> m_tooltipScript;

	[SerializeField]
	private GlowButton m_glowButton;

	//--- NonSerialized ---
	private TooltipPopupUI m_popup;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	void OnDisable()
	{
		OnPointerExit(null);
	}

	public void OnPointerEnter(PointerEventData a_eventData)
	{
		if (m_tooltipTemplate == tid.NULL && string.IsNullOrEmpty(m_text) && (m_tooltipScript == null || m_tooltipScript.GetPersistentEventCount() == 0))
			return;

		int popupTID = GameManager.Instance.UISettings.TooltipPopup;
		if (m_overridePopupTID != tid.NULL)
		{
			popupTID = m_overridePopupTID;
		}

		if (!string.IsNullOrEmpty(m_text))
		{
			m_popup = UIManager.Instance.OpenUI<TooltipPopupUI>(popupTID);
			m_popup.InitFromInstance(string.Empty, m_text, null);
		}
		else if (m_tooltipTemplate != tid.NULL)
		{
			m_popup = UIManager.Instance.OpenUI<TooltipPopupUI>(popupTID);
			m_popup.InitFromTemplate(AssetCacher.Instance.CacheAsset<DisplayTemplate>(m_tooltipTemplate));
		}
		else if (m_tooltipScript != null)
		{
			var tooltipInfo = new TooltipInfo();
			m_tooltipScript?.Invoke(tooltipInfo);
			if (!tooltipInfo.DontShow)
			{
				m_popup = UIManager.Instance.OpenUI<TooltipPopupUI>(popupTID);
				m_popup.InitFromInstance(tooltipInfo.Name, tooltipInfo.Desc, tooltipInfo.BuffTemplates);
			}

		}

		if (m_glowButton != null)
		{
			m_glowButton.OnPointerEnter(a_eventData);
		}
	}

	public void OnPointerExit(PointerEventData a_eventData)
	{
		if (m_popup != null)
		{
			Destroy(m_popup.gameObject);
			m_popup = null;
		}

	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR

	protected virtual void OnValidate()
	{
	}
#endif
}
