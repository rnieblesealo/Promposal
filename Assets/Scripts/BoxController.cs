using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour{
    [SerializeField] private float animTransitionDuration;
    
    private Animator anim;
    
    private void PlayAnimation(string name){
        anim.CrossFade(name, animTransitionDuration, 0);
    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Start(){
        PlayAnimation("Close");
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayAnimation("Close");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayAnimation("Open");
    }
}
