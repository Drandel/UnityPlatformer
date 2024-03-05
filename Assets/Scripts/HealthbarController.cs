using UnityEngine;
using TMPro;
using System;

public class DisplayNumber : MonoBehaviour
{
    public GameObject numberTextGO;
    private TextMeshProUGUI numberText;
    public GameObject livesTextGO;
    private TextMeshProUGUI livesText;
    public GameObject dinoGuy;
    private GameObject childObject;
    private float origanalWidth; 
    private float maxHealth;
    private GameStateController gameState;

    void Start()
    {
        numberText = numberTextGO.GetComponent<TextMeshProUGUI>();
        numberText.text = dinoGuy.GetComponent<HealthController>().Health.ToString();
        childObject = transform.GetChild(1).gameObject;
        origanalWidth = childObject.GetComponent<RectTransform>().sizeDelta.x;
        maxHealth = dinoGuy.GetComponent<HealthController>().maxHealth;
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        if(livesTextGO == null){
        livesText = livesTextGO.GetComponent<TextMeshProUGUI>();
        }
        if(gameState == null){
            Debug.LogError("GameState Component no found");
        }
    }

    void Update()
    {
        float Health = dinoGuy.GetComponent<HealthController>().Health;
        numberText.text = Health.ToString();
        float newWidth = origanalWidth * (Health / maxHealth);
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, childObject.GetComponent<RectTransform>().sizeDelta.y);
        if(gameState != null && livesTextGO != null) UpdateLivesRemaining();
    }

    private void UpdateLivesRemaining()
    {
        string lives = gameState.lifeCount.ToString();
        livesText.text = $"Lives: {lives}";
    }
}