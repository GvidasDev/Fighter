using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text blood;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text highscore;

    private void Start() {
        blood.text = "Blood: 0";
        score.text = "Score: 0";
        highscore.text = "Highscore: 0";
    }

    private void Update() {
        blood.text = $"Blood: {Player.Instance.GetBlood().ToString()}";
        score.text = $"Score: {Player.Instance.GetScore().ToString()}";
        highscore.text = $"Highscore: {Player.Instance.GetHighScore().ToString()}";
    }
}
