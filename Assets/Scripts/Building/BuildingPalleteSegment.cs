using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPalleteSegment : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // The building this pallete segment represents
    [field: SerializeField] public GameObject SegmentBuildingObject;
    public Building SegmentBuilding {get; private set;}
    // Index of building in building database
    private int _buildingIndex;
    [SerializeField] private Image _icon;
    [SerializeField] private TMPro.TextMeshProUGUI _nameTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI _costTextMesh;

    private Vector2 _dragStartPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int dbIndex)
    {
        _buildingIndex = dbIndex;
        SegmentBuilding = SegmentBuildingObject.GetComponent<Building>();
        Refresh();
    }

    public void Refresh()
    {
        _icon.sprite = GameState.Instance.SpriteData.Buildings[SegmentBuilding.BuildingSpriteIndex];
        _nameTextMesh.text = SegmentBuilding.BuildingName;

        int owned = BuildingManager.Instance.OwnedUnplacedAmounts[_buildingIndex];
        if (owned == 0)
        {
            _costTextMesh.text = SegmentBuilding.Cost + " " + SegmentBuilding.CurrencyType;
        }
        else
        {
            // display quantity of unplaced buildings we own of this type
            _costTextMesh.text = "x " + owned;
        }

        gameObject.SetActive(SegmentBuilding.IsPurchasable);
        
    }

    public void OnPointerEnter()
    {
        _nameTextMesh.text = SegmentBuilding.BuildingDescription;
    }

    public void OnPointerExit()
    {
        _nameTextMesh.text = SegmentBuilding.BuildingName;
    }

    public void OnSubmit()
    {
        Debug.Log(SegmentBuildingObject.name);
        // GameState.Instance.SelectedBuildingIndex = _buildingIndex;
    }
    
    // drag event handling
    public delegate void OnSegmentDrag(PointerEventData eventData, Vector2 startEventPos, int buildingIndex);
    public event OnSegmentDrag SegmentDragListenter;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // try place building
        BuildingManager.Instance.TryPlaceBuilding(
            GameState.Instance.SelectedBuildingIndex, 
            Level.Instance.WorldPosToTile(Camera.main.ScreenToWorldPoint(eventData.position))
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        SegmentDragListenter?.Invoke(eventData, _dragStartPos, _buildingIndex);
    }
}
