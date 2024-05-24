using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MAX_PATH_LENGTH = 50;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private void Start()
    {
        FunctionPeriodic.Create(() =>
        {
            float startTime = Time.realtimeSinceStartup;

            for (int i = 0; i < 5; i++)
            {
                PathJob findPathJob = new PathJob
                {
                    startPosition = new int2(0, 0),
                    endPosition = new int2(19, 19)
                };
                findPathJob.Run();
            }

            Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));
        }, 1f);
    }

    public struct PathJob : IJob
    {
        public int2 startPosition;
        public int2 endPosition;

        public void Execute()
        {
            int2 gridSize = new int2(20, 20);
            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode
                    {
                        x = x,
                        y = y,
                        index = CalculateIndex(x, y, gridSize.x),
                        gCost = int.MaxValue,
                        hCost = CalculateDistanceCost(new int2(x, y), endPosition),
                        isWalkable = true,
                        cameFromNodeIndex = -1
                    };
                    pathNode.CalculateFCost();
                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            pathNodeArray[CalculateIndex(1, 0, gridSize.x)].SetIsWalkable(false);
            pathNodeArray[CalculateIndex(1, 1, gridSize.x)].SetIsWalkable(false);
            pathNodeArray[CalculateIndex(1, 2, gridSize.x)].SetIsWalkable(false);

            NativeArray<int2> neibhorsOffcetArray = new NativeArray<int2>(new int2[]
            {
                new int2(-1, 0), // Left
                new int2(+1, 0), // Right
                new int2(0, +1), // Up
                new int2(0, -1), // Down
                new int2(-1, -1), // Left Down
                new int2(-1, +1), // Left Up
                new int2(+1, -1), // Right Down
                new int2(+1, +1) // Right Up
            }, Allocator.Temp);

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    break;
                }

                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neibhorsOffcetArray.Length; i++)
                {
                    int2 neibhorOffcet = neibhorsOffcetArray[i];
                    int2 neibhorPosition = new int2(currentNode.x + neibhorOffcet.x, currentNode.y + neibhorOffcet.y);

                    if (!IsPositionInGrid(neibhorPosition, gridSize))
                    {
                        continue;
                    }

                    int neibhorNodeIndex = CalculateIndex(neibhorPosition.x, neibhorPosition.y, gridSize.x);
                    if (closedList.Contains(neibhorNodeIndex))
                    {
                        continue;
                    }

                    PathNode neibhorNode = pathNodeArray[neibhorNodeIndex];
                    if (!neibhorNode.isWalkable)
                    {
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neibhorPosition);

                    if (tentativeGCost < neibhorNode.gCost)
                    {
                        neibhorNode.cameFromNodeIndex = currentNodeIndex;
                        neibhorNode.gCost = tentativeGCost;
                        neibhorNode.CalculateFCost();
                        pathNodeArray[neibhorNodeIndex] = neibhorNode;
                        if (!openList.Contains(neibhorNode.index))
                        {
                            openList.Add(neibhorNode.index);
                        }
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                Debug.Log("Path not found");
            }
            else
            {
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                foreach (int2 pathPosition in path)
                {
                    Debug.Log(pathPosition);
                }
                path.Dispose();
            }

            pathNodeArray.Dispose();
            neibhorsOffcetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }

        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.cameFromNodeIndex == -1)
            {
                return new NativeList<int2>(Allocator.Temp);
            }
            else
            {
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));
                PathNode currentNode = endNode;

                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;
            }
        }

        private bool IsPositionInGrid(int2 gridPosition, int2 gridSize)
        {
            return gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < gridSize.x && gridPosition.y < gridSize.y;
        }

        private int GetLowestFCostNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];

            for (int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testPathNode;
                }
            }

            return lowestCostPathNode.index;
        }

        private struct PathNode
        {
            public int x;
            public int y;
            public int index;
            public int gCost;
            public int hCost;
            public int fCost;
            public bool isWalkable;
            public int cameFromNodeIndex;

            public void CalculateFCost()
            {
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable)
            {
                this.isWalkable = isWalkable;
            }
        }
    }
}