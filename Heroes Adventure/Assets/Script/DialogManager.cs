using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    //Script component for the UI canvas
    //Control the dialog UI in the game

    public static DialogManager instance;
    public Text nameText;
    public Text dialogText;
    public GameObject nameBox;
    public GameObject dialogBox;
    public Image playerPotrait;

    public string[] dialogLines;
    private int currentLine;
    private bool isJustStarted;

    //Shop Variable
    private bool shouldOpenShopMenu;

    //Quest Variable
    private string questName;
    private bool questStatus;
    private bool shouldMarkQuest;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Check is the dialog is already active
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                AudioManager.instance.PlaySFX(5);
                //Check if the dialog just started or not
                if (isJustStarted)
                {
                    isJustStarted = false;
                }
                else
                {
                    currentLine++;  //Go to the next line

                    //Check the array index to prevent error
                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        FindObjectOfType<GameManager>().isDialogOpen = false;
                        if (shouldMarkQuest)
                        {
                            MarkQuestAfterDialog(questName, questStatus);
                        }
                        if (shouldOpenShopMenu)
                        {
                            ShopMenu.instance.OpenShop();
                        }
                        return;
                    }
                    //Set the NPC name and the dialog
                    SetName();
                    dialogText.text = dialogLines[currentLine];
                }
            }
        }
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        CheckPerson(isPerson);

        //Assign the dialog lines depends on the NPC
        dialogLines = newLines; 

        //Start the dialog
        currentLine = 0;
        SetName();
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        isJustStarted = true;

        FindObjectOfType<GameManager>().isDialogOpen = true; //make player cannot move during the dialog 
    }

    void SetName()
    {
        //Set the name to the dialog UI
        //To make a 2 ways conversation
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", ""); //Assign the name
            
            //Check if the speaker is the character
            for(int i = 0; i < GameManager.instance.characterStats.Length; i++)
            {
                //If it's a character and already unlocked
                if (GameManager.instance.characterStats[i].playerName.Equals(nameText.text)&&GameManager.instance.characterStats[i].gameObject.activeInHierarchy)
                {
                    playerPotrait.sprite = GameManager.instance.characterStats[i].playerPotrait;
                    playerPotrait.gameObject.SetActive(true);
                    currentLine++;
                    return;
                }
                //If not, just end the dialog
                if(GameManager.instance.characterStats[i].playerName.Equals(nameText.text) && !GameManager.instance.characterStats[i].gameObject.activeInHierarchy)
                {
                    dialogBox.SetActive(false);
                    FindObjectOfType<GameManager>().isDialogOpen = false;
                }
            }
            playerPotrait.gameObject.SetActive(false);
            currentLine++;
        }
    }

    void CheckPerson(bool isPerson)
    {
        //Set namebox Depend on it is a person or not 
        nameBox.SetActive(isPerson);
        playerPotrait.gameObject.SetActive(isPerson);

        //Check is it a person or just a sign
        //Change the text alignment
        if (!isPerson)
        {
            dialogText.alignment = TextAnchor.MiddleCenter;
        }
        else
        {
            dialogText.alignment = TextAnchor.UpperLeft;
        }
    }

    //Mark the quest if the conversation is ended
    public void MarkQuestAfterDialog(string questToCheck,bool markAsComplete)
    {
        questName = questToCheck;
        questStatus = markAsComplete;
        shouldMarkQuest = true;

        if (markAsComplete)
        {
            QuestManager.instance.MarkAsComplete(questToCheck);
        }
        else
        {
            QuestManager.instance.MarkAsIncomplete(questToCheck);
        }
    }

    public void OpeningShopAfterDialog(bool isShopkeeper)
    {
        shouldOpenShopMenu = isShopkeeper;
    }
}
