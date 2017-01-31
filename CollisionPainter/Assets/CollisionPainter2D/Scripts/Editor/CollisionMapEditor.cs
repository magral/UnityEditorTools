using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CollisionPainter
{
	[CustomEditor(typeof(CollisionMapController))]
	public class CollisionMapEditor : Editor
	{
		CollisionMapController controller;
		public override void OnInspectorGUI()
		{

			DrawDefaultInspector();
			controller = (CollisionMapController)target;

			controller.info = EditorGUILayout.IntField("sample Info", controller.info);

			if (GUILayout.Button("Info"))
			{
				controller.Info();
			}
			if (GUILayout.Button("Paint Zone"))
			{
				controller.Info();
			}
			if (GUILayout.Button("Paint Collision"))
			{
				controller.Info();
			}
			Repaint();
		}

		public void OnSceneGUI()
		{
			DrawBrush();
			Repaint();
			SceneView.RepaintAll();
		}
		public void DrawBrush()
		{
			Event e = Event.current;

			Handles.color = Color.green;

			Vector3 mousePos = e.mousePosition;
			mousePos.z = -SceneView.lastActiveSceneView.camera.worldToCameraMatrix.MultiplyPoint(controller.transform.position).z;
			mousePos.y = Screen.height - mousePos.y - 36.0f;
			mousePos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(mousePos);

			Handles.DrawWireCube(mousePos, new Vector3(.1f, .1f, .1f));

		}

		Vector2 FloorV2(Vector2 v)
		{
			return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
		}
	}
}
