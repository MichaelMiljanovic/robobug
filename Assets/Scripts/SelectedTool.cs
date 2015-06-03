using UnityEngine;
using System.Collections;

public class SelectedTool : MonoBehaviour
{
		GUIText tm;
		public int projectilecode = 0;
		Color toolOn = new Color (.7f, .7f, .7f);
		Color toolOff = new Color (.3f, .3f, .3f);
		bool losing = false;
		float lossTime = 0;
		float lossDelay = 2f;
		public GameObject[] toolIcons = new GameObject[6];
		public int[] toolCounts = new int[6];
		public GameObject codescreen;
		public GameObject hero;

		// Use this for initialization
		void Start ()
		{
				tm = this.GetComponent<GUIText> ();
				tm.text = "Bugcatcher";
		}
	
		// Update is called once per frame
		void Update ()
		{
				hero.GetComponent<hero2Controller> ().projectilecode = projectilecode;
				if (losing && toolCounts [0] > 0) {
						losing = false;
				} else if (losing) {
						if (Time.time > lossTime) {
								codescreen.GetComponent<LevelGenerator> ().GameOver ();
						}
				}
				if (Input.GetKeyDown ("tab")) {
						toolIcons [projectilecode].GetComponent<GUITexture> ().color = toolOff;
						projectilecode = (projectilecode + 1) % 6;
						while (!toolIcons[projectilecode].GetComponent<GUITexture>().enabled) {
								projectilecode = (projectilecode + 1) % 6;
						}
				}
				if (Input.GetKeyDown ("left ctrl") || Input.GetKeyDown ("right ctrl")) {
						toolCounts [projectilecode] -= 1;
						if (toolCounts [projectilecode] == 0) {
								toolIcons [projectilecode].GetComponent<GUITexture> ().enabled = false;
								projectilecode = (projectilecode + 1) % 6;
								while (!toolIcons[projectilecode].GetComponent<GUITexture>().enabled) {
										projectilecode = (projectilecode + 1) % 6;
								}
								if (projectilecode == 0) {
										losing = true;
										lossTime = Time.time + lossDelay;
								}
						}
				}
				switch (projectilecode) {
				case 0:
						tm.color = Color.white;
						tm.text = "Bugcatcher: " + toolCounts [0].ToString (); 
						toolIcons [0].GetComponent<GUITexture> ().color = toolOn;
						break;
				case 1:
						tm.color = Color.white;
						tm.text = "Tester: " + toolCounts [1].ToString (); 
						toolIcons [1].GetComponent<GUITexture> ().color = toolOn;
						break;
				case 2:
						tm.color = Color.white;
						tm.text = "Activator: " + toolCounts [2].ToString ();  
						toolIcons [2].GetComponent<GUITexture> ().color = toolOn;
						break;
				case 3:
						tm.color = Color.white;
						tm.text = "Breakpointer: " + toolCounts [3].ToString (); 
						toolIcons [3].GetComponent<GUITexture> ().color = toolOn;
						break;
				case 4:
						tm.color = Color.white;
						tm.text = "Warper: " + toolCounts [4].ToString (); 
						toolIcons [4].GetComponent<GUITexture> ().color = toolOn;
						break;
				case 5:
						tm.color = Color.white;
						tm.text = "Helper: " + toolCounts [5].ToString (); 
						toolIcons [5].GetComponent<GUITexture> ().color = toolOn;
						break;
				}
		}

}
