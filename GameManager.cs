using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool showGrid = true;
    public GameObject TextPrefab;
    public Block[,] theMap;
    public GameObject theMapGO;
    public List<GameObject> groundPrefab;
    public GameObject[,] mapBase;
    public GameObject Enemy;
    //public GameObject player;

    private float mouseX = -1;
    private float mouseY = -1;
    private GameObject[,] debugText;
    // Start is called before the first frame update
    void Start()
    {
        theMapGO = this.gameObject;
        Time.timeScale = 1f;
        //setUpMap(9,12);        
        visitMap();
        //Enemy.GetComponent<Enemy>().SetUpEnemy(player, theMap, new Vector3(2,6,0));
    }

    void Update() {
        UpdateMousePosition();
        if (Input.GetMouseButtonDown(0))
        {

           int x = (int)Mathf.Round(mouseX);
           int y = (int)Mathf.Round(mouseY);
           Enemy.GetComponent<tester>().goToDirection(x, y, theMap);
        }
        
        else if (Input.GetMouseButtonDown(1))
        {
            int x = (int)Mathf.Round(mouseX);
            int y = (int)Mathf.Round(mouseY);
            theMap[x, y].passable = !theMap[x, y].passable;
            if (debugText != null) {
                if (theMap[x, y].passable) debugText[x, y].SetActive(true);
                else debugText[x, y].SetActive(false);
            }
        }


    } 

    // visitMap(bool showGrid) check the position of each grid, and record 
    //      the position into the Block class and save them in theMap[,]
    //      if (showGrid = true), it will print the grid and number for each grid
    void visitMap() {
        theMap = new Block[12, 9];
        foreach (Transform child in theMapGO.transform) {
            Block b = child.GetComponent<Block>();
            // int x = (int) Mathf.Round(child.transform.position.x);
            // int y = (int) Mathf.Round(child.transform.position.y);
            int x = b.x;
            int y = b.y;
            theMap[x, y] = b; 
        }
        if (showGrid)
        {
            Vector3 OFFSET = new Vector3(-0.5f, -0.5f,0);
            debugText = new GameObject[12,9];
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 9; y++) 
                {
                    debugText[x, y] = Instantiate(TextPrefab, new Vector3(x, y, -0.01f), Quaternion.identity);
                    TextMesh txtMesh = debugText[x, y].GetComponent<TextMesh>();
                    txtMesh.text = "(" + x.ToString() + "," + y.ToString() + ")";
                    txtMesh.color = Color.white; // Set the text's color to red
                    if (!theMap[x, y].passable) debugText[x, y].SetActive(false);

                    Debug.DrawLine(new Vector3(x, y, -0.01f) + OFFSET, new Vector3(x + 1, y, -0.01f) + OFFSET, Color.white, 100f);
                    Debug.DrawLine(new Vector3(x, y, -0.01f) + OFFSET, new Vector3(x, y + 1, -0.01f) + OFFSET, Color.white, 100f);
                }
            }
            Debug.DrawLine(new Vector3(0, 9, -0.01f) + OFFSET, new Vector3(12, 9, -0.01f) + OFFSET, Color.white, 100f);
            Debug.DrawLine(new Vector3(12, 0, -0.01f) + OFFSET, new Vector3(12, 9, -0.01f) + OFFSET, Color.white, 100f);


        }
    }

    // UpdateMousePosition() updates current mouse position over time
    private void UpdateMousePosition()
    {

        // Construct a ray from the current mouse coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mouseX = mousePosition.x;
        mouseY = mousePosition.y;

    }

    //void setUpMap(int row, int col)
    //{
    //    mapBase = new GameObject[row, col];
    //    theMap = new Block[row, col];
    //    for (int i = 0; i < row; i++)
    //    {
    //        for (int j = 0; j < col; j++)
    //        {
    //            GameObject go = Instantiate(groundPrefab[0], new Vector3(i, j, 0), Quaternion.identity);
    //            go.transform.parent = GameObject.Find("testMap").transform;
    //            go.name = "(" + i + "," + j + ") " + "ground";
    //            mapBase[i, j] = go;

    //            theMap[i, j] = go.GetComponent<Block>();
    //            theMap[i, j].setPosition(i, j);
    //        }
    //    }
    //}
}
