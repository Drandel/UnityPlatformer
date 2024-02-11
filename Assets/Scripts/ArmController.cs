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
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        // Rotate the gun towards the mouse cursor
        handleArmLookDirection(angle);
    }
    private void handleArmLookDirection(float angle)
    {

        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x){ // if moving right
                // rotate character along y axis to face right
                Vector3 rotation = new Vector3(transform.rotation.x,0,angle);
                transform.rotation = Quaternion.Euler(rotation); 
        } else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) { // moving left
                // rotate character along y axis to face left
                
                Vector3 rotation = new Vector3(transform.rotation.x,180,angle);
                transform.rotation = Quaternion.Euler(rotation); 
        }
    }
}

