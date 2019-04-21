using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;
    public float speed = 5;    
    public float turnSpeed = 3;
    public float turnDst = 5;

    Path path;

    float effectiveRange = 5f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        
    }

    public void StartPathing(Vector3 target)
    {
        PathRequestManager.RequestPath( new PathRequest(transform.position, target, OnPathFound));
    }


    public void OnPathFound(Vector2[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            
            StopCoroutine(FollowPath());
            path = new Path(waypoints, transform.position, turnDst, effectiveRange);
            StartCoroutine(FollowPath());
        }
    }
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }        

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

        Vector2 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - (Vector3)targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                StartPathing(target.position);
                targetPosOld = target.position;
            }
        }
    }


    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            while (path.turnBoundaries[pathIndex].HasCrossedLine(transform.position))
            {
                if(pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                } else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && effectiveRange > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(transform.position) / effectiveRange);
                    if(speedPercent < 0.01)
                    {
                        followingPath = false;
                    }
                }
                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - (Vector2)transform.position);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnDst);
                transform.position = Vector2.MoveTowards(transform.position, path.lookPoints[pathIndex], speed * Time.deltaTime* speedPercent);
                //transform.Translate((target.position - target.position) * speed * speedPercent * Time.deltaTime, Space.Self);                
            }
            
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
