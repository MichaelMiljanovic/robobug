using UnityEngine;
using System.Collections;

public class warper : MonoBehaviour {

	public GameObject CodeScreen;

	public string filename;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D c){
		if (c.name == "projectileWarp(Clone)"){
			Destroy(c.gameObject);
			LevelGenerator lg = CodeScreen.GetComponent<LevelGenerator>();
			lg.BuildLevel(filename);
		}
	}
}
