using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherAnimation : MonoBehaviour
{
    public float floatRange = 1f;
    public float floatSpeed = 1f;

    private void Start() {
        Float();
    }

    private void Float()
    {
        float startY = transform.position.y;
        float targetY = startY + floatRange;

        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                FloatDown();
            });
    }

    private void FloatDown()
    {
        float startY = transform.position.y;
        float targetY = startY - floatRange;

        LeanTween.moveY(gameObject, targetY, floatSpeed)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                Float();
            });
    }
}
