using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoomArea : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float defaultOrthoSize = 5f;
    public float smallerOrthoSize = 3f;
    public float transitionDuration = 0.5f;
    public GameObject Boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LeanTween.value(gameObject, defaultOrthoSize, smallerOrthoSize, transitionDuration)
                .setOnUpdate((float value) => {
                    virtualCamera.m_Lens.OrthographicSize = value;
                });
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LeanTween.value(gameObject, smallerOrthoSize, defaultOrthoSize, transitionDuration)
                .setOnUpdate((float value) => {
                    virtualCamera.m_Lens.OrthographicSize = value;
                });
        }
    }
}
