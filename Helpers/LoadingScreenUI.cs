using Platform.UIManagement;

public class LoadingScreenUI : FullscreenUI
{
	private static LoadingScreenUI sm_loadingScreen;

	public static bool IsShowing { get { return sm_loadingScreen != null; } }

	public static LoadingScreenUI Show()
	{
		if (sm_loadingScreen == null)
		{
			sm_loadingScreen = UIManager.Instance.OpenUI<LoadingScreenUI>(GameManager.Instance.UISettings.LoadingScreenTID);
		}
		else
		{
			FullScreenManager.Instance.AddFullscreenToStack(sm_loadingScreen);
		}
		return sm_loadingScreen;
	}

	public static void Hide()
	{
		if (sm_loadingScreen != null)
		{
			sm_loadingScreen.CloseUI();
			sm_loadingScreen = null;
		}
	}
}
