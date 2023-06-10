using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FitScreenCamera : MonoBehaviour
{
    float width, height;
    Camera mainCam;
    float defaultSize;
    public Transform transformTopBar;
    private void Start()
    {
        mainCam = Camera.main;
        UpdateScreen();
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateScreen();
    }
#endif

    private void UpdateScreen()
    {
        width = Screen.width;
        height = Screen.height;
        defaultSize = 5;
        if (width / height > 0.6f)
        {
            mainCam.orthographicSize = defaultSize;
            transformTopBar.localScale = Vector3.one * 0.58f * height / width;
        }
        else
        {
            mainCam.orthographicSize = 3 * height / width;
            transformTopBar.localScale = Vector3.one;
        }
        transformTopBar.position = mainCam.WorldToScreenPoint(new Vector3(0, 3.5f, 0));
    }
}
