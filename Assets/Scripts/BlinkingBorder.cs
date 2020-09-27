using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingBorder : MonoBehaviour
{   
    private List< List<GameObject> > levelMapObjects = new List<List<GameObject>>();

    [SerializeField]
    private Color flashingCol;

    private bool isComplete;

    private float timer;

    [SerializeField]
    private float timeLimit;
    // Start is called before the first frame update
    public void StartFlashing()
    {
        levelMapObjects = gameObject.GetComponent<LevelGenerator>().getLevelMapObjects();
        for(int i = 0; i < levelMapObjects.Count; i++){
            for(int j = 0; j < levelMapObjects[i].Count; j++){
                if(i%2 == 0){
                    if(j%2 == 0){
                        levelMapObjects[i][j].GetComponent<SpriteRenderer>().color = flashingCol;
                    }
                }
                else if(i%2 == 1){
                    if(j%2 == 1){
                        levelMapObjects[i][j].GetComponent<SpriteRenderer>().color = flashingCol;
                    }
                }
            }
        }
        isComplete = true;    
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeLimit){  
            if(isComplete){
                for(int i = 0; i < levelMapObjects.Count; i++){
                    for(int j = 0; j < levelMapObjects[i].Count; j++){
                        if(levelMapObjects[i][j].GetComponent<SpriteRenderer>().color == new Color(1.0f, 1.0f, 1.0f)){
                            levelMapObjects[i][j].GetComponent<SpriteRenderer>().color = flashingCol;
                        }
                        else if(levelMapObjects[i][j].GetComponent<SpriteRenderer>().color == flashingCol){
                            levelMapObjects[i][j].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
                        }
                    }
                }
            }
            timer = 0;
        }
    }
}
