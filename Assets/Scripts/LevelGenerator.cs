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
		string codetext;
		public int linecount = 0;
		float initialLineY = 3f;
		float linespacing = 0.825f;
		float levelLineRatio = 0.55f;
		int bugline = 0;
		float bugsize = 1f;
		float bugscale = 1.5f;
		GameObject levelbug;
		int currentlevel = 1;
		float leveldelay = 1f;
		float startNextLevel = 0f;
		int maxlevel = 2;
		List<GameObject> lines;
		List<GameObject> outputs;
		List<GameObject> warps;


		// Use this for initialization
		void Start ()
		{
				lines = new List<GameObject> ();
				outputs = new List<GameObject> ();
				warps = new List<GameObject> ();

				BuildLevel ("level1.txt");
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (levelbug) {
						if (levelbug.GetComponent<Animator> ().GetBool ("Dying") == true) {
								if (startNextLevel == 0f) {
										startNextLevel = Time.time + leveldelay;
								} else if (Time.time > startNextLevel) {
										startNextLevel = 0f;
										currentlevel++;
										if (currentlevel <= maxlevel) {
												Destroy (levelbug);
												BuildLevel ("level" + currentlevel.ToString () + ".txt");
										}
								}
						}
				}
		}
	
		public void BuildLevel (string filename)
		{
				ResetLevel ();

				XmlDocument doc = new XmlDocument ();
				doc.Load (filename);	
				WriteCode (doc);
				PlaceBug (doc);
				PlaceObjects (doc);

				
				this.transform.position -= new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale += new Vector3 (0, levelLineRatio * linecount, 0);
		}

		void WriteCode (XmlDocument doc)
		{
				foreach (XmlNode codenode in doc.ChildNodes) {
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

		void PlaceBug (XmlDocument doc)
		{
				foreach (XmlNode codenode in doc.ChildNodes) {
						if (codenode.Name == "code") {
								string codexml = codenode.InnerXml;
								bugline = codexml.Substring (0, codexml.IndexOf ("<bug>")).Split ('\n').Length - 1;
								bugsize = codexml.Substring (0, codexml.IndexOf ("</bug>")).Split ('\n').Length - bugline;
						}
						levelbug = (GameObject)Instantiate (bugobject, new Vector3 (-9f + (bugsize - 1) * levelLineRatio, initialLineY - (bugline + 0.5f * (bugsize - 1)) * linespacing + 0.4f, 0f), transform.rotation);
						levelbug.transform.localScale += new Vector3 (bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
				}
		}

		void PlaceObjects (XmlDocument doc)
		{
				foreach (XmlNode codenode in doc.ChildNodes) {
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
										}
								}
								reader.Close ();
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
				this.transform.position += new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale -= new Vector3 (0, levelLineRatio * linecount, 0);
				linecount = 0;
				hero.transform.position = leveltext.transform.position;
		}
}
