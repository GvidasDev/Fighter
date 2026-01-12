using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    [SerializeField] private Button backButton;
    [SerializeField] private GameObject panelToDisable;

    [SerializeField] private Slider mainVolumeSlider;

    void Start() {
        if (backButton != null) {
            backButton.onClick.AddListener(DisablePanel);
        }

        if (mainVolumeSlider != null) {
            mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
        }

        if (mainVolumeSlider != null) {
            mainVolumeSlider.value = AudioListener.volume;
        }
    }

    private void DisablePanel() {
        if (panelToDisable != null) {
            panelToDisable.SetActive(false);
        }
    }

    private void SetMainVolume(float value) {
        AudioListener.volume = value;
    }
}
