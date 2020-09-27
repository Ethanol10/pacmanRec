using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost
{   
    private GameObject GhostShape;
    private GameObject Eyes;

    public Ghost(GameObject ghost, GameObject eyes){
        GhostShape = ghost;
        Eyes = eyes;
    }

    public GameObject getGhostShape(){
        return GhostShape;
    }

    public GameObject getGhostEyes(){
        return Eyes;
    }
}