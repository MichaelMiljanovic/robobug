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
		public GameObject hero;
		float botincrement;
		float barstretch;
		float barmove;
		float screenstretch;
		float screenmove;
		string codetext;
		string line;
		public int linecount = 0;
		float initialLineY = 3f;
		float linespacing = 0.825f;
		float levelLineRatio = 0.55f;
		int bugline = 0;
		float bugsize = 1f;
		float bugscale = 1.5f;
		List<GameObject> lines;
		GameObject levelbug;
		int currentlevel = 1;
		float leveldelay = 1f;
		float startNextLevel = 0f;
		int maxlevel = 2;

		// Use this for initialization
		void Start ()
		{
				lines = new List<GameObject> ();
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
	
		void BuildLevel (string filename)
		{
				ResetLevel ();

				XmlDocument doc = new XmlDocument ();
				doc.Load (filename);
				WriteCode (doc);
				PlaceBugs (doc);
				
				this.transform.position -= new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale += new Vector3 (0, levelLineRatio * linecount, 0);
		}

		void WriteCode (XmlDocument doc)
		{
				foreach (XmlNode codenode in doc.ChildNodes) {
						if (codenode.Name == "code") {
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

		void PlaceBugs (XmlDocument doc)
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

		void ResetLevel ()
		{
				foreach (GameObject line in lines) {
						Destroy (line);
				}
				this.transform.position += new Vector3 (0, (linecount / 2) * linespacing, 0);
				this.transform.localScale -= new Vector3 (0, levelLineRatio * linecount, 0);
				linecount = 0;
				hero.transform.position = leveltext.transform.position;
		}
}
