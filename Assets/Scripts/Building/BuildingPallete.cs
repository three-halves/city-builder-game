using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void OnSegmentDrag(Vector2 delta, int buildingIndex)
    {
        Debug.Log(delta);
        Debug.Log(_scrollRect.verticalNormalizedPosition);
        _scrollRect.verticalNormalizedPosition += -delta.y * 3f / _scrollRect.content.sizeDelta.y;
    }

}