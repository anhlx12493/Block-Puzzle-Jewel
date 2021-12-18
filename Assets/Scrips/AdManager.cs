using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private static AD ad;

    public static void AddAd(AD ad)
    {
        AdManager.ad = ad;
    }
    private void Awake()
    {
        LoadInter();
        LoadVideo();
    }

    public static void LoadBanner()
    {
        ad.LoadBanner();
    }

    public static void LoadInter()
    {
        Action onFailToLoad = () =>
        {
            LoadInter();
        };
        ad.LoadInter(null, onFailToLoad);
    }

    public static void LoadVideo()
    {
        Action onFailToLoad = () =>
        {
            LoadVideo();
        };
        ad.LoadVideo(null, onFailToLoad);
    }

    public static void IsInterReady()
    {
        ad.IsInterReady();
    }
    public static void IsVideoReady()
    {
        ad.IsVideoReady();
    }

    public static void ShowInter(Action onClose)
    {
        onClose += () =>
        {
            LoadInter();
        };
        ad.ShowInter(onClose);
    }

    public static void ShowVideo(Action onRewarded, Action onDidNotReward, Action onClose)
    {
        onClose += () =>
        {
            LoadVideo();
        };
        ad.ShowVideo(onRewarded, onDidNotReward, onClose);
    }
}
