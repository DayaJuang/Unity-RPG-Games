using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    bool canPick;
    public Item itemInside;
    public int quantity;
    public Animator chestAnim;
    public bool isOpen,isItem;
    public int gold;
    public static PickUpItem instance;

    // Update is called once per frame
    void Update()
    {
        if(canPick && Input.GetButtonDown("Fire1") && FindObjectOfType<PlayerController>().canMove)
        {
            if (!isOpen)
            {
                if (isItem)
                {
                    GameManager.instance.AddItem(itemInside.name, quantity);
                    FindObjectOfType<NotificationBox>().ShowNotifBox(itemInside.name, quantity);
                }
                else
                {
                    GameManager.instance.currentGold += gold;
                    FindObjectOfType<NotificationBox>().ShowNotifBox(gold);
                }

                chestAnim.SetBool("isEmpty", true);
                isOpen = true;
            }
            else
            {
                FindObjectOfType<NotificationBox>().ShowNotifBox("Empty");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPick = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPick = false;
        }
    }
}
