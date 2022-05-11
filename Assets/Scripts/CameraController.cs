using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform title;
    public Transform onBox;
    public float smoothTime;
    
    private Vector3 currentPos;
    private Vector3 currentRot;

    void Start(){
        SetCurrentPos(title);
    }

    void Update(){
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPos, smoothTime * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(currentRot), smoothTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SetCurrentPos(title);
        }
    
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SetCurrentPos(onBox);
        }
    }

    void SetCurrentPos(Transform newPos){        
        currentPos = newPos.position;
        currentRot = newPos.rotation.eulerAngles;
    }
}