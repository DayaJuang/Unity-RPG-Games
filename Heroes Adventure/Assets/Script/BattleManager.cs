using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST}

public class BattleManager : MonoBehaviour
{
    [Header("Battle UI")]
    public GameObject battleScene,playerDataPanel,enemySelectPanel,actionPanel,magicSelectPanel,playerSelectPanel;
    public SpriteRenderer background;
    public BattleInfo infoPanel;
    public Button[] enemyTargetButtons,playerTargetButton;
    public Text[] enemyNameText,playerNameText;
    public Text infoPanelText;
    public Text[] playerInfoText;
    public DamageText damageText, healText,criticalText;

    [Header("Magic UI")]
    public MagicSelect[] magics;

    [Header("Battle Character")]
    public Transform[] playerPosition;
    public Transform[] enemyPosition;
    public BattleChar[] playerChar;
    public BattleChar[] enemies;
    public BattleChar[] boss;

    [Header("Reward Panel")]
    public GameObject rewardPanel;
    public Text expText, itemtext;

    [Header("PlayerList")]
    public List<BattleChar> battlersList = new List<BattleChar>();

    [Header("Battle Parameter & Other")]
    public int turnPhase;
    public string moveToUse;
    public AttackEffect attackEffect;
    public ParticleSystem particle, itemEffect;
    

    public static BattleManager instance;
    public BattleState state;
    public bool isBoss;
    bool isBattleStart;
    int damage;
    bool isWaitingTurn;
    bool usingItem;

