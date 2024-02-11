using Unity.VisualScripting;
using UnityEngine;

// public class CameraController : MonoBehaviour
// {
//     public Transform target; // Reference to the player's transform
//     public float horizontalMarginPercentage = 0.35f; // Margin from the edge of the screen (percentage)
//     public float cameraSpeed = 5f; // Speed of camera movement

//     private float horizontalMargin; // Margin from the edge of the screen (value)
//     private Vector3 previousPlayerPosition; // Previous player position in world coordinates

//     void Start()
//     {
//         // Initialize previousPlayerPosition with the initial player position
//         previousPlayerPosition = target.position;

//         // Calculate horizontal margin value based on screen width
//         horizontalMargin = horizontalMarginPercentage * Screen.width;
//     }

//     void LateUpdate()
//     {
//         // Get the player's position in screen coordinates
//         Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(target.position);

//         // Check if player is within the margins
//         if (playerScreenPosition.x < horizontalMargin || playerScreenPosition.x > Screen.width - horizontalMargin)
//         {
//             // Calculate the movement direction based on the difference in player position between frames
//             Vector3 moveDirection = target.position - previousPlayerPosition;

//             // Move the camera by the same amount
//             transform.position += moveDirection;

//             // Store the current player position for the next frame
//             previousPlayerPosition = target.position;
//         }
//     }
// }

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float  smoothing = 1.25f;
    public Camera mainCamera;
    public float edgeDistanceThreshold = 0.75f; // Distance threshold from the edge (0 means exactly at the edge)

    private void LateUpdate(){
        Vector3 desiredPostion = player.transform.position + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPostion, smoothing * Time.deltaTime);
        transform.position = smoothedPostion;
    }

    private void checkPlayerNearBoundary(){
        if (player != null)
        {
            // Convert player's position to screen space coordinates
            Vector3 screenPos = mainCamera.WorldToViewportPoint(player.position);

            // Check how close the player is to the edges
            bool nearLeftEdge = screenPos.x <= edgeDistanceThreshold;
            bool nearRightEdge = screenPos.x >= 1 - edgeDistanceThreshold;
            bool nearBottomEdge = screenPos.y <= edgeDistanceThreshold;
            bool nearTopEdge = screenPos.y >= 1 - edgeDistanceThreshold;

            // Example usage (you can replace this with your desired behavior)
            if (nearLeftEdge)
            {
                Debug.Log("Player is near the left edge of the screen.");
            }
            if (nearRightEdge)
            {
                Debug.Log("Player is near the right edge of the screen.");
            }
            if (nearBottomEdge)
            {
                Debug.Log("Player is near the bottom edge of the screen.");
            }
            if (nearTopEdge)
            {
                Debug.Log("Player is near the top edge of the screen.");
            }
        }
    }
}
