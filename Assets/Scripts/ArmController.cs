using UnityEngine;

public class GunRotation : MonoBehaviour
{
    void Update()
    {
        // Get the direction from the gun position to the mouse cursor
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f; // Make sure the direction is in the 2D plane

        // Calculate the angle to rotate the gun
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun towards the mouse cursor
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}