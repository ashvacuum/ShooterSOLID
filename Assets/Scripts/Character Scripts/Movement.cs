using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;

    private CharInput input;

    void Awake()
    {
        input = GetComponent<CharInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = transform.position;        
        newPos.x += input.MoveSide * moveSpeed * Time.deltaTime;

        newPos.y += input.MoveUp * moveSpeed * Time.deltaTime;
        transform.position = newPos;
    }
}
