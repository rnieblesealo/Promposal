using System.Collections;
using UnityEngine;
using TMPro;

public class BoxController : MonoBehaviour{
    #region Singleton
    public static BoxController instance;

    private void Awake() {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    #endregion
    
    public bool selected = false;
    public bool opened = false;
    public Coroutine slideshow;

    [SerializeField] private  GameObject boxText;
    [SerializeField] private float animTransitionDuration;
    [SerializeField] private string[] slides;
    
    private Animator anim;
    private int currentSlide = -1;
    private string currentAnimation;
    
    public IEnumerator Slideshow(){
        while (enabled){
            if (currentSlide >= slides.Length - 1){
                yield break;
            }
            
            currentSlide++;
           
            boxText.GetComponent<TextMeshPro>().text = slides[currentSlide];
            yield return new WaitForSeconds(2.35f);
        }
    }

    public void PlayAnimation(string name){
        if (name == currentAnimation){
            return;
        }
        
        anim.CrossFade(name, animTransitionDuration, 0);
        currentAnimation = name;
    }

    private void Start(){
        anim = GetComponent<Animator>();
    }

    private void Update(){
        boxText.SetActive(CameraController.instance.location == CameraController.CameraLocation.OnScreen);

        if (selected && CameraController.instance.location == CameraController.CameraLocation.OnRoom || opened){
            PlayAnimation("Open");
        }
        else{
            PlayAnimation("Close");
        }
    }
}
