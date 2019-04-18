using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectile;

    [SerializeField] private Transform direction;

    [SerializeField] private float projectileSpeed = 300f;
    // Start is called before the first frame update
    void Start()
    {
        CharInput.Shoot += Fire;
    }   
    void Fire()
    {
        Rigidbody2D spawned = Instantiate(projectile, transform.position, transform.rotation);
        Vector2 directionFaced = direction.position - transform.position;
        spawned.AddForce(directionFaced * projectileSpeed);
    }
}

