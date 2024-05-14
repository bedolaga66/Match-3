using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private GameObject BGTile;
    private SpriteRenderer spriteBGTile;

    [Header("Board Variables")]
    public int column;
    public int row;

    public int previousColumn;
    public int previousRow;

    public int targetX;
    public int targetY;

    public bool isMatched = false;

    public bool coroutineWorking = false;

    private HintManager hintManager;

    private EndGameManager endGameManager;

    private FindMatches findMatches;
    private Board board;
    public GameObject otherDot;


    private Vector2 firstTouchPosition = Vector2.zero;
    private Vector2 finalTouchPosition = Vector2.zero;
    private Vector2 tempPosition;

    [Header("Swipe")]

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Bombs and stuff")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;
    public GameObject adjacentMarker;

    private GameObject fDot;
    private GameObject sDot;
    private Vector2 fDotPos;
    private Vector2 sDotPos;
    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        spriteBGTile = GameObject.FindWithTag("BackgroundTIle").GetComponent<SpriteRenderer>();
        BGTile = GameObject.FindWithTag("BackgroundTIle");
        endGameManager = FindObjectOfType<EndGameManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        //board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;
    }

    //Test & Debug only
    /*private void OnMouseOver()
    { 
        
        if (Input.GetMouseButtonDown(1))
        {
            isAdjacentBomb = true;
            GameObject Adjacent = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            Adjacent.transform.parent = this.transform;
        }
        
        
    }
    */

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0) && board.currentState != GameState.pause)
    //    {
    //        MakeSwap();
    //    }
    //    //if (Input.GetMouseButtonDown(0) && board.currentState == GameState.move && isBothDotsChoosen == true)
    //    //{
    //    //    MakeSwap();
    //    //}
    //}

    // Update is called once per frame
    void Update()
    {
        /*
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        */
        //SwapDots();
        //ChooseDots();
        //if (board.currentState == GameState.move && isBothDotsChoosen == true)
        //{
        //    MakeSwap();
        //}
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move towards target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column,row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }
           
        }
        else
        {
            //Directly set the pos
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move towards target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }
            
        }
        else
        {
            //Directly set the pos
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        
        if (isColorBomb)
        {
            coroutineWorking = true;
            //This piece is a color bomb, and other piece is the color to destroy
            if (otherDot.GetComponent<Dot>().isRowBomb)
            {
                findMatches.MatchPiecesOfColorAndRowBomb(otherDot.tag);
                
                findMatches.currentMatches.Union(findMatches.ChainRowBomb());

                isMatched = true;
            }
            else if (otherDot.GetComponent<Dot>().isColumnBomb)
            {
                findMatches.MatchPiecesOfColorAndColumnBomb(otherDot.tag);

                findMatches.currentMatches.Union(findMatches.ChainColumnBomb());

                isMatched = true;
            }
            else if (otherDot.GetComponent<Dot>().isAdjacentBomb)
            {
                findMatches.MatchPiecesOfColorAndAdjacentBomb(otherDot.tag);

                findMatches.currentMatches.Union(findMatches.ChainAdjacentBomb());

                isMatched = true;
            }
            else {
                findMatches.MatchPiecesOfColor(otherDot.tag);
                isMatched = true;
            }
            coroutineWorking = false;

        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            coroutineWorking = true;
            //The other piece is a color bomb,and this piece have color to destroy
            if (this.gameObject.GetComponent<Dot>().isRowBomb)
            {
                findMatches.MatchPiecesOfColorAndRowBomb(this.gameObject.tag);
                findMatches.currentMatches.Union(findMatches.ChainRowBomb());
                otherDot.GetComponent<Dot>().isMatched = true;
            }
            else if (this.gameObject.GetComponent<Dot>().isColumnBomb)
            {
                findMatches.MatchPiecesOfColorAndColumnBomb(this.gameObject.tag);
                findMatches.currentMatches.Union(findMatches.ChainColumnBomb());
                otherDot.GetComponent<Dot>().isMatched = true;
            }
            else if (this.gameObject.GetComponent<Dot>().isAdjacentBomb)
            {
                findMatches.MatchPiecesOfColorAndAdjacentBomb(this.gameObject.tag);
                findMatches.currentMatches.Union(findMatches.ChainAdjacentBomb());
                otherDot.GetComponent<Dot>().isMatched = true;
            }
            else {
                findMatches.MatchPiecesOfColor(this.gameObject.tag);
                otherDot.GetComponent<Dot>().isMatched = true;
            }
            coroutineWorking = false;
        }
        
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                if (endGameManager != null)
                {
                    if(endGameManager.requiremenets.gameType == GameType.Moves)
                    {
                        endGameManager.DecreaseCounterValue();
                    }
                }
                board.DestroyMatches();
                
            }
            //otherDot = null;
        }
        

    }

    //public IEnumerator CheckMoveCo2(Dot dot1, GameObject dot2)
    //{
    //    if (dot1.isColorBomb)
    //    {
    //        //This piece is a color bomb, and other piece is the color to destroy
    //        findMatches.MatchPiecesOfColor(dot2.tag);
    //        dot1.isMatched = true;
    //    }
    //    else if (dot2.GetComponent<Dot>().isColorBomb)
    //    {
    //        //The other piece is a color bomb,and this piece have color to destroy
    //        findMatches.MatchPiecesOfColor(this.gameObject.tag);
    //        dot2.GetComponent<Dot>().isMatched = true;
    //    }

    //    yield return new WaitForSeconds(.5f);
    //    if (dot2 != null)
    //    {
    //        if (!dot1.isMatched && !dot2.GetComponent<Dot>().isMatched)
    //        {
    //            Vector2 tempPosition1 = new Vector2(dot1.column, dot1.row);
    //            Vector2 tempPosition2 = new Vector2(dot2.GetComponent<Dot>().column, dot2.GetComponent<Dot>().row);

    //            // Обмен позициями
    //            dot1.column = (int)tempPosition2.x;
    //            dot1.row = (int)tempPosition2.y;
    //            dot2.GetComponent<Dot>().column = (int)tempPosition1.x;
    //            dot2.GetComponent<Dot>().row = (int)tempPosition1.y;
    //            //dot2.GetComponent<Dot>().row = row;
    //            //dot2.GetComponent<Dot>().column = column;
    //            //dot1.row = previousRow;
    //            //dot1.column = previousColumn;
    //            yield return new WaitForSeconds(.5f);
    //            //yield return null;
    //            board.currentDot = null;
    //            board.currentState = GameState.move;
    //        }
    //        else
    //        {
    //            if (endGameManager != null)
    //            {
    //                if (endGameManager.requiremenets.gameType == GameType.Moves)
    //                {
    //                    endGameManager.DecreaseCounterValue();
    //                }
    //            }
    //            board.currentDot.otherDot = otherDot;
    //            board.DestroyMatches();

    //        }
    //        //otherDot = null;
    //    }

    //}

    //private void FirstDotSelected()
    //{
    //    if (Input.GetMouseButtonDown(0) && board.currentState == GameState.move)
    //    {
    //        fDotPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Debug.Log(fDotPos);
    //        if (Mathf.Round(fDotPos.x) < (Mathf.Round(fDotPos.x) + 0.5f) && Mathf.Round(fDotPos.x) > (Mathf.Round(fDotPos.x) - 0.5f))
    //        {
    //            fDotPos.x = Mathf.Round(fDotPos.x);
    //        }
    //        if (Mathf.Round(fDotPos.y) < (Mathf.Round(fDotPos.y) + 0.5f) && Mathf.Round(fDotPos.y) > (Mathf.Round(fDotPos.y) - 0.5f))
    //        {
    //            fDotPos.y = Mathf.Round(fDotPos.y);
    //        }
    //        Debug.Log(fDotPos);
    //        fDot = board.allDots[(int)fDotPos.x, (int)fDotPos.y];
    //        Debug.Log(fDot);
    //        if (hintManager != null)
    //        {
    //            hintManager.DestroyHint();
    //        }
    //    }
    //}

    //private void SecondDotSelected()
    //{
    //    if (Input.GetMouseButtonDown(0) && board.currentState == GameState.move)
    //    {
    //        sDotPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Debug.Log(sDotPos);
    //        if (Mathf.Round(sDotPos.x) < (Mathf.Round(sDotPos.x) + 0.5f) && Mathf.Round(sDotPos.x) > (Mathf.Round(sDotPos.x) - 0.5f))
    //        {
    //            sDotPos.x = Mathf.Round(sDotPos.x);
    //        }
    //        if (Mathf.Round(sDotPos.y) < (Mathf.Round(sDotPos.y) + 0.5f) && Mathf.Round(sDotPos.y) > (Mathf.Round(sDotPos.y) - 0.5f))
    //        {
    //            sDotPos.y = Mathf.Round(sDotPos.y);
    //        }
    //        Debug.Log(sDotPos);
    //        sDot = board.allDots[(int)sDotPos.x, (int)sDotPos.y];
    //        Debug.Log(sDot);
    //        if (hintManager != null)
    //        {
    //            hintManager.DestroyHint();
    //        }
    //    }
    //    else
    //    {
    //        FirstDotSelected();
    //    }

    //}

    //private void SwapDots()
    //{
    //    //C
    //    if (board.currentDot != null && otherDot != null)
    //    {

    //        if (Mathf.Abs(board.currentDot.transform.position.x - otherDot.transform.position.x) == 1 && Mathf.Abs(board.currentDot.transform.position.y - otherDot.transform.position.y) == 0) //1.2f)
    //        {
    //            if (board.currentDot.transform.position.x - otherDot.transform.position.x < 0)
    //            {
    //                //MovePiecesActual(Vector2.right);
    //                MoveDots(board.currentDot, otherDot);
    //            }
    //            if (board.currentDot.transform.position.x - otherDot.transform.position.x > 0)
    //            {
    //                //MovePiecesActual(Vector2.left);
    //                MoveDots(board.currentDot, otherDot);
    //            }
    //        }
    //        else if (Mathf.Abs(board.currentDot.transform.position.x - otherDot.transform.position.x) == 0 && Mathf.Abs(board.currentDot.transform.position.y - otherDot.transform.position.y) == 1)
    //        {
    //            if (board.currentDot.transform.position.y - otherDot.transform.position.y < 0)
    //            {
    //                //MovePiecesActual(Vector2.up);
    //                MoveDots(board.currentDot, otherDot);
    //            }
    //            if (board.currentDot.transform.position.y - otherDot.transform.position.y > 0)
    //            {
    //                //MovePiecesActual(Vector2.down);
    //                MoveDots(board.currentDot, otherDot);
    //            }
    //        }
            
    //    }
    //    else
    //    {
    //        board.currentDot = null;
    //    }
    //}

    //void MoveDots(Dot dot1, GameObject dot2)
    //{
    //    // Сохраните начальные позиции объектов
    //    if (dot1 != null && dot2 != null)
    //    {
    //        Vector2 tempPosition1 = new Vector2(dot1.column, dot1.row);
    //        Vector2 tempPosition2 = new Vector2(dot2.GetComponent<Dot>().column, dot2.GetComponent<Dot>().row);

    //        // Обмен позициями
    //            dot1.column = (int)tempPosition2.x;
    //            dot1.row = (int)tempPosition2.y;
    //            dot2.GetComponent<Dot>().column = (int)tempPosition1.x;
    //            dot2.GetComponent<Dot>().row = (int)tempPosition1.y;
    //            StartCoroutine(CheckMoveCo2(dot1, dot2));
    //    }
    //    // Переместите объекты на новые позиции
    //    //StartCoroutine(MoveToPosition(dot1, new Vector2(dot1.column, dot1.row));
    //    // StartCoroutine(MoveToPosition(dot2, new Vector2(dot2.GetComponent<Dot>().column, dot2.GetComponent<Dot>().row));
    //}

    //void MakeSwap()
    //{
    //    if (board.currentDot == null)
    //    {
    //        FirstDotSelected();

    //        //isBothDotsChoosen = true;
    //        board.currentDot = this;
    //        //    Vector2 BGTilePos = BGTile.transform.position;
    //        //    Vector2 dotPos = board.currentDot.transform.position;
    //        //    if (BGTilePos == dotPos)
    //        //    {
    //        //        spriteBGTile.transform.position = BGTilePos;
    //        //        spriteBGTile.color = Color.green;
    //        //    }
    //    }
    //    else
    //    {

    //        SecondDotSelected();
    //        board.currentState = GameState.wait;
    //        otherDot = sDot;
    //        if(otherDot != null)
    //        {
    //            previousRow = otherDot.GetComponent<Dot>().row;
    //            previousColumn = otherDot.GetComponent<Dot>().column;
    //        }
            
    //        //isBothDotsChoosen = true;

    //        //MoveDots(board.currentDot, otherDot);


    //        SwapDots();
    //        //isBothDotsChoosen = false;
    //        //spriteBGTile.color = new Color(255,255,255,255);
    //    }
    //    //board.currentState = GameState.move;
    //}

    private void OnMouseDown()
    {
       //Destroy hint
       if(hintManager != null)
        {
            hintManager.DestroyHint();
        }
        if(board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }
        
    }

    private void OnMouseUp()
    {

        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }

    }


    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            
            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
            
        }
    }

    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if(otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
        
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right Swipe
            /*
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up Swipe
            /*
            otherDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left Swipe
            /*
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            /*
            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    public void MakeRowBomb()
    {
        if (!isColorBomb && !isColumnBomb && !isAdjacentBomb)
        {
            isRowBomb = true;
            GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }

    public void MakeColumnBomb()
    {
        if (!isRowBomb && !isColorBomb && !isAdjacentBomb)
        {
            isColumnBomb = true;
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }

    public void MakeColorBomb()
    {
        if (!isRowBomb && !isColumnBomb && !isAdjacentBomb)
        {
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
    }

    public void MakeAdjacentBomb()
    {
        if (!isRowBomb && !isColorBomb && !isColumnBomb)
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }
}
