using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public bool isBattleTrigger;
    public float battleTimer = 5f;
    public float battleTrigger;

    // Start is called before the first frame update
    void Start()
    {
        battleTrigger = Random.Range(battleTimer * .5f, battleTimer * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isBattleTrigger && PlayerController.instance.canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                battleTrigger -= Time.deltaTime;
            }
            if (battleTrigger <= 0)
            {
                battleTrigger = Random.Range(battleTimer * .5f, battleTimer * 1.5f);
                BattleManager.instance.StartBattle(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isBattleTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isBattleTrigger = false;
        }
    }
}
