using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int eyeNum = 0;

    [SerializeField]
    private GameObject ghostChaseSound;

    [SerializeField]
    private GameObject startingSound;
    [SerializeField]
    private List<Vector3> position;

    private int repeater = 0;
    // Start is called before the first frame update
    void Start()
    {
        startingSound = Instantiate(startingSound, new Vector3(0, 0, 0), Quaternion.identity);
        ghostChaseSound = Instantiate(ghostChaseSound, new Vector3(0, 0, 0), Quaternion.identity);
        if(gameObject.tag != "title"){
            startingSound.GetComponent<AudioSource>().enabled = true;
            ghostChaseSound.GetComponent<AudioSource>().enabled = true; 
        }
        else{
            startingSound.GetComponent<AudioSource>().enabled = false;
            ghostChaseSound.GetComponent<AudioSource>().enabled = false; 
        }

        for(int i = 0; i < 4; i++){
            GameObject newGhostShape = Instantiate(ghostPrimitive, new Vector3(position[i].x, position[i].y, 0), Quaternion.identity);
            GameObject newEyeState = Instantiate(eyes[0], new Vector3(position[i].x, position[i].y, -0.1f), Quaternion.identity);
            newGhostShape.GetComponent<SpriteRenderer>().color = ghostColor[i];
            Ghost newGhost = new Ghost(newGhostShape, newEyeState);
            ghosts.Add(newGhost);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(!startingSound.GetComponent<AudioSource>().isPlaying && gameObject.tag != "title"){
            if(!ghostChaseSound.GetComponent<AudioSource>().isPlaying){
                ghostChaseSound.GetComponent<AudioSource>().enabled = true;
                ghostChaseSound.GetComponent<AudioSource>().Play(0);
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
}
