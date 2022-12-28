using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class Maze : MonoBehaviour
{

    private List<MazeRoom> rooms = new List<MazeRoom>();
    public MazePassage passagePrefab;
    public int sizeX, sizeZ;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;
    public IntVector2 size;
    public float generationStepDelay;
    public MazeWall[] wallPrefabs;
    public MazeDoor doorPrefab;
    public MazeRoomSettings[] roomSettings;

    [Range(0f, 1f)] public float doorProbability;

    public MazeCell GetCell (IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }
    
    
    // each time when you generate a cell add it to list 
    // move one random step from last cell in the list, if you cannot remove current from activelist
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    
    }

    private void DoFirstGenerationStep (List<MazeCell> activeCells)
    {
        MazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
    }
    
    private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }
    

    
    // retrieve current cell, check whether the move is possible
    // remove cells whose all edges are initializedfrom list 
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        // since you are moving from last cell to a random direaction
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        
        // while you are inside of the maze 
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            //check if the current cell's neighbor doesnt exist yet if you dont have create a cell
            //since youc reated a cell create a passage among them
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currentCell.room == neighbor.room) {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            // if there is already a neighbour put a wall
            else
            {
                CreateWall(currentCell, neighbor, direction);
                // dont remove cells in here
            }
        }
        // if ypu are on  the maze boundaries create a wall
        else
        {
            CreateWall(currentCell, null, direction);
            // dont remove the cells here
        }
        
    }

    // put doors instead of the passages with some probability
    
    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        //since the passage will be on the same edge for both of them
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MazePassage;
        if (passage is MazeDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        // createwall's second cell wont exist at the edge of the maze
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
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

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "MazeCell" + coordinates.x + "," + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    private MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIndex == indexToExclude)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }

        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }
    
    
    
}
