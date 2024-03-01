using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveHeight = 5f; // Height the platform moves
    public float moveSpeed = 2f; // Speed of movement
    public float pauseDurationAtTop = 1f; // Pause duration at the top
    public float pauseDurationAtBottom = 1f; // Pause duration at the bottom

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingUp = true;

    private void Start()
    {
        // Store start and end positions
        startPosition = transform.position;
        endPosition = startPosition + Vector3.up * moveHeight;

        // Start the movement
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Move the platform up and down infinitely using LeanTween
        if (movingUp)
        {
            LeanTween.move(gameObject, endPosition, moveSpeed).setOnComplete(() =>
            {
                LeanTween.delayedCall(gameObject, pauseDurationAtTop, () =>
                {
                    movingUp = false;
                    MovePlatform();
                });
            });
        }
        else
        {
            LeanTween.move(gameObject, startPosition, moveSpeed).setOnComplete(() =>
            {
                LeanTween.delayedCall(gameObject, pauseDurationAtBottom, () =>
                {
                    movingUp = true;
                    MovePlatform();
                });
            });
        }
    }

    // Draw gizmos in the editor
    private void OnDrawGizmos()
    {
        // Draw a line gizmo to visualize the movement height
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * moveHeight);
    }
}
