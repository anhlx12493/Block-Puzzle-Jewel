using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instant;

    private void Awake()
    {
        if(Instant== null)
        {
            Instant = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
