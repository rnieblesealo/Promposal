using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour{
    public enum TileState {Idle, InWord, InPlace};

    public string letter = string.Empty;
    public TileState state = TileState.Idle;
    
    [SerializeField] Color mainColor;
    [SerializeField] Color inWordColor;
    [SerializeField] Color inPlaceColor;

    [SerializeField] private float smoothTime;
    
    private Color currentColor;

    private Image surface;
    private Text text;

    public void Check(string word, int index){
        //optimize string case handling
        if (word.ToUpper().Contains(letter.ToUpper())){
            if (word[index].ToString().ToUpper() == letter.ToUpper())
                state = TileState.InPlace;
            else
                state = TileState.InWord;
        }
    }   

    private void Start(){
        surface = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
    }

    private void Update(){
        currentColor = state switch{
            TileState.Idle => mainColor,
            TileState.InWord => inWordColor,
            TileState.InPlace => inPlaceColor,
            _ => mainColor
        };
        
        surface.color = Color.Lerp(surface.color, currentColor, smoothTime * Time.deltaTime);
        text.text = letter.ToUpper();
    }
}