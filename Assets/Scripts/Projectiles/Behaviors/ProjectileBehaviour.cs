using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{    
    [SerializeField] protected int damage;
    
    [SerializeField] protected GameObject deathParticle;

    [SerializeField] private float projectileDeathTime;
        

    private void Start()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(projectileDeathTime);
        Destroy(gameObject);
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamage>() != null)
        {
            collision.gameObject.GetComponent<IDamage>().ModifyHealth(-damage);
            
        }
        if (deathParticle != null)
        {
            GameObject g = Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(g, 1f);
        }
        if (!collision.gameObject.GetComponent<ProjectileBehaviour>())
        {
            Destroy(gameObject);
        }
        
    }


}
