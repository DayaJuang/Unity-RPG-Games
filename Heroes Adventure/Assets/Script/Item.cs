using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Detail")]
    public string itemName;
    [TextArea(3,10)]
    public string description;
    public int price;
    public Sprite itemSprite;

    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    public bool isAccessory;

    [Header("Item Affect")]
    public int amountToChange;
    public bool affectHP, affectMP, cureAilment, affectAtk,affectDef,affectMag;

    public void Use(int charToUse)
    {
        CharStat selectedChar = GameManager.instance.characterStats[charToUse];

        if (isItem)
        {
            if (affectHP)
            {
                if(selectedChar.currentHP == selectedChar.maxHP)
                {
                    FindObjectOfType<NotificationBox>().ShowNotifBox("Fully Recovered!");
                    return;
                }

                selectedChar.currentHP += amountToChange;

                if (selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                if (selectedChar.currentMP == selectedChar.maxMP)
                {
                    FindObjectOfType<NotificationBox>().ShowNotifBox("Fully Recovered!");
                    return;
                }

                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }
            if (affectAtk)
            {
                selectedChar.atkPwr += amountToChange;
            }
            if (affectDef)
            {
                selectedChar.atkPwr += amountToChange;
            }
        }
        if (isWeapon)
        {
            if (selectedChar.equipWp != "(none)")
            {
                GameManager.instance.AddItem(selectedChar.equipWp, 1);
            }
            selectedChar.equipWp = itemName;
            selectedChar.weaponPower = amountToChange;

        }
        if (isArmor)
        {
            if (selectedChar.equipArmor != "(none)")
            {
                GameManager.instance.AddItem(selectedChar.equipArmor, 1);
            }
            selectedChar.equipArmor = itemName;
            selectedChar.armorPower = amountToChange;

        }
        if (isAccessory)
        {
            if (selectedChar.equipAcc != "(none)")
            {
                GameManager.instance.AddItem(selectedChar.equipAcc, 1);
            }
            selectedChar.equipAcc = itemName;
            if (affectAtk)
            {
                selectedChar.weaponPower = amountToChange;
            }
            if (affectDef)
            {
                selectedChar.armorPower = amountToChange;
            }
            if (affectMag)
            {
                selectedChar.magicPower = amountToChange;
            }

        }
        GameManager.instance.SubtractItem(FindObjectOfType<MainMenu>().activeItem.itemName);
        GameManager.instance.SortItem();
    }
}
