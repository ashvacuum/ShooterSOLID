using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]private Transform[] startingPos;
    [SerializeField]private GameObject[] rooms;
    [SerializeField]private float moveAmount;
    [SerializeField] private float startTime = 0.25f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;    

    private float timeBetweenRoom;
    private int direction;
    private bool canGenerate;
    
    private void Start()
    {
        int randPos = Random.Range(0, startingPos.Length);
        transform.position = startingPos[randPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);
        canGenerate = true;
        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (canGenerate)
        {
            if (timeBetweenRoom <= 0)
            {
                Motion();
                timeBetweenRoom = startTime;
            }
            else
            {
                timeBetweenRoom -= Time.deltaTime;
            }
        }
    }

    private void Motion()
    {
        if(direction == 1 || direction == 2)//move right
        {
            if (transform.position.x > maxX)
                transform.position = ChangePosition(direction, moveAmount);
            else
                direction = 5;
        } else if(direction == 3 || direction == 4)
        {
            if (transform.position.x > minX)
                transform.position = transform.position = ChangePosition(direction, -moveAmount);
            else
                direction = 5;
        } else if(direction == 5)
        {
            if (transform.position.y > minY)
                transform.position = transform.position = ChangePosition(direction, moveAmount);
            else
                canGenerate = false;
        }
        Instantiate(rooms[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);    
    }

    Vector2 ChangePosition(int direction, float move)
    {
        Vector2 newPos = Vector2.zero;
        if (direction == 5)        
            newPos = new Vector2(transform.position.x, transform.position.y - move);        
        else        
            newPos = new Vector2(transform.position.x + move, transform.position.y);
        return newPos;
    }
}


