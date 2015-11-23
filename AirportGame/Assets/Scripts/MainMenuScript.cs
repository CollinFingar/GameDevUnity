using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 170, 100, 60), "Start"))
            Application.LoadLevel(1);
        if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 110, 100, 60), "Instructions"))
            Debug.Log("instructions");
    }
}
