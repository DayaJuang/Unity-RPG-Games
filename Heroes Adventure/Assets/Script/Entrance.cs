using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        //call the FadeAway method from UIFade script
        FindObjectOfType<UIFade>().FadeAway();

        //Check if the player transition name is the same as entrance's
        //Should be the same if the player loaded to another scene
        if(transitionName == FindObjectOfType<PlayerController>().transitionName)
        {
            //Set the player position in the new scene to the specific position
            FindObjectOfType<PlayerController>().transform.position = transform.position;
            //Make player can move again after the load scene
            FindObjectOfType<GameManager>().isLoading = false;
        }
    }
}
