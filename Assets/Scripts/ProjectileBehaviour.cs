using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]private float launchSpeed = 3000f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 direction = transform.position;
        direction.y += 1f;
        rb.AddForce(direction * launchSpeed);
    }    
}
