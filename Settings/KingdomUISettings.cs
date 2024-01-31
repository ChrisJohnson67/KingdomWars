using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UISettings
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Settings related to the UI.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/KingdomUISettings")]
public class KingdomUISettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, TemplateIDField(typeof(LoadingScreenUI), "Loading Screen", "")]
	private int m_loadingScreenTID;

	[SerializeField, TemplateIDField(typeof(TooltipPopupUI), "Tooltip Popup", "")]
	private int m_tooltipPopupTID;

	[SerializeField, TemplateIDField(typeof(GenericPopupUI), "Generic Popup", "")]
	private int m_genericPopupTID;

	[SerializeField, TemplateIDField(typeof(HealthbarUI), "Healthbar UI", "")]
	private int m_healthbarTID;

	[SerializeField, TemplateIDField(typeof(RoomNamePopup), "Room name popup UI", "")]
	private int m_roomNamePopupTID;

	[SerializeField, TemplateIDField(typeof(TitleScreenUI), "Title screen UI", "")]
	private int m_titleScreenTID;

	[SerializeField, TemplateIDField(typeof(RoomCreationUI), "RoomCreationUI ", "")]
	private int m_roomCreationTID;

	[SerializeField, TemplateIDField(typeof(CombatUI), "Combat UI", "")]
	private int m_combatUITID;

	[SerializeField, TemplateIDField(typeof(ChooseNamePopup), "Name popup UI", "")]
	private int m_namePopupTID;

	[SerializeField, TemplateIDField(typeof(LevelSelectUI), "Level Select UI", "")]
	private int m_levelSelectUITID;

	[SerializeField, TemplateIDField(typeof(FloatingDamageUI), "Floating damage UI", "")]
	private int m_floatingDamageTID;

	[SerializeField, TemplateIDField(typeof(LeaderboardUI), "Leaderboard UI", "")]
	private int m_leaderboardUITID;

	[SerializeField, TemplateIDField(typeof(VictoryUI), "VictoryUI", "")]
	private int m_vicotryUITID;

	[SerializeField, TemplateIDField(typeof(NotificationPopup), "Notif popup", "")]
	private int m_notifTID;

	[SerializeField, TemplateIDField(typeof(MyLevelsUI), "MyLevelsUI", "")]
	private int m_levelUI;

	[SerializeField, TemplateIDField(typeof(CombatStartPopup), "Combat StartPopup", "")]
	private int m_combatStartPopup;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int TooltipPopup { get { return m_tooltipPopupTID; } }
	public int LoadingScreenTID { get { return m_loadingScreenTID; } }
	public int GenericPopupUITID { get { return m_genericPopupTID; } }
	public int HealthbarTID { get { return m_healthbarTID; } }
	public int RoomNamePopupUITID { get { return m_roomNamePopupTID; } }
	public int TitleScreenTID { get { return m_titleScreenTID; } }
	public int RoomCreationTID { get { return m_roomCreationTID; } }
	public int CombatUITID { get { return m_combatUITID; } }
	public int NamePopupTID { get { return m_namePopupTID; } }
	public int LevelSelectUITID { get { return m_levelSelectUITID; } }
	public int FloatingDamageTID { get { return m_floatingDamageTID; } }
	public int LeaderboardUITID { get { return m_leaderboardUITID; } }
	public int VicotryUITID { get { return m_vicotryUITID; } }
	public int NotifTID { get { return m_notifTID; } }
	public int LevelUI { get { return m_levelUI; } }
	public int CombatStartPopup { get { return m_combatStartPopup; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	#endregion Runtime Functions
}
