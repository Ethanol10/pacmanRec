using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){
            if(!tweener.tweenActive()){
                // lastKey = 'w';
                tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                                0.7f);
            }
        }
        if(Input.GetKey("a")){
            if(!tweener.tweenActive()){
                //lastKey = 'a';
                tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                                0.7f);
            }
        }
        if(Input.GetKey("s")){
            if(!tweener.tweenActive()){
                //lastKey = 's';
                tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                                0.7f);
            }
        }
        if(Input.GetKey("d")){
            if(!tweener.tweenActive()){
                //lastKey = 'd';
                tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                0.7f);
            }
        }
    }
}
