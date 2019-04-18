using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInput : MonoBehaviour
{
    public float MoveSide { get; private set; }
    public float MoveUp { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        MoveUp = Input.GetAxisRaw("Vertical");
        MoveSide = Input.GetAxisRaw("Horizontal");
    }
}
