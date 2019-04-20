using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    
    public LayerMask unwalkable;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    [SerializeField]private LevelGeneration levelGen;

    List<Action> functionsToRunInMainThread;
    bool gridCreated;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        gridCreated = false;
        functionsToRunInMainThread = new List<Action>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);        
    }

    private void Update()
    {
        if (!gridCreated && !levelGen.CanGenerate)
        {
            CreateGrid();
        }
      
    }

    

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = Vector2.zero - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeRadius) + Vector2.up * (j * nodeDiameter + nodeRadius);          
                bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkable) == null);                
                grid[i, j] = new Node(walkable, worldPoint);                    
            }
        }
        gridCreated = true;
        
    }

    public void QueueMainThreadFunction(Action someFunction)
    {
        functionsToRunInMainThread.Add(someFunction);
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        if(grid != null && gridCreated)
        {            
            foreach( Node n in grid)
            {
                if (n != null)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    
                    Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter * 0.9f));
                } 
            }
        }
    }
}
