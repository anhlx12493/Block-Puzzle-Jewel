using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Square { empty, fill }
public class Board : MonoBehaviour
{
    [HideInInspector] public int numberInsertAreWaitting;
    public Insert[] inserts;
    Square[,] matrixBoard = new Square[8, 8];
    GameObject[,] matrixBoardGO = new GameObject[8, 8];
    float numberRandom;
    Vector2 firstInsertPos = new Vector2(-2.5f, -3);
    private void Awake()
    {
        CreateNewInsert();
    }
    private void Start()
    {
        ClearMatrixBoard();
    }
    public void CreateNewInsert()
    {
        numberInsertAreWaitting = 3;
        for(int i = 0; i < 3; i++)
        {
            numberRandom = Random.Range(0f, inserts.Length + 2f);
            GameObject gO;
            for (int x = 0; x < inserts.Length; x++)
            {
                if (numberRandom >= inserts[x].numberCorrespondingToInsert * x && numberRandom < inserts[x].numberCorrespondingToInsert * (x + 1))
                {
                    gO = Instantiate(inserts[x].gameObject);
                    gO.GetComponent<Insert>().board = this;
                    gO.transform.position = new Vector3(firstInsertPos.x + i * 2.5f, firstInsertPos.y);
                }
            }
        }
    }
    void ClearMatrixBoard()
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            for (int y = 0; y < matrixBoard.GetLength(1); y++)
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
    public void PutInsertGOIntoBoard(GameObject[,] insertGO, int x, int y)
    {
        for (int i = 0; i < insertGO.GetLength(0); i++)
        {
            for (int k = 0; k < insertGO.GetLength(1); k++)
            {
                PutSquareGOIntoBoard(insertGO[i, k], x + i, y + k);
            }
        }
    }
    private void PutSquareGOIntoBoard(GameObject square, int x, int y)
    {
        if (x < matrixBoard.GetLength(0) && x >= 0 && y < matrixBoard.GetLength(1) && y >= 0)
        {
            matrixBoardGO[x, y] = square;
        }
    }
    public bool CheckWidthBoard(int y)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            if (matrixBoard[x, y] == Square.empty)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckHeightBoard(int x)
    {
        for (int y = 0; y < matrixBoard.GetLength(0); y++)
        {
            if(matrixBoard[x, y] == Square.empty)
            {
                return false;
            }
        }
        return true;
    }
    public void ClearWidthBLocks(int y)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            if (matrixBoard[x, y] == Square.fill)
            {
                matrixBoard[x, y] = Square.empty;
                Destroy(matrixBoardGO[x, y]);
            }
        }
    }
    public void ClearHeightBLocks(int x)
    {
        for (int y = 0; y < matrixBoard.GetLength(0); y++)
        {
            if (matrixBoard[x, y] == Square.fill)
            {
                matrixBoard[x, y] = Square.empty;
                Destroy(matrixBoardGO[x, y]);
            }
        }
    }
}
