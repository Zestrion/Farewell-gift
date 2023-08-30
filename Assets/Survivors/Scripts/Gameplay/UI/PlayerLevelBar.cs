using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.Gameplay
{
    public class PlayerLevelBar : MonoBehaviour
    {
        [SerializeField] private Image experienceFillImage;
        [SerializeField] private TextMeshProUGUI levelText;

        private string originalLevelText;
        private float targetFillAmount;
        private const float ADD_EXPERIENCE_ANIMATION_SPEED = 2f;

        private void Awake()
        {
            experienceFillImage.fillAmount = 0f;
        }

        private void Start()
        {
            originalLevelText = levelText.text;
            levelText.text = originalLevelText + "1";
        }

        private void OnEnable()
        {
            ExperienceController.OnLevelChanged += PlayerLevelChanged;
            ExperienceController.OnExperienceChanged += PlayerExperienceChanged;
        }

        private void OnDisable()
        {
            ExperienceController.OnLevelChanged -= PlayerLevelChanged;
            ExperienceController.OnExperienceChanged -= PlayerExperienceChanged;
        }

        private void PlayerLevelChanged(int _playerLevel)
        {
            levelText.text = originalLevelText + _playerLevel.ToString();
        }

        private void PlayerExperienceChanged(int _playerExperience, int _nextLevelExperience, int _previousLevelExperience)
        {
            float _newTargetFillAmount = ((float)_playerExperience - (float)_previousLevelExperience) / ((float)_nextLevelExperience - (float)_previousLevelExperience);
            if (_newTargetFillAmount < targetFillAmount)
            {
                experienceFillImage.fillAmount = 0f;
            }
            targetFillAmount = _newTargetFillAmount;
        }

        private void TryUpdateFillAmount()
        {
            if (!Mathf.Approximately(experienceFillImage.fillAmount, targetFillAmount))
            {
                experienceFillImage.fillAmount = Mathf.MoveTowards(experienceFillImage.fillAmount, targetFillAmount, ADD_EXPERIENCE_ANIMATION_SPEED * Time.deltaTime);
            }
        }

        private void Update()
        {
            TryUpdateFillAmount();
        }
    }
}
