using UnityEngine;
using UnityEngine.UI;

public class BuildingPalleteSegment : MonoBehaviour
{
    // The building this pallete segment represents
    [field: SerializeField] public GameObject SegmentBuildingObject;
    public Building SegmentBuilding {get; private set;}
    // Index of building in building database
    private int _buildingIndex;
    [SerializeField] private Image _icon;
    [SerializeField] private TMPro.TextMeshProUGUI _nameTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI _costTextMesh;
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
            _costTextMesh.text = "$" + SegmentBuilding.Cost;
        }
        else
        {
            // display quantity of unplaced buildings we own of this type
            _costTextMesh.text = "x " + owned;
        }
        
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
        GameState.Instance.SelectedBuildingIndex = _buildingIndex;
    }
}
