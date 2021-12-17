using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] Button PauseButton;
    public Sprite[] insertColor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PauseButton.onClick.AddListener(PauseGame);
    }

    private void PauseGame()
    {
        if (GameplayManager.Instance.losePopup.gameObject.activeInHierarchy)
        {
            GameplayManager.Instance.losePopup.gameObject.SetActive(false);
        }
        else
        {
            GameplayManager.Instance.losePopup.gameObject.SetActive(true);
        }
    }
}
