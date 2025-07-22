using Battle;
using UnityEngine;
using UnityEngine.UI;

public class HeroPalleteSegment : MonoBehaviour
{
    public BattleCharacter SegmentCharacter {get; private set;}
    // Index of building in building database
    [SerializeField] private Image _icon;
    [SerializeField] private TMPro.TextMeshProUGUI _levelTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI _nextTextMesh;
    [SerializeField] private Image _healthFillImage;
    [SerializeField] private TMPro.TextMeshProUGUI _healthTextMesh;

    public void Setup(int index)
    {
        SegmentCharacter = BattleManager.Instance.PlayerCharacters[index];
        Refresh();
    }

    public void Refresh()
    {
        _icon.sprite = GameState.Instance.SpriteData.Battlers[SegmentCharacter.SpriteIndex];
        _levelTextMesh.text = "Level " + SegmentCharacter.Stats.Level;
        if (SegmentCharacter.Stats.Level - 1 == BattleManager.Instance.BattleDatabase.levelUpThresholds.Count)
        {
            _nextTextMesh.text = "MAX";
        }
        else 
        {
            _nextTextMesh.text = "NXT: " + (BattleManager.Instance.BattleDatabase.levelUpThresholds[SegmentCharacter.Stats.Level - 1] 
            - SegmentCharacter.Stats.Exp) + " EXP";
        }
        _healthFillImage.fillAmount = SegmentCharacter.HP / (float)SegmentCharacter.Stats.MaxHP;
        _healthTextMesh.text = SegmentCharacter.HP + " / " + SegmentCharacter.Stats.MaxHP + " HP";
    }
}