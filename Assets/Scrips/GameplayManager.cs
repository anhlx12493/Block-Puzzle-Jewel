using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public LosePopupManager losePopup;
    [HideInInspector] public int score = 0;

    private Board _board;
    public Board Board
    {
        get
        {
            if (!_board)
            {
                _board = FindObjectOfType<Board>();
            }
            return _board;
        }
    }
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
    private void Start()
    {
        UIManager.Instance.BestScore.text = SystemManager.Instance.BestScore.ToString();
    }
    public void Onlose()
    {
        losePopup.gameObject.SetActive(true);
        if (score > SystemManager.Instance.BestScore)
        {
            SystemManager.Instance.BestScore = score;
            UIManager.Instance.BestScore.text = SystemManager.Instance.BestScore.ToString();
        }
    }
    public void UpdateScore()
    {
        score++;
        UIManager.Instance.ScoreText.text = score.ToString();
    }
}
