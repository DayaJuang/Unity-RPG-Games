using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public string charName;
    public int level;
    public int currentHP, maxHP, currentMP, maxMP, atkPower, defPower,magPwr,exp;
    public bool isPlayer;
    public string[] dropableItem;
    public int[] dropRate;
    public Spell[] characterSpells;
    public SpriteRenderer sprite;

    private bool shouldFadeAway;
    float fadeSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeAway)
        {
            sprite.color = new Color(Mathf.MoveTowards(sprite.color.r, 255f, fadeSpeed*Time.deltaTime), Mathf.MoveTowards(sprite.color.g, 0f, fadeSpeed*Time.deltaTime), Mathf.MoveTowards(sprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(sprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(sprite.color.a == 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    public void Dead()
    {
        shouldFadeAway = true;
    }
}
