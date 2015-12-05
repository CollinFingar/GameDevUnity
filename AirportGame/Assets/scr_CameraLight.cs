using UnityEngine;
using System.Collections;

public class scr_CameraLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Light> ().spotAngle = this.GetComponentInParent<scr_CameraBehavior> ().coneRadius * 2;

		this.GetComponent<Light> ().range = this.GetComponentInParent<scr_CameraBehavior> ().seeDist;

	}
	
	// Update is called once per frame
	void Update () {
		float pitch = this.GetComponentInParent<scr_CameraBehavior> ().pitch;

		// I have no idea why this works but it does. The problems arise that camera pitch is assbackwards from typical eulerAngles but whatever.
		this.transform.Rotate (new Vector3 (this.transform.rotation.eulerAngles.x + pitch, 0, 0) * -1 );

		if (this.GetComponentInParent<scr_CameraBehavior> ().inSight == true) {
			this.GetComponent<Light> ().color = Color.red;
		} else {
			this.GetComponent<Light> ().color = Color.yellow;
		}
	}
}
