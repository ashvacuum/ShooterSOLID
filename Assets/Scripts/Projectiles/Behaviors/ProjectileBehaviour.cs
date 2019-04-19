using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{    
    [SerializeField] protected int damage;
    [SerializeField] protected GameObject startParticle;
    [SerializeField] protected GameObject deathParticle;
        

    private void Start()
    {
        //spawn particle fire
        GameObject g = Instantiate(startParticle, transform.position, transform.rotation);
        Destroy(g, 1f);

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

    private void OnDestroy()
    {
        Debug.Log("Spawned a death particle");
    }

}
