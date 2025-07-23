using UnityEngine;

public class FootprintVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _overlayRenderer;
    private Building _selectedBuilding;
    private Vector2Int _selectionSize;
    private Vector2Int _tilePos;
    private bool _showOverlay;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameState.Instance.HoveredTileListener += OnHoveredTile;
        GameState.Instance.HoveredBuildingListener += OnHoveredBuilding;
        GameState.Instance.SelectedBuildingListener += OnSelectedBuilding;
        _selectedBuilding = GameState.Instance.SelectedBuildingObject?.GetComponent<Building>();
    }

    void OnHoveredTile(Vector2Int pos, Tile tile)
    {
        _showOverlay = true;
        _tilePos = pos;
        _selectionSize = Vector2Int.one;
        CalculateSizeAndPosition();
    }

    void OnHoveredBuilding(Vector2Int pos, Building building)
    {
        _showOverlay = false;
        _tilePos = pos;
        _selectionSize = building.Footprint;
        CalculateSizeAndPosition();
    }

    void OnSelectedBuilding(Building building)
    {
        _selectedBuilding = building;
        CalculateSizeAndPosition();
    }

    void CalculateSizeAndPosition()
    {
        Vector2 size;
        if (_selectedBuilding == null || !_showOverlay)
        {
            size = _selectionSize;
            _overlayRenderer.sprite = null;
        }
        else
        {
            size = (Vector2)_selectedBuilding.Footprint * Level.Instance.TileScale;
            _overlayRenderer.sprite = GameState.Instance.SpriteData.Buildings[_selectedBuilding.BuildingSpriteIndex];
        }
        Vector2 offset = (size - Vector2.one) / 2f * new Vector2(-1, 1);  
        transform.position = (Vector2)_tilePos * Level.Instance.TileScale * new Vector2(1, -1) - offset;
        // transform.position = (Vector2)_tilePos * Level.Instance.TileScale * new Vector2(1, -1);
        _spriteRenderer.size = size;
        _overlayRenderer.size = size;
    }
}