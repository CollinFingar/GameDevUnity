using UnityEngine;
using System.Collections;

public class scr_Rotate : MonoBehaviour {

	public float xRate = 0.0f;
	public float yRate = 0.0f;
	public float zRate = 0.0f;

	float multiplier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.Space)) {
			multiplier = 10.0f;
		} 
		if (Input.GetKeyUp (KeyCode.Space)) {
			multiplier = 1.0f;
		}
		*/

		this.transform.Rotate (new Vector3 (xRate, yRate, zRate) * Time.deltaTime * multiplier);
	}
}
