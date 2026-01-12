using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button abilitiesShopButton;
    [SerializeField] private Button openPanelButton;
    [SerializeField] private Button quitGameButton;

    [SerializeField] private GameObject panelToOpen;

    void Start() {
        if (abilitiesShopButton != null) {
            abilitiesShopButton.onClick.AddListener(GoToAbilitiesShop);
        }
        if (openPanelButton != null) {
            openPanelButton.onClick.AddListener(OpenPanel);
        }
        if (quitGameButton != null) {
            quitGameButton.onClick.AddListener(QuitGame);
        }
    }

    private void GoToAbilitiesShop() {
        SceneManager.LoadScene("AbilitiesShop");
    }

    private void OpenPanel() {
        if (panelToOpen != null) {
            panelToOpen.SetActive(true);
        }
    }

    private void QuitGame() {
        Application.Quit();
    }
}
