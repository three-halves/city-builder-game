using UnityEngine;

public class MatchTextToCash : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _textMesh;

    void Update()
    {
        string s = "";
        foreach (var kvp in GameState.Instance.CurrencyDict)
        {
            if ((int)kvp.Value > 0 || kvp.Key == GameState.CurrencyType.Cash) 
                s += kvp.Key + ": " + (int)kvp.Value + "\n";
        }
        _textMesh.text = s;
    }
}
