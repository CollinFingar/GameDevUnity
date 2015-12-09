using UnityEngine;
using System.Collections;

public class scr_ChaseCheck : MonoBehaviour {


	bool beingChased = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		ChaseCheck ();

		if (beingChased && this.GetComponent<AudioSource> ().isPlaying == false) {
			this.GetComponent<AudioSource> ().Play ();
		} else if (beingChased == false) {
			this.GetComponent<AudioSource> ().Stop();
		}
	}

	void ChaseCheck() {
		if (this.GetComponentInParent<TSAWarning> ().numberChasing > 0) {
			beingChased = true;
			return;
		} else {
			beingChased = false;
		}


	}
}
