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
        int level = BattleManager.Instance.CalculateEXP(SegmentCharacter.EXPBuildingIndex);
        _levelTextMesh.text = "Level " + level;
        _nextTextMesh.text = "NXT: " + (BattleManager.Instance.BattleDatabase.levelUpThresholds[level - 1] 
            - BuildingManager.Instance.TotalBuiltCount[SegmentCharacter.EXPBuildingIndex]) + " EXP";

        _healthFillImage.fillAmount = SegmentCharacter.HP / (float)SegmentCharacter.Stats.MaxHP;
        _healthTextMesh.text = SegmentCharacter.HP + " / " + SegmentCharacter.Stats.MaxHP + " HP";
    }
}