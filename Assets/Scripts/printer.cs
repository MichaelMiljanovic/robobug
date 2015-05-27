using UnityEngine;
using System.Collections;

public class printer : MonoBehaviour {

	public string displaytext = "";
	public GameObject sidebar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileActivator(Clone)"){
			Destroy(c.gameObject);
			sidebar.GetComponent<GUIText>().text = displaytext;
		}
	}
}
