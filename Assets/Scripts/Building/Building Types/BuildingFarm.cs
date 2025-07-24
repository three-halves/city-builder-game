using System;
using UnityEngine;

public class BuildingFarm : Building
{
    // Randomized amount of time to produce 1 currency type
    [SerializeField] private Vector2 currencyInterval = Vector2.one;
    private int _storedCurrency;
    [SerializeField] private int _maxStorage;
    [SerializeField] private GameState.CurrencyType _currencyType;
    [SerializeField] private float _timer = 0f;
    public override string OverlayText 
    {
        get { return (_storedCurrency < 1) ? "" : "" + _storedCurrency;}
    }

    public override bool ShowOverlayIcon
    {
        get {return _maxStorage == 1 && _storedCurrency == 1;}
    }

    public override string BuildingTooltip 
    {
        get
        {
            if (currencyInterval.x <= 1f)
                return string.Format("Makes {0} {1} per second.", Math.Round(1f / currencyInterval.x, 2), _currencyType); 
            else
                return string.Format("Makes 1 {0} every {1}-{2} seconds.", _currencyType, currencyInterval.x, currencyInterval.y); 
        }
    }

    public override void Setup()
    {
        base.Setup();
        _storedCurrency = 0;
    }

    public override void Update()
    {
        base.Update();
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _storedCurrency = Mathf.Min(_storedCurrency +  1, _maxStorage);
            _view.Refresh(this);
            ResetTimer();
        }
    }

    public override void Place(Vector2Int pos)
    {
        base.Place(pos);
    }

    public override void OnInteract()
    {
        if (_storedCurrency == 0) return;
        GameState.Instance.CurrencyDict[_currencyType] += _storedCurrency;
        _storedCurrency = 0;
        _view.Refresh(this);
    }

    void ResetTimer()
    {
        _timer = UnityEngine.Random.Range(currencyInterval.x, currencyInterval.y);
    }
}