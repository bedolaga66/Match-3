using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    private Dot otherDot;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        otherDot = FindObjectOfType<Dot>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }

        if (dot2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }

        if (dot3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }
        return currentDots;
    }

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }
        return currentDots;
    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }

        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }

        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }
        return currentDots;
    }

    public List<GameObject> ChainAdjacentBomb()
    {
        List<GameObject> currentDots = new List<GameObject>();
        for (int i = 0; i < board.width - 1; i++)
        {
            for (int j = 0; j < board.height - 1; j++)
            {
                if (!board.blankSpaces[i, j])
                {
                    GameObject currentDot = board.allDots[i, j];
                    if (currentDot.GetComponent<Dot>() != null)
                    {
                        if (currentDot.GetComponent<Dot>().isAdjacentBomb)
                        {
                            currentMatches.Union(GetAdjacentPieces(currentDot.GetComponent<Dot>().column, currentDot.GetComponent<Dot>().row));
                        }
                    }
                }


            }
        }
        //if (dot.isAdjacentBomb)
        //{
        //    currentMatches.Union(GetAdjacentPieces(dot.column, dot.row));
        //}

        return currentDots;
    }

    public List<GameObject> ChainRowBomb()
    {
        List<GameObject> currentDots = new List<GameObject>();
        for(int i = 0; i < board.width - 1; i++)
        {
            for(int j = 0; j < board.height - 1; j++)
            {
                if(!board.blankSpaces[i, j]) {
                    GameObject currentDot = board.allDots[i, j];
                    if (currentDot.GetComponent<Dot>() != null)
                    {
                        if (currentDot.GetComponent<Dot>().isRowBomb)
                        {
                            currentMatches.Union(GetRowPieces(currentDot.GetComponent<Dot>().row));
                        }
                    }
                }
                
                
            }
        }
        //if (dot.isRowBomb)
        //{
            
        //    currentMatches.Union(GetRowPieces(dot.row));
        //}

        return currentDots;
    }

    public List<GameObject> ChainColumnBomb()
    {
        List<GameObject> currentDots = new List<GameObject>();
        for (int i = 0; i < board.width - 1; i++)
        {
            for (int j = 0; j < board.height - 1; j++)
            {
                if (!board.blankSpaces[i, j])
                {
                    GameObject currentDot = board.allDots[i, j];
                    if (currentDot.GetComponent<Dot>() != null)
                    {
                        if (currentDot.GetComponent<Dot>().isColumnBomb)
                        {
                            currentMatches.Union(GetColumnPieces(currentDot.GetComponent<Dot>().column));
                        }
                    }
                }


            }
        }
        //if (dot.isColumnBomb)
        //{
        //    currentMatches.Union(GetColumnPieces(dot.column));
        //}

        return currentDots;
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);

        AddToListAndMatch(dot2);

        AddToListAndMatch(dot3);
        
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.1f);
        //yield return null;
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                
                if(currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();

                    //if (currentDotDot.isRowBomb && board.currentDot.GetComponent<Dot>().coroutineWorking)
                    //{
                    //    currentMatches.Union(ChainRowBomb(currentDotDot));
                    //}
                    //if (currentDotDot.isColumnBomb && board.currentDot.GetComponent<Dot>().coroutineWorking)
                    //{
                    //    currentMatches.Union(ChainColumnBomb(currentDotDot));
                    //}
                    //if (currentDotDot.isAdjacentBomb && board.currentDot.GetComponent<Dot>().coroutineWorking)
                    //{
                    //    currentMatches.Union(ChainAdjacentBomb(currentDotDot));
                    //}

                    if ( i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        
                        GameObject rightDot = board.allDots[i + 1, j];

                        if (leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();

                            if (leftDot != null && rightDot != null)
                            {

                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                                {
                                    currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));

                                    currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));

                                    currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                                
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        
                        GameObject downDot = board.allDots[i, j - 1];

                        if (upDot != null && downDot != null)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();

                            if (upDot != null && downDot != null)
                            {
                                
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                                {


                                    currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                                    currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));

                                    currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));

                                    GetNearbyPieces(upDot, currentDot, downDot);

                                }
                            }
                        }
                    }
                }
            }
        }
        //yield return null;
    }

    public void MatchPiecesOfColor(string color)
    {
        for(int i = 0; i< board.width; i++)
        {
            for(int j = 0; j < board.height; j++)
            {
                //Check if that piece exists
                if (board.allDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                        
                    }
                }
            }
        }
    }

    public void MatchPiecesOfColorAndRowBomb(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                //Check if that piece exists
                if (board.allDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().MakeRowBomb();
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;

                    }
                }
            }
        }
    }

    public void MatchPiecesOfColorAndColumnBomb(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                //Check if that piece exists
                if (board.allDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().MakeColumnBomb();
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;

                    }
                }
            }
        }
    }

    public void MatchPiecesOfColorAndAdjacentBomb(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                //Check if that piece exists
                if (board.allDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().MakeAdjacentBomb();
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;

                    }
                }
            }
        }
    }

    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i = column - 1; i <= column + 1; i++)
        {
            for(int j = row - 1; j <= row + 1; j++)
            {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    if (board.allDots[i, j] != null)
                    {
                        if (board.allDots[i, j].GetComponent<Dot>().isRowBomb)
                        {
                            foreach(GameObject dot in GetRowPieces(j))
                            {
                                dots.Add(dot);
                            }
                        }
                        if (board.allDots[i, j].GetComponent<Dot>().isColumnBomb)
                        {
                            foreach (GameObject dot in GetColumnPieces(j))
                            {
                                dots.Add(dot);
                            }
                        }
                        dots.Add(board.allDots[i, j]);
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
        return dots;
    }

    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0; i < board.height; i++)
        {
            if (board.allDots[column,i] != null)
            {
                Dot dot = board.allDots[column, i].GetComponent<Dot>();
                if (dot.isRowBomb)
                {
                    dots.Union(GetRowPieces(i)).ToList();
                }
                if (dot.isAdjacentBomb)
                {
                    dots.Union(GetAdjacentPieces(column, i)).ToList();
                }
                dots.Add(board.allDots[column,i]);
                dot.isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                Dot dot = board.allDots[i, row].GetComponent<Dot>();
                if (dot.isColumnBomb)
                {
                    dots.Union(GetColumnPieces(i)).ToList();
                }
                if (dot.isAdjacentBomb)
                {
                    dots.Union(GetAdjacentPieces(i, row)).ToList();
                }
                dots.Add(board.allDots[i, row]);
                dot.isMatched = true;
            }
        }
        return dots;
    }

    public void CheckBombs(MatchType matchType)
    {
        //Move check
        if (board.currentDot != null)
        {
            //Moved thing matched?
            if (board.currentDot.isMatched && board.currentDot.tag == matchType.color)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //Make it unmatched
                board.currentDot.isMatched = false;      //????                             //BUG WITH 3-4-5 FALL & NO-MATCH
                //Decide what king of bomb to make
                /*
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50)
                {
                    //Make a row bomb
                    board.currentDot.MakeRowBomb();
                }else if(typeOfBomb > 50)
                {
                    //Make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
                */
                if((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                    || ((board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= -180 ) 
                    || (board.currentDot.swipeAngle >= 135 && board.currentDot.swipeAngle <= 180)))
                {
                    board.currentDot.MakeRowBomb();
                }
                else
                {
                    board.currentDot.MakeColumnBomb();
                }

            }
            //Is other thing matched?
            else if (board.currentDot.otherDot != null)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //Is the other dot matched?
                if (otherDot.isMatched && otherDot.tag == matchType.color)
                {
                    otherDot.isMatched = false;                                       //BUG WITH 3-4-5 FALL & NO-MATCH
                    //Decide what king of bomb to make
                    /*
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        //Make a row bomb
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb > 50)
                    {
                        //Make a column bomb
                        otherDot.MakeColumnBomb();
                    */

                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                    || ((board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= -180)
                    || (board.currentDot.swipeAngle >= 135 && board.currentDot.swipeAngle <= 180)))
                    {
                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
             
            }
        }
    }
}   
