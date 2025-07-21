using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _overlayRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Updates tile visuals with the given params
    /// </summary>
    public void Refresh(Tile tile)
    {
        _spriteRenderer.sprite = GameState.Instance.SpriteData.Tiles[(int)tile.TileBiome];
        _spriteRenderer.color = !tile.IsClaimed ? Color.gray : Color.white;
        _overlayRenderer.color = tile.IsInBattle ? Color.white : Color.clear;
    }
}