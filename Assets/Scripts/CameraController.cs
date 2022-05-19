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
    
    public enum CameraLocation {OnSky, OnRoom, OnBox, OnScreen};
    public CameraLocation location;

    public Transform onRoom;
    public Transform onBox;
    public Transform onSky;
    public Transform onScreen;
    public float smoothTime;
    
    private Vector3 currentPos;
    private Vector3 currentRot;

    void Start(){
        SetCurrentPos(CameraLocation.OnSky, instantly: true);
    }

    void Update(){
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPos, smoothTime * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(currentRot), smoothTime * Time.deltaTime);

        //gee!
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 500)) {
            if (hit.transform == BoxController.instance.transform){
                BoxController.instance.selected = true;
                if (Input.GetMouseButtonDown(0)){
                    SetCurrentPos(CameraLocation.OnBox);
                    Wordle.instance.StartGame(0);
                }
            }
            else{
                BoxController.instance.selected = false;
            }
        }
        else{
            BoxController.instance.selected = false;
        }
    }

    public void SetCurrentPos(CameraLocation newLocation, bool instantly = false){        
        location = newLocation;
        switch (newLocation){
            case CameraLocation.OnSky:
                currentPos = onSky.position;
                currentRot = onSky.rotation.eulerAngles;
                break;
            case CameraLocation.OnRoom:
                currentPos = onRoom.position;
                currentRot = onRoom.rotation.eulerAngles;
                break;
            case CameraLocation.OnBox:
                currentPos = onBox.position;
                currentRot = onBox.rotation.eulerAngles;
                break;
            case CameraLocation.OnScreen:
                currentPos = onScreen.position;
                currentRot = onScreen.rotation.eulerAngles;
                break;
        }
        
        if (instantly){
            gameObject.transform.position = currentPos;
            gameObject.transform.rotation = Quaternion.Euler(currentRot);
        }
    }
}