using UnityEngine;
using System.Collections;

public class scr_CameraBehavior : MonoBehaviour {
	// This is the script for the camera behaviour. This should be attached to an object childed to ingame Camera object
	// such that the origin of the cameras point of view is childed to the camera but may be placed anywhere on it
	public float coneRadius = 0.0f;	// The radius of the cone check
	public float seeDist = 0.0f;	// The maximum distance at which one sees.

	public float pitch = 0.0f;

	public GameObject player;

	public bool inSight = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// While camera rotation is decided on the object level, pitch most likely will not
		// We determine the direction of the primary raycast given pitch

		// We start with a flat forward direction, which gives us only lateral direction
		Vector3 direction = this.transform.forward;

		// nDist represents the length of the vector for normalization purposes, namely for setting the yvalue
		// x and z are set to fractions of their original values
		float nDist = Mathf.Sqrt ((direction.x * direction.x) + (direction.z * direction.z));
		direction.x = direction.x * Mathf.Cos (pitch * Mathf.Deg2Rad);
		direction.z = direction.z * Mathf.Cos (pitch * Mathf.Deg2Rad);
		direction.y = nDist * Mathf.Sin (pitch * Mathf.Deg2Rad);

		RaycastHit info;
		bool hit = Physics.Raycast (this.transform.position, direction, out info, seeDist, 1<<LayerMask.NameToLayer("Floor"));
		//Debug.DrawLine (this.transform.position, info.point, Color.blue, Time.deltaTime);


        //inSight = checkSight (player, direction);
        inSight = false;
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Player");
		for (int i=0; i<agents.Length; i++) {
			inSight = inSight || checkSight(agents[i], direction);
		}

	}

	bool checkSight(GameObject player, Vector3 direction) {

		// We now want to check the angle between the player position and the cameras orientation
		float angle = Vector3.Angle ((player.transform.position - this.transform.position), direction.normalized);
		
		float distToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
		
		if (angle < coneRadius && distToPlayer < seeDist) {
			return true;
		} else {
			return false;
		}
	}
}
