using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallTrigger : MonoBehaviour
{
    public BoxCollider2D wall;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
           
            wall.enabled = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("PLAYER Exited");
            wall.enabled = false;
        } 
    }
}
