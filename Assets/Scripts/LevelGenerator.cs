using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

public class LevelGenerator : MonoBehaviour
{

		public GameObject leveltext;
		public GameObject bugobject;
		public GameObject lineobject;
		public GameObject printobject;
		public GameObject warpobject;
		public GameObject hero;
		public GameObject sidebaroutput;
		public GameObject selectedtool;
		public GameObject[] toolIcons = new GameObject[6];
		string codetext;
		public GameObject sidebartimer;
		public int linecount = 0;
		float initialLineY = 3f;
		float linespacing = 0.825f;
		float levelLineRatio = 0.55f;
		float bugsize = 1f;
		float bugscale = 1.5f;
		GameObject levelbug;
		int currentlevel = 1;
		float leveldelay = 1f;
		float startNextLevel = 0f;
		int maxlevel = 2;
		int numOfTools = 6;
		float startTime = 0f;
		float endTime = 0f;
		public int num_of_bugs = 0;
		List<GameObject> lines;
		List<GameObject> outputs;
		List<GameObject> warps;
		List<GameObject> bugs;


		// Use this for initialization
		void Start ()
		{
				lines = new List<GameObject> ();
				outputs = new List<GameObject> ();
				warps = new List<GameObject> ();
				bugs = new List<GameObject> ();

				BuildLevel ("level1.txt");
		}
	
		// Update is called once per frame
		void Update ()
		{
				sidebartimer.GetComponent<GUIText> ().text = "Time remaning: " + ((int)(endTime - Time.time)).ToString () + " seconds";
				if (num_of_bugs == 0) {
						
						if (startNextLevel == 0f) {
								startNextLevel = Time.time + leveldelay;
						} else if (Time.time > startNextLevel) {
								startNextLevel = 0f;
								currentlevel++;
								if (currentlevel <= maxlevel) {
										foreach (GameObject bug in bugs) {
												Destroy (bug);
										}
										BuildLevel ("level" + currentlevel.ToString () + ".txt");
								} else {
										GameOver ();
								}
						}
						
				}
				if (endTime < Time.time && num_of_bugs > 0) {
						GameOver ();
				}
		}
	
		public void BuildLevel (string filename)
		{
				ResetLevel ();

				XmlDocument doc = new XmlDocument ();
				doc.Load (filename);
				XmlNode levelnode = doc.FirstChild;
				WriteCode (levelnode);
				PlaceObjects (levelnode);
				SetTools (levelnode);
				
				startTime = Time.time;
				foreach (XmlNode timenode in levelnode.ChildNodes) {
						if (timenode.Name == "time") {
								endTime = (float)int.Parse (timenode.InnerText) + startTime;
						}
				}
				
				this.transform.position -= new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale += new Vector3 (0, levelLineRatio * linecount, 0);
		}

		void WriteCode (XmlNode levelnode)
		{
				foreach (XmlNode codenode in levelnode.ChildNodes) {
						if (codenode.Name == "code") {
								foreach (XmlNode printnode in codenode.ChildNodes) {
										if (printnode.Name == "print") {
												printnode.InnerText = "<color=#00ffffff>" + printnode.InnerText + "</color>";
										}
										if (printnode.Name == "warp") {
												printnode.InnerText = "<color=#ffff00ff>" + printnode.InnerText + "</color>";
										}
								}
								codetext = codenode.InnerText;
								foreach (char c in codetext) {
										if (c == '\n')
												linecount++;
								}
						}

				}
				for (int i = 0; i<linecount; i++) {
						GameObject newline = (GameObject)Instantiate (lineobject, new Vector3 (-4, initialLineY - i * linespacing, 1), transform.rotation);
						lines.Add (newline);
				}
				leveltext.GetComponent<TextMesh> ().text = codetext;
		}

		void PlaceObjects (XmlNode levelnode)
		{
				foreach (XmlNode codenode in levelnode.ChildNodes) {
						if (codenode.Name == "code") {
								// Create the XmlNamespaceManager.
								XmlNamespaceManager nsmgr = new XmlNamespaceManager (new NameTable ());
				
								// Create the XmlParserContext.
								XmlParserContext context = new XmlParserContext (null, nsmgr, null, XmlSpace.None);
				
								// Create the reader.
								XmlValidatingReader reader = new XmlValidatingReader (codenode.InnerXml, XmlNodeType.Element, context);
				
								IXmlLineInfo lineInfo = ((IXmlLineInfo)reader);
								while (reader.Read()) {
										if (reader.NodeType == XmlNodeType.Element && reader.Name == "print") {
												GameObject newoutput = (GameObject)Instantiate (printobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
												outputs.Add (newoutput);
												printer printcode = newoutput.GetComponent<printer> ();
												printcode.displaytext = reader.GetAttribute ("text");
												printcode.sidebar = sidebaroutput;
										} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "warp") {
												GameObject newwarp = (GameObject)Instantiate (warpobject, new Vector3 (-7, initialLineY - (lineInfo.LineNumber - 1) * linespacing, 1), transform.rotation);
												warps.Add (newwarp);
												warper printcode = newwarp.GetComponent<warper> ();
												printcode.CodeScreen = this.gameObject;
												printcode.filename = reader.GetAttribute ("file");
										} else if (reader.NodeType == XmlNodeType.Element && reader.Name == "bug") {
												bugsize = int.Parse (reader.GetAttribute ("size"));
												levelbug = (GameObject)Instantiate (bugobject, new Vector3 (-9f + (bugsize - 1) * levelLineRatio, initialLineY - (lineInfo.LineNumber - 1 + 0.5f * (bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
												levelbug.transform.localScale += new Vector3 (bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
												levelbug.GetComponent<GenericBug> ().codescreen = this.gameObject;
												bugs.Add (levelbug);
												num_of_bugs++;
										}
								}
								reader.Close ();
						}
				}
		}

		public void SetTools (XmlNode levelnode)
		{
				for (int i = 0; i<numOfTools; i++) {
						toolIcons [i].GetComponent<GUITexture> ().enabled = false;
				}
				foreach (XmlNode codenode in levelnode.ChildNodes) {
						if (codenode.Name == "tools") {
								foreach (XmlNode toolnode in codenode.ChildNodes) {
										int toolnum = int.Parse (toolnode.InnerText);
										toolIcons [toolnum].GetComponent<GUITexture> ().enabled = true;
										selectedtool.GetComponent<SelectedTool> ().toolCounts [toolnum] = int.Parse (toolnode.Attributes ["count"].Value);
								}
						}
				}
		}

		public void ResetLevel ()
		{
				foreach (GameObject ln in lines) {
						Destroy (ln);
				}
				foreach (GameObject op in outputs) {
						Destroy (op);
				}
				foreach (GameObject wp in warps) {
						Destroy (wp);
				}
				for (int i = 1; i<numOfTools; i++) {
						toolIcons [i].GetComponent<GUITexture> ().enabled = false;
						toolIcons [i].GetComponent<GUITexture> ().color = new Color (0.3f, 0.3f, 0.3f);
						selectedtool.GetComponent<SelectedTool> ().toolCounts [i] = 0;
						selectedtool.GetComponent<SelectedTool> ().projectilecode = 0;
				}
				Destroy (levelbug);
				this.transform.position += new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale -= new Vector3 (0, levelLineRatio * linecount, 0);
				linecount = 0;
				hero.transform.position = leveltext.transform.position;
		}

		public void GameOver ()
		{
				Application.Quit ();
		}
}
