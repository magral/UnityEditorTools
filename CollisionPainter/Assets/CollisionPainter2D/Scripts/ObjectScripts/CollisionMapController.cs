using UnityEngine;
using System.Collections;

namespace CollisionPainter {
	public enum Tools{
		Info,
		Zone,
		Collision
	}
	public class CollisionMapController : MonoBehaviour
	{

		private int _info = 75;

		public int info
		{
			get { return _info; }
			set { _info = value; }
		}
		public void Info()
		{
			info = 50;
			Debug.Log("Clicked");
		}
	}
}
