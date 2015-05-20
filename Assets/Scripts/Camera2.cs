using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour {

	public GameObject hero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		camera.transform.position = new Vector3 (0f, hero.transform.position.y, -10f);
		camera.orthographicSize = 6;
	}
}
