using UnityEngine;

public class EssentialLoader : MonoBehaviour
{

    public GameObject uiCanvas;
    public GameObject player;
    public GameObject gm;
    public GameObject audioManager;
    public GameObject battleManager;

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<UIFade>() == null)
        {
            Instantiate(uiCanvas).GetComponent<UIFade>();
        }

        if(FindObjectOfType<PlayerController>() == null)
        {
            Instantiate(player).GetComponent<PlayerController>();
        }

        if (GameManager.instance == null)
        {
            Instantiate(gm).GetComponent<GameManager>();
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audioManager).GetComponent<AudioManager>();
        }

        if(BattleManager.instance == null)
        {
            Instantiate(battleManager).GetComponent<BattleManager>();
        }

    }
}
