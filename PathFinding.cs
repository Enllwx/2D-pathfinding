using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Block[,] theMap;

    private int mapX = 12;
    private int mapY = 9;

    private int startX;
    private int startY;

    private int targetX;
    private int targetY;

    private PathNode[,] theGrid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public void setPathFind(int currX, int currY, int tarX, int tarY, 
                            int maxX = 12, int maxY = 9) {
        mapX = maxX;
        mapY = maxY;
        startX = currX;
        startY = currY;
        targetX = tarX;
        targetY = tarY;
    }

    public List<Vector3> FindPath() {
        // the target position is not workable
        if (!theMap[targetX, targetY].passable) {
            return null;
        } 
        // if no move needed
        else if (startX == targetX && startY == targetY) {
            return null;
        }

        theGrid = new PathNode[mapX, mapY];
        for (int x = 0; x < mapX; x++) {
            for (int y = 0; y < mapY; y++) {
                theGrid[x, y] = new PathNode(x, y, theMap[x, y].passable);
                theGrid[x, y].cameFromNode = null;
                theGrid[x, y].gCost = 99999999;
                theGrid[x, y].FCost();
            }
        }

        PathNode startNode = theGrid[startX, startY];
        PathNode endNode = theGrid[targetX, targetY];
        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.FCost();

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        while (openList.Count > 0) {
            PathNode currentNode = FindMinFCostNode(openList);
            // if we reached the target node
            if (currentNode == endNode) {
                return FormPath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode node in FindNeighbourNode(currentNode)) {
                if (!node.isWalkable) {
                    closedList.Add(node);
                    continue;
                }

                int currGCost = currentNode.gCost + CalculateHCost(currentNode, node);
                if (currGCost < node.gCost) {
                    node.cameFromNode = currentNode;
                    node.gCost = currGCost;
                    node.hCost = CalculateHCost(node, endNode);
                    node.FCost();

                    if (!openList.Contains(node)) openList.Add(node);
                }
            }
        }

        // when openList is empty; therefore, no path found
        return null;
    }

    private int CalculateHCost(PathNode s, PathNode e) {
        int xDistance = Mathf.Abs(s.x - e.x);
        int yDistance = Mathf.Abs(s.y - e.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) 
            + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode FindMinFCostNode(List<PathNode> list) {
        if (list.Count == 0) return null;

        PathNode minFCostNode = list[0];

        for (int i = 1; i < list.Count; i++) {
            if (minFCostNode.fCost > list[i].fCost) {
                minFCostNode = list[i];
            }
        }
        return minFCostNode;
    }

    private List<PathNode> FindNeighbourNode(PathNode node)
    {
        int X = node.x;
        int Y = node.y;
        List<PathNode> Neighbourhood = new List<PathNode>();
        if (IsValidPosition(X, Y, X +1, Y+1)) Neighbourhood.Add(theGrid[X + 1, Y + 1]);
        if (IsValidPosition(X, Y, X +1, Y-1)) Neighbourhood.Add(theGrid[X + 1, Y - 1]);
        if (IsValidPosition(X, Y, X +1, Y)) Neighbourhood.Add(theGrid[X + 1, Y]);
        if (IsValidPosition(X, Y, X -1, Y+1)) Neighbourhood.Add(theGrid[X - 1, Y + 1]);
        if (IsValidPosition(X, Y, X -1, Y-1)) Neighbourhood.Add(theGrid[X - 1, Y - 1]);
        if (IsValidPosition(X, Y, X -1, Y)) Neighbourhood.Add(theGrid[X - 1, Y]);
        if (IsValidPosition(X, Y, X, Y+1)) Neighbourhood.Add(theGrid[X, Y + 1]);
        if (IsValidPosition(X, Y, X, Y-1)) Neighbourhood.Add(theGrid[X, Y - 1]);
        return Neighbourhood;
    }

    private bool IsValidPosition(int currX, int currY, int tarX, int tarY) {
        if (tarX < 0 || tarX >= mapX || tarY < 0 || tarY >= mapY) return false;

        if (closedList.Contains(theGrid[tarX, tarY])) return false;

        // test if can go diagonally
        int Xdiff = tarX - currX;
        int Ydiff = tarY - currY;
        if (Mathf.Abs(Xdiff) + Mathf.Abs(Ydiff) == 2) {
            if (!theGrid[currX + Xdiff, currY].isWalkable
                || !theGrid[currX, currY + Ydiff].isWalkable) return false;
        }
        return true;
    }

    private List<Vector3> FormPath(PathNode end) {
        PathNode node = end;
        List<Vector3> path = new List<Vector3> { new Vector3(node.x, node.y, 0) };
        while (node.cameFromNode != null) {
            node = node.cameFromNode;
            path.Add(new Vector3(node.x, node.y, 0));
        }
        path.Reverse();
        return path;
    }
}
