using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenRoomTrigger : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            // gameObject.SetActive(false);
            FadeToZero();
        }
    }

    public void FadeToZero()
    {
        StartCoroutine(FadeTilemapOpacity(0f, fadeDuration));
    }

    private IEnumerator FadeTilemapOpacity(float targetOpacity, float duration)
    {
        float startTime = Time.time;
        Color startColor = tilemap.color;
        float startOpacity = startColor.a;

        while (Time.time < startTime + duration)
        {
            float normalizedTime = (Time.time - startTime) / duration;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, normalizedTime);

            Color newColor = startColor;
            newColor.a = newOpacity;
            tilemap.color = newColor;

            yield return null;
        }

        Color finalColor = startColor;
        finalColor.a = targetOpacity;
        tilemap.color = finalColor;
    }
}
