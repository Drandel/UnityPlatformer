using System;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Rigidbody2D rb;
    public Transform firePoint;
    private Transform parentTransform;
    public float rocketSpeed = 10.0f;
    public float rocketKick = 15.0f;
    private float nextShot = -1.0f;
    private float fireRate = 5;
    private float shotTime = 0;
    public bool aquired = false;
    private GameObject cooldownTextGO;
    private TextMeshProUGUI cooldownText; // Assign this in the inspector
    

    public int count = 0;
    public bool left = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parentTransform = transform.parent;
        GetComponent<Renderer>().enabled = false;
        cooldownTextGO = GameObject.Find("CooldownText");
        cooldownText = cooldownTextGO.GetComponent<TextMeshProUGUI>();
        cooldownText.text = "";

    }
    void Update(){
        if (Input.GetMouseButtonDown(1) && nextShot == -1.0f && GetComponent<Renderer>().enabled)
        {
           Shoot();
           nextShot = 0;
           shotTime = Time.time;
        }

        if(nextShot != -1){
            cooldownText.text = $"Next Rocket: {Mathf.RoundToInt(5 + (shotTime - Time.time))} sec";
            nextShot = Time.time - shotTime;
            if(nextShot > fireRate){
            nextShot = -1.0f;
            cooldownText.text = "Next Rocket: Ready";
            } 
        }
    }
    void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f;
        float angle = (Mathf.Atan2(direction.y, Mathf.Abs(direction.x)))  * Mathf.Rad2Deg;

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
        GameObject spawnGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 90));

        Vector2 VectorAngle = new Vector2(Mathf.Cos((transform.rotation.eulerAngles.z) * Mathf.Deg2Rad),Mathf.Sin((transform.rotation.eulerAngles.z) * Mathf.Deg2Rad));

        if(left){
            VectorAngle.x = VectorAngle.x * -1;
        }else{
        }
        spawnGO.GetComponent<Rigidbody2D>().velocity = VectorAngle * rocketSpeed;
        Rigidbody2D parentRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        parentRigidbody.AddForce(VectorAngle * -rocketKick,ForceMode2D.Impulse);
    }

    internal void setCooldownText(string message)
    {
        cooldownText.text = message;
    }
}

