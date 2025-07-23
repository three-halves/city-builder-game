using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    [SerializeField] public BuildingDatabase buildingDatabase;
    // based on index in building database
    [field: SerializeField] public List<int> OwnedUnplacedAmounts {get; private set;}
    [field: SerializeField] public List<int> TotalPurchacedAmounts {get; private set;}

    public delegate void BuildingPlacedDelegate(int buildingIndex, bool wasPlaced);
    public event BuildingPlacedDelegate BuildingPlacedListener;

    // index correlates to database index
    [field: SerializeField] public List<int> TotalBuiltCount {get; private set;}

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public List<GameObject> GetPurchaseableObjects()
    {
        List<GameObject> objects = new();
        foreach(var obj in buildingDatabase.Buildings)
        {
            if (obj.GetComponent<Building>().IsPurchasable) objects.Add(obj);
        }
        return objects;
    }

    /// <summary>
    /// Attempt to place selected building at this tile. Validates cost, valid tile placement, etc
    /// </summary>
    /// <param name="buildingIndex"></param>
    /// <param name="tilePos"></param>
    public void TryPlaceBuilding(int buildingIndex, Vector2Int tilePos)
    {
        if (buildingIndex < 0) return;

        var buildingPrefab = buildingDatabase.Buildings[buildingIndex]; 
        var unplacedBuilding = buildingPrefab.GetComponent<Building>();

        // Check if building conditions are met
        // First, check if all tiles within building footprint are free.
        List<Tile> tilesInFootprint = Level.Instance.GetTilesInArea(
            tilePos, 
            new Vector2Int(unplacedBuilding.Footprint.x, unplacedBuilding.Footprint.y) + tilePos
        );

        bool canBuild = CanBuild(tilesInFootprint, buildingIndex, unplacedBuilding);
        // build success
        if (canBuild)
        {
            var bldg = Instantiate(buildingPrefab, transform).GetComponent<Building>();
            bldg.Place(tilePos);

            TotalBuiltCount[buildingIndex]++;
            // Only spend cash on building if we don't own any
            if (OwnedUnplacedAmounts[buildingIndex] > 0)
            {
                OwnedUnplacedAmounts[buildingIndex]--;

                // deselct building if we don't have any more owned uplaced ones
                if (OwnedUnplacedAmounts[buildingIndex] <= 0) 
                    GameState.Instance.SelectedBuildingIndex = -1;
            }
            else
            {
                GameState.Instance.Cash -= unplacedBuilding.Cost;
                unplacedBuilding.IncreaseCost();
            }
            
            foreach(Tile tile in tilesInFootprint)
            {
                tile.OccupyingBuilding = bldg;
                GameState.Instance.HoverBuilding(tilePos, bldg);
            }
            
            // deselect building only if we cant place another
            // if (!CanBuild(new(), buildingIndex, unplacedBuilding)) GameState.Instance.SelectedBuildingIndex = -1;
        }
        // build fail
        else 
        {
            Debug.Log("Build Fail");
        }

        BuildingPlacedListener.Invoke(buildingIndex, canBuild);
    }

    public bool CanBuild(List<Tile> tilesInFootprint, int buildingIndex, Building unplacedBuilding)
    {
        bool allTilesAreFree = true;
        foreach (Tile tile in tilesInFootprint)
        {
            allTilesAreFree &= !tile.IsOccupied && tile.IsClaimed && !unplacedBuilding.ExcludeBiomes.Contains(tile.TileBiome);
        }

        bool canAfford = (GameState.Instance.Cash >= unplacedBuilding.Cost)
            || OwnedUnplacedAmounts[buildingIndex] > 0;

        return allTilesAreFree && canAfford;
    }

    public Building GetBuilding(int i)
    {
        return buildingDatabase.Buildings[i].GetComponent<Building>();
    }
}