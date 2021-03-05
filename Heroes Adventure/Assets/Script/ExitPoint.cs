using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    public string onLoadScene;
    public string transitionName;
    public Entrance entrance;

    public float timeToLoad = 1.5f;
    bool shouldLoad;

    private void Start()
    {
        //set the transition name for the entrance script
        entrance.transitionName = transitionName;
    }

    private void Update()
    {
        //check if we should load a new scene or not
        if (shouldLoad)
        {
            shouldLoad = false;
            Invoke("Load", timeToLoad);
        }
    }

    //If the desire tag enter the collision box
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Make sure only trigered on player
        if(other.tag == "Player")
        {
            FindObjectOfType<GameManager>().isLoading = true; //Prevent player moving while loading
            shouldLoad = true; //Set the value True so the code in the update method is active
            FindObjectOfType<UIFade>().Fade(); //call Fade method from UIFade script
            FindObjectOfType<PlayerController>().transitionName = transitionName;   //set the player transition name
        }        
    }

    public void Load()
    {
        SceneManager.LoadScene(onLoadScene);
    }
}
