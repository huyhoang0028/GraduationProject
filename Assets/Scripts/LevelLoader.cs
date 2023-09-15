using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    
	/// <summary>
	/// The filename of the level to be loaded
	/// </summary>
	public string levelname = "level.tmx";

	/// <summary>
	/// Array, containing all tile prefabs in order
	/// </summary>
//	public GameObject emptyTile;

	//public GameObject emptyTile;

	public List<GameObject> tilesList;
	
	//width and height of level
	private int _height = 48;
	private int _width = 0;
		
	//start loading the level at the start
	void Start ()
	{
		levelname = "map_"+(Menu.selectedLevel+1).ToString()+".tmx";
		LoadTile ();
		StartCoroutine (LoadLevel (levelname));
	}



	//actually load and parse level
	private IEnumerator LoadLevel (string filename)
	{
		WWW www = new WWW (GetFilePath (filename));
		yield return www;
		string leveldata = www.text;
		XmlReader xmlReader = XmlReader.Create (new StringReader (leveldata));


		//keep reading until end-of-file
		while (xmlReader.Read ()) {
			//scan map size
			if (xmlReader.IsStartElement ("map")) {
				_width = int.Parse (xmlReader.GetAttribute ("width"));
				_height = int.Parse (xmlReader.GetAttribute ("height"));
			}
			//scan object layer
			if (xmlReader.IsStartElement ("object")) {
				int x = int.Parse (xmlReader.GetAttribute ("x"));
				int y = int.Parse (xmlReader.GetAttribute ("y"));
				int gid = int.Parse (xmlReader.GetAttribute ("gid"));
				string name = xmlReader.GetAttribute ("name");
				CreateTile (x, y, gid, name);
			}
			/*
			//scan layer
			if (xmlReader.IsStartElement ("layer")) {

				if (xmlReader.GetAttribute ("name") == "Info") {
					string data = xmlReader.ReadInnerXml ();
					string[] lines = data.Split ('\n');
					int height = lines.Length - 2; //removes additional empty line
					for (int j = 1; j < height + 1; j++) {
						string line = lines [j];
						string[] cols = line.Split (',');
						int width = cols.Length - 1;
						for (int i = 0; i < width + 1; i++) {
							int tile = 0;
							if (int.TryParse (cols [i], out tile)) {
								CreateTile (i, _height - j, tile, "");
							}
						}
					}
				}
			}
			*/

			//scan tile data layer
			if (xmlReader.IsStartElement ("data")) {
				string data = xmlReader.ReadInnerXml ();
				string[] lines = data.Split ('\n');
				int height = lines.Length - 2; //removes additional empty line
				for (int j = 1; j < height + 1; j++) {
					string line = lines [j];
					string[] cols = line.Split (',');
					int width = cols.Length - 1;
					for (int i = 0; i < width + 1; i++) {
						int tile = 0;
						if (int.TryParse (cols [i], out tile)) {
							CreateTile (i, _height - j, tile, "");
						}
					}
				}
			}

		}
	}
	
	//create a single tile, (0=empty space)
	private void CreateTile (int x, int y, int tile, string name)
	{
		if (tile == 0)
			return;
		GameObject newTile = (GameObject)Instantiate (tilesList [tile - 1]); //create tile
		if (name != "")
			newTile.name = "Tile" + tile; //set name if needed
		newTile.transform.position = new Vector3 (x, y, 0); //set position
		newTile.transform.parent = gameObject.transform; //make child of this object
		
	}
	
	//cross-device compatibility (last tested on unity )
	private string GetFilePath (string filename)
	{
		if (Application.platform == RuntimePlatform.Android)
			return "jar:file://" + Application.dataPath + "!/assets/" + filename;
		if (Application.platform == RuntimePlatform.WebGLPlayer)
    		return "StreamingAssets/" + filename;
		if (Application.platform == RuntimePlatform.OSXPlayer)
			return "file://" + Application.dataPath + "/Data/StreamingAssets/" + filename;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return "file://" + Application.streamingAssetsPath + "/" + filename;
		return "file://" + Application.dataPath + "/StreamingAssets/" + filename;
	}

	void LoadTile ()
	{
		tilesList = new List<GameObject> ();
		for (int i = 0; i <= 24; i++)
			tilesList.Add (Resources.Load ("Tile_" + i.ToString ()) as GameObject);
//		for (int i = 165; i <= 199; i++)
//			tilesList.Add (emptyTile);
//		for (int i = 200; i <= 272; i++)
//			tilesList.Add (Resources.Load ("tile_sheet_" + (i - 35).ToString()) as GameObject);
//		for (int i = 273; i <= 299; i++)
//			tilesList.Add (emptyTile);
	}
}