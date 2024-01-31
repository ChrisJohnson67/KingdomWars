using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DisplayTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "Assets/Display Template")]
public class DisplayTemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions
	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[Header("Display Settings")]

	[SerializeField]
	protected string m_displayName;

	[SerializeField]
	protected string m_description;

	[SerializeField]
	protected Sprite m_displayIcon;

	[SerializeField]
	protected string m_defaultTooltipDescription;

	[SerializeField, TemplateIDField(typeof(BuffTemplate), "Buff Templates To Display", "")]
	protected List<int> m_buffsToDisplay;

	[SerializeField]
	protected Color m_color = Color.white;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public string DisplayName { get { return m_displayName; } }
	public string Description { get { return m_description; } }
	public Sprite DisplayIcon { get { return m_displayIcon; } }
	public Color Color { get { return m_color; } }
	public List<int> BuffsToDisplay { get { return m_buffsToDisplay; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public virtual string GetTooltipDesc()
	{
		return m_defaultTooltipDescription;
	}

	#endregion Runtime Functions

}