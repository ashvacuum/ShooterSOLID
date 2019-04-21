using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    

    PathRequest currentPathRequest;
    PathFinding pathfinding;

    
    
    static PathRequestManager instance;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(PathRequest request)
    {

    }    

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
     
    }

    
}

struct PathRequest
{
    public Vector2 pathStart;
    public Vector2 pathEnd;
    public Action<Vector2[], bool> callback;

    public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> _callback)
    {
        pathStart = start;
        pathEnd = end;
        callback = _callback;
    }
}
