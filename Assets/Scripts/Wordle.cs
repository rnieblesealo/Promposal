using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wordle : MonoBehaviour{
    #region Singleton
    public static Wordle instance;
    private void Awake() {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject tile;
    public GameObject row;
    
    public string[] words;
    public string[] hints;
    public int guesses;

    [SerializeField] private int level = 0;
    
    private int usedGuesses = 0;
    private int selectedTile = 0; //which tile are we going to type on relative to the current row?
    private bool won = false;
    private List<List<Tile>> tiles = new List<List<Tile>>();
    [SerializeField] private Text levelText;
    [SerializeField] private Text hintText;

    public void StartGame(int newLevel = 0){
        if (newLevel > words.Length - 1){
            //move to box
            EndGame();
            Debug.LogWarning("Wordle: Final level has been reached!");
            return;
        }
        
        won = false;
        level = newLevel;
        usedGuesses = 0;
        selectedTile = 0;
        
        Clear();
        //bro this is some o(n^3) shit but at this point...
        for (int i = 0; i < guesses; ++i){
            Transform newRow = Instantiate(row, gameObject.transform).transform;
            List<Tile> newTiles = new List<Tile>();
            for (int j = 0; j < words[level].Length; ++j){
                Tile newTile = Instantiate(tile, newRow).GetComponent<Tile>();
                newTiles.Add(newTile);

                //make tile transparent at start
                foreach (Graphic graphic in newTile.GetComponentsInChildren<Graphic>()){
                    graphic.CrossFadeAlpha(0, 0, true);
                }
            }
            tiles.Add(newTiles);
        }
        
        SetHidden(false);
        levelText.text = "Level " + (level + 1).ToString() + "/" + (words.Length).ToString();
        hintText.text = "Hint: " + hints[level];
    }

    public void Clear(){
        //clears all rows & tiles
        for (int i = 0; i < tiles.Count; ++i){
            Destroy(tiles[i][0].transform.parent.gameObject);
        }
        
        tiles.RemoveRange(0, tiles.Count);
    }

    private char GetInputChar(){
        if (!Input.anyKey)
            return '\0';
        
        if (Input.GetKeyDown(KeyCode.A))
            return 'A';
        else if (Input.GetKeyDown(KeyCode.B))
            return 'B';
        else if (Input.GetKeyDown(KeyCode.C))
            return 'C';
        else if (Input.GetKeyDown(KeyCode.D))
            return 'D';
        else if (Input.GetKeyDown(KeyCode.E))
            return 'E';
        else if (Input.GetKeyDown(KeyCode.F))
            return 'F';
        else if (Input.GetKeyDown(KeyCode.G))
            return 'G';
        else if (Input.GetKeyDown(KeyCode.H))
            return 'H';
        else if (Input.GetKeyDown(KeyCode.I))
            return 'I';
        else if (Input.GetKeyDown(KeyCode.J))
            return 'J';
        else if (Input.GetKeyDown(KeyCode.K))
            return 'K';
        else if (Input.GetKeyDown(KeyCode.L))
            return 'L';
        else if (Input.GetKeyDown(KeyCode.M))
            return 'M';
        else if (Input.GetKeyDown(KeyCode.N))
            return 'N';
        else if (Input.GetKeyDown(KeyCode.O))
            return 'O';
        else if (Input.GetKeyDown(KeyCode.P))
            return 'P';
        else if (Input.GetKeyDown(KeyCode.Q))
            return 'Q';
        else if (Input.GetKeyDown(KeyCode.R))
            return 'R';
        else if (Input.GetKeyDown(KeyCode.S))
            return 'S';
        else if (Input.GetKeyDown(KeyCode.T))
            return 'T';
        else if (Input.GetKeyDown(KeyCode.U))
            return 'U';
        else if (Input.GetKeyDown(KeyCode.V))
            return 'V';
        else if (Input.GetKeyDown(KeyCode.W))
            return 'W';
        else if (Input.GetKeyDown(KeyCode.X))
            return 'X';
        else if (Input.GetKeyDown(KeyCode.Y))
            return 'Y';
        else if (Input.GetKeyDown(KeyCode.Z)){
            return 'Z';
        }
        else{
            return '\0';
        }
    }

    private void EndGame(){
        //reset game
        Clear();
        
        won = false;
        level = 0;
        usedGuesses = 0;
        selectedTile = 0;
        
        //hide panel
        SetHidden(true);

        //move camera to box, open box, begin slideshow
        CameraController.instance.SetCurrentPos(CameraController.CameraLocation.OnScreen);
        BoxController.instance.slideshow = BoxController.instance.StartCoroutine(BoxController.instance.Slideshow());
        BoxController.instance.opened = true;
    }

    private void UpdateGame(){
        if (tiles.Count == 0)
            return;
        string input = GetInputChar().ToString();
        Tile currentTile = null;
        if (!won){
            currentTile = tiles[usedGuesses][selectedTile];
            currentTile.selected = true;
        }
        if (input != "\0"){
            if (currentTile.letter == string.Empty)
                currentTile.letter = input;
            if (selectedTile < words[level].Length - 1){
                currentTile.selected = false;
                selectedTile++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)){
            if (selectedTile > 0){
                if (selectedTile == words[level].Length - 1){
                    if (currentTile.letter == string.Empty){
                        tiles[usedGuesses][selectedTile - 1].letter = string.Empty;
                        currentTile.selected = false;
                        selectedTile--;
                    }
                    else{
                        currentTile.letter = string.Empty;
                    }
                }
                else{
                    tiles[usedGuesses][selectedTile - 1].letter = string.Empty;
                    currentTile.selected = false;
                    selectedTile--;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) && selectedTile == words[level].Length - 1){
            if (won){
                StartGame(level + 1);
            }
            else{
                int correct = 0;
                for (int i = 0; i < tiles[usedGuesses].Count; ++i){
                    int returnCode = tiles[usedGuesses][i].Check(words[level], i);
                    correct += returnCode;
                }
                if (correct == words[level].Length){
                    won = true;
                    currentTile.selected = false;
                    hintText.text = "<b><color=#ffa500ff>Press ENTER to move on!</color></b>";
                }
                else if (usedGuesses < tiles.Count - 1){
                    usedGuesses++;
                    currentTile.selected = false;
                    selectedTile = 0;
                }
            }
        }
    }

    private void SetHidden(bool hidden = true){
        int newOpacity = hidden ? 0 : 1;
        foreach (Graphic graphic in GetComponentsInChildren<Graphic>()){
            graphic.CrossFadeAlpha(newOpacity, 0.75f, true);
        }
    }

    private void Start(){
        SetHidden(true);
    }

    private void Update(){
        UpdateGame();
    }
}