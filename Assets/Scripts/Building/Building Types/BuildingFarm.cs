using UnityEngine;

public class BuildingFarm : Building
{
    [SerializeField] private float _cashPerSecond;
    private float _storedCash;
    [SerializeField] private int _maxStorage;

    public override string OverlayText 
    {
        get { return ((int)_storedCash == 0) ? "" : "" + (int)_storedCash;}
    }

    public override string BuildingTooltip 
    {
        get
        {
            return "Makes " + _cashPerSecond + " cash per second.\nMax Capacity: " + _maxStorage +".";  
        }
    }

    public override void Setup()
    {
        Debug.Log(gameObject.name);
        base.Setup();
        _storedCash = 0f;
    }

    public override void Update()
    {
        base.Update();
        _storedCash = Mathf.Min(_storedCash + _cashPerSecond * Time.deltaTime, _maxStorage);
        if ((int)_storedCash != (int)(_storedCash - _cashPerSecond * Time.deltaTime))
        {
            _view.Refresh(this);
        }
        // GameState.Instance.Cash += cashPerSecond * Time.deltaTime;
    }

    public override void Place(Vector2Int pos)
    {
        base.Place(pos);
    }

    public override void OnInteract()
    {
        GameState.Instance.Cash += _storedCash;
        _storedCash = 0;
        _view.Refresh(this);
    }
}