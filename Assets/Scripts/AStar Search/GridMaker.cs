using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public GameObject enemies;

    public bool displayGridGizmos;
    public LayerMask unwalkable;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    Node[,] grid;
    LayerMask walkableMask;

    [SerializeField]private LevelGeneration levelGen;

    
    bool gridCreated;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        gridCreated = false;    
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);        
        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value
        }

    }

    private void Update()
    {
        if (!gridCreated && !levelGen.CanGenerate)
        {
            StartCoroutine(CreateGridWithDelay());
        }
      
    }    
    IEnumerator CreateGridWithDelay()
    {
        gridCreated = true;
        yield return new WaitForSeconds(0.5f);
        CreateGrid();
        yield return new WaitForSeconds(0.5f);
        SpawnEnemies(5);
    }

    void SpawnEnemies(int numberOfEnemies)
    {
        float actualWorldSizeX = gridWorldSize.x / 2;
        float actualWorldSizeY = gridWorldSize.y / 2;
        ;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(
                enemies, 
                    new Vector2(UnityEngine.Random.Range(-actualWorldSizeX, actualWorldSizeX), 
                    UnityEngine.Random.Range(-actualWorldSizeY, actualWorldSizeY)), 
                Quaternion.identity);
        }
    }


    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX 
                    && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
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

                int movementPenalty = 0;

                //raycast
                if (walkable)
                {
                    //Ray2D ray = new Ray2D
                }

                grid[i, j] = new Node(walkable, worldPoint, i, j, movementPenalty);                    
            }
        }
        gridCreated = true;        
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
        
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (grid != null && gridCreated && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                if (n != null)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;                    
                    Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter * 0.9f));
                }
            }
        }        
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
