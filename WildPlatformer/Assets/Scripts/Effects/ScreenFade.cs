using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;



public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime;
    [SerializeField] UnityEvent OnScreenFadedToBlack;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage.DOFade(0, fadeTime);
    }
    public void FadeToDark()
    {
        fadeImage.DOFade(1, fadeTime).OnComplete(InvokeScreenFadeEvent);
    }
    void InvokeScreenFadeEvent()
    {
        OnScreenFadedToBlack?.Invoke();
    }
}
