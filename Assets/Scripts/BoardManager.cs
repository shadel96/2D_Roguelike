
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;


    //boardholder will make all objects children of an object to prevent hierarchy clutter
    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitList()
    {
        gridPositions.Clear();

        //starts at 1 and minus 1 tp prevent obstacles against walls
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; x++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        //goes from -1 to +1 to go to outer wall
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; x++)
            {

                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject Instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                Instance.transform.SetParent(boardHolder);
            }
        }

    }

    Vector3 RandomPosition()
    {
        int randInd = Random.Range(0, gridPositions.Count);
        Vector3 randomPos = gridPositions[randInd];
        gridPositions.RemoveAt(randInd);
        return randomPos;
    }

    void PlaceRandomObjs(GameObject[] tileArray, int minimum, int maximum)
    {
        int objCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objCount; i++)
        {
            Vector3 tilePos = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, tilePos, Quaternion.identity);
        }
    }

    public void Setup(int level)
    {
        BoardSetup();
        InitList();
        PlaceRandomObjs(wallTiles, wallCount.minimum, wallCount.maximum);
        PlaceRandomObjs(foodTiles, foodCount.minimum, foodCount.maximum);

        //enemy amount scales with level
        int enemyCount = (int)Mathf.Log(level, 2f);
        PlaceRandomObjs(enemyTiles, enemyCount, enemyCount);

        //exit in same place always (top right)
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
  

}