using System.Collections.Generic;
using UnityEngine;

public class BuildingPallete : MonoBehaviour
{
    [SerializeField] private Transform _layoutParent;
    [SerializeField] private GameObject _segmentPrefab;

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
            _segments.Add(bps);
        }

        // Subscribe to events
        BuildingManager.Instance.BuildingPlacedListener += OnBuildingPlaced;
    }

    void OnBuildingPlaced(int buildingIndex, bool wasPlaced)
    {
        foreach(var segment in _segments) segment.Refresh();

    }
}