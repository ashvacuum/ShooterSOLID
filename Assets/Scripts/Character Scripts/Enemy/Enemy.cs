using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Transform player;    
    [SerializeField] private float range;
    Unit unit;
    bool hasTarget;
    float refreshRate = 0.5f;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        if (GameObject.FindGameObjectWithTag("Player").transform)
        {
            hasTarget = true;
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }        
        rotator = new CharRotator(rotationSpeed);
    }

    private void Start()
    {
        //StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        rotator.Rotate(player.position, transform);
    }

    IEnumerator UpdatePath()
    {
        while (hasTarget/* && Vector2.Distance(transform.position, player.position) > range*/)
        {
            unit.StartPathing(player.position);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
