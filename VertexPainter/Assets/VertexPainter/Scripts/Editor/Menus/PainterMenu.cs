using UnityEngine;
using System.Collections;
using UnityEditor;

public class PainterMenu : MonoBehaviour {

	[MenuItem("Vertex Painter/Tools/Vertex Painter", false, 10)]
	static void LaunchSomething()
	{
		PainterWindow.LaunchVertexPainter();
	}
}
