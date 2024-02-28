using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PyramidFacadeController : MonoBehaviour
{
    
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;

    public float fadeDuration = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeToZero();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeToOpaque();
        }
    }

    private Coroutine currentFadeCoroutine;

    public void FadeToZero()
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(FadeTilemapOpacity(0f, fadeDuration));
    }

    public void FadeToOpaque()
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(FadeTilemapOpacity(1f, fadeDuration));
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
