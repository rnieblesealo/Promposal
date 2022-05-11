using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour{
    public enum TileState {Idle, NotInWord, InWord, InPlace};

    public string letter = string.Empty;
    public TileState state = TileState.Idle;
    [HideInInspector] public bool selected = false;
    
    [SerializeField] private Color mainColor;
    [SerializeField] private Color notInWordColor;
    [SerializeField] private Color inWordColor;
    [SerializeField] private Color inPlaceColor;

    [SerializeField] private float smoothTime;
    
    private Color currentColor;
    private float currentScale;

    private Image surface;
    private Text text;

    public int Check(string word, int index){
        //TODO optimize string case handling!
        if (word.ToUpper().Contains(letter.ToUpper())){
            if (word[index].ToString().ToUpper() == letter.ToUpper()){
                state = TileState.InPlace;
                return 1;
            }
            state = TileState.InWord;
            return 0;
        }
        state = TileState.NotInWord;
        return 0;
    }   

    private void Start(){
        surface = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
    }

    private void Update(){
        currentColor = state switch{
            TileState.Idle => mainColor,
            TileState.NotInWord => notInWordColor,
            TileState.InWord => inWordColor,
            TileState.InPlace => inPlaceColor,
            _ => mainColor
        };
        
        currentScale = selected ? 0.75f : 1;

        surface.color = Color.Lerp(surface.color, currentColor, smoothTime * Time.deltaTime);
        surface.transform.localScale = Vector3.Lerp(surface.rectTransform.localScale, new Vector3(currentScale, currentScale, 0), (smoothTime * 10) * Time.deltaTime);
        text.text = letter.ToUpper();
    }
}