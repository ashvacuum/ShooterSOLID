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
    public int obstacleProximityPenalty = 10;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    [SerializeField]private LevelGeneration levelGen;

    
    bool gridCreated;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    private void Start()
    {
        gridCreated = false;    
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        MakeNewTerraintTypeDictionary();
    }

    private void Update()
    {
        if (!gridCreated && !levelGen.CanGenerate)
        {
            StartCoroutine(CreateGridWithDelay());
        }
      
    }    

    void MakeNewTerraintTypeDictionary()
    {
        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
    }

    IEnumerator CreateGridWithDelay()
    {
        gridCreated = true;        
        yield return new WaitForSeconds(0.5f);
        CreateGrid();
        yield return new WaitForSeconds(0.5f);
        //SpawnEnemies(5);
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

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for(int y= 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX -1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x,sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesHorizontalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if(blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if(blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
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
                
                Ray ray = new Ray((Vector3)worldPoint + Vector3.forward * 50, Vector3.back);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }
                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[i, j] = new Node(walkable, worldPoint, i, j, movementPenalty);                    
            }
        }
        BlurPenaltyMap(3);
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
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                    Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;                    
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
