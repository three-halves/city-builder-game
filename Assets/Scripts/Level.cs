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

    void GenerateLevel()
    {
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
                newTile.Setup(Tile.Biome.Grass, difficulty);
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