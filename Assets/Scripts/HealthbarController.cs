using UnityEngine;
using TMPro;
using System;

public class DisplayNumber : MonoBehaviour
{
    public GameObject numberTextGO;
    private TextMeshProUGUI numberText;
    public GameObject livesTextGO;
    public GameObject greenBar;
    private TextMeshProUGUI livesText;
    public GameObject dinoGuy;
    private float origanalWidth; 
    private float maxHealth;
    private GameStateController gameState;

    void Start()
    {
        numberText = numberTextGO.GetComponent<TextMeshProUGUI>();
        numberText.text = dinoGuy.GetComponent<HealthController>().Health.ToString();
        origanalWidth = greenBar.GetComponent<RectTransform>().sizeDelta.x;
        maxHealth = dinoGuy.GetComponent<HealthController>().maxHealth;
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        livesText = livesTextGO.GetComponent<TextMeshProUGUI>();
        if(gameState == null){
            Debug.LogError("GameState Component no found");
        }
    }

    void Update()
    {
        float Health = dinoGuy.GetComponent<HealthController>().Health;
        numberText.text = Health.ToString();
        float newWidth = origanalWidth * (Health / maxHealth);
        greenBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, greenBar.GetComponent<RectTransform>().sizeDelta.y);
        if(gameState != null && livesTextGO != null) UpdateLivesRemaining();
    }

    private void UpdateLivesRemaining()
    {
        string lives = gameState.lifeCount.ToString();
        livesText.text = $"Lives: {lives}";
    }
}