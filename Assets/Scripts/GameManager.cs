using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float levelStartDelay = 2f;
    public int playerFoodPoints = 100;
    public BoardManager boardScript;
    [HideInInspector] public bool playersTurn = true;
    private int level = 1;

    public float turnDelay = 0.2f;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;


    void Awake()
    {
       

        //hot singletons in your area am i right ladies
        if(instance == null)
        {
            instance = this;
        }
        else if( instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();

        enemies = new List<Enemy>();

        InitGame();

    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved";
        levelImage.SetActive(true);
        enabled = false;
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("day").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.Setup(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());


    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }


}
