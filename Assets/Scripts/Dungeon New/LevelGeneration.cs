using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]private Transform[] startingPos;
    [SerializeField]private GameObject[] rooms;// index 0 --> LR, 1 -->LRD, 2--> ULR, 3 -> ULRD
    [SerializeField]private float moveAmount;
    [SerializeField] private float startTime = 0.25f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;    

    public GameObject[] Rooms
    {
        get { return rooms; }
    }

    private float timeBetweenRoom;
    private int direction;
    private bool canGenerate;

    public bool CanGenerate { get { return canGenerate; } }

    [SerializeField]private LayerMask room;

    private int downCounter;
    
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
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                transform.position = ChangePosition(direction, moveAmount);


                RoomRandomize(0, rooms.Length);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        } else if(direction == 3 || direction == 4)
        {
            downCounter = 0;
            if (transform.position.x > minX)
            {
                transform.position = ChangePosition(direction, -moveAmount);
                RoomRandomize(0, rooms.Length);

                direction = Random.Range(3, 6);                
            }
            else
            {
                direction = 5;
            }
        } else if(direction == 5)
        {
            downCounter++;
            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomTypes>().type != 1 && roomDetection.GetComponent<RoomTypes>().type != 3 && roomDetection != null)
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomTypes>().RoomDestroy();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomTypes>().RoomDestroy();
                        int randRoom = Random.Range(1, 4);
                        if (randRoom == 2)
                        {
                            randRoom = 1;
                        }
                        Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                    }
                }

                transform.position = transform.position = ChangePosition(direction, moveAmount);                

                RoomRandomize(2, 4);

                direction = Random.Range(1, 6);
            }
            else
            {
                canGenerate = false;
            }
        }
            
    }

    void RoomRandomize(int start, int end)
    {
        int rand = Random.Range(0, rooms.Length);
        Instantiate(rooms[rand], transform.position, Quaternion.identity);
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


