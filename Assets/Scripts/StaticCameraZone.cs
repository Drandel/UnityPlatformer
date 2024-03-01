using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class StaticCameraZone : MonoBehaviour
{
    public Transform player;
    public Transform fixedCameraPosition;
    public float zoomOutSize = 10f;
    public float transitionDuration = 1f;
    public AnimationCurve zoomCurve;

    private CinemachineVirtualCamera virtualCamera;
    private float originalOrthoSize;
    private Transform originalFollowTarget;

    private void Start()
    {
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        originalOrthoSize = virtualCamera.m_Lens.OrthographicSize;
        originalFollowTarget = virtualCamera.Follow;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Follow = null;
            Vector3 newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, virtualCamera.gameObject.transform.position.z);
            LeanTween.move(virtualCamera.gameObject, newPosition, transitionDuration);
            LeanTween.value(gameObject, virtualCamera.m_Lens.OrthographicSize, 23f, transitionDuration)
                .setEase(zoomCurve)
                .setOnUpdate((float val) => virtualCamera.m_Lens.OrthographicSize = val);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Follow = originalFollowTarget;
            LeanTween.move(virtualCamera.gameObject, originalFollowTarget.position, transitionDuration);
            LeanTween.value(gameObject, virtualCamera.m_Lens.OrthographicSize, originalOrthoSize, transitionDuration)
                .setEase(zoomCurve)
                .setOnUpdate((float val) => virtualCamera.m_Lens.OrthographicSize = val);
        }
    }
}
