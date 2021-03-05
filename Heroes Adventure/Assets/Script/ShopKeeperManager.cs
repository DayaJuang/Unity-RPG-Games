using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperManager : MonoBehaviour
{
    public GameObject shopKeeper;

    // Update is called once per frame
    void Update()
    {
        if (shopKeeper.GetComponent<ShopKeeper>().canOpenShop)
        {
           ShopMenu.instance.itemToSale = shopKeeper.GetComponent<ShopKeeper>().itemToSale;
        }
        
        if (ShopMenu.instance.shopMenu.activeInHierarchy)
        {
            shopKeeper.GetComponent<StartDialog>().enabled = false;
        }
        else
        {
            shopKeeper.GetComponent<StartDialog>().enabled = true;
        }
    }
}
