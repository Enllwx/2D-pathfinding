using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public PathNode(int x, int y, bool isWalkable = true)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    public void FCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
