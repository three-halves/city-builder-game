using UnityEngine;

[RequireComponent(typeof(BuildingView))]
public class Building : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Footprint {get; private set;} = Vector2Int.one ;
    [field: SerializeField] public string BuildingName {get; private set;} = "Default Building Name";
    [field: SerializeField] public string BuildingDescription {get; private set;} = "Default Building Description";
    [field: SerializeField] public virtual string BuildingTooltip {get;} = "Default Building Tooltip";
    [field: SerializeField] public virtual string OverlayText {get;} = ""; 
    [field: SerializeField] public virtual bool ShowOverlayIcon {get;} = false; 
    // Is this building purchasable from the regular menu?
    [field: SerializeField] public bool IsPurchasable {get; private set;} = true;
    // Tiles this building cannot be placed on
    [field: SerializeField] public Tile.Biome[] ExcludeBiomes {get; private set;}
    // Index of sprite in sprite database
    [field: SerializeField] public int BuildingSpriteIndex {get; private set;}

    [field: SerializeField] public int BaseCost {get; private set;} = 100;
    public int Cost {get; private set;}
    // Cost multiplier each purchase
    [field: SerializeField] public float CostScaling {get; private set;} = 1.25f;

    public Vector2Int PlacedTilePos {get; private set;}

    protected BuildingView _view;

    public virtual void Setup()
    {
        Cost = BaseCost;
    }

    public virtual void Update()
    {

    }

    public virtual void Place(Vector2Int pos)
    {
        _view = GetComponent<BuildingView>();
        Vector2 offset = (Footprint - Vector2.one) / 2f * new Vector2(-1, 1);
        transform.position = Level.Instance.TileToWorldPos(pos) - offset;
        PlacedTilePos = pos;

        _view.Refresh(this);
    }

    public void IncreaseCost()
    {
        Cost = (int)Mathf.Ceil(CostScaling * Cost);
    }

    public virtual void OnInteract()
    {
        Debug.Log("Base Building Interact");
    }
}