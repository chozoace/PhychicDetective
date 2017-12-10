using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraEffects
{
    static GameObject _blackScreen;
    static float _fadeSpeed;

    static CameraEffects()
    {
        _blackScreen = Resources.Load<GameObject>("CameraEffects/BlackScreenFade");
        _fadeSpeed = 5;
    }

    public static float currentFadeAlpha()
    {
        GameObject fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");
        if(fadeScreenInstance)
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

    public static void FadeToBlack()
    {
        GameObject fadeScreenInstance;
        if (!GameObject.Find("BlackScreenFade(Clone)"))
        {
            _blackScreen.GetComponent<SpriteRenderer>().color = Color.clear;
            fadeScreenInstance = GameObject.Instantiate(_blackScreen, Vector2.zero, Quaternion.identity);
        }
        else
            fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");
        fadeScreenInstance.GetComponent<SpriteRenderer>().color = Color.Lerp(fadeScreenInstance.GetComponent<SpriteRenderer>().color,
            Color.black, _fadeSpeed * Time.deltaTime);
    }

    public static void FadeToClear()
    {
        GameObject fadeScreenInstance = GameObject.Find("BlackScreenFade(Clone)");
        fadeScreenInstance.GetComponent<SpriteRenderer>().color = Color.Lerp(fadeScreenInstance.GetComponent<SpriteRenderer>().color, 
            Color.clear, _fadeSpeed * Time.deltaTime);
    }
}
