using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public bool canOpenShop;
    public string[] itemToSale = new string[21];

    //Set the player to be able to open store
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpenShop = true;
        }
    }
    //Set player to cannot open the store
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpenShop = false;
        }
    }
}
