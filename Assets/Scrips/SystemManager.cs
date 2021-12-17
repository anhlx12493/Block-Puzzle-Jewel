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

    [RuntimeInitializeOnLoadMethod]
    static void OnLoad()
    {
        _instance = new SystemManager();
    }
}
