using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insert : MonoBehaviour
{
    Camera mainCam;
    Board board;
    bool isPickUpInsert;
    public int width, height;
    GameObject shadowParent;
    GameObject[,] shadow;
    Vector2 square0x0Board;
    int originX, originY;
    [SerializeField]
    private Square[] inputMatrix;

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
        board = FindObjectOfType<Board>();
        mainCam = Camera.main;
        CreateInsertAndShadow();
        square0x0Board = new Vector2(board.transform.position.x - 2.275f, board.transform.position.y + 2.275f);
    }
    void CreateInsertAndShadow()
    {
        GameObject gO;
        shadowParent = Instantiate(PrefabsManager.PrefabShadowParent);
        shadowParent.SetActive(false);
        _insertMatrix = new Square[width, height];
        shadow = new GameObject[width, height];
        for (int x = 0, y; x < width; x++)
        {
            for (y = 0; y < height; y++)
            {
                _insertMatrix[x, y] = inputMatrix[y * width + x];
                if (_insertMatrix[x, y] == Square.fill)
                {
                    gO = Instantiate(PrefabsManager.PrefabBLock, transform);
                    gO.transform.localPosition = new Vector3(x * 0.65f, -y * 0.65f);
                    gO = Instantiate(PrefabsManager.PrefabBLockShadow, shadowParent.transform);
                    gO.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.8f);
                    gO.transform.localPosition = new Vector3(x * 0.65f, -y * 0.65f);
                    shadow[x, y] = gO;
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x - 0.325f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).x < (transform.position.x + 0.325f) + width * 0.65f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y + 0.325f &&
                mainCam.ScreenToWorldPoint(Input.mousePosition).y > (transform.position.y + 0.325f) - height * 0.65f)
            {
                isPickUpInsert = true;
                shadowParent.SetActive(true);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPickUpInsert = false;
            shadowParent.SetActive(false);
        }
        if (isPickUpInsert)
        {
            transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 2);
        }
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
        float f1 = shadowParent.transform.position.x - square0x0Board.x;
        f1 = ((int)(f1 * 100f + (f1 > 0 ? 0.5f : -0.5)) / 100f);
        float f2 = shadowParent.transform.position.y - square0x0Board.y;
        f2 = ((int)(f2 * 100f + (f2 > 0 ? 0.5f : -0.5)) / 100f);
        originX = (int)(f1 / 0.65f);
        originY = -(int)(f2 / 0.65f);
        for (int  y = 0,x; y < height; y++)
        {
            for (x = 0; x < width; x++)
            {
                if (shadow[x, y])
                {
                    if (board.IsCanPutSquareIntoBoard(InsertMatrix[x, y], originX + x, originY + y))
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
}
