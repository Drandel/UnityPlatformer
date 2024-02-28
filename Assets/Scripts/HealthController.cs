
using UnityEngine;

public class HealthController : MonoBehaviour

{

    public float Health = 100;
    public float maxHealth = 100;

    private void Update() {
        if(Health <= 0) terminal();
    }

    public float damageTaken(float Increment)
    {
        Health -= Increment;
        return Health;
    }

    private void terminal(){
        if(gameObject.CompareTag("Player")){
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.DieAndRespawn();
            Health = maxHealth;
        } else {
            Destroy(gameObject);
        }
        
    }
}