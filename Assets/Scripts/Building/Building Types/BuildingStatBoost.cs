using Battle;
using UnityEngine;

public class BuildingStatBoost : Building
{
    // Index of hero in battle manager
    [SerializeField] private int _heroIndex;
    [SerializeField] private StatType _statType;
    [SerializeField] private int _boostAmount;

    public override string BuildingTooltip 
    {
        get
        {
            return string.Format(
                "Increases {0}'s {1} by {2}.", 
                BattleManager.Instance.PlayerCharacters[_heroIndex].CharacterName,
                _statType,
                _boostAmount
                );  
        }
    }

    public override void Setup()
    {
        base.Setup();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Place(Vector2Int pos)
    {
        base.Place(pos);
        BattleManager.Instance.PlayerCharacters[_heroIndex].Stats.ChangeStat(_statType, _boostAmount);
    }

    public override void OnInteract()
    {
        Debug.Log("Farm Building Interact");
    }
}