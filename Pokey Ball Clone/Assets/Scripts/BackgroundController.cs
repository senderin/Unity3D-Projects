using System;
using System.Collections;
using System.Collections.Generic;
using Imphenzia;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundController : MonoBehaviour
{
    internal void SetBackground(int level)
    {
        SetSizePosition(level);
        ChangeBackgroundColor();
    }

    private void ChangeBackgroundColor()
    {
        // GradientColorObject is ready script, it is just changed with the scope below.
        GradientColorKey[] keys = new GradientColorKey[]
        {
            new GradientColorKey(Random.ColorHSV(), 0),
            new GradientColorKey(Random.ColorHSV(), 0.3f),
            new GradientColorKey(Random.ColorHSV(), 0.6f),
            new GradientColorKey(Random.ColorHSV(), 1),
        };
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] {
            new GradientAlphaKey(255, 0),
            new GradientAlphaKey(255, 50),
            new GradientAlphaKey(255, 50),
            new GradientAlphaKey(255, 100)
        };

        GetComponent<GradientSkyObject>().gradient.SetKeys(keys, alphaKeys);
    }

    private void SetSizePosition(int level)
    {
        float length = (40 * level);
        transform.localScale = new Vector3(20, length, 1);
        transform.position = new Vector3(transform.position.x, length / 2 - 1, transform.position.z);
    }
}
