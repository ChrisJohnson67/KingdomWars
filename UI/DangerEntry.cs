//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// DangerEntry
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DangerEntry : DisplayTemplateEntry
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---


	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(int a_dangerTemplate)
	{
		base.Init(AssetCacher.Instance.CacheAsset<TagTemplate>(a_dangerTemplate));
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}