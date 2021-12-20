using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insert : MonoBehaviour
{
    [HideInInspector]public Board board;
    public float numberCorrespondingToInsert;
    public int width, height;
    [HideInInspector] public int numColor;
    Camera mainCam;
    bool isPickUpInsert, isUsedInsert;
    GameObject shadowParent;
    GameObject[,] shadow;
    GameObject[,] insertGO;
    int[,] insertColor;
    Vector2 square0x0Board;
    Vector2 startPos;
    [HideInInspector] public Vector2 createPos;
    int originX, originY;
    [HideInInspector] public bool canPut;
    [SerializeField]
    public Square[] inputMatrix;

    private Square[,] _insertMatrix;
    public Square[,] InsertMatrix
    {
        get
        {
            return _insertMatrix;
        }
    }
    private void Awake()
    {
        mainCam = Camera.main;
        numColor = -1;
    }
    private void Start()
    {
        CreateInsertAndShadow();
        startPos = transform.position;
        square0x0Board = new Vector2(board.transform.position.x - 2.275f, board.transform.position.y + 2.275f);
        if (board.CheckInsertCanPut(InsertMatrix))
        {
            canPut = true;
        }
        else
        {
            canPut = false;
        }
    }
    void CreateInsertAndShadow()
    {
        GameObject gO;
        shadowParent = Instantiate(PrefabsManager.PrefabShadowParent);
        shadowParent.SetActive(false);
        _insertMatrix = new Square[width, height];
        shadow = new GameObject[width, height];
        insertGO = new GameObject[width, height];
        insertColor = new int[width, height];
        if (numColor < 0)
        {
            numColor = UnityEngine.Random.Range(0, UIManager.Instance.insertColor.Length);
        }
        Sprite color = UIManager.Instance.insertColor[numColor];
        for (int x = 0, y; x < width; x++)
        {
            for (y = 0; y < height; y++)
            {
                _insertMatrix[x, y] = inputMatrix[y * width + x];
                if (_insertMatrix[x, y] == Square.fill)
                {
                    gO = Instantiate(PrefabsManager.PrefabBLock, transform);
                    gO.transform.localPosition = new Vector3(x * 0.65f, -y * 0.65f);
                    gO.GetComponent<SpriteRenderer>().sprite = color;
                    insertGO[x, y] = gO;
                    insertColor[x, y] = numColor;
                    gO = Instantiate(PrefabsManager.PrefabBLockShadow, shadowParent.transform);
                    gO.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.8f);
                    gO.transform.localPosition = new Vector3(x * 0.65f, -y * 0.65f);
                    shadow[x, y] = gO;
                }
            }
        }
        if (insertGO.GetLength(0) % 2 == 0)
        {
            transform.position -= new Vector3(0.325f * insertGO.GetLength(0) / 2, 0);
        }
        else
        {
            transform.position -= new Vector3(0.325f * (insertGO.GetLength(0) - 1) / 2, 0);
        }
    }
    private void Update()
    {
        if (!isUsedInsert)
        {
            PickUpAndPutInsert();
            PutShadow();
            CheckShadowPlacement();
        }
        else
        {
            DestroyInsert();
        }
    }

    void DestroyInsert()
    {
        Destroy(shadowParent);
        Destroy(gameObject);
    }

    void PickUpAndPutInsert()
    {
        if (canPut)
        {
            if (mainCam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x - 0.5f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x + 1.3f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y + 0.5f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y - 1.5f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isPickUpInsert = true;
                    shadowParent.SetActive(true);
                }
            }
            if (isPickUpInsert)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    isPickUpInsert = false;
                    shadowParent.SetActive(false);
                    if (board.IsCanPutInsertIntoBoard(InsertMatrix, originX, originY))
                    {
                        transform.position = shadowParent.transform.position;
                        board.PutInsertIntoBoard(InsertMatrix, originX, originY);
                        board.PutInsertGOIntoBoard(insertGO, originX, originY);
                        board.PutInsertColorIntoBoard(insertColor, originX, originY);
                        board.insertAreWaitting.Remove(this);
                        Square[,] squareClear = new Square[8, 8];
                        GameObject[,] squareGoClear = new GameObject[8, 8];
                        for (int x = 0; x < InsertMatrix.GetLength(0); x++)
                        {
                            if (board.CheckHeightBoard(board.matrixBoard, x + originX))
                            {
                                board.ListClearHeightBLocks(squareClear, squareGoClear, x + originX);
                            }
                        }
                        for (int y = 0; y < InsertMatrix.GetLength(1); y++)
                        {
                            if (board.CheckWidthBoard(board.matrixBoard, y + originY))
                            {
                                board.ListClearWidthBLocks(squareClear, squareGoClear, y + originY);
                            }
                        }
                        for (int x = 0; x < InsertMatrix.GetLength(0); x++)
                        {
                            if (board.CheckHeightBoard(squareClear, x + originX))
                            {
                                board.ClearHeightBLocks(board.matrixBoard, x + originX);
                                board.ClearHeightBlockGO(squareGoClear, x + originX);
                            }
                        }
                        for (int y = 0; y < InsertMatrix.GetLength(1); y++)
                        {
                            if (board.CheckWidthBoard(squareClear, y + originY))
                            {
                                board.ClearWidthBLocks(board.matrixBoard, y + originY);
                                board.ClearWidthBlockGO(squareGoClear, y + originY);
                            }
                        }
                        UpScore();
                        List<Transform> transforms = new List<Transform>();
                        transforms.AddRange(GetComponentsInChildren<Transform>());
                        transforms.Remove(transform);
                        for (int x = 0; x < transforms.Count; x++)
                        {
                            transforms[x].parent = board.transform;
                        }
                        isUsedInsert = true;


                        board.numberInsertAreWaitting--;
                        if (board.numberInsertAreWaitting == 0)
                        {
                            board.CreateNewInsert();
                        }

                    }
                    else
                    {
                        transform.position = startPos;
                    }
                }
            }
        }
        if (isPickUpInsert)
        {
            transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (insertGO.GetLength(0) % 2 == 0)
            {
                transform.position -= new Vector3(0.65f * insertGO.GetLength(0) / 2, -2);
            }
            else
            {
                transform.position -= new Vector3(0.65f * (insertGO.GetLength(0) - 1) / 2, -2);
            }
            transform.localScale = new Vector3(1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f);
        }
    }
    void PutShadow()
    {
        int numberWidth = (int)((transform.position.x - board.transform.position.x) / 0.65f);
        int numberHeigh = (int)((transform.position.y - board.transform.position.y) / 0.65f);
        if (transform.position.x > board.transform.position.x)
        {
            if (transform.position.y > board.transform.position.y)
            {
                shadowParent.transform.position = new Vector3(numberWidth * 0.65f + 0.325f, numberHeigh * 0.65f + 0.325f);
            }
            else
            {
                shadowParent.transform.position = new Vector3(numberWidth * 0.65f + 0.325f, numberHeigh * 0.65f - 0.325f);
            }
        }
        else
        {
            if (transform.position.y > board.transform.position.y)
            {
                shadowParent.transform.position = new Vector3(numberWidth * 0.65f - 0.325f, numberHeigh * 0.65f + 0.325f);
            }
            else
            {
                shadowParent.transform.position = new Vector3(numberWidth * 0.65f - 0.325f, numberHeigh * 0.65f - 0.325f);
            }
        }
    }
    void CheckShadowPlacement()
    {
        float f1 = shadowParent.transform.position.x - square0x0Board.x;
        f1 = ((int)(f1 * 100f + (f1 > 0 ? 0.5f : -0.5)) / 100f);
        float f2 = shadowParent.transform.position.y - square0x0Board.y;
        f2 = ((int)(f2 * 100f + (f2 > 0 ? 0.5f : -0.5)) / 100f);
        originX = (int)(f1 / 0.65f);
        originY = -(int)(f2 / 0.65f);
        for (int y = 0, x; y < height; y++)
        {
            for (x = 0; x < width; x++)
            {
                if (shadow[x, y])
                {
                    //if (board.IsCanPutSquareIntoBoard(InsertMatrix[x, y], originX + x, originY + y))
                    //{
                    //    shadow[x, y].SetActive(true);
                    //}
                    //else
                    //{
                    //    shadow[x, y].SetActive(false);
                    //}
                    if (board.IsCanPutInsertIntoBoard(InsertMatrix, originX, originY))
                    {
                        shadow[x, y].SetActive(true);
                    }
                    else
                    {
                        shadow[x, y].SetActive(false);
                    }
                }
            }
        }
    }
    void UpScore()
    {
        for(int i = 0; i < InsertMatrix.GetLength(0); i++)
        {
            for(int k = 0; k < InsertMatrix.GetLength(1); k++)
            {
                if (InsertMatrix[i, k] == Square.fill)
                {
                    GameplayManager.Instance.UpdateScore();
                }
            }
        }
    }
}

[Serializable]
public struct SaveInsert
{
    public int width, height, numColor;
    public Square[] insertMatrix;
    public Vector3 positon;
}
