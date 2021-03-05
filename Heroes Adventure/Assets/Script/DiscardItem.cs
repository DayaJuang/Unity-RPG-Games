using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardItem : MonoBehaviour
{
    public void Discard()
    {
        GameManager.instance.DiscardItem(FindObjectOfType<MainMenu>().activeItem.itemName);
        GameManager.instance.SortItem();
        FindObjectOfType<MainMenu>().discardPanel.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        if (FindObjectOfType<MainMenu>().discardPanel.gameObject.activeInHierarchy)
        {
            FindObjectOfType<MainMenu>().discardPanel.gameObject.SetActive(false);
        }

        if (MainMenu.instance.characterSelectPanel.gameObject.activeInHierarchy)
        {
            MainMenu.instance.characterSelectPanel.gameObject.SetActive(false);
        }
        
    }
}
