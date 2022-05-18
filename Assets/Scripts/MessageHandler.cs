using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/* coroutines in this script are managed by nullchecking.
 * use StopAndSetToNull in order to correctly stop a coroutine.
*/

public class MessageHandler : MonoBehaviour{
    [SerializeField] private int startDelay = 2;
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
    private Coroutine pause;

    IEnumerator Slideshow(){        
        while (enabled){
            if (currentSlide == slides.Length - 1){
                SetHidden(true);
                StopAndSetToNull(ref slideshow);
            }
            
            currentSlide++;
            
            if (slides[currentSlide] == "MOVE"){
                CameraController.instance.SetCurrentPos(CameraController.instance.onRoom);
                PauseSlides();
            }
            
            else{
                message.text = slides[currentSlide];
            }
            
            slideTimer = 0;
            nextSlideTimer = slides[currentSlide].Length > shortLength ? longDuration * slides[currentSlide].Length : shortDuration * slides[currentSlide].Length; //decide slide duration based on char count
            yield return new WaitForSeconds(nextSlideTimer);
        }
    }

    private void PauseSlides(float duration = 5f){
        if (slideshow == null || pause != null){
            return;
        }

        IEnumerator Pause(){
            yield return new WaitForSeconds(duration);
            SetHidden(false);
            slideshow = StartCoroutine(Slideshow());
            StopAndSetToNull(ref pause);
        }

        SetHidden(true);
        StopAndSetToNull(ref slideshow);
        pause = StartCoroutine(Pause());
    }

    private void SkipSlide(){
        if (slideshow == null || pause != null){ //disallow when paused as this dupes coroutine
            return;
        }
        
        StopAndSetToNull(ref slideshow);
        slideshow = StartCoroutine(Slideshow()); //skips slide by restarting coroutine if valid
    }

    private void SetHidden(bool hidden = true, bool instant = false){
        //set all graphics in children's opacity
        Graphic[] graphics = GetComponentsInChildren<Graphic>();
        int newOpacity = hidden ? 0 : 1;
        float duration = instant ? 0 : 0.75f;
        foreach (Graphic graphic in graphics){
            graphic.CrossFadeAlpha(newOpacity, duration, true);
        }
    }

    private void StopAndSetToNull(ref Coroutine coroutine){
        StopCoroutine(coroutine);
        coroutine = null;
    }

    private void Start(){        
        SetHidden(true, instant: true);
        
        //begin slideshow after short delay
        IEnumerator BeginSlideshow(){
            yield return new WaitForSeconds(startDelay);
            SetHidden(false);
            slideshow = StartCoroutine(Slideshow());
        }
        
        StartCoroutine(BeginSlideshow());
    }

    private void Update(){        
        if (slideshow != null){
            slideTimer += Time.deltaTime;
            progressBar.fillAmount = slideTimer / nextSlideTimer;
        }

        else{
            progressBar.fillAmount = 0;
        }

        //debug
        if (Input.GetKeyDown(KeyCode.Space)){
            SkipSlide();
        } 
    }
}
