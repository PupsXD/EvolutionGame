using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PathfindingParams : IComponentData
{
    public int2 startPosition;
    public int2 endPosition;
    
}