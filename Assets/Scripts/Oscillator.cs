using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    private Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        var cycles = Time.time / period;
        var rawSinWave = Mathf.Sin(cycles * (float) Math.PI * 2);
        var movementFactor = rawSinWave / 2f + 0.5;
        var offset = movementVector * (float) movementFactor;
        transform.position = startingPos + offset;
    }
}
