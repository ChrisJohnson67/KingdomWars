using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// TooltipPopupUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class TooltipPopupUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum TooltipHorzPosition
	{
		Left,
		Right
	}

	public enum TooltipVertPosition
	{
		Upper,
		Lower
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_nameText;

	[SerializeField]
	private TMP_Text m_descText;

	[SerializeField]
	private Vector2 m_offsetValues = new Vector2(10, 10);

	[SerializeField, TemplateIDField(typeof(BuffTooltipDisplay), "Buff tooltip", "")]
	private int m_buffDisplayTID;

	[SerializeField]
	private VerticalLayoutGroup m_buffParent;

	[SerializeField]
	private Transform m_buffRightPArentParent;

	[SerializeField]
	private Transform m_buffLeftParentParent;

	[SerializeField]
	private Transform m_buffMiddleParent;

	[SerializeField]
	private Transform m_buffPanelObject;

	//--- NonSerialized ---
	private Vector2 m_lastMousePos = Vector2.zero;
	private RectTransform m_rectTransform;
	private bool m_hidingPanel;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private void Awake()
	{
		m_rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		UpdateTooltipPosition();
	}

	public void InitFromTemplate(DisplayTemplate a_template)
	{
		if (a_template != null)
		{
			SetTexts(a_template.DisplayName, a_template.GetTooltipDesc());
		}
	}

	public void InitFromInstance(string a_name, string a_desc, List<int> a_buffs)
	{
		if (a_buffs != null)
		{
			foreach (var buffTID in a_buffs)
			{
				var entry = AssetCacher.Instance.InstantiateComponent<BuffTooltipDisplay>(m_buffDisplayTID, m_buffParent.transform);
				entry.Init(AssetCacher.Instance.CacheAsset<BuffTemplate>(buffTID));
			}

			print("creating instance");
		}

		if (string.IsNullOrEmpty(a_name) && string.IsNullOrEmpty(a_desc))
		{
			UIUtils.SetActive(m_buffPanelObject, false);
			m_hidingPanel = true;
		}

		SetTexts(a_name, a_desc);

		UpdateTooltipPosition();
	}

	private void SetTexts(string a_name, string a_desc)
	{
		if (m_nameText != null)
		{
			m_nameText.text = a_name;
		}
		if (m_descText != null)
		{
			m_descText.text = a_desc;
		}
	}

	private void UpdateTooltipPosition()
	{
		var newMousePos = Input.mousePosition;
		if (m_lastMousePos != (Vector2)newMousePos)
		{
			var width = m_rectTransform.rect.width;
			var height = m_rectTransform.rect.height;
			var horzPosition = newMousePos.x < Screen.width * 0.5f ? TooltipHorzPosition.Right : TooltipHorzPosition.Left;
			var vertPosition = Screen.height - newMousePos.y > Screen.height * 0.5f ? TooltipVertPosition.Upper : TooltipVertPosition.Lower;

			if (newMousePos.x > Screen.width / 2f)
			{
				horzPosition = TooltipHorzPosition.Left;
			}
			else if (newMousePos.x < Screen.width / 2f)
			{
				horzPosition = TooltipHorzPosition.Right;
			}

			if (m_hidingPanel)
			{
				m_buffParent.transform.SetParent(m_buffMiddleParent, false);
				m_buffParent.childAlignment = TextAnchor.MiddleLeft;
				horzPosition = TooltipHorzPosition.Left;
			}
			else if (newMousePos.x > Screen.width / 2f)
			{
				m_buffParent.transform.SetParent(m_buffLeftParentParent, false);
				m_buffParent.childAlignment = TextAnchor.MiddleRight;
			}
			else if (newMousePos.x < Screen.width / 2f)
			{
				m_buffParent.transform.SetParent(m_buffRightPArentParent, false);
				m_buffParent.childAlignment = TextAnchor.MiddleLeft;
			}


			if (Screen.height - newMousePos.y + height > Screen.height)
			{
				vertPosition = TooltipVertPosition.Upper;
			}
			else if (Screen.height - newMousePos.y - height < 0f)
			{
				vertPosition = TooltipVertPosition.Lower;
			}

			SetPosition(horzPosition, vertPosition);
			m_lastMousePos = (Vector2)newMousePos;
		}
	}

	private void SetPosition(TooltipHorzPosition a_horzPos, TooltipVertPosition a_vertPos)
	{
		var newMousePos = Input.mousePosition;
		float horzMultiplier = a_horzPos == TooltipHorzPosition.Left ? -1f : 1f;
		float vertMultiplier = a_vertPos == TooltipVertPosition.Lower ? -1f : 1f;
		Vector2 offset = new Vector2(m_rectTransform.rect.width * 0.5f * horzMultiplier + m_offsetValues.x * horzMultiplier, 0f);
		offset += new Vector2(0f, m_rectTransform.rect.height * 0.5f * vertMultiplier + m_offsetValues.y * vertMultiplier);

		RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.CanvasParent, newMousePos, null, out Vector2 localPoint);
		transform.localPosition = localPoint + offset;
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
