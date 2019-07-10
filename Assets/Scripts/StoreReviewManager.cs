//  ReviewManager.cs
//  http://kan-kikuchi.hatenablog.com/entry/ReviewManager
//
//  Created by kan kikuchi on 2017.06.03.

//各OSのIDを使っていないと警告が出るので抑制
#pragma warning disable 0414

//iOSまたはAndroidの実機用のdefineを設定
#if UNITY_IOS && !UNITY_EDITOR
#define iOS_DEVICE
#elif UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID_DEVICE
#endif

using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// レビュー関連の処理を管理するクラス
/// </summary>
public class StoreReviewManager : SingletonMonoBehaviour<StoreReviewManager>
{

    //各OSのID
    [SerializeField]
    [Header("-----各OSのID-----")]
    private int _iosID = 1470960960;    // 家庭科

    [SerializeField]
    private string _androidID = "com.Mikazuya.Home";

    //アプリ内でレビューを表示できるか
    private bool _canReviewInApp = false;
    public bool CanReviewInApp { get { return _canReviewInApp; } }

    //=================================================================================
    //初期化
    //=================================================================================

    protected override void Init()
    {
        base.Init();

        //iOS実機の場合はiOSのバージョンを取得し、アプリ内でレビューを表示できるか判定
#if iOS_DEVICE
    float iosVersion = _GetiOSVersion();
    _canReviewInApp = iosVersion >= 10.3f;
    Debug.Log ("iOS Version : " + iosVersion + ", Can Review In App : " + _canReviewInApp);
#endif
    }

#if iOS_DEVICE
  [DllImport("__Internal")]
  private static extern float _GetiOSVersion();
#endif

    //=================================================================================
    //レビューを催促
    //=================================================================================

    /// <summary>
    /// レビューを催促する
    /// </summary>
    public void RequestReview()
    {

#if iOS_DEVICE
    //アプリ内レビューに対応してる場合はアプリ内で表示
    if(_canReviewInApp){
      _RequestReview();
    }
    //それ以外の場合はストアに飛ばす
    else{
      Application.OpenURL(string.Format("https://itunes.apple.com/app/id{0}?action=write-review", _iosID));
    }
#elif ANDROID_DEVICE
    Application.OpenURL("market://details?id=" + _androidID);
#endif

    }

#if iOS_DEVICE
  [DllImport("__Internal")]
  private static extern IntPtr _RequestReview();
#endif

}