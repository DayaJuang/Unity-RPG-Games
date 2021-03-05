using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public GameObject shopMenu,buyWindow,sellWindow;
    public string[] itemToSale;
    
    [Header("Buy Window")]
    public ItemButton[] shopItemButtons;
    public Text itemName, itemDesc, valueText;

    [Header("Sell Window")]
    public ItemButton[] shopSellButtons;
    public Text itemSellName, itemSellDesc, sellValue;

    public Text currentGold;
    public static ShopMenu instance;
    public Item selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void OpenShop()
    {
        //Update the gold
        currentGold.text = GameManager.instance.currentGold.ToString() + "g";
        //Check if menu is already open or not
        if (!shopMenu.activeInHierarchy)
        {
            //Open the menu
            shopMenu.SetActive(true);
            //Open the buy tab for first time
            OpenBuyTab();
            //Prevent player to walk
            GameManager.instance.isMenuOpen = true;
        }
        else
        {
            //Close the menu
            shopMenu.SetActive(false);
            //Let player move
            GameManager.instance.isMenuOpen = false;
        }
    }

    //Opening a buy tab
    public void OpenBuyTab()
    {
        //Open the buy tab and close the sell if it open
        if (sellWindow.gameObject.activeInHierarchy)
        {
            sellWindow.gameObject.SetActive(false);
        }
        buyWindow.gameObject.SetActive(true);
        //Make always to select the first item in windows
        shopItemButtons[0].Press();
        //Show the item for sale
        for(int i = 0; i < shopItemButtons.Length; i++)
        {
            //Set index of each item box
            shopItemButtons[i].inventoryIndex = i;
            //Check if the box is null or not
            if(itemToSale[i] != "")
            {
                //Set the sprite and item data based on the itemForSale array
                shopItemButtons[i].itemImage.gameObject.SetActive(true);
                shopItemButtons[i].itemImage.sprite = GameManager.instance.FindItem(itemToSale[i]).itemSprite;
            }
            else
            {
                //Set the box to be blank
                shopItemButtons[i].itemImage.gameObject.SetActive(false);
            }
            //set the amount to be always unlimited
            shopItemButtons[i].amount.text = "";
        }
    }

    public void OpenSellTab()
    {
        //Open the sell tab and close the buy if it open
        if (buyWindow.gameObject.activeInHierarchy)
        {
            buyWindow.gameObject.SetActive(false);
        }
        sellWindow.gameObject.SetActive(true);
        //Make always to select the first item in windows
        shopItemButtons[0].Press();
        ShowSellItem();
    }

    void ShowSellItem()
    {
        GameManager.instance.SortItem();
        //Show player inventory
        for (int i = 0; i < shopSellButtons.Length; i++)
        {
            shopSellButtons[i].inventoryIndex = i;  //Set the index of each item box

            //If the item box is not empty
            if (GameManager.instance.itemHelds[i] != "")
            {
                shopSellButtons[i].itemImage.gameObject.SetActive(true);
                shopSellButtons[i].itemImage.sprite = GameManager.instance.FindItem(GameManager.instance.itemHelds[i]).itemSprite;
                shopSellButtons[i].amount.text = GameManager.instance.itemQuantity[i].ToString();
            }
            else
            {
                shopSellButtons[i].itemImage.gameObject.SetActive(false);
                shopSellButtons[i].amount.text = "";
            }
        }
    }

    //Select Item in Shop Menu
    public void SelectItemInShop(Item item)
    {
        selectedItem = item;
        if (selectedItem != null)
        {
            //Check if it buy or sell tab that open
            if (buyWindow.activeInHierarchy)
            {
                //Change the name and desc
                itemName.text = selectedItem.itemName;
                itemDesc.text = selectedItem.description;
                //Get the normal price
                valueText.text = "Price : " + selectedItem.price.ToString() + "g";
            }
            if(sellWindow.activeInHierarchy)
            {
                //Change the name and desc
                itemSellName.text = selectedItem.itemName;
                itemSellDesc.text = selectedItem.description;
                //Get the price for sell
                sellValue.text = "Price : " + Mathf.FloorToInt(selectedItem.price * .5f).ToString() + "g";
            }
        }
    }

    //Make action button for buy/sell
    public void BuyOrSell()
    {
        //If the buy window is open
        if(buyWindow.activeInHierarchy)
        {
            //Make sure the selected item is not null
            if(selectedItem != null)
            {
                //Check if player gold is greater than or equal to the item price
                if(GameManager.instance.currentGold >= selectedItem.price)
                {
                    //If the gold is enough
                    //Add the item that we want to buy
                    GameManager.instance.AddItem(selectedItem.itemName, 1);
                    //Subtract the current player gold
                    GameManager.instance.currentGold -= selectedItem.price;
                }
                else
                {
                    //If not enough
                    //Show the message "You don't have enough gold" 
                    FindObjectOfType<NotificationBox>().ShowNotifBox("You don't have enough gold");
                }
            }
        }
        if(sellWindow.activeInHierarchy)
        {
            //Make sure the selected item is not null
            if(selectedItem != null)
            {
                //Update the player gold by adding the sell price to the current gold
                GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.price * .5f);
                //Subtract the player item from inventory
                GameManager.instance.SubtractItem(selectedItem.name);
            }
            ShowSellItem();
        }
        //Update the gold
        currentGold.text = GameManager.instance.currentGold.ToString() + "g";
    }
}
