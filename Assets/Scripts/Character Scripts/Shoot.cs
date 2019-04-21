using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private string projectileTag;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] private Transform direction;
    [SerializeField] protected float FireRate;
    float lastFireRate;
    // Start is called before the first frame update
    void Start()
    {
        CharInput.Shoot += Fire;
        lastFireRate = 0;
    }   

    void Fire()
    {
        if (Time.time - lastFireRate > FireRate)
        {
            Vector2 face = direction.position - transform.position;            
            Rigidbody2D bullet = ObjectPooler.SharedInstance.GetPooledObject(projectileTag).GetComponent<Rigidbody2D>();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.gameObject.SetActive(true);
            }

            bullet.AddForce(face * projectileSpeed);
            lastFireRate = Time.time;
        }
    }

}

