using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastManager : PersistentSingleton<ToastManager>
{
    string toastString;
    string input;
    AndroidJavaObject currentActivity;
    AndroidJavaClass UnityPlayer;
    AndroidJavaObject context;

    protected override void Initialize()
    {
        base.Initialize();

#if UNITY_IOS || UNITY_ANDROID
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

#elif UNITY_EDITOR

#endif
    }

    public void ShowToastOnUiThread(string toastString)
    {
#if UNITY_IOS || UNITY_ANDROID
        this.toastString = toastString;
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(ShowToast));
#elif UNITY_EDITOR
        Debug.Log("toast: " + toastString);
#endif
    }

    void ShowToast()
    {
        Debug.Log(this + ": Running on UI thread");

        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }
}
