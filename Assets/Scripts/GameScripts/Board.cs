using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind
{
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class MatchType
{
    public int type;
    public string color;
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    [Header("Scriptable Object Stuff")]
    public World world;
    public int level;

    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    public TileType[] boardLayout;
    public bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
    public GameObject[,] allDots;

    [Header("Match Stuff")]
    public MatchType matchType;
    public Dot currentDot;
    private FindMatches findMatches;

    private ScoreManager scoreManager;
    public int basePieceValue = 20;
    private int streakValue = 1;

    private SoundManager soundManager;

    private GoalManager goalManager;

    public float refillDelay = 0.5f;

    public int[] scoreGoals;

    public bool matchesExist;


    private void Awake()
    {
        if(PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }
        if (world != null)
        {
            if (level < world.levels.Length)
            {
                if (world.levels[level] != null)
                {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    scoreGoals = world.levels[level].scoreGoals;
                    boardLayout = world.levels[level].boardLayout;
                }
            }
        }
    }

    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        soundManager = FindObjectOfType<SoundManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        breakableTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allDots = new GameObject[width, height];
        SetUp();
        currentState = GameState.pause;
    }


    public void GenerateBlankSpaces()
    {
        for(int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        //Look at all the tiles in the layout
        for(int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is a breakable tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                //create breakable tile at that pos
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        for( int i = 0; i < width; i++)
        {
            for ( int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "(" + i + "," + j + ")";

                    int dotToUse = Random.Range(0, dots.Length);

                    int maxIterations = 0;
                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }
                    maxIterations = 0;

                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;

                    dot.transform.parent = this.transform;
                    dot.name = "(" + i + "," + j + ")";

                    allDots[i, j] = dot;
                }
            }   
        }
    }
    
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if(allDots[column - 1, row] != null && allDots[column- 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
      
        return false;
    }

    private MatchType ColumnOrRow()
    {
        //make list of curr matches
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;
        matchType.type = 0;
        matchType.color = "";

        //cycle through all of match copy and decide if bomb need to be made
        for(int i = 0; i < matchCopy.Count; i++)
        {
            Dot thisDot = matchCopy[i].GetComponent<Dot>();
            string color = matchCopy[i].tag;
            int column = thisDot.column;
            int row = thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;
            for(int j = 0; j < matchCopy.Count; j++)
            {
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if(nextDot == thisDot)
                {
                    continue;
                }
                if(nextDot.column == thisDot.column && nextDot.tag == color){
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row && nextDot.tag == color)
                {
                    rowMatch++;
                }
            }
            //return 3 if column or row match
            //return 2 if adj
            //return 1 if color
            if(columnMatch == 4 || rowMatch == 4)
            {
                matchType.type = 1;
                matchType.color = color;
                return matchType;
            }
            else if(columnMatch == 2 && rowMatch == 2)
            {
                matchType.type = 2;
                matchType.color = color;
                return matchType;
            }
            else if(columnMatch == 3 || rowMatch == 3)
            {
                matchType.type = 3;
                matchType.color = color;
                return matchType;
            }
        }

        matchType.type = 0;
        matchType.color = "";
        return matchType;

        /*
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();
                if (dot.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if (dot.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHorizontal == 5);
        */
    }

    private void CheckToMakeBombs()
    {
        //How many object are in findMatches.currentMatches?
        if (findMatches.currentMatches.Count > 3)
        {
            MatchType typeOfMatch = ColumnOrRow();
            if (typeOfMatch.type == 1)
            {
                //Make a color bomb
                //Is the curr dot matched?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {

                    currentDot.isMatched = false;
                    currentDot.MakeColorBomb();
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                        {

                            otherDot.isMatched = false;
                            otherDot.MakeColorBomb();

                        }
                    }
                }
            }
            else if (typeOfMatch.type == 2)
            {
                //Make adjacent bomb
                //Is the curr dot matched?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {

                    currentDot.isMatched = false;
                    currentDot.MakeAdjacentBomb();
                }

                else if (currentDot.otherDot != null)
                {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                    {

                        otherDot.isMatched = false;
                        otherDot.MakeAdjacentBomb();

                    }

                }
            }
            else if (typeOfMatch.type == 3)
            {
                findMatches.CheckBombs(typeOfMatch);
            }
        }

        /*
        if(findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7){
            findMatches.CheckBombs();
        }
        if(findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                //Make a color bomb
                //Is the curr dot matched?
                if(currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if(currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //Make adjacent bomb
                //Is the curr dot matched?
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isAdjacentBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }
            }
        }
        */
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            

            //Does a tile need to break?
            if (breakableTiles[column, row] != null)
            {
                //if it does give 1 dmg
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column,row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }
            if (goalManager != null)
            {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }
            //Does the sound manager exist?
            if (soundManager != null)
            {
                soundManager.PlayRandomDestroyNoise();
            }
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            Destroy(allDots[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
            //Debug.Log("DestroyMatchesAt worked fine");
        }
    }

    public void DestroyMatches()
    {
        //How many elemnts are in the mathced pieces list from findmatches?
        if (findMatches.currentMatches.Count >= 4)
        {
            CheckToMakeBombs();
        }
        findMatches.currentMatches.Clear();
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        
        //Debug.Log("DestroyMatches worked fine");
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //If the curr spot isnt blank and empty
                if (!blankSpaces[i, j] && allDots[i,j] == null)
                {
                    //loop from the space above to the top of the column
                    for(int k = j + 1; k < height; k++)
                    {
                        //if a dot is found...
                        if (allDots[i, k] != null)
                        {
                            //move that dot to this empty space
                            allDots[i, k].GetComponent<Dot>().row = j;
                            //set that spot to be null
                            allDots[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i =0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i,j] == null)
                {
                    nullCount++;
                }else if(nullCount>0){
                    allDots[i,j].GetComponent<Dot>().row -=nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {

                if (allDots[i,j] == null && !blankSpaces[i,j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;
                    while(MatchesAt(i, j, dots[dotToUse]) && maxIterations<100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }

                    maxIterations = 0;
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
            
        }
        Debug.Log("refilled");
        StartCoroutine(DestroyM());
    }

    private bool MatchesOnBoard()
    {
        findMatches.FindAllMatches();
        for(int i = 0; i< width; i++)
        {
            for(int j=0; j< height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator DestroyM()
    {
        findMatches.FindAllMatches();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        yield return new WaitForSeconds(0.5f*refillDelay);
                        DestroyMatches();
                    }
                }
            }
        }
    }

    private IEnumerator FillBoardCo()
    {
        
        yield return new WaitForSeconds(refillDelay);
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);

        while (MatchesOnBoard())
        {
            Debug.Log("Matches are on board");
            yield return new WaitForSeconds(refillDelay);
            streakValue++;
            yield return new WaitForSeconds(refillDelay);
            DestroyMatches();
            //while (MatchesOnBoard())
            //{
            //    streakValue++;
            //    DestroyMatches();
            //}
            //yield break;

            yield return new WaitForSeconds(refillDelay);
            //matchesExist = MatchesOnBoard();
        }
        
        currentDot = null;
        yield return new WaitForSeconds(refillDelay);

        if (IsDeadlocked())
        {
            ShuffleBoard();
            Debug.Log("Deadlocked!");
        }
        System.GC.Collect();
        if (currentState != GameState.pause)
        {
            currentState = GameState.move;
        }
        streakValue = 1;
        
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        if (allDots[column + (int)direction.x, row + (int)direction.y])
        {
            //Take the second piece and save it in a holder
            GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
            //switching first dot to be the second pos
            allDots[column + (int)direction.x, row + (int)direction.y] =
                allDots[column, row];
            //set the first dot to be second dot
            allDots[column, row] = holder;
        }
    }

    private bool CheckForMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null && allDots[i,j] != blankSpaces[i,j])
                {
                    //Rights on the board
                    if (i < width - 2)
                    {
                        //check if the dots to the right and two to the rght exist
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag &&
                                allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    //Ups on the board
                    if (j < height - 2) {
                        //check if the dots above exist
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag &&
                                allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    private bool IsDeadlocked()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j=0;j<height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if(SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoard()
    {
        //Create a list of game object
        List<GameObject> newBoard = new List<GameObject>();
        //Add every piece in list
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        //for every  spot on board
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //if this spot shouldnt be blank
                if (!blankSpaces[i, j])
                {
                    //pick rand number
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    
                    int maxIterations = 0;
                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                    }
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                    maxIterations = 0;
                    piece.column = i;
                    piece.row = j;
                    allDots[i, j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }
        //check if its still deadlocked
        if (IsDeadlocked())
        {
            ShuffleBoard();
        }
    }
}
