using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Script that carry the player data

    public static GameManager instance;
    public CharStat[] characterStats;
    public bool isMenuOpen, isDialogOpen, isLoading,isBattle;

    public string[] itemHelds;
    public int[] itemQuantity;
    public Item[] reference;

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SortItem();
    }

    // Update is called once per frame
    void Update()
    {
        //Check the condition is the player can move or not
        if (isMenuOpen || isDialogOpen || isLoading||isBattle)
        {
            PlayerController.instance.canMove = false;
            FindObjectOfType<AreaUI>().isAbleToShow = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
            FindObjectOfType<AreaUI>().isAbleToShow = true;
        }

        FindObjectOfType<MainMenu>().UpdateStatus();
    }

    //To return the reference of item in the item script
    public Item FindItem(string itemName)
    {
        for(int i = 0; i < reference.Length; i++)
        {
            if(reference[i].itemName == itemName)
            {
                return reference[i];
            }
        }
        return null;
    }

    public void SortItem()
    {
        bool itemAfterSpace = true; //Check if there is still a space ot not
        FindObjectOfType<MainMenu>().ShowItem();

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for(int i=0; i < itemHelds.Length - 1; i++)
            {
                if(itemHelds[i] == "")
                {
                    itemHelds[i] = itemHelds[i + 1];
                    itemHelds[i + 1] = "";

                    itemQuantity[i] = itemQuantity[i + 1];
                    itemQuantity[i + 1] = 0;

                    //Do the loop until there is no space between each item
                    if(itemHelds[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    //Check if the item is exist or not
    bool CheckItem(string itemToCheck)
    {
        for (int i = 0; i < reference.Length; i++)
        {
            if(reference[i].itemName == itemToCheck)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(string itemToAdd,int amountToAdd)
    {
        bool itemExist = CheckItem(itemToAdd);

        for(int i = 0; i < itemHelds.Length; i++)
        {
            if(itemHelds[i] == itemToAdd || itemHelds[i] == "" && itemExist)
            {
                itemHelds[i] = itemToAdd;
                itemQuantity[i] += amountToAdd;
                return;
            }
        }
    }

    public void SubtractItem(string itemToSubtract)
    {
        bool itemExist = CheckItem(itemToSubtract);

        for (int i = 0; i < itemHelds.Length; i++)
        {
            if (itemHelds[i] == itemToSubtract && itemExist)
            {
                itemQuantity[i]--;
                if (itemQuantity[i] == 0)
                {
                    if (MainMenu.instance.mainMenu.activeInHierarchy)
                    {
                        itemHelds[i] = "";
                        MainMenu.instance.itemName.text = "";
                        MainMenu.instance.itemDesc.text = "";
                        SortItem();
                        return;
                    }
                    if (ShopMenu.instance.sellWindow.activeInHierarchy)
                    {
                        itemHelds[i] = "";
                        ShopMenu.instance.selectedItem = null;
                        ShopMenu.instance.itemSellName.text = "";
                        ShopMenu.instance.itemSellDesc.text = "";
                        SortItem();
                        return;
                    }
                }
            }
        }
    }

    public void DiscardItem(string itemToDiscard)
    {
        bool itemExist = CheckItem(itemToDiscard);

        for (int i = 0; i < itemHelds.Length; i++)
        {
            if (itemHelds[i] == itemToDiscard && itemExist)
            {
                itemHelds[i] = "";
                itemQuantity[i] = 0;
                MainMenu.instance.itemName.text = "";
                MainMenu.instance.itemDesc.text = "";
                SortItem();
                return;
            }
        }
    }

    public void SaveCharData()
    {
        //Save current scene
        PlayerPrefs.SetString("Scene_", SceneManager.GetActiveScene().name);

        for(int i = 0; i < characterStats.Length; i++)
        {
            if (characterStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + characterStats[i].name, 1);

                //Save Character Status
                PlayerPrefs.SetInt("Level_" + characterStats[i].name, characterStats[i].playerLevel);
                PlayerPrefs.SetInt("CurrentExp_" + characterStats[i].name, characterStats[i].currentEXP);
                PlayerPrefs.SetInt("CurrentHP_" + characterStats[i].name, characterStats[i].currentHP);
                PlayerPrefs.SetInt("MaxHP_" + characterStats[i].name, characterStats[i].maxHP);
                PlayerPrefs.SetInt("CurrentMP_" + characterStats[i].name, characterStats[i].currentMP);
                PlayerPrefs.SetInt("MaxMP_" + characterStats[i].name, characterStats[i].maxMP);
                PlayerPrefs.SetInt("STR_" + characterStats[i].name, characterStats[i].str);
                PlayerPrefs.SetInt("AGI_" + characterStats[i].name, characterStats[i].agi);
                PlayerPrefs.SetInt("DEF_" + characterStats[i].name, characterStats[i].def);
                PlayerPrefs.SetInt("INT_" + characterStats[i].name, characterStats[i].intelligence);
                PlayerPrefs.SetInt("AtkPower_" + characterStats[i].name, characterStats[i].atkPwr);
                PlayerPrefs.SetInt("WeaponPower_" + characterStats[i].name, characterStats[i].weaponPower);
                PlayerPrefs.SetInt("DefPower_" + characterStats[i].name, characterStats[i].defPwr);
                PlayerPrefs.SetInt("ArmorPower_" + characterStats[i].name, characterStats[i].armorPower);
                PlayerPrefs.SetInt("MagPower_" + characterStats[i].name, characterStats[i].magPwr);
                PlayerPrefs.SetInt("MagicPower_" + characterStats[i].name, characterStats[i].magicPower);
                PlayerPrefs.SetString("EquipWpn_" + characterStats[i].name, characterStats[i].equipWp);
                PlayerPrefs.SetString("EquipArmor_" + characterStats[i].name, characterStats[i].equipArmor);
                PlayerPrefs.SetString("EquipAcc_" + characterStats[i].name, characterStats[i].equipAcc);
            }
        }

        //Save Player Position
        PlayerPrefs.SetFloat("PosX", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("PosY", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("PosZ", PlayerController.instance.transform.position.z);

        //Save Player Inventory
        for(int i = 0; i < itemHelds.Length; i++)
        {
            PlayerPrefs.SetString("Inventory_" + i, itemHelds[i]);
            PlayerPrefs.SetInt("Quantity_" + i, itemQuantity[i]);
        }

        //Save Gold
        PlayerPrefs.SetInt("Gold", currentGold);
    }

    public void LoadCharData()
    {
        //Load Scene
        SceneManager.LoadScene(PlayerPrefs.GetString("Scene_"));

        for (int i = 0; i < characterStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_"+characterStats[i].name)==1)
            {
                characterStats[i].gameObject.SetActive(true);

                //Load Character Status
                characterStats[i].playerLevel = PlayerPrefs.GetInt("Level_" + characterStats[i].name);
                characterStats[i].currentEXP = PlayerPrefs.GetInt("CurrentExp_" + characterStats[i].name);
                characterStats[i].currentHP = PlayerPrefs.GetInt("CurrentHP_" + characterStats[i].name);
                characterStats[i].maxHP = PlayerPrefs.GetInt("MaxHP_" + characterStats[i].name);
                characterStats[i].currentMP = PlayerPrefs.GetInt("CurrentMP_" + characterStats[i].name);
                characterStats[i].maxMP = PlayerPrefs.GetInt("MaxMP_" + characterStats[i].name);
                characterStats[i].str = PlayerPrefs.GetInt("STR_" + characterStats[i].name);
                characterStats[i].agi = PlayerPrefs.GetInt("AGI_" + characterStats[i].name);
                characterStats[i].def = PlayerPrefs.GetInt("DEF_" + characterStats[i].name);
                characterStats[i].intelligence = PlayerPrefs.GetInt("INT_" + characterStats[i].name);
                characterStats[i].atkPwr = PlayerPrefs.GetInt("AtkPower_" + characterStats[i].name);
                characterStats[i].defPwr = PlayerPrefs.GetInt("DefPower_" + characterStats[i].name);
                characterStats[i].armorPower = PlayerPrefs.GetInt("ArmorPower_" + characterStats[i].name);
                characterStats[i].magPwr = PlayerPrefs.GetInt("MagPower_" + characterStats[i].name);
                characterStats[i].equipWp = PlayerPrefs.GetString("EquipWpn_" + characterStats[i].name);
                characterStats[i].equipArmor = PlayerPrefs.GetString("EquipArmor_" + characterStats[i].name);
                characterStats[i].equipAcc = PlayerPrefs.GetString("EquipAcc_" + characterStats[i].name);
                characterStats[i].magicPower = PlayerPrefs.GetInt("MagicPower_" + characterStats[i].name);
                characterStats[i].weaponPower = PlayerPrefs.GetInt("WeaponPower_" + characterStats[i].name);
            }
            else
            {
                characterStats[i].gameObject.SetActive(false);
            }
        }

        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        //Load Gold
        currentGold = PlayerPrefs.GetInt("Gold");

        //Load Inventory
        for(int i = 0; i < itemHelds.Length; i++)
        {
            itemHelds[i] = PlayerPrefs.GetString("Inventory_" + i);
            itemQuantity[i] = PlayerPrefs.GetInt("Quantity_" + i);
        }
    }
}
