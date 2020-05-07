using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform closestEnemy;
    public AverageEnemy averageEnemyPrefab;
    public TankEnemy tankEnemyPrefab;
    public TankEnemyTutorial tankEnemyTutPrefab;
    public NimbleEnemy nimbleEnemyPrefab;
    public Image blackScreen;
    [Header("Wave generator info")]
    public float summonInterval = 5f;
    public int maxEnemyCount = 25;
    public int enemiesOnScreen;
    public int maxEnemiesOnScreen = 3;
    public int enemyKillCount;
    private const int averageEnemyCnt = 5;
    private const int tankEnemyCnt = 2;
    private const int nimbleEnemyCnt = 3;
    [Header("Debug")]
    public float minXPos = -2.5f;
    public float maxXPos = 2.5f;
    public float maxYPos = 3f;
    public float scanInterval;
    private bool sufferedDamage;
    private bool isGameOver;
    private Enemy tankTutorial;
    public List<Enemy> enemies = new List<Enemy>();
    public static System.Action OnDialogueCall;
    public static System.Action OnGameOver;
    public static GameManager Instance {get; private set;}
    #endregion
    private void Awake() 
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start() 
    {
        Enemy.OnDefeated += ReactToEnemyDefeat;
        DialogueManager.OnDialogueEnd += OnDialogueEnd;
        Enemy.OnReachedDestination += LoseGame;
        Color color = Color.black;
        color.a = 0;
        blackScreen.color = color;
        GenerateEnemies();
        InvokeRepeating(nameof(FindClosestEnemy), 1f, scanInterval);
        StartCoroutine(FadeFromBlack());
        Invoke(nameof(CallDialogue), 0.25f);
        // CallDialogue();
    }
    private void OnDisable() 
    {
        Enemy.OnDefeated -= ReactToEnemyDefeat;
        DialogueManager.OnDialogueEnd -= OnDialogueEnd;
        Enemy.OnReachedDestination -= LoseGame;
    }
    private void OnDialogueEnd()
    {
        if (enemyKillCount == 0 || enemyKillCount == 1)
        {
            Enemy e = GetAverageEnemy();
            e.transform.position = GetStartingPosition();
            e.gameObject.SetActive(true);
            enemiesOnScreen++;
        }
        else if (enemyKillCount == 3)
        {
            Enemy tankTutorial = Instantiate(tankEnemyTutPrefab, transform, false);
            enemies.Add(tankTutorial);
            tankTutorial.transform.position = GetStartingPosition();
            tankTutorial.gameObject.SetActive(true);
            enemiesOnScreen++;
        }
        else if (enemyKillCount == 4)
        {
            enemies.Remove(tankTutorial);
            Destroy(tankTutorial);
            Enemy e = GetNimbleEnemy();
            e.transform.position = GetStartingPosition();
            e.gameObject.SetActive(true);
            enemiesOnScreen++;
        }
        else if (enemyKillCount == 5)
        {
            SummonEnemy();
        }

        if (isGameOver)
            StartCoroutine(FadeToBlack());
    }
    private void ReactToEnemyDefeat()
    {
        enemyKillCount++;
        enemiesOnScreen--;

        if (enemyKillCount == 1)
            CallDialogue();
        else if (enemyKillCount == 2)
        {
            Enemy e = GetAverageEnemy();
            e.transform.position = GetStartingPosition();
            e.gameObject.SetActive(true);
            enemiesOnScreen++;
        }
        else if (enemyKillCount == 3)
            CallDialogue();
        else if (enemyKillCount == 4)
            CallDialogue();
        else if (enemyKillCount == 5)
            CallDialogue();
        else if (enemyKillCount == 6)
            SummonEnemy();
    }
    private void FindClosestEnemy()
    {
        float minY = 3f;
        Transform lowestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy && enemy.transform.position.y < minY)
            {
                minY = enemy.transform.position.y;
                lowestEnemy = enemy.transform;
            }
        }

        closestEnemy = lowestEnemy;
    }
    private void GenerateEnemies()
    {
        for (int i = 0; i < averageEnemyCnt; i++)
        {
            enemies.Add(Instantiate(averageEnemyPrefab, transform, false));
        }
        for (int i = 0; i < tankEnemyCnt; i++)
        {
            enemies.Add(Instantiate(tankEnemyPrefab, transform, false));
        }
        for (int i = 0; i < nimbleEnemyCnt; i++)
        {
            enemies.Add(Instantiate(nimbleEnemyPrefab, transform, false));
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
    private void SummonEnemy()
    {
        if (enemyKillCount >= maxEnemyCount)
        {
            EndGame();
            return;
        }

        if (enemiesOnScreen >= maxEnemiesOnScreen)
        {
            Invoke(nameof(SummonEnemy), summonInterval);
            return;
        }

        if (enemyKillCount + enemiesOnScreen < maxEnemyCount)
        {
            int rng = Random.Range(0, enemies.Count);
            Enemy e = enemies[rng];

            if (e.gameObject.activeInHierarchy && rng < enemies.Count - 1)
                e = enemies[rng + 1];

            if (!e.gameObject.activeInHierarchy)
            {
                e.transform.position = GetStartingPosition();
                e.gameObject.SetActive(true);
                enemiesOnScreen++;
            }
        }
        
        Invoke(nameof(SummonEnemy), summonInterval);
    }
    private void EndGame()
    {
        if (OnGameOver != null) OnGameOver();
        
        isGameOver = true;

        if (!sufferedDamage)
            CallDialogue();   
        else
            StartCoroutine(FadeToBlack());
    }
    private IEnumerator FadeFromBlack()
    {
        Color color = Color.black;
        blackScreen.color = color;
        blackScreen.gameObject.SetActive(true);
        int i = 0;

        while (i < 20)
        {
            color = blackScreen.color;
            color.a -= 0.05f;
            blackScreen.color = color;
            i++;
            yield return new WaitForSeconds(0.025f);
        }
    }
    private IEnumerator FadeToBlack()
    {
        Color color = Color.black;
        color.a = 0;
        blackScreen.color = color;
        blackScreen.gameObject.SetActive(true);
        int i = 0;

        while (i < 20)
        {
            color = blackScreen.color;
            color.a += 0.05f;
            blackScreen.color = color;
            i++;
            yield return new WaitForSeconds(0.1f);
        }

        blackScreen.color = Color.black;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
    }
    private void LoseGame()
    {
        sufferedDamage = true;
        EndGame();
    }
    private void CallDialogue()
    {
        if (OnDialogueCall != null) OnDialogueCall();
    }
    private Enemy GetAverageEnemy()
    {
        for (int i = 0; i < averageEnemyCnt; i++)
        {
            if (!enemies[i].gameObject.activeInHierarchy)
            {
                return enemies[i];
            }
        }

        return null;
    }
    private Enemy GetTankEnemy()
    {
        for (int i = averageEnemyCnt; i < averageEnemyCnt + tankEnemyCnt; i++)
        {
            if (!enemies[i].gameObject.activeInHierarchy)
            {
                return enemies[i];
            }
        }
     
        return null;
    }
    private Enemy GetNimbleEnemy()
    {
        for (int i = averageEnemyCnt + tankEnemyCnt; i < enemies.Count; i++)
        {
            if (!enemies[i].gameObject.activeInHierarchy)
            {
                return enemies[i];
            }
        }
     
        return null;
    }
    private Vector3 GetStartingPosition()
    {
        return new Vector3(Random.Range(minXPos, maxXPos), maxYPos, 0);
    }
    public Transform GetClosestEnemy() {return closestEnemy;}
}
