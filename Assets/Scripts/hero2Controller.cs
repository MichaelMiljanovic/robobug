﻿using UnityEngine;
using System.Collections;

public class hero2Controller : MonoBehaviour
{

		public float maxSpeed = 10f;
		public float climbSpeed = 10f;
		public Rigidbody2D projectile;
		public Rigidbody2D projectileB;
		public Rigidbody2D projectileT;
		public Rigidbody2D projectileA;
		public Rigidbody2D projectileD;
		public Rigidbody2D projectileW;
		public Rigidbody2D projectileH;
		private bool walkloop = false;
		int projectilecode = 1;
		public GameObject projectileobject;
		public bool onWall = false;
		bool facingRight = true;
		float fireRate = 0.1f;
		private float nextFire = 0.0f;
		float animTime = 0.3f;
		private float animDelay = 0.0f;
		Animator anim;
		public bool dropping = false;
		float dropTime = 0.01f;
		private float dropDelay = 0.0f;
		bool quitting = false;



		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
		}

		void FixedUpdate ()
		{



				//movement
				float move = Input.GetAxis ("Horizontal");
				float up = Input.GetAxis ("Vertical");
				if (up > 0) {
						onWall = true;
						rigidbody2D.gravityScale = 0;
				}
				if (up < 0) {
						onWall = false;
						rigidbody2D.gravityScale = 1;
				}

				//set animation for movement
				anim.SetBool ("onWall", onWall);
				anim.SetFloat ("speed", Mathf.Abs (move));
				anim.SetFloat ("climbSpeed", up);
				if (move > 0) {
						facingRight = true;
				} else if (move < 0) {
						facingRight = false;
				}
				anim.SetBool ("facingRight", facingRight);


				//code for falling down through platforms
				//	if (Input.GetAxisRaw("Vertical") == -1 && Input.GetButton("Jump")){
				//	if (Input.GetButton("Jump")){
				if (Input.GetAxis ("Vertical") < 0 && !onWall) {
						dropDelay = Time.time + dropTime;
						dropping = true;
				}
				if (Time.time > dropDelay) {
						dropping = false;
				}
		
				Physics2D.IgnoreLayerCollision (0, 8, onWall || dropping || Input.GetKey ("up"));

				//move up if on the wall, otherwise let gravity do the work
				if (dropping) {
						if (rigidbody2D.velocity.y == 0) {
								rigidbody2D.isKinematic = true;
								rigidbody2D.AddForce (Vector2.up * -50);
								rigidbody2D.isKinematic = false;
						}
						rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
				} else {
						if (!onWall) {
								rigidbody2D.velocity = new Vector2 (move * maxSpeed, Mathf.Min (0f, rigidbody2D.velocity.y));
						} else {
								rigidbody2D.isKinematic = true;
								rigidbody2D.velocity = new Vector2 (move * maxSpeed, up * climbSpeed);
								rigidbody2D.isKinematic = false;
						}
				}
		}

		void Update ()
		{

				AudioSource ad = GetComponent<AudioSource> ();
				if (!walkloop && Input.GetAxis ("Horizontal") != 0f && rigidbody2D.velocity.y == 0 && !onWall) {
						ad.Play ();
						walkloop = true;
						ad.loop = true;
				}
				if (Input.GetAxis ("Horizontal") == 0f || rigidbody2D.velocity.y != 0 || onWall) {
						ad.loop = false;
						walkloop = false;
				}

				//stars
				if (Input.GetKeyDown ("tab")) {
						projectilecode = (projectilecode + 1) % 7;
						if (projectilecode == 0) {
								projectilecode = 1;
						}
				}

				//firing
				if ((Input.GetKeyDown ("left ctrl") || Input.GetKeyDown ("right ctrl")) && Time.time > nextFire && !onWall && rigidbody2D.velocity == Vector2.zero) {
						anim.SetBool ("throw", true);
						nextFire = Time.time + fireRate;
						animDelay = Time.time + animTime;
						Rigidbody2D newstar;
						switch (projectilecode) {
						case 0:
								break;
						case 1:
								newstar = (Rigidbody2D)Instantiate (projectileB, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						case 2:
								newstar = (Rigidbody2D)Instantiate (projectileT, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						case 3:
								newstar = (Rigidbody2D)Instantiate (projectileA, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						case 4:
								newstar = (Rigidbody2D)Instantiate (projectileD, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						case 5:
								newstar = (Rigidbody2D)Instantiate (projectileW, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						case 6:
								newstar = (Rigidbody2D)Instantiate (projectileH, transform.position, transform.rotation);
								if (facingRight) {
										newstar.rigidbody2D.AddForce (Vector2.right * 300);
								} else {
										newstar.rigidbody2D.AddForce (Vector2.right * -300);
								}
								break;
						}
				}
				if (Time.time > animDelay) {
						anim.SetBool ("throw", false);
				}



				//quit
				if (Input.GetKeyDown (KeyCode.Escape) == true) {
						quitting = true;
				}
				if (quitting) {
						//quitPrompt.GetComponent<GUIText> ().text = "Would you like to quit?\nPress Y to Quit\nPress N to return";
						if (Input.GetKeyDown ("y")) {
								Application.Quit ();
						} else if (Input.GetKeyDown ("n")) {
								quitting = false;
								
						}
				} else {
						//quitPrompt.GetComponent<GUIText> ().text = "";
				}
		}

}
