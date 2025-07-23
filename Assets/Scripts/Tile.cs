using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TileView))]
public class Tile : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum Biome 
    {
        Grass,
        Water,
        Rock
    }

    private TileView _view;

    // Biome type of this tile
    public Biome TileBiome {get; private set;}
    public bool IsHovered {get; private set;}
    public bool IsClaimed {get; private set;}
    public int Difficutly {get; private set;} = 1;
    public bool IsInBattle {get; private set;} = false;
    public List<BattleCharacter> foes = new();

    public Vector2Int tilePos;

    public bool IsOccupied {
        get
        {
            return OccupyingBuilding != null;
        }
    }

    public Building OccupyingBuilding = null;

    public void Setup(Biome biome, int difficulty)
    {
        _view = GetComponent<TileView>();
        TileBiome = biome;
        IsHovered = false;
        Difficutly = difficulty;

        // Choose foe from difficulty table
        List<Encounter> encounters = BattleManager.Instance.BattleDatabase.EncounterTable[Mathf.Min(Difficutly - 1, 10)].encounters;
        foes = BattleManager.Instance.IntToFoeList(encounters[Random.Range(0, encounters.Count)].foes);

        _view.Refresh(this);
    }

    public void SetBiome(Biome b)
    {
        TileBiome = b;
        _view.Refresh(this);
    }

    // Start battle sequence 
    public void TryClaim()
    {
        IsClaimed = true;
    }

    public void Claim()
    {
        IsClaimed = true;
        _view.Refresh(this);
    }

    // Pointer Event Handlers

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    /// <summary>
    /// Try to place the current selected building on this tile. If unclaimed, start battle to claim tile
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // interact with built building if possible
        if (OccupyingBuilding != null)
        {
            OccupyingBuilding.OnInteract();
            return;
        }
        
        // Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        // Level.Instance.GetAdjacentBiomes(tilePos.x, tilePos.y).ForEach(i => Debug.Log(i.ToString()));
        // Try build building
        BuildingManager.Instance.TryPlaceBuilding(GameState.Instance.SelectedBuildingIndex, tilePos);

        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.F2))
        {
            Claim();
            return;
        } 
        #endif

        // check if we can claim this tile (adjacent to claimed tile)
        bool adjacent = false;
        foreach (Tile t in Level.Instance.GetTilesInArea(tilePos + new Vector2Int(-1, -1), tilePos + new Vector2Int(2, 2)))
        {
            adjacent |= t.IsClaimed;
        }

        // Start battle to claim tile
        if (!IsClaimed && !BattleManager.Instance.IsBattleOngoing && adjacent)
        {
            StartCoroutine(StartBattle());
        }
    }

    private IEnumerator StartBattle()
    {
        Camera.main.GetComponent<CameraController>().SetTargetPos(transform.position + Vector3.up * 2);
        IsInBattle = true;
        yield return StartCoroutine(BattleManager.Instance.StartBattle(
            Difficutly, 
            TileBiome, transform.position + Vector3.up * 2.5f, 
            BattleCompleteCallback,
            foes
            )
        );
    }

    private void BattleCompleteCallback(bool won)
    {
        if (won) Claim();
        IsInBattle = false;
        _view.Refresh(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Select this tile if there is no building on top of it
        if (!IsOccupied)
        {
            IsHovered = true;
            GameState.Instance.HoverTile(tilePos, this);
        }
        // otherwise select the building
        else
        {
            GameState.Instance.HoverBuilding(OccupyingBuilding.PlacedTilePos, OccupyingBuilding);
        }

        _view.Refresh(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
        _view.Refresh(this);
    }
}