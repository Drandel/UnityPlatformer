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

    void Update()
    {
        float dist = (cam.transform.position.x - startpos) * parallaxEffect;

        transform.position = new Vector3(startpos + dist, BGHeight, transform.position.z);

        if (Mathf.Abs(cam.transform.position.x - transform.position.x) >= length)
        {
            startpos += Mathf.Sign(cam.transform.position.x - transform.position.x) * length;
        }
    }
}
