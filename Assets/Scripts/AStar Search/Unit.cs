using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    Vector2[] path;

    int targetIndex;
    float effectiveRange = 5f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            StopCoroutine(FollowPath());
            path = newPath;
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
    }
}
