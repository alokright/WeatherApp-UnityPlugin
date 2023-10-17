package com.clevertap.demo.weatherapp.unity;

import android.content.res.Configuration;
import android.view.KeyEvent;
import android.view.MotionEvent;

import com.unity3d.player.UnityPlayer;

/**
 * Created by Alok Kumar on 14/10/22.
 */
public class UnityPlayerDelegate {

    private UnityPlayer mRealUnityPlayer = null;

    private static volatile UnityPlayerDelegate delegateInstance = null;

    private UnityPlayerDelegate() {}

    public static synchronized UnityPlayerDelegate getInstance(UnityProxyActivity gameBaseActivity) {
        if (delegateInstance == null) {
            delegateInstance = new UnityPlayerDelegate();
            delegateInstance.mRealUnityPlayer = new UnityPlayer(gameBaseActivity);
        }
        return delegateInstance;
    }

    public void destroy() {  // destroy is not supported. As it will kill the process.
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.destroy();
        }
    }

    public void pause() {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.pause();
        }
    }

    public void resume() {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.resume();
        }
    }

    public void lowMemory() {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.lowMemory();
        }
    }

    public void configurationChanged(Configuration newConfig) {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.configurationChanged(newConfig);
        }
    }

    public void windowFocusChanged(boolean hasFocus) {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.windowFocusChanged(hasFocus);
        }
    }

    public boolean injectEvent(KeyEvent event) {
        return mRealUnityPlayer != null && mRealUnityPlayer.injectEvent(event);
    }

    public boolean injectEvent(MotionEvent event) {
        return mRealUnityPlayer != null && mRealUnityPlayer.injectEvent(event);
    }

    public void requestFocus() {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.requestFocus();
        }
    }

    public void clearFocus() {
        if (mRealUnityPlayer != null) {
            mRealUnityPlayer.clearFocus();
        }
    }

}

