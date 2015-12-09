using UnityEngine;
using System.Collections;

public class LevelProgresser : MonoBehaviour {

    public int nextLevelIndex = 0;
    public string nextLevelName = " ";
    public bool useString = false;

    public float distance = 4f;

    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < distance)
        {
            if (!useString)
            {
                Application.LoadLevel(nextLevelIndex);
            }
            else
            {
                Application.LoadLevel(nextLevelName);
            }
        }
	}
}
