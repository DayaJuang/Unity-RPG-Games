using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image itemImage;
    public Text amount;
    public int inventoryIndex;

    public void Press()
    {
        //If the item menu is open
        if (MainMenu.instance.mainMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemHelds[inventoryIndex] != "")
            {
                MainMenu.instance.SelectItem(GameManager.instance.FindItem(GameManager.instance.itemHelds[inventoryIndex]));
            }
        }

        //If the shop menu is open
        if (ShopMenu.instance.shopMenu.activeInHierarchy)
        {
            //Check if the buy tab is open
            if(ShopMenu.instance.buyWindow.activeInHierarchy)
            {
                //Show the detail for buy item
                ShopMenu.instance.SelectItemInShop(GameManager.instance.FindItem(ShopMenu.instance.itemToSale[inventoryIndex]));
            }
            if(ShopMenu.instance.sellWindow.activeInHierarchy)
            {
                //Show the detail item in inventory to sell
                ShopMenu.instance.SelectItemInShop(GameManager.instance.FindItem(GameManager.instance.itemHelds[inventoryIndex]));
            }
        }
        //If the battle item menu is open
        if (BattleItem.instance.menu.activeInHierarchy)
        {
            if (GameManager.instance.itemHelds[inventoryIndex] != "")
            {
                BattleItem.instance.SelectItem(GameManager.instance.FindItem(GameManager.instance.itemHelds[inventoryIndex]));
            }
        }
        
    }
}
