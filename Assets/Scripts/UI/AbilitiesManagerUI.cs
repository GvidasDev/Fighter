using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbilitiesManagerUI : MonoBehaviour {
    [SerializeField] private PlayerAbilitiesSO playerAbilities;

    [SerializeField] private Button shieldButton;
    [SerializeField] private Button immuneButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button loadSceneButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button enablePanelButton;
    [SerializeField] private TMP_Text bloodText;

    private int shieldPrice = 1500;
    private int immunePrice = 1000;
    private int speedPrice = 1000;

    [SerializeField] private Player player;
    [SerializeField] private GameObject panelToEnable;

    private void Awake() {
        player.LoadPlayerData();
    }

    private void Start() {
        SetDefault();

        shieldButton.onClick.AddListener(() => SetAbility(Abilities.Shield, shieldPrice));
        immuneButton.onClick.AddListener(() => SetAbility(Abilities.Immune, immunePrice));
        speedButton.onClick.AddListener(() => SetAbility(Abilities.Speed, speedPrice));
        loadSceneButton.onClick.AddListener(LoadScene);
        backButton.onClick.AddListener(LoadMainMenuScene);
        enablePanelButton.onClick.AddListener(EnablePanel);

        UpdateBloodUI();
    }

    private void Update() {
        UpdateBloodUI();
    }

    private void SetAbility(Abilities ability, int price) {
        if (player.GetBlood() >= price) {
            playerAbilities.Abilitie = ability;
            player.ReduceBlood(price);
            player.SavePlayerData();
        }
    }

    private void SetDefault() {
        playerAbilities.Abilitie = Abilities.None;
    }

    private void LoadScene() {
        SceneManager.LoadScene("Game");
    }

    private void LoadMainMenuScene() {
        SceneManager.LoadScene("Main Menu");
    }

    private void EnablePanel() {
        if (panelToEnable != null) {
            panelToEnable.SetActive(true);
        }
    }

    private void UpdateBloodUI() {
        if (bloodText != null) {
            bloodText.text = $"{player.GetBlood()}";
        }
    }
}
