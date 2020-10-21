using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostHandler : MonoBehaviour
{
    //0 up, 1 right, 2 down, 3 left
    [SerializeField]
    private List<GameObject> eyes = new List<GameObject>();

    [SerializeField]
    private List<Color> ghostColor = new List<Color>();
    [SerializeField]
    private GameObject ghostPrimitive;
    
    private List<Ghost> ghosts = new List<Ghost>();
    private float timer = 0;
    private bool timerActive;
    private int eyeNum = 0;

    [SerializeField]
    private GameObject ghostChaseSound;

    [SerializeField]
    private GameObject startingSound;
    [SerializeField]
    private List<Vector3> position;

    [SerializeField]
    private float scaredTimerLimit;
    [SerializeField]
    private AudioClip deadGhostRunningBack;
    [SerializeField]
    private AudioClip chasingGhost;
    public Canvas HUD;
    private Text ghostTimer;

    // Start is called before the first frame update
    void Awake()
    {
        //Create the starting noise, and the ghostchase sounds, and activate if on title.
        startingSound = Instantiate(startingSound, new Vector3(0, 0, 0), Quaternion.identity);
        ghostChaseSound = Instantiate(ghostChaseSound, new Vector3(0, 0, 0), Quaternion.identity);
        if(gameObject.tag != "title"){
            startingSound.GetComponent<AudioSource>().enabled = true;
            ghostChaseSound.GetComponent<AudioSource>().enabled = true; 
            ghostTimer = HUD.transform.GetChild(3).GetComponent<Text>();
            ghostTimer.enabled = false;
        }
        else{
            startingSound.GetComponent<AudioSource>().enabled = false;
            ghostChaseSound.GetComponent<AudioSource>().enabled = false; 
        }

        for(int i = 0; i < 4; i++){
            GameObject newGhostShape = Instantiate(ghostPrimitive, new Vector3(position[i].x, position[i].y, 0), Quaternion.identity);
            newGhostShape.GetComponent<SpriteRenderer>().color = ghostColor[i];
            Ghost newGhost = new Ghost(newGhostShape, eyes, (int)scaredTimerLimit, i);
            if(gameObject.tag == "title"){
                newGhostShape.GetComponent<GhostController>().enabled = false;
            }
            else{
                newGhostShape.GetComponent<GhostController>().ghost = newGhost;
                newGhostShape.GetComponent<GhostController>().aiVariant = i;
            }
            ghosts.Add(newGhost);
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if(!startingSound.GetComponent<AudioSource>().isPlaying && gameObject.tag != "title"){
            if(!ghostChaseSound.GetComponent<AudioSource>().isPlaying){
                for(int i = 0; i < ghosts.Count; i++){
                    if(ghosts[i].GetGhostState() == Ghost.GhostState.DEAD){
                        ghostChaseSound.GetComponent<AudioSource>().clip = deadGhostRunningBack;
                        ghostChaseSound.GetComponent<AudioSource>().enabled = true;
                        ghostChaseSound.GetComponent<AudioSource>().Play(0);
                        i = ghosts.Count;
                    }
                    else{
                        ghostChaseSound.GetComponent<AudioSource>().clip = chasingGhost;
                        ghostChaseSound.GetComponent<AudioSource>().enabled = true;
                        ghostChaseSound.GetComponent<AudioSource>().Play(0);
                    }
                }
            }
            for(int i = 0; i < ghosts.Count; i++){
                ghosts[i].ghostUpdate();
            }
        }
        if(timerActive){
            ghostTimer.enabled = true;
            timer += Time.deltaTime;
            int timeleft = (int)scaredTimerLimit - (int)timer;
            ghostTimer.text = "Ghost Timer: " + timeleft;
            if(timer > scaredTimerLimit){
                ghostTimer.enabled = false;
                timerActive = false;
                timer = 0.0f;
            }
        }
        // if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("ghostMoving")){
        //     timer += Time.deltaTime;
            
        //     for(int i = 0; i < ghostEyeState.Count; i++){
        //         ghostEyeState[i].SetActive(true);
        //     }
        //     if(timer >= 1.0f){

        //         eyeNum++;
        //         if(eyeNum >= 4){
        //             for(int i = 0; i < ghostAnimator.Count; i++){
        //                 ghostAnimator[i].SetBool("isScared", true);
        //             }
        //             eyeNum = 0;
        //         }
        //         float xPos = 2.0f;
        //         float yPos = 1.0f;
        //         for(int i = 0; i < ghostEyeState.Count; i++){
        //             Destroy(ghostEyeState[i]);
        //             ghostEyeState[i] = Instantiate(eyes[eyeNum], new Vector3(xPos, yPos, -1), Quaternion.identity);
        //             xPos += 2.0f;
        //         }
        //         timer = 0.0f;
        //     }
        // }
        // else if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("scaredGhost")){
        //     for(int i = 0; i < ghostEyeState.Count; i++){
        //         ghostEyeState[i].SetActive(false);
        //     }
        // }
        // else if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("recoveringGhost")){
        //     for(int i = 0; i < ghostEyeState.Count; i++){
        //         ghostEyeState[i].SetActive(false);
        //         ghostAnimator[i].SetBool("isDead", true);
        //         ghostAnimator[i].SetBool("isScared", false);
        //     }
        // }
        // else if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("deadState")){
        //     if(repeater >= 200){
        //         for(int i = 0; i < ghostAnimator.Count; i++){
        //             ghostAnimator[i].SetBool("isDead", false);
        //         }
        //         repeater = 0;
        //     }
        //     else{
        //         repeater++;
        //     }
        //     for(int i = 0; i < ghostEyeState.Count; i++){
        //         ghostEyeState[i].SetActive(true);
        //     }
        // }
    }

    public void setGhostState(string ghostState){
        for(int i = 0; i < ghosts.Count; i++){
            if(ghostState == "scared"){
                timerActive = true;
            }
            ghosts[i].setGhostState(ghostState);
        }
    }

    public List<Ghost> GetGhosts(){
        return ghosts;
    }
}
