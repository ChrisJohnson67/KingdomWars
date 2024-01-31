using Platform.Utility;
using System.Collections.Generic;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// FullscreenManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Manages fullscreen UIs
/// </summary>
public class FullScreenManager : MonoSingleton<FullScreenManager>
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---

	//--- NonSerialized ---
	private List<FullscreenUI> m_fullscreens = new List<FullscreenUI>();


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public FullscreenUI CurrentFullscreenShowing { get { return m_fullscreens.Count > 0 ? m_fullscreens[0] : null; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages


	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	/// <summary>
	/// Inserts a fullscreen into the front of the list. Hides all fullscreens currently showing
	/// </summary>
	public void AddFullscreenToStack(FullscreenUI a_ui)
	{
		if (a_ui == null)
		{
			return;
		}
		//hide all current fullscreens open
		for (int i = 0; i < m_fullscreens.Count; i++)
		{
			var fullscreen = m_fullscreens[i];
			if (fullscreen != null)
			{
				fullscreen.HideUI(true);
			}
		}

		//check if the fullscreen is already in the stack. If it is, remove it, then re-insert it
		int index = m_fullscreens.IndexOf(a_ui);
		if (index >= 0)
		{
			m_fullscreens.RemoveAt(index);
		}
		m_fullscreens.Insert(0, a_ui);
	}

	/// <summary>
	/// Removes a fullscreen from the list. If it was the top level fullscreen, unhide the next fullscreen in the list
	/// </summary>
	public void RemoveFullscreenFromStack(FullscreenUI a_ui)
	{
		if (a_ui == null)
		{
			return;
		}
		//remove the fullscreen from the stack and un-hide the next one
		int index = m_fullscreens.IndexOf(a_ui);

		if (index >= 0)
		{
			//show the next fullscreen if the top level fullscreen is closing
			if (m_fullscreens.Count > 1 && index == 0)
			{
				m_fullscreens[1].HideUI(false);
			}

			m_fullscreens.RemoveAt(index);
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
