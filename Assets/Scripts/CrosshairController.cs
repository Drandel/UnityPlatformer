using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // Hide the system cursor
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPos.z = 0f; // zero z
        rectTransform.position = mouseWorldPos;
    }
}
