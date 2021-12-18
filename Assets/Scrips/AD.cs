using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AD
{
    public abstract void LoadBanner();
    public abstract void LoadInter(Action onLoaded, Action onFailToLoad);
    public abstract void LoadVideo(Action onLoaded, Action onFailToLoad);
    public abstract void DestroyBanner();
    public abstract void DestroyInter();

    public abstract bool IsInterReady();
    public abstract bool IsVideoReady();
    public abstract void ShowInter(Action onClose);
    public abstract void ShowVideo(Action onRewarded, Action onDidNotReward, Action onClose);
}
