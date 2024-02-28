using UnityEngine;
using TMPro;
using System;

public class DisplayNumber : MonoBehaviour
{
    public TextMeshPro numberText; // Assign this in the inspector
    public GameObject livesTextGO; // Assign this in the inspector
    private TextMeshProUGUI livesText; // Assign this in the inspector
    public GameObject gd;
    private GameObject childObject;
    private float origanalWidth; 
    private float maxHealth;
    private GameStateController gameState;

    void Start()
    {
        numberText.text = gd.GetComponent<HealthController>().Health.ToString();
        childObject = transform.GetChild(1).gameObject;
        origanalWidth = childObject.GetComponent<RectTransform>().sizeDelta.x;
        maxHealth = gd.GetComponent<HealthController>().maxHealth;
        gameState = GameObject.Find("GameState").GetComponent<GameStateController>();
        livesText = livesTextGO.GetComponent<TextMeshProUGUI>();
        if(gameState == null){
            Debug.LogError("GameState Component no found");
        }
    }

    void Update()
    {
        float Health = gd.GetComponent<HealthController>().Health;
        numberText.text = Health.ToString();
        float newWidth = origanalWidth * (Health / maxHealth);
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, childObject.GetComponent<RectTransform>().sizeDelta.y);
        UpdateLivesRemaining();
    }

    private void UpdateLivesRemaining()
    {
        string lives = gameState.lifeCount.ToString();
        livesText.text = $"Lives: {lives}";
    }
}