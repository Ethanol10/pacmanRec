using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int[,] cornerlevelMap =
    {
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
    {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
    {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
    {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
    {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
    {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
    {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    }; 
    
    private List< List<GameObject> > levelMapObjects = new List<List<GameObject>>();

    [SerializeField]
    private GameObject[] wallObjectList;

    // Start is called before the first frame update
    void Start()
    {
        float xPos = 0;
        float yPos = 0;

        for(int col = 0; col < cornerlevelMap.GetLength(0); col++){
            List<GameObject> sublist = new List<GameObject>();
            for(int row = 0; row < cornerlevelMap.GetLength(1); row++){
                InstantiateWallObject(col, row, xPos, yPos, ref sublist);
                xPos += 1.25f;
            }
            for(int row = cornerlevelMap.GetLength(1) - 1; row > -1; row-- ){
                InstantiateWallObject(col, row, xPos, yPos, ref sublist);
                xPos += 1.25f;
            }
            levelMapObjects.Add(sublist);
            xPos = 0;
            yPos -= 1.25f;
        }
        for(int col = cornerlevelMap.GetLength(0) - 1; col > -1; col--){
            List<GameObject> sublist = new List<GameObject>();
            for(int row = 0; row < cornerlevelMap.GetLength(1); row++){
                InstantiateWallObject(col, row, xPos, yPos, ref sublist);
                xPos += 1.25f;
            }
            for(int row = cornerlevelMap.GetLength(1) - 1; row > -1; row-- ){
                InstantiateWallObject(col, row, xPos, yPos, ref sublist);
                xPos += 1.25f;
            }
            levelMapObjects.Add(sublist);
            xPos = 0;
            yPos -= 1.25f;
        }
        yPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InstantiateWallObject(int col, int row, float xPos, float yPos, ref List<GameObject> sublist){
        switch(cornerlevelMap[col,row]){
            case 0:
                sublist.Add(Instantiate(wallObjectList[0], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 1:
                sublist.Add(Instantiate(wallObjectList[1], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 2:
                sublist.Add(Instantiate(wallObjectList[2], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 3:
                sublist.Add(Instantiate(wallObjectList[3], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 4:
                sublist.Add(Instantiate(wallObjectList[4], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 5:
                sublist.Add(Instantiate(wallObjectList[5], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 6:
                sublist.Add(Instantiate(wallObjectList[6], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;
            case 7:
                sublist.Add(Instantiate(wallObjectList[7], new Vector3(xPos, yPos, 0), Quaternion.identity));
                break;    
        }
    }
}
