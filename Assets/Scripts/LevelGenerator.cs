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
        for(int col = cornerlevelMap.GetLength(0) - 2; col > -1; col--){
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

        orientWall();
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

    void orientWall(){
        for(int row = 0; row < levelMapObjects.Count; row++){
            for(int col = 0; col < levelMapObjects[row].Count; col++){
                List< List<GameObject> > surround = checkSurround(row, col);
                switch(levelMapObjects[row][col].tag){
                    case "innercorner":
                        //Rotate innerCorner
                        levelMapObjects[row][col].transform.Rotate(0, 0, innerCornerOrient(surround));
                        break;
                    case "innerwall":
                        //Rotate inner wall
                        // if(row == 7 && col == 14){
                        //     for(int i = 0; i < surround.Count; i++){
                        //         for(int j = 0; j < surround[i].Count; j++){
                        //             Debug.Log("surround: " + i + " " + j + " type: " + surround[i][j].tag);
                        //         }
                        //     }
                        // }
                        if(surround[0][1].tag == "pellet" || surround [2][1].tag == "pellet" || surround[0][1].tag == "empty" || surround [2][1].tag == "empty"){
                            //pellet above means wall should be rotated to be facing parallel
                            levelMapObjects[row][col].transform.Rotate(0,0,90);
                        }
                        else if(surround[0][1].tag == "innercorner" || surround [2][1].tag == "innercorner" || surround[0][1].tag == "innerwall" || surround [2][1].tag == "innerwall"){
                            //Don't do anything
                        }
                        else if(surround[1][0].tag == "innercorner" || surround[1][0].tag == "innerwall"){
                            levelMapObjects[row][col].transform.Rotate(0,0,90);
                        }
                        else if(surround[1][2].tag == "innercorner" || surround[1][2].tag == "innerwall"){
                            levelMapObjects[row][col].transform.Rotate(0,0,90);
                        }
                        break;
                    case "outerwall":
                        if(surround[1][0] != null){
                            if(surround[1][0].tag == "outercorner" || surround[1][0].tag == "outerwall" || surround[1][0].tag == "tjunc"){
                                levelMapObjects[row][col].transform.Rotate(0,0,90);
                            }
                        }
                        else if(surround[1][2] != null ){    
                            if(surround[1][2].tag == "outercorner" || surround[1][2].tag == "outerwall" || surround[1][2].tag == "tjunc"){
                                levelMapObjects[row][col].transform.Rotate(0,0,90);
                            }
                        }
                        break;
                    case "tjunc":
                        if(surround[1][0].tag == "tjunc"){
                            levelMapObjects[row][col].transform.localScale += new Vector3(-2,0,0);
                        }
                        if(row == levelMapObjects.Count -1){
                            levelMapObjects[row][col].transform.localScale += new Vector3(0,-2,0);
                        }
                        break;
                    case "outercorner":
                        //Orient to the inside if it's a strict corner.
                        switch(isCorner(row, col)){
                            case "topRight":
                                levelMapObjects[row][col].transform.Rotate(0, 0, 0);
                                break;
                            case "topLeft":
                                levelMapObjects[row][col].transform.Rotate(0, 0, 270);
                                break;
                            case "bottomRight":
                                levelMapObjects[row][col].transform.Rotate(0, 0, 90);
                                break;
                            case "bottomLeft":
                                levelMapObjects[row][col].transform.Rotate(0, 0, 180);
                                break;
                            default:
                                //Not a strict corner
                                //Check the location of the walls around it, and connect to them. 
                                if(surround[0][1] != null && surround[1][2] != null){
                                    if(surround[0][1].tag == "outerwall" && surround[1][2].tag == "outerwall"){
                                        levelMapObjects[row][col].transform.Rotate(0, 0, 90);
                                    }
                                }
                                if(surround[1][2] != null && surround[2][1] != null){
                                    if(surround[1][2].tag == "outerwall" && surround[2][1].tag == "outerwall"){
                                        levelMapObjects[row][col].transform.Rotate(0, 0, 0);
                                    }
                                }
                                if(surround[1][0] != null && surround[2][1] != null){
                                    if(surround[2][1].tag == "outerwall" && surround[1][0].tag == "outerwall"){
                                        levelMapObjects[row][col].transform.Rotate(0, 0, 270);
                                    }
                                }
                                if(surround[1][0] != null && surround[0][1] != null){   
                                    if(surround[1][0].tag == "outerwall" && surround[0][1].tag == "outerwall"){
                                        levelMapObjects[row][col].transform.Rotate(0, 0, 180);
                                    }
                                }
                                break;
                        }
                        break;
                    default:
                        //do nothing, since it's either a pellet or a empty spot. No orientation needed.
                        break;
                }
            }
        }
    }
    string isCorner(int row, int col){
        if(row == 0 || row == levelMapObjects.Count - 1){
            if(col == 0 || col == levelMapObjects[0].Count - 1){
                if(row == 0 && col == 0){
                    return "topRight";
                }
                else if(row == 0 && col == levelMapObjects[0].Count - 1){
                    return "topLeft";
                }
                else if(row == levelMapObjects.Count - 1 && col == 0){
                    return "bottomRight";
                }
                else if(row == levelMapObjects.Count - 1 && col == levelMapObjects[0].Count - 1){
                    return "bottomLeft";
                }
            }
        }

        return "notacorner";
    }

    int innerCornerOrient(List<List<GameObject>> surround){
        if(surround[0][1].tag == "innerwall" && surround[1][2].tag == "innerwall"){
            if(surround[0][0].tag == "innerwall" && surround[0][2].tag == "innerwall"){
                //do nothing
            }
            else if(surround[0][2].tag == "innerwall" && surround[2][2].tag == "innerwall"){
                //do nothing, again
            }
            else{
                return 90;
            }
        }
        if(surround[1][2].tag == "innerwall" && surround[2][1].tag == "innerwall"){
            if(surround[2][0].tag == "innerwall" && surround[2][2].tag == "innerwall"){
                //do nothing
            }
            else if(surround[0][2].tag == "innerwall" && surround[2][2].tag == "innerwall"){
                //do nothing, again
            }
            else{
                return 0;
            }
        }
        if(surround[2][1].tag == "innerwall" && surround[1][0].tag == "innerwall"){
            if(surround[2][0].tag == "innerwall" && surround[2][2].tag == "innerwall"){
                //do nothing
            }
            else{
                return 270;
            }
        }
        if(surround[1][0].tag == "innerwall" && surround[0][1].tag == "innerwall"){
            return 180;
        }

        //Dealing with corners against corners
        if(surround[1][2].tag == "innercorner"){
            if(surround[2][1].tag == "pellet" || surround[2][1].tag == "empty"){
                return 90;
            }
        }
        else if(surround[1][0].tag == "innercorner"){
            if(surround[2][1].tag == "pellet" || surround[2][1].tag == "empty"){
                return 180;
            }
            else{
                return 270;
            }
        }
        else if(surround[2][1].tag == "innercorner"){
            if(surround[1][0].tag == "pellet" || surround[1][0].tag == "empty"){
                return 0;
            }
            else{
                return 270;
            }
        }
        else if(surround[0][1].tag == "innercorner"){
            if(surround[1][0].tag == "pellet" || surround[1][0].tag == "empty"){
                return 90;
            }
            else{
                return 180;
            }
        }


        //Checks failed/No rotation required, return no rotate.
        return 0;
    }

    List< List<GameObject> > checkSurround(int row, int col){
        List< List<GameObject> > result = new List<List<GameObject>>();
        List<GameObject> aRow = new List<GameObject>();
        
        if(row - 1 < 0 || col -1 < 0){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row -1][col -1]);
        }
        if(row - 1 < 0){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row -1][col]);
        }
        if(row - 1 < 0 || col + 1 > levelMapObjects[row].Count-1){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row -1][col + 1]);
        }
        result.Add(aRow);
        aRow = new List<GameObject>();
        
        //2nd Row
        if(col - 1 < 0){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row][col -1]);
        }
        aRow.Add(levelMapObjects[row][col]);
        if(col + 1 > levelMapObjects[row].Count -1){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row][col + 1]);
        }
        result.Add(aRow);
        aRow = new List<GameObject>();

        //3rd row
        if(row + 1 > levelMapObjects.Count - 1 || col -1 < 0){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row +1][col -1]);
        }
        if(row + 1 > levelMapObjects.Count - 1){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row +1][col]);
        }
        if(row + 1 > levelMapObjects.Count -1 || col + 1 > levelMapObjects[row].Count - 1){
            aRow.Add(null);
        }
        else{
            aRow.Add(levelMapObjects[row +1][col + 1]);
        }
        result.Add(aRow);
        
        return result; 
    }
}
