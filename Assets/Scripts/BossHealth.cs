
using UnityEngine;
using TMPro;
using System;

public class BossHealthUI : MonoBehaviour
{
    private TextMeshProUGUI livesText; // Assign this in the inspector
    public GameObject Boss;
    private GameObject childObject;
    private float origanalWidth; 
    private float maxHealth;
    private GameStateController gameState;

    void Start()
    {
        childObject = transform.GetChild(1).gameObject;
        origanalWidth = childObject.GetComponent<RectTransform>().sizeDelta.x;
        maxHealth = Boss.GetComponent<HealthController>().maxHealth;
    }

    void Update()
    {
        float Health;
        if(Boss != null){
            Health = Boss.GetComponent<HealthController>().Health;
        }else{
            Health = 0;
        }
        float newWidth = origanalWidth * (Health / maxHealth);
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, childObject.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void UpdateLivesRemaining()
    {
        string lives = gameState.lifeCount.ToString();
        livesText.text = $"Lives: {lives}";
    }
}