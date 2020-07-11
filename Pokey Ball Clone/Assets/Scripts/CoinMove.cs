using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    public int oscillationCoef; // makes the oscillations different
    void Update()
    {
        // moves the coin in a sine wave
        transform.position += transform.right/100 * Mathf.Sin(Time.time * 3f + oscillationCoef) * 1f;
    }
}
