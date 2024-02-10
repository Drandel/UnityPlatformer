using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float horizontalMarginPercentage = 0.35f; // Margin from the edge of the screen (percentage)
    public float cameraSpeed = 5f; // Speed of camera movement

    private float horizontalMargin; // Margin from the edge of the screen (value)
    private Vector3 previousPlayerPosition; // Previous player position in world coordinates

    void Start()
    {
        // Initialize previousPlayerPosition with the initial player position
        previousPlayerPosition = target.position;

        // Calculate horizontal margin value based on screen width
        horizontalMargin = horizontalMarginPercentage * Screen.width;
    }

    void LateUpdate()
    {
        // Get the player's position in screen coordinates
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(target.position);

        // Check if player is within the margins
        if (playerScreenPosition.x < horizontalMargin || playerScreenPosition.x > Screen.width - horizontalMargin)
        {
            // Calculate the movement direction based on the difference in player position between frames
            Vector3 moveDirection = target.position - previousPlayerPosition;

            // Move the camera by the same amount
            transform.position += moveDirection;

            // Store the current player position for the next frame
            previousPlayerPosition = target.position;
        }
    }
}
