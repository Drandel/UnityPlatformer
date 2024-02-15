using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class DisplayNumber : MonoBehaviour
{
    public TextMeshPro numberText; // Assign this in the inspector
    public GameObject gd;
    private GameObject childObject;
    private float origanalWidth; 
    private float maxHealth;


    void Start()
    {
        numberText.text = gd.GetComponent<HealthController>().Health.ToString();
        childObject = transform.GetChild(1).gameObject;
        origanalWidth = childObject.GetComponent<RectTransform>().sizeDelta.x;
        maxHealth = gd.GetComponent<HealthController>().maxHealth;
    }

    void Update()
    {
        float Health = gd.GetComponent<HealthController>().Health;
        numberText.text = Health.ToString();
        float newWidth = origanalWidth * (Health / maxHealth);
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, childObject.GetComponent<RectTransform>().sizeDelta.y);
    } 
}