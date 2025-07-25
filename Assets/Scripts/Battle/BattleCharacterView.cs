using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class BattleCharacterView : MonoBehaviour
    {
        [SerializeField] private Image _healthBarFillImage;
        [SerializeField] private Image _timerFillImage;
        [SerializeField] private Image _battlerImage;
        [SerializeField] private Image _statusIcon;

        public void Setup(BattleCharacter bc)
        {
            _battlerImage.sprite = bc.Sprite;

        }

        public void Refresh(BattleCharacter bc)
        {
            _healthBarFillImage.fillAmount = (float)bc.HP / bc.LocalStats.MaxHP;
            _timerFillImage.fillAmount = bc.timer / (3f - bc.LocalStats.Spd * 0.04f);
            _statusIcon.enabled = bc.showStatusIcon;
            _statusIcon.sprite = GameState.Instance.SpriteData.Statuses[bc.statusSpriteIndex];

            if (bc.HP <= 0)
            {
                _battlerImage.color = Color.gray;
                _healthBarFillImage.enabled = false;
                _timerFillImage.enabled = false;
            }
        }

    }
}