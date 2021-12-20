using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LosePopupManager : MonoBehaviour
{
    [SerializeField] Button RestartButton;

    private void Awake()
    {
        RestartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        Action action = () =>
        {
            PlayerPrefs.DeleteKey("SaveGame");
            SystemManager.Instance.SaveGame = null;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        };
        AdManager.ShowInter(action);
    }
}
