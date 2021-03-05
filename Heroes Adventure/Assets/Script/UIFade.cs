using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public Image fadeScreen;
    bool shouldFade;
    bool shouldFadeAway;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if should load the screen or not
        if (shouldFade)
        {
            //Make the transition transparancy change over time(depend on user fps) using Mathf.MoveTowards
            //Make it become 1 slowly
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, 1f * Time.deltaTime));

            //If the transparancy already at max
            if (fadeScreen.color.a == 1f)
            {
                shouldFade = false; //Set to be false so we can load to multiple scene
            }
        }

        //If the FadeAway method is being called from entrance script
        if (shouldFadeAway)
        {
            //Make the transition transparancy change over time(depend on user fps) using Mathf.MoveTowards
            //set the transparancy to slowly become 0
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, 1f * Time.deltaTime));
            //If the transparancy already 0
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeAway = false; //Set to be false so we can load to multiple scene
            }
        }
    }

    //To make an exit loading transition appear
    public void Fade()
    {
        shouldFade = true;
        shouldFadeAway = false;
    }

    //Make the loading transition fade away
    public void FadeAway()
    {
        shouldFade = false;
        shouldFadeAway = true;
    }
}
