using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [Tooltip("Amount of fill required for the level to be considered \"Worthy\" to use.")]
    [SerializeField] [Range(0, 0.75f)] private float _minimumFillPercentage;

    [Header("Level Generation - Entities")]
    [SerializeField] private float _minLengthSpawnEnd;
    [SerializeField] private float _minLengthEnemies;
    [SerializeField] [Range(0, 10)] private int _amountOfEnemies;
    [SerializeField] [Range(10, 50)] private int _amountOfCoins;
    [SerializeField] [Range(0, 10)] private int _amountOfBatteries; 
    
    [Header("Level Creation - Prefabs")]
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _floor;

    [Header("Level Creation - Y Offsets")]
    [SerializeField] private float _wallOffset;
    [SerializeField] private float _floorOffset;

    [Header("Entity Spawning - Prefabs")]
    [SerializeField] private GameObject _playerSpawner;
    [SerializeField] private GameObject _enemySpawner;
    [SerializeField] private GameObject _endOfLevel;
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _battery;

    [Header("Entity Spawning - Y Offsets")]
    [SerializeField] private float _playerOffset;
    [SerializeField] private float _enemySpawnerOffset;
    [SerializeField] private float _endOfLevelOffset;
    [SerializeField] private float _coinOffset;
    [SerializeField] private float _batteryOffset;

    private List<Vector2Int> _usedPickUpLocations = new List<Vector2Int>();
    private const bool DEBUG_MODE = true;
       
    //also use player spawn min/max for the end of the level
    private const int MIN_NEIGHBOURS_PLAYER_SPAWN = 3;
    private const int MAX_NEIGHBOURS_PLAYER_SPAWN = 8;
    private const int MIN_NEIGHBOURS_ENEMY_SPAWN = 0;
    private const int MAX_NEIGHBOURS_ENEMY_SPAWN = 8;
    private const int MIN_NEIGHBOURS_PICKUP_SPAWN = 4;
    private const int MAX_NEIGHBOURS_PICKUP_SPAWN = 6;

    private GameObject EntitiesParent { get; set; }
    private int[,] Map { get; set; }
    private bool[,] Visited { get; set; }
    private bool NavMeshDirty { get; set; }

    /// <summary>
    /// Initializes the base of the map. Meaning a completely random map, with
    /// walls at the bounds, to make sure there won't be an opening. As well as
    /// an empty horizontal line in the middle of the map, to avoid big 
    /// vertical areas.
    /// </summary>
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

    /// <summary>
    /// Change the map based on the amount of neighbours each node has. 
    /// </summary>
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
    /// Find a random walkable position in the map and from that point start 
    /// the flood filling algorithm to make sure there will be no random areas
    /// that you are unable to walk to. This is to avoid spawns outside of the
    /// playable area.
    /// </summary>
    private void FixLevelGaps()
    {
        //Initialize the visited 2d array, to make sure the walls are already
        //visited.
        for (int xi = 0; xi < _mapWidth; xi++)
            for (int yi = 0; yi < _mapHeight; yi++)
                Visited[xi, yi] = Map[xi, yi] == 1 ? true : false;

        int tileType = 1;
        Vector2Int tilePosition = Vector2Int.zero;

        //While the tile type is not 0, which means an empty tile, keep 
        //searching for a random starting position to start the flood filling
        //algorithm.
        while (tileType != 0)
        {
            tilePosition = new Vector2Int(
                Random.Range(0, _mapWidth), 
                Random.Range(0, _mapHeight));
            tileType = Map[tilePosition.x, tilePosition.y];
        }

        //Call the flood fill algorithm from a random position.
        FloodFill(tilePosition.x, tilePosition.y);

        for (int x = 0; x < _mapWidth; x++)
            for (int y = 0; y < _mapHeight; y++)
                if (!Visited[x, y]) Map[x, y] = 1;
    }

    /// <summary>
    /// Used to find the size of the level, which is calculated by dividing the 
    /// size of all the walkable tiles by the amount of total tiles in the map.
    /// </summary>
    /// <returns> A percentage which represents the size of the level relative 
    /// to the full map </returns>
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

        //Clear the level in case there is already one generated.
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

        float lengthSpawnToEnd = 0;
        GameObject playerSpawner = Instantiate(_playerSpawner);
        GameObject endLevel = Instantiate(_endOfLevel);

        while (lengthSpawnToEnd < _minLengthSpawnEnd)
        {
            InitializeSpawnAndEndLocation(ref playerSpawner, ref endLevel);
            lengthSpawnToEnd = Vector3.Distance(
                playerSpawner.transform.position, 
                endLevel.transform.position);
        }

        for (int i = 0; i < _amountOfEnemies; i++)
        {
            SpawnEnemy(ref playerSpawner, 9999);
        }

        AddPickupSpawns(_coin, _amountOfCoins, 9999);
        AddPickupSpawns(_battery, _amountOfBatteries, 9999);

        NavMeshDirty = true;
    }

    //TODO: Actually fucking comment this. :)
    private void SpawnEnemy(ref GameObject playerSpawner, int maxTries)
    {
        Vector2Int playerLocation = new Vector2Int(
            (int) (playerSpawner.transform.position.x / playerSpawner.transform.localScale.x),
            (int) (playerSpawner.transform.position.z / playerSpawner.transform.localScale.z));
        Vector2Int spawnLocation = Vector2Int.zero;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemySpawner");
        
        int tries = 0;
        while (tries < maxTries)
        {
            spawnLocation = FindRandomEntityLocation(MIN_NEIGHBOURS_ENEMY_SPAWN, MAX_NEIGHBOURS_ENEMY_SPAWN);

            if (Vector2Int.Distance(playerLocation, spawnLocation) > _minLengthEnemies)
            {
                int tempTries = tries;
                tries = maxTries;

                if (enemies.Length != 0)
                {
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        Vector2Int enemyLocation = new Vector2Int(
                            (int)(enemies[i].transform.position.x / enemies[i].transform.localScale.x),
                            (int)(enemies[i].transform.position.z / enemies[i].transform.localScale.z));

                        if (Vector2Int.Distance(enemyLocation, spawnLocation) < _minLengthEnemies)
                        {
                            tries = tempTries;
                        }
                    }
                }
            }

            tries++;
        }

        //Even if we failed to get a good distance between other enemies, 
        //at least make sure the distance from the player is bigger than the
        //minimum to avoid getting attacked while spawning in.
        if (Vector2Int.Distance(playerLocation, spawnLocation) > _minLengthEnemies)
        {
            GameObject enemy = Instantiate(_enemySpawner);
            enemy.transform.position = new Vector3(
                spawnLocation.x * enemy.transform.localScale.x,
                _enemySpawnerOffset,
                spawnLocation.y * enemy.transform.localScale.z);
            enemy.transform.parent = EntitiesParent.transform;
        }
    }

    /// <summary>
    /// Initializes the spawn location and end of level location and passes it 
    /// to the referenced GameObjects.
    /// </summary>
    /// <param name="playerSpawner">A reference to the player spawner object</param>
    /// <param name="endLevel">A reference to the end of the level object</param>
    private void InitializeSpawnAndEndLocation(ref GameObject playerSpawner, ref GameObject endLevel)
    {
        //Player Spawner 
        Vector2Int spawnLocation = FindRandomEntityLocation(MIN_NEIGHBOURS_PLAYER_SPAWN, MAX_NEIGHBOURS_PLAYER_SPAWN);
        playerSpawner.transform.position = new Vector3(
            spawnLocation.x * playerSpawner.transform.localScale.x,
            _playerOffset,
            spawnLocation.y * playerSpawner.transform.localScale.z);
        playerSpawner.transform.parent = EntitiesParent.transform;

        //End of Level
        spawnLocation = FindRandomEntityLocation(MIN_NEIGHBOURS_PLAYER_SPAWN, MAX_NEIGHBOURS_PLAYER_SPAWN);
        endLevel.transform.position = new Vector3(
            spawnLocation.x * endLevel.transform.localScale.x,
            _endOfLevelOffset,
            spawnLocation.y * endLevel.transform.localScale.z);
        endLevel.transform.parent = EntitiesParent.transform;
    }

    /// <summary>
    /// Spawns pickup location using the MIN_NEIGHBOURS_PICKUP_SPAWN and MAX_NEIGHBOURS_PICKUP_SPAWN as references to where they have to spawn
    /// </summary>
    /// <param name="go">The Pickup to spawn</param>
    /// <param name="amount">The amount of pickups of this type that have to spawn</param>
    /// <param name="maxTries">The maximum amount of tries to find a good spawning location</param>
    private void AddPickupSpawns(GameObject go, int amount, int maxTries)
    {
        Vector2Int location = Vector2Int.zero;
        bool locationFound;
        int tries = 0;

        for (int i = 0; i < amount; i++)
        {
            locationFound = false;
            while (!locationFound && tries < maxTries) 
            {
                location = FindRandomEntityLocation(MIN_NEIGHBOURS_PICKUP_SPAWN, MAX_NEIGHBOURS_PICKUP_SPAWN);
                if (_usedPickUpLocations.Count == 0)
                {
                    locationFound = true;
                    _usedPickUpLocations.Add(location);
                }

                for (int j = 0; j < _usedPickUpLocations.Count; j++)
                {
                    if (location != _usedPickUpLocations[j])
                    {
                        locationFound = true;
                        _usedPickUpLocations.Add(location);
                        break;
                    }
                }

                tries++;
            }

            if (locationFound)
            {
                Transform t = Instantiate(go).transform;
                t.position = new Vector3(
                    location.x * _coin.transform.localScale.x,
                    _coinOffset,
                    location.y * _coin.transform.localScale.z);
                t.parent = EntitiesParent.transform;
            }
            else { Debug.Log("No Location found"); }
        }
    }

    /// <summary>
    /// Finds a random entity location based on the minimum and maximum amount 
    /// of neighbours passed to this method. Neighbours represent the walls of 
    /// the map.
    /// </summary>
    /// <param name="minNeighbours">The minimum amount of neighbours (inclusive)</param>
    /// <param name="maxNeighbours">The maximum amount of neighbours (inclusive)</param>
    /// <returns></returns>
    private Vector2Int FindRandomEntityLocation(int minNeighbours = 0, int maxNeighbours = 8)
    {
        Vector2Int location = Vector2Int.zero;
        bool foundLocation = false;

        while (!foundLocation)
        {
            location = new Vector2Int(Random.Range(0, _mapWidth), Random.Range(0, _mapHeight));
            if (Map[location.x, location.y] == 0)
                if (GetAliveNeighbourCount(location.x, location.y) >= minNeighbours &&
                    GetAliveNeighbourCount(location.x, location.y) <= maxNeighbours)
                    foundLocation = true;
        }

        return location;
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

        for (int i = 0; i < EntitiesParent.transform.childCount; i++)
        {
            Destroy(EntitiesParent.transform.GetChild(i).gameObject);
        }
    }
    
    /// <summary>
    /// Searches in all 8 tiles around the tile at the given position for how 
    /// many walls there are surrounding the current one.
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
    /// Checks whether the given position is outside of the bounds of the play 
    /// area.
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
    /// Using this flood filling algorithm we are checking whether every floor 
    /// tile from the given starting point is reachable.
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

#region Unity Methods

    private void Start ()
    {
        EntitiesParent = new GameObject("_Entities");
        NavMeshDirty = true;
        GenerateLevel();
	}

    private void Update()
    {
        if (NavMeshDirty)
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
            NavMeshDirty = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            GenerateLevel();
    }
    
#endregion
}
