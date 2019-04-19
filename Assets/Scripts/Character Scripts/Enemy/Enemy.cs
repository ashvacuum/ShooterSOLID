using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rotator = new CharRotator(rotationSpeed);
    }

    private void Update()
    {
        rotator.Rotate(player.position, transform);
    }
}
