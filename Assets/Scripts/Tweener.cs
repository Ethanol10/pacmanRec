using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tweener : MonoBehaviour
{
    private Tween activeTween;

    void Start(){
        activeTween = null;
    }

    void Update(){
        float currentTime = Time.time;

        // for(int i = 0; i < activeTweens.Count; i++){     
        //     Debug.Log(i);
        //     if(Vector3.Distance(activeTweens[i].Target.transform.position, activeTweens[i].EndPos) > 0.1f){
        //         float x = (currentTime - activeTweens[i].StartTime)/activeTweens[i].Duration;
        //         float timeFraction = 1 - Mathf.Cos((x * Mathf.PI) / 2);
        //         activeTweens[i].Target.transform.position = Vector3.Lerp(activeTweens[i].StartPos, activeTweens[i].EndPos, timeFraction);
        //     }
        //     else if(Vector3.Distance(activeTweens[i].Target.transform.position, activeTweens[i].EndPos) <= 0.1f){
        //         activeTweens[i].Target.transform.position = activeTweens[i].EndPos;
        //         activeTweens.RemoveAt(i);
        //     }
        // }
        if(activeTween != null){     
            if(Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.01f){
                //float x = (currentTime - activeTween.StartTime)/activeTween.Duration;
                //float timeFraction = 1 - Mathf.Cos((x * Mathf.PI) / 2);
                float timeFraction = (currentTime - activeTween.StartTime)/activeTween.Duration;
                activeTween.Target.transform.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);          
            }
            else if(Vector3.Distance(activeTween.Target.position, activeTween.EndPos) <= 0.01f){
                //Debug.Log(activeTweens[0].Target);
                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration){
        // if(!TweenExists(targetObject)){
        activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
        return true;
        // }
        // return false;
    }

    //public bool TweenExists(Transform target){
        // for(int i = 0; i < activeTweens.Count; i++){
        //     if(activeTweens[i].Target == target){
        //         return true;
        //     }
        // }
        // return false;
    //}

    public void destroyTween(){
        activeTween = null;
    }

    public bool tweenActive(){
        return (activeTween != null);
    }
}
