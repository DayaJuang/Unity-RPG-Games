using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSelect : MonoBehaviour
{
    public string magicName;
    public int manaCost;
    public int power;
    public bool isHealingMagic;
    public Text magicText;
    public Text manaText;
    public AttackEffect moveEffect;

    public void Use()
    {
        if(BattleManager.instance.battlersList[BattleManager.instance.turnPhase].currentMP >= manaCost)
        {
            BattleManager.instance.magicSelectPanel.SetActive(false);
            BattleManager.instance.moveToUse = magicName;

            if (!isHealingMagic)
            {
                BattleManager.instance.OpenEnemySelectMenu();
            }
            else
            {
                BattleManager.instance.OpenPlayerSelectMenu();
            }
            
        }
        else
        {
            BattleManager.instance.infoPanel.Activate();
            BattleManager.instance.infoPanelText.text = "Not Enough MP!";
            BattleManager.instance.OnMagicButton();
        }
    }
}
