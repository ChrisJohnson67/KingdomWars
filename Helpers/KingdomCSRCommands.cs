using Platform.CSR;
using Platform.UIManagement;

public partial class CSRCommands
{
	[CSRCommand]
	protected void StartServer()
	{
		KingdomNetworkingManager.Instance.HostServer();
	}

	[CSRCommand]
	protected void TestBattle()
	{
		var kingdomData = KingdomData.CreateNewKingdomToPublish();
		kingdomData.SetTextDetails("Test", "Kingdom Test");
		for (int i = 0; i < GameManager.Instance.KingdomSettings.RoomNumber; i++)
		{
			var roomData = new KingdomRoomData();
			roomData.CoreTID = GameManager.Instance.KingdomSettings.RoomCoreTIDs[0];
			roomData.MonsterGroupTID = GameManager.Instance.KingdomSettings.MonsterGroupTIDs[0];
			roomData.SpellTID = GameManager.Instance.KingdomSettings.RoomSpellTIDs[0];
			kingdomData.RoomDataList.Add(roomData);
		}

		var combatContextData = new CombatContextData();
		combatContextData.HeroTemplate = AssetCacher.Instance.CacheAsset<UnitTemplate>(GameManager.Instance.KingdomSettings.HeroTemplateTIDs[0]);
		combatContextData.KingdomData = kingdomData;
		var titleScreen = UIManager.Instance.GetUI<TitleScreenUI>();
		titleScreen.CloseUI();
		GameManager.Instance.StartCombat(combatContextData);
	}

	[CSRCommand]
	protected void TestPublishLevel(string a_name)
	{
		var kingdomData = KingdomData.CreateNewKingdomToPublish();
		kingdomData.SetTextDetails(a_name, a_name);
		for (int i = 0; i < GameManager.Instance.KingdomSettings.RoomNumber; i++)
		{
			var roomData = new KingdomRoomData();
			roomData.CoreTID = GameManager.Instance.KingdomSettings.RoomCoreTIDs[0];
			roomData.MonsterGroupTID = GameManager.Instance.KingdomSettings.MonsterGroupTIDs[0];
			roomData.SpellTID = GameManager.Instance.KingdomSettings.RoomSpellTIDs[0];
			kingdomData.RoomDataList.Add(roomData);
		}

		KingdomNetworkingManager.Instance.PublishLevel(kingdomData, OnPublishedLevel);
	}

	[CSRCommand]
	protected void WinCombat()
	{
		CombatManager.Instance.CSRWin();
	}

	private void OnPublishedLevel(bool a_error)
	{
		if (a_error)
		{
			GenericPopupUI.CreatePopup("There was an error uploading your level. Try again?", null, null);
		}
		else
		{
			GenericPopupUI.CreatePopup("Successfully uploaded! Would you like to playtest your level?", null, null);
		}
	}

	[CSRCommand]
	protected void ClearData()
	{
		SaveManager.ClearSavedData();
		GameManager.Instance.CachedKingdomData.Clear();
		GameManager.Instance.CachedLeaderboard.Clear();
	}
}
