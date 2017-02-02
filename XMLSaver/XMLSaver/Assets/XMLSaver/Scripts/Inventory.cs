using UnityEngine;
using System.Collections.Generic;

// Example class for using the XMLWriter
// This class will contain some inventory objects that the player has
public class Inventory {

	public List<Items> playerInventory;
	
	public Inventory(){ playerInventory = new List<Items>(); }

	public Inventory(params Items[] inventoryItems)
	{
		playerInventory = new List<Items>();
		foreach(Items i in inventoryItems)
		{
			playerInventory.Add(i);
		}
	}
}
