using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// GlowButton
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class GlowButton : Button, IPointerEnterHandler, IPointerExitHandler
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private GameObject m_glowObject;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public GameObject GlowObject { get { return m_glowObject; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	protected override void Awake()
	{
		base.Awake();
		UIUtils.SetActive(m_glowObject, false);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		UIUtils.SetActive(m_glowObject, false);
	}

	public override void OnPointerEnter(PointerEventData a_eventData)
	{
		base.OnPointerEnter(a_eventData);

		if (interactable)
			UIUtils.SetActive(m_glowObject, true);
	}

	public override void OnPointerExit(PointerEventData a_eventData)
	{
		base.OnPointerExit(a_eventData);

		UIUtils.SetActive(m_glowObject, false);
	}

	public void GetTooltipInfo(TooltipInfo a_info)
	{
		a_info.Name = "tooltip name";
		a_info.Desc = "this is my tooltip desc";
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR

	protected override void OnValidate()
	{
		base.OnValidate();

		if (m_glowObject == null)
		{
			var node = transform.FindNode("glow");
			if (node != null)
			{
				m_glowObject = node.gameObject;
			}
		}

	}

#endif
}
