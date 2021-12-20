using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    private static SystemManager _instance;
    public static SystemManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private int _bestScore;
    public int BestScore
    {
        get
        {
            _bestScore = PlayerPrefs.GetInt("BestScore");
            return _bestScore;
        }
        set
        {
            _bestScore = value;
            PlayerPrefs.SetInt("BestScore", value);
        }
    }

    private SaveGame? _saveGame;
    public SaveGame? SaveGame
    {
        get
        {
            if (_saveGame == null)
            {
                string s = PlayerPrefs.GetString("SaveGame", "");
                if (string.IsNullOrEmpty(s))
                {
                    _saveGame = null;
                }
                else
                {
                    _saveGame = JsonUtility.FromJson<SaveGame>(PlayerPrefs.GetString("SaveGame", ""));
                }
            }
            return _saveGame;
        }
        set
        {
            _saveGame = value;
            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(value));
        }
    }


    [RuntimeInitializeOnLoadMethod]
    static void OnLoad()
    {
        AdManager.AddAd(new AdAdmob());
        GameObject go = new GameObject("AdManager");
        go.AddComponent<AdManager>();
        _instance = go.AddComponent<SystemManager>();
        Object.DontDestroyOnLoad(go); ;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }

    public void OnApplicationQuit()
    {
        SaveData();
    }
    void SaveData()
    {
        SaveGame saveGame = new SaveGame();
        if (GameplayManager.Instance)
        {
            GameplayManager.Instance.Board.GetSaveData(ref saveGame);
        }
        SaveGame = saveGame;
    }
}
[System.Serializable]
public struct SaveGame
{
    public int numberInsertAreWaitting, remanningInsert;
    public int[] matrixColor;
    public Square[] matrixBoard;
    public SaveInsert[] inserts;
    public int score;
}
