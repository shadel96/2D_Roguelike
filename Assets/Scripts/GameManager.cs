using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int playerFoodPoints = 100;
    public BoardManager boardScript;
    [HideInInspector] public bool playersTurn = true;
    private int level = 4;

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
        InitGame();
    }

    public void GameOver()
    {
        enabled = false;
    }

    void InitGame()
    {
        boardScript.Setup(level);
    }


}
