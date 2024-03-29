using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// maze wall and maze passage classes will extend so make it abstract
public abstract class MazeCellEdge : MonoBehaviour
{
    // cell that the edge is connected and the other cell it connects with
    // direction to keep orientation
    public MazeCell cell, otherCell;
    public MazeDirection direction;


    // edges will be children of their cells, placed in the same location
    // cell should know that an edge is created
    // if you dont rotate it all of the walls will be positioned at the same direction
    public virtual void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        this.cell = cell;
        this.otherCell = otherCell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition =Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
    
    
}
