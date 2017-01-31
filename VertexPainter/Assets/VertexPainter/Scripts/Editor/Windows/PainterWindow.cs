using UnityEngine;
using UnityEditor;
using System.Collections;

public class PainterWindow : EditorWindow {

	#region Variables
	GUIStyle boxStyle;
	public Vector2 mousePos;
	public RaycastHit hit;

	//flags
	public bool isPainting = false;
	public bool canPaint = false;
	public bool editingBrush = false;

	//Brush controls
	public float brushSize = 1.0f;
	public float brushOpacity = 1.0f;
	public float brushFalloff = 1.0f;

	//mesh
	public GameObject currObj;
	public Mesh currMesh;
	public GameObject lastObj;

	//paint
	public Color foregroundColor;
	#endregion

	#region Main Method
	public static void LaunchVertexPainter()
	{
		PainterWindow window = EditorWindow.GetWindow<PainterWindow>(false, "VertexPainter", true);
		window.GenerateStyles();
	}
	void OnEnable()
	{
		SceneView.onSceneGUIDelegate -= this.onSceneGUI;
		SceneView.onSceneGUIDelegate += this.onSceneGUI;
	}
	void OnDestroy()
	{
		SceneView.onSceneGUIDelegate -= this.onSceneGUI;
	}
	#endregion

	#region TempPainter Method
	void PaintVertexColor()
	{
		if (currMesh)
		{
			Vector3[] verts = currMesh.vertices;
			Color[] colors = new Color[0];
			if(currMesh.colors.Length > 0)
			{
				colors = currMesh.colors;
			}
			else
			{
				colors = new Color[verts.Length];
			}
			for (int i = 0; i < verts.Length; i++)
			{
				Vector3 vertPosition = currObj.transform.TransformPoint(verts[i]);
				float sqrMag = (vertPosition - hit.point).sqrMagnitude;
				if(sqrMag > brushSize)
				{
					continue;
				}
				float falloff = PainterUtils.LinearFalloff(sqrMag, brushSize);
				falloff = Mathf.Pow(falloff, brushFalloff * 3f) * brushOpacity;
				colors[i] = PainterUtils.VertexColorLerp(colors[i], foregroundColor, falloff) ;
			}
			
			currMesh.colors = colors;
		}
		else
		{
			Debug.LogWarning("No mesh available to paint");
		}
	}
	#endregion
	#region GUI Methods
	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Box("Vertex Painter", boxStyle, GUILayout.Height(60), GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal();

		GUILayout.BeginVertical(boxStyle);
		GUILayout.Space(10);
		canPaint = GUILayout.Toggle(canPaint, "Paint mode", GUI.skin.button, GUILayout.Height(50));

		GUILayout.Space(10);
		foregroundColor = EditorGUILayout.ColorField("Foreground Color:", foregroundColor);
		GUILayout.EndVertical();
		//Update ui 
		Repaint();
	}

	void onSceneGUI(SceneView sceneView)
	{
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10, 10, 200, 150), boxStyle);
		GUILayout.Button("A button", GUILayout.Height(25));
		GUILayout.EndArea();
		Handles.EndGUI();

		if (canPaint)
		{
			if (hit.transform != null)
			{
				//inner disc
				Handles.color = new Color(foregroundColor.r, foregroundColor.g, foregroundColor.b, brushOpacity);
				Handles.DrawSolidDisc(hit.point, hit.normal, brushSize);
				//disc outline
				Handles.color = Color.red;
				Handles.DrawWireDisc(hit.point, hit.normal, brushSize);
				//inner brush -- falloff
				Handles.color = Color.white;
				Handles.DrawWireDisc(hit.point, hit.normal, brushFalloff);

			}

			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);

			if (!editingBrush)
			{
				if (Physics.Raycast(worldRay, out hit, 500f))
				{
					if(isPainting)
						PaintVertexColor();
				}
			}
		}
		ProcessInputs();
		//Update and repaint scene gui
		sceneView.Repaint();
	}

	void Update()
	{
		if (canPaint)
		{
			Selection.activeGameObject = null;
			if(currObj != null && currMesh != null)
			{
				
			}
		}
		else
		{
			currMesh = null;
			currObj = null;
		}

		if (hit.transform != null)
		{
			if(hit.transform.gameObject != lastObj)
			{
				currObj = hit.transform.gameObject;
				currMesh = PainterUtils.GetMesh(currObj);
				lastObj = currObj;
			}
		}
	}
	#endregion

	#region Utility Methods
	void ProcessInputs()
	{
		Event e = Event.current;
		mousePos = e.mousePosition;
		if (e.type == EventType.KeyDown)
		{
			if (e.isKey)
			{
				if (e.keyCode == KeyCode.Y)
				{
					canPaint = !canPaint;
					if (!canPaint)
					{
						Tools.current = Tool.View;
					}
					else
					{
						Tools.current = Tool.None;
					}
				}
			}
		}

		//Brush control combinations
		if (canPaint)
		{
			//brush size
			if(e.type == EventType.MouseDrag && e.control && e.button == 0 && !e.shift)
			{
				brushSize -= e.delta.x * 0.005f;
				brushSize = Mathf.Clamp(brushSize, 0.1f, 10f);
				if(brushFalloff > brushSize)
				{
					brushFalloff = brushSize;
				}
				editingBrush = true;
			}
			//opacity
			if(e.type == EventType.MouseDrag && !e.control && e.button == 0 && e.shift)
			{
				brushOpacity += e.delta.x * .005f;
				brushOpacity = Mathf.Clamp01(brushOpacity);
				editingBrush = true;
			}
			//falloff
			if (e.type == EventType.MouseDrag && e.control && e.button == 0 && e.shift)
			{
				brushFalloff += e.delta.x * .005f;
				brushFalloff = Mathf.Clamp(brushFalloff, 0f, brushSize);
				editingBrush = true;
			}
			//click to paint
			if (e.type == EventType.MouseDrag && !e.control && e.button == 0 && !e.shift && !e.alt)
			{
				isPainting = true;
			}
			if (e.type == EventType.MouseUp)
			{
				editingBrush = false;
				isPainting = false;
			}
		}
	}
	void GenerateStyles()
	{
		boxStyle = new GUIStyle();
		boxStyle.normal.background = (Texture2D)Resources.Load("Textures/default_box_bg");
		boxStyle.normal.textColor = Color.white;
		boxStyle.border = new RectOffset(3, 3, 3, 3);
		boxStyle.margin = new RectOffset(2, 2, 2, 2);
		boxStyle.fontSize = 25;
		boxStyle.fontStyle = FontStyle.Bold;
		boxStyle.alignment = TextAnchor.MiddleCenter;
		boxStyle.font = (Font) Resources.Load("Fonts/AGENCYB");
		boxStyle.margin = new RectOffset(10, 0, 0, 0);
	}
	#endregion
}
