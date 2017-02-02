using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System;

public static class XMLWriter  {

	// Make Save file
	// takes in the object, type and optional filepath name
	// DATA - Your object to be saved
	// TYPE - The type of your object [ this is the name of it's class ]
	// filePath - Where you want to save the file, defaults to SaveData
	public static void SerializeObject(object data, Type userType, params string[] filePath)
	{
		XmlSerializer ser = new XmlSerializer(userType);
		XmlTextWriter tWriter = new XmlTextWriter(Application.dataPath + 
			( filePath.Length != 0 ? filePath[0] + ".xml" : "/SaveData.xml" ), System.Text.Encoding.UTF8);
		ser.Serialize(tWriter, data);
		tWriter.Close();

		AssetDatabase.Refresh();
	}

	// Reads Save File
	// Takes in type of object and filePath if applicable
	// TYPE - the type of your object [ this is the name of it's class ]
	// filePath - The name of the file you saved, if none given pulls from default
	public static object DeserializeObject(Type type, params string[] filePath)
	{
		XmlSerializer ser = new XmlSerializer(type);
		XmlTextReader tReader = new XmlTextReader(Application.dataPath +  ( filePath.Length != 0 ? filePath[0] + ".xml" : "/SaveData.xml" ));
		object data = ser.Deserialize(tReader);
		tReader.Close();

		return data;
	}


}
