using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Initial Map Values")]
    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeight;
    [SerializeField][Range(0, 1)] private float _wallPercentage;

    [Header("Level Generation")]
    [SerializeField] [Range(0, 10)] private int _simulationSteps;
    [SerializeField] [Range(0, 5)] private int _birthLimit;
    [SerializeField] [Range(0, 5)] private int _starvationLimit;

    [Header("Level Confirmation")]
    [Tooltip("Amount of fill required for the level to be considered \"Worthy\" to use.")]
    [SerializeField] [Range(0, 1)] private float _minimumFillPercentage;

    [Header("Level Creation")]
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _floor;
    [SerializeField] private float _wallOffset, _floorOffset;

    private int[,] Map { get; set; }
    private bool[,] Visited { get; set; }

    private void InitializeMap()
    {
        Map = new int[_mapWidth, _mapHeight];
        Visited = new bool[_mapWidth, _mapHeight];

        int middle = _mapHeight / 2;

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                if (x == 0) Map[x, y] = 1;
                else if (y == 0) Map[x, y] = 1;
                else if (x == _mapWidth - 1) Map[x, y] = 1;
                else if (y == _mapHeight - 1) Map[x, y] = 1;
                else Map[x, y] = y == middle ? 0 : Random.value < _wallPercentage ? 1 : 0;
            }
        }
    }

    private void DoSimulationStep()
    {
        int[,] newMap = new int[_mapWidth, _mapHeight];

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                int neighbourCount = GetAliveNeighbourCount(x, y);

                if (Map[x, y] == 1)
                    newMap[x, y] = neighbourCount < _starvationLimit ? 0 : 1;

                else if (Map[x, y] == 0)
                    newMap[x, y] = neighbourCount > _birthLimit ? 1 : 0;
            }
        }

        Map = newMap;
    }

    private int GetAliveNeighbourCount(int x, int y)
    {
        int count = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbourX = x + i;
                int neighbourY = y + j;

                if (!(i == 0 && j == 0))
                {
                    if (IsOutOfBounds(neighbourX, neighbourY))
                        count++;

                    else if (Map[neighbourX, neighbourY] == 1)
                        count++;
                }
            }
        }

        return count;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _mapWidth || y >= _mapHeight)
            return true;

        return false;
    }

    private void FloodFill(int x, int y)
    {
        if (IsOutOfBounds(x, y))
            return;

        if (Visited[x, y])
            return;

        Visited[x, y] = true;
        
        FloodFill(x - 1, y);
        FloodFill(x, y - 1);
        FloodFill(x, y + 1);
        FloodFill(x + 1, y);
    }

    private void FixLevelGaps()
    {
        for (int xi = 0; xi < _mapWidth; xi++)
            for (int yi = 0; yi < _mapHeight; yi++)
                Visited[xi, yi] = Map[xi, yi] == 1 ? true : false;

        int tileType = 1;
        Vector2Int tilePosition = Vector2Int.zero;
        while (tileType != 0)
        {
            tilePosition = new Vector2Int(Random.Range(0, _mapWidth), Random.Range(0, _mapHeight));
            tileType = Map[tilePosition.x, tilePosition.y];
        }

        FloodFill(tilePosition.x, tilePosition.y);

        for (int x = 0; x < _mapWidth; x++)
            for (int y = 0; y < _mapHeight; y++)
                if (!Visited[x, y]) Map[x, y] = 1;
    }

    private float FindLevelSize()
    {
        int totalSize = _mapWidth * _mapHeight;
        int currentSize = 0;

        for (int x = 0; x < _mapWidth; x++)
            for (int y = 0; y < _mapHeight; y++)
                currentSize += Map[x, y] == 0 ? 1 : 0;

        return (float)currentSize / totalSize;
    }

    private void GenerateMap()
    {
        InitializeMap();

        for (int i = 0; i < _simulationSteps; i++)
            DoSimulationStep();

        FixLevelGaps();

        Debug.Log(FindLevelSize());

        if (FindLevelSize() < _minimumFillPercentage)
        {
            GenerateMap();
            return;
        }

        ClearLevel();

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                GameObject go = Instantiate(Map[x, y] == 1 ? _wall : _floor);
                go.transform.position = new Vector3(
                    x * go.transform.localScale.x, 
                    Map[x, y] == 1 ? _wallOffset : _floorOffset, 
                    y * go.transform.localScale.z);
                go.transform.parent = transform;
            }
        }
    }

    private void ClearLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    #region Unity Methods

    private void Start ()
    {
        GenerateMap();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateMap();
        }
    }
    
    #endregion
}
