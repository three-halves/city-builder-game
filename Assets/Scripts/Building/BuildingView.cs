using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _overlayTextMesh;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Refresh(Building building)
    {
        _overlayTextMesh.enabled = _overlayTextMesh.text != "";
        _overlayTextMesh.text = building.OverlayText;
        _spriteRenderer.sprite = GameState.Instance.SpriteData.Buildings[building.BuildingSpriteIndex];
    }
}