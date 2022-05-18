using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MessageHandler : MonoBehaviour{
    [SerializeField] private string[] slides;
    [SerializeField] private List<string> slideTimes;

    [SerializeField] private Text message;
    [SerializeField] private Image progressBar;

    private int shortLength = 12;
    private int currentSlide = -1;
    private float slideTimer = 0;
    private float nextSlideTimer = 0;
    private float shortDuration = .25f;
    private float longDuration = .05f;
    private Coroutine slideshow;

    IEnumerator Slideshow(){        
        while (enabled){
            if (currentSlide == slides.Length - 1){
                yield break; //auto cancel coroutine if slide index exceeds slide elements
            }
            
            currentSlide++;
            
            if (slides[currentSlide] == "MOVE"){
                CameraController.instance.SetCurrentPos(CameraController.instance.onRoom);
                continue;
            }
            
            else{
                message.text = slides[currentSlide];
            }
            
            slideTimer = 0;
            nextSlideTimer = slides[currentSlide].Length > shortLength ? longDuration * slides[currentSlide].Length : shortDuration * slides[currentSlide].Length; //decide slide duration based on char count
            yield return new WaitForSeconds(nextSlideTimer);
        }
    }

    private void SkipSlide(){
        if (slideshow == null){
            return;
        }
        
        StopCoroutine(slideshow);
        slideshow = StartCoroutine(Slideshow()); //skips slide by restarting coroutine if valid
    }

    private void Start(){        
        slideshow = StartCoroutine(Slideshow());
    }

    private void Update(){
        slideTimer += Time.deltaTime;
        progressBar.fillAmount = slideTimer / nextSlideTimer;

        //debug
        if (Input.GetKeyDown(KeyCode.Space)){
            SkipSlide();
        } 
    }
}
