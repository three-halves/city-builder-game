using UnityEngine;

public class BuildingFarm : Building
{
    [SerializeField] float cashPerSecond;

    public override string BuildingTooltip 
    {
        get
        {
            return "Makes " + cashPerSecond + " cash per second.";  
        }
    }

    public override void Setup()
    {
        Debug.Log(gameObject.name);
        base.Setup();
    }

    public override void Update()
    {
        base.Update();
        GameState.Instance.Cash += cashPerSecond * Time.deltaTime;
    }

    public override void Place(Vector2Int pos)
    {
        base.Place(pos);
    }

    public override void OnInteract()
    {
        Debug.Log("Farm Building Interact");
    }
}