using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {

    public Texture twitter1;
    public Texture twitter2;
    public Texture twitter3;
    public Texture tumblr1;
    public Texture tumblr2;
    public Texture tumblr3;

    private Texture current;

    private Texture[] twitterTextures = new Texture[3];
    private Texture[] tumblrTextures = new Texture[3];

    private int twitterIndex = 2;
    private int tumblrIndex = 2;

    public bool showingUp = false;
    public bool closingDown = false;

    private bool on = false;
    private byte alpha = 0;

    // Use this for initialization
    void Start () {
        twitterTextures[0] = twitter1;
        twitterTextures[1] = twitter2;
        twitterTextures[2] = twitter3;
        tumblrTextures[0] = tumblr1;
        tumblrTextures[1] = tumblr2;
        tumblrTextures[2] = tumblr3;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            makeVisible("tumblr");
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            makeVisible("twitter");
        }

        if (showingUp)
        {
            if (alpha <  200)
            {
                alpha += 20;
            }
            else
            {
                showingUp = false;
            }
        }
        if (closingDown)
        {
            if (alpha > 0)
            {
                alpha -= 20;
            }
            else
            {
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


    void makeVisible(string media)
    {
        if (!on)
        {
            setMediaTexture(media);
            on = true;
            showingUp = true;
            closingDown = false;
        }
        else
        {
            on = false;
            showingUp = false;
            closingDown = true;
        }
    }

    void setMediaTexture(string media) {
        if (media == "tumblr") {
            if (tumblrIndex < 2) {
                tumblrIndex++;
            } else {
                tumblrIndex = 0;
            }
            current = tumblrTextures[tumblrIndex];
        } else if (media == "twitter") {
            if (twitterIndex < 2) {
                twitterIndex++;
            } else {
                twitterIndex = 0;
            }
            current = twitterTextures[twitterIndex];
        }
    }

}
