using System.Collections.Generic;
using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    public Boss bossController;

    void FixedUpdate()
    {
    }

    public List<Collider2D> GetCollidersInKillBox()
    {
        // Define the box area using Physics2D.OverlapBox
        Collider2D[] AllhitColliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        List<Collider2D> hitColliders = new List<Collider2D>();

        // Iterate through the colliders found
        foreach (Collider2D collider in AllhitColliders)
        {
            if ( 
                (collider.CompareTag("Enemy") && collider.gameObject.name != "Boss") 
                || collider.CompareTag("Bullet") 
                || collider.CompareTag("Missile")
            )
            {
                hitColliders.Add(collider);
            }
        }
        return hitColliders;
    }

    public void RunKillBox()
    {
        foreach (Collider2D coll in GetCollidersInKillBox())
        {
            Destroy(coll.gameObject);
        }
        bossController.SetState("Waiting");
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
