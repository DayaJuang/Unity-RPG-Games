using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    //Make an array that contains chest name
    public string[] chestNameMarkers;
    //Make an array to check the chest status(open/close)
    public bool[] chestStatus;

    // Start is called before the first frame update
    void Start()
    {
        //Set the array of chestName and chestStatus to be the same
        chestStatus = new bool[chestNameMarkers.Length];
    }

    //Find the index of chestName
    //take the chest name as the parameter to compare
    //return the index of the chest name we search for
    public int FindChestIndex(string chestName)
    {
        //Loop through the array
        for(int i = 0; i < chestNameMarkers.Length; i++)
        {
            //if the chestName equal to chestNameMarkers at index i
            if (chestNameMarkers[i] == chestName)
            {
                //return the index
                return i;
            }
        }
        //if nothing found, return -1
        return -1;
    }

    //Check the chest status
    //take the chest name as the parameter
    //return true or false
    public bool CheckChestStatus(string chestToCheck)
    {
        //Based on the index check the status of the chest
        int chestIndex = FindChestIndex(chestToCheck);
        //if the index is not -1
        if(chestIndex != -1)
        {
            //return the check Status
            return chestStatus[chestIndex];
        }
        //else, just return false
        return false;
    }

    //Mark the chest as opened
    //take the chest name as the parameter
    //no return
    public void MarkAsOpened(string chestName)
    {
        chestStatus[FindChestIndex(chestName)] = true;
        UpdateChestStatus();
    }

    //Update the chest status everytime mark as opened
    public void UpdateChestStatus()
    {
        Chest[] chests = FindObjectsOfType<Chest>();
        //Loop through all the object and check the status
        for(int i = 0; i < chests.Length; i++)
        {
            chests[i].OpenedChest();
        }
    }

    public void SaveChestStatus()
    {
        for(int i = 0; i < chestNameMarkers.Length; i++)
        {
            if (chestStatus[i])
            {
                PlayerPrefs.SetInt("Chest_" + chestNameMarkers[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("Chest_" + chestNameMarkers[i], 0);
            }
        }
    }

    public void LoadChestStatus()
    {
        int initValue = 0;

        for(int i = 0; i < chestNameMarkers.Length; i++)
        {
            if (PlayerPrefs.HasKey("Chest_" + chestNameMarkers[i]))
            {
                initValue = PlayerPrefs.GetInt("Chest_" + chestNameMarkers[i]);
            }

            if(initValue == 1)
            {
                chestStatus[i] = true;
            }
            else
            {
                chestStatus[i] = false;
            }
        }
    }
}
