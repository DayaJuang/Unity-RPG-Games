using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    float speed = 250f;
    public Animator anim;
    public static PlayerController instance;
    public string transitionName;

    private Vector3 minBounds;
    private Vector3 maxBounds;

    public bool canMove = true; //To check is the character is able to walk in certain situation

    //Called only once when the game start
    private void Start()
    {
        //Check if the character already in the scene or not
        //If there is no instance assign instance to this game object
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy an object if the object is not equal to this(character) game object
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        //To make sure that the game object is not being detroy if the scene is change
        //Keep track the last scene of the Game Object
        DontDestroyOnLoad(gameObject);  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (canMove)
        {
            //Character movement control using rigid body's velocity on x and y position multiplying by character speed
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed*Time.deltaTime;    
        }
        else
        {
            rb.velocity = Vector2.zero; //if player is in certain situation an unable to move, make the velocity of its rigidbody to 0
        }

        //Setting the animation parameter depends on the player rigidbody(movement)
        anim.SetFloat("moveX", rb.velocity.x);
        anim.SetFloat("moveY", rb.velocity.y);

        //Set the idle position animation of the player
        if ((Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) && canMove)
        {
            //Set the parameter depend on where the character is facing
            anim.SetFloat("lastX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("lastY", Input.GetAxisRaw("Vertical"));
            
        }
            //Make a bound for character, so it cannot go through the boundaries
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), transform.position.z);
    }

    //To set the character boundaries, will be called on the camera controller script
    public void setBounds(Vector3 minB, Vector3 maxB)
    {
        //The extraction is meant to make the character is exactly stop before the boundaries
        minBounds = minB + new Vector3(.5f,.5f);
        maxBounds = maxB + new Vector3(-.5f,-.5f);
    }
}
