using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompleteAnimation : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float targetScale = 2f;
    public float floatHeight = 50f;
    public float floatDuration = 1f;

    void Start()
    {
    }
        

    public void ShowLevelComplete(){
         if (!textMeshPro.gameObject.activeSelf)
        {
            // Activate the TMP object
            textMeshPro.gameObject.SetActive(true);
        }
        // Set TMP text scale to 0
        textMeshPro.rectTransform.localScale = Vector3.zero;

        // Scale up from the center
        LeanTween.scale(textMeshPro.rectTransform, new Vector3(targetScale, targetScale, 1f), 1f)
            .setEaseOutQuad() // Example of easing function
            .setOnComplete(StartFloatAnimation); // Start float animation after scaling
    }

    void StartFloatAnimation()
    {
        // Float up and down
        LeanTween.moveY(textMeshPro.rectTransform, textMeshPro.rectTransform.anchoredPosition.y + floatHeight, floatDuration)
            .setLoopPingPong(); // Float up and down repeatedly
    }
}
