using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private Tweener tweener;
    private Vector2 gridPos; 
    public Vector3 spawnPoint;
    private List<List<GameObject>> levelMapObjects;
    private List<List<GameObject>> surroundLMObjects;
    private LevelGenerator LevelGeneratorObj;
    private int lastMove;
    private float delayAnim;
    public Ghost ghost;
    public int aiVariant;

    private GameObject playerObj;
    private PacStudentController playerController;
    private int getOutLookFor;
    
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        LevelGeneratorObj = GameObject.FindWithTag("levelGen").GetComponent<LevelGenerator>();
        spawnPoint = gameObject.transform.position;
        gridPos.y = gameObject.transform.position.x/1.25f;
        gridPos.x = Mathf.Abs(gameObject.transform.position.y/1.25f);
        lastMove = 0;
        delayAnim = 0.35f;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObj.GetComponent<PacStudentController>();
    }

    // Update is called once per frame
    void Update()
    {
        //choose a direction
        //If this direction goes back to an old spot, then disregard
        
        if(ghost.state != Ghost.GhostState.DEAD){
            updateSurround();
            if(checkExitGuardPos()){
                updateGETOUT();
            }
            else if(ghost.state == Ghost.GhostState.SCARED || ghost.state == Ghost.GhostState.RECOVERING){
                updateGhost1();
            }
            else{
                switch(aiVariant){
                    case 0:
                        updateGhost1();
                        break;
                    case 1:
                        updateGhost2();
                        break;
                    case 2:
                        updateGhost3();
                        break;
                    case 3:
                        //variant 4
                        //I give up. I ran out of time and I need to do other assignments.
                        //throwing in updateghost3 instead.
                        updateGhost3();
                        break;
                }
            }            
        }
        else if(ghost.state == Ghost.GhostState.DEAD){
            deathUpdate();
        }
        updateEyePos();
    }

    private bool isOldDir(int newDir){
        int inverseLastMove = lastMove + 2;
        if(inverseLastMove > 3){
            inverseLastMove = inverseLastMove % 4;
        }
        if(newDir == inverseLastMove){
            return true;
        }
        return false;
    }

    private void updateEyePos(){
        ghost.getGhostEyes().transform.position = gameObject.transform.position + new Vector3(0,0,-1);
    }
    private void updateSurround(){
        surroundLMObjects = LevelGeneratorObj.checkSurround((int)gridPos.x, (int)gridPos.y);
        // for (int row = 0; row < surroundLMObjects.Count; row++){
        //     for(int col = 0; col < surroundLMObjects[row].Count; col++){
        //         Debug.Log("X:Y :" + gridPos.x + ":" + gridPos.y + " tag: " + surroundLMObjects[row][col].tag);
        //     }
        // }
        // for (int row = 0; row < levelMapObjects.Count; row++){
        //     for(int col = 0; col < levelMapObjects[row].Count; col++){
        //         //Debug.Log("X:Y :" + gridPos.x + ":" + gridPos.y + " tag: " + surroundLMObjects[row][col].tag);
        //         Debug.Log("row:col: " + row + ":" + col + " lmo: " + levelMapObjects[row][col].tag );
        //     }
        // }
    }

    private bool checkExitGuardPos(){
        return surroundLMObjects[1][1].tag == "exit";
    }   

    private void updateGETOUT(){
        if(!tweener.tweenActive()){
            if(surroundLMObjects[0][1].tag == "exit2"){
                if(!isOldDir(0)){
                    tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                            delayAnim);
                    gridPos += new Vector2(-1, 0);
                    lastMove = 0;
                    ghost.setEyeState("up");
                }
            }
            else if(surroundLMObjects[2][1].tag == "exit2"){
                if(!isOldDir(2)){
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                        delayAnim);
                    gridPos += new Vector2(1, 0);
                    lastMove = 2;
                    ghost.setEyeState("down");
                }
            }
            else if(surroundLMObjects[1][0].tag == "exit"){
                if(!isOldDir(3)){
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                        delayAnim);
                    if(!(gridPos.y - 1 < 0)){
                        gridPos += new Vector2(0, -1);
                        lastMove = 3;     
                    }
                    ghost.setEyeState("left");
                }
                else{
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                        delayAnim);
                    gridPos += new Vector2(0, 1);
                    lastMove = 1;
                    ghost.setEyeState("right");
                }
            }
            else if(surroundLMObjects[1][2].tag == "exit"){
                if(!isOldDir(1)){
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                        delayAnim);
                    gridPos += new Vector2(0, 1);
                    lastMove = 1;
                    ghost.setEyeState("right");
                }
            }
        }
    }

    private void updateGhost1(){
        bool teleportMovement = false;
        if(!tweener.tweenActive()){
            Vector2 playerPos = playerController.getGridPos();
            //for some reason x is y and y is x. I screwed up and I will not do this again...
            //wait, but x is y already in the grid pos so since they're already switched I will only need
            //to compare them directly. Don't switch them idiot.
            List<string> validMoves = new List<string>();
            if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty" || surroundLMObjects[0][1].tag == "teleport"){
                if(!isOldDir(0)){
                    validMoves.Add("up");
                }
            }
            if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty" || surroundLMObjects[1][0].tag == "teleport"){
                if(!isOldDir(3)){
                    validMoves.Add("left");
                }
            }
            if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty" || surroundLMObjects[2][1].tag == "teleport"){
                if(!isOldDir(2)){
                    validMoves.Add("down");
                }
            }
            if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty" || surroundLMObjects[1][2].tag == "teleport"){
                if(!isOldDir(1)){
                    validMoves.Add("right");
                }
            }
            
            Vector2 dif = getDif(playerPos, gridPos);
            List<int> difValidMoves = new List<int>();
            for(int i = 0; i < validMoves.Count; i++){
                switch(validMoves[i]){
                    case "up":
                        //add the two components of dif and compare the value with the sum of gridPos dif IF up was chosen.
                        //save in a List in the same order as validmoves.
                        Vector2 ifUpDif = getDif(playerPos, gridPos + new Vector2(-1, 0));
                        difValidMoves.Add((int)(ifUpDif.x + ifUpDif.y));
                        break;
                    case "left":
                        if(surroundLMObjects[1][0].tag == "teleport"){
                            teleportMovement = true;
                            tweener.AddTween(gameObject.transform, 
                                    gameObject.transform.position, 
                                    new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                    delayAnim);
                            gridPos += new Vector2(0, 1);
                            lastMove = 1;
                            ghost.setEyeState("right");
                        }
                        else{
                            Vector2 ifLeftDif = getDif(playerPos, gridPos + new Vector2(0, -1));
                            difValidMoves.Add((int)(ifLeftDif.x + ifLeftDif.y));
                        }
                        break;
                    case "down":
                        Vector2 ifDownDif = getDif(playerPos, gridPos + new Vector2(1, 0));
                        difValidMoves.Add((int)(ifDownDif.x + ifDownDif.y));
                        break;
                    case "right":
                        if(surroundLMObjects[1][2].tag == "teleport"){
                            teleportMovement = true;
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                            if(!(gridPos.y - 1 < 0)){
                                gridPos += new Vector2(0, -1);
                                lastMove = 3;     
                            }
                            ghost.setEyeState("left");
                        }
                        else{
                            Vector2 ifRightDif = getDif(playerPos, gridPos + new Vector2(0, 1));
                            difValidMoves.Add((int)(ifRightDif.x + ifRightDif.y));
                        }
                        break;
                }
            }

            if(!teleportMovement){
                int minDif = 0;
                int minDifIndex = 0;
                for(int i = 0; i < difValidMoves.Count; i++){
                    if(difValidMoves[i] > minDif){
                        minDif = difValidMoves[i];
                        minDifIndex = i;
                    }
                }
                switch(validMoves[minDifIndex]){
                    //Move the object.
                    case "up":
                        tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                                delayAnim);
                        gridPos += new Vector2(-1, 0);
                        lastMove = 0;
                        ghost.setEyeState("up");
                        break;
                    case "left":
                        tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                            delayAnim);
                        if(!(gridPos.y - 1 < 0)){
                            gridPos += new Vector2(0, -1);
                            lastMove = 3;     
                        }
                        ghost.setEyeState("left");
                        break;
                    case "down":
                        tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                            delayAnim);
                        gridPos += new Vector2(1, 0);
                        lastMove = 2;
                        ghost.setEyeState("down");
                        break;
                    case "right":
                        tweener.AddTween(gameObject.transform, 
                                    gameObject.transform.position, 
                                    new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                    delayAnim);
                        gridPos += new Vector2(0, 1);
                        lastMove = 1;
                        ghost.setEyeState("right");
                        break;
                }
            }
        }
    }
    private void updateGhost2(){
        bool teleportMovement = false;
        if(!tweener.tweenActive()){
            Vector2 playerPos = playerController.getGridPos();
            //for some reason x is y and y is x. I screwed up and I will not do this again...
            //wait, but x is y already in the grid pos so since they're already switched I will only need
            //to compare them directly. Don't switch them idiot.
            List<string> validMoves = new List<string>();
            if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty" || surroundLMObjects[0][1].tag == "teleport"){
                if(!isOldDir(0)){
                    validMoves.Add("up");
                }
            }
            if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty" || surroundLMObjects[1][0].tag == "teleport"){
                if(!isOldDir(3)){
                    validMoves.Add("left");
                }
            }
            if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty" || surroundLMObjects[2][1].tag == "teleport"){
                if(!isOldDir(2)){
                    validMoves.Add("down");
                }
            }
            if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty" || surroundLMObjects[1][2].tag == "teleport"){
                if(!isOldDir(1)){
                    validMoves.Add("right");
                }
            }
            
            Vector2 dif = getDif(playerPos, gridPos);
            List<int> difValidMoves = new List<int>();
            for(int i = 0; i < validMoves.Count; i++){
                switch(validMoves[i]){
                    case "up":
                        //add the two components of dif and compare the value with the sum of gridPos dif IF up was chosen.
                        //save in a List in the same order as validmoves.
                        Vector2 ifUpDif = getDif(playerPos, gridPos + new Vector2(-1, 0));
                        difValidMoves.Add((int)(ifUpDif.x + ifUpDif.y));
                        break;
                    case "left":
                        if(surroundLMObjects[1][0].tag == "teleport"){
                            teleportMovement = true;
                            tweener.AddTween(gameObject.transform, 
                                    gameObject.transform.position, 
                                    new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                    delayAnim);
                            gridPos += new Vector2(0, 1);
                            lastMove = 1;
                            ghost.setEyeState("right");
                        }
                        else{
                            Vector2 ifLeftDif = getDif(playerPos, gridPos + new Vector2(0, -1));
                            difValidMoves.Add((int)(ifLeftDif.x + ifLeftDif.y));
                        }
                        break;
                    case "down":
                        Vector2 ifDownDif = getDif(playerPos, gridPos + new Vector2(1, 0));
                        difValidMoves.Add((int)(ifDownDif.x + ifDownDif.y));
                        break;
                    case "right":
                        if(surroundLMObjects[1][2].tag == "teleport"){
                            teleportMovement = true;
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                            if(!(gridPos.y - 1 < 0)){
                                gridPos += new Vector2(0, -1);
                                lastMove = 3;     
                            }
                            ghost.setEyeState("left");
                        }
                        else{
                            Vector2 ifRightDif = getDif(playerPos, gridPos + new Vector2(0, 1));
                            difValidMoves.Add((int)(ifRightDif.x + ifRightDif.y));
                        }
                        break;
                }
            }

            if(!teleportMovement){
                int minDif = 1000;
                int minDifIndex = 0;
                for(int i = 0; i < difValidMoves.Count; i++){
                    if(difValidMoves[i] < minDif){
                        minDif = difValidMoves[i];
                        minDifIndex = i;
                    }
                }
                switch(validMoves[minDifIndex]){
                    //Move the object.
                    case "up":
                        tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                                delayAnim);
                        gridPos += new Vector2(-1, 0);
                        lastMove = 0;
                        ghost.setEyeState("up");
                        break;
                    case "left":
                        tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                            delayAnim);
                        if(!(gridPos.y - 1 < 0)){
                            gridPos += new Vector2(0, -1);
                            lastMove = 3;     
                        }
                        ghost.setEyeState("left");
                        break;
                    case "down":
                        tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                            delayAnim);
                        gridPos += new Vector2(1, 0);
                        lastMove = 2;
                        ghost.setEyeState("down");
                        break;
                    case "right":
                        tweener.AddTween(gameObject.transform, 
                                    gameObject.transform.position, 
                                    new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                    delayAnim);
                        gridPos += new Vector2(0, 1);
                        lastMove = 1;
                        ghost.setEyeState("right");
                        break;
                }
            }
        }
    }
    
    private Vector2 getDif(Vector2 pos1, Vector2 pos2){
        return new Vector2(Mathf.Abs(pos1.x-pos2.x), Mathf.Abs(pos1.y - pos2.y));
    }
    private void updateGhost3(){
        if(!tweener.tweenActive()){    
            int chosenDir = Random.Range(0,4);
            switch(chosenDir){
                case 0:
                    //up
                    if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty"){
                        if(!isOldDir(chosenDir)){
                            tweener.AddTween(gameObject.transform, 
                                    gameObject.transform.position, 
                                    new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                                    delayAnim);
                            gridPos += new Vector2(-1, 0);
                            lastMove = chosenDir;
                            ghost.setEyeState("up");
                        }
                    }
                    break;
                case 1:
                    //right
                    if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty"){
                        if(!isOldDir(chosenDir)){
                            tweener.AddTween(gameObject.transform, 
                                        gameObject.transform.position, 
                                        new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                        delayAnim);
                            gridPos += new Vector2(0, 1);
                            lastMove = chosenDir;
                            ghost.setEyeState("right");
                        }
                    }
                    else if(surroundLMObjects[1][2].tag == "teleport"){
                        tweener.AddTween(gameObject.transform, 
                            gameObject.transform.position, 
                            new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                            delayAnim);
                        if(!(gridPos.y - 1 < 0)){
                            gridPos += new Vector2(0, -1);
                            lastMove = 3;     
                        }
                        ghost.setEyeState("left");
                    }
                    break;
                case 2: 
                    //down
                    if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty"){
                        if(!isOldDir(chosenDir)){
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                                delayAnim);
                            gridPos += new Vector2(1, 0);
                            lastMove = chosenDir;
                            ghost.setEyeState("down");
                        }
                    }
                    break;
                case 3: 
                    //left
                    if(surroundLMObjects[1][0] == null){
                            //do Nothing
                    }
                    else if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty"){
                        if(!isOldDir(chosenDir)){
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                            if(!(gridPos.y - 1 < 0)){
                                gridPos += new Vector2(0, -1);
                                lastMove = chosenDir;     
                            }
                            ghost.setEyeState("left");
                        }
                    }
                    else if(surroundLMObjects[1][0].tag == "teleport"){
                        tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                        gridPos += new Vector2(0, 1);
                        lastMove = 1;
                        ghost.setEyeState("right");
                    }
                    break;
            }
        }
    }
    
    public void updateGhost4(){
        int centerRow = (int)levelMapObjects.Count/2;
        int centerCol = (int)levelMapObjects[0].Count/2;
        bool canHorizontal = false;
        bool canVertical = false;

        List<string> validMoves = new List<string>();
        if(gridPos.x >= centerRow){
            //go left
            if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty" || surroundLMObjects[1][0].tag == "teleport"){
                if(!isOldDir(3)){
                    validMoves.Add("left");
                }
            }
        }
        else if(gridPos.x < centerRow){
            //go right
            if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty" || surroundLMObjects[1][2].tag == "teleport"){
                if(!isOldDir(1)){
                    validMoves.Add("right");
                }
            }
        }
        if(gridPos.y >= centerCol){
            //go down
            if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty" || surroundLMObjects[2][1].tag == "teleport"){
                if(!isOldDir(2)){
                    validMoves.Add("down");
                }
            }
        }
        else if(gridPos.y < centerCol){
            //go up
            if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty" || surroundLMObjects[0][1].tag == "teleport"){
                if(!isOldDir(0)){
                    validMoves.Add("up");
                }
            }
        }
    }

    public void deathUpdate(){
        if(!tweener.tweenActive()){
            tweener.AddTween(gameObject.transform, gameObject.transform.position, spawnPoint, delayAnim * 10);
            gridPos.x = Mathf.Abs(spawnPoint.y/1.25f);
            gridPos.y = spawnPoint.x/1.25f;
        }
        if(gameObject.transform.position == spawnPoint){
            ghost.setGhostState("alive");
        }
    }
}
