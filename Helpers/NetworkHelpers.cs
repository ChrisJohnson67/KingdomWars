using Photon.Pun;
using System.Collections.Generic;

public static class NetworkHelpers
{
	public static object SerializeListOverNetwork<T>(List<T> a_list)
	{
		var array = a_list.ToArray();
		return array as object;
	}

	public static float GetSecondsFromTimeStamp(PhotonMessageInfo a_info)
	{
		return (float)(PhotonNetwork.Time - a_info.SentServerTime) / 1000f;
	}
}