using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Square { empty, fill }
public class Board : MonoBehaviour
{
    Square[,] matrixBoard = new Square[8, 8];
    private void Start()
    {
        ClearMatrixBoard();
    }
    void ClearMatrixBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                matrixBoard[x, y] = Square.empty;
            }
        }
    }

    public bool IsCanPutSquareIntoBoard(Square square, int x, int y)
    {
        if (x < matrixBoard.GetLength(0) && x >= 0 && y < matrixBoard.GetLength(1) && y >= 0)
        {
            if (square == Square.empty || matrixBoard[x, y] == Square.empty)
            {
                return true;
            }
        }
        else
        {
            if (square == Square.empty)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsCanPutInsertIntoBoard(Square[,] insert, int x, int y)
    {
        for (int i = 0; i < insert.GetLength(0); i++)
        {
            for (int k = 0; k < insert.GetLength(1); k++)
            {
                if (!IsCanPutSquareIntoBoard(insert[i, k], x + i, y + k))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PutSquareIntoBoard(Square square, int x, int y)
    {
        if (x < matrixBoard.GetLength(0) && x >= 0 && y < matrixBoard.GetLength(1) && y >= 0)
        {
            matrixBoard[x, y] = square;
        }
    }
    public void PutInsertIntoBoard(Square[,] insert, int x, int y)
    {
        for (int i = 0; i < insert.GetLength(0); i++)
        {
            for (int k = 0; k < insert.GetLength(1); k++)
            {
                PutSquareIntoBoard(insert[i, k], x + i, y + k);
            }
        }
    }
}
