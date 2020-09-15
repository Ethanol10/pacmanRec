using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Tweener tweener;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {   
        tweener = GetComponent<Tweener>();
        tweener.AddTween(player.transform, new Vector3(1.25f, -1.25f, 0.0f), new Vector3(7.5f, -1.25f, 0.0f), 2.0f, 0.0f);
        tweener.AddTween(player.transform, new Vector3(7.5f, -1.25f, 0.0f), new Vector3(7.5f, -6.25f, 0.0f), 2.0f, 2.0f);
        tweener.AddTween(player.transform, new Vector3(7.5f, -6.25f, 0.0f), new Vector3(1.25f, -6.25f, 0.0f), 2.0f, 4.0f);
        tweener.AddTween(player.transform, new Vector3(1.25f, -6.25f, 0.0f), new Vector3(1.25f, -1.25f, 0.0f), 2.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;

        if(tweener.noOfActiveTweens() == 0){
            tweener.AddTween(player.transform, new Vector3(1.25f, -1.25f, 0.0f), new Vector3(7.5f, -1.25f, 0.0f), 2.0f, 0.0f);
            tweener.AddTween(player.transform, new Vector3(7.5f, -1.25f, 0.0f), new Vector3(7.5f, -6.25f, 0.0f), 2.0f, 2.0f);
            tweener.AddTween(player.transform,  new Vector3(7.5f, -6.25f, 0.0f), new Vector3(1.25f, -6.25f, 0.0f), 2.0f, 4.0f);
            tweener.AddTween(player.transform, new Vector3(1.25f, -6.25f, 0.0f), new Vector3(1.25f, -1.25f, 0.0f), 2.0f, 6.0f);
        }
        if(timer >= 2.0f){
            player.transform.Rotate(0.0f, 0.0f, -90.0f);
            timer = 0.0f;
        }

    }
}
