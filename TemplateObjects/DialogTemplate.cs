using System;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DialogTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu]
public class DialogTemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	[Serializable]
	public class DialogueBoxData
	{
		[SerializeField]
		private DisplayTemplate m_speaker;

		[SerializeField]
		private LocalizationText m_text;

		public DisplayTemplate Speaker { get { return m_speaker; } }
		public LocalizationText Text { get { return m_text; } }
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<DialogueBoxData> m_dialogueDataList;

	[SerializeField]
	private float m_waitTimeBetweenDialogues = 0f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int DialogueListCount { get { return m_dialogueDataList.Count; } }

	public float WaitTimeBetweenDialogues { get => m_waitTimeBetweenDialogues; }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public DialogueBoxData GetDialogueDataForIndex(int a_index)
	{
		if (a_index < 0 || a_index >= m_dialogueDataList.Count)
			return null;

		return m_dialogueDataList[a_index];
	}

	#endregion Runtime Functions

}