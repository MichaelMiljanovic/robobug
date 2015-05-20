using UnityEngine;
using System.Collections;

public class SelectedTool : MonoBehaviour {
	GUIText tm;
	int projectilecode = 1;

	// Use this for initialization
	void Start () {
		tm = this.GetComponent<GUIText> ();
		tm.text = "Bugcatcher";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("tab")) {
			projectilecode = (projectilecode+1)%7;
			if (projectilecode ==0){
				projectilecode = 1;
			}
		}
		switch (projectilecode) {
		case 1:
			tm.color = Color.white;
			tm.text = "Bugcatcher";
			break;
		case 2: tm.color = Color.white;
			tm.text = "Tester";
			break;
		case 3: tm.color = Color.white;
			tm.text = "Activator";
			break;
		case 4: tm.color = Color.white;
			tm.text = "Breakpointer";
			break;
		case 5: tm.color = Color.white;
			tm.text = "Warper";
			break;
		case 6: tm.color = Color.white;
			tm.text = "Helper";
			break;
				}
	}
}
