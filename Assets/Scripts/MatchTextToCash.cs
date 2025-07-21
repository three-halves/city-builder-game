using UnityEngine;

public class MatchTextToCash : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _textMesh;

    void Update()
    {
        _textMesh.text = "Cash: " + GameState.Instance.GetEffectiveCash();
    }
}
