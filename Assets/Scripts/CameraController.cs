using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake() {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    
    public Transform onRoom;
    public Transform onBox;
    public Transform onSky;
    public float smoothTime;
    
    private Vector3 currentPos;
    private Vector3 currentRot;

    void Start(){
        SetCurrentPos(onSky, instantly: true);
    }

    void Update(){
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPos, smoothTime * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(currentRot), smoothTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SetCurrentPos(onRoom);
        }
    
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SetCurrentPos(onBox);
        }
    }

    public void SetCurrentPos(Transform newPos, bool instantly = false){        
        currentPos = newPos.position;
        currentRot = newPos.rotation.eulerAngles;

        if (instantly){
            gameObject.transform.position = newPos.position;
            gameObject.transform.rotation = newPos.rotation;
        }
    }
}