using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {
    
    private Texture current;

    public Texture[] twitterTextures;
    public Texture[] tumblrTextures;

    private int twitterIndex = 2;
    private int tumblrIndex = 2;

    public bool showingUp = false;
    public bool closingDown = false;

    private bool on = false;
    private byte alpha = 0;

    private float elapsedSeconds = 0f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            makeVisible("tumblr");
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            makeVisible("twitter");
        }

        if (on){
            float notificationDuration = 10f;
            if (elapsedSeconds > notificationDuration){                               
                elapsedSeconds = 0;         
                endNotification();          
            }
            elapsedSeconds += Time.deltaTime;
        }

        if (showingUp){
            if (alpha <  200){
                alpha += 20;
            }
            else{
                showingUp = false;
            }
        }
        if (closingDown){
            if (alpha > 0){
                alpha -= 20;
            }
            else{
                closingDown = false;
            }
        }


    }

    void OnGUI() {
        if (on || (closingDown)) {
            GUI.color = new Color32(255, 255, 255, alpha);
            
            GUI.DrawTexture(new Rect(Screen.width - 400, 0, 400, 200), current, ScaleMode.StretchToFill, true, 10.0F);
        }
        
    }

    public void showNotification(string media){
        setMediaTexture(media);
        on = true;
        showingUp = true;
        closingDown = false;
        elapsedSeconds = 0;
    }

    private void endNotification(){
        on = false;
        showingUp = false;
        closingDown = true;
        elapsedSeconds = 0;
    }

    public void makeVisible(string media){
        if (!on){
            setMediaTexture(media);
            on = true;
            showingUp = true;
            closingDown = false;
        }
        else{
            on = false;
            showingUp = false;
            closingDown = true;
        }
    }

    void setMediaTexture(string media) {
        if (media == "tumblr") {
            if (tumblrIndex < tumblrTextures.Length) {
                tumblrIndex++;
            } else {
                tumblrIndex = 0;
            }
            current = tumblrTextures[tumblrIndex];
        } else if (media == "twitter") {
            if (twitterIndex < twitterTextures.Length) {
                twitterIndex++;
            } else {
                twitterIndex = 0;
            }
            current = twitterTextures[twitterIndex];
        }
    }

}
