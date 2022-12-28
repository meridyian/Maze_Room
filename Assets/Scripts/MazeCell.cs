using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeCell : MonoBehaviour
{

    public IntVector2 coordinates;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
        
    }

    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    
    // randomly decide how many uninitialized directions we would skip.
    // loop through edges array and whenever we find a hole check whether out of skips
    // if so this is the direction. 
    // decrease amount of skips by one
    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }

                    skips -= 1;
                    
                }
            }
            // if there is no edge left 
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }


}
