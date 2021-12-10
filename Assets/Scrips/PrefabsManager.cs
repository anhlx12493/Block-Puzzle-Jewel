using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    private static GameObject _prefabBlock;
    public static GameObject PrefabBLock
    {
        get
        {
            if (_prefabBlock == null)
            {
                _prefabBlock = (GameObject)Resources.Load("PrefabsGameplay/Block");
            }
            return _prefabBlock;
        }
    }
    private static GameObject _prefabBlockShadow;
    public static GameObject PrefabBLockShadow
    {
        get
        {
            if (_prefabBlockShadow == null)
            {
                _prefabBlockShadow = (GameObject)Resources.Load("PrefabsGameplay/Block");
            }
            return _prefabBlockShadow;
        }
    }
    private static GameObject _prefabShadowParent;
    public static GameObject PrefabShadowParent
    {
        get
        {
            if (_prefabShadowParent == null)
            {
                _prefabShadowParent = (GameObject)Resources.Load("PrefabsGameplay/ShadowParent");
            }
            return _prefabShadowParent;
        }
    }
}
