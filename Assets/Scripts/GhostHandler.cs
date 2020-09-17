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
    
    private List<GameObject> ghosts = new List<GameObject>();
    private List<GameObject> ghostEyeState = new List<GameObject>(); 

    private float timer = 0;
    private int eyeNum = 0;
    private List<Animator> ghostAnimator = new List<Animator>();
    // Start is called before the first frame update
    void Start()
    {
        float xPos = 2.0f;
        float yPos = 1.0f;
        for(int i = 0; i < 4; i++){
            GameObject newGhost = Instantiate(ghostPrimitive, new Vector3(xPos, yPos, 0), Quaternion.identity);
            GameObject newEyeState = Instantiate(eyes[0], new Vector3(xPos, yPos, -1), Quaternion.identity);
            newGhost.GetComponent<SpriteRenderer>().color = ghostColor[i];
            ghostEyeState.Add(newEyeState);
            ghosts.Add(newGhost);
            xPos += 2.0f;
            ghostAnimator.Add(ghosts[i].GetComponent<Animator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("ghostMoving")){
            timer += Time.deltaTime;
            
            for(int i = 0; i < ghostEyeState.Count; i++){
                ghostEyeState[i].SetActive(true);
            }
            if(timer >= 1.0f){

                eyeNum++;
                if(eyeNum >= 4){
                    for(int i = 0; i < ghostAnimator.Count; i++){
                        ghostAnimator[i].SetBool("isDead", true);
                    }
                    eyeNum = 0;
                }
                float xPos = 2.0f;
                float yPos = 1.0f;
                for(int i = 0; i < ghostEyeState.Count; i++){
                    Destroy(ghostEyeState[i]);
                    ghostEyeState[i] = Instantiate(eyes[eyeNum], new Vector3(xPos, yPos, -1), Quaternion.identity);
                    xPos += 2.0f;
                }
                timer = 0.0f;
            }
        }
        else if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("deadGhost")){
            for(int i = 0; i < ghostEyeState.Count; i++){
                ghostEyeState[i].SetActive(false);
            }
        }
        else if(ghostAnimator[0].GetCurrentAnimatorStateInfo(0).IsName("recoveringGhost")){
            for(int i = 0; i < ghostEyeState.Count; i++){
                ghostEyeState[i].SetActive(false);
                ghostAnimator[i].SetBool("isDead", false);
            }
        }
    }
}
