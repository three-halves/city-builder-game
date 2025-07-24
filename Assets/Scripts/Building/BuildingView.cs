using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _overlayTextMesh;
    [SerializeField] private SpriteRenderer _overlayRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Refresh(Building building)
    {
        if (_overlayTextMesh != null)
        {
            // _overlayTextMesh.enabled = _overlayTextMesh.text != "";
            _overlayTextMesh.text = building.OverlayText;
        }
        _spriteRenderer.sprite = GameState.Instance.SpriteData.Buildings[building.BuildingSpriteIndex];
        if (_overlayRenderer != null) _overlayRenderer.enabled = building.ShowOverlayIcon;
    }
}