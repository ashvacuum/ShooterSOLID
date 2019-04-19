using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Awake()
    {
        rotator = new CharRotator(rotationSpeed);
    }

    private void Update()
    {
        rotator.Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform);
    }
}
