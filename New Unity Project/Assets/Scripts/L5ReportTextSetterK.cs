﻿using UnityEngine;
using System.Collections;

public class L5ReportTextSetterK : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh tm = GetComponent<TextMesh> ();
		tm.color = Color.blue;
		tm.text = "object" +
				"\n\t" +
				"\n\t" + 
				"\n\tint" + 
				"\n\t" +
				"\n\t" +
				"\n\t" +
				"\n\tif                                         return " +
				"\n\telse    return";
		/*tm.text = "The bug is that the wrong objects are compared" +
				"\nThe bug is that the darker object is chosen" +
				"\nThe bug is that not all objects are compared" +
				"\nThe bug is that the more green object is chosen" +
				"\nThe bug is that the more red object is chosen" +
				"\nThe bug is that the more blue object is chosen" +
				"\nThe bug is that the blue component isn't used" +
				"\nThe bug is that the green component isn't used" +
				"\nThe bug is that the red component isn't used";*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}