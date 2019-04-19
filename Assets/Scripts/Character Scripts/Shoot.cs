using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] private Transform direction;
    [SerializeField] protected GameObject startParticle;
    // Start is called before the first frame update
    void Start()
    {
        CharInput.Shoot += Fire;
    }   
    void Fire()
    {
        GameObject g = Instantiate(startParticle, transform.position, transform.rotation);
        Destroy(g, 1f);
        Vector2 face = direction.position - transform.position;
        Rigidbody2D rb = Instantiate(projectile, transform.position, transform.rotation);
        rb.AddForce(face * projectileSpeed);
    }
}

