using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPallete : MonoBehaviour
{
    [SerializeField] private Transform _layoutParent;
    [SerializeField] private GameObject _segmentPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    private List<BuildingPalleteSegment> _segments = new();

    void Start()
    {
        // Create building pallete buttons
        for (int i = 0; i < BuildingManager.Instance.buildingDatabase.Buildings.Count; i++)
        {
            GameObject o = BuildingManager.Instance.buildingDatabase.Buildings[i];
            o.GetComponent<Building>().Setup();
            BuildingPalleteSegment bps = Instantiate(_segmentPrefab, _layoutParent).GetComponent<BuildingPalleteSegment>();
            bps.SegmentBuildingObject = o;
            bps.Setup(i);

            bps.SegmentDragListenter += OnSegmentDrag;

            _segments.Add(bps);
        }

        // Subscribe to events
        BuildingManager.Instance.BuildingPlacedListener += OnBuildingPlaced;
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    void OnBuildingPlaced(int buildingIndex, bool wasPlaced)
    {
        foreach(var segment in _segments) segment.Refresh();
    }

    void OnSegmentDrag(PointerEventData eventData, Vector2 dragStartPos, int buildingIndex)
    {
        Debug.Log(Mathf.Abs(eventData.position.x - dragStartPos.x));
        // Drag building to place
        if (Mathf.Abs(eventData.position.x - dragStartPos.x) > 125f)
        {
            GameState.Instance.SelectedBuildingIndex = buildingIndex;
        }
        // Scroll building list
        else
        {
            _scrollRect.verticalNormalizedPosition += -eventData.delta.y * 3f / _scrollRect.content.sizeDelta.y;
        }
        
    }

}