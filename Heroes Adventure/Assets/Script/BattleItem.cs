using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItem : MonoBehaviour
{
    public GameObject menu;
    public Text itemNameText, descriptionText,useButtonText;
    public ItemButton[] itemButtons;
    public Item selectedItem;
    public static BattleItem instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        if (!menu.gameObject.activeInHierarchy)
        {
            GameManager.instance.SortItem();
            menu.SetActive(true);
            ShowItem();

            itemButtons[0].Press();
        }
        else
        {
            menu.SetActive(false);
        }
    }

    public void ShowItem()
    {
        GameManager.instance.SortItem();
        //Show player inventory
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].inventoryIndex = i;  //Set the index of each item box

            //If the item box is not empty
            if (GameManager.instance.itemHelds[i] != "")
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

    public void SelectItem(Item item)
    {
        selectedItem = item;

        if(selectedItem != null)
        {
            itemNameText.text = selectedItem.itemName;
            descriptionText.text = selectedItem.description;

            if (item.isWeapon || item.isArmor || item.isAccessory)
            {
                useButtonText.text = "Equip";
            }
            else
            {
                useButtonText.text = "Use";
            }
        }
    }

    public void Use(int charToUse)
    {
        if (selectedItem != null)
        {
            List<BattleChar> player = new List<BattleChar>();

            for(int i = 0; i < BattleManager.instance.battlersList.Count; i++)
            {
                if (BattleManager.instance.battlersList[i].isPlayer)
                {
                    player.Add(BattleManager.instance.battlersList[i]);
                }
            }

            if (selectedItem.isItem)
            {
                if (selectedItem.affectHP)
                {
                    player[charToUse].currentHP += selectedItem.amountToChange;

                    if (player[charToUse].currentHP > player[charToUse].maxHP)
                    {
                        player[charToUse].currentHP = player[charToUse].maxHP;
                    }
                }

                if (selectedItem.affectMP)
                {
                    player[charToUse].currentMP += selectedItem.amountToChange;

                    if (player[charToUse].currentMP > player[charToUse].maxMP)
                    {
                        player[charToUse].currentMP = player[charToUse].maxMP;
                    }
                }
                if (selectedItem.affectAtk)
                {
                    player[charToUse].atkPower += selectedItem.amountToChange;
                }
                if (selectedItem.affectDef)
                {
                    player[charToUse].atkPower += selectedItem.amountToChange;
                }
            }
        }
        GameManager.instance.SubtractItem(selectedItem.itemName);
        GameManager.instance.SortItem();
    }
}
