using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private float timer;
    
    [SerializeField]
    private float timeLimit;
    [SerializeField]
    private float driftSpeed;

    [SerializeField]
    private List<Vector2> rightPos;
    [SerializeField]
    private List<Vector2> leftPos;
    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!tweener.tweenActive()){
            timer += Time.deltaTime;
        }
        if(timer >= timeLimit){
            tweener.AddTween(gameObject.transform, 
                        leftPos[(int)Random.Range(0,3)], 
                        rightPos[(int)Random.Range(0,3)], 
                        driftSpeed);
            timer = 0.0f;
        }
    }
}
