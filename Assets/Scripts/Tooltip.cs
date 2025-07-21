using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleTextMesh;
    [SerializeField] private TextMeshProUGUI _tooltipTextMesh;

    void Start()
    {
        GameState.Instance.SelectedTileListener += OnTileSelected;
    }

    private void OnTileSelected(Vector2Int pos, Tile tile)
    {
        _titleTextMesh.text = tile.IsClaimed ? "Owned Tile" : "Unowned Tile";
        _tooltipTextMesh.text = "Difficulty: " + tile.Difficutly;
    }
}