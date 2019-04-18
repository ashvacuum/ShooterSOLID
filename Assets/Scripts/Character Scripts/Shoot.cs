using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;    

    // Start is called before the first frame update
    void Start()
    {
        CharInput.Shoot += Fire;
    }   

    void Fire()
    {
        Instantiate(projectile, transform.position, transform.rotation);        
    }
}
