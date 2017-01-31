using UnityEngine;
using UnityEditor;
using System.Collections;

public static class PainterUtils {

	public static Mesh GetMesh(GameObject obj)
	{
		Mesh m = null;

		if (obj)
		{
			MeshFilter filter = obj.GetComponent<MeshFilter>();
			SkinnedMeshRenderer skinned = obj.GetComponent<SkinnedMeshRenderer>();
			if(filter && !skinned)
			{
				m = filter.sharedMesh;
			}
			if(!filter && skinned)
			{
				m = skinned.sharedMesh;
			}
		}

		return m;
	}

	//falloff
	public static float LinearFalloff(float distance, float brushRadius)
	{
		return Mathf.Clamp01(1 - distance / brushRadius);
	}

	public static Color VertexColorLerp(Color a, Color b, float value)
	{
		if (value > 1)
		{
			return b;
		}
		else if(value < 0)
		{
			return a;
		}

		//lerp colors
		return new Color(a.r + (b.r - a.r) * value, a.g + (b.g - a.g) * value, a.b + (b.b - a.b) * value, a.a + (b.a - a.a) * value);
	}
}
