using UnityEngine;
using System.Collections;

public class GenericBug : MonoBehaviour {
		public Animator anim;
		public bool dead = false;
		float deathtime = 1.5f;
		float deathdelay = 0f;
		public bool finished = false;
		
		// Use this for initialization
		void Start () {
			this.renderer.enabled = false;
			anim = GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
		}
		
		void OnTriggerEnter2D(Collider2D p){
			if (p.name == "projectileBug(Clone)") {
				this.renderer.enabled = true;
				Destroy (p.gameObject);
				anim.SetBool("Dying", true);
				GetComponent<AudioSource>().Play();
				dead = true;
				deathdelay = Time.time + deathtime;
			}
		}
	}
