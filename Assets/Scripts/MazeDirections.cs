using System.Collections;
using System.Collections.Generic;using System.Diagnostics;
using OpenCover.Framework.Model;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public static class  MazeDirections 
{
    // for the methods in mazedirection
    // ThreadWaitReason are 4 directions
    public const int Count = 4;

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }
    
    // a methıd to convert a direction into integer vector
    private static IntVector2[] vectors =
    {
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
        new IntVector2(-1, 0)
    };

    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    private static MazeDirection[] opposites =
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
        
    }
}
