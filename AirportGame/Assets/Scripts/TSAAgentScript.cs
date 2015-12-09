using UnityEngine;
using System.Collections;

public class TSAAgentScript : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 20f;

    public GameObject player;
    private TSAWarning warningScript;
    public float noticeDistance = 10f;

    //Moving bools
    public bool followingPlayer = false;
    public bool movingBack = false;

    //Details of it's watch post
    private Vector3 watchSpot1 = new Vector3();
    private Vector3 watchRotation = new Vector3();

    public bool movingBackToStart = false;

    public Vector3 watchSpot2 = new Vector3();
    
    public GameObject itself;

    private Rigidbody rb;

    

	// Use this for initialization
	void Start () {
        watchSpot1 = transform.position;
        watchRotation = transform.localEulerAngles;
        rb = GetComponent<Rigidbody>();
        warningScript = player.GetComponent<TSAWarning>();
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        Vector3 targetDirection = playerPos - transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

        //If the player is close enough to be noticed, in front of the agent, and not blocked by anything
        if (distanceToPlayer < noticeDistance && angleToPlayer < 30 && canSee()) {
            //If the player is semi-far from the agent
            if (distanceToPlayer > 2) {
                //Rotate the agent to look at the player and move in that direction
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed * Time.deltaTime;
                //If first update following the player, notify the player's warning script
                if (!followingPlayer) {
                    warningScript.numberChasing += 1;
                    followingPlayer = true;
                }
            } else {
                //End the game
                Application.LoadLevel(Application.loadedLevelName);
            }
        } else {
        //Else, if the player isn't followable
            //If just was following, move back to place. Update the player's warning script
            if (followingPlayer) {
                movingBack = true;
                followingPlayer = false;
                warningScript.numberChasing -= 1;
            }
            //If on the way back to it's guard location
            if (true)
            {
                Vector3 destination;
                if (movingBackToStart) {
                    if(Vector3.Distance(transform.position, watchSpot1) < 2) {
                        movingBackToStart = false;
                        destination = watchSpot2;
                    } else {
                        destination = watchSpot1;
                    }
                } else {
                    if (Vector3.Distance(transform.position, watchSpot2) < 2) {
                        movingBackToStart = true;
                        destination = watchSpot1;
                    } else {
                        destination = watchSpot2;
                    }
                }
                //move there
                targetDirection = destination - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed/2 * Time.deltaTime;
                //If close enough, stop
                
            }
            //If back, check rotation and rotate accordingly
            else {
                if (Mathf.Abs(transform.localEulerAngles.y - watchRotation.y) > 5)
                {
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 5, 0);
                }
                else {
                    transform.localEulerAngles = watchRotation;
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            
        }


        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
	}



    //Raycast in the player's direction. If there isn't anything, return true. Else, return false.
    bool canSee() {
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        RaycastHit[] hit = Physics.RaycastAll(transform.position, playerPosition - transform.position, Vector3.Distance(transform.position, playerPosition));
        
        for (int i = hit.Length - 1; i > -1; i--)
        {
            if (hit[i].collider.gameObject.tag != "Player")
            {
                return false;
                
            }
        }
        Debug.DrawRay(transform.position, playerPosition - transform.position, Color.green);
        Debug.Log(hit.Length);
        return true;
        
    }

}
