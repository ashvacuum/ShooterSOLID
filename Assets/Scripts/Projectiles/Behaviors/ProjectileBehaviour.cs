using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{    
    [SerializeField] protected int damage;
    
    [SerializeField] protected string deathParticleName;

    [SerializeField] private float projectileDeathTime;
    [SerializeField] private float particleDeathTime;



    private void OnEnable()
    {
        ObjectPooler.SharedInstance.ActivateDeathTimer(this.gameObject, projectileDeathTime);
    }

    private void OnDisable()
    {
        GameObject g = ObjectPooler.SharedInstance.GetPooledObject(deathParticleName);
        if (g != null)
        {
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            g.gameObject.SetActive(true);
            ObjectPooler.SharedInstance.ActivateDeathTimer(g, particleDeathTime);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamage>() != null)
        {
            collision.gameObject.GetComponent<IDamage>().ModifyHealth(-damage);            
        }        
        if (!collision.gameObject.GetComponent<ProjectileBehaviour>())
        {            
            ObjectPooler.SharedInstance.ActivateDeathTimer(this.gameObject);
        }

    }


}
