using Platform.UIManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for full screen UI's. Enabling/Disabling a canvas on the UI is much quicker than enabling/disabling gameObjects
/// </summary>
[RequireComponent(typeof(UIPrefab), typeof(Canvas), typeof(GraphicRaycaster))]
public abstract class FullscreenUI : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions
	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[Header("Fullscreen Settings")]

	[SerializeField]
	protected Canvas m_canvas;

	[SerializeField]
	protected GraphicRaycaster m_graphicRaycaster;

	[ReorderableListField(typeof(FullscreenUI), "m_scriptsToDisable", "Scripts to Disable", "")]
	[ReorderableListField(typeof(FullscreenUI), "m_gameObjectsToHide", "GameObjects to Hide", "")]

	[SerializeField]
	protected Animator m_animator;

	[SerializeField, HideInInspector, Tooltip("GameObjects to disable when this UI is hidden")]
	protected List<GameObject> m_gameObjectsToHide = new List<GameObject>();

	[SerializeField, HideInInspector, Tooltip("Scripts to disable when this UI is hidden")]
	protected List<Behaviour> m_scriptsToDisable = new List<Behaviour>();

	//--- NonSerialized ---
	protected bool m_closing = false;
	protected bool m_hiding = false;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	/// <summary>
	/// Called the first frame this GameObject is active.
	/// </summary>
	protected virtual void OnEnable()
	{
		FullScreenManager.Instance.AddFullscreenToStack(this);
		m_closing = false;
	}

	/// <summary>
	/// Called when this GameObject is inactive.
	/// </summary>
	protected virtual void OnDisable()
	{
		if (FullScreenManager.HasInstance)
			FullScreenManager.Instance.RemoveFullscreenFromStack(this);
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	/// <summary>
	/// Disable the canvas component to hide all elements in this screen. Disable the listed scripts. Tell the main hud to show/hide
	/// </summary>
	/// <param name="a_hide"></param>
	public virtual void HideUI(bool a_hide)
	{
		if (m_closing)
		{
			return;
		}
		if (m_hiding != a_hide)
		{
			m_hiding = a_hide;
			m_canvas.SetEnabled(!a_hide);
			for (int i = 0; i < m_gameObjectsToHide.Count; i++)
			{
				UIUtils.SetActive(m_gameObjectsToHide[i], !a_hide);
			}

			for (int i = 0; i < m_scriptsToDisable.Count; i++)
			{
				var script = m_scriptsToDisable[i];
				script.SetEnabled(!a_hide);
			}

			EnableInteraction(!a_hide);
		}
	}

	/// <summary>
	/// Enable/disable button clicks for this UI
	/// </summary>
	/// <param name="a_enable"></param>
	public virtual void EnableInteraction(bool a_enable)
	{
		if (m_graphicRaycaster != null)
			m_graphicRaycaster.enabled = a_enable;
	}

	/// <summary>
	/// Destroy objects created by this UI or release pooled objects
	/// </summary>
	protected virtual void Clear()
	{

	}

	/// <summary>
	/// Close this UI. UI is not immediately destroyed if it has a close animation
	/// </summary>
	public virtual void CloseUI()
	{
		if (!m_closing)
		{
			m_closing = true;

			EnableInteraction(false);

			Clear();

			UIManager.Instance.CloseUI(this);
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR

	protected virtual void OnValidate()
	{
		m_canvas = GetComponent<Canvas>();
		if (m_canvas == null)
		{
			m_canvas = gameObject.AddComponent<Canvas>();
		}
		m_graphicRaycaster = GetComponent<GraphicRaycaster>();
		if (m_graphicRaycaster == null)
		{
			m_graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
		}
	}

#endif
}
