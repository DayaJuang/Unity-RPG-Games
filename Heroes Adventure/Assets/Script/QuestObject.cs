using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    //Quest name
    public string questName;
    //The Object we want to change
    public GameObject questObject;
    //Are we want to activate it after the quest complete
    public bool activateObject;
    //Initial Check
    private bool initialCheck;
    //Is deactivate the script
    public bool deactivateQuest;

    // Update is called once per frame
    void Update()
    {
        if (!initialCheck)
        {
            initialCheck = true;
        }

        QuestCompletion();
    }

    //Check if the quest is completed or not
    //If complete, should we activate the object or not
    public void QuestCompletion()
    {
        if (QuestManager.instance.CheckQuestStatus(questName))
        {
            //Remove script after quest complete
            //If there are multiple script in one object
            if (deactivateQuest)
            {
                GetComponent<QuestObject>().enabled = false;
            }
            //Set the object to be active or not if the quest complete
            questObject.SetActive(activateObject);
        }
    }
}
