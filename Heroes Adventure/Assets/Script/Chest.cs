using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //Create 2 game object of PickUpItem
    //Closed chest and opened chest
    public PickUpItem closedChest, openedChest;
    ChestManager cm;
    //Chest name to check it to the chest manager
    public string chestName;
    //Create variable to check if is already checked or not
    bool isAlreadyChecked;

    // Update is called once per frame
    void Update()
    {
        cm = GetComponent<ChestManager>();

        if (!isAlreadyChecked)
        {
            isAlreadyChecked = true;

            OpenedChest();
        }
        if (closedChest.isOpen)
        {
            FindObjectOfType<ChestManager>().MarkAsOpened(chestName);
        }
    }

    //When opened, deactive the closed chest object and activate the opened chest object
    //No parameter
    //return nothing
    public void OpenedChest()
    {
        if (FindObjectOfType<ChestManager>().CheckChestStatus(chestName))
        {
            closedChest.gameObject.SetActive(false);
            openedChest.gameObject.SetActive(true);
        }
    }
}
