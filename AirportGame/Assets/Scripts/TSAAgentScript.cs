using UnityEngine;
using System.Collections;

public class TSAAgentScript : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 20f;

    public GameObject player;
    public float noticeDistance = 10f;

    public bool followingPlayer = false;
    public bool movingBack = false;

    private Vector3 watchSpot = new Vector3();
    private Vector3 watchRotation = new Vector3();
    private Vector3 velocity = new Vector3();

	// Use this for initialization
	void Start () {
        watchSpot = transform.position;
        watchRotation = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 targetDirection = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
        if (distanceToPlayer < noticeDistance && distanceToPlayer > 2 && angleToPlayer < 45) {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
            transform.position += transform.forward * speed * Time.deltaTime;
            followingPlayer = true;
        } else {
            if (followingPlayer) {
                movingBack = true;
                followingPlayer = false;
            }
            if (movingBack)
            {
                targetDirection = watchSpot - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed * Time.deltaTime;
                if (Vector3.Distance(watchSpot, transform.position) < 2)
                {
                    movingBack = false;
                }
            }
            else {
                if (Mathf.Abs(transform.localEulerAngles.y - watchRotation.y) < 5) {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 5, transform.localEulerAngles.z);
                }
            }
            
        }
	}
}
