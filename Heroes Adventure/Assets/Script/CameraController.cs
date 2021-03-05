using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [Header("Player and Camera Bounds")]
    public Transform target;
    public Tilemap theMap;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    //To make the camera stop exactly on the tilemap area
    private float halfHeight;
    private float halfWidth;

    [Header("Area Music")]
    public int musicToPlay;
    public int battleTrack;

    [Header("Battle Enemies")]
    public string[] enemies;
    public Sprite battleBG;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;    //Set the target to be the character transform everytime the scene start
        halfHeight = Camera.main.orthographicSize;  //set the halfheight to the size of the main camera
        halfWidth = halfHeight * Camera.main.aspect;    //set the halfwidht

        //The MIN bound value of x,y is NEGATIVE so add it with the halfwidth and halfheight to make it move toward center
        minBounds = theMap.localBounds.min + new Vector3(halfWidth,halfHeight,0f);
        //The MAX bound value of x,y is POSITIVE so subtract it with the halfwidth and halfheight to make it move toward center
        maxBounds = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight,0f);

        //Set the bound to the playercontroller script, with the value before it got add/subtract so the player didn't stop before boundary
        FindObjectOfType<PlayerController>().setBounds(theMap.localBounds.min, theMap.localBounds.max);

        if(target.transform == null)
        {
            return;
        }
    }

    //LateUpdate is called after all update process
    //The LateUpdate is use to make sure all update process run first(from character script)
    void LateUpdate()
    {
        if (target != null)
        {
            //Asign camera position to always follow the target(player)
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        //Keep Camera Inside the Bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), transform.position.z);

    }
}
