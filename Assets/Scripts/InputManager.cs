using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {   
        tweener = GetComponent<Tweener>();
        tweener.AddTween(player.transform, player.transform.position, new Vector3(1.25f, -1.25f, 0.0f), 2.0f);
        tweener.AddTween(player.transform, player.transform.position, new Vector3(7.5f, -1.25f, 0.0f), 2.0f);
        tweener.AddTween(player.transform, player.transform.position, new Vector3(7.5f, -6.25f, 0.0f), 2.0f);
        tweener.AddTween(player.transform, player.transform.position, new Vector3(1.25f, -6.25f, 0.0f), 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tweener.noOfActiveTweens());
        if(tweener.noOfActiveTweens() == 0){
            tweener.AddTween(player.transform, player.transform.position, new Vector3(1.25f, -1.25f, 0.0f), 2.0f);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(7.5f, -1.25f, 0.0f), 2.0f);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(7.5f, -6.25f, 0.0f), 2.0f);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(1.25f, -6.25f, 0.0f), 2.0f);
        }
    }
}
