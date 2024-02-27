using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTakeDamage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public string[] colliderTags;
    public float flashSpeed = 0.25f;

    private void OnCollisionEnter2D(Collision2D other) {
        foreach (var colliderTag in colliderTags)
        {
            if(other.gameObject.CompareTag(colliderTag)){
                FlashCharacter();
            }   
        }
        
    }

    private void FlashCharacter()
    {
        // Flash from white to red
        LeanTween.value(gameObject, Color.white, Color.red, flashSpeed)
            .setOnUpdateColor(color => spriteRenderer.color = color)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // Flash back from red to white
                LeanTween.value(gameObject, Color.red, Color.white, flashSpeed)
                    .setOnUpdateColor(color => spriteRenderer.color = color)
                    .setEase(LeanTweenType.easeOutQuad);
            });
    }
}
