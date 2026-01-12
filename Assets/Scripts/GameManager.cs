using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPlayerDead { get; set; }
    private void Awake() {
        if (Instance != null) {
            return;
        }
        Instance = this;

        Time.timeScale = 1;
        IsPlayerDead = false;
    }
}
