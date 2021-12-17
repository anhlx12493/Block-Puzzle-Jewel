using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public LosePopupManager losePopup;
    [SerializeField] Text ScoreText;
    int Score = 0;
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
    }

    public void Onlose()
    {
        losePopup.gameObject.SetActive(true);
    }
    public void UpdateScore()
    {
        Score++;
        ScoreText.text = Score + "";
    }
}
