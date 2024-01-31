using UnityEngine;

public class CameraHelpers
{
	public static Vector3 WorldToUISpace(Camera a_camera, Transform a_parentRect, Vector3 a_worldPos)
	{
		//Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
		Vector3 screenPos = a_camera.WorldToScreenPoint(a_worldPos);
		Vector2 movePos;

		//Convert the screenpoint to ui rectangle local point
		RectTransformUtility.ScreenPointToLocalPointInRectangle(a_parentRect as RectTransform, screenPos, null, out movePos);
		return movePos;
	}

}
