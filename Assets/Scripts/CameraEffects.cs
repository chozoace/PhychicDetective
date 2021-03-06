﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraEffects
{
    static GameObject _blackScreen;
    static float _fadeSpeed;
    static float _fadeWhiteSpeed;
    static bool _effectsPlaying;
    public static bool EffectsPlaying { get { return _effectsPlaying; } }

    static CameraEffects()
    {
        _blackScreen = Resources.Load<GameObject>("CameraEffects/BlackScreenFade");
        _fadeSpeed = 5;
        _fadeWhiteSpeed = .8f;
    }

    public static float currentFadeAlpha()
    {
        GameObject fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");

        if (fadeScreenInstance)
            return fadeScreenInstance.GetComponent<SpriteRenderer>().color.a;
        else
        {
            throw new System.Exception("fadeScreen is null");
        }
    }

    public static void removeFadeScreen()
    {
        GameObject.Destroy(GameObject.Find("BlackScreenFade(Clone)"));
    }

    public static IEnumerator startFadeRoutine(string color, DelegateTemplates.VoidDel del)
    {
        Color colorObj = Color.black;
        while (true)
        {
            if (color.Equals("Black"))
            {
                FadeToBlack();
                colorObj = Color.black;
            }
            else if (color.Equals("White"))
            {
                FadeToWhite();
                colorObj = Color.white;
            }
            else
                yield return null;
            if (currentFadeAlpha() >= .995f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = colorObj;
                Debug.Log("del being called");
                del();
                yield break;
            }
            yield return null;
        }
    }

    public static IEnumerator clearFadeRoutine(string color, DelegateTemplates.VoidDel del)
    {
        Color colorObj = Color.clear;
        while (true)
        {
            if (color.Equals("Black"))
                FadeToClear();
            else if (color.Equals("White"))
                FadeToClearWhite();
            else
                yield return null;
            if (currentFadeAlpha() <= .005f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = colorObj;
                del();
                yield break;
            }
            yield return null;
        }
    }

    public static void FadeToBlack()
    {
        fadeIn(Color.black, _fadeSpeed);
    }

    public static void FadeToClear()
    {
        fadeOut(_fadeSpeed);
    }

    public static void FadeToWhite()
    {
        fadeIn(Color.white, _fadeWhiteSpeed, true);
    }

    public static void FadeToClearWhite()
    {
        fadeOut(_fadeWhiteSpeed, true);
    }

    static void fadeIn(Color color, float fadeSpeed, bool removeTime = false)
    {
        GameObject fadeScreenInstance;
        float timeModifier = 1;
        if (removeTime)
            timeModifier = Time.deltaTime;
        if (!GameObject.Find("BlackScreenFade(Clone)"))
        {
            _blackScreen.GetComponent<SpriteRenderer>().color = Color.clear;
            fadeScreenInstance = GameObject.Instantiate(_blackScreen, Vector2.zero, Quaternion.identity);
        }
        else
            fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");
        fadeScreenInstance.GetComponent<SpriteRenderer>().color = Color.Lerp(fadeScreenInstance.GetComponent<SpriteRenderer>().color,
        color, fadeSpeed * Time.deltaTime / timeModifier);
    }

    static void fadeOut(float fadeSpeed, bool removeTime = false)
    {
        float timeModifier = 1;
        if (removeTime)
            timeModifier = Time.deltaTime;
        GameObject fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");
        fadeScreenInstance.GetComponent<SpriteRenderer>().color = Color.Lerp(fadeScreenInstance.GetComponent<SpriteRenderer>().color,
            Color.clear, fadeSpeed * Time.deltaTime / timeModifier);
    }
}
