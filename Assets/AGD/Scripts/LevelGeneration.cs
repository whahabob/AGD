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

    [Header("Entity Spawning")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _playerOffset;

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

    /// <summary>
    /// Find a random walkable position in the map and from that point start the flood filling algorithm to make sure
    /// 
    /// </summary>
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

    /// <summary>
    /// Used to find the size of the level, which is calculated by dividing the size of all the walkable tiles by the 
    /// amount of total tiles in the map.
    /// </summary>
    /// <returns> A percentage which represents the size of the level relative to the full map </returns>
    private float FindLevelSize()
    {
        int totalSize = _mapWidth * _mapHeight;
        int currentSize = 0;

        for (int x = 0; x < _mapWidth; x++)
            for (int y = 0; y < _mapHeight; y++)
                currentSize += Map[x, y] == 0 ? 1 : 0;

        return (float)currentSize / totalSize;
    }

    /// <summary>
    /// Combine all Level Generation methods to create an actual playable level.
    /// </summary>
    private void GenerateLevel()
    {
        InitializeMap();

        for (int i = 0; i < _simulationSteps; i++)
            DoSimulationStep();

        FixLevelGaps();

        if (FindLevelSize() < _minimumFillPercentage)
        {
            GenerateLevel();
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

    /// <summary>
    /// Clear all GameObjects that are currently used for the level.
    /// </summary>
    private void ClearLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    #region UtilityMethods

    /// <summary>
    /// Searches in all 8 tiles around the tile at the given position for how many walls there are surrounding the current one.
    /// </summary>
    /// <param name="x"> The x-position to check </param>
    /// <param name="y"> The y-position to check </param>
    /// <returns> The amount of neighbouring walls </returns>
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

    /// <summary>
    /// Checks whether the given position is outside of the bounds of the play area.
    /// </summary>
    /// <param name="x"> The x-position to check </param>
    /// <param name="y"> The y-position to check </param>
    /// <returns> Whether the given position is inside the play area. </returns>
    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _mapWidth || y >= _mapHeight)
            return true;

        return false;
    }

    /// <summary>
    /// Using this flood filling algorithm we are checking whether every floor tile from the given starting point is reachable.
    /// </summary>
    /// <param name="x"> The starting x-position of the algorithm </param>
    /// <param name="y"> The starting y-position of the algorithm </param>
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

    #endregion


    #region Unity Methods

    private void Start ()
    {
        GenerateLevel();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateLevel();
        }
    }
    
    #endregion
}
