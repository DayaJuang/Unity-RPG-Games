using UnityEngine;

public class StartDialog : MonoBehaviour
{
    //A Script component for the NPC
    //Contains difference dialog and interaction with player for each NPC

    [TextArea(3,10)]
    public string[] lines;
    bool canActive;
    public bool isPerson = true;

    [Header("Item Store")]
    public bool isShopkeeper;

    [Header("Quest Section")]
    public string questName;
    public bool markAsCompleted;
    public bool shouldActivateAfterDialog;

    // Update is called once per frame
    void Update()
    {
        //Show a dialog if the dialog haven't started yet and player press the button
        if(canActive && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines,isPerson);
            DialogManager.instance.MarkQuestAfterDialog(questName, markAsCompleted);
            DialogManager.instance.OpeningShopAfterDialog(isShopkeeper);
        }
    }

    //Check if the player is in the dialog area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            canActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActive = false;
        }
    }
    
}
