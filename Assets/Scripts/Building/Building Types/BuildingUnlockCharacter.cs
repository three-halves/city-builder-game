using System;
using Battle;
using UnityEngine;

public class BuildingUnlockCharacter : Building
{
    [SerializeField] private BattleCharacter character;
    // Makes the following buildings purchaseable on placed. based on database index
    [SerializeField] private int[] buildingsToUnlock;

    public override string OverlayText 
    {
        get { return "";}
    }

    public override bool ShowOverlayIcon
    {
        get {return false;}
    }

    public override string BuildingTooltip 
    {
        get
        {
            return "Adds " + character.CharacterName + " to your party";
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
        BattleManager.Instance.AddPartyMember(character);
        foreach (int i in buildingsToUnlock)
        {
            BuildingManager.Instance.buildingDatabase.Buildings[i].GetComponent<Building>().IsPurchasable = true;
        }
    }

    public override void OnInteract()
    {

    }

}