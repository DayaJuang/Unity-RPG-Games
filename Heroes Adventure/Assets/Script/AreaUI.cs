using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AreaUI : MonoBehaviour
{
    public Image areaUI;
    public Text areaName;
    public bool isAbleToShow = true;

    // Update is called once per frame
    void Update()
    {
        if (isAbleToShow && SceneManager.GetActiveScene().name!="LoadingScene")
        {
            areaName.text = SceneManager.GetActiveScene().name;
            areaUI.gameObject.SetActive(true);
        }
        else
        {
            areaUI.gameObject.SetActive(false);
        }
    }
}
