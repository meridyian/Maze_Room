using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int sizeX, sizeZ;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;
    public IntVector2 size;
    public float generationStepDelay;


    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        // random coordinates to generate maze
        IntVector2 coordinates = RandomCoordinates;
        while (ContainsCoordinates(coordinates))
        {
            yield return delay;
            CreateCell(coordinates);
            coordinates.z += 1;
        }
    }
    
    // for maze to be able to create random coordinates 
    public IntVector2 RandomCoordinates
    {
        get {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    // whether the produced coordinates are inside of the maze
    
    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    private void CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "MazeCell" + coordinates.x + "," + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
    }
    
    
    
    
}