    int criticalChance = 15;
    float criticalDmg = 2f;
    int expEarn = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        state = BattleState.START;
        if (FindObjectOfType<CameraController>())
        {
            background.sprite = FindObjectOfType<CameraController>().battleBG;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattleStart)
        {
            if (isWaitingTurn)
            {
                if(battlersList[turnPhase].isPlayer)
                {
                    actionPanel.SetActive(true);
                    state = BattleState.PLAYERTURN;
                }
                else
                {
                    actionPanel.SetActive(false);
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }

        UpdateUIData();
    }

    public void StartBattle(bool isBossBattle)
    {
        isBoss = isBossBattle;
        StartCoroutine(LoadingUp());
    }

    //Setting up player and enemies
    IEnumerator LoadingUp()
    {
            AudioManager.instance.PlaySFX(0);
            FindObjectOfType<UIFade>().Fade();
            GameManager.instance.isLoading = true;
            yield return new WaitForSeconds(1f);

            FinishLoading();
    }

    void FinishLoading()
    {
        GameManager.instance.isLoading = false;
        FindObjectOfType<UIFade>().FadeAway();
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        if (!isBattleStart)
        {
            isBattleStart = true;
            GameManager.instance.isBattle = true;
            battleScene.SetActive(true);
            playerDataPanel.SetActive(true);
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            AudioManager.instance.isBattle = true;

            infoPanel.Activate();
            infoPanelText.text = "Enemy Appear !";

            SettingUpPlayer();
            SettingUpEnemy();

            turnPhase = 0;
            isWaitingTurn = true;

            yield return new WaitForSeconds(1f);
        }
    }

    void SettingUpPlayer()
    {
        CharStat[] player = GameManager.instance.characterStats;

        for (int i = 0; i < playerChar.Length; i++)
        {
            if (player[i].gameObject.activeInHierarchy)
            {
                BattleChar newPlayer = Instantiate(playerChar[i], playerPosition[i].position, playerPosition[i].rotation);
                newPlayer.transform.parent = playerPosition[i];
                battlersList.Add(newPlayer);

                playerInfoText[i].gameObject.SetActive(true);
                battlersList[i].currentHP = player[i].currentHP;
                battlersList[i].maxHP = player[i].maxHP;
                battlersList[i].currentMP = player[i].currentMP;
                battlersList[i].maxMP = player[i].maxMP;
                battlersList[i].atkPower = player[i].atkPwr;
                battlersList[i].defPower = player[i].defPwr;
                battlersList[i].magPwr = player[i].magPwr;
            }
        }
    }

    void SettingUpEnemy()
    {
        CameraController area = FindObjectOfType<CameraController>();

        if (isBoss)
        {
            for (int i = 0; i < boss.Length; i++)
            {
                if (boss[i].charName == Boss.instance.bossName)
                {
                    BattleChar newEnemy = Instantiate(boss[i], enemyPosition[0].position, enemyPosition[0].rotation);
                    newEnemy.transform.parent = enemyPosition[0];
                    battlersList.Add(newEnemy);
                }
            }
        }
        else
        {
            int numberOfEnemies = Random.Range(1, enemyPosition.Length + 1);

            for (int i = 0; i < numberOfEnemies; i++)
            {
                int enemyToSpawn = Random.Range(0, area.enemies.Length);

                for (int j = 0; j < enemies.Length; j++)
                {
                    if (enemies[j].charName == area.enemies[enemyToSpawn])
                    {
                        BattleChar newEnemy = Instantiate(enemies[j], enemyPosition[i].position, enemyPosition[i].rotation);
                        newEnemy.transform.parent = enemyPosition[i];
                        battlersList.Add(newEnemy);
                    }
                }
            }
        }
    }

    public List<int> FindPlayerIndex()
    {
        List<int> playerIndex = new List<int>();
        for (int i = 0; i < battlersList.Count; i++)
        {
            if (battlersList[i].isPlayer && battlersList[i].currentHP > 0)
            {
                playerIndex.Add(i);
            }
        }

        return playerIndex;
    }

   public List<int> FindEnemyIndex()
    {
        List<int> enemyIndex = new List<int>();
        for (int i = 0; i < battlersList.Count; i++)
        {
            if (!battlersList[i].isPlayer)
            {
                enemyIndex.Add(i);
            }
        }

        return enemyIndex;
    }

    public void OnAttackButton()
    {
        moveToUse = "Attack";
        OpenEnemySelectMenu();
    }

    public void OnMagicButton()
    {
        for(int i = 0; i < magics.Length; i++)
        {
            magics[i].gameObject.SetActive(false);
        }

        if (!magicSelectPanel.gameObject.activeInHierarchy)
        {
            magicSelectPanel.gameObject.SetActive(true);

            for (int i = 0; i < battlersList[turnPhase].characterSpells.Length; i++)
            {
                if (battlersList[turnPhase].isPlayer)
                {
                    magics[i].gameObject.SetActive(true);
                    magics[i].magicName = battlersList[turnPhase].characterSpells[i].spellName;
                    magics[i].manaCost = battlersList[turnPhase].characterSpells[i].manaCost;
                    magics[i].power = battlersList[turnPhase].characterSpells[i].power;
                    magics[i].isHealingMagic = battlersList[turnPhase].characterSpells[i].isHealingSpell;
                    magics[i].magicText.text = battlersList[turnPhase].characterSpells[i].spellName;
                    magics[i].moveEffect = battlersList[turnPhase].characterSpells[i].moveEffect;
                    magics[i].manaText.text = battlersList[turnPhase].characterSpells[i].manaCost.ToString();
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            magicSelectPanel.gameObject.SetActive(false);
        }
    }

    public IEnumerator DamageEnemy(int targetIndex)
    {
        List<int> enemyIndex = FindEnemyIndex();

        Instantiate(particle, battlersList[turnPhase].transform.position, battlersList[turnPhase].transform.rotation);
        yield return new WaitForSeconds(.5f);

        if (moveToUse == "Attack")
        {
            damage = battlersList[turnPhase].atkPower - Mathf.FloorToInt(battlersList[enemyIndex[targetIndex]].defPower * 0.5f);
            int critical = Random.Range(0, 100);
            if (critical <= criticalChance)
            {
                damage *= Mathf.FloorToInt(criticalDmg);
                Instantiate(criticalText, battlersList[enemyIndex[targetIndex]].transform.position, battlersList[enemyIndex[targetIndex]].transform.rotation).SetText(damage);
            }
            else
            {
                Instantiate(damageText, battlersList[enemyIndex[targetIndex]].transform.position, battlersList[enemyIndex[targetIndex]].transform.rotation).SetText(damage);
            }

            Instantiate(attackEffect, battlersList[enemyIndex[targetIndex]].transform.position, battlersList[enemyIndex[targetIndex]].transform.rotation);
        }
        else
        {
            for (int i = 0; i < magics.Length; i++)
            {
                if (moveToUse == magics[i].magicName)
                {
                    damage = magics[i].power + battlersList[turnPhase].magPwr;
                    battlersList[turnPhase].currentMP -= magics[i].manaCost;
                    infoPanelText.text = magics[i].magicName;
                    infoPanel.Activate();
                    Instantiate(magics[i].moveEffect, battlersList[enemyIndex[targetIndex]].transform.position, battlersList[enemyIndex[targetIndex]].transform.rotation);
                    Instantiate(damageText, battlersList[enemyIndex[targetIndex]].transform.position, battlersList[enemyIndex[targetIndex]].transform.rotation).SetText(damage);
                }
            }
        }

        battlersList[enemyIndex[targetIndex]].TakeDamage(damage);
        OnAttackButton();
        yield return new WaitForSeconds(.5f);
        NextTurn();
    }

    public IEnumerator Heal(int targetIndex)
    {
        List<int> playerIndex = FindPlayerIndex();

        Instantiate(particle, battlersList[turnPhase].transform.position, battlersList[turnPhase].transform.rotation);
        yield return new WaitForSeconds(.5f);

        if (usingItem)
        {
            BattleItem.instance.Use(targetIndex);
            Instantiate(itemEffect, battlersList[playerIndex[targetIndex]].transform.position, battlersList[playerIndex[targetIndex]].transform.rotation);
            AudioManager.instance.PlaySFX(7);
        }
        else
        {
            for (int i = 0; i < magics.Length; i++)
            {
                if (moveToUse == magics[i].magicName)
                {
                    battlersList[playerIndex[targetIndex]].currentHP += magics[i].power;
                    battlersList[turnPhase].currentMP -= magics[i].manaCost;

                    if (battlersList[playerIndex[targetIndex]].currentHP >= battlersList[playerIndex[targetIndex]].maxHP)
                    {
                        battlersList[playerIndex[targetIndex]].currentHP = battlersList[playerIndex[targetIndex]].maxHP;
                    }

                    infoPanelText.text = magics[i].magicName;
                    infoPanel.Activate();
                    Instantiate(magics[i].moveEffect, battlersList[playerIndex[targetIndex]].transform.position, battlersList[playerIndex[targetIndex]].transform.rotation);
                    Instantiate(healText, battlersList[playerIndex[targetIndex]].transform.position, battlersList[playerIndex[targetIndex]].transform.rotation).SetText(magics[i].power);
                }
            }
        }
        
        OpenPlayerSelectMenu();
        yield return new WaitForSeconds(.5f);
        NextTurn();
    }

    public void Attack(int index)
    {
        StartCoroutine(DamageEnemy(index));
    }

    public void UseHeal(int index)
    {
        StartCoroutine(Heal(index));
    }

    public void Flee()
    {
        if (isBoss)
        {
            infoPanelText.text = "Can't Escape !";
            infoPanel.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess <= 50)
            {
                StartCoroutine(EndBattle());
            }
            else
            {
                infoPanelText.text = "Can't Escape !";
                infoPanel.Activate();
                state = BattleState.ENEMYTURN;
                while (battlersList[turnPhase].isPlayer)
                {
                    NextTurn();
                }
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        isWaitingTurn = false;
        yield return new WaitForSeconds(1f);
        Instantiate(particle, battlersList[turnPhase].transform.position, battlersList[turnPhase].transform.rotation);
        yield return new WaitForSeconds(1f);
        int enemyAction = Random.Range(0, 100);
        if(enemyAction <= 35 && battlersList[turnPhase].characterSpells.Length != 0)
        {
            EnemySpell();
        }
        else
        {
            EnemyAttack();
        }
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    void EnemyAttack()
    {
        List<int> playerIndex = FindPlayerIndex();

        int target = playerIndex[Random.Range(0, playerIndex.Count)];
        int damage = battlersList[turnPhase].atkPower - Mathf.FloorToInt(battlersList[target].defPower * .5f);
        Instantiate(attackEffect, battlersList[target].transform.position, battlersList[target].transform.rotation);
        Instantiate(damageText, battlersList[target].transform.position, battlersList[target].transform.rotation).SetText(damage);
        battlersList[target].TakeDamage(damage);
    }

    void EnemySpell()
    {
        List<int> playerIndex = FindPlayerIndex();

        int spellToUse = Random.Range(0, battlersList[turnPhase].characterSpells.Length);
        int target = playerIndex[Random.Range(0, playerIndex.Count)];

        if(battlersList[turnPhase].currentMP >= battlersList[turnPhase].characterSpells[spellToUse].manaCost)
        {
            int damage = battlersList[turnPhase].characterSpells[spellToUse].power + battlersList[turnPhase].magPwr;
            battlersList[turnPhase].currentMP -= battlersList[turnPhase].characterSpells[spellToUse].manaCost;
            infoPanelText.text = battlersList[turnPhase].characterSpells[spellToUse].spellName;
            infoPanel.Activate();
            Instantiate(battlersList[turnPhase].characterSpells[spellToUse].moveEffect, battlersList[playerIndex[target]].transform.position, battlersList[target].transform.rotation);
            Instantiate(damageText, battlersList[target].transform.position, battlersList[target].transform.rotation).SetText(damage);
            
            battlersList[target].TakeDamage(damage);
        }
        else
        {
            EnemyAttack();
        }
    }

    public void NextTurn()
    {
        turnPhase++;
        if (turnPhase >= battlersList.Count)
        {
            turnPhase = 0;
        }

        isWaitingTurn = true;
        CheckBattleStatus();
    }

    //Panel Code
    //For Select an Enemy Target
    public void OpenEnemySelectMenu()
    {
        List<int> enemyIndex = FindEnemyIndex();

        if (!enemySelectPanel.gameObject.activeInHierarchy)
        {
            for(int i = 0; i < enemyTargetButtons.Length; i++)
            {
                enemyTargetButtons[i].gameObject.SetActive(false);
            }
            enemySelectPanel.SetActive(true);
            for (int i = 0; i < enemyIndex.Count; i++)
            {
                if (battlersList[enemyIndex[i]].currentHP > 0 && !battlersList[enemyIndex[i]].isPlayer)
                {
                    enemyTargetButtons[i].gameObject.SetActive(true);
                    enemyNameText[i].text = battlersList[enemyIndex[i]].charName;
                }
                else
                {
                    enemyTargetButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            enemySelectPanel.SetActive(false);
        }
    }

    public void OpenPlayerSelectMenu()
    {
        List<int> playerIndex = FindPlayerIndex();
        if (BattleItem.instance.menu.activeInHierarchy)
        {
            BattleItem.instance.OpenMenu();
            usingItem = true;
        }
        else
        {
            usingItem = false;
        }

        if (!playerSelectPanel.gameObject.activeInHierarchy)
        {
            playerSelectPanel.SetActive(true);
            for (int i = 0; i < playerIndex.Count; i++)
            {
                if (battlersList[playerIndex[i]].currentHP > 0)
                {
                    playerTargetButton[i].gameObject.SetActive(true);
                    playerNameText[i].text = battlersList[playerIndex[i]].charName;
                }
                else
                {
                    playerTargetButton[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            playerSelectPanel.SetActive(false);
        }
    }

    //Checking Battle status
    public void CheckBattleStatus()
    {
        bool isAllEnemyDead = true;
        bool isAllPlayerDead = true;

        for(int i = 0; i < battlersList.Count; i++)
        {
            if(battlersList[i].currentHP == 0)
            {
                battlersList[i].Dead();
            }
            else
            {
                if (battlersList[i].isPlayer)
                {
                    isAllPlayerDead = false;
                }
                else
                {
                    isAllEnemyDead = false;
                }
            }
        }
        if(isAllEnemyDead || isAllPlayerDead)
        {
            //If all enemy dead
            if (isAllEnemyDead)
            {
                state = BattleState.WIN;
                StartCoroutine(BattleReward());
            }
            else
            {
                state = BattleState.LOST;
                StartCoroutine(GameOver());
            }
        }
        else
        {
            while(battlersList[turnPhase].currentHP <= 0)
            {
                turnPhase++;
                if (turnPhase >= battlersList.Count)
                {
                    turnPhase = 0;
                }
            }
        }
    }

    public IEnumerator BattleReward()
    {
        isBattleStart = false;
        AudioManager.instance.isBattle = false;
        AudioManager.instance.isBattleEndTransition = true;
        AudioManager.instance.bgm[FindObjectOfType<CameraController>().battleTrack].Stop();

        yield return new WaitForSeconds(1f);
        playerDataPanel.SetActive(false);
        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        playerSelectPanel.SetActive(false);
        AudioManager.instance.PlaySFX(8);
        for (int i = 0; i < battlersList.Count; i++)
        {
            if (!battlersList[i].isPlayer)
            {
                expEarn += battlersList[i].exp;

                for (int k = 0; k < battlersList[i].dropableItem.Length; k++)
                {
                    int getItem = Random.Range(0, 100);
                    if (getItem <= battlersList[i].dropRate[k])
                    {
                        GameManager.instance.AddItem(battlersList[i].dropableItem[k], 1);
                        itemtext.text = battlersList[i].dropableItem[k] + " " + "1x" + "\n";
                    }
                    else
                    {
                        itemtext.text = " ";
                    }
                }
            }
        }

        expText.text = "All your party earn " + expEarn + " exp";

        rewardPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        
        StartCoroutine(EndBattle());
    }

    public IEnumerator GameOver()
    {
        isBattleStart = false;
        playerDataPanel.SetActive(false);
        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        playerSelectPanel.SetActive(false);
        AudioManager.instance.isBattle = false;
        AudioManager.instance.isGameOver = true;
        AudioManager.instance.bgm[FindObjectOfType<CameraController>().battleTrack].Stop();
        AudioManager.instance.PlaySFX(9);

        yield return new WaitForSeconds(1f);
        FindObjectOfType<UIFade>().Fade();

        yield return new WaitForSeconds(2f);
        FindObjectOfType<UIFade>().FadeAway();
        SceneManager.LoadScene("GameOver");
    }

    public IEnumerator EndBattle()
    {
        isBattleStart = false;
        playerDataPanel.SetActive(false);
        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        playerSelectPanel.SetActive(false);
        rewardPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        FindObjectOfType<UIFade>().Fade();
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < battlersList.Count; i++)
        {
            if (battlersList[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.characterStats.Length; j++)
                {
                    if(battlersList[i].charName == GameManager.instance.characterStats[j].playerName)
                    {
                        GameManager.instance.characterStats[j].currentHP = battlersList[i].currentHP;
                        GameManager.instance.characterStats[j].currentMP = battlersList[i].currentMP;
                        GameManager.instance.characterStats[j].LevelingUp(expEarn);
                    }
                }
            }
            Destroy(battlersList[i].gameObject);
        }

        FindObjectOfType<UIFade>().FadeAway();
        battlersList.Clear();
        battleScene.SetActive(false);
        turnPhase = 0;
        expEarn = 0;
        GameManager.instance.isBattle = false;

        AudioManager.instance.isBattle = false;
        AudioManager.instance.isBattleEndTransition = false;

    }

    public void UpdateUIData()
    {
        for (int i = 0; i < battlersList.Count; i++)
        {
            if (battlersList[i].isPlayer)
            {
                playerInfoText[i].text = battlersList[i].charName + " HP " + battlersList[i].currentHP + "/" + battlersList[i].maxHP + " MP " + battlersList[i].currentMP + "/" + battlersList[i].maxMP;
            }
        }
    }
}


