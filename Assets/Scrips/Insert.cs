using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insert : MonoBehaviour
{
    Camera mainCam;
    Transform firstSquare;
    [SerializeField]GameObject shadow;
    [SerializeField]GameObject board;
    private void Awake()
    {
        mainCam = Camera.main;
        firstSquare = GetComponentsInChildren<Transform>()[1];
    }
    private void FixedUpdate()
    {
        transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        int xx = (int)((firstSquare.position.x - board.transform.position.x) / 0.65f);
        int yy = (int)((firstSquare.position.y - board.transform.position.y) / 0.65f);
        if(firstSquare.position.x > board.transform.position.x)
        {
            if(firstSquare.position.y > board.transform.position.y)
            {
                shadow.transform.position = new Vector3(xx * 0.65f + 0.325f, yy * 0.65f + 0.325f);
            }
            else
            {
                shadow.transform.position = new Vector3(xx * 0.65f + 0.325f, yy * 0.65f - 0.325f);
            }
        }
        else
        {
            if(firstSquare.position.y > board.transform.position.y)
            {
                shadow.transform.position = new Vector3(xx * 0.65f - 0.325f, yy * 0.65f + 0.325f);
            }
            else
            {
                shadow.transform.position = new Vector3(xx * 0.65f - 0.325f, yy * 0.65f - 0.325f);
            }
        }
    }


}
