using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInput : MonoBehaviour
{
    public float MoveSide { get; private set; }
    public float MoveUp { get; private set; }
    public bool FireWeapons { get; private set; }

    //delegate
    public delegate void OnFire();

    //event
    public static event OnFire Shoot;
    
    // Update is called once per frame
    void Update()
    {
        MoveUp = Input.GetAxisRaw("Vertical");
        MoveSide = Input.GetAxisRaw("Horizontal");
        FireWeapons = Input.GetButtonDown("Fire1");
        if (FireWeapons)
            Shoot();
            
    }
    
}
