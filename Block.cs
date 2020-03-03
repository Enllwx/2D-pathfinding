using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType {ROCK, WATER, GRASS, GROUND};
public class Block : MonoBehaviour
{   
    public GroundType groundType;
   
    public int x;  
    public int y;

    public bool passable;
}
