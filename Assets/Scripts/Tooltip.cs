using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleTextMesh;
    [SerializeField] private TextMeshProUGUI _tooltipTextMesh;

    void Start()
    {
        GameState.Instance.HoveredTileListener += OnTileSelected;
        GameState.Instance.HoveredBuildingListener += OnBuildingSelected;
    }

    private void OnTileSelected(Vector2Int pos, Tile tile)
    {
        _titleTextMesh.text = tile.IsClaimed ? "Owned Tile" : "Unowned Tile";
        _tooltipTextMesh.text = "Difficulty: " + tile.Difficutly;
    }

    private void OnBuildingSelected(Vector2Int pos, Building bldg)
    {
        _titleTextMesh.text = bldg.BuildingName;
        _tooltipTextMesh.text = bldg.BuildingTooltip;
    }
}