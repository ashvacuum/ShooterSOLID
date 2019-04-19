using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField]private LayerMask whatRoom;
    [SerializeField]private LevelGeneration levelGen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatRoom);
        if(roomDetection == null && !levelGen.CanGenerate)
        {
            int rand = Random.Range(0, levelGen.Rooms.Length);
            Instantiate(levelGen.Rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
