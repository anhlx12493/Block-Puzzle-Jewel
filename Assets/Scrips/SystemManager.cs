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


    [RuntimeInitializeOnLoadMethod]
    static void OnLoad()
    {
        _instance = new SystemManager();
    }
    public SystemManager()
    {
        AdManager.AddAd(new AdAdmob());
        GameObject go = new GameObject("AdManager");
        go.AddComponent<AdManager>();
        Object.DontDestroyOnLoad(go);
    }
}
