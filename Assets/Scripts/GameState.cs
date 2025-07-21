using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance {get; private set;}

    [SerializeField] public SpriteDatabase SpriteData;

    // building prefab selected by player from building palette
    [SerializeField] private int _selectedBuildingIndex;
    public int SelectedBuildingIndex 
    { 
        get { return _selectedBuildingIndex; }
        set 
        { 
            _selectedBuildingIndex = value;
            SelectedBuildingListener?.Invoke(SelectedBuildingObject?.GetComponent<Building>()); 
        }
    }

    public GameObject SelectedBuildingObject 
    {
        get
        {
            if (_selectedBuildingIndex < 0) return null;
            else return BuildingManager.Instance.buildingDatabase.Buildings[_selectedBuildingIndex];
        }
    }

    // primary currency
    private float _cash = 0;
    public float Cash
    {
        get { return _cash; }
        set { _cash = value; }
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public int GetEffectiveCash()
    {
        return (int)_cash;
    }

    public delegate void SelectedTileDelegate(Vector2Int tilePos, Tile tile);
    public event SelectedTileDelegate SelectedTileListener;

    public delegate void SelectedBuildingDelegate(Building building);
    public event SelectedBuildingDelegate SelectedBuildingListener;

    // called from tile object
    public void SelectTile(Vector2Int tilePos, Tile tile)
    {
        SelectedTileListener?.Invoke(tilePos, tile);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
