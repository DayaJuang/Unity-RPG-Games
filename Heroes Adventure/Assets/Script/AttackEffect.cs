using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float time;
    public int trackNumber;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(trackNumber);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, time);
    }
}
