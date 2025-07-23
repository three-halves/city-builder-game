using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance {get; private set;} 

    [SerializeField] private int _levelWidth;
    [SerializeField] private int _levelHeight;

    [SerializeField] private int _startingClaimedWidth;
    [SerializeField] private int _startingClaimedHeight;

    // assuming all tiles are square
    [field: SerializeField] public float TileScale {get; private set;}

    [SerializeField] private GameObject baseTilePrefab;

    private Tile[,] _tiles;

    // Level gen settings
    [SerializeField] private float waterAltThreshold = 0.25f;
    [SerializeField] private float rockAltThreshold = 0.75f;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _tiles = new Tile[_levelHeight, _levelWidth];
        GenerateLevel();

        // center cam
        Camera.main.GetComponent<CameraController>().SetTargetPos(TileToWorldPos(new Vector2(_levelWidth / 2, _levelHeight / 2)));
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GenerateLevel();
            Debug.Log("GenerateLevel");
        }
        #endif
    }

    void GenerateLevel()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        // Initial pass, set up positions and biome
        for (int i = 0; i < _levelHeight; i++)
        {
            for (int j = 0; j < _levelWidth; j++)
            {
                GameObject newTileObject = Instantiate(baseTilePrefab, transform);
                newTileObject.name = "Tile" + j + ", " + i;

                var newTile = newTileObject.GetComponent<Tile>();
                // get euclidian distance from center to assign difficulty
                float dist = Mathf.Sqrt((i - _levelHeight / 2) * (i - _levelHeight / 2) + (j - _levelWidth / 2) * (j - _levelWidth / 2));
                int difficulty = Mathf.Max((int)Mathf.Ceil(dist) - _startingClaimedWidth / 2 + Random.Range(-1, 2), 1);
                Tile.Biome biome = CalculateBiome(i, j, dist);            
                newTile.Setup(biome, difficulty);
                newTile.transform.position = new Vector3(j * TileScale, -i * TileScale, 0);
                newTile.tilePos = new Vector2Int(j, i);

                int outery = _levelHeight / 2 - _startingClaimedHeight / 2;
                int outerx = _levelWidth / 2 - _startingClaimedWidth / 2;
                // check if this tile should be initially claimed
                Vector2Int offset = new (outerx, outery);
                if (i < _startingClaimedHeight + offset.y && i >= offset.y && j < _startingClaimedWidth + offset.x && j >= offset.x)
                    newTile.Claim();

                _tiles[i, j] = newTile;
            }
        }

        // Second pass, finalize tile biomes
        for (int i = 0; i < _levelHeight; i++)
        {
            for (int j = 0; j < _levelWidth; j++)
            {
                if (_tiles[j,i].TileBiome == Tile.Biome.Water && !GetAdjacentBiomes(i, j).Contains(Tile.Biome.Water))
                    _tiles[j,i].SetBiome(Tile.Biome.Grass);
            }
        }
    }

    private Tile.Biome CalculateBiome(int i, int j, float dist)
    {
        float py = i / (float)_levelHeight * 10;
        float px = j / (float)_levelWidth * 10;
        float altitude = Mathf.PerlinNoise(px, py);
        Tile.Biome b = Tile.Biome.Grass;
        // bounds and starting area check
        if (dist < _startingClaimedWidth || i < 0 || j < 0 || i > _levelHeight || j > _levelWidth) 
            return b;

        if (altitude < waterAltThreshold) 
            b = Tile.Biome.Water;
        if (altitude > rockAltThreshold) 
            b = Tile.Biome.Rock;

        return b;
    }

    // returns a list of the biomes of the 4-way adjacent tiles of the position given
    public List<Tile.Biome> GetAdjacentBiomes(int x, int y)
    {
        return new()
        {
            (y + 1 >= _levelHeight) ? Tile.Biome.Grass : _tiles[y + 1, x].TileBiome,
            (y - 1 < 0) ? Tile.Biome.Grass : _tiles[y - 1, x].TileBiome,
            (x + 1 >= _levelWidth) ? Tile.Biome.Grass : _tiles[y, x + 1].TileBiome,
            (x - 1 < 0) ? Tile.Biome.Grass : _tiles[y, x - 1].TileBiome
        };
    }

    public Vector2 TileToWorldPos(Vector2 tile)
    {
        return tile * TileScale * new Vector2Int(1, -1);
    }

    public List<Tile> GetTilesInArea(Vector2Int p1, Vector2Int p2)
    {
        List<Tile> tiles = new();
        for (int i = p1.x; i < p2.x; i++)
        {
            for (int j = p1.y; j < p2.y; j++)
            {
                tiles.Add(_tiles[j, i]);
            }
        }
        return tiles;
    }
}