using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string loadScene;
    public string mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(MainMenu.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(loadScene);
        Destroy(gameObject);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Destroy(gameObject);
    }
}
