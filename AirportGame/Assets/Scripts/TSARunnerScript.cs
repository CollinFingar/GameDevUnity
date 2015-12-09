using UnityEngine;
using System.Collections;

public class TSARunnerScript : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 20f;

    public GameObject player;
    private TSAWarning warningScript;
    public float noticeDistance = 10f;
    
    

    private Rigidbody rb;

    private bool firstTime = true;
    private bool chased = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        warningScript = player.GetComponent<TSAWarning>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        Vector3 targetDirection = playerPos - transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

        
        //If the player is close enough to be noticed, in front of the agent, and not blocked by anything
        if (distanceToPlayer < noticeDistance && angleToPlayer < 30 && canSee())
        {
            Debug.Log("skjdlfjsdf");
            if (firstTime) {
                warningScript.numberChasing += 1;
                chased = true;
                firstTime = false;
            }
            //If the player is semi-far from the agent
            if (distanceToPlayer > 2)
            {
                //Rotate the agent to look at the player and move in that direction
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f));
                transform.position += transform.forward * speed * Time.deltaTime;

            }
            else
            {
                //End the game
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
        else
        {
            SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
            if (!r.isVisible && !canSee()) {
                if (chased) {
                    warningScript.numberChasing -= 1;
                }
                Destroy(gameObject);
                
            }

        }


        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }



    //Raycast in the player's direction. If there isn't anything, return true. Else, return false.
    bool canSee()
    {
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
