using UnityEngine;
using UnityEngine.UI;

public class NotificationBox : MonoBehaviour
{
    public GameObject notifBox;
    public Text notifText;
    bool isJustOpen;

    private void Update()
    {
        if (notifBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                AudioManager.instance.PlaySFX(5);

                if (isJustOpen)
                {
                    isJustOpen = false;
                }
                else
                {
                    CloseNotifBox();
                }
            }
        }
    }

    public void ShowNotifBox(string text)
    {
        notifBox.gameObject.SetActive(true);
        GameManager.instance.isDialogOpen = true;
        notifText.text = text;
        isJustOpen = true;
    }

    public void ShowNotifBox(int gold)
    {
        notifBox.gameObject.SetActive(true);
        GameManager.instance.isDialogOpen = true;
        notifText.text = "You got " + gold+ "g";
        isJustOpen = true;

    }

    public void ShowNotifBox(string itemName,int quantity)
    {
        notifBox.gameObject.SetActive(true);
        GameManager.instance.isDialogOpen = true;
        notifText.text = "You got " + itemName + " " + quantity + "x";
        isJustOpen = true;
        
    }

    public void CloseNotifBox()
    {
        notifBox.gameObject.SetActive(false);
        GameManager.instance.isDialogOpen = false;
        
    }
}
