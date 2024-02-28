using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public float BGHeight;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
       // Calculate the distance the camera has moved horizontally
        float dist = (cam.transform.position.x - startpos) * parallaxEffect;

        // Move the background only along the X-axis, keeping Y and Z the same
        transform.position = new Vector3(startpos + dist, BGHeight, transform.position.z);

        // If the camera has moved past the background's length, adjust start position
        if (Mathf.Abs(cam.transform.position.x - transform.position.x) >= length)
        {
            startpos += Mathf.Sign(cam.transform.position.x - transform.position.x) * length;
        }
    }
}
