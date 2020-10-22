using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private List<List<GameObject>> levelMapObjects;
    private List<GameObject> teleporters;
    private List<Vector2> teleporterPosition;
    // Start is called before the first frame update
    void Start()
    {
        levelMapObjects = GetComponent<LevelGenerator>().getLevelMapObjects();
        teleporters = new List<GameObject>();
        teleporterPosition = new List<Vector2>();
        Debug.Log(levelMapObjects.Count);
        Debug.Log(levelMapObjects[0].Count);
        for(int i = 0; i < levelMapObjects.Count; i++){
            for(int j = 0; j < levelMapObjects[i].Count; j++){
                if(levelMapObjects[i][j].tag == "teleport"){
                    teleporters.Add(levelMapObjects[i][j]);
                    teleporterPosition.Add(new Vector2(i, j));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 swapPosition(Transform objectToMove){
        Debug.Log(objectToMove.position);
        Debug.Log(teleporters[1].transform.position);
        if(objectToMove.position.x == teleporters[0].transform.position.x + 1.25){
            objectToMove.position = teleporters[1].transform.position;
            return teleporterPosition[1];
        }
        else if(objectToMove.position.x == teleporters[1].transform.position.x - 1.25){
            objectToMove.position = teleporters[0].transform.position;
            return teleporterPosition[0];
        }
        return new Vector2(0, 0);
    }
}
