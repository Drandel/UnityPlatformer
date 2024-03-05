using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveHeight = 5f;
    public float moveSpeed = 2f;
    public float pauseDurationAtTop = 1f;
    public float pauseDurationAtBottom = 1f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingUp = true;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + Vector3.up * moveHeight;

        // Start the movement
        MovePlatform();
    }

    public void MovePlatform()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * moveHeight);
    }
}
