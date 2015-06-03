using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour {

	public GameObject hero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Camera>().transform.position = new Vector3 (0f, hero.transform.position.y, -10f);
		GetComponent<Camera>().orthographicSize = 6;
	}
}
