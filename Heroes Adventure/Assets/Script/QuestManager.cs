using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //Make an instance
    public static QuestManager instance;
    //Create an Array of the quest
    public string[] questNames;
    //Create an Array of the quest completion
    public bool[] questCompletions;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //Set the length of 2 arrays to be the same
        questCompletions = new bool[questNames.Length];
    }

    //Find the index of the quest name
    //Used for check the completion afterwards
    //Take quest name parameter
    //return index
    public int GetQuestIndex(string questToCheck)
    {
        //Loop through the questNames array
        //Check if the questName at i is equal to questToCheck
        for(int i = 0; i < questNames.Length; i++)
        {
            if (questNames[i] == questToCheck)
            {
                return i;
            }
        }
        return -1;
    }

    //Check the quest completion
    //Take quest name as parameter
    //return true or false
    public bool CheckQuestStatus(string questName)
    {
        //Get the index of quest name
        //Use GetQuestIndex function
        int questIndex = GetQuestIndex(questName);
        if (questIndex != -1)
        {
            //Return the status of the quest
            return questCompletions[questIndex];
        }
        return false;
    }

    //Mark Quest as Completed
    //Take quest name as parameter
    //return nothing
    public void MarkAsComplete(string questName)
    {
        questCompletions[GetQuestIndex(questName)] = true;
        UpdateQuestStatus();
    }

    //Mark Quest as Incompleted
    //Take quest name as parameter
    //return nothing
    public void MarkAsIncomplete(string questName)
    {
        questCompletions[GetQuestIndex(questName)] = false;
        UpdateQuestStatus();
    }

    //Update the quest status
    public void UpdateQuestStatus()
    {
        QuestObject[] questObjects = FindObjectsOfType<QuestObject>();

        for(int i = 0; i < questObjects.Length; i++)
        {
            questObjects[i].QuestCompletion();
        }
    }

    public void SaveQuestStatus()
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            //If the quest is completed
            //Set the value to be 1, else 0
            if (questCompletions[i])
            {
                PlayerPrefs.SetInt("Quest_" + questNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("Quest_" + questNames[i], 0);
            }
        }
    }

    public void LoadQuestData()
    {
        //Set the initValue to be 0
        int initValue = 0;
        //Loop the quest name
        for (int i = 0; i < questNames.Length; i++)
        {
            if (PlayerPrefs.HasKey("Quest_" + questNames[i]))
            {
                initValue = PlayerPrefs.GetInt("Quest_" + questNames[i]);
            }
            if (initValue == 1)
            {
                MarkAsComplete(questNames[i]);
            }
            else
            {
                MarkAsIncomplete(questNames[i]);
            }

        }
    }
}
