using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost
{   
    private GameObject GhostShape;
    private List<GameObject> Eyes;
    private GameObject eye;
    private Text timer;
    public enum GhostState{
        ALIVE,
        SCARED,
        RECOVERING,
        DEAD
    };

    public GhostState state;
    public Animator ghostAnimator;

    public Ghost(GameObject ghost, List<GameObject> eyes){
        GhostShape = ghost;
        Eyes = eyes;
        state = GhostState.ALIVE;
        ghostAnimator = GhostShape.GetComponent<Animator>();
        eye = MonoBehaviour.Instantiate(Eyes[0], GhostShape.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        timer = GhostShape.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        GhostShape.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ghostUpdate(){
        animationUpdate();
    }

    public void animationUpdate(){
        if(state == GhostState.SCARED){
            eye.SetActive(false);
            GhostShape.transform.GetChild(0).gameObject.SetActive(true);
            ghostAnimator.SetBool("isScared", true);
        }
        if(state == GhostState.RECOVERING){
            eye.SetActive(false);
            GhostShape.transform.GetChild(0).gameObject.SetActive(true);
            ghostAnimator.SetBool("isRecovering", true);
        }
        if(state == GhostState.DEAD){
            eye.SetActive(true);
            GhostShape.transform.GetChild(0).gameObject.SetActive(false);
            ghostAnimator.SetBool("isDead", true);
        }
        if(state == GhostState.ALIVE){
            eye.SetActive(true);
            GhostShape.transform.GetChild(0).gameObject.SetActive(false);
            ghostAnimator.SetBool("isScared", false);
            ghostAnimator.SetBool("isRecovering", false);
            ghostAnimator.SetBool("isDead", false);
        }
    }

    public void updateHUD(float timeLeft){
        timer.text = "" + (int)timeLeft;
    }

    public GameObject getGhostShape(){
        return GhostShape;
    }

    public GameObject getGhostEyes(){
        return eye;
    }
    
    public void setEyeState(string eyeString){
        MonoBehaviour.Destroy(eye);
        if(eyeString == "up"){
            eye = MonoBehaviour.Instantiate(Eyes[0], GhostShape.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }
        else if(eyeString == "right"){
            eye = MonoBehaviour.Instantiate(Eyes[1], GhostShape.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }
        else if(eyeString == "down"){
            eye = MonoBehaviour.Instantiate(Eyes[2], GhostShape.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }
        else if(eyeString == "left"){
            eye = MonoBehaviour.Instantiate(Eyes[3], GhostShape.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }
    }

    public void setGhostState(string inpState){
        if(inpState == "alive"){
            state = GhostState.ALIVE;
        }
        else if(inpState == "scared"){
            state = GhostState.SCARED;
        }
        else if(inpState == "recovering"){
            state = GhostState.RECOVERING;
        }
        else if(inpState == "dead"){
            state = GhostState.DEAD;
        }
    }
}