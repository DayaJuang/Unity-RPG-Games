using UnityEngine;
using UnityEngine.UI;

public class CharStat : MonoBehaviour
{
    public string playerName = "Aldo";
    public string characterClass;
    public int playerLevel = 1;
    public int currentEXP;
    int baseExp = 100;
    public int[] expToNextLevel;
    int maxLevel = 100;

    public int currentHP;
    public int maxHP = 150;
    public int currentMP;
    public int maxMP = 50;
    public int str;
    public int def;
    public int intelligence;
    public int agi;

    public int atkPwr,weaponPower;
    public int defPwr,armorPower;
    public int magPwr,magicPower;

    public string equipWp;
    public string equipArmor;
    public string equipAcc;

    public Sprite playerImage;
    public Sprite playerPotrait;

    System.Random rnd;



    // Start is called before the first frame update
    void Start()
    {
        //Create an EXP for each level
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExp;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //Adding status for each level depending on the status
        atkPwr = Mathf.FloorToInt(str * 2.05f) + weaponPower;
        defPwr = Mathf.FloorToInt(def * 1.05f) + armorPower;
        magPwr = Mathf.FloorToInt(intelligence * 2.5f) + magicPower;
    }

    public void LevelingUp(int exp)
    {
        currentEXP += exp;

        if(playerLevel < maxLevel)
        {
            if (currentEXP >= expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;
                StatusUp();
            }
        }
        if(playerLevel == maxLevel)
        {
            currentEXP = expToNextLevel[maxLevel];
        }
       
    }

    void StatusUp()
    {
        rnd = new System.Random();

        switch (characterClass)
        {
            case "Warrior":
                //Strength gain
                str += rnd.Next(1, 4);
                //Def gain
                def += rnd.Next(1, 3);
                //int gain
                intelligence += rnd.Next(0, 3);
                break;
            case "Knight":
                //Strength gain
                str += rnd.Next(1, 2);
                //Def gain
                def += rnd.Next(1, 4);
                //int gain
                intelligence += rnd.Next(0, 2);
                break;
            case "Mage":
                //Strength gain
                str += rnd.Next(0, 3);
                //Def gain
                def += rnd.Next(1, 2);
                //int gain
                intelligence += rnd.Next(1, 4);
                break;

        }
        maxHP += Mathf.FloorToInt(def * 0.5f);
        maxMP += Mathf.FloorToInt(intelligence * 0.5f);

        currentHP = maxHP;
        currentMP = maxMP;
    }
}
