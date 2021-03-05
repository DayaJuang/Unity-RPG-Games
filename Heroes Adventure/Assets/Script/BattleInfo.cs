using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfo : MonoBehaviour
{
    float awakeTime = 1f;
    float counterTime;

    // Update is called once per frame
    void Update()
    {
        if(counterTime > 0)
        {
            counterTime -= Time.deltaTime;

            if(counterTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        counterTime = awakeTime;
    }
}
