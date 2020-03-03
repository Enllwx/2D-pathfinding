using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    private Rigidbody2D rb;
    private PathFinding pf;

    //private int tarX;
    //private int tarY;
    private Vector3 target;
    private Vector3 direction;
    private List<Vector3> path;

    // Start is called before the first frame update
    void Start()
    {
        path = new List<Vector3>();
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        pf = this.gameObject.GetComponent<PathFinding>();
    }

    void FixedUpdate()
    {
        if (path != null && path.Count != 0) {
            float posX = this.transform.position.x;
            float posY = this.transform.position.y;
            float tarX = target.x;
            float tarY = target.y;
            target = path[0];
            direction = target - this.transform.position;
            direction.Normalize();

            if (withinTarget(posX, posY,tarX,tarY, 0.05f)) {
                path.Remove(target);

                if (path == null || path.Count == 0)
                {
                    direction = Vector3.zero;
                    path.Clear();
                    return;
                }
                else {
                    target = path[0];
                    direction = target - this.transform.position;
                    direction.Normalize();
                }
               
            }
            Debug.Log(target);
            Debug.Log(direction);

            moveCharacter(direction);
        }
    }
    
    bool withinTarget(float posX, float posY,
                      float tarX, float tarY, float t) {
        if (Mathf.Abs(posX - tarX) + Mathf.Abs(posY - tarY) <= t) { return true; }

        return false;
    }


    // moveCharacter() physicially change the position of charactor to the player position
    void moveCharacter(Vector3 direction)
    {
        Vector3 newPosition = this.transform.position + (direction * 1f * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    //public void goToDirection(int tarX, int tarY, Block[,] theMap) {
    //    pf.theMap = theMap;
    //    pf.setPathFind(currX, currY, tarX, tarY, 12, 9);
    //    path = pf.FindPath();
    //    if (path == null || path.Count == 0) { return; }
    //    target = path[0];
    //    direction = target - this.transform.position;
    //    direction.Normalize();
    //    this.tarX = tarX;
    //    this.tarY = tarY;
    //}

    public void goToDirection(int tarX, int tarY, Block[,] theMap)
    {
        pf.theMap = theMap;
        int currX = (int)Mathf.Round(this.transform.position.x);
        int currY = (int)Mathf.Round(this.transform.position.y);
        pf.setPathFind(currX, currY, tarX, tarY, 12, 9);
        path = pf.FindPath();
    }
}
