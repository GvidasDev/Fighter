using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BarState {
    Health,
    Stamina
}

public class BarFillUI : MonoBehaviour {
    public BarState state;

    [SerializeField] private Image barFillImage;
    [SerializeField] private int maxValue;
    private int currentValue;

    void Start() {
        if (state == BarState.Health) {
            maxValue = Player.Instance.GetHealth();
            currentValue = Player.Instance.GetHealth();
            UpdateHealthBar();
        } else if (state == BarState.Stamina) {
            maxValue = 100;
            currentValue = Player.Instance.GetStamina();
            UpdateStaminaBar();
        }
    }

    void Update() {
        if (state == BarState.Health) {
            currentValue = Player.Instance.GetHealth();
            UpdateHealthBar();
        } else if (state == BarState.Stamina) {
            currentValue = Player.Instance.GetStamina();
            UpdateStaminaBar();
        }
    }

    private void UpdateHealthBar() {
        if (barFillImage != null) {
            barFillImage.fillAmount = (float)currentValue / maxValue;
        }
    }

    private void UpdateStaminaBar() {
        if (barFillImage != null) {
            barFillImage.fillAmount = (float)currentValue / maxValue;
        }
    }
}
