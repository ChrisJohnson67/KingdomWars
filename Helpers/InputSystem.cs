//#define KI_UNITY_REMOTE
using Platform.Logging;
using Platform.Utility;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoSingleton<InputSystem>
{
	private static readonly LogSubSystem SubSys = LogSubSystem.Create("InputSystem", LogLevel.All);

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, Tooltip("Multiplier to the zoom/pinch for PC scroll wheel")]
	private float m_zoomScrollWheelMultiplier = 1f;

	[SerializeField, Tooltip("Multiplier to the zoom/pinch for PC keyboard")]
	private float m_zoomKeyboardWheelMultiplier = 1f;

	//--- Private Serialized ---

	private InputPlatform m_inputPlatform;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public float ZoomScrollWheelMultiplier { get { return m_zoomScrollWheelMultiplier; } set { m_zoomScrollWheelMultiplier = value; } }
	public float ZoomKeyboardWheelMultiplier { get { return m_zoomKeyboardWheelMultiplier; } set { m_zoomKeyboardWheelMultiplier = value; } }
	public Vector2 Acceleration { get { return m_inputPlatform.Acceleration; } }
	public bool AccelerometerEnabled { get; set; } = true;

	#endregion Accessors

	// Touch Events-------------------------------------------------------------------
	public static Action<Vector2> TouchDownEvent { get; set; }
	public static Action<Vector2> TouchUpEvent { get; set; }
	public static Action<Vector2> TouchMovedEvent { get; set; }

	// Multi-Touch Input---------------------------------------------------------------
	public static Action<Vector2, Vector2> MultiTouchMovedEvent { get; set; }
	public static Action MultiTouchStartEvent { get; set; }
	public static Action MultiTouchEndedEvent { get; set; }

	// Mouse Input (PC only)-----------------------------------------------------------
	public static Action<float> MouseScrollWheelEvent { get; set; }

	//Android back/PC Esc-------------------------------
	public static Action OnBackEvent { get; set; }

	void Start()
	{
#if UNITY_EDITOR && KI_UNITY_REMOTE
		m_inputPlatform = new InputPlatformMobile();
#elif UNITY_EDITOR
		m_inputPlatform = new InputPlatformDesktop();
#else
		switch (Application.platform)
		{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				{
					m_inputPlatform = new InputPlatformDesktop();
					break;
				}
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				{
					m_inputPlatform = new InputPlatformMobile();
					break;
				}
			default:
				{
					log.Debug(SubSys.Error() + "Invalid Platform detected (" + Application.platform + ")");
					break;
				}
		}
#endif
	}

	private void Update()
	{
		m_inputPlatform.Update();
	}

	private static void OnTouchDown(Vector2 a_pos)
	{
		if (TouchDownEvent != null)
			TouchDownEvent(a_pos);
	}

	private static void OnTouchUp(Vector2 a_pos)
	{
		if (TouchUpEvent != null)
			TouchUpEvent(a_pos);
	}

	private static void OnTouchMoved(Vector2 a_p)
	{
		if (TouchMovedEvent != null)
			TouchMovedEvent(a_p);
	}

	private static void OnMultiTouchMoved(Vector2 a_pinchPoint1, Vector2 a_pinchPoint2)
	{
		if (MultiTouchMovedEvent != null)
			MultiTouchMovedEvent(a_pinchPoint1, a_pinchPoint2);
	}

	private static void OnScrollWheel(float a_scrollWheelValue)
	{
		if (MouseScrollWheelEvent != null)
		{
			MouseScrollWheelEvent(a_scrollWheelValue);
		}
	}

	private static void OnMultiTouchStart()
	{
		if (MultiTouchStartEvent != null)
		{
			MultiTouchStartEvent();
		}
	}

	private static void OnMultiTouchEnded()
	{
		if (MultiTouchEndedEvent != null)
			MultiTouchEndedEvent();
	}

	#region InputPlatform

	[Serializable]
	public class InputPlatform
	{
		protected bool m_isMultiTouch;
		protected Vector2 m_accelerator = Vector2.zero;

		public Vector2 Acceleration { get { return m_accelerator; } }

		public virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (OnBackEvent != null)
				{
					OnBackEvent();
				}
			}
		}
	}

	[Serializable]
	public class InputPlatformDesktop : InputPlatform
	{
		// For Keyboard input
		private const float SCROLL_WHEEL_DEAD_ZONE = .002f;

		public override void Update()
		{
			base.Update();

			if (Input.GetMouseButtonDown(0))
			{
				if (!IsOverUI())
				{
					OnTouchDown(Input.mousePosition);
				}
			}
			else if (Input.GetMouseButton(0))
			{
				OnTouchMoved(Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (!IsOverUI())
				{
					OnTouchUp(Input.mousePosition);
				}
			}

			float scrollWheelAxis = Input.GetAxis("Mouse ScrollWheel");
			if (Input.GetKey(KeyCode.UpArrow))
			{
				OnScrollWheel(Instance.ZoomKeyboardWheelMultiplier);
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				OnScrollWheel(Instance.ZoomKeyboardWheelMultiplier * -1);
			}
			else if (Mathf.Abs(scrollWheelAxis) > SCROLL_WHEEL_DEAD_ZONE)
			{
				float zoom = scrollWheelAxis * Instance.ZoomScrollWheelMultiplier;
				OnScrollWheel(zoom);
			}

			// Dampen the accelerator without inputs
			m_accelerator *= 0.5f;

			if (Input.GetKey(KeyCode.RightArrow))
				m_accelerator.x = 1;
			if (Input.GetKey(KeyCode.LeftArrow))
				m_accelerator.x = -1;
			if (Input.GetKey(KeyCode.UpArrow))
				m_accelerator.y = 1;
			if (Input.GetKey(KeyCode.DownArrow))
				m_accelerator.y = -1;
		}

		private bool IsOverUI()
		{
			if (EventSystem.current == null)
			{
				return false;
			}

			return EventSystem.current.IsPointerOverGameObject();
		}
	}

	[Serializable]
	public class InputPlatformMobile : InputPlatform
	{
		public override void Update()
		{
			m_accelerator = Input.acceleration;

			base.Update();

			if (Input.touchCount >= 2)
			{
				HandleMultiTouch();
				return;
			}
			else if (m_isMultiTouch)
			{
				OnMultiTouchEnded();
				m_isMultiTouch = false;
			}

			if (Input.touchCount > 0)
			{
				var touch1 = Input.GetTouch(0);
				switch (touch1.phase)
				{
					case TouchPhase.Began:
						{
							if (!IsOverUI())
							{
								OnTouchDown(touch1.position);
							}
							break;
						}
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						{
							OnTouchMoved(touch1.position);
							break;
						}
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
						{
							if (!IsOverUI())
							{
								OnTouchUp(touch1.position);
							}
							break;
						}
				}
			}
		}

		private void HandleMultiTouch()
		{
			if (m_isMultiTouch == false)
			{
				OnMultiTouchStart();
			}
			m_isMultiTouch = true;
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);

			OnMultiTouchMoved(touch1.position, touch2.position);
		}

		private bool IsOverUI()
		{
			if (Input.touchCount <= 0)
				return false;

			Touch touch1 = Input.GetTouch(0);
			return EventSystem.current.IsPointerOverGameObject(touch1.fingerId);
		}
	}

	#endregion
}
