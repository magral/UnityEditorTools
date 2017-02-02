using UnityEngine;
using System.Collections;

public enum Items
{
	HealthPot,
	ManaPot,
	ArmorPlate,
	Weapon
}
// Example class for XMLWriter -- Add this to any game object and run scene to test
// Simply adds some items to the Inventory then saves it
// Calls the seriazlier and deserialize for demonstration of how to use the class, then prints out the contents of the inventory
public class AddToInventory : MonoBehaviour {

	public void Start()
	{
		Inventory inv = new Inventory(Items.ArmorPlate, Items.HealthPot);
		XMLWriter.SerializeObject( inv, typeof(Inventory));
		Inventory newInv = (Inventory) XMLWriter.DeserializeObject(typeof(Inventory));

		Debug.Log(newInv.playerInventory[0] + " " + newInv.playerInventory[1]);
	}

	
}
