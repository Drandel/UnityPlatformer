using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Rigidbody2D rb;
    public Transform firePoint;
    public float bulletSpeed = 10.0f;

    public int count = 0;
    public bool left = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update(){

        if (Input.GetMouseButtonDown(0) && !PauseMenuController.IsPaused)
        {
           Shoot();
        }
    }
    void FixedUpdate()
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
                left = false;
        } else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) { // moving left
                // rotate character along y axis to face left
                
                Vector3 rotation = new Vector3(transform.rotation.x,-180,angle);
                transform.rotation = Quaternion.Euler(rotation); 
                left = true;
        }
    }
    void Shoot()
    {
        GameObject spawnGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Vector2 VectorAngle = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad),Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

        if(left){
            VectorAngle.x = VectorAngle.x * -1;
        }else{
        }
        spawnGO.GetComponent<Rigidbody2D>().velocity = VectorAngle * bulletSpeed;
    }
}

