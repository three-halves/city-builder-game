using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Refresh(Building building)
    {
        _spriteRenderer.sprite = GameState.Instance.SpriteData.Buildings[building.BuildingSpriteIndex];
    }
}