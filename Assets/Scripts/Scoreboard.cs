using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    public TextMeshProUGUI livesRemainingText;
    public TextMeshProUGUI EnemiesKilledText;
    public TextMeshProUGUI TimeSpentText;
    public TextMeshProUGUI TotalPointsText;
    private GameStateController gameState;
    public float livesRemainingPointsMultiplier = 100;
    public float enemiesKilledPointsMultiplier = 50;
    public float timeSpentPointsBase = 10000;
    public int basePoints = 800;

    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        livesRemainingText.text = $"Lives Remaining: {gameState.lifeCount}";
        EnemiesKilledText.text = $"Enemies Killed: {gameState.enemiesKilled}";
        TimeSpentText.text = $"Time Spent: {gameState.getTimeSpent()} seconds";
        TotalPointsText.text = $"Total Score: {getTotalScore()}";
    }

    private int getTotalScore()
    {
        int livesRemainingPoints = (int)Math.Round(gameState.lifeCount * livesRemainingPointsMultiplier);
        int enemiesKilledPoints = (int)Math.Round(gameState.enemiesKilled * enemiesKilledPointsMultiplier);
        float timeSpentPoints = timeSpentPointsBase / (float)Math.Sqrt(gameState.getTimeSpent());
        return (int)Math.Round(livesRemainingPoints + enemiesKilledPoints + timeSpentPoints);
    }

    public void QuitButton(){
        Application.Quit();
    }

    public void MenuButton(){
        SceneManager.LoadScene("MainMenu");
        // SceneManager.LoadScene("Level3");
    }
}
