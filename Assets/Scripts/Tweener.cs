using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tweener : MonoBehaviour
{
    private List<Tween> activeTweens = new List<Tween>();

    void Start(){

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
        if(Vector3.Distance(activeTweens[0].Target.position, activeTweens[0].EndPos) > 0.1f){
            // float x = (currentTime - activeTweens[0].StartTime)/activeTweens[0].Duration;
            // float timeFraction = 1 - Mathf.Cos((x * Mathf.PI) / 2);
            float timeFraction = (currentTime - activeTweens[0].StartTime)/activeTweens[0].Duration;
            activeTweens[0].Target.transform.position = Vector3.Lerp(activeTweens[0].StartPos, activeTweens[0].EndPos, timeFraction);          
        }
        else if(Vector3.Distance(activeTweens[0].Target.position, activeTweens[0].EndPos) <= 0.1f){
            Debug.Log(activeTweens[0].Target);
            activeTweens[0].Target.position = activeTweens[0].EndPos;
            activeTweens.RemoveAt(0);
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration, float waitMod){
        // if(!TweenExists(targetObject)){
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time + waitMod, duration));
            return true;
        // }
        // return false;
    }

    public bool TweenExists(Transform target){
        for(int i = 0; i < activeTweens.Count; i++){
            if(activeTweens[i].Target == target){
                return true;
            }
        }
        return false;
    }

    public int noOfActiveTweens(){
        return activeTweens.Count;
    }
}
