using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    float timeToLoad = 1f;

    // Update is called once per frame
    void Update()
    {
        if (timeToLoad > 0)
        {
            timeToLoad -= Time.deltaTime;

            if (timeToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Scene_"));
                GameManager.instance.LoadCharData();
                QuestManager.instance.LoadQuestData();
                FindObjectOfType<ChestManager>().LoadChestStatus();
            }
        }
    }
}
