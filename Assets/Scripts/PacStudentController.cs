﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    private List<List<GameObject>> levelMapObjects;
    private List<List<GameObject>> surroundLMObjects;
    private LevelGenerator LevelGeneratorObj;
    private Vector2 gridPos = new Vector2(1, 1);
    private Vector2 oldGridPos = new Vector2(1, 1);
    public float delayAnim = 0.5f;
    private AudioSource movingAudio;
    public AudioClip eatingAudio;
    public AudioClip movingNoEating;
    
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        LevelGeneratorObj = GameObject.FindWithTag("levelGen").GetComponent<LevelGenerator>();
        levelMapObjects = LevelGeneratorObj.getLevelMapObjects();
        surroundLMObjects = LevelGeneratorObj.checkSurround((int)gridPos.x, (int)gridPos.y);
        movingAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        updateSurround();
        if(Input.GetKey("w")){
            
            if(!tweener.tweenActive()){
                // lastKey = 'w';
                transform.rotation = Quaternion.Euler(0, 0, 90);
                if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty"){
                    switchAudio(surroundLMObjects[0][1].tag);
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                        delayAnim);
                    gridPos += new Vector2(-1, 0);
                    
                }
            }
        }
        if(Input.GetKey("a")){
            
            if(!tweener.tweenActive()){
                //lastKey = 'a';
                transform.rotation = Quaternion.Euler(0, 0, 180);
                if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty"){
                    switchAudio(surroundLMObjects[1][0].tag);
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                        delayAnim);
                    gridPos += new Vector2(0, -1);
                }
            }
        }
        if(Input.GetKey("s")){
            
            if(!tweener.tweenActive()){
                //lastKey = 's';
                transform.rotation = Quaternion.Euler(0, 0, 270);
                if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty"){
                    switchAudio(surroundLMObjects[2][1].tag);
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                        delayAnim);
                    gridPos += new Vector2(1, 0);
                }
            }
        }
        if(Input.GetKey("d")){
            
            if(!tweener.tweenActive()){
                //lastKey = 'd';
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty"){
                    switchAudio(surroundLMObjects[1][2].tag);
                    tweener.AddTween(gameObject.transform, 
                        gameObject.transform.position, 
                        new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                        delayAnim);
                    gridPos += new Vector2(0, 1);
                }
            }
        }
    }

    private void switchAudio(string type){
        if(type == "pellet" || type == "powerpellet"){
            movingAudio.clip = eatingAudio;
        }
        else if(type == "empty"){
            movingAudio.clip = movingNoEating;
        }
        movingAudio.Play();
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
}   
