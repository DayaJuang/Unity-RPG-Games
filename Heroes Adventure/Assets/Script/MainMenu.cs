using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    private CharStat[] charStats;
    public Text[] nameTexts,levelTexts, hpTexts, mpTexts, expTexts, strTexts, intTexts, defTexts, agiTexts;
    public GameObject[] charHolders;
    public Image[] charImages;
    public Image charPotraits;

    //To Open Different Windows in Menu
    public GameObject[] menuWindows;

    //Status Windows Button
    public GameObject[] charStatusButton;

    //Item Button
    public ItemButton[] itemButtons;

    //Gold Text
    public Text goldText;

    
    public Text nameStatus, levelStatus, hpStatus, mpStatus, strStatus, intStatus, defStatus, agiStatus, atkPower, defPower,magPower,wpnText,armorText,accText;
    public Image wpnIcon, armorIcon, accIcon;

    [Header("Item Panel")]
    public static MainMenu instance;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDesc,useButtonText;

    [Header("Discard System")]
    public GameObject discardPanel;

    [Header("Use System")]
    public GameObject characterSelectPanel;
    public Button[] characterButton;
    public Text[] useCharName, useCharLevel, useCharHP, useCharMP;
    public Image[] useCharImage;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        mainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();

        if (Input.GetKeyDown(KeyCode.M))
        {
            //Prevent opening menu while other UI is open
            if (!GameManager.instance.isDialogOpen && !GameManager.instance.isLoading && !ShopMenu.instance.shopMenu.activeInHierarchy)
            {
                //Close the menu
                if (mainMenu.activeInHierarchy)
                {
                    for (int i = 0; i < menuWindows.Length; i++)
                    {
                        menuWindows[i].SetActive(false);
                    }
                    FindObjectOfType<GameManager>().isMenuOpen = false;
                    mainMenu.SetActive(false);
                }
                else
                {
                    FindObjectOfType<GameManager>().isMenuOpen = true;
                    UpdateStatus();
                    mainMenu.SetActive(true);
                }
            }
            AudioManager.instance.PlaySFX(4);
        }

        if (characterSelectPanel.activeInHierarchy)
        {
            CharacterSelectPanel();
        }
    }

    //Update a Character Status
    public void UpdateStatus()
    {
        //Set the array to be the same as the Character Status in Game Manager Script
        charStats = GameManager.instance.characterStats;

        //Update Current Gold
        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        for(int i = 0; i < charStats.Length; i++)
        {
            //If the character is already unlocked
            //Set the holder ON and update the status
            if (charStats[i].gameObject.activeInHierarchy)
            {
                charHolders[i].SetActive(true);
                nameTexts[i].text = charStats[i].playerName;
                levelTexts[i].text = "LV " + charStats[i].playerLevel;
                expTexts[i].text = "EXP : " + charStats[i].currentEXP + "/" + charStats[i].expToNextLevel[charStats[i].playerLevel];
                hpTexts[i].text = "HP : " + charStats[i].currentHP + "/" + charStats[i].maxHP;
                mpTexts[i].text = "MP : " + charStats[i].currentMP + "/" + charStats[i].maxMP;
                strTexts[i].text = "STR : " + charStats[i].str;
                defTexts[i].text = "DEF : " + charStats[i].def;
                intTexts[i].text = "INT : " + charStats[i].intelligence;
                agiTexts[i].text = "AGI : " + charStats[i].agi;

                charImages[i].sprite = charStats[i].playerImage;
            }
            else
            {
                charHolders[i].SetActive(false);
            }
        }
    }

    //To Activate a window and close the other
    public void OpenWindows(int windowsNumber)
    {
        //windowNumber indicate the index of the button in the menu
        UpdateStatus();

        for(int i = 0; i < menuWindows.Length; i++)
        {
            if (i == windowsNumber)
            {
                menuWindows[i].SetActive(!menuWindows[i].activeInHierarchy);
            }
            else
            {
                menuWindows[i].SetActive(false);
            }
        }
    }

    //Open a Status window, change the button text to be a player name
    //Based on it's active in the hierarcy or no
    public void OpenStatus()
    {
        UpdateStatus(); //Upadate player status everytime window is open

        SelectCharStatus(0); //Make the main character to always be shown first

        for(int i=0;i < charStatusButton.Length; i++)
        {
            charStatusButton[i].SetActive(charStats[i].gameObject.activeInHierarchy);   //Set the button based on the player already being unlocked or not
            charStatusButton[i].GetComponentInChildren<Text>().text = charStats[i].playerName;  //Change the button text
        }
        
    }

    //Select a Character and show their status
    public void SelectCharStatus(int selected)
    {
        charPotraits.sprite = charStats[selected].playerPotrait;

        nameStatus.text = charStats[selected].playerName.ToString();
        levelStatus.text = "Level : " + charStats[selected].playerLevel;
        hpStatus.text = "HP : " + charStats[selected].currentHP + "/" + charStats[selected].maxHP;
        mpStatus.text = "MP : " + charStats[selected].currentMP + "/" + charStats[selected].maxMP;
        strStatus.text = "Strength : " + charStats[selected].str;
        defStatus.text = "Defence : " + charStats[selected].def;
        intStatus.text = "Intelligence : " + charStats[selected].intelligence;
        agiStatus.text = "Agility : " + charStats[selected].agi;
        atkPower.text = "Attack Power : " + charStats[selected].atkPwr;
        defPower.text = "Defence Power : " + charStats[selected].defPwr;
        magPower.text = "Magic Power : " + charStats[selected].magPwr;

        //Check if there is weapon equipped or not
        if(charStats[selected].equipWp == "")
        {
            //Set the equipped weapon to none
            //Deactive the weapon icon
            wpnText.text = "(none)";
            wpnIcon.gameObject.SetActive(false);

        }
        else
        {
            wpnIcon.gameObject.SetActive(true);
            wpnIcon.sprite = FindObjectOfType<GameManager>().FindItem(charStats[selected].equipWp).itemSprite;
            wpnText.text = charStats[selected].equipWp;
        }

        //Check if there is an armor equippde
        if (charStats[selected].equipArmor == "")
        {
            //Set equipped armor to none
            armorText.text = "(none)";
            //Deactive armor sprite
            armorIcon.gameObject.SetActive(false);
        }
        else
        {
            //Activate the armor icon
            armorIcon.gameObject.SetActive(true);
            //Change icon
            armorIcon.sprite = FindObjectOfType<GameManager>().FindItem(charStats[selected].equipArmor).itemSprite;
            //Change equipped armor text
            armorText.text = charStats[selected].equipArmor;
        }

        if(charStats[selected].equipAcc == "")
        {
            accText.text = "(none)";
            accIcon.gameObject.SetActive(false);
        }
        else
        {
            accIcon.gameObject.SetActive(true);
            accIcon.sprite = FindObjectOfType<GameManager>().FindItem(charStats[selected].equipAcc).itemSprite;
            accText.text = charStats[selected].equipAcc;
        }
    }

    //If item menu is pressed, run this function
    public void ShowItem()
    {
        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].inventoryIndex = i;  //Set the index of each item box

            //If the item box is not empty
            if(GameManager.instance.itemHelds[i] != "")
            {
                itemButtons[i].itemImage.gameObject.SetActive(true);
                itemButtons[i].itemImage.sprite = GameManager.instance.FindItem(GameManager.instance.itemHelds[i]).itemSprite;
                itemButtons[i].amount.text = GameManager.instance.itemQuantity[i].ToString();
            }
            else
            {
                itemButtons[i].itemImage.gameObject.SetActive(false);
                itemButtons[i].amount.text = "";
            }
        }
    }

    //Function that will be called if the button in the item box is pressed
    //Change the information of the item based on the reference from GameManager
    public void SelectItem(Item item)
    {
        activeItem = item;

        itemName.text = activeItem.itemName;
        itemDesc.text = activeItem.description;

        if (item.isWeapon || item.isArmor || item.isAccessory)
        {
            useButtonText.text = "Equip";
        }
        else
        {
            useButtonText.text = "Use";
        }
    }

    public void ShowDiscardPanel()
    {
        if(activeItem != null)
        {
            discardPanel.gameObject.SetActive(true);
        }
    }

    public void CharacterSelectPanel()
    {
        if (activeItem != null)
        {
            characterSelectPanel.gameObject.SetActive(true);

            for(int i = 0; i < characterButton.Length; i++)
            {
                characterButton[i].gameObject.SetActive(GameManager.instance.characterStats[i].gameObject.activeInHierarchy);
                useCharName[i].text = GameManager.instance.characterStats[i].playerName;
                useCharLevel[i].text = "LV " + GameManager.instance.characterStats[i].playerLevel;
                useCharHP[i].text = "HP : " + GameManager.instance.characterStats[i].currentHP + "/" + GameManager.instance.characterStats[i].maxHP;
                useCharMP[i].text = "MP : " + GameManager.instance.characterStats[i].currentMP + "/" + GameManager.instance.characterStats[i].maxMP;
                useCharImage[i].sprite = GameManager.instance.characterStats[i].playerImage;
            }
        }
    }
    //Close the character menu window function
    public void CloseCharacterMenu()
    {
        //Check if the Character menu is open or not
        if (mainMenu.activeInHierarchy)
        {
            //Close all open tab / window
            for (int i = 0; i < menuWindows.Length; i++)
            {
                menuWindows[i].SetActive(false);
            }
            //Make player can move
            FindObjectOfType<GameManager>().isMenuOpen = false;
            //Close the character window
            mainMenu.SetActive(false);
        }
    }

    public void UseItem(int charToUse)
    {
        activeItem.Use(charToUse);

        if (!activeItem.isItem)
        {
            FindObjectOfType<NotificationBox>().ShowNotifBox("Equipped!");
            AudioManager.instance.PlaySFX(1);
            characterSelectPanel.SetActive(false);
        }
        else
        {
                AudioManager.instance.PlaySFX(7);
        }
    }

    public void SaveData()
    {
        GameManager.instance.SaveCharData();
        FindObjectOfType<ChestManager>().SaveChestStatus();
        QuestManager.instance.SaveQuestStatus();
        FindObjectOfType<NotificationBox>().ShowNotifBox("Data Saved !");
    }

    public void LoadData()
    {
        GameManager.instance.LoadCharData();
        FindObjectOfType<ChestManager>().LoadChestStatus();
        QuestManager.instance.LoadQuestData();
        mainMenu.SetActive(false);
        GameManager.instance.isMenuOpen = false;
    }

    public void ButtonSound()
    {
        AudioManager.instance.PlaySFX(5);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(GameManager.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(gameObject);
    }
}
