using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMainCanvasUI : MonoBehaviour {
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Button resumeButon;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button endGameBackButton;

    private void Start() {
        if (resumeButon != null) {
            resumeButon.onClick.AddListener(ResumeGame);
        }
        if (optionsButton != null) {
            optionsButton.onClick.AddListener(OpenOptions);
        }
        if (backButton != null) {
            backButton.onClick.AddListener(BackToMainMenu);
        }
        if (endGameBackButton != null) {
            endGameBackButton.onClick.AddListener(BackToMainMenu);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }

        if (GameManager.Instance.IsPlayerDead) {
            GameOver();
        }
    }

    private void PauseGame() {
        if (pausePanel != null) {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void ResumeGame() {
        if (pausePanel != null) {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void OpenOptions() {
        if (optionsPanel != null) {
            optionsPanel.SetActive(true);
        }
    }

    private void BackToMainMenu() {
        SceneManager.LoadScene("Main menu");
    }

    private void GameOver() {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
}
