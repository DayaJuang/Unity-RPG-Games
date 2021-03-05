using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float movement;
    public Rigidbody2D rb;
    public float walkingTime;
    float walkingCounter;
    public Animator npcAnim;
    public bool canWalk = true;
    public bool walkUpDown;
    bool isWalkingDown;
    bool isWalkingUp;
    bool isWalkingRight;

    private void Start()
    {
        walkingCounter = walkingTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (canWalk)
        {
            npcAnim.SetBool("isPlayer", false);
            if (walkUpDown)
            {
                if (walkingCounter >= walkingTime || isWalkingDown)
                {
                    walkingCounter -= Time.deltaTime;
                    isWalkingDown = true;
                    isWalkingUp = false;
                    npcAnim.SetBool("walkingUp", isWalkingUp);
                    rb.velocity = new Vector2(0, -movement);
                }

                if (walkingCounter <= 0 || isWalkingUp)
                {
                    walkingCounter += Time.deltaTime;
                    isWalkingDown = false;
                    isWalkingUp = true;
                    npcAnim.SetBool("walkingUp", isWalkingUp);
                    rb.velocity = new Vector2(0, movement);
                }
            }
            else
            {
                if (walkingCounter >= walkingTime || isWalkingRight)
                {
                    walkingCounter -= Time.deltaTime;
                    isWalkingRight = true;
                    npcAnim.SetBool("walkingRight", isWalkingRight);
                    rb.velocity = new Vector2(movement, 0);
                }

                if (walkingCounter <= 0 || !isWalkingRight)
                {
                    walkingCounter += Time.deltaTime;
                    isWalkingRight = false;
                    npcAnim.SetBool("walkingRight", isWalkingRight);
                    rb.velocity = new Vector2(-movement, 0);
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            npcAnim.SetBool("isPlayer", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            canWalk = false;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            canWalk = true;
        }
    }
}
