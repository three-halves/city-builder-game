using System.Collections.Generic;
using Battle;
using UnityEngine;

public class HeroPallete : MonoBehaviour
{
    [SerializeField] private Transform _layoutParent;
    [SerializeField] private GameObject _segmentPrefab;

    private List<HeroPalleteSegment> _segments = new();

    void Start()
    {
        // Create building pallete buttons
        for (int i = 0; i < BattleManager.Instance.PlayerCharacters.Count; i++)
        {
            HeroPalleteSegment hps = Instantiate(_segmentPrefab, _layoutParent).GetComponent<HeroPalleteSegment>();
            hps.Setup(i);
            _segments.Add(hps);
        }

        BattleManager.Instance.PartyMemberAddedListener += AddCharacter;
    }

    void Update()
    {
        foreach(var segment in _segments) segment.Refresh();
    }

    private void AddCharacter()
    {
        HeroPalleteSegment hps = Instantiate(_segmentPrefab, _layoutParent).GetComponent<HeroPalleteSegment>();
        hps.Setup(BattleManager.Instance.PlayerCharacters.Count - 1);
        _segments.Add(hps);
    }
}