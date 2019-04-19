using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{    
    [SerializeField] protected int damage;
    
    [SerializeField] protected GameObject deathParticle;
        

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<IDamage>() != null)
        {
            collision.gameObject.GetComponent<IDamage>().ModifyHealth(-damage);
        }
        GameObject g = Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(g, 1f);
        Destroy(gameObject);
    }
}
