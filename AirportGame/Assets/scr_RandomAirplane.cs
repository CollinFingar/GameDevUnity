using UnityEngine;
using System.Collections;

public class scr_RandomAirplane : MonoBehaviour {

	public AudioClip[] apSounds = new AudioClip[8];

	// Min time between airplane noises
	public float timeBetween = 0.0f;
	// Time since last sound played
	public float lastSound = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lastSound += Time.deltaTime;

		// If we have waited long enough since another airplane, check every 10 seconds
		if (lastSound > timeBetween && (Mathf.Floor (Time.time - timeBetween) % 10) == 0) {
			int x = Random.Range(apSounds.Length * -1, apSounds.Length - 1);
			// 50% chance of playing a random sound
			if  ( x >= 0) {
				this.GetComponent<AudioSource>().clip = apSounds[x];
				this.GetComponent<AudioSource>().Play();
				lastSound = 0;
			}
		}

	}
}
