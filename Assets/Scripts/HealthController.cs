
using UnityEngine;

public class HealthController : MonoBehaviour

{

    public float Health = 100;
    
    public void damageTaken(float Increment)
    {
        Health -= Increment;
        if(Health < 0) terminal();
    }

    private void terminal(){

    }
}