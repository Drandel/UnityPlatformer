using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherAnimation : MonoBehaviour
{
    public float floatRange = 1f; // Range of floating motion
    public float floatSpeed = 1f; // Speed of floating motion

    private void Start() {
        Float();
    }

    private void Float()
    {
        // Calculate the target positions
        float startY = transform.position.y;
        float targetY = startY + floatRange;

        // Use LeanTween to animate the floating motion
        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                // When reaching the top, float down
                FloatDown();
            });
    }

    private void FloatDown()
    {
        // Calculate the target positions
        float startY = transform.position.y;
        float targetY = startY - floatRange;

        // Use LeanTween to animate the floating motion
        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                // When reaching the bottom, float up again
                Float();
            });
    }
}
