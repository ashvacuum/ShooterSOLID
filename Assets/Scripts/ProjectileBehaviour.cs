using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]private float launchSpeed = 300f;    

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * launchSpeed);
    }
}
