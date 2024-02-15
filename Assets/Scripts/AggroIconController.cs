using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroIconController : MonoBehaviour
{
    Vector3 startingScale =  new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 middleScale = new Vector3(0.9f, 0.9f, 0.9f);
    Vector3 endingScale = new Vector3(0.4f, 0.4f, 0.4f);
    public float fraction = 0;
    public float fraction2 = 0;
    public float speed = 0.5f;
    bool swap = false;
    public float destroyTime = 1f;
    public float fadeSpeed = 1f;
    bool started = false;
    bool fading = false;

    void Start()
    {
        transform.localScale = startingScale;
    }

    void Update()
    {
        if(!swap){
            fraction += Time.deltaTime * speed;
            transform.localScale = Vector3.Lerp(startingScale, middleScale, fraction);
        }
        
        if(middleScale == transform.localScale){
            swap = true;
            fraction2 += Time.deltaTime * speed;
            transform.localScale = Vector3.Lerp(middleScale, endingScale, fraction);
        }
        if(transform.localScale == endingScale){
            if(!started) StartCoroutine(WaitFor(1));

            if(fading){
                Color objectColor = GetComponent<Renderer>().material.color;
                float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color (objectColor.r, objectColor.g, objectColor.b, fadeAmount);

                GetComponent<Renderer>().material.color = objectColor;

                if(objectColor.a <= 0){
                    Destroy(gameObject);
                }
            }
            
        }
    }

    IEnumerator WaitFor(float seconds)
    {
        started = true;
        yield return new WaitForSeconds(seconds);
        fading = true;
    }
}
