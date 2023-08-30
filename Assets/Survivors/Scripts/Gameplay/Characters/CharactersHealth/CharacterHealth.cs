using UnityEngine;

namespace Survivors {
    public class CharacterHealth : MonoBehaviour {
        [SerializeField] private CharacterHealthBar healthBar;

        private int maxHealth;
        private int currentHealth;
        private bool containsHealthBar;
        private bool isInitialized;

        internal void Init(int _maxHealth, int _currentHealth = -1) {
            maxHealth = _maxHealth;
            currentHealth = _currentHealth == -1 ? _maxHealth : _currentHealth;
            containsHealthBar = healthBar != null;
            isInitialized = true;
            UpdateHealthBar();
        }


        internal void TakeDamage(int _damage, bool _critical)
        {
            AddToHealth(-_damage);
        }

        internal void AddToMaxHealth(int _maxHealth) {
            maxHealth += _maxHealth;
        }

        internal bool IsAlive() => currentHealth > 0;

        internal void AddToHealth(int _health) {
            CheckIfInitialized();
            currentHealth += _health;
            currentHealth = currentHealth > 0 ? currentHealth : 0;
            UpdateHealthBar();
        }

        private void UpdateHealthBar() {
            if(containsHealthBar) {
                if (IsAlive()) {
                    healthBar.SetHealthFillAmount((float)currentHealth / (float)maxHealth);
                } else {
                    healthBar.SetHealthFillAmount(0f);
                }
            }
        }

        private void CheckIfInitialized() {
            if (!isInitialized) {
                Debug.LogError("Character health is not initialized! Initialize it before use.");
            }
        }
    }
}
