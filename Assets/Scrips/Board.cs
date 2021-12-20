using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Square { empty, fill }
public class Board : MonoBehaviour
{
    [HideInInspector] public int numberInsertAreWaitting, remanningInsert;
    public Insert[] inserts;
    [HideInInspector] public List<Insert> insertAreWaitting = new List<Insert>();
    [HideInInspector] public Square[,] matrixBoard = new Square[8, 8];
    GameObject[,] matrixBoardGO = new GameObject[8, 8];
    int[,] matrixColor = new int[8, 8];
    float numberRandom;
    Vector2 firstInsertPos = new Vector2(-2f, -3.5f);
    Vector2 square0x0Board;
    float maxInclusive=0;
    float inclusive;
    
    private void Awake()
    {
        square0x0Board = new Vector2(transform.position.x - 2.275f, transform.position.y + 2.275f);
    }
    private void Start()
    {
        if (SystemManager.Instance.SaveGame != null)
        {
            Continue();
        }
        else
        {
            ClearMatrixBoard();
            CreateNewInsert();
            remanningInsert = insertAreWaitting.Count;
        }
    }
    public void CreateNewInsert()
    {
        numberInsertAreWaitting = 3;
        maxInclusive = 0;
        for (int x = 0; x < inserts.Length; x++)
        {
            maxInclusive += inserts[x].numberCorrespondingToInsert;
        }
        for (int i = 0; i < 3; i++)
        {
            numberRandom = UnityEngine.Random.Range(0f, maxInclusive);
            GameObject gO;
            inclusive = 0;
            for (int x = 0; x < inserts.Length; x++)
            {
                if (numberRandom >= inclusive && numberRandom < inclusive + inserts[x].numberCorrespondingToInsert)
                {
                    gO = Instantiate(inserts[x].gameObject);
                    gO.GetComponent<Insert>().board = this;
                    gO.GetComponent<Insert>().createPos = new Vector3(firstInsertPos.x + i * 2f, firstInsertPos.y);
                    gO.transform.position = new Vector3(firstInsertPos.x + i * 2f , firstInsertPos.y);
                    insertAreWaitting.Add(gO.GetComponent<Insert>());
                }
                inclusive += inserts[x].numberCorrespondingToInsert;
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
            if(matrixBoard[x, y] == Square.empty)
            {
                matrixBoard[x, y] = square;
            }
        }
    }
    public void PutInsertColorIntoBoard(int[,] numColor, int x, int y)
    {
        for (int i = 0; i < numColor.GetLength(0); i++)
        {
            for (int k = 0; k < numColor.GetLength(1); k++)
            {
                PutColorIntoBoard(numColor[i, k], x + i, y + k);
            }
        }
    }
    public void PutColorIntoBoard(int numColor, int x, int y)
    {
        if (x < matrixBoard.GetLength(0) && x >= 0 && y < matrixBoard.GetLength(1) && y >= 0)
        {
            matrixColor[x, y] = numColor;
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
        if (x < matrixBoardGO.GetLength(0) && x >= 0 && y < matrixBoardGO.GetLength(1) && y >= 0)
        {
            if (matrixBoardGO[x, y] == null)
            {
                matrixBoardGO[x, y] = square;
            }
        }
    }
    public bool CheckWidthBoard(Square[,] board, int y)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            if (board[x, y] == Square.empty)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckHeightBoard(Square[,] board, int x)
    {
        for (int y = 0; y < matrixBoard.GetLength(1); y++)
        {
            if(board[x, y] == Square.empty)
            {
                return false;
            }
        }
        return true;
    }
    public void ClearWidthBLocks(Square[,] squares, int y)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            if (squares[x, y] == Square.fill)
            {
                matrixBoard[x, y] = Square.empty;
            }
        }
    }
    public void ClearHeightBLocks(Square[,] squares, int x)
    {
        for (int y = 0; y < matrixBoard.GetLength(1); y++)
        {
            if (squares[x, y] == Square.fill)
            {
                matrixBoard[x, y] = Square.empty;
            }
        }
    }
    IEnumerator DestroySquare(GameObject square, float time)
    {
        yield return new WaitForSeconds(time);
        while (true)
        {
            if (square == null)
            {
                yield break;
            }
            square.transform.localScale -= new Vector3(0.02f, 0.02f);
            if (square.transform.localScale.x <= 0)
            {
                GameplayManager.Instance.UpdateScore();
                Destroy(square);
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
    public bool CheckGameOver()
    {
        for(int x = 0; x < insertAreWaitting.Count; x++)
        {
            if (insertAreWaitting[x].canPut)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckInsertCanPut(Square[,] insert)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            for (int y = 0; y < matrixBoard.GetLength(1); y++)
            {
                if (IsCanPutInsertIntoBoard(insert, x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void ListClearWidthBLocks(Square[,] squares, GameObject[,] gOs, int y)
    {
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            if (matrixBoard[x, y] == Square.fill)
            {
                squares[x, y] = Square.fill;
                gOs[x, y] = matrixBoardGO[x, y];
            }
        }
    }
    public void ListClearHeightBLocks(Square[,] squares, GameObject[,] gOs, int x)
    {
        for (int y = 0; y < matrixBoard.GetLength(1); y++)
        {
            if (matrixBoard[x, y] == Square.fill)
            {
                squares[x, y] = Square.fill;
                gOs[x, y] = matrixBoardGO[x, y];
            }
        }
    }
    public void ClearWidthBlockGO(GameObject[,] gOs, int y)
    {
        for(int x = 0; x <= 4; x++)
        {
            if(gOs[4 - x, y] != null)
            {
                StartCoroutine(DestroySquare(gOs[4 - x, y], 0.1f * x));
            }
            if(gOs[3 + x, y] != null)
            {
                StartCoroutine(DestroySquare(gOs[3 + x, y], 0.1f * x));
            }
        }
    }
    public void ClearHeightBlockGO(GameObject[,] gOs, int x)
    {
        for(int y = 0; y <= 4; y++)
        {
            if(gOs[x, 4 - y] != null)
            {
                StartCoroutine(DestroySquare(gOs[x, 4 - y], 0.1f * y));
            }
            if(gOs[x, 3 + y] != null)
            {
                StartCoroutine(DestroySquare(gOs[x, 3 + y], 0.1f * y));
            }
        }
    }
    private void Update()
    {
        if (insertAreWaitting.Count != remanningInsert)
        {
            for (int x = 0; x < insertAreWaitting.Count; x++)
            {
                if (CheckInsertCanPut(insertAreWaitting[x].InsertMatrix))
                {
                    insertAreWaitting[x].canPut = true;
                }
                else
                {
                    insertAreWaitting[x].canPut = false;
                }
            }
            remanningInsert--;
            if (remanningInsert == 0)
            {
                remanningInsert = insertAreWaitting.Count;
            }
            if (CheckGameOver())
            {
                Action action = () =>
                {
                    GameplayManager.Instance.Onlose();
                };
                AdManager.ShowVideo(action, null, null);
            }
        }
    }

    public void GetSaveData(ref SaveGame saveGame)
    {
        saveGame.matrixBoard = new Square[matrixBoard.Length];
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            for (int y = 0; y < matrixBoard.GetLength(1); y++)
            {
                saveGame.matrixBoard[y * matrixBoard.GetLength(0) + x] = matrixBoard[x, y];
            }
        }
        saveGame.matrixColor = new int[matrixColor.Length];
        for (int x = 0; x < matrixColor.GetLength(0); x++)
        {
            for (int y = 0; y < matrixColor.GetLength(1); y++)
            {
                saveGame.matrixColor[y * matrixColor.GetLength(0) + x] = matrixColor[x, y];
            }
        }
        SaveInsert[] saveInserts = new SaveInsert[insertAreWaitting.Count];
        for (int x = 0; x < insertAreWaitting.Count; x++)
        {
            saveInserts[x].numColor = insertAreWaitting[x].numColor;
            saveInserts[x].width = insertAreWaitting[x].width;
            saveInserts[x].height = insertAreWaitting[x].height;
            saveInserts[x].positon = insertAreWaitting[x].createPos;
            saveInserts[x].insertMatrix = new Square[insertAreWaitting[x].InsertMatrix.Length];
            for (int i = 0; i < insertAreWaitting[x].width; i++)
            {
                for(int k = 0; k < insertAreWaitting[x].height; k++)
                {
                    saveInserts[x].insertMatrix[k * insertAreWaitting[x].width + i] = insertAreWaitting[x].InsertMatrix[i, k];
                }
            }
        }
        saveGame.inserts = saveInserts;
        saveGame.numberInsertAreWaitting = numberInsertAreWaitting;
        saveGame.remanningInsert = remanningInsert;
        saveGame.score = GameplayManager.Instance.score;
    }

    void Continue()
    {
        SaveGame saveGame = SystemManager.Instance.SaveGame.Value;
        GameplayManager.Instance.score = saveGame.score;
        for (int x = 0; x < matrixBoard.GetLength(0); x++)
        {
            for (int y = 0; y < matrixBoard.GetLength(1); y++)
            {
                matrixBoard[x, y] = saveGame.matrixBoard[y * matrixBoard.GetLength(0) + x];
                if (matrixBoard[x, y] == Square.fill)
                {
                    matrixBoardGO[x, y] = Instantiate(PrefabsManager.PrefabBLock, transform);
                    matrixBoardGO[x, y].transform.position = new Vector3(square0x0Board.x + x * 0.65f, square0x0Board.y - y * 0.65f);
                    Sprite color = UIManager.Instance.insertColor[saveGame.matrixColor[y * matrixColor.GetLength(0) + x]];
                    matrixColor[x, y] = saveGame.matrixColor[y * matrixColor.GetLength(0) + x];
                    matrixBoardGO[x, y].GetComponent<SpriteRenderer>().sprite = color;
                }
            }
        }
        numberInsertAreWaitting = saveGame.numberInsertAreWaitting;
        remanningInsert = saveGame.remanningInsert;
        Insert insert;
        GameObject go;
        for (int x = 0; x < saveGame.inserts.Length; x++)
        {
            go = new GameObject();
            insert = go.AddComponent<Insert>();
            insert.board = this;
            insert.width = saveGame.inserts[x].width;
            insert.height = saveGame.inserts[x].height;
            insert.inputMatrix = new Square[insert.width*insert.height];
            for (int i = 0; i < saveGame.inserts[x].insertMatrix.Length; i++)
            {
                insert.inputMatrix[i] = saveGame.inserts[x].insertMatrix[i];
            }
            insert.numColor = saveGame.inserts[x].numColor;
            insert.createPos = saveGame.inserts[x].positon;
            insertAreWaitting.Add(insert);
            go.transform.position = saveGame.inserts[x].positon;
        }
    }
}
