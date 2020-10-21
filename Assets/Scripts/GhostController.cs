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
    
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        LevelGeneratorObj = GameObject.FindWithTag("levelGen").GetComponent<LevelGenerator>();
        spawnPoint = gameObject.transform.position;
        gridPos.y = gameObject.transform.position.x/1.25f;
        gridPos.x = Mathf.Abs(gameObject.transform.position.y/1.25f);
        lastMove = 0;
        delayAnim = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        //choose a direction
        //If this direction goes back to an old spot, then disregard
        //
        if(ghost.state != Ghost.GhostState.DEAD){
            switch(aiVariant){
                case 0:
                    //variant 1
                    break;
                case 1:
                    //variant 2
                    break;
                case 2:
                    updateGhost3();
                    break;
                case 3:
                    //variant 4
                    break;
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

    private void updateGhost1(){

    }

    private void updateGhost3(){
        if(!tweener.tweenActive()){    
            updateSurround();
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
