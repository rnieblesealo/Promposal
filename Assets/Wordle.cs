using System.Collections.Generic;
using UnityEngine;

public class Wordle : MonoBehaviour{
    public GameObject tile;
    public GameObject row;
    
    public string word;
    public int guesses;

    private int expendedGuesses = 0;
    private int selectedTile = 0; //which tile are we going to type on relative to the current row?

    [SerializeField] private List<List<Tile>> tiles = new List<List<Tile>>();

    void StartGame(){
        expendedGuesses = 0;
        selectedTile = 0;
        for (int i = 0; i < guesses; ++i){
            Transform newRow = Instantiate(row, gameObject.transform).transform;
            List<Tile> newTiles = new List<Tile>();
            for (int j = 0; j < word.Length; ++j){
                Tile newTile = Instantiate(tile, newRow).GetComponent<Tile>();
                newTiles.Add(newTile);
            }
            tiles.Add(newTiles);
        }
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

    private void Start(){
        StartGame();
    }

    private void Update(){
        if (tiles.Count < 0)
            return;
        string input = GetInputChar().ToString();
        Tile currentTile = tiles[expendedGuesses][selectedTile];
        if (input != "\0"){
            if (currentTile.letter == string.Empty)
                currentTile.letter = input;
            if (selectedTile < word.Length - 1)
                selectedTile++;
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)){
            currentTile.letter = string.Empty;
            if (selectedTile > 0)
                selectedTile--;
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
            for (int i = 0; i < tiles[expendedGuesses].Count; ++i){
                tiles[expendedGuesses][i].Check(word, i);
            }
        }
    }
}